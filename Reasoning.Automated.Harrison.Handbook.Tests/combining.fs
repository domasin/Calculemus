﻿// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Reasoning.Automated.Harrison.Handbook.Tests.combining

open Reasoning.Automated.Harrison.Handbook.lib
open Reasoning.Automated.Harrison.Handbook.formulas
open Reasoning.Automated.Harrison.Handbook.folMod
open Reasoning.Automated.Harrison.Handbook.cong
open Reasoning.Automated.Harrison.Handbook.cooper
open Reasoning.Automated.Harrison.Handbook.real
open Reasoning.Automated.Harrison.Handbook.combining
open NUnit.Framework
open FsUnit

// pg. 440
// ------------------------------------------------------------------------- //
// Running example if we magically knew the interpolant.                     //
// ------------------------------------------------------------------------- //
    
[<Test>]
let ``integer qelim``() = 
    (integer_qelim << generalize) (parse
        @"(u + 1 = v /\ v_1 + 1 = u - 1 /\ v_2 - 1 = v + 1 /\ v_3 = v - 1) ==> u = v_3 /\ ~(v_1 = v_2)")
    |> should equal formula<fol>.True // be careful with generic True

[<Test>]
let ``ccvalid``() = 
    ccvalid (parse
        @"(v_2 = f(v_3) /\ v_1 = f(u)) ==> ~(u = v_3 /\ ~(v_1 = v_2))")
    |> should be True
        
// pg. 444
// ------------------------------------------------------------------------- //
// Check that our example works.                                             //
// ------------------------------------------------------------------------- //

[<Test>]
let ``nelop001``() =
    nelop001 (add_default [int_lang]) (parse
        @"f(v - 1) - 1 = v + 1 /\ f(u) + 1 = u - 1 /\ u + 1 = v ==> false")
    |> should be True

[<TestCase(@"y <= x /\ y >= x + z /\ z >= 0 ==> f(f(x) - f(y)) = f(z)", Result = true)>]
[<TestCase(@"x = y /\ y >= z /\ z >= x ==> f(z) = f(x)", Result = true)>]
[<TestCase(@"z = f(x - y) /\ x = z + y /\ ~(-(y) = -(x - f(f(z)))) ==> false", Result = true)>]
[<TestCase(@"a + 2 = b ==> f(read(update(A,a,3),b-2)) = f(b - a + 1)", Result = false)>]
[<TestCase(@"hd(x) = hd(y) /\ tl(x) = tl(y) /\ ~(x = nil) /\ ~(y = nil) ==> f(x) = f(y)", Result = false)>]
[<TestCase(@"~(f(f(x) - f(y)) = f(z)) /\ y <= x /\ y >= x + z /\ z >= 0 ==> false", Result = true)>]
[<TestCase(@"(x >= y ==> MAX(x,y) = x) /\ (y >= x ==> MAX(x,y) = y) ==> x = y + 2 ==> MAX(x,y) = x", Result = true)>]
[<TestCase(@"x <= g(x) /\ x >= g(x) ==> x = g(g(g(g(x))))", Result = true)>]
[<TestCase(@"2 * f(x + y) = 3 * y /\ 2 * x = y ==> f(f(x + y)) = 3 * x", Result = true)>]
[<TestCase(@"4 * x = 2 * x + 2 * y /\ x = f(2 * x - y) /\ f(2 * y - x) = 3 /\ f(x) = 4 ==> false", Result = true)>]
let ``nelop int_lang`` f = 
    nelop (add_default [int_lang]) (parse f)

[<TestCase(@"x^2 =  1 ==> (f(x) = f(-(x)))  ==> (f(x) = f(1))", Result = true)>]
let ``nelop real_lang`` f = 
    nelop (add_default [real_lang]) (parse f)

// pg. 447
// ------------------------------------------------------------------------- //
// Confirmation of non-convexity.                                            //
// ------------------------------------------------------------------------- //

[<Test>]
let ``non-convexity real_qelim``() = 
    List.map (real_qelim << generalize) [
        parse @"x * y = 0 /\ z = 0 ==> x = z \/ y = z";
        parse @"x * y = 0 /\ z = 0 ==> x = z";
        parse @"x * y = 0 /\ z = 0 ==> y = z"; ]
    |> should equal [formula<fol>.True; formula<fol>.False; formula<fol>.False]

[<Test>]
let ``non-convexity integer_qelim``() = 
    List.map (integer_qelim << generalize) [
        parse @"0 <= x /\ x < 2 /\ y = 0 /\ z = 1 ==> x = y \/ x = z";
        parse @"0 <= x /\ x < 2 /\ y = 0 /\ z = 1 ==> x = y";
        parse @"0 <= x /\ x < 2 /\ y = 0 /\ z = 1 ==> x = z"; ]
    |> should equal [formula<fol>.True; formula<fol>.False; formula<fol>.False]





