// ========================================================================= //
// Copyright (c) 2023 Domenico Masini (added fsi file for documentation)     //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //
namespace Calculemus

open Lib.Fpf

open Formulas
open Fol

/// <summary>
/// Paramodulation.
/// </summary>
/// 
/// <category index="5">Equality</category>
module Paramodulation = 

    /// <summary>
    /// Finds all possibile 'overlaps' of an equation with a literal.
    /// </summary>
    /// 
    /// <param name="l">The LHS of the input equation.</param>
    /// <param name="r">The RHS of the input equation.</param>
    /// <param name="fm">The input literal.</param>
    /// <param name="rfn">The function that applied to the resulting instantiation gives the critical pair arising from that overlap.</param>
    /// 
    /// <returns>
    /// All possibile overlaps of the equation <c>l = r</c> with <c>fm</c>.
    /// </returns>
    val overlapl:
      l: term * r: term ->
        fm: formula<fol> ->
        rfn: (func<string,term> -> formula<fol> -> 'a) ->
        'a list

    /// <summary>
    /// Finds all possibile 'overlaps' of an equation with a clause.
    /// </summary>
    /// 
    /// <param name="l">The LHS of the input equation.</param>
    /// <param name="r">The RHS of the input equation.</param>
    /// <param name="cl">The input literal.</param>
    /// <param name="rfn">The function that applied to the resulting instantiation gives the critical pair arising from that overlap.</param>
    /// 
    /// <returns>
    /// All possibile overlaps of the equation <c>l = r</c> with <c>fm</c>.
    /// </returns>
    val overlapc:
      l: term * r: term ->
        cl: formula<fol> list ->
        rfn: (func<string,term> -> formula<fol> list -> 'a) ->
        acc: 'a list -> 'a list

    /// <summary>
    /// Applies paramodulation to a clause <c>ocl</c> using all the positive 
    /// equations in a paramodulating clause <c>pcl</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Paramodulation is the following inference rule, where \(s = t\) may
    /// be either \(s = t\) or \(t = s\):
    /// \[
    ///     \dfrac
    ///     {C \lor s = t \quad D \lor P[s']}
    ///     {\text{subst}\ \sigma\ (C \lor D \lor P[t])}
    /// \]
    /// </remarks>
    /// 
    /// <param name="pcl">The paramodulating clause.</param>
    /// <param name="ocl">The clause to which paramodulation is applied.</param>
    /// 
    /// <returns>
    /// The list of clauses resulting from the paramodulation.
    /// </returns>
    /// 
    /// <example id="paramodulate-1">
    /// <code lang="fsharp">
    /// paramodulate !!>["C";"S(0) = 1"] !!>["P(S(x))";"D"]
    /// </code>
    /// Evaluates to <c>[[`C`; `D`; `P(1)`]]</c>.
    /// </example>
    val paramodulate:
      pcl: formula<fol> list ->
        ocl: formula<fol> list -> formula<fol> list list

    /// <summary>
    /// Performs paramodulation of each clause within the other.
    /// </summary>
    /// 
    /// <param name="cls1">The first input clause.</param>
    /// <param name="cls2">The second input clause.</param>
    /// 
    /// <returns>
    /// All the paramodulants between the two clauses.
    /// </returns>
    val para_clauses:
      cls1: formula<fol> list ->
        cls2: formula<fol> list -> formula<fol> list list

    /// <summary>
    /// Resolution loop with paramodulation.
    /// </summary>
    /// 
    /// <param name="used">The working list of clauses, initially empty.</param>
    /// <param name="unused">The input list of clauses to be refuted.</param>
    /// 
    /// <returns>
    /// true, if a refutation with paramodulation for <c>unused</c> is found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found.</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>
    /// </note>
    /// 
    val paraloop:
      used: formula<fol> list list *
      unused: formula<fol> list list -> bool

    /// <summary>
    /// Tests the unsatisfiability of a formula using a resolution with 
    /// paramodulation.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// true, if the input formula is unsatisfiable and a refutation could be 
    /// found.
    /// </returns>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no refutation could be found (and this is always the case if the formula is either valid or satisfiable).</exception>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    val pure_paramodulation: fm: formula<fol> -> bool

    /// <summary>
    /// Tests the validity of a formula splitting it in subproblems and then 
    /// testing them with a resolution procedure with paramodulation.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The list of the results of 
    /// <see cref='M:Calculemus.Paramodulation.pure_paramodulation'/> on each 
    /// subproblems, if the formula is valid and a proof could be found.
    /// </returns>
    /// 
    /// <note>
    /// Prints diagnostic informations to the <c>stdout</c>.
    /// </note>
    /// 
    /// <exception cref="T:System.Exception">Thrown with message <c>No proof found</c> when no proof could be found.</exception>
    /// 
    /// <example id="paramodulation-1">
    /// <code lang="fsharp">
    /// !! @"(forall x. f(f(x)) = f(x)) /\ (forall x. exists y. f(y) = x)
    ///     ==> forall x. f(x) = x"
    /// |> paramodulation
    /// </code>
    /// Evaluates to <c>[true]</c> and prints to the <c>stdout</c>:
    /// <code lang="fsharp">
    /// 0 used; 4 unused.
    /// 1 used; 3 unused.
    /// 2 used; 4 unused.
    /// 3 used; 8 unused.
    /// 4 used; 11 unused.
    /// 5 used; 13 unused.
    /// 6 used; 21 unused.
    /// 7 used; 40 unused.
    /// 8 used; 64 unused.
    /// 9 used; 83 unused.
    /// 10 used; 108 unused.
    /// 11 used; 125 unused.
    /// </code>
    /// </example>
    val paramodulation: fm: formula<fol> -> bool list