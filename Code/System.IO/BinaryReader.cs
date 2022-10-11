// Decompiled with JetBrains decompiler
// Type: System.IO.BinaryReader
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;


#nullable enable
namespace System.IO
{
  /// <summary>Reads primitive data types as binary values in a specific encoding.</summary>
  public class BinaryReader : IDisposable
  {

    #nullable disable
    private readonly Stream _stream;
    private readonly byte[] _buffer;
    private readonly Decoder _decoder;
    private byte[] _charBytes;
    private char[] _charBuffer;
    private readonly int _maxCharsSize;
    private readonly bool _2BytesPerChar;
    private readonly bool _isMemoryStream;
    private readonly bool _leaveOpen;
    private bool _disposed;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryReader" /> class based on the specified stream and using UTF-8 encoding.</summary>
    /// <param name="input">The input stream.</param>
    /// <exception cref="T:System.ArgumentException">The stream does not support reading, is <see langword="null" />, or is already closed.</exception>
    public BinaryReader(Stream input)
      : this(input, Encoding.UTF8, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryReader" /> class based on the specified stream and character encoding.</summary>
    /// <param name="input">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">The stream does not support reading, is <see langword="null" />, or is already closed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="encoding" /> is <see langword="null" />.</exception>
    public BinaryReader(Stream input, Encoding encoding)
      : this(input, encoding, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryReader" /> class based on the specified stream and character encoding, and optionally leaves the stream open.</summary>
    /// <param name="input">The input stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.BinaryReader" /> object is disposed; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">The stream does not support reading, is <see langword="null" />, or is already closed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="encoding" /> or <paramref name="input" /> is <see langword="null" />.</exception>
    public BinaryReader(Stream input, Encoding encoding, bool leaveOpen)
    {
      if (input == null)
        throw new ArgumentNullException(nameof (input));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      this._stream = input.CanRead ? input : throw new ArgumentException(SR.Argument_StreamNotReadable);
      this._decoder = encoding.GetDecoder();
      this._maxCharsSize = encoding.GetMaxCharCount(128);
      int length = encoding.GetMaxByteCount(1);
      if (length < 16)
        length = 16;
      this._buffer = new byte[length];
      this._2BytesPerChar = encoding is UnicodeEncoding;
      this._isMemoryStream = this._stream.GetType() == typeof (MemoryStream);
      this._leaveOpen = leaveOpen;
    }

    /// <summary>Exposes access to the underlying stream of the <see cref="T:System.IO.BinaryReader" />.</summary>
    /// <returns>The underlying stream associated with the <see langword="BinaryReader" />.</returns>
    public virtual Stream BaseStream => this._stream;

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.BinaryReader" /> class and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing && !this._leaveOpen)
        this._stream.Close();
      this._disposed = true;
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.IO.BinaryReader" /> class.</summary>
    public void Dispose() => this.Dispose(true);

    /// <summary>Closes the current reader and the underlying stream.</summary>
    public virtual void Close() => this.Dispose(true);

    private void ThrowIfDisposed()
    {
      if (!this._disposed)
        return;
      ThrowHelper.ThrowObjectDisposedException_FileClosed();
    }

    /// <summary>Returns the next available character and does not advance the byte or character position.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">The current character cannot be decoded into the internal character buffer by using the <see cref="T:System.Text.Encoding" /> selected for the stream.</exception>
    /// <returns>The next available character, or -1 if no more characters are available or the stream does not support seeking.</returns>
    public virtual int PeekChar()
    {
      this.ThrowIfDisposed();
      if (!this._stream.CanSeek)
        return -1;
      long position = this._stream.Position;
      int num = this.Read();
      this._stream.Position = position;
      return num;
    }

    /// <summary>Reads characters from the underlying stream and advances the current position of the stream in accordance with the <see langword="Encoding" /> used and the specific character being read from the stream.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <returns>The next character from the input stream, or -1 if no characters are currently available.</returns>
    public virtual unsafe int Read()
    {
      this.ThrowIfDisposed();
      int num1 = 0;
      long num2 = 0;
      if (this._stream.CanSeek)
        num2 = this._stream.Position;
      if (this._charBytes == null)
        this._charBytes = new byte[128];
      // ISSUE: untyped stack allocation
      Span<char> chars = new Span<char>((void*) __untypedstackalloc(new IntPtr(2)), 1);
      while (num1 == 0)
      {
        int length = this._2BytesPerChar ? 2 : 1;
        int num3 = this._stream.ReadByte();
        this._charBytes[0] = (byte) num3;
        if (num3 == -1)
          length = 0;
        if (length == 2)
        {
          int num4 = this._stream.ReadByte();
          this._charBytes[1] = (byte) num4;
          if (num4 == -1)
            length = 1;
        }
        if (length == 0)
          return -1;
        try
        {
          num1 = this._decoder.GetChars(new ReadOnlySpan<byte>(this._charBytes, 0, length), chars, false);
        }
        catch
        {
          if (this._stream.CanSeek)
            this._stream.Seek(num2 - this._stream.Position, SeekOrigin.Current);
          throw;
        }
      }
      return (int) chars[0];
    }

    /// <summary>Reads the next byte from the current stream and advances the current position of the stream by one byte.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The next byte read from the current stream.</returns>
    public virtual byte ReadByte() => this.InternalReadByte();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private byte InternalReadByte()
    {
      this.ThrowIfDisposed();
      int num = this._stream.ReadByte();
      if (num == -1)
        ThrowHelper.ThrowEndOfFileException();
      return (byte) num;
    }

    /// <summary>Reads a signed byte from this stream and advances the current position of the stream by one byte.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A signed byte read from the current stream.</returns>
    [CLSCompliant(false)]
    public virtual sbyte ReadSByte() => (sbyte) this.InternalReadByte();

    /// <summary>Reads a <see langword="Boolean" /> value from the current stream and advances the current position of the stream by one byte.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>
    /// <see langword="true" /> if the byte is nonzero; otherwise, <see langword="false" />.</returns>
    public virtual bool ReadBoolean() => this.InternalReadByte() > (byte) 0;

    /// <summary>Reads the next character from the current stream and advances the current position of the stream in accordance with the <see langword="Encoding" /> used and the specific character being read from the stream.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">A surrogate character was read.</exception>
    /// <returns>A character read from the current stream.</returns>
    public virtual char ReadChar()
    {
      int num = this.Read();
      if (num == -1)
        ThrowHelper.ThrowEndOfFileException();
      return (char) num;
    }

    /// <summary>Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A 2-byte signed integer read from the current stream.</returns>
    public virtual short ReadInt16() => BinaryPrimitives.ReadInt16LittleEndian(this.InternalRead(2));

    /// <summary>Reads a 2-byte unsigned integer from the current stream using little-endian encoding and advances the position of the stream by two bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A 2-byte unsigned integer read from this stream.</returns>
    [CLSCompliant(false)]
    public virtual ushort ReadUInt16() => BinaryPrimitives.ReadUInt16LittleEndian(this.InternalRead(2));

    /// <summary>Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A 4-byte signed integer read from the current stream.</returns>
    public virtual int ReadInt32() => BinaryPrimitives.ReadInt32LittleEndian(this.InternalRead(4));

    /// <summary>Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A 4-byte unsigned integer read from this stream.</returns>
    [CLSCompliant(false)]
    public virtual uint ReadUInt32() => BinaryPrimitives.ReadUInt32LittleEndian(this.InternalRead(4));

    /// <summary>Reads an 8-byte signed integer from the current stream and advances the current position of the stream by eight bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>An 8-byte signed integer read from the current stream.</returns>
    public virtual long ReadInt64() => BinaryPrimitives.ReadInt64LittleEndian(this.InternalRead(8));

    /// <summary>Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <returns>An 8-byte unsigned integer read from this stream.</returns>
    [CLSCompliant(false)]
    public virtual ulong ReadUInt64() => BinaryPrimitives.ReadUInt64LittleEndian(this.InternalRead(8));

    /// <summary>Reads a 2-byte floating point value from the current stream and advances the current position of the stream by two bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A 2-byte floating point value read from the current stream.</returns>
    public virtual Half ReadHalf() => BitConverter.Int16BitsToHalf(BinaryPrimitives.ReadInt16LittleEndian(this.InternalRead(2)));

    /// <summary>Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A 4-byte floating point value read from the current stream.</returns>
    public virtual float ReadSingle() => BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(this.InternalRead(4)));

    /// <summary>Reads an 8-byte floating point value from the current stream and advances the current position of the stream by eight bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>An 8-byte floating point value read from the current stream.</returns>
    public virtual double ReadDouble() => BitConverter.Int64BitsToDouble(BinaryPrimitives.ReadInt64LittleEndian(this.InternalRead(8)));

    /// <summary>Reads a decimal value from the current stream and advances the current position of the stream by sixteen bytes.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A decimal value read from the current stream.</returns>
    public virtual Decimal ReadDecimal()
    {
      ReadOnlySpan<byte> span = this.InternalRead(16);
      try
      {
        return Decimal.ToDecimal(span);
      }
      catch (ArgumentException ex)
      {
        throw new IOException(SR.Arg_DecBitCtor, (Exception) ex);
      }
    }

    /// <summary>Reads a string from the current stream. The string is prefixed with the length, encoded as an integer seven bits at a time.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The string being read.</returns>
    public virtual string ReadString()
    {
      this.ThrowIfDisposed();
      int num1 = 0;
      int num2 = this.Read7BitEncodedInt();
      if (num2 < 0)
        throw new IOException(SR.Format(SR.IO_InvalidStringLen_Len, (object) num2));
      if (num2 == 0)
        return string.Empty;
      if (this._charBytes == null)
        this._charBytes = new byte[128];
      if (this._charBuffer == null)
        this._charBuffer = new char[this._maxCharsSize];
      StringBuilder sb = (StringBuilder) null;
      do
      {
        int byteCount = this._stream.Read(this._charBytes, 0, num2 - num1 > 128 ? 128 : num2 - num1);
        if (byteCount == 0)
          ThrowHelper.ThrowEndOfFileException();
        int chars = this._decoder.GetChars(this._charBytes, 0, byteCount, this._charBuffer, 0);
        if (num1 == 0 && byteCount == num2)
          return new string(this._charBuffer, 0, chars);
        if (sb == null)
          sb = StringBuilderCache.Acquire(Math.Min(num2, 360));
        sb.Append(this._charBuffer, 0, chars);
        num1 += byteCount;
      }
      while (num1 < num2);
      return StringBuilderCache.GetStringAndRelease(sb);
    }

    /// <summary>Reads the specified number of characters from the stream, starting from a specified point in the character array.</summary>
    /// <param name="buffer">The buffer to read data into.</param>
    /// <param name="index">The starting point in the buffer at which to begin reading into the buffer.</param>
    /// <param name="count">The number of characters to read.</param>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.
    /// 
    /// -or-
    /// 
    /// The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The total number of characters read into the buffer. This might be less than the number of characters requested if that many characters are not currently available, or it might be zero if the end of the stream is reached.</returns>
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
      this.ThrowIfDisposed();
      return this.InternalReadChars(new Span<char>(buffer, index, count));
    }

