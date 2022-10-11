// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.ParallelLoopState
// Assembly: System.Threading.Tasks.Parallel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: CD664842-108A-425B-971D-793D618C3E3A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.Tasks.Parallel.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Tasks.Parallel.xml

using System.Diagnostics;

namespace System.Threading.Tasks
{
  /// <summary>Enables iterations of parallel loops to interact with other iterations. An instance of this class is provided by the <see cref="T:System.Threading.Tasks.Parallel" /> class to each loop; you can not create instances in your code.</summary>
  [DebuggerDisplay("ShouldExitCurrentIteration = {ShouldExitCurrentIteration}")]
  public class ParallelLoopState
  {
    private readonly ParallelLoopStateFlags _flagsBase;

    internal ParallelLoopState(ParallelLoopStateFlags fbase) => this._flagsBase = fbase;

    internal virtual bool InternalShouldExitCurrentIteration => throw new NotSupportedException(SR.ParallelState_NotSupportedException_UnsupportedMethod);

    /// <summary>Gets whether the current iteration of the loop should exit based on requests made by this or other iterations.</summary>
    /// <returns>
    /// <see langword="true" /> if the current iteration should exit; otherwise, <see langword="false" />.</returns>
    public bool ShouldExitCurrentIteration => this.InternalShouldExitCurrentIteration;

    /// <summary>Gets whether any iteration of the loop has called the <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> method.</summary>
    /// <returns>
    /// <see langword="true" /> if any iteration has stopped the loop by calling the <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> method; otherwise, <see langword="false" />.</returns>
    public bool IsStopped => (this._flagsBase.LoopStateFlags & 4) != 0;

    /// <summary>Gets whether any iteration of the loop has thrown an exception that went unhandled by that iteration.</summary>
    /// <returns>
    /// <see langword="true" /> if an unhandled exception was thrown; otherwise, <see langword="false" />.</returns>
    public bool IsExceptional => (this._flagsBase.LoopStateFlags & 1) != 0;

    internal virtual long? InternalLowestBreakIteration => throw new NotSupportedException(SR.ParallelState_NotSupportedException_UnsupportedMethod);

    /// <summary>Gets the lowest iteration of the loop from which <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> was called.</summary>
    /// <returns>The lowest iteration from which <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> was called. In the case of a <see cref="M:System.Threading.Tasks.Parallel.ForEach``1(System.Collections.Concurrent.Partitioner{``0},System.Action{``0})" /> loop, the value is based on an internally-generated index.</returns>
    public long? LowestBreakIteration => this.InternalLowestBreakIteration;

    /// <summary>Communicates that the <see cref="T:System.Threading.Tasks.Parallel" /> loop should cease execution at the system's earliest convenience.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> method was called previously. <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> and <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> may not be used in combination by iterations of the same loop.</exception>
    public void Stop() => this._flagsBase.Stop();

    internal virtual void InternalBreak() => throw new NotSupportedException(SR.ParallelState_NotSupportedException_UnsupportedMethod);

    /// <summary>Communicates that the <see cref="T:System.Threading.Tasks.Parallel" /> loop should cease execution of iterations beyond the current iteration at the system's earliest convenience.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> method was previously called. <see cref="M:System.Threading.Tasks.ParallelLoopState.Break" /> and <see cref="M:System.Threading.Tasks.ParallelLoopState.Stop" /> may not be used in combination by iterations of the same loop.</exception>
    public void Break() => this.InternalBreak();

    internal static void Break(int iteration, ParallelLoopStateFlags32 pflags)
    {
      int oldState = 0;
      if (!pflags.AtomicLoopStateUpdate(2, 13, ref oldState))
      {
        if ((oldState & 4) != 0)
          throw new InvalidOperationException(SR.ParallelState_Break_InvalidOperationException_BreakAfterStop);
      }
      else
      {
        int lowestBreakIteration = pflags._lowestBreakIteration;
        if (iteration >= lowestBreakIteration)
          return;
        SpinWait spinWait = new SpinWait();
        while (Interlocked.CompareExchange(ref pflags._lowestBreakIteration, iteration, lowestBreakIteration) != lowestBreakIteration)
        {
          spinWait.SpinOnce();
          lowestBreakIteration = pflags._lowestBreakIteration;
          if (iteration > lowestBreakIteration)
            break;
        }
      }
    }

    internal static void Break(long iteration, ParallelLoopStateFlags64 pflags)
    {
      int oldState = 0;
      if (!pflags.AtomicLoopStateUpdate(2, 13, ref oldState))
      {
        if ((oldState & 4) != 0)
          throw new InvalidOperationException(SR.ParallelState_Break_InvalidOperationException_BreakAfterStop);
      }
      else
      {
        long lowestBreakIteration = pflags.LowestBreakIteration;
        if (iteration >= lowestBreakIteration)
          return;
        SpinWait spinWait = new SpinWait();
        while (Interlocked.CompareExchange(ref pflags._lowestBreakIteration, iteration, lowestBreakIteration) != lowestBreakIteration)
        {
          spinWait.SpinOnce();
          lowestBreakIteration = pflags.LowestBreakIteration;
          if (iteration > lowestBreakIteration)
            break;
        }
      }
    }
  }
}
