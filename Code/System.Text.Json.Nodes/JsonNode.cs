// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Nodes.JsonNode
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization.Converters;


#nullable enable
namespace System.Text.Json.Nodes
{
  /// <summary>The base class that represents a single node within a mutable JSON document.</summary>
  public abstract class JsonNode
  {

    #nullable disable
    private JsonNode _parent;
    private JsonNodeOptions? _options;


    #nullable enable
    /// <summary>Options to control the behavior.</summary>
    public JsonNodeOptions? Options
    {
      get
      {
        if (!this._options.HasValue && this.Parent != null)
          this._options = this.Parent.Options;
        return this._options;
      }
    }


    #nullable disable
    internal JsonNode(JsonNodeOptions? options = null) => this._options = options;


    #nullable enable
    /// <summary>Casts to the derived <see cref="T:System.Text.Json.Nodes.JsonArray" /> type.</summary>
    /// <exception cref="T:System.InvalidOperationException">The node is not a <see cref="T:System.Text.Json.Nodes.JsonArray" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonArray" />.</returns>
    public JsonArray AsArray() => this is JsonArray jsonArray ? jsonArray : throw new InvalidOperationException(SR.Format(SR.NodeWrongType, (object) "JsonArray"));

    /// <summary>Casts to the derived <see cref="T:System.Text.Json.Nodes.JsonObject" /> type.</summary>
    /// <exception cref="T:System.InvalidOperationException">The node is not a <see cref="T:System.Text.Json.Nodes.JsonObject" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonObject" />.</returns>
    public JsonObject AsObject() => this is JsonObject jsonObject ? jsonObject : throw new InvalidOperationException(SR.Format(SR.NodeWrongType, (object) "JsonObject"));

    /// <summary>Casts to the derived <see cref="T:System.Text.Json.Nodes.JsonValue" /> type.</summary>
    /// <exception cref="T:System.InvalidOperationException">The node is not a <see cref="T:System.Text.Json.Nodes.JsonValue" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonValue" />.</returns>
    public JsonValue AsValue() => this is JsonValue jsonValue ? jsonValue : throw new InvalidOperationException(SR.Format(SR.NodeWrongType, (object) "JsonValue"));

    /// <summary>Gets the parent <see cref="T:System.Text.Json.Nodes.JsonNode" />.
    /// If there is no parent, <see langword="null" /> is returned.
    /// A parent can either be a <see cref="T:System.Text.Json.Nodes.JsonObject" /> or a <see cref="T:System.Text.Json.Nodes.JsonArray" />.</summary>
    public JsonNode? Parent
    {
      get => this._parent;
      internal set => this._parent = value;
    }

    /// <summary>Gets the JSON path.</summary>
    /// <returns>The JSON Path value.</returns>
    public string GetPath()
    {
      if (this.Parent == null)
        return "$";
      List<string> path = new List<string>();
      this.GetPath(path, (JsonNode) null);
      StringBuilder stringBuilder = new StringBuilder("$");
      for (int index = path.Count - 1; index >= 0; --index)
        stringBuilder.Append(path[index]);
      return stringBuilder.ToString();
    }


    #nullable disable
    internal abstract void GetPath(List<string> path, JsonNode child);


    #nullable enable
    /// <summary>Gets the root <see cref="T:System.Text.Json.Nodes.JsonNode" />.
    /// If the current <see cref="T:System.Text.Json.Nodes.JsonNode" /> is a root, <see langword="null" /> is returned.</summary>
    public JsonNode Root
    {
      get
      {
        JsonNode parent = this.Parent;
        if (parent == null)
          return this;
        while (parent.Parent != null)
          parent = parent.Parent;
        return parent;
      }
    }

