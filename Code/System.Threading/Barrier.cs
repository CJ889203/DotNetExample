// Decompiled with JetBrains decompiler
// Type: System.Threading.Barrier
// Assembly: System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3EED92AD-A1EE-4F59-AFCF-58DB2345788A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics;
using System.Runtime.Versioning;


#nullable enable
namespace System.Threading
{
  /// <summary>Enables multiple tasks to cooperatively work on an algorithm in parallel through multiple phases.</summary>
  [DebuggerDisplay("Participant Count={ParticipantCount},Participants Remaining={ParticipantsRemaining}")]
  public class Barrier : IDisposable
  {
    private volatile int _currentTotalCount;
    private long _currentPhase;
    private bool _disposed;

    #nullable disable
    private readonly ManualResetEventSlim _oddEvent;
    private readonly ManualResetEventSlim _evenEvent;
    private readonly ExecutionContext _ownerThreadContext;
    private static ContextCallback s_invokePostPhaseAction;
    private readonly Action<Barrier> _postPhaseAction;
    private Exception _exception;
    private int _actionCallerID;

    /// <summary>Gets the number of participants in the barrier that haven't yet signaled in the current phase.</summary>
    /// <returns>Returns the number of participants in the barrier that haven't yet signaled in the current phase.</returns>
    public int ParticipantsRemaining
    {
      get
      {
        int currentTotalCount = this._currentTotalCount;
        return (currentTotalCount & (int) short.MaxValue) - ((currentTotalCount & 2147418112) >> 16);
      }
    }

    /// <summary>Gets the total number of participants in the barrier.</summary>
    /// <returns>Returns the total number of participants in the barrier.</returns>
    public int ParticipantCount => this._currentTotalCount & (int) short.MaxValue;

