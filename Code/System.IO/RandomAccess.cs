// Decompiled with JetBrains decompiler
// Type: System.IO.RandomAccess
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Microsoft.Win32.SafeHandles;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Strategies;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;


#nullable enable
namespace System.IO
{
  /// <summary>Provides offset-based APIs for reading and writing files in a thread-safe manner.</summary>
  public static class RandomAccess
  {

    #nullable disable
    private static readonly IOCompletionCallback s_callback = RandomAccess.AllocateCallback();
    private static readonly int s_cachedPageSize = Environment.SystemPageSize;


    #nullable enable
    /// <summary>Gets the length of the file in bytes.</summary>
    /// <param name="handle">The file handle.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <returns>A long value representing the length of the file in bytes.</returns>
    public static long GetLength(SafeFileHandle handle)
    {
      RandomAccess.ValidateInput(handle, 0L);
      return RandomAccess.GetFileLength(handle);
    }

    /// <summary>Reads a sequence of bytes from given file at given offset.</summary>
    /// <param name="handle">The file handle.</param>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the file.</param>
    /// <param name="fileOffset">The file position to read from.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileOffset" /> is negative.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="handle" /> was not opened for reading.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the file has been reached.</returns>
    public static int Read(SafeFileHandle handle, Span<byte> buffer, long fileOffset)
    {
      RandomAccess.ValidateInput(handle, fileOffset);
      return RandomAccess.ReadAtOffset(handle, buffer, fileOffset);
    }

    /// <summary>Reads a sequence of bytes from given file at given offset.</summary>
    /// <param name="handle">The file handle.</param>
    /// <param name="buffers">A list of memory buffers. When this method returns, the contents of the buffers are replaced by the bytes read from the file.</param>
    /// <param name="fileOffset">The file position to read from.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> or <paramref name="buffers" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileOffset" /> is negative.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="handle" /> was not opened for reading.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The total number of bytes read into the buffers. This can be less than the number of bytes allocated in the buffers if that many bytes are not currently available, or zero (0) if the end of the file has been reached.</returns>
    public static long Read(
      SafeFileHandle handle,
      IReadOnlyList<Memory<byte>> buffers,
      long fileOffset)
    {
      RandomAccess.ValidateInput(handle, fileOffset);
      RandomAccess.ValidateBuffers<Memory<byte>>(buffers);
      return RandomAccess.ReadScatterAtOffset(handle, buffers, fileOffset);
    }

    /// <summary>Reads a sequence of bytes from given file at given offset.</summary>
    /// <param name="handle">The file handle.</param>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the file.</param>
    /// <param name="fileOffset">The file position to read from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileOffset" /> is negative.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="handle" /> was not opened for reading.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the file has been reached.</returns>
    public static ValueTask<int> ReadAsync(
      SafeFileHandle handle,
      Memory<byte> buffer,
      long fileOffset,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      RandomAccess.ValidateInput(handle, fileOffset);
      return cancellationToken.IsCancellationRequested ? ValueTask.FromCanceled<int>(cancellationToken) : RandomAccess.ReadAtOffsetAsync(handle, buffer, fileOffset, cancellationToken);
    }

    /// <summary>Reads a sequence of bytes from given file at given offset.</summary>
    /// <param name="handle">The file handle.</param>
    /// <param name="buffers">A list of memory buffers. When this method returns, the contents of these buffers are replaced by the bytes read from the file.</param>
    /// <param name="fileOffset">The file position to read from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> or <paramref name="buffers" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileOffset" /> is negative.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="handle" /> was not opened for reading.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The total number of bytes read into the buffers. This can be less than the number of bytes allocated in the buffers if that many bytes are not currently available, or zero (0) if the end of the file has been reached.</returns>
    public static ValueTask<long> ReadAsync(
      SafeFileHandle handle,
      IReadOnlyList<Memory<byte>> buffers,
      long fileOffset,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      RandomAccess.ValidateInput(handle, fileOffset);
      RandomAccess.ValidateBuffers<Memory<byte>>(buffers);
      return cancellationToken.IsCancellationRequested ? ValueTask.FromCanceled<long>(cancellationToken) : RandomAccess.ReadScatterAtOffsetAsync(handle, buffers, fileOffset, cancellationToken);
    }

    /// <summary>Writes a sequence of bytes from given buffer to given file at given offset.</summary>
    /// <param name="handle">The file handle.</param>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the file.</param>
    /// <param name="fileOffset">The file position to write to.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileOffset" /> is negative.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="handle" /> was not opened for writing.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static void Write(SafeFileHandle handle, ReadOnlySpan<byte> buffer, long fileOffset)
    {
      RandomAccess.ValidateInput(handle, fileOffset);
      RandomAccess.WriteAtOffset(handle, buffer, fileOffset);
    }

