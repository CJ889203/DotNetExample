// Decompiled with JetBrains decompiler
// Type: System.IO.Stream
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Provides a generic view of a sequence of bytes. This is an abstract class.</summary>
  public abstract class Stream : MarshalByRefObject, IDisposable, IAsyncDisposable
  {
    /// <summary>A <see langword="Stream" /> with no backing store.</summary>
    public static readonly Stream Null = (Stream) new Stream.NullStream();

    #nullable disable
    private protected SemaphoreSlim _asyncActiveSemaphore;

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool HasOverriddenBeginEndRead();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool HasOverriddenBeginEndWrite();

    [MemberNotNull("_asyncActiveSemaphore")]
    private protected SemaphoreSlim EnsureAsyncActiveSemaphoreInitialized() => Volatile.Read<SemaphoreSlim>(ref this._asyncActiveSemaphore) ?? Interlocked.CompareExchange<SemaphoreSlim>(ref this._asyncActiveSemaphore, new SemaphoreSlim(1, 1), (SemaphoreSlim) null) ?? this._asyncActiveSemaphore;

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports reading.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports reading; otherwise, <see langword="false" />.</returns>
    public abstract bool CanRead { get; }

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports writing.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports writing; otherwise, <see langword="false" />.</returns>
    public abstract bool CanWrite { get; }

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports seeking.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports seeking; otherwise, <see langword="false" />.</returns>
    public abstract bool CanSeek { get; }

    /// <summary>Gets a value that determines whether the current stream can time out.</summary>
    /// <returns>A value that determines whether the current stream can time out.</returns>
    public virtual bool CanTimeout => false;

    /// <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
    /// <exception cref="T:System.NotSupportedException">A class derived from <see langword="Stream" /> does not support seeking.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>A long value representing the length of the stream in bytes.</returns>
    public abstract long Length { get; }

    /// <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The current position within the stream.</returns>
    public abstract long Position { get; set; }

    /// <summary>Gets or sets a value, in milliseconds, that determines how long the stream will attempt to read before timing out.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.ReadTimeout" /> method always throws an <see cref="T:System.InvalidOperationException" />.</exception>
    /// <returns>A value, in milliseconds, that determines how long the stream will attempt to read before timing out.</returns>
    public virtual int ReadTimeout
    {
      get => throw new InvalidOperationException(SR.InvalidOperation_TimeoutsNotSupported);
      set => throw new InvalidOperationException(SR.InvalidOperation_TimeoutsNotSupported);
    }

    /// <summary>Gets or sets a value, in milliseconds, that determines how long the stream will attempt to write before timing out.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.WriteTimeout" /> method always throws an <see cref="T:System.InvalidOperationException" />.</exception>
    /// <returns>A value, in milliseconds, that determines how long the stream will attempt to write before timing out.</returns>
    public virtual int WriteTimeout
    {
      get => throw new InvalidOperationException(SR.InvalidOperation_TimeoutsNotSupported);
      set => throw new InvalidOperationException(SR.InvalidOperation_TimeoutsNotSupported);
    }


    #nullable enable
    /// <summary>Reads the bytes from the current stream and writes them to another stream.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support reading.
    /// 
    /// -or-
    /// 
    /// <paramref name="destination" /> does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Either the current stream or <paramref name="destination" /> were closed before the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)" /> method was called.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public void CopyTo(Stream destination) => this.CopyTo(destination, this.GetCopyBufferSize());

    /// <summary>Reads the bytes from the current stream and writes them to another stream, using a specified buffer size.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative or zero.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support reading.
    /// 
    /// -or-
    /// 
    /// <paramref name="destination" /> does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Either the current stream or <paramref name="destination" /> were closed before the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream)" /> method was called.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    public virtual void CopyTo(Stream destination, int bufferSize)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      if (!this.CanRead)
      {
        if (this.CanWrite)
          ThrowHelper.ThrowNotSupportedException_UnreadableStream();
        ThrowHelper.ThrowObjectDisposedException_StreamClosed(this.GetType().Name);
      }
      byte[] numArray = ArrayPool<byte>.Shared.Rent(bufferSize);
      try
      {
        int count;
        while ((count = this.Read(numArray, 0, numArray.Length)) != 0)
          destination.Write(numArray, 0, count);
      }
      finally
      {
        ArrayPool<byte>.Shared.Return(numArray);
      }
    }

    /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public Task CopyToAsync(Stream destination) => this.CopyToAsync(destination, this.GetCopyBufferSize());

    /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified buffer size.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="buffersize" /> is negative or zero.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public Task CopyToAsync(Stream destination, int bufferSize) => this.CopyToAsync(destination, bufferSize, CancellationToken.None);

    /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified cancellation token.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public Task CopyToAsync(Stream destination, CancellationToken cancellationToken) => this.CopyToAsync(destination, this.GetCopyBufferSize(), cancellationToken);

    /// <summary>Asynchronously reads the bytes from the current stream and writes them to another stream, using a specified buffer size and cancellation token.</summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="buffersize" /> is negative or zero.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Either the current stream or the destination stream is disposed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support reading, or the destination stream does not support writing.</exception>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public virtual Task CopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      if (!this.CanRead)
      {
        if (this.CanWrite)
          ThrowHelper.ThrowNotSupportedException_UnreadableStream();
        ThrowHelper.ThrowObjectDisposedException_StreamClosed(this.GetType().Name);
      }
      return Core(this, destination, bufferSize, cancellationToken);


      #nullable disable
      static async Task Core(
        Stream source,
        Stream destination,
        int bufferSize,
        CancellationToken cancellationToken)
      {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
        try
        {
          while (true)
          {
            int length;
            if ((length = await source.ReadAsync(new Memory<byte>(buffer), cancellationToken).ConfigureAwait(false)) != 0)
              await destination.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, length), cancellationToken).ConfigureAwait(false);
            else
              break;
          }
        }
        finally
        {
          ArrayPool<byte>.Shared.Return(buffer);
        }
        buffer = (byte[]) null;
      }
    }

    private int GetCopyBufferSize()
    {
      int val1 = 81920;
      if (this.CanSeek)
      {
        long length = this.Length;
        long position = this.Position;
        if (length <= position)
        {
          val1 = 1;
        }
        else
        {
          long val2 = length - position;
          if (val2 > 0L)
            val1 = (int) Math.Min((long) val1, val2);
        }
      }
      return val1;
    }

    /// <summary>Releases all resources used by the <see cref="T:System.IO.Stream" />.</summary>
    public void Dispose() => this.Close();

    /// <summary>Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream. Instead of calling this method, ensure that the stream is properly disposed.</summary>
    public virtual void Close()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    /// <summary>Asynchronously releases the unmanaged resources used by the <see cref="T:System.IO.Stream" />.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public virtual ValueTask DisposeAsync()
    {
      try
      {
        this.Dispose();
        return new ValueTask();
      }
      catch (Exception ex)
      {
        return ValueTask.FromException(ex);
      }
    }

    /// <summary>When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    public abstract void Flush();


    #nullable enable
    /// <summary>Asynchronously clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public Task FlushAsync() => this.FlushAsync(CancellationToken.None);

    /// <summary>Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public virtual Task FlushAsync(CancellationToken cancellationToken) => Task.Factory.StartNew((Action<object>) (state => ((Stream) state).Flush()), (object) this, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

    /// <summary>Allocates a <see cref="T:System.Threading.WaitHandle" /> object.</summary>
    /// <returns>A reference to the allocated <see langword="WaitHandle" />.</returns>
    [Obsolete("CreateWaitHandle has been deprecated. Use the ManualResetEvent(false) constructor instead.")]
    protected virtual WaitHandle CreateWaitHandle() => (WaitHandle) new ManualResetEvent(false);

    /// <summary>Begins an asynchronous read operation. (Consider using <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)</summary>
    /// <param name="buffer">The buffer to read the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data read from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="callback">An optional asynchronous callback, to be called when the read is complete.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
    /// <exception cref="T:System.IO.IOException">Attempted an asynchronous read past the end of the stream, or a disk error occurs.</exception>
    /// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current <see langword="Stream" /> implementation does not support the read operation.</exception>
    /// <returns>An <see cref="T:System.IAsyncResult" /> that represents the asynchronous read, which could still be pending.</returns>
    public virtual IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? callback,
      object? state)
    {
      return (IAsyncResult) this.BeginReadInternal(buffer, offset, count, callback, state, false, true);
    }


    #nullable disable
    internal Task<int> BeginReadInternal(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state,
      bool serializeAsynchronously,
      bool apm)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (!this.CanRead)
        ThrowHelper.ThrowNotSupportedException_UnreadableStream();
      SemaphoreSlim semaphoreSlim = this.EnsureAsyncActiveSemaphoreInitialized();
      Task asyncWaiter = (Task) null;
      if (serializeAsynchronously)
        asyncWaiter = semaphoreSlim.WaitAsync();
      else
        semaphoreSlim.Wait();
      Stream.ReadWriteTask readWriteTask = new Stream.ReadWriteTask(true, apm, (Func<object, int>) (_param1 =>
      {
        Stream.ReadWriteTask internalCurrent = Task.InternalCurrent as Stream.ReadWriteTask;
        try
        {
          return internalCurrent._stream.Read(internalCurrent._buffer, internalCurrent._offset, internalCurrent._count);
        }
        finally
        {
          if (!internalCurrent._apm)
            internalCurrent._stream.FinishTrackingAsyncOperation(internalCurrent);
          internalCurrent.ClearBeginState();
        }
      }), state, this, buffer, offset, count, callback);
      if (asyncWaiter != null)
        Stream.RunReadWriteTaskWhenReady(asyncWaiter, readWriteTask);
      else
        Stream.RunReadWriteTask(readWriteTask);
      return (Task<int>) readWriteTask;
    }


    #nullable enable
    /// <summary>Waits for the pending asynchronous read to complete. (Consider using <see cref="M:System.IO.Stream.ReadAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)</summary>
    /// <param name="asyncResult">The reference to the pending asynchronous request to finish.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">A handle to the pending read operation is not available.
    /// 
    /// -or-
    /// 
    /// The pending operation does not support reading.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Stream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception>
    /// <returns>The number of bytes read from the stream, between zero (0) and the number of bytes you requested. Streams return zero (0) only at the end of the stream, otherwise, they should block until at least one byte is available.</returns>
    public virtual int EndRead(IAsyncResult asyncResult)
    {
      if (asyncResult == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.asyncResult);
      if (!(asyncResult is Stream.ReadWriteTask task) || !task._isRead)
        ThrowHelper.ThrowArgumentException(ExceptionResource.InvalidOperation_WrongAsyncResultOrEndCalledMultiple);
      else if (task._endCalled)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_WrongAsyncResultOrEndCalledMultiple);
      try
      {
        return task.GetAwaiter().GetResult();
      }
      finally
      {
        this.FinishTrackingAsyncOperation(task);
      }
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public Task<int> ReadAsync(byte[] buffer, int offset, int count) => this.ReadAsync(buffer, offset, count, CancellationToken.None);

    /// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public virtual Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return !cancellationToken.IsCancellationRequested ? this.BeginEndReadAsync(buffer, offset, count) : Task.FromCanceled<int>(cancellationToken);
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation. The value of its <see cref="P:System.Threading.Tasks.ValueTask`1.Result" /> property contains the total number of bytes read into the buffer. The result value can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public virtual ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      ArraySegment<byte> segment;
      if (MemoryMarshal.TryGetArray<byte>((ReadOnlyMemory<byte>) buffer, out segment))
        return new ValueTask<int>(this.ReadAsync(segment.Array, segment.Offset, segment.Count, cancellationToken));
      byte[] numArray = ArrayPool<byte>.Shared.Rent(buffer.Length);
      return FinishReadAsync(this.ReadAsync(numArray, 0, buffer.Length, cancellationToken), numArray, buffer);


      #nullable disable
      static async ValueTask<int> FinishReadAsync(
        Task<int> readTask,
        byte[] localBuffer,
        Memory<byte> localDestination)
      {
        int num;
        try
        {
          int length = await readTask.ConfigureAwait(false);
          new ReadOnlySpan<byte>(localBuffer, 0, length).CopyTo(localDestination.Span);
          num = length;
        }
        finally
        {
          ArrayPool<byte>.Shared.Return(localBuffer);
        }
        return num;
      }
    }

    private Task<int> BeginEndReadAsync(byte[] buffer, int offset, int count)
    {
      if (!this.HasOverriddenBeginEndRead())
        return this.BeginReadInternal(buffer, offset, count, (AsyncCallback) null, (object) null, true, false);
      return TaskFactory<int>.FromAsyncTrim<Stream, Stream.ReadWriteParameters>(this, new Stream.ReadWriteParameters()
      {
        Buffer = buffer,
        Offset = offset,
        Count = count
      }, (Func<Stream, Stream.ReadWriteParameters, AsyncCallback, object, IAsyncResult>) ((stream, args, callback, state) => stream.BeginRead(args.Buffer, args.Offset, args.Count, callback, state)), (Func<Stream, IAsyncResult, int>) ((stream, asyncResult) => stream.EndRead(asyncResult)));
    }


    #nullable enable
    /// <summary>Begins an asynchronous write operation. (Consider using <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> from which to begin writing.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="callback">An optional asynchronous callback, to be called when the write is complete.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
    /// <exception cref="T:System.IO.IOException">Attempted an asynchronous write past the end of the stream, or a disk error occurs.</exception>
    /// <exception cref="T:System.ArgumentException">One or more of the arguments is invalid.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current <see langword="Stream" /> implementation does not support the write operation.</exception>
    /// <returns>An <see langword="IAsyncResult" /> that represents the asynchronous write, which could still be pending.</returns>
    public virtual IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? callback,
      object? state)
    {
      return (IAsyncResult) this.BeginWriteInternal(buffer, offset, count, callback, state, false, true);
    }


    #nullable disable
    internal Task BeginWriteInternal(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state,
      bool serializeAsynchronously,
      bool apm)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (!this.CanWrite)
        ThrowHelper.ThrowNotSupportedException_UnwritableStream();
      SemaphoreSlim semaphoreSlim = this.EnsureAsyncActiveSemaphoreInitialized();
      Task asyncWaiter = (Task) null;
      if (serializeAsynchronously)
        asyncWaiter = semaphoreSlim.WaitAsync();
      else
        semaphoreSlim.Wait();
      Stream.ReadWriteTask readWriteTask = new Stream.ReadWriteTask(false, apm, (Func<object, int>) (_param1 =>
      {
        Stream.ReadWriteTask internalCurrent = Task.InternalCurrent as Stream.ReadWriteTask;
        try
        {
          internalCurrent._stream.Write(internalCurrent._buffer, internalCurrent._offset, internalCurrent._count);
          return 0;
        }
        finally
        {
          if (!internalCurrent._apm)
            internalCurrent._stream.FinishTrackingAsyncOperation(internalCurrent);
          internalCurrent.ClearBeginState();
        }
      }), state, this, buffer, offset, count, callback);
      if (asyncWaiter != null)
        Stream.RunReadWriteTaskWhenReady(asyncWaiter, readWriteTask);
      else
        Stream.RunReadWriteTask(readWriteTask);
      return (Task) readWriteTask;
    }

    private static void RunReadWriteTaskWhenReady(
      Task asyncWaiter,
      Stream.ReadWriteTask readWriteTask)
    {
      if (asyncWaiter.IsCompleted)
        Stream.RunReadWriteTask(readWriteTask);
      else
        asyncWaiter.ContinueWith((Action<Task, object>) ((t, state) => Stream.RunReadWriteTask((Stream.ReadWriteTask) state)), (object) readWriteTask, new CancellationToken(), TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
    }

    private static void RunReadWriteTask(Stream.ReadWriteTask readWriteTask)
    {
      readWriteTask.m_taskScheduler = TaskScheduler.Default;
      readWriteTask.ScheduleAndStart(false);
    }

    private void FinishTrackingAsyncOperation(Stream.ReadWriteTask task)
    {
      task._endCalled = true;
      this._asyncActiveSemaphore.Release();
    }


    #nullable enable
    /// <summary>Ends an asynchronous write operation. (Consider using <see cref="M:System.IO.Stream.WriteAsync(System.Byte[],System.Int32,System.Int32)" /> instead.)</summary>
    /// <param name="asyncResult">A reference to the outstanding asynchronous I/O request.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">A handle to the pending write operation is not available.
    /// 
    /// -or-
    /// 
    /// The pending operation does not support writing.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="asyncResult" /> did not originate from a <see cref="M:System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> method on the current stream.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception>
    public virtual void EndWrite(IAsyncResult asyncResult)
    {
      if (asyncResult == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.asyncResult);
      if (!(asyncResult is Stream.ReadWriteTask task) || task._isRead)
        ThrowHelper.ThrowArgumentException(ExceptionResource.InvalidOperation_WrongAsyncResultOrEndCalledMultiple);
      else if (task._endCalled)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.InvalidOperation_WrongAsyncResultOrEndCalledMultiple);
      try
      {
        task.GetAwaiter().GetResult();
      }
      finally
      {
        this.FinishTrackingAsyncOperation(task);
      }
    }

    /// <summary>Asynchronously writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public Task WriteAsync(byte[] buffer, int offset, int count) => this.WriteAsync(buffer, offset, count, CancellationToken.None);

    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public virtual Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return !cancellationToken.IsCancellationRequested ? this.BeginEndWriteAsync(buffer, offset, count) : Task.FromCanceled(cancellationToken);
    }

    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public virtual ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      ArraySegment<byte> segment;
      if (MemoryMarshal.TryGetArray<byte>(buffer, out segment))
        return new ValueTask(this.WriteAsync(segment.Array, segment.Offset, segment.Count, cancellationToken));
      byte[] numArray = ArrayPool<byte>.Shared.Rent(buffer.Length);
      buffer.Span.CopyTo((Span<byte>) numArray);
      return new ValueTask(Stream.FinishWriteAsync(this.WriteAsync(numArray, 0, buffer.Length, cancellationToken), numArray));
    }


    #nullable disable
    private static async Task FinishWriteAsync(Task writeTask, byte[] localBuffer)
    {
      try
      {
        await writeTask.ConfigureAwait(false);
      }
      finally
      {
        ArrayPool<byte>.Shared.Return(localBuffer);
      }
    }

    private Task BeginEndWriteAsync(byte[] buffer, int offset, int count)
    {
      if (!this.HasOverriddenBeginEndWrite())
        return this.BeginWriteInternal(buffer, offset, count, (AsyncCallback) null, (object) null, true, false);
      return (Task) TaskFactory<VoidTaskResult>.FromAsyncTrim<Stream, Stream.ReadWriteParameters>(this, new Stream.ReadWriteParameters()
      {
        Buffer = buffer,
        Offset = offset,
        Count = count
      }, (Func<Stream, Stream.ReadWriteParameters, AsyncCallback, object, IAsyncResult>) ((stream, args, callback, state) => stream.BeginWrite(args.Buffer, args.Offset, args.Count, callback, state)), (Func<Stream, IAsyncResult, VoidTaskResult>) ((stream, asyncResult) =>
      {
        stream.EndWrite(asyncResult);
        return new VoidTaskResult();
      }));
    }

    /// <summary>When overridden in a derived class, sets the position within the current stream.</summary>
    /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
    /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The new position within the current stream.</returns>
    public abstract long Seek(long offset, SeekOrigin origin);

    /// <summary>When overridden in a derived class, sets the length of the current stream.</summary>
    /// <param name="value">The desired length of the current stream in bytes.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public abstract void SetLength(long value);


    #nullable enable
    /// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
    /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
    /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public abstract int Read(byte[] buffer, int offset, int count);

    /// <summary>When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current source.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public virtual int Read(Span<byte> buffer)
    {
      byte[] numArray = ArrayPool<byte>.Shared.Rent(buffer.Length);
      try
      {
        int length = this.Read(numArray, 0, buffer.Length);
        if ((uint) length > (uint) buffer.Length)
          throw new IOException(SR.IO_StreamTooLong);
        new ReadOnlySpan<byte>(numArray, 0, length).CopyTo(buffer);
        return length;
      }
      finally
      {
        ArrayPool<byte>.Shared.Return(numArray);
      }
    }

    /// <summary>Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.</summary>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The unsigned byte cast to an <see cref="T:System.Int32" />, or -1 if at the end of the stream.</returns>
    public virtual int ReadByte()
    {
      byte[] buffer = new byte[1];
      return this.Read(buffer, 0, 1) != 0 ? (int) buffer[0] : -1;
    }

    /// <summary>When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
    /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The number of bytes to be written to the current stream.</param>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is greater than the buffer length.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred, such as the specified file cannot be found.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">
    /// <see cref="M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32)" /> was called after the stream was closed.</exception>
    public abstract void Write(byte[] buffer, int offset, int count);

    /// <summary>When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.</summary>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the current stream.</param>
    public virtual void Write(ReadOnlySpan<byte> buffer)
    {
      byte[] numArray = ArrayPool<byte>.Shared.Rent(buffer.Length);
      try
      {
        buffer.CopyTo((Span<byte>) numArray);
        this.Write(numArray, 0, buffer.Length);
      }
      finally
      {
        ArrayPool<byte>.Shared.Return(numArray);
      }
    }

    /// <summary>Writes a byte to the current position in the stream and advances the position within the stream by one byte.</summary>
    /// <param name="value">The byte to write to the stream.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing, or the stream is already closed.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public virtual void WriteByte(byte value) => this.Write(new byte[1]
    {
      value
    }, 0, 1);

    /// <summary>Creates a thread-safe (synchronized) wrapper around the specified <see cref="T:System.IO.Stream" /> object.</summary>
    /// <param name="stream">The <see cref="T:System.IO.Stream" /> object to synchronize.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <returns>A thread-safe <see cref="T:System.IO.Stream" /> object.</returns>
    public static Stream Synchronized(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      return !(stream is Stream.SyncStream) ? (Stream) new Stream.SyncStream(stream) : stream;
    }

    /// <summary>Provides support for a <see cref="T:System.Diagnostics.Contracts.Contract" />.</summary>
    [Obsolete("Do not call or override this method.")]
    protected virtual void ObjectInvariant()
    {
    }

    /// <summary>Validates arguments provided to reading and writing methods on <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="buffer">The array "buffer" argument passed to the reading or writing method.</param>
    /// <param name="offset">The integer "offset" argument passed to the reading or writing method.</param>
    /// <param name="count">The integer "count" argument passed to the reading or writing method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> was outside the bounds of <paramref name="buffer" />, or <paramref name="count" /> was negative, or the range specified by the combination of <paramref name="offset" /> and <paramref name="count" /> exceed the length of <paramref name="buffer" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static void ValidateBufferArguments(byte[] buffer, int offset, int count)
    {
      if (buffer == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.buffer);
      if (offset < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.offset, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if ((long) (uint) count <= (long) (buffer.Length - offset))
        return;
      ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.Argument_InvalidOffLen);
    }

    /// <summary>Validates arguments provided to the <see cref="M:System.IO.Stream.CopyTo(System.IO.Stream,System.Int32)" /> or <see cref="M:System.IO.Stream.CopyToAsync(System.IO.Stream,System.Int32,System.Threading.CancellationToken)" /> methods.</summary>
    /// <param name="destination">The <see cref="T:System.IO.Stream" /> "destination" argument passed to the copy method.</param>
    /// <param name="bufferSize">The integer "bufferSize" argument passed to the copy method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="destination" /> was <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> was not a positive value.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="destination" /> does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">
    /// <paramref name="destination" /> does not support writing or reading.</exception>
    protected static void ValidateCopyToArguments(Stream destination, int bufferSize)
    {
      if (destination == null)
        throw new ArgumentNullException(nameof (destination));
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), (object) bufferSize, SR.ArgumentOutOfRange_NeedPosNum);
      if (destination.CanWrite)
        return;
      if (destination.CanRead)
        ThrowHelper.ThrowNotSupportedException_UnwritableStream();
      ThrowHelper.ThrowObjectDisposedException_StreamClosed(destination.GetType().Name);
    }


    #nullable disable
    private struct ReadWriteParameters
    {
      internal byte[] Buffer;
      internal int Offset;
      internal int Count;
    }

    private sealed class ReadWriteTask : Task<int>, ITaskCompletionAction
    {
      internal readonly bool _isRead;
      internal readonly bool _apm;
      internal bool _endCalled;
      internal Stream _stream;
      internal byte[] _buffer;
      internal readonly int _offset;
      internal readonly int _count;
      private AsyncCallback _callback;
      private ExecutionContext _context;
      private static ContextCallback s_invokeAsyncCallback;

      internal void ClearBeginState()
      {
        this._stream = (Stream) null;
        this._buffer = (byte[]) null;
      }

      public ReadWriteTask(
        bool isRead,
        bool apm,
        Func<object, int> function,
        object state,
        Stream stream,
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback callback)
        : base(function, state, CancellationToken.None, TaskCreationOptions.DenyChildAttach)
      {
        this._isRead = isRead;
        this._apm = apm;
        this._stream = stream;
        this._buffer = buffer;
        this._offset = offset;
        this._count = count;
        if (callback == null)
          return;
        this._callback = callback;
        this._context = ExecutionContext.Capture();
        this.AddCompletionAction((ITaskCompletionAction) this);
      }

      private static void InvokeAsyncCallback(object completedTask)
      {
        Stream.ReadWriteTask ar = (Stream.ReadWriteTask) completedTask;
        AsyncCallback callback = ar._callback;
        ar._callback = (AsyncCallback) null;
        callback((IAsyncResult) ar);
      }

      void ITaskCompletionAction.Invoke(Task completingTask)
      {
        ExecutionContext context = this._context;
        if (context == null)
        {
          AsyncCallback callback = this._callback;
          this._callback = (AsyncCallback) null;
          callback((IAsyncResult) completingTask);
        }
        else
        {
          this._context = (ExecutionContext) null;
          ContextCallback callback = Stream.ReadWriteTask.s_invokeAsyncCallback ?? (Stream.ReadWriteTask.s_invokeAsyncCallback = new ContextCallback(Stream.ReadWriteTask.InvokeAsyncCallback));
          ExecutionContext.RunInternal(context, callback, (object) this);
        }
      }

      bool ITaskCompletionAction.InvokeMayRunArbitraryCode => true;
    }

    private sealed class NullStream : Stream
    {
      internal NullStream()
      {
      }

      public override bool CanRead => true;

      public override bool CanWrite => true;

      public override bool CanSeek => true;

      public override long Length => 0;

      public override long Position
      {
        get => 0;
        set
        {
        }
      }

      public override void CopyTo(Stream destination, int bufferSize)
      {
      }

      public override Task CopyToAsync(
        Stream destination,
        int bufferSize,
        CancellationToken cancellationToken)
      {
        return !cancellationToken.IsCancellationRequested ? Task.CompletedTask : Task.FromCanceled(cancellationToken);
      }

      protected override void Dispose(bool disposing)
      {
      }

      public override void Flush()
      {
      }

      public override Task FlushAsync(CancellationToken cancellationToken) => !cancellationToken.IsCancellationRequested ? Task.CompletedTask : Task.FromCanceled(cancellationToken);

      public override IAsyncResult BeginRead(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback callback,
        object state)
      {
        return TaskToApm.Begin((Task) Task<int>.s_defaultResultTask, callback, state);
      }

      public override int EndRead(IAsyncResult asyncResult) => TaskToApm.End<int>(asyncResult);

      public override IAsyncResult BeginWrite(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback callback,
        object state)
      {
        return TaskToApm.Begin(Task.CompletedTask, callback, state);
      }

      public override void EndWrite(IAsyncResult asyncResult) => TaskToApm.End(asyncResult);

      public override int Read(byte[] buffer, int offset, int count) => 0;

      public override int Read(Span<byte> buffer) => 0;

      public override Task<int> ReadAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        return !cancellationToken.IsCancellationRequested ? Task.FromResult<int>(0) : Task.FromCanceled<int>(cancellationToken);
      }

      public override ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken)
      {
        return !cancellationToken.IsCancellationRequested ? new ValueTask<int>() : ValueTask.FromCanceled<int>(cancellationToken);
      }

      public override int ReadByte() => -1;

      public override void Write(byte[] buffer, int offset, int count)
      {
      }

      public override void Write(ReadOnlySpan<byte> buffer)
      {
      }

      public override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        return !cancellationToken.IsCancellationRequested ? Task.CompletedTask : Task.FromCanceled(cancellationToken);
      }

      public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default (CancellationToken))
      {
        return !cancellationToken.IsCancellationRequested ? new ValueTask() : ValueTask.FromCanceled(cancellationToken);
      }

      public override void WriteByte(byte value)
      {
      }

      public override long Seek(long offset, SeekOrigin origin) => 0;

      public override void SetLength(long length)
      {
      }
    }

    private sealed class SyncStream : Stream, IDisposable
    {
      private readonly Stream _stream;

      internal SyncStream(Stream stream) => this._stream = stream ?? throw new ArgumentNullException(nameof (stream));

      public override bool CanRead => this._stream.CanRead;

      public override bool CanWrite => this._stream.CanWrite;

      public override bool CanSeek => this._stream.CanSeek;

      public override bool CanTimeout => this._stream.CanTimeout;

      public override long Length
      {
        get
        {
          lock (this._stream)
            return this._stream.Length;
        }
      }

      public override long Position
      {
        get
        {
          lock (this._stream)
            return this._stream.Position;
        }
        set
        {
          lock (this._stream)
            this._stream.Position = value;
        }
      }

      public override int ReadTimeout
      {
        get => this._stream.ReadTimeout;
        set => this._stream.ReadTimeout = value;
      }

      public override int WriteTimeout
      {
        get => this._stream.WriteTimeout;
        set => this._stream.WriteTimeout = value;
      }

      public override void Close()
      {
        lock (this._stream)
        {
          try
          {
            this._stream.Close();
          }
          finally
          {
            base.Dispose(true);
          }
        }
      }

      protected override void Dispose(bool disposing)
      {
        lock (this._stream)
        {
          try
          {
            if (!disposing)
              return;
            this._stream.Dispose();
          }
          finally
          {
            base.Dispose(disposing);
          }
        }
      }

      public override ValueTask DisposeAsync()
      {
        lock (this._stream)
          return this._stream.DisposeAsync();
      }

      public override void Flush()
      {
        lock (this._stream)
          this._stream.Flush();
      }

      public override int Read(byte[] bytes, int offset, int count)
      {
        lock (this._stream)
          return this._stream.Read(bytes, offset, count);
      }

      public override int Read(Span<byte> buffer)
      {
        lock (this._stream)
          return this._stream.Read(buffer);
      }

      public override int ReadByte()
      {
        lock (this._stream)
          return this._stream.ReadByte();
      }

      public override IAsyncResult BeginRead(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback callback,
        object state)
      {
        bool flag = this._stream.HasOverriddenBeginEndRead();
        lock (this._stream)
          return flag ? this._stream.BeginRead(buffer, offset, count, callback, state) : (IAsyncResult) this._stream.BeginReadInternal(buffer, offset, count, callback, state, true, true);
      }

      public override int EndRead(IAsyncResult asyncResult)
      {
        if (asyncResult == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.asyncResult);
        lock (this._stream)
          return this._stream.EndRead(asyncResult);
      }

      public override long Seek(long offset, SeekOrigin origin)
      {
        lock (this._stream)
          return this._stream.Seek(offset, origin);
      }

      public override void SetLength(long length)
      {
        lock (this._stream)
          this._stream.SetLength(length);
      }

      public override void Write(byte[] bytes, int offset, int count)
      {
        lock (this._stream)
          this._stream.Write(bytes, offset, count);
      }

      public override void Write(ReadOnlySpan<byte> buffer)
      {
        lock (this._stream)
          this._stream.Write(buffer);
      }

      public override void WriteByte(byte b)
      {
        lock (this._stream)
          this._stream.WriteByte(b);
      }

      public override IAsyncResult BeginWrite(
        byte[] buffer,
        int offset,
        int count,
        AsyncCallback callback,
        object state)
      {
        bool flag = this._stream.HasOverriddenBeginEndWrite();
        lock (this._stream)
          return flag ? this._stream.BeginWrite(buffer, offset, count, callback, state) : (IAsyncResult) this._stream.BeginWriteInternal(buffer, offset, count, callback, state, true, true);
      }

      public override void EndWrite(IAsyncResult asyncResult)
      {
        if (asyncResult == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.asyncResult);
        lock (this._stream)
          this._stream.EndWrite(asyncResult);
      }
    }
  }
}