// TEMP : To help solve a small bug in 'nelop'

open Reasoning.Automated.Harrison.Handbook.prop
open Reasoning.Automated.Harrison.Handbook.equal

[<Test>]
let ``nelop bugfix 1``() =
    let fm_2 = parse @"z = f(x - y) /\ x = z + y /\ ~(-(y) = -(x - f(f(z)))) ==> false"
    fm_2
    |> should equal
    <| Imp
        (And
           (Atom (R ("=",[Var "z"; Fn ("f",[Fn ("-",[Var "x"; Var "y"])])])),
            And
              (Atom (R ("=",[Var "x"; Fn ("+",[Var "z"; Var "y"])])),
               Not
                 (Atom
                    (R ("=",
                        [Fn ("-",[Var "y"]);
                         Fn
                           ("-",
                            [Fn
                               ("-",[Var "x"; Fn ("f",[Fn ("f",[Var "z"])])])])]))))),
         formula<fol>.False)


    let simp_dnf_fm_2 = simpdnf (psimplify (Not fm_2))
    simp_dnf_fm_2
    |> should equal
    <| [[Atom (R ("=",[Var "x"; Fn ("+",[Var "z"; Var "y"])]));
        Atom (R ("=",[Var "z"; Fn ("f",[Fn ("-",[Var "x"; Var "y"])])]));
        Not
          (Atom
             (R ("=",
                 [Fn ("-",[Var "y"]);
                  Fn
                    ("-",[Fn ("-",[Var "x"; Fn ("f",[Fn ("f",[Var "z"])])])])])))]]


    let simp_dnf_fm_2' = List.head simp_dnf_fm_2
    simp_dnf_fm_2'
    |> should equal
    <| [Atom (R ("=",[Var "x"; Fn ("+",[Var "z"; Var "y"])]));
       Atom (R ("=",[Var "z"; Fn ("f",[Fn ("-",[Var "x"; Var "y"])])]));
       Not
         (Atom
            (R ("=",
                [Fn ("-",[Var "y"]);
                 Fn
                   ("-",[Fn ("-",[Var "x"; Fn ("f",[Fn ("f",[Var "z"])])])])])))]


    let langs = add_default [int_lang]

    let fms = homogenize langs simp_dnf_fm_2'
    fms
    |> should equal
    <| [Atom (R ("=",[Var "x"; Fn ("+",[Var "z"; Var "y"])]));
       Atom (R ("=",[Var "z"; Fn ("f",[Var "v_1"])]));
       Not
         (Atom
            (R ("=",
                [Fn ("-",[Var "y"]);
                 Fn ("-",[Fn ("-",[Var "x"; Var "v_2"])])])));
       Atom (R ("=",[Var "v_2"; Fn ("f",[Fn ("f",[Var "z"])])]));
       Atom (R ("=",[Var "v_1"; Fn ("-",[Var "x"; Var "y"])]))]


    let seps = langpartition langs fms
    seps
    |> should equal
    <| [[Atom (R ("=",[Var "x"; Fn ("+",[Var "z"; Var "y"])]));
        Not
          (Atom
             (R ("=",
                 [Fn ("-",[Var "y"]);
                  Fn ("-",[Fn ("-",[Var "x"; Var "v_2"])])])));
        Atom (R ("=",[Var "v_1"; Fn ("-",[Var "x"; Var "y"])]))];
       [Atom (R ("=",[Var "z"; Fn ("f",[Var "v_1"])]));
        Atom (R ("=",[Var "v_2"; Fn ("f",[Fn ("f",[Var "z"])])]))]]


    let fvlist = List.map (unions << List.map fv) seps
    fvlist
    |> should equal [["v_1"; "v_2"; "x"; "y"; "z"]; ["v_1"; "v_2"; "z"]]
    

    let vars = List.filter (fun x -> List.length (List.filter (mem x) fvlist) >= 2) (unions fvlist)
    vars
    |> should equal ["v_1"; "v_2"; "z"]


    let eqs = List.map (fun (a, b) -> mk_eq (Var a) (Var b)) (distinctpairs vars)
    eqs
    |> should equal
    <| [Atom (R ("=",[Var "v_1"; Var "v_2"]));
       Atom (R ("=",[Var "v_1"; Var "z"]));
       Atom (R ("=",[Var "v_2"; Var "z"]))]


    let ldseps = List.zip langs seps

    let eqs_negated = List.map negate eqs
    eqs_negated
    |> should equal
    <| [Not (Atom (R ("=",[Var "v_1"; Var "v_2"])));
       Not (Atom (R ("=",[Var "v_1"; Var "z"])));
       Not (Atom (R ("=",[Var "v_2"; Var "z"])))]


    let dj = findsubset (trydps ldseps) eqs_negated
    dj
    |> should equal [Atom (R ("=", [Var "v_1"; Var "z"]))]


