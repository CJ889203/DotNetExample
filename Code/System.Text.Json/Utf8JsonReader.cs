// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Utf8JsonReader
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Provides a high-performance API for forward-only, read-only access to UTF-8 encoded JSON text.</summary>
  [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
  public ref struct Utf8JsonReader
  {

    #nullable disable
    private ReadOnlySpan<byte> _buffer;
    private readonly bool _isFinalBlock;
    private readonly bool _isInputSequence;
    private long _lineNumber;
    private long _bytePositionInLine;
    private int _consumed;
    private bool _inObject;
    private bool _isNotPrimitive;
    private JsonTokenType _tokenType;
    private JsonTokenType _previousTokenType;
    private JsonReaderOptions _readerOptions;
    private BitStack _bitStack;
    private long _totalConsumed;
    private bool _isLastSegment;
    internal bool _stringHasEscaping;
    private readonly bool _isMultiSegment;
    private bool _trailingCommaBeforeComment;
    private SequencePosition _nextPosition;
    private SequencePosition _currentPosition;
    private readonly ReadOnlySequence<byte> _sequence;

    private bool IsLastSpan
    {
      get
      {
        if (!this._isFinalBlock)
          return false;
        return !this._isMultiSegment || this._isLastSegment;
      }
    }


    #nullable enable
    internal ReadOnlySequence<byte> OriginalSequence => this._sequence;

    internal ReadOnlySpan<byte> OriginalSpan => !this._sequence.IsEmpty ? new ReadOnlySpan<byte>() : this._buffer;

    /// <summary>Gets the raw value of the last processed token as a ReadOnlySpan&lt;byte&gt; slice of the input payload, if the token fits in a single segment or if the reader was constructed with a JSON payload contained in a ReadOnlySpan&lt;byte&gt;.</summary>
    /// <returns>A read-only span of bytes.</returns>
    public ReadOnlySpan<byte> ValueSpan { get; private set; }

    /// <summary>Gets the total number of bytes consumed so far by this instance of the <see cref="T:System.Text.Json.Utf8JsonReader" />.</summary>
    /// <returns>The total number of bytes consumed so far.</returns>
    public long BytesConsumed => this._totalConsumed + (long) this._consumed;

    /// <summary>Gets the index that the last processed JSON token starts at (within the given UTF-8 encoded input text), skipping any white space.</summary>
    /// <returns>The starting index of the last processed JSON token within the given UTF-8 encoded input text.</returns>
    public long TokenStartIndex { get; private set; }

    /// <summary>Gets the depth of the current token.</summary>
    /// <returns>The depth of the current token.</returns>
    public int CurrentDepth
    {
      get
      {
        int currentDepth = this._bitStack.CurrentDepth;
        if (this.TokenType == JsonTokenType.StartArray || this.TokenType == JsonTokenType.StartObject)
          --currentDepth;
        return currentDepth;
      }
    }

    internal bool IsInArray => !this._inObject;

    /// <summary>Gets the type of the last processed JSON token in the UTF-8 encoded JSON text.</summary>
    /// <returns>The type of the last processed JSON token.</returns>
    public JsonTokenType TokenType => this._tokenType;

    /// <summary>Gets a value that indicates which <c>Value</c> property to use to get the token value.</summary>
    /// <returns>
    /// <see langword="true" /> if <see cref="P:System.Text.Json.Utf8JsonReader.ValueSequence" /> should be used to get the token value; <see langword="false" /> if <see cref="P:System.Text.Json.Utf8JsonReader.ValueSpan" /> should be used instead.</returns>
    public bool HasValueSequence { get; private set; }

    /// <summary>Gets the mode of this instance of the <see cref="T:System.Text.Json.Utf8JsonReader" /> which indicates whether all the JSON data was provided or there is more data to come.</summary>
    /// <returns>
    /// <see langword="true" /> if the reader was constructed with the input span or sequence containing the entire JSON data to process; <see langword="false" /> if the reader was constructed with an input span or sequence that may contain partial JSON data with more data to follow.</returns>
    public bool IsFinalBlock => this._isFinalBlock;

    /// <summary>Gets the raw value of the last processed token as a ReadOnlySequence&lt;byte&gt; slice of the input payload, only if the token is contained within multiple segments.</summary>
    /// <returns>A byte read-only sequence.</returns>
    public ReadOnlySequence<byte> ValueSequence { get; private set; }

    /// <summary>Gets the current <see cref="T:System.SequencePosition" /> within the provided UTF-8 encoded input ReadOnlySequence&lt;byte&gt; or a default <see cref="T:System.SequencePosition" /> if the <see cref="T:System.Text.Json.Utf8JsonReader" /> struct was constructed with a ReadOnlySpan&lt;byte&gt;.</summary>
    /// <returns>The current <see cref="T:System.SequencePosition" /> within the provided UTF-8 encoded input ReadOnlySequence&lt;byte&gt; or a default <see cref="T:System.SequencePosition" /> if the <see cref="T:System.Text.Json.Utf8JsonReader" /> struct was constructed with a ReadOnlySpan&lt;byte&gt;.</returns>
    public SequencePosition Position => this._isInputSequence ? this._sequence.GetPosition((long) this._consumed, this._currentPosition) : new SequencePosition();

    /// <summary>Gets the current <see cref="T:System.Text.Json.Utf8JsonReader" /> state to pass to a <see cref="T:System.Text.Json.Utf8JsonReader" /> constructor with more data.</summary>
    /// <returns>The current reader state.</returns>
    public JsonReaderState CurrentState => new JsonReaderState()
    {
      _lineNumber = this._lineNumber,
      _bytePositionInLine = this._bytePositionInLine,
      _inObject = this._inObject,
      _isNotPrimitive = this._isNotPrimitive,
      _stringHasEscaping = this._stringHasEscaping,
      _trailingCommaBeforeComment = this._trailingCommaBeforeComment,
      _tokenType = this._tokenType,
      _previousTokenType = this._previousTokenType,
      _readerOptions = this._readerOptions,
      _bitStack = this._bitStack
    };

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Utf8JsonReader" /> structure that processes a read-only span of UTF-8 encoded text and indicates whether the input contains all the text to process.</summary>
    /// <param name="jsonData">The UTF-8 encoded JSON text to process.</param>
    /// <param name="isFinalBlock">
    /// <see langword="true" /> to indicate that the input sequence contains the entire data to process; <see langword="false" /> to indicate that the input span contains partial data with more data to follow.</param>
    /// <param name="state">An object that contains the reader state. If this is the first call to the constructor, pass the default state; otherwise, pass the value of the <see cref="P:System.Text.Json.Utf8JsonReader.CurrentState" /> property from the previous instance of the <see cref="T:System.Text.Json.Utf8JsonReader" />.</param>
    public Utf8JsonReader(ReadOnlySpan<byte> jsonData, bool isFinalBlock, JsonReaderState state)
    {
      this._buffer = jsonData;
      this._isFinalBlock = isFinalBlock;
      this._isInputSequence = false;
      this._lineNumber = state._lineNumber;
      this._bytePositionInLine = state._bytePositionInLine;
      this._inObject = state._inObject;
      this._isNotPrimitive = state._isNotPrimitive;
      this._stringHasEscaping = state._stringHasEscaping;
      this._trailingCommaBeforeComment = state._trailingCommaBeforeComment;
      this._tokenType = state._tokenType;
      this._previousTokenType = state._previousTokenType;
      this._readerOptions = state._readerOptions;
      if (this._readerOptions.MaxDepth == 0)
        this._readerOptions.MaxDepth = 64;
      this._bitStack = state._bitStack;
      this._consumed = 0;
      this.TokenStartIndex = 0L;
      this._totalConsumed = 0L;
      this._isLastSegment = this._isFinalBlock;
      this._isMultiSegment = false;
      this.ValueSpan = ReadOnlySpan<byte>.Empty;
      this._currentPosition = new SequencePosition();
      this._nextPosition = new SequencePosition();
      this._sequence = new ReadOnlySequence<byte>();
      this.HasValueSequence = false;
      this.ValueSequence = ReadOnlySequence<byte>.Empty;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Utf8JsonReader" /> structure that processes a read-only span of UTF-8 encoded text using the specified options.</summary>
    /// <param name="jsonData">The UTF-8 encoded JSON text to process.</param>
    /// <param name="options">Defines customized behavior of the <see cref="T:System.Text.Json.Utf8JsonReader" /> that differs from the JSON RFC (for example how to handle comments or maximum depth allowed when reading). By default, the <see cref="T:System.Text.Json.Utf8JsonReader" /> follows the JSON RFC strictly; comments within the JSON are invalid, and the maximum depth is 64.</param>
    public Utf8JsonReader(ReadOnlySpan<byte> jsonData, JsonReaderOptions options = default (JsonReaderOptions))
      : this(jsonData, true, new JsonReaderState(options))
    {
    }

    /// <summary>Reads the next JSON token from the input source.</summary>
    /// <exception cref="T:System.Text.Json.JsonException">An invalid JSON token according to the JSON RFC is encountered.
    /// 
    /// -or-
    /// 
    /// The current depth exceeds the recursive limit set by the maximum depth.</exception>
    /// <returns>
    /// <see langword="true" /> if the token was read successfully; otherwise, <see langword="false" />.</returns>
    public bool Read()
    {
      bool flag = this._isMultiSegment ? this.ReadMultiSegment() : this.ReadSingleSegment();
      if (!flag && this._isFinalBlock && this.TokenType == JsonTokenType.None)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedJsonTokens);
      return flag;
    }

    /// <summary>Skips the children of the current JSON token.</summary>
    /// <exception cref="T:System.InvalidOperationException">The reader was given partial data with more data to follow (that is, <see cref="P:System.Text.Json.Utf8JsonReader.IsFinalBlock" /> is <see langword="false" />).</exception>
    /// <exception cref="T:System.Text.Json.JsonException">An invalid JSON token was encountered while skipping, according to the JSON RFC.
    /// 
    /// -or-
    /// 
    /// The current depth exceeds the recursive limit set by the maximum depth.</exception>
    public void Skip()
    {
      if (!this._isFinalBlock)
        throw ThrowHelper.GetInvalidOperationException_CannotSkipOnPartial();
      this.SkipHelper();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SkipHelper()
    {
      if (this.TokenType == JsonTokenType.PropertyName)
        this.Read();
      if (this.TokenType != JsonTokenType.StartObject && this.TokenType != JsonTokenType.StartArray)
        return;
      int currentDepth = this.CurrentDepth;
      do
      {
        this.Read();
      }
      while (currentDepth < this.CurrentDepth);
    }

    /// <summary>Tries to skip the children of the current JSON token.</summary>
    /// <exception cref="T:System.Text.Json.JsonException">An invalid JSON token was encountered while skipping, according to the JSON RFC.
    /// 
    /// -or -
    /// 
    /// The current depth exceeds the recursive limit set by the maximum depth.</exception>
    /// <returns>
    /// <see langword="true" /> if there was enough data for the children to be skipped successfully; otherwise, <see langword="false" />.</returns>
    public bool TrySkip()
    {
      if (!this._isFinalBlock)
        return this.TrySkipHelper();
      this.SkipHelper();
      return true;
    }

    private bool TrySkipHelper()
    {
      Utf8JsonReader utf8JsonReader = this;
      if (this.TokenType != JsonTokenType.PropertyName || this.Read())
      {
        if (this.TokenType == JsonTokenType.StartObject || this.TokenType == JsonTokenType.StartArray)
        {
          int currentDepth = this.CurrentDepth;
          while (this.Read())
          {
            if (currentDepth >= this.CurrentDepth)
              goto label_5;
          }
          goto label_6;
        }
label_5:
        return true;
      }
label_6:
      this = utf8JsonReader;
      return false;
    }

    /// <summary>Compares the UTF-8 encoded text in a read-only byte span to the unescaped JSON token value in the source and returns a value that indicates whether they match.</summary>
    /// <param name="utf8Text">The UTF-8 encoded text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token is not a JSON string (that is, it is not <see cref="F:System.Text.Json.JsonTokenType.String" /> or <see cref="F:System.Text.Json.JsonTokenType.PropertyName" />).</exception>
    /// <returns>
    /// <see langword="true" /> if the JSON token value in the source matches the UTF-8 encoded lookup text; otherwise, <see langword="false" />.</returns>
    public bool ValueTextEquals(ReadOnlySpan<byte> utf8Text)
    {
      if (!Utf8JsonReader.IsTokenTypeString(this.TokenType))
        throw ThrowHelper.GetInvalidOperationException_ExpectedStringComparison(this.TokenType);
      return this.TextEqualsHelper(utf8Text);
    }

    /// <summary>Compares the string text to the unescaped JSON token value in the source and returns a value that indicates whether they match.</summary>
    /// <param name="text">The text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token is not a JSON string (that is, it is not <see cref="F:System.Text.Json.JsonTokenType.String" /> or <see cref="F:System.Text.Json.JsonTokenType.PropertyName" />).</exception>
    /// <returns>
    /// <see langword="true" /> if the JSON token value in the source matches the lookup text; otherwise, <see langword="false" />.</returns>
    public bool ValueTextEquals(string? text) => this.ValueTextEquals(text.AsSpan());


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TextEqualsHelper(ReadOnlySpan<byte> otherUtf8Text)
    {
      if (this.HasValueSequence)
        return this.CompareToSequence(otherUtf8Text);
      return this._stringHasEscaping ? this.UnescapeAndCompare(otherUtf8Text) : otherUtf8Text.SequenceEqual<byte>(this.ValueSpan);
    }


    #nullable enable
    /// <summary>Compares the text in a read-only character span to the unescaped JSON token value in the source and returns a value that indicates whether they match.</summary>
    /// <param name="text">The text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token is not a JSON string (that is, it is not <see cref="F:System.Text.Json.JsonTokenType.String" /> or <see cref="F:System.Text.Json.JsonTokenType.PropertyName" />).</exception>
    /// <returns>
    /// <see langword="true" /> if the JSON token value in the source matches the lookup text; otherwise, <see langword="false" />.</returns>
    public unsafe bool ValueTextEquals(ReadOnlySpan<char> text)
    {
      if (!Utf8JsonReader.IsTokenTypeString(this.TokenType))
        throw ThrowHelper.GetInvalidOperationException_ExpectedStringComparison(this.TokenType);
      if (this.MatchNotPossible(text.Length))
        return false;
      byte[] array = (byte[]) null;
      int minimumLength = checked (text.Length * 3);
      Span<byte> utf8Destination;
      if (minimumLength > 256)
      {
        array = ArrayPool<byte>.Shared.Rent(minimumLength);
        utf8Destination = (Span<byte>) array;
      }
      else
      {
        byte* pointer = stackalloc byte[256];
        utf8Destination = new Span<byte>((void*) pointer, 256);
      }
      int bytesWritten;
      bool flag = JsonWriterHelper.ToUtf8(MemoryMarshal.AsBytes<char>(text), utf8Destination, out int _, out bytesWritten) <= OperationStatus.DestinationTooSmall && this.TextEqualsHelper((ReadOnlySpan<byte>) utf8Destination.Slice(0, bytesWritten));
      if (array != null)
      {
        utf8Destination.Slice(0, bytesWritten).Clear();
        ArrayPool<byte>.Shared.Return(array);
      }
      return flag;
    }


    #nullable disable
    private bool CompareToSequence(ReadOnlySpan<byte> other)
    {
      if (this._stringHasEscaping)
        return this.UnescapeSequenceAndCompare(other);
      ReadOnlySequence<byte> valueSequence = this.ValueSequence;
      if (valueSequence.Length != (long) other.Length)
        return false;
      int start = 0;
      foreach (ReadOnlyMemory<byte> readOnlyMemory in valueSequence)
      {
        ReadOnlySpan<byte> span = readOnlyMemory.Span;
        if (!other.Slice(start).StartsWith<byte>(span))
          return false;
        start += span.Length;
      }
      return true;
    }

    private bool UnescapeAndCompare(ReadOnlySpan<byte> other)
    {
      ReadOnlySpan<byte> valueSpan = this.ValueSpan;
      if (valueSpan.Length < other.Length || valueSpan.Length / 6 > other.Length)
        return false;
      int num = valueSpan.IndexOf<byte>((byte) 92);
      return other.StartsWith<byte>(valueSpan.Slice(0, num)) && JsonReaderHelper.UnescapeAndCompare(valueSpan.Slice(num), other.Slice(num));
    }

    private bool UnescapeSequenceAndCompare(ReadOnlySpan<byte> other)
    {
      ReadOnlySequence<byte> valueSequence = this.ValueSequence;
      long length1 = valueSequence.Length;
      if (length1 < (long) other.Length || length1 / 6L > (long) other.Length)
        return false;
      int start1 = 0;
      bool flag = false;
      foreach (ReadOnlyMemory<byte> readOnlyMemory in valueSequence)
      {
        ReadOnlySpan<byte> span = readOnlyMemory.Span;
        int length2 = span.IndexOf<byte>((byte) 92);
        if (length2 != -1)
        {
          if (other.Slice(start1).StartsWith<byte>(span.Slice(0, length2)))
          {
            int start2 = start1 + length2;
            other = other.Slice(start2);
            ReadOnlySequence<byte> utf8Source = valueSequence.Slice((long) start2);
            flag = !utf8Source.IsSingleSegment ? JsonReaderHelper.UnescapeAndCompare(utf8Source, other) : JsonReaderHelper.UnescapeAndCompare(utf8Source.First.Span, other);
            break;
          }
          break;
        }
        if (other.Slice(start1).StartsWith<byte>(span))
          start1 += span.Length;
        else
          break;
      }
      return flag;
    }

    private static bool IsTokenTypeString(JsonTokenType tokenType) => tokenType == JsonTokenType.PropertyName || tokenType == JsonTokenType.String;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool MatchNotPossible(int charTextLength)
    {
      if (this.HasValueSequence)
        return this.MatchNotPossibleSequence(charTextLength);
      int length = this.ValueSpan.Length;
      return length < charTextLength || length / (this._stringHasEscaping ? 6 : 3) > charTextLength;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private bool MatchNotPossibleSequence(int charTextLength)
    {
      long length = this.ValueSequence.Length;
      return length < (long) charTextLength || length / (this._stringHasEscaping ? 6L : 3L) > (long) charTextLength;
    }

    private void StartObject()
    {
      if (this._bitStack.CurrentDepth >= this._readerOptions.MaxDepth)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ObjectDepthTooLarge);
      this._bitStack.PushTrue();
      this.ValueSpan = this._buffer.Slice(this._consumed, 1);
      ++this._consumed;
      ++this._bytePositionInLine;
      this._tokenType = JsonTokenType.StartObject;
      this._inObject = true;
    }

    private void EndObject()
    {
      if (!this._inObject || this._bitStack.CurrentDepth <= 0)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.MismatchedObjectArray, (byte) 125);
      if (this._trailingCommaBeforeComment)
      {
        if (!this._readerOptions.AllowTrailingCommas)
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd);
        this._trailingCommaBeforeComment = false;
      }
      this._tokenType = JsonTokenType.EndObject;
      this.ValueSpan = this._buffer.Slice(this._consumed, 1);
      this.UpdateBitStackOnEndToken();
    }

    private void StartArray()
    {
      if (this._bitStack.CurrentDepth >= this._readerOptions.MaxDepth)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ArrayDepthTooLarge);
      this._bitStack.PushFalse();
      this.ValueSpan = this._buffer.Slice(this._consumed, 1);
      ++this._consumed;
      ++this._bytePositionInLine;
      this._tokenType = JsonTokenType.StartArray;
      this._inObject = false;
    }

    private void EndArray()
    {
      if (this._inObject || this._bitStack.CurrentDepth <= 0)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.MismatchedObjectArray, (byte) 93);
      if (this._trailingCommaBeforeComment)
      {
        if (!this._readerOptions.AllowTrailingCommas)
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd);
        this._trailingCommaBeforeComment = false;
      }
      this._tokenType = JsonTokenType.EndArray;
      this.ValueSpan = this._buffer.Slice(this._consumed, 1);
      this.UpdateBitStackOnEndToken();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateBitStackOnEndToken()
    {
      ++this._consumed;
      ++this._bytePositionInLine;
      this._inObject = this._bitStack.Pop();
    }

    private bool ReadSingleSegment()
    {
      bool flag = false;
      this.ValueSpan = new ReadOnlySpan<byte>();
      if (this.HasMoreData())
      {
        byte num = this._buffer[this._consumed];
        if (num <= (byte) 32)
        {
          this.SkipWhiteSpace();
          if (this.HasMoreData())
            num = this._buffer[this._consumed];
          else
            goto label_19;
        }
        this.TokenStartIndex = (long) this._consumed;
        if (this._tokenType != JsonTokenType.None)
        {
          if (num == (byte) 47)
          {
            flag = this.ConsumeNextTokenOrRollback(num);
          }
          else
          {
            if (this._tokenType == JsonTokenType.StartObject)
            {
              switch (num)
              {
                case 34:
                  int consumed = this._consumed;
                  long bytePositionInLine = this._bytePositionInLine;
                  long lineNumber = this._lineNumber;
                  flag = this.ConsumePropertyName();
                  if (!flag)
                  {
                    this._consumed = consumed;
                    this._tokenType = JsonTokenType.StartObject;
                    this._bytePositionInLine = bytePositionInLine;
                    this._lineNumber = lineNumber;
                    goto label_19;
                  }
                  else
                    goto label_19;
                case 125:
                  this.EndObject();
                  break;
                default:
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num);
                  goto case 34;
              }
            }
            else if (this._tokenType == JsonTokenType.StartArray)
            {
              if (num == (byte) 93)
              {
                this.EndArray();
              }
              else
              {
                flag = this.ConsumeValue(num);
                goto label_19;
              }
            }
            else
            {
              flag = this._tokenType != JsonTokenType.PropertyName ? this.ConsumeNextTokenOrRollback(num) : this.ConsumeValue(num);
              goto label_19;
            }
            flag = true;
          }
        }
        else
          flag = this.ReadFirstToken(num);
      }
