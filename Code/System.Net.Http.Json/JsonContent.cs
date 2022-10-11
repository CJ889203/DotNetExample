// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Json.JsonContent
// Assembly: System.Net.Http.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: E6D11FA5-4F09-4596-91F0-8A3CA774A5BE
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.Json.xml

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http.Json
{
  /// <summary>Provides HTTP content based on JSON.</summary>
  public sealed class JsonContent : HttpContent
  {

    #nullable disable
    private readonly JsonSerializerOptions _jsonSerializerOptions;


    #nullable enable
    /// <summary>Gets the type of the <see cref="P:System.Net.Http.Json.JsonContent.Value" /> to be serialized by this instance.</summary>
    /// <returns>The type of the <see cref="P:System.Net.Http.Json.JsonContent.Value" /> to be serialized by this instance.</returns>
    public Type ObjectType { get; }

    /// <summary>Gets the value to be serialized and used as the body of the <see cref="T:System.Net.Http.HttpRequestMessage" /> that sends this instance.</summary>
    /// <returns>The value to be serialized and used as the body of the <see cref="T:System.Net.Http.HttpRequestMessage" /> that sends this instance.</returns>
    public object? Value { get; }


    #nullable disable
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    private JsonContent(
      object inputValue,
      Type inputType,
      MediaTypeHeaderValue mediaType,
      JsonSerializerOptions options)
    {
      if (inputType == (Type) null)
        throw new ArgumentNullException(nameof (inputType));
      this.Value = inputValue == null || inputType.IsAssignableFrom(inputValue.GetType()) ? inputValue : throw new ArgumentException(SR.Format(SR.SerializeWrongType, (object) inputType, (object) inputValue.GetType()));
      this.ObjectType = inputType;
      this.Headers.ContentType = mediaType ?? JsonHelpers.GetDefaultMediaType();
      this._jsonSerializerOptions = options ?? JsonHelpers.s_defaultSerializerOptions;
    }


    #nullable enable
    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.Json.JsonContent" /> class that will contain the <paramref name="inputValue" /> serialized as JSON.</summary>
    /// <param name="inputValue">The value to serialize.</param>
    /// <param name="mediaType">The media type to use for the content.</param>
    /// <param name="options">Options to control the behavior during serialization, the default options are <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <returns>A <see cref="T:System.Net.Http.Json.JsonContent" /> instance.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static JsonContent Create<T>(
      T inputValue,
      MediaTypeHeaderValue? mediaType = null,
      JsonSerializerOptions? options = null)
    {
      return JsonContent.Create((object) inputValue, typeof (T), mediaType, options);
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Net.Http.Json.JsonContent" /> class that will contain the <paramref name="inputValue" /> serialized as JSON.</summary>
    /// <param name="inputValue">The value to serialize.</param>
    /// <param name="inputType">The type of the value to serialize.</param>
    /// <param name="mediaType">The media type to use for the content.</param>
    /// <param name="options">Options to control the behavior during serialization, the default options are <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <returns>A <see cref="T:System.Net.Http.Json.JsonContent" /> instance.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static JsonContent Create(
      object? inputValue,
      Type inputType,
      MediaTypeHeaderValue? mediaType = null,
      JsonSerializerOptions? options = null)
    {
      return new JsonContent(inputValue, inputType, mediaType, options);
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) => this.SerializeToStreamAsyncCore(stream, true, CancellationToken.None);

    protected override bool TryComputeLength(out long length)
    {
      length = 0L;
      return false;
    }


    #nullable disable
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "The ctor is annotated with RequiresUnreferencedCode.")]
    private async Task SerializeToStreamAsyncCore(
      Stream targetStream,
      bool async,
      CancellationToken cancellationToken)
    {
      JsonContent jsonContent = this;
      Encoding encoding = JsonHelpers.GetEncoding(jsonContent.Headers.ContentType?.CharSet);
      if (encoding != null && encoding != Encoding.UTF8)
      {
        Stream transcodingStream = Encoding.CreateTranscodingStream(targetStream, encoding, Encoding.UTF8, true);
        try
        {
          if (async)
            await SerializeAsyncHelper(transcodingStream, jsonContent.Value, jsonContent.ObjectType, jsonContent._jsonSerializerOptions, cancellationToken).ConfigureAwait(false);
          else
            SerializeSyncHelper(transcodingStream, jsonContent.Value, jsonContent.ObjectType, jsonContent._jsonSerializerOptions);
        }
        finally
        {
          if (async)
            await transcodingStream.DisposeAsync().ConfigureAwait(false);
          else
            transcodingStream.Dispose();
        }
        transcodingStream = (Stream) null;
      }
      else if (async)
        await SerializeAsyncHelper(targetStream, jsonContent.Value, jsonContent.ObjectType, jsonContent._jsonSerializerOptions, cancellationToken).ConfigureAwait(false);
      else
        SerializeSyncHelper(targetStream, jsonContent.Value, jsonContent.ObjectType, jsonContent._jsonSerializerOptions);

      [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
      static void SerializeSyncHelper(
        Stream utf8Json,
        object value,
        Type inputType,
        JsonSerializerOptions options)
      {
        JsonSerializer.Serialize(utf8Json, value, inputType, options);
      }

      [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
      static Task SerializeAsyncHelper(
        Stream utf8Json,
        object value,
        Type inputType,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
      {
        return JsonSerializer.SerializeAsync(utf8Json, value, inputType, options, cancellationToken);
      }
    }


    #nullable enable
    protected override void SerializeToStream(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      this.SerializeToStreamAsyncCore(stream, false, cancellationToken).GetAwaiter().GetResult();
    }

    protected override Task SerializeToStreamAsync(
      Stream stream,
      TransportContext? context,
      CancellationToken cancellationToken)
    {
      return this.SerializeToStreamAsyncCore(stream, true, cancellationToken);
    }
  }
}
