// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

module Calculemus.Tests.Stal

open Xunit
open FsUnit.Xunit
open FsUnitTyped

open Calculemus

open Stal
open Propexamples

[<Fact>]
let ``stalmarck (prime 11) should return true.``() = 
    stalmarck (prime 11)
    |> should equal true

// [<Fact>] // slow test
let ``stalmarck (mk_adder_test 6 3) should return true.``() = 
    stalmarck (mk_adder_test 6 3)
    |> should equal true