    /// <summary>Gets the value for the current <see cref="T:System.Text.Json.Nodes.JsonValue" />.</summary>
    /// <typeparam name="T">The type of the value to obtain from the <see cref="T:System.Text.Json.Nodes.JsonValue" />.</typeparam>
    /// <exception cref="T:System.FormatException">The current <see cref="T:System.Text.Json.Nodes.JsonNode" /> cannot be represented as a {TValue}.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Text.Json.Nodes.JsonNode" /> is not a <see cref="T:System.Text.Json.Nodes.JsonValue" /> or is not compatible with {TValue}.</exception>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</returns>
    public virtual T GetValue<T>() => throw new InvalidOperationException(SR.Format(SR.NodeWrongType, (object) "JsonValue"));

    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than 0 or <paramref name="index" /> is greater than the number of properties.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Text.Json.Nodes.JsonNode" /> is not a <see cref="T:System.Text.Json.Nodes.JsonArray" />.</exception>
    public JsonNode? this[int index]
    {
      get => this.AsArray().GetItem(index);
      set => this.AsArray().SetItem(index, value);
    }

    /// <summary>Gets or sets the element with the specified property name.
    /// If the property is not found, <see langword="null" /> is returned.</summary>
    /// <param name="propertyName">The name of the property to return.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Text.Json.Nodes.JsonNode" /> is not a <see cref="T:System.Text.Json.Nodes.JsonObject" />.</exception>
    public JsonNode? this[string propertyName]
    {
      get => this.AsObject().GetItem(propertyName);
      set => this.AsObject().SetItem(propertyName, value);
    }


    #nullable disable
    internal void AssignParent(JsonNode parent)
    {
      if (this.Parent != null)
        ThrowHelper.ThrowInvalidOperationException_NodeAlreadyHasParent();
      for (JsonNode jsonNode = parent; jsonNode != null; jsonNode = jsonNode.Parent)
      {
        if (jsonNode == this)
          ThrowHelper.ThrowInvalidOperationException_NodeCycleDetected();
      }
      this.Parent = parent;
    }


