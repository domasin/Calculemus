#r "../src/Calculemus/bin/Debug/net7.0/Calculemus.dll"
// #r "nuget: Calculemus, 1.0.3"

open Calculemus
open Calculemus.Lib
open Calculemus.Intro
open Calculemus.Formulas
open Calculemus.Prop
open Calculemus.Fol

// let v1 = function 
//     | 0 -> true
//     | 1 -> false

// let v2 = function 
//     | 0 -> true
//     | 1 -> true

// let v3 = function 
//     | 0 -> false
//     | 1 -> true

// let subfn (v:int -> bool) = 
//     if v 0 = false then 
//         true
//     else
//         false
// [3]
// |> onallvaluations subfn v3

module FormulasAndTermsFormatter =

    Formatter.SetPreferredMimeTypesFor(typeof<bool list> ,"text/plain")
    Formatter.Register<bool list>((fun xs -> 
                                    xs
                                    |> Seq.map (fun x -> sprintf "%A" x)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<int list> ,"text/plain")
    Formatter.Register<int list>((fun xs -> 
                                    xs
                                    |> Seq.map (fun x -> sprintf "%A" x)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")

    // expression
    Formatter.SetPreferredMimeTypesFor(typeof<expression> ,"text/plain")
    Formatter.Register<expression>((fun expr -> expr |> sprint_exp), "text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<expression list> ,"text/plain")
    Formatter.Register<expression list>((fun xs -> 
                                    xs
                                    |> Seq.map (sprint_exp)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")
    
    Formatter.SetPreferredMimeTypesFor(typeof<expression * expression> ,"text/plain")
    Formatter.Register<expression * expression>((fun (x,y) -> 
                                    sprintf "(%s, %s)" (x |> sprint_exp) (y |> sprint_exp)),"text/plain")
    
    // prop formula
    Formatter.SetPreferredMimeTypesFor(typeof<formula<prop>> ,"text/plain")
    Formatter.Register<formula<prop>>((fun fm -> sprint_prop_formula fm), "text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<formula<prop> list> ,"text/plain")
    Formatter.Register<formula<prop> list>((fun xs -> 
                                    xs
                                    |> Seq.map (sprint_prop_formula)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<formula<prop> * formula<prop>> ,"text/plain")
    Formatter.Register<formula<prop> * formula<prop>>((fun (x,y) -> 
                                    sprintf "(%s, %s)" (x |> sprint_prop_formula) (y |> sprint_prop_formula)),"text/plain")

    // fol terms
    Formatter.SetPreferredMimeTypesFor(typeof<term> ,"text/plain")
    Formatter.Register<term>((fun tm -> tm |> sprint_term), "text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<term list> ,"text/plain")
    Formatter.Register<term list>((fun xs -> 
                                    xs
                                    |> Seq.map (sprint_term)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<term * term> ,"text/plain")
    Formatter.Register<term * term>((fun (x,y) -> 
                                    sprintf "(%s, %s)" (x |> sprint_term) (y |> sprint_term)),"text/plain")

    // fol formula
    Formatter.SetPreferredMimeTypesFor(typeof<formula<fol>> ,"text/plain")
    Formatter.Register<formula<fol>>((fun fm -> sprint_fol_formula fm), "text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<formula<fol> list> ,"text/plain")
    Formatter.Register<formula<fol> list>((fun xs -> 
                                    xs
                                    |> Seq.map (sprint_fol_formula)
                                    |> fun x -> sprintf "[%s]" (x |> String.concat "; ")),"text/plain")

    Formatter.SetPreferredMimeTypesFor(typeof<formula<fol> * formula<fol>> ,"text/plain")
    Formatter.Register<formula<fol> * formula<fol>>((fun (x,y) -> 
                                    sprintf "(%s, %s)" (x |> sprint_fol_formula) (y |> sprint_fol_formula)),"text/plain")

    // Formatter.SetPreferredMimeTypesFor(typeof<term> ,"text/plain")
    // Formatter.Register<term>((fun tm -> print_term tm), "text/plain")
    
    // Formatter.Register<term * term>((fun (x,y) -> 
    //     sprintf "(%s, %s)" (x |> print_term) (y |> print_term)
    // ),"text/plain")

    // Formatter.SetPreferredMimeTypesFor(typeof<thm> ,"text/plain")
    // Formatter.Register<thm>((fun thm -> print_thm thm), "text/plain")

    // Formatter.SetPreferredMimeTypesFor(typeof<Proof Location> ,"text/plain")
    // Formatter.Register<Proof Location>((fun loc -> "$" + (latexStr loc) + "$"), "text/plain")