// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus

open Calculemus.Lib.Fpf
open Calculemus.Formulas

/// <summary>
/// Basic stuff for propositional logic: datatype, parsing and prettyprinting, 
/// syntax and semantics, normal forms.
/// </summary>
/// 
/// <note>
/// Although this module defines a specific type 
/// <see cref='T:Calculemus.Prop.prop'/> for primitive propositions, most of 
/// the functions defined here for propositional logic are applicable (and 
/// intended to be applied) in general to any kind of 
/// <see cref='T:Calculemus.Formulas.formula`1'/> and in particular 
/// to our specific type of first order logic formulas 
/// <see cref='T:Calculemus.FolModule.fol'/>. These functions handle symbolic 
/// computation at the propositional level for any kind of 
/// <see cref='T:Calculemus.Formulas.formula`1'/> unless the signature 
/// restricts them to the <see cref='T:Calculemus.Prop.prop'/> type.
/// <p>
/// </p>
/// As remarked in the handbook, the defined type 
/// <see cref='T:Calculemus.Prop.prop'/> for propositional variables is fixed 
/// here just to make experimentation with some of the operations easier.
/// </note>
/// 
/// <category index="3">Propositional logic</category>
module Prop = 

    /// <summary>
    /// Type of propositional variables.
    /// </summary>
    type prop = 
        /// <summary>
        /// Propositional variable.
        /// </summary>
        /// 
        /// <param name="Item">Name of the propositional variable.</param>
        | P of string

    /// <summary>
    /// Returns the name of a propositional variable.
    /// </summary>
    /// 
    /// <param name="p">The input propositional variable.</param>
    /// <returns>The name of the propositional variable.</returns>
    /// 
    /// <example id="pname-1">
    /// <code lang="fsharp">
    /// P "x" |> pname
    /// </code>
    /// Evaluates to <c>"x"</c>.
    /// </example>
    /// 
    /// <category index="3">Syntax operations</category>
    val inline pname: p: prop -> string

    /// <summary>
    /// Parses atomic propositional variables.
    /// </summary>
    /// 
    /// <category index="1">Parsing</category>
    val parse_propvar:
      vs: 'a -> inp: string list -> formula<prop> * string list

    /// <summary>
    /// Parses a string in a propositional formula.
    /// </summary>
    /// 
    /// <category index="1">Parsing</category>
    val parse_prop_formula: (string -> formula<prop>)

    /// <summary>
    /// A convenient parsing operator to make it easier to parse prop formulas
    /// </summary>
    /// 
    /// <param name="s">The string to be parsed.</param>
    /// <returns>The parsed prop formula.</returns>
    /// 
    /// <example id="exclamation-greater-1">
    /// <code lang="fsharp">
    /// !> "p /\ q ==> q /\ r"
    /// </code>
    /// Evaluates to <c>Imp (And (Atom (P "p"), Atom (P "q")), And (Atom (P "q"), Atom (P "r")))</c>.
    /// </example>
    /// 
    /// <category index="1">Parsing</category>
    val (!>): s: string -> formula<prop>

    /// <summary>
    /// Prints a propositional variable using a TextWriter.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val fprint_propvar: sw: System.IO.TextWriter -> prec: 'a -> p: prop -> unit

    /// <summary>
    /// Prints a propositional variable.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val inline print_propvar: prec: 'a -> p: prop -> unit

    /// <summary>
    /// Returns a string representation of a propositional variable.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val inline sprint_propvar: prec: 'a -> p: prop -> string

    /// <summary>
    /// Prints a propositional formula using a TextWriter.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val fprint_prop_formula:
      sw: System.IO.TextWriter -> (formula<prop> -> unit)

    /// <summary>
    /// Prints a propositional formula.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val inline print_prop_formula: f: formula<prop> -> unit

    /// <summary>
    /// Returns a string representation of a propositional formula instead of 
    /// its abstract syntax tree.
    /// </summary>
    /// 
    /// <category index="2">Prettyprinting</category>
    val inline sprint_prop_formula: f: formula<prop> -> string

    /// <summary>
    /// Evaluates the truth-value of a formula given a valuation.
    /// </summary>
    /// 
    /// <remarks>
    /// A valuation is a function from the set of atoms to the set of 
    /// truth-values {<c>false</c>, <c>true</c>} (note that these are elements 
    /// of the metalanguage, in this case F#, that represent the semantic 
    /// concepts of truth-values and are not the same thing as <c>False</c> and 
    /// <c>True</c> which are element of the object language and so syntactic 
    /// elements).
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <param name="v">The input valuation.</param>
    /// <returns>
    /// <c>true</c>, if the formula is true in the give valuation; otherwise 
    /// <c>false</c>.
    /// </returns>
    /// 
    /// <example id="eval-1">
    /// The following evaluates the formula given a valuation that evaluates 
    /// <c>p</c> and <c>r</c> to <c>true</c> and <c>q</c> to <c>false</c>:
    /// <code lang="fsharp">
    /// eval (!>"p /\ q ==> q /\ r") 
    ///     (function P"p" -> true | P"q" -> false | P"r" -> true)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="4">Semantics</category>
    val eval: fm: formula<'a> -> v: ('a -> bool) -> bool

    /// <summary>
    /// Returns the set of atoms in a formula.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The set of atoms in the formula</returns>
    /// 
    /// <example id="atoms-1">
    /// <code lang="fsharp">
    /// !>"p /\ q ==> q /\ r" |> atoms
    /// </code>
    /// Evaluates to <c>[P "p"; P "q"; P "r"]</c>.
    /// </example>
    /// 
    /// <category index="3">Syntax operations</category>
    val atoms: fm: formula<'a> -> 'a list when 'a: comparison

    /// <summary>
    /// Tests whether a function <c>subfn</c> returns <c>true</c> on all 
    /// possible valuations of the atoms <c>ats</c>, using an existing 
    /// valuation <c>v</c> for all other atoms.
    /// </summary>
    /// 
    /// <remarks>
    /// This function is used to define both truth-table (see 
    /// <see cref='M:Calculemus.Prop.print_truthtable'/>) and 
    /// <see cref='M:Calculemus.Prop.tautology``1'/>
    /// </remarks>
    /// 
    /// <param name="subfn">A function that given a valuation return true or false.</param>
    /// <param name="v">The default valuation for other atoms.</param>
    /// <param name="ats">The list of atoms on which to test all possibile valuations.</param>
    /// 
    /// <returns>true, if <c>subfn</c> returns <c>true</c> on all 
    /// possible valuations of the atoms <c>ats</c> with 
    /// valuation <c>v</c> for all other atoms; otherwise false.</returns>
    /// 
    /// <example id="onallvaluations-1">
    /// <c>eval True</c> returns <c>true</c> on all valuations:
    /// <code lang="fsharp">
    /// onallvaluations (eval True) (fun _ -> false) []
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="5">Truth tables</category>
    val onallvaluations:
      subfn: (('a -> bool) -> bool) -> v: ('a -> bool) -> ats: 'a list -> bool
        when 'a: equality

    /// <summary>
    /// Returns all possible valuations of the atoms in <c>fm</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// This function is not part of the handbook's code and it is added 
    /// here for convenience and exercise.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>All possible valuations of the atoms in the formula.</returns>
    /// 
    /// <example id="allvaluations-1">
    /// <code lang="fsharp">
    /// let fm = !> @"(p /\ q) \/ s"
    /// 
    /// allvaluations fm
    /// // graphs of all valuations of atoms in fm
    /// |> List.map (fun v -> 
    ///     atoms fm
    ///     |> List.map (fun a -> (a, v a))
    /// )
    /// </code>
    /// Evaluates to:
    /// <code lang="fsharp">
    /// [[(P "p", false); (P "q", false); (P "s", false)];
    ///  [(P "p", false); (P "q", false); (P "s", true)];
    ///  [(P "p", false); (P "q", true); (P "s", false)];
    ///  [(P "p", false); (P "q", true); (P "s", true)];
    ///  [(P "p", true); (P "q", false); (P "s", false)];
    ///  [(P "p", true); (P "q", false); (P "s", true)];
    ///  [(P "p", true); (P "q", true); (P "s", false)];
    ///  [(P "p", true); (P "q", true); (P "s", true)]]
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Truth tables</category>
    val allvaluations: fm: formula<'a> -> list<('a -> bool)> when 'a: comparison

    /// <summary>
    /// Prints the truth table of the prop formula <c>fm</c> to a TextWriter 
    /// <c>sw</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// See also <see cref='M:Calculemus.Prop.print_truthtable'/>
    /// </remarks>
    /// 
    /// <param name="sw">The TextWriter to print to.</param>
    /// <param name="fm">The input prop formula.</param>
    /// 
    /// <example id="fprint_truthtable-1">
    /// <code lang="fsharp">
    /// let file = System.IO.File.CreateText("out.txt")
    /// fprint_truthtable file (!>"p ==> q")
    /// file.Close()
    /// </code>
    /// After evaluation the file contains the text
    /// <code lang="fsharp">
    /// p     q     |   formula
    /// ---------------------
    /// false false | true  
    /// false true  | true  
    /// true  false | false 
    /// true  true  | true  
    /// ---------------------
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Truth tables</category>
    val fprint_truthtable:
      sw: System.IO.TextWriter -> fm: formula<prop> -> unit

    /// <summary>
    /// Prints the truth table of the prop formula <c>fm</c> to the 
    /// <c>stdout</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// <p>Each logical connective is interpreted by a corresponding bool 
    /// operator of the metalanguage.</p>
    /// 
    /// <p>A truth-table shows how the truth-value assigned to a formula is 
    /// determined by those of its atoms, based on the interpretation of its 
    /// logical connectives.</p>
    /// 
    /// For binary connective we have:
    /// 
    /// \begin{array}{|c|c||c|c|c|c|}
    /// \hline
    /// p &amp; q &amp; p \land q &amp; p \lor q &amp; p \Rightarrow q &amp; p \Leftrightarrow q \\
    /// \hline
    /// false &amp; false &amp; false &amp; false &amp; true &amp; true \\
    /// \hline
    /// false &amp; true &amp; false &amp; true &amp; true &amp; false \\
    /// \hline
    /// true &amp; false &amp; false &amp; true &amp; false &amp; false \\
    /// \hline
    /// true &amp; true &amp; true &amp; true &amp; true &amp; true \\
    /// \hline
    /// \end{array}
    /// 
    /// while for the unary negation:
    /// 
    /// \begin{array}{|c||c|}
    /// 	\hline
    /// 	p &amp; \neg p \\
    /// 	\hline
    /// 	false &amp; true\\
    /// 	\hline
    /// 	true &amp; false\\
    /// 	\hline
    /// \end{array} 
    /// 
    /// <p>A truth table has one column for each propositional variable and one 
    /// final column showing all of the possible results of the logical 
    /// operation that the table represents. Each row of the truth table 
    /// contains one possible configuration of the propositional variables, 
    /// and the result of the operation for those values.</p>
    /// 
    /// In particular, truth-tables can be used to show whether (i) a prop 
    /// formula is logically valid (i.e. a tautology: see 
    /// <see cref='M:Calculemus.Prop.tautology``1'/> that, as this 
    /// function, is based on 
    /// <see cref='M:Calculemus.Prop.onallvaluations``1'/>) when the result 
    /// column has <c>true</c> in each rows of the table; (ii) 
    /// <see cref='M:Calculemus.Prop.satisfiable``1'/>, when the result colum 
    /// has <c>true</c> at least in one row; (iii) 
    /// <see cref='M:Calculemus.Prop.unsatisfiable``1'/> when has 
    /// <c>false</c> in each rows.
    /// </remarks>
    /// 
    /// <param name="fm">The input prop formula.</param>
    /// 
    /// <example id="print_truthtable-1">
    /// <code lang="fsharp">
    /// print_truthtable !>"p /\ q ==> q /\ r"
    /// </code>
    /// After evaluation the following text is printed to the 
    /// <c>stdout</c>:
    /// <code lang="fsharp">
    /// p     q     r     |   formula
    /// ---------------------------
    /// false false false | true  
    /// false false true  | true  
    /// false true  false | true  
    /// false true  true  | true  
    /// true  false false | true  
    /// true  false true  | true  
    /// true  true  false | false 
    /// true  true  true  | true  
    /// ---------------------------
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Truth tables</category>
    val inline print_truthtable: fm: formula<prop> -> unit

    /// <summary>
    /// Returns a string representation of the truth table of the prop formula 
    /// <c>fm</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// See also <see cref='M:Calculemus.Prop.print_truthtable'/>
    /// </remarks>
    /// 
    /// <param name="fm">The input prop formula.</param>
    /// <returns>
    /// The string representation of the truth table of the formula.
    /// </returns>
    /// 
    /// <example id="sprint_truthtable-1">
    /// <code lang="fsharp">
    /// sprint_truthtable !>"p ==> q"
    /// </code>
    /// Evaluates to:
    /// <code lang="fsharp">
    /// "p     q     |   formula
    /// ---------------------
    /// false false | true  
    /// false true  | true  
    /// true  false | false 
    /// true  true  | true  
    /// ---------------------
    /// 
    /// "
    /// </code>
    /// </example>
    /// 
    /// <category index="5">Truth tables</category>
    val inline sprint_truthtable: fm: formula<prop> -> string

    /// <summary>
    /// Checks if a formula is a tautology at the propositional level.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// true, if the formula is a tautology at the propositional level; 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="tautology-1">
    /// <code lang="fsharp">
    /// !> @"p \/ ~p"
    /// |> tautology
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="tautology-2">
    /// <code lang="fsharp">
    /// !> @"p \/ q ==> p"
    /// |> tautology
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="tautology-3">
    /// <code lang="fsharp">
    /// !> @"p \/ q ==> q \/ (p &lt;=&gt; q)"
    /// |> tautology
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="tautology-4">
    /// <code lang="fsharp">
    /// !> @"(p \/ q) /\ ~(p /\ q) ==> (~p &lt;=&gt; q)"
    /// |> tautology
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val tautology: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Checks if a formula is unsatisfiable at the propositional level.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// true, if the formula is a unsatisfiable at the propositional level; 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="unsatisfiable-1">
    /// <code lang="fsharp">
    /// !> "p /\ ~p"
    /// |> unsatisfiable
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="unsatisfiable-2">
    /// <code lang="fsharp">
    /// !> @"p"
    /// |> unsatisfiable
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val unsatisfiable: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Checks if a formula is satisfiable at the propositional level.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// <returns>
    /// true, if the formula is a satisfiable at the propositional level; 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="satisfiable-1">
    /// <code lang="fsharp">
    /// !> "p /\ ~p"
    /// |> satisfiable
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <example id="satisfiable-2">
    /// <code lang="fsharp">
    /// !> @"p"
    /// |> satisfiable
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="6">Tautology, unsatisfiability and satisfiability</category>
    val satisfiable: fm: formula<'a> -> bool when 'a: comparison

    /// <summary>
    /// Substitutes atoms in a formula with other formulas based on an fpf.
    /// </summary>
    /// 
    /// <param name="subfn">The fpf mapping atoms to formulas.</param>
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The formula resulting from substituting atoms of <c>fm</c> with the 
    /// formulas based on the mapping defined in <c>sbfn</c>.
    /// </returns>
    /// 
    /// <example id="psubst-1">
    /// <code lang="fsharp">
    /// !> "p /\ q /\ p /\ q"
    /// |> psubst (P"p" |=> !>"p /\ q")
    /// </code>
    /// Evaluates to <c>`(p /\ q) /\ q /\ (p /\ q) /\ q`</c>.
    /// </example>
    /// 
    /// <category index="3">Syntax operations</category>
    val psubst:
      subfn: func<'a,formula<'a>> ->
        fm: formula<'a> -> formula<'a> when 'a: comparison

    /// <summary>
    /// Returns the dual of the input formula: i.e. the result of 
    /// systematically exchanging <c>/\</c> with <c>\/</c> and also 
    /// <c>True</c> with <c>False</c>.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The dual of the input formula.</returns>
    /// 
    /// <example id="dual-1">
    /// <code lang="fsharp">
    /// !> @"p \/ ~p"
    /// |> dual
    /// </code>
    /// Evaluates to <c>`p /\ ~p`</c>.
    /// </example>
    /// 
    /// <category index="3">Syntax operations</category>
    val dual: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Performs a propositional simplification routine (but just at the first 
    /// level) of the input formula, eliminating the basic 
    /// propositional constants <c>False</c> and <c>True</c> and the double 
    /// negations <c>~~p</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// This function applies to formulas a simplification routine similar to 
    /// the one <see cref='M:Calculemus.Intro.simplify1'/> applies on 
    /// algebraic expressions.
    /// 
    /// It eliminates the basic propositional 
    /// constants \(\bot\) and \(\top\) based on the equivalences similar to 
    /// the following (see the implementation for details):
    /// <ul>
    /// <li>\(\bot \land p \Leftrightarrow \bot\)</li>
    /// <li>\(\top \lor p \Leftrightarrow p\)</li>
    /// <li>\(p \Rightarrow \bot \Leftrightarrow \neg p\)</li>
    /// <li>...</li>
    /// </ul>
    /// 
    /// At the same time, it also eliminates double negations \(\neg \neg p\).
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The formula simplified.</returns>
    /// 
    /// <example id="psimplify1-1">
    /// <code lang="fsharp">
    /// !> "false /\ p"
    /// |> psimplify1
    /// </code>
    /// Evaluates to <c>`false`</c>.
    /// </example>
    /// 
    /// <category index="7">Simplification</category>
    val psimplify1: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Performs a propositional simplification routine eliminating 
    /// the basic propositional constants <c>False</c> and <c>True</c> and the 
    /// double negations <c>~~p</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// Completes the simplification routine psimplify1 applying it at every 
    /// level of the formula.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>The formula simplified.</returns>
    /// 
    /// <example id="psimplify-1">
    /// <code lang="fsharp">
    /// !> "((x ==> y) ==> true) \/ ~false"
    /// |> psimplify
    /// </code>
    /// Evaluates to <c>`true`</c>.
    /// </example>
    /// 
    /// <category index="7">Simplification</category>
    val psimplify: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Checks if a literal is negative.
    /// </summary>
    /// 
    /// <remarks>
    /// A literal is an atomic formula or its negation. This function can be 
    /// applied to any kind of formulas but is specifically intended to be used 
    /// on literals.
    /// </remarks>
    /// 
    /// <param name="lit">The input literal.</param>
    /// 
    /// <returns>true, if the literal is negative; otherwise false.</returns>
    /// 
    /// <example id="negative-1">
    /// <code lang="fsharp">
    /// !> "~p" |> negative
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="negative-2">
    /// <code lang="fsharp">
    /// !> "p" |> negative
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="8">Literals</category>
    val negative: lit: formula<'a> -> bool

    /// <summary>
    /// Checks if a literal is positive.
    /// </summary>
    /// 
    /// <remarks>
    /// A literal is an atomic formula or its negation. This function can be 
    /// applied to any kind of formulas but is specifically intended to be used 
    /// on literals.
    /// </remarks>
    /// 
    /// <param name="lit">The input literal.</param>
    /// 
    /// <returns>true, if the literal is positive; otherwise false.</returns>
    /// 
    /// <example id="positive-1">
    /// <code lang="fsharp">
    /// !> "p" |> positive
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <example id="positive-2">
    /// <code lang="fsharp">
    /// !> "~p" |> positive
    /// </code>
    /// Evaluates to <c>false</c>.
    /// </example>
    /// 
    /// <category index="8">Literals</category>
    val positive: lit: formula<'a> -> bool

    /// <summary>Changes a literal into its contrary.</summary>
    /// 
    /// <remarks>
    /// A literal is an atomic formula or its negation. This function can be 
    /// applied to any kind of formulas but is specifically intended to be used 
    /// on literals.
    /// </remarks>
    /// 
    /// <param name="lit">The input literal.</param>
    /// 
    /// <returns>
    /// The negated literal if the input is positive and vice versa.
    /// </returns>
    /// 
    /// <example id="negate-1">
    /// <code lang="fsharp">
    /// !> "p" |> negate
    /// </code>
    /// Evaluates to <c>`~p`</c>.
    /// </example>
    /// 
    /// <example id="negate-2">
    /// <code lang="fsharp">
    /// !> "~p" |> negate
    /// </code>
    /// Evaluates to <c>`p`</c>.
    /// </example>
    /// 
    /// <category index="8">Literals</category>
    val negate: lit: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into a naive negation normal form.
    /// </summary>
    /// 
    /// <remarks>
    /// A formula is in <em>negation normal form</em> (NNF) if it is 
    /// constructed from literals using only the binary connectives \(\land\) 
    /// and \(\lor\), or else is one of the degenerate cases \(\top\) and 
    /// \(\bot\).
    /// 
    /// <c>nnf_naive</c> implements an incomplete transformation of NNF which 
    /// pushes down negation on atoms and removes the binary connective 
    /// \(\Rightarrow\) and \(\Leftrightarrow\) but keeps \(\top\) and 
    /// \(\bot\) mixed with other formulas.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in a naive negation normal form.
    /// </returns>
    /// 
    /// <example id="nnf_naive-1">
    /// <code lang="fsharp">
    /// !> "~ (p ==> false)"
    /// |> nnf_naive
    /// </code>
    /// Evaluates to <c>`p /\ ~false`</c>.
    /// </example>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nnf_naive: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into a naive negation normal form.
    /// </summary>
    /// 
    /// <remarks>
    /// A formula is in <em>negation normal form</em> (NNF) if it is 
    /// constructed from literals using only the binary connectives \(\land\) 
    /// and \(\lor\), or else is one of the degenerate cases \(\top\) and 
    /// \(\bot\).
    /// <p>
    /// </p>
    /// It is analogous to the following procedure in ordinary algebra: 
    /// (i) replace subtraction by its definition \(x - y = x + -y\) 
    /// and then (ii) systematically push negations down using 
    /// \(-(x + y) = -x + -y\), \(-(xy) = (-x)y\), \(-(-x) = x\).
    /// <p>
    /// </p>
    /// <c>nnf</c> implements a complete transformation in NNF 
    /// applying <see cref='M:Calculemus.Prop.psimplify``1'/> first 
    /// and then <see cref='M:Calculemus.Prop.nnf_naive``1'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in a negation normal form.
    /// </returns>
    /// 
    /// <example id="nnf-1">
    /// <code lang="fsharp">
    /// !> "~ (p ==> false)"
    /// |> nnf
    /// </code>
    /// Evaluates to <c>`p`</c>.
    /// </example>
    /// 
    /// <example id="nnf-2">
    /// <code lang="fsharp">
    /// !> "(p &lt;=&gt; q) &lt;=&gt; ~(r ==> s)"
    /// |> nnf
    /// </code>
    /// Evaluates to <c>`(p /\ q \/ ~p /\ ~q) /\ r /\ ~s \/ (p /\ ~q \/ ~p /\ q) /\ (~r \/ s)`</c>.
    /// </example>
    /// 
    /// <note>
    /// Negation normal form is not a canonical form: for example, 
    /// \(a \land (b \lor \lnot c)\) and \((a \land b) \lor (a \land \lnot c)\) 
    /// are equivalent, and are both in negation normal form.
    /// (from 
    /// <a href="https://en.wikipedia.org/wiki/Negation_normal_form">https://en.wikipedia.org/wiki/Negation_normal_form</a>)
    /// </note>
    /// 
    /// <category index="9">Negation Normal Form</category> 
    val nnf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into negation normal form but keeps logical 
    /// equivalences and <c>False</c> and <c>True</c> mixed with other 
    /// formulas.
    /// </summary>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in a negation normal form that keeps 
    /// equivalences and logical constants mixed with other formulas.
    /// </returns>
    /// 
    /// <example id="nenf_naive-1">
    /// <code lang="fsharp">
    /// !> "~ (p &lt;=&gt; q)"
    /// |> nenf_naive
    /// </code>
    /// Evaluates to <c>`p &lt;=&gt; ~q`</c>.
    /// </example>
    /// 
    /// <example id="nenf_naive-2">
    /// <code lang="fsharp">
    /// !> "~ (false &lt;=&gt; q)"
    /// |> nenf_naive
    /// </code>
    /// Evaluates to <c>`false &lt;=&gt; ~q`</c>.
    /// </example>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nenf_naive: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Changes a formula into negation normal form but keeps logical 
    /// equivalences.
    /// </summary>
    /// 
    /// <remarks>
    /// Applies <see cref='M:Calculemus.Prop.psimplify``1'/> first 
    /// and then <see cref='M:Calculemus.Prop.nenf_naive``1'/>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in a negation normal form that keeps 
    /// equivalences.
    /// </returns>
    /// 
    /// <example id="nenf-1">
    /// <code lang="fsharp">
    /// !> "~ (p &lt;=&gt; q)"
    /// |> nenf
    /// </code>
    /// Evaluates to <c>`p &lt;=&gt; ~q)`</c>.
    /// </example>
    /// 
    /// <example id="nenf-2">
    /// <code lang="fsharp">
    /// !> "~ (false &lt;=&gt; q)"
    /// |> nenf
    /// </code>
    /// Evaluates to <c>`q`</c>.
    /// </example>
    /// 
    /// <category index="9">Negation Normal Form</category>
    val nenf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Constructs a conjunction from a list of formulas.
    /// </summary>
    /// 
    /// <param name="l">The list of formulas to conjunct.</param>
    /// 
    /// <returns>The conjunction of the input formulas.</returns>
    /// 
    /// <example id="list_conj-1">
    /// <code lang="fsharp">
    /// list_conj [!>"p";!>"q";!>"r"]
    /// </code>
    /// Evaluates to <c>`p /\ q /\ r`</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val list_conj:
      l: formula<'a> list -> formula<'a> when 'a: equality

    /// <summary>
    /// Constructs a disjunction from a list of formulas.
    /// </summary>
    /// 
    /// <param name="l">The list of formulas to disjunct.</param>
    /// 
    /// <returns>The disjunction of the input formulas.</returns>
    /// 
    /// <example id="list_disj-1">
    /// <code lang="fsharp">
    /// list_disj [!>"p";!>"q";!>"r"]
    /// </code>
    /// Evaluates to <c>`p \/ q \/ r`</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val list_disj:
      l: formula<'a> list -> formula<'a> when 'a: equality

    /// <summary>
    /// Constructs a conjunction from a list of formulas <c>pvs</c> and their 
    /// negations, according to whether each is satisfied by a valuation 
    /// <c>v</c>.
    /// </summary>
    /// 
    /// <remarks>
    /// As the name suggest, it is intended to be used on literals and 
    /// actually is used just to define 
    /// <see cref='M:Calculemus.Prop.dnf_by_truth_tables``1'/>
    /// </remarks>
    /// 
    /// <param name="pvs">The input list of formulas.</param>
    /// <param name="v">The input valuation.</param>
    /// 
    /// <returns>
    /// The conjunction of the <c>pvs</c> formulas (positive or negated 
    /// depending on <c>v</c>).
    /// </returns>
    /// 
    /// <example id="mk_lits-1">
    /// <code lang="fsharp">
    /// mk_lits [!>"p";!>"q"] 
    ///     (function P"p" -> true | P"q" -> false)
    /// </code>
    /// Evaluates to <c>`p /\ ~q`</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val mk_lits:
      pvs: formula<'a> list -> v: ('a -> bool) -> formula<'a>
        when 'a: equality

    /// <summary>
    /// A close analogue of 
    /// <see cref='M:Calculemus.Prop.onallvaluations``1'/> that collects 
    /// into a list the valuations for which <c>subfn</c> holds.
    /// </summary>
    /// 
    /// <param name="subfn">A function that given a valuation return true or false.</param>
    /// <param name="v">The default valuation for other atoms.</param>
    /// <param name="pvs">The list of atoms on which to test all possibile valuations.</param>
    /// 
    /// <example id="allsatvaluations-1">
    /// <code lang="fsharp">
    /// let fm = !> "p /\ q"
    /// let atms = atoms fm
    /// let satvals = allsatvaluations (eval fm) (fun _ -> false) atms
    /// 
    /// satvals[0] (P"p") // true
    /// satvals[0] (P"q") // true
    /// satvals[0] (P"x") // false
    /// </code>
    /// </example>
    /// 
    /// <example id="allsatvaluations-2">
    /// <c>allsatvaluations</c> applied to <c>eval fm</c> 
    /// extracts all valuations satisfying <c>fm</c>.
    /// <code lang="fsharp">
    /// let fm = !> @"(p /\ q) \/ (s /\ t)"
    /// let atms = atoms fm
    /// 
    /// allsatvaluations (eval fm) (fun _ -> false) atms
    /// // graphs of all valuations satisfying fm
    /// |> List.map (fun v -> 
    ///     atms
    ///     |> List.map (fun a -> (a, v a))
    /// )
    /// </code>
    /// Evaluates to:
    /// <code lang="fsharp">
    /// [[(P "p", false); (P "q", false); (P "s", true); (P "t", true)];
    ///  [(P "p", false); (P "q", true); (P "s", true); (P "t", true)];
    ///  [(P "p", true); (P "q", false); (P "s", true); (P "t", true)];
    ///  [(P "p", true); (P "q", true); (P "s", false); (P "t", false)];
    ///  [(P "p", true); (P "q", true); (P "s", false); (P "t", true)];
    ///  [(P "p", true); (P "q", true); (P "s", true); (P "t", false)];
    ///  [(P "p", true); (P "q", true); (P "s", true); (P "t", true)]]
    /// </code>
    /// </example>
    /// <returns>
    /// The list of valuations for which <c>subfn</c> holds on <c>pvs</c>.
    /// </returns>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val allsatvaluations:
      subfn: (('a -> bool) -> bool) ->
        v: ('a -> bool) -> pvs: 'a list -> ('a -> bool) list when 'a: equality

    /// <summary>
    /// Transforms a formula <c>fm</c> in disjunctive normal form by the 
    /// valuations satisfying it.
    /// </summary>
    /// 
    /// <remarks>
    /// See <see cref='M:Calculemus.Prop.dnf``1'/> for a definition of DNF.
    /// <p>
    /// </p>
    /// <c>dnf_by_truth_tables</c>, from all valuations satisfying the formula, 
    /// generates an equivalent that is the disjunction of the conjunctions of 
    /// the atoms that in each evaluation are mapped to true or their negations 
    /// if they're mapped to false. Thus, it is based on the same semantic 
    /// process of truth-tables.
    /// <p>
    /// </p>
    /// Note also that since it is based on truth-tables (or more precisely 
    /// on valuations), it generates disjunctions in which each conjunct 
    /// contains every atoms of the original formula, while other dnf functions 
    /// generate more cleaner results.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The input formula transformed in disjunctive normal form.
    /// </returns>
    /// 
    /// <example id="dnf_by_truth_tables-1">
    /// <code lang="fsharp">
    /// !> @"p ==> q"
    /// |> dnf_by_truth_tables 
    /// // Evaluates to `~p /\ ~q \/ ~p /\ q \/ p /\ q`
    /// 
    /// // Note the symmetry between the conjunctions 
    /// // and the true-rows of the truth table.
    /// !> @"p ==> q"
    /// |> print_truthtable
    /// 
    /// // p     q     |   formula
    /// // ---------------------
    /// // false false | true  
    /// // false true  | true  
    /// // true  false | false 
    /// // true  true  | true  
    /// // ---------------------
    /// </code>
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val dnf_by_truth_tables:
      fm: formula<'a> -> formula<'a> when 'a: comparison

    /// <summary>
    /// Applies the distributive laws of <c>/\</c> and <c>\/</c> to the input 
    /// formula <c>fm</c>, assuming that its immediate subformulas are in DNF.
    /// </summary>
    /// 
    /// <remarks>
    /// The distributive laws are the following:
    /// <ul>
    /// <li><c>p /\ (q \/ r) &lt;=&gt; p /\ q \/ p /\ r</c></li>
    /// <li><c>(p \/ q) /\ r &lt;=&gt; p /\ r \/ q /\ r</c>.</li>
    /// </ul>
    /// Analogous to the following in algebra:
    /// <ul>
    /// <li><c>x(y + z) = xy + xz</c></li>
    /// <li><c>(x + y)z = xz + yz</c>.</li>
    /// </ul>
    /// </remarks>
    /// 
    /// <param name="fm">The input formula with immediate subformulas in DNF.</param>
    /// 
    /// <returns>
    /// The formula transformed with the distributive laws of 
    /// <c>/\</c> and <c>\/</c>, if its immediate subformulas are  in DNF; 
    /// otherwise, the input unchanged.
    /// </returns>
    /// 
    /// <example id="distrib_naive-1">
    /// <code lang="fsharp">
    /// !> @"p /\ (q \/ r)" |> distrib_naive
    /// </code>
    /// Evaluates to <c>`p /\ q \/ p /\ r`</c>.
    /// </example>
    /// 
    /// <example id="distrib_naive-2">
    /// <code lang="fsharp">
    /// !> "p ==> q" |> distrib_naive
    /// </code>
    /// Evaluates to <c>`p ==> q`</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val distrib_naive: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Transforms an NNF formula in a raw disjunctive normal form.
    /// </summary>
    /// 
    /// <remarks>
    /// It is raw because:
    /// <ol>
    /// <li>it requires the input formula being in NNF;</li>
    /// <li>
    /// it's  hard to read due to mixed associations in iterated conjunctions 
    /// and disjunctions enclosed in parentheses. E.g. 
    /// <c>(p /\ q \/ p /\ r) \/ s</c> instead of <c>p /\ q \/ p /\ r \/ s</c>;
    /// </li>
    /// <li>it can contain redundant disjuncts equivalent to <c>False</c>.</li>
    /// </ol>
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// The formula transformed in a raw DNF if the input is already in NNF; 
    /// otherwise, the input unchanged.
    /// </returns>
    /// 
    /// <example id="rawdnf-1">
    /// <code lang="fsharp">
    /// !> @"p /\ (q \/ r) \/ s" |> rawdnf
    /// </code>
    /// Evaluates to <c>`(p /\ q \/ p /\ r) \/ s`</c>.
    /// </example>
    /// 
    /// <example id="rawdnf-2">
    /// <code lang="fsharp">
    /// !> "p ==> q" |> rawdnf
    /// </code>
    /// Evaluates to <c>`p ==> q`</c>.
    /// </example>
    /// 
    /// <example id="rawdnf-3">
    /// <code lang="fsharp">
    /// !> "(p \/ q /\ r) /\ (~p \/ ~r)" |> rawdnf
    /// </code>
    /// Evaluates to 
    /// <c>`(p /\ ~p \/ (q /\ r) /\ ~p) \/ p /\ ~r \/ (q /\ r) /\ ~r`</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val rawdnf: fm: formula<'a> -> formula<'a>

    /// <summary>
    /// Takes two sets of sets and returns the set of the unions of each set in 
    /// the first with sets in the second.
    /// </summary>
    /// 
    /// <remarks>
    /// It is used to obtain the distributive laws of <c>/\</c> and  
    /// <c>\/</c> (see <see cref='M:Calculemus.Prop.distrib_naive``1'/>) in 
    /// the context of a set of sets representation of dnf (see 
    /// <see cref='M:Calculemus.Prop.purednf``1'/>)
    /// </remarks>
    /// 
    /// <param name="s1">The first input set of set.</param>
    /// <param name="s2">The second input set of set.</param>
    /// 
    /// <returns>The set of the unions of first sets with the latter.</returns>
    /// 
    /// <example id="distrib-1">
    /// <code lang="fsharp">
    /// distrib [[1;2];[2]] [[3];[4]]
    /// </code>
    /// Evaluates to 
    /// <c>[[1; 2; 3]; [1; 2; 4]; [2; 3]; [2; 4]]</c>.
    /// </example>
    /// 
    /// <example id="distrib-2">
    /// Representing the distributive laws:
    /// <code lang="fsharp">
    /// distrib [["p"]] [["q"];["r"]] // [["p"; "q"]; ["p"; "r"]]
    /// distrib [["p"];["q"]] [["r"]] // [["p"; "r"]; ["q"; "r"]]
    /// </code>
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val distrib:
      s1: 'a list list -> s2: 'a list list -> 'a list list when 'a: comparison

    /// <summary>
    /// Transform a formula already in NNF in DNF with a set of sets 
    /// representation form as output (with possible superfluous and subsumed 
    /// disjuncts in the result).
    /// </summary>
    /// 
    /// <remarks>
    /// The resulting global set represents the disjunction and the 
    /// subsets the conjunctions: e.g. the set of sets representation of
    /// <c>p /\ q \/ ~ p /\ r</c> is <c>[[p; q]; [~ p; r]]</c>.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// A dnf equivalent of the input formula in a set of sets representation 
    /// with possible superfluous and subsumed disjuncts, if the input is in 
    /// NNF; otherwise a meaningless list of lists of the input itself.
    /// </returns>
    /// 
    /// <example id="purednf-1">
    /// <code lang="fsharp">
    /// !> @"(p \/ q /\ r) /\ (~p \/ ~r)"
    /// |> purednf
    /// </code>
    /// Evaluates to 
    /// <c>[[`p`; `~p`]; [`p`; `~r`]; [`q`; `r`; `~p`]; [`q`; `r`; `~r`]]</c>.
    /// </example>
    /// 
    /// <example id="purednf-2">
    /// <code lang="fsharp">
    /// !> "p ==> q"
    /// |> purednf
    /// </code>
    /// Evaluates to 
    /// <c>[[`p ==> q`]]</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val purednf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// <summary>
    /// Check if there are complementary literals of the form <c>p</c> and 
    /// <c>~ p</c> in a list of formulas.
    /// </summary>
    /// 
    /// <param name="lits">The input list of literals.</param>
    /// 
    /// <returns>
    /// true, if there are complementary literals in the input list; 
    /// otherwise false.
    /// </returns>
    /// 
    /// <example id="trivial-1">
    /// <code lang="fsharp">
    /// trivial [!>"p";!>"~p"]
    /// </code>
    /// Evaluates to 
    /// <c>true</c>.
    /// </example>
    /// 
    /// <example id="trivial-2">
    /// <code lang="fsharp">
    /// trivial [!>"p";!>"~q"]
    /// </code>
    /// Evaluates to 
    /// <c>false</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val trivial: lits: formula<'a> list -> bool when 'a: comparison

    /// <summary>
    /// Transforms any kind of formula in disjunctive normal form 
    /// returning a set of set representation.
    /// </summary>
    /// 
    /// <remarks>
    /// It exploits the list of list representation filtering out trivial 
    /// complementary literals and subsumed ones (with subsumption checking, 
    /// done very naively: quadratic).
    /// </remarks>
    /// 
    /// <param name="fm">The input formulas.</param>
    /// 
    /// <returns>
    /// A set of set representation of a dnf equivalent of the input.
    /// </returns>
    /// 
    /// <example id="simpdnf-1">
    /// <code lang="fsharp">
    /// !> @"p ==> q" |> simpdnf
    /// </code>
    /// Evaluates to 
    /// <c>[[`q`]; [`~p`]]</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val simpdnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// <summary>
    /// Transforms any kind of formula in disjunctive normal form.
    /// </summary>
    /// 
    /// <remarks>
    /// A formula is in disjunctive normal form (DNF) if it is an iterated 
    /// disjunction of conjunctions of litterals. 
    /// <p>
    /// </p>
    /// It is analogous to a fully expanded <em>sum of products</em> expression 
    /// like \(x^3 + x^2 y + xy + z\) in algebra.
    /// <p>
    /// </p>
    /// It is not canonical since for example 
    /// <c>p /\ ~q /\ r \/ p /\ q /\ ~r \/ p /\ q /\ r</c> and 
    /// <c>p /\ q \/ p /\ r</c> are both equivalent DNF.
    /// </remarks>
    /// 
    /// <param name="fm">The input formulas.</param>
    /// 
    /// <returns>
    /// A dnf equivalent of the input.
    /// </returns>
    /// 
    /// <example id="dnf-1">
    /// <code lang="fsharp">
    /// !> @"p ==> q" |> dnf
    /// </code>
    /// Evaluates to 
    /// <c>`q \/ ~p`</c>.
    /// </example>
    /// 
    /// <example id="dnf-2">
    /// <code lang="fsharp">
    /// let fm = !> @"(p \/ q /\ r) /\ (~p \/ ~r)"
    /// let dnf = dnf fm // `p /\ ~r \/ q /\ r /\ ~p`
    /// tautology(mk_iff fm dnf)
    /// </code>
    /// Evaluates to <c>true</c>.
    /// </example>
    /// 
    /// <category index="10">Disjunctive Normal Form</category>
    val dnf: fm: formula<'a> -> formula<'a> when 'a: comparison

    /// <summary>
    /// Transforms any kind of input formulas in conjunctive normal form with a 
    /// set of sets representation form as output, but keeps possible 
    /// superfluous and subsumed conjuncts.
    /// </summary>
    /// 
    /// <remarks>
    /// In the cnf context the outer set represents the iterated conjunction 
    /// and the subsets the disjunctions. Thus, <c>[[p; q]; [~ p; r]]</c> 
    /// represents <c>p \/ q /\ ~ p \/ r</c>
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// A cnf equivalent of the input formula in a set of sets representation 
    /// with possible superfluous and subsumed conjuncts.
    /// </returns>
    /// 
    /// <example id="purecnf-1">
    /// <code lang="fsharp">
    /// !> @"p ==> q" |> purecnf
    /// </code>
    /// Evaluates to 
    /// <c>[[`q`; `~p`]]</c>.
    /// </example>
    /// 
    /// <example id="purecnf-2">
    /// <code lang="fsharp">
    /// !> @"p \/ ~p" |> purecnf
    /// </code>
    /// Evaluates to <c>[[`p`; `~p`]]</c>.
    /// </example>
    /// 
    /// <category index="11">Conjunctive Normal Form</category>
    val purecnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// <summary>
    /// Transforms any kind of input formulas in conjunctive normal form with a 
    /// set of sets representation form as output.
    /// </summary>
    /// 
    /// <remarks>
    /// It exploits the list of list representation filtering out trivial 
    /// complementary literals and subsumed ones.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// A cnf equivalent of the input formula in a set of sets representation.
    /// </returns>
    /// 
    /// <example id="simpcnf-1">
    /// <code lang="fsharp">
    /// !> @"p ==> q" |> simpcnf
    /// </code>
    /// Evaluates to 
    /// <c>[[`q`; `~p`]]</c>.
    /// </example>
    /// 
    /// <example id="simpcnf-2">
    /// <code lang="fsharp">
    /// !> @"p \/ ~p" |> simpcnf
    /// </code>
    /// Evaluates to <c>[[`p`; `~p`]]</c>.
    /// </example>
    /// 
    /// <category index="11">Conjunctive Normal Form</category>
    val simpcnf:
      fm: formula<'a> -> formula<'a> list list when 'a: comparison

    /// <summary>
    /// Transforms any kind of input formulas in conjunctive normal.
    /// </summary>
    /// 
    /// <remarks>
    /// A formula is in conjunctive normal form (CNF) if it is an iterated 
    /// conjunction of disjunctions of litterals. 
    /// <p>
    /// </p>
    /// It is analogous to a fully factorized <em>product of sums</em> 
    /// expression like \((x + 1)(y + 2)(z + 3)\) in algebra.
    /// </remarks>
    /// 
    /// <param name="fm">The input formula.</param>
    /// 
    /// <returns>
    /// A cnf equivalent of the input formula.
    /// </returns>
    /// 
    /// <example id="cnf-1">
    /// <code lang="fsharp">
    /// let fm = !> @"(p \/ q /\ r) /\ (~p \/ ~r)"
    /// let cnf = cnf fm // `(p \/ q) /\ (p \/ r) /\ (~p \/ ~r)`
    /// tautology(mk_iff fm cnf)
    /// </code>
    /// Evaluates to 
    /// <c>true</c>.
    /// </example>
    /// 
    /// <category index="11">Conjunctive Normal Form</category>
    val cnf: fm: formula<'a> -> formula<'a> when 'a: comparison