    /// <summary>Reads, from the current stream, the same number of characters as the length of the provided buffer, writes them in the provided buffer, and advances the current position in accordance with the <see langword="Encoding" /> used and the specific character being read from the stream.</summary>
    /// <param name="buffer">A span of characters. When this method returns, the contents of this region are replaced by the characters read from the current source.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The total number of characters read into the buffer. This might be less than the number of characters requested if that many characters are not currently available, or it might be zero if the end of the stream is reached.</returns>
    public virtual int Read(Span<char> buffer)
    {
      this.ThrowIfDisposed();
      return this.InternalReadChars(buffer);
    }


    #nullable disable
    private int InternalReadChars(Span<char> buffer)
    {
      int num = 0;
      while (!buffer.IsEmpty)
      {
        int count = buffer.Length;
        if (this._2BytesPerChar)
          count <<= 1;
        if (count > 1 && (!(this._decoder is DecoderNLS decoder) || decoder.HasState))
        {
          --count;
          if (this._2BytesPerChar && count > 2)
            count -= 2;
        }
        ReadOnlySpan<byte> bytes;
        if (this._isMemoryStream)
        {
          MemoryStream stream = (MemoryStream) this._stream;
          int position = stream.InternalGetPosition();
          int length = stream.InternalEmulateRead(count);
          bytes = new ReadOnlySpan<byte>(stream.InternalGetBuffer(), position, length);
        }
        else
        {
          if (this._charBytes == null)
            this._charBytes = new byte[128];
          if (count > 128)
            count = 128;
          bytes = new ReadOnlySpan<byte>(this._charBytes, 0, this._stream.Read(this._charBytes, 0, count));
        }
        if (!bytes.IsEmpty)
        {
          int chars = this._decoder.GetChars(bytes, buffer, false);
          buffer = buffer.Slice(chars);
          num += chars;
        }
        else
          break;
      }
      return num;
    }


