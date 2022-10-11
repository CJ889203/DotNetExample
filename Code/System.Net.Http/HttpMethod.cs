// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HttpMethod
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Diagnostics.CodeAnalysis;
using System.Net.Http.QPack;
using System.Threading;


#nullable enable
namespace System.Net.Http
{
  /// <summary>A helper class for retrieving and comparing standard HTTP methods and for creating new HTTP methods.</summary>
  public class HttpMethod : IEquatable<HttpMethod>
  {

    #nullable disable
    private readonly string _method;
    private readonly int? _http3Index;
    private int _hashcode;
    private static readonly HttpMethod s_getMethod = new HttpMethod("GET", 17);
    private static readonly HttpMethod s_putMethod = new HttpMethod("PUT", 21);
    private static readonly HttpMethod s_postMethod = new HttpMethod("POST", 20);
    private static readonly HttpMethod s_deleteMethod = new HttpMethod("DELETE", 16);
    private static readonly HttpMethod s_headMethod = new HttpMethod("HEAD", 18);
    private static readonly HttpMethod s_optionsMethod = new HttpMethod("OPTIONS", 19);
    private static readonly HttpMethod s_traceMethod = new HttpMethod("TRACE", -1);
    private static readonly HttpMethod s_patchMethod = new HttpMethod("PATCH", -1);
    private static readonly HttpMethod s_connectMethod = new HttpMethod("CONNECT", 15);
    private byte[] _http3EncodedBytes;


    #nullable enable
    /// <summary>Represents an HTTP GET protocol method.</summary>
    /// <returns>Returns <see cref="T:System.Net.Http.HttpMethod" />.</returns>
    public static HttpMethod Get => HttpMethod.s_getMethod;

    /// <summary>Represents an HTTP PUT protocol method that is used to replace an entity identified by a URI.</summary>
    /// <returns>Returns <see cref="T:System.Net.Http.HttpMethod" />.</returns>
    public static HttpMethod Put => HttpMethod.s_putMethod;

    /// <summary>Represents an HTTP POST protocol method that is used to post a new entity as an addition to a URI.</summary>
    /// <returns>Returns <see cref="T:System.Net.Http.HttpMethod" />.</returns>
    public static HttpMethod Post => HttpMethod.s_postMethod;

    /// <summary>Represents an HTTP DELETE protocol method.</summary>
    /// <returns>Returns <see cref="T:System.Net.Http.HttpMethod" />.</returns>
    public static HttpMethod Delete => HttpMethod.s_deleteMethod;

    /// <summary>Represents an HTTP HEAD protocol method. The HEAD method is identical to GET except that the server only returns message-headers in the response, without a message-body.</summary>
    /// <returns>Returns <see cref="T:System.Net.Http.HttpMethod" />.</returns>
    public static HttpMethod Head => HttpMethod.s_headMethod;

    /// <summary>Represents an HTTP OPTIONS protocol method.</summary>
    /// <returns>Returns <see cref="T:System.Net.Http.HttpMethod" />.</returns>
    public static HttpMethod Options => HttpMethod.s_optionsMethod;

    /// <summary>Represents an HTTP TRACE protocol method.</summary>
    /// <returns>Returns <see cref="T:System.Net.Http.HttpMethod" />.</returns>
    public static HttpMethod Trace => HttpMethod.s_traceMethod;

    /// <summary>Gets the HTTP PATCH protocol method.</summary>
    /// <returns>The HTTP PATCH protocol method.</returns>
    public static HttpMethod Patch => HttpMethod.s_patchMethod;

    internal static HttpMethod Connect => HttpMethod.s_connectMethod;

    /// <summary>An HTTP method.</summary>
    /// <returns>An HTTP method represented as a <see cref="T:System.String" />.</returns>
    public string Method => this._method;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.HttpMethod" /> class with a specific HTTP method.</summary>
    /// <param name="method">The HTTP method.</param>
    public HttpMethod(string method)
    {
      if (string.IsNullOrEmpty(method))
        throw new ArgumentException(SR.net_http_argument_empty_string, nameof (method));
      this._method = HttpRuleParser.GetTokenLength(method, 0) == method.Length ? method : throw new FormatException(SR.net_http_httpmethod_format_error);
    }


    #nullable disable
    private HttpMethod(string method, int http3StaticTableIndex)
    {
      this._method = method;
      this._http3Index = new int?(http3StaticTableIndex);
    }