    /// <summary>Gets the number of the barrier's current phase.</summary>
    /// <returns>Returns the number of the barrier's current phase.</returns>
    public long CurrentPhaseNumber
    {
      get => Volatile.Read(ref this._currentPhase);
      internal set => Volatile.Write(ref this._currentPhase, value);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Barrier" /> class.</summary>
    /// <param name="participantCount">The number of participating threads.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="participantCount" /> is less than 0 or greater than 32,767.</exception>
    public Barrier(int participantCount)
      : this(participantCount, (Action<Barrier>) null)
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Barrier" /> class.</summary>
    /// <param name="participantCount">The number of participating threads.</param>
    /// <param name="postPhaseAction">The <see cref="T:System.Action`1" /> to be executed after each phase. null (Nothing in Visual Basic) may be passed to indicate no action is taken.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="participantCount" /> is less than 0 or greater than 32,767.</exception>
    public Barrier(int participantCount, Action<Barrier>? postPhaseAction)
    {
      this._currentTotalCount = participantCount >= 0 && participantCount <= (int) short.MaxValue ? participantCount : throw new ArgumentOutOfRangeException(nameof (participantCount), (object) participantCount, SR.Barrier_ctor_ArgumentOutOfRange);
      this._postPhaseAction = postPhaseAction;
      this._oddEvent = new ManualResetEventSlim(true);
      this._evenEvent = new ManualResetEventSlim(false);
      if (postPhaseAction != null)
        this._ownerThreadContext = ExecutionContext.Capture();
      this._actionCallerID = 0;
    }


    #nullable disable
    private void GetCurrentTotal(int currentTotal, out int current, out int total, out bool sense)
    {
      total = currentTotal & (int) short.MaxValue;
      current = (currentTotal & 2147418112) >> 16;
      sense = (currentTotal & int.MinValue) == 0;
    }

    private bool SetCurrentTotal(int currentTotal, int current, int total, bool sense)
    {
      int num = current << 16 | total;
      if (!sense)
        num |= int.MinValue;
      return Interlocked.CompareExchange(ref this._currentTotalCount, num, currentTotal) == currentTotal;
    }

    /// <summary>Notifies the <see cref="T:System.Threading.Barrier" /> that there will be an additional participant.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">Adding a participant would cause the barrier's participant count to exceed 32,767.
    /// 
    /// -or-
    /// 
    /// The method was invoked from within a post-phase action.</exception>
    /// <returns>The phase number of the barrier in which the new participants will first participate.</returns>
    [UnsupportedOSPlatform("browser")]
    public long AddParticipant()
    {
      try
      {
        return this.AddParticipants(1);
      }
      catch (ArgumentOutOfRangeException ex)
      {
        throw new InvalidOperationException(SR.Barrier_AddParticipants_Overflow_ArgumentOutOfRange);
      }
    }

    /// <summary>Notifies the <see cref="T:System.Threading.Barrier" /> that there will be additional participants.</summary>
    /// <param name="participantCount">The number of additional participants to add to the barrier.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="participantCount" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// Adding <paramref name="participantCount" /> participants would cause the barrier's participant count to exceed 32,767.</exception>
    /// <exception cref="T:System.InvalidOperationException">The method was invoked from within a post-phase action.</exception>
    /// <returns>The phase number of the barrier in which the new participants will first participate.</returns>
    [UnsupportedOSPlatform("browser")]
    public long AddParticipants(int participantCount)
    {
      this.ThrowIfDisposed();
      if (participantCount < 1)
        throw new ArgumentOutOfRangeException(nameof (participantCount), (object) participantCount, SR.Barrier_AddParticipants_NonPositive_ArgumentOutOfRange);
      if (participantCount > (int) short.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (participantCount), SR.Barrier_AddParticipants_Overflow_ArgumentOutOfRange);
      if (this._actionCallerID != 0 && Environment.CurrentManagedThreadId == this._actionCallerID)
        throw new InvalidOperationException(SR.Barrier_InvalidOperation_CalledFromPHA);
      SpinWait spinWait = new SpinWait();
      bool sense;
      while (true)
      {
        int currentTotalCount = this._currentTotalCount;
        int current;
        int total;
        this.GetCurrentTotal(currentTotalCount, out current, out total, out sense);
        if (participantCount + total <= (int) short.MaxValue)
        {
          if (!this.SetCurrentTotal(currentTotalCount, current, total + participantCount, sense))
            spinWait.SpinOnce(-1);
          else
            goto label_10;
        }
        else
          break;
      }
      throw new ArgumentOutOfRangeException(nameof (participantCount), SR.Barrier_AddParticipants_Overflow_ArgumentOutOfRange);
label_10:
      long currentPhaseNumber = this.CurrentPhaseNumber;
      long num = sense != (currentPhaseNumber % 2L == 0L) ? currentPhaseNumber + 1L : currentPhaseNumber;
      if (num != currentPhaseNumber)
      {
        if (sense)
          this._oddEvent.Wait();
        else
          this._evenEvent.Wait();
      }
      else if (sense && this._evenEvent.IsSet)
        this._evenEvent.Reset();
      else if (!sense && this._oddEvent.IsSet)
        this._oddEvent.Reset();
      return num;
    }

    /// <summary>Notifies the <see cref="T:System.Threading.Barrier" /> that there will be one less participant.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The barrier already has 0 participants.
    /// 
    /// -or-
    /// 
    /// The method was invoked from within a post-phase action.</exception>
    public void RemoveParticipant() => this.RemoveParticipants(1);

    /// <summary>Notifies the <see cref="T:System.Threading.Barrier" /> that there will be fewer participants.</summary>
    /// <param name="participantCount">The number of additional participants to remove from the barrier.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The total participant count is less than the specified <paramref name="participantCount" /></exception>
    /// <exception cref="T:System.InvalidOperationException">The barrier already has 0 participants.
    /// 
    /// -or-
    /// 
    /// The method was invoked from within a post-phase action.
    /// 
    /// -or-
    /// 
    /// The current participant count is less than the specified participantCount.</exception>
    public void RemoveParticipants(int participantCount)
    {
      this.ThrowIfDisposed();
      if (participantCount < 1)
        throw new ArgumentOutOfRangeException(nameof (participantCount), (object) participantCount, SR.Barrier_RemoveParticipants_NonPositive_ArgumentOutOfRange);
      if (this._actionCallerID != 0 && Environment.CurrentManagedThreadId == this._actionCallerID)
        throw new InvalidOperationException(SR.Barrier_InvalidOperation_CalledFromPHA);
      SpinWait spinWait = new SpinWait();
      bool sense;
      while (true)
      {
        int currentTotalCount = this._currentTotalCount;
        int current;
        int total;
        this.GetCurrentTotal(currentTotalCount, out current, out total, out sense);
        if (total >= participantCount)
        {
          if (total - participantCount >= current)
          {
            int num = total - participantCount;
            if (num > 0 && current == num)
            {
              if (this.SetCurrentTotal(currentTotalCount, 0, total - participantCount, !sense))
                goto label_11;
            }
            else if (this.SetCurrentTotal(currentTotalCount, current, total - participantCount, sense))
              goto label_14;
            spinWait.SpinOnce(-1);
          }
          else
            goto label_8;
        }
        else
          break;
      }
      throw new ArgumentOutOfRangeException(nameof (participantCount), SR.Barrier_RemoveParticipants_ArgumentOutOfRange);
label_8:
      throw new InvalidOperationException(SR.Barrier_RemoveParticipants_InvalidOperation);
label_11:
      this.FinishPhase(sense);
      return;
label_14:;
    }

    /// <summary>Signals that a participant has reached the barrier and waits for all other participants to reach the barrier as well.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The method was invoked from within a post-phase action, the barrier currently has 0 participants, or the barrier is signaled by more threads than are registered as participants.</exception>
    /// <exception cref="T:System.Threading.BarrierPostPhaseException">If an exception is thrown from the post phase action of a Barrier after all participating threads have called SignalAndWait, the exception will be wrapped in a BarrierPostPhaseException and be thrown on all participating threads.</exception>
    [UnsupportedOSPlatform("browser")]
    public void SignalAndWait() => this.SignalAndWait(CancellationToken.None);

    /// <summary>Signals that a participant has reached the barrier and waits for all other participants to reach the barrier, while observing a cancellation token.</summary>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The method was invoked from within a post-phase action, the barrier currently has 0 participants, or the barrier is signaled by more threads than are registered as participants.</exception>
    [UnsupportedOSPlatform("browser")]
    public void SignalAndWait(CancellationToken cancellationToken) => this.SignalAndWait(-1, cancellationToken);

    /// <summary>Signals that a participant has reached the barrier and waits for all other participants to reach the barrier as well, using a <see cref="T:System.TimeSpan" /> object to measure the time interval.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out, or it is greater than 32,767.</exception>
    /// <exception cref="T:System.InvalidOperationException">The method was invoked from within a post-phase action, the barrier currently has 0 participants, or the barrier is signaled by more threads than are registered as participants.</exception>
    /// <returns>
    /// <see langword="true" /> if all other participants reached the barrier; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool SignalAndWait(TimeSpan timeout) => this.SignalAndWait(timeout, CancellationToken.None);

    /// <summary>Signals that a participant has reached the barrier and waits for all other participants to reach the barrier as well, using a <see cref="T:System.TimeSpan" /> object to measure the time interval, while observing a cancellation token.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, or a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> is a negative number other than -1 milliseconds, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.InvalidOperationException">The method was invoked from within a post-phase action, the barrier currently has 0 participants, or the barrier is signaled by more threads than are registered as participants.</exception>
    /// <returns>
    /// <see langword="true" /> if all other participants reached the barrier; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool SignalAndWait(TimeSpan timeout, CancellationToken cancellationToken)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (timeout), (object) timeout, SR.Barrier_SignalAndWait_ArgumentOutOfRange);
      return this.SignalAndWait((int) timeout.TotalMilliseconds, cancellationToken);
    }