    /// <summary>Writes a sequence of bytes from given buffers to given file at given offset.</summary>
    /// <param name="handle">The file handle.</param>
    /// <param name="buffers">A list of memory buffers. This method copies the contents of these buffers to the file.</param>
    /// <param name="fileOffset">The file position to write to.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> or <paramref name="buffers" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileOffset" /> is negative.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="handle" /> was not opened for writing.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public static void Write(
      SafeFileHandle handle,
      IReadOnlyList<ReadOnlyMemory<byte>> buffers,
      long fileOffset)
    {
      RandomAccess.ValidateInput(handle, fileOffset);
      RandomAccess.ValidateBuffers<ReadOnlyMemory<byte>>(buffers);
      RandomAccess.WriteGatherAtOffset(handle, buffers, fileOffset);
    }

    /// <summary>Writes a sequence of bytes from given buffer to given file at given offset.</summary>
    /// <param name="handle">The file handle.</param>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the file.</param>
    /// <param name="fileOffset">The file position to write to.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileOffset" /> is negative.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="handle" /> was not opened for writing.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A task representing the asynchronous completion of the write operation.</returns>
    public static ValueTask WriteAsync(
      SafeFileHandle handle,
      ReadOnlyMemory<byte> buffer,
      long fileOffset,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      RandomAccess.ValidateInput(handle, fileOffset);
      return cancellationToken.IsCancellationRequested ? ValueTask.FromCanceled(cancellationToken) : RandomAccess.WriteAtOffsetAsync(handle, buffer, fileOffset, cancellationToken);
    }

    /// <summary>Writes a sequence of bytes from given buffers to given file at given offset.</summary>
    /// <param name="handle">The file handle.</param>
    /// <param name="buffers">A list of memory buffers. This method copies the contents of these buffers to the file.</param>
    /// <param name="fileOffset">The file position to write to.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="handle" /> or <paramref name="buffers" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The file does not support seeking (pipe or socket).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="fileOffset" /> is negative.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="handle" /> was not opened for writing.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A task representing the asynchronous completion of the write operation.</returns>
    public static ValueTask WriteAsync(
      SafeFileHandle handle,
      IReadOnlyList<ReadOnlyMemory<byte>> buffers,
      long fileOffset,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      RandomAccess.ValidateInput(handle, fileOffset);
      RandomAccess.ValidateBuffers<ReadOnlyMemory<byte>>(buffers);
      return cancellationToken.IsCancellationRequested ? ValueTask.FromCanceled(cancellationToken) : RandomAccess.WriteGatherAtOffsetAsync(handle, buffers, fileOffset, cancellationToken);
    }


    #nullable disable
    private static void ValidateInput(SafeFileHandle handle, long fileOffset)
    {
      if (handle == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.handle);
      else if (handle.IsInvalid)
        ThrowHelper.ThrowArgumentException_InvalidHandle(nameof (handle));
      else if (!handle.CanSeek)
      {
        if (handle.IsClosed)
          ThrowHelper.ThrowObjectDisposedException_FileClosed();
        ThrowHelper.ThrowNotSupportedException_UnseekableStream();
      }
      else
      {
        if (fileOffset >= 0L)
          return;
        ThrowHelper.ThrowArgumentOutOfRangeException_NeedNonNegNum(nameof (fileOffset));
      }
    }

    private static void ValidateBuffers<T>(IReadOnlyList<T> buffers)
    {
      if (buffers != null)
        return;
      ThrowHelper.ThrowArgumentNullException(ExceptionArgument.buffers);
    }

    private static ValueTask<int> ScheduleSyncReadAtOffsetAsync(
      SafeFileHandle handle,
      Memory<byte> buffer,
      long fileOffset,
      CancellationToken cancellationToken,
      OSFileStreamStrategy strategy)
    {
      return handle.GetThreadPoolValueTaskSource().QueueRead(buffer, fileOffset, cancellationToken, strategy);
    }

    private static ValueTask<long> ScheduleSyncReadScatterAtOffsetAsync(
      SafeFileHandle handle,
      IReadOnlyList<Memory<byte>> buffers,
      long fileOffset,
      CancellationToken cancellationToken)
    {
      return handle.GetThreadPoolValueTaskSource().QueueReadScatter(buffers, fileOffset, cancellationToken);
    }

    private static ValueTask ScheduleSyncWriteAtOffsetAsync(
      SafeFileHandle handle,
      ReadOnlyMemory<byte> buffer,
      long fileOffset,
      CancellationToken cancellationToken,
      OSFileStreamStrategy strategy)
    {
      return handle.GetThreadPoolValueTaskSource().QueueWrite(buffer, fileOffset, cancellationToken, strategy);
    }

