// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonSerializer
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Provides functionality to serialize objects or value types to JSON and to deserialize JSON into objects or value types.</summary>
  public static class JsonSerializer
  {

    #nullable disable
    internal static readonly byte[] s_idPropertyName = new byte[3]
    {
      (byte) 36,
      (byte) 105,
      (byte) 100
    };
    internal static readonly byte[] s_refPropertyName = new byte[4]
    {
      (byte) 36,
      (byte) 114,
      (byte) 101,
      (byte) 102
    };
    internal static readonly byte[] s_valuesPropertyName = new byte[7]
    {
      (byte) 36,
      (byte) 118,
      (byte) 97,
      (byte) 108,
      (byte) 117,
      (byte) 101,
      (byte) 115
    };
    internal static readonly JsonEncodedText s_metadataId = JsonEncodedText.Encode("$id");
    internal static readonly JsonEncodedText s_metadataRef = JsonEncodedText.Encode("$ref");
    internal static readonly JsonEncodedText s_metadataValues = JsonEncodedText.Encode("$values");


    #nullable enable
    /// <summary>Converts the <see cref="T:System.Text.Json.JsonDocument" /> representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="document">The <see cref="T:System.Text.Json.JsonDocument" /> to convert.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="document" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <typeparamref name="TValue" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static TValue? Deserialize<TValue>(
      this JsonDocument document,
      JsonSerializerOptions? options = null)
    {
      if (document == null)
        throw new ArgumentNullException(nameof (document));
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, typeof (TValue));
      return JsonSerializer.ReadDocument<TValue>(document, typeInfo);
    }

    /// <summary>Converts the <see cref="T:System.Text.Json.JsonDocument" /> representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="document">The <see cref="T:System.Text.Json.JsonDocument" /> to convert.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="document" /> or <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="returnType" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static object? Deserialize(
      this JsonDocument document,
      Type returnType,
      JsonSerializerOptions? options = null)
    {
      if (document == null)
        throw new ArgumentNullException(nameof (document));
      JsonTypeInfo jsonTypeInfo = !(returnType == (Type) null) ? JsonSerializer.GetTypeInfo(options, returnType) : throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.ReadDocument<object>(document, jsonTypeInfo);
    }

    /// <summary>Converts the <see cref="T:System.Text.Json.JsonDocument" /> representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="document">The <see cref="T:System.Text.Json.JsonDocument" /> to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="document" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <typeparamref name="TValue" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static TValue? Deserialize<TValue>(
      this JsonDocument document,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      if (document == null)
        throw new ArgumentNullException(nameof (document));
      return jsonTypeInfo != null ? JsonSerializer.ReadDocument<TValue>(document, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Converts the <see cref="T:System.Text.Json.JsonDocument" /> representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="document">The <see cref="T:System.Text.Json.JsonDocument" /> to convert.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="document" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="returnType" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <paramref name="returnType" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static object? Deserialize(
      this JsonDocument document,
      Type returnType,
      JsonSerializerContext context)
    {
      if (document == null)
        throw new ArgumentNullException(nameof (document));
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      JsonTypeInfo jsonTypeInfo = context != null ? JsonSerializer.GetTypeInfo(context, returnType) : throw new ArgumentNullException(nameof (context));
      return JsonSerializer.ReadDocument<object>(document, jsonTypeInfo);
    }


    #nullable disable
    private static TValue ReadDocument<TValue>(JsonDocument document, JsonTypeInfo jsonTypeInfo) => JsonSerializer.ReadFromSpan<TValue>(document.GetRootRawValue().Span, jsonTypeInfo);


    #nullable enable
    /// <summary>Converts the <see cref="T:System.Text.Json.JsonElement" /> representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="element">The <see cref="T:System.Text.Json.JsonElement" /> to convert.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <typeparamref name="TValue" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static TValue? Deserialize<TValue>(
      this JsonElement element,
      JsonSerializerOptions? options = null)
    {
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, typeof (TValue));
      return JsonSerializer.ReadUsingMetadata<TValue>(element, typeInfo);
    }

    /// <summary>Converts the <see cref="T:System.Text.Json.JsonElement" /> representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="element">The <see cref="T:System.Text.Json.JsonElement" /> to convert.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="returnType" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static object? Deserialize(
      this JsonElement element,
      Type returnType,
      JsonSerializerOptions? options = null)
    {
      JsonTypeInfo jsonTypeInfo = !(returnType == (Type) null) ? JsonSerializer.GetTypeInfo(options, returnType) : throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.ReadUsingMetadata<object>(element, jsonTypeInfo);
    }

    /// <summary>Converts the <see cref="T:System.Text.Json.JsonElement" /> representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="element">The <see cref="T:System.Text.Json.JsonElement" /> to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <typeparamref name="TValue" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static TValue? Deserialize<TValue>(
      this JsonElement element,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      return jsonTypeInfo != null ? JsonSerializer.ReadUsingMetadata<TValue>(element, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Converts the <see cref="T:System.Text.Json.JsonElement" /> representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="element">The <see cref="T:System.Text.Json.JsonElement" /> to convert.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="returnType" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <paramref name="returnType" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static object? Deserialize(
      this JsonElement element,
      Type returnType,
      JsonSerializerContext context)
    {
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      JsonTypeInfo jsonTypeInfo = context != null ? JsonSerializer.GetTypeInfo(context, returnType) : throw new ArgumentNullException(nameof (context));
      return JsonSerializer.ReadUsingMetadata<object>(element, jsonTypeInfo);
    }


    #nullable disable
    private static TValue ReadUsingMetadata<TValue>(JsonElement element, JsonTypeInfo jsonTypeInfo) => JsonSerializer.ReadFromSpan<TValue>(element.GetRawValue().Span, jsonTypeInfo);


    #nullable enable
    /// <summary>Converts the <see cref="T:System.Text.Json.Nodes.JsonNode" /> representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="node">The <see cref="T:System.Text.Json.Nodes.JsonNode" /> to convert.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <typeparamref name="TValue" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static TValue? Deserialize<TValue>(this JsonNode? node, JsonSerializerOptions? options = null)
    {
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, typeof (TValue));
      return JsonSerializer.ReadNode<TValue>(node, typeInfo);
    }

    /// <summary>Converts the <see cref="T:System.Text.Json.Nodes.JsonNode" /> representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="node">The <see cref="T:System.Text.Json.Nodes.JsonNode" /> to convert.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <paramref name="returnType" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static object? Deserialize(
      this JsonNode? node,
      Type returnType,
      JsonSerializerOptions? options = null)
    {
      JsonTypeInfo jsonTypeInfo = !(returnType == (Type) null) ? JsonSerializer.GetTypeInfo(options, returnType) : throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.ReadNode<object>(node, jsonTypeInfo);
    }

    /// <summary>Converts the <see cref="T:System.Text.Json.Nodes.JsonNode" /> representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="node">The <see cref="T:System.Text.Json.Nodes.JsonNode" /> to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">
    /// <typeparamref name="TValue" /> is not compatible with the JSON.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static TValue? Deserialize<TValue>(this JsonNode? node, JsonTypeInfo<TValue> jsonTypeInfo) => jsonTypeInfo != null ? JsonSerializer.ReadNode<TValue>(node, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));

    /// <summary>Converts the <see cref="T:System.Text.Json.Nodes.JsonNode" /> representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="node">The <see cref="T:System.Text.Json.Nodes.JsonNode" /> to convert.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="returnType" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <paramref name="returnType" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static object? Deserialize(
      this JsonNode? node,
      Type returnType,
      JsonSerializerContext context)
    {
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      JsonTypeInfo jsonTypeInfo = context != null ? JsonSerializer.GetTypeInfo(context, returnType) : throw new ArgumentNullException(nameof (context));
      return JsonSerializer.ReadNode<object>(node, jsonTypeInfo);
    }


    #nullable disable
    private static TValue ReadNode<TValue>(JsonNode node, JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, options.GetWriterOptions()))
        {
          if (node == null)
            writer.WriteNullValue();
          else
            node.WriteTo(writer, options);
        }
        return JsonSerializer.ReadFromSpan<TValue>(byteBufferWriter.WrittenMemory.Span, jsonTypeInfo);
      }
    }


    #nullable enable
    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonDocument" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static JsonDocument SerializeToDocument<TValue>(
      TValue value,
      JsonSerializerOptions? options = null)
    {
      Type runtimeType = JsonSerializer.GetRuntimeType<TValue>(in value);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, runtimeType);
      return JsonSerializer.WriteDocumentUsingSerializer<TValue>(in value, typeInfo);
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" />  or its serializable members.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonDocument" /> representation of the value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static JsonDocument SerializeToDocument(
      object? value,
      Type inputType,
      JsonSerializerOptions? options = null)
    {
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, validateInputType);
      return JsonSerializer.WriteDocumentUsingSerializer<object>(in value, typeInfo);
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonDocument" /> representation of the value.</returns>
    public static JsonDocument SerializeToDocument<TValue>(
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      return jsonTypeInfo != null ? JsonSerializer.WriteDocumentUsingGeneratedSerializer<TValue>(in value, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonDocument" /> representation of the value.</returns>
    public static JsonDocument SerializeToDocument(
      object? value,
      Type inputType,
      JsonSerializerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      return JsonSerializer.WriteDocumentUsingGeneratedSerializer<object>(in value, JsonSerializer.GetTypeInfo(context, validateInputType));
    }


    #nullable disable
    private static JsonDocument WriteDocumentUsingGeneratedSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      PooledByteBufferWriter utf8Json = new PooledByteBufferWriter(options.DefaultBufferSize);
      using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) utf8Json, options.GetWriterOptions()))
        JsonSerializer.WriteUsingGeneratedSerializer<TValue>(writer, in value, jsonTypeInfo);
      return JsonDocument.ParseRented(utf8Json, options.GetDocumentOptions());
    }

    private static JsonDocument WriteDocumentUsingSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      PooledByteBufferWriter utf8Json = new PooledByteBufferWriter(options.DefaultBufferSize);
      using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) utf8Json, options.GetWriterOptions()))
        JsonSerializer.WriteUsingSerializer<TValue>(writer, in value, jsonTypeInfo);
      return JsonDocument.ParseRented(utf8Json, options.GetDocumentOptions());
    }


    #nullable enable
    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonDocument" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static JsonElement SerializeToElement<TValue>(
      TValue value,
      JsonSerializerOptions? options = null)
    {
      Type runtimeType = JsonSerializer.GetRuntimeType<TValue>(in value);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, runtimeType);
      return JsonSerializer.WriteElementUsingSerializer<TValue>(in value, typeInfo);
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" />  or its serializable members.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonDocument" /> representation of the value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static JsonElement SerializeToElement(
      object? value,
      Type inputType,
      JsonSerializerOptions? options = null)
    {
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, validateInputType);
      return JsonSerializer.WriteElementUsingSerializer<object>(in value, typeInfo);
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonDocument" /> representation of the value.</returns>
    public static JsonElement SerializeToElement<TValue>(
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      return jsonTypeInfo != null ? JsonSerializer.WriteElementUsingGeneratedSerializer<TValue>(in value, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.JsonDocument" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.JsonDocument" /> representation of the value.</returns>
    public static JsonElement SerializeToElement(
      object? value,
      Type inputType,
      JsonSerializerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(context, validateInputType);
      return JsonSerializer.WriteElementUsingGeneratedSerializer<object>(in value, typeInfo);
    }


    #nullable disable
    private static JsonElement WriteElementUsingGeneratedSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, options.GetWriterOptions()))
          JsonSerializer.WriteUsingGeneratedSerializer<TValue>(writer, in value, jsonTypeInfo);
        return JsonElement.ParseValue(byteBufferWriter.WrittenMemory.Span, options.GetDocumentOptions());
      }
    }

    private static JsonElement WriteElementUsingSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, options.GetWriterOptions()))
          JsonSerializer.WriteUsingSerializer<TValue>(writer, in value, jsonTypeInfo);
        return JsonElement.ParseValue(byteBufferWriter.WrittenMemory.Span, options.GetDocumentOptions());
      }
    }


    #nullable enable
    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static JsonNode? SerializeToNode<TValue>(
      TValue value,
      JsonSerializerOptions? options = null)
    {
      Type runtimeType = JsonSerializer.GetRuntimeType<TValue>(in value);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, runtimeType);
      return JsonSerializer.WriteNodeUsingSerializer<TValue>(in value, typeInfo);
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" />  or its serializable members.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static JsonNode? SerializeToNode(
      object? value,
      Type inputType,
      JsonSerializerOptions? options = null)
    {
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, validateInputType);
      return JsonSerializer.WriteNodeUsingSerializer<object>(in value, typeInfo);
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the value.</returns>
    public static JsonNode? SerializeToNode<TValue>(
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      return jsonTypeInfo != null ? JsonSerializer.WriteNodeUsingGeneratedSerializer<TValue>(in value, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Text.Json.Nodes.JsonNode" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Text.Json.Nodes.JsonNode" /> representation of the value.</returns>
    public static JsonNode? SerializeToNode(
      object? value,
      Type inputType,
      JsonSerializerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(context, validateInputType);
      return JsonSerializer.WriteNodeUsingGeneratedSerializer<object>(in value, typeInfo);
    }


    #nullable disable
    private static JsonNode WriteNodeUsingGeneratedSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, options.GetWriterOptions()))
          JsonSerializer.WriteUsingGeneratedSerializer<TValue>(writer, in value, jsonTypeInfo);
        return JsonNode.Parse(byteBufferWriter.WrittenMemory.Span, new JsonNodeOptions?(options.GetNodeOptions()));
      }
    }

    private static JsonNode WriteNodeUsingSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, options.GetWriterOptions()))
          JsonSerializer.WriteUsingSerializer<TValue>(writer, in value, jsonTypeInfo);
        return JsonNode.Parse(byteBufferWriter.WrittenMemory.Span, new JsonNodeOptions?(options.GetNodeOptions()));
      }
    }

    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    private static JsonTypeInfo GetTypeInfo(
      JsonSerializerOptions options,
      Type runtimeType)
    {
      if (options == null)
        options = JsonSerializerOptions.s_defaultOptions;
      if (!options.IsInitializedForReflectionSerializer)
        options.InitializeForReflectionSerializer();
      return options.GetOrAddClassForRootType(runtimeType);
    }

    private static JsonTypeInfo GetTypeInfo(JsonSerializerContext context, Type type)
    {
      JsonTypeInfo typeInfo = context.GetTypeInfo(type);
      if (typeInfo == null)
        ThrowHelper.ThrowInvalidOperationException_NoMetadataForType(type);
      return typeInfo;
    }

    internal static bool IsValidNumberHandlingValue(JsonNumberHandling handling) => JsonHelpers.IsInRangeInclusive((int) handling, 0, 7);

    internal static bool ResolveMetadataForJsonObject<T>(
      ref Utf8JsonReader reader,
      ref ReadStack state,
      JsonSerializerOptions options)
    {
      JsonConverter converterBase = state.Current.JsonTypeInfo.PropertyInfoForTypeInfo.ConverterBase;
      if (state.Current.ObjectState < StackFrameObjectState.ReadAheadNameOrEndObject && !JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadNameOrEndObject))
        return false;
      if (state.Current.ObjectState == StackFrameObjectState.ReadNameOrEndObject)
      {
        if (reader.TokenType != JsonTokenType.PropertyName)
        {
          state.Current.ObjectState = StackFrameObjectState.PropertyValue;
          state.Current.PropertyState = StackFramePropertyState.ReadName;
          return true;
        }
        ReadOnlySpan<byte> span = reader.GetSpan();
        switch (JsonSerializer.GetMetadataPropertyName(span))
        {
          case MetadataPropertyName.Values:
            ThrowHelper.ThrowJsonException_MetadataInvalidPropertyWithLeadingDollarSign(span, ref state, in reader);
            break;
          case MetadataPropertyName.Id:
            state.Current.JsonPropertyName = JsonSerializer.s_idPropertyName;
            if (!converterBase.CanHaveIdMetadata)
              ThrowHelper.ThrowJsonException_MetadataCannotParsePreservedObjectIntoImmutable(converterBase.TypeToConvert);
            state.Current.ObjectState = StackFrameObjectState.ReadAheadIdValue;
            break;
          case MetadataPropertyName.Ref:
            state.Current.JsonPropertyName = JsonSerializer.s_refPropertyName;
            if (converterBase.IsValueType)
              ThrowHelper.ThrowJsonException_MetadataInvalidReferenceToValueType(converterBase.TypeToConvert);
            state.Current.ObjectState = StackFrameObjectState.ReadAheadRefValue;
            break;
          default:
            state.Current.ObjectState = StackFrameObjectState.PropertyValue;
            state.Current.PropertyState = StackFramePropertyState.ReadName;
            return true;
        }
      }
      if (state.Current.ObjectState == StackFrameObjectState.ReadAheadRefValue)
      {
        if (!JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadRefValue))
          return false;
      }
      else if (state.Current.ObjectState == StackFrameObjectState.ReadAheadIdValue && !JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadIdValue))
        return false;
      if (state.Current.ObjectState == StackFrameObjectState.ReadRefValue)
      {
        if (reader.TokenType != JsonTokenType.String)
          ThrowHelper.ThrowJsonException_MetadataValueWasNotString(reader.TokenType);
        string referenceId = reader.GetString();
        object obj = state.ReferenceResolver.ResolveReference(referenceId);
        JsonSerializer.ValidateValueIsCorrectType<T>(obj, referenceId);
        state.Current.ReturnValue = obj;
        state.Current.ObjectState = StackFrameObjectState.ReadAheadRefEndObject;
      }
      else if (state.Current.ObjectState == StackFrameObjectState.ReadIdValue)
      {
        if (reader.TokenType != JsonTokenType.String)
          ThrowHelper.ThrowJsonException_MetadataValueWasNotString(reader.TokenType);
        converterBase.CreateInstanceForReferenceResolver(ref reader, ref state, options);
        string referenceId = reader.GetString();
        state.ReferenceResolver.AddReference(referenceId, state.Current.ReturnValue);
        state.Current.ObjectState = StackFrameObjectState.CreatedObject;
      }
      state.Current.JsonPropertyName = (byte[]) null;
      if (state.Current.ObjectState == StackFrameObjectState.ReadAheadRefEndObject && !JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadRefEndObject))
        return false;
      if (state.Current.ObjectState == StackFrameObjectState.ReadRefEndObject && reader.TokenType != JsonTokenType.EndObject)
        ThrowHelper.ThrowJsonException_MetadataReferenceObjectCannotContainOtherProperties(reader.GetSpan(), ref state);
      return true;
    }

    internal static bool ResolveMetadataForJsonArray<T>(
      ref Utf8JsonReader reader,
      ref ReadStack state,
      JsonSerializerOptions options)
    {
      JsonConverter converterBase = state.Current.JsonTypeInfo.PropertyInfoForTypeInfo.ConverterBase;
      if (state.Current.ObjectState < StackFrameObjectState.ReadAheadNameOrEndObject && !JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadNameOrEndObject))
        return false;
      if (state.Current.ObjectState == StackFrameObjectState.ReadNameOrEndObject)
      {
        if (reader.TokenType != JsonTokenType.PropertyName)
          ThrowHelper.ThrowJsonException_MetadataPreservedArrayValuesNotFound(ref state, converterBase.TypeToConvert);
        ReadOnlySpan<byte> span = reader.GetSpan();
        switch (JsonSerializer.GetMetadataPropertyName(span))
        {
          case MetadataPropertyName.Values:
            ThrowHelper.ThrowJsonException_MetadataMissingIdBeforeValues(ref state, span);
            break;
          case MetadataPropertyName.Id:
            state.Current.JsonPropertyName = JsonSerializer.s_idPropertyName;
            if (!converterBase.CanHaveIdMetadata)
              ThrowHelper.ThrowJsonException_MetadataCannotParsePreservedObjectIntoImmutable(converterBase.TypeToConvert);
            state.Current.ObjectState = StackFrameObjectState.ReadAheadIdValue;
            break;
          case MetadataPropertyName.Ref:
            state.Current.JsonPropertyName = JsonSerializer.s_refPropertyName;
            if (converterBase.IsValueType)
              ThrowHelper.ThrowJsonException_MetadataInvalidReferenceToValueType(converterBase.TypeToConvert);
            state.Current.ObjectState = StackFrameObjectState.ReadAheadRefValue;
            break;
          default:
            ThrowHelper.ThrowJsonException_MetadataPreservedArrayInvalidProperty(ref state, converterBase.TypeToConvert, in reader);
            break;
        }
      }
      if (state.Current.ObjectState == StackFrameObjectState.ReadAheadRefValue)
      {
        if (!JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadRefValue))
          return false;
      }
      else if (state.Current.ObjectState == StackFrameObjectState.ReadAheadIdValue && !JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadIdValue))
        return false;
      if (state.Current.ObjectState == StackFrameObjectState.ReadRefValue)
      {
        if (reader.TokenType != JsonTokenType.String)
          ThrowHelper.ThrowJsonException_MetadataValueWasNotString(reader.TokenType);
        string referenceId = reader.GetString();
        object obj = state.ReferenceResolver.ResolveReference(referenceId);
        JsonSerializer.ValidateValueIsCorrectType<T>(obj, referenceId);
        state.Current.ReturnValue = obj;
        state.Current.ObjectState = StackFrameObjectState.ReadAheadRefEndObject;
      }
      else if (state.Current.ObjectState == StackFrameObjectState.ReadIdValue)
      {
        if (reader.TokenType != JsonTokenType.String)
          ThrowHelper.ThrowJsonException_MetadataValueWasNotString(reader.TokenType);
        converterBase.CreateInstanceForReferenceResolver(ref reader, ref state, options);
        string referenceId = reader.GetString();
        state.ReferenceResolver.AddReference(referenceId, state.Current.ReturnValue);
        state.Current.ObjectState = StackFrameObjectState.ReadAheadValuesName;
      }
      state.Current.JsonPropertyName = (byte[]) null;
      if (state.Current.ObjectState == StackFrameObjectState.ReadAheadRefEndObject && !JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadRefEndObject))
        return false;
      if (state.Current.ObjectState == StackFrameObjectState.ReadRefEndObject)
      {
        if (reader.TokenType != JsonTokenType.EndObject)
          ThrowHelper.ThrowJsonException_MetadataReferenceObjectCannotContainOtherProperties(reader.GetSpan(), ref state);
        return true;
      }
      if (state.Current.ObjectState == StackFrameObjectState.ReadAheadValuesName && !JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadValuesName))
        return false;
      if (state.Current.ObjectState == StackFrameObjectState.ReadValuesName)
      {
        if (reader.TokenType != JsonTokenType.PropertyName)
          ThrowHelper.ThrowJsonException_MetadataPreservedArrayValuesNotFound(ref state, converterBase.TypeToConvert);
        if (JsonSerializer.GetMetadataPropertyName(reader.GetSpan()) != MetadataPropertyName.Values)
          ThrowHelper.ThrowJsonException_MetadataPreservedArrayInvalidProperty(ref state, converterBase.TypeToConvert, in reader);
        state.Current.JsonPropertyName = JsonSerializer.s_valuesPropertyName;
        state.Current.ObjectState = StackFrameObjectState.ReadAheadValuesStartArray;
      }
      if (state.Current.ObjectState == StackFrameObjectState.ReadAheadValuesStartArray && !JsonSerializer.TryReadAheadMetadataAndSetState(ref reader, ref state, StackFrameObjectState.ReadValuesStartArray))
        return false;
      if (state.Current.ObjectState == StackFrameObjectState.ReadValuesStartArray)
      {
        if (reader.TokenType != JsonTokenType.StartArray)
          ThrowHelper.ThrowJsonException_MetadataValuesInvalidToken(reader.TokenType);
        state.Current.ValidateEndTokenOnArray = true;
        state.Current.ObjectState = StackFrameObjectState.CreatedObject;
      }
      return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryReadAheadMetadataAndSetState(
      ref Utf8JsonReader reader,
      ref ReadStack state,
      StackFrameObjectState nextState)
    {
      state.Current.ObjectState = nextState;
      return reader.Read();
    }

    internal static MetadataPropertyName GetMetadataPropertyName(
      ReadOnlySpan<byte> propertyName)
    {
      if (propertyName.Length > 0 && propertyName[0] == (byte) 36)
      {
        switch (propertyName.Length)
        {
          case 3:
            if (propertyName[1] == (byte) 105 && propertyName[2] == (byte) 100)
              return MetadataPropertyName.Id;
            break;
          case 4:
            if (propertyName[1] == (byte) 114 && propertyName[2] == (byte) 101 && propertyName[3] == (byte) 102)
              return MetadataPropertyName.Ref;
            break;
          case 7:
            if (propertyName[1] == (byte) 118 && propertyName[2] == (byte) 97 && propertyName[3] == (byte) 108 && propertyName[4] == (byte) 117 && propertyName[5] == (byte) 101 && propertyName[6] == (byte) 115)
              return MetadataPropertyName.Values;
            break;
        }
      }
      return MetadataPropertyName.NoMetadata;
    }

    internal static bool TryGetReferenceFromJsonElement(
      ref ReadStack state,
      JsonElement element,
      out object referenceValue)
    {
      bool referenceFromJsonElement = false;
      referenceValue = (object) null;
      if (element.ValueKind == JsonValueKind.Object)
      {
        int num = 0;
        foreach (JsonProperty jsonProperty in element.EnumerateObject())
        {
          ++num;
          if (referenceFromJsonElement)
            ThrowHelper.ThrowJsonException_MetadataReferenceObjectCannotContainOtherProperties();
          else if (jsonProperty.EscapedNameEquals((ReadOnlySpan<byte>) JsonSerializer.s_refPropertyName))
          {
            if (num > 1)
              ThrowHelper.ThrowJsonException_MetadataReferenceObjectCannotContainOtherProperties();
            if (jsonProperty.Value.ValueKind != JsonValueKind.String)
              ThrowHelper.ThrowJsonException_MetadataValueWasNotString(jsonProperty.Value.ValueKind);
            referenceValue = state.ReferenceResolver.ResolveReference(jsonProperty.Value.GetString());
            referenceFromJsonElement = true;
          }
        }
      }
      return referenceFromJsonElement;
    }

    private static void ValidateValueIsCorrectType<T>(object value, string referenceId)
    {
      try
      {
        T obj = (T) value;
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowInvalidOperationException_MetadataReferenceOfTypeCannotBeAssignedToType(referenceId, value.GetType(), typeof (T));
        throw;
      }
    }

    internal static JsonPropertyInfo LookupProperty(
      object obj,
      ReadOnlySpan<byte> unescapedPropertyName,
      ref ReadStack state,
      JsonSerializerOptions options,
      out bool useExtensionProperty,
      bool createExtensionProperty = true)
    {
      useExtensionProperty = false;
      byte[] utf8PropertyName;
      JsonPropertyInfo jsonPropertyInfo = state.Current.JsonTypeInfo.GetProperty(unescapedPropertyName, ref state.Current, out utf8PropertyName);
      ++state.Current.PropertyIndex;
      state.Current.JsonPropertyName = utf8PropertyName;
      if (jsonPropertyInfo == JsonPropertyInfo.s_missingProperty)
      {
        JsonPropertyInfo extensionProperty = state.Current.JsonTypeInfo.DataExtensionProperty;
        if (extensionProperty != null && extensionProperty.HasGetter && extensionProperty.HasSetter)
        {
          state.Current.JsonPropertyNameAsString = JsonHelpers.Utf8GetString(unescapedPropertyName);
          if (createExtensionProperty)
            JsonSerializer.CreateDataExtensionProperty(obj, extensionProperty, options);
          jsonPropertyInfo = extensionProperty;
          useExtensionProperty = true;
        }
      }
      state.Current.JsonPropertyInfo = jsonPropertyInfo;
      state.Current.NumberHandling = jsonPropertyInfo.NumberHandling;
      return jsonPropertyInfo;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ReadOnlySpan<byte> GetPropertyName(
      ref ReadStack state,
      ref Utf8JsonReader reader,
      JsonSerializerOptions options)
    {
      ReadOnlySpan<byte> span = reader.GetSpan();
      ReadOnlySpan<byte> propertyName;
      if (reader._stringHasEscaping)
      {
        int idx = span.IndexOf<byte>((byte) 92);
        propertyName = JsonReaderHelper.GetUnescapedSpan(span, idx);
      }
      else
        propertyName = span;
      if (options.ReferenceHandlingStrategy == ReferenceHandlingStrategy.Preserve && span.Length > 0 && span[0] == (byte) 36)
        ThrowHelper.ThrowUnexpectedMetadataException(span, ref reader, ref state);
      return propertyName;
    }

    internal static void CreateDataExtensionProperty(
      object obj,
      JsonPropertyInfo jsonPropertyInfo,
      JsonSerializerOptions options)
    {
      object extensionDict = jsonPropertyInfo.GetValueAsObject(obj);
      if (extensionDict != null)
        return;
      if (jsonPropertyInfo.RuntimeTypeInfo.CreateObject == null)
      {
        if (jsonPropertyInfo.DeclaredPropertyType.FullName == "System.Text.Json.Nodes.JsonObject")
          extensionDict = jsonPropertyInfo.ConverterBase.CreateObject(options);
        else
          ThrowHelper.ThrowNotSupportedException_SerializationNotSupported(jsonPropertyInfo.DeclaredPropertyType);
      }
      else
        extensionDict = jsonPropertyInfo.RuntimeTypeInfo.CreateObject();
      jsonPropertyInfo.SetExtensionDictionaryAsObject(obj, extensionDict);
    }

    private static TValue ReadCore<TValue>(
      JsonConverter jsonConverter,
      ref Utf8JsonReader reader,
      JsonSerializerOptions options,
      ref ReadStack state)
    {
      return jsonConverter is JsonConverter<TValue> jsonConverter1 ? jsonConverter1.ReadCore(ref reader, options, ref state) : (TValue) jsonConverter.ReadCoreAsObject(ref reader, options, ref state);
    }

    private static TValue ReadFromSpan<TValue>(
      ReadOnlySpan<byte> utf8Json,
      JsonTypeInfo jsonTypeInfo,
      int? actualByteCount = null)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      JsonReaderState state1 = new JsonReaderState(options.GetReaderOptions());
      Utf8JsonReader reader = new Utf8JsonReader(utf8Json, true, state1);
      ReadStack state2 = new ReadStack();
      state2.Initialize(jsonTypeInfo);
      JsonConverter converterBase = jsonTypeInfo.PropertyInfoForTypeInfo.ConverterBase;
      return !(converterBase is JsonConverter<TValue> jsonConverter) ? (TValue) converterBase.ReadCoreAsObject(ref reader, options, ref state2) : jsonConverter.ReadCore(ref reader, options, ref state2);
    }


    #nullable enable
    /// <summary>Parses the UTF-8 encoded text representing a single JSON value into an instance of the type specified by a generic type parameter.</summary>
    /// <param name="utf8Json">The JSON text to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <typeparam name="TValue">The target type of the UTF-8 encoded text.</typeparam>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the span beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static TValue? Deserialize<TValue>(
      ReadOnlySpan<byte> utf8Json,
      JsonSerializerOptions? options = null)
    {
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, typeof (TValue));
      return JsonSerializer.ReadFromSpan<TValue>(utf8Json, typeInfo);
    }

    /// <summary>Parses the UTF-8 encoded text representing a single JSON value into an instance of a specified type.</summary>
    /// <param name="utf8Json">The JSON text to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="returnType" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the span beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static object? Deserialize(
      ReadOnlySpan<byte> utf8Json,
      Type returnType,
      JsonSerializerOptions? options = null)
    {
      JsonTypeInfo jsonTypeInfo = !(returnType == (Type) null) ? JsonSerializer.GetTypeInfo(options, returnType) : throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.ReadFromSpan<object>(utf8Json, jsonTypeInfo);
    }

    /// <summary>Parses the UTF-8 encoded text representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="utf8Json">JSON text to parse.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, <typeparamref name="TValue" /> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static TValue? Deserialize<TValue>(
      ReadOnlySpan<byte> utf8Json,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      return jsonTypeInfo != null ? JsonSerializer.ReadFromSpan<TValue>(utf8Json, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Parses the UTF-8 encoded text representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="utf8Json">JSON text to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, <paramref name="returnType" /> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method on the provided <paramref name="context" /> did not return a compatible <see cref="T:System.Text.Json.Serialization.Metadata.JsonTypeInfo" /> for <paramref name="returnType" />.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static object? Deserialize(
      ReadOnlySpan<byte> utf8Json,
      Type returnType,
      JsonSerializerContext context)
    {
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return JsonSerializer.ReadFromSpan<object>(utf8Json, JsonSerializer.GetTypeInfo(context, returnType));
    }

    /// <summary>Asynchronously reads the UTF-8 encoded text representing a single JSON value into an instance of a type specified by a generic type parameter. The stream will be read to completion.</summary>
    /// <param name="utf8Json">The JSON data to parse.</param>
    /// <param name="options">Options to control the behavior during reading.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
    /// <typeparam name="TValue">The target type of the JSON value.</typeparam>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" />is <see langword="null" />.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static ValueTask<TValue?> DeserializeAsync<TValue>(
      Stream utf8Json,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, typeof (TValue));
      return JsonSerializer.ReadAllAsync<TValue>(utf8Json, typeInfo, cancellationToken);
    }

    /// <summary>Reads the UTF-8 encoded text representing a single JSON value into a <typeparamref name="TValue" />.
    /// The Stream will be read to completion.</summary>
    /// <param name="utf8Json">JSON data to parse.</param>
    /// <param name="options">Options to control the behavior during reading.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, <typeparamref name="TValue" /> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static TValue? Deserialize<TValue>(Stream utf8Json, JsonSerializerOptions? options = null) => utf8Json != null ? JsonSerializer.ReadAllUsingOptions<TValue>(utf8Json, typeof (TValue), options) : throw new ArgumentNullException(nameof (utf8Json));

    /// <summary>Asynchronously reads the UTF-8 encoded text representing a single JSON value into an instance of a specified type. The stream will be read to completion.</summary>
    /// <param name="utf8Json">The JSON data to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the behavior during reading.</param>
    /// <param name="cancellationToken">A cancellation token that may be used to cancel the read operation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> or <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static ValueTask<object?> DeserializeAsync(
      Stream utf8Json,
      Type returnType,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      JsonTypeInfo jsonTypeInfo = !(returnType == (Type) null) ? JsonSerializer.GetTypeInfo(options, returnType) : throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.ReadAllAsync<object>(utf8Json, jsonTypeInfo, cancellationToken);
    }

    /// <summary>Reads the UTF-8 encoded text representing a single JSON value into a <paramref name="returnType" />.
    /// The Stream will be read to completion.</summary>
    /// <param name="utf8Json">JSON data to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the behavior during reading.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> or <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, the <paramref name="returnType" /> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static object? Deserialize(
      Stream utf8Json,
      Type returnType,
      JsonSerializerOptions? options = null)
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.ReadAllUsingOptions<object>(utf8Json, returnType, options);
    }

    /// <summary>Reads the UTF-8 encoded text representing a single JSON value into a <typeparamref name="TValue" />.
    /// The Stream will be read to completion.</summary>
    /// <param name="utf8Json">JSON data to parse.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> which may be used to cancel the read operation.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> or <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, <typeparamref name="TValue" /> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static ValueTask<TValue?> DeserializeAsync<TValue>(
      Stream utf8Json,
      JsonTypeInfo<TValue> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (jsonTypeInfo == null)
        throw new ArgumentNullException(nameof (jsonTypeInfo));
      return JsonSerializer.ReadAllAsync<TValue>(utf8Json, (JsonTypeInfo) jsonTypeInfo, cancellationToken);
    }

    /// <summary>Reads the UTF-8 encoded text representing a single JSON value into a <typeparamref name="TValue" />.
    /// The Stream will be read to completion.</summary>
    /// <param name="utf8Json">JSON data to parse.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> or <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, <typeparamref name="TValue" /> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static TValue? Deserialize<TValue>(Stream utf8Json, JsonTypeInfo<TValue> jsonTypeInfo)
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      return jsonTypeInfo != null ? JsonSerializer.ReadAll<TValue>(utf8Json, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Reads the UTF-8 encoded text representing a single JSON value into a <paramref name="returnType" />.
    /// The Stream will be read to completion.</summary>
    /// <param name="utf8Json">JSON data to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that can be used to cancel the read operation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" />, <paramref name="returnType" />, or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, the <paramref name="returnType" /> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method on the provided <paramref name="context" /> did not return a compatible <see cref="T:System.Text.Json.Serialization.Metadata.JsonTypeInfo" /> for <paramref name="returnType" />.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static ValueTask<object?> DeserializeAsync(
      Stream utf8Json,
      Type returnType,
      JsonSerializerContext context,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return JsonSerializer.ReadAllAsync<object>(utf8Json, JsonSerializer.GetTypeInfo(context, returnType), cancellationToken);
    }

    /// <summary>Reads the UTF-8 encoded text representing a single JSON value into a <paramref name="returnType" />.
    /// The Stream will be read to completion.</summary>
    /// <param name="utf8Json">JSON data to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" />, <paramref name="returnType" />, or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, the <paramref name="returnType" /> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method on the provided <paramref name="context" /> did not return a compatible <see cref="T:System.Text.Json.Serialization.Metadata.JsonTypeInfo" /> for <paramref name="returnType" />.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static object? Deserialize(
      Stream utf8Json,
      Type returnType,
      JsonSerializerContext context)
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return JsonSerializer.ReadAll<object>(utf8Json, JsonSerializer.GetTypeInfo(context, returnType));
    }

    /// <summary>Wraps the UTF-8 encoded text into an <see cref="T:System.Collections.Generic.IAsyncEnumerable`1" /> that can be used to deserialize root-level JSON arrays in a streaming manner.</summary>
    /// <param name="utf8Json">JSON data to parse.</param>
    /// <param name="options">Options to control the behavior during reading.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> which may be used to cancel the read operation.</param>
    /// <typeparam name="TValue">The element type to deserialize asynchronously.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> is <see langword="null" />.</exception>
    /// <returns>An <see cref="T:System.Collections.Generic.IAsyncEnumerable`1" /> representation of the provided JSON array.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static IAsyncEnumerable<TValue?> DeserializeAsyncEnumerable<TValue>(
      Stream utf8Json,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (options == null)
        options = JsonSerializerOptions.s_defaultOptions;
      if (!options.IsInitializedForReflectionSerializer)
        options.InitializeForReflectionSerializer();
      return CreateAsyncEnumerableDeserializer(utf8Json, options, cancellationToken);


      #nullable disable
      [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
      static async IAsyncEnumerable<TValue> CreateAsyncEnumerableDeserializer(
        Stream utf8Json,
        JsonSerializerOptions options,
        [EnumeratorCancellation] CancellationToken cancellationToken)
      {
        ReadBufferState bufferState = new ReadBufferState(options.DefaultBufferSize);
        JsonConverter converter = (JsonConverter) QueueOfTConverter<Queue<TValue>, TValue>.Instance;
        JsonTypeInfo queueJsonTypeInfo = JsonSerializer.CreateQueueJsonTypeInfo<TValue>(converter, options);
        ReadStack readStack = new ReadStack();
        readStack.Initialize(queueJsonTypeInfo, true);
        JsonReaderState jsonReaderState = new JsonReaderState(options.GetReaderOptions());
        try
        {
          do
          {
            bufferState = await JsonSerializer.ReadFromStreamAsync(utf8Json, bufferState, cancellationToken).ConfigureAwait(false);
            JsonSerializer.ContinueDeserialize<Queue<TValue>>(ref bufferState, ref jsonReaderState, ref readStack, converter, options);
            if (readStack.Current.ReturnValue is Queue<TValue> queue)
            {
              while (queue.Count > 0)
                yield return queue.Dequeue();
            }
            queue = (Queue<TValue>) null;
          }
          while (!bufferState.IsFinalBlock);
        }
        finally
        {
          bufferState.Dispose();
        }
      }
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Workaround for https://github.com/mono/linker/issues/1416. All usages are marked as unsafe.")]
    private static JsonTypeInfo CreateQueueJsonTypeInfo<TValue>(
      JsonConverter queueConverter,
      JsonSerializerOptions queueOptions)
    {
      return new JsonTypeInfo(typeof (Queue<TValue>), queueConverter, typeof (Queue<TValue>), queueOptions);
    }

    internal static async ValueTask<TValue> ReadAllAsync<TValue>(
      Stream utf8Json,
      JsonTypeInfo jsonTypeInfo,
      CancellationToken cancellationToken)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      ReadBufferState bufferState = new ReadBufferState(options.DefaultBufferSize);
      ReadStack readStack = new ReadStack();
      readStack.Initialize(jsonTypeInfo, true);
      JsonConverter converter = readStack.Current.JsonPropertyInfo.ConverterBase;
      JsonReaderState jsonReaderState = new JsonReaderState(options.GetReaderOptions());
      TValue obj1;
      try
      {
        TValue obj2;
        do
        {
          bufferState = await JsonSerializer.ReadFromStreamAsync(utf8Json, bufferState, cancellationToken).ConfigureAwait(false);
          obj2 = JsonSerializer.ContinueDeserialize<TValue>(ref bufferState, ref jsonReaderState, ref readStack, converter, options);
        }
        while (!bufferState.IsFinalBlock);
        obj1 = obj2;
      }
      finally
      {
        bufferState.Dispose();
      }
      options = (JsonSerializerOptions) null;
      bufferState = new ReadBufferState();
      readStack = new ReadStack();
      converter = (JsonConverter) null;
      jsonReaderState = new JsonReaderState();
      return obj1;
    }

    internal static TValue ReadAll<TValue>(Stream utf8Json, JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      ReadBufferState bufferState = new ReadBufferState(options.DefaultBufferSize);
      ReadStack readStack = new ReadStack();
      readStack.Initialize(jsonTypeInfo, true);
      JsonConverter converterBase = readStack.Current.JsonPropertyInfo.ConverterBase;
      JsonReaderState jsonReaderState = new JsonReaderState(options.GetReaderOptions());
      try
      {
        TValue obj;
        do
        {
          bufferState = JsonSerializer.ReadFromStream(utf8Json, bufferState);
          obj = JsonSerializer.ContinueDeserialize<TValue>(ref bufferState, ref jsonReaderState, ref readStack, converterBase, options);
        }
        while (!bufferState.IsFinalBlock);
        return obj;
      }
      finally
      {
        bufferState.Dispose();
      }
    }

    internal static async ValueTask<ReadBufferState> ReadFromStreamAsync(
      Stream utf8Json,
      ReadBufferState bufferState,
      CancellationToken cancellationToken)
    {
      do
      {
        int num = await utf8Json.ReadAsync(bufferState.Buffer.AsMemory<byte>(bufferState.BytesInBuffer), cancellationToken).ConfigureAwait(false);
        if (num == 0)
        {
          bufferState.IsFinalBlock = true;
          break;
        }
        bufferState.BytesInBuffer += num;
      }
      while (bufferState.BytesInBuffer != bufferState.Buffer.Length);
      return bufferState;
    }

    internal static ReadBufferState ReadFromStream(
      Stream utf8Json,
      ReadBufferState bufferState)
    {
      do
      {
        int num = utf8Json.Read(bufferState.Buffer.AsSpan<byte>(bufferState.BytesInBuffer));
        if (num == 0)
        {
          bufferState.IsFinalBlock = true;
          break;
        }
        bufferState.BytesInBuffer += num;
      }
      while (bufferState.BytesInBuffer != bufferState.Buffer.Length);
      return bufferState;
    }

    internal static TValue ContinueDeserialize<TValue>(
      ref ReadBufferState bufferState,
      ref JsonReaderState jsonReaderState,
      ref ReadStack readStack,
      JsonConverter converter,
      JsonSerializerOptions options)
    {
      if (bufferState.BytesInBuffer > bufferState.ClearMax)
        bufferState.ClearMax = bufferState.BytesInBuffer;
      int start = 0;
      if (bufferState.IsFirstIteration)
      {
        bufferState.IsFirstIteration = false;
        if (bufferState.Buffer.AsSpan<byte>().StartsWith<byte>(JsonConstants.Utf8Bom))
        {
          start += JsonConstants.Utf8Bom.Length;
          bufferState.BytesInBuffer -= JsonConstants.Utf8Bom.Length;
        }
      }
      TValue obj = JsonSerializer.ReadCore<TValue>(ref jsonReaderState, bufferState.IsFinalBlock, new ReadOnlySpan<byte>(bufferState.Buffer, start, bufferState.BytesInBuffer), options, ref readStack, converter);
      int bytesConsumed = checked ((int) readStack.BytesConsumed);
      bufferState.BytesInBuffer -= bytesConsumed;
      if (!bufferState.IsFinalBlock)
      {
        if ((uint) bufferState.BytesInBuffer > (uint) bufferState.Buffer.Length / 2U)
        {
          byte[] buffer = bufferState.Buffer;
          int clearMax = bufferState.ClearMax;
          byte[] dst = ArrayPool<byte>.Shared.Rent(bufferState.Buffer.Length < 1073741823 ? bufferState.Buffer.Length * 2 : int.MaxValue);
          Buffer.BlockCopy((Array) buffer, bytesConsumed + start, (Array) dst, 0, bufferState.BytesInBuffer);
          bufferState.Buffer = dst;
          bufferState.ClearMax = bufferState.BytesInBuffer;
          new Span<byte>(buffer, 0, clearMax).Clear();
          ArrayPool<byte>.Shared.Return(buffer);
        }
        else if (bufferState.BytesInBuffer != 0)
          Buffer.BlockCopy((Array) bufferState.Buffer, bytesConsumed + start, (Array) bufferState.Buffer, 0, bufferState.BytesInBuffer);
      }
      return obj;
    }

    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    private static TValue ReadAllUsingOptions<TValue>(
      Stream utf8Json,
      Type returnType,
      JsonSerializerOptions options)
    {
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, returnType);
      return JsonSerializer.ReadAll<TValue>(utf8Json, typeInfo);
    }

    private static TValue ReadCore<TValue>(
      ref JsonReaderState readerState,
      bool isFinalBlock,
      ReadOnlySpan<byte> buffer,
      JsonSerializerOptions options,
      ref ReadStack state,
      JsonConverter converterBase)
    {
      Utf8JsonReader reader = new Utf8JsonReader(buffer, isFinalBlock, readerState);
      state.ReadAhead = !isFinalBlock;
      state.BytesConsumed = 0L;
      TValue obj = JsonSerializer.ReadCore<TValue>(converterBase, ref reader, options, ref state);
      readerState = reader.CurrentState;
      return obj;
    }


    #nullable enable
    /// <summary>Parses the text representing a single JSON value into an instance of the type specified by a generic type parameter.</summary>
    /// <param name="json">The JSON text to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <typeparam name="TValue">The target type of the JSON value.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static TValue? Deserialize<TValue>(string json, JsonSerializerOptions? options = null)
    {
      if (json == null)
        throw new ArgumentNullException(nameof (json));
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, typeof (TValue));
      return JsonSerializer.ReadFromSpan<TValue>(json.AsSpan(), typeInfo);
    }

    /// <summary>Parses the text representing a single JSON value into an instance of the type specified by a generic type parameter.</summary>
    /// <param name="json">The JSON text to parse.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the span beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static TValue? Deserialize<TValue>(
      ReadOnlySpan<char> json,
      JsonSerializerOptions? options = null)
    {
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, typeof (TValue));
      return JsonSerializer.ReadFromSpan<TValue>(json, typeInfo);
    }

    /// <summary>Parses the text representing a single JSON value into an instance of a specified type.</summary>
    /// <param name="json">The JSON text to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="json" /> or <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static object? Deserialize(string json, Type returnType, JsonSerializerOptions? options = null)
    {
      if (json == null)
        throw new ArgumentNullException(nameof (json));
      JsonTypeInfo jsonTypeInfo = !(returnType == (Type) null) ? JsonSerializer.GetTypeInfo(options, returnType) : throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.ReadFromSpan<object>(json.AsSpan(), jsonTypeInfo);
    }

    /// <summary>Parses the text representing a single JSON value into an instance of a specified type.</summary>
    /// <param name="json">The JSON text to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the behavior during parsing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <paramref name="returnType" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the span beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static object? Deserialize(
      ReadOnlySpan<char> json,
      Type returnType,
      JsonSerializerOptions? options = null)
    {
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      JsonTypeInfo jsonTypeInfo = !(returnType == (Type) null) ? JsonSerializer.GetTypeInfo(options, returnType) : throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.ReadFromSpan<object>(json, jsonTypeInfo);
    }

    /// <summary>Parses the text representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="json">JSON text to parse.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="json" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static TValue? Deserialize<TValue>(string json, JsonTypeInfo<TValue> jsonTypeInfo)
    {
      if (json == null)
        throw new ArgumentNullException(nameof (json));
      return jsonTypeInfo != null ? JsonSerializer.ReadFromSpan<TValue>(json.AsSpan(), (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Parses the text representing a single JSON value into a <typeparamref name="TValue" />.</summary>
    /// <param name="json">JSON text to parse.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="json" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static TValue? Deserialize<TValue>(
      ReadOnlySpan<char> json,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      return jsonTypeInfo != null ? JsonSerializer.ReadFromSpan<TValue>(json, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Parses the text representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="json">JSON text to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="json" /> or <paramref name="returnType" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <paramref name="returnType" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static object? Deserialize(string json, Type returnType, JsonSerializerContext context)
    {
      if (json == null)
        throw new ArgumentNullException(nameof (json));
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      JsonTypeInfo jsonTypeInfo = context != null ? JsonSerializer.GetTypeInfo(context, returnType) : throw new ArgumentNullException(nameof (context));
      return JsonSerializer.ReadFromSpan<object>(json.AsSpan(), jsonTypeInfo);
    }

    /// <summary>Parses the text representing a single JSON value into a <paramref name="returnType" />.</summary>
    /// <param name="json">JSON text to parse.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="json" /> or <paramref name="returnType" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <paramref name="returnType" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// There is remaining data in the string beyond a single JSON value.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static object? Deserialize(
      ReadOnlySpan<char> json,
      Type returnType,
      JsonSerializerContext context)
    {
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      JsonTypeInfo jsonTypeInfo = context != null ? JsonSerializer.GetTypeInfo(context, returnType) : throw new ArgumentNullException(nameof (context));
      return JsonSerializer.ReadFromSpan<object>(json, jsonTypeInfo);
    }


    #nullable disable
    private static TValue ReadFromSpan<TValue>(ReadOnlySpan<char> json, JsonTypeInfo jsonTypeInfo)
    {
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
        return JsonSerializer.ReadFromSpan<TValue>((ReadOnlySpan<byte>) span, jsonTypeInfo, new int?(utf8FromText));
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


    #nullable enable
    /// <summary>Reads one JSON value (including objects or arrays) from the provided reader into an instance of the type specified by a generic type parameter.</summary>
    /// <param name="reader">The reader to read the JSON from.</param>
    /// <param name="options">Options to control serializer behavior during reading.</param>
    /// <typeparam name="TValue">The target type of the JSON value.</typeparam>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="TValue" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// A value could not be read from the reader.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="reader" /> uses unsupported options.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static TValue? Deserialize<TValue>(
      ref Utf8JsonReader reader,
      JsonSerializerOptions? options = null)
    {
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, typeof (TValue));
      return JsonSerializer.Read<TValue>(ref reader, typeInfo);
    }

    /// <summary>Reads one JSON value (including objects or arrays) from the provided reader and converts it into an instance of  a specified type.</summary>
    /// <param name="reader">The reader to read the JSON from.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="options">Options to control the serializer behavior during reading.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="returnType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid.
    /// 
    /// -or-
    /// 
    /// <typeparamref name="returnType" /> is not compatible with the JSON.
    /// 
    /// -or-
    /// 
    /// A value could not be read from the reader.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="reader" /> is using unsupported options.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static object? Deserialize(
      ref Utf8JsonReader reader,
      Type returnType,
      JsonSerializerOptions? options = null)
    {
      JsonTypeInfo jsonTypeInfo = !(returnType == (Type) null) ? JsonSerializer.GetTypeInfo(options, returnType) : throw new ArgumentNullException(nameof (returnType));
      return JsonSerializer.Read<object>(ref reader, jsonTypeInfo);
    }

    /// <summary>Reads one JSON value (including objects or arrays) from the provided reader into a <typeparamref name="TValue" />.</summary>
    /// <param name="reader">The reader to read.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type to deserialize the JSON value into.</typeparam>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, <typeparamref name="TValue" /> is not compatible with the JSON, or a value could not be read from the reader.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="reader" /> is using unsupported options.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A <typeparamref name="TValue" /> representation of the JSON value.</returns>
    public static TValue? Deserialize<TValue>(
      ref Utf8JsonReader reader,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      return jsonTypeInfo != null ? JsonSerializer.Read<TValue>(ref reader, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Reads one JSON value (including objects or arrays) from the provided reader into a <paramref name="returnType" />.</summary>
    /// <param name="reader">The reader to read.</param>
    /// <param name="returnType">The type of the object to convert to and return.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="returnType" /> or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Text.Json.JsonException">The JSON is invalid, <paramref name="returnType" /> is not compatible with the JSON, or a value could not be read from the reader.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="reader" /> is using unsupported options.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="returnType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method on the provided <paramref name="context" /> did not return a compatible <see cref="T:System.Text.Json.Serialization.Metadata.JsonTypeInfo" /> for <paramref name="returnType" />.</exception>
    /// <returns>A <paramref name="returnType" /> representation of the JSON value.</returns>
    public static object? Deserialize(
      ref Utf8JsonReader reader,
      Type returnType,
      JsonSerializerContext context)
    {
      if (returnType == (Type) null)
        throw new ArgumentNullException(nameof (returnType));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return JsonSerializer.Read<object>(ref reader, JsonSerializer.GetTypeInfo(context, returnType));
    }


    #nullable disable
    private static TValue Read<TValue>(ref Utf8JsonReader reader, JsonTypeInfo jsonTypeInfo)
    {
      ReadStack state = new ReadStack();
      state.Initialize(jsonTypeInfo);
      JsonReaderState currentState = reader.CurrentState;
      if (currentState.Options.CommentHandling == JsonCommentHandling.Allow)
        throw new ArgumentException(SR.JsonSerializerDoesNotSupportComments, nameof (reader));
      Utf8JsonReader utf8JsonReader = reader;
      ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>();
      ReadOnlySequence<byte> source = new ReadOnlySequence<byte>();
      try
      {
        switch (reader.TokenType)
        {
          case JsonTokenType.None:
          case JsonTokenType.PropertyName:
            if (!reader.Read())
            {
              ThrowHelper.ThrowJsonReaderException(ref reader, ExceptionResource.ExpectedOneCompleteToken);
              break;
            }
            break;
        }
        switch (reader.TokenType)
        {
          case JsonTokenType.StartObject:
          case JsonTokenType.StartArray:
            long tokenStartIndex = reader.TokenStartIndex;
            if (!reader.TrySkip())
              ThrowHelper.ThrowJsonReaderException(ref reader, ExceptionResource.NotEnoughData);
            long length1 = reader.BytesConsumed - tokenStartIndex;
            ReadOnlySequence<byte> originalSequence1 = reader.OriginalSequence;
            if (originalSequence1.IsEmpty)
            {
              readOnlySpan = reader.OriginalSpan.Slice(checked ((int) tokenStartIndex), checked ((int) length1));
              break;
            }
            source = originalSequence1.Slice(tokenStartIndex, length1);
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
            source = originalSequence2.Slice(reader.TokenStartIndex, length3);
            break;
          case JsonTokenType.Number:
          case JsonTokenType.True:
          case JsonTokenType.False:
          case JsonTokenType.Null:
            if (reader.HasValueSequence)
            {
              source = reader.ValueSequence;
              break;
            }
            readOnlySpan = reader.ValueSpan;
            break;
          default:
            byte nextByte = !reader.HasValueSequence ? reader.ValueSpan[0] : reader.ValueSequence.First.Span[0];
            ThrowHelper.ThrowJsonReaderException(ref reader, ExceptionResource.ExpectedStartOfValueNotFound, nextByte);
            break;
        }
      }
      catch (JsonReaderException ex)
      {
        reader = utf8JsonReader;
        ThrowHelper.ReThrowWithPath(ref state, ex);
      }
      int num1 = readOnlySpan.IsEmpty ? checked ((int) source.Length) : readOnlySpan.Length;
      byte[] array = ArrayPool<byte>.Shared.Rent(num1);
      Span<byte> span = array.AsSpan<byte>(0, num1);
      try
      {
        if (readOnlySpan.IsEmpty)
          source.CopyTo<byte>(span);
        else
          readOnlySpan.CopyTo(span);
        JsonReaderOptions options = currentState.Options;
        Utf8JsonReader reader1 = new Utf8JsonReader((ReadOnlySpan<byte>) span, options);
        return JsonSerializer.ReadCore<TValue>(state.Current.JsonPropertyInfo.ConverterBase, ref reader1, jsonTypeInfo.Options, ref state);
      }
      catch (JsonException ex)
      {
        reader = utf8JsonReader;
        throw;
      }
      finally
      {
        span.Clear();
        ArrayPool<byte>.Shared.Return(array);
      }
    }


    #nullable enable
    /// <summary>Converts the value of a type specified by a generic type parameter into a JSON string, encoded as UTF-8 bytes.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A JSON string representation of the value, encoded as UTF-8 bytes.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static byte[] SerializeToUtf8Bytes<TValue>(TValue value, JsonSerializerOptions? options = null)
    {
      Type runtimeType = JsonSerializer.GetRuntimeType<TValue>(in value);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, runtimeType);
      return JsonSerializer.WriteBytesUsingSerializer<TValue>(in value, typeInfo);
    }

    /// <summary>Converts a value of the specified type into a JSON string, encoded as UTF-8 bytes.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <returns>A JSON string representation of the value, encoded as UTF-8 bytes.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static byte[] SerializeToUtf8Bytes(
      object? value,
      Type inputType,
      JsonSerializerOptions? options = null)
    {
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, validateInputType);
      return JsonSerializer.WriteBytesUsingSerializer<object>(in value, typeInfo);
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Byte" /> array.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <returns>A UTF-8 representation of the value.</returns>
    public static byte[] SerializeToUtf8Bytes<TValue>(
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      return jsonTypeInfo != null ? JsonSerializer.WriteBytesUsingGeneratedSerializer<TValue>(in value, (JsonTypeInfo) jsonTypeInfo) : throw new ArgumentNullException(nameof (jsonTypeInfo));
    }

    /// <summary>Converts the provided value into a <see cref="T:System.Byte" /> array.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" />  or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <returns>A UTF-8 representation of the value.</returns>
    public static byte[] SerializeToUtf8Bytes(
      object? value,
      Type inputType,
      JsonSerializerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(context, validateInputType);
      return JsonSerializer.WriteBytesUsingGeneratedSerializer<object>(in value, typeInfo);
    }


    #nullable disable
    private static byte[] WriteBytesUsingGeneratedSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, options.GetWriterOptions()))
          JsonSerializer.WriteUsingGeneratedSerializer<TValue>(writer, in value, jsonTypeInfo);
        return byteBufferWriter.WrittenMemory.ToArray();
      }
    }

    private static byte[] WriteBytesUsingSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, options.GetWriterOptions()))
          JsonSerializer.WriteUsingSerializer<TValue>(writer, in value, jsonTypeInfo);
        return byteBufferWriter.WrittenMemory.ToArray();
      }
    }

    internal static MetadataPropertyName WriteReferenceForObject(
      JsonConverter jsonConverter,
      object currentValue,
      ref WriteStack state,
      Utf8JsonWriter writer)
    {
      MetadataPropertyName metadataPropertyName;
      if (state.BoxedStructReferenceId != null)
      {
        writer.WriteString(JsonSerializer.s_metadataId, state.BoxedStructReferenceId);
        metadataPropertyName = MetadataPropertyName.Id;
        state.BoxedStructReferenceId = (string) null;
      }
      else if (!jsonConverter.CanHaveIdMetadata || jsonConverter.IsValueType)
      {
        metadataPropertyName = MetadataPropertyName.NoMetadata;
      }
      else
      {
        bool alreadyExists;
        string reference = state.ReferenceResolver.GetReference(currentValue, out alreadyExists);
        if (alreadyExists)
        {
          writer.WriteString(JsonSerializer.s_metadataRef, reference);
          writer.WriteEndObject();
          metadataPropertyName = MetadataPropertyName.Ref;
        }
        else
        {
          writer.WriteString(JsonSerializer.s_metadataId, reference);
          metadataPropertyName = MetadataPropertyName.Id;
        }
      }
      return metadataPropertyName;
    }

    internal static MetadataPropertyName WriteReferenceForCollection(
      JsonConverter jsonConverter,
      object currentValue,
      ref WriteStack state,
      Utf8JsonWriter writer)
    {
      MetadataPropertyName metadataPropertyName;
      if (state.BoxedStructReferenceId != null)
      {
        writer.WriteStartObject();
        writer.WriteString(JsonSerializer.s_metadataId, state.BoxedStructReferenceId);
        writer.WriteStartArray(JsonSerializer.s_metadataValues);
        metadataPropertyName = MetadataPropertyName.Id;
        state.BoxedStructReferenceId = (string) null;
      }
      else if (!jsonConverter.CanHaveIdMetadata || jsonConverter.IsValueType)
      {
        writer.WriteStartArray();
        metadataPropertyName = MetadataPropertyName.NoMetadata;
      }
      else
      {
        bool alreadyExists;
        string reference = state.ReferenceResolver.GetReference(currentValue, out alreadyExists);
        if (alreadyExists)
        {
          writer.WriteStartObject();
          writer.WriteString(JsonSerializer.s_metadataRef, reference);
          writer.WriteEndObject();
          metadataPropertyName = MetadataPropertyName.Ref;
        }
        else
        {
          writer.WriteStartObject();
          writer.WriteString(JsonSerializer.s_metadataId, reference);
          writer.WriteStartArray(JsonSerializer.s_metadataValues);
          metadataPropertyName = MetadataPropertyName.Id;
        }
      }
      return metadataPropertyName;
    }

    internal static bool TryWriteReferenceForBoxedStruct(
      object currentValue,
      ref WriteStack state,
      Utf8JsonWriter writer)
    {
      bool alreadyExists;
      string reference = state.ReferenceResolver.GetReference(currentValue, out alreadyExists);
      if (alreadyExists)
      {
        writer.WriteStartObject();
        writer.WriteString(JsonSerializer.s_metadataRef, reference);
        writer.WriteEndObject();
      }
      else
        state.BoxedStructReferenceId = reference;
      return alreadyExists;
    }

    private static bool WriteCore<TValue>(
      JsonConverter jsonConverter,
      Utf8JsonWriter writer,
      in TValue value,
      JsonSerializerOptions options,
      ref WriteStack state)
    {
      bool flag = !(jsonConverter is JsonConverter<TValue> jsonConverter1) ? jsonConverter.WriteCoreAsObject(writer, (object) value, options, ref state) : jsonConverter1.WriteCore(writer, in value, options, ref state);
      writer.Flush();
      return flag;
    }

    private static void WriteUsingGeneratedSerializer<TValue>(
      Utf8JsonWriter writer,
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      if (jsonTypeInfo.HasSerialize && jsonTypeInfo is JsonTypeInfo<TValue> jsonTypeInfo1)
      {
        JsonSerializerContext context = jsonTypeInfo1.Options._context;
        if ((context != null ? (context.CanUseSerializationLogic ? 1 : 0) : 0) != 0)
        {
          jsonTypeInfo1.SerializeHandler(writer, value);
          writer.Flush();
          return;
        }
      }
      JsonSerializer.WriteUsingSerializer<TValue>(writer, in value, jsonTypeInfo);
    }

    private static void WriteUsingSerializer<TValue>(
      Utf8JsonWriter writer,
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      WriteStack state = new WriteStack();
      state.Initialize(jsonTypeInfo, false);
      JsonConverter converterBase = jsonTypeInfo.PropertyInfoForTypeInfo.ConverterBase;
      if (converterBase is JsonConverter<TValue> jsonConverter)
        jsonConverter.WriteCore(writer, in value, jsonTypeInfo.Options, ref state);
      else
        converterBase.WriteCoreAsObject(writer, (object) value, jsonTypeInfo.Options, ref state);
      writer.Flush();
    }

    private static Type GetRuntimeType<TValue>(in TValue value)
    {
      Type runtimeType = typeof (TValue);
      if (runtimeType == JsonTypeInfo.ObjectType && (object) value != null)
        runtimeType = value.GetType();
      return runtimeType;
    }

    private static Type GetRuntimeTypeAndValidateInputType(object value, Type inputType)
    {
      if ((object) inputType == null)
        throw new ArgumentNullException(nameof (inputType));
      if (value != null)
      {
        Type type = value.GetType();
        if (!inputType.IsAssignableFrom(type))
          ThrowHelper.ThrowArgumentException_DeserializeWrongType(inputType, value);
        if (inputType == JsonTypeInfo.ObjectType)
          return type;
      }
      return inputType;
    }


    #nullable enable
    /// <summary>Asynchronously converts a value of a type specified by a generic type parameter to UTF-8 encoded JSON text and writes it to a stream.</summary>
    /// <param name="utf8Json">The UTF-8 stream to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control serialization behavior.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the write operation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task SerializeAsync<TValue>(
      Stream utf8Json,
      TValue value,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      Type runtimeType = JsonSerializer.GetRuntimeType<TValue>(in value);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, runtimeType);
      return JsonSerializer.WriteStreamAsync<TValue>(utf8Json, value, typeInfo, cancellationToken);
    }

    /// <summary>Converts the provided value to UTF-8 encoded JSON text and write it to the <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="utf8Json">The UTF-8 <see cref="T:System.IO.Stream" /> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void Serialize<TValue>(
      Stream utf8Json,
      TValue value,
      JsonSerializerOptions? options = null)
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      Type runtimeType = JsonSerializer.GetRuntimeType<TValue>(in value);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, runtimeType);
      JsonSerializer.WriteStream<TValue>(utf8Json, in value, typeInfo);
    }

    /// <summary>Asynchronously converts the value of a specified type to UTF-8 encoded JSON text and writes it to the specified stream.</summary>
    /// <param name="utf8Json">The UTF-8 stream to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control serialization behavior.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the write operation.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> or <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task SerializeAsync(
      Stream utf8Json,
      object? value,
      Type inputType,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, validateInputType);
      return JsonSerializer.WriteStreamAsync<object>(utf8Json, value, typeInfo, cancellationToken);
    }

    /// <summary>Converts the provided value to UTF-8 encoded JSON text and write it to the <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="utf8Json">The UTF-8 <see cref="T:System.IO.Stream" /> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> or <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" />  or its serializable members.</exception>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void Serialize(
      Stream utf8Json,
      object? value,
      Type inputType,
      JsonSerializerOptions? options = null)
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, validateInputType);
      JsonSerializer.WriteStream<object>(utf8Json, in value, typeInfo);
    }

    /// <summary>Converts the provided value to UTF-8 encoded JSON text and write it to the <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="utf8Json">The UTF-8 <see cref="T:System.IO.Stream" /> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that can be used to cancel the write operation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task SerializeAsync<TValue>(
      Stream utf8Json,
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (jsonTypeInfo == null)
        throw new ArgumentNullException(nameof (jsonTypeInfo));
      return JsonSerializer.WriteStreamAsync<TValue>(utf8Json, value, (JsonTypeInfo) jsonTypeInfo, cancellationToken);
    }

    /// <summary>Converts the provided value to UTF-8 encoded JSON text and write it to the <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="utf8Json">The UTF-8 <see cref="T:System.IO.Stream" /> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    public static void Serialize<TValue>(
      Stream utf8Json,
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (jsonTypeInfo == null)
        throw new ArgumentNullException(nameof (jsonTypeInfo));
      JsonSerializer.WriteStream<TValue>(utf8Json, in value, (JsonTypeInfo) jsonTypeInfo);
    }

    /// <summary>Converts the provided value to UTF-8 encoded JSON text and write it to the <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="utf8Json">The UTF-8 <see cref="T:System.IO.Stream" /> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" /> that can be used to cancel the write operation.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" />, <paramref name="inputType" />, or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" />  or its serializable members.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public static Task SerializeAsync(
      Stream utf8Json,
      object? value,
      Type inputType,
      JsonSerializerContext context,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      return JsonSerializer.WriteStreamAsync<object>(utf8Json, value, JsonSerializer.GetTypeInfo(context, validateInputType), cancellationToken);
    }

    /// <summary>Converts the provided value to UTF-8 encoded JSON text and write it to the <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="utf8Json">The UTF-8 <see cref="T:System.IO.Stream" /> to write to.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="utf8Json" />, <paramref name="inputType" />, or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" />  or its serializable members.</exception>
    public static void Serialize(
      Stream utf8Json,
      object? value,
      Type inputType,
      JsonSerializerContext context)
    {
      if (utf8Json == null)
        throw new ArgumentNullException(nameof (utf8Json));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonSerializer.WriteStream<object>(utf8Json, in value, JsonSerializer.GetTypeInfo(context, validateInputType));
    }


    #nullable disable
    private static async Task WriteStreamAsync<TValue>(
      Stream utf8Json,
      TValue value,
      JsonTypeInfo jsonTypeInfo,
      CancellationToken cancellationToken)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      JsonWriterOptions writerOptions = options.GetWriterOptions();
      using (PooledByteBufferWriter bufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) bufferWriter, writerOptions))
        {
          WriteStack state = new WriteStack()
          {
            CancellationToken = cancellationToken
          };
          JsonConverter converter = state.Initialize(jsonTypeInfo, true);
          try
          {
            bool isFinalBlock;
            do
            {
              state.FlushThreshold = (int) ((double) bufferWriter.Capacity * 0.8999999761581421);
              try
              {
                isFinalBlock = JsonSerializer.WriteCore<TValue>(converter, writer, in value, options, ref state);
                if (state.SuppressFlush)
                {
                  state.SuppressFlush = false;
                }
                else
                {
                  await bufferWriter.WriteToStreamAsync(utf8Json, cancellationToken).ConfigureAwait(false);
                  bufferWriter.Clear();
                }
              }
              finally
              {
                if (state.PendingTask != null)
                {
                  try
                  {
                    await state.PendingTask.ConfigureAwait(false);
                  }
                  catch
                  {
                  }
                }
                List<IAsyncDisposable> asyncDisposables = state.CompletedAsyncDisposables;
                // ISSUE: explicit non-virtual call
                if ((asyncDisposables != null ? (__nonvirtual (asyncDisposables.Count) > 0 ? 1 : 0) : 0) != 0)
                  await state.DisposeCompletedAsyncDisposables().ConfigureAwait(false);
              }
            }
            while (!isFinalBlock);
          }
          catch (object ex)
          {
            await state.DisposePendingDisposablesOnExceptionAsync().ConfigureAwait(false);
            throw;
          }
          state = new WriteStack();
          converter = (JsonConverter) null;
        }
      }
      options = (JsonSerializerOptions) null;
    }

    private static void WriteStream<TValue>(
      Stream utf8Json,
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions options = jsonTypeInfo.Options;
      JsonWriterOptions writerOptions = options.GetWriterOptions();
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(options.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, writerOptions))
        {
          WriteStack state = new WriteStack();
          JsonConverter jsonConverter = state.Initialize(jsonTypeInfo, true);
          bool flag;
          do
          {
            state.FlushThreshold = (int) ((double) byteBufferWriter.Capacity * 0.8999999761581421);
            flag = JsonSerializer.WriteCore<TValue>(jsonConverter, writer, in value, options, ref state);
            byteBufferWriter.WriteToStream(utf8Json);
            byteBufferWriter.Clear();
          }
          while (!flag);
        }
      }
    }


    #nullable enable
    /// <summary>Converts the value of a type specified by a generic type parameter into a JSON string.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="options">Options to control serialization behavior.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <returns>A JSON string representation of the value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null)
    {
      Type runtimeType = JsonSerializer.GetRuntimeType<TValue>(in value);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, runtimeType);
      return JsonSerializer.WriteStringUsingSerializer<TValue>(in value, typeInfo);
    }

    /// <summary>Converts the value of a specified type into a JSON string.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <returns>The JSON string representation of the value.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static string Serialize(object? value, Type inputType, JsonSerializerOptions? options = null)
    {
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, validateInputType);
      return JsonSerializer.WriteStringUsingSerializer<object>(in value, typeInfo);
    }

    /// <summary>Converts the provided value into a <see cref="T:System.String" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.String" /> representation of the value.</returns>
    public static string Serialize<TValue>(TValue value, JsonTypeInfo<TValue> jsonTypeInfo) => JsonSerializer.WriteStringUsingGeneratedSerializer<TValue>(in value, (JsonTypeInfo) jsonTypeInfo);

    /// <summary>Converts the provided value into a <see cref="T:System.String" />.</summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inputType" /> or <paramref name="context" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.String" /> representation of the value.</returns>
    public static string Serialize(object? value, Type inputType, JsonSerializerContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(context, validateInputType);
      return JsonSerializer.WriteStringUsingGeneratedSerializer<object>(in value, typeInfo);
    }


    #nullable disable
    private static string WriteStringUsingGeneratedSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions serializerOptions = jsonTypeInfo != null ? jsonTypeInfo.Options : throw new ArgumentNullException(nameof (jsonTypeInfo));
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(serializerOptions.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, serializerOptions.GetWriterOptions()))
          JsonSerializer.WriteUsingGeneratedSerializer<TValue>(writer, in value, jsonTypeInfo);
        return JsonReaderHelper.TranscodeHelper(byteBufferWriter.WrittenMemory.Span);
      }
    }

    private static string WriteStringUsingSerializer<TValue>(
      in TValue value,
      JsonTypeInfo jsonTypeInfo)
    {
      JsonSerializerOptions serializerOptions = jsonTypeInfo != null ? jsonTypeInfo.Options : throw new ArgumentNullException(nameof (jsonTypeInfo));
      using (PooledByteBufferWriter byteBufferWriter = new PooledByteBufferWriter(serializerOptions.DefaultBufferSize))
      {
        using (Utf8JsonWriter writer = new Utf8JsonWriter((IBufferWriter<byte>) byteBufferWriter, serializerOptions.GetWriterOptions()))
          JsonSerializer.WriteUsingSerializer<TValue>(writer, in value, jsonTypeInfo);
        return JsonReaderHelper.TranscodeHelper(byteBufferWriter.WrittenMemory.Span);
      }
    }


    #nullable enable
    /// <summary>Writes the JSON representation of a type specified by a generic type parameter to the provided writer.</summary>
    /// <param name="writer">A JSON writer to write to.</param>
    /// <param name="value">The value to convert and write.</param>
    /// <param name="options">Options to control serialization behavior.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="writer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void Serialize<TValue>(
      Utf8JsonWriter writer,
      TValue value,
      JsonSerializerOptions? options = null)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      Type runtimeType = JsonSerializer.GetRuntimeType<TValue>(in value);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, runtimeType);
      JsonSerializer.WriteUsingSerializer<TValue>(writer, in value, typeInfo);
    }

    /// <summary>Writes the JSON representation of the specified type to the provided writer.</summary>
    /// <param name="writer">The JSON writer to write to.</param>
    /// <param name="value">The value to convert and write.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="options">Options to control serialization behavior.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" /></exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="writer" /> or <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static void Serialize(
      Utf8JsonWriter writer,
      object? value,
      Type inputType,
      JsonSerializerOptions? options = null)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonTypeInfo typeInfo = JsonSerializer.GetTypeInfo(options, validateInputType);
      JsonSerializer.WriteUsingSerializer<object>(writer, in value, typeInfo);
    }

    /// <summary>Writes one JSON value (including objects or arrays) to the provided writer.</summary>
    /// <param name="writer">The writer to write.</param>
    /// <param name="value">The value to convert and write.</param>
    /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="writer" /> or <paramref name="jsonTypeInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <typeparamref name="TValue" /> or its serializable members.</exception>
    public static void Serialize<TValue>(
      Utf8JsonWriter writer,
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (jsonTypeInfo == null)
        throw new ArgumentNullException(nameof (jsonTypeInfo));
      JsonSerializer.WriteUsingGeneratedSerializer<TValue>(writer, in value, (JsonTypeInfo) jsonTypeInfo);
    }

    /// <summary>Writes one JSON value (including objects or arrays) to the provided writer.</summary>
    /// <param name="writer">A JSON writer to write to.</param>
    /// <param name="value">The value to convert and write.</param>
    /// <param name="inputType">The type of the <paramref name="value" /> to convert.</param>
    /// <param name="context">A metadata provider for serializable types.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="inputType" /> is not compatible with <paramref name="value" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="writer" /> or <paramref name="inputType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">There is no compatible <see cref="T:System.Text.Json.Serialization.JsonConverter" /> for <paramref name="inputType" /> or its serializable members.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Text.Json.Serialization.JsonSerializerContext.GetTypeInfo(System.Type)" /> method of the provided <paramref name="context" /> returns <see langword="null" /> for the type to convert.</exception>
    public static void Serialize(
      Utf8JsonWriter writer,
      object? value,
      Type inputType,
      JsonSerializerContext context)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      Type validateInputType = JsonSerializer.GetRuntimeTypeAndValidateInputType(value, inputType);
      JsonSerializer.WriteUsingGeneratedSerializer<object>(writer, in value, JsonSerializer.GetTypeInfo(context, validateInputType));
    }
  }
}
