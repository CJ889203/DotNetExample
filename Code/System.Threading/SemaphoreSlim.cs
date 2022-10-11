// Decompiled with JetBrains decompiler
// Type: System.Threading.SemaphoreSlim
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;


#nullable enable
namespace System.Threading
{
  /// <summary>Represents a lightweight alternative to <see cref="T:System.Threading.Semaphore" /> that limits the number of threads that can access a resource or pool of resources concurrently.</summary>
  [DebuggerDisplay("Current Count = {m_currentCount}")]
  public class SemaphoreSlim : IDisposable
  {
    private volatile int m_currentCount;
    private readonly int m_maxCount;
    private int m_waitCount;
    private int m_countOfWaitersPulsedToWake;

    #nullable disable
    private readonly StrongBox<bool> m_lockObjAndDisposed;
    private volatile ManualResetEvent m_waitHandle;
    private SemaphoreSlim.TaskNode m_asyncHead;
    private SemaphoreSlim.TaskNode m_asyncTail;
    private static readonly Action<object> s_cancellationTokenCanceledEventHandler = new Action<object>(SemaphoreSlim.CancellationTokenCanceledEventHandler);

    /// <summary>Gets the number of remaining threads that can enter the <see cref="T:System.Threading.SemaphoreSlim" /> object.</summary>
    /// <returns>The number of remaining threads that can enter the semaphore.</returns>
    public int CurrentCount => this.m_currentCount;


    #nullable enable
    /// <summary>Returns a <see cref="T:System.Threading.WaitHandle" /> that can be used to wait on the semaphore.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.SemaphoreSlim" /> has been disposed.</exception>
    /// <returns>A <see cref="T:System.Threading.WaitHandle" /> that can be used to wait on the semaphore.</returns>
    public WaitHandle AvailableWaitHandle
    {
      get
      {
        this.CheckDispose();
        if (this.m_waitHandle != null)
          return (WaitHandle) this.m_waitHandle;
        lock (this.m_lockObjAndDisposed)
        {
          if (this.m_waitHandle == null)
            this.m_waitHandle = new ManualResetEvent(this.m_currentCount != 0);
        }
        return (WaitHandle) this.m_waitHandle;
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.SemaphoreSlim" /> class, specifying the initial number of requests that can be granted concurrently.</summary>
    /// <param name="initialCount">The initial number of requests for the semaphore that can be granted concurrently.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="initialCount" /> is less than 0.</exception>
    public SemaphoreSlim(int initialCount)
      : this(initialCount, int.MaxValue)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.SemaphoreSlim" /> class, specifying the initial and maximum number of requests that can be granted concurrently.</summary>
    /// <param name="initialCount">The initial number of requests for the semaphore that can be granted concurrently.</param>
    /// <param name="maxCount">The maximum number of requests for the semaphore that can be granted concurrently.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="initialCount" /> is less than 0, or <paramref name="initialCount" /> is greater than <paramref name="maxCount" />, or <paramref name="maxCount" /> is equal to or less than 0.</exception>
    public SemaphoreSlim(int initialCount, int maxCount)
    {
      if (initialCount < 0 || initialCount > maxCount)
        throw new ArgumentOutOfRangeException(nameof (initialCount), (object) initialCount, SR.SemaphoreSlim_ctor_InitialCountWrong);
      this.m_maxCount = maxCount > 0 ? maxCount : throw new ArgumentOutOfRangeException(nameof (maxCount), (object) maxCount, SR.SemaphoreSlim_ctor_MaxCountWrong);
      this.m_currentCount = initialCount;
      this.m_lockObjAndDisposed = new StrongBox<bool>();
    }

    /// <summary>Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    [UnsupportedOSPlatform("browser")]
    public void Wait() => this.Wait(-1, CancellationToken.None);

    /// <summary>Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> token to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    [UnsupportedOSPlatform("browser")]
    public void Wait(CancellationToken cancellationToken) => this.Wait(-1, cancellationToken);

    /// <summary>Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a <see cref="T:System.TimeSpan" /> to specify the timeout.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely, or a <see cref="T:System.TimeSpan" /> that represents 0 milliseconds to test the wait handle and return immediately.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="timeout" /> is a negative number other than -1, which represents an infinite timeout.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The semaphoreSlim instance has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (timeout), (object) timeout, SR.SemaphoreSlim_Wait_TimeoutWrong);
      return this.Wait((int) timeout.TotalMilliseconds, CancellationToken.None);
    }

    /// <summary>Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a <see cref="T:System.TimeSpan" /> that specifies the timeout, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely, or a <see cref="T:System.TimeSpan" /> that represents 0 milliseconds to test the wait handle and return immediately.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="timeout" /> is a negative number other than -1, which represents an infinite timeout.
    /// 
    /// -or-.
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The semaphoreSlim instance has been disposed.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has already been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(TimeSpan timeout, CancellationToken cancellationToken)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (timeout), (object) timeout, SR.SemaphoreSlim_Wait_TimeoutWrong);
      return this.Wait((int) timeout.TotalMilliseconds, cancellationToken);
    }

