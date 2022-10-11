// Decompiled with JetBrains decompiler
// Type: System.Threading.ReaderWriterLock
// Assembly: System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3EED92AD-A1EE-4F59-AFCF-58DB2345788A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace System.Threading
{
  /// <summary>Defines a lock that supports single writers and multiple readers.</summary>
  public sealed class ReaderWriterLock : CriticalFinalizerObject
  {
    private static readonly int DefaultSpinCount = Environment.ProcessorCount != 1 ? 500 : 0;
    private static long s_mostRecentLockID;
    private ManualResetEventSlim _readerEvent;
    private AutoResetEvent _writerEvent;
    private readonly long _lockID;
    private volatile int _state;
    private int _writerID = -1;
    private int _writerSeqNum = 1;
    private ushort _writerLevel;

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.ReaderWriterLock" /> class.</summary>
    public ReaderWriterLock() => this._lockID = Interlocked.Increment(ref ReaderWriterLock.s_mostRecentLockID);

    /// <summary>Gets a value indicating whether the current thread holds a reader lock.</summary>
    /// <returns>
    /// <see langword="true" /> if the current thread holds a reader lock; otherwise, <see langword="false" />.</returns>
    public bool IsReaderLockHeld
    {
      get
      {
        ReaderWriterLock.ThreadLocalLockEntry current = ReaderWriterLock.ThreadLocalLockEntry.GetCurrent(this._lockID);
        return current != null && current._readerLevel > (ushort) 0;
      }
    }

    /// <summary>Gets a value indicating whether the current thread holds the writer lock.</summary>
    /// <returns>
    /// <see langword="true" /> if the current thread holds the writer lock; otherwise, <see langword="false" />.</returns>
    public bool IsWriterLockHeld => this._writerID == ReaderWriterLock.GetCurrentThreadID();

    /// <summary>Gets the current sequence number.</summary>
    /// <returns>The current sequence number.</returns>
    public int WriterSeqNum => this._writerSeqNum;

    /// <summary>Indicates whether the writer lock has been granted to any thread since the sequence number was obtained.</summary>
    /// <param name="seqNum">The sequence number.</param>
    /// <returns>
    /// <see langword="true" /> if the writer lock has been granted to any thread since the sequence number was obtained; otherwise, <see langword="false" />.</returns>
    public bool AnyWritersSince(int seqNum)
    {
      if (this._writerID == ReaderWriterLock.GetCurrentThreadID())
        ++seqNum;
      return (uint) this._writerSeqNum > (uint) seqNum;
    }

    /// <summary>Acquires a reader lock, using an <see cref="T:System.Int32" /> value for the time-out.</summary>
    /// <param name="millisecondsTimeout">The time-out in milliseconds.</param>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="millisecondsTimeout" /> expires before the lock request is granted.</exception>
    [UnsupportedOSPlatform("browser")]
    public void AcquireReaderLock(int millisecondsTimeout)
    {
      if (millisecondsTimeout < -1)
        throw ReaderWriterLock.GetInvalidTimeoutException(nameof (millisecondsTimeout));
      ReaderWriterLock.ThreadLocalLockEntry current = ReaderWriterLock.ThreadLocalLockEntry.GetOrCreateCurrent(this._lockID);
      if (Interlocked.CompareExchange(ref this._state, 1, 0) != 0)
      {
        if (current._readerLevel > (ushort) 0)
        {
          if (current._readerLevel == ushort.MaxValue)
            throw new OverflowException(SR.Overflow_UInt16);
          ++current._readerLevel;
          return;
        }
        if (this._writerID == ReaderWriterLock.GetCurrentThreadID())
        {
          this.AcquireWriterLock(millisecondsTimeout);
          return;
        }
        int num1 = 0;
        int num2 = this._state;
        do
        {
          int comparand = num2;
          if (comparand < 1023 || (comparand & 1024) != 0 && (comparand & 4096) == 0 && (comparand & 1023) + ((comparand & 8380416) >> 13) <= 1021)
          {
            num2 = Interlocked.CompareExchange(ref this._state, comparand + 1, comparand);
            if (num2 == comparand)
              break;
          }
          else if ((comparand & 1023) == 1023 || (comparand & 8380416) == 8380416 || (comparand & 3072) == 1024)
          {
            int millisecondsTimeout1 = 100;
            if ((comparand & 1023) == 1023 || (comparand & 8380416) == 8380416)
              millisecondsTimeout1 = 1000;
            Thread.Sleep(millisecondsTimeout1);
            num1 = 0;
            num2 = this._state;
          }
          else
          {
            ++num1;
            if ((comparand & 3072) == 3072)
            {
              if (num1 > ReaderWriterLock.DefaultSpinCount)
              {
                Thread.Sleep(1);
                num1 = 0;
              }
              num2 = this._state;
            }
            else if (num1 <= ReaderWriterLock.DefaultSpinCount)
            {
              num2 = this._state;
            }
            else
            {
              num2 = Interlocked.CompareExchange(ref this._state, comparand + 8192, comparand);
              if (num2 == comparand)
              {
                int num3 = -8192;
                ManualResetEventSlim manualResetEventSlim = (ManualResetEventSlim) null;
                bool flag = false;
                try
                {
                  manualResetEventSlim = this.GetOrCreateReaderEvent();
                  flag = manualResetEventSlim.Wait(millisecondsTimeout);
                  if (flag)
                    ++num3;
                }
                finally
                {
                  comparand = Interlocked.Add(ref this._state, num3) - num3;
                  if (!flag && (comparand & 1024) != 0 && (comparand & 8380416) == 8192)
                  {
                    if (manualResetEventSlim == null)
                      manualResetEventSlim = this._readerEvent;
                    manualResetEventSlim.Wait();
                    manualResetEventSlim.Reset();
                    Interlocked.Add(ref this._state, -1023);
                    ++current._readerLevel;
                    this.ReleaseReaderLock();
                  }
                }
                if (!flag)
                  throw ReaderWriterLock.GetTimeoutException();
                if ((comparand & 8380416) == 8192)
                {
                  manualResetEventSlim.Reset();
                  Interlocked.Add(ref this._state, -1024);
                  break;
                }
                break;
              }
            }
          }
        }
        while (ReaderWriterLock.YieldProcessor());
      }
      ++current._readerLevel;
    }

    /// <summary>Acquires a reader lock, using a <see cref="T:System.TimeSpan" /> value for the time-out.</summary>
    /// <param name="timeout">A <see langword="TimeSpan" /> specifying the time-out period.</param>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="timeout" /> expires before the lock request is granted.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> specifies a negative value other than -1 milliseconds.</exception>
    [UnsupportedOSPlatform("browser")]
    public void AcquireReaderLock(TimeSpan timeout) => this.AcquireReaderLock(ReaderWriterLock.ToTimeoutMilliseconds(timeout));

    /// <summary>Acquires the writer lock, using an <see cref="T:System.Int32" /> value for the time-out.</summary>
    /// <param name="millisecondsTimeout">The time-out in milliseconds.</param>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="timeout" /> expires before the lock request is granted.</exception>
    public void AcquireWriterLock(int millisecondsTimeout)
    {
      if (millisecondsTimeout < -1)
        throw ReaderWriterLock.GetInvalidTimeoutException(nameof (millisecondsTimeout));
      int currentThreadId = ReaderWriterLock.GetCurrentThreadID();
      if (Interlocked.CompareExchange(ref this._state, 4096, 0) != 0)
      {
        if (this._writerID == currentThreadId)
        {
          if (this._writerLevel == ushort.MaxValue)
            throw new OverflowException(SR.Overflow_UInt16);
          ++this._writerLevel;
          return;
        }
        int num1 = 0;
        int num2 = this._state;
        do
        {
          int comparand = num2;
          switch (comparand)
          {
            case 0:
            case 3072:
              num2 = Interlocked.CompareExchange(ref this._state, comparand + 4096, comparand);
              if (num2 != comparand)
                break;
              goto label_32;
            default:
              if ((comparand & -8388608) == -8388608)
              {
                Thread.Sleep(1000);
                num1 = 0;
                num2 = this._state;
                break;
              }
              ++num1;
              if ((comparand & 3072) == 3072)
              {
                if (num1 > ReaderWriterLock.DefaultSpinCount)
                {
                  Thread.Sleep(1);
                  num1 = 0;
                }
                num2 = this._state;
                break;
              }
              if (num1 <= ReaderWriterLock.DefaultSpinCount)
              {
                num2 = this._state;
                break;
              }
              num2 = Interlocked.CompareExchange(ref this._state, comparand + 8388608, comparand);
              if (num2 == comparand)
              {
                int num3 = -8388608;
                AutoResetEvent autoResetEvent = (AutoResetEvent) null;
                bool flag = false;
                try
                {
                  autoResetEvent = this.GetOrCreateWriterEvent();
                  flag = autoResetEvent.WaitOne(millisecondsTimeout);
                  if (flag)
                    num3 += 2048;
                }
                finally
                {
                  int num4 = Interlocked.Add(ref this._state, num3) - num3;
                  if (!flag && (num4 & 2048) != 0 && (num4 & -8388608) == 8388608)
                  {
                    if (autoResetEvent == null)
                      autoResetEvent = this._writerEvent;
                    do
                    {
                      int state = this._state;
                      if ((state & 2048) == 0 || (state & -8388608) != 0)
                        goto label_28;
                    }
                    while (!autoResetEvent.WaitOne(10));
                    int num5 = 2048;
                    int num6 = Interlocked.Add(ref this._state, num5) - num5;
                    this._writerID = currentThreadId;
                    this._writerLevel = (ushort) 1;
                    this.ReleaseWriterLock();
                  }
label_28:;
                }
                if (!flag)
                  throw ReaderWriterLock.GetTimeoutException();
                goto label_32;
              }
              else
                break;
          }
        }
        while (ReaderWriterLock.YieldProcessor());
      }
label_32:
      this._writerID = currentThreadId;
      this._writerLevel = (ushort) 1;
      ++this._writerSeqNum;
    }

    /// <summary>Acquires the writer lock, using a <see cref="T:System.TimeSpan" /> value for the time-out.</summary>
    /// <param name="timeout">The <see langword="TimeSpan" /> specifying the time-out period.</param>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="timeout" /> expires before the lock request is granted.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> specifies a negative value other than -1 milliseconds.</exception>
    public void AcquireWriterLock(TimeSpan timeout) => this.AcquireWriterLock(ReaderWriterLock.ToTimeoutMilliseconds(timeout));

    /// <summary>Decrements the lock count.</summary>
    /// <exception cref="T:System.ApplicationException">The thread does not have any reader or writer locks.</exception>
    public void ReleaseReaderLock()
    {
      if (this._writerID == ReaderWriterLock.GetCurrentThreadID())
      {
        this.ReleaseWriterLock();
      }
      else
      {
        ReaderWriterLock.ThreadLocalLockEntry current = ReaderWriterLock.ThreadLocalLockEntry.GetCurrent(this._lockID);
        if (current == null)
          throw ReaderWriterLock.GetNotOwnerException();
        --current._readerLevel;
        if (current._readerLevel > (ushort) 0)
          return;
        AutoResetEvent autoResetEvent = (AutoResetEvent) null;
        ManualResetEventSlim manualResetEventSlim = (ManualResetEventSlim) null;
        int num1 = this._state;
        bool flag1;
        bool flag2;
        int comparand;
        do
        {
          flag1 = false;
          flag2 = false;
          comparand = num1;
          int num2 = -1;
          if ((comparand & 2047) == 1)
          {
            flag1 = true;
            if ((comparand & -8388608) != 0)
            {
              autoResetEvent = this.TryGetOrCreateWriterEvent();
              if (autoResetEvent == null)
              {
                Thread.Sleep(100);
                num1 = this._state;
                comparand = 0;
                goto label_19;
              }
              else
                num2 += 2048;
            }
            else if ((comparand & 8380416) != 0)
            {
              manualResetEventSlim = this.TryGetOrCreateReaderEvent();
              if (manualResetEventSlim == null)
              {
                Thread.Sleep(100);
                num1 = this._state;
                comparand = 0;
                goto label_19;
              }
              else
                num2 += 1024;
            }
            else if (comparand == 1 && (this._readerEvent != null || this._writerEvent != null))
            {
              flag2 = true;
              num2 += 3072;
            }
          }
          num1 = Interlocked.CompareExchange(ref this._state, comparand + num2, comparand);
label_19:;
        }
        while (num1 != comparand);
        if (!flag1)
          return;
        if ((comparand & -8388608) != 0)
          autoResetEvent.Set();
        else if ((comparand & 8380416) != 0)
        {
          manualResetEventSlim.Set();
        }
        else
        {
          if (!flag2)
            return;
          this.ReleaseEvents();
        }
      }
    }

    /// <summary>Decrements the lock count on the writer lock.</summary>
    /// <exception cref="T:System.ApplicationException">The thread does not have the writer lock.</exception>
    public void ReleaseWriterLock()
    {
      if (this._writerID != ReaderWriterLock.GetCurrentThreadID())
        throw ReaderWriterLock.GetNotOwnerException();
      --this._writerLevel;
      if (this._writerLevel > (ushort) 0)
        return;
      this._writerID = -1;
      ManualResetEventSlim manualResetEventSlim = (ManualResetEventSlim) null;
      AutoResetEvent autoResetEvent = (AutoResetEvent) null;
      int num1 = this._state;
      bool flag;
      int comparand;
      do
      {
        flag = false;
        comparand = num1;
        int num2 = -4096;
        if ((comparand & 8380416) != 0)
        {
          manualResetEventSlim = this.TryGetOrCreateReaderEvent();
          if (manualResetEventSlim == null)
          {
            Thread.Sleep(100);
            num1 = this._state;
            comparand = 0;
            goto label_16;
          }
          else
            num2 += 1024;
        }
        else if ((comparand & -8388608) != 0)
        {
          autoResetEvent = this.TryGetOrCreateWriterEvent();
          if (autoResetEvent == null)
          {
            Thread.Sleep(100);
            num1 = this._state;
            comparand = 0;
            goto label_16;
          }
          else
            num2 += 2048;
        }
        else if (comparand == 4096 && (this._readerEvent != null || this._writerEvent != null))
        {
          flag = true;
          num2 += 3072;
        }
        num1 = Interlocked.CompareExchange(ref this._state, comparand + num2, comparand);
label_16:;
      }
      while (num1 != comparand);
      if ((comparand & 8380416) != 0)
        manualResetEventSlim.Set();
      else if ((comparand & -8388608) != 0)
      {
        autoResetEvent.Set();
      }
      else
      {
        if (!flag)
          return;
        this.ReleaseEvents();
      }
    }

    /// <summary>Upgrades a reader lock to the writer lock, using an <see cref="T:System.Int32" /> value for the time-out.</summary>
    /// <param name="millisecondsTimeout">The time-out in milliseconds.</param>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="millisecondsTimeout" /> expires before the lock request is granted.</exception>
    /// <returns>A <see cref="T:System.Threading.LockCookie" /> value.</returns>
    [UnsupportedOSPlatform("browser")]
    public LockCookie UpgradeToWriterLock(int millisecondsTimeout)
    {
      if (millisecondsTimeout < -1)
        throw ReaderWriterLock.GetInvalidTimeoutException(nameof (millisecondsTimeout));
      LockCookie lockCookie = new LockCookie();
      int currentThreadId = ReaderWriterLock.GetCurrentThreadID();
      lockCookie._threadID = currentThreadId;
      if (this._writerID == currentThreadId)
      {
        lockCookie._flags = LockCookieFlags.Upgrade | LockCookieFlags.OwnedWriter;
        lockCookie._writerLevel = this._writerLevel;
        this.AcquireWriterLock(millisecondsTimeout);
        return lockCookie;
      }
      ReaderWriterLock.ThreadLocalLockEntry current = ReaderWriterLock.ThreadLocalLockEntry.GetCurrent(this._lockID);
      if (current == null)
      {
        lockCookie._flags = LockCookieFlags.Upgrade | LockCookieFlags.OwnedNone;
      }
      else
      {
        lockCookie._flags = LockCookieFlags.Upgrade | LockCookieFlags.OwnedReader;
        lockCookie._readerLevel = current._readerLevel;
        if (Interlocked.CompareExchange(ref this._state, 4096, 1) == 1)
        {
          current._readerLevel = (ushort) 0;
          this._writerID = currentThreadId;
          this._writerLevel = (ushort) 1;
          ++this._writerSeqNum;
          return lockCookie;
        }
        current._readerLevel = (ushort) 1;
        this.ReleaseReaderLock();
      }
      bool flag = false;
      try
      {
        this.AcquireWriterLock(millisecondsTimeout);
        flag = true;
        return lockCookie;
      }
      finally
      {
        if (!flag)
        {
          LockCookieFlags flags = lockCookie._flags;
          lockCookie._flags = LockCookieFlags.Invalid;
          this.RecoverLock(ref lockCookie, flags & LockCookieFlags.OwnedReader);
        }
      }
    }

    /// <summary>Upgrades a reader lock to the writer lock, using a <see langword="TimeSpan" /> value for the time-out.</summary>
    /// <param name="timeout">The <see langword="TimeSpan" /> specifying the time-out period.</param>
    /// <exception cref="T:System.ApplicationException">
    /// <paramref name="timeout" /> expires before the lock request is granted.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="timeout" /> specifies a negative value other than -1 milliseconds.</exception>
    /// <returns>A <see cref="T:System.Threading.LockCookie" /> value.</returns>
    [UnsupportedOSPlatform("browser")]
    public LockCookie UpgradeToWriterLock(TimeSpan timeout) => this.UpgradeToWriterLock(ReaderWriterLock.ToTimeoutMilliseconds(timeout));

    /// <summary>Restores the lock status of the thread to what it was before <see cref="M:System.Threading.ReaderWriterLock.UpgradeToWriterLock(System.Int32)" /> was called.</summary>
    /// <param name="lockCookie">A <see cref="T:System.Threading.LockCookie" /> returned by <see cref="M:System.Threading.ReaderWriterLock.UpgradeToWriterLock(System.Int32)" />.</param>
    /// <exception cref="T:System.ApplicationException">The thread does not have the writer lock.</exception>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="lockCookie" /> is a null pointer.</exception>
    public void DowngradeFromWriterLock(ref LockCookie lockCookie)
    {
      int currentThreadId = ReaderWriterLock.GetCurrentThreadID();
      if (this._writerID != currentThreadId)
        throw ReaderWriterLock.GetNotOwnerException();
      LockCookieFlags flags = lockCookie._flags;
      ushort writerLevel = lockCookie._writerLevel;
      if ((flags & LockCookieFlags.Invalid) != ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader) || lockCookie._threadID != currentThreadId || (flags & (LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter)) != ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader) && (int) this._writerLevel <= (int) writerLevel)
        throw ReaderWriterLock.GetInvalidLockCookieException();
      if ((flags & LockCookieFlags.OwnedReader) != ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader))
      {
        ReaderWriterLock.ThreadLocalLockEntry current = ReaderWriterLock.ThreadLocalLockEntry.GetOrCreateCurrent(this._lockID);
        this._writerID = -1;
        this._writerLevel = (ushort) 0;
        ManualResetEventSlim manualResetEventSlim = (ManualResetEventSlim) null;
        int num1 = this._state;
        int comparand;
        do
        {
          comparand = num1;
          int num2 = -4095;
          if ((comparand & 8380416) != 0)
          {
            manualResetEventSlim = this.TryGetOrCreateReaderEvent();
            if (manualResetEventSlim == null)
            {
              Thread.Sleep(100);
              num1 = this._state;
              comparand = 0;
              goto label_11;
            }
            else
              num2 += 1024;
          }
          num1 = Interlocked.CompareExchange(ref this._state, comparand + num2, comparand);
label_11:;
        }
        while (num1 != comparand);
        if ((comparand & 8380416) != 0)
          manualResetEventSlim.Set();
        current._readerLevel = lockCookie._readerLevel;
      }
      else if ((flags & (LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter)) != ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader))
      {
        if (writerLevel > (ushort) 0)
        {
          this._writerLevel = writerLevel;
        }
        else
        {
          if (this._writerLevel != (ushort) 1)
            this._writerLevel = (ushort) 1;
          this.ReleaseWriterLock();
        }
      }
      lockCookie._flags = LockCookieFlags.Invalid;
    }

    /// <summary>Releases the lock, regardless of the number of times the thread acquired the lock.</summary>
    /// <returns>A <see cref="T:System.Threading.LockCookie" /> value representing the released lock.</returns>
    public LockCookie ReleaseLock()
    {
      LockCookie lockCookie = new LockCookie();
      int currentThreadId = ReaderWriterLock.GetCurrentThreadID();
      lockCookie._threadID = currentThreadId;
      if (this._writerID == currentThreadId)
      {
        lockCookie._flags = LockCookieFlags.Release | LockCookieFlags.OwnedWriter;
        lockCookie._writerLevel = this._writerLevel;
        this._writerLevel = (ushort) 1;
        this.ReleaseWriterLock();
        return lockCookie;
      }
      ReaderWriterLock.ThreadLocalLockEntry current = ReaderWriterLock.ThreadLocalLockEntry.GetCurrent(this._lockID);
      if (current == null)
      {
        lockCookie._flags = LockCookieFlags.Release | LockCookieFlags.OwnedNone;
        return lockCookie;
      }
      lockCookie._flags = LockCookieFlags.Release | LockCookieFlags.OwnedReader;
      lockCookie._readerLevel = current._readerLevel;
      current._readerLevel = (ushort) 1;
      this.ReleaseReaderLock();
      return lockCookie;
    }

    /// <summary>Restores the lock status of the thread to what it was before calling <see cref="M:System.Threading.ReaderWriterLock.ReleaseLock" />.</summary>
    /// <param name="lockCookie">A <see cref="T:System.Threading.LockCookie" /> returned by <see cref="M:System.Threading.ReaderWriterLock.ReleaseLock" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="lockCookie" /> is a null pointer.</exception>
    [UnsupportedOSPlatform("browser")]
    public void RestoreLock(ref LockCookie lockCookie)
    {
      int currentThreadId = ReaderWriterLock.GetCurrentThreadID();
      if (lockCookie._threadID != currentThreadId)
        throw ReaderWriterLock.GetInvalidLockCookieException();
      if (this._writerID == currentThreadId || ReaderWriterLock.ThreadLocalLockEntry.GetCurrent(this._lockID) != null)
        throw new SynchronizationLockException(SR.ReaderWriterLock_RestoreLockWithOwnedLocks);
      LockCookieFlags flags = lockCookie._flags;
      if ((flags & LockCookieFlags.Invalid) != ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader))
        throw ReaderWriterLock.GetInvalidLockCookieException();
      if ((flags & LockCookieFlags.OwnedNone) == ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader))
      {
        if ((flags & LockCookieFlags.OwnedWriter) != ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader))
        {
          if (Interlocked.CompareExchange(ref this._state, 4096, 0) == 0)
          {
            this._writerID = currentThreadId;
            this._writerLevel = lockCookie._writerLevel;
            ++this._writerSeqNum;
            goto label_14;
          }
        }
        else if ((flags & LockCookieFlags.OwnedReader) != ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader))
        {
          ReaderWriterLock.ThreadLocalLockEntry current = ReaderWriterLock.ThreadLocalLockEntry.GetOrCreateCurrent(this._lockID);
          int state = this._state;
          if (state < 1023 && Interlocked.CompareExchange(ref this._state, state + 1, state) == state)
          {
            current._readerLevel = lockCookie._readerLevel;
            goto label_14;
          }
        }
        this.RecoverLock(ref lockCookie, flags);
      }
