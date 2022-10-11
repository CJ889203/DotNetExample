// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Json.HttpClientJsonExtensions
// Assembly: System.Net.Http.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: E6D11FA5-4F09-4596-91F0-8A3CA774A5BE
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.Json.xml

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Net.Http.Json
{
  /// <summary>Contains extension methods to send and receive HTTP content as JSON.</summary>
  public static class HttpClientJsonExtensions
  {
    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="type">The type of the object to deserialize to and return.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<object?> GetFromJsonAsync(
      this HttpClient client,
      string? requestUri,
      Type type,
      JsonSerializerOptions? options,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return HttpClientJsonExtensions.GetFromJsonAsyncCore(client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken), type, options, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="type">The type of the object to deserialize to and return.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<object?> GetFromJsonAsync(
      this HttpClient client,
      Uri? requestUri,
      Type type,
      JsonSerializerOptions? options,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return HttpClientJsonExtensions.GetFromJsonAsyncCore(client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken), type, options, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<TValue?> GetFromJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      JsonSerializerOptions? options,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return HttpClientJsonExtensions.GetFromJsonAsyncCore<TValue>(client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken), options, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="options">Options to control the behavior during deserialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<TValue?> GetFromJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      JsonSerializerOptions? options,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return HttpClientJsonExtensions.GetFromJsonAsyncCore<TValue>(client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken), options, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="type">The type of the object to deserialize to and return.</param>
    /// <param name="context">Source generated JsonSerializerContext used to control the deserialization behavior.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<object?> GetFromJsonAsync(
      this HttpClient client,
      string? requestUri,
      Type type,
      JsonSerializerContext context,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return HttpClientJsonExtensions.GetFromJsonAsyncCore(client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken), type, context, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="type">The type of the object to deserialize to and return.</param>
    /// <param name="context">Source generated JsonSerializerContext used to control the deserialization behavior.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<object?> GetFromJsonAsync(
      this HttpClient client,
      Uri? requestUri,
      Type type,
      JsonSerializerContext context,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return HttpClientJsonExtensions.GetFromJsonAsyncCore(client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken), type, context, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="jsonTypeInfo">Source generated JsonTypeInfo to control the behavior during deserialization.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<TValue?> GetFromJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      JsonTypeInfo<TValue> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return HttpClientJsonExtensions.GetFromJsonAsyncCore<TValue>(client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken), jsonTypeInfo, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="jsonTypeInfo">Source generated JsonTypeInfo to control the behavior during deserialization.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<TValue?> GetFromJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      JsonTypeInfo<TValue> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      return HttpClientJsonExtensions.GetFromJsonAsyncCore<TValue>(client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken), jsonTypeInfo, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="type">The type of the object to deserialize to and return.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<object?> GetFromJsonAsync(
      this HttpClient client,
      string? requestUri,
      Type type,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return client.GetFromJsonAsync(requestUri, type, (JsonSerializerOptions) null, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="type">The type of the object to deserialize to and return.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<object?> GetFromJsonAsync(
      this HttpClient client,
      Uri? requestUri,
      Type type,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return client.GetFromJsonAsync(requestUri, type, (JsonSerializerOptions) null, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<TValue?> GetFromJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return client.GetFromJsonAsync<TValue>(requestUri, (JsonSerializerOptions) null, cancellationToken);
    }

    /// <summary>Sends a GET request to the specified Uri and returns the value that results from deserializing the response body as JSON in an asynchronous operation.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The target type to deserialize to.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<TValue?> GetFromJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return client.GetFromJsonAsync<TValue>(requestUri, (JsonSerializerOptions) null, cancellationToken);
    }


    #nullable disable
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    private static async Task<object> GetFromJsonAsyncCore(
      Task<HttpResponseMessage> taskResponse,
      Type type,
      JsonSerializerOptions options,
      CancellationToken cancellationToken)
    {
      object fromJsonAsyncCore;
      using (HttpResponseMessage response = await taskResponse.ConfigureAwait(false))
      {
        response.EnsureSuccessStatusCode();
        fromJsonAsyncCore = await ReadFromJsonAsyncHelper(response.Content, type, options, cancellationToken).ConfigureAwait(false);
      }
      return fromJsonAsyncCore;

      [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
      [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2067:UnrecognizedReflectionPattern", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
      static Task<object> ReadFromJsonAsyncHelper(
        HttpContent content,
        Type type,
        JsonSerializerOptions options,
        CancellationToken cancellationToken)
      {
        return content.ReadFromJsonAsync(type, options, cancellationToken);
      }
    }

    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    private static async Task<T> GetFromJsonAsyncCore<T>(
      Task<HttpResponseMessage> taskResponse,
      JsonSerializerOptions options,
      CancellationToken cancellationToken)
    {
      T fromJsonAsyncCore;
      using (HttpResponseMessage response = await taskResponse.ConfigureAwait(false))
      {
        response.EnsureSuccessStatusCode();
        fromJsonAsyncCore = await HttpClientJsonExtensions.ReadFromJsonAsyncHelper<T>(response.Content, options, cancellationToken).ConfigureAwait(false);
      }
      return fromJsonAsyncCore;
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2091:UnrecognizedReflectionPattern", Justification = "Workaround for https://github.com/mono/linker/issues/1416. The outer method is marked as RequiresUnreferencedCode.")]
    private static Task<T> ReadFromJsonAsyncHelper<T>(
      HttpContent content,
      JsonSerializerOptions options,
      CancellationToken cancellationToken)
    {
      return content.ReadFromJsonAsync<T>(options, cancellationToken);
    }

    private static async Task<object> GetFromJsonAsyncCore(
      Task<HttpResponseMessage> taskResponse,
      Type type,
      JsonSerializerContext context,
      CancellationToken cancellationToken)
    {
      object fromJsonAsyncCore;
      using (HttpResponseMessage response = await taskResponse.ConfigureAwait(false))
      {
        response.EnsureSuccessStatusCode();
        fromJsonAsyncCore = await response.Content.ReadFromJsonAsync(type, context, cancellationToken).ConfigureAwait(false);
      }
      return fromJsonAsyncCore;
    }

    private static async Task<T> GetFromJsonAsyncCore<T>(
      Task<HttpResponseMessage> taskResponse,
      JsonTypeInfo<T> jsonTypeInfo,
      CancellationToken cancellationToken)
    {
      T fromJsonAsyncCore;
      using (HttpResponseMessage response = await taskResponse.ConfigureAwait(false))
      {
        response.EnsureSuccessStatusCode();
        fromJsonAsyncCore = await response.Content.ReadFromJsonAsync<T>(jsonTypeInfo, cancellationToken).ConfigureAwait(false);
      }
      return fromJsonAsyncCore;
    }


    #nullable enable
    /// <summary>Sends a POST request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during serialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      TValue value,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      JsonContent content = JsonContent.Create<TValue>(value, options: options);
      return client.PostAsync(requestUri, (HttpContent) content, cancellationToken);
    }

    /// <summary>Sends a POST request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during serialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      TValue value,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      JsonContent content = JsonContent.Create<TValue>(value, options: options);
      return client.PostAsync(requestUri, (HttpContent) content, cancellationToken);
    }

    /// <summary>Sends a POST request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      TValue value,
      CancellationToken cancellationToken)
    {
      return client.PostAsJsonAsync<TValue>(requestUri, value, (JsonSerializerOptions) null, cancellationToken);
    }

    /// <summary>Sends a POST request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      TValue value,
      CancellationToken cancellationToken)
    {
      return client.PostAsJsonAsync<TValue>(requestUri, value, (JsonSerializerOptions) null, cancellationToken);
    }

    /// <summary>Sends a POST request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="jsonTypeInfo">Source generated JsonTypeInfo to control the behavior during serialization.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      JsonContent<TValue> content = new JsonContent<TValue>(value, jsonTypeInfo);
      return client.PostAsync(requestUri, (HttpContent) content, cancellationToken);
    }

    /// <summary>Sends a POST request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="jsonTypeInfo">Source generated JsonTypeInfo to control the behavior during serialization.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      JsonContent<TValue> content = new JsonContent<TValue>(value, jsonTypeInfo);
      return client.PostAsync(requestUri, (HttpContent) content, cancellationToken);
    }

    /// <summary>Send a PUT request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during serialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      TValue value,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      JsonContent content = JsonContent.Create<TValue>(value, options: options);
      return client.PutAsync(requestUri, (HttpContent) content, cancellationToken);
    }

    /// <summary>Send a PUT request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="options">Options to control the behavior during serialization. The default options are those specified by <see cref="F:System.Text.Json.JsonSerializerDefaults.Web" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      TValue value,
      JsonSerializerOptions? options = null,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      JsonContent content = JsonContent.Create<TValue>(value, options: options);
      return client.PutAsync(requestUri, (HttpContent) content, cancellationToken);
    }

    /// <summary>Send a PUT request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      TValue value,
      CancellationToken cancellationToken)
    {
      return client.PutAsJsonAsync<TValue>(requestUri, value, (JsonSerializerOptions) null, cancellationToken);
    }

    /// <summary>Send a PUT request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      TValue value,
      CancellationToken cancellationToken)
    {
      return client.PutAsJsonAsync<TValue>(requestUri, value, (JsonSerializerOptions) null, cancellationToken);
    }

    /// <summary>Send a PUT request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="jsonTypeInfo">Source generated JsonTypeInfo to control the behavior during serialization.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(
      this HttpClient client,
      string? requestUri,
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      JsonContent<TValue> content = new JsonContent<TValue>(value, jsonTypeInfo);
      return client.PutAsync(requestUri, (HttpContent) content, cancellationToken);
    }

    /// <summary>Send a PUT request to the specified Uri containing the <paramref name="value" /> serialized as JSON in the request body.</summary>
    /// <param name="client">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="value">The value to serialize.</param>
    /// <param name="jsonTypeInfo">Source generated JsonTypeInfo to control the behavior during serialization.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static Task<HttpResponseMessage> PutAsJsonAsync<TValue>(
      this HttpClient client,
      Uri? requestUri,
      TValue value,
      JsonTypeInfo<TValue> jsonTypeInfo,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (client == null)
        throw new ArgumentNullException(nameof (client));
      JsonContent<TValue> content = new JsonContent<TValue>(value, jsonTypeInfo);
      return client.PutAsync(requestUri, (HttpContent) content, cancellationToken);
    }
  }
}