    #nullable enable
    /// <summary>Determines whether the specified <see cref="T:System.Net.Http.HttpMethod" /> is equal to the current <see cref="T:System.Object" />.</summary>
    /// <param name="other">The HTTP method to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public bool Equals([NotNullWhen(true)] HttpMethod? other)
    {
      if ((object) other == null)
        return false;
      return (object) this._method == (object) other._method || string.Equals(this._method, other._method, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => this.Equals(obj as HttpMethod);

    /// <summary>Serves as a hash function for this type.</summary>
    /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
    public override int GetHashCode()
    {
      if (this._hashcode == 0)
        this._hashcode = StringComparer.OrdinalIgnoreCase.GetHashCode(this._method);
      return this._hashcode;
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string representing the current object.</returns>
    public override string ToString() => this._method;

    /// <summary>The equality operator for comparing two <see cref="T:System.Net.Http.HttpMethod" /> objects.</summary>
    /// <param name="left">The left <see cref="T:System.Net.Http.HttpMethod" /> to an equality operator.</param>
    /// <param name="right">The right  <see cref="T:System.Net.Http.HttpMethod" /> to an equality operator.</param>
    /// <returns>
    /// <see langword="true" /> if the specified <paramref name="left" /> and <paramref name="right" /> parameters are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(HttpMethod? left, HttpMethod? right) => (object) left != null && (object) right != null ? left.Equals(right) : (object) left == (object) right;

    /// <summary>The inequality operator for comparing two <see cref="T:System.Net.Http.HttpMethod" /> objects.</summary>
    /// <param name="left">The left <see cref="T:System.Net.Http.HttpMethod" /> to an inequality operator.</param>
    /// <param name="right">The right  <see cref="T:System.Net.Http.HttpMethod" /> to an inequality operator.</param>
    /// <returns>
    /// <see langword="true" /> if the specified <paramref name="left" /> and <paramref name="right" /> parameters are inequal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(HttpMethod? left, HttpMethod? right) => !(left == right);


    #nullable disable
    internal static HttpMethod Normalize(HttpMethod method)
    {
      if (!method._http3Index.HasValue && method._method.Length >= 3)
      {
        HttpMethod httpMethod1;
        switch ((int) method._method[0] | 32)
        {
          case 99:
            httpMethod1 = HttpMethod.s_connectMethod;
            break;
          case 100:
            httpMethod1 = HttpMethod.s_deleteMethod;
            break;
          case 103:
            httpMethod1 = HttpMethod.s_getMethod;
            break;
          case 104:
            httpMethod1 = HttpMethod.s_headMethod;
            break;
          case 111:
            httpMethod1 = HttpMethod.s_optionsMethod;
            break;
          case 112:
            HttpMethod httpMethod2;
            switch (method._method.Length)
            {
              case 3:
                httpMethod2 = HttpMethod.s_putMethod;
                break;
              case 4:
                httpMethod2 = HttpMethod.s_postMethod;
                break;
              default:
                httpMethod2 = HttpMethod.s_patchMethod;
                break;
            }
            httpMethod1 = httpMethod2;
            break;
          case 116:
            httpMethod1 = HttpMethod.s_traceMethod;
            break;
          default:
            httpMethod1 = (HttpMethod) null;
            break;
        }
        HttpMethod httpMethod3 = httpMethod1;
        if ((object) httpMethod3 != null && string.Equals(method._method, httpMethod3._method, StringComparison.OrdinalIgnoreCase))
          return httpMethod3;
      }
      return method;
    }

    internal bool MustHaveRequestBody => (object) this != (object) HttpMethod.Get && (object) this != (object) HttpMethod.Head && (object) this != (object) HttpMethod.Connect && (object) this != (object) HttpMethod.Options && (object) this != (object) HttpMethod.Delete;


    #nullable enable
    internal byte[] Http3EncodedBytes
    {
      get
      {
        byte[] http3EncodedBytes = Volatile.Read<byte[]>(ref this._http3EncodedBytes);
        if (http3EncodedBytes == null)
        {
          ref byte[] local = ref this._http3EncodedBytes;
          int? http3Index = this._http3Index;
          byte[] array;
          if (http3Index.HasValue)
          {
            int valueOrDefault = http3Index.GetValueOrDefault();
            if (valueOrDefault >= 0)
            {
              array = QPackEncoder.EncodeStaticIndexedHeaderFieldToArray(valueOrDefault);
              goto label_5;
            }
          }
          array = QPackEncoder.EncodeLiteralHeaderFieldWithStaticNameReferenceToArray(17, this._method);
label_5:
          http3EncodedBytes = array;
          Volatile.Write<byte[]>(ref local, array);
        }
        return http3EncodedBytes;
      }
    }
  }
}
