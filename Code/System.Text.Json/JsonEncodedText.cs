// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonEncodedText
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Provides methods to transform UTF-8 or UTF-16 encoded text into a form that is suitable for JSON.</summary>
  public readonly struct JsonEncodedText : IEquatable<JsonEncodedText>
  {

    #nullable disable
    internal readonly byte[] _utf8Value;
    internal readonly string _value;


    #nullable enable
    /// <summary>Gets the UTF-8 encoded representation of the pre-encoded JSON text.</summary>
    /// <returns>The UTF-8 encoded representation of the pre-encoded JSON text.</returns>
    public ReadOnlySpan<byte> EncodedUtf8Bytes => (ReadOnlySpan<byte>) this._utf8Value;


    #nullable disable
    private JsonEncodedText(byte[] utf8Value)
    {
      this._value = JsonReaderHelper.GetTextFromUtf8((ReadOnlySpan<byte>) utf8Value);
      this._utf8Value = utf8Value;
    }


    #nullable enable
    /// <summary>Encodes the string text value as a JSON string.</summary>
    /// <param name="value">The value to convert to JSON encoded text.</param>
    /// <param name="encoder">The encoder to use when escaping the string, or <see langword="null" /> to use the default encoder.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="value" /> is too large.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> contains invalid UTF-16 characters.</exception>
    /// <returns>The encoded JSON text.</returns>
    public static JsonEncodedText Encode(string value, JavaScriptEncoder? encoder = null) => value != null ? JsonEncodedText.Encode(value.AsSpan(), encoder) : throw new ArgumentNullException(nameof (value));

    /// <summary>Encodes a specified text value as a JSON string.</summary>
    /// <param name="value">The value to convert to JSON encoded text.</param>
    /// <param name="encoder">The encoder to use when escaping the string, or <see langword="null" /> to use the default encoder.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="value" /> is too large.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> contains invalid UTF-16 characters.</exception>
    /// <returns>The encoded JSON text.</returns>
    public static JsonEncodedText Encode(
      ReadOnlySpan<char> value,
      JavaScriptEncoder? encoder = null)
    {
      return value.Length == 0 ? new JsonEncodedText(Array.Empty<byte>()) : JsonEncodedText.TranscodeAndEncode(value, encoder);
    }


    #nullable disable
    private static JsonEncodedText TranscodeAndEncode(
      ReadOnlySpan<char> value,
      JavaScriptEncoder encoder)
    {
      JsonWriterHelper.ValidateValue(value);
      int utf8ByteCount = JsonReaderHelper.GetUtf8ByteCount(value);
      byte[] numArray = ArrayPool<byte>.Shared.Rent(utf8ByteCount);
      int utf8FromText = JsonReaderHelper.GetUtf8FromText(value, (Span<byte>) numArray);
      JsonEncodedText jsonEncodedText = JsonEncodedText.EncodeHelper((ReadOnlySpan<byte>) numArray.AsSpan<byte>(0, utf8FromText), encoder);
      numArray.AsSpan<byte>(0, utf8ByteCount).Clear();
      ArrayPool<byte>.Shared.Return(numArray);
      return jsonEncodedText;
    }


    #nullable enable
    /// <summary>Encodes a UTF-8 text value as a JSON string.</summary>
    /// <param name="utf8Value">The UTF-8 encoded text to convert to JSON encoded text.</param>
    /// <param name="encoder">The encoder to use when escaping the string, or <see langword="null" /> to use the default encoder.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="utf8Value" /> is too large.
    /// 
    /// -or-
    /// 
    /// <paramref name="utf8Value" /> contains invalid UTF-8 bytes.</exception>
    /// <returns>The encoded JSON text.</returns>
    public static JsonEncodedText Encode(
      ReadOnlySpan<byte> utf8Value,
      JavaScriptEncoder? encoder = null)
    {
      if (utf8Value.Length == 0)
        return new JsonEncodedText(Array.Empty<byte>());
      JsonWriterHelper.ValidateValue(utf8Value);
      return JsonEncodedText.EncodeHelper(utf8Value, encoder);
    }


    #nullable disable
    private static JsonEncodedText EncodeHelper(
      ReadOnlySpan<byte> utf8Value,
      JavaScriptEncoder encoder)
    {
      int firstEscapeIndexVal = JsonWriterHelper.NeedsEscaping(utf8Value, encoder);
      return firstEscapeIndexVal != -1 ? new JsonEncodedText(JsonHelpers.EscapeValue(utf8Value, firstEscapeIndexVal, encoder)) : new JsonEncodedText(utf8Value.ToArray());
    }

    /// <summary>Determines whether this instance and another specified <see cref="T:System.Text.Json.JsonEncodedText" /> instance have the same value.</summary>
    /// <param name="other">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if this instance and <paramref name="other" /> have the same value; otherwise, <see langword="false" />.</returns>
    public bool Equals(JsonEncodedText other) => this._value == null ? other._value == null : this._value.Equals(other._value);


    #nullable enable
    /// <summary>Determines whether this instance and a specified object, which must also be a <see cref="T:System.Text.Json.JsonEncodedText" /> instance, have the same value.</summary>
    /// <param name="obj">The object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is JsonEncodedText other && this.Equals(other);

    /// <summary>Converts the value of this instance to a <see cref="T:System.String" />.</summary>
    /// <returns>The underlying UTF-16 encoded string.</returns>
    public override string ToString() => this._value ?? string.Empty;

    /// <summary>Returns the hash code for this <see cref="T:System.Text.Json.JsonEncodedText" />.</summary>
    /// <returns>The hash code for this instance.</returns>
    public override int GetHashCode() => this._value != null ? this._value.GetHashCode() : 0;
  }
}
