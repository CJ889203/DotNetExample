// Decompiled with JetBrains decompiler
// Type: System.IO.BinaryWriter
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Writes primitive types in binary to a stream and supports writing strings in a specific encoding.</summary>
  public class BinaryWriter : IDisposable, IAsyncDisposable
  {
    /// <summary>Specifies a <see cref="T:System.IO.BinaryWriter" /> with no backing store.</summary>
    public static readonly BinaryWriter Null = new BinaryWriter();
    /// <summary>Holds the underlying stream.</summary>
    protected Stream OutStream;

    #nullable disable
    private readonly Encoding _encoding;
    private readonly bool _leaveOpen;
    private readonly bool _useFastUtf8;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryWriter" /> class that writes to a stream.</summary>
    protected BinaryWriter()
    {
      this.OutStream = Stream.Null;
      this._encoding = Encoding.UTF8;
      this._useFastUtf8 = true;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryWriter" /> class based on the specified stream and using UTF-8 encoding.</summary>
    /// <param name="output">The output stream.</param>
    /// <exception cref="T:System.ArgumentException">The stream does not support writing or is already closed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="output" /> is <see langword="null" />.</exception>
    public BinaryWriter(Stream output)
      : this(output, Encoding.UTF8, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryWriter" /> class based on the specified stream and character encoding.</summary>
    /// <param name="output">The output stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">The stream does not support writing or is already closed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="output" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    public BinaryWriter(Stream output, Encoding encoding)
      : this(output, encoding, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.BinaryWriter" /> class based on the specified stream and character encoding, and optionally leaves the stream open.</summary>
    /// <param name="output">The output stream.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.BinaryWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">The stream does not support writing or is already closed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="output" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    public BinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
    {
      if (output == null)
        throw new ArgumentNullException(nameof (output));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      this.OutStream = output.CanWrite ? output : throw new ArgumentException(SR.Argument_StreamNotWritable);
      this._encoding = encoding;
      this._leaveOpen = leaveOpen;
      this._useFastUtf8 = encoding.IsUTF8CodePage && encoding.EncoderFallback.MaxCharCount <= 1;
    }

    /// <summary>Closes the current <see cref="T:System.IO.BinaryWriter" /> and the underlying stream.</summary>
    public virtual void Close() => this.Dispose(true);

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.BinaryWriter" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      if (this._leaveOpen)
        this.OutStream.Flush();
      else
        this.OutStream.Close();
    }

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.IO.BinaryWriter" /> class.</summary>
    public void Dispose() => this.Dispose(true);

    /// <summary>Asynchronously releases all resources used by the current instance of the <see cref="T:System.IO.BinaryWriter" /> class.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public virtual ValueTask DisposeAsync()
    {
      try
      {
        if (this.GetType() == typeof (BinaryWriter))
        {
          if (this._leaveOpen)
            return new ValueTask(this.OutStream.FlushAsync());
          this.OutStream.Close();
        }
        else
          this.Dispose();
        return new ValueTask();
      }
      catch (Exception ex)
      {
        return ValueTask.FromException(ex);
      }
    }

    /// <summary>Gets the underlying stream of the <see cref="T:System.IO.BinaryWriter" />.</summary>
    /// <returns>The underlying stream associated with the <see langword="BinaryWriter" />.</returns>
    public virtual Stream BaseStream
    {
      get
      {
        this.Flush();
        return this.OutStream;
      }
    }

    /// <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.</summary>
    public virtual void Flush() => this.OutStream.Flush();

    /// <summary>Sets the position within the current stream.</summary>
    /// <param name="offset">A byte offset relative to <paramref name="origin" />.</param>
    /// <param name="origin">A field of <see cref="T:System.IO.SeekOrigin" /> indicating the reference point from which the new position is to be obtained.</param>
    /// <exception cref="T:System.IO.IOException">The file pointer was moved to an invalid location.</exception>
    /// <exception cref="T:System.ArgumentException">The <see cref="T:System.IO.SeekOrigin" /> value is invalid.</exception>
    /// <returns>The position with the current stream.</returns>
    public virtual long Seek(int offset, SeekOrigin origin) => this.OutStream.Seek((long) offset, origin);

    /// <summary>Writes a one-byte <see langword="Boolean" /> value to the current stream, with 0 representing <see langword="false" /> and 1 representing <see langword="true" />.</summary>
    /// <param name="value">The <see langword="Boolean" /> value to write (0 or 1).</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual void Write(bool value) => this.OutStream.WriteByte(value ? (byte) 1 : (byte) 0);

    /// <summary>Writes an unsigned byte to the current stream and advances the stream position by one byte.</summary>
    /// <param name="value">The unsigned byte to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual void Write(byte value) => this.OutStream.WriteByte(value);

    /// <summary>Writes a signed byte to the current stream and advances the stream position by one byte.</summary>
    /// <param name="value">The signed byte to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    [CLSCompliant(false)]
    public virtual void Write(sbyte value) => this.OutStream.WriteByte((byte) value);

    /// <summary>Writes a byte array to the underlying stream.</summary>
    /// <param name="buffer">A byte array containing the data to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    public virtual void Write(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      this.OutStream.Write(buffer, 0, buffer.Length);
    }

    /// <summary>Writes a region of a byte array to the current stream.</summary>
    /// <param name="buffer">A byte array containing the data to write.</param>
    /// <param name="index">The index of the first byte to read from <paramref name="buffer" /> and to write to the stream.</param>
    /// <param name="count">The number of bytes to read from <paramref name="buffer" /> and to write to the stream.</param>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual void Write(byte[] buffer, int index, int count) => this.OutStream.Write(buffer, index, count);

    /// <summary>Writes a Unicode character to the current stream and advances the current position of the stream in accordance with the <see langword="Encoding" /> used and the specific characters being written to the stream.</summary>
    /// <param name="ch">The non-surrogate, Unicode character to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="ch" /> is a single surrogate character.</exception>
    public virtual unsafe void Write(char ch)
    {
      Rune result;
      if (!Rune.TryCreate(ch, out result))
        throw new ArgumentException(SR.Arg_SurrogatesNotAllowedAsSingleChar);
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(8)), 8);
      if (this._useFastUtf8)
      {
        int utf8 = result.EncodeToUtf8(span);
        this.OutStream.Write((ReadOnlySpan<byte>) span.Slice(0, utf8));
      }
      else
      {
        byte[] array = (byte[]) null;
        int maxByteCount = this._encoding.GetMaxByteCount(1);
        if (maxByteCount > span.Length)
        {
          array = ArrayPool<byte>.Shared.Rent(maxByteCount);
          span = (Span<byte>) array;
        }
        int bytes = this._encoding.GetBytes(MemoryMarshal.CreateReadOnlySpan<char>(ref ch, 1), span);
        this.OutStream.Write((ReadOnlySpan<byte>) span.Slice(0, bytes));
        if (array == null)
          return;
        ArrayPool<byte>.Shared.Return(array);
      }
    }

    /// <summary>Writes a character array to the current stream and advances the current position of the stream in accordance with the <see langword="Encoding" /> used and the specific characters being written to the stream.</summary>
    /// <param name="chars">A character array containing the data to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="chars" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    public virtual void Write(char[] chars)
    {
      if (chars == null)
        throw new ArgumentNullException(nameof (chars));
      this.WriteCharsCommonWithoutLengthPrefix((ReadOnlySpan<char>) chars, false);
    }

    /// <summary>Writes a section of a character array to the current stream, and advances the current position of the stream in accordance with the <see langword="Encoding" /> used and perhaps the specific characters being written to the stream.</summary>
    /// <param name="chars">A character array containing the data to write.</param>
    /// <param name="index">The index of the first character to read from <paramref name="chars" /> and to write to the stream.</param>
    /// <param name="count">The number of characters to read from <paramref name="chars" /> and to write to the stream.</param>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="chars" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual void Write(char[] chars, int index, int count)
    {
      if (chars == null)
        throw new ArgumentNullException(nameof (chars));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (index > chars.Length - count)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_IndexCount);
      this.WriteCharsCommonWithoutLengthPrefix((ReadOnlySpan<char>) chars.AsSpan<char>(index, count), false);
    }

    /// <summary>Writes an eight-byte floating-point value to the current stream and advances the stream position by eight bytes.</summary>
    /// <param name="value">The eight-byte floating-point value to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual unsafe void Write(double value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(8)), 8);
      BinaryPrimitives.WriteDoubleLittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes a decimal value to the current stream and advances the stream position by sixteen bytes.</summary>
    /// <param name="value">The decimal value to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual unsafe void Write(Decimal value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> buffer = new Span<byte>((void*) __untypedstackalloc(new IntPtr(16)), 16);
      Decimal.GetBytes(in value, buffer);
      this.OutStream.Write((ReadOnlySpan<byte>) buffer);
    }

    /// <summary>Writes a two-byte signed integer to the current stream and advances the stream position by two bytes.</summary>
    /// <param name="value">The two-byte signed integer to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual unsafe void Write(short value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(2)), 2);
      BinaryPrimitives.WriteInt16LittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes a two-byte unsigned integer to the current stream and advances the stream position by two bytes.</summary>
    /// <param name="value">The two-byte unsigned integer to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    [CLSCompliant(false)]
    public virtual unsafe void Write(ushort value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(2)), 2);
      BinaryPrimitives.WriteUInt16LittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes a four-byte signed integer to the current stream and advances the stream position by four bytes.</summary>
    /// <param name="value">The four-byte signed integer to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual unsafe void Write(int value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(4)), 4);
      BinaryPrimitives.WriteInt32LittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes a four-byte unsigned integer to the current stream and advances the stream position by four bytes.</summary>
    /// <param name="value">The four-byte unsigned integer to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    [CLSCompliant(false)]
    public virtual unsafe void Write(uint value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(4)), 4);
      BinaryPrimitives.WriteUInt32LittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes an eight-byte signed integer to the current stream and advances the stream position by eight bytes.</summary>
    /// <param name="value">The eight-byte signed integer to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual unsafe void Write(long value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(8)), 8);
      BinaryPrimitives.WriteInt64LittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes an eight-byte unsigned integer to the current stream and advances the stream position by eight bytes.</summary>
    /// <param name="value">The eight-byte unsigned integer to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    [CLSCompliant(false)]
    public virtual unsafe void Write(ulong value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(8)), 8);
      BinaryPrimitives.WriteUInt64LittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes a four-byte floating-point value to the current stream and advances the stream position by four bytes.</summary>
    /// <param name="value">The four-byte floating-point value to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual unsafe void Write(float value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(4)), 4);
      BinaryPrimitives.WriteSingleLittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes an two-byte floating-point value to the current stream and advances the stream position by two bytes.</summary>
    /// <param name="value">The two-byte floating-point value to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual unsafe void Write(Half value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(2)), 2);
      BinaryPrimitives.WriteHalfLittleEndian(span, value);
      this.OutStream.Write((ReadOnlySpan<byte>) span);
    }

    /// <summary>Writes a length-prefixed string to this stream in the current encoding of the <see cref="T:System.IO.BinaryWriter" />, and advances the current position of the stream in accordance with the encoding used and the specific characters being written to the stream.</summary>
    /// <param name="value">The value to write.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public virtual unsafe void Write(string value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this._useFastUtf8)
      {
        if (value.Length <= 42)
        {
          // ISSUE: untyped stack allocation
          Span<byte> span = new Span<byte>((void*) __untypedstackalloc(new IntPtr(128)), 128);
          int bytes = this._encoding.GetBytes((ReadOnlySpan<char>) value, span.Slice(1));
          span[0] = (byte) bytes;
          this.OutStream.Write((ReadOnlySpan<byte>) span.Slice(0, bytes + 1));
          return;
        }
        if (value.Length <= 21845)
        {
          byte[] numArray = ArrayPool<byte>.Shared.Rent(value.Length * 3);
          int bytes = this._encoding.GetBytes((ReadOnlySpan<char>) value, (Span<byte>) numArray);
          this.Write7BitEncodedInt(bytes);
          this.OutStream.Write(numArray, 0, bytes);
          ArrayPool<byte>.Shared.Return(numArray);
          return;
        }
      }
      this.Write7BitEncodedInt(this._encoding.GetByteCount(value));
      this.WriteCharsCommonWithoutLengthPrefix((ReadOnlySpan<char>) value, false);
    }

    /// <summary>Writes a span of bytes to the current stream.</summary>
    /// <param name="buffer">The span of bytes to write.</param>
    public virtual void Write(ReadOnlySpan<byte> buffer)
    {
      if (this.GetType() == typeof (BinaryWriter))
      {
        this.OutStream.Write(buffer);
      }
      else
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
    }

    /// <summary>Writes a span of characters to the current stream, and advances the current position of the stream in accordance with the <see langword="Encoding" /> used and perhaps the specific characters being written to the stream.</summary>
    /// <param name="chars">A span of chars to write.</param>
    public virtual void Write(ReadOnlySpan<char> chars) => this.WriteCharsCommonWithoutLengthPrefix(chars, true);


    #nullable disable
    private void WriteCharsCommonWithoutLengthPrefix(
      ReadOnlySpan<char> chars,
      bool useThisWriteOverride)
    {
      if (chars.Length <= 65536)
      {
        int maxByteCount = this._encoding.GetMaxByteCount(chars.Length);
        if (maxByteCount <= 65536)
        {
          byte[] numArray = ArrayPool<byte>.Shared.Rent(maxByteCount);
          int bytes = this._encoding.GetBytes(chars, (Span<byte>) numArray);
          WriteToOutStream(numArray, 0, bytes, useThisWriteOverride);
          ArrayPool<byte>.Shared.Return(numArray);
          return;
        }
      }
      byte[] numArray1 = ArrayPool<byte>.Shared.Rent(65536);
      Encoder encoder = this._encoding.GetEncoder();
      bool completed;
      do
      {
        int charsUsed;
        int bytesUsed;
        encoder.Convert(chars, (Span<byte>) numArray1, true, out charsUsed, out bytesUsed, out completed);
        if (bytesUsed != 0)
          WriteToOutStream(numArray1, 0, bytesUsed, useThisWriteOverride);
        chars = chars.Slice(charsUsed);
      }
      while (!completed);
      ArrayPool<byte>.Shared.Return(numArray1);

      void WriteToOutStream(byte[] buffer, int offset, int count, bool useThisWriteOverride)
      {
        if (useThisWriteOverride)
          this.Write(buffer, offset, count);
        else
          this.OutStream.Write(buffer, offset, count);
      }
    }

    /// <summary>Writes a 32-bit integer in a compressed format.</summary>
    /// <param name="value">The 32-bit integer to be written.</param>
    /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is closed.</exception>
    public void Write7BitEncodedInt(int value)
    {
      uint num;
      for (num = (uint) value; num > (uint) sbyte.MaxValue; num >>= 7)
        this.Write((byte) (num | 4294967168U));
      this.Write((byte) num);
    }

    /// <summary>Writes out a number 7 bits at a time.</summary>
    /// <param name="value">The value to write.</param>
    public void Write7BitEncodedInt64(long value)
    {
      ulong num;
      for (num = (ulong) value; num > (ulong) sbyte.MaxValue; num >>= 7)
        this.Write((byte) ((uint) num | 4294967168U));
      this.Write((byte) num);
    }
  }
}