label_19:
      return flag;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool HasMoreData()
    {
      if ((long) this._consumed < (long) (uint) this._buffer.Length)
        return true;
      if (this._isNotPrimitive && this.IsLastSpan)
      {
        if (this._bitStack.CurrentDepth != 0)
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ZeroDepthAtEnd);
        if (this._readerOptions.CommentHandling == JsonCommentHandling.Allow && this._tokenType == JsonTokenType.Comment || this._tokenType == JsonTokenType.EndArray || this._tokenType == JsonTokenType.EndObject)
          return false;
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJsonNonPrimitive);
      }
      return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool HasMoreData(ExceptionResource resource)
    {
      if ((long) this._consumed < (long) (uint) this._buffer.Length)
        return true;
      if (this.IsLastSpan)
        ThrowHelper.ThrowJsonReaderException(ref this, resource);
      return false;
    }

    private bool ReadFirstToken(byte first)
    {
      switch (first)
      {
        case 91:
          this._bitStack.ResetFirstBit();
          this._tokenType = JsonTokenType.StartArray;
          this.ValueSpan = this._buffer.Slice(this._consumed, 1);
          ++this._consumed;
          ++this._bytePositionInLine;
          this._isNotPrimitive = true;
          break;
        case 123:
          this._bitStack.SetFirstBit();
          this._tokenType = JsonTokenType.StartObject;
          this.ValueSpan = this._buffer.Slice(this._consumed, 1);
          ++this._consumed;
          ++this._bytePositionInLine;
          this._inObject = true;
          this._isNotPrimitive = true;
          break;
        default:
          ReadOnlySpan<byte> buffer = this._buffer;
          if (JsonHelpers.IsDigit(first) || first == (byte) 45)
          {
            int consumed;
            if (!this.TryGetNumber(buffer.Slice(this._consumed), out consumed))
              return false;
            this._tokenType = JsonTokenType.Number;
            this._consumed += consumed;
            this._bytePositionInLine += (long) consumed;
            return true;
          }
          if (!this.ConsumeValue(first))
            return false;
          if (this._tokenType == JsonTokenType.StartObject || this._tokenType == JsonTokenType.StartArray)
          {
            this._isNotPrimitive = true;
            break;
          }
          break;
      }
      return true;
    }

    private void SkipWhiteSpace()
    {
      for (ReadOnlySpan<byte> buffer = this._buffer; this._consumed < buffer.Length; ++this._consumed)
      {
        byte num = buffer[this._consumed];
        switch (num)
        {
          case 9:
          case 10:
          case 13:
          case 32:
            if (num == (byte) 10)
            {
              ++this._lineNumber;
              this._bytePositionInLine = 0L;
              continue;
            }
            ++this._bytePositionInLine;
            continue;
          default:
            return;
        }
      }
    }

    private bool ConsumeValue(byte marker)
    {
      while (true)
      {
        this._trailingCommaBeforeComment = false;
        switch (marker)
        {
          case 34:
            goto label_1;
          case 91:
            goto label_3;
          case 123:
            goto label_2;
          default:
            if (!JsonHelpers.IsDigit(marker))
            {
              switch (marker)
              {
                case 45:
                  goto label_6;
                case 102:
                  goto label_7;
                case 110:
                  goto label_9;
                case 116:
                  goto label_8;
                default:
                  switch (this._readerOptions.CommentHandling)
                  {
                    case JsonCommentHandling.Disallow:
                      goto label_25;
                    case JsonCommentHandling.Allow:
                      goto label_11;
                    default:
                      if (marker == (byte) 47)
                      {
                        if (this.SkipComment())
                        {
                          if ((long) this._consumed < (long) (uint) this._buffer.Length)
                          {
                            marker = this._buffer[this._consumed];
                            if (marker <= (byte) 32)
                            {
                              this.SkipWhiteSpace();
                              if (this.HasMoreData())
                                marker = this._buffer[this._consumed];
                              else
                                goto label_21;
                            }
                            this.TokenStartIndex = (long) this._consumed;
                            continue;
                          }
                          goto label_16;
                        }
                        else
                          goto label_24;
                      }
                      else
                        goto label_25;
                  }
              }
            }
            else
              goto label_6;
        }
      }
label_1:
      return this.ConsumeString();
label_2:
      this.StartObject();
      goto label_26;
label_3:
      this.StartArray();
      goto label_26;
label_6:
      return this.ConsumeNumber();
label_7:
      return this.ConsumeLiteral(JsonConstants.FalseValue, JsonTokenType.False);
label_8:
      return this.ConsumeLiteral(JsonConstants.TrueValue, JsonTokenType.True);
label_9:
      return this.ConsumeLiteral(JsonConstants.NullValue, JsonTokenType.Null);
label_11:
      if (marker == (byte) 47)
        return this.ConsumeComment();
      goto label_25;
label_16:
      if (this._isNotPrimitive && this.IsLastSpan && this._tokenType != JsonTokenType.EndArray && this._tokenType != JsonTokenType.EndObject)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJsonNonPrimitive);
      return false;
label_21:
      return false;
label_24:
      return false;
label_25:
      ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
label_26:
      return true;
    }

    private bool ConsumeLiteral(ReadOnlySpan<byte> literal, JsonTokenType tokenType)
    {
      ReadOnlySpan<byte> span = this._buffer.Slice(this._consumed);
      if (!span.StartsWith<byte>(literal))
        return this.CheckLiteral(span, literal);
      this.ValueSpan = span.Slice(0, literal.Length);
      this._tokenType = tokenType;
      this._consumed += literal.Length;
      this._bytePositionInLine += (long) literal.Length;
      return true;
    }

    private bool CheckLiteral(ReadOnlySpan<byte> span, ReadOnlySpan<byte> literal)
    {
      int num = 0;
      for (int index = 1; index < literal.Length; ++index)
      {
        if (span.Length > index)
        {
          if ((int) span[index] != (int) literal[index])
          {
            this._bytePositionInLine += (long) index;
            this.ThrowInvalidLiteral(span);
          }
        }
        else
        {
          num = index;
          break;
        }
      }
      if (this.IsLastSpan)
      {
        this._bytePositionInLine += (long) num;
        this.ThrowInvalidLiteral(span);
      }
      return false;
    }

    private void ThrowInvalidLiteral(ReadOnlySpan<byte> span)
    {
      ExceptionResource resource;
      switch (span[0])
      {
        case 102:
          resource = ExceptionResource.ExpectedFalse;
          break;
        case 116:
          resource = ExceptionResource.ExpectedTrue;
          break;
        default:
          resource = ExceptionResource.ExpectedNull;
          break;
      }
      ThrowHelper.ThrowJsonReaderException(ref this, resource, bytes: span);
    }

    private bool ConsumeNumber()
    {
      int consumed;
      if (!this.TryGetNumber(this._buffer.Slice(this._consumed), out consumed))
        return false;
      this._tokenType = JsonTokenType.Number;
      this._consumed += consumed;
      this._bytePositionInLine += (long) consumed;
      if ((long) this._consumed >= (long) (uint) this._buffer.Length && this._isNotPrimitive)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, this._buffer[this._consumed - 1]);
      return true;
    }

    private bool ConsumePropertyName()
    {
      this._trailingCommaBeforeComment = false;
      if (!this.ConsumeString() || !this.HasMoreData(ExceptionResource.ExpectedValueAfterPropertyNameNotFound))
        return false;
      byte nextByte = this._buffer[this._consumed];
      if (nextByte <= (byte) 32)
      {
        this.SkipWhiteSpace();
        if (!this.HasMoreData(ExceptionResource.ExpectedValueAfterPropertyNameNotFound))
          return false;
        nextByte = this._buffer[this._consumed];
      }
      if (nextByte != (byte) 58)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedSeparatorAfterPropertyNameNotFound, nextByte);
      ++this._consumed;
      ++this._bytePositionInLine;
      this._tokenType = JsonTokenType.PropertyName;
      return true;
    }

    private bool ConsumeString()
    {
      ReadOnlySpan<byte> readOnlySpan = this._buffer.Slice(this._consumed + 1);
      int num = readOnlySpan.IndexOfQuoteOrAnyControlOrBackSlash();
      if (num >= 0)
      {
        if (readOnlySpan[num] != (byte) 34)
          return this.ConsumeStringAndValidate(readOnlySpan, num);
        this._bytePositionInLine += (long) (num + 2);
        this.ValueSpan = readOnlySpan.Slice(0, num);
        this._stringHasEscaping = false;
        this._tokenType = JsonTokenType.String;
        this._consumed += num + 2;
        return true;
      }
      if (this.IsLastSpan)
      {
        this._bytePositionInLine += (long) (readOnlySpan.Length + 1);
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
      }
      return false;
    }

    private bool ConsumeStringAndValidate(ReadOnlySpan<byte> data, int idx)
    {
      long bytePositionInLine = this._bytePositionInLine;
      long lineNumber = this._lineNumber;
      this._bytePositionInLine += (long) (idx + 1);
      bool flag = false;
      for (; idx < data.Length; ++idx)
      {
        byte nextByte = data[idx];
        switch (nextByte)
        {
          case 34:
            if (flag)
            {
              flag = false;
              break;
            }
            goto label_21;
          case 92:
            flag = !flag;
            break;
          default:
            if (flag)
            {
              if (JsonConstants.EscapableChars.IndexOf<byte>(nextByte) == -1)
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterAfterEscapeWithinString, nextByte);
              if (nextByte == (byte) 117)
              {
                ++this._bytePositionInLine;
                if (this.ValidateHexDigits(data, idx + 1))
                {
                  idx += 4;
                }
                else
                {
                  idx = data.Length;
                  goto label_17;
                }
              }
              flag = false;
              break;
            }
            if (nextByte < (byte) 32)
            {
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, nextByte);
              break;
            }
            break;
        }
        ++this._bytePositionInLine;
      }
label_17:
      if (idx >= data.Length)
      {
        if (this.IsLastSpan)
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
        this._lineNumber = lineNumber;
        this._bytePositionInLine = bytePositionInLine;
        return false;
      }
label_21:
      ++this._bytePositionInLine;
      this.ValueSpan = data.Slice(0, idx);
      this._stringHasEscaping = true;
      this._tokenType = JsonTokenType.String;
      this._consumed += idx + 2;
      return true;
    }

    private bool ValidateHexDigits(ReadOnlySpan<byte> data, int idx)
    {
      for (int index = idx; index < data.Length; ++index)
      {
        byte nextByte = data[index];
        if (!JsonReaderHelper.IsHexDigit(nextByte))
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidHexCharacterWithinString, nextByte);
        if (index - idx >= 3)
          return true;
        ++this._bytePositionInLine;
      }
      return false;
    }

    private bool TryGetNumber(ReadOnlySpan<byte> data, out int consumed)
    {
      consumed = 0;
      int i = 0;
      if (this.ConsumeNegativeSign(ref data, ref i) == ConsumeNumberResult.NeedMoreData)
        return false;
      byte nextByte1;
      if (data[i] == (byte) 48)
      {
        switch (this.ConsumeZero(ref data, ref i))
        {
          case ConsumeNumberResult.Success:
            goto label_20;
          case ConsumeNumberResult.NeedMoreData:
            return false;
          default:
            nextByte1 = data[i];
            break;
        }
      }
      else
      {
        ++i;
        switch (this.ConsumeIntegerDigits(ref data, ref i))
        {
          case ConsumeNumberResult.Success:
            goto label_20;
          case ConsumeNumberResult.NeedMoreData:
            return false;
          default:
            nextByte1 = data[i];
            switch (nextByte1)
            {
              case 46:
              case 69:
              case 101:
                break;
              default:
                this._bytePositionInLine += (long) i;
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, nextByte1);
                break;
            }
            break;
        }
      }
      if (nextByte1 == (byte) 46)
      {
        ++i;
        switch (this.ConsumeDecimalDigits(ref data, ref i))
        {
          case ConsumeNumberResult.Success:
            goto label_20;
          case ConsumeNumberResult.NeedMoreData:
            return false;
          default:
            byte nextByte2 = data[i];
            switch (nextByte2)
            {
              case 69:
              case 101:
                break;
              default:
                this._bytePositionInLine += (long) i;
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitEValueNotFound, nextByte2);
                break;
            }
            break;
        }
      }
      ++i;
      if (this.ConsumeSign(ref data, ref i) == ConsumeNumberResult.NeedMoreData)
        return false;
      ++i;
      switch (this.ConsumeIntegerDigits(ref data, ref i))
      {
        case ConsumeNumberResult.Success:
          break;
        case ConsumeNumberResult.NeedMoreData:
          return false;
        default:
          this._bytePositionInLine += (long) i;
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, data[i]);
          break;
      }