    private static ValueTask ScheduleSyncWriteGatherAtOffsetAsync(
      SafeFileHandle handle,
      IReadOnlyList<ReadOnlyMemory<byte>> buffers,
      long fileOffset,
      CancellationToken cancellationToken)
    {
      return handle.GetThreadPoolValueTaskSource().QueueWriteGather(buffers, fileOffset, cancellationToken);
    }

    internal static unsafe long GetFileLength(SafeFileHandle handle)
    {
      Interop.Kernel32.FILE_STANDARD_INFO fileStandardInfo;
      return Interop.Kernel32.GetFileInformationByHandleEx(handle, 1, (void*) &fileStandardInfo, (uint) sizeof (Interop.Kernel32.FILE_STANDARD_INFO)) ? fileStandardInfo.EndOfFile : throw Win32Marshal.GetExceptionForLastWin32Error(handle.Path);
    }

    internal static unsafe int ReadAtOffset(
      SafeFileHandle handle,
      Span<byte> buffer,
      long fileOffset)
    {
      if (handle.IsAsync)
        return RandomAccess.ReadSyncUsingAsyncHandle(handle, buffer, fileOffset);
      NativeOverlapped overlappedForSyncHandle = RandomAccess.GetNativeOverlappedForSyncHandle(handle, fileOffset);
      fixed (byte* bytes = &MemoryMarshal.GetReference<byte>(buffer))
      {
        int numBytesRead;
        if (Interop.Kernel32.ReadFile((SafeHandle) handle, bytes, buffer.Length, out numBytesRead, &overlappedForSyncHandle) != 0)
          return numBytesRead;
        int disposeHandleIfInvalid = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
        switch (disposeHandleIfInvalid)
        {
          case 38:
            return numBytesRead;
          case 109:
            return 0;
          default:
            throw Win32Marshal.GetExceptionForWin32Error(disposeHandleIfInvalid, handle.Path);
        }
      }
    }

    private static unsafe int ReadSyncUsingAsyncHandle(
      SafeFileHandle handle,
      Span<byte> buffer,
      long fileOffset)
    {
      handle.EnsureThreadPoolBindingInitialized();
      RandomAccess.CallbackResetEvent resetEvent = new RandomAccess.CallbackResetEvent(handle.ThreadPoolBinding);
      NativeOverlapped* nativeOverlappedPtr = (NativeOverlapped*) null;
      try
      {
        nativeOverlappedPtr = RandomAccess.GetNativeOverlappedForAsyncHandle(handle, fileOffset, resetEvent);
        fixed (byte* bytes = &MemoryMarshal.GetReference<byte>(buffer))
        {
          Interop.Kernel32.ReadFile((SafeHandle) handle, bytes, buffer.Length, IntPtr.Zero, nativeOverlappedPtr);
          int errorCode = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          if (errorCode == 997)
          {
            resetEvent.WaitOne();
            errorCode = 0;
          }
          if (errorCode == 0)
          {
            int lpNumberOfBytesTransferred = 0;
            if (Interop.Kernel32.GetOverlappedResult(handle, nativeOverlappedPtr, ref lpNumberOfBytesTransferred, false))
              return lpNumberOfBytesTransferred;
            errorCode = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          }
          else
            resetEvent.ReleaseRefCount(nativeOverlappedPtr);
          if (errorCode != 38 && errorCode != 109)
            throw Win32Marshal.GetExceptionForWin32Error(errorCode, handle.Path);
          nativeOverlappedPtr->InternalLow = IntPtr.Zero;
          return 0;
        }
      }
      finally
      {
        if ((IntPtr) nativeOverlappedPtr != IntPtr.Zero)
          resetEvent.ReleaseRefCount(nativeOverlappedPtr);
        resetEvent.Dispose();
      }
    }

    internal static unsafe void WriteAtOffset(
      SafeFileHandle handle,
      ReadOnlySpan<byte> buffer,
      long fileOffset)
    {
      if (buffer.IsEmpty)
        return;
      if (handle.IsAsync)
      {
        RandomAccess.WriteSyncUsingAsyncHandle(handle, buffer, fileOffset);
      }
      else
      {
        NativeOverlapped overlappedForSyncHandle = RandomAccess.GetNativeOverlappedForSyncHandle(handle, fileOffset);
        fixed (byte* bytes = &MemoryMarshal.GetReference<byte>(buffer))
        {
          if (Interop.Kernel32.WriteFile((SafeHandle) handle, bytes, buffer.Length, out int _, &overlappedForSyncHandle) != 0)
            return;
          int disposeHandleIfInvalid = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          if (disposeHandleIfInvalid != 232)
            throw Win32Marshal.GetExceptionForWin32Error(disposeHandleIfInvalid, handle.Path);
        }
      }
    }

