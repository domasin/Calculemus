// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

/// <summary>
/// Skolemizing a set of formulas.
/// </summary>
/// 
/// <category index="4">First order logic</category>
module FolAutomReas.Skolems

open Fol
open Skolem

// ========================================================================= //
// Illustration of Skolemizing a set of formulas                             //
// ========================================================================= //

let rec rename_term tm =
    match tm with
    | Fn (f, args) ->
        Fn ("old_" + f, List.map rename_term args)
    | _ -> tm

let rename_form = onformula rename_term

let rec skolems fms corr =
    match fms with
    | [] ->
        [], corr
    | p :: ofms ->
        let p', corr' = skolem (rename_form p) corr
        let ps', corr'' = skolems ofms corr'
        p' :: ps', corr''

let skolemizes fms =
    fst <| skolems fms []
