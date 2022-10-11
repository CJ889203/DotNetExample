// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonElement
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Represents a specific JSON value within a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
  [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
  public readonly struct JsonElement
  {

    #nullable disable
    private readonly JsonDocument _parent;
    private readonly int _idx;

    internal JsonElement(JsonDocument parent, int idx)
    {
      this._parent = parent;
      this._idx = idx;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private JsonTokenType TokenType
    {
      get
      {
        JsonDocument parent = this._parent;
        return parent == null ? JsonTokenType.None : parent.GetJsonTokenType(this._idx);
      }
    }

    /// <summary>Gets the type of the current JSON value.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The type of the current JSON value.</returns>
    public JsonValueKind ValueKind => this.TokenType.ToValueKind();

    /// <summary>Gets the value at the specified index if the current value is an <see cref="F:System.Text.Json.JsonValueKind.Array" />.</summary>
    /// <param name="index">The item index.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Array" />.</exception>
    /// <exception cref="T:System.IndexOutOfRangeException">
    /// <paramref name="index" /> is not in the range [0, <see cref="M:System.Text.Json.JsonElement.GetArrayLength" />()).</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The value at the specified index.</returns>
    public JsonElement this[int index]
    {
      get
      {
        this.CheckValidInstance();
        return this._parent.GetArrayIndexElement(this._idx, index);
      }
    }

    /// <summary>Gets the number of values contained within the current array value.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Array" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The number of values contained within the current array value.</returns>
    public int GetArrayLength()
    {
      this.CheckValidInstance();
      return this._parent.GetArrayLength(this._idx);
    }


    #nullable enable
    /// <summary>Gets a <see cref="T:System.Text.Json.JsonElement" /> representing the value of a required property identified by <paramref name="propertyName" />.</summary>
    /// <param name="propertyName">The name of the property whose value is to be returned.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Object" />.</exception>
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">No property was found with the requested name.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonElement" /> representing the value of the requested property.</returns>
    public JsonElement GetProperty(string propertyName)
    {
      if (propertyName == null)
        throw new ArgumentNullException(nameof (propertyName));
      JsonElement property;
      if (this.TryGetProperty(propertyName, out property))
        return property;
      throw new KeyNotFoundException();
    }

    /// <summary>Gets a <see cref="T:System.Text.Json.JsonElement" /> representing the value of a required property identified by <paramref name="propertyName" />.</summary>
    /// <param name="propertyName">The name of the property whose value is to be returned.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Object" />.</exception>
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">No property was found with the requested name.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonElement" /> representing the value of the requested property.</returns>
    public JsonElement GetProperty(ReadOnlySpan<char> propertyName)
    {
      JsonElement property;
      if (this.TryGetProperty(propertyName, out property))
        return property;
      throw new KeyNotFoundException();
    }

    /// <summary>Gets a <see cref="T:System.Text.Json.JsonElement" /> representing the value of a required property identified by <paramref name="utf8PropertyName" />.</summary>
    /// <param name="utf8PropertyName">The UTF-8 representation (with no Byte-Order-Mark (BOM)) of the name of the property to return.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Object" />.</exception>
    /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">No property was found with the requested name.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonElement" /> representing the value of the requested property.</returns>
    public JsonElement GetProperty(ReadOnlySpan<byte> utf8PropertyName)
    {
      JsonElement property;
      if (this.TryGetProperty(utf8PropertyName, out property))
        return property;
      throw new KeyNotFoundException();
    }

    /// <summary>Looks for a property named <paramref name="propertyName" /> in the current object, returning a value that indicates whether or not such a property exists. When the property exists, its value is assigned to the <paramref name="value" /> argument.</summary>
    /// <param name="propertyName">The name of the property to find.</param>
    /// <param name="value">When this method returns, contains the value of the specified property.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Object" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the property was found; otherwise, <see langword="false" />.</returns>
    public bool TryGetProperty(string propertyName, out JsonElement value)
    {
      if (propertyName == null)
        throw new ArgumentNullException(nameof (propertyName));
      return this.TryGetProperty(propertyName.AsSpan(), out value);
    }

    /// <summary>Looks for a property named <paramref name="propertyName" /> in the current object, returning a value that indicates whether or not such a property exists. When the property exists, the method assigns its value to the <paramref name="value" /> argument.</summary>
    /// <param name="propertyName">The name of the property to find.</param>
    /// <param name="value">When this method returns, contains the value of the specified property.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Object" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the property was found; otherwise, <see langword="false" />.</returns>
    public bool TryGetProperty(ReadOnlySpan<char> propertyName, out JsonElement value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetNamedPropertyValue(this._idx, propertyName, out value);
    }

    /// <summary>Looks for a property named <paramref name="utf8PropertyName" /> in the current object, returning a value that indicates whether or not such a property exists. When the property exists, the method assigns its value to the <paramref name="value" /> argument.</summary>
    /// <param name="utf8PropertyName">The UTF-8 (with no Byte-Order-Mark (BOM)) representation of the name of the property to return.</param>
    /// <param name="value">Receives the value of the located property.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Object" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the property was found; otherwise, <see langword="false" />.</returns>
    public bool TryGetProperty(ReadOnlySpan<byte> utf8PropertyName, out JsonElement value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetNamedPropertyValue(this._idx, utf8PropertyName, out value);
    }

    /// <summary>Gets the value of the element as a <see cref="T:System.Boolean" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is neither <see cref="F:System.Text.Json.JsonValueKind.True" /> nor <see cref="F:System.Text.Json.JsonValueKind.False" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The value of the element as a <see cref="T:System.Boolean" />.</returns>
    public bool GetBoolean()
    {
      JsonTokenType tokenType = this.TokenType;
      switch (tokenType)
      {
        case JsonTokenType.True:
          return true;
        case JsonTokenType.False:
          return false;
        default:
          throw ThrowHelper.GetJsonElementWrongTypeException("Boolean", tokenType);
      }
    }

    /// <summary>Gets the value of the element as a <see cref="T:System.String" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is neither <see cref="F:System.Text.Json.JsonValueKind.String" /> nor <see cref="F:System.Text.Json.JsonValueKind.Null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The value of the element as a <see cref="T:System.String" />.</returns>
    public string? GetString()
    {
      this.CheckValidInstance();
      return this._parent.GetString(this._idx, JsonTokenType.String);
    }

    /// <summary>Attempts to represent the current JSON string as a byte array, assuming that it is Base64 encoded.</summary>
    /// <param name="value">If the method succeeds, contains the decoded binary representation of the Base64 text.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the entire token value is encoded as valid Base64 text and can be successfully decoded to bytes; otherwise, <see langword="false" />.</returns>
    public bool TryGetBytesFromBase64([NotNullWhen(true)] out byte[]? value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the value of the element as a byte array.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <exception cref="T:System.FormatException">The value is not encoded as Base64 text and hence cannot be decoded to bytes.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The value decoded as a byte array.</returns>
    public byte[] GetBytesFromBase64()
    {
      byte[] bytesFromBase64;
      if (this.TryGetBytesFromBase64(out bytesFromBase64))
        return bytesFromBase64;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON number as an <see cref="T:System.SByte" />.</summary>
    /// <param name="value">When the method returns, contains the signed byte equivalent of the current JSON number if the conversion succeeded.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as an <see cref="T:System.SByte" />; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool TryGetSByte(out sbyte value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as an <see cref="T:System.SByte" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as an <see cref="T:System.SByte" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as an <see cref="T:System.SByte" />.</returns>
    [CLSCompliant(false)]
    public sbyte GetSByte()
    {
      sbyte num;
      if (this.TryGetSByte(out num))
        return num;
      throw new FormatException();
    }

    /// <summary>Attempts to represent the current JSON number as a <see cref="T:System.Byte" />.</summary>
    /// <param name="value">When the method returns, contains the byte equivalent of the current JSON number if the conversion succeeded.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as a <see cref="T:System.Byte" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetByte(out byte value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as a <see cref="T:System.Byte" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.Byte" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as a <see cref="T:System.Byte" />.</returns>
    public byte GetByte()
    {
      byte num;
      if (this.TryGetByte(out num))
        return num;
      throw new FormatException();
    }

    /// <summary>Attempts to represent the current JSON number as an <see cref="T:System.Int16" />.</summary>
    /// <param name="value">When the method returns, contains the 16-bit integer equivalent of the current JSON number if the conversion succeeded.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as an <see cref="T:System.Int16" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetInt16(out short value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as an <see cref="T:System.Int16" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as an <see cref="T:System.Int16" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as an <see cref="T:System.Int16" />.</returns>
    public short GetInt16()
    {
      short int16;
      if (this.TryGetInt16(out int16))
        return int16;
      throw new FormatException();
    }

    /// <summary>Attempts to represent the current JSON number as a <see cref="T:System.UInt16" />.</summary>
    /// <param name="value">When the method returns, contains the unsigned 16-bit integer equivalent of the current JSON number if the conversion succeeded.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as a <see cref="T:System.UInt16" />; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool TryGetUInt16(out ushort value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as a <see cref="T:System.UInt16" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.UInt16" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as a <see cref="T:System.UInt16" />.</returns>
    [CLSCompliant(false)]
    public ushort GetUInt16()
    {
      ushort uint16;
      if (this.TryGetUInt16(out uint16))
        return uint16;
      throw new FormatException();
    }

    /// <summary>Attempts to represent the current JSON number as an <see cref="T:System.Int32" />.</summary>
    /// <param name="value">When this method returns, contains the 32-bit integer value equivalent to the current JSON number.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as an <see cref="T:System.Int32" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetInt32(out int value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as an <see cref="T:System.Int32" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as an <see cref="T:System.Int32" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as an <see cref="T:System.Int32" />.</returns>
    public int GetInt32()
    {
      int int32;
      if (this.TryGetInt32(out int32))
        return int32;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON number as a <see cref="T:System.UInt32" />.</summary>
    /// <param name="value">When this method returns, contains unsigned 32-bit integer value equivalent to the current JSON number.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as a <see cref="T:System.UInt32" />; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool TryGetUInt32(out uint value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as a <see cref="T:System.UInt32" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.UInt32" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as a <see cref="T:System.UInt32" />.</returns>
    [CLSCompliant(false)]
    public uint GetUInt32()
    {
      uint uint32;
      if (this.TryGetUInt32(out uint32))
        return uint32;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON number as a <see cref="T:System.Int64" />.</summary>
    /// <param name="value">When this method returns, contains the 64-bit integer value equivalent to the current JSON number.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as a <see cref="T:System.Int64" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetInt64(out long value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as an <see cref="T:System.Int64" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.Int64" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as an <see cref="T:System.Int64" />.</returns>
    public long GetInt64()
    {
      long int64;
      if (this.TryGetInt64(out int64))
        return int64;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON number as a <see cref="T:System.UInt64" />.</summary>
    /// <param name="value">When this method returns, contains unsigned 64-bit integer value equivalent to the current JSON number.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as a <see cref="T:System.UInt64" />; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public bool TryGetUInt64(out ulong value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as a <see cref="T:System.UInt64" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.UInt64" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as a <see cref="T:System.UInt64" />.</returns>
    [CLSCompliant(false)]
    public ulong GetUInt64()
    {
      ulong uint64;
      if (this.TryGetUInt64(out uint64))
        return uint64;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON number as a <see cref="T:System.Double" />.</summary>
    /// <param name="value">When this method returns, contains a double-precision floating point value equivalent to the current JSON number.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as a <see cref="T:System.Double" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetDouble(out double value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as a <see cref="T:System.Double" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.Double" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as a <see cref="T:System.Double" />.</returns>
    public double GetDouble()
    {
      double num;
      if (this.TryGetDouble(out num))
        return num;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON number as a <see cref="T:System.Single" />.</summary>
    /// <param name="value">When this method returns, contains the single-precision floating point value equivalent to the current JSON number.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as a <see cref="T:System.Single" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetSingle(out float value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as a <see cref="T:System.Single" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.Single" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as a <see cref="T:System.Single" />.</returns>
    public float GetSingle()
    {
      float single;
      if (this.TryGetSingle(out single))
        return single;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON number as a <see cref="T:System.Decimal" />.</summary>
    /// <param name="value">When this method returns, contains the decimal equivalent of the current JSON number.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the number can be represented as a <see cref="T:System.Decimal" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetDecimal(out Decimal value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the current JSON number as a <see cref="T:System.Decimal" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Number" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.Decimal" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The current JSON number as a <see cref="T:System.Decimal" />.</returns>
    public Decimal GetDecimal()
    {
      Decimal num;
      if (this.TryGetDecimal(out num))
        return num;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON string as a <see cref="T:System.DateTime" />.</summary>
    /// <param name="value">When this method returns, contains the date and time value equivalent to the current JSON string.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the string can be represented as a <see cref="T:System.DateTime" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetDateTime(out DateTime value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the value of the element as a <see cref="T:System.DateTime" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be read as a <see cref="T:System.DateTime" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The value of the element as a <see cref="T:System.DateTime" />.</returns>
    public DateTime GetDateTime()
    {
      DateTime dateTime;
      if (this.TryGetDateTime(out dateTime))
        return dateTime;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON string as a <see cref="T:System.DateTimeOffset" />.</summary>
    /// <param name="value">When this method returns, contains the date and time equivalent to the current JSON string.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the string can be represented as a <see cref="T:System.DateTimeOffset" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetDateTimeOffset(out DateTimeOffset value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the value of the element as a <see cref="T:System.DateTimeOffset" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be read as a <see cref="T:System.DateTimeOffset" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The value of the element as a <see cref="T:System.DateTimeOffset" />.</returns>
    public DateTimeOffset GetDateTimeOffset()
    {
      DateTimeOffset dateTimeOffset;
      if (this.TryGetDateTimeOffset(out dateTimeOffset))
        return dateTimeOffset;
      throw ThrowHelper.GetFormatException();
    }

    /// <summary>Attempts to represent the current JSON string as a <see cref="T:System.Guid" />.</summary>
    /// <param name="value">When this method returns, contains the GUID equivalent to the current JSON string.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the string can be represented as a <see cref="T:System.Guid" />; otherwise, <see langword="false" />.</returns>
    public bool TryGetGuid(out Guid value)
    {
      this.CheckValidInstance();
      return this._parent.TryGetValue(this._idx, out value);
    }

    /// <summary>Gets the value of the element as a <see cref="T:System.Guid" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <exception cref="T:System.FormatException">The value cannot be represented as a <see cref="T:System.Guid" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The value of the element as a <see cref="T:System.Guid" />.</returns>
    public Guid GetGuid()
    {
      Guid guid;
      if (this.TryGetGuid(out guid))
        return guid;
      throw ThrowHelper.GetFormatException();
    }


    #nullable disable
    internal string GetPropertyName()
    {
      this.CheckValidInstance();
      return this._parent.GetNameOfPropertyValue(this._idx);
    }


    #nullable enable
    /// <summary>Gets a string that represents the original input data backing this value.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>The original input data backing this value.</returns>
    public string GetRawText()
    {
      this.CheckValidInstance();
      return this._parent.GetRawValueAsString(this._idx);
    }


    #nullable disable
    internal ReadOnlyMemory<byte> GetRawValue()
    {
      this.CheckValidInstance();
      return this._parent.GetRawValue(this._idx, true);
    }

    internal string GetPropertyRawText()
    {
      this.CheckValidInstance();
      return this._parent.GetPropertyRawValueAsString(this._idx);
    }


    #nullable enable
    /// <summary>Compares a specified string to the string value of this element.</summary>
    /// <param name="text">The text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the string value of this element matches <paramref name="text" />; otherwise, <see langword="false" />.</returns>
    public bool ValueEquals(string? text) => this.TokenType == JsonTokenType.Null ? text == null : this.TextEqualsHelper(text.AsSpan(), false);

    /// <summary>Compares the text represented by a UTF8-encoded byte span to the string value of this element.</summary>
    /// <param name="utf8Text">The UTF-8 encoded text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the string value of this element has the same UTF-8 encoding as
    ///       <paramref name="utf8Text" />; otherwise, <see langword="false" />.</returns>
    public bool ValueEquals(ReadOnlySpan<byte> utf8Text) => this.TokenType == JsonTokenType.Null ? utf8Text == new ReadOnlySpan<byte>() : this.TextEqualsHelper(utf8Text, false, true);

    /// <summary>Compares a specified read-only character span to the string value of this element.</summary>
    /// <param name="text">The text to compare against.</param>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.String" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the string value of this element matches <paramref name="text" />; otherwise, <see langword="false" />.</returns>
    public bool ValueEquals(ReadOnlySpan<char> text) => this.TokenType == JsonTokenType.Null ? text == new ReadOnlySpan<char>() : this.TextEqualsHelper(text, false);


    #nullable disable
    internal bool TextEqualsHelper(
      ReadOnlySpan<byte> utf8Text,
      bool isPropertyName,
      bool shouldUnescape)
    {
      this.CheckValidInstance();
      return this._parent.TextEquals(this._idx, utf8Text, isPropertyName, shouldUnescape);
    }

    internal bool TextEqualsHelper(ReadOnlySpan<char> text, bool isPropertyName)
    {
      this.CheckValidInstance();
      return this._parent.TextEquals(this._idx, text, isPropertyName);
    }


    #nullable enable
    /// <summary>Writes the element to the specified writer as a JSON value.</summary>
    /// <param name="writer">The writer to which to write the element.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="writer" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Text.Json.JsonElement.ValueKind" /> of this value is <see cref="F:System.Text.Json.JsonValueKind.Undefined" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    public void WriteTo(Utf8JsonWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      this.CheckValidInstance();
      this._parent.WriteElementTo(this._idx, writer);
    }

    /// <summary>Gets an enumerator to enumerate the values in the JSON array represented by this JsonElement.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Array" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>An enumerator to enumerate the values in the JSON array represented by this JsonElement.</returns>
    public JsonElement.ArrayEnumerator EnumerateArray()
    {
      this.CheckValidInstance();
      JsonTokenType tokenType = this.TokenType;
      if (tokenType != JsonTokenType.StartArray)
        throw ThrowHelper.GetJsonElementWrongTypeException(JsonTokenType.StartArray, tokenType);
      return new JsonElement.ArrayEnumerator(this);
    }

    /// <summary>Gets an enumerator to enumerate the properties in the JSON object represented by this JsonElement.</summary>
    /// <exception cref="T:System.InvalidOperationException">This value's <see cref="P:System.Text.Json.JsonElement.ValueKind" /> is not <see cref="F:System.Text.Json.JsonValueKind.Object" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>An enumerator to enumerate the properties in the JSON object represented by this JsonElement.</returns>
    public JsonElement.ObjectEnumerator EnumerateObject()
    {
      this.CheckValidInstance();
      JsonTokenType tokenType = this.TokenType;
      if (tokenType != JsonTokenType.StartObject)
        throw ThrowHelper.GetJsonElementWrongTypeException(JsonTokenType.StartObject, tokenType);
      return new JsonElement.ObjectEnumerator(this);
    }

    /// <summary>Gets a string representation for the current value appropriate to the value type.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The parent <see cref="T:System.Text.Json.JsonDocument" /> has been disposed.</exception>
    /// <returns>A string representation for the current value appropriate to the value type.</returns>
    public override string ToString()
    {
      switch (this.TokenType)
      {
        case JsonTokenType.None:
        case JsonTokenType.Null:
          return string.Empty;
        case JsonTokenType.StartObject:
        case JsonTokenType.StartArray:
        case JsonTokenType.Number:
          return this._parent.GetRawValueAsString(this._idx);
        case JsonTokenType.String:
          return this.GetString();
        case JsonTokenType.True:
          return bool.TrueString;
        case JsonTokenType.False:
          return bool.FalseString;
        default:
          return string.Empty;
      }
    }

    /// <summary>Gets a JsonElement that can be safely stored beyond the lifetime of the original <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <returns>A JsonElement that can be safely stored beyond the lifetime of the original <see cref="T:System.Text.Json.JsonDocument" />.</returns>
    public JsonElement Clone()
    {
      this.CheckValidInstance();
      return !this._parent.IsDisposable ? this : this._parent.CloneElement(this._idx);
    }

    private void CheckValidInstance()
    {
      if (this._parent == null)
        throw new InvalidOperationException();
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string DebuggerDisplay
    {
      get
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 2);
        interpolatedStringHandler.AppendLiteral("ValueKind = ");
        interpolatedStringHandler.AppendFormatted<JsonValueKind>(this.ValueKind);
        interpolatedStringHandler.AppendLiteral(" : \"");
        interpolatedStringHandler.AppendFormatted(this.ToString());
        interpolatedStringHandler.AppendLiteral("\"");
        return interpolatedStringHandler.ToStringAndClear();
      }
    }

    /// <summary>Parses one JSON value (including objects or arrays) from the provided reader.</summary>
    /// <param name="reader">The reader to read.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="reader" /> is using unsupported options.</exception>
    /// <exception cref="T:System.ArgumentException">The current <paramref name="reader" /> token does not start or represent a value.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">A value could not be read from the reader.</exception>
    /// <returns>A JsonElement representing the value (and nested values) read from the reader.</returns>
    public static JsonElement ParseValue(ref Utf8JsonReader reader)
    {
      JsonDocument document;
      JsonDocument.TryParseValue(ref reader, out document, true, false);
      return document.RootElement;
    }


    #nullable disable
    internal static JsonElement ParseValue(Stream utf8Json, JsonDocumentOptions options) => JsonDocument.ParseValue(utf8Json, options).RootElement;

    internal static JsonElement ParseValue(
      ReadOnlySpan<byte> utf8Json,
      JsonDocumentOptions options)
    {
      return JsonDocument.ParseValue(utf8Json, options).RootElement;
    }

    internal static JsonElement ParseValue(string json, JsonDocumentOptions options) => JsonDocument.ParseValue(json, options).RootElement;


    #nullable enable
    /// <summary>Attempts to parse one JSON value (including objects or arrays) from the provided reader.</summary>
    /// <param name="reader">The reader to read.</param>
    /// <param name="element">Receives the parsed element.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="reader" /> is using unsupported options.</exception>
    /// <exception cref="T:System.ArgumentException">The current <paramref name="reader" /> token does not start or represent a value.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">A value could not be read from the reader.</exception>
    /// <returns>
    /// <see langword="true" /> if a value was read and parsed into a JsonElement; <see langword="false" /> if the reader ran out of data while parsing.
    ///        All other situations result in an exception being thrown.</returns>
    public static bool TryParseValue(ref Utf8JsonReader reader, [NotNullWhen(true)] out JsonElement? element)
    {
      JsonDocument document;
      bool flag = JsonDocument.TryParseValue(ref reader, out document, false, false);
      element = document?.RootElement;
      return flag;
    }

    /// <summary>Represents an enumerator for the contents of a JSON array.</summary>
    [System.Diagnostics.DebuggerDisplay("{Current,nq}")]
    public struct ArrayEnumerator : 
      IEnumerable<JsonElement>,
      IEnumerable,
      IEnumerator<JsonElement>,
      IEnumerator,
      IDisposable
    {
      private readonly JsonElement _target;
      private int _curIdx;
      private readonly int _endIdxOrVersion;

      internal ArrayEnumerator(JsonElement target)
      {
        this._target = target;
        this._curIdx = -1;
        this._endIdxOrVersion = target._parent.GetEndIndex(this._target._idx, false);
      }

      /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
      /// <returns>The element in the collection at the current position of the enumerator.</returns>
      public JsonElement Current => this._curIdx < 0 ? new JsonElement() : new JsonElement(this._target._parent, this._curIdx);

      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An enumerator that can be used to iterate through the array.</returns>
      public JsonElement.ArrayEnumerator GetEnumerator() => this with
      {
        _curIdx = -1
      };


      #nullable disable
      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An enumerator that can be used to iterate through the collection.</returns>
      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An enumerator for an array of <see cref="T:System.Text.Json.JsonElement" /> that can be used to iterate through the collection.</returns>
      IEnumerator<JsonElement> IEnumerable<JsonElement>.GetEnumerator() => (IEnumerator<JsonElement>) this.GetEnumerator();

      /// <summary>Releases the resources used by this <see cref="T:System.Text.Json.JsonElement.ArrayEnumerator" /> instance.</summary>
      public void Dispose() => this._curIdx = this._endIdxOrVersion;

      /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
      public void Reset() => this._curIdx = -1;


      #nullable enable
      /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
      /// <returns>The element in the collection at the current position of the enumerator.</returns>
      object IEnumerator.Current => (object) this.Current;

      /// <summary>Advances the enumerator to the next element of the collection.</summary>
      /// <returns>
      /// <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
      public bool MoveNext()
      {
        if (this._curIdx >= this._endIdxOrVersion)
          return false;
        this._curIdx = this._curIdx >= 0 ? this._target._parent.GetEndIndex(this._curIdx, true) : this._target._idx + 12;
        return this._curIdx < this._endIdxOrVersion;
      }
    }

    /// <summary>Represents an enumerator for the properties of a JSON object.</summary>
    [System.Diagnostics.DebuggerDisplay("{Current,nq}")]
    public struct ObjectEnumerator : 
      IEnumerable<JsonProperty>,
      IEnumerable,
      IEnumerator<JsonProperty>,
      IEnumerator,
      IDisposable
    {
      private readonly JsonElement _target;
      private int _curIdx;
      private readonly int _endIdxOrVersion;

      internal ObjectEnumerator(JsonElement target)
      {
        this._target = target;
        this._curIdx = -1;
        this._endIdxOrVersion = target._parent.GetEndIndex(this._target._idx, false);
      }

      /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
      /// <returns>The element in the collection at the current position of the enumerator.</returns>
      public JsonProperty Current => this._curIdx < 0 ? new JsonProperty() : new JsonProperty(new JsonElement(this._target._parent, this._curIdx));

      /// <summary>Returns an enumerator that iterates the properties of an object.</summary>
      /// <returns>An enumerator that can be used to iterate through the object.</returns>
      public JsonElement.ObjectEnumerator GetEnumerator() => this with
      {
        _curIdx = -1
      };


      #nullable disable
      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An enumerator that can be used to iterate through the collection.</returns>
      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

      /// <summary>Returns an enumerator that iterates through a collection.</summary>
      /// <returns>An enumerator for <see cref="T:System.Text.Json.JsonProperty" /> objects that can be used to iterate through the collection.</returns>
      IEnumerator<JsonProperty> IEnumerable<JsonProperty>.GetEnumerator() => (IEnumerator<JsonProperty>) this.GetEnumerator();

      /// <summary>Releases the resources used by this <see cref="T:System.Text.Json.JsonElement.ObjectEnumerator" /> instance.</summary>
      public void Dispose() => this._curIdx = this._endIdxOrVersion;

      /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
      public void Reset() => this._curIdx = -1;


      #nullable enable
      /// <summary>Gets the element in the collection at the current position of the enumerator.</summary>
      /// <returns>The element in the collection at the current position of the enumerator.</returns>
      object IEnumerator.Current => (object) this.Current;

      /// <summary>Advances the enumerator to the next element of the collection.</summary>
      /// <returns>
      /// <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
      public bool MoveNext()
      {
        if (this._curIdx >= this._endIdxOrVersion)
          return false;
        this._curIdx = this._curIdx >= 0 ? this._target._parent.GetEndIndex(this._curIdx, true) : this._target._idx + 12;
        this._curIdx += 12;
        return this._curIdx < this._endIdxOrVersion;
      }
    }
  }
}