    #nullable enable
    /// <summary>Reads the specified number of characters from the current stream, returns the data in a character array, and advances the current position in accordance with the <see langword="Encoding" /> used and the specific character being read from the stream.</summary>
    /// <param name="count">The number of characters to read.</param>
    /// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="count" /> is negative.</exception>
    /// <returns>A character array containing data read from the underlying stream. This might be less than the number of characters requested if the end of the stream is reached.</returns>
    public virtual char[] ReadChars(int count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      this.ThrowIfDisposed();
      if (count == 0)
        return Array.Empty<char>();
      char[] chArray = new char[count];
      int length = this.InternalReadChars(new Span<char>(chArray));
      if (length != count)
      {
        char[] dst = new char[length];
        Buffer.BlockCopy((Array) chArray, 0, (Array) dst, 0, 2 * length);
        chArray = dst;
      }
      return chArray;
    }

    /// <summary>Reads the specified number of bytes from the stream, starting from a specified point in the byte array.</summary>
    /// <param name="buffer">The buffer to read data into.</param>
    /// <param name="index">The starting point in the buffer at which to begin reading into the buffer.</param>
    /// <param name="count">The number of bytes to read.</param>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.
    /// 
    /// -or-
    /// 
    /// The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The number of bytes read into <paramref name="buffer" />. This might be less than the number of bytes requested if that many bytes are not available, or it might be zero if the end of the stream is reached.</returns>
    public virtual int Read(byte[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      this.ThrowIfDisposed();
      return this._stream.Read(buffer, index, count);
    }

    /// <summary>Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</summary>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current source.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public virtual int Read(Span<byte> buffer)
    {
      this.ThrowIfDisposed();
      return this._stream.Read(buffer);
    }

    /// <summary>Reads the specified number of bytes from the current stream into a byte array and advances the current position by that number of bytes.</summary>
    /// <param name="count">The number of bytes to read. This value must be 0 or a non-negative number or an exception will occur.</param>
    /// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than <paramref name="count" />. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="count" /> is negative.</exception>
    /// <returns>A byte array containing data read from the underlying stream. This might be less than the number of bytes requested if the end of the stream is reached.</returns>
    public virtual byte[] ReadBytes(int count)
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      this.ThrowIfDisposed();
      if (count == 0)
        return Array.Empty<byte>();
      byte[] numArray = new byte[count];
      int length = 0;
      do
      {
        int num = this._stream.Read(numArray, length, count);
        if (num != 0)
        {
          length += num;
          count -= num;
        }
        else
          break;
      }
      while (count > 0);
      if (length != numArray.Length)
      {
        byte[] dst = new byte[length];
        Buffer.BlockCopy((Array) numArray, 0, (Array) dst, 0, length);
        numArray = dst;
      }
      return numArray;
    }


