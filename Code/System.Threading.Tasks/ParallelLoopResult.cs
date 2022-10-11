// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.ParallelLoopResult
// Assembly: System.Threading.Tasks.Parallel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: CD664842-108A-425B-971D-793D618C3E3A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.Tasks.Parallel.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Tasks.Parallel.xml

namespace System.Threading.Tasks
{
  /// <summary>Provides completion status on the execution of a <see cref="T:System.Threading.Tasks.Parallel" /> loop.</summary>
  public struct ParallelLoopResult
  {
    internal bool _completed;
    internal long? _lowestBreakIteration;

    /// <summary>Gets whether the loop ran to completion, such that all iterations of the loop were executed and the loop didn't receive a request to end prematurely.</summary>
    /// <returns>
    /// <see langword="true" /> if the loop ran to completion; otherwise, <see langword="false" />.</returns>
    public bool IsCompleted => this._completed;

    /// <summary>Gets the index of the lowest iteration from which <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> was called.</summary>
    /// <returns>Returns an integer that represents the lowest iteration from which the Break statement was called.</returns>
    public long? LowestBreakIteration => this._lowestBreakIteration;
  }
}
