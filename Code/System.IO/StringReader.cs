// Decompiled with JetBrains decompiler
// Type: System.IO.StringReader
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Implements a <see cref="T:System.IO.TextReader" /> that reads from a string.</summary>
  public class StringReader : TextReader
  {

    #nullable disable
    private string _s;
    private int _pos;
    private int _length;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StringReader" /> class that reads from the specified string.</summary>
    /// <param name="s">The string to which the <see cref="T:System.IO.StringReader" /> should be initialized.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="s" /> parameter is <see langword="null" />.</exception>
    public StringReader(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      this._s = s;
      this._length = s.Length;
    }

    /// <summary>Closes the <see cref="T:System.IO.StringReader" />.</summary>
    public override void Close() => this.Dispose(true);

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.StringReader" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      this._s = (string) null;
      this._pos = 0;
      this._length = 0;
      base.Dispose(disposing);
    }

    /// <summary>Returns the next available character but does not consume it.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current reader is closed.</exception>
    /// <returns>An integer representing the next character to be read, or -1 if no more characters are available or the stream does not support seeking.</returns>
    public override int Peek()
    {
      if (this._s == null)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_ReaderClosed);
      return this._pos == this._length ? -1 : (int) this._s[this._pos];
    }

    /// <summary>Reads the next character from the input string and advances the character position by one character.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current reader is closed.</exception>
    /// <returns>The next character from the underlying string, or -1 if no more characters are available.</returns>
    public override int Read()
    {
      if (this._s == null)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_ReaderClosed);
      return this._pos == this._length ? -1 : (int) this._s[this._pos++];
    }

    /// <summary>Reads a block of characters from the input string and advances the character position by <paramref name="count" />.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
    /// <param name="index">The starting index in the buffer.</param>
    /// <param name="count">The number of characters to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current reader is closed.</exception>
    /// <returns>The total number of characters read into the buffer. This can be less than the number of characters requested if that many characters are not currently available, or zero if the end of the underlying string has been reached.</returns>
    public override int Read(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      if (this._s == null)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_ReaderClosed);
      int count1 = this._length - this._pos;
      if (count1 > 0)
      {
        if (count1 > count)
          count1 = count;
        this._s.CopyTo(this._pos, buffer, index, count1);
        this._pos += count1;
      }
      return count1;
    }

    /// <summary>Reads all the characters from the input string, starting at the current position, and advances the current position to the end of the input string.</summary>
    /// <param name="buffer">When this method returns, contains the characters read from the current source. If the total number of characters read is zero, the span remains unmodified.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current string reader instance is closed.</exception>
    /// <returns>The total number of characters read into the buffer.</returns>
    public override int Read(Span<char> buffer)
    {
      if (this.GetType() != typeof (StringReader))
        return base.Read(buffer);
      if (this._s == null)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_ReaderClosed);
      int length = this._length - this._pos;
      if (length > 0)
      {
        if (length > buffer.Length)
          length = buffer.Length;
        this._s.AsSpan(this._pos, length).CopyTo(buffer);
        this._pos += length;
      }
      return length;
    }

    /// <summary>Reads all the characters from the input string starting at the current position and advances the current position to the end of the input string.</summary>
    /// <param name="buffer">When this method returns, contains the characters read from the current source. If the total number of characters read is zero, the span remains unmodified.</param>
    /// <exception cref="T:System.ObjectDisposedException">The current string reader instance is closed.</exception>
    /// <returns>The total number of characters read into the buffer.</returns>
    public override int ReadBlock(Span<char> buffer) => this.Read(buffer);

    /// <summary>Reads all characters from the current position to the end of the string and returns them as a single string.</summary>
    /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current reader is closed.</exception>
    /// <returns>The content from the current position to the end of the underlying string.</returns>
    public override string ReadToEnd()
    {
      if (this._s == null)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_ReaderClosed);
      string end = this._pos != 0 ? this._s.Substring(this._pos, this._length - this._pos) : this._s;
      this._pos = this._length;
      return end;
    }

    /// <summary>Reads a line of characters from the current string and returns the data as a string.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The current reader is closed.</exception>
    /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
    /// <returns>The next line from the current string, or <see langword="null" /> if the end of the string is reached.</returns>
    public override string? ReadLine()
    {
      if (this._s == null)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_ReaderClosed);
      int pos;
      for (pos = this._pos; pos < this._length; ++pos)
      {
        char ch = this._s[pos];
        switch (ch)
        {
          case '\n':
          case '\r':
            string str = this._s.Substring(this._pos, pos - this._pos);
            this._pos = pos + 1;
            if (ch == '\r' && this._pos < this._length && this._s[this._pos] == '\n')
              ++this._pos;
            return str;
          default:
            continue;
        }
      }
      if (pos <= this._pos)
        return (string) null;
      string str1 = this._s.Substring(this._pos, pos - this._pos);
      this._pos = pos;
      return str1;
    }

    /// <summary>Reads a line of characters asynchronously from the current string and returns the data as a string.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The string reader has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the next line from the string reader, or is <see langword="null" /> if all the characters have been read.</returns>
    public override Task<string?> ReadLineAsync() => Task.FromResult<string>(this.ReadLine());

    /// <summary>Reads all characters from the current position to the end of the string asynchronously and returns them as a single string.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The string reader has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains a string with the characters from the current position to the end of the string.</returns>
    public override Task<string> ReadToEndAsync() => Task.FromResult<string>(this.ReadToEnd());

    /// <summary>Reads a specified maximum number of characters from the current string asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read. If the end of the string is reached before the specified number of characters is written into the buffer, the method returns.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The string reader has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the string has been reached.</returns>
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

    /// <summary>Asynchronously reads all the characters from the input string starting at the current position and advances the current position to the end of the input string.</summary>
    /// <param name="buffer">When this method returns, contains the characters read from the current source. If the total number of characters read is zero, the span remains unmodified.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task representing the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of characters read into the buffer.</returns>
    public override ValueTask<int> ReadBlockAsync(
      Memory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return !cancellationToken.IsCancellationRequested ? new ValueTask<int>(this.ReadBlock(buffer.Span)) : ValueTask.FromCanceled<int>(cancellationToken);
    }

    /// <summary>Reads a specified maximum number of characters from the current string asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read. If the end of the string is reached before the specified number of characters is written into the buffer, the method returns.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The string reader has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the string has been reached.</returns>
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

    /// <summary>Asynchronously reads all the characters from the input string, starting at the current position, and advances the current position to the end of the input string.</summary>
    /// <param name="buffer">When this method returns, contains the characters read from the current source.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of characters read into the buffer.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return !cancellationToken.IsCancellationRequested ? new ValueTask<int>(this.Read(buffer.Span)) : ValueTask.FromCanceled<int>(cancellationToken);
    }
  }
}
