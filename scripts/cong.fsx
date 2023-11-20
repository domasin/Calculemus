#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"

open Calculemus
open Lib.Fpf
open Lib.Partition
open Fol
open Cong

// fsi.AddPrinter sprint_fol_formula
// fsi.AddPrinter sprint_term

congruent 
    (equate (!!!"2",!!!"4")unequal) 
    (!!!"f(4)", !!!"f(2)")

congruent 
    (equate (!!!"2",!!!"4")unequal) 
    (!!!"f(4)", !!!"f(3)")

(unequal,undefined)
|> emerge (!!!"0",!!!"1") 
|> fun (Partition f,pfn) -> graph f, graph pfn

predecessors !!!"f(0,g(1,0))" undefined
|> graph

!!>["m(0,1)=1";"~(m(0,1)=0)"]
|> ccsatisfiable

!!>["P(0)"]
|> ccsatisfiable

!! @"f(f(f(f(f(c))))) = c /\ f(f(f(c))) = c
==> f(c) = c \/ f(g(c)) = g(f(c))"
|> ccvalid

!! @"f(f(f(f(f(c))))) = c /\ f(f(f(c))) = c
==> f(c) = c"
|> ccvalid

!! @"f(f(f(f(c)))) = c /\ f(f(c)) = c ==> f(c) = c"
|> ccvalid

!!"P(0)"
|> ccvalid