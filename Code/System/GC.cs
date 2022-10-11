// Decompiled with JetBrains decompiler
// Type: System.GC
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Controls the system garbage collector, a service that automatically reclaims unused memory.</summary>
  public static class GC
  {

    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void GetMemoryInfo(GCMemoryInfoData data, int kind);

    /// <summary>Gets garbage collection memory information.</summary>
    /// <returns>An object that contains information about the garbage collector's memory usage.</returns>
    public static GCMemoryInfo GetGCMemoryInfo() => GC.GetGCMemoryInfo(GCKind.Any);

    /// <summary>Gets garbage collection memory information.</summary>
    /// <param name="kind">The kind of collection for which to retrieve memory information.</param>
    /// <returns>An object that contains information about the garbage collector's memory usage.</returns>
    public static GCMemoryInfo GetGCMemoryInfo(GCKind kind)
    {
      if (kind < GCKind.Any || kind > GCKind.Background)
        throw new ArgumentOutOfRangeException(nameof (kind), SR.Format(SR.ArgumentOutOfRange_Bounds_Lower_Upper, (object) GCKind.Any, (object) GCKind.Background));
      GCMemoryInfoData data = new GCMemoryInfoData();
      GC.GetMemoryInfo(data, (int) kind);
      return new GCMemoryInfo(data);
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    internal static extern int _StartNoGCRegion(
      long totalSize,
      bool lohSizeKnown,
      long lohSize,
      bool disallowFullBlockingGC);

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    internal static extern int _EndNoGCRegion();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern Array AllocateNewArray(
      IntPtr typeHandle,
      int length,
      GC.GC_ALLOC_FLAGS flags);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetGenerationWR(IntPtr handle);

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern long GetTotalMemory();

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void _Collect(int generation, int mode);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int GetMaxGeneration();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int _CollectionCount(int generation, int getSpecialGCCount);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern ulong GetSegmentSize();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetLastGCPercentTimeInGC();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern ulong GetGenerationSize(int gen);

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void _AddMemoryPressure(ulong bytesAllocated);

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void _RemoveMemoryPressure(ulong bytesAllocated);

    /// <summary>Informs the runtime of a large allocation of unmanaged memory that should be taken into account when scheduling garbage collection.</summary>
    /// <param name="bytesAllocated">The incremental amount of unmanaged memory that has been allocated.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="bytesAllocated" /> is less than or equal to 0.
    /// 
    /// -or-
    /// 
    /// On a 32-bit computer, <paramref name="bytesAllocated" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    public static void AddMemoryPressure(long bytesAllocated)
    {
      if (bytesAllocated <= 0L)
        throw new ArgumentOutOfRangeException(nameof (bytesAllocated), SR.ArgumentOutOfRange_NeedPosNum);
      if (4 != IntPtr.Size)
        ;
      GC._AddMemoryPressure((ulong) bytesAllocated);
    }

    /// <summary>Informs the runtime that unmanaged memory has been released and no longer needs to be taken into account when scheduling garbage collection.</summary>
    /// <param name="bytesAllocated">The amount of unmanaged memory that has been released.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="bytesAllocated" /> is less than or equal to 0.
    /// 
    /// -or-
    /// 
    /// On a 32-bit computer, <paramref name="bytesAllocated" /> is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    public static void RemoveMemoryPressure(long bytesAllocated)
    {
      if (bytesAllocated <= 0L)
        throw new ArgumentOutOfRangeException(nameof (bytesAllocated), SR.ArgumentOutOfRange_NeedPosNum);
      if (4 != IntPtr.Size)
        ;
      GC._RemoveMemoryPressure((ulong) bytesAllocated);
    }


    #nullable enable
    /// <summary>Returns the current generation number of the specified object.</summary>
    /// <param name="obj">The object that generation information is retrieved for.</param>
    /// <returns>The current generation number of <paramref name="obj" />.</returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int GetGeneration(object obj);

    /// <summary>Forces an immediate garbage collection from generation 0 through a specified generation.</summary>
    /// <param name="generation">The number of the oldest generation to be garbage collected.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="generation" /> is not valid.</exception>
    public static void Collect(int generation) => GC.Collect(generation, GCCollectionMode.Default);

    /// <summary>Forces an immediate garbage collection of all generations.</summary>
    public static void Collect() => GC._Collect(-1, 2);

    /// <summary>Forces a garbage collection from generation 0 through a specified generation, at a time specified by a <see cref="T:System.GCCollectionMode" /> value.</summary>
    /// <param name="generation">The number of the oldest generation to be garbage collected.</param>
    /// <param name="mode">An enumeration value that specifies whether the garbage collection is forced (<see cref="F:System.GCCollectionMode.Default" /> or <see cref="F:System.GCCollectionMode.Forced" />) or optimized (<see cref="F:System.GCCollectionMode.Optimized" />).</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="generation" /> is not valid.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is not one of the <see cref="T:System.GCCollectionMode" /> values.</exception>
    public static void Collect(int generation, GCCollectionMode mode) => GC.Collect(generation, mode, true);

    /// <summary>Forces a garbage collection from generation 0 through a specified generation, at a time specified by a <see cref="T:System.GCCollectionMode" /> value, with a value specifying whether the collection should be blocking.</summary>
    /// <param name="generation">The number of the oldest generation to be garbage collected.</param>
    /// <param name="mode">An enumeration value that specifies whether the garbage collection is forced (<see cref="F:System.GCCollectionMode.Default" /> or <see cref="F:System.GCCollectionMode.Forced" />) or optimized (<see cref="F:System.GCCollectionMode.Optimized" />).</param>
    /// <param name="blocking">
    /// <see langword="true" /> to perform a blocking garbage collection; <see langword="false" /> to perform a background garbage collection where possible.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="generation" /> is not valid.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is not one of the <see cref="T:System.GCCollectionMode" /> values.</exception>
    public static void Collect(int generation, GCCollectionMode mode, bool blocking) => GC.Collect(generation, mode, blocking, false);

    /// <summary>Forces a garbage collection from generation 0 through a specified generation, at a time specified by a <see cref="T:System.GCCollectionMode" /> value, with values that specify whether the collection should be blocking and compacting.</summary>
    /// <param name="generation">The number of the oldest generation to be garbage collected.</param>
    /// <param name="mode">An enumeration value that specifies whether the garbage collection is forced (<see cref="F:System.GCCollectionMode.Default" /> or <see cref="F:System.GCCollectionMode.Forced" />) or optimized (<see cref="F:System.GCCollectionMode.Optimized" />).</param>
    /// <param name="blocking">
    /// <see langword="true" /> to perform a blocking garbage collection; <see langword="false" /> to perform a background garbage collection where possible.</param>
    /// <param name="compacting">
    /// <see langword="true" /> to compact the small object heap; <see langword="false" /> to sweep only.</param>
    public static void Collect(
      int generation,
      GCCollectionMode mode,
      bool blocking,
      bool compacting)
    {
      if (generation < 0)
        throw new ArgumentOutOfRangeException(nameof (generation), SR.ArgumentOutOfRange_GenericPositive);
      if (mode < GCCollectionMode.Default || mode > GCCollectionMode.Optimized)
        throw new ArgumentOutOfRangeException(nameof (mode), SR.ArgumentOutOfRange_Enum);
      int mode1 = 0;
      if (mode == GCCollectionMode.Optimized)
        mode1 |= 4;
      if (compacting)
        mode1 |= 8;
      if (blocking)
        mode1 |= 2;
      else if (!compacting)
        mode1 |= 1;
      GC._Collect(generation, mode1);
    }

    /// <summary>Returns the number of times garbage collection has occurred for the specified generation of objects.</summary>
    /// <param name="generation">The generation of objects for which the garbage collection count is to be determined.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="generation" /> is less than 0.</exception>
    /// <returns>The number of times garbage collection has occurred for the specified generation since the process was started.</returns>
    public static int CollectionCount(int generation) => generation >= 0 ? GC._CollectionCount(generation, 0) : throw new ArgumentOutOfRangeException(nameof (generation), SR.ArgumentOutOfRange_GenericPositive);

    /// <summary>References the specified object, which makes it ineligible for garbage collection from the start of the current routine to the point where this method is called.</summary>
    /// <param name="obj">The object to reference.</param>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void KeepAlive(object? obj)
    {
    }

    /// <summary>Returns the current generation number of the target of a specified weak reference.</summary>
    /// <param name="wo">A <see cref="T:System.WeakReference" /> that refers to the target object whose generation number is to be determined.</param>
    /// <exception cref="T:System.ArgumentException">Garbage collection has already been performed on <paramref name="wo" />.</exception>
    /// <returns>The current generation number of the target of <paramref name="wo" />.</returns>
    public static int GetGeneration(WeakReference wo)
    {
      int generationWr = GC.GetGenerationWR(wo.m_handle);
      GC.KeepAlive((object) wo);
      return generationWr;
    }

    /// <summary>Gets the maximum number of generations that the system currently supports.</summary>
    /// <returns>A value that ranges from zero to the maximum number of supported generations.</returns>
    public static int MaxGeneration => GC.GetMaxGeneration();

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void _WaitForPendingFinalizers();

    /// <summary>Suspends the current thread until the thread that is processing the queue of finalizers has emptied that queue.</summary>
    public static void WaitForPendingFinalizers() => GC._WaitForPendingFinalizers();


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void _SuppressFinalize(object o);


    #nullable enable
    /// <summary>Requests that the common language runtime not call the finalizer for the specified object.</summary>
    /// <param name="obj">The object whose finalizer must not be executed.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="obj" /> is <see langword="null" />.</exception>
    public static void SuppressFinalize(object obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      GC._SuppressFinalize(obj);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void _ReRegisterForFinalize(object o);


    #nullable enable
    /// <summary>Requests that the system call the finalizer for the specified object for which <see cref="M:System.GC.SuppressFinalize(System.Object)" /> has previously been called.</summary>
    /// <param name="obj">The object that a finalizer must be called for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="obj" /> is <see langword="null" />.</exception>
    public static void ReRegisterForFinalize(object obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      GC._ReRegisterForFinalize(obj);
    }

    /// <summary>Retrieves the number of bytes currently thought to be allocated. A parameter indicates whether this method can wait a short interval before returning, to allow the system to collect garbage and finalize objects.</summary>
    /// <param name="forceFullCollection">
    /// <see langword="true" /> to indicate that this method can wait for garbage collection to occur before returning; otherwise, <see langword="false" />.</param>
    /// <returns>A number that is the best available approximation of the number of bytes currently allocated in managed memory.</returns>
    public static long GetTotalMemory(bool forceFullCollection)
    {
      long totalMemory1 = GC.GetTotalMemory();
      if (!forceFullCollection)
        return totalMemory1;
      int num1 = 20;
      long totalMemory2 = totalMemory1;
      float num2;
      do
      {
        GC.WaitForPendingFinalizers();
        GC.Collect();
        long num3 = totalMemory2;
        totalMemory2 = GC.GetTotalMemory();
        num2 = (float) (totalMemory2 - num3) / (float) num3;
      }
      while (num1-- > 0 && (-0.05 >= (double) num2 || (double) num2 >= 0.05));
      return totalMemory2;
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern IntPtr _RegisterFrozenSegment(
      IntPtr sectionAddress,
      [NativeInteger] IntPtr sectionSize);

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void _UnregisterFrozenSegment(IntPtr segmentHandle);

    /// <summary>Gets the total number of bytes allocated to the current thread since the beginning of its lifetime.</summary>
    /// <returns>The total number of bytes allocated to the current thread since the beginning of its lifetime.</returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern long GetAllocatedBytesForCurrentThread();

    /// <summary>Gets a count of the bytes allocated over the lifetime of the process. The returned value does not include any native allocations.</summary>
    /// <param name="precise">If <see langword="true" />, gather a precise number; otherwise, gather an approximate count. Gathering a precise value entails a significant performance penalty.</param>
    /// <returns>The total number of bytes allocated over the lifetime of the process.</returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern long GetTotalAllocatedBytes(bool precise = false);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool _RegisterForFullGCNotification(
      int maxGenerationPercentage,
      int largeObjectHeapPercentage);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool _CancelFullGCNotification();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int _WaitForFullGCApproach(int millisecondsTimeout);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int _WaitForFullGCComplete(int millisecondsTimeout);

    /// <summary>Specifies that a garbage collection notification should be raised when conditions favor full garbage collection and when the collection has been completed.</summary>
    /// <param name="maxGenerationThreshold">A number between 1 and 99 that specifies when the notification should be raised based on the objects allocated in generation 2.</param>
    /// <param name="largeObjectHeapThreshold">A number between 1 and 99 that specifies when the notification should be raised based on objects allocated in the large object heap.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="maxGenerationThreshold" /> or <paramref name="largeObjectHeapThreshold" /> is not between 1 and 99.</exception>
    /// <exception cref="T:System.InvalidOperationException">This member is not available when concurrent garbage collection is enabled. See the &lt;gcConcurrent&gt; runtime setting for information about how to disable concurrent garbage collection.</exception>
    public static void RegisterForFullGCNotification(
      int maxGenerationThreshold,
      int largeObjectHeapThreshold)
    {
      if (maxGenerationThreshold <= 0 || maxGenerationThreshold >= 100)
        throw new ArgumentOutOfRangeException(nameof (maxGenerationThreshold), SR.Format(SR.ArgumentOutOfRange_Bounds_Lower_Upper, (object) 1, (object) 99));
      if (largeObjectHeapThreshold <= 0 || largeObjectHeapThreshold >= 100)
        throw new ArgumentOutOfRangeException(nameof (largeObjectHeapThreshold), SR.Format(SR.ArgumentOutOfRange_Bounds_Lower_Upper, (object) 1, (object) 99));
      if (!GC._RegisterForFullGCNotification(maxGenerationThreshold, largeObjectHeapThreshold))
        throw new InvalidOperationException(SR.InvalidOperation_NotWithConcurrentGC);
    }

    /// <summary>Cancels the registration of a garbage collection notification.</summary>
    /// <exception cref="T:System.InvalidOperationException">This member is not available when concurrent garbage collection is enabled. See the &lt;gcConcurrent&gt; runtime setting for information about how to disable concurrent garbage collection.</exception>
    public static void CancelFullGCNotification()
    {
      if (!GC._CancelFullGCNotification())
        throw new InvalidOperationException(SR.InvalidOperation_NotWithConcurrentGC);
    }

    /// <summary>Returns the status of a registered notification for determining whether a full, blocking garbage collection by the common language runtime is imminent.</summary>
    /// <returns>The status of the registered garbage collection notification.</returns>
    public static GCNotificationStatus WaitForFullGCApproach() => (GCNotificationStatus) GC._WaitForFullGCApproach(-1);

    /// <summary>Returns, in a specified time-out period, the status of a registered notification for determining whether a full, blocking garbage collection by the common language runtime is imminent.</summary>
    /// <param name="millisecondsTimeout">The length of time to wait before a notification status can be obtained. Specify -1 to wait indefinitely.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="millisecondsTimeout" /> must be either non-negative or less than or equal to <see cref="F:System.Int32.MaxValue" /> or -1.</exception>
    /// <returns>The status of the registered garbage collection notification.</returns>
    public static GCNotificationStatus WaitForFullGCApproach(
      int millisecondsTimeout)
    {
      return millisecondsTimeout >= -1 ? (GCNotificationStatus) GC._WaitForFullGCApproach(millisecondsTimeout) : throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
    }

    /// <summary>Returns the status of a registered notification for determining whether a full, blocking garbage collection by the common language runtime has completed.</summary>
    /// <returns>The status of the registered garbage collection notification.</returns>
    public static GCNotificationStatus WaitForFullGCComplete() => (GCNotificationStatus) GC._WaitForFullGCComplete(-1);

    /// <summary>Returns, in a specified time-out period, the status of a registered notification for determining whether a full, blocking garbage collection by common language the runtime has completed.</summary>
    /// <param name="millisecondsTimeout">The length of time to wait before a notification status can be obtained. Specify -1 to wait indefinitely.</param>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="millisecondsTimeout" /> must be either non-negative or less than or equal to <see cref="F:System.Int32.MaxValue" /> or -1.</exception>
    /// <returns>The status of the registered garbage collection notification.</returns>
    public static GCNotificationStatus WaitForFullGCComplete(
      int millisecondsTimeout)
    {
      return millisecondsTimeout >= -1 ? (GCNotificationStatus) GC._WaitForFullGCComplete(millisecondsTimeout) : throw new ArgumentOutOfRangeException(nameof (millisecondsTimeout), SR.ArgumentOutOfRange_NeedNonNegOrNegative1);
    }

    private static bool StartNoGCRegionWorker(
      long totalSize,
      bool hasLohSize,
      long lohSize,
      bool disallowFullBlockingGC)
    {
      if (totalSize <= 0L)
        throw new ArgumentOutOfRangeException(nameof (totalSize), "totalSize can't be zero or negative");
      if (hasLohSize)
      {
        if (lohSize <= 0L)
          throw new ArgumentOutOfRangeException(nameof (lohSize), "lohSize can't be zero or negative");
        if (lohSize > totalSize)
          throw new ArgumentOutOfRangeException(nameof (lohSize), "lohSize can't be greater than totalSize");
      }
      switch (GC._StartNoGCRegion(totalSize, hasLohSize, lohSize, disallowFullBlockingGC))
      {
        case 1:
          return false;
        case 2:
          throw new ArgumentOutOfRangeException(nameof (totalSize), "totalSize is too large. For more information about setting the maximum size, see \"Latency Modes\" in https://go.microsoft.com/fwlink/?LinkId=522706");
        case 3:
          throw new InvalidOperationException("The NoGCRegion mode was already in progress");
        default:
          return true;
      }
    }

    /// <summary>Attempts to disallow garbage collection during the execution of a critical path if a specified amount of memory is available.</summary>
    /// <param name="totalSize">The amount of memory in bytes to allocate without triggering a garbage collection. It must be less than or equal to the size of an ephemeral segment. For information on the size of an ephemeral segment, see the "Ephemeral generations and segments" section in the Fundamentals of Garbage Collection article.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="totalSize" /> exceeds the ephemeral segment size.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process is already in no GC region latency mode.</exception>
    /// <returns>
    /// <see langword="true" /> if the runtime was able to commit the required amount of memory and the garbage collector is able to enter no GC region latency mode; otherwise, <see langword="false" />.</returns>
    public static bool TryStartNoGCRegion(long totalSize) => GC.StartNoGCRegionWorker(totalSize, false, 0L, false);

    /// <summary>Attempts to disallow garbage collection during the execution of a critical path if a specified amount of memory is available for the large object heap and the small object heap.</summary>
    /// <param name="totalSize">The amount of memory in bytes to allocate without triggering a garbage collection. <paramref name="totalSize" /> -<paramref name="lohSize" /> must be less than or equal to the size of an ephemeral segment. For information on the size of an ephemeral segment, see the "Ephemeral generations and segments" section in the Fundamentals of Garbage Collection article.</param>
    /// <param name="lohSize">The number of bytes in <paramref name="totalSize" /> to use for large object heap (LOH) allocations.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="totalSize" /> - <paramref name="lohSize" /> exceeds the ephemeral segment size.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process is already in no GC region latency mode.</exception>
    /// <returns>
    /// <see langword="true" /> if the runtime was able to commit the required amount of memory and the garbage collector is able to enter no GC region latency mode; otherwise, <see langword="false" />.</returns>
    public static bool TryStartNoGCRegion(long totalSize, long lohSize) => GC.StartNoGCRegionWorker(totalSize, true, lohSize, false);

    /// <summary>Attempts to disallow garbage collection during the execution of a critical path if a specified amount of memory is available, and controls whether the garbage collector does a full blocking garbage collection if not enough memory is initially available.</summary>
    /// <param name="totalSize">The amount of memory in bytes to allocate without triggering a garbage collection. It must be less than or equal to the size of an ephemeral segment. For information on the size of an ephemeral segment, see the "Ephemeral generations and segments" section in the Fundamentals of Garbage Collection article.</param>
    /// <param name="disallowFullBlockingGC">
    /// <see langword="true" /> to omit a full blocking garbage collection if the garbage collector is initially unable to allocate <paramref name="totalSize" /> bytes; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="totalSize" /> exceeds the ephemeral segment size.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process is already in no GC region latency mode.</exception>
    /// <returns>
    /// <see langword="true" /> if the runtime was able to commit the required amount of memory and the garbage collector is able to enter no GC region latency mode; otherwise, <see langword="false" />.</returns>
    public static bool TryStartNoGCRegion(long totalSize, bool disallowFullBlockingGC) => GC.StartNoGCRegionWorker(totalSize, false, 0L, disallowFullBlockingGC);

    /// <summary>Attempts to disallow garbage collection during the execution of a critical path if a specified amount of memory is available for the large object heap and the small object heap, and controls whether the garbage collector does a full blocking garbage collection if not enough memory is initially available.</summary>
    /// <param name="totalSize">The amount of memory in bytes to allocate without triggering a garbage collection. <paramref name="totalSize" /> -<paramref name="lohSize" /> must be less than or equal to the size of an ephemeral segment. For information on the size of an ephemeral segment, see the "Ephemeral generations and segments" section in the Fundamentals of Garbage Collection article.</param>
    /// <param name="lohSize">The number of bytes in <paramref name="totalSize" /> to use for large object heap (LOH) allocations.</param>
    /// <param name="disallowFullBlockingGC">
    /// <see langword="true" /> to omit a full blocking garbage collection if the garbage collector is initially unable to allocate the specified memory on the small object heap (SOH) and LOH; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="totalSize" /> - <paramref name="lohSize" /> exceeds the ephemeral segment size.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process is already in no GC region latency mode.</exception>
    /// <returns>
    /// <see langword="true" /> if the runtime was able to commit the required amount of memory and the garbage collector is able to enter no GC region latency mode; otherwise, <see langword="false" />.</returns>
    public static bool TryStartNoGCRegion(
      long totalSize,
      long lohSize,
      bool disallowFullBlockingGC)
    {
      return GC.StartNoGCRegionWorker(totalSize, true, lohSize, disallowFullBlockingGC);
    }

    /// <summary>Ends the no GC region latency mode.</summary>
    /// <exception cref="T:System.InvalidOperationException">The garbage collector is not in no GC region latency mode.
    /// 
    /// -or-
    /// 
    /// The no GC region latency mode was ended previously because a garbage collection was induced.
    /// 
    /// -or-
    /// 
    /// A memory allocation exceeded the amount specified in the call to the <see cref="M:System.GC.TryStartNoGCRegion(System.Int64)" /> method.</exception>
    public static void EndNoGCRegion()
    {
      switch ((GC.EndNoGCRegionStatus) GC._EndNoGCRegion())
      {
        case GC.EndNoGCRegionStatus.NotInProgress:
          throw new InvalidOperationException("NoGCRegion mode must be set");
        case GC.EndNoGCRegionStatus.GCInduced:
          throw new InvalidOperationException("Garbage collection was induced in NoGCRegion mode");
        case GC.EndNoGCRegionStatus.AllocationExceeded:
          throw new InvalidOperationException("Allocated memory exceeds specified memory for NoGCRegion mode");
      }
    }

    /// <summary>Allocates an array while skipping zero-initialization, if possible.</summary>
    /// <param name="length">Specifies the length of the array.</param>
    /// <param name="pinned">Specifies whether the allocated array must be pinned.</param>
    /// <typeparam name="T">Specifies the type of the array element.</typeparam>
    /// <returns>An array object with uninitialized memory except if it contains references or if it's too small for unpinned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] AllocateUninitializedArray<T>(int length, bool pinned = false)
    {
      if (!pinned)
      {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
          return new T[length];
        if (length < 2048 / Unsafe.SizeOf<T>())
          return new T[length];
      }
      else if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        ThrowHelper.ThrowInvalidTypeWithPointersNotSupported(typeof (T));
      return AllocateNewUninitializedArray(length, pinned);


      #nullable disable
      static T[] AllocateNewUninitializedArray(int length, bool pinned)
      {
        GC.GC_ALLOC_FLAGS flags = GC.GC_ALLOC_FLAGS.GC_ALLOC_ZEROING_OPTIONAL;
        if (pinned)
          flags |= GC.GC_ALLOC_FLAGS.GC_ALLOC_PINNED_OBJECT_HEAP;
        return Unsafe.As<T[]>((object) GC.AllocateNewArray(typeof (T[]).TypeHandle.Value, length, flags));
      }
    }


    #nullable enable
    /// <summary>Allocates an array.</summary>
    /// <param name="length">Specifies the length of the array.</param>
    /// <param name="pinned">Specifies whether the allocated array must be pinned.</param>
    /// <typeparam name="T">Specifies the type of the array element.</typeparam>
    /// <returns>An array object.</returns>
    public static T[] AllocateArray<T>(int length, bool pinned = false)
    {
      GC.GC_ALLOC_FLAGS flags = GC.GC_ALLOC_FLAGS.GC_ALLOC_NO_FLAGS;
      if (pinned)
      {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
          ThrowHelper.ThrowInvalidTypeWithPointersNotSupported(typeof (T));
        flags = GC.GC_ALLOC_FLAGS.GC_ALLOC_PINNED_OBJECT_HEAP;
      }
      return Unsafe.As<T[]>((object) GC.AllocateNewArray(typeof (T[]).TypeHandle.Value, length, flags));
    }


    #nullable disable
    internal enum GC_ALLOC_FLAGS
    {
      GC_ALLOC_NO_FLAGS = 0,
      GC_ALLOC_ZEROING_OPTIONAL = 16, // 0x00000010
      GC_ALLOC_PINNED_OBJECT_HEAP = 64, // 0x00000040
    }

    private enum StartNoGCRegionStatus
    {
      Succeeded,
      NotEnoughMemory,
      AmountTooLarge,
      AlreadyInProgress,
    }

    private enum EndNoGCRegionStatus
    {
      Succeeded,
      NotInProgress,
      GCInduced,
      AllocationExceeded,
    }
  }
}