    /// <summary>Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a 32-bit signed integer that specifies the timeout.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite timeout -or- timeout is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.SemaphoreSlim" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(int millisecondsTimeout) => this.Wait(millisecondsTimeout, CancellationToken.None);

    /// <summary>Blocks the current thread until it can enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a 32-bit signed integer that specifies the timeout, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, <see cref="F:System.Threading.Timeout.Infinite" />(-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite timeout.
    /// 
    /// -or-
    /// 
    /// <paramref name="millisecondsTimeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.SemaphoreSlim" /> instance has been disposed, or the <see cref="T:System.Threading.CancellationTokenSource" /> that created <paramref name="cancellationToken" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />; otherwise, <see langword="false" />.</returns>
    [UnsupportedOSPlatform("browser")]
    public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      this.CheckDispose();
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), (object) millisecondsTimeout, SR.SemaphoreSlim_Wait_TimeoutWrong);
      cancellationToken.ThrowIfCancellationRequested();
      if (millisecondsTimeout == 0 && this.m_currentCount == 0)
        return false;
      uint startTime = 0;
      if (millisecondsTimeout != -1 && millisecondsTimeout > 0)
        startTime = TimeoutHelper.GetTime();
      bool flag = false;
      Task<bool> task = (Task<bool>) null;
      bool lockTaken = false;
      CancellationTokenRegistration tokenRegistration = cancellationToken.UnsafeRegister(SemaphoreSlim.s_cancellationTokenCanceledEventHandler, (object) this);
      try
      {
        if (this.m_currentCount == 0)
        {
          int num = SpinWait.SpinCountforSpinBeforeWait * 4;
          SpinWait spinWait = new SpinWait();
          while (spinWait.Count < num)
          {
            spinWait.SpinOnce(-1);
            if (this.m_currentCount != 0)
              break;
          }
        }
        Monitor.Enter((object) this.m_lockObjAndDisposed, ref lockTaken);
        ++this.m_waitCount;
        if (this.m_asyncHead != null)
        {
          task = this.WaitAsync(millisecondsTimeout, cancellationToken);
        }
        else
        {
          OperationCanceledException canceledException = (OperationCanceledException) null;
          if (this.m_currentCount == 0)
          {
            if (millisecondsTimeout == 0)
              return false;
            try
            {
              flag = this.WaitUntilCountOrTimeout(millisecondsTimeout, startTime, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
              canceledException = ex;
            }
          }
          if (this.m_currentCount > 0)
          {
            flag = true;
            --this.m_currentCount;
          }
          else if (canceledException != null)
            throw canceledException;
          if (this.m_waitHandle != null)
          {
            if (this.m_currentCount == 0)
              this.m_waitHandle.Reset();
          }
        }
      }
      finally
      {
        if (lockTaken)
        {
          --this.m_waitCount;
          Monitor.Exit((object) this.m_lockObjAndDisposed);
        }
        tokenRegistration.Dispose();
      }
      if (task == null)
        return flag;
      return task.GetAwaiter().GetResult();
    }

    [UnsupportedOSPlatform("browser")]
    private bool WaitUntilCountOrTimeout(
      int millisecondsTimeout,
      uint startTime,
      CancellationToken cancellationToken)
    {
      int millisecondsTimeout1 = -1;
      while (this.m_currentCount == 0)
      {
        cancellationToken.ThrowIfCancellationRequested();
        if (millisecondsTimeout != -1)
        {
          millisecondsTimeout1 = TimeoutHelper.UpdateTimeOut(startTime, millisecondsTimeout);
          if (millisecondsTimeout1 <= 0)
            return false;
        }
        bool flag = Monitor.Wait((object) this.m_lockObjAndDisposed, millisecondsTimeout1);
        if (this.m_countOfWaitersPulsedToWake != 0)
          --this.m_countOfWaitersPulsedToWake;
        if (!flag)
          return false;
      }
      return true;
    }

    /// <summary>Asynchronously waits to enter the <see cref="T:System.Threading.SemaphoreSlim" />.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.SemaphoreSlim" /> has been disposed.</exception>
    /// <returns>A task that will complete when the semaphore has been entered.</returns>
    public Task WaitAsync() => (Task) this.WaitAsync(-1, new CancellationToken());

    /// <summary>Asynchronously waits to enter the <see cref="T:System.Threading.SemaphoreSlim" />, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> token to observe.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <returns>A task that will complete when the semaphore has been entered.</returns>
    public Task WaitAsync(CancellationToken cancellationToken) => (Task) this.WaitAsync(-1, cancellationToken);

    /// <summary>Asynchronously waits to enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a 32-bit signed integer to measure the time interval.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="millisecondsTimeout" /> is a negative number other than -1, which represents an infinite timeout.
    /// 
    /// -or-
    /// 
    /// <paramref name="millisecondsTimeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A task that will complete with a result of <see langword="true" /> if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />, otherwise with a result of <see langword="false" />.</returns>
    public Task<bool> WaitAsync(int millisecondsTimeout) => this.WaitAsync(millisecondsTimeout, new CancellationToken());

    /// <summary>Asynchronously waits to enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a <see cref="T:System.TimeSpan" /> to measure the time interval.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely, or a <see cref="T:System.TimeSpan" /> that represents 0 milliseconds to test the wait handle and return immediately.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="timeout" /> is a negative number other than -1, which represents an infinite timeout.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A task that will complete with a result of <see langword="true" /> if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />, otherwise with a result of <see langword="false" />.</returns>
    public Task<bool> WaitAsync(TimeSpan timeout) => this.WaitAsync(timeout, new CancellationToken());

    /// <summary>Asynchronously waits to enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a <see cref="T:System.TimeSpan" /> to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="timeout">A <see cref="T:System.TimeSpan" /> that represents the number of milliseconds to wait, a <see cref="T:System.TimeSpan" /> that represents -1 milliseconds to wait indefinitely, or a <see cref="T:System.TimeSpan" /> that represents 0 milliseconds to test the wait handle and return immediately.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> token to observe.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="timeout" /> is a negative number other than -1, which represents an infinite timeout.
    /// 
    /// -or-
    /// 
    /// <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.SemaphoreSlim" /> has been disposed.</exception>
    /// <returns>A task that will complete with a result of <see langword="true" /> if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />, otherwise with a result of <see langword="false" />.</returns>
    public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      if (totalMilliseconds < -1L || totalMilliseconds > (long) int.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (timeout), (object) timeout, SR.SemaphoreSlim_Wait_TimeoutWrong);
      return this.WaitAsync((int) timeout.TotalMilliseconds, cancellationToken);
    }

    /// <summary>Asynchronously waits to enter the <see cref="T:System.Threading.SemaphoreSlim" />, using a 32-bit signed integer to measure the time interval, while observing a <see cref="T:System.Threading.CancellationToken" />.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely, or zero to test the state of the wait handle and return immediately.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> to observe.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="millisecondsTimeout" /> is a number other than -1, which represents an infinite timeout.
    /// 
    /// -or-
    /// 
    /// <paramref name="millisecondsTimeout" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.OperationCanceledException">
    /// <paramref name="cancellationToken" /> was canceled.</exception>
    /// <returns>A task that will complete with a result of <see langword="true" /> if the current thread successfully entered the <see cref="T:System.Threading.SemaphoreSlim" />, otherwise with a result of <see langword="false" />.</returns>
    public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken)
    {
      this.CheckDispose();
      if (millisecondsTimeout < -1)
        throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), (object) millisecondsTimeout, SR.SemaphoreSlim_Wait_TimeoutWrong);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled<bool>(cancellationToken);
      lock (this.m_lockObjAndDisposed)
      {
        if (this.m_currentCount > 0)
        {
          --this.m_currentCount;
          if (this.m_waitHandle != null && this.m_currentCount == 0)
            this.m_waitHandle.Reset();
          return Task.FromResult<bool>(true);
        }
        if (millisecondsTimeout == 0)
          return Task.FromResult<bool>(false);
        SemaphoreSlim.TaskNode andAddAsyncWaiter = this.CreateAndAddAsyncWaiter();
        return millisecondsTimeout != -1 || cancellationToken.CanBeCanceled ? this.WaitUntilCountOrTimeoutAsync(andAddAsyncWaiter, millisecondsTimeout, cancellationToken) : (Task<bool>) andAddAsyncWaiter;
      }
    }


    #nullable disable
    private SemaphoreSlim.TaskNode CreateAndAddAsyncWaiter()
    {
      SemaphoreSlim.TaskNode andAddAsyncWaiter = new SemaphoreSlim.TaskNode();
      if (this.m_asyncHead == null)
      {
        this.m_asyncHead = andAddAsyncWaiter;
        this.m_asyncTail = andAddAsyncWaiter;
      }
      else
      {
        this.m_asyncTail.Next = andAddAsyncWaiter;
        andAddAsyncWaiter.Prev = this.m_asyncTail;
        this.m_asyncTail = andAddAsyncWaiter;
      }
      return andAddAsyncWaiter;
    }

    private bool RemoveAsyncWaiter(SemaphoreSlim.TaskNode task)
    {
      bool flag = this.m_asyncHead == task || task.Prev != null;
      if (task.Next != null)
        task.Next.Prev = task.Prev;
      if (task.Prev != null)
        task.Prev.Next = task.Next;
      if (this.m_asyncHead == task)
        this.m_asyncHead = task.Next;
      if (this.m_asyncTail == task)
        this.m_asyncTail = task.Prev;
      task.Next = task.Prev = (SemaphoreSlim.TaskNode) null;
      return flag;
    }

    private async Task<bool> WaitUntilCountOrTimeoutAsync(
      SemaphoreSlim.TaskNode asyncWaiter,
      int millisecondsTimeout,
      CancellationToken cancellationToken)
    {
      await new SemaphoreSlim.ConfiguredNoThrowAwaiter<bool>(asyncWaiter.WaitAsync(TimeSpan.FromMilliseconds((double) millisecondsTimeout), cancellationToken));
      if (cancellationToken.IsCancellationRequested)
        await TaskScheduler.Default;
      if (asyncWaiter.IsCompleted)
        return true;
      lock (this.m_lockObjAndDisposed)
      {
        if (this.RemoveAsyncWaiter(asyncWaiter))
        {
          cancellationToken.ThrowIfCancellationRequested();
          return false;
        }
      }
      return await asyncWaiter.ConfigureAwait(false);
    }

    /// <summary>Releases the <see cref="T:System.Threading.SemaphoreSlim" /> object once.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.Threading.SemaphoreFullException">The <see cref="T:System.Threading.SemaphoreSlim" /> has already reached its maximum size.</exception>
    /// <returns>The previous count of the <see cref="T:System.Threading.SemaphoreSlim" />.</returns>
    public int Release() => this.Release(1);

    /// <summary>Releases the <see cref="T:System.Threading.SemaphoreSlim" /> object a specified number of times.</summary>
    /// <param name="releaseCount">The number of times to exit the semaphore.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="releaseCount" /> is less than 1.</exception>
    /// <exception cref="T:System.Threading.SemaphoreFullException">The <see cref="T:System.Threading.SemaphoreSlim" /> has already reached its maximum size.</exception>
    /// <returns>The previous count of the <see cref="T:System.Threading.SemaphoreSlim" />.</returns>
    public int Release(int releaseCount)
    {
      this.CheckDispose();
      if (releaseCount < 1)
        throw new ArgumentOutOfRangeException(nameof (releaseCount), (object) releaseCount, SR.SemaphoreSlim_Release_CountWrong);
      int num1;
      lock (this.m_lockObjAndDisposed)
      {
        int currentCount = this.m_currentCount;
        num1 = currentCount;
        if (this.m_maxCount - currentCount < releaseCount)
          throw new SemaphoreFullException();
        int val1 = currentCount + releaseCount;
        int waitCount = this.m_waitCount;
        int num2 = Math.Min(val1, waitCount) - this.m_countOfWaitersPulsedToWake;
        if (num2 > 0)
        {
          if (num2 > releaseCount)
            num2 = releaseCount;
          this.m_countOfWaitersPulsedToWake += num2;
          for (int index = 0; index < num2; ++index)
            Monitor.Pulse((object) this.m_lockObjAndDisposed);
        }
        if (this.m_asyncHead != null)
        {
          int num3 = val1 - waitCount;
          while (num3 > 0 && this.m_asyncHead != null)
          {
            --val1;
            --num3;
            SemaphoreSlim.TaskNode asyncHead = this.m_asyncHead;
            this.RemoveAsyncWaiter(asyncHead);
            asyncHead.TrySetResult(true);
          }
        }
        this.m_currentCount = val1;
        if (this.m_waitHandle != null)
        {
          if (num1 == 0)
          {
            if (val1 > 0)
              this.m_waitHandle.Set();
          }
        }
      }
      return num1;
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.SemaphoreSlim" /> class.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.Threading.SemaphoreSlim" />, and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      WaitHandle waitHandle = (WaitHandle) this.m_waitHandle;
      if (waitHandle != null)
      {
        waitHandle.Dispose();
        this.m_waitHandle = (ManualResetEvent) null;
      }
      this.m_lockObjAndDisposed.Value = true;
      this.m_asyncHead = (SemaphoreSlim.TaskNode) null;
      this.m_asyncTail = (SemaphoreSlim.TaskNode) null;
    }

    private static void CancellationTokenCanceledEventHandler(object obj)
    {
      SemaphoreSlim semaphoreSlim = (SemaphoreSlim) obj;
      lock (semaphoreSlim.m_lockObjAndDisposed)
        Monitor.PulseAll((object) semaphoreSlim.m_lockObjAndDisposed);
    }

    private void CheckDispose()
    {
      if (this.m_lockObjAndDisposed.Value)
        throw new ObjectDisposedException((string) null, SR.SemaphoreSlim_Disposed);
    }

    private sealed class TaskNode : Task<bool>
    {
      internal SemaphoreSlim.TaskNode Prev;
      internal SemaphoreSlim.TaskNode Next;

      internal TaskNode()
        : base((object) null, TaskCreationOptions.RunContinuationsAsynchronously)
      {
      }
    }

    private readonly struct ConfiguredNoThrowAwaiter<T> : 
      ICriticalNotifyCompletion,
      INotifyCompletion
    {
      private readonly Task<T> _task;

      public ConfiguredNoThrowAwaiter(Task<T> task) => this._task = task;

      public SemaphoreSlim.ConfiguredNoThrowAwaiter<T> GetAwaiter() => this;

      public bool IsCompleted => this._task.IsCompleted;

      public void GetResult() => this._task.MarkExceptionsAsHandled();

      public void UnsafeOnCompleted(Action continuation) => this._task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(continuation);

      public void OnCompleted(Action continuation) => this._task.ConfigureAwait(false).GetAwaiter().OnCompleted(continuation);
    }
  }
}
