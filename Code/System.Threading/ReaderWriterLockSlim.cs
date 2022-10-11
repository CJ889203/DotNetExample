// Decompiled with JetBrains decompiler
// Type: System.Threading.ReaderWriterLockSlim
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Threading
{
  /// <summary>Represents a lock that is used to manage access to a resource, allowing multiple threads for reading or exclusive access for writing.</summary>
  public class ReaderWriterLockSlim : IDisposable
  {
    private readonly bool _fIsReentrant;
    private ReaderWriterLockSlim.SpinLock _spinLock;
    private uint _numWriteWaiters;
    private uint _numReadWaiters;
    private uint _numWriteUpgradeWaiters;
    private uint _numUpgradeWaiters;
    private ReaderWriterLockSlim.WaiterStates _waiterStates;
    private int _upgradeLockOwnerId;
    private int _writeLockOwnerId;
    private EventWaitHandle _writeEvent;
    private EventWaitHandle _readEvent;
    private EventWaitHandle _upgradeEvent;
    private EventWaitHandle _waitUpgradeEvent;
    private static long s_nextLockID;
    private readonly long _lockID;
    [ThreadStatic]
    private static ReaderWriterCount t_rwc;
    private bool _fUpgradeThreadHoldingRead;
    private uint _owners;
    private bool _fDisposed;

    private void InitializeThreadCounts()
    {
      this._upgradeLockOwnerId = -1;
      this._writeLockOwnerId = -1;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.ReaderWriterLockSlim" /> class with default property values.</summary>
    public ReaderWriterLockSlim()
      : this(LockRecursionPolicy.NoRecursion)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.ReaderWriterLockSlim" /> class, specifying the lock recursion policy.</summary>
    /// <param name="recursionPolicy">One of the enumeration values that specifies the lock recursion policy.</param>
    public ReaderWriterLockSlim(LockRecursionPolicy recursionPolicy)
    {
      if (recursionPolicy == LockRecursionPolicy.SupportsRecursion)
        this._fIsReentrant = true;
      this.InitializeThreadCounts();
      this._waiterStates = ReaderWriterLockSlim.WaiterStates.NoWaiters;
      this._lockID = Interlocked.Increment(ref ReaderWriterLockSlim.s_nextLockID);
    }

    private bool HasNoWaiters
    {
      get => (this._waiterStates & ReaderWriterLockSlim.WaiterStates.NoWaiters) != 0;
      set
      {
        if (value)
          this._waiterStates |= ReaderWriterLockSlim.WaiterStates.NoWaiters;
        else
          this._waiterStates &= ~ReaderWriterLockSlim.WaiterStates.NoWaiters;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsRWEntryEmpty(ReaderWriterCount rwc) => rwc.lockID == 0L || rwc.readercount == 0 && rwc.writercount == 0 && rwc.upgradecount == 0;

    private bool IsRwHashEntryChanged(ReaderWriterCount lrwc) => lrwc.lockID != this._lockID;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReaderWriterCount GetThreadRWCount(bool dontAllocate)
    {
      ReaderWriterCount rwc = ReaderWriterLockSlim.t_rwc;
      ReaderWriterCount threadRwCount = (ReaderWriterCount) null;
      for (; rwc != null; rwc = rwc.next)
      {
        if (rwc.lockID == this._lockID)
          return rwc;
        if (!dontAllocate && threadRwCount == null && ReaderWriterLockSlim.IsRWEntryEmpty(rwc))
          threadRwCount = rwc;
      }
      if (dontAllocate)
        return (ReaderWriterCount) null;
      if (threadRwCount == null)
      {
        threadRwCount = new ReaderWriterCount();
        threadRwCount.next = ReaderWriterLockSlim.t_rwc;
        ReaderWriterLockSlim.t_rwc = threadRwCount;
      }
      threadRwCount.lockID = this._lockID;
      return threadRwCount;
    }

    /// <summary>Tries to enter the lock in read mode.</summary>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" />, and the current thread has attempted to acquire the read lock when it already holds the read lock.
    /// 
    /// -or-
    /// 
    /// The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" />, and the current thread has attempted to acquire the read lock when it already holds the write lock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. This limit is so large that applications should never encounter this exception.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    public void EnterReadLock() => this.TryEnterReadLock(-1);

    /// <summary>Tries to enter the lock in read mode, with an optional time-out.</summary>
    /// <param name="timeout">The interval to wait, or -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
    /// 
    /// -or-
    /// 
    /// The value of <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the calling thread entered read mode, otherwise, <see langword="false" />.</returns>
    public bool TryEnterReadLock(TimeSpan timeout) => this.TryEnterReadLock(new ReaderWriterLockSlim.TimeoutTracker(timeout));

    /// <summary>Tries to enter the lock in read mode, with an optional integer time-out.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or -1 (<see cref="F:System.Threading.Timeout.Infinite" />) to wait indefinitely.</param>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout" /> is negative, but it is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (-1), which is the only negative value allowed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the calling thread entered read mode, otherwise, <see langword="false" />.</returns>
    public bool TryEnterReadLock(int millisecondsTimeout) => this.TryEnterReadLock(new ReaderWriterLockSlim.TimeoutTracker(millisecondsTimeout));

    private bool TryEnterReadLock(ReaderWriterLockSlim.TimeoutTracker timeout) => this.TryEnterReadLockCore(timeout);

    private bool TryEnterReadLockCore(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      if (this._fDisposed)
        throw new ObjectDisposedException((string) null);
      int currentManagedThreadId = Environment.CurrentManagedThreadId;
      ReaderWriterCount threadRwCount;
      if (!this._fIsReentrant)
      {
        if (currentManagedThreadId == this._writeLockOwnerId)
          throw new LockRecursionException(SR.LockRecursionException_ReadAfterWriteNotAllowed);
        this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead);
        threadRwCount = this.GetThreadRWCount(false);
        if (threadRwCount.readercount > 0)
        {
          this._spinLock.Exit();
          throw new LockRecursionException(SR.LockRecursionException_RecursiveReadNotAllowed);
        }
        if (currentManagedThreadId == this._upgradeLockOwnerId)
        {
          ++threadRwCount.readercount;
          ++this._owners;
          this._spinLock.Exit();
          return true;
        }
      }
      else
      {
        this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead);
        threadRwCount = this.GetThreadRWCount(false);
        if (threadRwCount.readercount > 0)
        {
          ++threadRwCount.readercount;
          this._spinLock.Exit();
          return true;
        }
        if (currentManagedThreadId == this._upgradeLockOwnerId)
        {
          ++threadRwCount.readercount;
          ++this._owners;
          this._spinLock.Exit();
          this._fUpgradeThreadHoldingRead = true;
          return true;
        }
        if (currentManagedThreadId == this._writeLockOwnerId)
        {
          ++threadRwCount.readercount;
          ++this._owners;
          this._spinLock.Exit();
          return true;
        }
      }
      bool flag = true;
      int spinCount = 0;
      while (this._owners >= 268435454U)
      {
        if (timeout.IsExpired)
        {
          this._spinLock.Exit();
          return false;
        }
        if (spinCount < 20 && this.ShouldSpinForEnterAnyRead())
        {
          this._spinLock.Exit();
          ++spinCount;
          ReaderWriterLockSlim.SpinWait(spinCount);
          this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead);
          if (this.IsRwHashEntryChanged(threadRwCount))
            threadRwCount = this.GetThreadRWCount(false);
        }
        else if (this._readEvent == null)
        {
          this.LazyCreateEvent(ref this._readEvent, ReaderWriterLockSlim.EnterLockType.Read);
          if (this.IsRwHashEntryChanged(threadRwCount))
            threadRwCount = this.GetThreadRWCount(false);
        }
        else
        {
          flag = this.WaitOnEvent(this._readEvent, ref this._numReadWaiters, timeout, ReaderWriterLockSlim.EnterLockType.Read);
          if (!flag)
            return false;
          if (this.IsRwHashEntryChanged(threadRwCount))
            threadRwCount = this.GetThreadRWCount(false);
        }
      }
      ++this._owners;
      ++threadRwCount.readercount;
      this._spinLock.Exit();
      return flag;
    }

    /// <summary>Tries to enter the lock in write mode.</summary>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock in any mode.
    /// 
    /// -or-
    /// 
    /// The current thread has entered read mode and doesn't already own a write lock, so trying to enter the lock in write mode would create the possibility of a deadlock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    public void EnterWriteLock() => this.TryEnterWriteLock(-1);

    /// <summary>Tries to enter the lock in write mode, with an optional time-out.</summary>
    /// <param name="timeout">The interval to wait, or -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock.
    /// 
    /// -or-
    /// 
    /// The current thread initially entered the lock in read mode, and therefore trying to enter write mode would create the possibility of a deadlock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
    /// 
    /// -or-
    /// 
    /// The value of <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the calling thread entered write mode, otherwise, <see langword="false" />.</returns>
    public bool TryEnterWriteLock(TimeSpan timeout) => this.TryEnterWriteLock(new ReaderWriterLockSlim.TimeoutTracker(timeout));

    /// <summary>Tries to enter the lock in write mode, with an optional time-out.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or -1 (<see cref="F:System.Threading.Timeout.Infinite" />) to wait indefinitely.</param>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock.
    /// 
    /// -or-
    /// 
    /// The current thread initially entered the lock in read mode, and therefore trying to enter write mode would create the possibility of a deadlock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout" /> is negative, but it is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (-1), which is the only negative value allowed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the calling thread entered write mode, otherwise, <see langword="false" />.</returns>
    public bool TryEnterWriteLock(int millisecondsTimeout) => this.TryEnterWriteLock(new ReaderWriterLockSlim.TimeoutTracker(millisecondsTimeout));

    private bool TryEnterWriteLock(ReaderWriterLockSlim.TimeoutTracker timeout) => this.TryEnterWriteLockCore(timeout);

    private bool TryEnterWriteLockCore(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      if (this._fDisposed)
        throw new ObjectDisposedException((string) null);
      int currentManagedThreadId = Environment.CurrentManagedThreadId;
      bool isUpgradeToWrite = false;
      ReaderWriterCount threadRwCount;
      if (!this._fIsReentrant)
      {
        if (currentManagedThreadId == this._writeLockOwnerId)
          throw new LockRecursionException(SR.LockRecursionException_RecursiveWriteNotAllowed);
        ReaderWriterLockSlim.EnterSpinLockReason reason;
        if (currentManagedThreadId == this._upgradeLockOwnerId)
        {
          isUpgradeToWrite = true;
          reason = ReaderWriterLockSlim.EnterSpinLockReason.UpgradeToWrite;
        }
        else
          reason = ReaderWriterLockSlim.EnterSpinLockReason.EnterWrite;
        this._spinLock.Enter(reason);
        threadRwCount = this.GetThreadRWCount(true);
        if (threadRwCount != null && threadRwCount.readercount > 0)
        {
          this._spinLock.Exit();
          throw new LockRecursionException(SR.LockRecursionException_WriteAfterReadNotAllowed);
        }
      }
      else
      {
        this._spinLock.Enter(currentManagedThreadId != this._writeLockOwnerId ? (currentManagedThreadId != this._upgradeLockOwnerId ? ReaderWriterLockSlim.EnterSpinLockReason.EnterWrite : ReaderWriterLockSlim.EnterSpinLockReason.UpgradeToWrite) : ReaderWriterLockSlim.EnterSpinLockReason.EnterRecursiveWrite);
        threadRwCount = this.GetThreadRWCount(false);
        if (currentManagedThreadId == this._writeLockOwnerId)
        {
          ++threadRwCount.writercount;
          this._spinLock.Exit();
          return true;
        }
        if (currentManagedThreadId == this._upgradeLockOwnerId)
          isUpgradeToWrite = true;
        else if (threadRwCount.readercount > 0)
        {
          this._spinLock.Exit();
          throw new LockRecursionException(SR.LockRecursionException_WriteAfterReadNotAllowed);
        }
      }
      int spinCount = 0;
      while (!this.IsWriterAcquired())
      {
        if (isUpgradeToWrite)
        {
          switch (this.GetNumReaders())
          {
            case 1:
              this.SetWriterAcquired();
              goto label_40;
            case 2:
              if (threadRwCount != null)
              {
                if (this.IsRwHashEntryChanged(threadRwCount))
                  threadRwCount = this.GetThreadRWCount(false);
                if (threadRwCount.readercount > 0)
                {
                  this.SetWriterAcquired();
                  goto label_40;
                }
                else
                  break;
              }
              else
                break;
          }
        }
        if (timeout.IsExpired)
        {
          this._spinLock.Exit();
          return false;
        }
        if (spinCount < 20 && this.ShouldSpinForEnterAnyWrite(isUpgradeToWrite))
        {
          this._spinLock.Exit();
          ++spinCount;
          ReaderWriterLockSlim.SpinWait(spinCount);
          this._spinLock.Enter(isUpgradeToWrite ? ReaderWriterLockSlim.EnterSpinLockReason.UpgradeToWrite : ReaderWriterLockSlim.EnterSpinLockReason.EnterWrite);
        }
        else if (isUpgradeToWrite)
        {
          if (this._waitUpgradeEvent == null)
            this.LazyCreateEvent(ref this._waitUpgradeEvent, ReaderWriterLockSlim.EnterLockType.UpgradeToWrite);
          else if (!this.WaitOnEvent(this._waitUpgradeEvent, ref this._numWriteUpgradeWaiters, timeout, ReaderWriterLockSlim.EnterLockType.UpgradeToWrite))
            return false;
        }
        else if (this._writeEvent == null)
          this.LazyCreateEvent(ref this._writeEvent, ReaderWriterLockSlim.EnterLockType.Write);
        else if (!this.WaitOnEvent(this._writeEvent, ref this._numWriteWaiters, timeout, ReaderWriterLockSlim.EnterLockType.Write))
          return false;
      }
      this.SetWriterAcquired();
label_40:
      if (this._fIsReentrant)
      {
        if (this.IsRwHashEntryChanged(threadRwCount))
          threadRwCount = this.GetThreadRWCount(false);
        ++threadRwCount.writercount;
      }
      this._spinLock.Exit();
      this._writeLockOwnerId = currentManagedThreadId;
      return true;
    }

    /// <summary>Tries to enter the lock in upgradeable mode.</summary>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock in any mode.
    /// 
    /// -or-
    /// 
    /// The current thread has entered read mode, so trying to enter upgradeable mode would create the possibility of a deadlock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    public void EnterUpgradeableReadLock() => this.TryEnterUpgradeableReadLock(-1);

    /// <summary>Tries to enter the lock in upgradeable mode, with an optional time-out.</summary>
    /// <param name="timeout">The interval to wait, or -1 milliseconds to wait indefinitely.</param>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock.
    /// 
    /// -or-
    /// 
    /// The current thread initially entered the lock in read mode, and therefore trying to enter upgradeable mode would create the possibility of a deadlock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="timeout" /> is negative, but it is not equal to -1 milliseconds, which is the only negative value allowed.
    /// 
    /// -or-
    /// 
    /// The value of <paramref name="timeout" /> is greater than <see cref="F:System.Int32.MaxValue" /> milliseconds.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the calling thread entered upgradeable mode, otherwise, <see langword="false" />.</returns>
    public bool TryEnterUpgradeableReadLock(TimeSpan timeout) => this.TryEnterUpgradeableReadLock(new ReaderWriterLockSlim.TimeoutTracker(timeout));

    /// <summary>Tries to enter the lock in upgradeable mode, with an optional time-out.</summary>
    /// <param name="millisecondsTimeout">The number of milliseconds to wait, or -1 (<see cref="F:System.Threading.Timeout.Infinite" />) to wait indefinitely.</param>
    /// <exception cref="T:System.Threading.LockRecursionException">The <see cref="P:System.Threading.ReaderWriterLockSlim.RecursionPolicy" /> property is <see cref="F:System.Threading.LockRecursionPolicy.NoRecursion" /> and the current thread has already entered the lock.
    /// 
    /// -or-
    /// 
    /// The current thread initially entered the lock in read mode, and therefore trying to enter upgradeable mode would create the possibility of a deadlock.
    /// 
    /// -or-
    /// 
    /// The recursion number would exceed the capacity of the counter. The limit is so large that applications should never encounter it.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="millisecondsTimeout" /> is negative, but it is not equal to <see cref="F:System.Threading.Timeout.Infinite" /> (-1), which is the only negative value allowed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Threading.ReaderWriterLockSlim" /> object has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the calling thread entered upgradeable mode, otherwise, <see langword="false" />.</returns>
    public bool TryEnterUpgradeableReadLock(int millisecondsTimeout) => this.TryEnterUpgradeableReadLock(new ReaderWriterLockSlim.TimeoutTracker(millisecondsTimeout));

    private bool TryEnterUpgradeableReadLock(ReaderWriterLockSlim.TimeoutTracker timeout) => this.TryEnterUpgradeableReadLockCore(timeout);

    private bool TryEnterUpgradeableReadLockCore(ReaderWriterLockSlim.TimeoutTracker timeout)
    {
      if (this._fDisposed)
        throw new ObjectDisposedException((string) null);
      int currentManagedThreadId = Environment.CurrentManagedThreadId;
      ReaderWriterCount threadRwCount;
      if (!this._fIsReentrant)
      {
        if (currentManagedThreadId == this._upgradeLockOwnerId)
          throw new LockRecursionException(SR.LockRecursionException_RecursiveUpgradeNotAllowed);
        if (currentManagedThreadId == this._writeLockOwnerId)
          throw new LockRecursionException(SR.LockRecursionException_UpgradeAfterWriteNotAllowed);
        this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead);
        threadRwCount = this.GetThreadRWCount(true);
        if (threadRwCount != null && threadRwCount.readercount > 0)
        {
          this._spinLock.Exit();
          throw new LockRecursionException(SR.LockRecursionException_UpgradeAfterReadNotAllowed);
        }
      }
      else
      {
        this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead);
        threadRwCount = this.GetThreadRWCount(false);
        if (currentManagedThreadId == this._upgradeLockOwnerId)
        {
          ++threadRwCount.upgradecount;
          this._spinLock.Exit();
          return true;
        }
        if (currentManagedThreadId == this._writeLockOwnerId)
        {
          ++this._owners;
          this._upgradeLockOwnerId = currentManagedThreadId;
          ++threadRwCount.upgradecount;
          if (threadRwCount.readercount > 0)
            this._fUpgradeThreadHoldingRead = true;
          this._spinLock.Exit();
          return true;
        }
        if (threadRwCount.readercount > 0)
        {
          this._spinLock.Exit();
          throw new LockRecursionException(SR.LockRecursionException_UpgradeAfterReadNotAllowed);
        }
      }
      int spinCount = 0;
      while (this._upgradeLockOwnerId != -1 || this._owners >= 268435454U)
      {
        if (timeout.IsExpired)
        {
          this._spinLock.Exit();
          return false;
        }
        if (spinCount < 20 && this.ShouldSpinForEnterAnyRead())
        {
          this._spinLock.Exit();
          ++spinCount;
          ReaderWriterLockSlim.SpinWait(spinCount);
          this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead);
        }
        else if (this._upgradeEvent == null)
          this.LazyCreateEvent(ref this._upgradeEvent, ReaderWriterLockSlim.EnterLockType.UpgradeableRead);
        else if (!this.WaitOnEvent(this._upgradeEvent, ref this._numUpgradeWaiters, timeout, ReaderWriterLockSlim.EnterLockType.UpgradeableRead))
          return false;
      }
      ++this._owners;
      this._upgradeLockOwnerId = currentManagedThreadId;
      if (this._fIsReentrant)
      {
        if (this.IsRwHashEntryChanged(threadRwCount))
          threadRwCount = this.GetThreadRWCount(false);
        ++threadRwCount.upgradecount;
      }
      this._spinLock.Exit();
      return true;
    }

    /// <summary>Reduces the recursion count for read mode, and exits read mode if the resulting count is 0 (zero).</summary>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The current thread has not entered the lock in read mode.</exception>
    public void ExitReadLock()
    {
      this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.ExitAnyRead);
      ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
      if (threadRwCount == null || threadRwCount.readercount < 1)
      {
        this._spinLock.Exit();
        throw new SynchronizationLockException(SR.SynchronizationLockException_MisMatchedRead);
      }
      if (this._fIsReentrant)
      {
        if (threadRwCount.readercount > 1)
        {
          --threadRwCount.readercount;
          this._spinLock.Exit();
          return;
        }
        if (Environment.CurrentManagedThreadId == this._upgradeLockOwnerId)
          this._fUpgradeThreadHoldingRead = false;
      }
      --this._owners;
      --threadRwCount.readercount;
      this.ExitAndWakeUpAppropriateWaiters();
    }

    /// <summary>Reduces the recursion count for write mode, and exits write mode if the resulting count is 0 (zero).</summary>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The current thread has not entered the lock in write mode.</exception>
    public void ExitWriteLock()
    {
      if (!this._fIsReentrant)
      {
        if (Environment.CurrentManagedThreadId != this._writeLockOwnerId)
          throw new SynchronizationLockException(SR.SynchronizationLockException_MisMatchedWrite);
        this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.ExitAnyWrite);
      }
      else
      {
        this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.ExitAnyWrite);
        ReaderWriterCount threadRwCount = this.GetThreadRWCount(false);
        if (threadRwCount == null)
        {
          this._spinLock.Exit();
          throw new SynchronizationLockException(SR.SynchronizationLockException_MisMatchedWrite);
        }
        if (threadRwCount.writercount < 1)
        {
          this._spinLock.Exit();
          throw new SynchronizationLockException(SR.SynchronizationLockException_MisMatchedWrite);
        }
        --threadRwCount.writercount;
        if (threadRwCount.writercount > 0)
        {
          this._spinLock.Exit();
          return;
        }
      }
      this.ClearWriterAcquired();
      this._writeLockOwnerId = -1;
      this.ExitAndWakeUpAppropriateWaiters();
    }

    /// <summary>Reduces the recursion count for upgradeable mode, and exits upgradeable mode if the resulting count is 0 (zero).</summary>
    /// <exception cref="T:System.Threading.SynchronizationLockException">The current thread has not entered the lock in upgradeable mode.</exception>
    public void ExitUpgradeableReadLock()
    {
      if (!this._fIsReentrant)
      {
        if (Environment.CurrentManagedThreadId != this._upgradeLockOwnerId)
          throw new SynchronizationLockException(SR.SynchronizationLockException_MisMatchedUpgrade);
        this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.ExitAnyRead);
      }
      else
      {
        this._spinLock.Enter(ReaderWriterLockSlim.EnterSpinLockReason.ExitAnyRead);
        ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
        if (threadRwCount == null)
        {
          this._spinLock.Exit();
          throw new SynchronizationLockException(SR.SynchronizationLockException_MisMatchedUpgrade);
        }
        if (threadRwCount.upgradecount < 1)
        {
          this._spinLock.Exit();
          throw new SynchronizationLockException(SR.SynchronizationLockException_MisMatchedUpgrade);
        }
        --threadRwCount.upgradecount;
        if (threadRwCount.upgradecount > 0)
        {
          this._spinLock.Exit();
          return;
        }
        this._fUpgradeThreadHoldingRead = false;
      }
      --this._owners;
      this._upgradeLockOwnerId = -1;
      this.ExitAndWakeUpAppropriateWaiters();
    }

    private void LazyCreateEvent(
      [NotNull] ref EventWaitHandle waitEvent,
      ReaderWriterLockSlim.EnterLockType enterLockType)
    {
      this._spinLock.Exit();
      EventWaitHandle eventWaitHandle = new EventWaitHandle(false, enterLockType == ReaderWriterLockSlim.EnterLockType.Read ? EventResetMode.ManualReset : EventResetMode.AutoReset);
      ReaderWriterLockSlim.EnterSpinLockReason reason;
      switch (enterLockType)
      {
        case ReaderWriterLockSlim.EnterLockType.Read:
        case ReaderWriterLockSlim.EnterLockType.UpgradeableRead:
          reason = ReaderWriterLockSlim.EnterSpinLockReason.Wait;
          break;
        case ReaderWriterLockSlim.EnterLockType.Write:
          reason = ReaderWriterLockSlim.EnterSpinLockReason.EnterWrite | ReaderWriterLockSlim.EnterSpinLockReason.Wait;
          break;
        default:
          reason = ReaderWriterLockSlim.EnterSpinLockReason.UpgradeToWrite | ReaderWriterLockSlim.EnterSpinLockReason.Wait;
          break;
      }
      this._spinLock.Enter(reason);
      if (waitEvent == null)
        waitEvent = eventWaitHandle;
      else
        eventWaitHandle.Dispose();
    }

    private bool WaitOnEvent(
      EventWaitHandle waitEvent,
      ref uint numWaiters,
      ReaderWriterLockSlim.TimeoutTracker timeout,
      ReaderWriterLockSlim.EnterLockType enterLockType)
    {
      ReaderWriterLockSlim.WaiterStates waiterStates = ReaderWriterLockSlim.WaiterStates.None;
      ReaderWriterLockSlim.EnterSpinLockReason reason;
      switch (enterLockType)
      {
        case ReaderWriterLockSlim.EnterLockType.Read:
          reason = ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead;
          break;
        case ReaderWriterLockSlim.EnterLockType.UpgradeableRead:
          waiterStates = ReaderWriterLockSlim.WaiterStates.UpgradeableReadWaiterSignaled;
          goto case ReaderWriterLockSlim.EnterLockType.Read;
        case ReaderWriterLockSlim.EnterLockType.Write:
          waiterStates = ReaderWriterLockSlim.WaiterStates.WriteWaiterSignaled;
          reason = ReaderWriterLockSlim.EnterSpinLockReason.EnterWrite;
          break;
        default:
          reason = ReaderWriterLockSlim.EnterSpinLockReason.UpgradeToWrite;
          break;
      }
      if (waiterStates != ReaderWriterLockSlim.WaiterStates.None && (this._waiterStates & waiterStates) != ReaderWriterLockSlim.WaiterStates.None)
        this._waiterStates &= ~waiterStates;
      waitEvent.Reset();
      ++numWaiters;
      this.HasNoWaiters = false;
      if (this._numWriteWaiters == 1U)
        this.SetWritersWaiting();
      if (this._numWriteUpgradeWaiters == 1U)
        this.SetUpgraderWaiting();
      bool flag = false;
      this._spinLock.Exit();
      try
      {
        flag = waitEvent.WaitOne(timeout.RemainingMilliseconds);
        return flag;
      }
      finally
      {
        this._spinLock.Enter(reason);
        --numWaiters;
        if (flag && waiterStates != ReaderWriterLockSlim.WaiterStates.None && (this._waiterStates & waiterStates) != ReaderWriterLockSlim.WaiterStates.None)
          this._waiterStates &= ~waiterStates;
        if (this._numWriteWaiters == 0U && this._numWriteUpgradeWaiters == 0U && this._numUpgradeWaiters == 0U && this._numReadWaiters == 0U)
          this.HasNoWaiters = true;
        if (this._numWriteWaiters == 0U)
          this.ClearWritersWaiting();
        if (this._numWriteUpgradeWaiters == 0U)
          this.ClearUpgraderWaiting();
        if (!flag)
        {
          if (enterLockType >= ReaderWriterLockSlim.EnterLockType.Write)
            this.ExitAndWakeUpAppropriateReadWaiters();
          else
            this._spinLock.Exit();
        }
      }
    }

    private void ExitAndWakeUpAppropriateWaiters()
    {
      if (this.HasNoWaiters)
        this._spinLock.Exit();
      else
        this.ExitAndWakeUpAppropriateWaitersPreferringWriters();
    }

    private void ExitAndWakeUpAppropriateWaitersPreferringWriters()
    {
      uint numReaders = this.GetNumReaders();
      if (this._fIsReentrant && this._numWriteUpgradeWaiters > 0U && this._fUpgradeThreadHoldingRead && numReaders == 2U)
      {
        this._spinLock.Exit();
        this._waitUpgradeEvent.Set();
      }
      else if (numReaders == 1U && this._numWriteUpgradeWaiters > 0U)
      {
        this._spinLock.Exit();
        this._waitUpgradeEvent.Set();
      }
      else if (numReaders == 0U && this._numWriteWaiters > 0U)
      {
        ReaderWriterLockSlim.WaiterStates waiterStates = this._waiterStates & ReaderWriterLockSlim.WaiterStates.WriteWaiterSignaled;
        if (waiterStates == ReaderWriterLockSlim.WaiterStates.None)
          this._waiterStates |= ReaderWriterLockSlim.WaiterStates.WriteWaiterSignaled;
        this._spinLock.Exit();
        if (waiterStates != ReaderWriterLockSlim.WaiterStates.None)
          return;
        this._writeEvent.Set();
      }
      else
        this.ExitAndWakeUpAppropriateReadWaiters();
    }

    private void ExitAndWakeUpAppropriateReadWaiters()
    {
      if (this._numWriteWaiters != 0U || this._numWriteUpgradeWaiters != 0U || this.HasNoWaiters)
      {
        this._spinLock.Exit();
      }
      else
      {
        bool flag1 = this._numReadWaiters > 0U;
        bool flag2 = this._numUpgradeWaiters != 0U && this._upgradeLockOwnerId == -1;
        if (flag2)
        {
          if ((this._waiterStates & ReaderWriterLockSlim.WaiterStates.UpgradeableReadWaiterSignaled) == ReaderWriterLockSlim.WaiterStates.None)
            this._waiterStates |= ReaderWriterLockSlim.WaiterStates.UpgradeableReadWaiterSignaled;
          else
            flag2 = false;
        }
        this._spinLock.Exit();
        if (flag1)
          this._readEvent.Set();
        if (!flag2)
          return;
        this._upgradeEvent.Set();
      }
    }

    private bool IsWriterAcquired() => ((int) this._owners & -1073741825) == 0;

    private void SetWriterAcquired() => this._owners |= 2147483648U;

    private void ClearWriterAcquired() => this._owners &= (uint) int.MaxValue;

    private void SetWritersWaiting() => this._owners |= 1073741824U;

    private void ClearWritersWaiting() => this._owners &= 3221225471U;

    private void SetUpgraderWaiting() => this._owners |= 536870912U;

    private void ClearUpgraderWaiting() => this._owners &= 3758096383U;

    private uint GetNumReaders() => this._owners & 268435455U;

    private bool ShouldSpinForEnterAnyRead()
    {
      if (this.HasNoWaiters)
        return true;
      return this._numWriteWaiters == 0U && this._numWriteUpgradeWaiters == 0U;
    }

    private bool ShouldSpinForEnterAnyWrite(bool isUpgradeToWrite) => isUpgradeToWrite || this._numWriteUpgradeWaiters == 0U;

    private static void SpinWait(int spinCount)
    {
      if (spinCount < 5 && Environment.ProcessorCount > 1)
        Thread.SpinWait(20 * spinCount);
      else
        Thread.Sleep(0);
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.ReaderWriterLockSlim" /> class.</summary>
    /// <exception cref="T:System.Threading.SynchronizationLockException">
    ///        <see cref="P:System.Threading.ReaderWriterLockSlim.WaitingReadCount" /> is greater than zero.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Threading.ReaderWriterLockSlim.WaitingUpgradeCount" /> is greater than zero.
    /// 
    /// -or-
    /// 
    /// <see cref="P:System.Threading.ReaderWriterLockSlim.WaitingWriteCount" /> is greater than zero.</exception>
    public void Dispose() => this.Dispose(true);

    private void Dispose(bool disposing)
    {
      if (!disposing || this._fDisposed)
        return;
      if (this.WaitingReadCount > 0 || this.WaitingUpgradeCount > 0 || this.WaitingWriteCount > 0)
        throw new SynchronizationLockException(SR.SynchronizationLockException_IncorrectDispose);
      if (this.IsReadLockHeld || this.IsUpgradeableReadLockHeld || this.IsWriteLockHeld)
        throw new SynchronizationLockException(SR.SynchronizationLockException_IncorrectDispose);
      if (this._writeEvent != null)
      {
        this._writeEvent.Dispose();
        this._writeEvent = (EventWaitHandle) null;
      }
      if (this._readEvent != null)
      {
        this._readEvent.Dispose();
        this._readEvent = (EventWaitHandle) null;
      }
      if (this._upgradeEvent != null)
      {
        this._upgradeEvent.Dispose();
        this._upgradeEvent = (EventWaitHandle) null;
      }
      if (this._waitUpgradeEvent != null)
      {
        this._waitUpgradeEvent.Dispose();
        this._waitUpgradeEvent = (EventWaitHandle) null;
      }
      this._fDisposed = true;
    }

    /// <summary>Gets a value that indicates whether the current thread has entered the lock in read mode.</summary>
    /// <returns>
    /// <see langword="true" /> if the current thread has entered read mode; otherwise, <see langword="false" />.</returns>
    public bool IsReadLockHeld => this.RecursiveReadCount > 0;

    /// <summary>Gets a value that indicates whether the current thread has entered the lock in upgradeable mode.</summary>
    /// <returns>
    /// <see langword="true" /> if the current thread has entered upgradeable mode; otherwise, <see langword="false" />.</returns>
    public bool IsUpgradeableReadLockHeld => this.RecursiveUpgradeCount > 0;

    /// <summary>Gets a value that indicates whether the current thread has entered the lock in write mode.</summary>
    /// <returns>
    /// <see langword="true" /> if the current thread has entered write mode; otherwise, <see langword="false" />.</returns>
    public bool IsWriteLockHeld => this.RecursiveWriteCount > 0;

    /// <summary>Gets a value that indicates the recursion policy for the current <see cref="T:System.Threading.ReaderWriterLockSlim" /> object.</summary>
    /// <returns>One of the enumeration values that specifies the lock recursion policy.</returns>
    public LockRecursionPolicy RecursionPolicy => this._fIsReentrant ? LockRecursionPolicy.SupportsRecursion : LockRecursionPolicy.NoRecursion;

    /// <summary>Gets the total number of unique threads that have entered the lock in read mode.</summary>
    /// <returns>The number of unique threads that have entered the lock in read mode.</returns>
    public int CurrentReadCount
    {
      get
      {
        int numReaders = (int) this.GetNumReaders();
        return this._upgradeLockOwnerId != -1 ? numReaders - 1 : numReaders;
      }
    }

    /// <summary>Gets the number of times the current thread has entered the lock in read mode, as an indication of recursion.</summary>
    /// <returns>0 (zero) if the current thread has not entered read mode, 1 if the thread has entered read mode but has not entered it recursively, or n if the thread has entered the lock recursively n - 1 times.</returns>
    public int RecursiveReadCount
    {
      get
      {
        int recursiveReadCount = 0;
        ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
        if (threadRwCount != null)
          recursiveReadCount = threadRwCount.readercount;
        return recursiveReadCount;
      }
    }

    /// <summary>Gets the number of times the current thread has entered the lock in upgradeable mode, as an indication of recursion.</summary>
    /// <returns>0 if the current thread has not entered upgradeable mode, 1 if the thread has entered upgradeable mode but has not entered it recursively, or n if the thread has entered upgradeable mode recursively n - 1 times.</returns>
    public int RecursiveUpgradeCount
    {
      get
      {
        if (this._fIsReentrant)
        {
          int recursiveUpgradeCount = 0;
          ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
          if (threadRwCount != null)
            recursiveUpgradeCount = threadRwCount.upgradecount;
          return recursiveUpgradeCount;
        }
        return Environment.CurrentManagedThreadId == this._upgradeLockOwnerId ? 1 : 0;
      }
    }

    /// <summary>Gets the number of times the current thread has entered the lock in write mode, as an indication of recursion.</summary>
    /// <returns>0 if the current thread has not entered write mode, 1 if the thread has entered write mode but has not entered it recursively, or n if the thread has entered write mode recursively n - 1 times.</returns>
    public int RecursiveWriteCount
    {
      get
      {
        if (this._fIsReentrant)
        {
          int recursiveWriteCount = 0;
          ReaderWriterCount threadRwCount = this.GetThreadRWCount(true);
          if (threadRwCount != null)
            recursiveWriteCount = threadRwCount.writercount;
          return recursiveWriteCount;
        }
        return Environment.CurrentManagedThreadId == this._writeLockOwnerId ? 1 : 0;
      }
    }

    /// <summary>Gets the total number of threads that are waiting to enter the lock in read mode.</summary>
    /// <returns>The total number of threads that are waiting to enter read mode.</returns>
    public int WaitingReadCount => (int) this._numReadWaiters;

    /// <summary>Gets the total number of threads that are waiting to enter the lock in upgradeable mode.</summary>
    /// <returns>The total number of threads that are waiting to enter upgradeable mode.</returns>
    public int WaitingUpgradeCount => (int) this._numUpgradeWaiters;

    /// <summary>Gets the total number of threads that are waiting to enter the lock in write mode.</summary>
    /// <returns>The total number of threads that are waiting to enter write mode.</returns>
    public int WaitingWriteCount => (int) this._numWriteWaiters;

    private struct TimeoutTracker
    {
      private readonly int _total;
      private readonly int _start;

      public TimeoutTracker(TimeSpan timeout)
      {
        long totalMilliseconds = (long) timeout.TotalMilliseconds;
        this._total = totalMilliseconds >= -1L && totalMilliseconds <= (long) int.MaxValue ? (int) totalMilliseconds : throw new ArgumentOutOfRangeException(nameof (timeout));
        if (this._total != -1 && this._total != 0)
          this._start = Environment.TickCount;
        else
          this._start = 0;
      }

      public TimeoutTracker(int millisecondsTimeout)
      {
        this._total = millisecondsTimeout >= -1 ? millisecondsTimeout : throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout));
        if (this._total != -1 && this._total != 0)
          this._start = Environment.TickCount;
        else
          this._start = 0;
      }

      public int RemainingMilliseconds
      {
        get
        {
          if (this._total == -1 || this._total == 0)
            return this._total;
          int num = Environment.TickCount - this._start;
          return num < 0 || num >= this._total ? 0 : this._total - num;
        }
      }

      public bool IsExpired => this.RemainingMilliseconds == 0;
    }

    private struct SpinLock
    {
      private int _isLocked;
      private int _enterDeprioritizationState;

      private static int GetEnterDeprioritizationStateChange(
        ReaderWriterLockSlim.EnterSpinLockReason reason)
      {
        switch (reason & ReaderWriterLockSlim.EnterSpinLockReason.OperationMask)
        {
          case ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead:
            return 0;
          case ReaderWriterLockSlim.EnterSpinLockReason.ExitAnyRead:
            return 1;
          case ReaderWriterLockSlim.EnterSpinLockReason.EnterWrite:
            return 65536;
          default:
            return 65537;
        }
      }

      private ushort EnterForEnterAnyReadDeprioritizedCount => (ushort) ((uint) this._enterDeprioritizationState >> 16);

      private ushort EnterForEnterAnyWriteDeprioritizedCount => (ushort) this._enterDeprioritizationState;

      private bool IsEnterDeprioritized(ReaderWriterLockSlim.EnterSpinLockReason reason)
      {
        switch (reason)
        {
          case ReaderWriterLockSlim.EnterSpinLockReason.EnterAnyRead:
            return this.EnterForEnterAnyReadDeprioritizedCount > (ushort) 0;
          case ReaderWriterLockSlim.EnterSpinLockReason.EnterWrite:
            return this.EnterForEnterAnyWriteDeprioritizedCount > (ushort) 0;
          case ReaderWriterLockSlim.EnterSpinLockReason.UpgradeToWrite:
            return this.EnterForEnterAnyWriteDeprioritizedCount > (ushort) 1;
          default:
            return false;
        }
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private bool TryEnter() => Interlocked.CompareExchange(ref this._isLocked, 1, 0) == 0;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void Enter(ReaderWriterLockSlim.EnterSpinLockReason reason)
      {
        if (this.TryEnter())
          return;
        this.EnterSpin(reason);
      }

      private void EnterSpin(ReaderWriterLockSlim.EnterSpinLockReason reason)
      {
        int deprioritizationStateChange = ReaderWriterLockSlim.SpinLock.GetEnterDeprioritizationStateChange(reason);
        if (deprioritizationStateChange != 0)
          Interlocked.Add(ref this._enterDeprioritizationState, deprioritizationStateChange);
        int processorCount = Environment.ProcessorCount;
        int num = 0;
        while (true)
        {
          if (num < 10 && processorCount > 1)
            Thread.SpinWait(20 * (num + 1));
          else if (num < 15)
            Thread.Sleep(0);
          else
            Thread.Sleep(1);
          if (!this.IsEnterDeprioritized(reason))
          {
            if (this._isLocked == 0 && this.TryEnter())
              break;
          }
          else if (num >= 20)
          {
            reason |= ReaderWriterLockSlim.EnterSpinLockReason.Wait;
            num = -1;
          }
          ++num;
        }
        if (deprioritizationStateChange == 0)
          return;
        Interlocked.Add(ref this._enterDeprioritizationState, -deprioritizationStateChange);
      }

      public void Exit() => Volatile.Write(ref this._isLocked, 0);
    }

    [Flags]
    private enum WaiterStates : byte
    {
      None = 0,
      NoWaiters = 1,
      WriteWaiterSignaled = 2,
      UpgradeableReadWaiterSignaled = 4,
    }

    private enum EnterSpinLockReason
    {
      EnterAnyRead = 0,
      ExitAnyRead = 1,
      EnterWrite = 2,
      UpgradeToWrite = 3,
      EnterRecursiveWrite = 4,
      ExitAnyWrite = 5,
      OperationMask = 7,
      Wait = 8,
    }

    private enum EnterLockType
    {
      Read,
      UpgradeableRead,
      Write,
      UpgradeToWrite,
    }
  }
}
