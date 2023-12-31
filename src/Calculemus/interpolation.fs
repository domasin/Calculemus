// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

// ========================================================================= //
// Implementation/proof of the Craig-Robinson interpolation theorem.         //
//                                                                           //
// This is based on the proof in Kreisel & Krivine, which works very nicely  //
// in our context.                                                           //
// ========================================================================= //

/// <summary>
/// Constructive Craig/Robinson interpolation.
/// </summary>
/// 
/// <category index="6">Decidable subsets and theories</category>
module Calculemus.Interpolation

open Calculemus.Lib.Sort
open Calculemus.Lib.Set
open Calculemus.Lib.Fpf

open Formulas
open Prop
open Defcnf
open Fol
open Skolem
open Herbrand
open Equal
open Order
open Eqelim

// pg. 428
// ------------------------------------------------------------------------- //
// Interpolation for propositional logic.                                    //
// ------------------------------------------------------------------------- //

let pinterpolate p q =
    let orify a r = Or (psubst (a |=> False) r, psubst (a |=> True) r)
    psimplify (List.foldBack orify (subtract (atoms p) (atoms q)) p)
        
// pg. 429
// ------------------------------------------------------------------------- //
// Relation-symbol interpolation for universal closed formulas.              //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let urinterpolate p q =
    let fm = specialize (prenex (And (p, q)))
    let fvs = fv fm
    let consts, funcs = herbfuns fm
    let cntms = List.map (fun (c, _) -> Fn (c, [])) consts
    let tups = dp_refine_loop (simpcnf fm) cntms funcs fvs 0 [] [] []
    let fmis = List.map (fun tup -> subst (fpf fvs tup) fm) tups
    let ps, qs = 
        fmis
        |> List.map (fun fm -> 
            match fm with 
            | (And (p, q)) -> 
                p, q
            | _ -> failwith "urinterpolate: incomplete pattern matching"
        ) 
        |> List.unzip 
    pinterpolate (list_conj (setify ps)) (list_conj (setify qs))
    
// pg. 432
// ------------------------------------------------------------------------- //
// Pick the topmost terms starting with one of the given function symbols.   //
// ------------------------------------------------------------------------- //

let rec toptermt fns tm =
    match tm with
    | Var x -> []
    | Fn (f, args) ->
        if mem (f, List.length args) fns then [tm]
        else List.foldBack (union << toptermt fns) args []

let topterms fns =
    atom_union (fun (R (p, args)) -> List.foldBack (union << toptermt fns) args [])
        
// pg. 433
// ------------------------------------------------------------------------- //
// Interpolation for arbitrary universal formulas.                           //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let uinterpolate p q =
    let rec fp = functions p
    and fq = functions q
    let rec simpinter tms n c =
        match tms with
        | [] -> c
        | (Fn (f, args) as tm) :: otms ->
            let v = "v_" + (string n)
            let c' = replace (tm |=> Var v) c
            let c'' =
                if mem (f, List.length args) fp
                then Exists (v, c')
                else Forall (v, c')
            simpinter otms (n + 1) c''
        | _ -> failwith "uinterpolate: incomplete pattern matching"
        
    let c = urinterpolate p q
    let tts = topterms (union (subtract fp fq) (subtract fq fp)) c
    let tms = sort (decreasing termsize) tts
    simpinter tms 1 c
    
// pg. 434
// ------------------------------------------------------------------------- //
// Now lift to arbitrary formulas with no common free variables.             //
// ------------------------------------------------------------------------- //

// dom modified to remove warning
let cinterpolate p q =
    let fm = nnf (And (p, q))
    let rec efm = List.foldBack mk_exists (fv fm) fm
    and fns = List.map fst (functions fm)
    match skolem efm fns with 
    | And (p', q'), _ -> 
        uinterpolate p' q'
    | _ -> failwith "cinterpolate: incomplete pattern matching"
        
// pg. 434
// ------------------------------------------------------------------------- //
// Now to completely arbitrary formulas.                                     //
// ------------------------------------------------------------------------- //

let interpolate p q =
    let rec vs = List.map (fun v -> Var v) (intersect (fv p) (fv q))
    and fns = functions (And (p, q))
    let n = List.foldBack (max_varindex "c_" << fst) fns (0 |> bigint) + (1 |> bigint)
    let cs = 
        [n..(n + bigint (List.length vs - 1))]
        |> List.map (fun i -> Fn ("c_" + i.ToString(), [])) 
    let rec fn_vc = fpf vs cs
    and fn_cv = fpf cs vs
    let rec p' = replace fn_vc p
    and q' = replace fn_vc q
    replace fn_cv (cinterpolate p' q')
    
// pg. 435
// ------------------------------------------------------------------------- //
// Lift to logic with equality.                                              //
// ------------------------------------------------------------------------- //

let einterpolate p q =
    let rec p' = equalitize p
    and q' = equalitize q
    let rec p'' = if p' = p then p else And (fst (dest_imp p'), p)
    and q'' = if q' = q then q else And (fst (dest_imp q'), q)
    interpolate p'' q''