label_20:
      this.ValueSpan = data.Slice(0, i);
      consumed = i;
      return true;
    }

    private ConsumeNumberResult ConsumeNegativeSign(
      ref ReadOnlySpan<byte> data,
      ref int i)
    {
      if (data[i] == (byte) 45)
      {
        ++i;
        if (i >= data.Length)
        {
          if (this.IsLastSpan)
          {
            this._bytePositionInLine += (long) i;
            ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
          }
          return ConsumeNumberResult.NeedMoreData;
        }
        byte nextByte = data[i];
        if (!JsonHelpers.IsDigit(nextByte))
        {
          this._bytePositionInLine += (long) i;
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundAfterSign, nextByte);
        }
      }
      return ConsumeNumberResult.OperationIncomplete;
    }

    private ConsumeNumberResult ConsumeZero(
      ref ReadOnlySpan<byte> data,
      ref int i)
    {
      ++i;
      if (i < data.Length)
      {
        if (JsonConstants.Delimiters.IndexOf<byte>(data[i]) >= 0)
          return ConsumeNumberResult.Success;
        byte nextByte = data[i];
        switch (nextByte)
        {
          case 46:
          case 69:
          case 101:
            return ConsumeNumberResult.OperationIncomplete;
          default:
            this._bytePositionInLine += (long) i;
            ThrowHelper.ThrowJsonReaderException(ref this, JsonHelpers.IsInRangeInclusive((int) nextByte, 48, 57) ? ExceptionResource.InvalidLeadingZeroInNumber : ExceptionResource.ExpectedEndOfDigitNotFound, nextByte);
            goto case 46;
        }
      }
      else
        return this.IsLastSpan ? ConsumeNumberResult.Success : ConsumeNumberResult.NeedMoreData;
    }

    private ConsumeNumberResult ConsumeIntegerDigits(
      ref ReadOnlySpan<byte> data,
      ref int i)
    {
      byte num = 0;
      while (i < data.Length)
      {
        num = data[i];
        if (JsonHelpers.IsDigit(num))
          ++i;
        else
          break;
      }
      return i >= data.Length ? (this.IsLastSpan ? ConsumeNumberResult.Success : ConsumeNumberResult.NeedMoreData) : (JsonConstants.Delimiters.IndexOf<byte>(num) >= 0 ? ConsumeNumberResult.Success : ConsumeNumberResult.OperationIncomplete);
    }

    private ConsumeNumberResult ConsumeDecimalDigits(
      ref ReadOnlySpan<byte> data,
      ref int i)
    {
      if (i >= data.Length)
      {
        if (this.IsLastSpan)
        {
          this._bytePositionInLine += (long) i;
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
        }
        return ConsumeNumberResult.NeedMoreData;
      }
      byte nextByte = data[i];
      if (!JsonHelpers.IsDigit(nextByte))
      {
        this._bytePositionInLine += (long) i;
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundAfterDecimal, nextByte);
      }
      ++i;
      return this.ConsumeIntegerDigits(ref data, ref i);
    }

    private ConsumeNumberResult ConsumeSign(
      ref ReadOnlySpan<byte> data,
      ref int i)
    {
      if (i >= data.Length)
      {
        if (this.IsLastSpan)
        {
          this._bytePositionInLine += (long) i;
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
        }
        return ConsumeNumberResult.NeedMoreData;
      }
      byte nextByte = data[i];
      switch (nextByte)
      {
        case 43:
        case 45:
          ++i;
          if (i >= data.Length)
          {
            if (this.IsLastSpan)
            {
              this._bytePositionInLine += (long) i;
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
            }
            return ConsumeNumberResult.NeedMoreData;
          }
          nextByte = data[i];
          break;
      }
      if (!JsonHelpers.IsDigit(nextByte))
      {
        this._bytePositionInLine += (long) i;
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundAfterSign, nextByte);
      }
      return ConsumeNumberResult.OperationIncomplete;
    }

    private bool ConsumeNextTokenOrRollback(byte marker)
    {
      int consumed = this._consumed;
      long bytePositionInLine = this._bytePositionInLine;
      long lineNumber = this._lineNumber;
      JsonTokenType tokenType = this._tokenType;
      bool commaBeforeComment = this._trailingCommaBeforeComment;
      switch (this.ConsumeNextToken(marker))
      {
        case ConsumeTokenResult.Success:
          return true;
        case ConsumeTokenResult.NotEnoughDataRollBackState:
          this._consumed = consumed;
          this._tokenType = tokenType;
          this._bytePositionInLine = bytePositionInLine;
          this._lineNumber = lineNumber;
          this._trailingCommaBeforeComment = commaBeforeComment;
          break;
      }
      return false;
    }

    private ConsumeTokenResult ConsumeNextToken(byte marker)
    {
      if (this._readerOptions.CommentHandling != JsonCommentHandling.Disallow)
      {
        if (this._readerOptions.CommentHandling != JsonCommentHandling.Allow)
          return this.ConsumeNextTokenUntilAfterAllCommentsAreSkipped(marker);
        if (marker == (byte) 47)
          return !this.ConsumeComment() ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
        if (this._tokenType == JsonTokenType.Comment)
          return this.ConsumeNextTokenFromLastNonCommentToken();
      }
      if (this._bitStack.CurrentDepth == 0)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndAfterSingleJson, marker);
      switch (marker)
      {
        case 44:
          ++this._consumed;
          ++this._bytePositionInLine;
          if ((long) this._consumed >= (long) (uint) this._buffer.Length)
          {
            if (this.IsLastSpan)
            {
              --this._consumed;
              --this._bytePositionInLine;
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
            }
            return ConsumeTokenResult.NotEnoughDataRollBackState;
          }
          byte num = this._buffer[this._consumed];
          if (num <= (byte) 32)
          {
            this.SkipWhiteSpace();
            if (!this.HasMoreData(ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
              return ConsumeTokenResult.NotEnoughDataRollBackState;
            num = this._buffer[this._consumed];
          }
          this.TokenStartIndex = (long) this._consumed;
          if (this._readerOptions.CommentHandling == JsonCommentHandling.Allow && num == (byte) 47)
          {
            this._trailingCommaBeforeComment = true;
            return !this.ConsumeComment() ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
          }
          if (this._inObject)
          {
            switch (num)
            {
              case 34:
                return !this.ConsumePropertyName() ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
              case 125:
                if (this._readerOptions.AllowTrailingCommas)
                {
                  this.EndObject();
                  return ConsumeTokenResult.Success;
                }
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd);
                goto default;
              default:
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num);
                goto case 34;
            }
          }
          else
          {
            if (num == (byte) 93)
            {
              if (this._readerOptions.AllowTrailingCommas)
              {
                this.EndArray();
                return ConsumeTokenResult.Success;
              }
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd);
            }
            return !this.ConsumeValue(num) ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
          }
        case 93:
          this.EndArray();
          break;
        case 125:
          this.EndObject();
          break;
        default:
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.FoundInvalidCharacter, marker);
          break;
      }
      return ConsumeTokenResult.Success;
    }

    private ConsumeTokenResult ConsumeNextTokenFromLastNonCommentToken()
    {
      this._tokenType = !JsonReaderHelper.IsTokenTypePrimitive(this._previousTokenType) ? this._previousTokenType : (this._inObject ? JsonTokenType.StartObject : JsonTokenType.StartArray);
      if (this.HasMoreData())
      {
        byte num1 = this._buffer[this._consumed];
        if (num1 <= (byte) 32)
        {
          this.SkipWhiteSpace();
          if (this.HasMoreData())
            num1 = this._buffer[this._consumed];
          else
            goto label_48;
        }
        if (this._bitStack.CurrentDepth == 0 && this._tokenType != JsonTokenType.None)
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndAfterSingleJson, num1);
        this.TokenStartIndex = (long) this._consumed;
        switch (num1)
        {
          case 44:
            if (this._previousTokenType <= JsonTokenType.StartObject || this._previousTokenType == JsonTokenType.StartArray || this._trailingCommaBeforeComment)
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueAfterComment, num1);
            ++this._consumed;
            ++this._bytePositionInLine;
            if ((long) this._consumed >= (long) (uint) this._buffer.Length)
            {
              if (this.IsLastSpan)
              {
                --this._consumed;
                --this._bytePositionInLine;
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                goto label_48;
              }
              else
                goto label_48;
            }
            else
            {
              byte num2 = this._buffer[this._consumed];
              if (num2 <= (byte) 32)
              {
                this.SkipWhiteSpace();
                if (this.HasMoreData(ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
                  num2 = this._buffer[this._consumed];
                else
                  goto label_48;
              }
              this.TokenStartIndex = (long) this._consumed;
              if (num2 == (byte) 47)
              {
                this._trailingCommaBeforeComment = true;
                if (!this.ConsumeComment())
                  goto label_48;
                else
                  break;
              }
              else if (this._inObject)
              {
                switch (num2)
                {
                  case 34:
                    if (!this.ConsumePropertyName())
                      goto label_48;
                    else
                      break;
                  case 125:
                    if (this._readerOptions.AllowTrailingCommas)
                    {
                      this.EndObject();
                      break;
                    }
                    ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd);
                    goto default;
                  default:
                    ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num2);
                    goto case 34;
                }
              }
              else
              {
                if (num2 == (byte) 93)
                {
                  if (this._readerOptions.AllowTrailingCommas)
                  {
                    this.EndArray();
                    break;
                  }
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd);
                }
                if (!this.ConsumeValue(num2))
                  goto label_48;
                else
                  break;
              }
            }
            break;
          case 93:
            this.EndArray();
            break;
          case 125:
            this.EndObject();
            break;
          default:
            if (this._tokenType == JsonTokenType.None)
            {
              if (!this.ReadFirstToken(num1))
                goto label_48;
              else
                break;
            }
            else if (this._tokenType == JsonTokenType.StartObject)
            {
              if (num1 != (byte) 34)
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num1);
              int consumed = this._consumed;
              long bytePositionInLine = this._bytePositionInLine;
              long lineNumber = this._lineNumber;
              if (!this.ConsumePropertyName())
              {
                this._consumed = consumed;
                this._tokenType = JsonTokenType.StartObject;
                this._bytePositionInLine = bytePositionInLine;
                this._lineNumber = lineNumber;
                goto label_48;
              }
              else
                break;
            }
            else if (this._tokenType == JsonTokenType.StartArray)
            {
              if (this.ConsumeValue(num1))
                break;
              goto label_48;
            }
            else if (this._tokenType == JsonTokenType.PropertyName)
            {
              if (this.ConsumeValue(num1))
                break;
              goto label_48;
            }
            else if (this._inObject)
            {
              if (num1 != (byte) 34)
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num1);
              if (!this.ConsumePropertyName())
                goto label_48;
              else
                break;
            }
            else if (!this.ConsumeValue(num1))
              goto label_48;
            else
              break;
        }
        return ConsumeTokenResult.Success;
      }
label_48:
      return ConsumeTokenResult.NotEnoughDataRollBackState;
    }

    private bool SkipAllComments(ref byte marker)
    {
      while (marker == (byte) 47)
      {
        if (this.SkipComment() && this.HasMoreData())
        {
          marker = this._buffer[this._consumed];
          if (marker <= (byte) 32)
          {
            this.SkipWhiteSpace();
            if (this.HasMoreData())
            {
              marker = this._buffer[this._consumed];
              continue;
            }
          }
          else
            continue;
        }
        return false;
      }
      return true;
    }

    private bool SkipAllComments(ref byte marker, ExceptionResource resource)
    {
      while (marker == (byte) 47)
      {
        if (this.SkipComment() && this.HasMoreData(resource))
        {
          marker = this._buffer[this._consumed];
          if (marker <= (byte) 32)
          {
            this.SkipWhiteSpace();
            if (this.HasMoreData(resource))
            {
              marker = this._buffer[this._consumed];
              continue;
            }
          }
          else
            continue;
        }
        return false;
      }
      return true;
    }

    private ConsumeTokenResult ConsumeNextTokenUntilAfterAllCommentsAreSkipped(
      byte marker)
    {
      if (this.SkipAllComments(ref marker))
      {
        this.TokenStartIndex = (long) this._consumed;
        if (this._tokenType == JsonTokenType.StartObject)
        {
          switch (marker)
          {
            case 34:
              int consumed = this._consumed;
              long bytePositionInLine = this._bytePositionInLine;
              long lineNumber = this._lineNumber;
              if (!this.ConsumePropertyName())
              {
                this._consumed = consumed;
                this._tokenType = JsonTokenType.StartObject;
                this._bytePositionInLine = bytePositionInLine;
                this._lineNumber = lineNumber;
                goto label_45;
              }
              else
                break;
            case 125:
              this.EndObject();
              break;
            default:
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, marker);
              goto case 34;
          }
        }
        else if (this._tokenType == JsonTokenType.StartArray)
        {
          if (marker == (byte) 93)
            this.EndArray();
          else if (!this.ConsumeValue(marker))
            goto label_45;
        }
        else if (this._tokenType == JsonTokenType.PropertyName)
        {
          if (!this.ConsumeValue(marker))
            goto label_45;
        }
        else if (this._bitStack.CurrentDepth == 0)
        {
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndAfterSingleJson, marker);
        }
        else
        {
          switch (marker)
          {
            case 44:
              ++this._consumed;
              ++this._bytePositionInLine;
              if ((long) this._consumed >= (long) (uint) this._buffer.Length)
              {
                if (this.IsLastSpan)
                {
                  --this._consumed;
                  --this._bytePositionInLine;
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                }
                return ConsumeTokenResult.NotEnoughDataRollBackState;
              }
              marker = this._buffer[this._consumed];
              if (marker <= (byte) 32)
              {
                this.SkipWhiteSpace();
                if (!this.HasMoreData(ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
                  return ConsumeTokenResult.NotEnoughDataRollBackState;
                marker = this._buffer[this._consumed];
              }
              if (!this.SkipAllComments(ref marker, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
                return ConsumeTokenResult.NotEnoughDataRollBackState;
              this.TokenStartIndex = (long) this._consumed;
              if (this._inObject)
              {
                switch (marker)
                {
                  case 34:
                    return !this.ConsumePropertyName() ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
                  case 125:
                    if (this._readerOptions.AllowTrailingCommas)
                    {
                      this.EndObject();
                      break;
                    }
                    ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd);
                    goto default;
                  default:
                    ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, marker);
                    goto case 34;
                }
              }
              else
              {
                if (marker == (byte) 93)
                {
                  if (this._readerOptions.AllowTrailingCommas)
                  {
                    this.EndArray();
                    break;
                  }
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd);
                }
                return !this.ConsumeValue(marker) ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
              }
              break;
            case 93:
              this.EndArray();
              break;
            case 125:
              this.EndObject();
              break;
            default:
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.FoundInvalidCharacter, marker);
              break;
          }
        }
        return ConsumeTokenResult.Success;
      }
label_45:
      return ConsumeTokenResult.IncompleteNoRollBackNecessary;
    }

    private bool SkipComment()
    {
      ReadOnlySpan<byte> readOnlySpan = this._buffer.Slice(this._consumed + 1);
      int idx;
      if (readOnlySpan.Length > 0)
      {
        switch (readOnlySpan[0])
        {
          case 42:
            return this.SkipMultiLineComment(readOnlySpan.Slice(1), out idx);
          case 47:
            return this.SkipSingleLineComment(readOnlySpan.Slice(1), out idx);
          default:
            ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, (byte) 47);
            break;
        }
      }
      if (this.IsLastSpan)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, (byte) 47);
      return false;
    }

    private bool SkipSingleLineComment(ReadOnlySpan<byte> localBuffer, out int idx)
    {
      idx = this.FindLineSeparator(localBuffer);
      int num1;
      if (idx != -1)
      {
        int num2 = idx;
        if (localBuffer[idx] != (byte) 10)
        {
          if (idx < localBuffer.Length - 1)
          {
            if (localBuffer[idx + 1] == (byte) 10)
              ++num2;
          }
          else if (!this.IsLastSpan)
            return false;
        }
        num1 = num2 + 1;
        this._bytePositionInLine = 0L;
        ++this._lineNumber;
      }
      else
      {
        if (!this.IsLastSpan)
          return false;
        idx = localBuffer.Length;
        num1 = idx;
        this._bytePositionInLine += (long) (2 + localBuffer.Length);
      }
      this._consumed += 2 + num1;
      return true;
    }

    private int FindLineSeparator(ReadOnlySpan<byte> localBuffer)
    {
      int num = 0;
      int lineSeparator;
      while (true)
      {
        int index = localBuffer.IndexOfAny<byte>((byte) 10, (byte) 13, (byte) 226);
        if (index != -1)
        {
          lineSeparator = num + index;
          if (localBuffer[index] == (byte) 226)
          {
            num = lineSeparator + 1;
            localBuffer = localBuffer.Slice(index + 1);
            this.ThrowOnDangerousLineSeparator(localBuffer);
          }
          else
            goto label_4;
        }
        else
          break;
      }
      return -1;
label_4:
      return lineSeparator;
    }

    private void ThrowOnDangerousLineSeparator(ReadOnlySpan<byte> localBuffer)
    {
      if (localBuffer.Length < 2)
        return;
      byte num = localBuffer[1];
      if (localBuffer[0] != (byte) 128 || num != (byte) 168 && num != (byte) 169)
        return;
      ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfLineSeparator);
    }

    private bool SkipMultiLineComment(ReadOnlySpan<byte> localBuffer, out int idx)
    {
      idx = 0;
      int num1;
      while (true)
      {
        num1 = localBuffer.Slice(idx).IndexOf<byte>((byte) 47);
        switch (num1)
        {
          case -1:
            goto label_2;
          case 0:
            idx += num1 + 1;
            continue;
          default:
            if (localBuffer[num1 + idx - 1] != (byte) 42)
              goto case 0;
            else
              goto label_6;
        }
      }
label_2:
      if (this.IsLastSpan)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfCommentNotFound);
      return false;