    /// <summary>Signals that a participant has reached the barrier and waits for all other participants to reach the barrier as well, using a 32-bit signed integer to measure the timeout.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.InvalidOperationException">The method was invoked from within a post-phase action, the barrier currently has 0 participants, or the barrier is signaled by more threads than are registered as participants.</exception>
    /// <exception cref="T:System.Threading.BarrierPostPhaseException">If an exception is thrown from the post phase action of a Barrier after all participating threads have called SignalAndWait, the exception will be wrapped in a BarrierPostPhaseException and be thrown on all participating threads.</exception>
    /// <returns>
    /// <see langword="true" /> if all participants reached the barrier within the specified time; otherwise <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool SignalAndWait(int millisecondsTimeout) => this.SignalAndWait(millisecondsTimeout, CancellationToken.None);

    /// <summary>Signals that a participant has reached the barrier and waits for all other participants to reach the barrier as well, using a 32-bit signed integer to measure the timeout, while observing a cancellation token.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> has been canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <exception cref="T:System.InvalidOperationException">The method was invoked from within a post-phase action, the barrier currently has 0 participants, or the barrier is signaled by more threads than are registered as participants.</exception>
    /// <returns>
    /// <see langword="true" /> if all participants reached the barrier within the specified time; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool SignalAndWait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      this.ThrowIfDisposed();
      cancellationToken.ThrowIfCancellationRequested();
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), (object) millisecondsTimeout, SR.Barrier_SignalAndWait_ArgumentOutOfRange);
      if (this._actionCallerID != 0 && Environment.CurrentManagedThreadId == this._actionCallerID)
        throw new InvalidOperationException(SR.Barrier_InvalidOperation_CalledFromPHA);
      SpinWait spinWait = new SpinWait();
      int current;
      int total;
      bool sense1;
      long currentPhaseNumber;
      while (true)
      {
        int currentTotalCount = this._currentTotalCount;
        this.GetCurrentTotal(currentTotalCount, out current, out total, out sense1);
        currentPhaseNumber = this.CurrentPhaseNumber;
        if (total != 0)
        {
          if (current != 0 || sense1 == (this.CurrentPhaseNumber % 2L == 0L))
          {
            if (current + 1 == total)
            {
              if (this.SetCurrentTotal(currentTotalCount, 0, total, !sense1))
                goto label_11;
            }
            else if (this.SetCurrentTotal(currentTotalCount, current + 1, total, sense1))
              goto label_16;
            spinWait.SpinOnce(-1);
          }
          else
            goto label_8;
        }
        else
          break;
      }
      throw new InvalidOperationException(SR.Barrier_SignalAndWait_InvalidOperation_ZeroTotal);
