// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Calculemus

open Lib.List
open Lib.Set
open Lib.Fpf
open Lib.Search

open Formulas
open Prop
open Fol
open Skolem
open Tableaux
open Prolog

module Meson = 

    // ====================================================================== //
    // Model elimination procedure (MESON version, based on Stickel's PTTP).  //
    // ====================================================================== //

    // ---------------------------------------------------------------------- //
    // Generation of contrapositives.                                         //
    // ---------------------------------------------------------------------- //

    let contrapositives cls =
        let baseClause = List.map (fun c -> List.map negate (subtract cls [c]), c)  cls
        if List.forall negative cls then
            (List.map negate cls, False) :: baseClause 
        else baseClause

    // ---------------------------------------------------------------------- //
    // The core of MESON: ancestor unification or Prolog-style extension.     //
    // ---------------------------------------------------------------------- //

    let rec mexpand_basic rules ancestors g cont (env, n, k) =
        if n < 0 then failwith "Too deep"
        else
            try 
                tryfind (fun a -> cont (unify_literals env (g, negate a), n, k))    ancestors
            with _ ->
                tryfind (fun rule ->
                    let (asm, c) ,k' = renamerule k rule
                    List.foldBack (mexpand_basic rules (g :: ancestors)) asm cont
                        (unify_literals env (g, c), n - List.length asm, k'))
                    rules

    // ---------------------------------------------------------------------- //
    // Full MESON procedure.                                                  //
    // ---------------------------------------------------------------------- //

    let puremeson_basic fm =
        let cls = simpcnf (specialize (pnf fm))
        let rules = List.foldBack ((@) << contrapositives) cls []
        deepen (fun n ->
            mexpand_basic rules [] False id (undefined, n, 0)
            |> ignore
            n) 0

    let meson_basic fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (puremeson_basic << list_conj) (simpdnf fm1)

    // ---------------------------------------------------------------------- //
    // With repetition checking and divide-and-conquer search.                //
    // ---------------------------------------------------------------------- //

    let rec equal env fm1 fm2 =
        try unify_literals env (fm1, fm2) = env
        with _ -> false

    let expand expfn goals1 n1 goals2 n2 n3 cont env k =
        expfn goals1 (fun (e1, r1, k1) ->
            expfn goals2 (fun (e2, r2, k2) ->
                            if n2 + r1 <= n3 + r2 then failwith "pair"
                            else cont(e2, r2, k2))
                    (e1, n2 + r1, k1))
            (env, n1, k)

    let rec mexpand rules ancestors g cont (env, n, k) =

        let rec mexpands rules ancestors gs cont (env, n, k) =
            if n < 0 then failwith "Too deep" 
            else
                let m = List.length gs
                if m <= 1 then List.foldBack (mexpand rules ancestors) gs cont   (env, n, k) 
                else
                    let n1 = n / 2
                    let n2 = n - n1
                    let goals1,goals2 = chop_list (m / 2) gs
                    let expfn = expand (mexpands rules ancestors)
                    try expfn goals1 n1 goals2 n2 -1 cont env k
                    with _ -> expfn goals2 n1 goals1 n2 n1 cont env k

        if n < 0 then
            failwith "Too deep"
        elif List.exists (equal env g) ancestors then
            failwith "repetition"
        else
            try tryfind (fun a -> cont (unify_literals env (g, negate a), n, k))    ancestors with
            | Failure _ ->
                tryfind (fun r ->
                    let (asm, c), k' = renamerule k r
                    mexpands rules (g :: ancestors) asm cont (unify_literals env     (g, c), n - List.length asm, k'))
                    rules

    let puremeson fm =   
        let cls = simpcnf (specialize (pnf fm))
        let rules = List.foldBack ((@) << contrapositives) cls []
        deepen (fun n ->
            mexpand rules [] False id (undefined, n, 0)
            |> ignore
            n) 0

    let meson fm =
        let fm1 = askolemize (Not (generalize fm))
        List.map (puremeson << list_conj) (simpdnf fm1)