label_6:
      idx += num1 - 1;
      this._consumed += 4 + idx;
      (int num2, int num3) = JsonReaderHelper.CountNewLines(localBuffer.Slice(0, idx));
      this._lineNumber += (long) num2;
      if (num3 != -1)
        this._bytePositionInLine = (long) (idx - num3 + 1);
      else
        this._bytePositionInLine += (long) (4 + idx);
      return true;
    }

    private bool ConsumeComment()
    {
      ReadOnlySpan<byte> readOnlySpan = this._buffer.Slice(this._consumed + 1);
      if (readOnlySpan.Length > 0)
      {
        byte nextByte = readOnlySpan[0];
        switch (nextByte)
        {
          case 42:
            return this.ConsumeMultiLineComment(readOnlySpan.Slice(1), this._consumed);
          case 47:
            return this.ConsumeSingleLineComment(readOnlySpan.Slice(1), this._consumed);
          default:
            ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterAtStartOfComment, nextByte);
            break;
        }
      }
      if (this.IsLastSpan)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfDataWhileReadingComment);
      return false;
    }

    private bool ConsumeSingleLineComment(ReadOnlySpan<byte> localBuffer, int previousConsumed)
    {
      int idx;
      if (!this.SkipSingleLineComment(localBuffer, out idx))
        return false;
      this.ValueSpan = this._buffer.Slice(previousConsumed + 2, idx);
      if (this._tokenType != JsonTokenType.Comment)
        this._previousTokenType = this._tokenType;
      this._tokenType = JsonTokenType.Comment;
      return true;
    }

    private bool ConsumeMultiLineComment(ReadOnlySpan<byte> localBuffer, int previousConsumed)
    {
      int idx;
      if (!this.SkipMultiLineComment(localBuffer, out idx))
        return false;
      this.ValueSpan = this._buffer.Slice(previousConsumed + 2, idx);
      if (this._tokenType != JsonTokenType.Comment)
        this._previousTokenType = this._tokenType;
      this._tokenType = JsonTokenType.Comment;
      return true;
    }


    #nullable enable
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
    {
      get
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 3);
        interpolatedStringHandler.AppendLiteral("TokenType = ");
        interpolatedStringHandler.AppendFormatted(this.DebugTokenType);
        interpolatedStringHandler.AppendLiteral(" (TokenStartIndex = ");
        interpolatedStringHandler.AppendFormatted<long>(this.TokenStartIndex);
        interpolatedStringHandler.AppendLiteral(") Consumed = ");
        interpolatedStringHandler.AppendFormatted<long>(this.BytesConsumed);
        return interpolatedStringHandler.ToStringAndClear();
      }
    }

    private string DebugTokenType
    {
      get
      {
        string debugTokenType;
        switch (this.TokenType)
        {
          case JsonTokenType.None:
            debugTokenType = "None";
            break;
          case JsonTokenType.StartObject:
            debugTokenType = "StartObject";
            break;
          case JsonTokenType.EndObject:
            debugTokenType = "EndObject";
            break;
          case JsonTokenType.StartArray:
            debugTokenType = "StartArray";
            break;
          case JsonTokenType.EndArray:
            debugTokenType = "EndArray";
            break;
          case JsonTokenType.PropertyName:
            debugTokenType = "PropertyName";
            break;
          case JsonTokenType.Comment:
            debugTokenType = "Comment";
            break;
          case JsonTokenType.String:
            debugTokenType = "String";
            break;
          case JsonTokenType.Number:
            debugTokenType = "Number";
            break;
          case JsonTokenType.True:
            debugTokenType = "True";
            break;
          case JsonTokenType.False:
            debugTokenType = "False";
            break;
          case JsonTokenType.Null:
            debugTokenType = "Null";
            break;
          default:
            debugTokenType = ((byte) this.TokenType).ToString();
            break;
        }
        return debugTokenType;
      }
    }


    #nullable disable
    private ReadOnlySpan<byte> GetUnescapedSpan()
    {
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> unescapedSpan = readOnlySpan;
      if (this._stringHasEscaping)
      {
        int idx = unescapedSpan.IndexOf<byte>((byte) 92);
        unescapedSpan = JsonReaderHelper.GetUnescapedSpan(unescapedSpan, idx);
      }
      return unescapedSpan;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Utf8JsonReader" /> structure that processes a read-only sequence of UTF-8 encoded text and indicates whether the input contains all the text to process.</summary>
    /// <param name="jsonData">The UTF-8 encoded JSON text to process.</param>
    /// <param name="isFinalBlock">
    /// <see langword="true" /> to indicate that the input sequence contains the entire data to process; <see langword="false" /> to indicate that the input span contains partial data with more data to follow.</param>
    /// <param name="state">An object that contains the reader state. If this is the first call to the constructor, pass the default state; otherwise, pass the value of the <see cref="P:System.Text.Json.Utf8JsonReader.CurrentState" /> property from the previous instance of the <see cref="T:System.Text.Json.Utf8JsonReader" />.</param>
    public Utf8JsonReader(
      ReadOnlySequence<byte> jsonData,
      bool isFinalBlock,
      JsonReaderState state)
    {
      ReadOnlyMemory<byte> memory1 = jsonData.First;
      this._buffer = memory1.Span;
      this._isFinalBlock = isFinalBlock;
      this._isInputSequence = true;
      this._lineNumber = state._lineNumber;
      this._bytePositionInLine = state._bytePositionInLine;
      this._inObject = state._inObject;
      this._isNotPrimitive = state._isNotPrimitive;
      this._stringHasEscaping = state._stringHasEscaping;
      this._trailingCommaBeforeComment = state._trailingCommaBeforeComment;
      this._tokenType = state._tokenType;
      this._previousTokenType = state._previousTokenType;
      this._readerOptions = state._readerOptions;
      if (this._readerOptions.MaxDepth == 0)
        this._readerOptions.MaxDepth = 64;
      this._bitStack = state._bitStack;
      this._consumed = 0;
      this.TokenStartIndex = 0L;
      this._totalConsumed = 0L;
      this.ValueSpan = ReadOnlySpan<byte>.Empty;
      this._sequence = jsonData;
      this.HasValueSequence = false;
      this.ValueSequence = ReadOnlySequence<byte>.Empty;
      if (jsonData.IsSingleSegment)
      {
        this._nextPosition = new SequencePosition();
        this._currentPosition = jsonData.Start;
        this._isLastSegment = isFinalBlock;
        this._isMultiSegment = false;
      }
      else
      {
        this._currentPosition = jsonData.Start;
        this._nextPosition = this._currentPosition;
        bool flag = this._buffer.Length == 0;
        if (flag)
        {
          SequencePosition nextPosition = this._nextPosition;
          ReadOnlyMemory<byte> memory2;
          while (jsonData.TryGet(ref this._nextPosition, out memory2))
          {
            this._currentPosition = nextPosition;
            if (memory2.Length != 0)
            {
              this._buffer = memory2.Span;
              break;
            }
            nextPosition = this._nextPosition;
          }
        }
        this._isLastSegment = !jsonData.TryGet(ref this._nextPosition, out memory1, !flag) & isFinalBlock;
        this._isMultiSegment = true;
      }
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Utf8JsonReader" /> structure that processes a read-only sequence of UTF-8 encoded text using the specified options.</summary>
    /// <param name="jsonData">The UTF-8 encoded JSON text to process.</param>
    /// <param name="options">Defines customized behavior of the <see cref="T:System.Text.Json.Utf8JsonReader" /> that differs from the JSON RFC (for example how to handle comments or maximum depth allowed when reading). By default, the <see cref="T:System.Text.Json.Utf8JsonReader" /> follows the JSON RFC strictly; comments within the JSON are invalid, and the maximum depth is 64.</param>
    public Utf8JsonReader(ReadOnlySequence<byte> jsonData, JsonReaderOptions options = default (JsonReaderOptions))
      : this(jsonData, true, new JsonReaderState(options))
    {
    }

    private bool ReadMultiSegment()
    {
      bool flag = false;
      this.HasValueSequence = false;
      this.ValueSpan = new ReadOnlySpan<byte>();
      this.ValueSequence = new ReadOnlySequence<byte>();
      if (this.HasMoreDataMultiSegment())
      {
        byte num = this._buffer[this._consumed];
        if (num <= (byte) 32)
        {
          this.SkipWhiteSpaceMultiSegment();
          if (this.HasMoreDataMultiSegment())
            num = this._buffer[this._consumed];
          else
            goto label_19;
        }
        this.TokenStartIndex = this.BytesConsumed;
        if (this._tokenType != JsonTokenType.None)
        {
          if (num == (byte) 47)
          {
            flag = this.ConsumeNextTokenOrRollbackMultiSegment(num);
          }
          else
          {
            if (this._tokenType == JsonTokenType.StartObject)
            {
              switch (num)
              {
                case 34:
                  long totalConsumed = this._totalConsumed;
                  int consumed = this._consumed;
                  long bytePositionInLine = this._bytePositionInLine;
                  long lineNumber = this._lineNumber;
                  SequencePosition currentPosition = this._currentPosition;
                  flag = this.ConsumePropertyNameMultiSegment();
                  if (!flag)
                  {
                    this._consumed = consumed;
                    this._tokenType = JsonTokenType.StartObject;
                    this._bytePositionInLine = bytePositionInLine;
                    this._lineNumber = lineNumber;
                    this._totalConsumed = totalConsumed;
                    this._currentPosition = currentPosition;
                    goto label_19;
                  }
                  else
                    goto label_19;
                case 125:
                  this.EndObject();
                  break;
                default:
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num);
                  goto case 34;
              }
            }
            else if (this._tokenType == JsonTokenType.StartArray)
            {
              if (num == (byte) 93)
              {
                this.EndArray();
              }
              else
              {
                flag = this.ConsumeValueMultiSegment(num);
                goto label_19;
              }
            }
            else
            {
              flag = this._tokenType != JsonTokenType.PropertyName ? this.ConsumeNextTokenOrRollbackMultiSegment(num) : this.ConsumeValueMultiSegment(num);
              goto label_19;
            }
            flag = true;
          }
        }
        else
          flag = this.ReadFirstTokenMultiSegment(num);
      }
label_19:
      return flag;
    }

    private bool ValidateStateAtEndOfData()
    {
      if (this._bitStack.CurrentDepth != 0)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ZeroDepthAtEnd);
      if (this._readerOptions.CommentHandling == JsonCommentHandling.Allow && this._tokenType == JsonTokenType.Comment)
        return false;
      if (this._tokenType != JsonTokenType.EndArray && this._tokenType != JsonTokenType.EndObject)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJsonNonPrimitive);
      return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool HasMoreDataMultiSegment()
    {
      if ((long) this._consumed >= (long) (uint) this._buffer.Length)
      {
        if (this._isNotPrimitive && this.IsLastSpan && !this.ValidateStateAtEndOfData())
          return false;
        if (!this.GetNextSpan())
        {
          if (this._isNotPrimitive && this.IsLastSpan)
            this.ValidateStateAtEndOfData();
          return false;
        }
      }
      return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool HasMoreDataMultiSegment(ExceptionResource resource)
    {
      if ((long) this._consumed >= (long) (uint) this._buffer.Length)
      {
        if (this.IsLastSpan)
          ThrowHelper.ThrowJsonReaderException(ref this, resource);
        if (!this.GetNextSpan())
        {
          if (this.IsLastSpan)
            ThrowHelper.ThrowJsonReaderException(ref this, resource);
          return false;
        }
      }
      return true;
    }

    private bool GetNextSpan()
    {
      ReadOnlyMemory<byte> memory = new ReadOnlyMemory<byte>();
      SequencePosition currentPosition;
      while (true)
      {
        currentPosition = this._currentPosition;
        this._currentPosition = this._nextPosition;
        if (this._sequence.TryGet(ref this._nextPosition, out memory))
        {
          if (memory.Length == 0)
            this._currentPosition = currentPosition;
          else
            goto label_5;
        }
        else
          break;
      }
      this._currentPosition = currentPosition;
      this._isLastSegment = true;
      return false;
label_5:
      if (this._isFinalBlock)
        this._isLastSegment = !this._sequence.TryGet(ref this._nextPosition, out ReadOnlyMemory<byte> _, false);
      this._buffer = memory.Span;
      this._totalConsumed += (long) this._consumed;
      this._consumed = 0;
      return true;
    }

    private bool ReadFirstTokenMultiSegment(byte first)
    {
      switch (first)
      {
        case 91:
          this._bitStack.ResetFirstBit();
          this._tokenType = JsonTokenType.StartArray;
          this.ValueSpan = this._buffer.Slice(this._consumed, 1);
          ++this._consumed;
          ++this._bytePositionInLine;
          this._isNotPrimitive = true;
          break;
        case 123:
          this._bitStack.SetFirstBit();
          this._tokenType = JsonTokenType.StartObject;
          this.ValueSpan = this._buffer.Slice(this._consumed, 1);
          ++this._consumed;
          ++this._bytePositionInLine;
          this._inObject = true;
          this._isNotPrimitive = true;
          break;
        default:
          if (JsonHelpers.IsDigit(first) || first == (byte) 45)
          {
            int consumed;
            if (!this.TryGetNumberMultiSegment(this._buffer.Slice(this._consumed), out consumed))
              return false;
            this._tokenType = JsonTokenType.Number;
            this._consumed += consumed;
            return true;
          }
          if (!this.ConsumeValueMultiSegment(first))
            return false;
          if (this._tokenType == JsonTokenType.StartObject || this._tokenType == JsonTokenType.StartArray)
          {
            this._isNotPrimitive = true;
            break;
          }
          break;
      }
      return true;
    }

    private void SkipWhiteSpaceMultiSegment()
    {
      do
      {
        this.SkipWhiteSpace();
      }
      while (this._consumed >= this._buffer.Length && this.GetNextSpan());
    }

    private bool ConsumeValueMultiSegment(byte marker)
    {
      SequencePosition currentPosition1;
      while (true)
      {
        this._trailingCommaBeforeComment = false;
        switch (marker)
        {
          case 34:
            goto label_1;
          case 91:
            goto label_3;
          case 123:
            goto label_2;
          default:
            if (!JsonHelpers.IsDigit(marker))
            {
              switch (marker)
              {
                case 45:
                  goto label_6;
                case 102:
                  goto label_7;
                case 110:
                  goto label_9;
                case 116:
                  goto label_8;
                default:
                  switch (this._readerOptions.CommentHandling)
                  {
                    case JsonCommentHandling.Disallow:
                      goto label_30;
                    case JsonCommentHandling.Allow:
                      goto label_11;
                    default:
                      if (marker == (byte) 47)
                      {
                        currentPosition1 = this._currentPosition;
                        if (this.SkipCommentMultiSegment(out int _))
                        {
                          if ((long) this._consumed >= (long) (uint) this._buffer.Length)
                          {
                            if (this._isNotPrimitive && this.IsLastSpan && this._tokenType != JsonTokenType.EndArray && this._tokenType != JsonTokenType.EndObject)
                              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJsonNonPrimitive);
                            if (!this.GetNextSpan())
                              goto label_21;
                          }
                          marker = this._buffer[this._consumed];
                          if (marker <= (byte) 32)
                          {
                            this.SkipWhiteSpaceMultiSegment();
                            if (this.HasMoreDataMultiSegment())
                              marker = this._buffer[this._consumed];
                            else
                              goto label_26;
                          }
                          this.TokenStartIndex = this.BytesConsumed;
                          continue;
                        }
                        goto label_29;
                      }
                      else
                        goto label_30;
                  }
              }
            }
            else
              goto label_6;
        }
      }
label_1:
      return this.ConsumeStringMultiSegment();
label_2:
      this.StartObject();
      goto label_31;
label_3:
      this.StartArray();
      goto label_31;
label_6:
      return this.ConsumeNumberMultiSegment();
label_7:
      return this.ConsumeLiteralMultiSegment(JsonConstants.FalseValue, JsonTokenType.False);
label_8:
      return this.ConsumeLiteralMultiSegment(JsonConstants.TrueValue, JsonTokenType.True);
label_9:
      return this.ConsumeLiteralMultiSegment(JsonConstants.NullValue, JsonTokenType.Null);
label_11:
      if (marker == (byte) 47)
      {
        SequencePosition currentPosition2 = this._currentPosition;
        if (this.SkipOrConsumeCommentMultiSegmentWithRollback())
          return true;
        this._currentPosition = currentPosition2;
        return false;
      }
      goto label_30;
label_21:
      if (this._isNotPrimitive && this.IsLastSpan && this._tokenType != JsonTokenType.EndArray && this._tokenType != JsonTokenType.EndObject)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidEndOfJsonNonPrimitive);
      this._currentPosition = currentPosition1;
      return false;
label_26:
      this._currentPosition = currentPosition1;
      return false;
label_29:
      this._currentPosition = currentPosition1;
      return false;
label_30:
      ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfValueNotFound, marker);
