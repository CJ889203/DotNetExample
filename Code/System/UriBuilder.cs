// Decompiled with JetBrains decompiler
// Type: System.UriBuilder
// Assembly: System.Private.Uri, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: EFC59026-8404-447E-A976-365A5080B26A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Uri.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Text;


#nullable enable
namespace System
{
  /// <summary>Provides a custom constructor for uniform resource identifiers (URIs) and modifies URIs for the <see cref="T:System.Uri" /> class.</summary>
  public class UriBuilder
  {

    #nullable disable
    private string _scheme = "http";
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _host = "localhost";
    private int _port = -1;
    private string _path = "/";
    private string _query = string.Empty;
    private string _fragment = string.Empty;
    private bool _changed = true;
    private Uri _uri;

    /// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class.</summary>
    public UriBuilder()
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified URI.</summary>
    /// <param name="uri">A URI string.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="uri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.UriFormatException">
    ///         <paramref name="uri" /> is a zero-length string or contains only spaces.
    /// 
    ///  -or-
    /// 
    ///  The parsing routine detected a scheme in an invalid form.
    /// 
    ///  -or-
    /// 
    ///  The parser detected more than two consecutive slashes in a URI that does not use the "file" scheme.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="uri" /> is not a valid URI.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.</exception>
    public UriBuilder(string uri)
    {
      this._uri = new Uri(uri, UriKind.RelativeOrAbsolute);
      if (!this._uri.IsAbsoluteUri)
        this._uri = new Uri(Uri.UriSchemeHttp + Uri.SchemeDelimiter + uri);
      this.SetFieldsFromUri();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified <see cref="T:System.Uri" /> instance.</summary>
    /// <param name="uri">An instance of the <see cref="T:System.Uri" /> class.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="uri" /> is <see langword="null" />.</exception>
    public UriBuilder(Uri uri)
    {
      this._uri = uri ?? throw new ArgumentNullException(nameof (uri));
      this.SetFieldsFromUri();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme and host.</summary>
    /// <param name="schemeName">An Internet access protocol.</param>
    /// <param name="hostName">A DNS-style domain name or IP address.</param>
    public UriBuilder(string? schemeName, string? hostName)
    {
      this.Scheme = schemeName;
      this.Host = hostName;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, and port.</summary>
    /// <param name="scheme">An Internet access protocol.</param>
    /// <param name="host">A DNS-style domain name or IP address.</param>
    /// <param name="portNumber">An IP port number for the service.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="portNumber" /> is less than -1 or greater than 65,535.</exception>
    public UriBuilder(string? scheme, string? host, int portNumber)
      : this(scheme, host)
    {
      this.Port = portNumber;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, port number, and path.</summary>
    /// <param name="scheme">An Internet access protocol.</param>
    /// <param name="host">A DNS-style domain name or IP address.</param>
    /// <param name="port">An IP port number for the service.</param>
    /// <param name="pathValue">The path to the Internet resource.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="port" /> is less than -1 or greater than 65,535.</exception>
    public UriBuilder(string? scheme, string? host, int port, string? pathValue)
      : this(scheme, host, port)
    {
      this.Path = pathValue;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, port number, path, and query string or fragment identifier.</summary>
    /// <param name="scheme">An Internet access protocol.</param>
    /// <param name="host">A DNS-style domain name or IP address.</param>
    /// <param name="port">An IP port number for the service.</param>
    /// <param name="path">The path to the Internet resource.</param>
    /// <param name="extraValue">A query string or fragment identifier.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="extraValue" /> is neither <see langword="null" /> nor <see cref="F:System.String.Empty" />, nor does a valid fragment identifier begin with a number sign (#), nor a valid query string begin with a question mark (?).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="port" /> is less than -1 or greater than 65,535.</exception>
    public UriBuilder(string? scheme, string? host, int port, string? path, string? extraValue)
      : this(scheme, host, port, path)
    {
      if (string.IsNullOrEmpty(extraValue))
        return;
      if (extraValue[0] == '#')
      {
        this._fragment = extraValue;
      }
      else
      {
        int num = extraValue[0] == '?' ? extraValue.IndexOf('#') : throw new ArgumentException(SR.Argument_ExtraNotValid, nameof (extraValue));
        if (num == -1)
        {
          this._query = extraValue;
        }
        else
        {
          this._query = extraValue.Substring(0, num);
          this._fragment = extraValue.Substring(num);
        }
      }
      if (this._query.Length == 1)
        this._query = string.Empty;
      if (this._fragment.Length != 1)
        return;
      this._fragment = string.Empty;
    }

    /// <summary>Gets or sets the scheme name of the URI.</summary>
    /// <exception cref="T:System.ArgumentException">The scheme cannot be set to an invalid scheme name.</exception>
    /// <returns>The scheme of the URI.</returns>
    public string Scheme
    {
      get => this._scheme;
      [param: AllowNull] set
      {
        if (value == null)
          value = string.Empty;
        if (value.Length != 0)
        {
          if (!Uri.CheckSchemeName(value))
          {
            int length = value.IndexOf(':');
            if (length != -1)
              value = value.Substring(0, length);
            if (!Uri.CheckSchemeName(value))
              throw new ArgumentException(SR.net_uri_BadScheme, nameof (value));
          }
          value = value.ToLowerInvariant();
        }
        this._scheme = value;
        this._changed = true;
      }
    }

    /// <summary>Gets or sets the user name associated with the user that accesses the URI.</summary>
    /// <returns>The name of the user that accesses the URI.</returns>
    public string UserName
    {
      get => this._username;
      [param: AllowNull] set
      {
        this._username = value ?? string.Empty;
        this._changed = true;
      }
    }

    /// <summary>Gets or sets the password associated with the user that accesses the URI.</summary>
    /// <returns>The password of the user that accesses the URI.</returns>
    public string Password
    {
      get => this._password;
      [param: AllowNull] set
      {
        this._password = value ?? string.Empty;
        this._changed = true;
      }
    }

    /// <summary>Gets or sets the Domain Name System (DNS) host name or IP address of a server.</summary>
    /// <returns>The DNS host name or IP address of the server.</returns>
    public string Host
    {
      get => this._host;
      [param: AllowNull] set
      {
        if (!string.IsNullOrEmpty(value) && value.Contains(':') && value[0] != '[')
          value = "[" + value + "]";
        this._host = value ?? string.Empty;
        this._changed = true;
      }
    }

    /// <summary>Gets or sets the port number of the URI.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The port cannot be set to a value less than -1 or greater than 65,535.</exception>
    /// <returns>The port number of the URI.</returns>
    public int Port
    {
      get => this._port;
      set
      {
        this._port = value >= -1 && value <= (int) ushort.MaxValue ? value : throw new ArgumentOutOfRangeException(nameof (value));
        this._changed = true;
      }
    }

    /// <summary>Gets or sets the path to the resource referenced by the URI.</summary>
    /// <returns>The path to the resource referenced by the URI.</returns>
    public string Path
    {
      get => this._path;
      [param: AllowNull] set
      {
        this._path = string.IsNullOrEmpty(value) ? "/" : Uri.InternalEscapeString(value.Replace('\\', '/'));
        this._changed = true;
      }
    }

    /// <summary>Gets or sets any query information included in the URI.</summary>
    /// <returns>The query information included in the URI.</returns>
    public string Query
    {
      get => this._query;
      [param: AllowNull] set
      {
        if (!string.IsNullOrEmpty(value) && value[0] != '?')
          value = "?" + value;
        this._query = value ?? string.Empty;
        this._changed = true;
      }
    }

    /// <summary>Gets or sets the fragment portion of the URI.</summary>
    /// <returns>The fragment portion of the URI.</returns>
    public string Fragment
    {
      get => this._fragment;
      [param: AllowNull] set
      {
        if (!string.IsNullOrEmpty(value) && value[0] != '#')
          value = "#" + value;
        this._fragment = value ?? string.Empty;
        this._changed = true;
      }
    }

    /// <summary>Gets the <see cref="T:System.Uri" /> instance constructed by the specified <see cref="T:System.UriBuilder" /> instance.</summary>
    /// <exception cref="T:System.UriFormatException">The URI constructed by the <see cref="T:System.UriBuilder" /> properties is invalid.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.</exception>
    /// <returns>The URI constructed by the <see cref="T:System.UriBuilder" />.</returns>
    public Uri Uri
    {
      get
      {
        if (this._changed)
        {
          this._uri = new Uri(this.ToString());
          this.SetFieldsFromUri();
          this._changed = false;
        }
        return this._uri;
      }
    }

    /// <summary>Compares an existing <see cref="T:System.Uri" /> instance with the contents of the <see cref="T:System.UriBuilder" /> for equality.</summary>
    /// <param name="rparam">The object to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="rparam" /> represents the same <see cref="T:System.Uri" /> as the <see cref="T:System.Uri" /> constructed by this <see cref="T:System.UriBuilder" /> instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? rparam) => rparam != null && this.Uri.Equals((object) rparam.ToString());

    /// <summary>Returns the hash code for the URI.</summary>
    /// <returns>The hash code generated for the URI.</returns>
    public override int GetHashCode() => this.Uri.GetHashCode();

    private void SetFieldsFromUri()
    {
      this._scheme = this._uri.Scheme;
      this._host = this._uri.Host;
      this._port = this._uri.Port;
      this._path = this._uri.AbsolutePath;
      this._query = this._uri.Query;
      this._fragment = this._uri.Fragment;
      string userInfo = this._uri.UserInfo;
      if (userInfo.Length <= 0)
        return;
      int length = userInfo.IndexOf(':');
      if (length != -1)
      {
        this._password = userInfo.Substring(length + 1);
        this._username = userInfo.Substring(0, length);
      }
      else
        this._username = userInfo;
    }

    /// <summary>Returns the display string for the specified <see cref="T:System.UriBuilder" /> instance.</summary>
    /// <exception cref="T:System.UriFormatException">The <see cref="T:System.UriBuilder" /> instance has a bad password.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.</exception>
    /// <returns>The string that contains the unescaped display string of the <see cref="T:System.UriBuilder" />.</returns>
    public override unsafe string ToString()
    {
      if (this.UserName.Length == 0 && this.Password.Length != 0)
        throw new UriFormatException(SR.net_uri_BadUserPassword);
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(1024)), 512));
      string scheme = this.Scheme;
      string host = this.Host;
      if (scheme.Length != 0)
      {
        UriParser syntax = UriParser.GetSyntax(scheme);
        string s = syntax != null ? (syntax.InFact(UriSyntaxFlags.MustHaveAuthority) || host.Length != 0 && syntax.NotAny(UriSyntaxFlags.MailToLikeUri) && syntax.InFact(UriSyntaxFlags.OptionalAuthority) ? Uri.SchemeDelimiter : ":") : (host.Length == 0 ? ":" : Uri.SchemeDelimiter);
        valueStringBuilder.Append(scheme);
        valueStringBuilder.Append(s);
      }
      string userName = this.UserName;
      if (userName.Length != 0)
      {
        valueStringBuilder.Append(userName);
        string password = this.Password;
        if (password.Length != 0)
        {
          valueStringBuilder.Append(':');
          valueStringBuilder.Append(password);
        }
        valueStringBuilder.Append('@');
      }
      if (host.Length != 0)
      {
        valueStringBuilder.Append(host);
        if (this._port != -1)
        {
          valueStringBuilder.Append(':');
          int charsWritten;
          this._port.TryFormat(valueStringBuilder.AppendSpan(5), out charsWritten);
          valueStringBuilder.Length -= 5 - charsWritten;
        }
      }
      string path = this.Path;
      if (path.Length != 0)
      {
        if (!path.StartsWith('/') && host.Length != 0)
          valueStringBuilder.Append('/');
        valueStringBuilder.Append(path);
      }
      valueStringBuilder.Append(this.Query);
      valueStringBuilder.Append(this.Fragment);
      return valueStringBuilder.ToString();
    }
  }
}
