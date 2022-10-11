// Decompiled with JetBrains decompiler
// Type: System.IO.StreamWriter
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Implements a <see cref="T:System.IO.TextWriter" /> for writing characters to a stream in a particular encoding.</summary>
  public class StreamWriter : TextWriter
  {
    /// <summary>Provides a <see langword="StreamWriter" /> with no backing store that can be written to, but not read from.</summary>
    public static readonly StreamWriter Null = new StreamWriter(Stream.Null, StreamWriter.UTF8NoBOM, 128, true);

    #nullable disable
    private readonly Stream _stream;
    private readonly Encoding _encoding;
    private readonly Encoder _encoder;
    private byte[] _byteBuffer;
    private readonly char[] _charBuffer;
    private int _charPos;
    private int _charLen;
    private bool _autoFlush;
    private bool _haveWrittenPreamble;
    private readonly bool _closable;
    private bool _disposed;
    private Task _asyncWriteTask = Task.CompletedTask;

    private void CheckAsyncTaskInProgress()
    {
      if (this._asyncWriteTask.IsCompleted)
        return;
      StreamWriter.ThrowAsyncIOInProgress();
    }

    [DoesNotReturn]
    private static void ThrowAsyncIOInProgress() => throw new InvalidOperationException(SR.InvalidOperation_AsyncIOInProgress);


    #nullable enable
    private static Encoding UTF8NoBOM => EncodingCache.UTF8NoBOM;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using UTF-8 encoding and the default buffer size.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    public StreamWriter(Stream stream)
      : this(stream, StreamWriter.UTF8NoBOM, 1024, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and the default buffer size.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public StreamWriter(Stream stream, Encoding encoding)
      : this(stream, encoding, 1024, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and buffer size.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="bufferSize">The buffer size, in bytes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public StreamWriter(Stream stream, Encoding encoding, int bufferSize)
      : this(stream, encoding, bufferSize, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and buffer size, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="bufferSize">The buffer size, in bytes.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.StreamWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public StreamWriter(Stream stream, Encoding? encoding = null, int bufferSize = -1, bool leaveOpen = false)
      : base((IFormatProvider) null)
    {
      if (stream == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.stream);
      if (encoding == null)
        encoding = StreamWriter.UTF8NoBOM;
      if (!stream.CanWrite)
        throw new ArgumentException(SR.Argument_StreamNotWritable);
      if (bufferSize == -1)
        bufferSize = 1024;
      else if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), SR.ArgumentOutOfRange_NeedPosNum);
      this._stream = stream;
      this._encoding = encoding;
      this._encoder = this._encoding.GetEncoder();
      if (bufferSize < 128)
        bufferSize = 128;
      this._charBuffer = new char[bufferSize];
      this._charLen = bufferSize;
      if (this._stream.CanSeek && this._stream.Position > 0L)
        this._haveWrittenPreamble = true;
      this._closable = !leaveOpen;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the default encoding and buffer size.</summary>
    /// <param name="path">The complete file path to write to. <paramref name="path" /> can be a file name.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is an empty string ("").
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public StreamWriter(string path)
      : this(path, false, StreamWriter.UTF8NoBOM, 1024)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the default encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="append">
    /// <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is empty.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public StreamWriter(string path, bool append)
      : this(path, append, StreamWriter.UTF8NoBOM, 1024)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the specified encoding and default buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="append">
    /// <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is empty.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public StreamWriter(string path, bool append, Encoding encoding)
      : this(path, append, encoding, 1024)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file on the specified path, using the specified encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="append">
    /// <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="bufferSize">The buffer size, in bytes.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is an empty string ("").
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    public StreamWriter(string path, bool append, Encoding encoding, int bufferSize)
      : this(StreamWriter.ValidateArgsAndOpenPath(path, append, encoding, bufferSize), encoding, bufferSize, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file, using the default encoding, and configured with the specified <see cref="T:System.IO.FileStreamOptions" /> object.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="options">An object that specifies the configuration options for the underlying <see cref="T:System.IO.FileStream" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="options" /> is <see langword="null" />
    /// .</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public StreamWriter(string path, FileStreamOptions options)
      : this(path, StreamWriter.UTF8NoBOM, options)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file, using the specified encoding, and configured with the specified <see cref="T:System.IO.FileStreamOptions" /> object.</summary>
    /// <param name="path">The complete file path to write to.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="options">An object that specifies the configuration options for the underlying <see cref="T:System.IO.FileStream" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="options" /> is <see langword="null" />
    /// .</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> is not writable.</exception>
    public StreamWriter(string path, Encoding encoding, FileStreamOptions options)
      : this(StreamWriter.ValidateArgsAndOpenPath(path, encoding, options), encoding, 4096)
    {
    }


    #nullable disable
    private static Stream ValidateArgsAndOpenPath(
      string path,
      Encoding encoding,
      FileStreamOptions options)
    {
      StreamWriter.ValidateArgs(path, encoding);
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      return (options.Access & FileAccess.Write) != (FileAccess) 0 ? (Stream) new FileStream(path, options) : throw new ArgumentException(SR.Argument_StreamNotWritable, nameof (options));
    }

    private static Stream ValidateArgsAndOpenPath(
      string path,
      bool append,
      Encoding encoding,
      int bufferSize)
    {
      StreamWriter.ValidateArgs(path, encoding);
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), SR.ArgumentOutOfRange_NeedPosNum);
      return (Stream) new FileStream(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, 4096);
    }

    private static void ValidateArgs(string path, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath);
    }

    /// <summary>Closes the current <see langword="StreamWriter" /> object and the underlying stream.</summary>
    /// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
    public override void Close()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Causes any buffered data to be written to the underlying stream, releases the unmanaged resources used by the <see cref="T:System.IO.StreamWriter" />, and optionally the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    /// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!(!this._disposed & disposing))
          return;
        this.CheckAsyncTaskInProgress();
        this.Flush(true, true);
      }
      finally
      {
        this.CloseStreamFromDispose(disposing);
      }
    }

    private void CloseStreamFromDispose(bool disposing)
    {
      if (!this._closable)
        return;
      if (this._disposed)
        return;
      try
      {
        if (!disposing)
          return;
        this._stream.Close();
      }
      finally
      {
        this._disposed = true;
        this._charLen = 0;
        base.Dispose(disposing);
      }
    }

    /// <summary>Asynchronously writes any buffered data to the underlying stream and releases the unmanaged resources used by the <see cref="T:System.IO.StreamWriter" />.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override ValueTask DisposeAsync() => !(this.GetType() != typeof (StreamWriter)) ? this.DisposeAsyncCore() : base.DisposeAsync();

    private async ValueTask DisposeAsyncCore()
    {
      StreamWriter streamWriter = this;
      try
      {
        if (!streamWriter._disposed)
          await streamWriter.FlushAsync().ConfigureAwait(false);
      }
      finally
      {
        streamWriter.CloseStreamFromDispose(true);
      }
      GC.SuppressFinalize((object) streamWriter);
    }

    /// <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current writer is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error has occurred.</exception>
    /// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
    public override void Flush()
    {
      this.CheckAsyncTaskInProgress();
      this.Flush(true, true);
    }

    private unsafe void Flush(bool flushStream, bool flushEncoder)
    {
      this.ThrowIfDisposed();
      if (this._charPos == 0 && !flushStream && !flushEncoder)
        return;
      if (!this._haveWrittenPreamble)
      {
        this._haveWrittenPreamble = true;
        ReadOnlySpan<byte> preamble = this._encoding.Preamble;
        if (preamble.Length > 0)
          this._stream.Write(preamble);
      }
      Span<byte> span = new Span<byte>();
      // ISSUE: untyped stack allocation
      Span<byte> bytes1 = this._byteBuffer == null ? (this._encoding.GetMaxByteCount(this._charPos) > 1024 ? (Span<byte>) (this._byteBuffer = new byte[this._encoding.GetMaxByteCount(this._charBuffer.Length)]) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(1024)), 1024)) : (Span<byte>) this._byteBuffer;
      int bytes2 = this._encoder.GetBytes(new ReadOnlySpan<char>(this._charBuffer, 0, this._charPos), bytes1, flushEncoder);
      this._charPos = 0;
      if (bytes2 > 0)
        this._stream.Write((ReadOnlySpan<byte>) bytes1.Slice(0, bytes2));
      if (!flushStream)
        return;
      this._stream.Flush();
    }

    /// <summary>Gets or sets a value indicating whether the <see cref="T:System.IO.StreamWriter" /> will flush its buffer to the underlying stream after every call to <see cref="M:System.IO.StreamWriter.Write(System.Char)" />.</summary>
    /// <returns>
    /// <see langword="true" /> to force <see cref="T:System.IO.StreamWriter" /> to flush its buffer; otherwise, <see langword="false" />.</returns>
    public virtual bool AutoFlush
    {
      get => this._autoFlush;
      set
      {
        this.CheckAsyncTaskInProgress();
        this._autoFlush = value;
        if (!value)
          return;
        this.Flush(true, false);
      }
    }


    #nullable enable
    /// <summary>Gets the underlying stream that interfaces with a backing store.</summary>
    /// <returns>The stream this <see langword="StreamWriter" /> is writing to.</returns>
    public virtual Stream BaseStream => this._stream;

    /// <summary>Gets the <see cref="T:System.Text.Encoding" /> in which the output is written.</summary>
    /// <returns>The <see cref="T:System.Text.Encoding" /> specified in the constructor for the current instance, or <see cref="T:System.Text.UTF8Encoding" /> if an encoding was not specified.</returns>
    public override Encoding Encoding => this._encoding;

    /// <summary>Writes a character to the stream.</summary>
    /// <param name="value">The character to write to the stream.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">
    /// <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream.</exception>
    public override void Write(char value)
    {
      this.CheckAsyncTaskInProgress();
      if (this._charPos == this._charLen)
        this.Flush(false, false);
      this._charBuffer[this._charPos] = value;
      ++this._charPos;
      if (!this._autoFlush)
        return;
      this.Flush(true, false);
    }

    /// <summary>Writes a character array to the stream.</summary>
    /// <param name="buffer">A character array containing the data to write. If <paramref name="buffer" /> is <see langword="null" />, nothing is written.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">
    /// <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void Write(char[]? buffer) => this.WriteSpan((ReadOnlySpan<char>) buffer, false);

    /// <summary>Writes a subarray of characters to the stream.</summary>
    /// <param name="buffer">A character array that contains the data to write.</param>
    /// <param name="index">The character position in the buffer at which to start reading data.</param>
    /// <param name="count">The maximum number of characters to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">
    /// <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void Write(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      this.WriteSpan((ReadOnlySpan<char>) buffer.AsSpan<char>(index, count), false);
    }

    /// <summary>Writes a character span to the stream.</summary>
    /// <param name="buffer">The character span to write.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void Write(ReadOnlySpan<char> buffer)
    {
      if (this.GetType() == typeof (StreamWriter))
        this.WriteSpan(buffer, false);
      else
        base.Write(buffer);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe void WriteSpan(ReadOnlySpan<char> buffer, bool appendNewLine)
    {
      this.CheckAsyncTaskInProgress();
      if (buffer.Length <= 4 && buffer.Length <= this._charLen - this._charPos)
      {
        for (int index = 0; index < buffer.Length; ++index)
          this._charBuffer[this._charPos++] = buffer[index];
      }
      else
      {
        this.ThrowIfDisposed();
        char[] charBuffer = this._charBuffer;
        fixed (char* chPtr1 = &MemoryMarshal.GetReference<char>(buffer))
          fixed (char* chPtr2 = &charBuffer[0])
          {
            char* source = chPtr1;
            int length = buffer.Length;
            int num1 = this._charPos;
            int num2;
            for (; length > 0; length -= num2)
            {
              if (num1 == charBuffer.Length)
              {
                this.Flush(false, false);
                num1 = 0;
              }
              num2 = Math.Min(charBuffer.Length - num1, length);
              int num3 = num2 * 2;
              Buffer.MemoryCopy((void*) source, (void*) (chPtr2 + num1), (long) num3, (long) num3);
              this._charPos += num2;
              num1 += num2;
              source += num2;
            }
          }
      }
      if (appendNewLine)
      {
        foreach (char ch in this.CoreNewLine)
        {
          if (this._charPos == this._charLen)
            this.Flush(false, false);
          this._charBuffer[this._charPos] = ch;
          ++this._charPos;
        }
      }
      if (!this._autoFlush)
        return;
      this.Flush(true, false);
    }


    #nullable enable
    /// <summary>Writes a string to the stream.</summary>
    /// <param name="value">The string to write to the stream. If <paramref name="value" /> is null, nothing is written.</param>
    /// <exception cref="T:System.ObjectDisposedException">
    /// <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void Write(string? value) => this.WriteSpan((ReadOnlySpan<char>) value, false);

    /// <summary>Writes a string to the stream, followed by a line terminator.</summary>
    /// <param name="value">The string to write. If <paramref name="value" /> is <see langword="null" />, only the line terminator is written.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void WriteLine(string? value)
    {
      this.CheckAsyncTaskInProgress();
      this.WriteSpan((ReadOnlySpan<char>) value, true);
    }

    /// <summary>Writes the text representation of a character span to the stream, followed by a line terminator.</summary>
    /// <param name="buffer">The character span to write to the stream.</param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void WriteLine(ReadOnlySpan<char> buffer)
    {
      if (this.GetType() == typeof (StreamWriter))
      {
        this.CheckAsyncTaskInProgress();
        this.WriteSpan(buffer, true);
      }
      else
        base.WriteLine(buffer);
    }


    #nullable disable
    private void WriteFormatHelper(string format, ParamsArray args, bool appendNewLine)
    {
      StringBuilder sb = StringBuilderCache.Acquire((format != null ? format.Length : 0) + args.Length * 8).AppendFormatHelper((IFormatProvider) null, format, args);
      StringBuilder.ChunkEnumerator chunks = sb.GetChunks();
      bool flag = chunks.MoveNext();
      while (flag)
      {
        ReadOnlySpan<char> span = chunks.Current.Span;
        flag = chunks.MoveNext();
        this.WriteSpan(span, !flag && appendNewLine);
      }
      StringBuilderCache.Release(sb);
    }


    #nullable enable
    /// <summary>Writes a formatted string to the stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object)" /> method.</summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The object to format and write.</param>
    public override void Write(string format, object? arg0)
    {
      if (this.GetType() == typeof (StreamWriter))
        this.WriteFormatHelper(format, new ParamsArray(arg0), false);
      else
        base.Write(format, arg0);
    }

    /// <summary>Writes a formatted string to the stream using the same semantics  as the <see cref="M:System.String.Format(System.String,System.Object,System.Object)" /> method.</summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The first object to format and write.</param>
    /// <param name="arg1">The second object to format and write.</param>
    public override void Write(string format, object? arg0, object? arg1)
    {
      if (this.GetType() == typeof (StreamWriter))
        this.WriteFormatHelper(format, new ParamsArray(arg0, arg1), false);
      else
        base.Write(format, arg0, arg1);
    }

    /// <summary>Writes a formatted string to the stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object,System.Object,System.Object)" /> method.</summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The first object to format and write.</param>
    /// <param name="arg1">The second object to format and write.</param>
    /// <param name="arg2">The third object to format and write.</param>
    public override void Write(string format, object? arg0, object? arg1, object? arg2)
    {
      if (this.GetType() == typeof (StreamWriter))
        this.WriteFormatHelper(format, new ParamsArray(arg0, arg1, arg2), false);
      else
        base.Write(format, arg0, arg1, arg2);
    }

    /// <summary>Writes a formatted string to the stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object[])" /> method.</summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg">An object array that contains zero or more objects to format and write.</param>
    public override void Write(string format, params object?[] arg)
    {
      if (this.GetType() == typeof (StreamWriter))
      {
        if (arg == null)
          throw new ArgumentNullException(format == null ? nameof (format) : nameof (arg));
        this.WriteFormatHelper(format, new ParamsArray(arg), false);
      }
      else
        base.Write(format, arg);
    }

    /// <summary>Writes a formatted string and a new line to the stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object)" /> method.</summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The object to format and write.</param>
    public override void WriteLine(string format, object? arg0)
    {
      if (this.GetType() == typeof (StreamWriter))
        this.WriteFormatHelper(format, new ParamsArray(arg0), true);
      else
        base.WriteLine(format, arg0);
    }

    /// <summary>Writes a formatted string and a new line to the stream, using the same semantics as the <see cref="M:System.String.Format(System.String,System.Object,System.Object)" /> method.</summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The first object to format and write.</param>
    /// <param name="arg1">The second object to format and write.</param>
    public override void WriteLine(string format, object? arg0, object? arg1)
    {
      if (this.GetType() == typeof (StreamWriter))
        this.WriteFormatHelper(format, new ParamsArray(arg0, arg1), true);
      else
        base.WriteLine(format, arg0, arg1);
    }

    /// <summary>Writes out a formatted string and a new line to the stream, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg0">The first object to format and write.</param>
    /// <param name="arg1">The second object to format and write.</param>
    /// <param name="arg2">The third object to format and write.</param>
    public override void WriteLine(string format, object? arg0, object? arg1, object? arg2)
    {
      if (this.GetType() == typeof (StreamWriter))
        this.WriteFormatHelper(format, new ParamsArray(arg0, arg1, arg2), true);
      else
        base.WriteLine(format, arg0, arg1, arg2);
    }

    /// <summary>Writes out a formatted string and a new line to the stream, using the same semantics as <see cref="M:System.String.Format(System.String,System.Object)" />.</summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg">An object array that contains zero or more objects to format and write.</param>
    public override void WriteLine(string format, params object?[] arg)
    {
      if (this.GetType() == typeof (StreamWriter))
      {
        if (arg == null)
          throw new ArgumentNullException(nameof (arg));
        this.WriteFormatHelper(format, new ParamsArray(arg), true);
      }
      else
        base.WriteLine(format, arg);
    }

    /// <summary>Asynchronously writes a character to the stream.</summary>
    /// <param name="value">The character to write to the stream.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(char value)
    {
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteAsync(value);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task task = this.WriteAsyncInternal(value, false);
      this._asyncWriteTask = task;
      return task;
    }


    #nullable disable
    private async Task WriteAsyncInternal(char value, bool appendNewLine)
    {
      StreamWriter streamWriter1 = this;
      if (streamWriter1._charPos == streamWriter1._charLen)
        await streamWriter1.FlushAsyncInternal(false, false).ConfigureAwait(false);
      char[] charBuffer1 = streamWriter1._charBuffer;
      StreamWriter streamWriter2 = streamWriter1;
      int charPos1 = streamWriter1._charPos;
      int num1 = charPos1 + 1;
      streamWriter2._charPos = num1;
      int index1 = charPos1;
      int num2 = (int) value;
      charBuffer1[index1] = (char) num2;
      if (appendNewLine)
      {
        for (int i = 0; i < streamWriter1.CoreNewLine.Length; ++i)
        {
          if (streamWriter1._charPos == streamWriter1._charLen)
            await streamWriter1.FlushAsyncInternal(false, false).ConfigureAwait(false);
          char[] charBuffer2 = streamWriter1._charBuffer;
          StreamWriter streamWriter3 = streamWriter1;
          int charPos2 = streamWriter1._charPos;
          int num3 = charPos2 + 1;
          streamWriter3._charPos = num3;
          int index2 = charPos2;
          int num4 = (int) streamWriter1.CoreNewLine[i];
          charBuffer2[index2] = (char) num4;
        }
      }
      if (!streamWriter1._autoFlush)
        return;
      await streamWriter1.FlushAsyncInternal(true, false).ConfigureAwait(false);
    }


    #nullable enable
    /// <summary>Asynchronously writes a string to the stream.</summary>
    /// <param name="value">The string to write to the stream. If <paramref name="value" /> is <see langword="null" />, nothing is written.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(string? value)
    {
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteAsync(value);
      if (value == null)
        return Task.CompletedTask;
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task task = this.WriteAsyncInternal(value.AsMemory(), false, new CancellationToken());
      this._asyncWriteTask = task;
      return task;
    }

    /// <summary>Asynchronously writes a subarray of characters to the stream.</summary>
    /// <param name="buffer">A character array that contains the data to write.</param>
    /// <param name="index">The character position in the buffer at which to begin reading data.</param>
    /// <param name="count">The maximum number of characters to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="index" /> plus <paramref name="count" /> is greater than the buffer length.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteAsync(buffer, index, count);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task task = this.WriteAsyncInternal(new ReadOnlyMemory<char>(buffer, index, count), false, new CancellationToken());
      this._asyncWriteTask = task;
      return task;
    }

    /// <summary>Asynchronously writes a character memory region to the stream.</summary>
    /// <param name="buffer">The character memory region to write to the stream.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(
      ReadOnlyMemory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteAsync(buffer, cancellationToken);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      Task task = this.WriteAsyncInternal(buffer, false, cancellationToken);
      this._asyncWriteTask = task;
      return task;
    }


    #nullable disable
    private async Task WriteAsyncInternal(
      ReadOnlyMemory<char> source,
      bool appendNewLine,
      CancellationToken cancellationToken)
    {
      StreamWriter streamWriter1 = this;
      int length;
      for (int copied = 0; copied < source.Length; copied += length)
      {
        if (streamWriter1._charPos == streamWriter1._charLen)
          await streamWriter1.FlushAsyncInternal(false, false, cancellationToken).ConfigureAwait(false);
        length = Math.Min(streamWriter1._charLen - streamWriter1._charPos, source.Length - copied);
        ReadOnlySpan<char> readOnlySpan = source.Span;
        readOnlySpan = readOnlySpan.Slice(copied, length);
        readOnlySpan.CopyTo(new Span<char>(streamWriter1._charBuffer, streamWriter1._charPos, length));
        streamWriter1._charPos += length;
      }
      if (appendNewLine)
      {
        for (int i = 0; i < streamWriter1.CoreNewLine.Length; ++i)
        {
          if (streamWriter1._charPos == streamWriter1._charLen)
            await streamWriter1.FlushAsyncInternal(false, false, cancellationToken).ConfigureAwait(false);
          char[] charBuffer = streamWriter1._charBuffer;
          StreamWriter streamWriter2 = streamWriter1;
          int charPos = streamWriter1._charPos;
          int num1 = charPos + 1;
          streamWriter2._charPos = num1;
          int index = charPos;
          int num2 = (int) streamWriter1.CoreNewLine[i];
          charBuffer[index] = (char) num2;
        }
      }
      if (!streamWriter1._autoFlush)
        return;
      await streamWriter1.FlushAsyncInternal(true, false, cancellationToken).ConfigureAwait(false);
    }


    #nullable enable
    /// <summary>Asynchronously writes a line terminator to the stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync()
    {
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteLineAsync();
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task task = this.WriteAsyncInternal(ReadOnlyMemory<char>.Empty, true, new CancellationToken());
      this._asyncWriteTask = task;
      return task;
    }

    /// <summary>Asynchronously writes a character to the stream, followed by a line terminator.</summary>
    /// <param name="value">The character to write to the stream.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(char value)
    {
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteLineAsync(value);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task task = this.WriteAsyncInternal(value, true);
      this._asyncWriteTask = task;
      return task;
    }

    /// <summary>Asynchronously writes a string to the stream, followed by a line terminator.</summary>
    /// <param name="value">The string to write. If the value is <see langword="null" />, only a line terminator is written.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(string? value)
    {
      if (value == null)
        return this.WriteLineAsync();
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteLineAsync(value);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task task = this.WriteAsyncInternal(value.AsMemory(), true, new CancellationToken());
      this._asyncWriteTask = task;
      return task;
    }

    /// <summary>Asynchronously writes a subarray of characters to the stream, followed by a line terminator.</summary>
    /// <param name="buffer">The character array to write data from.</param>
    /// <param name="index">The character position in the buffer at which to start reading data.</param>
    /// <param name="count">The maximum number of characters to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="index" /> plus <paramref name="count" /> is greater than the buffer length.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteLineAsync(buffer, index, count);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task task = this.WriteAsyncInternal(new ReadOnlyMemory<char>(buffer, index, count), true, new CancellationToken());
      this._asyncWriteTask = task;
      return task;
    }

    /// <summary>Asynchronously writes the text representation of a character memory region to the stream, followed by a line terminator.</summary>
    /// <param name="buffer">The character memory region to write to the stream.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(
      ReadOnlyMemory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this.GetType() != typeof (StreamWriter))
        return base.WriteLineAsync(buffer, cancellationToken);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      Task task = this.WriteAsyncInternal(buffer, true, cancellationToken);
      this._asyncWriteTask = task;
      return task;
    }

    /// <summary>Clears all buffers for this stream asynchronously and causes any buffered data to be written to the underlying device.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync()
    {
      if (this.GetType() != typeof (StreamWriter))
        return base.FlushAsync();
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task task = this.FlushAsyncInternal(true, true);
      this._asyncWriteTask = task;
      return task;
    }


    #nullable disable
    private Task FlushAsyncInternal(
      bool flushStream,
      bool flushEncoder,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      return this._charPos == 0 && !flushStream && !flushEncoder ? Task.CompletedTask : Core(flushStream, flushEncoder, cancellationToken);

      async Task Core(bool flushStream, bool flushEncoder, CancellationToken cancellationToken)
      {
        if (!this._haveWrittenPreamble)
        {
          this._haveWrittenPreamble = true;
          byte[] preamble = this._encoding.GetPreamble();
          if (preamble.Length != 0)
            await this._stream.WriteAsync(new ReadOnlyMemory<byte>(preamble), cancellationToken).ConfigureAwait(false);
        }
        byte[] numArray = this._byteBuffer ?? (this._byteBuffer = new byte[this._encoding.GetMaxByteCount(this._charBuffer.Length)]);
        int bytes = this._encoder.GetBytes(new ReadOnlySpan<char>(this._charBuffer, 0, this._charPos), (Span<byte>) numArray, flushEncoder);
        this._charPos = 0;
        if (bytes > 0)
          await this._stream.WriteAsync(new ReadOnlyMemory<byte>(numArray, 0, bytes), cancellationToken).ConfigureAwait(false);
        if (!flushStream)
          return;
        await this._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
      }
    }

    private void ThrowIfDisposed()
    {
      if (!this._disposed)
        return;
      ThrowObjectDisposedException();

      void ThrowObjectDisposedException() => throw new ObjectDisposedException(this.GetType().Name, SR.ObjectDisposed_WriterClosed);
    }
  }
}