label_31:
      return true;
    }


    #nullable disable
    private bool ConsumeLiteralMultiSegment(ReadOnlySpan<byte> literal, JsonTokenType tokenType)
    {
      ReadOnlySpan<byte> span = this._buffer.Slice(this._consumed);
      int consumed1 = literal.Length;
      if (!span.StartsWith<byte>(literal))
      {
        int consumed2 = this._consumed;
        if (!this.CheckLiteralMultiSegment(span, literal, out consumed1))
        {
          this._consumed = consumed2;
          return false;
        }
      }
      else
      {
        this.ValueSpan = span.Slice(0, literal.Length);
        this.HasValueSequence = false;
      }
      this._tokenType = tokenType;
      this._consumed += consumed1;
      this._bytePositionInLine += (long) consumed1;
      return true;
    }

    private unsafe bool CheckLiteralMultiSegment(
      ReadOnlySpan<byte> span,
      ReadOnlySpan<byte> literal,
      out int consumed)
    {
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(5)), 5);
      int num1 = 0;
      long totalConsumed = this._totalConsumed;
      SequencePosition currentPosition1 = this._currentPosition;
      if (span.Length >= literal.Length || this.IsLastSpan)
      {
        this._bytePositionInLine += (long) this.FindMismatch(span, literal);
        int length = Math.Min(span.Length, (int) this._bytePositionInLine + 1);
        span.Slice(0, length).CopyTo(destination);
        num1 += length;
      }
      else if (!literal.StartsWith<byte>(span))
      {
        this._bytePositionInLine += (long) this.FindMismatch(span, literal);
        int length = Math.Min(span.Length, (int) this._bytePositionInLine + 1);
        span.Slice(0, length).CopyTo(destination);
        num1 += length;
      }
      else
      {
        ReadOnlySpan<byte> readOnlySpan = literal.Slice(span.Length);
        SequencePosition currentPosition2 = this._currentPosition;
        int consumed1 = this._consumed;
        int num2 = literal.Length - readOnlySpan.Length;
        while (true)
        {
          this._totalConsumed += (long) num2;
          this._bytePositionInLine += (long) num2;
          if (this.GetNextSpan())
          {
            int length = Math.Min(span.Length, destination.Length - num1);
            span.Slice(0, length).CopyTo(destination.Slice(num1));
            num1 += length;
            span = this._buffer;
            if (!span.StartsWith<byte>(readOnlySpan))
            {
              if (readOnlySpan.StartsWith<byte>(span))
              {
                readOnlySpan = readOnlySpan.Slice(span.Length);
                num2 = span.Length;
              }
              else
                goto label_11;
            }
            else
              goto label_9;
          }
          else
            break;
        }
        this._totalConsumed = totalConsumed;
        consumed = 0;
        this._currentPosition = currentPosition1;
        if (!this.IsLastSpan)
          return false;
        goto label_13;
label_9:
        this.HasValueSequence = true;
        this.ValueSequence = this._sequence.Slice(new SequencePosition(currentPosition2.GetObject(), currentPosition2.GetInteger() + consumed1), new SequencePosition(this._currentPosition.GetObject(), this._currentPosition.GetInteger() + readOnlySpan.Length));
        consumed = readOnlySpan.Length;
        return true;
label_11:
        this._bytePositionInLine += (long) this.FindMismatch(span, readOnlySpan);
        int length1 = Math.Min(span.Length, (int) this._bytePositionInLine + 1);
        span.Slice(0, length1).CopyTo(destination.Slice(num1));
        num1 += length1;
      }
label_13:
      this._totalConsumed = totalConsumed;
      consumed = 0;
      this._currentPosition = currentPosition1;
      throw this.GetInvalidLiteralMultiSegment((ReadOnlySpan<byte>) destination.Slice(0, num1).ToArray());
    }

    private int FindMismatch(ReadOnlySpan<byte> span, ReadOnlySpan<byte> literal)
    {
      int num = Math.Min(span.Length, literal.Length);
      int index = 0;
      while (index < num && (int) span[index] == (int) literal[index])
        ++index;
      return index;
    }

    private JsonException GetInvalidLiteralMultiSegment(ReadOnlySpan<byte> span)
    {
      ExceptionResource resource;
      switch (span[0])
      {
        case 102:
          resource = ExceptionResource.ExpectedFalse;
          break;
        case 116:
          resource = ExceptionResource.ExpectedTrue;
          break;
        default:
          resource = ExceptionResource.ExpectedNull;
          break;
      }
      return ThrowHelper.GetJsonReaderException(ref this, resource, (byte) 0, span);
    }

    private bool ConsumeNumberMultiSegment()
    {
      int consumed;
      if (!this.TryGetNumberMultiSegment(this._buffer.Slice(this._consumed), out consumed))
        return false;
      this._tokenType = JsonTokenType.Number;
      this._consumed += consumed;
      if ((long) this._consumed >= (long) (uint) this._buffer.Length && this._isNotPrimitive)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, this._buffer[this._consumed - 1]);
      return true;
    }

    private bool ConsumePropertyNameMultiSegment()
    {
      this._trailingCommaBeforeComment = false;
      if (!this.ConsumeStringMultiSegment() || !this.HasMoreDataMultiSegment(ExceptionResource.ExpectedValueAfterPropertyNameNotFound))
        return false;
      byte nextByte = this._buffer[this._consumed];
      if (nextByte <= (byte) 32)
      {
        this.SkipWhiteSpaceMultiSegment();
        if (!this.HasMoreDataMultiSegment(ExceptionResource.ExpectedValueAfterPropertyNameNotFound))
          return false;
        nextByte = this._buffer[this._consumed];
      }
      if (nextByte != (byte) 58)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedSeparatorAfterPropertyNameNotFound, nextByte);
      ++this._consumed;
      ++this._bytePositionInLine;
      this._tokenType = JsonTokenType.PropertyName;
      return true;
    }

    private bool ConsumeStringMultiSegment()
    {
      ReadOnlySpan<byte> readOnlySpan = this._buffer.Slice(this._consumed + 1);
      int num = readOnlySpan.IndexOfQuoteOrAnyControlOrBackSlash();
      if (num >= 0)
      {
        if (readOnlySpan[num] != (byte) 34)
          return this.ConsumeStringAndValidateMultiSegment(readOnlySpan, num);
        this._bytePositionInLine += (long) (num + 2);
        this.ValueSpan = readOnlySpan.Slice(0, num);
        this.HasValueSequence = false;
        this._stringHasEscaping = false;
        this._tokenType = JsonTokenType.String;
        this._consumed += num + 2;
        return true;
      }
      if (this.IsLastSpan)
      {
        this._bytePositionInLine += (long) (readOnlySpan.Length + 1);
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
      }
      return this.ConsumeStringNextSegment();
    }

    private bool ConsumeStringNextSegment()
    {
      Utf8JsonReader.PartialStateForRollback state = this.CaptureState();
      this.HasValueSequence = true;
      int num1 = this._buffer.Length - this._consumed;
      while (this.GetNextSpan())
      {
        ReadOnlySpan<byte> buffer = this._buffer;
        int index1 = buffer.IndexOfQuoteOrAnyControlOrBackSlash();
        if (index1 >= 0)
        {
          SequencePosition end;
          if (buffer[index1] == (byte) 34)
          {
            end = new SequencePosition(this._currentPosition.GetObject(), this._currentPosition.GetInteger() + index1);
            this._bytePositionInLine += (long) (num1 + index1 + 1);
            this._totalConsumed += (long) num1;
            this._consumed = index1 + 1;
            this._stringHasEscaping = false;
          }
          else
          {
            this._bytePositionInLine += (long) (num1 + index1);
            this._stringHasEscaping = true;
            bool flag = false;
label_33:
            while (true)
            {
              while (index1 < buffer.Length)
              {
                byte nextByte1 = buffer[index1];
                switch (nextByte1)
                {
                  case 34:
                    if (flag)
                    {
                      flag = false;
                      break;
                    }
                    ++this._bytePositionInLine;
                    this._consumed = index1 + 1;
                    this._totalConsumed += (long) num1;
                    end = new SequencePosition(this._currentPosition.GetObject(), this._currentPosition.GetInteger() + index1);
                    goto label_41;
                  case 92:
                    flag = !flag;
                    break;
                  default:
                    if (flag)
                    {
                      if (JsonConstants.EscapableChars.IndexOf<byte>(nextByte1) == -1)
                      {
                        this.RollBackState(in state, true);
                        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterAfterEscapeWithinString, nextByte1);
                      }
                      if (nextByte1 == (byte) 117)
                      {
                        ++this._bytePositionInLine;
                        int num2 = 0;
                        int index2 = index1 + 1;
                        while (true)
                        {
                          for (; index2 < buffer.Length; ++index2)
                          {
                            byte nextByte2 = buffer[index2];
                            if (!JsonReaderHelper.IsHexDigit(nextByte2))
                            {
                              this.RollBackState(in state, true);
                              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidHexCharacterWithinString, nextByte2);
                            }
                            ++num2;
                            ++this._bytePositionInLine;
                            if (num2 >= 4)
                            {
                              flag = false;
                              index1 = index2 + 1;
                              goto label_33;
                            }
                          }
                          if (this.GetNextSpan())
                          {
                            this._totalConsumed += (long) buffer.Length;
                            buffer = this._buffer;
                            index2 = 0;
                          }
                          else
                            break;
                        }
                        if (this.IsLastSpan)
                        {
                          this.RollBackState(in state, true);
                          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                        }
                        this.RollBackState(in state);
                        return false;
                      }
                      flag = false;
                      break;
                    }
                    if (nextByte1 < (byte) 32)
                    {
                      this.RollBackState(in state, true);
                      ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, nextByte1);
                      break;
                    }
                    break;
                }
                ++this._bytePositionInLine;
                ++index1;
              }
              if (this.GetNextSpan())
              {
                this._totalConsumed += (long) buffer.Length;
                buffer = this._buffer;
                index1 = 0;
              }
              else
                break;
            }
            if (this.IsLastSpan)
            {
              this.RollBackState(in state, true);
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
            }
            this.RollBackState(in state);
            return false;
          }
label_41:
          this.ValueSequence = this._sequence.Slice(state.GetStartPosition(1), end);
          this._tokenType = JsonTokenType.String;
          return true;
        }
        this._totalConsumed += (long) buffer.Length;
        this._bytePositionInLine += (long) buffer.Length;
      }
      if (this.IsLastSpan)
      {
        this._bytePositionInLine += (long) num1;
        this.RollBackState(in state, true);
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
      }
      this.RollBackState(in state);
      return false;
    }

    private bool ConsumeStringAndValidateMultiSegment(ReadOnlySpan<byte> data, int idx)
    {
      Utf8JsonReader.PartialStateForRollback state = this.CaptureState();
      this.HasValueSequence = false;
      int num1 = this._buffer.Length - this._consumed;
      this._bytePositionInLine += (long) (idx + 1);
      bool flag = false;
label_27:
      while (true)
      {
        while (idx < data.Length)
        {
          byte nextByte1 = data[idx];
          switch (nextByte1)
          {
            case 34:
              if (flag)
              {
                flag = false;
                break;
              }
              if (this.HasValueSequence)
              {
                ++this._bytePositionInLine;
                this._consumed = idx + 1;
                this._totalConsumed += (long) num1;
                SequencePosition end = new SequencePosition(this._currentPosition.GetObject(), this._currentPosition.GetInteger() + idx);
                this.ValueSequence = this._sequence.Slice(state.GetStartPosition(1), end);
              }
              else
              {
                ++this._bytePositionInLine;
                this._consumed += idx + 2;
                this.ValueSpan = data.Slice(0, idx);
              }
              this._stringHasEscaping = true;
              this._tokenType = JsonTokenType.String;
              return true;
            case 92:
              flag = !flag;
              break;
            default:
              if (flag)
              {
                if (JsonConstants.EscapableChars.IndexOf<byte>(nextByte1) == -1)
                {
                  this.RollBackState(in state, true);
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterAfterEscapeWithinString, nextByte1);
                }
                if (nextByte1 == (byte) 117)
                {
                  ++this._bytePositionInLine;
                  int num2 = 0;
                  int index = idx + 1;
                  while (true)
                  {
                    for (; index < data.Length; ++index)
                    {
                      byte nextByte2 = data[index];
                      if (!JsonReaderHelper.IsHexDigit(nextByte2))
                      {
                        this.RollBackState(in state, true);
                        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidHexCharacterWithinString, nextByte2);
                      }
                      ++num2;
                      ++this._bytePositionInLine;
                      if (num2 >= 4)
                      {
                        flag = false;
                        idx = index + 1;
                        goto label_27;
                      }
                    }
                    if (this.GetNextSpan())
                    {
                      if (this.HasValueSequence)
                        this._totalConsumed += (long) data.Length;
                      data = this._buffer;
                      index = 0;
                      this.HasValueSequence = true;
                    }
                    else
                      break;
                  }
                  if (this.IsLastSpan)
                  {
                    this.RollBackState(in state, true);
                    ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
                  }
                  this.RollBackState(in state);
                  return false;
                }
                flag = false;
                break;
              }
              if (nextByte1 < (byte) 32)
              {
                this.RollBackState(in state, true);
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterWithinString, nextByte1);
                break;
              }
              break;
          }
          ++this._bytePositionInLine;
          ++idx;
        }
        if (this.GetNextSpan())
        {
          if (this.HasValueSequence)
            this._totalConsumed += (long) data.Length;
          data = this._buffer;
          idx = 0;
          this.HasValueSequence = true;
        }
        else
          break;
      }
      if (this.IsLastSpan)
      {
        this.RollBackState(in state, true);
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.EndOfStringNotFound);
      }
      this.RollBackState(in state);
      return false;
    }

    private void RollBackState(in Utf8JsonReader.PartialStateForRollback state, bool isError = false)
    {
      this._totalConsumed = state._prevTotalConsumed;
      if (!isError)
        this._bytePositionInLine = state._prevBytePositionInLine;
      this._consumed = state._prevConsumed;
      this._currentPosition = state._prevCurrentPosition;
    }

    private bool TryGetNumberMultiSegment(ReadOnlySpan<byte> data, out int consumed)
    {
      Utf8JsonReader.PartialStateForRollback stateForRollback = this.CaptureState();
      consumed = 0;
      int i = 0;
      if (this.ConsumeNegativeSignMultiSegment(ref data, ref i, in stateForRollback) == ConsumeNumberResult.NeedMoreData)
      {
        this.RollBackState(in stateForRollback);
        return false;
      }
      byte nextByte1;
      if (data[i] == (byte) 48)
      {
        switch (this.ConsumeZeroMultiSegment(ref data, ref i, in stateForRollback))
        {
          case ConsumeNumberResult.Success:
            goto label_20;
          case ConsumeNumberResult.NeedMoreData:
            this.RollBackState(in stateForRollback);
            return false;
          default:
            nextByte1 = data[i];
            break;
        }
      }
      else
      {
        switch (this.ConsumeIntegerDigitsMultiSegment(ref data, ref i))
        {
          case ConsumeNumberResult.Success:
            goto label_20;
          case ConsumeNumberResult.NeedMoreData:
            this.RollBackState(in stateForRollback);
            return false;
          default:
            nextByte1 = data[i];
            switch (nextByte1)
            {
              case 46:
              case 69:
              case 101:
                break;
              default:
                this.RollBackState(in stateForRollback, true);
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, nextByte1);
                break;
            }
            break;
        }
      }
      if (nextByte1 == (byte) 46)
      {
        ++i;
        ++this._bytePositionInLine;
        switch (this.ConsumeDecimalDigitsMultiSegment(ref data, ref i, in stateForRollback))
        {
          case ConsumeNumberResult.Success:
            goto label_20;
          case ConsumeNumberResult.NeedMoreData:
            this.RollBackState(in stateForRollback);
            return false;
          default:
            byte nextByte2 = data[i];
            switch (nextByte2)
            {
              case 69:
              case 101:
                break;
              default:
                this.RollBackState(in stateForRollback, true);
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedNextDigitEValueNotFound, nextByte2);
                break;
            }
            break;
        }
      }
      ++i;
      ++this._bytePositionInLine;
      if (this.ConsumeSignMultiSegment(ref data, ref i, in stateForRollback) == ConsumeNumberResult.NeedMoreData)
      {
        this.RollBackState(in stateForRollback);
        return false;
      }
      ++i;
      ++this._bytePositionInLine;
      switch (this.ConsumeIntegerDigitsMultiSegment(ref data, ref i))
      {
        case ConsumeNumberResult.Success:
          break;
        case ConsumeNumberResult.NeedMoreData:
          this.RollBackState(in stateForRollback);
          return false;
        default:
          this.RollBackState(in stateForRollback, true);
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndOfDigitNotFound, data[i]);
          break;
      }
