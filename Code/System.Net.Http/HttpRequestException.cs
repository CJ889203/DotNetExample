// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpRequestException
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml


#nullable enable
namespace System.Net.Http
{
  /// <summary>A base class for exceptions thrown by the <see cref="T:System.Net.Http.HttpClient" /> and <see cref="T:System.Net.Http.HttpMessageHandler" /> classes.</summary>
  public class HttpRequestException : Exception
  {
    internal RequestRetryType AllowRetry { get; }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpRequestException" /> class.</summary>
    public HttpRequestException()
      : this((string) null, (Exception) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpRequestException" /> class with a specific message that describes the current exception.</summary>
    /// <param name="message">A message that describes the current exception.</param>
    public HttpRequestException(string? message)
      : this(message, (Exception) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpRequestException" /> class with a specific message that describes the current exception and an inner exception.</summary>
    /// <param name="message">A message that describes the current exception.</param>
    /// <param name="inner">The inner exception.</param>
    public HttpRequestException(string? message, Exception? inner)
      : base(message, inner)
    {
      if (inner == null)
        return;
      this.HResult = inner.HResult;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpRequestException" /> class with a specific message that describes the current exception, an inner exception, and an HTTP status code.</summary>
    /// <param name="message">A message that describes the current exception.</param>
    /// <param name="inner">The inner exception.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    public HttpRequestException(string? message, Exception? inner, HttpStatusCode? statusCode)
      : this(message, inner)
    {
      this.StatusCode = statusCode;
    }

    /// <summary>Gets the HTTP status code to be returned with the exception.</summary>
    /// <returns>An HTTP status code if the exception represents a non-successful result, otherwise <c>null</c>.</returns>
    public HttpStatusCode? StatusCode { get; }


    #nullable disable
    internal HttpRequestException(string message, Exception inner, RequestRetryType allowRetry)
      : this(message, inner)
    {
      this.AllowRetry = allowRetry;
    }
  }
}