label_14:
      lockCookie._flags = LockCookieFlags.Invalid;
    }

    [UnsupportedOSPlatform("browser")]
    private void RecoverLock(ref LockCookie lockCookie, LockCookieFlags flags)
    {
      if ((flags & LockCookieFlags.OwnedWriter) != ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader))
      {
        this.AcquireWriterLock(-1);
        this._writerLevel = lockCookie._writerLevel;
      }
      else
      {
        if ((flags & LockCookieFlags.OwnedReader) == ~(LockCookieFlags.Invalid | LockCookieFlags.Upgrade | LockCookieFlags.Release | LockCookieFlags.OwnedNone | LockCookieFlags.OwnedWriter | LockCookieFlags.OwnedReader))
          return;
        this.AcquireReaderLock(-1);
        ReaderWriterLock.ThreadLocalLockEntry.GetCurrent(this._lockID)._readerLevel = lockCookie._readerLevel;
      }
    }

    private static int GetCurrentThreadID() => Environment.CurrentManagedThreadId;

    private static bool YieldProcessor()
    {
      Thread.SpinWait(1);
      return true;
    }

    private ManualResetEventSlim GetOrCreateReaderEvent()
    {
      ManualResetEventSlim readerEvent1 = this._readerEvent;
      if (readerEvent1 != null)
        return readerEvent1;
      ManualResetEventSlim readerEvent2 = new ManualResetEventSlim(false, 0);
      ManualResetEventSlim readerEvent3 = Interlocked.CompareExchange<ManualResetEventSlim>(ref this._readerEvent, readerEvent2, (ManualResetEventSlim) null);
      if (readerEvent3 == null)
        return readerEvent2;
      readerEvent2.Dispose();
      return readerEvent3;
    }

    private AutoResetEvent GetOrCreateWriterEvent()
    {
      AutoResetEvent writerEvent1 = this._writerEvent;
      if (writerEvent1 != null)
        return writerEvent1;
      AutoResetEvent writerEvent2 = new AutoResetEvent(false);
      AutoResetEvent writerEvent3 = Interlocked.CompareExchange<AutoResetEvent>(ref this._writerEvent, writerEvent2, (AutoResetEvent) null);
      if (writerEvent3 == null)
        return writerEvent2;
      writerEvent2.Dispose();
      return writerEvent3;
    }

    private ManualResetEventSlim TryGetOrCreateReaderEvent()
    {
      try
      {
        return this.GetOrCreateReaderEvent();
      }
      catch
      {
        return (ManualResetEventSlim) null;
      }
    }

    private AutoResetEvent TryGetOrCreateWriterEvent()
    {
      try
      {
        return this.GetOrCreateWriterEvent();
      }
      catch
      {
        return (AutoResetEvent) null;
      }
    }

    private void ReleaseEvents()
    {
      AutoResetEvent writerEvent = this._writerEvent;
      this._writerEvent = (AutoResetEvent) null;
      ManualResetEventSlim readerEvent = this._readerEvent;
      this._readerEvent = (ManualResetEventSlim) null;
      Interlocked.Add(ref this._state, -3072);
      writerEvent?.Dispose();
      readerEvent?.Dispose();
    }

    private static ArgumentOutOfRangeException GetInvalidTimeoutException(
      string parameterName)
    {
      return new ArgumentOutOfRangeException(parameterName, SR.ArgumentOutOfRange_TimeoutMilliseconds);
    }

    private static int ToTimeoutMilliseconds(TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      return totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? (int) totalMilliseconds : throw ReaderWriterLock.GetInvalidTimeoutException(nameof (timeout));
    }

    private static ApplicationException GetTimeoutException() => (ApplicationException) new ReaderWriterLock.ReaderWriterLockApplicationException(-2147023436, SR.ReaderWriterLock_Timeout);

    private static ApplicationException GetNotOwnerException() => (ApplicationException) new ReaderWriterLock.ReaderWriterLockApplicationException(288, SR.ReaderWriterLock_NotOwner);

    private static ApplicationException GetInvalidLockCookieException() => (ApplicationException) new ReaderWriterLock.ReaderWriterLockApplicationException(-2147024809, SR.ReaderWriterLock_InvalidLockCookie);

    [Serializable]
    private sealed class ReaderWriterLockApplicationException : ApplicationException
    {
      public ReaderWriterLockApplicationException(int errorHResult, string message)
        : base(SR.Format(message, (object) SR.Format(SR.ExceptionFromHResult, (object) errorHResult)))
      {
        this.HResult = errorHResult;
      }

      public ReaderWriterLockApplicationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
      {
      }
    }

    private sealed class ThreadLocalLockEntry
    {
      [ThreadStatic]
      private static ReaderWriterLock.ThreadLocalLockEntry t_lockEntryHead;
      private long _lockID;
      private ReaderWriterLock.ThreadLocalLockEntry _next;
      public ushort _readerLevel;

      private ThreadLocalLockEntry(long lockID) => this._lockID = lockID;

      public bool IsFree => this._readerLevel == (ushort) 0;

      public static ReaderWriterLock.ThreadLocalLockEntry GetCurrent(long lockID)
      {
        for (ReaderWriterLock.ThreadLocalLockEntry threadLocalLockEntry = ReaderWriterLock.ThreadLocalLockEntry.t_lockEntryHead; threadLocalLockEntry != null; threadLocalLockEntry = threadLocalLockEntry._next)
        {
          if (threadLocalLockEntry._lockID == lockID)
            return !threadLocalLockEntry.IsFree ? threadLocalLockEntry : (ReaderWriterLock.ThreadLocalLockEntry) null;
        }
        return (ReaderWriterLock.ThreadLocalLockEntry) null;
      }

      public static ReaderWriterLock.ThreadLocalLockEntry GetOrCreateCurrent(
        long lockID)
      {
        ReaderWriterLock.ThreadLocalLockEntry tLockEntryHead = ReaderWriterLock.ThreadLocalLockEntry.t_lockEntryHead;
        if (tLockEntryHead != null)
        {
          if (tLockEntryHead._lockID == lockID)
            return tLockEntryHead;
          if (tLockEntryHead.IsFree)
          {
            tLockEntryHead._lockID = lockID;
            return tLockEntryHead;
          }
        }
        return ReaderWriterLock.ThreadLocalLockEntry.GetOrCreateCurrentSlow(lockID, tLockEntryHead);
      }

      private static ReaderWriterLock.ThreadLocalLockEntry GetOrCreateCurrentSlow(
        long lockID,
        ReaderWriterLock.ThreadLocalLockEntry headEntry)
      {
        ReaderWriterLock.ThreadLocalLockEntry currentSlow1 = (ReaderWriterLock.ThreadLocalLockEntry) null;
        ReaderWriterLock.ThreadLocalLockEntry threadLocalLockEntry1 = (ReaderWriterLock.ThreadLocalLockEntry) null;
        ReaderWriterLock.ThreadLocalLockEntry currentSlow2 = (ReaderWriterLock.ThreadLocalLockEntry) null;
        if (headEntry != null)
        {
          if (headEntry.IsFree)
            currentSlow2 = headEntry;
          ReaderWriterLock.ThreadLocalLockEntry threadLocalLockEntry2 = headEntry;
          for (ReaderWriterLock.ThreadLocalLockEntry next = headEntry._next; next != null; next = next._next)
          {
            if (next._lockID == lockID)
            {
              threadLocalLockEntry2._next = next._next;
              currentSlow1 = next;
              break;
            }
            if (currentSlow2 == null && next.IsFree)
            {
              threadLocalLockEntry1 = threadLocalLockEntry2;
              currentSlow2 = next;
            }
            threadLocalLockEntry2 = next;
          }
        }
        if (currentSlow1 == null)
        {
          if (currentSlow2 != null)
          {
            currentSlow2._lockID = lockID;
            if (threadLocalLockEntry1 == null)
              return currentSlow2;
            threadLocalLockEntry1._next = currentSlow2._next;
            currentSlow1 = currentSlow2;
          }
          else
            currentSlow1 = new ReaderWriterLock.ThreadLocalLockEntry(lockID);
        }
        currentSlow1._next = headEntry;
        ReaderWriterLock.ThreadLocalLockEntry.t_lockEntryHead = currentSlow1;
        return currentSlow1;
      }
    }
  }
}
