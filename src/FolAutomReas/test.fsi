namespace FSharp

module FolAutomReas.Lib.Num

/// Infinite-precision rational number.
type ratio = MathNet.Numerics.BigRational

/// arbitrary-precision rational numbers derived from OCaml equivalent
[<CustomEquality; CustomComparison>]
type Num =
    
    /// 32-bit signed integer.
    | Int of int
    
    /// Arbitrary-precision integer.
    | Big_int of bigint
    | Ratio of ratio
    interface System.IComparable<Num>
    interface System.IComparable
    interface System.IEquatable<Num>
    
    static member (%) : x: Num * y: Num -> Num
    
    static member ( * ) : x: Num * y: Num -> Num
    
    static member (+) : x: Num * y: Num -> Num
    
    static member (-) : x: Num * y: Num -> Num
    
    static member (/) : x: Num * y: Num -> Num
    
    static member (~-) : x: Num -> Num
    
    static member Abs: x: Num -> Num
    
    static member private AreEqual: x: Num * y: Num -> bool
    
    static member Ceiling: x: Num -> Num
    
    static member private Compare: x: Num * y: Num -> int
    
    static member Floor: x: Num -> Num
    
    static member private FromBigInt: value: bigint -> Num
    
    static member
      private FromBigRational: value: MathNet.Numerics.BigRational -> Num
    
    static member private FromInt64: value: int64 -> Num
    
    static member Max: x: Num * y: Num -> Num
    
    static member Min: x: Num * y: Num -> Num
    
    static member Parse: str: string -> Num
    
    static member Pow: x: Num * y: int -> Num
    
    static member Pow: x: Num * y: Num -> Num
    
    static member Quotient: x: Num * y: Num -> Num
    
    static member Round: x: Num -> Num
    
    static member Sign: x: Num -> int
    
    static member Truncate: x: Num -> Num
    
    override Equals: other: obj -> bool
    
    override GetHashCode: unit -> int
    
    override ToString: unit -> string
    
    member IsZero: bool
    
    static member One: Num
    
    static member Zero: Num

type num = Num

val inline num_of_int: r: int -> num

/// Convert a string to a number.
/// Raise Failure "num_of_string" if the given string is not a valid representation of an integer
val num_of_string: str: string -> num

val inline (=/) : x: num -> y: num -> bool

/// Computes greatest common divisor of two unlimited-precision integers.
/// 
/// The call gcd_num m n for two unlimited-precision (type num) integers m and n 
/// returns the (positive) greatest common divisor of m and n. If both m and n are zero, 
/// it returns zero.
/// 
/// Fails if either number is not an integer (the type num supports arbitrary rationals).
val gcd_num: n1: num -> n2: num -> num

/// Computes lowest common multiple of two unlimited-precision integers.
/// 
/// The call lcm_num m n for two unlimited-precision (type num) integers m and n 
/// returns the(positive) lowest common multiple of m and n. If either m or n (or both) 
/// are both zero, it returns zero.
/// 
/// Fails if either number is not an integer (the type num supports arbitrary rationals).
val lcm_num: n1: num -> n2: num -> Num


module FolAutomReas.Lib.PredAndFun

