// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonDocument
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Provides a mechanism for examining the structural content of a JSON value without automatically instantiating data values.</summary>
  public sealed class JsonDocument : IDisposable
  {

    #nullable disable
    private ReadOnlyMemory<byte> _utf8Json;
    private JsonDocument.MetadataDb _parsedData;
    private byte[] _extraRentedArrayPoolBytes;
    private bool _hasExtraRentedArrayPoolBytes;
    private PooledByteBufferWriter _extraPooledByteBufferWriter;
    private bool _hasExtraPooledByteBufferWriter;
    private (int, string) _lastIndexAndString = (-1, (string) null);
    private static JsonDocument s_nullLiteral;
    private static JsonDocument s_trueLiteral;
    private static JsonDocument s_falseLiteral;

    internal bool IsDisposable { get; }

    /// <summary>Gets the root element of this JSON document.</summary>
    /// <returns>A <see cref="T:System.Text.Json.JsonElement" /> representing the value of the document.</returns>
    public JsonElement RootElement => new JsonElement(this, 0);

    private JsonDocument(
      ReadOnlyMemory<byte> utf8Json,
      JsonDocument.MetadataDb parsedData,
      byte[] extraRentedArrayPoolBytes = null,
      PooledByteBufferWriter extraPooledByteBufferWriter = null,
      bool isDisposable = true)
    {
      this._utf8Json = utf8Json;
      this._parsedData = parsedData;
      if (extraRentedArrayPoolBytes != null)
      {
        this._hasExtraRentedArrayPoolBytes = true;
        this._extraRentedArrayPoolBytes = extraRentedArrayPoolBytes;
      }
      else if (extraPooledByteBufferWriter != null)
      {
        this._hasExtraPooledByteBufferWriter = true;
        this._extraPooledByteBufferWriter = extraPooledByteBufferWriter;
      }
      this.IsDisposable = isDisposable;
    }

    /// <summary>Releases the resources used by this <see cref="T:System.Text.Json.JsonDocument" /> instance.</summary>
    public void Dispose()
    {
      int length = this._utf8Json.Length;
      if (length == 0 || !this.IsDisposable)
        return;
      this._parsedData.Dispose();
      this._utf8Json = ReadOnlyMemory<byte>.Empty;
      if (this._hasExtraRentedArrayPoolBytes)
      {
        byte[] array = Interlocked.Exchange<byte[]>(ref this._extraRentedArrayPoolBytes, (byte[]) null);
        if (array == null)
          return;
        array.AsSpan<byte>(0, length).Clear();
        ArrayPool<byte>.Shared.Return(array);
      }
      else
      {
        if (!this._hasExtraPooledByteBufferWriter)
          return;
        Interlocked.Exchange<PooledByteBufferWriter>(ref this._extraPooledByteBufferWriter, (PooledByteBufferWriter) null)?.Dispose();
      }
    }


    #nullable enable
    /// <summary>Writes the document to the provided writer as a JSON value.</summary>
    /// <param name="writer">The writer to which to write the document.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="writer" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Text.Json.JsonElement.ValueKind" /> of this <see cref="P:System.Text.Json.JsonDocument.RootElement" /> would result in invalid JSON.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    public void WriteTo(Utf8JsonWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      this.RootElement.WriteTo(writer);
    }

    internal JsonTokenType GetJsonTokenType(int index)
    {
      this.CheckNotDisposed();
      return this._parsedData.GetJsonTokenType(index);
    }

    internal int GetArrayLength(int index)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.StartArray, dbRow.TokenType);
      return dbRow.SizeOrLength;
    }

    internal JsonElement GetArrayIndexElement(int currentIndex, int arrayIndex)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow1 = this._parsedData.Get(currentIndex);
      this.CheckExpectedType(JsonTokenType.StartArray, dbRow1.TokenType);
      int sizeOrLength = dbRow1.SizeOrLength;
      if ((uint) arrayIndex >= (uint) sizeOrLength)
        throw new IndexOutOfRangeException();
      if (!dbRow1.HasComplexChildren)
        return new JsonElement(this, currentIndex + (arrayIndex + 1) * 12);
      int num = 0;
      for (int index = currentIndex + 12; index < this._parsedData.Length; index += 12)
      {
        if (arrayIndex == num)
          return new JsonElement(this, index);
        JsonDocument.DbRow dbRow2 = this._parsedData.Get(index);
        if (!dbRow2.IsSimpleValue)
          index += 12 * dbRow2.NumberOfRows;
        ++num;
      }
      throw new IndexOutOfRangeException();
    }

    internal int GetEndIndex(int index, bool includeEndElement)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      if (dbRow.IsSimpleValue)
        return index + 12;
      int endIndex = index + 12 * dbRow.NumberOfRows;
      if (includeEndElement)
        endIndex += 12;
      return endIndex;
    }


    #nullable disable
    internal ReadOnlyMemory<byte> GetRootRawValue() => this.GetRawValue(0, true);

    internal ReadOnlyMemory<byte> GetRawValue(int index, bool includeQuotes)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow1 = this._parsedData.Get(index);
      if (dbRow1.IsSimpleValue)
        return includeQuotes && dbRow1.TokenType == JsonTokenType.String ? this._utf8Json.Slice(dbRow1.Location - 1, dbRow1.SizeOrLength + 2) : this._utf8Json.Slice(dbRow1.Location, dbRow1.SizeOrLength);
      int endIndex = this.GetEndIndex(index, false);
      int location = dbRow1.Location;
      JsonDocument.DbRow dbRow2 = this._parsedData.Get(endIndex);
      return this._utf8Json.Slice(location, dbRow2.Location - location + dbRow2.SizeOrLength);
    }

    private ReadOnlyMemory<byte> GetPropertyRawValue(int valueIndex)
    {
      this.CheckNotDisposed();
      int start = this._parsedData.Get(valueIndex - 12).Location - 1;
      JsonDocument.DbRow dbRow1 = this._parsedData.Get(valueIndex);
      if (dbRow1.IsSimpleValue)
      {
        int num = dbRow1.Location + dbRow1.SizeOrLength;
        if (dbRow1.TokenType == JsonTokenType.String)
          ++num;
        return this._utf8Json.Slice(start, num - start);
      }
      JsonDocument.DbRow dbRow2 = this._parsedData.Get(this.GetEndIndex(valueIndex, false));
      int num1 = dbRow2.Location + dbRow2.SizeOrLength;
      return this._utf8Json.Slice(start, num1 - start);
    }

    internal string GetString(int index, JsonTokenType expectedType)
    {
      this.CheckNotDisposed();
      (int num, string str1) = this._lastIndexAndString;
      if (num == index)
        return str1;
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      JsonTokenType tokenType = dbRow.TokenType;
      if (tokenType == JsonTokenType.Null)
        return (string) null;
      this.CheckExpectedType(expectedType, tokenType);
      ReadOnlySpan<byte> readOnlySpan = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      string str2;
      if (dbRow.HasComplexChildren)
      {
        int idx = readOnlySpan.IndexOf<byte>((byte) 92);
        str2 = JsonReaderHelper.GetUnescapedString(readOnlySpan, idx);
      }
      else
        str2 = JsonReaderHelper.TranscodeHelper(readOnlySpan);
      this._lastIndexAndString = (index, str2);
      return str2;
    }

    internal unsafe bool TextEquals(int index, ReadOnlySpan<char> otherText, bool isPropertyName)
    {
      this.CheckNotDisposed();
      int num1 = isPropertyName ? index - 12 : index;
      (int num2, string text) = this._lastIndexAndString;
      if (num2 == num1)
        return otherText.SequenceEqual<char>(text.AsSpan());
      byte[] array = (byte[]) null;
      int minimumLength = checked (otherText.Length * 3);
      // ISSUE: untyped stack allocation
      Span<byte> utf8Destination = minimumLength > 256 ? (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(minimumLength)) : new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int bytesWritten;
      bool flag = JsonWriterHelper.ToUtf8(MemoryMarshal.AsBytes<char>(otherText), utf8Destination, out int _, out bytesWritten) <= OperationStatus.DestinationTooSmall && this.TextEquals(index, (ReadOnlySpan<byte>) utf8Destination.Slice(0, bytesWritten), isPropertyName, true);
      if (array != null)
      {
        utf8Destination.Slice(0, bytesWritten).Clear();
        ArrayPool<byte>.Shared.Return(array);
      }
      return flag;
    }

    internal bool TextEquals(
      int index,
      ReadOnlySpan<byte> otherUtf8Text,
      bool isPropertyName,
      bool shouldUnescape)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(isPropertyName ? index - 12 : index);
      this.CheckExpectedType(isPropertyName ? JsonTokenType.PropertyName : JsonTokenType.String, dbRow.TokenType);
      ReadOnlySpan<byte> span = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      if (otherUtf8Text.Length > span.Length || !shouldUnescape && otherUtf8Text.Length != span.Length)
        return false;
      if (!(dbRow.HasComplexChildren & shouldUnescape))
        return span.SequenceEqual<byte>(otherUtf8Text);
      if (otherUtf8Text.Length < span.Length / 6)
        return false;
      int num = span.IndexOf<byte>((byte) 92);
      return otherUtf8Text.StartsWith<byte>(span.Slice(0, num)) && JsonReaderHelper.UnescapeAndCompare(span.Slice(num), otherUtf8Text.Slice(num));
    }

    internal string GetNameOfPropertyValue(int index) => this.GetString(index - 12, JsonTokenType.PropertyName);

    internal bool TryGetValue(int index, [NotNullWhen(true)] out byte[] value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.String, dbRow.TokenType);
      ReadOnlySpan<byte> readOnlySpan = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      if (!dbRow.HasComplexChildren)
        return JsonReaderHelper.TryDecodeBase64(readOnlySpan, out value);
      int idx = readOnlySpan.IndexOf<byte>((byte) 92);
      return JsonReaderHelper.TryGetUnescapedBase64Bytes(readOnlySpan, idx, out value);
    }

    internal bool TryGetValue(int index, out sbyte value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      sbyte num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && bytesConsumed == source.Length)
      {
        value = num;
        return true;
      }
      value = (sbyte) 0;
      return false;
    }

    internal bool TryGetValue(int index, out byte value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      byte num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && bytesConsumed == source.Length)
      {
        value = num;
        return true;
      }
      value = (byte) 0;
      return false;
    }

    internal bool TryGetValue(int index, out short value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      short num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && bytesConsumed == source.Length)
      {
        value = num;
        return true;
      }
      value = (short) 0;
      return false;
    }

    internal bool TryGetValue(int index, out ushort value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      ushort num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && bytesConsumed == source.Length)
      {
        value = num;
        return true;
      }
      value = (ushort) 0;
      return false;
    }

    internal bool TryGetValue(int index, out int value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      int num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && bytesConsumed == source.Length)
      {
        value = num;
        return true;
      }
      value = 0;
      return false;
    }

    internal bool TryGetValue(int index, out uint value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      uint num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && bytesConsumed == source.Length)
      {
        value = num;
        return true;
      }
      value = 0U;
      return false;
    }

    internal bool TryGetValue(int index, out long value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      long num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && bytesConsumed == source.Length)
      {
        value = num;
        return true;
      }
      value = 0L;
      return false;
    }

    internal bool TryGetValue(int index, out ulong value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      ulong num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && bytesConsumed == source.Length)
      {
        value = num;
        return true;
      }
      value = 0UL;
      return false;
    }

    internal bool TryGetValue(int index, out double value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      double num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && source.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = 0.0;
      return false;
    }

    internal bool TryGetValue(int index, out float value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      float num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && source.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = 0.0f;
      return false;
    }

    internal bool TryGetValue(int index, out Decimal value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.Number, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      Decimal num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(source, out num, out bytesConsumed) && source.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = 0M;
      return false;
    }

    internal bool TryGetValue(int index, out DateTime value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.String, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      if (!JsonHelpers.IsValidDateTimeOffsetParseLength(source.Length))
      {
        value = new DateTime();
        return false;
      }
      if (dbRow.HasComplexChildren)
        return JsonReaderHelper.TryGetEscapedDateTime(source, out value);
      DateTime dateTime;
      if (JsonHelpers.TryParseAsISO(source, out dateTime))
      {
        value = dateTime;
        return true;
      }
      value = new DateTime();
      return false;
    }

    internal bool TryGetValue(int index, out DateTimeOffset value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.String, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      if (!JsonHelpers.IsValidDateTimeOffsetParseLength(source.Length))
      {
        value = new DateTimeOffset();
        return false;
      }
      if (dbRow.HasComplexChildren)
        return JsonReaderHelper.TryGetEscapedDateTimeOffset(source, out value);
      DateTimeOffset dateTimeOffset;
      if (JsonHelpers.TryParseAsISO(source, out dateTimeOffset))
      {
        value = dateTimeOffset;
        return true;
      }
      value = new DateTimeOffset();
      return false;
    }

    internal bool TryGetValue(int index, out Guid value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.String, dbRow.TokenType);
      ReadOnlySpan<byte> source = this._utf8Json.Span.Slice(dbRow.Location, dbRow.SizeOrLength);
      if (source.Length > 216)
      {
        value = new Guid();
        return false;
      }
      if (dbRow.HasComplexChildren)
        return JsonReaderHelper.TryGetEscapedGuid(source, out value);
      Guid guid;
      if (source.Length == 36 && Utf8Parser.TryParse(source, out guid, out int _, 'D'))
      {
        value = guid;
        return true;
      }
      value = new Guid();
      return false;
    }

    internal string GetRawValueAsString(int index) => JsonReaderHelper.TranscodeHelper(this.GetRawValue(index, true).Span);

    internal string GetPropertyRawValueAsString(int valueIndex) => JsonReaderHelper.TranscodeHelper(this.GetPropertyRawValue(valueIndex).Span);

    internal JsonElement CloneElement(int index)
    {
      int endIndex = this.GetEndIndex(index, true);
      JsonDocument.MetadataDb parsedData = this._parsedData.CopySegment(index, endIndex);
      return new JsonDocument((ReadOnlyMemory<byte>) this.GetRawValue(index, true).ToArray(), parsedData, isDisposable: false).RootElement;
    }

    internal void WriteElementTo(int index, Utf8JsonWriter writer)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow row = this._parsedData.Get(index);
      switch (row.TokenType)
      {
        case JsonTokenType.StartObject:
          writer.WriteStartObject();
          this.WriteComplexElement(index, writer);
          break;
        case JsonTokenType.StartArray:
          writer.WriteStartArray();
          this.WriteComplexElement(index, writer);
          break;
        case JsonTokenType.String:
          this.WriteString(in row, writer);
          break;
        case JsonTokenType.Number:
          writer.WriteNumberValue(this._utf8Json.Slice(row.Location, row.SizeOrLength).Span);
          break;
        case JsonTokenType.True:
          writer.WriteBooleanValue(true);
          break;
        case JsonTokenType.False:
          writer.WriteBooleanValue(false);
          break;
        case JsonTokenType.Null:
          writer.WriteNullValue();
          break;
      }
    }

    private void WriteComplexElement(int index, Utf8JsonWriter writer)
    {
      int endIndex = this.GetEndIndex(index, true);
      for (int index1 = index + 12; index1 < endIndex; index1 += 12)
      {
        JsonDocument.DbRow row = this._parsedData.Get(index1);
        switch (row.TokenType)
        {
          case JsonTokenType.StartObject:
            writer.WriteStartObject();
            break;
          case JsonTokenType.EndObject:
            writer.WriteEndObject();
            break;
          case JsonTokenType.StartArray:
            writer.WriteStartArray();
            break;
          case JsonTokenType.EndArray:
            writer.WriteEndArray();
            break;
          case JsonTokenType.PropertyName:
            this.WritePropertyName(in row, writer);
            break;
          case JsonTokenType.String:
            this.WriteString(in row, writer);
            break;
          case JsonTokenType.Number:
            writer.WriteNumberValue(this._utf8Json.Slice(row.Location, row.SizeOrLength).Span);
            break;
          case JsonTokenType.True:
            writer.WriteBooleanValue(true);
            break;
          case JsonTokenType.False:
            writer.WriteBooleanValue(false);
            break;
          case JsonTokenType.Null:
            writer.WriteNullValue();
            break;
        }
      }
    }

    private ReadOnlySpan<byte> UnescapeString(
      in JsonDocument.DbRow row,
      out ArraySegment<byte> rented)
    {
      int location = row.Location;
      int sizeOrLength = row.SizeOrLength;
      ReadOnlySpan<byte> span = this._utf8Json.Slice(location, sizeOrLength).Span;
      if (!row.HasComplexChildren)
      {
        rented = new ArraySegment<byte>();
        return span;
      }
      int num = span.IndexOf<byte>((byte) 92);
      byte[] numArray = ArrayPool<byte>.Shared.Rent(sizeOrLength);
      span.Slice(0, num).CopyTo((Span<byte>) numArray);
      int written;
      JsonReaderHelper.Unescape(span, (Span<byte>) numArray, num, out written);
      rented = new ArraySegment<byte>(numArray, 0, written);
      return (ReadOnlySpan<byte>) rented.AsSpan<byte>();
    }

    private static void ClearAndReturn(ArraySegment<byte> rented)
    {
      if (rented.Array == null)
        return;
      rented.AsSpan<byte>().Clear();
      ArrayPool<byte>.Shared.Return(rented.Array);
    }

    private void WritePropertyName(in JsonDocument.DbRow row, Utf8JsonWriter writer)
    {
      ArraySegment<byte> rented = new ArraySegment<byte>();
      try
      {
        writer.WritePropertyName(this.UnescapeString(in row, out rented));
      }
      finally
      {
        JsonDocument.ClearAndReturn(rented);
      }
    }

    private void WriteString(in JsonDocument.DbRow row, Utf8JsonWriter writer)
    {
      ArraySegment<byte> rented = new ArraySegment<byte>();
      try
      {
        writer.WriteStringValue(this.UnescapeString(in row, out rented));
      }
      finally
      {
        JsonDocument.ClearAndReturn(rented);
      }
    }

    private static void Parse(
      ReadOnlySpan<byte> utf8JsonSpan,
      JsonReaderOptions readerOptions,
      ref JsonDocument.MetadataDb database,
      ref JsonDocument.StackRowStack stack)
    {
      bool flag = false;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      Utf8JsonReader utf8JsonReader = new Utf8JsonReader(utf8JsonSpan, true, new JsonReaderState(readerOptions));
      while (utf8JsonReader.Read())
      {
        JsonTokenType tokenType = utf8JsonReader.TokenType;
        int tokenStartIndex = (int) utf8JsonReader.TokenStartIndex;
        switch (tokenType)
        {
          case JsonTokenType.StartObject:
            if (flag)
              ++num1;
            ++num3;
            database.Append(tokenType, tokenStartIndex, -1);
            JsonDocument.StackRow row1 = new JsonDocument.StackRow(num2 + 1);
            stack.Push(row1);
            num2 = 0;
            break;
          case JsonTokenType.EndObject:
            int unsetSizeOrLength1 = database.FindIndexOfFirstUnsetSizeOrLength(JsonTokenType.StartObject);
            ++num3;
            int num4 = num2 + 1;
            database.SetLength(unsetSizeOrLength1, num4);
            int length1 = database.Length;
            database.Append(tokenType, tokenStartIndex, utf8JsonReader.ValueSpan.Length);
            database.SetNumberOfRows(unsetSizeOrLength1, num4);
            database.SetNumberOfRows(length1, num4);
            JsonDocument.StackRow stackRow1 = stack.Pop();
            num2 = num4 + stackRow1.SizeOrLength;
            break;
          case JsonTokenType.StartArray:
            if (flag)
              ++num1;
            ++num2;
            database.Append(tokenType, tokenStartIndex, -1);
            JsonDocument.StackRow row2 = new JsonDocument.StackRow(num1, num3 + 1);
            stack.Push(row2);
            num1 = 0;
            num3 = 0;
            break;
          case JsonTokenType.EndArray:
            int unsetSizeOrLength2 = database.FindIndexOfFirstUnsetSizeOrLength(JsonTokenType.StartArray);
            int numberOfRows = num3 + 1;
            ++num2;
            database.SetLength(unsetSizeOrLength2, num1);
            database.SetNumberOfRows(unsetSizeOrLength2, numberOfRows);
            if (num1 + 1 != numberOfRows)
              database.SetHasComplexChildren(unsetSizeOrLength2);
            int length2 = database.Length;
            database.Append(tokenType, tokenStartIndex, utf8JsonReader.ValueSpan.Length);
            database.SetNumberOfRows(length2, numberOfRows);
            JsonDocument.StackRow stackRow2 = stack.Pop();
            num1 = stackRow2.SizeOrLength;
            num3 = numberOfRows + stackRow2.NumberOfRows;
            break;
          case JsonTokenType.PropertyName:
            ++num3;
            ++num2;
            database.Append(tokenType, tokenStartIndex + 1, utf8JsonReader.ValueSpan.Length);
            if (utf8JsonReader._stringHasEscaping)
            {
              database.SetHasComplexChildren(database.Length - 12);
              break;
            }
            break;
          default:
            ++num3;
            ++num2;
            if (flag)
              ++num1;
            if (tokenType == JsonTokenType.String)
            {
              database.Append(tokenType, tokenStartIndex + 1, utf8JsonReader.ValueSpan.Length);
              if (utf8JsonReader._stringHasEscaping)
              {
                database.SetHasComplexChildren(database.Length - 12);
                break;
              }
              break;
            }
            database.Append(tokenType, tokenStartIndex, utf8JsonReader.ValueSpan.Length);
            break;
        }
        flag = utf8JsonReader.IsInArray;
      }
      database.CompleteAllocations();
    }

    private void CheckNotDisposed()
    {
      if (this._utf8Json.IsEmpty)
        throw new ObjectDisposedException(nameof (JsonDocument));
    }

    private void CheckExpectedType(JsonTokenType expected, JsonTokenType actual)
    {
      if (expected != actual)
        throw ThrowHelper.GetJsonElementWrongTypeException(expected, actual);
    }

    private static void CheckSupportedOptions(JsonReaderOptions readerOptions, string paramName)
    {
      if (readerOptions.CommentHandling == JsonCommentHandling.Allow)
        throw new ArgumentException(SR.JsonDocumentDoesNotSupportComments, paramName);
    }


    #nullable enable
    /// <summary>Parses memory as UTF-8-encoded text representing a single JSON byte value into a JsonDocument.</summary>
    /// <param name="utf8Json">The JSON text to parse.</param>
    /// <param name="options">Options to control the reader behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> contains unsupported options.</exception>
    /// <returns>A JsonDocument representation of the JSON value.</returns>
    public static JsonDocument Parse(
      ReadOnlyMemory<byte> utf8Json,
      JsonDocumentOptions options = default (JsonDocumentOptions))
    {
      return JsonDocument.Parse(utf8Json, options.GetReaderOptions());
    }

    /// <summary>Parses a sequence as UTF-8-encoded text representing a single JSON byte value into a JsonDocument.</summary>
    /// <param name="utf8Json">The JSON text to parse.</param>
    /// <param name="options">Options to control the reader behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> contains unsupported options.</exception>
    /// <returns>A JsonDocument representation of the JSON value.</returns>
    public static JsonDocument Parse(
      ReadOnlySequence<byte> utf8Json,
      JsonDocumentOptions options = default (JsonDocumentOptions))
    {
      JsonReaderOptions readerOptions = options.GetReaderOptions();
      if (utf8Json.IsSingleSegment)
        return JsonDocument.Parse(utf8Json.First, readerOptions);
      int length = checked ((int) utf8Json.Length);
      byte[] numArray = ArrayPool<byte>.Shared.Rent(length);
      try
      {
        utf8Json.CopyTo<byte>(numArray.AsSpan<byte>());
        return JsonDocument.Parse((ReadOnlyMemory<byte>) numArray.AsMemory<byte>(0, length), readerOptions, numArray);
      }
      catch
      {
        numArray.AsSpan<byte>(0, length).Clear();
        ArrayPool<byte>.Shared.Return(numArray);
        throw;
      }
    }

    /// <summary>Parses a <see cref="T:System.IO.Stream" /> as UTF-8-encoded data representing a single JSON value into a JsonDocument. The stream is read to completion.</summary>
    /// <param name="utf8Json">The JSON data to parse.</param>
    /// <param name="options">Options to control the reader behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> contains unsupported options.</exception>
    /// <returns>A JsonDocument representation of the JSON value.</returns>
    public static JsonDocument Parse(Stream utf8Json, JsonDocumentOptions options = default (JsonDocumentOptions))
    {
      ArraySegment<byte> segment = utf8Json != null ? JsonDocument.ReadToEnd(utf8Json) : throw new ArgumentNullException(nameof (utf8Json));
      try
      {
        return JsonDocument.Parse((ReadOnlyMemory<byte>) segment.AsMemory<byte>(), options.GetReaderOptions(), segment.Array);
      }
      catch
      {
        segment.AsSpan<byte>().Clear();
        ArrayPool<byte>.Shared.Return(segment.Array);
        throw;
      }
    }


    #nullable disable
    internal static JsonDocument ParseRented(
      PooledByteBufferWriter utf8Json,
      JsonDocumentOptions options = default (JsonDocumentOptions))
    {
      return JsonDocument.Parse(utf8Json.WrittenMemory, options.GetReaderOptions(), extraPooledByteBufferWriter: utf8Json);
    }

    internal static JsonDocument ParseValue(
      Stream utf8Json,
      JsonDocumentOptions options)
    {
      ArraySegment<byte> end = JsonDocument.ReadToEnd(utf8Json);
      byte[] numArray = new byte[end.Count];
      Buffer.BlockCopy((Array) end.Array, 0, (Array) numArray, 0, end.Count);
      end.AsSpan<byte>().Clear();
      ArrayPool<byte>.Shared.Return(end.Array);
      return JsonDocument.ParseUnrented((ReadOnlyMemory<byte>) numArray.AsMemory<byte>(), options.GetReaderOptions());
    }

    internal static JsonDocument ParseValue(
      ReadOnlySpan<byte> utf8Json,
      JsonDocumentOptions options)
    {
      byte[] numArray = new byte[utf8Json.Length];
      utf8Json.CopyTo((Span<byte>) numArray);
      return JsonDocument.ParseUnrented((ReadOnlyMemory<byte>) numArray.AsMemory<byte>(), options.GetReaderOptions());
    }

    internal static JsonDocument ParseValue(string json, JsonDocumentOptions options) => JsonDocument.ParseValue(json.AsMemory(), options);


    #nullable enable
    /// <summary>Parses a <see cref="T:System.IO.Stream" /> as UTF-8-encoded data representing a single JSON value into a JsonDocument. The stream is read to completion.</summary>
    /// <param name="utf8Json">The JSON data to parse.</param>
    /// <param name="options">Options to control the reader behavior during parsing.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> contains unsupported options.</exception>
    /// <returns>A task to produce a JsonDocument representation of the JSON value.</returns>
    public static Task<JsonDocument> ParseAsync(
      Stream utf8Json,
      JsonDocumentOptions options = default (JsonDocumentOptions),
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      return JsonDocument.ParseAsyncCore(utf8Json, options, cancellationToken);
    }


    #nullable disable
    private static async Task<JsonDocument> ParseAsyncCore(
      Stream utf8Json,
      JsonDocumentOptions options = default (JsonDocumentOptions),
      CancellationToken cancellationToken = default (CancellationToken))
    {
      ArraySegment<byte> segment = await JsonDocument.ReadToEndAsync(utf8Json, cancellationToken).ConfigureAwait(false);
      JsonDocument asyncCore;
      try
      {
        asyncCore = JsonDocument.Parse((ReadOnlyMemory<byte>) segment.AsMemory<byte>(), options.GetReaderOptions(), segment.Array);
      }
      catch
      {
        segment.AsSpan<byte>().Clear();
        ArrayPool<byte>.Shared.Return(segment.Array);
        throw;
      }
      return asyncCore;
    }


    #nullable enable
    /// <summary>Parses text representing a single JSON value into a JsonDocument.</summary>
    /// <param name="json">The JSON text to parse.</param>
    /// <param name="options">Options to control the reader behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="json" /> does not represent a valid single JSON value.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> contains unsupported options.</exception>
    /// <returns>A JsonDocument representation of the JSON value.</returns>
    public static JsonDocument Parse(
      ReadOnlyMemory<char> json,
      JsonDocumentOptions options = default (JsonDocumentOptions))
    {
      ReadOnlySpan<char> span = json.Span;
      int utf8ByteCount = JsonReaderHelper.GetUtf8ByteCount(span);
      byte[] numArray = ArrayPool<byte>.Shared.Rent(utf8ByteCount);
      try
      {
        int utf8FromText = JsonReaderHelper.GetUtf8FromText(span, (Span<byte>) numArray);
        return JsonDocument.Parse((ReadOnlyMemory<byte>) numArray.AsMemory<byte>(0, utf8FromText), options.GetReaderOptions(), numArray);
      }
      catch
      {
        numArray.AsSpan<byte>(0, utf8ByteCount).Clear();
        ArrayPool<byte>.Shared.Return(numArray);
        throw;
      }
    }


    #nullable disable
    internal static JsonDocument ParseValue(
      ReadOnlyMemory<char> json,
      JsonDocumentOptions options)
    {
      ReadOnlySpan<char> span = json.Span;
      int utf8ByteCount = JsonReaderHelper.GetUtf8ByteCount(span);
      byte[] numArray1 = ArrayPool<byte>.Shared.Rent(utf8ByteCount);
      byte[] numArray2;
      try
      {
        int utf8FromText = JsonReaderHelper.GetUtf8FromText(span, (Span<byte>) numArray1);
        numArray2 = new byte[utf8FromText];
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, utf8FromText);
      }
      finally
      {
        numArray1.AsSpan<byte>(0, utf8ByteCount).Clear();
        ArrayPool<byte>.Shared.Return(numArray1);
      }
      return JsonDocument.ParseUnrented((ReadOnlyMemory<byte>) numArray2.AsMemory<byte>(), options.GetReaderOptions());
    }


    #nullable enable
    /// <summary>Parses text representing a single JSON string value into a JsonDocument.</summary>
    /// <param name="json">The JSON text to parse.</param>
    /// <param name="options">Options to control the reader behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="json" /> does not represent a valid single JSON value.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> contains unsupported options.</exception>
    /// <returns>A JsonDocument representation of the JSON value.</returns>
    public static JsonDocument Parse(string json, JsonDocumentOptions options = default (JsonDocumentOptions)) => json != null ? JsonDocument.Parse(json.AsMemory(), options) : throw new ArgumentNullException(nameof (json));

    /// <summary>Attempts to parse one JSON value (including objects or arrays) from the provided reader.</summary>
    /// <param name="reader">The reader to read.</param>
    /// <param name="document">When the method returns, contains the parsed document.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="reader" /> contains unsupported options.
    /// 
    /// -or-
    /// 
    /// The current <paramref name="reader" /> token does not start or represent a value.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">A value could not be read from the reader.</exception>
    /// <returns>
    /// <see langword="true" /> if a value was read and parsed into a JsonDocument; <see langword="false" /> if the reader ran out of data while parsing. All other situations result in an exception being thrown.</returns>
    public static bool TryParseValue(ref Utf8JsonReader reader, [NotNullWhen(true)] out JsonDocument? document) => JsonDocument.TryParseValue(ref reader, out document, false, true);

    /// <summary>Parses one JSON value (including objects or arrays) from the provided reader.</summary>
    /// <param name="reader">The reader to read.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="reader" /> contains unsupported options.
    /// 
    /// -or-
    /// 
    /// The current <paramref name="reader" /> token does not start or represent a value.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">A value could not be read from the reader.</exception>
    /// <returns>A JsonDocument representing the value (and nested values) read from the reader.</returns>
    public static JsonDocument ParseValue(ref Utf8JsonReader reader)
    {
      JsonDocument document;
      JsonDocument.TryParseValue(ref reader, out document, true, true);
      return document;
    }


    #nullable disable
    internal static bool TryParseValue(
      ref Utf8JsonReader reader,
      [NotNullWhen(true)] out JsonDocument document,
      bool shouldThrow,
      bool useArrayPools)
    {
      JsonReaderState currentState = reader.CurrentState;
      JsonDocument.CheckSupportedOptions(currentState.Options, nameof (reader));
      Utf8JsonReader utf8JsonReader = reader;
      ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>();
      ReadOnlySequence<byte> readOnlySequence = new ReadOnlySequence<byte>();
      try
      {
        switch (reader.TokenType)
        {
          case JsonTokenType.None:
          case JsonTokenType.PropertyName:
            if (!reader.Read())
            {
              if (shouldThrow)
                ThrowHelper.ThrowJsonReaderException(ref reader, ExceptionResource.ExpectedJsonTokens);
              reader = utf8JsonReader;
              document = (JsonDocument) null;
              return false;
            }
            break;
        }
        switch (reader.TokenType)
        {
          case JsonTokenType.StartObject:
          case JsonTokenType.StartArray:
            long tokenStartIndex = reader.TokenStartIndex;
            if (!reader.TrySkip())
            {
              if (shouldThrow)
                ThrowHelper.ThrowJsonReaderException(ref reader, ExceptionResource.ExpectedJsonTokens);
              reader = utf8JsonReader;
              document = (JsonDocument) null;
              return false;
            }
            long length1 = reader.BytesConsumed - tokenStartIndex;
            ReadOnlySequence<byte> originalSequence1 = reader.OriginalSequence;
            if (originalSequence1.IsEmpty)
            {
              readOnlySpan = reader.OriginalSpan.Slice(checked ((int) tokenStartIndex), checked ((int) length1));
              break;
            }
            readOnlySequence = originalSequence1.Slice(tokenStartIndex, length1);
            break;
          case JsonTokenType.String:
            ReadOnlySequence<byte> originalSequence2 = reader.OriginalSequence;
            if (originalSequence2.IsEmpty)
            {
              int length2 = reader.ValueSpan.Length + 2;
              readOnlySpan = reader.OriginalSpan.Slice((int) reader.TokenStartIndex, length2);
              break;
            }
            long num = 2;
            long length3 = !reader.HasValueSequence ? num + (long) reader.ValueSpan.Length : num + reader.ValueSequence.Length;
            readOnlySequence = originalSequence2.Slice(reader.TokenStartIndex, length3);
            break;
          case JsonTokenType.Number:
            if (reader.HasValueSequence)
            {
              readOnlySequence = reader.ValueSequence;
              break;
            }
            readOnlySpan = reader.ValueSpan;
            break;
          case JsonTokenType.True:
          case JsonTokenType.False:
          case JsonTokenType.Null:
            if (useArrayPools)
            {
              if (reader.HasValueSequence)
              {
                readOnlySequence = reader.ValueSequence;
                break;
              }
              readOnlySpan = reader.ValueSpan;
              break;
            }
            document = JsonDocument.CreateForLiteral(reader.TokenType);
            return true;
          default:
            if (shouldThrow)
            {
              byte nextByte = reader.ValueSpan[0];
              ThrowHelper.ThrowJsonReaderException(ref reader, ExceptionResource.ExpectedStartOfValueNotFound, nextByte);
            }
            reader = utf8JsonReader;
            document = (JsonDocument) null;
            return false;
        }
      }
      catch
      {
        reader = utf8JsonReader;
        throw;
      }
      int num1 = readOnlySpan.IsEmpty ? checked ((int) readOnlySequence.Length) : readOnlySpan.Length;
      if (useArrayPools)
      {
        byte[] numArray = ArrayPool<byte>.Shared.Rent(num1);
        Span<byte> destination = numArray.AsSpan<byte>(0, num1);
        try
        {
          if (readOnlySpan.IsEmpty)
            readOnlySequence.CopyTo<byte>(destination);
          else
            readOnlySpan.CopyTo(destination);
          document = JsonDocument.Parse((ReadOnlyMemory<byte>) numArray.AsMemory<byte>(0, num1), currentState.Options, numArray);
        }
        catch
        {
          destination.Clear();
          ArrayPool<byte>.Shared.Return(numArray);
          throw;
        }
      }
      else
      {
        byte[] utf8Json = !readOnlySpan.IsEmpty ? readOnlySpan.ToArray() : readOnlySequence.ToArray<byte>();
        document = JsonDocument.ParseUnrented((ReadOnlyMemory<byte>) utf8Json, currentState.Options, reader.TokenType);
      }
      return true;
    }

    private static JsonDocument CreateForLiteral(JsonTokenType tokenType)
    {
      switch (tokenType)
      {
        case JsonTokenType.True:
          if (JsonDocument.s_trueLiteral == null)
            JsonDocument.s_trueLiteral = Create(JsonConstants.TrueValue.ToArray());
          return JsonDocument.s_trueLiteral;
        case JsonTokenType.False:
          if (JsonDocument.s_falseLiteral == null)
            JsonDocument.s_falseLiteral = Create(JsonConstants.FalseValue.ToArray());
          return JsonDocument.s_falseLiteral;
        default:
          if (JsonDocument.s_nullLiteral == null)
            JsonDocument.s_nullLiteral = Create(JsonConstants.NullValue.ToArray());
          return JsonDocument.s_nullLiteral;
      }

      JsonDocument Create(byte[] utf8Json)
      {
        JsonDocument.MetadataDb locked = JsonDocument.MetadataDb.CreateLocked(utf8Json.Length);
        locked.Append(tokenType, 0, utf8Json.Length);
        return new JsonDocument((ReadOnlyMemory<byte>) utf8Json, locked);
      }
    }

    private static JsonDocument Parse(
      ReadOnlyMemory<byte> utf8Json,
      JsonReaderOptions readerOptions,
      byte[] extraRentedArrayPoolBytes = null,
      PooledByteBufferWriter extraPooledByteBufferWriter = null)
    {
      ReadOnlySpan<byte> span = utf8Json.Span;
      JsonDocument.MetadataDb rented = JsonDocument.MetadataDb.CreateRented(utf8Json.Length, false);
      using (JsonDocument.StackRowStack stack = new JsonDocument.StackRowStack(512))
      {
        try
        {
          JsonDocument.Parse(span, readerOptions, ref rented, ref stack);
        }
        catch
        {
          rented.Dispose();
          throw;
        }
      }
      return new JsonDocument(utf8Json, rented, extraRentedArrayPoolBytes, extraPooledByteBufferWriter);
    }

    private static JsonDocument ParseUnrented(
      ReadOnlyMemory<byte> utf8Json,
      JsonReaderOptions readerOptions,
      JsonTokenType tokenType = JsonTokenType.None)
    {
      ReadOnlySpan<byte> span = utf8Json.Span;
      JsonDocument.MetadataDb database;
      if (tokenType == JsonTokenType.String || tokenType == JsonTokenType.Number)
      {
        database = JsonDocument.MetadataDb.CreateLocked(utf8Json.Length);
        JsonDocument.StackRowStack stack = new JsonDocument.StackRowStack();
        JsonDocument.Parse(span, readerOptions, ref database, ref stack);
      }
      else
      {
        database = JsonDocument.MetadataDb.CreateRented(utf8Json.Length, true);
        using (JsonDocument.StackRowStack stack = new JsonDocument.StackRowStack(512))
          JsonDocument.Parse(span, readerOptions, ref database, ref stack);
      }
      return new JsonDocument(utf8Json, database);
    }

    private static ArraySegment<byte> ReadToEnd(Stream stream)
    {
      int num1 = 0;
      byte[] numArray1 = (byte[]) null;
      ReadOnlySpan<byte> utf8Bom = JsonConstants.Utf8Bom;
      try
      {
        numArray1 = !stream.CanSeek ? ArrayPool<byte>.Shared.Rent(4096) : ArrayPool<byte>.Shared.Rent(checked ((int) unchecked (Math.Max((long) utf8Bom.Length, stream.Length - stream.Position) + 1L)));
        int num2;
        do
        {
          num2 = stream.Read(numArray1, num1, utf8Bom.Length - num1);
          num1 += num2;
        }
        while (num2 > 0 && num1 < utf8Bom.Length);
        if (num1 == utf8Bom.Length && utf8Bom.SequenceEqual<byte>((ReadOnlySpan<byte>) numArray1.AsSpan<byte>(0, utf8Bom.Length)))
          num1 = 0;
        int num3;
        do
        {
          if (numArray1.Length == num1)
          {
            byte[] numArray2 = numArray1;
            numArray1 = ArrayPool<byte>.Shared.Rent(checked (numArray2.Length * 2));
            Buffer.BlockCopy((Array) numArray2, 0, (Array) numArray1, 0, numArray2.Length);
            ArrayPool<byte>.Shared.Return(numArray2, true);
          }
          num3 = stream.Read(numArray1, num1, numArray1.Length - num1);
          num1 += num3;
        }
        while (num3 > 0);
        return new ArraySegment<byte>(numArray1, 0, num1);
      }
      catch
      {
        if (numArray1 != null)
        {
          numArray1.AsSpan<byte>(0, num1).Clear();
          ArrayPool<byte>.Shared.Return(numArray1);
        }
        throw;
      }
    }

    private static async ValueTask<ArraySegment<byte>> ReadToEndAsync(
      Stream stream,
      CancellationToken cancellationToken)
    {
      int written = 0;
      byte[] rented = (byte[]) null;
      ArraySegment<byte> endAsync;
      try
      {
        int utf8BomLength = JsonConstants.Utf8Bom.Length;
        rented = !stream.CanSeek ? ArrayPool<byte>.Shared.Rent(4096) : ArrayPool<byte>.Shared.Rent(checked ((int) unchecked (Math.Max((long) utf8BomLength, stream.Length - stream.Position) + 1L)));
        int num1;
        do
        {
          num1 = await stream.ReadAsync(rented.AsMemory<byte>(written, utf8BomLength - written), cancellationToken).ConfigureAwait(false);
          written += num1;
        }
        while (num1 > 0 && written < utf8BomLength);
        if (written == utf8BomLength && JsonConstants.Utf8Bom.SequenceEqual<byte>((ReadOnlySpan<byte>) rented.AsSpan<byte>(0, utf8BomLength)))
          written = 0;
        int num2;
        do
        {
          if (rented.Length == written)
          {
            byte[] numArray = rented;
            rented = ArrayPool<byte>.Shared.Rent(numArray.Length * 2);
            Buffer.BlockCopy((Array) numArray, 0, (Array) rented, 0, numArray.Length);
            ArrayPool<byte>.Shared.Return(numArray, true);
          }
          num2 = await stream.ReadAsync(rented.AsMemory<byte>(written), cancellationToken).ConfigureAwait(false);
          written += num2;
        }
        while (num2 > 0);
        endAsync = new ArraySegment<byte>(rented, 0, written);
      }
      catch
      {
        if (rented != null)
        {
          rented.AsSpan<byte>(0, written).Clear();
          ArrayPool<byte>.Shared.Return(rented);
        }
        throw;
      }
      rented = (byte[]) null;
      return endAsync;
    }

    internal unsafe bool TryGetNamedPropertyValue(
      int index,
      ReadOnlySpan<char> propertyName,
      out JsonElement value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow1 = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.StartObject, dbRow1.TokenType);
      if (dbRow1.NumberOfRows == 1)
      {
        value = new JsonElement();
        return false;
      }
      int maxByteCount = JsonReaderHelper.s_utf8Encoding.GetMaxByteCount(propertyName.Length);
      int startIndex = index + 12;
      int endIndex = checked (dbRow1.NumberOfRows * 12 + index);
      if (maxByteCount < 256)
      {
        // ISSUE: untyped stack allocation
        Span<byte> dest = new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
        int utf8FromText = JsonReaderHelper.GetUtf8FromText(propertyName, dest);
        Span<byte> propertyName1 = dest.Slice(0, utf8FromText);
        return this.TryGetNamedPropertyValue(startIndex, endIndex, (ReadOnlySpan<byte>) propertyName1, out value);
      }
      int length = propertyName.Length;
      int index1;
      for (int index2 = endIndex - 12; index2 > index; index2 = index1 - 12)
      {
        int num = index2;
        JsonDocument.DbRow dbRow2 = this._parsedData.Get(index2);
        index1 = !dbRow2.IsSimpleValue ? index2 - 12 * (dbRow2.NumberOfRows + 1) : index2 - 12;
        dbRow2 = this._parsedData.Get(index1);
        if (dbRow2.SizeOrLength >= length)
        {
          byte[] numArray = ArrayPool<byte>.Shared.Rent(maxByteCount);
          Span<byte> propertyName2 = new Span<byte>();
          try
          {
            int utf8FromText = JsonReaderHelper.GetUtf8FromText(propertyName, (Span<byte>) numArray);
            propertyName2 = numArray.AsSpan<byte>(0, utf8FromText);
            return this.TryGetNamedPropertyValue(startIndex, num + 12, (ReadOnlySpan<byte>) propertyName2, out value);
          }
          finally
          {
            propertyName2.Clear();
            ArrayPool<byte>.Shared.Return(numArray);
          }
        }
      }
      value = new JsonElement();
      return false;
    }

    internal bool TryGetNamedPropertyValue(
      int index,
      ReadOnlySpan<byte> propertyName,
      out JsonElement value)
    {
      this.CheckNotDisposed();
      JsonDocument.DbRow dbRow = this._parsedData.Get(index);
      this.CheckExpectedType(JsonTokenType.StartObject, dbRow.TokenType);
      if (dbRow.NumberOfRows == 1)
      {
        value = new JsonElement();
        return false;
      }
      int endIndex = checked (dbRow.NumberOfRows * 12 + index);
      return this.TryGetNamedPropertyValue(index + 12, endIndex, propertyName, out value);
    }

    private unsafe bool TryGetNamedPropertyValue(
      int startIndex,
      int endIndex,
      ReadOnlySpan<byte> propertyName,
      out JsonElement value)
    {
      ReadOnlySpan<byte> span1 = this._utf8Json.Span;
      // ISSUE: untyped stack allocation
      Span<byte> span2 = new Span<byte>((void*) __untypedstackalloc(new IntPtr(256)), 256);
      int index1;
      for (int index2 = endIndex - 12; index2 > startIndex; index2 = index1 - 12)
      {
        JsonDocument.DbRow dbRow = this._parsedData.Get(index2);
        index1 = !dbRow.IsSimpleValue ? index2 - 12 * (dbRow.NumberOfRows + 1) : index2 - 12;
        dbRow = this._parsedData.Get(index1);
        ReadOnlySpan<byte> span3 = span1.Slice(dbRow.Location, dbRow.SizeOrLength);
        if (dbRow.HasComplexChildren)
        {
          if (span3.Length > propertyName.Length)
          {
            int num = span3.IndexOf<byte>((byte) 92);
            if (propertyName.Length > num && span3.Slice(0, num).SequenceEqual<byte>(propertyName.Slice(0, num)))
            {
              int minimumLength = span3.Length - num;
              int written = 0;
              byte[] array = (byte[]) null;
              try
              {
                Span<byte> destination = minimumLength <= span2.Length ? span2 : (Span<byte>) (array = ArrayPool<byte>.Shared.Rent(minimumLength));
                JsonReaderHelper.Unescape(span3.Slice(num), destination, 0, out written);
                if (destination.Slice(0, written).SequenceEqual<byte>(propertyName.Slice(num)))
                {
                  value = new JsonElement(this, index1 + 12);
                  return true;
                }
              }
              finally
              {
                if (array != null)
                {
                  array.AsSpan<byte>(0, written).Clear();
                  ArrayPool<byte>.Shared.Return(array);
                }
              }
            }
          }
        }
        else if (span3.SequenceEqual<byte>(propertyName))
        {
          value = new JsonElement(this, index1 + 12);
          return true;
        }
      }
      value = new JsonElement();
      return false;
    }

    internal readonly struct DbRow
    {
      private readonly int _location;
      private readonly int _sizeOrLengthUnion;
      private readonly int _numberOfRowsAndTypeUnion;

      internal int Location => this._location;

      internal int SizeOrLength => this._sizeOrLengthUnion & int.MaxValue;

      internal bool IsUnknownSize => this._sizeOrLengthUnion == -1;

      internal bool HasComplexChildren => this._sizeOrLengthUnion < 0;

      internal int NumberOfRows => this._numberOfRowsAndTypeUnion & 268435455;

      internal JsonTokenType TokenType => (JsonTokenType) ((uint) this._numberOfRowsAndTypeUnion >> 28);

      internal DbRow(JsonTokenType jsonTokenType, int location, int sizeOrLength)
      {
        this._location = location;
        this._sizeOrLengthUnion = sizeOrLength;
        this._numberOfRowsAndTypeUnion = (int) jsonTokenType << 28;
      }

      internal bool IsSimpleValue => this.TokenType >= JsonTokenType.PropertyName;
    }

    private struct MetadataDb : IDisposable
    {
      private byte[] _data;
      private bool _convertToAlloc;
      private bool _isLocked;

      internal int Length { get; private set; }

      private MetadataDb(byte[] initialDb, bool isLocked, bool convertToAlloc)
      {
        this._data = initialDb;
        this._isLocked = isLocked;
        this._convertToAlloc = convertToAlloc;
        this.Length = 0;
      }

      internal MetadataDb(byte[] completeDb)
      {
        this._data = completeDb;
        this._isLocked = true;
        this._convertToAlloc = false;
        this.Length = completeDb.Length;
      }

      internal static JsonDocument.MetadataDb CreateRented(
        int payloadLength,
        bool convertToAlloc)
      {
        int minimumLength = payloadLength + 12;
        if (minimumLength > 1048576 && minimumLength <= 4194304)
          minimumLength = 1048576;
        return new JsonDocument.MetadataDb(ArrayPool<byte>.Shared.Rent(minimumLength), false, convertToAlloc);
      }

      internal static JsonDocument.MetadataDb CreateLocked(int payloadLength) => new JsonDocument.MetadataDb(new byte[payloadLength + 12], true, false);

      public void Dispose()
      {
        byte[] array = Interlocked.Exchange<byte[]>(ref this._data, (byte[]) null);
        if (array == null)
          return;
        ArrayPool<byte>.Shared.Return(array);
        this.Length = 0;
      }

      internal void CompleteAllocations()
      {
        if (this._isLocked)
          return;
        if (this._convertToAlloc)
        {
          byte[] data = this._data;
          this._data = this._data.AsSpan<byte>(0, this.Length).ToArray();
          this._isLocked = true;
          this._convertToAlloc = false;
          ArrayPool<byte>.Shared.Return(data);
        }
        else
        {
          if (this.Length > this._data.Length / 2)
            return;
          byte[] dst = ArrayPool<byte>.Shared.Rent(this.Length);
          byte[] array = dst;
          if (dst.Length < this._data.Length)
          {
            Buffer.BlockCopy((Array) this._data, 0, (Array) dst, 0, this.Length);
            array = this._data;
            this._data = dst;
          }
          ArrayPool<byte>.Shared.Return(array);
        }
      }

      internal void Append(JsonTokenType tokenType, int startLocation, int length)
      {
        if (this.Length >= this._data.Length - 12)
          this.Enlarge();
        JsonDocument.DbRow dbRow = new JsonDocument.DbRow(tokenType, startLocation, length);
        MemoryMarshal.Write<JsonDocument.DbRow>(this._data.AsSpan<byte>(this.Length), ref dbRow);
        this.Length += 12;
      }

      private void Enlarge()
      {
        byte[] data = this._data;
        this._data = ArrayPool<byte>.Shared.Rent(data.Length * 2);
        Buffer.BlockCopy((Array) data, 0, (Array) this._data, 0, data.Length);
        ArrayPool<byte>.Shared.Return(data);
      }

      internal void SetLength(int index, int length) => MemoryMarshal.Write<int>(this._data.AsSpan<byte>(index + 4), ref length);

      internal void SetNumberOfRows(int index, int numberOfRows)
      {
        Span<byte> span = this._data.AsSpan<byte>(index + 8);
        int num = MemoryMarshal.Read<int>((ReadOnlySpan<byte>) span) & -268435456 | numberOfRows;
        MemoryMarshal.Write<int>(span, ref num);
      }

      internal void SetHasComplexChildren(int index)
      {
        Span<byte> span = this._data.AsSpan<byte>(index + 4);
        int num = MemoryMarshal.Read<int>((ReadOnlySpan<byte>) span) | int.MinValue;
        MemoryMarshal.Write<int>(span, ref num);
      }

      internal int FindIndexOfFirstUnsetSizeOrLength(JsonTokenType lookupType) => this.FindOpenElement(lookupType);

      private int FindOpenElement(JsonTokenType lookupType)
      {
        Span<byte> span = this._data.AsSpan<byte>(0, this.Length);
        for (int start = this.Length - 12; start >= 0; start -= 12)
        {
          JsonDocument.DbRow dbRow = MemoryMarshal.Read<JsonDocument.DbRow>((ReadOnlySpan<byte>) span.Slice(start));
          if (dbRow.IsUnknownSize && dbRow.TokenType == lookupType)
            return start;
        }
        return -1;
      }

      internal JsonDocument.DbRow Get(int index) => MemoryMarshal.Read<JsonDocument.DbRow>((ReadOnlySpan<byte>) this._data.AsSpan<byte>(index));

      internal JsonTokenType GetJsonTokenType(int index) => (JsonTokenType) (MemoryMarshal.Read<uint>((ReadOnlySpan<byte>) this._data.AsSpan<byte>(index + 8)) >> 28);

      internal JsonDocument.MetadataDb CopySegment(int startIndex, int endIndex)
      {
        JsonDocument.DbRow dbRow = this.Get(startIndex);
        int length = endIndex - startIndex;
        byte[] numArray = new byte[length];
        this._data.AsSpan<byte>(startIndex, length).CopyTo((Span<byte>) numArray);
        Span<int> span = MemoryMarshal.Cast<byte, int>((Span<byte>) numArray);
        int num = span[0];
        if (dbRow.TokenType == JsonTokenType.String)
          --num;
        for (int index = (length - 12) / 4; index >= 0; index -= 3)
          span[index] -= num;
        return new JsonDocument.MetadataDb(numArray);
      }
    }

    private struct StackRow
    {
      internal int SizeOrLength;
      internal int NumberOfRows;

      internal StackRow(int sizeOrLength = 0, int numberOfRows = -1)
      {
        this.SizeOrLength = sizeOrLength;
        this.NumberOfRows = numberOfRows;
      }
    }

    private struct StackRowStack : IDisposable
    {
      private byte[] _rentedBuffer;
      private int _topOfStack;

      public StackRowStack(int initialSize)
      {
        this._rentedBuffer = ArrayPool<byte>.Shared.Rent(initialSize);
        this._topOfStack = this._rentedBuffer.Length;
      }

      public void Dispose()
      {
        byte[] rentedBuffer = this._rentedBuffer;
        this._rentedBuffer = (byte[]) null;
        this._topOfStack = 0;
        if (rentedBuffer == null)
          return;
        ArrayPool<byte>.Shared.Return(rentedBuffer);
      }

      internal void Push(JsonDocument.StackRow row)
      {
        if (this._topOfStack < 8)
          this.Enlarge();
        this._topOfStack -= 8;
        MemoryMarshal.Write<JsonDocument.StackRow>(this._rentedBuffer.AsSpan<byte>(this._topOfStack), ref row);
      }

      internal JsonDocument.StackRow Pop()
      {
        JsonDocument.StackRow stackRow = MemoryMarshal.Read<JsonDocument.StackRow>((ReadOnlySpan<byte>) this._rentedBuffer.AsSpan<byte>(this._topOfStack));
        this._topOfStack += 8;
        return stackRow;
      }

      private void Enlarge()
      {
        byte[] rentedBuffer = this._rentedBuffer;
        this._rentedBuffer = ArrayPool<byte>.Shared.Rent(rentedBuffer.Length * 2);
        Buffer.BlockCopy((Array) rentedBuffer, this._topOfStack, (Array) this._rentedBuffer, this._rentedBuffer.Length - rentedBuffer.Length + this._topOfStack, rentedBuffer.Length - this._topOfStack);
        this._topOfStack += this._rentedBuffer.Length - rentedBuffer.Length;
        ArrayPool<byte>.Shared.Return(rentedBuffer);
      }
    }
  }
}
