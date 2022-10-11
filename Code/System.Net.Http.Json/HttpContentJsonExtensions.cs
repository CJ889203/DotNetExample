// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Json.HttpContentJsonExtensions
// Assembly: System.Net.Http.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: E6D11FA5-4F09-4596-91F0-8A3CA774A5BE
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.Json.xml

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http.Json
{
  /// <summary>Contains extension methods to read and then parse the <see cref="T:System.Net.Http.HttpContent" /> from JSON.</summary>
  public static class HttpContentJsonExtensions
  {
    /// <summary>Reads the HTTP content and returns the value that results from deserializing the content as JSON in an asynchronous operation.</summary>
    /// <param name="content">The content to read from.</param>
    /// <param name="type">The type of the object to deserialize to and return.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<object?> ReadFromJsonAsync(
      this HttpContent content,
      Type type,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      Encoding encoding = JsonHelpers.GetEncoding(content.Headers.ContentType?.CharSet);
      return HttpContentJsonExtensions.ReadFromJsonAsyncCore(content, type, encoding, options, cancellationToken);
    }

    /// <summary>Reads the HTTP content and returns the value that results from deserializing the content as JSON in an asynchronous operation.</summary>
    /// <param name="content">The content to read from.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<T?> ReadFromJsonAsync<T>(
      this HttpContent content,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      Encoding encoding = JsonHelpers.GetEncoding(content.Headers.ContentType?.CharSet);
      return HttpContentJsonExtensions.ReadFromJsonAsyncCore<T>(content, encoding, options, cancellationToken);
    }


    #nullable disable
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    private static async Task<object> ReadFromJsonAsyncCore(
      HttpContent content,
      Type type,
      Encoding sourceEncoding,
      JsonSerializerOptions options,
      CancellationToken cancellationToken)
    {
      object obj;
      using (Stream contentStream = await HttpContentJsonExtensions.GetContentStream(content, sourceEncoding, cancellationToken).ConfigureAwait(false))
        obj = await DeserializeAsyncHelper(contentStream, type, options ?? JsonHelpers.s_defaultSerializerOptions, cancellationToken).ConfigureAwait(false);
      return obj;

      [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
      [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2067:UnrecognizedReflectionPattern", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
      static ValueTask<object> DeserializeAsyncHelper(
        Stream contentStream,
        Type returnType,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
      {
        return JsonSerializer.DeserializeAsync(contentStream, returnType, options, cancellationToken);
      }
    }

    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    private static async Task<T> ReadFromJsonAsyncCore<T>(
      HttpContent content,
      Encoding sourceEncoding,
      JsonSerializerOptions options,
      CancellationToken cancellationToken)
    {
      T obj;
      using (Stream contentStream = await HttpContentJsonExtensions.GetContentStream(content, sourceEncoding, cancellationToken).ConfigureAwait(false))
        obj = await HttpContentJsonExtensions.DeserializeAsyncHelper<T>(contentStream, options ?? JsonHelpers.s_defaultSerializerOptions, cancellationToken).ConfigureAwait(false);
      return obj;
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2091:UnrecognizedReflectionPattern", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
    private static ValueTask<TValue> DeserializeAsyncHelper<TValue>(
      Stream contentStream,
      JsonSerializerOptions options,
      CancellationToken cancellationToken)
    {
      return JsonSerializer.DeserializeAsync<TValue>(contentStream, options, cancellationToken);
    }


    #nullable enable
    /// <summary>Reads the HTTP content and returns the value that results from deserializing the content as JSON in an asynchronous operation.</summary>
    /// <param name="content">The content to read from.</param>
    /// <param name="type">The type of the object to deserialize to and return.</param>
    /// <param name="context">Source generated JsonSerializerContext used to control the behavior during deserialization.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<object?> ReadFromJsonAsync(
      this HttpContent content,
      Type type,
      JsonSerializerContext context,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      Encoding encoding = JsonHelpers.GetEncoding(content.Headers.ContentType?.CharSet);
      return HttpContentJsonExtensions.ReadFromJsonAsyncCore(content, type, encoding, context, cancellationToken);
    }

    /// <summary>Reads the HTTP content and returns the value that results from deserializing the content as JSON in an asynchronous operation.</summary>
    /// <param name="content">The content to read from.</param>
    /// <param name="jsonTypeInfo">Source generated JsonTypeInfo to control the behavior during deserialization.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="T">The target type to deserialize to.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<T?> ReadFromJsonAsync<T>(
      this HttpContent content,
      JsonTypeInfo<T> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      Encoding encoding = JsonHelpers.GetEncoding(content.Headers.ContentType?.CharSet);
      return HttpContentJsonExtensions.ReadFromJsonAsyncCore<T>(content, encoding, jsonTypeInfo, cancellationToken);
    }


    #nullable disable
    private static async Task<object> ReadFromJsonAsyncCore(
      HttpContent content,
      Type type,
      Encoding sourceEncoding,
      JsonSerializerContext context,
      CancellationToken cancellationToken)
    {
      object obj;
      using (Stream contentStream = await HttpContentJsonExtensions.GetContentStream(content, sourceEncoding, cancellationToken).ConfigureAwait(false))
        obj = await JsonSerializer.DeserializeAsync(contentStream, type, context, cancellationToken).ConfigureAwait(false);
      return obj;
    }

    private static async Task<T> ReadFromJsonAsyncCore<T>(
      HttpContent content,
      Encoding sourceEncoding,
      JsonTypeInfo<T> jsonTypeInfo,
      CancellationToken cancellationToken)
    {
      T obj;
      using (Stream contentStream = await HttpContentJsonExtensions.GetContentStream(content, sourceEncoding, cancellationToken).ConfigureAwait(false))
        obj = await JsonSerializer.DeserializeAsync<T>(contentStream, jsonTypeInfo, cancellationToken).ConfigureAwait(false);
      return obj;
    }

    private static async Task<Stream> GetContentStream(
      HttpContent content,
      Encoding sourceEncoding,
      CancellationToken cancellationToken)
    {
      Stream contentStream = await HttpContentJsonExtensions.ReadHttpContentStreamAsync(content, cancellationToken).ConfigureAwait(false);
      if (sourceEncoding != null && sourceEncoding != Encoding.UTF8)
        contentStream = HttpContentJsonExtensions.GetTranscodingStream(contentStream, sourceEncoding);
      return contentStream;
    }

    private static Task<Stream> ReadHttpContentStreamAsync(
      HttpContent content,
      CancellationToken cancellationToken)
    {
      return content.ReadAsStreamAsync(cancellationToken);
    }

    private static Stream GetTranscodingStream(Stream contentStream, Encoding sourceEncoding) => Encoding.CreateTranscodingStream(contentStream, sourceEncoding, Encoding.UTF8);
  }
}