    #nullable enable
    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Boolean" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Boolean" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(bool value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Boolean" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Boolean" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(bool? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Byte" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Byte" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(byte value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Byte" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Byte" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(byte? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Char" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Char" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(char value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Char" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Char" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(char? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.DateTime" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.DateTime" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(DateTime value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.DateTime" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.DateTime" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(DateTime? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.DateTimeOffset" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.DateTimeOffset" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(DateTimeOffset value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.DateTimeOffset" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.DateTimeOffset" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(DateTimeOffset? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Decimal" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Decimal" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(Decimal value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Decimal" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Decimal" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(Decimal? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Double" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Double" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(double value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Double" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Double" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(double? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Guid" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Guid" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(Guid value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Guid" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Guid" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(Guid? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Int16" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int16" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(short value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Int16" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int16" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(short? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Int32" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int32" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(int value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Int32" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int32" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(int? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Int64" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int64" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(long value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Int64" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int64" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(long? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.SByte" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.SByte" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    [CLSCompliant(false)]
    public static implicit operator JsonNode(sbyte value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.SByte" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.SByte" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    [CLSCompliant(false)]
    public static implicit operator JsonNode?(sbyte? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Single" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Single" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode(float value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.Single" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Single" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(float? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.String" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.String" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    public static implicit operator JsonNode?(string? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.UInt16" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt16" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    [CLSCompliant(false)]
    public static implicit operator JsonNode(ushort value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.UInt16" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt16" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    [CLSCompliant(false)]
    public static implicit operator JsonNode?(ushort? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.UInt32" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt32" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    [CLSCompliant(false)]
    public static implicit operator JsonNode(uint value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.UInt32" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt32" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    [CLSCompliant(false)]
    public static implicit operator JsonNode?(uint? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.UInt64" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt64" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    [CLSCompliant(false)]
    public static implicit operator JsonNode(ulong value) => (JsonNode) JsonValue.Create(value, new JsonNodeOptions?());

    /// <summary>Defines an implicit conversion of a given <see cref="T:System.UInt64" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt64" /> to implicitly convert.</param>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance converted from the <paramref name="value" /> parameter.</returns>
    [CLSCompliant(false)]
    public static implicit operator JsonNode?(ulong? value) => (JsonNode) JsonValue.Create(value);

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Boolean" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Boolean" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator bool(JsonNode value) => value.GetValue<bool>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Boolean" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Boolean" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator bool?(JsonNode? value) => value?.GetValue<bool>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Byte" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Byte" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator byte(JsonNode value) => value.GetValue<byte>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Byte" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Byte" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator byte?(JsonNode? value) => value?.GetValue<byte>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Char" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Char" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator char(JsonNode value) => value.GetValue<char>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Char" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Char" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator char?(JsonNode? value) => value?.GetValue<char>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.DateTime" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.DateTime" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator DateTime(JsonNode value) => value.GetValue<DateTime>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.DateTime" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.DateTime" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator DateTime?(JsonNode? value) => value?.GetValue<DateTime>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.DateTimeOffset" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.DateTimeOffset" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator DateTimeOffset(JsonNode value) => value.GetValue<DateTimeOffset>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.DateTimeOffset" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.DateTimeOffset" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator DateTimeOffset?(JsonNode? value) => value?.GetValue<DateTimeOffset>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Decimal" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Decimal" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator Decimal(JsonNode value) => value.GetValue<Decimal>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Decimal" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Decimal" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator Decimal?(JsonNode? value) => value?.GetValue<Decimal>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Double" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Double" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator double(JsonNode value) => value.GetValue<double>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Double" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Double" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator double?(JsonNode? value) => value?.GetValue<double>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Guid" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Guid" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator Guid(JsonNode value) => value.GetValue<Guid>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Guid" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Guid" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator Guid?(JsonNode? value) => value?.GetValue<Guid>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Int16" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int16" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator short(JsonNode value) => value.GetValue<short>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Int16" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int16" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator short?(JsonNode? value) => value?.GetValue<short>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Int32" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int32" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator int(JsonNode value) => value.GetValue<int>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Int32" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int32" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator int?(JsonNode? value) => value?.GetValue<int>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Int64" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int64" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator long(JsonNode value) => value.GetValue<long>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Int64" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Int64" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator long?(JsonNode? value) => value?.GetValue<long>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.SByte" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.SByte" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    [CLSCompliant(false)]
    public static explicit operator sbyte(JsonNode value) => value.GetValue<sbyte>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.SByte" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.SByte" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    [CLSCompliant(false)]
    public static explicit operator sbyte?(JsonNode? value) => value?.GetValue<sbyte>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Single" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Single" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator float(JsonNode value) => value.GetValue<float>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.Single" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.Single" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator float?(JsonNode? value) => value?.GetValue<float>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.String" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.String" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    public static explicit operator string?(JsonNode? value) => value?.GetValue<string>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.UInt16" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt16" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    [CLSCompliant(false)]
    public static explicit operator ushort(JsonNode value) => value.GetValue<ushort>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.UInt16" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt16" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    [CLSCompliant(false)]
    public static explicit operator ushort?(JsonNode? value) => value?.GetValue<ushort>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.UInt32" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt32" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    [CLSCompliant(false)]
    public static explicit operator uint(JsonNode value) => value.GetValue<uint>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.UInt32" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt32" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    [CLSCompliant(false)]
    public static explicit operator uint?(JsonNode? value) => value?.GetValue<uint>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.UInt64" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt64" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    [CLSCompliant(false)]
    public static explicit operator ulong(JsonNode value) => value.GetValue<ulong>();

    /// <summary>Defines an explicit conversion of a given <see cref="T:System.UInt64" /> to a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">A <see cref="T:System.UInt64" /> to implicitly convert.</param>
    /// <returns>A value converted from the <see cref="T:System.Text.Json.Nodes.JsonNode" /> instance.</returns>
    [CLSCompliant(false)]
    public static explicit operator ulong?(JsonNode? value) => value?.GetValue<ulong>();

    /// <summary>Parses one JSON value (including objects or arrays) from the provided reader.</summary>
    /// <param name="reader">The reader to read.</param>
    /// <param name="nodeOptions">Options to control the behavior.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="reader" /> is using unsupported options.</exception>
    /// <exception cref="T:System.ArgumentException">The current <paramref name="reader" /> token does not start or represent a value.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">A value could not be read from the reader.</exception>
    /// <returns>The <see cref="T:System.Text.Json.Nodes.JsonNode" /> from the reader.</returns>
    public static JsonNode? Parse(ref Utf8JsonReader reader, JsonNodeOptions? nodeOptions = null) => JsonNodeConverter.Create(JsonElement.ParseValue(ref reader), nodeOptions);

    /// <summary>Parses text representing a single JSON value.</summary>
    /// <param name="json">JSON text to parse.</param>
    /// <param name="nodeOptions">Options to control the node behavior after parsing.</param>
    /// <param name="documentOptions">Options to control the document behavior during parsing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="json" /> does not represent a valid single JSON value.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value.</returns>
    public static JsonNode? Parse(
      string json,
      JsonNodeOptions? nodeOptions = null,
      JsonDocumentOptions documentOptions = default (JsonDocumentOptions))
    {
      if (json == null)
        throw new ArgumentNullException(nameof (json));
      return JsonNodeConverter.Create(JsonElement.ParseValue(json, documentOptions), nodeOptions);
    }

    /// <summary>Parses text representing a single JSON value.</summary>
    /// <param name="utf8Json">JSON text to parse.</param>
    /// <param name="nodeOptions">Options to control the node behavior after parsing.</param>
    /// <param name="documentOptions">Options to control the document behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value.</returns>
    public static JsonNode? Parse(
      ReadOnlySpan<byte> utf8Json,
      JsonNodeOptions? nodeOptions = null,
      JsonDocumentOptions documentOptions = default (JsonDocumentOptions))
    {
      return JsonNodeConverter.Create(JsonElement.ParseValue(utf8Json, documentOptions), nodeOptions);
    }

    /// <summary>Parse a <see cref="T:System.IO.Stream" /> as UTF-8-encoded data representing a single JSON value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.  The Stream will be read to completion.</summary>
    /// <param name="utf8Json">JSON text to parse.</param>
    /// <param name="nodeOptions">Options to control the node behavior after parsing.</param>
    /// <param name="documentOptions">Options to control the document behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="utf8Json" /> does not represent a valid single JSON value.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value.</returns>
    public static JsonNode? Parse(
      Stream utf8Json,
      JsonNodeOptions? nodeOptions = null,
      JsonDocumentOptions documentOptions = default (JsonDocumentOptions))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      return JsonNodeConverter.Create(JsonElement.ParseValue(utf8Json, documentOptions), nodeOptions);
    }

    /// <summary>Converts the current instance to string in JSON format.</summary>
    /// <param name="options">Options to control the serialization behavior.</param>
    /// <returns>JSON representation of current instance.</returns>
    public string ToJsonString(JsonSerializerOptions? options = null)
    {
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(16384))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, options == null ? new JsonWriterOptions() : options.GetWriterOptions()))
          this.WriteTo(writer, options);
        return JsonHelpers.Utf8GetString((ReadOnlySpan<byte>) byteBufferWriter.WrittenMemory.ToArray());
      }
    }

    /// <summary>Gets a string representation for the current value appropriate to the node type.</summary>
    /// <returns>A string representation for the current value appropriate to the node type.</returns>
    public override string ToString()
    {
      if (this is JsonValue)
      {
        if (this is JsonValue<string> jsonValue1)
          return jsonValue1.Value;
        if (this is JsonValue<JsonElement> jsonValue2 && jsonValue2.Value.ValueKind == JsonValueKind.String)
          return jsonValue2.Value.GetString();
      }
      using (PooledByteBufferWriter byteBufferWriter1 = new PooledByteBufferWriter(16384))
      {
        PooledByteBufferWriter byteBufferWriter2 = byteBufferWriter1;
        JsonWriterOptions options = new JsonWriterOptions()
        {
          Indented = true
        };
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter2, options))
          this.WriteTo(writer);
        return JsonHelpers.Utf8GetString((ReadOnlySpan<byte>) byteBufferWriter1.WrittenMemory.ToArray());
      }
    }

    /// <summary>Write the <see cref="T:System.Text.Json.Nodes.JsonNode" /> into the provided <see cref="T:System.Text.Json.Utf8JsonWriter" /> as JSON.</summary>
    /// <param name="writer">The <see cref="T:System.Text.Json.Utf8JsonWriter" />.</param>
    /// <param name="options">Options to control the serialization behavior.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="writer" /> parameter is <see langword="null" />.</exception>
    public abstract void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions? options = null);
  }
}