label_8:
      throw new InvalidOperationException(SR.Barrier_SignalAndWait_InvalidOperation_ThreadsExceeded);
label_11:
      if (CdsSyncEtwBCLProvider.Log.IsEnabled())
        CdsSyncEtwBCLProvider.Log.Barrier_PhaseFinished(sense1, this.CurrentPhaseNumber);
      this.FinishPhase(sense1);
      return true;
label_16:
      ManualResetEventSlim currentPhaseEvent = sense1 ? this._evenEvent : this._oddEvent;
      bool flag1 = false;
      bool flag2 = false;
      try
      {
        flag2 = this.DiscontinuousWait(currentPhaseEvent, millisecondsTimeout, cancellationToken, currentPhaseNumber);
      }
      catch (OperationCanceledException ex)
      {
        flag1 = true;
      }
      catch (ObjectDisposedException ex)
      {
        if (currentPhaseNumber < this.CurrentPhaseNumber)
          flag2 = true;
        else
          throw;
      }
      if (!flag2)
      {
        spinWait.Reset();
        while (true)
        {
          int currentTotalCount = this._currentTotalCount;
          bool sense2;
          this.GetCurrentTotal(currentTotalCount, out current, out total, out sense2);
          if (currentPhaseNumber >= this.CurrentPhaseNumber && sense1 == sense2)
          {
            if (!this.SetCurrentTotal(currentTotalCount, current - 1, total, sense1))
              spinWait.SpinOnce(-1);
            else
              goto label_27;
          }
          else
            break;
        }
        this.WaitCurrentPhase(currentPhaseEvent, currentPhaseNumber);
        goto label_31;
label_27:
        if (flag1)
          throw new OperationCanceledException(SR.Common_OperationCanceled, cancellationToken);
        return false;
      }