    private static unsafe void WriteSyncUsingAsyncHandle(
      SafeFileHandle handle,
      ReadOnlySpan<byte> buffer,
      long fileOffset)
    {
      if (buffer.IsEmpty)
        return;
      handle.EnsureThreadPoolBindingInitialized();
      RandomAccess.CallbackResetEvent resetEvent = new RandomAccess.CallbackResetEvent(handle.ThreadPoolBinding);
      NativeOverlapped* nativeOverlappedPtr = (NativeOverlapped*) null;
      try
      {
        nativeOverlappedPtr = RandomAccess.GetNativeOverlappedForAsyncHandle(handle, fileOffset, resetEvent);
        fixed (byte* bytes = &MemoryMarshal.GetReference<byte>(buffer))
        {
          Interop.Kernel32.WriteFile((SafeHandle) handle, bytes, buffer.Length, IntPtr.Zero, nativeOverlappedPtr);
          int errorCode = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          if (errorCode == 997)
          {
            resetEvent.WaitOne();
            errorCode = 0;
          }
          if (errorCode == 0)
          {
            int lpNumberOfBytesTransferred = 0;
            if (Interop.Kernel32.GetOverlappedResult(handle, nativeOverlappedPtr, ref lpNumberOfBytesTransferred, false))
              return;
            errorCode = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          }
          else
            resetEvent.ReleaseRefCount(nativeOverlappedPtr);
          if (errorCode == 87)
            throw new IOException(SR.IO_FileTooLong);
          if (errorCode != 232)
            throw Win32Marshal.GetExceptionForWin32Error(errorCode, handle.Path);
        }
      }
      finally
      {
        if ((IntPtr) nativeOverlappedPtr != IntPtr.Zero)
          resetEvent.ReleaseRefCount(nativeOverlappedPtr);
        resetEvent.Dispose();
      }
    }

    internal static ValueTask<int> ReadAtOffsetAsync(
      SafeFileHandle handle,
      Memory<byte> buffer,
      long fileOffset,
      CancellationToken cancellationToken,
      OSFileStreamStrategy strategy = null)
    {
      if (!handle.IsAsync)
        return RandomAccess.ScheduleSyncReadAtOffsetAsync(handle, buffer, fileOffset, cancellationToken, strategy);
      (SafeFileHandle.OverlappedValueTaskSource overlappedValueTaskSource, int errorCode) = RandomAccess.QueueAsyncReadFile(handle, buffer, fileOffset, cancellationToken, strategy);
      if (overlappedValueTaskSource != null)
        return new ValueTask<int>((IValueTaskSource<int>) overlappedValueTaskSource, overlappedValueTaskSource.Version);
      return errorCode == 0 ? ValueTask.FromResult<int>(0) : ValueTask.FromException<int>(Win32Marshal.GetExceptionForWin32Error(errorCode, handle.Path));
    }

    private static unsafe (SafeFileHandle.OverlappedValueTaskSource vts, int errorCode) QueueAsyncReadFile(
      SafeFileHandle handle,
      Memory<byte> buffer,
      long fileOffset,
      CancellationToken cancellationToken,
      OSFileStreamStrategy strategy)
    {
      handle.EnsureThreadPoolBindingInitialized();
      SafeFileHandle.OverlappedValueTaskSource overlappedValueTaskSource = handle.GetOverlappedValueTaskSource();
      int num = 0;
      try
      {
        NativeOverlapped* overlapped = overlappedValueTaskSource.PrepareForOperation((ReadOnlyMemory<byte>) buffer, fileOffset, strategy);
        if (Interop.Kernel32.ReadFile((SafeHandle) handle, (byte*) overlappedValueTaskSource._memoryHandle.Pointer, buffer.Length, IntPtr.Zero, overlapped) == 0)
        {
          num = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          switch (num)
          {
            case 38:
            case 109:
              overlapped->InternalLow = IntPtr.Zero;
              overlappedValueTaskSource.Dispose();
              return ((SafeFileHandle.OverlappedValueTaskSource) null, 0);
            case 997:
              overlappedValueTaskSource.RegisterForCancellation(cancellationToken);
              break;
            default:
              overlappedValueTaskSource.Dispose();
              return ((SafeFileHandle.OverlappedValueTaskSource) null, num);
          }
        }
      }
      catch
      {
        overlappedValueTaskSource.Dispose();
        throw;
      }
      finally
      {
        if (num != 997 && num != 0 && strategy != null)
          strategy.OnIncompleteOperation(buffer.Length, 0);
      }
      overlappedValueTaskSource.FinishedScheduling();
      return (overlappedValueTaskSource, -1);
    }