    #nullable disable
    private ReadOnlySpan<byte> InternalRead(int numBytes)
    {
      if (this._isMemoryStream)
        return ((MemoryStream) this._stream).InternalReadSpan(numBytes);
      this.ThrowIfDisposed();
      int offset = 0;
      do
      {
        int num = this._stream.Read(this._buffer, offset, numBytes - offset);
        if (num == 0)
          ThrowHelper.ThrowEndOfFileException();
        offset += num;
      }
      while (offset < numBytes);
      return (ReadOnlySpan<byte>) this._buffer;
    }

    /// <summary>Fills the internal buffer with the specified number of bytes read from the stream.</summary>
    /// <param name="numBytes">The number of bytes to be read.</param>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached before <paramref name="numBytes" /> could be read.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Requested <paramref name="numBytes" /> is larger than the internal buffer size.</exception>
    protected virtual void FillBuffer(int numBytes)
    {
      if (numBytes < 0 || numBytes > this._buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (numBytes), SR.ArgumentOutOfRange_BinaryReaderFillBuffer);
      int offset = 0;
      this.ThrowIfDisposed();
      if (numBytes == 1)
      {
        int num = this._stream.ReadByte();
        if (num == -1)
          ThrowHelper.ThrowEndOfFileException();
        this._buffer[0] = (byte) num;
      }
      else
      {
        do
        {
          int num = this._stream.Read(this._buffer, offset, numBytes - offset);
          if (num == 0)
            ThrowHelper.ThrowEndOfFileException();
          offset += num;
        }
        while (offset < numBytes);
      }
    }

    /// <summary>Reads in a 32-bit integer in compressed format.</summary>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.FormatException">The stream is corrupted.</exception>
    /// <returns>A 32-bit integer in compressed format.</returns>
    public int Read7BitEncodedInt()
    {
      uint num1 = 0;
      for (int index = 0; index < 28; index += 7)
      {
        byte num2 = this.ReadByte();
        num1 |= (uint) (((int) num2 & (int) sbyte.MaxValue) << index);
        if (num2 <= (byte) 127)
          return (int) num1;
      }
      byte num3 = this.ReadByte();
      if (num3 > (byte) 15)
        throw new FormatException(SR.Format_Bad7BitInt);
      return (int) (num1 | (uint) num3 << 28);
    }

    /// <summary>Reads a number 7 bits at a time.</summary>
    /// <returns>The number that is read from this binary reader instance.</returns>
    public long Read7BitEncodedInt64()
    {
      ulong num1 = 0;
      for (int index = 0; index < 63; index += 7)
      {
        byte num2 = this.ReadByte();
        num1 |= (ulong) (((long) num2 & (long) sbyte.MaxValue) << index);
        if (num2 <= (byte) 127)
          return (long) num1;
      }
      byte num3 = this.ReadByte();
      if (num3 > (byte) 1)
        throw new FormatException(SR.Format_Bad7BitInt);
      return (long) (num1 | (ulong) num3 << 63);
    }
  }
}