label_31:
      if (this._exception != null)
        throw new BarrierPostPhaseException(this._exception);
      return true;
    }

    private void FinishPhase(bool observedSense)
    {
      if (this._postPhaseAction != null)
      {
        try
        {
          this._actionCallerID = Environment.CurrentManagedThreadId;
          if (this._ownerThreadContext != null)
          {
            ContextCallback callback = Barrier.s_invokePostPhaseAction;
            if (callback == null)
              Barrier.s_invokePostPhaseAction = callback = new ContextCallback(Barrier.InvokePostPhaseAction);
            ExecutionContext.Run(this._ownerThreadContext, callback, (object) this);
          }
          else
            this._postPhaseAction(this);
          this._exception = (Exception) null;
        }
        catch (Exception ex)
        {
          this._exception = ex;
        }
        finally
        {
          this._actionCallerID = 0;
          this.SetResetEvents(observedSense);
          if (this._exception != null)
            throw new BarrierPostPhaseException(this._exception);
        }
      }
      else
        this.SetResetEvents(observedSense);
    }

    private static void InvokePostPhaseAction(object obj)
    {
      Barrier barrier = (Barrier) obj;
      barrier._postPhaseAction(barrier);
    }

    private void SetResetEvents(bool observedSense)
    {
      ++this.CurrentPhaseNumber;
      if (observedSense)
      {
        this._oddEvent.Reset();
        this._evenEvent.Set();
      }
      else
      {
        this._evenEvent.Reset();
        this._oddEvent.Set();
      }
    }

    private void WaitCurrentPhase(ManualResetEventSlim currentPhaseEvent, long observedPhase)
    {
      SpinWait spinWait = new SpinWait();
      while (!currentPhaseEvent.IsSet && this.CurrentPhaseNumber - observedPhase <= 1L)
        spinWait.SpinOnce();
    }

    [UnsupportedOSPlatform("browser")]
    private bool DiscontinuousWait(
      ManualResetEventSlim currentPhaseEvent,
      int totalTimeout,
      CancellationToken token,
      long observedPhase)
    {
      int val1 = 100;
      int val2 = 10000;
      while (observedPhase == this.CurrentPhaseNumber)
      {
        int millisecondsTimeout = totalTimeout == -1 ? val1 : Math.Min(val1, totalTimeout);
        if (currentPhaseEvent.Wait(millisecondsTimeout, token))
          return true;
        if (totalTimeout != -1)
        {
          totalTimeout -= millisecondsTimeout;
          if (totalTimeout <= 0)
            return false;
        }
        val1 = val1 >= val2 ? val2 : Math.Min(val1 << 1, val2);
      }
      this.WaitCurrentPhase(currentPhaseEvent, observedPhase);
      return true;
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.Barrier" /> class.</summary>
    /// <exception cref="T:System.InvalidOperationException">The method was invoked from within a post-phase action.</exception>
    public void Dispose()
    {
      if (this._actionCallerID != 0 && Environment.CurrentManagedThreadId == this._actionCallerID)
        throw new InvalidOperationException(SR.Barrier_InvalidOperation_CalledFromPHA);
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Threading.Barrier" />, and optionally releases the managed resources.</summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing)
      {
        this._oddEvent.Dispose();
        this._evenEvent.Dispose();
      }
      this._disposed = true;
    }

    private void ThrowIfDisposed()
    {
      if (this._disposed)
        throw new ObjectDisposedException(nameof (Barrier), SR.Barrier_Dispose);
    }
  }
}