    internal static ValueTask WriteAtOffsetAsync(
      SafeFileHandle handle,
      ReadOnlyMemory<byte> buffer,
      long fileOffset,
      CancellationToken cancellationToken,
      OSFileStreamStrategy strategy = null)
    {
      if (!handle.IsAsync)
        return RandomAccess.ScheduleSyncWriteAtOffsetAsync(handle, buffer, fileOffset, cancellationToken, strategy);
      (SafeFileHandle.OverlappedValueTaskSource overlappedValueTaskSource, int errorCode) = RandomAccess.QueueAsyncWriteFile(handle, buffer, fileOffset, cancellationToken, strategy);
      if (overlappedValueTaskSource != null)
        return new ValueTask((IValueTaskSource) overlappedValueTaskSource, overlappedValueTaskSource.Version);
      return errorCode == 0 ? ValueTask.CompletedTask : ValueTask.FromException(Win32Marshal.GetExceptionForWin32Error(errorCode, handle.Path));
    }

    private static unsafe (SafeFileHandle.OverlappedValueTaskSource vts, int errorCode) QueueAsyncWriteFile(
      SafeFileHandle handle,
      ReadOnlyMemory<byte> buffer,
      long fileOffset,
      CancellationToken cancellationToken,
      OSFileStreamStrategy strategy)
    {
      handle.EnsureThreadPoolBindingInitialized();
      SafeFileHandle.OverlappedValueTaskSource overlappedValueTaskSource = handle.GetOverlappedValueTaskSource();
      int num = 0;
      try
      {
        NativeOverlapped* lpOverlapped = overlappedValueTaskSource.PrepareForOperation(buffer, fileOffset, strategy);
        if (Interop.Kernel32.WriteFile((SafeHandle) handle, (byte*) overlappedValueTaskSource._memoryHandle.Pointer, buffer.Length, IntPtr.Zero, lpOverlapped) == 0)
        {
          num = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          switch (num)
          {
            case 232:
              overlappedValueTaskSource.Dispose();
              return ((SafeFileHandle.OverlappedValueTaskSource) null, 0);
            case 997:
              overlappedValueTaskSource.RegisterForCancellation(cancellationToken);
              break;
            default:
              overlappedValueTaskSource.Dispose();
              return ((SafeFileHandle.OverlappedValueTaskSource) null, num);
          }
        }
      }
      catch
      {
        overlappedValueTaskSource.Dispose();
        throw;
      }
      finally
      {
        if (num != 997 && num != 0 && strategy != null)
          strategy.OnIncompleteOperation(buffer.Length, 0);
      }
      overlappedValueTaskSource.FinishedScheduling();
      return (overlappedValueTaskSource, -1);
    }

    internal static long ReadScatterAtOffset(
      SafeFileHandle handle,
      IReadOnlyList<Memory<byte>> buffers,
      long fileOffset)
    {
      long num1 = 0;
      int count = buffers.Count;
      for (int index = 0; index < count; ++index)
      {
        Span<byte> span = buffers[index].Span;
        int num2 = RandomAccess.ReadAtOffset(handle, span, fileOffset + num1);
        num1 += (long) num2;
        if (num2 != span.Length)
          break;
      }
      return num1;
    }

    internal static void WriteGatherAtOffset(
      SafeFileHandle handle,
      IReadOnlyList<ReadOnlyMemory<byte>> buffers,
      long fileOffset)
    {
      int num = 0;
      int count = buffers.Count;
      for (int index = 0; index < count; ++index)
      {
        ReadOnlySpan<byte> span = buffers[index].Span;
        RandomAccess.WriteAtOffset(handle, span, fileOffset + (long) num);
        num += span.Length;
      }
    }

    private static bool CanUseScatterGatherWindowsAPIs(SafeFileHandle handle) => handle.IsAsync && (handle.GetFileOptions() & (FileOptions) 536870912) != 0;

