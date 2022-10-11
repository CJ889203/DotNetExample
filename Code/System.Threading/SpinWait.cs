// Decompiled with JetBrains decompiler
// Type: System.Threading.SpinWait
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Provides support for spin-based waiting.</summary>
  public struct SpinWait
  {
    internal static readonly int SpinCountforSpinBeforeWait = Environment.IsSingleProcessor ? 1 : 35;
    private int _count;

    /// <summary>Gets the number of times <see cref="M:System.Threading.SpinWait.SpinOnce" /> has been called on this instance.</summary>
    /// <returns>Returns an integer that represents the number of times <see cref="M:System.Threading.SpinWait.SpinOnce" /> has been called on this instance.</returns>
    public int Count
    {
      get => this._count;
      internal set => this._count = value;
    }

    /// <summary>Gets whether the next call to <see cref="M:System.Threading.SpinWait.SpinOnce" /> will yield the processor, triggering a forced context switch.</summary>
    /// <returns>Whether the next call to <see cref="M:System.Threading.SpinWait.SpinOnce" /> will yield the processor, triggering a forced context switch.</returns>
    public bool NextSpinWillYield => this._count >= 10 || Environment.IsSingleProcessor;

    /// <summary>Performs a single spin.</summary>
    public void SpinOnce() => this.SpinOnceCore(20);

    /// <summary>Performs a single spin and calls <see cref="M:System.Threading.Thread.Sleep(System.Int32)" /> after a minimum spin count.</summary>
    /// <param name="sleep1Threshold">A minimum spin count after which <see langword="Thread.Sleep(1)" /> may be used. A value of -1 disables the use of <see langword="Thread.Sleep(1)" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="sleep1Threshold" /> is less than -1.</exception>
    public void SpinOnce(int sleep1Threshold)
    {
      if (sleep1Threshold < -1)
        throw new ArgumentOutOfRangeException(nameof (sleep1Threshold), (object) sleep1Threshold, SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
      if (sleep1Threshold >= 0 && sleep1Threshold < 10)
        sleep1Threshold = 10;
      this.SpinOnceCore(sleep1Threshold);
    }

    private void SpinOnceCore(int sleep1Threshold)
    {
      if (this._count >= 10 && (this._count >= sleep1Threshold && sleep1Threshold >= 0 || (this._count - 10) % 2 == 0) || Environment.IsSingleProcessor)
      {
        if (this._count >= sleep1Threshold && sleep1Threshold >= 0)
          Thread.Sleep(1);
        else if ((this._count >= 10 ? (this._count - 10) / 2 : this._count) % 5 == 4)
          Thread.Sleep(0);
        else
          Thread.Yield();
      }
      else
      {
        int iterations = Thread.OptimalMaxSpinWaitsPerSpinIteration;
        if (this._count <= 30 && 1 << this._count < iterations)
          iterations = 1 << this._count;
        Thread.SpinWait(iterations);
      }
      this._count = this._count == int.MaxValue ? 10 : this._count + 1;
    }

    /// <summary>Resets the spin counter.</summary>
    public void Reset() => this._count = 0;

    /// <summary>Spins until the specified condition is satisfied.</summary>
    /// <param name="condition">A delegate to be executed over and over until it returns true.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
    public static void SpinUntil(Func<bool> condition) => SpinWait.SpinUntil(condition, -1);

    /// <summary>Spins until the specified condition is satisfied or until the specified timeout is expired.</summary>
    /// <param name="condition">A delegate to be executed over and over until it returns true.</param>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out -or- timeout is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the condition is satisfied within the timeout; otherwise, false.</returns>
    public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? SpinWait.SpinUntil(condition, (int) totalMilliseconds) : throw new ArgumentOutOfRangeException(nameof (timeout), (object) timeout, SR.SpinWait_SpinUntil_TimeoutWrong);
    }

    /// <summary>Spins until the specified condition is satisfied or until the specified timeout is expired.</summary>
    /// <param name="condition">A delegate to be executed over and over until it returns true.</param>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="condition" /> argument is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <returns>
    /// <see langword="true" /> if the condition is satisfied within the timeout; otherwise, false.</returns>
    public static bool SpinUntil(Func<bool> condition, int millisecondsTimeout)
    {
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), (object) millisecondsTimeout, SR.SpinWait_SpinUntil_TimeoutWrong);
      if (condition == null)
        throw new ArgumentNullException(nameof (condition), SR.SpinWait_SpinUntil_ArgumentNull);
      uint num = 0;
      if (millisecondsTimeout != 0 && millisecondsTimeout != -1)
        num = TimeoutHelper.GetTime();
      SpinWait spinWait = new SpinWait();
      while (!condition())
      {
        if (millisecondsTimeout == 0)
          return false;
        spinWait.SpinOnce();
        if (millisecondsTimeout != -1 && spinWait.NextSpinWillYield && (long) millisecondsTimeout <= (long) (TimeoutHelper.GetTime() - num))
          return false;
      }
      return true;
    }
  }
}
