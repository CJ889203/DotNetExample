// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonReaderState
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

namespace System.Text.Json
{
  /// <summary>Defines an opaque type that holds and saves all the relevant state information, which must be provided to the <see cref="T:System.Text.Json.Utf8JsonReader" /> to continue reading after processing incomplete data.</summary>
  public struct JsonReaderState
  {
    internal long _lineNumber;
    internal long _bytePositionInLine;
    internal bool _inObject;
    internal bool _isNotPrimitive;
    internal bool _stringHasEscaping;
    internal bool _trailingCommaBeforeComment;
    internal JsonTokenType _tokenType;
    internal JsonTokenType _previousTokenType;
    internal JsonReaderOptions _readerOptions;
    internal BitStack _bitStack;

    /// <summary>Constructs a new <see cref="T:System.Text.Json.JsonReaderState" /> instance.</summary>
    /// <param name="options">Defines the customized behavior of the <see cref="T:System.Text.Json.Utf8JsonReader" /> that is different from the JSON RFC (for example how to handle comments, or the maximum depth allowed when reading). By default, the <see cref="T:System.Text.Json.Utf8JsonReader" /> follows the JSON RFC strictly (comments within the JSON are invalid) and reads up to a maximum depth of 64.</param>
    /// <exception cref="T:System.ArgumentException">The maximum depth is set to a non-positive value (&lt; 0).</exception>
    public JsonReaderState(JsonReaderOptions options = default (JsonReaderOptions))
    {
      this._lineNumber = 0L;
      this._bytePositionInLine = 0L;
      this._inObject = false;
      this._isNotPrimitive = false;
      this._stringHasEscaping = false;
      this._trailingCommaBeforeComment = false;
      this._tokenType = JsonTokenType.None;
      this._previousTokenType = JsonTokenType.None;
      this._readerOptions = options;
      this._bitStack = new BitStack();
    }

    /// <summary>Gets the custom behavior to use when reading JSON data using the <see cref="T:System.Text.Json.Utf8JsonReader" /> struct that may deviate from strict adherence to the JSON specification, which is the default behavior.</summary>
    /// <returns>The custom behavior to use when reading JSON data.</returns>
    public JsonReaderOptions Options => this._readerOptions;
  }
}