/// Revereses a predicate
/// 
/// (non f) equals (fun x -> not (f x)). 
/// 4 |> non (fun x -> x = 2) gives true.
/// 2 |> non (fun x -> x = 2) gives false.
val inline non: p: ('a -> bool) -> x: 'a -> bool

/// Checks that a value satisfies a predicate.
/// 
/// check p x returns x if the application p x yields true. Otherwise, check p x fails.
/// 
/// check p x fails with Failure "check" if the predicate p yields false when applied 
/// to the value x.
val check: p: ('a -> bool) -> x: 'a -> 'a

/// Iterates a function a fixed number of times.
/// 
/// funpow n f x applies f to x, n times, giving the result f (f ... (f x)...) where the
/// number of f’s is n. funpow 0 f x returns x. If n is negative, it is treated as zero.
/// 
/// funpow n f x fails if any of the n applications of f fail.
val funpow: n: int -> f: ('a -> 'a) -> x: 'a -> 'a

/// Tests for failure.
/// 
/// can f x evaluates to true if the application of f to x succeeds. It evaluates to false if
/// the application fails with a Failure _ exception.
/// 
/// Never fails on Failure _ exceptions.
val can: f: ('a -> 'b) -> x: 'a -> bool

/// Repeatedly apply a function until it fails.
/// 
/// The call repeat f x successively applies f over and over again starting with x, and stops
/// at the first point when a Failure _ exception occurs.
/// 
/// Never fails. If f fails at once it returns x.
val repeat: f: ('a -> 'a) -> x: 'a -> 'a


module FolAutomReas.Lib.ListFunctions

/// Gives a finite list of integers between the given bounds.
/// 
/// The call m--n returns the list of consecutive numbers from m to n.
val inline (--) : m: int -> n: int -> int list

/// Gives a finite list of nums between the given bounds.
/// 
/// The call m--n returns the list of consecutive numbers from m to n.
val inline (---) :
  m: FolAutomReas.Lib.Num.num ->
    n: FolAutomReas.Lib.Num.num -> FolAutomReas.Lib.Num.num list

/// Computes the last element of a list.
/// 
/// last [x1;...;xn] returns xn.
/// 
/// Fails with last if the list is empty.
val last: l: 'a list -> 'a

/// Computes the sub-list of a list consisting of all but the last element.
/// 
/// butlast [x1;...;xn] returns [x1;...;x(n-1)].
/// 
/// Fails if the list is empty.
val butlast: l: 'a list -> 'a list

/// Compute list of all results from applying function to pairs from two lists.
/// 
/// The call allpairs f [x1;...;xm] [y1;...;yn] returns the list of results 
/// [f x1 y1; f x1 y2; ... ; f x1 yn; fx2 y1 ; f x2 y2 ...; f xm y1; 
/// f xm y2 ...; f xm yn ]
/// 
/// Never fails.
val allpairs: f: ('a -> 'b -> 'c) -> l1: 'a list -> l2: 'b list -> 'c list

/// produces all pairs of distinct elements from a single list
/// 
/// distinctpairs [1;2;3;4] returns [(1, 2); (1, 3); (1, 4); (2, 3); (2, 4); (3, 4)]
val distinctpairs: l: 'a list -> ('a * 'a) list

/// Chops a list into two parts at a specified point.
/// 
/// chop_list i [x1;...;xn] returns ([x0;...;xi-1],[xi;...;xn]).
/// 
/// Fails with chop_list if n is negative or greater than the length of the list.
val chop_list: n: int -> l: 'a list -> 'a list * 'a list

/// Adds an element in a list at the specified index
/// 
/// insertat 2 999 [0;1;2;3] returns [0; 1; 999; 2; 3]
/// 
/// Fails if index is negative or exceeds the list length-1
val insertat: i: int -> x: 'a -> l: 'a list -> 'a list

/// Returns position of given element in list.
/// 
/// The call index x l where l is a list returns the position number of the 
/// first instance of x in the list, failing if there is none. The indices 
/// start at zero, corresponding to el.
val inline index: x: 'a -> xs: 'a list -> int when 'a: equality

/// Checks if x comes earlier than y in list l
/// 
/// earlier [0;1;2;3] 2 3 return true, earlier [0;1;2;3] 3 2 false.
/// earlier returns false also if x or y is not in the list.
val earlier: l: 'a list -> x: 'a -> y: 'a -> bool when 'a: comparison


module FolAutomReas.Lib.AssociationLists

/// Searches a list of pairs for a pair whose first component equals a 
/// specified value. 
/// 
/// assoc x [(x1,y1);...;(xn,yn)] returns the first yi in the list such that xi
/// equals x.
/// 
/// Fails if no matching pair is found. This will always be the case if the list is empty.
val assoc: a: 'a -> l: ('a * 'b) list -> 'b when 'a: comparison

/// Searches a list of pairs for a pair whose second component equals a 
/// specified value.
/// 
/// rev_assoc y [(x1,y1);...;(xn,yn)] returns the first xi in the list such 
/// that yi equals y.
/// 
/// Fails if no matching pair is found. This will always be the case if the 
/// list is empty.
val rev_assoc: a: 'a -> l: ('b * 'a) list -> 'b when 'a: comparison


module FolAutomReas.Lib.Sorting

/// Merges together two sorted lists with respect to a given ordering.
/// 
/// If two lists l1 and l2 are sorted with respect to the given ordering 
/// comparer, then merge ord l1 l2 will merge them into a sorted list of all 
/// the elements. The merge keeps any duplicates; it is not a set operation.
/// merge (<) [1;3;7] [2;4;5;6] returns [1;2;3;4;5;6;7]
/// 
/// Never fails, but if the lists are not appropriately sorted the results 
/// will not in general be correct.
val merge: comparer: ('a -> 'a -> bool) -> l1: 'a list -> l2: 'a list -> 'a list

/// <summary>
/// Sorts a list using a given transitive ‘ordering’ relation.
/// 
/// The call sort op list where op is a transitive relation on the elements of 
/// list, will topologically sort the list, i.e. will permute it such that if x 
/// op y but not y op x then x will occur to the left of y in the sorted list. 
/// In particular if op is a total order, the list will be sorted in the usual 
/// sense of the word
/// 
/// Never fails
/// </summary>
/// <remarks>
/// This function uses the Quicksort algorithm internally, which has good 
/// typical-case performance and will sort topologically. However, its 
/// worst-case performance is quadratic. By contrast mergesort gives a good 
/// worst-case performance but requires a total order. Note that any 
/// comparison-based topological sorting function must have quadratic behaviour 
/// in the worst case. For an n-element list, there are n(n-1)/2 pairs. For any 
/// topological sorting algorithm, we can make sure the first n(n-1)/2-1 pairs 
/// compared are unrelated in either direction, while still leaving the option 
/// of choosing for the last pair (a,b) either 
/// </remarks>
val sort: ord: ('a -> 'a -> bool) -> ('a list -> 'a list) when 'a: equality

/// increasing predicate to use with "sort"
/// 
/// increasing List.length [1] [1;2] returns true`
val increasing: f: ('a -> 'b) -> x: 'a -> y: 'a -> bool when 'b: comparison

/// decreasing predicate to use with "sort"
/// 
/// decreasing List.length [1;2] [1] returns true
val decreasing: f: ('a -> 'b) -> x: 'a -> y: 'a -> bool when 'b: comparison


module FolAutomReas.Lib.Repetitions

/// removes adjacent duplicated elements from a list
///
/// uniq [1;1;3;2;2] returns [1;3;2]
val uniq: l: 'a list -> 'a list when 'a: comparison

/// returns the number of repetitions in a list
/// 
/// repetitions [1;1;3;2;2] returns [(1,2);(3,1);(2,2)].
val repetitions: ('a list -> ('a * int) list) when 'a: comparison

/// Returns the result of the first successful application of a function `f` 
/// to the elements of a list `l`.
/// 
/// `tryfind f [x1;...;xn]` returns `f xi` for the first `xi` in the list for 
/// which application of `f` succeeds.
val tryfind: f: ('a -> 'b) -> l: 'a list -> 'b


module FolAutomReas.Lib.Search

/// Returns the result of the first successful application of a function to the 
/// elements of a list.
/// 
/// tryfind f [x1;...;xn] returns (f xi) for the first xi in the list for which 
/// application of f succeeds.
/// 
/// Fails with tryfind if the application of the function fails for all 
/// elements in the list. This will always be the case if the list is empty.
val tryfind: f: ('a -> 'b) -> l: 'a list -> 'b

/// Applies a function to every element of a list, returning a list of results 
/// for those elements for which application succeeds.
/// 
/// mapfilter last [[1;2;3];[4;5];[];[6;7;8];[]] returns [3;5;8]
/// 
/// Fails if an exception not of the form Failure _ is generated by any 
/// application to the elements.
val mapfilter: f: ('a -> 'b) -> l: 'a list -> 'b list

/// finds the element of a list l that maximizes or minimizes a function f 
/// based on the given ord.
/// 
/// Support function for use with maximize and minimize.
val optimize: ord: ('a -> 'a -> bool) -> f: ('b -> 'a) -> lst: 'b list -> 'b

/// finds the element of a list l that maximizes a function f
/// 
/// maximize ((*) -1) [-1;2;3] returns -1
val maximize: f: ('a -> 'b) -> l: 'a list -> 'a when 'b: comparison

/// finds the element of a list l that minimizes a function f
/// 
/// minimize ((*) -1) [-1;2;3] returns 3
val minimize: f: ('a -> 'b) -> l: 'a list -> 'a when 'b: comparison


module FolAutomReas.Lib.SetOperations

/// Removes repeated elements from a list. Makes a list into a ‘set’.
/// 
/// setify l removes repeated elements from l, leaving the last occurrence of 
/// each duplicate in the list.
/// 
/// <example>
/// <code>setify [1;2;3;1;4;3]</code> returns <code>[1;2;3;4]</code>
/// </example>
/// 
/// Never fails
val setify: ('a list -> 'a list) when 'a: comparison

/// Computes the union of two ‘sets’.
/// 
/// union l1 l2 returns a list consisting of the elements of l1 not already in 
/// l2 concatenated with l2. If l1 and l2 are initially free from duplicates, 
/// this gives a set-theoretic union operation. union [1;2;3] [1;5;4;3] returns 
/// [1;2;3;4;5]; union [1;1;1] [1;2;3;2] returns [1;2;3].
/// 
/// Never fails.
val union: ('a list -> 'a list -> 'a list) when 'a: comparison

/// Computes the intersection of two ‘sets’.
/// 
/// intersect l1 l2 returns a list consisting of those elements of l1 that also 
/// appear in l2. If both sets are free of repetitions, this can be considered 
/// a set-theoretic intersection operation. intersect [1;2;3] [3;5;4;1] returns 
/// [1;3]; intersect [1;2;4;1] [1;2;3;2] returns [1;2].
/// 
/// Never fails.
val intersect: ('a list -> 'a list -> 'a list) when 'a: comparison

/// Computes the set-theoretic difference of two ‘sets’
/// 
/// subtract l1 l2 returns a list consisting of those elements of l1 that do 
/// not appear in l2. If both lists are initially free of repetitions, this can 
/// be considered a set difference operation. subtract [1;2;3] [3;5;4;1] 
/// returns [2]; subtract [1;2;4;1] [4;5] returns [1;2]
/// 
/// Never fails.
val subtract: ('a list -> 'a list -> 'a list) when 'a: comparison

val subset: ('a list -> 'a list -> bool) when 'a: comparison

val psubset: ('b list -> 'b list -> bool) when 'b: comparison

/// Tests two ‘sets’ for equality.
/// 
/// set_eq l1 l2 returns true if every element of l1 appears in l2 and every 
/// element of l2 appears in l1. Otherwise it returns false. In other words, it 
/// tests if the lists are the same considered as sets, i.e. ignoring 
/// duplicates. set_eq [1;2] [2;1;2] returns true; set_eq [1;2] [1;3] returns 
/// false.
/// 
/// Never fails.
val set_eq: s1: 'a list -> s2: 'a list -> bool when 'a: comparison

/// inserts one new element into a set
/// 
/// insert 4 [2;3;3;5] returns [2;3;4;5] 
val insert: x: 'a -> s: 'a list -> 'a list when 'a: comparison

val image: f: ('a -> 'b) -> s: 'a list -> 'b list when 'b: comparison

val unions: s: 'a list list -> 'a list when 'a: comparison

val mem: x: 'a -> lis: 'a list -> bool when 'a: equality

val allsets: m: int -> l: 'a list -> 'a list list when 'a: comparison

val allsubsets: s: 'a list -> 'a list list when 'a: comparison

val allnonemptysubsets: s: 'a list -> 'a list list when 'a: comparison


module FolAutomReas.Lib.StringExplosion

val explode: s: string -> string list

val implode: l: string list -> string


module FolAutomReas.Lib.FPF

/// Type of functions represented as a patritia tree.
type func<'a,'b> =
    | Empty
    | Leaf of int * ('a * 'b) list
    | Branch of int * int * func<'a,'b> * func<'a,'b>




module FolAutomReas.Lib.UnionFindAlgorithm

type pnode<'a> =
    | Nonterminal of 'a
    | Terminal of 'a * int

type partition<'a> = | Partition of FolAutomReas.Lib.FPF.func<'a,pnode<'a>>

val string_of_partition: par: partition<'a> -> string

val sprint_partition: pt: partition<'a> -> string

val print_partition: pt: partition<'a> -> unit

val terminus: ptn: partition<'a> -> a: 'a -> 'a * int when 'a: comparison

val tryterminus: ptn: partition<'a> -> a: 'a -> 'a * int when 'a: comparison

val canonize: ptn: partition<'a> -> a: 'a -> 'a when 'a: comparison

val equivalent: eqv: partition<'a> -> a: 'a -> b: 'a -> bool when 'a: comparison

val equate:
  a: 'a * b: 'a -> ptn: partition<'a> -> partition<'a> when 'a: comparison

val unequal: partition<'a>

val equated: partition<'a> -> 'a list when 'a: comparison

val first:
  n: FolAutomReas.Lib.Num.Num ->
    p: (FolAutomReas.Lib.Num.Num -> bool) -> FolAutomReas.Lib.Num.Num

/// Write from a StringWriter to a string
val writeToString: fn: (System.IO.StringWriter -> unit) -> string


module FolAutomReas.Lib.Timing

/// Timing; useful for documentation but not logically necessary. 
val time: f: ('a -> 'b) -> x: 'a -> 'b


module FolAutomReas.Intro

/// Abstract syntax tree of algebraic expressions.
type expression =
    | Var of string
    | Const of int
    | Add of expression * expression
    | Mul of expression * expression

/// Simplify an algebraic expression at the first level.
val simplify1: expr: expression -> expression

/// Recursively simplifies any immediate subexpressions as much as possible, 
/// then applies `simplify1` to the result.
val simplify: expr: expression -> expression

/// Creates a pattern matching function based on the input `s` as the pattern.
val matches: s: string -> (string -> bool)

/// Classifies string characters as spaces.
val space: (string -> bool)

/// Classifies string characters as punctuation.
val punctuation: (string -> bool)

/// Classifies string characters as symbolic.
val symbolic: (string -> bool)

/// Classifies string characters as numeric.
val numeric: (string -> bool)

/// Classifies string characters as alphanumeric.
val alphanumeric: (string -> bool)

/// Takes a property `prop` of characters, such as one of the classifying 
/// predicates (`space`, `punctuation`, `symbolic`, `numeric`, `alphanumeric`), 
/// and a list of input characters `inp`, separating oﬀ as a string the longest 
/// initial sequence of that list of characters satisfying `prop`.
val lexwhile: prop: (string -> bool) -> inp: string list -> string * string list

/// Lexical analyser. It maps a list of input characters `inp` into a list of 
/// token strings.
val lex: inp: string list -> string list

/// Recursive descent parsing of expression. It takes a list of tokens `i` and 
/// returns a pair consisting of the parsed expression tree together with any 
/// unparsed input.
val parse_expression: i: string list -> expression * string list

/// Parses a product.
val parse_product: i: string list -> expression * string list

/// Parses an atom.
val parse_atom: i: string list -> expression * string list

/// A wrapper function that explodes the input string, lexically analyzes it, 
/// parses the sequence of tokens and then ﬁnally checks that no input remains 
/// unparsed.
val make_parser: pfn: (string list -> 'a * 'b list) -> s: string -> 'a

/// Parses a string into an expression.
val parse_exp: (string -> expression)

/// Reverses transformation, from abstract to concrete syntax keeping brackets. 
/// It puts brackets uniformly round each instance of a binary operator, 
/// which is perfectly correct but sometimes looks cumbersome to a human.
val string_of_exp_naive: e: expression -> string

/// Auxiliary function to print expressions without unnecessary brackets. 
val string_of_exp: pr: int -> e: expression -> string

/// Prints an expression `e` in concrete syntax removing unnecessary brackets. 
/// It omits the outermost brackets, and those that are implicit in rules for 
/// precedence or associativity.
val print_exp: e: expression -> unit

/// Returns a string of the concrete syntax of an expression `e` removing 
/// unnecessary brackets. It omits the outermost brackets, and those that are 
/// implicit in rules for precedence or associativity.
val sprint_exp: e: expression -> string


module FolAutomReas.Formulas

/// Abstract syntax tree of polymorphic type of formulas.
type formula<'a> =
    | False
    | True
    | Atom of 'a
    | Not of formula<'a>
    | And of formula<'a> * formula<'a>
    | Or of formula<'a> * formula<'a>
    | Imp of formula<'a> * formula<'a>
    | Iff of formula<'a> * formula<'a>
    | Forall of string * formula<'a>
    | Exists of string * formula<'a>

/// General parsing of iterated infixes. 
val parse_ginfix:
  opsym: 'a ->
    opupdate: (('b -> 'c) -> 'b -> 'b -> 'c) ->
    sof: ('b -> 'c) ->
    subparser: ('a list -> 'b * 'a list) -> inp: 'a list -> 'c * 'a list
    when 'a: equality

/// Parses left infix.
val parse_left_infix:
  opsym: 'a ->
    opcon: ('b * 'b -> 'b) ->
    (('a list -> 'b * 'a list) -> 'a list -> 'b * 'a list) when 'a: equality

/// Parses right infix.
val parse_right_infix:
  opsym: 'a ->
    opcon: ('b * 'b -> 'b) ->
    (('a list -> 'b * 'a list) -> 'a list -> 'b * 'a list) when 'a: equality

/// Parses a list: used to parse the list of arguments to a function.
val parse_list:
  opsym: 'a -> (('a list -> 'b * 'a list) -> 'a list -> 'b list * 'a list)
    when 'a: equality

/// Applies a function to the ﬁrst element of a pair, the idea being to modify 
/// the returned abstract syntax tree while leaving the ‘unparsed input’ alone.
val inline papply: f: ('a -> 'b) -> ast: 'a * rest: 'c -> 'b * 'c

/// Checks if the head of a list (typically the list of unparsed input) is some 
/// particular item, but also ﬁrst checks that the list is nonempty before 
/// looking at its head.
val nextin: inp: 'a list -> tok: 'a -> bool when 'a: equality

/// Deals with the common situation of syntactic items enclosed in brackets. It 
/// simply calls the subparser and then checks and eliminates the closing 
/// bracket. In principle, the terminating character can be anything, so this 
/// function could equally be used for other purposes, but we will always use 
/// ')' for the cbra (‘closing bracket’) argument.
val parse_bracketed:
  subparser: ('a -> 'b * 'c list) -> cbra: 'c -> inp: 'a -> 'b * 'c list
    when 'c: equality

/// Attempts to parse an atomic formula as a term followed by an inﬁx predicate 
/// symbol and only if that fails proceed to considering other kinds of 
/// formulas.
val parse_atomic_formula:
  ifn: (string list -> string list -> formula<'a> * string list) *
  afn: (string list -> string list -> formula<'a> * string list) ->
    vs: string list -> inp: string list -> formula<'a> * string list

/// Parses quantifiers.
val parse_quant:
  ifn: (string list -> string list -> formula<'a> * string list) *
  afn: (string list -> string list -> formula<'a> * string list) ->
    vs: string list ->
    qcon: (string * formula<'a> -> formula<'a>) ->
    x: string -> inp: string list -> formula<'a> * string list

/// Recursive descent parser of polymorphic formulas built up from an 
/// atomic formula parser by cascading instances of parse infix in order of 
/// precedence, following the conventions with '/\' coming highest and '<=>' 
/// lowest.
/// 
/// It takes a list of string tokens `inp` and In order to check whether a name 
/// is within the scope of a quantiﬁer, it takes an additional argument `vs` 
/// which is the set of bound variables in the current scope.
/// 
/// It returns a pair consisting of the parsed formula tree together with 
/// any unparsed input. 
val parse_formula:
  ifn: (string list -> string list -> formula<'a> * string list) *
  afn: (string list -> string list -> formula<'a> * string list) ->
    vs: string list -> inp: string list -> formula<'a> * string list

/// Modiﬁes a basic printer to have a kind of boxing 'wrapped' around it.
val fbracket:
  tw: System.IO.TextWriter ->
    p: bool -> n: 'a -> f: ('b -> 'c -> unit) -> x: 'b -> y: 'c -> unit

/// Breaks up a quantiﬁed term into its quantiﬁed variables and body.
val strip_quant: fm: formula<'a> -> string list * formula<'a>

/// Printing parametrized by a function `pfn` to print atoms.
val fprint_formula:
  tw: System.IO.TextWriter -> pfn: (int -> 'a -> unit) -> (formula<'a> -> unit)

/// Main toplevel printer. It just adds the guillemot-style quotations round 
/// the formula so that it looks like the quoted formulas we parse.
val fprint_qformula:
  tw: System.IO.TextWriter ->
    pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

/// Prints a formula `fm` using a function `pfn` to print atoms.
val inline print_formula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

/// Returns a string representation of a formula `fm` using a function `pfn` to 
/// print atoms.
val inline sprint_formula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> string

/// Prints a formula `fm` using a function `pfn` to print atoms.
val inline print_qformula: pfn: (int -> 'a -> unit) -> fm: formula<'a> -> unit

/// Returns a string representation of a formula `fm` using a function `pfn` to 
/// print atoms.
val inline sprint_qformula:
  pfn: (int -> 'a -> unit) -> fm: formula<'a> -> string

/// Constructs a conjunction.
val inline mk_and: p: formula<'a> -> q: formula<'a> -> formula<'a>

/// Constructs a disjunction.
val inline mk_or: p: formula<'a> -> q: formula<'a> -> formula<'a>

/// Constructs an implication.
val inline mk_imp: p: formula<'a> -> q: formula<'a> -> formula<'a>

/// Constructs a logical equivalence.
val inline mk_iff: p: formula<'a> -> q: formula<'a> -> formula<'a>

/// Constructs a universal quantification.
val inline mk_forall: x: string -> p: formula<'a> -> formula<'a>

/// Constructs an existential quantification.
val inline mk_exists: x: string -> p: formula<'a> -> formula<'a>

/// Formula destructor for logical equivalence.
val dest_iff: _arg1: formula<'a> -> formula<'a> * formula<'a>

/// Formula destructor for conjunction into the two conjuncts.
val dest_and: _arg1: formula<'a> -> formula<'a> * formula<'a>

/// Iteratively breaks apart a conjunction.
val conjuncts: _arg1: formula<'a> -> formula<'a> list

/// Breaks apart a disjunction into the two disjuncts.
val dest_or: _arg1: formula<'a> -> formula<'a> * formula<'a>

/// Iteratively breaks apart a disjunction.
val disjuncts: _arg1: formula<'a> -> formula<'a> list

/// Breaks apart an implication into antecedent and consequent.
val dest_imp: _arg1: formula<'a> -> formula<'a> * formula<'a>

/// Returns the antecedent of an implication.
val inline antecedent: fm: formula<'a> -> formula<'a>

/// Returns the consequent of an implication.
val inline consequent: fm: formula<'a> -> formula<'a>

/// Applies a function to all the atoms in a formula, but otherwise leaves 
/// the structure unchanged. It can be used, for example, to perform systematic 
/// replacement of one particular atomic proposition by another formula.
val onatoms: f: ('a -> formula<'a>) -> fm: formula<'a> -> formula<'a>

/// Formula analog of list iterator `List.foldBack`. It iterates a binary 
/// function `f` over all the atoms of a formula (as the first argument) using 
/// the input `b` as the second argument.
val overatoms: f: ('a -> 'b -> 'b) -> fm: formula<'a> -> b: 'b -> 'b

/// Collects together some set of attributes associated with the atoms; in the 
/// simplest case just returning the set of all atoms. It does this by 
/// iterating a function `f` together with an ‘append’ over all the atoms, and 
/// ﬁnally converting the result to a set to remove duplicates.
val atom_union:
  f: ('a -> 'b list) -> fm: formula<'a> -> 'b list when 'b: comparison


module FolAutomReas.Prop

/// Type of primitive propositions indexed by names.
type prop = | P of string

/// Returns constant or variable name of a propositional formula.
val inline pname: prop -> string

/// Parses atomic propositions.
val parse_propvar:
  vs: 'a -> inp: string list -> Formulas.formula<prop> * string list

/// Parses a string in a propositional formula.
val parse_prop_formula: (string -> Formulas.formula<prop>)

/// Prints a prop variable using a TextWriter.
val fprint_propvar: sw: System.IO.TextWriter -> prec: 'a -> p: prop -> unit

/// Prints a prop variable
val inline print_propvar: prec: 'a -> p: prop -> unit

/// Returns a string representation of a prop variable.
val inline sprint_propvar: prec: 'a -> p: prop -> string

/// Prints a prop formula using a TextWriter.
val fprint_prop_formula:
  sw: System.IO.TextWriter -> (Formulas.formula<prop> -> unit)

/// Prints a prop formula
val inline print_prop_formula: f: Formulas.formula<prop> -> unit

/// Returns a string representation of a propositional formula instead of 
/// its abstract syntax tree..
val inline sprint_prop_formula: f: Formulas.formula<prop> -> string

/// Interpretation of formulas. 
val eval: fm: Formulas.formula<'a> -> v: ('a -> bool) -> bool

/// Return the set of propositional variables in a formula.
val atoms: fm: Formulas.formula<'a> -> 'a list when 'a: comparison

/// Tests whether a function `subfn` returns `true` on all possible valuations 
/// of the atoms `ats`, using an existing valuation `v` for all other atoms.
val onallvaluations:
  subfn: (('a -> bool) -> bool) -> v: ('a -> bool) -> ats: 'a list -> bool
    when 'a: equality

/// Prints the truth table of a formula `fm` using a TextWriter.
val fprint_truthtable:
  sw: System.IO.TextWriter -> fm: Formulas.formula<prop> -> unit

/// Prints the truth table of the propositional formula `f`.
val inline print_truthtable: f: Formulas.formula<prop> -> unit

/// Returns a string representation of the truth table of the propositional 
/// formula `f`.
val inline sprint_truthtable: f: Formulas.formula<prop> -> string

/// Checks if a propositional formula is a tautology.
val tautology: fm: Formulas.formula<'a> -> bool when 'a: comparison

/// Checks if a propositional formula is unsatisfiable.
val unsatisfiable: fm: Formulas.formula<'a> -> bool when 'a: comparison

/// Checks if a propositional formula is satisfiable.
val satisfiable: fm: Formulas.formula<'a> -> bool when 'a: comparison

/// Returns the formula resulting from applying the substitution `sbfn` 
/// to the input formula.
val psubst:
  subfn: Lib.FPF.func<'a,Formulas.formula<'a>> ->
    (Formulas.formula<'a> -> Formulas.formula<'a>) when 'a: comparison

/// Returns the dual of the input formula `fm`: i.e. the result of 
/// systematically exchanging `/\`s with `\/`s and also `True` with 
/// `False`.
val dual: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Performs a simplification routine but just at the first level of the input 
/// formula `fm`. It eliminates the basic propositional constants `False` and 
/// `True`. 
/// 
/// Whenever `False` and `True` occur in combination, there is always a a 
/// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
/// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
/// also eliminates double negations `~~p`.
val psimplify1: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Performs a simplification routine on the input formula 
/// `fm` eliminating the basic propositional constants `False` and `True`. 
/// 
/// Whenever `False` and `True` occur in combination, there is always a a 
/// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
/// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
/// also eliminates double negations `~~p`.
/// 
/// While `psimplify1` performs the transformation just at the first level, 
/// `psimplify` performs it at every levels in a recursive bottom-up sweep.
val psimplify: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Checks if a literal is negative.
val negative: _arg1: Formulas.formula<'a> -> bool

/// Checks if a literal is positive.
val positive: lit: Formulas.formula<'a> -> bool

/// Changes a literal into its contrary.
val negate: _arg1: Formulas.formula<'a> -> Formulas.formula<'a>

/// Changes a formula into its negation normal form without simplifying it.
val nnf_naive: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Changes a formula into its negation normal and applies it the routine 
/// simplification `psimplify`.
val nnf: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Simply pushes negations in the input formula `fm` down to the level of atoms without simplifying it.
val nenf_naive: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Simply pushes negations in the input formula `fm` down to the level of 
/// atoms and applies it the routine simplification `psimplify`.
val nenf: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Creates a conjunction of all the formulas in the input list `l`.
val list_conj:
  l: Formulas.formula<'a> list -> Formulas.formula<'a> when 'a: equality

/// Creates a disjunction of all the formulas in the input list `l`.
val list_disj:
  l: Formulas.formula<'a> list -> Formulas.formula<'a> when 'a: equality

/// Given a list of formulas `pvs`, makes a conjunction of these formulas and 
/// their negations according to whether each is satisﬁed by the valuation `v`.
val mk_lits:
  pvs: Formulas.formula<'a> list -> v: ('a -> bool) -> Formulas.formula<'a>
    when 'a: equality

/// A close analogue of `onallvaluations` that collects the valuations for 
/// which `subfn` holds into a list.
val allsatvaluations:
  subfn: (('a -> bool) -> bool) ->
    v: ('a -> bool) -> pvs: 'a list -> ('a -> bool) list when 'a: equality

/// Transforms a formula `fm` in disjunctive normal form using truth tables.
val dnf_by_truth_tables:
  fm: Formulas.formula<'a> -> Formulas.formula<'a> when 'a: comparison

/// Applies the distributive laws to the input formula `fm`.
val distrib_naive: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Transforms the input formula `fm` in disjunctive normal form.
val rawdnf: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Applies the distributive laws of propositional connectives `/\` and `\/` 
/// using a list representation of the formulas `s1` and `s2` on which 
/// to operate.
val distrib:
  s1: 'a list list -> s2: 'a list list -> 'a list list when 'a: comparison

/// Transforms the input formula `fm` in disjunctive normal form using 
/// (internally) a list representation of the formula as a set of sets. 
/// `p /\ q \/ ~ p /\ r` as `[[p; q]; [~ p; r]]`
val purednf:
  fm: Formulas.formula<'a> -> Formulas.formula<'a> list list when 'a: comparison

/// Check if there are complementary literals of the form p and ~ p 
/// in the same list.
val trivial: lits: Formulas.formula<'a> list -> bool when 'a: comparison

/// Transforms the input formula `fm` in a list of list representation of  
/// disjunctive normal form. It exploits the list of list representation 
/// filtering out trivial complementary literals and subsumed ones.
val simpdnf:
  fm: Formulas.formula<'a> -> Formulas.formula<'a> list list when 'a: comparison

/// Transforms the input formula `fm` in disjunctive normal form.
val dnf: fm: Formulas.formula<'a> -> Formulas.formula<'a> when 'a: comparison

/// Transforms the input formula `fm` in conjunctive normal form 
/// by using `purednf`.
val purecnf:
  fm: Formulas.formula<'a> -> Formulas.formula<'a> list list when 'a: comparison

/// Transforms the input formula `fm` in a list of list representation of 
/// conjunctive normal form. It exploits the list of list representation 
/// filtering out trivial complementary literals and subsumed ones.
val simpcnf:
  fm: Formulas.formula<'a> -> Formulas.formula<'a> list list when 'a: comparison

/// Transforms the input formula `fm` in conjunctive normal form.
val cnf: fm: Formulas.formula<'a> -> Formulas.formula<'a> when 'a: comparison


module FolAutomReas.Propexamples

/// Generate assertion equivalent to R(s,t) <= n for the Ramsey number R(s,t) 
val ramsey: s: int -> t: int -> n: int -> Formulas.formula<Prop.prop>

/// Generates the propositional formula whose truth value correponds to the 
/// carry of an half adder, given the `x` and `y` digits to be summed also 
/// represented as prop formulas: false for 0 true for 1.
/// 
/// x /\ y
val halfsum:
  x: Formulas.formula<'a> -> y: Formulas.formula<'a> -> Formulas.formula<'a>

/// Generates the propositional formulas whose truth value correponds to the 
/// sum of an half adder, given the `x` and `y` digits to be summed also 
/// represented as prop formulas: false for 0 true for 1. 
/// 
/// x <=> ~ y.
val halfcarry:
  x: Formulas.formula<'a> -> y: Formulas.formula<'a> -> Formulas.formula<'a>

/// Half adder function.
/// 
/// Generates a propositional formula that is true if the input formulas 
/// represent respectively two digits `x` and `y` to be summed, the resulting 
/// sum `s` and the carry `c`.
val ha:
  x: Formulas.formula<'a> ->
    y: Formulas.formula<'a> ->
    s: Formulas.formula<'a> -> c: Formulas.formula<'a> -> Formulas.formula<'a>

/// Generates the propositional formula whose truth value correponds to the 
/// carry of a full adder, given the `x`, `y` and `z` digits to be summed also 
/// represented as formulas: false for 0 true for 1. 
/// 
/// (x /\ y) \/ ((x \/ y) /\ z)
val carry:
  x: Formulas.formula<'a> ->
    y: Formulas.formula<'a> -> z: Formulas.formula<'a> -> Formulas.formula<'a>

/// Generates the propositional formula whose truth value correponds to the sum 
/// of a full adder, given the `x`, `y` and `z` digits to be summed also 
/// represented as formulas: false for 0 true for 1. 
/// 
/// (x <=> ~ y) <=> ~ z
val sum:
  x: Formulas.formula<'a> ->
    y: Formulas.formula<'a> -> z: Formulas.formula<'a> -> Formulas.formula<'a>

/// Full adder function.
/// 
/// Generates a propositional formula that is true if the input terms represent 
/// respectively two digits `x` and `y` to be summed, the `z` carry from a 
/// previous sum, the resulting sum `s` and the carry `c`.
val fa:
  x: Formulas.formula<'a> ->
    y: Formulas.formula<'a> ->
    z: Formulas.formula<'a> ->
    s: Formulas.formula<'a> -> c: Formulas.formula<'a> -> Formulas.formula<'a>

/// An auxiliary function to define ripplecarry.
/// 
/// Given a function that creates a prop formula from an index and a list of 
/// indexes, it puts multiple full-adders together into an n-bit adder.
val conjoin:
  f: ('a -> Formulas.formula<'b>) -> l: 'a list -> Formulas.formula<'b>
    when 'b: equality

/// n-bit ripple carry adder with carry c(0) propagated in and c(n) out.  
/// 
/// Generates a propsitional formula that represent a riple-carry adder circuit.
/// Filtering the true rows of its truth table gives the sum and carry values 
/// for each digits.
/// 
/// It expects the user to supply functions `x`, `y`, `out` and `c` that, when 
/// given an index, generates an appropriate new variable. Use `mk_index` 
/// to generate such functions.
/// 
/// For example, 
/// 
/// `let [x; y; out; c] = map mk_index ["X"; "Y"; "OUT"; "C"]`
/// 
/// `ripplecarry x y c out 2`
val ripplecarry:
  x: (int -> Formulas.formula<'a>) ->
    y: (int -> Formulas.formula<'a>) ->
    c: (int -> Formulas.formula<'a>) ->
    out: (int -> Formulas.formula<'a>) -> n: int -> Formulas.formula<'a>
    when 'a: equality

/// An auxiliary function to generate input for ripplecarry.
/// 
/// Given a prpo formula `x` and an index `i`, it generates a propositional 
/// variable `P "x_i"`.
/// 
/// `let [x; y; out; c] = map mk_index ["X"; "Y"; "OUT"; "C"]` 
/// generates the x, y, out and c functions that can be given 
/// as input to ripplecarry
val mk_index: x: string -> i: int -> Formulas.formula<Prop.prop>

/// Similar to `mk_index`. 
/// 
/// Given a prop formula `x` and an indexes `i` and `j`, it generates a 
/// propositional variable `P "x_i_j"`.
val mk_index2: x: string -> i: 'a -> j: 'b -> Formulas.formula<Prop.prop>

/// n-bit ripple carry adder with carry c(0) forced to 0.
/// 
/// It can be used when we are not interested in a carry in at the low end.
val ripplecarry0:
  x: (int -> Formulas.formula<'a>) ->
    y: (int -> Formulas.formula<'a>) ->
    c: (int -> Formulas.formula<'a>) ->
    out: (int -> Formulas.formula<'a>) -> n: int -> Formulas.formula<'a>
    when 'a: equality

/// n-bit ripple carry adder with carry c(0) forced at 1.
/// 
/// It is used to define the carry-select adder. In a carry-select adder the 
/// n-bit inputs are split into several blocks of k, and corresponding k-bit 
/// blocks are added twice, once assuming a carry-in of 0 and once assuming a 
/// carry-in of 1.
val ripplecarry1:
  x: (int -> Formulas.formula<'a>) ->
    y: (int -> Formulas.formula<'a>) ->
    c: (int -> Formulas.formula<'a>) ->
    out: (int -> Formulas.formula<'a>) -> n: int -> Formulas.formula<'a>
    when 'a: equality

/// Multiplexer used to define the carry-select adder. We will use it to 
/// select between the two alternatives (carry-in of 0 or 1) when we do 
/// carry propagation.
val mux:
  sel: Formulas.formula<'a> ->
    in0: Formulas.formula<'a> ->
    in1: Formulas.formula<'a> -> Formulas.formula<'a>

/// An auxiliary function to oﬀset the indices in an array of bits. 
/// It is used to define the carry-select adder.
val offset: n: int -> x: (int -> 'a) -> i: int -> 'a

val carryselect:
  x: (int -> Formulas.formula<'a>) ->
    y: (int -> Formulas.formula<'a>) ->
    c0: (int -> Formulas.formula<'a>) ->
    c1: (int -> Formulas.formula<'a>) ->
    s0: (int -> Formulas.formula<'a>) ->
    s1: (int -> Formulas.formula<'a>) ->
    c: (int -> Formulas.formula<'a>) ->
    s: (int -> Formulas.formula<'a>) -> n: int -> k: int -> Formulas.formula<'a>
    when 'a: equality

/// Generates propositions that state the equivalence of various ripplecarry 
/// and carryselect circuits based on the input `n` (number of bit to be added) 
/// and `k` (number of blocks in the carryselect circuit).
/// 
/// If the proposition generated is a tautology, the equivalence between the 
/// two circuit is proved.
val mk_adder_test: n: int -> k: int -> Formulas.formula<Prop.prop>

/// Ripple carry stage that separates off the final result of a multiplication.
val rippleshift:
  u: (int -> Formulas.formula<'a>) ->
    v: (int -> Formulas.formula<'a>) ->
    c: (int -> Formulas.formula<'a>) ->
    z: Formulas.formula<'a> ->
    w: (int -> Formulas.formula<'a>) -> n: int -> Formulas.formula<'a>
    when 'a: equality

/// Naive multiplier based on repeated ripple carry. 
val multiplier:
  x: (int -> int -> Formulas.formula<'a>) ->
    u: (int -> int -> Formulas.formula<'a>) ->
    v: (int -> int -> Formulas.formula<'a>) ->
    out: (int -> Formulas.formula<'a>) -> n: int -> Formulas.formula<'a>
    when 'a: equality

/// Returns the nuber of bit needed to represent x in binary notation.
val bitlength: x: int -> int

/// Extract the `n`th bit (as a boolean value) of a nonnegative integer `x`.
val bit: n: int -> x: int -> bool

/// Produces a propositional formula asserting that the atoms `x`(i) encode 
/// the bits of a value `m`, at least modulo 2^`n`.
val congruent_to:
  x: (int -> Formulas.formula<'a>) -> m: int -> n: int -> Formulas.formula<'a>
    when 'a: equality

/// Applied to a positive integer `p` generates a propositional formula 
/// that is a tautology precisely if `p` is prime.
val prime: p: int -> Formulas.formula<Prop.prop>


module FolAutomReas.Defcnf

val mkprop: n: Lib.Num.num -> Formulas.formula<Prop.prop> * Lib.Num.Num

val maincnf:
  Formulas.formula<Prop.prop> *
  Lib.FPF.func<Formulas.formula<Prop.prop>,
               (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
  Lib.Num.num ->
    Formulas.formula<Prop.prop> *
    Lib.FPF.func<Formulas.formula<Prop.prop>,
                 (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
    Lib.Num.num

val defstep:
  op: (Formulas.formula<Prop.prop> ->
         Formulas.formula<Prop.prop> -> Formulas.formula<Prop.prop>) ->
    p: Formulas.formula<Prop.prop> * q: Formulas.formula<Prop.prop> ->
      fm: Formulas.formula<Prop.prop> *
      defs: Lib.FPF.func<Formulas.formula<Prop.prop>,
                         (Formulas.formula<Prop.prop> *
                          Formulas.formula<Prop.prop>)> * n: Lib.Num.num ->
        Formulas.formula<Prop.prop> *
        Lib.FPF.func<Formulas.formula<Prop.prop>,
                     (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
        Lib.Num.num

val max_varindex: pfx: string -> s: string -> n: Lib.Num.num -> Lib.Num.num

val mk_defcnf:
  fn: (Formulas.formula<Prop.prop> * Lib.FPF.func<'a,'b> * Lib.Num.Num ->
         Formulas.formula<'c> * Lib.FPF.func<'d,('e * Formulas.formula<'c>)> *
         'f) ->
    fm: Formulas.formula<Prop.prop> -> Formulas.formula<'c> list list
    when 'c: comparison and 'd: comparison and 'e: comparison

val defcnfOrig: fm: Formulas.formula<Prop.prop> -> Formulas.formula<Prop.prop>

val subcnf:
  sfn: ('a * 'b * 'c -> 'd * 'b * 'c) ->
    op: ('d -> 'd -> 'e) ->
    p: 'a * q: 'a -> fm: 'f * defs: 'b * n: 'c -> 'e * 'b * 'c

val orcnf:
  Formulas.formula<Prop.prop> *
  Lib.FPF.func<Formulas.formula<Prop.prop>,
               (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
  Lib.Num.num ->
    Formulas.formula<Prop.prop> *
    Lib.FPF.func<Formulas.formula<Prop.prop>,
                 (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
    Lib.Num.num

val andcnf:
  Formulas.formula<Prop.prop> *
  Lib.FPF.func<Formulas.formula<Prop.prop>,
               (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
  Lib.Num.num ->
    Formulas.formula<Prop.prop> *
    Lib.FPF.func<Formulas.formula<Prop.prop>,
                 (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
    Lib.Num.num

val defcnfs:
  fm: Formulas.formula<Prop.prop> -> Formulas.formula<Prop.prop> list list

val defcnf: fm: Formulas.formula<Prop.prop> -> Formulas.formula<Prop.prop>

val andcnf3:
  Formulas.formula<Prop.prop> *
  Lib.FPF.func<Formulas.formula<Prop.prop>,
               (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
  Lib.Num.num ->
    Formulas.formula<Prop.prop> *
    Lib.FPF.func<Formulas.formula<Prop.prop>,
                 (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>)> *
    Lib.Num.num

val defcnf3: fm: Formulas.formula<Prop.prop> -> Formulas.formula<Prop.prop>


module FolAutomReas.Dp

val containOneLitterals: clauses: 'a list list -> bool

val one_literal_rule:
  clauses: Formulas.formula<'a> list list -> Formulas.formula<'a> list list
    when 'a: comparison

val containPureLitterals:
  clauses: Formulas.formula<'a> list list -> bool when 'a: comparison

val affirmative_negative_rule:
  clauses: Formulas.formula<'a> list list -> Formulas.formula<'a> list list
    when 'a: comparison

val resolve_on:
  p: Formulas.formula<'a> ->
    clauses: Formulas.formula<'a> list list -> Formulas.formula<'a> list list
    when 'a: comparison

val resolution_blowup:
  cls: Formulas.formula<'a> list list -> l: Formulas.formula<'a> -> int
    when 'a: equality

val resolution_rule:
  clauses: Formulas.formula<'a> list list -> Formulas.formula<'a> list list
    when 'a: comparison

val dp: clauses: Formulas.formula<'a> list list -> bool when 'a: comparison

val dpsat: fm: Formulas.formula<Prop.prop> -> bool

val dptaut: fm: Formulas.formula<Prop.prop> -> bool

val posneg_count:
  cls: Formulas.formula<'a> list list -> l: Formulas.formula<'a> -> int
    when 'a: equality

val dpll: clauses: Formulas.formula<'a> list list -> bool when 'a: comparison

val dpllsat: fm: Formulas.formula<Prop.prop> -> bool

val dplltaut: fm: Formulas.formula<Prop.prop> -> bool

type trailmix =
    | Guessed
    | Deduced

val unassigned:
  (Formulas.formula<'a> list list ->
     (Formulas.formula<'a> * 'b) list -> Formulas.formula<'a> list)
    when 'a: comparison

val unit_subpropagate:
  cls: Formulas.formula<'a> list list *
  fn: Lib.FPF.func<Formulas.formula<'a>,unit> *
  trail: (Formulas.formula<'a> * trailmix) list ->
    Formulas.formula<'a> list list * Lib.FPF.func<Formulas.formula<'a>,unit> *
    (Formulas.formula<'a> * trailmix) list when 'a: comparison

val unit_propagate:
  cls: Formulas.formula<'a> list list *
  trail: (Formulas.formula<'a> * trailmix) list ->
    Formulas.formula<'a> list list * (Formulas.formula<'a> * trailmix) list
    when 'a: comparison

val backtrack: trail: ('a * trailmix) list -> ('a * trailmix) list

val dpli:
  cls: Formulas.formula<'a> list list ->
    trail: (Formulas.formula<'a> * trailmix) list -> bool when 'a: comparison

val dplisat: fm: Formulas.formula<Prop.prop> -> bool

val dplitaut: fm: Formulas.formula<Prop.prop> -> bool

val backjump:
  cls: Formulas.formula<'a> list list ->
    p: Formulas.formula<'a> ->
    trail: (Formulas.formula<'a> * trailmix) list ->
    (Formulas.formula<'a> * trailmix) list when 'a: comparison

val dplb:
  cls: Formulas.formula<'a> list list ->
    trail: (Formulas.formula<'a> * trailmix) list -> bool when 'a: comparison

val dplbsat: fm: Formulas.formula<Prop.prop> -> bool

val dplbtaut: fm: Formulas.formula<Prop.prop> -> bool


module FolAutomReas.Stal

val triplicate:
  fm: Formulas.formula<Prop.prop> ->
    Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop> list

val atom: lit: Formulas.formula<'a> -> Formulas.formula<'a>

val align:
  p: Formulas.formula<'a> * q: Formulas.formula<'a> ->
    Formulas.formula<'a> * Formulas.formula<'a> when 'a: comparison

val equate2:
  p: Formulas.formula<'a> * q: Formulas.formula<'a> ->
    eqv: Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> when 'a: comparison

val irredundant:
  rel: Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> ->
    eqs: (Formulas.formula<'a> * Formulas.formula<'a>) list ->
    (Formulas.formula<'a> * Formulas.formula<'a>) list when 'a: comparison

val consequences:
  Formulas.formula<'a> * Formulas.formula<'a> ->
    fm: Formulas.formula<'a> ->
    eqs: (Formulas.formula<'a> * Formulas.formula<'a>) list ->
    (Formulas.formula<'a> * Formulas.formula<'a>) list when 'a: comparison

val triggers:
  fm: Formulas.formula<'a> ->
    ((Formulas.formula<'a> * Formulas.formula<'a>) *
     (Formulas.formula<'a> * Formulas.formula<'a>) list) list
    when 'a: comparison

val trigger:
  (Formulas.formula<Prop.prop> ->
     ((Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>) *
      (Formulas.formula<Prop.prop> * Formulas.formula<Prop.prop>) list) list)

val relevance:
  trigs: (('a * 'a) * 'b) list -> Lib.FPF.func<'a,(('a * 'a) * 'b) list>
    when 'a: comparison and 'b: comparison

val equatecons:
  p0: Formulas.formula<'a> * q0: Formulas.formula<'a> ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
    Lib.FPF.func<Formulas.formula<'a>,('b * 'c list) list> ->
      'c list *
      (Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
       Lib.FPF.func<Formulas.formula<'a>,('b * 'c list) list>)
    when 'a: comparison and 'b: comparison and 'c: comparison

val zero_saturate:
  Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
  Lib.FPF.func<Formulas.formula<'a>,
               ('b * (Formulas.formula<'a> * Formulas.formula<'a>) list) list> ->
    assigs: (Formulas.formula<'a> * Formulas.formula<'a>) list ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
    Lib.FPF.func<Formulas.formula<'a>,
                 ('b * (Formulas.formula<'a> * Formulas.formula<'a>) list) list>
    when 'a: comparison and 'b: comparison

val zero_saturate_and_check:
  Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
  Lib.FPF.func<Formulas.formula<'a>,
               ('b * (Formulas.formula<'a> * Formulas.formula<'a>) list) list> ->
    trigs: (Formulas.formula<'a> * Formulas.formula<'a>) list ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
    Lib.FPF.func<Formulas.formula<'a>,
                 ('b * (Formulas.formula<'a> * Formulas.formula<'a>) list) list>
    when 'a: comparison and 'b: comparison

val truefalse:
  pfn: Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> -> bool
    when 'a: comparison

val equateset:
  s0: Formulas.formula<'a> list ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
    Lib.FPF.func<Formulas.formula<'a>,('b * 'c list) list> ->
      Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
      Lib.FPF.func<Formulas.formula<'a>,('b * 'c list) list>
    when 'a: comparison and 'b: comparison and 'c: comparison

val inter:
  els: Formulas.formula<'a> list ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> * 'b ->
      Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> * 'c ->
        rev1: Lib.FPF.func<Formulas.formula<'a>,Formulas.formula<'a> list> ->
        rev2: Lib.FPF.func<Formulas.formula<'a>,Formulas.formula<'a> list> ->
        Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
        Lib.FPF.func<Formulas.formula<'a>,('d * 'e list) list> ->
          Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
          Lib.FPF.func<Formulas.formula<'a>,('d * 'e list) list>
    when 'a: comparison and 'd: comparison and 'e: comparison

val reverseq:
  domain: 'a list ->
    eqv: Lib.UnionFindAlgorithm.partition<'a> -> Lib.FPF.func<'a,'a list>
    when 'a: comparison

val stal_intersect:
  Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
  Lib.FPF.func<Formulas.formula<'a>,('b * 'c list) list> ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
    Lib.FPF.func<Formulas.formula<'a>,('b * 'c list) list> ->
      Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
      Lib.FPF.func<Formulas.formula<'a>,('b * 'c list) list> ->
        Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
        Lib.FPF.func<Formulas.formula<'a>,('b * 'c list) list>
    when 'a: comparison and 'b: comparison and 'c: comparison

val saturate:
  n: int ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
    Lib.FPF.func<Formulas.formula<'a>,
                 ('b * (Formulas.formula<'a> * Formulas.formula<'a>) list) list> ->
      assigs: (Formulas.formula<'a> * Formulas.formula<'a>) list ->
      allvars: Formulas.formula<'a> list ->
      Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
      Lib.FPF.func<Formulas.formula<'a>,
                   ('b * (Formulas.formula<'a> * Formulas.formula<'a>) list) list>
    when 'a: comparison and 'b: comparison

val splits:
  n: int ->
    Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
    Lib.FPF.func<Formulas.formula<'a>,
                 ('b * (Formulas.formula<'a> * Formulas.formula<'a>) list) list> ->
      allvars: Formulas.formula<'a> list ->
      vars: Formulas.formula<'a> list ->
      Lib.UnionFindAlgorithm.partition<Formulas.formula<'a>> *
      Lib.FPF.func<Formulas.formula<'a>,
                   ('b * (Formulas.formula<'a> * Formulas.formula<'a>) list) list>
    when 'a: comparison and 'b: comparison

val saturate_upto:
  vars: Formulas.formula<'a> list ->
    n: int ->
    m: int ->
    trigs: ((Formulas.formula<'a> * Formulas.formula<'a>) *
            (Formulas.formula<'a> * Formulas.formula<'a>) list) list ->
    assigs: (Formulas.formula<'a> * Formulas.formula<'a>) list -> bool
    when 'a: comparison

val stalmarck: fm: Formulas.formula<Prop.prop> -> bool

module FolAutomReas.Fol

/// Type for first order terms.
type term =
    | Var of string
    | Fn of string * term list

/// Type for atomic first order formulas.
type fol = | R of string * term list

/// Applies a subfunction `f` to the top *terms*.
val onformula:
  f: (term -> term) -> (Formulas.formula<fol> -> Formulas.formula<fol>)

/// Checks if a string is a constant term. Only numerals and the empty list constant "nil" are considered as constants.
val is_const_name: s: string -> bool

/// Parses an atomic term.
val parse_atomic_term: vs: string list -> inp: string list -> term * string list

/// Recursive descent parser of terms built up from an atomic term parser 
/// by cascading instances of parse infix in order of precedence, following the 
/// conventions with '^' coming highest and '::' lowest.
/// 
/// It takes a list of string tokens `inp` and returns a pair consisting of the 
/// parsed term tree together with any unparsed input. 
/// 
/// In order to check whether a name is within the scope of a quantiﬁer, it 
/// takes an additional argument `vs` which is the set of bound variables in 
/// the current scope.
val parse_term: vs: string list -> inp: string list -> term * string list

/// Parses a string into a term.
val parset: (string -> term)

/// A convenient operator to call `parset`.
val (!!!) : (string -> term)

/// A special recognizer for 'inﬁx' atomic formulas like s < t.
val parse_infix_atom:
  vs: string list -> inp: string list -> Formulas.formula<fol> * string list

/// Parses atomic fol formulas.
val parse_atom:
  vs: string list -> inp: string list -> Formulas.formula<fol> * string list

/// Parses a fol formula
val parse: (string -> Formulas.formula<fol>)

/// A convenient operator to call `parse`.
val (!!) : (string -> Formulas.formula<fol>)

/// Prints terms.
val fprint_term: tw: System.IO.TextWriter -> prec: int -> fm: term -> unit

/// Prints a function and its arguments.
val fprint_fargs:
  tw: System.IO.TextWriter -> f: string -> args: term list -> unit

/// Prints an infix operation.
val fprint_infix_term:
  tw: System.IO.TextWriter ->
    isleft: bool ->
    oldprec: int -> newprec: int -> sym: string -> p: term -> q: term -> unit

/// Term printer with TextWriter.
val fprintert: tw: System.IO.TextWriter -> tm: term -> unit

/// Term printer.
val inline print_term: t: term -> unit

/// Return the string of the concrete syntax representation of a term.
val inline sprint_term: t: term -> string

/// Printer of atomic fol formulas with TextWriter.
val fprint_atom: tw: System.IO.TextWriter -> prec: 'a -> fol -> unit

/// Printer of atomic fol formulas.
val inline print_atom: prec: 'a -> arg: fol -> unit

/// Returns the concrete syntax representation of an atom.
val inline sprint_atom: prec: 'a -> arg: fol -> string

/// Printer of fol formulas with TextWriter.
val fprint_fol_formula:
  tw: System.IO.TextWriter -> (Formulas.formula<fol> -> unit)

/// Printer of fol formulas.
val inline print_fol_formula: f: Formulas.formula<fol> -> unit

/// Returns the string of the concrete syntax representation of fol formulas.
val inline sprint_fol_formula: f: Formulas.formula<fol> -> string

/// Returns the value of a term `tm` in a particular 
/// interpretation M (`domain`, `func`, `pred`) and valuation `v`.
val termval:
  domain: 'a * func: (string -> 'b list -> 'b) * pred: 'c ->
    v: Lib.FPF.func<string,'b> -> tm: term -> 'b

/// Evaluates a fol formula `fm` in the interpretation specified
/// by the triplet `domain`, `func`, `pred` and the variables valuation `v`.
val holds:
  domain: 'a list * func: (string -> 'a list -> 'a) *
  pred: (string -> 'a list -> bool) ->
    v: Lib.FPF.func<string,'a> -> fm: Formulas.formula<fol> -> bool

/// An interpretation à la Boole.
val bool_interp:
  bool list * (string -> bool list -> bool) * (string -> 'a list -> bool)
    when 'a: equality

/// An arithmetic modulo `n` interpretation.
val mod_interp:
  n: int -> int list * (string -> int list -> int) * (string -> 'a list -> bool)
    when 'a: equality

/// Returns the free variables in the term `tm`.
val fvt: tm: term -> string list

/// Returns all the variables in the FOL formula `fm`.
val var: fm: Formulas.formula<fol> -> string list

/// Returns the free variables in the FOL formula `fm`.
val fv: fm: Formulas.formula<fol> -> string list

/// Universal closure of a formula.
val generalize: fm: Formulas.formula<fol> -> Formulas.formula<fol>

/// Substitution within terms.                                                //
val tsubst: sfn: Lib.FPF.func<string,term> -> tm: term -> term

/// Creates a ‘variant’ of a variable name by adding prime characters to it 
/// until it is distinct from some given list of variables to avoid.
/// 
/// `variant "x" ["x"; "y"]` returns `"x'"`.
val variant: x: string -> vars: string list -> string

/// Given a substitution function `sbfn` applies it to the input formula `fm`.
/// Bound variables will be renamed if necessary to avoid capture.
/// 
/// `subst ("y" |=> Var "x") ("forall x. x = y" |> parse)` returns 
/// `<<forall x'. x' = x>>`.
val subst:
  subfn: Lib.FPF.func<string,term> ->
    fm: Formulas.formula<fol> -> Formulas.formula<fol>

/// Checks whether there would be variable capture if the bound variable 
/// `x` is not renamed.
val substq:
  subfn: Lib.FPF.func<string,term> ->
    quant: (string -> Formulas.formula<fol> -> Formulas.formula<fol>) ->
    x: string -> p: Formulas.formula<fol> -> Formulas.formula<fol>


module FolAutomReas.Skolem

/// Performs a simplification routine but just at the first level of the input 
/// formula `fm`. It eliminates the basic propositional constants `False` and 
/// `True` and also the vacuous universal and existential quantiﬁers (those 
/// applied to variables that does not occur free in the body).
/// 
/// Whenever `False` and `True` occur in combination, there is always a a 
/// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
/// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
/// also eliminates double negations `~~p`.
/// 
/// If x not in FV(p) then forall x. p and exists x. p are logically 
/// equivalent to p.
/// 
/// `simplify1 (parse @"exists x. P(y)")` returns `<<P(y)>>`
/// 
/// `simplify1 (parse @"true ==> exists x. P(x)")` returns `<<exists x. P(x)>>`
val simplify1: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

/// Performs a simplification routine on the input formula 
/// `fm` eliminating the basic propositional constants `False` and `True`
/// and also the vacuous universal and existential quantiﬁers (those 
/// applied to variables that does not occur free in the body).
/// 
/// Whenever `False` and `True` occur in combination, there is always a a 
/// tautology justifying the equivalence with a simpler formula, e.g. `False /\ 
/// p <=> False`, `True \/ p <=> p`, `p ==> False <=> ~p`. At he same time, it 
/// also eliminates double negations `~~p`.
/// 
/// If x not in FV(p) then forall x. p and exists x. p are logically 
/// equivalent to p.
/// 
/// While `simplify1` performs the transformation just at the first level, 
/// `simplify` performs it at every levels in a recursive bottom-up sweep.
/// 
/// `simplify (parse @"true ==> (p <=> (p <=> false))")` returns 
/// `<<p <=> ~p>>`
/// 
/// `simplify (parse @"exists x y z. P(x) ==> Q(z) ==> false")` 
/// returns `<<exists x z. P(x) ==> ~Q(z)>>`
val simplify: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

/// Transforms the input formula `fm` in negation normal form.
/// 
/// It eliminates implication and equivalence, and pushes down negations 
/// through quantiﬁers.
/// 
/// `nnf (parse @"~ exists x. P(x) <=> Q(x)")` returns 
/// `<<forall x. P(x) /\ ~Q(x) \/ ~P(x) /\ Q(x)>>`
val nnf: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// It pulls out quantifiers.
val pullquants: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

/// calls the main `pullquants` functions again on the body to pull up 
/// further quantiﬁers
val pullq:
  l: bool * r: bool ->
    fm: Formulas.formula<Fol.fol> ->
    quant: (string -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    op: (Formulas.formula<Fol.fol> ->
           Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    x: string ->
    y: string ->
    p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

/// leaves quantiﬁed formulas alone, and for conjunctions and disjunctions 
/// recursively prenexes the immediate subformulas and then uses pullquants
val prenex: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

/// Transforms the input formula `fm` in prenex normal form and simplifies it.
/// 
/// * simplifies away False, True, vacuous quantiﬁcation, etc.;
/// * eliminates implication and equivalence, push down negations;
/// * pulls out quantiﬁers.
/// 
/// `pnf (parse @"(forall x. P(x) \/ R(y)) ==> exists y z. Q(y) \/ ~(exists z. P
/// (z) /\ Q(z))")` 
/// returns `<<exists x. forall z. ~P(x) /\ ~R(y) \/ Q(x) \/ ~P(z) \/ ~Q(z)>>`
val pnf: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

/// Returns the functions present in the input term `tm`
/// 
/// `funcs (parset @"x + 1")` returns `[("+", 2); ("1", 0)]`
val funcs: tm: Fol.term -> (string * int) list

/// Returns the functions present in the input formula `fm`
/// 
/// `functions (parse @"x + 1 > 0 /\ f(z) > g(z,i)")`
/// returns `[("+", 2); ("0", 0); ("1", 0); ("f", 1); ("g", 2)]`
/// `
val functions: fm: Formulas.formula<Fol.fol> -> (string * int) list

/// Core Skolemization function specifically intended to be used on NNF 
/// formulas. 
/// 
/// It simply recursively descends the formula, Skolemizing any existential 
/// formulas and then proceeding to subformulas using `skolem2` for binary 
/// connectives.
val skolem:
  fm: Formulas.formula<Fol.fol> ->
    fns: string list -> Formulas.formula<Fol.fol> * string list

/// Auxiliary to `skolem` when dealing with binary connectives. 
/// It updates the set of functions to avoid with new Skolem functions 
/// introduced into one formula before tackling the other.
val skolem2:
  cons: (Formulas.formula<Fol.fol> * Formulas.formula<Fol.fol> ->
           Formulas.formula<Fol.fol>) ->
    p: Formulas.formula<Fol.fol> * q: Formulas.formula<Fol.fol> ->
      fns: string list -> Formulas.formula<Fol.fol> * string list

/// Overall Skolemization function, intended to be used with any type of 
/// initial fol formula.
val askolemize: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

/// Removes all universale quantifiers from the input formula `p`.
/// 
/// `specialize <<forall x y. P(x) /\ P(y)>>` returns `<<P(x) /\ P(y)>>`
val specialize: fm: Formulas.formula<'a> -> Formulas.formula<'a>

/// Puts the input formula `fm` into skolem normal form 
/// while also removing all universal quantifiers.
/// 
/// It puts the formula in prenex normal form, substitutes existential 
/// quantifiers with skolem functions and also removes all universal 
/// quantifiers.
/// 
/// `skolemize (parse @"forall x. exists y. R(x,y)")`
/// returns `<<R(x,f_y(x))>>`
val skolemize: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>


module FolAutomReas.Pelletier

/// ¬¬(∃x. ∀y z. (P(y) ⟶ Q(z)) ⟶ (P(x) ⟶ Q(x)))
val p19: Formulas.formula<Fol.fol>

/// (∀x y. ∃z. ∀w. (P(x) ∧ Q(y) ⟶ R(z) ∧ S(w))) ⟶ (∃x y. P(x) ∧ Q(y)) ⟶ (∃z. R(z))
val p20: Formulas.formula<Fol.fol>

/// ~(∃ x. U(x) ∧ Q(x)) ∧ (∀ x. P(x) ⟶ Q(x) \/ R(x)) ∧ 
/// ~(∃ x. P(x) ⟶ (∃ x. Q(x))) ∧ (∀ x. Q(x) ∧ R(x) ⟶ U(x)) 
/// ⟶ (∃ x. P(x) ∧ R(x))
val p24: Formulas.formula<Fol.fol>

/// (∀x. ∃y. J(x,y)) ∧ (∀x. ∃y. G(x,y)) ∧ 
/// (∀x y. J(x,y) ∨ G(x,y) ⟶ (∀z. J(y,z) ∨ G(y,z) ⟶ H(x,z))) ⟶ (∀x. ∃y. H(x,y))
val p36: Formulas.formula<Fol.fol>

val p38: Formulas.formula<Fol.fol>

/// ¬ (∃x. ∀y. P(y,x) ⟷ (¬P(y,y))
val p39: Formulas.formula<Fol.fol>

/// ¬ (∃y. ∀x. P(x,y) ⟷ (∃z. P(x,z) ∧ P(z,y))
val p42: Formulas.formula<Fol.fol>

/// (∀x y. Q(x,y) ⟷ ∀z. P(z,x) ⟷ P(z,y)) ⟶ ∀x y. Q(x,y) ⟷ Q(y,x)
val p43: Formulas.formula<Fol.fol>

/// (∀x. P(x) ⟶ (∃y. G(y) ∧ H(x,y) ∧ 
/// (∃y. G(y) ∧ ~ H(x,y)))) ∧ 
/// (∃x. J(x) ∧ (∀y. G(y) ⟶ H(x,y))) 
/// ⟶ (∃x. j(x) ∧ ¬f(x))
val p44: Formulas.formula<Fol.fol>

/// (∀ x. P(x) ∧ (∀ y. G(y) ∧ H(x,y) ⟶ J(x,y)) ⟶ (∀ y. G(y) ∧ H(x,y) ⟶ R(y))) ∧ 
/// ~(∃ y. L(y) ∧ R(y)) ∧ (∃ x. P(x) ∧ (∀ y. H(x,y) ⟶ L(y)) ∧ 
/// (∀ y. G(y) ∧ H(x,y) ⟶ J(x,y))) ⟶ (∃ x. P(x) ∧ ~(∃ y. G(y) ∧ H(x,y)))
val p45: Formulas.formula<Fol.fol>

/// (∀ x. P(x) ⟷ ~P(f(x))) ⟶ (∃ x. P(x) ∧ ~P(f(x)))
val p59: Formulas.formula<Fol.fol>

/// ∀x. P(x,f(x)) ⟷ (∃y. (∀z. P(z,y) ⟶ P (z,f(x))) ∧ P(x,y))
val p60: Formulas.formula<Fol.fol>


module FolAutomReas.Herbrand

/// A variant of the notion of propositional evaluation `eval` where the 
/// input propositional valuation `d` maps atomic formulas themselves to 
/// truth values.
/// 
/// It determines if the input formula `fm` holds in the sense of propositional 
/// logic for this notion of valuation.
/// 
/// `pholds (function Atom (R ("P", [Var "x"])) -> true) (parse "P(x)")`
/// returns `true`
val pholds:
  d: (Formulas.formula<'a> -> bool) -> fm: Formulas.formula<'a> -> bool

/// Gets the constants for Herbrand base, adding nullary one if necessary. 
val herbfuns:
  fm: Formulas.formula<Fol.fol> -> (string * int) list * (string * int) list

/// Enumerates all ground terms involving `n` functions.
/// 
/// If `n` = 0, it returns the constant terms, otherwise tries all possible 
/// functions.
/// 
/// `groundterms [0;1] [(f,1);(g,2)] 0` returns `[0,1]`.
/// 
/// `groundterms [0;1] [(f,1);(g,2)] 1` returns `[f(0);f(1);g(0,0);g(0,1);g...]`
/// 
/// `groundterms [0;1] [(f,1);(g,1)] 2` returns `[f(f(0));...;f(g(0,0));...]`
val groundterms:
  cntms: Fol.term list -> funcs: (string * int) list -> n: int -> Fol.term list

/// generates all `m`-tuples of ground terms involving (in total) `n` functions.
/// 
/// `groundtuples [0] [(f,1)] 1 1` returns `[[f(0)]]`
/// 
/// `groundtuples [0] [(f,1)] 1 2` returns `[[0;f(0)]; [f(0);0]]`
val groundtuples:
  cntms: Fol.term list ->
    funcs: (string * int) list -> n: int -> m: int -> Fol.term list list

/// <summary>
/// A generic function to be used with different 'herbrand procedures'.
/// 
/// It tests larger and larger conjunctions of ground instances for 
/// unsatisfiability, iterating modifier `mfn` over ground terms 
/// till `tfn` fails. 
/// </summary>
/// <param name="mfn">The modification function that augments the ground 
/// instances with a new instance.</param>
/// <param name="tfn">The satisfiability test to be done.</param>
/// <param name="fl0">The initial formula in some transformed list 
/// representation.</param>
/// <param name="cntms">The constant terms of the formula.</param>
/// <param name="funcs">The functions (name, arity) of the formula.</param>
/// <param name="fvs">The free variables of the formula.</param>
/// <param name="n">The next level of the enumeration to generate.</param>
/// <param name="fl">The set of ground instances so far.</param>
/// <param name="tried">The instances tried.</param>
/// <param name="tuples">The remaining ground instances in the current level.
/// </param>
val herbloop:
  mfn: ('a ->
          (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
          'b list -> 'b list) ->
    tfn: ('b list -> bool) ->
    fl0: 'a ->
    cntms: Fol.term list ->
    funcs: (string * int) list ->
    fvs: string list ->
    n: int ->
    fl: 'b list ->
    tried: Fol.term list list ->
    tuples: Fol.term list list -> Fol.term list list

/// In the specific case of the gilmore procedure, the generic herbrand loop 
/// `herbloop` is called with the initial formula `fl0` and the ground 
/// instances so far `fl` are maintained in a DNF list representation and the 
/// modification function applies the instantiation to the starting formula 
/// and combines the DNFs by distribution.
val gilmore_loop:
  (Formulas.formula<Fol.fol> list list ->
     Fol.term list ->
     (string * int) list ->
     string list ->
     int ->
     Formulas.formula<Fol.fol> list list ->
     Fol.term list list -> Fol.term list list -> Fol.term list list)

/// Tests an input fol formula `fm` for validity based on a gilmore-like 
/// procedure.
/// 
/// The initial formula is generalized, negated and Skolemized, then the 
/// specific herbrand loop for the gilmore procedure is called to test for 
/// the unsatisfiability of the transformed formula.
/// 
/// If the test terminates, it reports how many ground instances where tried.
val gilmore: fm: Formulas.formula<Fol.fol> -> int

/// <summary>
/// The modification function (specific to the Davis-Putnam procedure), that 
/// augments the ground instances with a new one.
/// </summary>
/// <example>
/// This example shows the first generation of ground instance when the set is 
/// initially empty.
/// <code lang="fsharp">
/// dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf ["x"] [!!!"c"])) []
/// </code>
/// returns 
/// <code lang="fsharp">
/// [[P(c)]; [~P(f_y(c))]]
/// </code>
/// This example shows the second generation of ground instance when the 
/// nonempty set is augmented.
/// <code lang="fsharp">
/// dp_mfn [[!!"P(x)"]; [!!"~P(f_y(x))"]] (subst (fpf ["x"] [!!!"f_y(c)"])) [[!!"P(c)"]; [!!"~P(f_y(c))"]]
/// </code>
/// returns 
/// <code lang="fsharp">
/// [[P(c)]; [P(f_y(c))]; [~P(f_y(c))]; [~P(f_y(f_y(c)))]]
/// </code>
/// </example>
/// <param name="cjs0">The initial formula in a list of list representation of conjunctive normal.</param>
/// <param name="ifn">The instantiation to be applied to the formula to generate ground instances.</param>
/// <param name="cjs">The set of ground instances so far.</param>
/// <returns>
/// The set of ground instances incremented.
/// </returns>
val dp_mfn:
  cjs0: 'a list list -> ifn: ('a -> 'b) -> cjs: 'b list list -> 'b list list
    when 'b: comparison

/// In the specific case of the davis-putnam procedure, the generic 
/// herbrand loop `herbloop` is called with the initial formula `fl0` 
/// and the ground instances so far `fl` are maintained in a CNF list 
/// representation and each time we incorporate a new instance, we check for 
/// unsatisfiability using `dpll`.
val dp_loop:
  (Formulas.formula<Fol.fol> list list ->
     Fol.term list ->
     (string * int) list ->
     string list ->
     int ->
     Formulas.formula<Fol.fol> list list ->
     Fol.term list list -> Fol.term list list -> Fol.term list list)

/// Tests an input fol formula `fm` for validity based on the Davis-Putnam 
/// procedure.
/// 
/// The initial formula is generalized, negated and Skolemized, then the 
/// specific herbrand loop for the davis-putnam procedure is called to test for 
/// the unsatisfiability of the transformed formula.
/// 
/// If the test terminates, it reports how many ground instances where tried.
val davisputnam: fm: Formulas.formula<Fol.fol> -> int

/// Auxiliary function to redefine the Davis-Putnam procedure to run through 
/// the list of possibly-needed instances `dunno`, putting them onto the list 
/// of needed ones `need` only if the other instances are satisfiable.
val dp_refine:
  cjs0: Formulas.formula<Fol.fol> list list ->
    fvs: string list ->
    dunno: Fol.term list list -> need: Fol.term list list -> Fol.term list list

val dp_refine_loop:
  cjs0: Formulas.formula<Fol.fol> list list ->
    cntms: Fol.term list ->
    funcs: (string * int) list ->
    fvs: string list ->
    n: int ->
    cjs: Formulas.formula<Fol.fol> list list ->
    tried: Fol.term list list ->
    tuples: Fol.term list list -> Fol.term list list

/// Tests an input fol formula `fm` for validity based on the Davis-Putnam 
/// procedure redefined to run through the list of possibly-needed 
/// instances, putting them onto the list of needed ones only if 
/// the other instances are satisfiable.
val davisputnam002: fm: Formulas.formula<Fol.fol> -> int








module FolAutomReas.Resolution

val barb: Formulas.formula<Fol.fol>

val mgu:
  l: Formulas.formula<Fol.fol> list ->
    env: Lib.FPF.func<string,Fol.term> -> Lib.FPF.func<string,Fol.term>

val unifiable:
  p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> bool

val rename:
  pfx: string ->
    cls: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val resolvents:
  cl1: Formulas.formula<Fol.fol> list ->
    cl2: Formulas.formula<Fol.fol> list ->
    p: Formulas.formula<Fol.fol> ->
    acc: Formulas.formula<Fol.fol> list list ->
    Formulas.formula<Fol.fol> list list

val resolve_clauses:
  cls1: Formulas.formula<Fol.fol> list ->
    cls2: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list list

val resloop001:
  used: Formulas.formula<Fol.fol> list list *
  unused: Formulas.formula<Fol.fol> list list -> bool

val pure_resolution001: fm: Formulas.formula<Fol.fol> -> bool

val resolution001: fm: Formulas.formula<Fol.fol> -> bool list

val term_match:
  env: Lib.FPF.func<string,Fol.term> ->
    eqs: (Fol.term * Fol.term) list -> Lib.FPF.func<string,Fol.term>

val match_literals:
  env: Lib.FPF.func<string,Fol.term> ->
    Formulas.formula<Fol.fol> * Formulas.formula<Fol.fol> ->
      Lib.FPF.func<string,Fol.term>

val subsumes_clause:
  cls1: Formulas.formula<Fol.fol> list ->
    cls2: Formulas.formula<Fol.fol> list -> bool

val replace:
  cl: Formulas.formula<Fol.fol> list ->
    lis: Formulas.formula<Fol.fol> list list ->
    Formulas.formula<Fol.fol> list list

val incorporate:
  gcl: Formulas.formula<Fol.fol> list ->
    cl: Formulas.formula<Fol.fol> list ->
    unused: Formulas.formula<Fol.fol> list list ->
    Formulas.formula<Fol.fol> list list

val resloop002:
  used: Formulas.formula<Fol.fol> list list *
  unused: Formulas.formula<Fol.fol> list list -> bool

val pure_resolution002: fm: Formulas.formula<Fol.fol> -> bool

val resolution002: fm: Formulas.formula<Fol.fol> -> bool list

val presolve_clauses:
  cls1: Formulas.formula<Fol.fol> list ->
    cls2: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list list

val presloop:
  used: Formulas.formula<Fol.fol> list list *
  unused: Formulas.formula<Fol.fol> list list -> bool

val pure_presolution: fm: Formulas.formula<Fol.fol> -> bool

val presolution: fm: Formulas.formula<Fol.fol> -> bool list

val pure_resolution: fm: Formulas.formula<Fol.fol> -> bool

val resolution003: fm: Formulas.formula<Fol.fol> -> bool list


module FolAutomReas.Prolog

val renamerule:
  k: int ->
    asm: Formulas.formula<Fol.fol> list * c: Formulas.formula<Fol.fol> ->
      (Formulas.formula<Fol.fol> list * Formulas.formula<Fol.fol>) * int

val backchain:
  rules: (Formulas.formula<Fol.fol> list * Formulas.formula<Fol.fol>) list ->
    n: int ->
    k: int ->
    env: Lib.FPF.func<string,Fol.term> ->
    goals: Formulas.formula<Fol.fol> list -> Lib.FPF.func<string,Fol.term>

val hornify:
  cls: Formulas.formula<'a> list ->
    Formulas.formula<'a> list * Formulas.formula<'a> when 'a: equality

val hornprove:
  fm: Formulas.formula<Fol.fol> -> Lib.FPF.func<string,Fol.term> * int

val parserule:
  s: string -> Formulas.formula<Fol.fol> list * Formulas.formula<Fol.fol>

val simpleprolog:
  rules: string list -> gl: string -> Lib.FPF.func<string,Fol.term>

val prolog: rules: string list -> gl: string -> Formulas.formula<Fol.fol> list


module FolAutomReas.Meson

val contrapositives:
  cls: Formulas.formula<'a> list ->
    (Formulas.formula<'a> list * Formulas.formula<'a>) list when 'a: comparison

val mexpand001:
  rules: (Formulas.formula<Fol.fol> list * Formulas.formula<Fol.fol>) list ->
    ancestors: Formulas.formula<Fol.fol> list ->
    g: Formulas.formula<Fol.fol> ->
    cont: (Lib.FPF.func<string,Fol.term> * int * int -> 'a) ->
    env: Lib.FPF.func<string,Fol.term> * n: int * k: int -> 'a

val puremeson001: fm: Formulas.formula<Fol.fol> -> int

val meson001: fm: Formulas.formula<Fol.fol> -> int list

val equal:
  env: Lib.FPF.func<string,Fol.term> ->
    fm1: Formulas.formula<Fol.fol> -> fm2: Formulas.formula<Fol.fol> -> bool

val expand2:
  expfn: ('a -> ('b * int * 'c -> 'd) -> 'b * int * 'c -> 'd) ->
    goals1: 'a ->
    n1: int ->
    goals2: 'a ->
    n2: int -> n3: int -> cont: ('b * int * 'c -> 'd) -> env: 'b -> k: 'c -> 'd

val mexpand002:
  rules: (Formulas.formula<Fol.fol> list * Formulas.formula<Fol.fol>) list ->
    ancestors: Formulas.formula<Fol.fol> list ->
    g: Formulas.formula<Fol.fol> ->
    cont: (Lib.FPF.func<string,Fol.term> * int * int -> 'a) ->
    env: Lib.FPF.func<string,Fol.term> * n: int * k: int -> 'a

val puremeson002: fm: Formulas.formula<Fol.fol> -> int

val meson002: fm: Formulas.formula<Fol.fol> -> int list


module FolAutomReas.Skolems

val rename_term: tm: Fol.term -> Fol.term

val rename_form: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val skolems:
  fms: Formulas.formula<Fol.fol> list ->
    corr: string list -> Formulas.formula<Fol.fol> list * string list

val skolemizes:
  fms: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list


module FolAutomReas.Equal

val is_eq: _arg1: Formulas.formula<Fol.fol> -> bool

val mk_eq: s: Fol.term -> t: Fol.term -> Formulas.formula<Fol.fol>

val dest_eq: fm: Formulas.formula<Fol.fol> -> Fol.term * Fol.term

val lhs: eq: Formulas.formula<Fol.fol> -> Fol.term

val rhs: eq: Formulas.formula<Fol.fol> -> Fol.term

val predicates: fm: Formulas.formula<Fol.fol> -> (string * int) list

val function_congruence: f: string * n: int -> Formulas.formula<Fol.fol> list

val predicate_congruence: p: string * n: int -> Formulas.formula<Fol.fol> list

val equivalence_axioms: Formulas.formula<Fol.fol> list

val equalitize: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>


module FolAutomReas.Cong

val subterms: tm: Fol.term -> Fol.term list

val congruent:
  eqv: Lib.UnionFindAlgorithm.partition<Fol.term> ->
    s: Fol.term * t: Fol.term -> bool

val emerge:
  s: Fol.term * t: Fol.term ->
    eqv: Lib.UnionFindAlgorithm.partition<Fol.term> *
    pfn: Lib.FPF.func<Fol.term,Fol.term list> ->
      Lib.UnionFindAlgorithm.partition<Fol.term> *
      Lib.FPF.func<Fol.term,Fol.term list>

val predecessors:
  t: Fol.term ->
    pfn: Lib.FPF.func<Fol.term,Fol.term list> ->
    Lib.FPF.func<Fol.term,Fol.term list>

val ccsatisfiable: fms: Formulas.formula<Fol.fol> list -> bool

val ccvalid: fm: Formulas.formula<Fol.fol> -> bool


module FolAutomReas.Rewrite

val rewrite1: eqs: Formulas.formula<Fol.fol> list -> t: Fol.term -> Fol.term

val rewrite: eqs: Formulas.formula<Fol.fol> list -> tm: Fol.term -> Fol.term


module FolAutomReas.Order

val termsize: tm: Fol.term -> int

val lexord:
  ord: ('a -> 'a -> bool) -> l1: 'a list -> l2: 'a list -> bool
    when 'a: equality

val lpo_gt:
  w: (string * int -> string * int -> bool) ->
    s: Fol.term -> t: Fol.term -> bool

val lpo_ge:
  w: (string * int -> string * int -> bool) ->
    s: Fol.term -> t: Fol.term -> bool

val weight:
  lis: 'a list -> f: 'a * n: 'b -> g: 'a * m: 'b -> bool
    when 'a: comparison and 'b: comparison


module FolAutomReas.Completion

val renamepair:
  fm1: Formulas.formula<Fol.fol> * fm2: Formulas.formula<Fol.fol> ->
    Formulas.formula<Fol.fol> * Formulas.formula<Fol.fol>

val listcases:
  fn: ('a -> ('b -> 'a -> 'c) -> 'd list) ->
    rfn: ('b -> 'a list -> 'c) -> lis: 'a list -> acc: 'd list -> 'd list

val overlaps:
  l: Fol.term * r: Fol.term ->
    tm: Fol.term ->
    rfn: (Lib.FPF.func<string,Fol.term> -> Fol.term -> 'a) -> 'a list

val crit1:
  Formulas.formula<Fol.fol> ->
    Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol> list

val critical_pairs:
  fma: Formulas.formula<Fol.fol> ->
    fmb: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol> list

val normalize_and_orient:
  ord: (Fol.term -> Fol.term -> bool) ->
    eqs: Formulas.formula<Fol.fol> list ->
    Formulas.formula<Fol.fol> -> Fol.term * Fol.term

val status:
  eqs: 'a list * def: 'b list * crs: 'c list -> eqs0: 'a list -> unit
    when 'a: equality

val complete:
  ord: (Fol.term -> Fol.term -> bool) ->
    eqs: Formulas.formula<Fol.fol> list * def: Formulas.formula<Fol.fol> list *
    crits: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val interreduce:
  dun: Formulas.formula<Fol.fol> list ->
    eqs: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val complete_and_simplify:
  wts: string list ->
    eqs: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val eqs: Formulas.formula<Fol.fol> list

val wts: string list

val ord: (Fol.term -> Fol.term -> bool)

val def: 'a list

val crits: Formulas.formula<Fol.fol> list

val complete1:
  ord: (Fol.term -> Fol.term -> bool) ->
    eqs: Formulas.formula<Fol.fol> list * def: Formulas.formula<Fol.fol> list *
    crits: Formulas.formula<Fol.fol> list ->
      Formulas.formula<Fol.fol> list * Formulas.formula<Fol.fol> list *
      Formulas.formula<Fol.fol> list


module FolAutomReas.Eqelim

val modify_S:
  cl: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list list

val modify_T:
  cl: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val is_nonvar: _arg1: Fol.term -> bool

val find_nestnonvar: tm: Fol.term -> Fol.term

val find_nvsubterm: fm: Formulas.formula<Fol.fol> -> Fol.term

val replacet: rfn: Lib.FPF.func<Fol.term,Fol.term> -> tm: Fol.term -> Fol.term

val replace:
  rfn: Lib.FPF.func<Fol.term,Fol.term> ->
    (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val emodify:
  fvs: string list ->
    cls: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val modify_E:
  cls: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val brand:
  cls: Formulas.formula<Fol.fol> list list ->
    Formulas.formula<Fol.fol> list list

val bpuremeson: fm: Formulas.formula<Fol.fol> -> int

val bmeson: fm: Formulas.formula<Fol.fol> -> int list

val emeson: fm: Formulas.formula<Fol.fol> -> int list


module FolAutomReas.Paramodulation

val overlapl:
  l: Fol.term * r: Fol.term ->
    fm: Formulas.formula<Fol.fol> ->
    rfn: (Lib.FPF.func<string,Fol.term> -> Formulas.formula<Fol.fol> -> 'a) ->
    'a list

val overlapc:
  l: Fol.term * r: Fol.term ->
    cl: Formulas.formula<Fol.fol> list ->
    rfn: (Lib.FPF.func<string,Fol.term> -> Formulas.formula<Fol.fol> list -> 'a) ->
    acc: 'a list -> 'a list

val paramodulate:
  pcl: Formulas.formula<Fol.fol> list ->
    ocl: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list list

val para_clauses:
  cls1: Formulas.formula<Fol.fol> list ->
    cls2: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list list

val paraloop:
  used: Formulas.formula<Fol.fol> list list *
  unused: Formulas.formula<Fol.fol> list list -> bool

val pure_paramodulation: fm: Formulas.formula<Fol.fol> -> bool

val paramodulation: fm: Formulas.formula<Fol.fol> -> bool list


module FolAutomReas.Decidable

val aedecide: fm: Formulas.formula<Fol.fol> -> bool

val separate:
  x: string -> cjs: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol>

val pushquant:
  x: string -> p: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val miniscope: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val wang: fm: Formulas.formula<Fol.fol> -> bool

val atom: p: string -> x: string -> Formulas.formula<Fol.fol>

val premiss_A: p: string * q: string -> Formulas.formula<Fol.fol>

val premiss_E: p: string * q: string -> Formulas.formula<Fol.fol>

val premiss_I: p: string * q: string -> Formulas.formula<Fol.fol>

val premiss_O: p: string * q: string -> Formulas.formula<Fol.fol>

val anglicize_premiss: fm: Formulas.formula<Fol.fol> -> string

val anglicize_syllogism: Formulas.formula<Fol.fol> -> string

val all_possible_syllogisms: Formulas.formula<Fol.fol> list

val all_possible_syllogisms': Formulas.formula<Fol.fol> list

val alltuples: n: int -> l: 'a list -> 'a list list

val allmappings:
  dom: 'a list -> ran: 'b list -> ('a -> 'b) list when 'a: equality

val alldepmappings:
  dom: ('a * 'b) list -> ran: ('b -> 'c list) -> ('a -> 'c) list
    when 'a: equality

val allfunctions:
  dom: 'a list -> n: int -> ('a list -> 'a) list when 'a: equality

val allpredicates:
  dom: 'a list -> n: int -> ('a list -> bool) list when 'a: equality

val decide_finite: n: int -> fm: Formulas.formula<Fol.fol> -> bool

val limmeson:
  n: int ->
    fm: Formulas.formula<Fol.fol> -> Lib.FPF.func<string,Fol.term> * int * int

val limited_meson:
  n: int ->
    fm: Formulas.formula<Fol.fol> ->
    (Lib.FPF.func<string,Fol.term> * int * int) list

val decide_fmp: fm: Formulas.formula<Fol.fol> -> bool

val decide_monadic: fm: Formulas.formula<Fol.fol> -> bool


module FolAutomReas.Qelim

val qelim:
  bfn: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    x: string -> p: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val lift_qelim:
  afn: (string list -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    nfn: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    qfn: (string list -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val cnnf:
  lfn: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>) ->
    (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val lfn_dlo: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val dlobasic: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val afn_dlo:
  vars: 'a -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val quelim_dlo: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)


module FolAutomReas.Cooper

val zero: Fol.term

val mk_numeral: n: Lib.Num.num -> Fol.term

val dest_numeral: t: Fol.term -> Lib.Num.num

val is_numeral: (Fol.term -> bool)

val numeral1: fn: (Lib.Num.num -> Lib.Num.num) -> n: Fol.term -> Fol.term

val numeral2:
  fn: (Lib.Num.num -> Lib.Num.num -> Lib.Num.num) ->
    m: Fol.term -> n: Fol.term -> Fol.term

val linear_cmul: n: Lib.Num.num -> tm: Fol.term -> Fol.term

val linear_add: vars: string list -> tm1: Fol.term -> tm2: Fol.term -> Fol.term

val linear_neg: tm: Fol.term -> Fol.term

val linear_sub: vars: string list -> tm1: Fol.term -> tm2: Fol.term -> Fol.term

val linear_mul: tm1: Fol.term -> tm2: Fol.term -> Fol.term

val lint: vars: string list -> tm: Fol.term -> Fol.term

val mkatom:
  vars: string list -> p: string -> t: Fol.term -> Formulas.formula<Fol.fol>

val linform:
  vars: string list ->
    fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val posineq: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val formlcm: x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Lib.Num.num

val adjustcoeff:
  x: Fol.term ->
    l: Lib.Num.Num -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val unitycoeff:
  x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val minusinf:
  x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val divlcm: x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Lib.Num.num

val bset: x: Fol.term -> fm: Formulas.formula<Fol.fol> -> Fol.term list

val linrep:
  vars: string list ->
    x: Fol.term ->
    t: Fol.term -> fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val cooper:
  vars: string list ->
    fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val operations: (string * (Lib.Num.num -> Lib.Num.num -> bool)) list

val evalc: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val integer_qelim: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val relativize:
  r: (string -> Formulas.formula<'a>) ->
    fm: Formulas.formula<'a> -> Formulas.formula<'a>

val natural_qelim: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)


module FolAutomReas.Complex

val poly_add: vars: string list -> pol1: Fol.term -> pol2: Fol.term -> Fol.term

val poly_ladd: vars: string list -> pol1: Fol.term -> Fol.term -> Fol.term

val poly_neg: _arg1: Fol.term -> Fol.term

val poly_sub: vars: string list -> p: Fol.term -> q: Fol.term -> Fol.term

val poly_mul: vars: string list -> pol1: Fol.term -> pol2: Fol.term -> Fol.term

val poly_lmul: vars: string list -> pol1: Fol.term -> Fol.term -> Fol.term

val poly_pow: vars: string list -> p: Fol.term -> n: int -> Fol.term

val poly_div: vars: string list -> p: Fol.term -> q: Fol.term -> Fol.term

val poly_var: x: string -> Fol.term

val polynate: vars: string list -> tm: Fol.term -> Fol.term

val polyatom:
  vars: string list ->
    fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val coefficients: vars: string list -> _arg1: Fol.term -> Fol.term list

val degree: vars: string list -> p: Fol.term -> int

val is_constant: vars: string list -> p: Fol.term -> bool

val head: vars: string list -> p: Fol.term -> Fol.term

val behead: vars: string list -> _arg1: Fol.term -> Fol.term

val poly_cmul: k: Lib.Num.Num -> p: Fol.term -> Fol.term

val headconst: p: Fol.term -> Lib.Num.num

val monic: p: Fol.term -> Fol.term * bool

val pdivide: (string list -> Fol.term -> Fol.term -> int * Fol.term)

type sign =
    | Zero
    | Nonzero
    | Positive
    | Negative

val swap: swf: bool -> s: sign -> sign

val findsign: sgns: (Fol.term * sign) list -> p: Fol.term -> sign

val assertsign:
  sgns: (Fol.term * sign) list ->
    p: Fol.term * s: sign -> (Fol.term * sign) list

val split_zero:
  sgns: (Fol.term * sign) list ->
    pol: Fol.term ->
    cont_z: ((Fol.term * sign) list -> Formulas.formula<Fol.fol>) ->
    cont_n: ((Fol.term * sign) list -> Formulas.formula<Fol.fol>) ->
    Formulas.formula<Fol.fol>

val poly_nonzero:
  vars: string list ->
    sgns: (Fol.term * sign) list -> pol: Fol.term -> Formulas.formula<Fol.fol>

val poly_nondiv:
  vars: string list ->
    sgns: (Fol.term * sign) list ->
    p: Fol.term -> s: Fol.term -> Formulas.formula<Fol.fol>

val cqelim:
  vars: string list ->
    eqs: Fol.term list * neqs: Fol.term list ->
      sgns: (Fol.term * sign) list -> Formulas.formula<Fol.fol>

val init_sgns: (Fol.term * sign) list

val basic_complex_qelim:
  vars: string list -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val complex_qelim: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)


module FolAutomReas.Real

val poly_diffn: x: Fol.term -> n: int -> p: Fol.term -> Fol.term

val poly_diff: vars: string list -> p: Fol.term -> Fol.term

val rel_signs: (string * Complex.sign list) list

val testform:
  pmat: (Fol.term * Complex.sign) list -> fm: Formulas.formula<Fol.fol> -> bool

val inferpsign:
  pd: Complex.sign list * qd: Complex.sign list -> Complex.sign list

val condense: ps: Complex.sign list list -> Complex.sign list list

val inferisign: ps: Complex.sign list list -> Complex.sign list list

val dedmatrix:
  cont: (Complex.sign list list -> 'a) -> mat: Complex.sign list list -> 'a

val pdivide_pos:
  vars: string list ->
    sgns: (Fol.term * Complex.sign) list ->
    s: Fol.term -> p: Fol.term -> Fol.term

val split_sign:
  sgns: (Fol.term * Complex.sign) list ->
    pol: Fol.term ->
    cont: ((Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>) ->
    Formulas.formula<Fol.fol>

val split_trichotomy:
  sgns: (Fol.term * Complex.sign) list ->
    pol: Fol.term ->
    cont_z: ((Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>) ->
    cont_pn: ((Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>) ->
    Formulas.formula<Fol.fol>

val casesplit:
  vars: string list ->
    dun: Fol.term list ->
    pols: Fol.term list ->
    cont: (Complex.sign list list -> Formulas.formula<Fol.fol>) ->
    sgns: (Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>

val delconst:
  vars: string list ->
    dun: Fol.term list ->
    p: Fol.term ->
    ops: Fol.term list ->
    cont: (Complex.sign list list -> Formulas.formula<Fol.fol>) ->
    sgns: (Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>

val matrix:
  vars: string list ->
    pols: Fol.term list ->
    cont: (Complex.sign list list -> Formulas.formula<Fol.fol>) ->
    sgns: (Fol.term * Complex.sign) list -> Formulas.formula<Fol.fol>

val basic_real_qelim:
  vars: string list -> Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val real_qelim: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val grpterm: tm: Fol.term -> Fol.term

val grpform: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val real_qelim': (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)


module FolAutomReas.Grobner

val mmul:
  c1: Lib.Num.Num * m1: int list ->
    c2: Lib.Num.Num * m2: int list -> Lib.Num.Num * int list

val mdiv:
  (Lib.Num.Num * int list -> Lib.Num.Num * int list -> Lib.Num.Num * int list)

val mlcm:
  c1: 'a * m1: 'b list -> c2: 'c * m2: 'b list -> Lib.Num.Num * 'b list
    when 'b: comparison

val morder_lt: m1: int list -> m2: int list -> bool

val mpoly_mmul:
  Lib.Num.Num * int list ->
    pol: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_neg: ((Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list)

val mpoly_const:
  vars: 'a list -> c: Lib.Num.Num -> (Lib.Num.Num * int list) list

val mpoly_var:
  vars: 'a list -> x: 'a -> (Lib.Num.Num * int list) list when 'a: equality

val mpoly_add:
  l1: (Lib.Num.Num * int list) list ->
    l2: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_sub:
  l1: (Lib.Num.Num * int list) list ->
    l2: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_mul:
  l1: (Lib.Num.Num * int list) list ->
    l2: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_pow:
  vars: 'a list ->
    l: (Lib.Num.Num * int list) list -> n: int -> (Lib.Num.Num * int list) list

val mpoly_inv: p: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpoly_div:
  p: (Lib.Num.Num * int list) list ->
    q: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val mpolynate:
  vars: string list -> tm: Fol.term -> (Lib.Num.Num * int list) list

val mpolyatom:
  vars: string list ->
    fm: Formulas.formula<Fol.fol> -> (Lib.Num.Num * int list) list

val reduce1:
  Lib.Num.Num * int list ->
    pol: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val reduceb:
  Lib.Num.Num * int list ->
    pols: (Lib.Num.Num * int list) list list -> (Lib.Num.Num * int list) list

val reduce:
  pols: (Lib.Num.Num * int list) list list ->
    pol: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val spoly:
  pol1: (Lib.Num.Num * int list) list ->
    pol2: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list

val grobner:
  basis: (Lib.Num.Num * int list) list list ->
    pairs: ((Lib.Num.Num * int list) list * (Lib.Num.Num * int list) list) list ->
    (Lib.Num.Num * int list) list list

val groebner:
  basis: (Lib.Num.Num * int list) list list ->
    (Lib.Num.Num * int list) list list

val rabinowitsch:
  vars: 'a list ->
    v: 'a -> p: (Lib.Num.Num * int list) list -> (Lib.Num.Num * int list) list
    when 'a: equality

val grobner_trivial: fms: Formulas.formula<Fol.fol> list -> bool

val grobner_decide: fm: Formulas.formula<Fol.fol> -> bool

val term_of_varpow: vars: 'a -> x: string * k: int -> Fol.term

val term_of_varpows: vars: string list -> lis: int list -> Fol.term

val term_of_monomial:
  vars: string list -> c: Lib.Num.num * m: int list -> Fol.term

val term_of_poly:
  vars: string list -> pol: (Lib.Num.num * int list) list -> Fol.term

val grobner_basis:
  vars: string list -> pols: Formulas.formula<Fol.fol> list -> Fol.term list


module FolAutomReas.Geom

val coordinations: (string * Formulas.formula<Fol.fol>) list

val coordinate: (Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val invariant:
  x': Fol.term * y': Fol.term ->
    s: string * z: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val invariant_under_translation:
  (string * Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val invariant_under_rotation:
  string * Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val originate: fm: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val invariant_under_scaling:
  string * Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val invariant_under_shearing:
  (string * Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>)

val pprove:
  vars: string list ->
    triang: Fol.term list ->
    p: Fol.term ->
    degens: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val triangulate:
  vars: string list ->
    consts: Fol.term list -> pols: Fol.term list -> Fol.term list

val wu:
  fm: Formulas.formula<Fol.fol> ->
    vars: string list -> zeros: string list -> Formulas.formula<Fol.fol> list


module FolAutomReas.Interpolation

val pinterpolate:
  p: Formulas.formula<'a> -> q: Formulas.formula<'a> -> Formulas.formula<'a>
    when 'a: comparison

val urinterpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val toptermt: fns: (string * int) list -> tm: Fol.term -> Fol.term list

val topterms:
  fns: (string * int) list -> (Formulas.formula<Fol.fol> -> Fol.term list)

val uinterpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val cinterpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val interpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val einterpolate:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>


module FolAutomReas.Combining

val real_lang:
  (string * int -> bool) * (string * int -> bool) *
  (Formulas.formula<Fol.fol> -> bool)

val int_lang:
  (string * int -> bool) * (string * int -> bool) *
  (Formulas.formula<Fol.fol> -> bool)

val add_default:
  langs: (('a -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    (('a -> bool) * (string * int -> bool) * (Formulas.formula<Fol.fol> -> bool)) list

val chooselang:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fm: Formulas.formula<Fol.fol> ->
    (string * int -> bool) * (string * int -> bool) * 'a

val listify:
  f: ('a -> ('b -> 'c) -> 'c) -> l: 'a list -> cont: ('b list -> 'c) -> 'c

val homot:
  fn: (string * int -> bool) * pr: 'a * dp: 'b ->
    tm: Fol.term ->
    cont: (Fol.term -> Lib.Num.num -> Formulas.formula<Fol.fol> list -> 'c) ->
    n: Lib.Num.num -> defs: Formulas.formula<Fol.fol> list -> 'c

val homol:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fm: Formulas.formula<Fol.fol> ->
    cont: (Formulas.formula<Fol.fol> ->
             Lib.Num.num -> Formulas.formula<Fol.fol> list -> 'b) ->
    n: Lib.Num.num -> defs: Formulas.formula<Fol.fol> list -> 'b

val homo:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fms: Formulas.formula<Fol.fol> list ->
    cont: (Formulas.formula<Fol.fol> list ->
             Lib.Num.num -> Formulas.formula<Fol.fol> list -> 'b) ->
    (Lib.Num.num -> Formulas.formula<Fol.fol> list -> 'b)

val homogenize:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fms: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val belongs:
  fn: (string * int -> bool) * pr: (string * int -> bool) * dp: 'a ->
    fm: Formulas.formula<Fol.fol> -> bool

val langpartition:
  langs: ((string * int -> bool) * (string * int -> bool) * 'a) list ->
    fms: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list list

val arreq: l: string list -> Formulas.formula<Fol.fol> list

val arrangement: part: string list list -> Formulas.formula<Fol.fol> list

val dest_def: fm: Formulas.formula<Fol.fol> -> string * Fol.term

val redeqs:
  eqs: Formulas.formula<Fol.fol> list -> Formulas.formula<Fol.fol> list

val trydps:
  ldseps: (('a * 'b * (Formulas.formula<Fol.fol> -> bool)) *
           Formulas.formula<Fol.fol> list) list ->
    fms: Formulas.formula<Fol.fol> list -> bool

val allpartitions: ('a list -> 'a list list list) when 'a: comparison

val nelop_refute001:
  vars: string list ->
    ldseps: (('a * 'b * (Formulas.formula<Fol.fol> -> bool)) *
             Formulas.formula<Fol.fol> list) list -> bool

val nelop1001:
  langs: ((string * int -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    fms0: Formulas.formula<Fol.fol> list -> bool

val nelop001:
  langs: ((string * int -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    fm: Formulas.formula<Fol.fol> -> bool

val findasubset: p: ('a list -> 'b) -> m: int -> l: 'a list -> 'b

val findsubset: p: ('a list -> bool) -> l: 'a list -> 'a list

val nelop_refute:
  eqs: Formulas.formula<Fol.fol> list ->
    ldseps: (('a * 'b * (Formulas.formula<Fol.fol> -> bool)) *
             Formulas.formula<Fol.fol> list) list -> bool

val nelop1:
  langs: ((string * int -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    fms0: Formulas.formula<Fol.fol> list -> bool

val nelop:
  langs: ((string * int -> bool) * (string * int -> bool) *
          (Formulas.formula<Fol.fol> -> bool)) list ->
    fm: Formulas.formula<Fol.fol> -> bool


module FolAutomReas.Lcf

/// checks whether a term s occurs as a sub-term of another term t
val occurs_in: s: Fol.term -> t: Fol.term -> bool

/// checks whether a term t occurs free in a formula fm
val free_in: t: Fol.term -> fm: Formulas.formula<Fol.fol> -> bool

/// The Core LCF proof system
/// 
/// The core proof system is the minimum set of inference rules and/or axioms 
/// sound and complete with respect to the defined semantics.
[<AutoOpen>]
module ProofSystem =
    
    type thm = private | Theorem of Formulas.formula<Fol.fol>
    
    /// modusponens (proper inference rule)
    /// 
    /// |- p -> q |- p ==> |- q
    val modusponens: pq: thm -> thm -> thm
    
    /// generalization (proper inference rule)
    /// 
    /// |- p ==> !x. p
    val gen: x: string -> thm -> thm
    
    /// |- p -> (q -> p)
    val axiom_addimp:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- (p -> q -> r) -> (p -> q) -> (p -> r)
    val axiom_distribimp:
      p: Formulas.formula<Fol.fol> ->
        q: Formulas.formula<Fol.fol> -> r: Formulas.formula<Fol.fol> -> thm
    
    /// |- ((p -> ⊥) -> ⊥) -> p
    val axiom_doubleneg: p: Formulas.formula<Fol.fol> -> thm
    
    /// |- (!x. p -> q) -> (!x. p) -> (!x. q)
    val axiom_allimp:
      x: string ->
        p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- p -> !x. p [provided x not in FV(p)]
    val axiom_impall: x: string -> p: Formulas.formula<Fol.fol> -> thm
    
    /// |- (?x. x = t) [provided x not in FVT(t)]
    val axiom_existseq: x: string -> t: Fol.term -> thm
    
    /// |- t = t
    val axiom_eqrefl: t: Fol.term -> thm
    
    /// |- s1 = t1 -> ... -> sn = tn -> f(s1, ..., sn) = f(t1, ..., tn)
    val axiom_funcong:
      f: string -> lefts: Fol.term list -> rights: Fol.term list -> thm
    
    /// |- s1 = t1 -> ... -> sn = tn -> f(s1, ..., sn) = f(t1, ..., tn)
    val axiom_predcong:
      p: string -> lefts: Fol.term list -> rights: Fol.term list -> thm
    
    /// |- (p <-> q) -> p -> q
    val axiom_iffimp1:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- (p <-> q) -> q -> p
    val axiom_iffimp2:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- (p -> q) -> (q -> p) -> (p <-> q)
    val axiom_impiff:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- ⊤ <-> (⊥ -> ⊥)
    val axiom_true: thm
    
    /// |- ~p <-> (p -> ⊥)
    val axiom_not: p: Formulas.formula<Fol.fol> -> thm
    
    /// |- p /\ q <-> (p -> q -> ⊥) -> ⊥
    val axiom_and:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// |- p \/ q <-> ~(~p /\ ~q)
    val axiom_or:
      p: Formulas.formula<Fol.fol> -> q: Formulas.formula<Fol.fol> -> thm
    
    /// (?x. p) <-> ~(!x. ~p)
    val axiom_exists: x: string -> p: Formulas.formula<Fol.fol> -> thm
    
    /// maps a theorem back to the formula that it proves
    val concl: thm -> Formulas.formula<Fol.fol>

/// Prints a theorem using a TextWriter.
val fprint_thm: sw: System.IO.TextWriter -> th: ProofSystem.thm -> unit

/// A printer for theorems
val inline print_thm: th: ProofSystem.thm -> unit

/// Theorem to string
val inline sprint_thm: th: ProofSystem.thm -> string


module FolAutomReas.Lcfprop

val imp_refl: p: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_unduplicate: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val negatef: fm: Formulas.formula<'a> -> Formulas.formula<'a>

val negativef: fm: Formulas.formula<'a> -> bool

val add_assum:
  p: Formulas.formula<Fol.fol> -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_add_assum:
  p: Formulas.formula<Fol.fol> -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_trans:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_insert:
  q: Formulas.formula<Fol.fol> -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_swap: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_trans_th:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> ->
    r: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_add_concl:
  r: Formulas.formula<Fol.fol> -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_swap_th:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> ->
    r: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_swap2: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_mp:
  ith: Lcf.ProofSystem.thm -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val iff_imp1: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val iff_imp2: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_antisym:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_doubleneg: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val ex_falso: p: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_trans2:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_trans_chain:
  ths: Lcf.ProofSystem.thm list ->
    th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_truefalse:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_mono_th:
  p: Formulas.formula<Fol.fol> ->
    p': Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> ->
    q': Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val truth: Lcf.ProofSystem.thm

val contrapos: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val and_left:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val and_right:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val conjths: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm list

val and_pair:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val shunt: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val unshunt: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val iff_def:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val expand_connective: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val eliminate_connective: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_false_conseqs:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm list

val imp_false_rule: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_true_rule:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val imp_contr:
  p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_front_th: n: int -> fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_front: n: int -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val lcfptab:
  fms: Formulas.formula<Fol.fol> list ->
    lits: Formulas.formula<Fol.fol> list -> Lcf.ProofSystem.thm

val lcftaut: p: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm


module FolAutomReas.Folderived

val eq_sym: s: Fol.term -> t: Fol.term -> Lcf.ProofSystem.thm

val eq_trans: s: Fol.term -> t: Fol.term -> u: Fol.term -> Lcf.ProofSystem.thm

val icongruence:
  s: Fol.term ->
    t: Fol.term -> stm: Fol.term -> ttm: Fol.term -> Lcf.ProofSystem.thm

val gen_right_th:
  x: string ->
    p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val genimp: x: string -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val gen_right: x: string -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val exists_left_th:
  x: string ->
    p: Formulas.formula<Fol.fol> ->
    q: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val exists_left: x: string -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val subspec: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val subalpha: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val isubst:
  s: Fol.term ->
    t: Fol.term ->
    sfm: Formulas.formula<Fol.fol> ->
    tfm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val alpha: z: string -> fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val ispec: t: Fol.term -> fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val spec: t: Fol.term -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm


module FolAutomReas.Lcffol

val unify_complementsf:
  env: Lib.FPF.func<string,Fol.term> ->
    Formulas.formula<Fol.fol> * Formulas.formula<Fol.fol> ->
      Lib.FPF.func<string,Fol.term>

val use_laterimp:
  i: Formulas.formula<Fol.fol> ->
    fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val imp_false_rule':
  th: ('a -> Lcf.ProofSystem.thm) -> es: 'a -> Lcf.ProofSystem.thm

val imp_true_rule':
  th1: ('a -> Lcf.ProofSystem.thm) ->
    th2: ('a -> Lcf.ProofSystem.thm) -> es: 'a -> Lcf.ProofSystem.thm

val imp_front':
  n: int -> thp: ('a -> Lcf.ProofSystem.thm) -> es: 'a -> Lcf.ProofSystem.thm

val add_assum':
  fm: Formulas.formula<Fol.fol> ->
    thp: ((Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm) ->
    (Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm

val eliminate_connective':
  fm: Formulas.formula<Fol.fol> ->
    thp: ((Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm) ->
    (Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm

val spec':
  y: Fol.term ->
    fm: Formulas.formula<Fol.fol> ->
    n: int ->
    thp: ((Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm) ->
    e: (Fol.term -> Fol.term) * s: 'a -> Lcf.ProofSystem.thm

val ex_falso':
  fms: Formulas.formula<Fol.fol> list ->
    e: (Fol.term -> Fol.term) * s: Formulas.formula<Fol.fol> ->
      Lcf.ProofSystem.thm

val complits':
  Formulas.formula<Fol.fol> list * lits: Formulas.formula<Fol.fol> list ->
    i: int ->
    e: (Fol.term -> Fol.term) * s: Formulas.formula<Fol.fol> ->
      Lcf.ProofSystem.thm

val deskol':
  skh: Formulas.formula<Fol.fol> ->
    thp: ((Fol.term -> Fol.term) * 'a -> Lcf.ProofSystem.thm) ->
    e: (Fol.term -> Fol.term) * s: 'a -> Lcf.ProofSystem.thm

val lcftab:
  skofun: (Formulas.formula<Fol.fol> -> Fol.term) ->
    fms: Formulas.formula<Fol.fol> list * lits: Formulas.formula<Fol.fol> list *
    n: int ->
      cont: (((Fol.term -> Fol.term) * Formulas.formula<Fol.fol> ->
                Lcf.ProofSystem.thm) ->
               Lib.FPF.func<string,Fol.term> *
               (Formulas.formula<Fol.fol> * Fol.term) list * int -> 'a) ->
      Lib.FPF.func<string,Fol.term> *
      (Formulas.formula<Fol.fol> * Fol.term) list * int -> 'a

val quantforms:
  e: bool -> fm: Formulas.formula<'a> -> Formulas.formula<'a> list
    when 'a: comparison

val skolemfuns:
  fm: Formulas.formula<Fol.fol> -> (Formulas.formula<Fol.fol> * Fol.term) list

val form_match:
  Formulas.formula<Fol.fol> * Formulas.formula<Fol.fol> ->
    env: Lib.FPF.func<string,Fol.term> -> Lib.FPF.func<string,Fol.term>

val lcfrefute:
  fm: Formulas.formula<Fol.fol> ->
    n: int ->
    cont: (((Fol.term -> Fol.term) * Formulas.formula<Fol.fol> ->
              Lcf.ProofSystem.thm) ->
             Lib.FPF.func<string,Fol.term> *
             (Formulas.formula<Fol.fol> * Fol.term) list * int -> 'a) -> 'a

val mk_skol:
  Formulas.formula<Fol.fol> * fx: Fol.term ->
    q: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val simpcont:
  thp: ((Fol.term -> Fol.term) * Formulas.formula<Fol.fol> -> 'a) ->
    env: Lib.FPF.func<string,Fol.term> *
    sks: (Formulas.formula<Fol.fol> * Fol.term) list * k: 'b -> 'a

val elim_skolemvar: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val deskolcont:
  thp: ((Fol.term -> Fol.term) * Formulas.formula<Fol.fol> ->
          Lcf.ProofSystem.thm) ->
    env: Lib.FPF.func<string,Fol.term> *
    sks: (Formulas.formula<Fol.fol> * Fol.term) list * k: 'a ->
      Lcf.ProofSystem.thm

val lcffol: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm


module FolAutomReas.Tactics

type goals =
    | Goals of
      ((string * Formulas.formula<Fol.fol>) list * Formulas.formula<Fol.fol>) list *
      (Lcf.ProofSystem.thm list -> Lcf.ProofSystem.thm)

val fprint_goal: sw: System.IO.TextWriter -> (goals -> unit)

val inline print_goal: g: goals -> unit

val inline sprint_goal: g: goals -> string

val set_goal: p: Formulas.formula<Fol.fol> -> goals

val extract_thm: gls: goals -> Lcf.ProofSystem.thm

val tac_proof: g: goals -> prf: (goals -> goals) list -> Lcf.ProofSystem.thm

val prove:
  p: Formulas.formula<Fol.fol> ->
    prf: (goals -> goals) list -> Lcf.ProofSystem.thm

val conj_intro_tac: goals -> goals

val jmodify: jfn: ('a list -> 'b) -> tfn: ('a -> 'a) -> 'a list -> 'b

val gen_right_alpha:
  y: string -> x: string -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val forall_intro_tac: y: string -> goals -> goals

val right_exists:
  x: string ->
    t: Fol.term -> p: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val exists_intro_tac: t: Fol.term -> goals -> goals

val imp_intro_tac: s: string -> goals -> goals

val assumptate: goals -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val firstassum:
  asl: ('a * Formulas.formula<Fol.fol>) list -> Lcf.ProofSystem.thm
    when 'a: equality

val using:
  ths: Lcf.ProofSystem.thm list -> p: 'a -> g: goals -> Lcf.ProofSystem.thm list

val assumps:
  asl: ('a * Formulas.formula<Fol.fol>) list -> ('a * Lcf.ProofSystem.thm) list

val by: hyps: string list -> p: 'a -> goals -> Lcf.ProofSystem.thm list

val justify:
  byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> p: Formulas.formula<Fol.fol> -> g: goals -> Lcf.ProofSystem.thm

val proof:
  tacs: (goals -> goals) list ->
    p: Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list

val at: once: 'a -> p: 'b -> gl: 'c -> 'd list

val once: 'a list

val auto_tac:
  byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val lemma_tac:
  s: string ->
    p: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val exists_elim_tac:
  l: string ->
    fm: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val ante_disj:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val disj_elim_tac:
  l: string ->
    fm: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val multishunt: i: int -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val assume: lps: (string * Formulas.formula<Fol.fol>) list -> goals -> goals

val note:
  l: string * p: Formulas.formula<Fol.fol> ->
    (('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
       'a -> goals -> goals)

val have:
  p: Formulas.formula<Fol.fol> ->
    (('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
       'a -> goals -> goals)

val so:
  tac: ('a -> ('b -> 'c -> goals -> Lcf.ProofSystem.thm list) -> 'd) ->
    arg: 'a -> byfn: ('b -> 'c -> goals -> Lcf.ProofSystem.thm list) -> 'd

val fix: (string -> goals -> goals)

val consider:
  x: string * p: Formulas.formula<Fol.fol> ->
    (('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
       'a -> goals -> goals)

val take: (Fol.term -> goals -> goals)

val cases:
  fm: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> g: goals -> goals

val conclude:
  p: Formulas.formula<Fol.fol> ->
    byfn: ('a -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'a -> gl: goals -> goals

val our:
  thesis: 'a ->
    byfn: ('b -> Formulas.formula<Fol.fol> -> goals -> Lcf.ProofSystem.thm list) ->
    hyps: 'b -> gl: goals -> goals

val thesis: string

val qed: gl: goals -> goals

val test001: n: 'a -> (Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm)

val double_th: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val testcase: n: int -> Lcf.ProofSystem.thm

val test002: n: int -> Lcf.ProofSystem.thm * Formulas.formula<Fol.fol>


module FolAutomReas.Limitations

val numeral: n: Lib.Num.Num -> Fol.term

val number: s: string -> Lib.Num.Num

val pair: x: Lib.Num.Num -> y: Lib.Num.Num -> Lib.Num.Num

val gterm: tm: Fol.term -> Lib.Num.Num

val gform: fm: Formulas.formula<Fol.fol> -> Lib.Num.Num

val gnumeral: n: Lib.Num.Num -> Lib.Num.Num

val diag001: s: string -> string

val phi001: string

val qdiag001: s: 'a -> (string -> string -> string)

val phi002: (string -> string -> string)

val diag002:
  x: string -> p: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val qdiag002:
  x: string -> p: Formulas.formula<Fol.fol> -> Formulas.formula<Fol.fol>

val dtermval: v: Lib.FPF.func<string,Lib.Num.Num> -> tm: Fol.term -> Lib.Num.Num

val dholds:
  v: Lib.FPF.func<string,Lib.Num.Num> -> fm: Formulas.formula<Fol.fol> -> bool

val dhquant:
  pred: ((Lib.Num.Num -> bool) -> Lib.Num.Num list -> bool) ->
    v: Lib.FPF.func<string,Lib.Num.Num> ->
    x: string ->
    y: string ->
    a: string -> t: Fol.term -> p: Formulas.formula<Fol.fol> -> bool

type formulaclass =
    | Sigma
    | Pi
    | Delta

val opp: _arg1: formulaclass -> formulaclass

val classify: c: formulaclass -> n: int -> fm: Formulas.formula<Fol.fol> -> bool

val veref:
  sign: (bool -> bool) ->
    m: Lib.Num.num ->
    v: Lib.FPF.func<string,Lib.Num.Num> -> fm: Formulas.formula<Fol.fol> -> bool

val verefboundquant:
  m: Lib.Num.num ->
    v: Lib.FPF.func<string,Lib.Num.Num> ->
    x: string ->
    y: string ->
    a: string ->
    t: Fol.term -> sign: (bool -> bool) -> p: Formulas.formula<Fol.fol> -> bool

val sholds:
  (Lib.Num.num ->
     Lib.FPF.func<string,Lib.Num.Num> -> Formulas.formula<Fol.fol> -> bool)

val sigma_bound: fm: Formulas.formula<Fol.fol> -> Lib.Num.Num

type symbol =
    | Blank
    | One

type direction =
    | Left
    | Right
    | Stay

type tape = | Tape of int * Lib.FPF.func<int,symbol>

val look: tape -> symbol

val write: s: symbol -> tape -> tape

val move: dir: direction -> tape -> tape

type config = | Config of int * tape

val run:
  prog: Lib.FPF.func<(int * symbol),(symbol * direction * int)> ->
    config: config -> config

val input_tape: (int list -> tape)

val output_tape: tape: tape -> int

val exec:
  prog: Lib.FPF.func<(int * symbol),(symbol * direction * int)> ->
    args: int list -> int

val robinson: Formulas.formula<Fol.fol>

val suc_inj: Lcf.ProofSystem.thm

val num_cases: Lcf.ProofSystem.thm

val mul_suc: Lcf.ProofSystem.thm

val mul_0: Lcf.ProofSystem.thm

val lt_def: Lcf.ProofSystem.thm

val le_def: Lcf.ProofSystem.thm

val add_suc: Lcf.ProofSystem.thm

val add_0: Lcf.ProofSystem.thm

val right_spec: t: Fol.term -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_mp:
  ith: Lcf.ProofSystem.thm -> th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_imp_trans:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_sym: th: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val right_trans:
  th1: Lcf.ProofSystem.thm -> th2: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val robop: tm: Fol.term -> Lcf.ProofSystem.thm

val robeval: tm: Fol.term -> Lcf.ProofSystem.thm

val robinson_consequences: Formulas.formula<Fol.fol>

val robinson_thm: Lcf.ProofSystem.thm

val suc_inj_false: Lcf.ProofSystem.thm

val suc_0_r: Lcf.ProofSystem.thm

val suc_0_l: Lcf.ProofSystem.thm

val num_lecases: Lcf.ProofSystem.thm

val lt_suc: Lcf.ProofSystem.thm

val lt_0: Lcf.ProofSystem.thm

val le_suc: Lcf.ProofSystem.thm

val le_0: Lcf.ProofSystem.thm

val expand_nlt: Lcf.ProofSystem.thm

val expand_nle: Lcf.ProofSystem.thm

val expand_lt: Lcf.ProofSystem.thm

val expand_le: Lcf.ProofSystem.thm

val rob_eq: s: Fol.term -> t: Fol.term -> Lcf.ProofSystem.thm

val rob_nen: s: Fol.term * t: Fol.term -> Lcf.ProofSystem.thm

val rob_ne: s: Fol.term -> t: Fol.term -> Lcf.ProofSystem.thm

val introduce_connective: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val elim_bex: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val sigma_elim: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val boundquant_step:
  th0: Lcf.ProofSystem.thm -> th1: Lcf.ProofSystem.thm -> Lcf.ProofSystem.thm

val sigma_prove: fm: Formulas.formula<Fol.fol> -> Lcf.ProofSystem.thm

val bounded_prove:
  a: string * x: string * t: Fol.term * q: Formulas.formula<Fol.fol> ->
    Lcf.ProofSystem.thm

val boundednum_prove:
  a: string * x: string * t: Fol.term * q: Formulas.formula<Fol.fol> ->
    Lcf.ProofSystem.thm