    private static unsafe bool TryPrepareScatterGatherBuffers<T, THandler>(
      IReadOnlyList<T> buffers,
      THandler handler,
      [NotNullWhen(true)] out MemoryHandle[] handlesToDispose,
      out IntPtr segmentsPtr,
      out int totalBytes)
      where THandler : struct, RandomAccess.IMemoryHandler<T>
    {
      int cachedPageSize = RandomAccess.s_cachedPageSize;
      long num1 = (long) (cachedPageSize - 1);
      int count = buffers.Count;
      handlesToDispose = (MemoryHandle[]) null;
      segmentsPtr = IntPtr.Zero;
      totalBytes = 0;
      long* segmentsPtr1 = (long*) null;
      bool flag = false;
      try
      {
        long num2 = 0;
        for (int index = 0; index < count; ++index)
        {
          T memory = buffers[index];
          int length = handler.GetLength(in memory);
          num2 += (long) length;
          if (length != cachedPageSize || num2 > (long) int.MaxValue)
            return false;
          MemoryHandle memoryHandle = handler.Pin(in memory);
          long pointer = (long) memoryHandle.Pointer;
          if ((pointer & num1) != 0L)
          {
            memoryHandle.Dispose();
            return false;
          }
          (handlesToDispose ?? (handlesToDispose = new MemoryHandle[count]))[index] = memoryHandle;
          if ((IntPtr) segmentsPtr1 == IntPtr.Zero)
          {
            segmentsPtr1 = (long*) NativeMemory.Alloc((UIntPtr) count + new UIntPtr(1), new UIntPtr(8));
            segmentsPtr1[count] = 0L;
          }
          segmentsPtr1[index] = pointer;
        }
        segmentsPtr = (IntPtr) (void*) segmentsPtr1;
        totalBytes = (int) num2;
        flag = true;
        return handlesToDispose != null;
      }
      finally
      {
        if (!flag)
          RandomAccess.CleanupScatterGatherBuffers(handlesToDispose, (IntPtr) (void*) segmentsPtr1);
      }
    }

    private static unsafe void CleanupScatterGatherBuffers(
      MemoryHandle[] handlesToDispose,
      IntPtr segmentsPtr)
    {
      if (handlesToDispose != null)
      {
        foreach (MemoryHandle memoryHandle in handlesToDispose)
          memoryHandle.Dispose();
      }
      if (!(segmentsPtr != IntPtr.Zero))
        return;
      NativeMemory.Free((void*) segmentsPtr);
    }

    private static ValueTask<long> ReadScatterAtOffsetAsync(
      SafeFileHandle handle,
      IReadOnlyList<Memory<byte>> buffers,
      long fileOffset,
      CancellationToken cancellationToken)
    {
      if (!handle.IsAsync)
        return RandomAccess.ScheduleSyncReadScatterAtOffsetAsync(handle, buffers, fileOffset, cancellationToken);
      MemoryHandle[] handlesToDispose;
      IntPtr segmentsPtr;
      int totalBytes;
      return RandomAccess.CanUseScatterGatherWindowsAPIs(handle) && RandomAccess.TryPrepareScatterGatherBuffers<Memory<byte>, RandomAccess.MemoryHandler>(buffers, new RandomAccess.MemoryHandler(), out handlesToDispose, out segmentsPtr, out totalBytes) ? RandomAccess.ReadScatterAtOffsetSingleSyscallAsync(handle, handlesToDispose, segmentsPtr, fileOffset, totalBytes, cancellationToken) : RandomAccess.ReadScatterAtOffsetMultipleSyscallsAsync(handle, buffers, fileOffset, cancellationToken);
    }

    private static async ValueTask<long> ReadScatterAtOffsetSingleSyscallAsync(
      SafeFileHandle handle,
      MemoryHandle[] handlesToDispose,
      IntPtr segmentsPtr,
      long fileOffset,
      int totalBytes,
      CancellationToken cancellationToken)
    {
      long num;
      try
      {
        num = (long) await RandomAccess.ReadFileScatterAsync(handle, segmentsPtr, totalBytes, fileOffset, cancellationToken).ConfigureAwait(false);
      }
      finally
      {
        RandomAccess.CleanupScatterGatherBuffers(handlesToDispose, segmentsPtr);
      }
      return num;
    }

    private static unsafe ValueTask<int> ReadFileScatterAsync(
      SafeFileHandle handle,
      IntPtr segmentsPtr,
      int bytesToRead,
      long fileOffset,
      CancellationToken cancellationToken)
    {
      handle.EnsureThreadPoolBindingInitialized();
      SafeFileHandle.OverlappedValueTaskSource overlappedValueTaskSource = handle.GetOverlappedValueTaskSource();
      try
      {
        NativeOverlapped* lpOverlapped = overlappedValueTaskSource.PrepareForOperation((ReadOnlyMemory<byte>) Memory<byte>.Empty, fileOffset);
        if (Interop.Kernel32.ReadFileScatter((SafeHandle) handle, (long*) (void*) segmentsPtr, bytesToRead, IntPtr.Zero, lpOverlapped) == 0)
        {
          int disposeHandleIfInvalid = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          switch (disposeHandleIfInvalid)
          {
            case 38:
            case 109:
              lpOverlapped->InternalLow = IntPtr.Zero;
              overlappedValueTaskSource.Dispose();
              return ValueTask.FromResult<int>(0);
            case 997:
              overlappedValueTaskSource.RegisterForCancellation(cancellationToken);
              break;
            default:
              overlappedValueTaskSource.Dispose();
              return ValueTask.FromException<int>(Win32Marshal.GetExceptionForWin32Error(disposeHandleIfInvalid, handle.Path));
          }
        }
      }
      catch
      {
        overlappedValueTaskSource.Dispose();
        throw;
      }
      overlappedValueTaskSource.FinishedScheduling();
      return new ValueTask<int>((IValueTaskSource<int>) overlappedValueTaskSource, overlappedValueTaskSource.Version);
    }

