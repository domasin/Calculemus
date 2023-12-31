// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Lib.Fpf

open Fol

module Unif = 

    let rec istriv env x t =
        match t with
        | Var y -> 
            y = x
            || defined env y
            && istriv env x (apply env y)
        | Fn(f,args) ->
            List.exists (istriv env x) args 
            && failwith "cyclic"

    let rec unify (env : func<string, term>) eqs =
        match eqs with
        | [] -> env
        | (Fn (f, fargs), Fn (g, gargs)) :: oth ->
            if f = g && List.length fargs = List.length gargs then
                unify env (List.zip fargs gargs @ oth)
            else
                failwith "impossible unification"
        | (Var x, t) :: oth
        | (t, Var x) :: oth ->
            // If there is already a definition (say x |-> s) in env, then 
            // the pair is expanded into (s, t) and the recursion proceeds.
            if defined env x then
                unify env ((apply env x,t) :: oth)
            // Otherwise we know that condition x |-> s is not in env,
            // so x |-> t is a candidate for incorporation into env.
            else
                unify 
                    (
                        // If there is a benign cycle in env, env is unchanged; 
                        // while if there is a malicious one, the unification 
                        // will fail.
                        if istriv env x t then env
                        // Otherwise, x |-> t is incorporated into env for the 
                        // next recursive call.
                        else (x |-> t) env
                    ) oth

    let rec solve env =
        let env' = mapf (tsubst env) env
        if env' = env then env 
        else solve env'

    let fullunify eqs = solve (unify undefined eqs)

    let unify_and_apply eqs =
        let i = fullunify eqs
        let apply (t1, t2) =
            tsubst i t1, tsubst i t2
        List.map apply eqs