label_20:
      if (this.HasValueSequence)
      {
        this.ValueSequence = this._sequence.Slice(stateForRollback.GetStartPosition(), new SequencePosition(this._currentPosition.GetObject(), this._currentPosition.GetInteger() + i));
        consumed = i;
      }
      else
      {
        this.ValueSpan = data.Slice(0, i);
        consumed = i;
      }
      return true;
    }

    private ConsumeNumberResult ConsumeNegativeSignMultiSegment(
      ref ReadOnlySpan<byte> data,
      ref int i,
      in Utf8JsonReader.PartialStateForRollback rollBackState)
    {
      if (data[i] == (byte) 45)
      {
        ++i;
        ++this._bytePositionInLine;
        if (i >= data.Length)
        {
          if (this.IsLastSpan)
          {
            this.RollBackState(in rollBackState, true);
            ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
          }
          if (!this.GetNextSpan())
          {
            if (this.IsLastSpan)
            {
              this.RollBackState(in rollBackState, true);
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
            }
            return ConsumeNumberResult.NeedMoreData;
          }
          this._totalConsumed += (long) i;
          this.HasValueSequence = true;
          i = 0;
          data = this._buffer;
        }
        byte nextByte = data[i];
        if (!JsonHelpers.IsDigit(nextByte))
        {
          this.RollBackState(in rollBackState, true);
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundAfterSign, nextByte);
        }
      }
      return ConsumeNumberResult.OperationIncomplete;
    }

    private ConsumeNumberResult ConsumeZeroMultiSegment(
      ref ReadOnlySpan<byte> data,
      ref int i,
      in Utf8JsonReader.PartialStateForRollback rollBackState)
    {
      ++i;
      ++this._bytePositionInLine;
      if (i < data.Length)
      {
        if (JsonConstants.Delimiters.IndexOf<byte>(data[i]) >= 0)
          return ConsumeNumberResult.Success;
      }
      else
      {
        if (this.IsLastSpan)
          return ConsumeNumberResult.Success;
        if (!this.GetNextSpan())
          return this.IsLastSpan ? ConsumeNumberResult.Success : ConsumeNumberResult.NeedMoreData;
        this._totalConsumed += (long) i;
        this.HasValueSequence = true;
        i = 0;
        data = this._buffer;
        if (JsonConstants.Delimiters.IndexOf<byte>(data[i]) >= 0)
          return ConsumeNumberResult.Success;
      }
      byte nextByte = data[i];
      switch (nextByte)
      {
        case 46:
        case 69:
        case 101:
          return ConsumeNumberResult.OperationIncomplete;
        default:
          this.RollBackState(in rollBackState, true);
          ThrowHelper.ThrowJsonReaderException(ref this, JsonHelpers.IsInRangeInclusive((int) nextByte, 48, 57) ? ExceptionResource.InvalidLeadingZeroInNumber : ExceptionResource.ExpectedEndOfDigitNotFound, nextByte);
          goto case 46;
      }
    }

    private ConsumeNumberResult ConsumeIntegerDigitsMultiSegment(
      ref ReadOnlySpan<byte> data,
      ref int i)
    {
      byte num1 = 0;
      int num2 = 0;
      while (i < data.Length)
      {
        num1 = data[i];
        if (JsonHelpers.IsDigit(num1))
        {
          ++num2;
          ++i;
        }
        else
          break;
      }
      if (i >= data.Length)
      {
        if (this.IsLastSpan)
        {
          this._bytePositionInLine += (long) num2;
          return ConsumeNumberResult.Success;
        }
        while (this.GetNextSpan())
        {
          this._totalConsumed += (long) i;
          this._bytePositionInLine += (long) num2;
          num2 = 0;
          this.HasValueSequence = true;
          i = 0;
          data = this._buffer;
          while (i < data.Length)
          {
            num1 = data[i];
            if (JsonHelpers.IsDigit(num1))
              ++i;
            else
              break;
          }
          this._bytePositionInLine += (long) i;
          if (i >= data.Length)
          {
            if (this.IsLastSpan)
              return ConsumeNumberResult.Success;
          }
          else
            goto label_19;
        }
        if (!this.IsLastSpan)
          return ConsumeNumberResult.NeedMoreData;
        this._bytePositionInLine += (long) num2;
        return ConsumeNumberResult.Success;
      }
      this._bytePositionInLine += (long) num2;
label_19:
      return JsonConstants.Delimiters.IndexOf<byte>(num1) >= 0 ? ConsumeNumberResult.Success : ConsumeNumberResult.OperationIncomplete;
    }

    private ConsumeNumberResult ConsumeDecimalDigitsMultiSegment(
      ref ReadOnlySpan<byte> data,
      ref int i,
      in Utf8JsonReader.PartialStateForRollback rollBackState)
    {
      if (i >= data.Length)
      {
        if (this.IsLastSpan)
        {
          this.RollBackState(in rollBackState, true);
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
        }
        if (!this.GetNextSpan())
        {
          if (this.IsLastSpan)
          {
            this.RollBackState(in rollBackState, true);
            ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
          }
          return ConsumeNumberResult.NeedMoreData;
        }
        this._totalConsumed += (long) i;
        this.HasValueSequence = true;
        i = 0;
        data = this._buffer;
      }
      byte nextByte = data[i];
      if (!JsonHelpers.IsDigit(nextByte))
      {
        this.RollBackState(in rollBackState, true);
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundAfterDecimal, nextByte);
      }
      ++i;
      ++this._bytePositionInLine;
      return this.ConsumeIntegerDigitsMultiSegment(ref data, ref i);
    }

    private ConsumeNumberResult ConsumeSignMultiSegment(
      ref ReadOnlySpan<byte> data,
      ref int i,
      in Utf8JsonReader.PartialStateForRollback rollBackState)
    {
      if (i >= data.Length)
      {
        if (this.IsLastSpan)
        {
          this.RollBackState(in rollBackState, true);
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
        }
        if (!this.GetNextSpan())
        {
          if (this.IsLastSpan)
          {
            this.RollBackState(in rollBackState, true);
            ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
          }
          return ConsumeNumberResult.NeedMoreData;
        }
        this._totalConsumed += (long) i;
        this.HasValueSequence = true;
        i = 0;
        data = this._buffer;
      }
      byte nextByte = data[i];
      switch (nextByte)
      {
        case 43:
        case 45:
          ++i;
          ++this._bytePositionInLine;
          if (i >= data.Length)
          {
            if (this.IsLastSpan)
            {
              this.RollBackState(in rollBackState, true);
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
            }
            if (!this.GetNextSpan())
            {
              if (this.IsLastSpan)
              {
                this.RollBackState(in rollBackState, true);
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundEndOfData);
              }
              return ConsumeNumberResult.NeedMoreData;
            }
            this._totalConsumed += (long) i;
            this.HasValueSequence = true;
            i = 0;
            data = this._buffer;
          }
          nextByte = data[i];
          break;
      }
      if (!JsonHelpers.IsDigit(nextByte))
      {
        this.RollBackState(in rollBackState, true);
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.RequiredDigitNotFoundAfterSign, nextByte);
      }
      return ConsumeNumberResult.OperationIncomplete;
    }

    private bool ConsumeNextTokenOrRollbackMultiSegment(byte marker)
    {
      long totalConsumed = this._totalConsumed;
      int consumed = this._consumed;
      long bytePositionInLine = this._bytePositionInLine;
      long lineNumber = this._lineNumber;
      JsonTokenType tokenType = this._tokenType;
      SequencePosition currentPosition = this._currentPosition;
      bool commaBeforeComment = this._trailingCommaBeforeComment;
      switch (this.ConsumeNextTokenMultiSegment(marker))
      {
        case ConsumeTokenResult.Success:
          return true;
        case ConsumeTokenResult.NotEnoughDataRollBackState:
          this._consumed = consumed;
          this._tokenType = tokenType;
          this._bytePositionInLine = bytePositionInLine;
          this._lineNumber = lineNumber;
          this._totalConsumed = totalConsumed;
          this._currentPosition = currentPosition;
          this._trailingCommaBeforeComment = commaBeforeComment;
          break;
      }
      return false;
    }

    private ConsumeTokenResult ConsumeNextTokenMultiSegment(byte marker)
    {
      if (this._readerOptions.CommentHandling != JsonCommentHandling.Disallow)
      {
        if (this._readerOptions.CommentHandling != JsonCommentHandling.Allow)
          return this.ConsumeNextTokenUntilAfterAllCommentsAreSkippedMultiSegment(marker);
        if (marker == (byte) 47)
          return !this.SkipOrConsumeCommentMultiSegmentWithRollback() ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
        if (this._tokenType == JsonTokenType.Comment)
          return this.ConsumeNextTokenFromLastNonCommentTokenMultiSegment();
      }
      if (this._bitStack.CurrentDepth == 0)
        ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndAfterSingleJson, marker);
      switch (marker)
      {
        case 44:
          ++this._consumed;
          ++this._bytePositionInLine;
          if ((long) this._consumed >= (long) (uint) this._buffer.Length)
          {
            if (this.IsLastSpan)
            {
              --this._consumed;
              --this._bytePositionInLine;
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
            }
            if (!this.GetNextSpan())
            {
              if (this.IsLastSpan)
              {
                --this._consumed;
                --this._bytePositionInLine;
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
              }
              return ConsumeTokenResult.NotEnoughDataRollBackState;
            }
          }
          byte num = this._buffer[this._consumed];
          if (num <= (byte) 32)
          {
            this.SkipWhiteSpaceMultiSegment();
            if (!this.HasMoreDataMultiSegment(ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
              return ConsumeTokenResult.NotEnoughDataRollBackState;
            num = this._buffer[this._consumed];
          }
          this.TokenStartIndex = this.BytesConsumed;
          if (this._readerOptions.CommentHandling == JsonCommentHandling.Allow && num == (byte) 47)
          {
            this._trailingCommaBeforeComment = true;
            return !this.SkipOrConsumeCommentMultiSegmentWithRollback() ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
          }
          if (this._inObject)
          {
            switch (num)
            {
              case 34:
                return !this.ConsumePropertyNameMultiSegment() ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
              case 125:
                if (this._readerOptions.AllowTrailingCommas)
                {
                  this.EndObject();
                  return ConsumeTokenResult.Success;
                }
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd);
                goto default;
              default:
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num);
                goto case 34;
            }
          }
          else
          {
            if (num == (byte) 93)
            {
              if (this._readerOptions.AllowTrailingCommas)
              {
                this.EndArray();
                return ConsumeTokenResult.Success;
              }
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd);
            }
            return !this.ConsumeValueMultiSegment(num) ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
          }
        case 93:
          this.EndArray();
          break;
        case 125:
          this.EndObject();
          break;
        default:
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.FoundInvalidCharacter, marker);
          break;
      }
      return ConsumeTokenResult.Success;
    }

    private ConsumeTokenResult ConsumeNextTokenFromLastNonCommentTokenMultiSegment()
    {
      this._tokenType = !JsonReaderHelper.IsTokenTypePrimitive(this._previousTokenType) ? this._previousTokenType : (this._inObject ? JsonTokenType.StartObject : JsonTokenType.StartArray);
      if (this.HasMoreDataMultiSegment())
      {
        byte num1 = this._buffer[this._consumed];
        if (num1 <= (byte) 32)
        {
          this.SkipWhiteSpaceMultiSegment();
          if (this.HasMoreDataMultiSegment())
            num1 = this._buffer[this._consumed];
          else
            goto label_51;
        }
        if (this._bitStack.CurrentDepth == 0 && this._tokenType != JsonTokenType.None)
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndAfterSingleJson, num1);
        this.TokenStartIndex = this.BytesConsumed;
        switch (num1)
        {
          case 44:
            if (this._previousTokenType <= JsonTokenType.StartObject || this._previousTokenType == JsonTokenType.StartArray || this._trailingCommaBeforeComment)
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueAfterComment, num1);
            ++this._consumed;
            ++this._bytePositionInLine;
            if ((long) this._consumed >= (long) (uint) this._buffer.Length)
            {
              if (this.IsLastSpan)
              {
                --this._consumed;
                --this._bytePositionInLine;
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
              }
              if (!this.GetNextSpan())
              {
                if (this.IsLastSpan)
                {
                  --this._consumed;
                  --this._bytePositionInLine;
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                  goto label_51;
                }
                else
                  goto label_51;
              }
            }
            byte num2 = this._buffer[this._consumed];
            if (num2 <= (byte) 32)
            {
              this.SkipWhiteSpaceMultiSegment();
              if (this.HasMoreDataMultiSegment(ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
                num2 = this._buffer[this._consumed];
              else
                goto label_51;
            }
            this.TokenStartIndex = this.BytesConsumed;
            if (num2 == (byte) 47)
            {
              this._trailingCommaBeforeComment = true;
              if (!this.SkipOrConsumeCommentMultiSegmentWithRollback())
                goto label_51;
              else
                break;
            }
            else if (this._inObject)
            {
              switch (num2)
              {
                case 34:
                  if (!this.ConsumePropertyNameMultiSegment())
                    goto label_51;
                  else
                    break;
                case 125:
                  if (this._readerOptions.AllowTrailingCommas)
                  {
                    this.EndObject();
                    break;
                  }
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd);
                  goto default;
                default:
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num2);
                  goto case 34;
              }
            }
            else
            {
              if (num2 == (byte) 93)
              {
                if (this._readerOptions.AllowTrailingCommas)
                {
                  this.EndArray();
                  break;
                }
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd);
              }
              if (!this.ConsumeValueMultiSegment(num2))
                goto label_51;
              else
                break;
            }
            break;
          case 93:
            this.EndArray();
            break;
          case 125:
            this.EndObject();
            break;
          default:
            if (this._tokenType == JsonTokenType.None)
            {
              if (!this.ReadFirstTokenMultiSegment(num1))
                goto label_51;
              else
                break;
            }
            else if (this._tokenType == JsonTokenType.StartObject)
            {
              if (num1 != (byte) 34)
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num1);
              long totalConsumed = this._totalConsumed;
              int consumed = this._consumed;
              long bytePositionInLine = this._bytePositionInLine;
              long lineNumber = this._lineNumber;
              if (!this.ConsumePropertyNameMultiSegment())
              {
                this._consumed = consumed;
                this._tokenType = JsonTokenType.StartObject;
                this._bytePositionInLine = bytePositionInLine;
                this._lineNumber = lineNumber;
                this._totalConsumed = totalConsumed;
                goto label_51;
              }
              else
                break;
            }
            else if (this._tokenType == JsonTokenType.StartArray)
            {
              if (this.ConsumeValueMultiSegment(num1))
                break;
              goto label_51;
            }
            else if (this._tokenType == JsonTokenType.PropertyName)
            {
              if (this.ConsumeValueMultiSegment(num1))
                break;
              goto label_51;
            }
            else if (this._inObject)
            {
              if (num1 != (byte) 34)
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, num1);
              if (!this.ConsumePropertyNameMultiSegment())
                goto label_51;
              else
                break;
            }
            else if (!this.ConsumeValueMultiSegment(num1))
              goto label_51;
            else
              break;
        }
        return ConsumeTokenResult.Success;
      }
label_51:
      return ConsumeTokenResult.NotEnoughDataRollBackState;
    }

    private bool SkipAllCommentsMultiSegment(ref byte marker)
    {
      while (marker == (byte) 47)
      {
        if (this.SkipOrConsumeCommentMultiSegmentWithRollback() && this.HasMoreDataMultiSegment())
        {
          marker = this._buffer[this._consumed];
          if (marker <= (byte) 32)
          {
            this.SkipWhiteSpaceMultiSegment();
            if (this.HasMoreDataMultiSegment())
            {
              marker = this._buffer[this._consumed];
              continue;
            }
          }
          else
            continue;
        }
        return false;
      }
      return true;
    }

    private bool SkipAllCommentsMultiSegment(ref byte marker, ExceptionResource resource)
    {
      while (marker == (byte) 47)
      {
        if (this.SkipOrConsumeCommentMultiSegmentWithRollback() && this.HasMoreDataMultiSegment(resource))
        {
          marker = this._buffer[this._consumed];
          if (marker <= (byte) 32)
          {
            this.SkipWhiteSpaceMultiSegment();
            if (this.HasMoreDataMultiSegment(resource))
            {
              marker = this._buffer[this._consumed];
              continue;
            }
          }
          else
            continue;
        }
        return false;
      }
      return true;
    }

    private ConsumeTokenResult ConsumeNextTokenUntilAfterAllCommentsAreSkippedMultiSegment(
      byte marker)
    {
      if (this.SkipAllCommentsMultiSegment(ref marker))
      {
        this.TokenStartIndex = this.BytesConsumed;
        if (this._tokenType == JsonTokenType.StartObject)
        {
          switch (marker)
          {
            case 34:
              long totalConsumed = this._totalConsumed;
              int consumed = this._consumed;
              long bytePositionInLine = this._bytePositionInLine;
              long lineNumber = this._lineNumber;
              SequencePosition currentPosition = this._currentPosition;
              if (!this.ConsumePropertyNameMultiSegment())
              {
                this._consumed = consumed;
                this._tokenType = JsonTokenType.StartObject;
                this._bytePositionInLine = bytePositionInLine;
                this._lineNumber = lineNumber;
                this._totalConsumed = totalConsumed;
                this._currentPosition = currentPosition;
                goto label_48;
              }
              else
                break;
            case 125:
              this.EndObject();
              break;
            default:
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, marker);
              goto case 34;
          }
        }
        else if (this._tokenType == JsonTokenType.StartArray)
        {
          if (marker == (byte) 93)
            this.EndArray();
          else if (!this.ConsumeValueMultiSegment(marker))
            goto label_48;
        }
        else if (this._tokenType == JsonTokenType.PropertyName)
        {
          if (!this.ConsumeValueMultiSegment(marker))
            goto label_48;
        }
        else if (this._bitStack.CurrentDepth == 0)
        {
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedEndAfterSingleJson, marker);
        }
        else
        {
          switch (marker)
          {
            case 44:
              ++this._consumed;
              ++this._bytePositionInLine;
              if ((long) this._consumed >= (long) (uint) this._buffer.Length)
              {
                if (this.IsLastSpan)
                {
                  --this._consumed;
                  --this._bytePositionInLine;
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                }
                if (!this.GetNextSpan())
                {
                  if (this.IsLastSpan)
                  {
                    --this._consumed;
                    --this._bytePositionInLine;
                    ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound);
                  }
                  return ConsumeTokenResult.NotEnoughDataRollBackState;
                }
              }
              marker = this._buffer[this._consumed];
              if (marker <= (byte) 32)
              {
                this.SkipWhiteSpaceMultiSegment();
                if (!this.HasMoreDataMultiSegment(ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
                  return ConsumeTokenResult.NotEnoughDataRollBackState;
                marker = this._buffer[this._consumed];
              }
              if (!this.SkipAllCommentsMultiSegment(ref marker, ExceptionResource.ExpectedStartOfPropertyOrValueNotFound))
                return ConsumeTokenResult.NotEnoughDataRollBackState;
              this.TokenStartIndex = this.BytesConsumed;
              if (this._inObject)
              {
                switch (marker)
                {
                  case 34:
                    return !this.ConsumePropertyNameMultiSegment() ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
                  case 125:
                    if (this._readerOptions.AllowTrailingCommas)
                    {
                      this.EndObject();
                      break;
                    }
                    ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeObjectEnd);
                    goto default;
                  default:
                    ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.ExpectedStartOfPropertyNotFound, marker);
                    goto case 34;
                }
              }
              else
              {
                if (marker == (byte) 93)
                {
                  if (this._readerOptions.AllowTrailingCommas)
                  {
                    this.EndArray();
                    break;
                  }
                  ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.TrailingCommaNotAllowedBeforeArrayEnd);
                }
                return !this.ConsumeValueMultiSegment(marker) ? ConsumeTokenResult.NotEnoughDataRollBackState : ConsumeTokenResult.Success;
              }
              break;
            case 93:
              this.EndArray();
              break;
            case 125:
              this.EndObject();
              break;
            default:
              ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.FoundInvalidCharacter, marker);
              break;
          }
        }
        return ConsumeTokenResult.Success;
      }