    private static async ValueTask<long> ReadScatterAtOffsetMultipleSyscallsAsync(
      SafeFileHandle handle,
      IReadOnlyList<Memory<byte>> buffers,
      long fileOffset,
      CancellationToken cancellationToken)
    {
      long total = 0;
      int buffersCount = buffers.Count;
      for (int i = 0; i < buffersCount; ++i)
      {
        Memory<byte> buffer = buffers[i];
        int num = await RandomAccess.ReadAtOffsetAsync(handle, buffer, fileOffset + total, cancellationToken).ConfigureAwait(false);
        total += (long) num;
        if (num == buffer.Length)
          buffer = new Memory<byte>();
        else
          break;
      }
      return total;
    }

    private static ValueTask WriteGatherAtOffsetAsync(
      SafeFileHandle handle,
      IReadOnlyList<ReadOnlyMemory<byte>> buffers,
      long fileOffset,
      CancellationToken cancellationToken)
    {
      if (!handle.IsAsync)
        return RandomAccess.ScheduleSyncWriteGatherAtOffsetAsync(handle, buffers, fileOffset, cancellationToken);
      MemoryHandle[] handlesToDispose;
      IntPtr segmentsPtr;
      int totalBytes;
      return RandomAccess.CanUseScatterGatherWindowsAPIs(handle) && RandomAccess.TryPrepareScatterGatherBuffers<ReadOnlyMemory<byte>, RandomAccess.ReadOnlyMemoryHandler>(buffers, new RandomAccess.ReadOnlyMemoryHandler(), out handlesToDispose, out segmentsPtr, out totalBytes) ? RandomAccess.WriteGatherAtOffsetSingleSyscallAsync(handle, handlesToDispose, segmentsPtr, fileOffset, totalBytes, cancellationToken) : RandomAccess.WriteGatherAtOffsetMultipleSyscallsAsync(handle, buffers, fileOffset, cancellationToken);
    }

    private static async ValueTask WriteGatherAtOffsetSingleSyscallAsync(
      SafeFileHandle handle,
      MemoryHandle[] handlesToDispose,
      IntPtr segmentsPtr,
      long fileOffset,
      int totalBytes,
      CancellationToken cancellationToken)
    {
      try
      {
        await RandomAccess.WriteFileGatherAsync(handle, segmentsPtr, totalBytes, fileOffset, cancellationToken).ConfigureAwait(false);
      }
      finally
      {
        RandomAccess.CleanupScatterGatherBuffers(handlesToDispose, segmentsPtr);
      }
    }

    private static unsafe ValueTask WriteFileGatherAsync(
      SafeFileHandle handle,
      IntPtr segmentsPtr,
      int bytesToWrite,
      long fileOffset,
      CancellationToken cancellationToken)
    {
      handle.EnsureThreadPoolBindingInitialized();
      SafeFileHandle.OverlappedValueTaskSource overlappedValueTaskSource = handle.GetOverlappedValueTaskSource();
      try
      {
        NativeOverlapped* lpOverlapped = overlappedValueTaskSource.PrepareForOperation(ReadOnlyMemory<byte>.Empty, fileOffset);
        if (Interop.Kernel32.WriteFileGather((SafeHandle) handle, (long*) (void*) segmentsPtr, bytesToWrite, IntPtr.Zero, lpOverlapped) == 0)
        {
          int disposeHandleIfInvalid = FileStreamHelpers.GetLastWin32ErrorAndDisposeHandleIfInvalid(handle);
          if (disposeHandleIfInvalid == 997)
          {
            overlappedValueTaskSource.RegisterForCancellation(cancellationToken);
          }
          else
          {
            overlappedValueTaskSource.Dispose();
            return disposeHandleIfInvalid == 232 ? ValueTask.CompletedTask : ValueTask.FromException(SafeFileHandle.OverlappedValueTaskSource.GetIOError(disposeHandleIfInvalid, (string) null));
          }
        }
      }
      catch
      {
        overlappedValueTaskSource.Dispose();
        throw;
      }
      overlappedValueTaskSource.FinishedScheduling();
      return new ValueTask((IValueTaskSource) overlappedValueTaskSource, overlappedValueTaskSource.Version);
    }

