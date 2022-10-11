// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Nodes.JsonValue
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;


#nullable enable
namespace System.Text.Json.Nodes
{
  /// <summary>Represents a mutable JSON value.</summary>
  public abstract class JsonValue : JsonNode
  {

    #nullable disable
    internal const string CreateUnreferencedCodeMessage = "Creating JsonValue instances with non-primitive types is not compatible with trimming. It can result in non-primitive types being serialized, which may have their members trimmed.";


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(bool value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<bool>(value, JsonMetadataServices.BooleanConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(bool? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<bool>(value.Value, JsonMetadataServices.BooleanConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(byte value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<byte>(value, JsonMetadataServices.ByteConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(byte? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<byte>(value.Value, JsonMetadataServices.ByteConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(char value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<char>(value, JsonMetadataServices.CharConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(char? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<char>(value.Value, JsonMetadataServices.CharConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(DateTime value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<DateTime>(value, JsonMetadataServices.DateTimeConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(DateTime? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<DateTime>(value.Value, JsonMetadataServices.DateTimeConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(DateTimeOffset value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<DateTimeOffset>(value, JsonMetadataServices.DateTimeOffsetConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(DateTimeOffset? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<DateTimeOffset>(value.Value, JsonMetadataServices.DateTimeOffsetConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(Decimal value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<Decimal>(value, JsonMetadataServices.DecimalConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(Decimal? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<Decimal>(value.Value, JsonMetadataServices.DecimalConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(double value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<double>(value, JsonMetadataServices.DoubleConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(double? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<double>(value.Value, JsonMetadataServices.DoubleConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(Guid value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<Guid>(value, JsonMetadataServices.GuidConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(Guid? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<Guid>(value.Value, JsonMetadataServices.GuidConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(short value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<short>(value, JsonMetadataServices.Int16Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(short? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<short>(value.Value, JsonMetadataServices.Int16Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(int value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<int>(value, JsonMetadataServices.Int32Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(int? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<int>(value.Value, JsonMetadataServices.Int32Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(long value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<long>(value, JsonMetadataServices.Int64Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(long? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<long>(value.Value, JsonMetadataServices.Int64Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [CLSCompliant(false)]
    public static JsonValue Create(sbyte value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<sbyte>(value, JsonMetadataServices.SByteConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [CLSCompliant(false)]
    public static JsonValue? Create(sbyte? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<sbyte>(value.Value, JsonMetadataServices.SByteConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue Create(float value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<float>(value, JsonMetadataServices.SingleConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(float? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<float>(value.Value, JsonMetadataServices.SingleConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(string? value, JsonNodeOptions? options = null) => value == null ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<string>(value, JsonMetadataServices.StringConverter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [CLSCompliant(false)]
    public static JsonValue Create(ushort value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<ushort>(value, JsonMetadataServices.UInt16Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [CLSCompliant(false)]
    public static JsonValue? Create(ushort? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<ushort>(value.Value, JsonMetadataServices.UInt16Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [CLSCompliant(false)]
    public static JsonValue Create(uint value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<uint>(value, JsonMetadataServices.UInt32Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [CLSCompliant(false)]
    public static JsonValue? Create(uint? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<uint>(value.Value, JsonMetadataServices.UInt32Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [CLSCompliant(false)]
    public static JsonValue Create(ulong value, JsonNodeOptions? options = null) => (JsonValue) new JsonValueTrimmable<ulong>(value, JsonMetadataServices.UInt64Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [CLSCompliant(false)]
    public static JsonValue? Create(ulong? value, JsonNodeOptions? options = null) => !value.HasValue ? (JsonValue) null : (JsonValue) new JsonValueTrimmable<ulong>(value.Value, JsonMetadataServices.UInt64Converter);

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(JsonElement value, JsonNodeOptions? options = null)
    {
      if (value.ValueKind == JsonValueKind.Null)
        return (JsonValue) null;
      JsonValue.VerifyJsonElementIsNotArrayOrObject(ref value);
      return (JsonValue) new JsonValueTrimmable<JsonElement>(value, JsonMetadataServices.JsonElementConverter);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The underlying value of the new <see cref="T:System.Text.Json.Nodes.JsonValue" /> instance.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create(JsonElement? value, JsonNodeOptions? options = null)
    {
      if (!value.HasValue)
        return (JsonValue) null;
      JsonElement element = value.Value;
      if (element.ValueKind == JsonValueKind.Null)
        return (JsonValue) null;
      JsonValue.VerifyJsonElementIsNotArrayOrObject(ref element);
      return (JsonValue) new JsonValueTrimmable<JsonElement>(element, JsonMetadataServices.JsonElementConverter);
    }


    #nullable disable
    private protected JsonValue(JsonNodeOptions? options = null)
      : base(options)
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The value to create.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <typeparam name="T">The type of value to create.</typeparam>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    [RequiresUnreferencedCode("Creating JsonValue instances with non-primitive types is not compatible with trimming. It can result in non-primitive types being serialized, which may have their members trimmed. Use the overload that takes a JsonTypeInfo, or make sure all of the required types are preserved.")]
    public static JsonValue? Create<T>(T? value, JsonNodeOptions? options = null)
    {
      if ((object) value == null)
        return (JsonValue) null;
      if (!(value is JsonElement element))
        return (JsonValue) new JsonValueNotTrimmable<T>(value, options);
      if (element.ValueKind == JsonValueKind.Null)
        return (JsonValue) null;
      JsonValue.VerifyJsonElementIsNotArrayOrObject(ref element);
      return (JsonValue) new JsonValueTrimmable<JsonElement>(element, JsonMetadataServices.JsonElementConverter, options);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</summary>
    /// <param name="value">The value to create.</param>
    /// <param name="jsonTypeInfo">The <see cref="T:System.Text.Json.Serialization.Metadata.JsonTypeInfo" /> that will be used to serialize the value.</param>
    /// <param name="options">Options to control the behavior.</param>
    /// <typeparam name="T">The type of value to create.</typeparam>
    /// <returns>The new instance of the <see cref="T:System.Text.Json.Nodes.JsonValue" /> class that contains the specified value.</returns>
    public static JsonValue? Create<T>(
      T? value,
      JsonTypeInfo<T> jsonTypeInfo,
      JsonNodeOptions? options = null)
    {
      if (jsonTypeInfo == null)
        throw new ArgumentNullException(nameof (jsonTypeInfo));
      if ((object) value == null)
        return (JsonValue) null;
      if (value is JsonElement element)
      {
        if (element.ValueKind == JsonValueKind.Null)
          return (JsonValue) null;
        JsonValue.VerifyJsonElementIsNotArrayOrObject(ref element);
      }
      return (JsonValue) new JsonValueTrimmable<T>(value, jsonTypeInfo, options);
    }


    #nullable disable
    internal override void GetPath(List<string> path, JsonNode child)
    {
      if (this.Parent == null)
        return;
      this.Parent.GetPath(path, (JsonNode) this);
    }


    #nullable enable
    /// <summary>Tries to obtain the current JSON value and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="value">When this method returns, contains the parsed value.</param>
    /// <typeparam name="T">The type of value to obtain.</typeparam>
    /// <returns>
    /// <see langword="true" /> if the value can be successfully obtained; otherwise, <see langword="false" />.</returns>
    public abstract bool TryGetValue<T>([NotNullWhen(true)] out T? value);


    #nullable disable
    private static void VerifyJsonElementIsNotArrayOrObject(ref JsonElement element)
    {
      if (element.ValueKind == JsonValueKind.Object || element.ValueKind == JsonValueKind.Array)
        throw new InvalidOperationException(SR.NodeElementCannotBeObjectOrArray);
    }
  }
}
