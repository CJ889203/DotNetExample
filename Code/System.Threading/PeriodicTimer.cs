// Decompiled with JetBrains decompiler
// Type: System.Threading.PeriodicTimer
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace System.Threading
{
  /// <summary>Provides a periodic timer that enables waiting asynchronously for timer ticks.</summary>
  public sealed class PeriodicTimer : IDisposable
  {
    private readonly TimerQueueTimer _timer;
    private readonly PeriodicTimer.State _state;

    /// <summary>Initializes the timer.</summary>
    /// <param name="period">The time interval in milliseconds between invocations of the callback.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="period" /> is less than or equal to 0, or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    public PeriodicTimer(TimeSpan period)
    {
      long totalMilliseconds = (long) period.TotalMilliseconds;
      if (totalMilliseconds < 1L || totalMilliseconds > 4294967294L)
      {
        GC.SuppressFinalize((object) this);
        throw new ArgumentOutOfRangeException(nameof (period));
      }
      this._state = new PeriodicTimer.State();
      this._timer = new TimerQueueTimer((TimerCallback) (s => ((PeriodicTimer.State) s).Signal()), (object) this._state, (uint) totalMilliseconds, (uint) totalMilliseconds, false);
    }

    /// <summary>Waits for the next tick of the timer, or for the timer to be stopped.</summary>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> for cancelling the asynchronous wait. If cancellation is requested, it affects only the single wait operation; the underlying timer continues firing.</param>
    /// <returns>A task that will be completed due to the timer firing, <see cref="M:System.Threading.PeriodicTimer.Dispose" /> being called to stop the timer, or cancellation being requested.</returns>
    public ValueTask<bool> WaitForNextTickAsync(CancellationToken cancellationToken = default (CancellationToken)) => this._state.WaitForNextTickAsync(this, cancellationToken);

    /// <summary>Stops the timer and releases the associated managed resources.</summary>
    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._timer.Close();
      this._state.Signal(true);
    }

    ~PeriodicTimer() => this.Dispose();

    private sealed class State : IValueTaskSource<bool>
    {
      private PeriodicTimer _owner;
      private ManualResetValueTaskSourceCore<bool> _mrvtsc;
      private CancellationTokenRegistration _ctr;
      private bool _stopped;
      private bool _signaled;
      private bool _activeWait;

      public ValueTask<bool> WaitForNextTickAsync(
        PeriodicTimer owner,
        CancellationToken cancellationToken1)
      {
        lock (this)
        {
          if (this._activeWait)
            ThrowHelper.ThrowInvalidOperationException();
          if (cancellationToken1.IsCancellationRequested)
            return ValueTask.FromCanceled<bool>(cancellationToken1);
          if (this._signaled)
          {
            if (!this._stopped)
              this._signaled = false;
            return new ValueTask<bool>(!this._stopped);
          }
          this._owner = owner;
          this._activeWait = true;
          this._ctr = cancellationToken1.UnsafeRegister((Action<object, CancellationToken>) ((state, cancellationToken2) => ((PeriodicTimer.State) state).Signal(cancellationToken: cancellationToken2)), (object) this);
          return new ValueTask<bool>((IValueTaskSource<bool>) this, this._mrvtsc.Version);
        }
      }

      public void Signal(bool stopping = false, CancellationToken cancellationToken = default (CancellationToken))
      {
        bool flag = false;
        lock (this)
        {
          this._stopped |= stopping;
          if (!this._signaled)
          {
            this._signaled = true;
            flag = this._activeWait;
          }
        }
        if (!flag)
          return;
        if (cancellationToken.IsCancellationRequested)
          this._mrvtsc.SetException(ExceptionDispatchInfo.SetCurrentStackTrace((Exception) new OperationCanceledException(cancellationToken)));
        else
          this._mrvtsc.SetResult(true);
      }

      bool IValueTaskSource<bool>.GetResult(short token)
      {
        this._ctr.Dispose();
        lock (this)
        {
          try
          {
            this._mrvtsc.GetResult(token);
          }
          finally
          {
            this._mrvtsc.Reset();
            this._ctr = new CancellationTokenRegistration();
            this._activeWait = false;
            this._owner = (PeriodicTimer) null;
            if (!this._stopped)
              this._signaled = false;
          }
          return !this._stopped;
        }
      }

      ValueTaskSourceStatus IValueTaskSource<bool>.GetStatus(
        short token)
      {
        return this._mrvtsc.GetStatus(token);
      }

      void IValueTaskSource<bool>.OnCompleted(
        Action<object> continuation,
        object state,
        short token,
        ValueTaskSourceOnCompletedFlags flags)
      {
        this._mrvtsc.OnCompleted(continuation, state, token, flags);
      }
    }
  }
}