// The 'findsubset' line in the test above currently crashes
// due to some bug in cooper.linform (or a function reachable from it).
[<Test>]
let ``nelop bugfix 2``() =
    let fm =
        Atom
            (R ("=",
              [Fn ("-", [Var "y"]);
               Fn ("-",
                [Fn ("-", [Fn ("+", [Var "z"; Var "y"]); Var "v_2"])])]))

    let vars = ["z"; "y"; "v_2"]

    linform vars fm
    |> should equal
    <| Atom
         (R ("=",
           [Fn ("0", []);
            Fn ("+",
             [Fn ("*", [Fn ("-1", []); Var "z"]);
              Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])]))

(* OCaml trace of the ``nelop bugfix 2`` test *)
(*
# #trace linform;;
linform is now traced.
# #trace mkatom;;
mkatom is now traced.
# #trace lint;;
lint is now traced.
# #trace linear_neg;;
linear_neg is now traced.
# #trace linear_add;;
linear_add is now traced.
# #trace linear_sub;;
linear_sub is now traced.
# #trace linear_mul;;
linear_mul is now traced.
# #trace earlier;;
earlier is now traced.
# linform vars fm;;
linform <-- ["z"; "y"; "v_2"]
linform --> <fun>
linform* <--
  Atom
   (R ("=",
     [Fn ("-", [Var "y"]);
      Fn ("-", [Fn ("-", [Fn ("+", [Var "z"; Var "y"]); Var "v_2"])])]))
mkatom <-- ["z"; "y"; "v_2"]
mkatom --> <fun>
mkatom* <-- "="
mkatom* --> <fun>
mkatom** <--
  Fn ("-",
   [Fn ("-", [Fn ("-", [Fn ("+", [Var "z"; Var "y"]); Var "v_2"])]);
    Fn ("-", [Var "y"])])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <--
  Fn ("-",
   [Fn ("-", [Fn ("-", [Fn ("+", [Var "z"; Var "y"]); Var "v_2"])]);
    Fn ("-", [Var "y"])])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <-- Fn ("-", [Var "y"])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <-- Var "y"
lint* --> Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
linear_neg <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
linear_neg -->
  Fn ("+", [Fn ("*", [Fn ("-1", []); Var "y"]); Fn ("0", [])])
lint* --> Fn ("+", [Fn ("*", [Fn ("-1", []); Var "y"]); Fn ("0", [])])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <--
  Fn ("-", [Fn ("-", [Fn ("+", [Var "z"; Var "y"]); Var "v_2"])])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <-- Fn ("-", [Fn ("+", [Var "z"; Var "y"]); Var "v_2"])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <-- Var "v_2"
lint* --> Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <-- Fn ("+", [Var "z"; Var "y"])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <-- Var "y"
lint* --> Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
lint <-- ["z"; "y"; "v_2"]
lint --> <fun>
lint* <-- Var "z"
lint* --> Fn ("+", [Fn ("*", [Fn ("1", []); Var "z"]); Fn ("0", [])])
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "z"]); Fn ("0", [])])
linear_add* --> <fun>
linear_add** <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
earlier <-- [<poly>; <poly>; <poly>]
earlier --> <fun>
earlier* <-- <poly>
earlier* --> <fun>
earlier** <-- <poly>
earlier** --> true
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <-- Fn ("0", [])
linear_add* --> <fun>
linear_add** <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <-- Fn ("0", [])
linear_add* --> <fun>
linear_add** <-- Fn ("0", [])
linear_add** --> Fn ("0", [])
linear_add** -->
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
linear_add** -->
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "z"]);
    Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])])
lint* -->
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "z"]);
    Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])])
linear_sub <-- ["z"; "y"; "v_2"]
linear_sub --> <fun>
linear_sub* <--
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "z"]);
    Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])])
linear_sub* --> <fun>
linear_sub** <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])
linear_neg <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])
linear_neg -->
  Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <--
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "z"]);
    Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])])
linear_add* --> <fun>
linear_add** <--
  Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])
earlier <-- [<poly>; <poly>; <poly>]
earlier --> <fun>
earlier* <-- <poly>
earlier* --> <fun>
earlier** <-- <poly>
earlier** --> true
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
linear_add* --> <fun>
linear_add** <--
  Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])
