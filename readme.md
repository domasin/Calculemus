# Automated Reasoning in First Order Logic

This is a fork of the repository https://github.com/jack-pappas/fsharp-logic-examples, which in turn is the porting in F# of the code from **John Harrison's "[Handbook of Practical Logic and Automated Reasoning](https://www.cl.cam.ac.uk/~jrh13/atp/index.html)"**.

![nuget package workflow](https://github.com/domasin/FolAutomReas/actions/workflows/publish.yml/badge.svg)

## Fork main purposes

* to have a .Net Core version of the solution
* to have a [nuget package](https://www.nuget.org/packages/FolAutomReas)
* to have an [api documentation](https://domasin.github.io/FolAutomReas/reference/index.html) (in progress)

## Fork main changes

* The conversion to .NET Core itself.

* The `thm` type has been changed from a simple type abbreviation of `formula<fol>` to a discriminated union with a single private case `Theorem of formula<fol>` to ensure that it is impossible to create new theorems without going through the inference rules defined and thus introduce theorems inconsistent with expressions of the type

          let t : thm = False

* In conjunction with the above change, the `ProofOperators` module in `lcf` has been renamed to `ProofSystem` like the original OCaml module.

* For better understanding of the source code and more modular documentation, the lib file has been split into a series of modules organized together in the `FOL.lib`  (to maintain the reference to the original file) namespace.

* The name of the root namespace has been changed to FolAutomReas (Automated Reasoning in First Order Logic).

* Use of exception removed in `dp` and `dpll`, both for performance and because `Failure _`, in any case, should be replaced with `_` to work (since built-in functions, like `List.head`, are used instead of those redefined with custom exceptions, like `hd`).

* Unit tests with XUnit, FSUnit, FSCheck (in progress).

### Changes in source code not yet released

* Quotations symbols `<<>>` replaced with `` since the latter cause less problems in writing documentation. 

* Separation in .fs .fsi files for documentation: in progress.

* Num dependencies removed and replaced with standard bigint (2023-10-22)

