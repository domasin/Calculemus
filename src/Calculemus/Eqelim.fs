// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Lib.Search
open Lib.Set
open Lib.Fpf

open Formulas
open Prop
open Fol
open Skolem
open Tableaux
open Meson
open Equal

module Eqelim = 

    // ====================================================================== //
    // Equality elimination including Brand transformation and relatives.     //
    // ====================================================================== //

    // ---------------------------------------------------------------------- //
    // Brand's S and T modifications on clauses.                              //
    // ---------------------------------------------------------------------- //

    let rec modify_S cl =
        try 
            let (s,t) = tryfind dest_eq cl
            let eq1 = mk_eq s t 
            let eq2 = mk_eq t s
            let sub = modify_S (subtract cl [eq1])
            List.map (insert eq1) sub @ List.map (insert eq2) sub
        with 
            Failure _ -> [cl]

    let rec modify_T cl =
        match cl with
        | [] -> []
        | (Atom (R ("=", [s; t])) as eq) :: ps ->
            let ps' = modify_T ps
            let w = Var (variant "w" (List.foldBack (union << fv) ps' (fv eq)))
            Not (mk_eq t w) :: (mk_eq s w) :: ps'
        | p :: ps ->
            p :: (modify_T ps)


    // ---------------------------------------------------------------------- //
    // Finding nested non-variable sub-terms.                                 //
    // ---------------------------------------------------------------------- //

    let is_nonvar tm = 
        match tm with
        | Var x -> false
        | _ -> true

    let find_nestnonvar tm =
        match tm with
        | Var x -> failwith "findnvsubt"
        | Fn (f, args) ->
            List.find is_nonvar args

    let rec find_nvsubterm fm =
        match fm with
        | Atom (R ("=", [s; t])) ->
            tryfind find_nestnonvar [s;t]
        | Atom (R (p, args)) ->
            List.find is_nonvar args
        | Not p ->
            find_nvsubterm p
        | _ -> invalidArg "fm" "find_nvsubterm: not a literal"

    // ---------------------------------------------------------------------- //
    // Replacement (substitution for non-variable) in term and literal.       //
    // ---------------------------------------------------------------------- //

    let rec replacet rfn tm =
        try apply rfn tm
        with Failure _ ->
            match tm with
            | Fn (f, args) ->
                Fn (f, List.map (replacet rfn) args)
            | _ -> tm

    let replace rfn fm =
        fm |> onformula (replacet rfn)

    // ---------------------------------------------------------------------- //
    // E-modification of a clause.                                            //
    // ---------------------------------------------------------------------- //

    let rec emodify fvs cls =
        try
            let t = tryfind find_nvsubterm cls
            let w = variant "w" fvs
            let cls' = List.map (replace (t |=> Var w)) cls
            emodify (w :: fvs) (Not (mk_eq t (Var w)) :: cls')
        with Failure _ ->
            cls

    let modify_E cls = emodify (List.foldBack (union << fv) cls []) cls

    // ---------------------------------------------------------------------- //
    // Overall Brand transformation.                                          //
    // ---------------------------------------------------------------------- //

    let brand cls =
        let cls1 = List.map modify_E cls
        let cls2 = List.foldBack (union << modify_S) cls1 []
        [mk_eq (Var "x") (Var "x")] :: (List.map modify_T cls2)

    // ---------------------------------------------------------------------- //
    // Incorporation into MESON.                                              //
    // ---------------------------------------------------------------------- //

    let bpuremeson fm =
        let cls = brand (simpcnf (specialize (pnf fm)))
        let rules = List.foldBack ((@) << contrapositives) cls []
        deepen (fun n ->
            mexpand rules [] False id (undefined, n, 0)
            |> ignore
            n) 0

    let bmeson fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (bpuremeson << list_conj) (simpdnf fm1)

    let emeson fm = meson (equalitize fm)