    private static async ValueTask WriteGatherAtOffsetMultipleSyscallsAsync(
      SafeFileHandle handle,
      IReadOnlyList<ReadOnlyMemory<byte>> buffers,
      long fileOffset,
      CancellationToken cancellationToken)
    {
      int buffersCount = buffers.Count;
      for (int i = 0; i < buffersCount; ++i)
      {
        ReadOnlyMemory<byte> rom = buffers[i];
        await RandomAccess.WriteAtOffsetAsync(handle, rom, fileOffset, cancellationToken).ConfigureAwait(false);
        fileOffset += (long) rom.Length;
        rom = new ReadOnlyMemory<byte>();
      }
    }

    private static unsafe NativeOverlapped* GetNativeOverlappedForAsyncHandle(
      SafeFileHandle handle,
      long fileOffset,
      RandomAccess.CallbackResetEvent resetEvent)
    {
      NativeOverlapped* overlappedForAsyncHandle = handle.ThreadPoolBinding.UnsafeAllocateNativeOverlapped(RandomAccess.s_callback, (object) resetEvent, (object) null);
      if (handle.CanSeek)
      {
        overlappedForAsyncHandle->OffsetLow = (int) fileOffset;
        overlappedForAsyncHandle->OffsetHigh = (int) (fileOffset >> 32);
      }
      overlappedForAsyncHandle->EventHandle = resetEvent.SafeWaitHandle.DangerousGetHandle();
      return overlappedForAsyncHandle;
    }

    private static NativeOverlapped GetNativeOverlappedForSyncHandle(
      SafeFileHandle handle,
      long fileOffset)
    {
      NativeOverlapped overlappedForSyncHandle = new NativeOverlapped();
      if (handle.CanSeek)
      {
        overlappedForSyncHandle.OffsetLow = (int) fileOffset;
        overlappedForSyncHandle.OffsetHigh = (int) (fileOffset >> 32);
      }
      return overlappedForSyncHandle;
    }

    private static unsafe IOCompletionCallback AllocateCallback()
    {
      return new IOCompletionCallback(Callback);

      static unsafe void Callback(uint errorCode, uint numBytes, NativeOverlapped* pOverlapped) => ((RandomAccess.CallbackResetEvent) ThreadPoolBoundHandle.GetNativeOverlappedState(pOverlapped)).ReleaseRefCount(pOverlapped);
    }

    private sealed class CallbackResetEvent : EventWaitHandle
    {
      private readonly ThreadPoolBoundHandle _threadPoolBoundHandle;
      private int _freeWhenZero = 2;

      internal CallbackResetEvent(ThreadPoolBoundHandle threadPoolBoundHandle)
        : base(false, EventResetMode.ManualReset)
      {
        this._threadPoolBoundHandle = threadPoolBoundHandle;
      }

      internal unsafe void ReleaseRefCount(NativeOverlapped* pOverlapped)
      {
        if (Interlocked.Decrement(ref this._freeWhenZero) != 0)
          return;
        this._threadPoolBoundHandle.FreeNativeOverlapped(pOverlapped);
      }
    }

    private interface IMemoryHandler<T>
    {
      int GetLength(in T memory);

      MemoryHandle Pin(in T memory);
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private readonly struct MemoryHandler : RandomAccess.IMemoryHandler<Memory<byte>>
    {
      public int GetLength(in Memory<byte> memory) => memory.Length;

      public MemoryHandle Pin(in Memory<byte> memory) => memory.Pin();

      int RandomAccess.IMemoryHandler<Memory<byte>>.GetLength(
        in Memory<byte> memory)
      {
        return this.GetLength(in memory);
      }

      MemoryHandle RandomAccess.IMemoryHandler<Memory<byte>>.Pin(
        in Memory<byte> memory)
      {
        return this.Pin(in memory);
      }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private readonly struct ReadOnlyMemoryHandler : RandomAccess.IMemoryHandler<ReadOnlyMemory<byte>>
    {
      public int GetLength(in ReadOnlyMemory<byte> memory) => memory.Length;

      public MemoryHandle Pin(in ReadOnlyMemory<byte> memory) => memory.Pin();

      int RandomAccess.IMemoryHandler<ReadOnlyMemory<byte>>.GetLength(
        in ReadOnlyMemory<byte> memory)
      {
        return this.GetLength(in memory);
      }

      MemoryHandle RandomAccess.IMemoryHandler<ReadOnlyMemory<byte>>.Pin(
        in ReadOnlyMemory<byte> memory)
      {
        return this.Pin(in memory);
      }
    }
  }
}
