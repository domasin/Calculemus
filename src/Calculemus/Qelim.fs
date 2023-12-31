// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023 Domenico Masini                                        //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Lib.List
open Lib.Set
open Lib.Fpf

open Formulas
open Prop
open Fol
open Skolem
open Equal
open Decidable

module Qelim = 

    // ===================================================================== //
    // Introduction to quantifier  elimination.                              //
    // ===================================================================== //
    
    // --------------------------------------------------------------------- //
    // Lift procedure given literal modifier, formula normalizer, and a      //
    // basic elimination procedure for existential formulas with conjunctive //
    // body.                                                                 //
    // --------------------------------------------------------------------- //
    
    let qelim bfn x p =
        let cjs = conjuncts p
        let ycjs, ncjs = List.partition (mem x << fv) cjs
        if ycjs = [] then p
        else
            let q = bfn (Exists (x, list_conj ycjs))
            List.foldBack mk_and ncjs q
    
    let lift_qelim afn nfn qfn fm =
        let rec qelift vars fm =
            match fm with
            | Atom (R (_,_)) ->
                afn vars fm
            | Not p ->
                Not (qelift vars p)
            | And (p, q) ->
                And (qelift vars p, qelift vars q)
            | Or (p, q) ->
                Or (qelift vars p, qelift vars q)
            | Imp (p, q) ->
                Imp (qelift vars p, qelift vars q)
            | Iff (p, q) ->
                Iff (qelift vars p, qelift vars q)
            | Forall (x, p) ->
                Not (qelift vars (Exists (x, Not p)))
            | Exists (x, p) ->
                    let djs = disjuncts (nfn (qelift (x :: vars) p))
                    list_disj (List.map (qelim (qfn vars) x) djs)
            | _ -> fm
    
        simplify (qelift (fv fm) (miniscope fm))
    
    // --------------------------------------------------------------------- // 
    // Cleverer (propositional) NNF with conditional and literal             //
    // modification.                                                         // 
    // --------------------------------------------------------------------- // 
    
    let cnnf lfn fm =
        let rec cnnf fm =
            match fm with
            | And (p, q) ->
                And (cnnf p, cnnf q)
            | Or (p, q) ->
                Or (cnnf p, cnnf q)
            | Imp (p, q) ->
                Or (cnnf(Not p), cnnf q)
            | Iff (p, q) ->
                Or (And (cnnf p, cnnf q), And (cnnf (Not p), cnnf (Not q)))
            | Not (Not p) ->
                cnnf p
            | Not (And (p, q)) ->
                Or (cnnf (Not p), cnnf (Not q))
            | Not (Or (And (p, q), And (p', r))) when p' = negate p ->
                Or (cnnf (And (p, Not q)), cnnf (And (p', Not r)))
            | Not (Or (p, q)) ->
                And (cnnf (Not p), cnnf (Not q))
            | Not (Imp (p, q)) ->
                And (cnnf p, cnnf (Not q))
            | Not (Iff (p, q)) ->
                Or (And (cnnf p, cnnf (Not q)), And (cnnf (Not p), cnnf q))
            | _ -> lfn fm
        (simplify << cnnf << simplify) fm
    
    // --------------------------------------------------------------------- // 
    // Initial literal simplifier and intermediate literal modifier.         // 
    // --------------------------------------------------------------------- // 
    
    let lfn_dlo fm =
        match fm with
        | Not (Atom (R ("<", [s; t]))) ->
            Or (Atom (R ("=", [s; t])), Atom (R ("<", [t; s])))
        | Not (Atom (R ("=", [s; t]))) ->
            Or (Atom (R ("<", [s; t])), Atom (R ("<", [t; s])))
        | _ -> fm
    
    // --------------------------------------------------------------------- // 
    // Simple example of dense linear orderings; this is the base  function. //
    // --------------------------------------------------------------------- // 
    
    let dlobasic fm =
        match fm with
        | Exists (x, p) ->
            let cjs = subtract (conjuncts p) [Atom (R ("=", [Var x; Var x]))]
            try
                let eqn = List.find is_eq cjs
                let s, t = dest_eq eqn
                let y = if s = Var x then t else s
                list_conj (List.map (subst (x |=> y)) (subtract cjs [eqn]))
            with 
            | _ ->
                if mem (Atom (R ("<", [Var x; Var x]))) cjs then False
                else
                    let lefts, rights = 
                        cjs
                        |> List.partition (fun fm -> 
                            match fm with 
                            | Atom (R ("<", [s; t])) -> 
                                t = Var x
                            | _ -> 
                                failwith "dlobasic: incomplete pattern matching"
                        ) 
                    let ls = 
                        lefts
                        |> List.map (fun fm -> 
                            match fm with 
                            | (Atom (R ("<", [l;_]))) -> l
                            | _ -> 
                                failwith "dlobasic: incomplete pattern matching"
                        ) 
                    let rs = 
                        rights
                        |> List.map (fun fm -> 
                            match fm with 
                            | (Atom (R ("<", [_;r]))) -> r
                            | _ -> 
                                failwith "dlobasic: incomplete pattern matching"
                        ) 
                    list_conj (allpairs (fun l r -> Atom (R ("<", [l; r]))) ls rs)
        | _ -> failwith "dlobasic"
    
    // --------------------------------------------------------------------- // 
    // Overall quelim  procedure.                                            //
    // --------------------------------------------------------------------- // 
    
    let afn_dlo vars fm =
        match fm with
        | Atom (R ("<=", [s; t])) ->
            Not (Atom (R ("<", [t; s])))
        | Atom (R (">=", [s; t])) ->
            Not (Atom (R ("<", [s; t])))
        | Atom (R (">", [s; t])) ->
            Atom (R ("<", [t; s]))
        | _ -> fm
    
    let quelim_dlo fm =
        fm |> lift_qelim afn_dlo (dnf << cnnf lfn_dlo) (fun v -> dlobasic)
