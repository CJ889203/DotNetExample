// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Utf8JsonWriter
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Provides a high-performance API for forward-only, non-cached writing of UTF-8 encoded JSON text.</summary>
  [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
  public sealed class Utf8JsonWriter : IDisposable, IAsyncDisposable
  {
    private static readonly int s_newLineLength = Environment.NewLine.Length;

    #nullable disable
    private IBufferWriter<byte> _output;
    private Stream _stream;
    private ArrayBufferWriter<byte> _arrayBufferWriter;
    private Memory<byte> _memory;
    private bool _inObject;
    private JsonTokenType _tokenType;
    private BitStack _bitStack;
    private int _currentDepth;
    private JsonWriterOptions _options;
    private static readonly char[] s_singleLineCommentDelimiter = new char[2]
    {
      '*',
      '/'
    };

    /// <summary>Gets the number of bytes written by the <see cref="T:System.Text.Json.Utf8JsonWriter" /> so far that have not yet been flushed to the output and committed.</summary>
    /// <returns>The number of bytes written so far by the <see cref="T:System.Text.Json.Utf8JsonWriter" /> that have not yet been flushed to the output and committed.</returns>
    public int BytesPending { get; private set; }

    /// <summary>Gets the total number of bytes committed to the output by the current instance so far.</summary>
    /// <returns>The total number of bytes committed to the output by the <see cref="T:System.Text.Json.Utf8JsonWriter" /> so far.</returns>
    public long BytesCommitted { get; private set; }

    /// <summary>Gets the custom behavior when writing JSON using this instance, which indicates whether to format the output while writing, whether to skip structural JSON validation, and which characters to escape.</summary>
    /// <returns>The custom behavior of this instance of the writer for formatting, validating, and escaping.</returns>
    public JsonWriterOptions Options => this._options;

    private int Indentation => this.CurrentDepth * 2;

    internal JsonTokenType TokenType => this._tokenType;

    /// <summary>Gets the depth of the current token.</summary>
    /// <returns>The depth of the current token.</returns>
    public int CurrentDepth => this._currentDepth & int.MaxValue;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Utf8JsonWriter" /> class using the specified <see cref="T:System.Buffers.IBufferWriter`1" /> to write the output to and customization options.</summary>
    /// <param name="bufferWriter">The destination for writing JSON text.</param>
    /// <param name="options">Defines the customized behavior of the <see cref="T:System.Text.Json.Utf8JsonWriter" />. By default, it writes minimized JSON (with no extra white space) and validates that the JSON being written is structurally valid according to the JSON RFC.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="bufferWriter" /> is <see langword="null" />.</exception>
    public Utf8JsonWriter(IBufferWriter<byte> bufferWriter, JsonWriterOptions options = default (JsonWriterOptions))
    {
      this._output = bufferWriter ?? throw new ArgumentNullException(nameof (bufferWriter));
      this._options = options;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Utf8JsonWriter" /> class using the specified stream to write the output to and customization options.</summary>
    /// <param name="utf8Json">The destination for writing JSON text.</param>
    /// <param name="options">Defines the customized behavior of the <see cref="T:System.Text.Json.Utf8JsonWriter" />. By default, it writes minimized JSON (with no extra white space) and validates that the JSON being written is structurally valid according to the JSON RFC.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> is <see langword="null" />.</exception>
    public Utf8JsonWriter(Stream utf8Json, JsonWriterOptions options = default (JsonWriterOptions))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      this._stream = utf8Json.CanWrite ? utf8Json : throw new ArgumentException(SR.StreamNotWritable);
      this._options = options;
      this._arrayBufferWriter = new ArrayBufferWriter<byte>();
    }

    /// <summary>Resets the internal state of this instance so that it can be reused.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This instance has been disposed.</exception>
    public void Reset()
    {
      this.CheckNotDisposed();
      this._arrayBufferWriter?.Clear();
      this.ResetHelper();
    }

    /// <summary>Resets the internal state of this instance so that it can be reused with a new instance of <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="utf8Json">The destination for writing JSON text.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This instance has been disposed.</exception>
    public void Reset(Stream utf8Json)
    {
      this.CheckNotDisposed();
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      this._stream = utf8Json.CanWrite ? utf8Json : throw new ArgumentException(SR.StreamNotWritable);
      if (this._arrayBufferWriter == null)
        this._arrayBufferWriter = new ArrayBufferWriter<byte>();
      else
        this._arrayBufferWriter.Clear();
      this._output = (IBufferWriter<byte>) null;
      this.ResetHelper();
    }

    /// <summary>Resets the internal state of this instance so that it can be reused with a new instance of <see cref="T:System.Buffers.IBufferWriter`1" />.</summary>
    /// <param name="bufferWriter">The destination for writing JSON text.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="bufferWriter" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This instance has been disposed.</exception>
    public void Reset(IBufferWriter<byte> bufferWriter)
    {
      this.CheckNotDisposed();
      this._output = bufferWriter ?? throw new ArgumentNullException(nameof (bufferWriter));
      this._stream = (Stream) null;
      this._arrayBufferWriter = (ArrayBufferWriter<byte>) null;
      this.ResetHelper();
    }

    private void ResetHelper()
    {
      this.BytesPending = 0;
      this.BytesCommitted = 0L;
      this._memory = new Memory<byte>();
      this._inObject = false;
      this._tokenType = JsonTokenType.None;
      this._currentDepth = 0;
      this._bitStack = new BitStack();
    }

    private void CheckNotDisposed()
    {
      if (this._stream == null && this._output == null)
        throw new ObjectDisposedException(nameof (Utf8JsonWriter));
    }

    /// <summary>Commits the JSON text written so far, which makes it visible to the output destination.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This instance has been disposed.</exception>
    public void Flush()
    {
      this.CheckNotDisposed();
      this._memory = new Memory<byte>();
      if (this._stream != null)
      {
        if (this.BytesPending != 0)
        {
          this._arrayBufferWriter.Advance(this.BytesPending);
          this.BytesPending = 0;
          this._stream.Write(this._arrayBufferWriter.WrittenSpan);
          this.BytesCommitted += (long) this._arrayBufferWriter.WrittenCount;
          this._arrayBufferWriter.Clear();
        }
        this._stream.Flush();
      }
      else
      {
        if (this.BytesPending == 0)
          return;
        this._output.Advance(this.BytesPending);
        this.BytesCommitted += (long) this.BytesPending;
        this.BytesPending = 0;
      }
    }

    /// <summary>Commits any leftover JSON text that has not yet been flushed and releases all resources used by the current instance.</summary>
    public void Dispose()
    {
      if (this._stream == null && this._output == null)
        return;
      this.Flush();
      this.ResetHelper();
      this._stream = (Stream) null;
      this._arrayBufferWriter = (ArrayBufferWriter<byte>) null;
      this._output = (IBufferWriter<byte>) null;
    }

    /// <summary>Asynchronously commits any leftover JSON text that has not yet been flushed and releases all resources used by the current instance.</summary>
    /// <returns>A task representing the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
      if (this._stream == null && this._output == null)
        return;
      await this.FlushAsync().ConfigureAwait(false);
      this.ResetHelper();
      this._stream = (Stream) null;
      this._arrayBufferWriter = (ArrayBufferWriter<byte>) null;
      this._output = (IBufferWriter<byte>) null;
    }

    /// <summary>Asynchronously commits the JSON text written so far, which makes it visible to the output destination.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <exception cref="T:System.ObjectDisposedException">This instance has been disposed.</exception>
    /// <returns>A task representing the asynchronous flush operation.</returns>
    public async Task FlushAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
      this.CheckNotDisposed();
      this._memory = new Memory<byte>();
      if (this._stream != null)
      {
        if (this.BytesPending != 0)
        {
          this._arrayBufferWriter.Advance(this.BytesPending);
          this.BytesPending = 0;
          await this._stream.WriteAsync(this._arrayBufferWriter.WrittenMemory, cancellationToken).ConfigureAwait(false);
          this.BytesCommitted += (long) this._arrayBufferWriter.WrittenCount;
          this._arrayBufferWriter.Clear();
        }
        await this._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
      }
      else
      {
        if (this.BytesPending == 0)
          return;
        this._output.Advance(this.BytesPending);
        this.BytesCommitted += (long) this.BytesPending;
        this.BytesPending = 0;
      }
    }

    /// <summary>Writes the beginning of a JSON array.</summary>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON exceeds the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this write operation would produce invalid JSON.</exception>
    public void WriteStartArray()
    {
      this.WriteStart((byte) 91);
      this._tokenType = JsonTokenType.StartArray;
    }

    /// <summary>Writes the beginning of a JSON object.</summary>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON exceeds the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteStartObject()
    {
      this.WriteStart((byte) 123);
      this._tokenType = JsonTokenType.StartObject;
    }

    private void WriteStart(byte token)
    {
      if (this.CurrentDepth >= 1000)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.DepthTooLarge, this._currentDepth, (byte) 0, JsonTokenType.None);
      if (this._options.IndentedOrNotSkipValidation)
        this.WriteStartSlow(token);
      else
        this.WriteStartMinimized(token);
      this._currentDepth &= int.MaxValue;
      ++this._currentDepth;
    }

    private void WriteStartMinimized(byte token)
    {
      if (this._memory.Length - this.BytesPending < 2)
        this.Grow(2);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = token;
    }

    private void WriteStartSlow(byte token)
    {
      if (this._options.Indented)
      {
        if (!this._options.SkipValidation)
        {
          this.ValidateStart();
          this.UpdateBitStackOnStart(token);
        }
        this.WriteStartIndented(token);
      }
      else
      {
        this.ValidateStart();
        this.UpdateBitStackOnStart(token);
        this.WriteStartMinimized(token);
      }
    }

    private void ValidateStart()
    {
      if (this._inObject)
      {
        if (this._tokenType == JsonTokenType.PropertyName)
          return;
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CannotStartObjectArrayWithoutProperty, 0, (byte) 0, this._tokenType);
      }
      else
      {
        if (this.CurrentDepth != 0 || this._tokenType == JsonTokenType.None)
          return;
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CannotStartObjectArrayAfterPrimitiveOrClose, 0, (byte) 0, this._tokenType);
      }
    }

    private void WriteStartIndented(byte token)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 1 + 3;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      span[this.BytesPending++] = token;
    }

    /// <summary>Writes the beginning of a JSON array with a pre-encoded property name as the key.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON array to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON has exceeded the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteStartArray(JsonEncodedText propertyName)
    {
      this.WriteStartHelper(propertyName.EncodedUtf8Bytes, (byte) 91);
      this._tokenType = JsonTokenType.StartArray;
    }

    /// <summary>Writes the beginning of a JSON object with a pre-encoded property name as the key.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON has exceeded the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteStartObject(JsonEncodedText propertyName)
    {
      this.WriteStartHelper(propertyName.EncodedUtf8Bytes, (byte) 123);
      this._tokenType = JsonTokenType.StartObject;
    }


    #nullable disable
    private void WriteStartHelper(ReadOnlySpan<byte> utf8PropertyName, byte token)
    {
      this.ValidateDepth();
      this.WriteStartByOptions(utf8PropertyName, token);
      this._currentDepth &= int.MaxValue;
      ++this._currentDepth;
    }


    #nullable enable
    /// <summary>Writes the beginning of a JSON array with a property name specified as a read-only span of bytes as the key.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON array to be written.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON exceeds the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this write operation would produce invalid JSON.</exception>
    public void WriteStartArray(ReadOnlySpan<byte> utf8PropertyName)
    {
      this.ValidatePropertyNameAndDepth(utf8PropertyName);
      this.WriteStartEscape(utf8PropertyName, (byte) 91);
      this._currentDepth &= int.MaxValue;
      ++this._currentDepth;
      this._tokenType = JsonTokenType.StartArray;
    }

    /// <summary>Writes the beginning of a JSON object with a property name specified as a read-only span of bytes as the key.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON exceeds the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this write operation would produce invalid JSON.</exception>
    public void WriteStartObject(ReadOnlySpan<byte> utf8PropertyName)
    {
      this.ValidatePropertyNameAndDepth(utf8PropertyName);
      this.WriteStartEscape(utf8PropertyName, (byte) 123);
      this._currentDepth &= int.MaxValue;
      ++this._currentDepth;
      this._tokenType = JsonTokenType.StartObject;
    }


    #nullable disable
    private void WriteStartEscape(ReadOnlySpan<byte> utf8PropertyName, byte token)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStartEscapeProperty(utf8PropertyName, token, firstEscapeIndexProp);
      else
        this.WriteStartByOptions(utf8PropertyName, token);
    }

    private void WriteStartByOptions(ReadOnlySpan<byte> utf8PropertyName, byte token)
    {
      this.ValidateWritingProperty(token);
      if (this._options.Indented)
        this.WritePropertyNameIndented(utf8PropertyName, token);
      else
        this.WritePropertyNameMinimized(utf8PropertyName, token);
    }

    private unsafe void WriteStartEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      byte token,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteStartByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), token);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }


    #nullable enable
    /// <summary>Writes the beginning of a JSON array with a property name specified as a string as the key.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON array to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON exceeds the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteStartArray(string propertyName) => this.WriteStartArray((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan());

    /// <summary>Writes the beginning of a JSON object with a property name specified as a string as the key.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON exceeds the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteStartObject(string propertyName) => this.WriteStartObject((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan());

    /// <summary>Writes the beginning of a JSON array with a property name specified as a read-only character span as the key.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON array to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON exceeds the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this write operation would produce invalid JSON.</exception>
    public void WriteStartArray(ReadOnlySpan<char> propertyName)
    {
      this.ValidatePropertyNameAndDepth(propertyName);
      this.WriteStartEscape(propertyName, (byte) 91);
      this._currentDepth &= int.MaxValue;
      ++this._currentDepth;
      this._tokenType = JsonTokenType.StartArray;
    }

    /// <summary>Writes the beginning of a JSON object with a property name specififed as a read-only character span as the key.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">The depth of the JSON exceeds the maximum depth of 1,000.
    /// 
    /// -or-
    /// 
    /// Validation is enabled, and this write operation would produce invalid JSON.</exception>
    public void WriteStartObject(ReadOnlySpan<char> propertyName)
    {
      this.ValidatePropertyNameAndDepth(propertyName);
      this.WriteStartEscape(propertyName, (byte) 123);
      this._currentDepth &= int.MaxValue;
      ++this._currentDepth;
      this._tokenType = JsonTokenType.StartObject;
    }


    #nullable disable
    private void WriteStartEscape(ReadOnlySpan<char> propertyName, byte token)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStartEscapeProperty(propertyName, token, firstEscapeIndexProp);
      else
        this.WriteStartByOptions(propertyName, token);
    }

    private void WriteStartByOptions(ReadOnlySpan<char> propertyName, byte token)
    {
      this.ValidateWritingProperty(token);
      if (this._options.Indented)
        this.WritePropertyNameIndented(propertyName, token);
      else
        this.WritePropertyNameMinimized(propertyName, token);
    }

    private unsafe void WriteStartEscapeProperty(
      ReadOnlySpan<char> propertyName,
      byte token,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteStartByOptions((ReadOnlySpan<char>) destination.Slice(0, written), token);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    /// <summary>Writes the end of a JSON array.</summary>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteEndArray()
    {
      this.WriteEnd((byte) 93);
      this._tokenType = JsonTokenType.EndArray;
    }

    /// <summary>Writes the end of a JSON object.</summary>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteEndObject()
    {
      this.WriteEnd((byte) 125);
      this._tokenType = JsonTokenType.EndObject;
    }

    private void WriteEnd(byte token)
    {
      if (this._options.IndentedOrNotSkipValidation)
        this.WriteEndSlow(token);
      else
        this.WriteEndMinimized(token);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      if (this.CurrentDepth == 0)
        return;
      --this._currentDepth;
    }

    private void WriteEndMinimized(byte token)
    {
      if (this._memory.Length - this.BytesPending < 1)
        this.Grow(1);
      this._memory.Span[this.BytesPending++] = token;
    }

    private void WriteEndSlow(byte token)
    {
      if (this._options.Indented)
      {
        if (!this._options.SkipValidation)
          this.ValidateEnd(token);
        this.WriteEndIndented(token);
      }
      else
      {
        this.ValidateEnd(token);
        this.WriteEndMinimized(token);
      }
    }

    private void ValidateEnd(byte token)
    {
      if (this._bitStack.CurrentDepth <= 0 || this._tokenType == JsonTokenType.PropertyName)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.MismatchedObjectArray, 0, token, this._tokenType);
      if (token == (byte) 93)
      {
        if (this._inObject)
          ThrowHelper.ThrowInvalidOperationException(ExceptionResource.MismatchedObjectArray, 0, token, this._tokenType);
      }
      else if (!this._inObject)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.MismatchedObjectArray, 0, token, this._tokenType);
      this._inObject = this._bitStack.Pop();
    }

    private void WriteEndIndented(byte token)
    {
      if (this._tokenType == JsonTokenType.StartObject || this._tokenType == JsonTokenType.StartArray)
      {
        this.WriteEndMinimized(token);
      }
      else
      {
        int indentation = this.Indentation;
        if (indentation != 0)
          indentation -= 2;
        int requiredSize = indentation + 3;
        if (this._memory.Length - this.BytesPending < requiredSize)
          this.Grow(requiredSize);
        Span<byte> span = this._memory.Span;
        this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
        span[this.BytesPending++] = token;
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteNewLine(Span<byte> output)
    {
      if (Utf8JsonWriter.s_newLineLength == 2)
        output[this.BytesPending++] = (byte) 13;
      output[this.BytesPending++] = (byte) 10;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateBitStackOnStart(byte token)
    {
      if (token == (byte) 91)
      {
        this._bitStack.PushFalse();
        this._inObject = false;
      }
      else
      {
        this._bitStack.PushTrue();
        this._inObject = true;
      }
    }

    private void Grow(int requiredSize)
    {
      if (this._memory.Length == 0)
      {
        this.FirstCallToGetMemory(requiredSize);
      }
      else
      {
        int sizeHint = Math.Max(4096, requiredSize);
        if (this._stream != null)
        {
          int num = this.BytesPending + sizeHint;
          JsonHelpers.ValidateInt32MaxArrayLength((uint) num);
          this._memory = this._arrayBufferWriter.GetMemory(num);
        }
        else
        {
          this._output.Advance(this.BytesPending);
          this.BytesCommitted += (long) this.BytesPending;
          this.BytesPending = 0;
          this._memory = this._output.GetMemory(sizeHint);
          if (this._memory.Length >= sizeHint)
            return;
          ThrowHelper.ThrowInvalidOperationException_NeedLargerSpan();
        }
      }
    }

    private void FirstCallToGetMemory(int requiredSize)
    {
      int sizeHint = Math.Max(256, requiredSize);
      if (this._stream != null)
      {
        this._memory = this._arrayBufferWriter.GetMemory(sizeHint);
      }
      else
      {
        this._memory = this._output.GetMemory(sizeHint);
        if (this._memory.Length >= sizeHint)
          return;
        ThrowHelper.ThrowInvalidOperationException_NeedLargerSpan();
      }
    }

    private void SetFlagToAddListSeparatorBeforeNextItem() => this._currentDepth |= int.MinValue;


    #nullable enable
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
    {
      get
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
        interpolatedStringHandler.AppendLiteral("BytesCommitted = ");
        interpolatedStringHandler.AppendFormatted<long>(this.BytesCommitted);
        interpolatedStringHandler.AppendLiteral(" BytesPending = ");
        interpolatedStringHandler.AppendFormatted<int>(this.BytesPending);
        interpolatedStringHandler.AppendLiteral(" CurrentDepth = ");
        interpolatedStringHandler.AppendFormatted<int>(this.CurrentDepth);
        return interpolatedStringHandler.ToStringAndClear();
      }
    }

    /// <summary>Writes the pre-encoded property name and raw bytes value (as a Base64 encoded JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON-encoded name of the property to write.</param>
    /// <param name="bytes">The binary data to write as Base64 encoded text.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteBase64String(JsonEncodedText propertyName, ReadOnlySpan<byte> bytes)
    {
      ReadOnlySpan<byte> encodedUtf8Bytes = propertyName.EncodedUtf8Bytes;
      JsonWriterHelper.ValidateBytes(bytes);
      this.WriteBase64ByOptions(encodedUtf8Bytes, bytes);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes the property name and raw bytes value (as a Base64 encoded JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="bytes">The binary data to write as Base64 encoded text.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteBase64String(string propertyName, ReadOnlySpan<byte> bytes) => this.WriteBase64String((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), bytes);

    /// <summary>Writes the property name and raw bytes value (as a Base64 encoded JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="bytes">The binary data to write as Base64 encoded text.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteBase64String(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> bytes)
    {
      JsonWriterHelper.ValidatePropertyAndBytes(propertyName, bytes);
      this.WriteBase64Escape(propertyName, bytes);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes the property name and raw bytes value (as a Base64 encoded JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded name of the property to write.</param>
    /// <param name="bytes">The binary data to write as Base64 encoded text.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteBase64String(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> bytes)
    {
      JsonWriterHelper.ValidatePropertyAndBytes(utf8PropertyName, bytes);
      this.WriteBase64Escape(utf8PropertyName, bytes);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable disable
    private void WriteBase64Escape(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> bytes)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteBase64EscapeProperty(propertyName, bytes, firstEscapeIndexProp);
      else
        this.WriteBase64ByOptions(propertyName, bytes);
    }

    private void WriteBase64Escape(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> bytes)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteBase64EscapeProperty(utf8PropertyName, bytes, firstEscapeIndexProp);
      else
        this.WriteBase64ByOptions(utf8PropertyName, bytes);
    }

    private unsafe void WriteBase64EscapeProperty(
      ReadOnlySpan<char> propertyName,
      ReadOnlySpan<byte> bytes,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteBase64ByOptions((ReadOnlySpan<char>) destination.Slice(0, written), bytes);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteBase64EscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> bytes,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteBase64ByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), bytes);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteBase64ByOptions(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> bytes)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteBase64Indented(propertyName, bytes);
      else
        this.WriteBase64Minimized(propertyName, bytes);
    }

    private void WriteBase64ByOptions(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> bytes)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteBase64Indented(utf8PropertyName, bytes);
      else
        this.WriteBase64Minimized(utf8PropertyName, bytes);
    }

    private void WriteBase64Minimized(
      ReadOnlySpan<char> escapedPropertyName,
      ReadOnlySpan<byte> bytes)
    {
      int encodedToUtf8Length = Base64.GetMaxEncodedToUtf8Length(bytes.Length);
      int requiredSize = escapedPropertyName.Length * 3 + encodedToUtf8Length + 6;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      this.Base64EncodeAndWrite(bytes, span, encodedToUtf8Length);
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteBase64Minimized(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<byte> bytes)
    {
      int encodedToUtf8Length = Base64.GetMaxEncodedToUtf8Length(bytes.Length);
      int requiredSize = escapedPropertyName.Length + encodedToUtf8Length + 6;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      this.Base64EncodeAndWrite(bytes, span, encodedToUtf8Length);
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteBase64Indented(
      ReadOnlySpan<char> escapedPropertyName,
      ReadOnlySpan<byte> bytes)
    {
      int indentation = this.Indentation;
      int encodedToUtf8Length = Base64.GetMaxEncodedToUtf8Length(bytes.Length);
      int requiredSize = indentation + escapedPropertyName.Length * 3 + encodedToUtf8Length + 7 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      this.Base64EncodeAndWrite(bytes, span, encodedToUtf8Length);
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteBase64Indented(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<byte> bytes)
    {
      int indentation = this.Indentation;
      int encodedToUtf8Length = Base64.GetMaxEncodedToUtf8Length(bytes.Length);
      int requiredSize = indentation + escapedPropertyName.Length + encodedToUtf8Length + 7 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      this.Base64EncodeAndWrite(bytes, span, encodedToUtf8Length);
      span[this.BytesPending++] = (byte) 34;
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.DateTime" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(JsonEncodedText propertyName, DateTime value)
    {
      this.WriteStringByOptions(propertyName.EncodedUtf8Bytes, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a <see cref="T:System.DateTime" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteString(string propertyName, DateTime value) => this.WriteString((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.DateTime" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<char> propertyName, DateTime value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      this.WriteStringEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes a UTF-8 property name and a <see cref="T:System.DateTime" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<byte> utf8PropertyName, DateTime value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      this.WriteStringEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable disable
    private void WriteStringEscape(ReadOnlySpan<char> propertyName, DateTime value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStringEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteStringByOptions(propertyName, value);
    }

    private void WriteStringEscape(ReadOnlySpan<byte> utf8PropertyName, DateTime value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStringEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteStringByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteStringEscapeProperty(
      ReadOnlySpan<char> propertyName,
      DateTime value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteStringEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      DateTime value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteStringByOptions(ReadOnlySpan<char> propertyName, DateTime value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(propertyName, value);
      else
        this.WriteStringMinimized(propertyName, value);
    }

    private void WriteStringByOptions(ReadOnlySpan<byte> utf8PropertyName, DateTime value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(utf8PropertyName, value);
      else
        this.WriteStringMinimized(utf8PropertyName, value);
    }

    private void WriteStringMinimized(ReadOnlySpan<char> escapedPropertyName, DateTime value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 33 + 6;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringMinimized(ReadOnlySpan<byte> escapedPropertyName, DateTime value)
    {
      int requiredSize = escapedPropertyName.Length + 33 + 5 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(ReadOnlySpan<char> escapedPropertyName, DateTime value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 33 + 7 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(ReadOnlySpan<byte> escapedPropertyName, DateTime value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 33 + 6 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    internal unsafe void WritePropertyName(DateTime value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> buffer = new Span<byte>((void*) __untypedstackalloc(new IntPtr(33)), 33);
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeTrimmed(buffer, value, out bytesWritten);
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) buffer.Slice(0, bytesWritten));
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.DateTimeOffset" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(JsonEncodedText propertyName, DateTimeOffset value)
    {
      this.WriteStringByOptions(propertyName.EncodedUtf8Bytes, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a <see cref="T:System.DateTimeOffset" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteString(string propertyName, DateTimeOffset value) => this.WriteString((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.DateTimeOffset" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<char> propertyName, DateTimeOffset value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      this.WriteStringEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes a UTF-8 property name and a <see cref="T:System.DateTimeOffset" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<byte> utf8PropertyName, DateTimeOffset value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      this.WriteStringEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable disable
    private void WriteStringEscape(ReadOnlySpan<char> propertyName, DateTimeOffset value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStringEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteStringByOptions(propertyName, value);
    }

    private void WriteStringEscape(ReadOnlySpan<byte> utf8PropertyName, DateTimeOffset value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStringEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteStringByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteStringEscapeProperty(
      ReadOnlySpan<char> propertyName,
      DateTimeOffset value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteStringEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      DateTimeOffset value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteStringByOptions(ReadOnlySpan<char> propertyName, DateTimeOffset value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(propertyName, value);
      else
        this.WriteStringMinimized(propertyName, value);
    }

    private void WriteStringByOptions(ReadOnlySpan<byte> utf8PropertyName, DateTimeOffset value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(utf8PropertyName, value);
      else
        this.WriteStringMinimized(utf8PropertyName, value);
    }

    private void WriteStringMinimized(ReadOnlySpan<char> escapedPropertyName, DateTimeOffset value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 33 + 6;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeOffsetTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringMinimized(ReadOnlySpan<byte> escapedPropertyName, DateTimeOffset value)
    {
      int requiredSize = escapedPropertyName.Length + 33 + 5 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeOffsetTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(ReadOnlySpan<char> escapedPropertyName, DateTimeOffset value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 33 + 7 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeOffsetTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(ReadOnlySpan<byte> escapedPropertyName, DateTimeOffset value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 33 + 6 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeOffsetTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    internal unsafe void WritePropertyName(DateTimeOffset value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> buffer = new Span<byte>((void*) __untypedstackalloc(new IntPtr(33)), 33);
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeOffsetTrimmed(buffer, value, out bytesWritten);
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) buffer.Slice(0, bytesWritten));
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.Decimal" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteNumber(JsonEncodedText propertyName, Decimal value)
    {
      this.WriteNumberByOptions(propertyName.EncodedUtf8Bytes, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a <see cref="T:System.Decimal" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteNumber(string propertyName, Decimal value) => this.WriteNumber((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.Decimal" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<char> propertyName, Decimal value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      this.WriteNumberEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    /// <summary>Writes a property name specified as a read-only span of bytes and a <see cref="T:System.Decimal" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, Decimal value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      this.WriteNumberEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }


    #nullable disable
    private void WriteNumberEscape(ReadOnlySpan<char> propertyName, Decimal value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(propertyName, value);
    }

    private void WriteNumberEscape(ReadOnlySpan<byte> utf8PropertyName, Decimal value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<char> propertyName,
      Decimal value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      Decimal value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteNumberByOptions(ReadOnlySpan<char> propertyName, Decimal value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(propertyName, value);
      else
        this.WriteNumberMinimized(propertyName, value);
    }

    private void WriteNumberByOptions(ReadOnlySpan<byte> utf8PropertyName, Decimal value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(utf8PropertyName, value);
      else
        this.WriteNumberMinimized(utf8PropertyName, value);
    }

    private void WriteNumberMinimized(ReadOnlySpan<char> escapedPropertyName, Decimal value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 31 + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberMinimized(ReadOnlySpan<byte> escapedPropertyName, Decimal value)
    {
      int requiredSize = escapedPropertyName.Length + 31 + 3 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<char> escapedPropertyName, Decimal value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 31 + 5 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<byte> escapedPropertyName, Decimal value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 31 + 4 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    internal unsafe void WritePropertyName(Decimal value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(31)), 31);
      int bytesWritten;
      Utf8Formatter.TryFormat(value, destination, out bytesWritten);
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.Double" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteNumber(JsonEncodedText propertyName, double value)
    {
      ReadOnlySpan<byte> encodedUtf8Bytes = propertyName.EncodedUtf8Bytes;
      JsonWriterHelper.ValidateDouble(value);
      this.WriteNumberByOptions(encodedUtf8Bytes, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a <see cref="T:System.Double" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteNumber(string propertyName, double value) => this.WriteNumber((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.Double" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<char> propertyName, double value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      JsonWriterHelper.ValidateDouble(value);
      this.WriteNumberEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    /// <summary>Writes a property name specified as a read-only span of bytes and a <see cref="T:System.Double" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, double value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      JsonWriterHelper.ValidateDouble(value);
      this.WriteNumberEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }


    #nullable disable
    private void WriteNumberEscape(ReadOnlySpan<char> propertyName, double value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(propertyName, value);
    }

    private void WriteNumberEscape(ReadOnlySpan<byte> utf8PropertyName, double value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<char> propertyName,
      double value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      double value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteNumberByOptions(ReadOnlySpan<char> propertyName, double value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(propertyName, value);
      else
        this.WriteNumberMinimized(propertyName, value);
    }

    private void WriteNumberByOptions(ReadOnlySpan<byte> utf8PropertyName, double value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(utf8PropertyName, value);
      else
        this.WriteNumberMinimized(utf8PropertyName, value);
    }

    private void WriteNumberMinimized(ReadOnlySpan<char> escapedPropertyName, double value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 128 + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8JsonWriter.TryFormatDouble(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberMinimized(ReadOnlySpan<byte> escapedPropertyName, double value)
    {
      int requiredSize = escapedPropertyName.Length + 128 + 3 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8JsonWriter.TryFormatDouble(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<char> escapedPropertyName, double value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 128 + 5 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8JsonWriter.TryFormatDouble(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<byte> escapedPropertyName, double value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 128 + 4 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8JsonWriter.TryFormatDouble(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    internal unsafe void WritePropertyName(double value)
    {
      JsonWriterHelper.ValidateDouble(value);
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(128)), 128);
      int bytesWritten;
      Utf8JsonWriter.TryFormatDouble(value, destination, out bytesWritten);
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.Single" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteNumber(JsonEncodedText propertyName, float value)
    {
      ReadOnlySpan<byte> encodedUtf8Bytes = propertyName.EncodedUtf8Bytes;
      JsonWriterHelper.ValidateSingle(value);
      this.WriteNumberByOptions(encodedUtf8Bytes, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a <see cref="T:System.Single" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteNumber(string propertyName, float value) => this.WriteNumber((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.Single" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<char> propertyName, float value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      JsonWriterHelper.ValidateSingle(value);
      this.WriteNumberEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    /// <summary>Writes a property name specified as a read-only span of bytes and a <see cref="T:System.Single" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, float value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      JsonWriterHelper.ValidateSingle(value);
      this.WriteNumberEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }


    #nullable disable
    private void WriteNumberEscape(ReadOnlySpan<char> propertyName, float value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(propertyName, value);
    }

    private void WriteNumberEscape(ReadOnlySpan<byte> utf8PropertyName, float value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<char> propertyName,
      float value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      float value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteNumberByOptions(ReadOnlySpan<char> propertyName, float value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(propertyName, value);
      else
        this.WriteNumberMinimized(propertyName, value);
    }

    private void WriteNumberByOptions(ReadOnlySpan<byte> utf8PropertyName, float value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(utf8PropertyName, value);
      else
        this.WriteNumberMinimized(utf8PropertyName, value);
    }

    private void WriteNumberMinimized(ReadOnlySpan<char> escapedPropertyName, float value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 128 + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8JsonWriter.TryFormatSingle(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberMinimized(ReadOnlySpan<byte> escapedPropertyName, float value)
    {
      int requiredSize = escapedPropertyName.Length + 128 + 3 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8JsonWriter.TryFormatSingle(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<char> escapedPropertyName, float value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 128 + 5 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8JsonWriter.TryFormatSingle(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<byte> escapedPropertyName, float value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 128 + 4 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8JsonWriter.TryFormatSingle(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    internal unsafe void WritePropertyName(float value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(128)), 128);
      int bytesWritten;
      Utf8JsonWriter.TryFormatSingle(value, destination, out bytesWritten);
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.Guid" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(JsonEncodedText propertyName, Guid value)
    {
      this.WriteStringByOptions(propertyName.EncodedUtf8Bytes, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a <see cref="T:System.Guid" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteString(string propertyName, Guid value) => this.WriteString((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.Guid" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<char> propertyName, Guid value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      this.WriteStringEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes a UTF-8 property name and a <see cref="T:System.Guid" /> value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<byte> utf8PropertyName, Guid value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      this.WriteStringEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable disable
    private void WriteStringEscape(ReadOnlySpan<char> propertyName, Guid value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStringEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteStringByOptions(propertyName, value);
    }

    private void WriteStringEscape(ReadOnlySpan<byte> utf8PropertyName, Guid value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStringEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteStringByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteStringEscapeProperty(
      ReadOnlySpan<char> propertyName,
      Guid value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteStringEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      Guid value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteStringByOptions(ReadOnlySpan<char> propertyName, Guid value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(propertyName, value);
      else
        this.WriteStringMinimized(propertyName, value);
    }

    private void WriteStringByOptions(ReadOnlySpan<byte> utf8PropertyName, Guid value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(utf8PropertyName, value);
      else
        this.WriteStringMinimized(utf8PropertyName, value);
    }

    private void WriteStringMinimized(ReadOnlySpan<char> escapedPropertyName, Guid value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 36 + 6;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringMinimized(ReadOnlySpan<byte> escapedPropertyName, Guid value)
    {
      int requiredSize = escapedPropertyName.Length + 36 + 5 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(ReadOnlySpan<char> escapedPropertyName, Guid value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 36 + 7 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(ReadOnlySpan<byte> escapedPropertyName, Guid value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 36 + 6 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    internal unsafe void WritePropertyName(Guid value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(36)), 36);
      int bytesWritten;
      Utf8Formatter.TryFormat(value, destination, out bytesWritten);
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidatePropertyNameAndDepth(ReadOnlySpan<char> propertyName)
    {
      if (propertyName.Length <= 166666666 && this.CurrentDepth < 1000)
        return;
      ThrowHelper.ThrowInvalidOperationOrArgumentException(propertyName, this._currentDepth);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidatePropertyNameAndDepth(ReadOnlySpan<byte> utf8PropertyName)
    {
      if (utf8PropertyName.Length <= 166666666 && this.CurrentDepth < 1000)
        return;
      ThrowHelper.ThrowInvalidOperationOrArgumentException(utf8PropertyName, this._currentDepth);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidateDepth()
    {
      if (this.CurrentDepth < 1000)
        return;
      ThrowHelper.ThrowInvalidOperationException(this._currentDepth);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidateWritingProperty()
    {
      if (this._options.SkipValidation || this._inObject && this._tokenType != JsonTokenType.PropertyName)
        return;
      ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CannotWritePropertyWithinArray, 0, (byte) 0, this._tokenType);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ValidateWritingProperty(byte token)
    {
      if (this._options.SkipValidation)
        return;
      if (!this._inObject || this._tokenType == JsonTokenType.PropertyName)
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CannotWritePropertyWithinArray, 0, (byte) 0, this._tokenType);
      this.UpdateBitStackOnStart(token);
    }

    private void WritePropertyNameMinimized(ReadOnlySpan<byte> escapedPropertyName, byte token)
    {
      int requiredSize = escapedPropertyName.Length + 4 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = token;
    }

    private void WritePropertyNameIndented(ReadOnlySpan<byte> escapedPropertyName, byte token)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 5 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = token;
    }

    private void WritePropertyNameMinimized(ReadOnlySpan<char> escapedPropertyName, byte token)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 5;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = token;
    }

    private void WritePropertyNameIndented(ReadOnlySpan<char> escapedPropertyName, byte token)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 6 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = token;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void TranscodeAndWrite(ReadOnlySpan<char> escapedPropertyName, Span<byte> output)
    {
      int bytesWritten;
      JsonWriterHelper.ToUtf8(MemoryMarshal.AsBytes<char>(escapedPropertyName), output.Slice(this.BytesPending), out int _, out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    /// <summary>Writes the pre-encoded property name and the JSON literal null as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteNull(JsonEncodedText propertyName)
    {
      this.WriteLiteralHelper(propertyName.EncodedUtf8Bytes, JsonConstants.NullValue);
      this._tokenType = JsonTokenType.Null;
    }

    internal void WriteNullSection(ReadOnlySpan<byte> escapedPropertyNameSection)
    {
      if (this._options.Indented)
      {
        this.WriteLiteralHelper(escapedPropertyNameSection.Slice(1, escapedPropertyNameSection.Length - 3), JsonConstants.NullValue);
        this._tokenType = JsonTokenType.Null;
      }
      else
      {
        ReadOnlySpan<byte> nullValue = JsonConstants.NullValue;
        this.WriteLiteralSection(escapedPropertyNameSection, nullValue);
        this.SetFlagToAddListSeparatorBeforeNextItem();
        this._tokenType = JsonTokenType.Null;
      }
    }

    private void WriteLiteralHelper(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> value)
    {
      this.WriteLiteralByOptions(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and the JSON literal null as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteNull(string propertyName) => this.WriteNull((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan());

    /// <summary>Writes a property name specified as a read-only character span and the JSON literal null as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNull(ReadOnlySpan<char> propertyName)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      ReadOnlySpan<byte> nullValue = JsonConstants.NullValue;
      this.WriteLiteralEscape(propertyName, nullValue);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Null;
    }

    /// <summary>Writes a property name specified as a read-only span of bytes and the JSON literal null as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNull(ReadOnlySpan<byte> utf8PropertyName)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      ReadOnlySpan<byte> nullValue = JsonConstants.NullValue;
      this.WriteLiteralEscape(utf8PropertyName, nullValue);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Null;
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.Boolean" /> value (as a JSON literal true or false) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON literal true or false as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteBoolean(JsonEncodedText propertyName, bool value)
    {
      if (value)
      {
        this.WriteLiteralHelper(propertyName.EncodedUtf8Bytes, JsonConstants.TrueValue);
        this._tokenType = JsonTokenType.True;
      }
      else
      {
        this.WriteLiteralHelper(propertyName.EncodedUtf8Bytes, JsonConstants.FalseValue);
        this._tokenType = JsonTokenType.False;
      }
    }

    /// <summary>Writes a property name specified as a string and a <see cref="T:System.Boolean" /> value (as a JSON literal true or false) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON literal true or false as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteBoolean(string propertyName, bool value) => this.WriteBoolean((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.Boolean" /> value (as a JSON literal true or false) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON literal true or false as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteBoolean(ReadOnlySpan<char> propertyName, bool value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      ReadOnlySpan<byte> readOnlySpan = value ? JsonConstants.TrueValue : JsonConstants.FalseValue;
      this.WriteLiteralEscape(propertyName, readOnlySpan);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = value ? JsonTokenType.True : JsonTokenType.False;
    }

    /// <summary>Writes a property name specified as a read-only span of bytes and a <see cref="T:System.Boolean" /> value (as a JSON literal true or false) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON literal true or false as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteBoolean(ReadOnlySpan<byte> utf8PropertyName, bool value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      ReadOnlySpan<byte> readOnlySpan = value ? JsonConstants.TrueValue : JsonConstants.FalseValue;
      this.WriteLiteralEscape(utf8PropertyName, readOnlySpan);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = value ? JsonTokenType.True : JsonTokenType.False;
    }


    #nullable disable
    private void WriteLiteralEscape(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteLiteralEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteLiteralByOptions(propertyName, value);
    }

    private void WriteLiteralEscape(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteLiteralEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteLiteralByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteLiteralEscapeProperty(
      ReadOnlySpan<char> propertyName,
      ReadOnlySpan<byte> value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteLiteralByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteLiteralEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteLiteralByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteLiteralByOptions(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteLiteralIndented(propertyName, value);
      else
        this.WriteLiteralMinimized(propertyName, value);
    }

    private void WriteLiteralByOptions(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteLiteralIndented(utf8PropertyName, value);
      else
        this.WriteLiteralMinimized(utf8PropertyName, value);
    }

    private void WriteLiteralMinimized(
      ReadOnlySpan<char> escapedPropertyName,
      ReadOnlySpan<byte> value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + value.Length + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += value.Length;
    }

    private void WriteLiteralMinimized(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<byte> value)
    {
      int requiredSize = escapedPropertyName.Length + value.Length + 3 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += value.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteLiteralSection(
      ReadOnlySpan<byte> escapedPropertyNameSection,
      ReadOnlySpan<byte> value)
    {
      int requiredSize = escapedPropertyNameSection.Length + value.Length + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      escapedPropertyNameSection.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyNameSection.Length;
      value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += value.Length;
    }

    private void WriteLiteralIndented(
      ReadOnlySpan<char> escapedPropertyName,
      ReadOnlySpan<byte> value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + value.Length + 5 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += value.Length;
    }

    private void WriteLiteralIndented(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<byte> value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + value.Length + 4 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += value.Length;
    }

    internal unsafe void WritePropertyName(bool value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(5)), 5);
      int bytesWritten;
      Utf8Formatter.TryFormat(value, destination, out bytesWritten);
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.Int64" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteNumber(JsonEncodedText propertyName, long value)
    {
      this.WriteNumberByOptions(propertyName.EncodedUtf8Bytes, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and an <see cref="T:System.Int64" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteNumber(string propertyName, long value) => this.WriteNumber((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and an <see cref="T:System.Int64" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<char> propertyName, long value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      this.WriteNumberEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    /// <summary>Writes a property name specified as a read-only span of bytes and an <see cref="T:System.Int64" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, long value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      this.WriteNumberEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.Int32" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteNumber(JsonEncodedText propertyName, int value) => this.WriteNumber(propertyName, (long) value);

    /// <summary>Writes a property name specified as a string and an <see cref="T:System.Int32" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteNumber(string propertyName, int value) => this.WriteNumber((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), (long) value);

    /// <summary>Writes a property name specified as a read-only character span and an <see cref="T:System.Int32" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<char> propertyName, int value) => this.WriteNumber(propertyName, (long) value);

    /// <summary>Writes a property name specified as a read-only span of bytes and an <see cref="T:System.Int32" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, int value) => this.WriteNumber(utf8PropertyName, (long) value);


    #nullable disable
    private void WriteNumberEscape(ReadOnlySpan<char> propertyName, long value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(propertyName, value);
    }

    private void WriteNumberEscape(ReadOnlySpan<byte> utf8PropertyName, long value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<char> propertyName,
      long value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      long value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteNumberByOptions(ReadOnlySpan<char> propertyName, long value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(propertyName, value);
      else
        this.WriteNumberMinimized(propertyName, value);
    }

    private void WriteNumberByOptions(ReadOnlySpan<byte> utf8PropertyName, long value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(utf8PropertyName, value);
      else
        this.WriteNumberMinimized(utf8PropertyName, value);
    }

    private void WriteNumberMinimized(ReadOnlySpan<char> escapedPropertyName, long value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 20 + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberMinimized(ReadOnlySpan<byte> escapedPropertyName, long value)
    {
      int requiredSize = escapedPropertyName.Length + 20 + 3 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<char> escapedPropertyName, long value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 20 + 5 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<byte> escapedPropertyName, long value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 20 + 4 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    internal void WritePropertyName(int value) => this.WritePropertyName((long) value);

    internal unsafe void WritePropertyName(long value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(20)), 20);
      int bytesWritten;
      Utf8Formatter.TryFormat(value, destination, out bytesWritten, new StandardFormat());
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    /// <summary>Writes the pre-encoded property name (as a JSON string) as the first part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this write operation would produce invalid JSON.</exception>
    public void WritePropertyName(JsonEncodedText propertyName) => this.WritePropertyNameHelper(propertyName.EncodedUtf8Bytes);

    internal void WritePropertyNameSection(ReadOnlySpan<byte> escapedPropertyNameSection)
    {
      if (this._options.Indented)
      {
        this.WritePropertyNameHelper(escapedPropertyNameSection.Slice(1, escapedPropertyNameSection.Length - 3));
      }
      else
      {
        this.WriteStringPropertyNameSection(escapedPropertyNameSection);
        this._currentDepth &= int.MaxValue;
        this._tokenType = JsonTokenType.PropertyName;
      }
    }

    private void WritePropertyNameHelper(ReadOnlySpan<byte> utf8PropertyName)
    {
      this.WriteStringByOptionsPropertyName(utf8PropertyName);
      this._currentDepth &= int.MaxValue;
      this._tokenType = JsonTokenType.PropertyName;
    }


    #nullable enable
    /// <summary>Writes the property name (as a JSON string) as the first part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyName" /> is <see langword="null" />.</exception>
    public void WritePropertyName(string propertyName) => this.WritePropertyName((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan());

    /// <summary>Writes the property name (as a JSON string) as the first part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this write operation would produce invalid JSON.</exception>
    public void WritePropertyName(ReadOnlySpan<char> propertyName)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStringEscapeProperty(propertyName, firstEscapeIndexProp);
      else
        this.WriteStringByOptionsPropertyName(propertyName);
      this._currentDepth &= int.MaxValue;
      this._tokenType = JsonTokenType.PropertyName;
    }


    #nullable disable
    private unsafe void WriteStringEscapeProperty(
      ReadOnlySpan<char> propertyName,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      if (firstEscapeIndexProp != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
        Span<char> destination;
        if (maxEscapedLength > 128)
        {
          array = ArrayPool<char>.Shared.Rent(maxEscapedLength);
          destination = (Span<char>) array;
        }
        else
        {
          char* pointer = stackalloc char[128];
          destination = new Span<char>((void*) pointer, 128);
        }
        int written;
        JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
        propertyName = (ReadOnlySpan<char>) destination.Slice(0, written);
      }
      this.WriteStringByOptionsPropertyName(propertyName);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private void WriteStringByOptionsPropertyName(ReadOnlySpan<char> propertyName)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndentedPropertyName(propertyName);
      else
        this.WriteStringMinimizedPropertyName(propertyName);
    }

    private void WriteStringMinimizedPropertyName(ReadOnlySpan<char> escapedPropertyName)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
    }

    private void WriteStringIndentedPropertyName(ReadOnlySpan<char> escapedPropertyName)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 5 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
    }


    #nullable enable
    /// <summary>Writes the UTF-8 property name (as a JSON string) as the first part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this write operation would produce invalid JSON.</exception>
    public void WritePropertyName(ReadOnlySpan<byte> utf8PropertyName)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteStringEscapeProperty(utf8PropertyName, firstEscapeIndexProp);
      else
        this.WriteStringByOptionsPropertyName(utf8PropertyName);
      this._currentDepth &= int.MaxValue;
      this._tokenType = JsonTokenType.PropertyName;
    }


    #nullable disable
    private void WritePropertyNameUnescaped(ReadOnlySpan<byte> utf8PropertyName)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      this.WriteStringByOptionsPropertyName(utf8PropertyName);
      this._currentDepth &= int.MaxValue;
      this._tokenType = JsonTokenType.PropertyName;
    }

    private unsafe void WriteStringEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      if (firstEscapeIndexProp != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
        Span<byte> destination;
        if (maxEscapedLength > 256)
        {
          array = ArrayPool<byte>.Shared.Rent(maxEscapedLength);
          destination = (Span<byte>) array;
        }
        else
        {
          byte* pointer = stackalloc byte[256];
          destination = new Span<byte>((void*) pointer, 256);
        }
        int written;
        JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
        utf8PropertyName = (ReadOnlySpan<byte>) destination.Slice(0, written);
      }
      this.WriteStringByOptionsPropertyName(utf8PropertyName);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteStringByOptionsPropertyName(ReadOnlySpan<byte> utf8PropertyName)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndentedPropertyName(utf8PropertyName);
      else
        this.WriteStringMinimizedPropertyName(utf8PropertyName);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteStringMinimizedPropertyName(ReadOnlySpan<byte> escapedPropertyName)
    {
      int requiredSize = escapedPropertyName.Length + 3 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteStringPropertyNameSection(ReadOnlySpan<byte> escapedPropertyNameSection)
    {
      int requiredSize = escapedPropertyNameSection.Length + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      escapedPropertyNameSection.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyNameSection.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteStringIndentedPropertyName(ReadOnlySpan<byte> escapedPropertyName)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 4 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
    }

    /// <summary>Writes the pre-encoded property name and pre-encoded value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The JSON encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(JsonEncodedText propertyName, JsonEncodedText value) => this.WriteStringHelper(propertyName.EncodedUtf8Bytes, value.EncodedUtf8Bytes);

    private void WriteStringHelper(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> utf8Value)
    {
      this.WriteStringByOptions(utf8PropertyName, utf8Value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes the property name and pre-encoded value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The JSON encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteString(string propertyName, JsonEncodedText value) => this.WriteString((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a string and a string text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteString(string propertyName, string? value)
    {
      if (propertyName == null)
        throw new ArgumentNullException(nameof (propertyName));
      if (value == null)
        this.WriteNull(propertyName.AsSpan());
      else
        this.WriteString(propertyName.AsSpan(), value.AsSpan());
    }

    /// <summary>Writes a UTF-16 property name and UTF-16 text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
    {
      JsonWriterHelper.ValidatePropertyAndValue(propertyName, value);
      this.WriteStringEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes a UTF-8 property name and UTF-8 text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="utf8Value">The UTF-8 encoded value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<byte> utf8Value)
    {
      JsonWriterHelper.ValidatePropertyAndValue(utf8PropertyName, utf8Value);
      this.WriteStringEscape(utf8PropertyName, utf8Value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes the pre-encoded property name and string text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(JsonEncodedText propertyName, string? value)
    {
      if (value == null)
        this.WriteNull(propertyName);
      else
        this.WriteString(propertyName, value.AsSpan());
    }

    /// <summary>Writes the pre-encoded property name and text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(JsonEncodedText propertyName, ReadOnlySpan<char> value) => this.WriteStringHelperEscapeValue(propertyName.EncodedUtf8Bytes, value);


    #nullable disable
    private void WriteStringHelperEscapeValue(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<char> value)
    {
      JsonWriterHelper.ValidateValue(value);
      int firstEscapeIndex = JsonWriterHelper.NeedsEscaping(value, this._options.Encoder);
      if (firstEscapeIndex != -1)
        this.WriteStringEscapeValueOnly(utf8PropertyName, value, firstEscapeIndex);
      else
        this.WriteStringByOptions(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a UTF-16 text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteString(string propertyName, ReadOnlySpan<char> value) => this.WriteString((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a UTF-8 property name and UTF-16 text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<char> value)
    {
      JsonWriterHelper.ValidatePropertyAndValue(utf8PropertyName, value);
      this.WriteStringEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes the pre-encoded property name and UTF-8 text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="utf8Value">The UTF-8 encoded value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(JsonEncodedText propertyName, ReadOnlySpan<byte> utf8Value) => this.WriteStringHelperEscapeValue(propertyName.EncodedUtf8Bytes, utf8Value);


    #nullable disable
    private void WriteStringHelperEscapeValue(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> utf8Value)
    {
      JsonWriterHelper.ValidateValue(utf8Value);
      int firstEscapeIndex = JsonWriterHelper.NeedsEscaping(utf8Value, this._options.Encoder);
      if (firstEscapeIndex != -1)
        this.WriteStringEscapeValueOnly(utf8PropertyName, utf8Value, firstEscapeIndex);
      else
        this.WriteStringByOptions(utf8PropertyName, utf8Value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a UTF-8 text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="utf8Value">The UTF-8 encoded value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    public void WriteString(string propertyName, ReadOnlySpan<byte> utf8Value) => this.WriteString((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), utf8Value);

    /// <summary>Writes a UTF-16 property name and UTF-8 text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="utf8Value">The UTF-8 encoded value to be written as a JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> utf8Value)
    {
      JsonWriterHelper.ValidatePropertyAndValue(propertyName, utf8Value);
      this.WriteStringEscape(propertyName, utf8Value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes the property name and pre-encoded value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The JSON encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<char> propertyName, JsonEncodedText value) => this.WriteStringHelperEscapeProperty(propertyName, value.EncodedUtf8Bytes);


    #nullable disable
    private void WriteStringHelperEscapeProperty(
      ReadOnlySpan<char> propertyName,
      ReadOnlySpan<byte> utf8Value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      int firstEscapeIndex = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndex != -1)
        this.WriteStringEscapePropertyOnly(propertyName, utf8Value, firstEscapeIndex);
      else
        this.WriteStringByOptions(propertyName, utf8Value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes a UTF-16 property name and string text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<char> propertyName, string? value)
    {
      if (value == null)
        this.WriteNull(propertyName);
      else
        this.WriteString(propertyName, value.AsSpan());
    }

    /// <summary>Writes the UTF-8 property name and pre-encoded value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The JSON encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<byte> utf8PropertyName, JsonEncodedText value) => this.WriteStringHelperEscapeProperty(utf8PropertyName, value.EncodedUtf8Bytes);


    #nullable disable
    private void WriteStringHelperEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> utf8Value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      int firstEscapeIndex = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndex != -1)
        this.WriteStringEscapePropertyOnly(utf8PropertyName, utf8Value, firstEscapeIndex);
      else
        this.WriteStringByOptions(utf8PropertyName, utf8Value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes a UTF-8 property name and string text value (as a JSON string) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON string as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name or value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteString(ReadOnlySpan<byte> utf8PropertyName, string? value)
    {
      if (value == null)
        this.WriteNull(utf8PropertyName);
      else
        this.WriteString(utf8PropertyName, value.AsSpan());
    }


    #nullable disable
    private unsafe void WriteStringEscapeValueOnly(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<byte> utf8Value,
      int firstEscapeIndex)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8Value.Length, firstEscapeIndex);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8Value, destination, firstEscapeIndex, this._options.Encoder, out written);
      this.WriteStringByOptions(escapedPropertyName, (ReadOnlySpan<byte>) destination.Slice(0, written));
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private unsafe void WriteStringEscapeValueOnly(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<char> value,
      int firstEscapeIndex)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(value.Length, firstEscapeIndex);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(value, destination, firstEscapeIndex, this._options.Encoder, out written);
      this.WriteStringByOptions(escapedPropertyName, (ReadOnlySpan<char>) destination.Slice(0, written));
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteStringEscapePropertyOnly(
      ReadOnlySpan<char> propertyName,
      ReadOnlySpan<byte> escapedValue,
      int firstEscapeIndex)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndex);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndex, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<char>) destination.Slice(0, written), escapedValue);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteStringEscapePropertyOnly(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> escapedValue,
      int firstEscapeIndex)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndex);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndex, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), escapedValue);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteStringEscape(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
    {
      int firstEscapeIndexVal = JsonWriterHelper.NeedsEscaping(value, this._options.Encoder);
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexVal + firstEscapeIndexProp != -2)
        this.WriteStringEscapePropertyOrValue(propertyName, value, firstEscapeIndexProp, firstEscapeIndexVal);
      else
        this.WriteStringByOptions(propertyName, value);
    }

    private void WriteStringEscape(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> utf8Value)
    {
      int firstEscapeIndexVal = JsonWriterHelper.NeedsEscaping(utf8Value, this._options.Encoder);
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexVal + firstEscapeIndexProp != -2)
        this.WriteStringEscapePropertyOrValue(utf8PropertyName, utf8Value, firstEscapeIndexProp, firstEscapeIndexVal);
      else
        this.WriteStringByOptions(utf8PropertyName, utf8Value);
    }

    private void WriteStringEscape(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> utf8Value)
    {
      int firstEscapeIndexVal = JsonWriterHelper.NeedsEscaping(utf8Value, this._options.Encoder);
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexVal + firstEscapeIndexProp != -2)
        this.WriteStringEscapePropertyOrValue(propertyName, utf8Value, firstEscapeIndexProp, firstEscapeIndexVal);
      else
        this.WriteStringByOptions(propertyName, utf8Value);
    }

    private void WriteStringEscape(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<char> value)
    {
      int firstEscapeIndexVal = JsonWriterHelper.NeedsEscaping(value, this._options.Encoder);
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexVal + firstEscapeIndexProp != -2)
        this.WriteStringEscapePropertyOrValue(utf8PropertyName, value, firstEscapeIndexProp, firstEscapeIndexVal);
      else
        this.WriteStringByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteStringEscapePropertyOrValue(
      ReadOnlySpan<char> propertyName,
      ReadOnlySpan<char> value,
      int firstEscapeIndexProp,
      int firstEscapeIndexVal)
    {
      char[] array1 = (char[]) null;
      char[] array2 = (char[]) null;
      if (firstEscapeIndexVal != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(value.Length, firstEscapeIndexVal);
        Span<char> destination;
        if (maxEscapedLength > 128)
        {
          array1 = ArrayPool<char>.Shared.Rent(maxEscapedLength);
          destination = (Span<char>) array1;
        }
        else
        {
          char* pointer = stackalloc char[128];
          destination = new Span<char>((void*) pointer, 128);
        }
        int written;
        JsonWriterHelper.EscapeString(value, destination, firstEscapeIndexVal, this._options.Encoder, out written);
        value = (ReadOnlySpan<char>) destination.Slice(0, written);
      }
      if (firstEscapeIndexProp != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
        Span<char> destination;
        if (maxEscapedLength > 128)
        {
          array2 = ArrayPool<char>.Shared.Rent(maxEscapedLength);
          destination = (Span<char>) array2;
        }
        else
        {
          char* pointer = stackalloc char[128];
          destination = new Span<char>((void*) pointer, 128);
        }
        int written;
        JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
        propertyName = (ReadOnlySpan<char>) destination.Slice(0, written);
      }
      this.WriteStringByOptions(propertyName, value);
      if (array1 != null)
        ArrayPool<char>.Shared.Return(array1);
      if (array2 == null)
        return;
      ArrayPool<char>.Shared.Return(array2);
    }

    private unsafe void WriteStringEscapePropertyOrValue(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> utf8Value,
      int firstEscapeIndexProp,
      int firstEscapeIndexVal)
    {
      byte[] array1 = (byte[]) null;
      byte[] array2 = (byte[]) null;
      if (firstEscapeIndexVal != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8Value.Length, firstEscapeIndexVal);
        Span<byte> destination;
        if (maxEscapedLength > 256)
        {
          array1 = ArrayPool<byte>.Shared.Rent(maxEscapedLength);
          destination = (Span<byte>) array1;
        }
        else
        {
          byte* pointer = stackalloc byte[256];
          destination = new Span<byte>((void*) pointer, 256);
        }
        int written;
        JsonWriterHelper.EscapeString(utf8Value, destination, firstEscapeIndexVal, this._options.Encoder, out written);
        utf8Value = (ReadOnlySpan<byte>) destination.Slice(0, written);
      }
      if (firstEscapeIndexProp != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
        Span<byte> destination;
        if (maxEscapedLength > 256)
        {
          array2 = ArrayPool<byte>.Shared.Rent(maxEscapedLength);
          destination = (Span<byte>) array2;
        }
        else
        {
          byte* pointer = stackalloc byte[256];
          destination = new Span<byte>((void*) pointer, 256);
        }
        int written;
        JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
        utf8PropertyName = (ReadOnlySpan<byte>) destination.Slice(0, written);
      }
      this.WriteStringByOptions(utf8PropertyName, utf8Value);
      if (array1 != null)
        ArrayPool<byte>.Shared.Return(array1);
      if (array2 == null)
        return;
      ArrayPool<byte>.Shared.Return(array2);
    }

    private unsafe void WriteStringEscapePropertyOrValue(
      ReadOnlySpan<char> propertyName,
      ReadOnlySpan<byte> utf8Value,
      int firstEscapeIndexProp,
      int firstEscapeIndexVal)
    {
      byte[] array1 = (byte[]) null;
      char[] array2 = (char[]) null;
      if (firstEscapeIndexVal != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8Value.Length, firstEscapeIndexVal);
        Span<byte> destination;
        if (maxEscapedLength > 256)
        {
          array1 = ArrayPool<byte>.Shared.Rent(maxEscapedLength);
          destination = (Span<byte>) array1;
        }
        else
        {
          byte* pointer = stackalloc byte[256];
          destination = new Span<byte>((void*) pointer, 256);
        }
        int written;
        JsonWriterHelper.EscapeString(utf8Value, destination, firstEscapeIndexVal, this._options.Encoder, out written);
        utf8Value = (ReadOnlySpan<byte>) destination.Slice(0, written);
      }
      if (firstEscapeIndexProp != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
        Span<char> destination;
        if (maxEscapedLength > 128)
        {
          array2 = ArrayPool<char>.Shared.Rent(maxEscapedLength);
          destination = (Span<char>) array2;
        }
        else
        {
          char* pointer = stackalloc char[128];
          destination = new Span<char>((void*) pointer, 128);
        }
        int written;
        JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
        propertyName = (ReadOnlySpan<char>) destination.Slice(0, written);
      }
      this.WriteStringByOptions(propertyName, utf8Value);
      if (array1 != null)
        ArrayPool<byte>.Shared.Return(array1);
      if (array2 == null)
        return;
      ArrayPool<char>.Shared.Return(array2);
    }

    private unsafe void WriteStringEscapePropertyOrValue(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<char> value,
      int firstEscapeIndexProp,
      int firstEscapeIndexVal)
    {
      char[] array1 = (char[]) null;
      byte[] array2 = (byte[]) null;
      if (firstEscapeIndexVal != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(value.Length, firstEscapeIndexVal);
        Span<char> destination;
        if (maxEscapedLength > 128)
        {
          array1 = ArrayPool<char>.Shared.Rent(maxEscapedLength);
          destination = (Span<char>) array1;
        }
        else
        {
          char* pointer = stackalloc char[128];
          destination = new Span<char>((void*) pointer, 128);
        }
        int written;
        JsonWriterHelper.EscapeString(value, destination, firstEscapeIndexVal, this._options.Encoder, out written);
        value = (ReadOnlySpan<char>) destination.Slice(0, written);
      }
      if (firstEscapeIndexProp != -1)
      {
        int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
        Span<byte> destination;
        if (maxEscapedLength > 256)
        {
          array2 = ArrayPool<byte>.Shared.Rent(maxEscapedLength);
          destination = (Span<byte>) array2;
        }
        else
        {
          byte* pointer = stackalloc byte[256];
          destination = new Span<byte>((void*) pointer, 256);
        }
        int written;
        JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
        utf8PropertyName = (ReadOnlySpan<byte>) destination.Slice(0, written);
      }
      this.WriteStringByOptions(utf8PropertyName, value);
      if (array1 != null)
        ArrayPool<char>.Shared.Return(array1);
      if (array2 == null)
        return;
      ArrayPool<byte>.Shared.Return(array2);
    }

    private void WriteStringByOptions(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(propertyName, value);
      else
        this.WriteStringMinimized(propertyName, value);
    }

    private void WriteStringByOptions(
      ReadOnlySpan<byte> utf8PropertyName,
      ReadOnlySpan<byte> utf8Value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(utf8PropertyName, utf8Value);
      else
        this.WriteStringMinimized(utf8PropertyName, utf8Value);
    }

    private void WriteStringByOptions(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> utf8Value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(propertyName, utf8Value);
      else
        this.WriteStringMinimized(propertyName, utf8Value);
    }

    private void WriteStringByOptions(ReadOnlySpan<byte> utf8PropertyName, ReadOnlySpan<char> value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteStringIndented(utf8PropertyName, value);
      else
        this.WriteStringMinimized(utf8PropertyName, value);
    }

    private void WriteStringMinimized(
      ReadOnlySpan<char> escapedPropertyName,
      ReadOnlySpan<char> escapedValue)
    {
      int requiredSize = (escapedPropertyName.Length + escapedValue.Length) * 3 + 6;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedValue, span);
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringMinimized(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<byte> escapedValue)
    {
      int requiredSize = escapedPropertyName.Length + escapedValue.Length + 5 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      escapedValue.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedValue.Length;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringMinimized(
      ReadOnlySpan<char> escapedPropertyName,
      ReadOnlySpan<byte> escapedValue)
    {
      int requiredSize = escapedPropertyName.Length * 3 + escapedValue.Length + 6;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      escapedValue.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedValue.Length;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringMinimized(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<char> escapedValue)
    {
      int requiredSize = escapedValue.Length * 3 + escapedPropertyName.Length + 6;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedValue, span);
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(
      ReadOnlySpan<char> escapedPropertyName,
      ReadOnlySpan<char> escapedValue)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + (escapedPropertyName.Length + escapedValue.Length) * 3 + 7 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedValue, span);
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<byte> escapedValue)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + escapedValue.Length + 6 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      escapedValue.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedValue.Length;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(
      ReadOnlySpan<char> escapedPropertyName,
      ReadOnlySpan<byte> escapedValue)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + escapedValue.Length + 7 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      escapedValue.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedValue.Length;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(
      ReadOnlySpan<byte> escapedPropertyName,
      ReadOnlySpan<char> escapedValue)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedValue.Length * 3 + escapedPropertyName.Length + 7 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedValue, span);
      span[this.BytesPending++] = (byte) 34;
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.UInt64" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    [CLSCompliant(false)]
    public void WriteNumber(JsonEncodedText propertyName, ulong value)
    {
      this.WriteNumberByOptions(propertyName.EncodedUtf8Bytes, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }


    #nullable enable
    /// <summary>Writes a property name specified as a string and a <see cref="T:System.UInt64" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    [CLSCompliant(false)]
    public void WriteNumber(string propertyName, ulong value) => this.WriteNumber((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.UInt64" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    [CLSCompliant(false)]
    public void WriteNumber(ReadOnlySpan<char> propertyName, ulong value)
    {
      JsonWriterHelper.ValidateProperty(propertyName);
      this.WriteNumberEscape(propertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    /// <summary>Writes a property name specified as a read-only span of bytes and a <see cref="T:System.UInt64" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    [CLSCompliant(false)]
    public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, ulong value)
    {
      JsonWriterHelper.ValidateProperty(utf8PropertyName);
      this.WriteNumberEscape(utf8PropertyName, value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    /// <summary>Writes the pre-encoded property name and <see cref="T:System.UInt32" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The JSON encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    [CLSCompliant(false)]
    public void WriteNumber(JsonEncodedText propertyName, uint value) => this.WriteNumber(propertyName, (ulong) value);

    /// <summary>Writes a property name specified as a string and a <see cref="T:System.UInt32" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="propertyName" /> parameter is <see langword="null" />.</exception>
    [CLSCompliant(false)]
    public void WriteNumber(string propertyName, uint value) => this.WriteNumber((propertyName ?? throw new ArgumentNullException(nameof (propertyName))).AsSpan(), (ulong) value);

    /// <summary>Writes a property name specified as a read-only character span and a <see cref="T:System.UInt32" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="propertyName">The UTF-16 encoded property name of the JSON object to be transcoded and written as UTF-8.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    [CLSCompliant(false)]
    public void WriteNumber(ReadOnlySpan<char> propertyName, uint value) => this.WriteNumber(propertyName, (ulong) value);

    /// <summary>Writes a property name specified as a read-only span of bytes and a <see cref="T:System.UInt32" /> value (as a JSON number) as part of a name/value pair of a JSON object.</summary>
    /// <param name="utf8PropertyName">The UTF-8 encoded property name of the JSON object to be written.</param>
    /// <param name="value">The value to be written as a JSON number as part of the name/value pair.</param>
    /// <exception cref="T:System.ArgumentException">The specified property name is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    [CLSCompliant(false)]
    public void WriteNumber(ReadOnlySpan<byte> utf8PropertyName, uint value) => this.WriteNumber(utf8PropertyName, (ulong) value);


    #nullable disable
    private void WriteNumberEscape(ReadOnlySpan<char> propertyName, ulong value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(propertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(propertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(propertyName, value);
    }

    private void WriteNumberEscape(ReadOnlySpan<byte> utf8PropertyName, ulong value)
    {
      int firstEscapeIndexProp = JsonWriterHelper.NeedsEscaping(utf8PropertyName, this._options.Encoder);
      if (firstEscapeIndexProp != -1)
        this.WriteNumberEscapeProperty(utf8PropertyName, value, firstEscapeIndexProp);
      else
        this.WriteNumberByOptions(utf8PropertyName, value);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<char> propertyName,
      ulong value,
      int firstEscapeIndexProp)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(propertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(propertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<char>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }

    private unsafe void WriteNumberEscapeProperty(
      ReadOnlySpan<byte> utf8PropertyName,
      ulong value,
      int firstEscapeIndexProp)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8PropertyName.Length, firstEscapeIndexProp);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8PropertyName, destination, firstEscapeIndexProp, this._options.Encoder, out written);
      this.WriteNumberByOptions((ReadOnlySpan<byte>) destination.Slice(0, written), value);
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    private void WriteNumberByOptions(ReadOnlySpan<char> propertyName, ulong value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(propertyName, value);
      else
        this.WriteNumberMinimized(propertyName, value);
    }

    private void WriteNumberByOptions(ReadOnlySpan<byte> utf8PropertyName, ulong value)
    {
      this.ValidateWritingProperty();
      if (this._options.Indented)
        this.WriteNumberIndented(utf8PropertyName, value);
      else
        this.WriteNumberMinimized(utf8PropertyName, value);
    }

    private void WriteNumberMinimized(ReadOnlySpan<char> escapedPropertyName, ulong value)
    {
      int requiredSize = escapedPropertyName.Length * 3 + 20 + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberMinimized(ReadOnlySpan<byte> escapedPropertyName, ulong value)
    {
      int requiredSize = escapedPropertyName.Length + 20 + 3 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<char> escapedPropertyName, ulong value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length * 3 + 20 + 5 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedPropertyName, span);
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberIndented(ReadOnlySpan<byte> escapedPropertyName, ulong value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedPropertyName.Length + 20 + 4 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 34;
      escapedPropertyName.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedPropertyName.Length;
      span[this.BytesPending++] = (byte) 34;
      span[this.BytesPending++] = (byte) 58;
      span[this.BytesPending++] = (byte) 32;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    internal void WritePropertyName(uint value) => this.WritePropertyName((ulong) value);

    internal unsafe void WritePropertyName(ulong value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(20)), 20);
      int bytesWritten;
      Utf8Formatter.TryFormat(value, destination, out bytesWritten, new StandardFormat());
      this.WritePropertyNameUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }


    #nullable enable
    /// <summary>Writes the raw bytes value as a Base64 encoded JSON string as an element of a JSON array.</summary>
    /// <param name="bytes">The binary data to be written as a Base64 encoded JSON string element of a JSON array.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and this method would result in writing invalid JSON.</exception>
    public void WriteBase64StringValue(ReadOnlySpan<byte> bytes)
    {
      JsonWriterHelper.ValidateBytes(bytes);
      this.WriteBase64ByOptions(bytes);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable disable
    private void WriteBase64ByOptions(ReadOnlySpan<byte> bytes)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteBase64Indented(bytes);
      else
        this.WriteBase64Minimized(bytes);
    }

    private void WriteBase64Minimized(ReadOnlySpan<byte> bytes)
    {
      int encodedToUtf8Length = Base64.GetMaxEncodedToUtf8Length(bytes.Length);
      int requiredSize = encodedToUtf8Length + 3;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.Base64EncodeAndWrite(bytes, span, encodedToUtf8Length);
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteBase64Indented(ReadOnlySpan<byte> bytes)
    {
      int indentation = this.Indentation;
      int encodedToUtf8Length = Base64.GetMaxEncodedToUtf8Length(bytes.Length);
      int requiredSize = indentation + encodedToUtf8Length + 3 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      span[this.BytesPending++] = (byte) 34;
      this.Base64EncodeAndWrite(bytes, span, encodedToUtf8Length);
      span[this.BytesPending++] = (byte) 34;
    }


    #nullable enable
    private static unsafe ReadOnlySpan<byte> SingleLineCommentDelimiterUtf8 => new ReadOnlySpan<byte>((void*) &\u003CPrivateImplementationDetails\u003E.ED9623369FF082ABE9B1DF1FB3A161857BAB3BFCCDA2886B31D526248A676201, 2);

    /// <summary>Writes a string text value as a JSON comment.</summary>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON comment within <c>/*..*/</c>.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> contains a comment delimiter (that is, <c>*/</c>).</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />.</exception>
    public void WriteCommentValue(string value) => this.WriteCommentValue((value ?? throw new ArgumentNullException(nameof (value))).AsSpan());

    /// <summary>Writes a UTF-16 text value as a JSON comment.</summary>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON comment within <c>/*..*/</c>.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> contains a comment delimiter (that is, <c>*/</c>).</exception>
    public void WriteCommentValue(ReadOnlySpan<char> value)
    {
      JsonWriterHelper.ValidateValue(value);
      if (value.IndexOf<char>((ReadOnlySpan<char>) Utf8JsonWriter.s_singleLineCommentDelimiter) != -1)
        ThrowHelper.ThrowArgumentException_InvalidCommentValue();
      this.WriteCommentByOptions(value);
    }


    #nullable disable
    private void WriteCommentByOptions(ReadOnlySpan<char> value)
    {
      if (this._options.Indented)
        this.WriteCommentIndented(value);
      else
        this.WriteCommentMinimized(value);
    }

    private void WriteCommentMinimized(ReadOnlySpan<char> value)
    {
      int requiredSize = value.Length * 3 + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      span[this.BytesPending++] = (byte) 47;
      ref Span<byte> local = ref span;
      int bytesConsumed = this.BytesPending++;
      int index = bytesConsumed;
      local[index] = (byte) 42;
      int bytesWritten;
      JsonWriterHelper.ToUtf8(MemoryMarshal.AsBytes<char>(value), span.Slice(this.BytesPending), out bytesConsumed, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 42;
      span[this.BytesPending++] = (byte) 47;
    }

    private void WriteCommentIndented(ReadOnlySpan<char> value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + value.Length * 3 + 4 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._tokenType != JsonTokenType.None)
        this.WriteNewLine(span);
      JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
      this.BytesPending += indentation;
      span[this.BytesPending++] = (byte) 47;
      ref Span<byte> local1 = ref span;
      int bytesConsumed = this.BytesPending++;
      int index1 = bytesConsumed;
      local1[index1] = (byte) 42;
      int bytesWritten;
      JsonWriterHelper.ToUtf8(MemoryMarshal.AsBytes<char>(value), span.Slice(this.BytesPending), out bytesConsumed, out bytesWritten);
      this.BytesPending += bytesWritten;
      ref Span<byte> local2 = ref span;
      bytesConsumed = this.BytesPending++;
      int index2 = bytesConsumed;
      local2[index2] = (byte) 42;
      ref Span<byte> local3 = ref span;
      bytesConsumed = this.BytesPending++;
      int index3 = bytesConsumed;
      local3[index3] = (byte) 47;
    }


    #nullable enable
    /// <summary>Writes a UTF-8 text value as a JSON comment.</summary>
    /// <param name="utf8Value">The UTF-8 encoded value to be written as a JSON comment within <c>/*..*/</c>.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.
    /// 
    /// -or-
    /// 
    /// <paramref name="utf8Value" /> contains a comment delimiter (that is, <c>*/</c>).</exception>
    public void WriteCommentValue(ReadOnlySpan<byte> utf8Value)
    {
      JsonWriterHelper.ValidateValue(utf8Value);
      if (utf8Value.IndexOf<byte>(Utf8JsonWriter.SingleLineCommentDelimiterUtf8) != -1)
        ThrowHelper.ThrowArgumentException_InvalidCommentValue();
      this.WriteCommentByOptions(utf8Value);
    }


    #nullable disable
    private void WriteCommentByOptions(ReadOnlySpan<byte> utf8Value)
    {
      if (this._options.Indented)
        this.WriteCommentIndented(utf8Value);
      else
        this.WriteCommentMinimized(utf8Value);
    }

    private void WriteCommentMinimized(ReadOnlySpan<byte> utf8Value)
    {
      int requiredSize = utf8Value.Length + 4;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      span[this.BytesPending++] = (byte) 47;
      span[this.BytesPending++] = (byte) 42;
      utf8Value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += utf8Value.Length;
      span[this.BytesPending++] = (byte) 42;
      span[this.BytesPending++] = (byte) 47;
    }

    private void WriteCommentIndented(ReadOnlySpan<byte> utf8Value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + utf8Value.Length + 4 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      span[this.BytesPending++] = (byte) 47;
      span[this.BytesPending++] = (byte) 42;
      utf8Value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += utf8Value.Length;
      span[this.BytesPending++] = (byte) 42;
      span[this.BytesPending++] = (byte) 47;
    }

    /// <summary>Writes a <see cref="T:System.DateTime" /> value (as a JSON string) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON string as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteStringValue(DateTime value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteStringValueIndented(value);
      else
        this.WriteStringValueMinimized(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    private void WriteStringValueMinimized(DateTime value)
    {
      int requiredSize = 36;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringValueIndented(DateTime value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 33 + 3 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    /// <summary>Writes a <see cref="T:System.DateTimeOffset" /> value (as a JSON string) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON string as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteStringValue(DateTimeOffset value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteStringValueIndented(value);
      else
        this.WriteStringValueMinimized(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    private void WriteStringValueMinimized(DateTimeOffset value)
    {
      int requiredSize = 36;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeOffsetTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringValueIndented(DateTimeOffset value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 33 + 3 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      JsonWriterHelper.WriteDateTimeOffsetTrimmed(span.Slice(this.BytesPending), value, out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    /// <summary>Writes a <see cref="T:System.Decimal" /> value (as a JSON number) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON number as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteNumberValue(Decimal value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteNumberValueIndented(value);
      else
        this.WriteNumberValueMinimized(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    private void WriteNumberValueMinimized(Decimal value)
    {
      int requiredSize = 32;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberValueIndented(Decimal value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 31 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    internal unsafe void WriteNumberValueAsString(Decimal value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(31)), 31);
      int bytesWritten;
      Utf8Formatter.TryFormat(value, destination, out bytesWritten);
      this.WriteNumberValueAsStringUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    /// <summary>Writes a <see cref="T:System.Double" /> value (as a JSON number) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON number as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteNumberValue(double value)
    {
      JsonWriterHelper.ValidateDouble(value);
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteNumberValueIndented(value);
      else
        this.WriteNumberValueMinimized(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    private void WriteNumberValueMinimized(double value)
    {
      int requiredSize = 129;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      int bytesWritten;
      Utf8JsonWriter.TryFormatDouble(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberValueIndented(double value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 128 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      int bytesWritten;
      Utf8JsonWriter.TryFormatDouble(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private static bool TryFormatDouble(double value, Span<byte> destination, out int bytesWritten) => Utf8Formatter.TryFormat(value, destination, out bytesWritten);

    internal unsafe void WriteNumberValueAsString(double value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(128)), 128);
      int bytesWritten;
      Utf8JsonWriter.TryFormatDouble(value, destination, out bytesWritten);
      this.WriteNumberValueAsStringUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    internal void WriteFloatingPointConstant(double value)
    {
      if (double.IsNaN(value))
        this.WriteNumberValueAsStringUnescaped(JsonConstants.NaNValue);
      else if (double.IsPositiveInfinity(value))
        this.WriteNumberValueAsStringUnescaped(JsonConstants.PositiveInfinityValue);
      else if (double.IsNegativeInfinity(value))
        this.WriteNumberValueAsStringUnescaped(JsonConstants.NegativeInfinityValue);
      else
        this.WriteNumberValue(value);
    }

    /// <summary>Writes a <see cref="T:System.Single" /> value (as a JSON number) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON number as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteNumberValue(float value)
    {
      JsonWriterHelper.ValidateSingle(value);
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteNumberValueIndented(value);
      else
        this.WriteNumberValueMinimized(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    private void WriteNumberValueMinimized(float value)
    {
      int requiredSize = 129;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      int bytesWritten;
      Utf8JsonWriter.TryFormatSingle(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberValueIndented(float value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 128 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      int bytesWritten;
      Utf8JsonWriter.TryFormatSingle(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
    }

    private static bool TryFormatSingle(float value, Span<byte> destination, out int bytesWritten) => Utf8Formatter.TryFormat(value, destination, out bytesWritten, new StandardFormat());

    internal unsafe void WriteNumberValueAsString(float value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(128)), 128);
      int bytesWritten;
      Utf8JsonWriter.TryFormatSingle(value, destination, out bytesWritten);
      this.WriteNumberValueAsStringUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    internal void WriteFloatingPointConstant(float value)
    {
      if (float.IsNaN(value))
        this.WriteNumberValueAsStringUnescaped(JsonConstants.NaNValue);
      else if (float.IsPositiveInfinity(value))
        this.WriteNumberValueAsStringUnescaped(JsonConstants.PositiveInfinityValue);
      else if (float.IsNegativeInfinity(value))
        this.WriteNumberValueAsStringUnescaped(JsonConstants.NegativeInfinityValue);
      else
        this.WriteNumberValue(value);
    }

    internal void WriteNumberValue(ReadOnlySpan<byte> utf8FormattedNumber)
    {
      JsonWriterHelper.ValidateValue(utf8FormattedNumber);
      JsonWriterHelper.ValidateNumber(utf8FormattedNumber);
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteNumberValueIndented(utf8FormattedNumber);
      else
        this.WriteNumberValueMinimized(utf8FormattedNumber);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    private void WriteNumberValueMinimized(ReadOnlySpan<byte> utf8Value)
    {
      int requiredSize = utf8Value.Length + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      utf8Value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += utf8Value.Length;
    }

    private void WriteNumberValueIndented(ReadOnlySpan<byte> utf8Value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + utf8Value.Length + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      utf8Value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += utf8Value.Length;
    }

    /// <summary>Writes a <see cref="T:System.Guid" /> value (as a JSON string) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON string as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteStringValue(Guid value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteStringValueIndented(value);
      else
        this.WriteStringValueMinimized(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    private void WriteStringValueMinimized(Guid value)
    {
      int requiredSize = 39;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringValueIndented(Guid value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 36 + 3 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      span[this.BytesPending++] = (byte) 34;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten);
      this.BytesPending += bytesWritten;
      span[this.BytesPending++] = (byte) 34;
    }

    private void ValidateWritingValue()
    {
      if (this._inObject)
      {
        if (this._tokenType == JsonTokenType.PropertyName)
          return;
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CannotWriteValueWithinObject, 0, (byte) 0, this._tokenType);
      }
      else
      {
        if (this.CurrentDepth != 0 || this._tokenType == JsonTokenType.None)
          return;
        ThrowHelper.ThrowInvalidOperationException(ExceptionResource.CannotWriteValueAfterPrimitiveOrClose, 0, (byte) 0, this._tokenType);
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe void Base64EncodeAndWrite(
      ReadOnlySpan<byte> bytes,
      Span<byte> output,
      int encodingLength)
    {
      byte[] array = (byte[]) null;
      // ISSUE: untyped stack allocation
      Span<byte> utf8 = encodingLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(encodingLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int bytesWritten;
      Base64.EncodeToUtf8(bytes, utf8, out int _, out bytesWritten);
      Span<byte> span = utf8.Slice(0, bytesWritten);
      Span<byte> destination = output.Slice(this.BytesPending);
      span.Slice(0, bytesWritten).CopyTo(destination);
      this.BytesPending += bytesWritten;
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    /// <summary>Writes the JSON literal null as an element of a JSON array.</summary>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteNullValue()
    {
      this.WriteLiteralByOptions(JsonConstants.NullValue);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Null;
    }

    /// <summary>Writes a <see cref="T:System.Boolean" /> value (as a JSON literal true or false) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON literal true or false as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteBooleanValue(bool value)
    {
      if (value)
      {
        this.WriteLiteralByOptions(JsonConstants.TrueValue);
        this._tokenType = JsonTokenType.True;
      }
      else
      {
        this.WriteLiteralByOptions(JsonConstants.FalseValue);
        this._tokenType = JsonTokenType.False;
      }
      this.SetFlagToAddListSeparatorBeforeNextItem();
    }

    private void WriteLiteralByOptions(ReadOnlySpan<byte> utf8Value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteLiteralIndented(utf8Value);
      else
        this.WriteLiteralMinimized(utf8Value);
    }

    private void WriteLiteralMinimized(ReadOnlySpan<byte> utf8Value)
    {
      int requiredSize = utf8Value.Length + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      utf8Value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += utf8Value.Length;
    }

    private void WriteLiteralIndented(ReadOnlySpan<byte> utf8Value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + utf8Value.Length + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      utf8Value.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += utf8Value.Length;
    }


    #nullable enable
    /// <summary>Writes the input as JSON content. It is expected that the input content is a single complete JSON value.</summary>
    /// <param name="json">The raw JSON content to write.</param>
    /// <param name="skipInputValidation">
    /// <see langword="false" /> to validate if the input is an RFC 8259-compliant JSON payload; <see langword="true" /> otherwise.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The length of the input is zero or greater than 715,827,882 (<see cref="F:System.Int32.MaxValue" /> / 3).</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="skipInputValidation" /> is <see langword="false" />, and the input is not a valid, complete, single JSON value according to the JSON RFC, or the input JSON exceeds a recursive depth of 64.</exception>
    public void WriteRawValue(string json, bool skipInputValidation = false)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (json == null)
        throw new ArgumentNullException(nameof (json));
      this.TranscodeAndWriteRawValue(json.AsSpan(), skipInputValidation);
    }

    /// <summary>Writes the input as JSON content. It is expected that the input content is a single complete JSON value.</summary>
    /// <param name="json">The raw JSON content to write.</param>
    /// <param name="skipInputValidation">
    /// <see langword="false" /> to validate if the input is an RFC 8259-compliant JSON payload; <see langword="true" /> otherwise.</param>
    /// <exception cref="T:System.ArgumentException">The length of the input is zero or greater than 715,827,882 (<see cref="F:System.Int32.MaxValue" /> / 3).</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="skipInputValidation" /> is <see langword="false" />, and the input is not a valid, complete, single JSON value according to the JSON RFC, or the input JSON exceeds a recursive depth of 64.</exception>
    public void WriteRawValue(ReadOnlySpan<char> json, bool skipInputValidation = false)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      this.TranscodeAndWriteRawValue(json, skipInputValidation);
    }

    /// <summary>Writes the input as JSON content. It is expected that the input content is a single complete JSON value.</summary>
    /// <param name="utf8Json">The raw JSON content to write.</param>
    /// <param name="skipInputValidation">
    /// <see langword="false" /> to validate if the input is an RFC 8259-compliant JSON payload; <see langword="true" /> otherwise.</param>
    /// <exception cref="T:System.ArgumentException">The length of the input is zero or equal to <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="skipInputValidation" /> is <see langword="false" />, and the input is not a valid, complete, single JSON value according to the JSON RFC, or the input JSON exceeds a recursive depth of 64.</exception>
    public void WriteRawValue(ReadOnlySpan<byte> utf8Json, bool skipInputValidation = false)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (utf8Json.Length == int.MaxValue)
        ThrowHelper.ThrowArgumentException_ValueTooLarge(int.MaxValue);
      this.WriteRawValueCore(utf8Json, skipInputValidation);
    }


    #nullable disable
    private void TranscodeAndWriteRawValue(ReadOnlySpan<char> json, bool skipInputValidation)
    {
      if (json.Length > 715827882)
        ThrowHelper.ThrowArgumentException_ValueTooLarge(json.Length);
      byte[] array = (byte[]) null;
      byte[] numArray;
      if (json.Length > 349525)
        numArray = new byte[JsonReaderHelper.GetUtf8ByteCount(json)];
      else
        array = numArray = ArrayPool<byte>.Shared.Rent(json.Length * 3);
      Span<byte> span = (Span<byte>) numArray;
      try
      {
        int utf8FromText = JsonReaderHelper.GetUtf8FromText(json, span);
        span = span.Slice(0, utf8FromText);
        this.WriteRawValueCore((ReadOnlySpan<byte>) span, skipInputValidation);
      }
      finally
      {
        if (array != null)
        {
          span.Clear();
          ArrayPool<byte>.Shared.Return(array);
        }
      }
    }

    private void WriteRawValueCore(ReadOnlySpan<byte> utf8Json, bool skipInputValidation)
    {
      int length = utf8Json.Length;
      if (length == 0)
        ThrowHelper.ThrowArgumentException(SR.ExpectedJsonTokens);
      if (skipInputValidation)
      {
        this._tokenType = JsonTokenType.String;
      }
      else
      {
        Utf8JsonReader utf8JsonReader = new Utf8JsonReader(utf8Json);
        do
          ;
        while (utf8JsonReader.Read());
        this._tokenType = utf8JsonReader.TokenType;
      }
      int requiredSize = length + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      utf8Json.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += length;
      this.SetFlagToAddListSeparatorBeforeNextItem();
    }

    /// <summary>Writes an <see cref="T:System.Int32" /> value (as a JSON number) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON number as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteNumberValue(int value) => this.WriteNumberValue((long) value);

    /// <summary>Writes an <see cref="T:System.Int64" /> value (as a JSON number) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON number as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    public void WriteNumberValue(long value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteNumberValueIndented(value);
      else
        this.WriteNumberValueMinimized(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    private void WriteNumberValueMinimized(long value)
    {
      int requiredSize = 21;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberValueIndented(long value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 20 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    internal unsafe void WriteNumberValueAsString(long value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(20)), 20);
      int bytesWritten;
      Utf8Formatter.TryFormat(value, destination, out bytesWritten, new StandardFormat());
      this.WriteNumberValueAsStringUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }

    /// <summary>Writes the pre-encoded text value (as a JSON string) as an element of a JSON array.</summary>
    /// <param name="value">The JSON encoded value to be written as a UTF-8 transcoded JSON string element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteStringValue(JsonEncodedText value)
    {
      this.WriteStringByOptions(value.EncodedUtf8Bytes);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable enable
    /// <summary>Writes a string text value (as a JSON string) as an element of a JSON array.</summary>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON string element of a JSON array.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteStringValue(string? value)
    {
      if (value == null)
        this.WriteNullValue();
      else
        this.WriteStringValue(value.AsSpan());
    }

    /// <summary>Writes a UTF-16 text value (as a JSON string) as an element of a JSON array.</summary>
    /// <param name="value">The UTF-16 encoded value to be written as a UTF-8 transcoded JSON string element of a JSON array.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteStringValue(ReadOnlySpan<char> value)
    {
      JsonWriterHelper.ValidateValue(value);
      this.WriteStringEscape(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable disable
    private void WriteStringEscape(ReadOnlySpan<char> value)
    {
      int firstEscapeIndexVal = JsonWriterHelper.NeedsEscaping(value, this._options.Encoder);
      if (firstEscapeIndexVal != -1)
        this.WriteStringEscapeValue(value, firstEscapeIndexVal);
      else
        this.WriteStringByOptions(value);
    }

    private void WriteStringByOptions(ReadOnlySpan<char> value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteStringIndented(value);
      else
        this.WriteStringMinimized(value);
    }

    private void WriteStringMinimized(ReadOnlySpan<char> escapedValue)
    {
      int requiredSize = escapedValue.Length * 3 + 3;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedValue, span);
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(ReadOnlySpan<char> escapedValue)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedValue.Length * 3 + 3 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      span[this.BytesPending++] = (byte) 34;
      this.TranscodeAndWrite(escapedValue, span);
      span[this.BytesPending++] = (byte) 34;
    }

    private unsafe void WriteStringEscapeValue(ReadOnlySpan<char> value, int firstEscapeIndexVal)
    {
      char[] array = (char[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(value.Length, firstEscapeIndexVal);
      // ISSUE: untyped stack allocation
      Span<char> destination = maxEscapedLength > 128 ? (Span<char>) (array = ArrayPool<char>.Shared.Rent(maxEscapedLength)) : new Span<char>((void*) __untypedstackalloc(new IntPtr(256)), 128);
      int written;
      JsonWriterHelper.EscapeString(value, destination, firstEscapeIndexVal, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<char>) destination.Slice(0, written));
      if (array == null)
        return;
      ArrayPool<char>.Shared.Return(array);
    }


    #nullable enable
    /// <summary>Writes a UTF-8 text value (as a JSON string) as an element of a JSON array.</summary>
    /// <param name="utf8Value">The UTF-8 encoded value to be written as a JSON string element of a JSON array.</param>
    /// <exception cref="T:System.ArgumentException">The specified value is too large.</exception>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the write operation would produce invalid JSON.</exception>
    public void WriteStringValue(ReadOnlySpan<byte> utf8Value)
    {
      JsonWriterHelper.ValidateValue(utf8Value);
      this.WriteStringEscape(utf8Value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }


    #nullable disable
    private void WriteStringEscape(ReadOnlySpan<byte> utf8Value)
    {
      int firstEscapeIndexVal = JsonWriterHelper.NeedsEscaping(utf8Value, this._options.Encoder);
      if (firstEscapeIndexVal != -1)
        this.WriteStringEscapeValue(utf8Value, firstEscapeIndexVal);
      else
        this.WriteStringByOptions(utf8Value);
    }

    private void WriteStringByOptions(ReadOnlySpan<byte> utf8Value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteStringIndented(utf8Value);
      else
        this.WriteStringMinimized(utf8Value);
    }

    private void WriteStringMinimized(ReadOnlySpan<byte> escapedValue)
    {
      int requiredSize = escapedValue.Length + 2 + 1;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      span[this.BytesPending++] = (byte) 34;
      escapedValue.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedValue.Length;
      span[this.BytesPending++] = (byte) 34;
    }

    private void WriteStringIndented(ReadOnlySpan<byte> escapedValue)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + escapedValue.Length + 2 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      span[this.BytesPending++] = (byte) 34;
      escapedValue.CopyTo(span.Slice(this.BytesPending));
      this.BytesPending += escapedValue.Length;
      span[this.BytesPending++] = (byte) 34;
    }

    private unsafe void WriteStringEscapeValue(
      ReadOnlySpan<byte> utf8Value,
      int firstEscapeIndexVal)
    {
      byte[] array = (byte[]) null;
      int maxEscapedLength = JsonWriterHelper.GetMaxEscapedLength(utf8Value.Length, firstEscapeIndexVal);
      // ISSUE: untyped stack allocation
      Span<byte> destination = maxEscapedLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(maxEscapedLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int written;
      JsonWriterHelper.EscapeString(utf8Value, destination, firstEscapeIndexVal, this._options.Encoder, out written);
      this.WriteStringByOptions((ReadOnlySpan<byte>) destination.Slice(0, written));
      if (array == null)
        return;
      ArrayPool<byte>.Shared.Return(array);
    }

    internal void WriteNumberValueAsStringUnescaped(ReadOnlySpan<byte> utf8Value)
    {
      this.WriteStringByOptions(utf8Value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.String;
    }

    /// <summary>Writes a <see cref="T:System.UInt32" /> value (as a JSON number) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON number as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    [CLSCompliant(false)]
    public void WriteNumberValue(uint value) => this.WriteNumberValue((ulong) value);

    /// <summary>Writes a <see cref="T:System.UInt64" /> value (as a JSON number) as an element of a JSON array.</summary>
    /// <param name="value">The value to be written as a JSON number as an element of a JSON array.</param>
    /// <exception cref="T:System.InvalidOperationException">Validation is enabled, and the operation would result in writing invalid JSON.</exception>
    [CLSCompliant(false)]
    public void WriteNumberValue(ulong value)
    {
      if (!this._options.SkipValidation)
        this.ValidateWritingValue();
      if (this._options.Indented)
        this.WriteNumberValueIndented(value);
      else
        this.WriteNumberValueMinimized(value);
      this.SetFlagToAddListSeparatorBeforeNextItem();
      this._tokenType = JsonTokenType.Number;
    }

    private void WriteNumberValueMinimized(ulong value)
    {
      int requiredSize = 21;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    private void WriteNumberValueIndented(ulong value)
    {
      int indentation = this.Indentation;
      int requiredSize = indentation + 20 + 1 + Utf8JsonWriter.s_newLineLength;
      if (this._memory.Length - this.BytesPending < requiredSize)
        this.Grow(requiredSize);
      Span<byte> span = this._memory.Span;
      if (this._currentDepth < 0)
        span[this.BytesPending++] = (byte) 44;
      if (this._tokenType != JsonTokenType.PropertyName)
      {
        if (this._tokenType != JsonTokenType.None)
          this.WriteNewLine(span);
        JsonWriterHelper.WriteIndentation(span.Slice(this.BytesPending), indentation);
        this.BytesPending += indentation;
      }
      int bytesWritten;
      Utf8Formatter.TryFormat(value, span.Slice(this.BytesPending), out bytesWritten, new StandardFormat());
      this.BytesPending += bytesWritten;
    }

    internal unsafe void WriteNumberValueAsString(ulong value)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(20)), 20);
      int bytesWritten;
      Utf8Formatter.TryFormat(value, destination, out bytesWritten, new StandardFormat());
      this.WriteNumberValueAsStringUnescaped((ReadOnlySpan<byte>) destination.Slice(0, bytesWritten));
    }
  }
}