label_48:
      return ConsumeTokenResult.IncompleteNoRollBackNecessary;
    }

    private bool SkipOrConsumeCommentMultiSegmentWithRollback()
    {
      long bytesConsumed = this.BytesConsumed;
      SequencePosition start = new SequencePosition(this._currentPosition.GetObject(), this._currentPosition.GetInteger() + this._consumed);
      int tailBytesToIgnore;
      bool flag = this.SkipCommentMultiSegment(out tailBytesToIgnore);
      if (flag)
      {
        if (this._readerOptions.CommentHandling == JsonCommentHandling.Allow)
        {
          SequencePosition end = new SequencePosition(this._currentPosition.GetObject(), this._currentPosition.GetInteger() + this._consumed);
          ReadOnlySequence<byte> readOnlySequence = this._sequence.Slice(start, end);
          readOnlySequence = readOnlySequence.Slice(2L, readOnlySequence.Length - 2L - (long) tailBytesToIgnore);
          this.HasValueSequence = !readOnlySequence.IsSingleSegment;
          if (this.HasValueSequence)
            this.ValueSequence = readOnlySequence;
          else
            this.ValueSpan = readOnlySequence.First.Span;
          if (this._tokenType != JsonTokenType.Comment)
            this._previousTokenType = this._tokenType;
          this._tokenType = JsonTokenType.Comment;
        }
      }
      else
      {
        this._totalConsumed = bytesConsumed;
        this._consumed = 0;
      }
      return flag;
    }

    private bool SkipCommentMultiSegment(out int tailBytesToIgnore)
    {
      ++this._consumed;
      ++this._bytePositionInLine;
      ReadOnlySpan<byte> readOnlySpan = this._buffer.Slice(this._consumed);
      if (readOnlySpan.Length == 0)
      {
        if (this.IsLastSpan)
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfDataWhileReadingComment);
        if (!this.GetNextSpan())
        {
          if (this.IsLastSpan)
            ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfDataWhileReadingComment);
          tailBytesToIgnore = 0;
          return false;
        }
        readOnlySpan = this._buffer;
      }
      byte nextByte = readOnlySpan[0];
      switch (nextByte)
      {
        case 42:
        case 47:
          bool flag = nextByte == (byte) 42;
          ++this._consumed;
          ++this._bytePositionInLine;
          ReadOnlySpan<byte> localBuffer = readOnlySpan.Slice(1);
          if (localBuffer.Length == 0)
          {
            if (this.IsLastSpan)
            {
              tailBytesToIgnore = 0;
              if (flag)
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfDataWhileReadingComment);
              return true;
            }
            if (!this.GetNextSpan())
            {
              tailBytesToIgnore = 0;
              if (!this.IsLastSpan)
                return false;
              if (flag)
                ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfDataWhileReadingComment);
              return true;
            }
            localBuffer = this._buffer;
          }
          if (!flag)
            return this.SkipSingleLineCommentMultiSegment(localBuffer, out tailBytesToIgnore);
          tailBytesToIgnore = 2;
          return this.SkipMultiLineCommentMultiSegment(localBuffer);
        default:
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.InvalidCharacterAtStartOfComment, nextByte);
          goto case 42;
      }
    }

    private bool SkipSingleLineCommentMultiSegment(
      ReadOnlySpan<byte> localBuffer,
      out int tailBytesToSkip)
    {
      bool flag = false;
      int dangerousLineSeparatorBytesConsumed = 0;
      tailBytesToSkip = 0;
      while (!flag)
      {
        int separatorMultiSegment = this.FindLineSeparatorMultiSegment(localBuffer, ref dangerousLineSeparatorBytesConsumed);
        if (separatorMultiSegment != -1)
        {
          ++tailBytesToSkip;
          this._consumed += separatorMultiSegment + 1;
          this._bytePositionInLine += (long) (separatorMultiSegment + 1);
          if (localBuffer[separatorMultiSegment] != (byte) 10)
          {
            if (separatorMultiSegment < localBuffer.Length - 1)
            {
              if (localBuffer[separatorMultiSegment + 1] == (byte) 10)
              {
                ++tailBytesToSkip;
                ++this._consumed;
                ++this._bytePositionInLine;
                goto label_20;
              }
              else
                goto label_20;
            }
            else
              flag = true;
          }
          else
            goto label_20;
        }
        else
        {
          this._consumed += localBuffer.Length;
          this._bytePositionInLine += (long) localBuffer.Length;
        }
        if (this.IsLastSpan)
        {
          if (!flag)
            return true;
          goto label_20;
        }
        else if (!this.GetNextSpan())
        {
          if (!this.IsLastSpan)
            return false;
          if (!flag)
            return true;
          goto label_20;
        }
        else
          localBuffer = this._buffer;
      }
      if (localBuffer[0] == (byte) 10)
      {
        ++tailBytesToSkip;
        ++this._consumed;
      }
label_20:
      this._bytePositionInLine = 0L;
      ++this._lineNumber;
      return true;
    }

    private int FindLineSeparatorMultiSegment(
      ReadOnlySpan<byte> localBuffer,
      ref int dangerousLineSeparatorBytesConsumed)
    {
      if (dangerousLineSeparatorBytesConsumed != 0)
      {
        this.ThrowOnDangerousLineSeparatorMultiSegment(localBuffer, ref dangerousLineSeparatorBytesConsumed);
        if (dangerousLineSeparatorBytesConsumed != 0)
          return -1;
      }
      int num = 0;
      do
      {
        int index = localBuffer.IndexOfAny<byte>((byte) 10, (byte) 13, (byte) 226);
        dangerousLineSeparatorBytesConsumed = 0;
        if (index == -1)
          return -1;
        if (localBuffer[index] != (byte) 226)
          return num + index;
        int start = index + 1;
        localBuffer = localBuffer.Slice(start);
        num += start;
        ++dangerousLineSeparatorBytesConsumed;
        this.ThrowOnDangerousLineSeparatorMultiSegment(localBuffer, ref dangerousLineSeparatorBytesConsumed);
      }
      while (dangerousLineSeparatorBytesConsumed == 0);
      return -1;
    }

    private void ThrowOnDangerousLineSeparatorMultiSegment(
      ReadOnlySpan<byte> localBuffer,
      ref int dangerousLineSeparatorBytesConsumed)
    {
      if (localBuffer.IsEmpty)
        return;
      if (dangerousLineSeparatorBytesConsumed == 1)
      {
        if (localBuffer[0] == (byte) 128)
        {
          localBuffer = localBuffer.Slice(1);
          ++dangerousLineSeparatorBytesConsumed;
          if (localBuffer.IsEmpty)
            return;
        }
        else
        {
          dangerousLineSeparatorBytesConsumed = 0;
          return;
        }
      }
      if (dangerousLineSeparatorBytesConsumed != 2)
        return;
      switch (localBuffer[0])
      {
        case 168:
        case 169:
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfLineSeparator);
          break;
        default:
          dangerousLineSeparatorBytesConsumed = 0;
          break;
      }
    }

    private bool SkipMultiLineCommentMultiSegment(ReadOnlySpan<byte> localBuffer)
    {
      bool flag1 = false;
      bool flag2 = false;
      while (true)
      {
        do
        {
          if (flag1)
          {
            if (localBuffer[0] == (byte) 47)
            {
              ++this._consumed;
              ++this._bytePositionInLine;
              return true;
            }
            flag1 = false;
          }
          if (flag2)
          {
            if (localBuffer[0] == (byte) 10)
            {
              ++this._consumed;
              localBuffer = localBuffer.Slice(1);
            }
            flag2 = false;
          }
          int index = localBuffer.IndexOfAny<byte>((byte) 42, (byte) 10, (byte) 13);
          if (index != -1)
          {
            int start = index + 1;
            byte num = localBuffer[index];
            localBuffer = localBuffer.Slice(start);
            this._consumed += start;
            switch (num)
            {
              case 10:
                this._bytePositionInLine = 0L;
                ++this._lineNumber;
                break;
              case 42:
                flag1 = true;
                this._bytePositionInLine += (long) start;
                break;
              default:
                this._bytePositionInLine = 0L;
                ++this._lineNumber;
                flag2 = true;
                break;
            }
          }
          else
          {
            this._consumed += localBuffer.Length;
            this._bytePositionInLine += (long) localBuffer.Length;
            localBuffer = ReadOnlySpan<byte>.Empty;
          }
        }
        while (!localBuffer.IsEmpty);
        if (this.IsLastSpan)
          ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfDataWhileReadingComment);
        if (!this.GetNextSpan())
        {
          if (this.IsLastSpan)
            ThrowHelper.ThrowJsonReaderException(ref this, ExceptionResource.UnexpectedEndOfDataWhileReadingComment);
          else
            break;
        }
        localBuffer = this._buffer;
      }
      return false;
    }

    private Utf8JsonReader.PartialStateForRollback CaptureState() => new Utf8JsonReader.PartialStateForRollback(this._totalConsumed, this._bytePositionInLine, this._consumed, this._currentPosition);


    #nullable enable
    /// <summary>Reads the next JSON token value from the source, unescaped, and transcoded as a string.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a string (that is, not a <see cref="F:System.Text.Json.JsonTokenType.String" />, <see cref="F:System.Text.Json.JsonTokenType.PropertyName" />, or <see cref="F:System.Text.Json.JsonTokenType.Null" />).
    /// 
    /// -or-
    /// 
    /// The JSON string contains invalid UTF-8 bytes or invalid UTF-16 surrogates.</exception>
    /// <returns>The token value parsed to a string, or <see langword="null" /> if <see cref="P:System.Text.Json.Utf8JsonReader.TokenType" /> is <see cref="F:System.Text.Json.JsonTokenType.Null" />.</returns>
    public string? GetString()
    {
      if (this.TokenType == JsonTokenType.Null)
        return (string) null;
      if (this.TokenType != JsonTokenType.String && this.TokenType != JsonTokenType.PropertyName)
        throw ThrowHelper.GetInvalidOperationException_ExpectedString(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan1;
      if (!this.HasValueSequence)
      {
        readOnlySpan1 = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan1 = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> readOnlySpan2 = readOnlySpan1;
      if (!this._stringHasEscaping)
        return JsonReaderHelper.TranscodeHelper(readOnlySpan2);
      int idx = readOnlySpan2.IndexOf<byte>((byte) 92);
      return JsonReaderHelper.GetUnescapedString(readOnlySpan2, idx);
    }

    /// <summary>Parses the current JSON token value from the source as a comment, transcoded it as a <see cref="T:System.String" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token is not a comment.</exception>
    /// <returns>The comment that represents the current JSON token value.</returns>
    public string GetComment()
    {
      if (this.TokenType != JsonTokenType.Comment)
        throw ThrowHelper.GetInvalidOperationException_ExpectedComment(this.TokenType);
      ReadOnlySpan<byte> utf8Unescaped;
      if (!this.HasValueSequence)
      {
        utf8Unescaped = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        utf8Unescaped = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      return JsonReaderHelper.TranscodeHelper(utf8Unescaped);
    }

    /// <summary>Reads the next JSON token value from the source as a <see cref="T:System.Boolean" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token isn't a boolean value (that is, <see cref="F:System.Text.Json.JsonTokenType.True" /> or <see cref="F:System.Text.Json.JsonTokenType.False" />).</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="P:System.Text.Json.Utf8JsonReader.TokenType" /> is <see cref="F:System.Text.Json.JsonTokenType.True" />; <see langword="false" /> if the <see cref="P:System.Text.Json.Utf8JsonReader.TokenType" /> is <see cref="F:System.Text.Json.JsonTokenType.False" />.</returns>
    public bool GetBoolean()
    {
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      if (this.TokenType == JsonTokenType.True)
        return true;
      if (this.TokenType == JsonTokenType.False)
        return false;
      throw ThrowHelper.GetInvalidOperationException_ExpectedBoolean(this.TokenType);
    }

    /// <summary>Parses the current JSON token value from the source and decodes the Base64 encoded JSON string as a byte array.</summary>
    /// <exception cref="T:System.InvalidOperationException">The type of the JSON token is not a <see cref="F:System.Text.Json.JsonTokenType.String" />.</exception>
    /// <exception cref="T:System.FormatException">The value is not encoded as Base64 text, so it can't be decoded to bytes.
    /// 
    /// -or-
    /// 
    /// The value contains invalid or more than two padding characters.
    /// 
    /// -or-
    /// 
    /// The value is incomplete. That is, the JSON string length is not a multiple of 4.</exception>
    /// <returns>The byte array that represents the current JSON token value.</returns>
    public byte[] GetBytesFromBase64()
    {
      byte[] bytesFromBase64;
      if (!this.TryGetBytesFromBase64(out bytesFromBase64))
        throw ThrowHelper.GetFormatException(DataType.Base64String);
      return bytesFromBase64;
    }

    /// <summary>Parses the current JSON token value from the source as a <see cref="T:System.Byte" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token is not a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The numeric format of the JSON token value is incorrect (for example, it contains a fractional value or is written in scientific notation).
    /// 
    /// -or-
    /// 
    /// The JSON token value represents a number less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>The value of the UTF-8 encoded token.</returns>
    public byte GetByte()
    {
      byte num;
      if (!this.TryGetByte(out num))
        throw ThrowHelper.GetFormatException(NumericType.Byte);
      return num;
    }

    internal byte GetByteWithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      byte byteWithQuotes;
      if (!this.TryGetByteCore(out byteWithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.Byte);
      return byteWithQuotes;
    }

    /// <summary>Parses the current JSON token value from the source as an <see cref="T:System.SByte" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token is not a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The numeric format of the JSON token value is incorrect (for example, it contains a fractional value or is written in scientific notation).
    /// 
    /// -or-
    /// 
    /// The JSON token value represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to an <see cref="T:System.SByte" />.</returns>
    [CLSCompliant(false)]
    public sbyte GetSByte()
    {
      sbyte num;
      if (!this.TryGetSByte(out num))
        throw ThrowHelper.GetFormatException(NumericType.SByte);
      return num;
    }

    internal sbyte GetSByteWithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      sbyte sbyteWithQuotes;
      if (!this.TryGetSByteCore(out sbyteWithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.SByte);
      return sbyteWithQuotes;
    }

    /// <summary>Parses the current JSON token value from the source as a <see cref="T:System.Int16" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token is not a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The numeric format of the JSON token value is incorrect (for example, it contains a fractional value or is written in scientific notation).
    /// 
    /// -or-
    /// 
    /// The JSON token value represents a number less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to an <see cref="T:System.Int16" />.</returns>
    public short GetInt16()
    {
      short int16;
      if (!this.TryGetInt16(out int16))
        throw ThrowHelper.GetFormatException(NumericType.Int16);
      return int16;
    }

    internal short GetInt16WithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      short int16WithQuotes;
      if (!this.TryGetInt16Core(out int16WithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.Int16);
      return int16WithQuotes;
    }

    /// <summary>Reads the next JSON token value from the source and parses it to an <see cref="T:System.Int32" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value is of the incorrect numeric format. For example, it contains a decimal or is written in scientific notation.
    /// 
    /// -or-
    /// 
    /// The JSON token value represents a number less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to an <see cref="T:System.Int32" />.</returns>
    public int GetInt32()
    {
      int int32;
      if (!this.TryGetInt32(out int32))
        throw ThrowHelper.GetFormatException(NumericType.Int32);
      return int32;
    }

    internal int GetInt32WithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      int int32WithQuotes;
      if (!this.TryGetInt32Core(out int32WithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.Int32);
      return int32WithQuotes;
    }

    /// <summary>Reads the next JSON token value from the source and parses it to an <see cref="T:System.Int64" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value is of the incorrect numeric format. For example, it contains a decimal or is written in scientific notation.
    /// 
    /// -or-
    /// 
    /// The JSON token value represents a number less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to an <see cref="T:System.Int64" />.</returns>
    public long GetInt64()
    {
      long int64;
      if (!this.TryGetInt64(out int64))
        throw ThrowHelper.GetFormatException(NumericType.Int64);
      return int64;
    }

    internal long GetInt64WithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      long int64WithQuotes;
      if (!this.TryGetInt64Core(out int64WithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.Int64);
      return int64WithQuotes;
    }

    /// <summary>Parses the current JSON token value from the source as a <see cref="T:System.UInt16" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token is not a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The numeric format of the JSON token value is incorrect (for example, it contains a fractional value or is written in scientific notation).
    /// 
    /// -or-
    /// 
    /// The JSON token value represents a number less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to a <see cref="T:System.UInt16" />.</returns>
    [CLSCompliant(false)]
    public ushort GetUInt16()
    {
      ushort uint16;
      if (!this.TryGetUInt16(out uint16))
        throw ThrowHelper.GetFormatException(NumericType.UInt16);
      return uint16;
    }

    internal ushort GetUInt16WithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      ushort uint16WithQuotes;
      if (!this.TryGetUInt16Core(out uint16WithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.UInt16);
      return uint16WithQuotes;
    }

    /// <summary>Reads the next JSON token value from the source and parses it to a <see cref="T:System.UInt32" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value is of the incorrect numeric format. For example, it contains a decimal or is written in scientific notation.
    /// 
    /// -or-
    /// 
    /// The JSON token value represents a number less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to a <see cref="T:System.UInt32" />.</returns>
    [CLSCompliant(false)]
    public uint GetUInt32()
    {
      uint uint32;
      if (!this.TryGetUInt32(out uint32))
        throw ThrowHelper.GetFormatException(NumericType.UInt32);
      return uint32;
    }

    internal uint GetUInt32WithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      uint uint32WithQuotes;
      if (!this.TryGetUInt32Core(out uint32WithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.UInt32);
      return uint32WithQuotes;
    }

    /// <summary>Reads the next JSON token value from the source and parses it to a <see cref="T:System.UInt64" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value is of the incorrect numeric format. For example, it contains a decimal or is written in scientific notation.
    /// 
    /// -or-
    /// 
    /// The JSON token value represents a number less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to a <see cref="T:System.UInt64" />.</returns>
    [CLSCompliant(false)]
    public ulong GetUInt64()
    {
      ulong uint64;
      if (!this.TryGetUInt64(out uint64))
        throw ThrowHelper.GetFormatException(NumericType.UInt64);
      return uint64;
    }

    internal ulong GetUInt64WithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      ulong uint64WithQuotes;
      if (!this.TryGetUInt64Core(out uint64WithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.UInt64);
      return uint64WithQuotes;
    }

    /// <summary>Reads the next JSON token value from the source and parses it to a <see cref="T:System.Single" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value represents a number less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to a <see cref="T:System.Single" />.</returns>
    public float GetSingle()
    {
      float single;
      if (!this.TryGetSingle(out single))
        throw ThrowHelper.GetFormatException(NumericType.Single);
      return single;
    }

    internal float GetSingleWithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      float singleWithQuotes;
      int bytesConsumed;
      if (JsonReaderHelper.TryGetFloatingPointConstant(unescapedSpan, out singleWithQuotes) || Utf8Parser.TryParse(unescapedSpan, out singleWithQuotes, out bytesConsumed) && unescapedSpan.Length == bytesConsumed && JsonHelpers.IsFinite(singleWithQuotes))
        return singleWithQuotes;
      throw ThrowHelper.GetFormatException(NumericType.Single);
    }

    internal float GetSingleFloatingPointConstant()
    {
      float floatingPointConstant;
      if (JsonReaderHelper.TryGetFloatingPointConstant(this.GetUnescapedSpan(), out floatingPointConstant))
        return floatingPointConstant;
      throw ThrowHelper.GetFormatException(NumericType.Single);
    }

    /// <summary>Reads the next JSON token value from the source and parses it to a <see cref="T:System.Double" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value represents a number less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to a <see cref="T:System.Double" />.</returns>
    public double GetDouble()
    {
      double num;
      if (!this.TryGetDouble(out num))
        throw ThrowHelper.GetFormatException(NumericType.Double);
      return num;
    }

    internal double GetDoubleWithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      double doubleWithQuotes;
      int bytesConsumed;
      if (JsonReaderHelper.TryGetFloatingPointConstant(unescapedSpan, out doubleWithQuotes) || Utf8Parser.TryParse(unescapedSpan, out doubleWithQuotes, out bytesConsumed) && unescapedSpan.Length == bytesConsumed && JsonHelpers.IsFinite(doubleWithQuotes))
        return doubleWithQuotes;
      throw ThrowHelper.GetFormatException(NumericType.Double);
    }

    internal double GetDoubleFloatingPointConstant()
    {
      double floatingPointConstant;
      if (JsonReaderHelper.TryGetFloatingPointConstant(this.GetUnescapedSpan(), out floatingPointConstant))
        return floatingPointConstant;
      throw ThrowHelper.GetFormatException(NumericType.Double);
    }

    /// <summary>Reads the next JSON token value from the source and parses it to a <see cref="T:System.Decimal" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The UTF-8 encoded token value parsed to a <see cref="T:System.Decimal" />.</returns>
    public Decimal GetDecimal()
    {
      Decimal num;
      if (!this.TryGetDecimal(out num))
        throw ThrowHelper.GetFormatException(NumericType.Decimal);
      return num;
    }

    internal Decimal GetDecimalWithQuotes()
    {
      ReadOnlySpan<byte> unescapedSpan = this.GetUnescapedSpan();
      Decimal decimalWithQuotes;
      if (!this.TryGetDecimalCore(out decimalWithQuotes, unescapedSpan))
        throw ThrowHelper.GetFormatException(NumericType.Decimal);
      return decimalWithQuotes;
    }

    /// <summary>Reads the next JSON token value from the source and parses it to a <see cref="T:System.DateTime" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token isn't a <see cref="F:System.Text.Json.JsonTokenType.String" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value cannot be read as a <see cref="T:System.DateTime" />.
    /// 
    /// -or-
    /// 
    /// The entire UTF-8 encoded token value cannot be parsed to a <see cref="T:System.DateTime" /> value.
    /// 
    /// -or-
    /// 
    /// The JSON token value is of an unsupported format.</exception>
    /// <returns>The date and time value, if the entire UTF-8 encoded token value can be successfully parsed.</returns>
    public DateTime GetDateTime()
    {
      DateTime dateTime;
      if (!this.TryGetDateTime(out dateTime))
        throw ThrowHelper.GetFormatException(DataType.DateTime);
      return dateTime;
    }

    internal DateTime GetDateTimeNoValidation()
    {
      DateTime timeNoValidation;
      if (!this.TryGetDateTimeCore(out timeNoValidation))
        throw ThrowHelper.GetFormatException(DataType.DateTime);
      return timeNoValidation;
    }

    /// <summary>Reads the next JSON token value from the source and parses it to a <see cref="T:System.DateTimeOffset" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token isn't a <see cref="F:System.Text.Json.JsonTokenType.String" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value cannot be read as a <see cref="T:System.DateTimeOffset" />.
    /// 
    /// -or-
    /// 
    /// The entire UTF-8 encoded token value cannot be parsed to a <see cref="T:System.DateTimeOffset" /> value.
    /// 
    /// -or-
    /// 
    /// The JSON token value is of an unsupported format.</exception>
    /// <returns>The date and time offset, if the entire UTF-8 encoded token value can be successfully parsed.</returns>
    public DateTimeOffset GetDateTimeOffset()
    {
      DateTimeOffset dateTimeOffset;
      if (!this.TryGetDateTimeOffset(out dateTimeOffset))
        throw ThrowHelper.GetFormatException(DataType.DateTimeOffset);
      return dateTimeOffset;
    }

    internal DateTimeOffset GetDateTimeOffsetNoValidation()
    {
      DateTimeOffset offsetNoValidation;
      if (!this.TryGetDateTimeOffsetCore(out offsetNoValidation))
        throw ThrowHelper.GetFormatException(DataType.DateTimeOffset);
      return offsetNoValidation;
    }

    /// <summary>Reads the next JSON token value from the source and parses it to a <see cref="T:System.Guid" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token isn't a <see cref="F:System.Text.Json.JsonTokenType.String" />.</exception>
    /// <exception cref="T:System.FormatException">The JSON token value is in an unsupported format for a Guid.
    /// 
    /// -or-
    /// 
    /// The entire UTF-8 encoded token value cannot be parsed to a <see cref="T:System.Guid" /> value.</exception>
    /// <returns>The GUID value, if the entire UTF-8 encoded token value can be successfully parsed.</returns>
    public Guid GetGuid()
    {
      Guid guid;
      if (!this.TryGetGuid(out guid))
        throw ThrowHelper.GetFormatException(DataType.Guid);
      return guid;
    }

    internal Guid GetGuidNoValidation()
    {
      Guid guidNoValidation;
      if (!this.TryGetGuidCore(out guidNoValidation))
        throw ThrowHelper.GetFormatException(DataType.Guid);
      return guidNoValidation;
    }

    /// <summary>Tries to parse the current JSON token value from the source and decodes the Base64 encoded JSON string as a byte array and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the decoded binary representation of the Base64 text.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token is not a <see cref="F:System.Text.Json.JsonTokenType.String" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire token value is encoded as valid Base64 text and can be successfully decoded to bytes; otherwise, <see langword="false" />.</returns>
    public bool TryGetBytesFromBase64([NotNullWhen(true)] out byte[]? value)
    {
      if (this.TokenType != JsonTokenType.String)
        throw ThrowHelper.GetInvalidOperationException_ExpectedString(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan1;
      if (!this.HasValueSequence)
      {
        readOnlySpan1 = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan1 = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> readOnlySpan2 = readOnlySpan1;
      if (!this._stringHasEscaping)
        return JsonReaderHelper.TryDecodeBase64(readOnlySpan2, out value);
      int idx = readOnlySpan2.IndexOf<byte>((byte) 92);
      return JsonReaderHelper.TryGetUnescapedBase64Bytes(readOnlySpan2, idx, out value);
    }

    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.Byte" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.Byte" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetByte(out byte value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetByteCore(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetByteCore(out byte value, ReadOnlySpan<byte> span)
    {
      byte num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = (byte) 0;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as an <see cref="T:System.SByte" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to an <see cref="T:System.SByte" /> value; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool TryGetSByte(out sbyte value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetSByteCore(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetSByteCore(out sbyte value, ReadOnlySpan<byte> span)
    {
      sbyte num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = (sbyte) 0;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as an <see cref="T:System.Int16" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.Int16" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetInt16(out short value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetInt16Core(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetInt16Core(out short value, ReadOnlySpan<byte> span)
    {
      short num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = (short) 0;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as an <see cref="T:System.Int32" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to an <see cref="T:System.Int32" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetInt32(out int value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetInt32Core(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetInt32Core(out int value, ReadOnlySpan<byte> span)
    {
      int num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = 0;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as an <see cref="T:System.Int64" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to an <see cref="T:System.Int64" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetInt64(out long value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetInt64Core(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetInt64Core(out long value, ReadOnlySpan<byte> span)
    {
      long num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = 0L;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.UInt16" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.UInt16" /> value; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool TryGetUInt16(out ushort value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetUInt16Core(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetUInt16Core(out ushort value, ReadOnlySpan<byte> span)
    {
      ushort num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = (ushort) 0;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.UInt32" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.UInt32" /> value; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool TryGetUInt32(out uint value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetUInt32Core(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetUInt32Core(out uint value, ReadOnlySpan<byte> span)
    {
      uint num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = 0U;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.UInt64" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.UInt64" /> value; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool TryGetUInt64(out ulong value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetUInt64Core(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetUInt64Core(out ulong value, ReadOnlySpan<byte> span)
    {
      ulong num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = 0UL;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.Single" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to an <see cref="T:System.Single" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetSingle(out float value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> source = readOnlySpan;
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

    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.Double" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.Double" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetDouble(out double value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> source = readOnlySpan;
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

    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.Decimal" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The JSON token value isn't a <see cref="F:System.Text.Json.JsonTokenType.Number" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.Decimal" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetDecimal(out Decimal value)
    {
      if (this.TokenType != JsonTokenType.Number)
        throw ThrowHelper.GetInvalidOperationException_ExpectedNumber(this.TokenType);
      ReadOnlySpan<byte> readOnlySpan;
      if (!this.HasValueSequence)
      {
        readOnlySpan = this.ValueSpan;
      }
      else
      {
        ReadOnlySequence<byte> sequence = this.ValueSequence;
        readOnlySpan = (ReadOnlySpan<byte>) sequence.ToArray<byte>();
      }
      ReadOnlySpan<byte> span = readOnlySpan;
      return this.TryGetDecimalCore(out value, span);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool TryGetDecimalCore(out Decimal value, ReadOnlySpan<byte> span)
    {
      Decimal num;
      int bytesConsumed;
      if (Utf8Parser.TryParse(span, out num, out bytesConsumed) && span.Length == bytesConsumed)
      {
        value = num;
        return true;
      }
      value = 0M;
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.DateTime" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token isn't a <see cref="F:System.Text.Json.JsonTokenType.String" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.DateTime" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetDateTime(out DateTime value)
    {
      if (this.TokenType != JsonTokenType.String)
        throw ThrowHelper.GetInvalidOperationException_ExpectedString(this.TokenType);
      return this.TryGetDateTimeCore(out value);
    }


    #nullable disable
    internal unsafe bool TryGetDateTimeCore(out DateTime value)
    {
      ReadOnlySpan<byte> readOnlySpan = (ReadOnlySpan<byte>) new Span<byte>();
      int upperBound = this._stringHasEscaping ? 252 : 42;
      ReadOnlySpan<byte> source1;
      if (this.HasValueSequence)
      {
        long length1 = this.ValueSequence.Length;
        if (!JsonHelpers.IsInRangeInclusive(length1, 10L, (long) upperBound))
        {
          value = new DateTime();
          return false;
        }
        int length2 = this._stringHasEscaping ? 252 : 42;
        // ISSUE: untyped stack allocation
        Span<byte> destination = new Span<byte>((void*) __untypedstackalloc((IntPtr) (uint) length2), length2);
        ReadOnlySequence<byte> source2 = this.ValueSequence;
        source2.CopyTo<byte>(destination);
        source1 = (ReadOnlySpan<byte>) destination.Slice(0, (int) length1);
      }
      else
      {
        if (!JsonHelpers.IsInRangeInclusive(this.ValueSpan.Length, 10, upperBound))
        {
          value = new DateTime();
          return false;
        }
        source1 = this.ValueSpan;
      }
      if (this._stringHasEscaping)
        return JsonReaderHelper.TryGetEscapedDateTime(source1, out value);
      DateTime dateTime;
      if (JsonHelpers.TryParseAsISO(source1, out dateTime))
      {
        value = dateTime;
        return true;
      }
      value = new DateTime();
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.DateTimeOffset" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token isn't a <see cref="F:System.Text.Json.JsonTokenType.String" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.DateTimeOffset" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetDateTimeOffset(out DateTimeOffset value)
    {
      if (this.TokenType != JsonTokenType.String)
        throw ThrowHelper.GetInvalidOperationException_ExpectedString(this.TokenType);
      return this.TryGetDateTimeOffsetCore(out value);
    }


    #nullable disable
    internal unsafe bool TryGetDateTimeOffsetCore(out DateTimeOffset value)
    {
      ReadOnlySpan<byte> readOnlySpan = (ReadOnlySpan<byte>) new Span<byte>();
      int upperBound = this._stringHasEscaping ? 252 : 42;
      ReadOnlySpan<byte> source1;
      if (this.HasValueSequence)
      {
        long length1 = this.ValueSequence.Length;
        if (!JsonHelpers.IsInRangeInclusive(length1, 10L, (long) upperBound))
        {
          value = new DateTimeOffset();
          return false;
        }
        int length2 = this._stringHasEscaping ? 252 : 42;
        // ISSUE: untyped stack allocation
        Span<byte> destination = new Span<byte>((void*) __untypedstackalloc((IntPtr) (uint) length2), length2);
        ReadOnlySequence<byte> source2 = this.ValueSequence;
        source2.CopyTo<byte>(destination);
        source1 = (ReadOnlySpan<byte>) destination.Slice(0, (int) length1);
      }
      else
      {
        if (!JsonHelpers.IsInRangeInclusive(this.ValueSpan.Length, 10, upperBound))
        {
          value = new DateTimeOffset();
          return false;
        }
        source1 = this.ValueSpan;
      }
      if (this._stringHasEscaping)
        return JsonReaderHelper.TryGetEscapedDateTimeOffset(source1, out value);
      DateTimeOffset dateTimeOffset;
      if (JsonHelpers.TryParseAsISO(source1, out dateTimeOffset))
      {
        value = dateTimeOffset;
        return true;
      }
      value = new DateTimeOffset();
      return false;
    }


    #nullable enable
    /// <summary>Tries to parse the current JSON token value from the source as a <see cref="T:System.Guid" /> and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <exception cref="T:System.InvalidOperationException">The value of the JSON token isn't a <see cref="F:System.Text.Json.JsonTokenType.String" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire UTF-8 encoded token value can be successfully parsed to a <see cref="T:System.Guid" /> value; otherwise, <see langword="false" />.</returns>
    public bool TryGetGuid(out Guid value)
    {
      if (this.TokenType != JsonTokenType.String)
        throw ThrowHelper.GetInvalidOperationException_ExpectedString(this.TokenType);
      return this.TryGetGuidCore(out value);
    }


    #nullable disable
    internal unsafe bool TryGetGuidCore(out Guid value)
    {
      ReadOnlySpan<byte> readOnlySpan = (ReadOnlySpan<byte>) new Span<byte>();
      int num = this._stringHasEscaping ? 216 : 36;
      ReadOnlySpan<byte> source1;
      if (this.HasValueSequence)
      {
        long length1 = this.ValueSequence.Length;
        if (length1 > (long) num)
        {
          value = new Guid();
          return false;
        }
        int length2 = this._stringHasEscaping ? 216 : 36;
        // ISSUE: untyped stack allocation
        Span<byte> destination = new Span<byte>((void*) __untypedstackalloc((IntPtr) (uint) length2), length2);
        ReadOnlySequence<byte> source2 = this.ValueSequence;
        source2.CopyTo<byte>(destination);
        source1 = (ReadOnlySpan<byte>) destination.Slice(0, (int) length1);
      }
      else
      {
        if (this.ValueSpan.Length > num)
        {
          value = new Guid();
          return false;
        }
        source1 = this.ValueSpan;
      }
      if (this._stringHasEscaping)
        return JsonReaderHelper.TryGetEscapedGuid(source1, out value);
      Guid guid;
      if (source1.Length == 36 && Utf8Parser.TryParse(source1, out guid, out int _, 'D'))
      {
        value = guid;
        return true;
      }
      value = new Guid();
      return false;
    }

    private readonly struct PartialStateForRollback
    {
      public readonly long _prevTotalConsumed;
      public readonly long _prevBytePositionInLine;
      public readonly int _prevConsumed;
      public readonly SequencePosition _prevCurrentPosition;

      public PartialStateForRollback(
        long totalConsumed,
        long bytePositionInLine,
        int consumed,
        SequencePosition currentPosition)
      {
        this._prevTotalConsumed = totalConsumed;
        this._prevBytePositionInLine = bytePositionInLine;
        this._prevConsumed = consumed;
        this._prevCurrentPosition = currentPosition;
      }

      public SequencePosition GetStartPosition(int offset = 0) => new SequencePosition(this._prevCurrentPosition.GetObject(), this._prevCurrentPosition.GetInteger() + this._prevConsumed + offset);
    }
  }
}
