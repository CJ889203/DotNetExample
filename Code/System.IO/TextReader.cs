// Decompiled with JetBrains decompiler
// Type: System.IO.TextReader
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Represents a reader that can read a sequential series of characters.</summary>
  public abstract class TextReader : MarshalByRefObject, IDisposable
  {
    /// <summary>Provides a <see langword="TextReader" /> with no data to read from.</summary>
    public static readonly TextReader Null = (TextReader) new TextReader.NullTextReader();

    /// <summary>Closes the <see cref="T:System.IO.TextReader" /> and releases any system resources associated with the <see langword="TextReader" />.</summary>
    public virtual void Close()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases all resources used by the <see cref="T:System.IO.TextReader" /> object.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.TextReader" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    /// <summary>Reads the next character without changing the state of the reader or the character source. Returns the next available character without actually reading it from the reader.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader" /> is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <returns>An integer representing the next character to be read, or -1 if no more characters are available or the reader does not support seeking.</returns>
    public virtual int Peek() => -1;

    /// <summary>Reads the next character from the text reader and advances the character position by one character.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader" /> is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <returns>The next character from the text reader, or -1 if no more characters are available. The default implementation returns -1.</returns>
    public virtual int Read() => -1;

    /// <summary>Reads a specified maximum number of characters from the current reader and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read. If the end of the reader is reached before the specified number of characters is read into the buffer, the method returns.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader" /> is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <returns>The number of characters that have been read. The number will be less than or equal to <paramref name="count" />, depending on whether the data is available within the reader. This method returns 0 (zero) if it is called when no more characters are left to read.</returns>
    public virtual int Read(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      int num1;
      for (num1 = 0; num1 < count; ++num1)
      {
        int num2 = this.Read();
        if (num2 != -1)
          buffer[index + num1] = (char) num2;
        else
          break;
      }
      return num1;
    }

    /// <summary>Reads the characters from the current reader and writes the data to the specified buffer.</summary>
    /// <param name="buffer">When this method returns, contains the specified span of characters replaced by the characters read from the current source.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">The number of characters read from the stream is larger than the length of the <paramref name="buffer" />.</exception>
    /// <returns>The number of characters that have been read. The number will be less than or equal to the <paramref name="buffer" /> length, depending on whether the data is available within the reader. This method returns 0 (zero) if it is called when no more characters are left to read.</returns>
    public virtual int Read(Span<char> buffer)
    {
      char[] chArray = ArrayPool<char>.Shared.Rent(buffer.Length);
      try
      {
        int length = this.Read(chArray, 0, buffer.Length);
        if ((uint) length > (uint) buffer.Length)
          throw new IOException(SR.IO_InvalidReadLength);
        new Span<char>(chArray, 0, length).CopyTo(buffer);
        return length;
      }
      finally
      {
        ArrayPool<char>.Shared.Return(chArray);
      }
    }

    /// <summary>Reads all characters from the current position to the end of the text reader and returns them as one string.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader" /> is closed.</exception>
    /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue" /></exception>
    /// <returns>A string that contains all characters from the current position to the end of the text reader.</returns>
    public virtual string ReadToEnd()
    {
      char[] buffer = new char[4096];
      StringBuilder stringBuilder = new StringBuilder(4096);
      int charCount;
      while ((charCount = this.Read(buffer, 0, buffer.Length)) != 0)
        stringBuilder.Append(buffer, 0, charCount);
      return stringBuilder.ToString();
    }

    /// <summary>Reads a specified maximum number of characters from the current text reader and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, this parameter contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> -1) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader" /> is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <returns>The number of characters that have been read. The number will be less than or equal to <paramref name="count" />, depending on whether all input characters have been read.</returns>
    public virtual int ReadBlock(char[] buffer, int index, int count)
    {
      int num1 = 0;
      int num2;
      do
      {
        num1 += num2 = this.Read(buffer, index + num1, count - num1);
      }
      while (num2 > 0 && num1 < count);
      return num1;
    }

    /// <summary>Reads the characters from the current stream and writes the data to a buffer.</summary>
    /// <param name="buffer">When this method returns, contains the specified span of characters replaced by the characters read from the current source.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.StreamReader" /> is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The number of characters that have been read. The number will be less than or equal to the <paramref name="buffer" /> length, depending on whether all input characters have been read.</returns>
    public virtual int ReadBlock(Span<char> buffer)
    {
      char[] chArray = ArrayPool<char>.Shared.Rent(buffer.Length);
      try
      {
        int length = this.ReadBlock(chArray, 0, buffer.Length);
        if ((uint) length > (uint) buffer.Length)
          throw new IOException(SR.IO_InvalidReadLength);
        new Span<char>(chArray, 0, length).CopyTo(buffer);
        return length;
      }
      finally
      {
        ArrayPool<char>.Shared.Return(chArray);
      }
    }

    /// <summary>Reads a line of characters from the text reader and returns the data as a string.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextReader" /> is closed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue" /></exception>
    /// <returns>The next line from the reader, or <see langword="null" /> if all characters have been read.</returns>
    public virtual string? ReadLine()
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num;
      while (true)
      {
        num = this.Read();
        switch (num)
        {
          case -1:
            goto label_6;
          case 10:
          case 13:
            goto label_2;
          default:
            stringBuilder.Append((char) num);
            continue;
        }
      }
label_2:
      if (num == 13 && this.Peek() == 10)
        this.Read();
      return stringBuilder.ToString();
label_6:
      return stringBuilder.Length > 0 ? stringBuilder.ToString() : (string) null;
    }

    /// <summary>Reads a line of characters asynchronously and returns the data as a string.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The text reader has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the next line from the text reader, or is <see langword="null" /> if all of the characters have been read.</returns>
    public virtual Task<string?> ReadLineAsync() => Task<string>.Factory.StartNew((Func<object, string>) (state => ((TextReader) state).ReadLine()), (object) this, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

    /// <summary>Reads all characters from the current position to the end of the text reader asynchronously and returns them as one string.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The text reader has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains a string with the characters from the current position to the end of the text reader.</returns>
    public virtual async Task<string> ReadToEndAsync()
    {
      StringBuilder sb = new StringBuilder(4096);
      char[] chars = ArrayPool<char>.Shared.Rent(4096);
      try
      {
        while (true)
        {
          int charCount;
          if ((charCount = await this.ReadAsyncInternal((Memory<char>) chars, new CancellationToken()).ConfigureAwait(false)) != 0)
            sb.Append(chars, 0, charCount);
          else
            break;
        }
      }
      finally
      {
        ArrayPool<char>.Shared.Return(chars);
      }
      string endAsync = sb.ToString();
      sb = (StringBuilder) null;
      chars = (char[]) null;
      return endAsync;
    }

    /// <summary>Reads a specified maximum number of characters from the current text reader asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read. If the end of the text is reached before the specified number of characters is read into the buffer, the current method returns.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The text reader has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the text has been reached.</returns>
    public virtual Task<int> ReadAsync(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0 || count < 0)
        throw new ArgumentOutOfRangeException(index < 0 ? nameof (index) : nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      return this.ReadAsyncInternal(new Memory<char>(buffer, index, count), new CancellationToken()).AsTask();
    }

    /// <summary>Asynchronously reads the characters from the current stream into a memory block.</summary>
    /// <param name="buffer">When this method returns, contains the specified memory block of characters replaced by the characters read from the current source.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A value task that represents the asynchronous read operation. The value of the type parameter contains the number of characters that have been read, or 0 if at the end of the stream and no data was read. The number will be less than or equal to the <paramref name="buffer" /> length, depending on whether the data is available within the stream.</returns>
    public virtual ValueTask<int> ReadAsync(
      Memory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      ArraySegment<char> segment;
      return new ValueTask<int>(MemoryMarshal.TryGetArray<char>((ReadOnlyMemory<char>) buffer, out segment) ? this.ReadAsync(segment.Array, segment.Offset, segment.Count) : Task<int>.Factory.StartNew((Func<object, int>) (state =>
      {
        TupleSlim<TextReader, Memory<char>> tupleSlim = (TupleSlim<TextReader, Memory<char>>) state;
        return tupleSlim.Item1.Read(tupleSlim.Item2.Span);
      }), (object) new TupleSlim<TextReader, Memory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default));
    }


    #nullable disable
    internal virtual ValueTask<int> ReadAsyncInternal(
      Memory<char> buffer,
      CancellationToken cancellationToken)
    {
      return new ValueTask<int>(Task<int>.Factory.StartNew((Func<object, int>) (state =>
      {
        TupleSlim<TextReader, Memory<char>> tupleSlim = (TupleSlim<TextReader, Memory<char>>) state;
        return tupleSlim.Item1.Read(tupleSlim.Item2.Span);
      }), (object) new TupleSlim<TextReader, Memory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default));
    }


    #nullable enable
    /// <summary>Reads a specified maximum number of characters from the current text reader asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read. If the end of the text is reached before the specified number of characters is read into the buffer, the current method returns.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The text reader has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the text has been reached.</returns>
    public virtual Task<int> ReadBlockAsync(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0 || count < 0)
        throw new ArgumentOutOfRangeException(index < 0 ? nameof (index) : nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      return this.ReadBlockAsyncInternal(new Memory<char>(buffer, index, count), new CancellationToken()).AsTask();
    }

    /// <summary>Asynchronously reads the characters from the current stream and writes the data to a buffer.</summary>
    /// <param name="buffer">When this method returns, contains the specified memory block of characters replaced by the characters read from the current source.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A value task that represents the asynchronous read operation. The value of the type parameter contains the total number of characters read into the buffer. The result value can be less than the number of characters requested if the number of characters currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public virtual ValueTask<int> ReadBlockAsync(
      Memory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      ArraySegment<char> segment;
      return new ValueTask<int>(MemoryMarshal.TryGetArray<char>((ReadOnlyMemory<char>) buffer, out segment) ? this.ReadBlockAsync(segment.Array, segment.Offset, segment.Count) : Task<int>.Factory.StartNew((Func<object, int>) (state =>
      {
        TupleSlim<TextReader, Memory<char>> tupleSlim = (TupleSlim<TextReader, Memory<char>>) state;
        return tupleSlim.Item1.ReadBlock(tupleSlim.Item2.Span);
      }), (object) new TupleSlim<TextReader, Memory<char>>(this, buffer), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default));
    }


    #nullable disable
    internal async ValueTask<int> ReadBlockAsyncInternal(
      Memory<char> buffer,
      CancellationToken cancellationToken)
    {
      int n = 0;
      int num;
      do
      {
        num = await this.ReadAsyncInternal(buffer.Slice(n), cancellationToken).ConfigureAwait(false);
        n += num;
      }
      while (num > 0 && n < buffer.Length);
      return n;
    }


    #nullable enable
    /// <summary>Creates a thread-safe wrapper around the specified <see langword="TextReader" />.</summary>
    /// <param name="reader">The <see langword="TextReader" /> to synchronize.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="reader" /> is <see langword="null" />.</exception>
    /// <returns>A thread-safe <see cref="T:System.IO.TextReader" />.</returns>
    public static TextReader Synchronized(TextReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      return !(reader is TextReader.SyncTextReader) ? (TextReader) new TextReader.SyncTextReader(reader) : reader;
    }


    #nullable disable
    private sealed class NullTextReader : TextReader
    {
      public override int Read(char[] buffer, int index, int count) => 0;

      public override string ReadLine() => (string) null;
    }

    internal sealed class SyncTextReader : TextReader
    {
      internal readonly TextReader _in;

      internal SyncTextReader(TextReader t) => this._in = t;

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override void Close() => this._in.Close();

      [MethodImpl(MethodImplOptions.Synchronized)]
      protected override void Dispose(bool disposing)
      {
        if (!disposing)
          return;
        this._in.Dispose();
      }

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override int Peek() => this._in.Peek();

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override int Read() => this._in.Read();

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override int Read(char[] buffer, int index, int count) => this._in.Read(buffer, index, count);

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override int ReadBlock(char[] buffer, int index, int count) => this._in.ReadBlock(buffer, index, count);

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override string ReadLine() => this._in.ReadLine();

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override string ReadToEnd() => this._in.ReadToEnd();

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override Task<string> ReadLineAsync() => Task.FromResult<string>(this.ReadLine());

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override Task<string> ReadToEndAsync() => Task.FromResult<string>(this.ReadToEnd());

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
      {
        if (buffer == null)
          throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
        if (index < 0 || count < 0)
          throw new ArgumentOutOfRangeException(index < 0 ? nameof (index) : nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
        if (buffer.Length - index < count)
          throw new ArgumentException(SR.Argument_InvalidOffLen);
        return Task.FromResult<int>(this.ReadBlock(buffer, index, count));
      }

      [MethodImpl(MethodImplOptions.Synchronized)]
      public override Task<int> ReadAsync(char[] buffer, int index, int count)
      {
        if (buffer == null)
          throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
        if (index < 0 || count < 0)
          throw new ArgumentOutOfRangeException(index < 0 ? nameof (index) : nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
        if (buffer.Length - index < count)
          throw new ArgumentException(SR.Argument_InvalidOffLen);
        return Task.FromResult<int>(this.Read(buffer, index, count));
      }
    }
  }
}
