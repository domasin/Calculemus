// ========================================================================= //
// Copyright (c) 2023, Domenico Masini.                                      //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

namespace Calculemus.Lib

/// <summary>
/// Timing: useful for documentation but not logically necessary.
/// </summary>
/// 
/// <category index="7">Diagnostics</category>
module Time =

    /// <summary>
    /// Report CPU time taken by a function.
    /// </summary>
    /// 
    /// <remarks>
    /// A call <c>time f x</c> will evaluate <c>f x</c> as usual, but will also 
    /// print the CPU time taken by that function evaluation.
    /// <p>
    /// </p>
    /// Never fails in itself, though it propagates any exception generated by 
    /// the call <c>f x</c> itself.
    /// </remarks>
    /// 
    /// <param name="f">The input function.</param>
    /// <param name="x">The input argument for the function.</param>
    /// 
    /// <returns>
    /// The application <c>f x</c> plus the CPU time taken to evaluate it.
    /// </returns>
    /// 
    /// <example id="time-1">
    /// <code lang="fsharp">
    /// time tab p38
    /// </code>
    /// Evaluates to:
    /// <code lang="fsharp">
    /// Searching with depth limit 0
    /// Searching with depth limit 1
    /// Searching with depth limit 2
    /// Searching with depth limit 3
    /// Searching with depth limit 4
    /// CPU time (user): 0.088133
    /// </code>
    /// </example>
    val time: f: ('a -> 'b) -> x: 'a -> 'b