earlier <-- [<poly>; <poly>; <poly>]
earlier --> <fun>
earlier* <-- <poly>
earlier* --> <fun>
earlier** <-- <poly>
earlier <-- [<poly>; <poly>]
earlier --> <fun>
earlier* <-- <poly>
earlier* --> <fun>
earlier** <-- <poly>
earlier** --> true
earlier** --> true
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <-- Fn ("0", [])
linear_add* --> <fun>
linear_add** <--
  Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <-- Fn ("0", [])
linear_add* --> <fun>
linear_add** <-- Fn ("0", [])
linear_add** --> Fn ("0", [])
linear_add** -->
  Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])
linear_add** -->
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "y"]);
    Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])])
linear_add** -->
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "z"]);
    Fn ("+",
     [Fn ("*", [Fn ("1", []); Var "y"]);
      Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])])])
linear_sub** -->
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "z"]);
    Fn ("+",
     [Fn ("*", [Fn ("1", []); Var "y"]);
      Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])])])
lint* -->
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "z"]);
    Fn ("+",
     [Fn ("*", [Fn ("1", []); Var "y"]);
      Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])])])
linear_neg <--
  Fn ("+",
   [Fn ("*", [Fn ("1", []); Var "z"]);
    Fn ("+",
     [Fn ("*", [Fn ("1", []); Var "y"]);
      Fn ("+", [Fn ("*", [Fn ("-1", []); Var "v_2"]); Fn ("0", [])])])])
linear_neg -->
  Fn ("+",
   [Fn ("*", [Fn ("-1", []); Var "z"]);
    Fn ("+",
     [Fn ("*", [Fn ("-1", []); Var "y"]);
      Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])])
lint* -->
  Fn ("+",
   [Fn ("*", [Fn ("-1", []); Var "z"]);
    Fn ("+",
     [Fn ("*", [Fn ("-1", []); Var "y"]);
      Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])])
linear_sub <-- ["z"; "y"; "v_2"]
linear_sub --> <fun>
linear_sub* <--
  Fn ("+",
   [Fn ("*", [Fn ("-1", []); Var "z"]);
    Fn ("+",
     [Fn ("*", [Fn ("-1", []); Var "y"]);
      Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])])
linear_sub* --> <fun>
linear_sub** <--
  Fn ("+", [Fn ("*", [Fn ("-1", []); Var "y"]); Fn ("0", [])])
linear_neg <--
  Fn ("+", [Fn ("*", [Fn ("-1", []); Var "y"]); Fn ("0", [])])
linear_neg -->
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <--
  Fn ("+",
   [Fn ("*", [Fn ("-1", []); Var "z"]);
    Fn ("+",
     [Fn ("*", [Fn ("-1", []); Var "y"]);
      Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])])
linear_add* --> <fun>
linear_add** <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
earlier <-- [<poly>; <poly>; <poly>]
earlier --> <fun>
earlier* <-- <poly>
earlier* --> <fun>
earlier** <-- <poly>
earlier** --> true
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <--
  Fn ("+",
   [Fn ("*", [Fn ("-1", []); Var "y"]);
    Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])
linear_add* --> <fun>
linear_add** <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "y"]); Fn ("0", [])])
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <--
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])
linear_add* --> <fun>
linear_add** <-- Fn ("0", [])
linear_add <-- ["z"; "y"; "v_2"]
linear_add --> <fun>
linear_add* <-- Fn ("0", [])
linear_add* --> <fun>
linear_add** <-- Fn ("0", [])
linear_add** --> Fn ("0", [])
linear_add** -->
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])
linear_add** -->
  Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])
linear_add** -->
  Fn ("+",
   [Fn ("*", [Fn ("-1", []); Var "z"]);
    Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])
linear_sub** -->
  Fn ("+",
   [Fn ("*", [Fn ("-1", []); Var "z"]);
    Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])
lint* -->
  Fn ("+",
   [Fn ("*", [Fn ("-1", []); Var "z"]);
    Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])
mkatom** -->
  Atom
   (R ("=",
     [Fn ("0", []);
      Fn ("+",
       [Fn ("*", [Fn ("-1", []); Var "z"]);
        Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])]))
linform* -->
  Atom
   (R ("=",
     [Fn ("0", []);
      Fn ("+",
       [Fn ("*", [Fn ("-1", []); Var "z"]);
        Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])]))
- : fol formula =
Atom
 (R ("=",
   [Fn ("0", []);
    Fn ("+",
     [Fn ("*", [Fn ("-1", []); Var "z"]);
      Fn ("+", [Fn ("*", [Fn ("1", []); Var "v_2"]); Fn ("0", [])])])]))
*)