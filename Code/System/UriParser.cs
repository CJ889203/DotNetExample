// Decompiled with JetBrains decompiler
// Type: System.UriParser
// Assembly: System.Private.Uri, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: EFC59026-8404-447E-A976-365A5080B26A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Uri.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Collections;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Parses a new URI scheme. This is an abstract class.</summary>
  public abstract class UriParser
  {

    #nullable disable
    internal static readonly UriParser HttpUri = (UriParser) new UriParser.BuiltInUriParser("http", 80, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser HttpsUri = (UriParser) new UriParser.BuiltInUriParser("https", 443, UriParser.HttpUri._flags);
    internal static readonly UriParser WsUri = (UriParser) new UriParser.BuiltInUriParser("ws", 80, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser WssUri = (UriParser) new UriParser.BuiltInUriParser("wss", 443, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser FtpUri = (UriParser) new UriParser.BuiltInUriParser("ftp", 21, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser FileUri = (UriParser) new UriParser.BuiltInUriParser("file", -1, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowEmptyHost | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.FileLikeUri | UriSyntaxFlags.AllowDOSPath | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.UnEscapeDotsAndSlashes | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser UnixFileUri = (UriParser) new UriParser.BuiltInUriParser("file", -1, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowEmptyHost | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.FileLikeUri | UriSyntaxFlags.AllowDOSPath | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.UnEscapeDotsAndSlashes | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser GopherUri = (UriParser) new UriParser.BuiltInUriParser("gopher", 70, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser NntpUri = (UriParser) new UriParser.BuiltInUriParser("nntp", 119, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser NewsUri = (UriParser) new UriParser.BuiltInUriParser("news", -1, UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser MailToUri = (UriParser) new UriParser.BuiltInUriParser("mailto", 25, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowEmptyHost | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.MailToLikeUri | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser UuidUri = (UriParser) new UriParser.BuiltInUriParser("uuid", -1, UriParser.NewsUri._flags);
    internal static readonly UriParser TelnetUri = (UriParser) new UriParser.BuiltInUriParser("telnet", 23, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser LdapUri = (UriParser) new UriParser.BuiltInUriParser("ldap", 389, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowEmptyHost | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser NetTcpUri = (UriParser) new UriParser.BuiltInUriParser("net.tcp", 808, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.UnEscapeDotsAndSlashes | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser NetPipeUri = (UriParser) new UriParser.BuiltInUriParser("net.pipe", -1, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.UnEscapeDotsAndSlashes | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    internal static readonly UriParser VsMacrosUri = (UriParser) new UriParser.BuiltInUriParser("vsmacros", -1, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.MustHaveAuthority | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowEmptyHost | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.FileLikeUri | UriSyntaxFlags.AllowDOSPath | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.CanonicalizeAsFilePath | UriSyntaxFlags.UnEscapeDotsAndSlashes | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
    private static readonly Hashtable s_table = new Hashtable(16)
    {
      {
        (object) UriParser.HttpUri.SchemeName,
        (object) UriParser.HttpUri
      },
      {
        (object) UriParser.HttpsUri.SchemeName,
        (object) UriParser.HttpsUri
      },
      {
        (object) UriParser.WsUri.SchemeName,
        (object) UriParser.WsUri
      },
      {
        (object) UriParser.WssUri.SchemeName,
        (object) UriParser.WssUri
      },
      {
        (object) UriParser.FtpUri.SchemeName,
        (object) UriParser.FtpUri
      },
      {
        (object) UriParser.FileUri.SchemeName,
        (object) UriParser.FileUri
      },
      {
        (object) UriParser.GopherUri.SchemeName,
        (object) UriParser.GopherUri
      },
      {
        (object) UriParser.NntpUri.SchemeName,
        (object) UriParser.NntpUri
      },
      {
        (object) UriParser.NewsUri.SchemeName,
        (object) UriParser.NewsUri
      },
      {
        (object) UriParser.MailToUri.SchemeName,
        (object) UriParser.MailToUri
      },
      {
        (object) UriParser.UuidUri.SchemeName,
        (object) UriParser.UuidUri
      },
      {
        (object) UriParser.TelnetUri.SchemeName,
        (object) UriParser.TelnetUri
      },
      {
        (object) UriParser.LdapUri.SchemeName,
        (object) UriParser.LdapUri
      },
      {
        (object) UriParser.NetTcpUri.SchemeName,
        (object) UriParser.NetTcpUri
      },
      {
        (object) UriParser.NetPipeUri.SchemeName,
        (object) UriParser.NetPipeUri
      },
      {
        (object) UriParser.VsMacrosUri.SchemeName,
        (object) UriParser.VsMacrosUri
      }
    };
    private static Hashtable s_tempTable = new Hashtable(25);
    private UriSyntaxFlags _flags;
    private int _port;
    private string _scheme;


    #nullable enable
    internal string SchemeName => this._scheme;

    internal int DefaultPort => this._port;

    /// <summary>Constructs a default URI parser.</summary>
    protected UriParser()
      : this(UriSyntaxFlags.MayHavePath)
    {
    }

    /// <summary>Invoked by a <see cref="T:System.Uri" /> constructor to get a <see cref="T:System.UriParser" /> instance.</summary>
    /// <returns>A <see cref="T:System.UriParser" /> for the constructed <see cref="T:System.Uri" />.</returns>
    protected virtual UriParser OnNewUri() => this;

    /// <summary>Invoked by the Framework when a <see cref="T:System.UriParser" /> method is registered.</summary>
    /// <param name="schemeName">The scheme that is associated with this <see cref="T:System.UriParser" />.</param>
    /// <param name="defaultPort">The port number of the scheme.</param>
    protected virtual void OnRegister(string schemeName, int defaultPort)
    {
    }

    /// <summary>Initialize the state of the parser and validate the URI.</summary>
    /// <param name="uri">The <see cref="T:System.Uri" /> to validate.</param>
    /// <param name="parsingError">Validation errors, if any.</param>
    protected virtual void InitializeAndValidate(Uri uri, out UriFormatException? parsingError)
    {
      if (uri._syntax == null)
        throw new InvalidOperationException(SR.net_uri_NotAbsolute);
      if (uri._syntax != this)
        throw new InvalidOperationException(SR.Format(SR.net_uri_UserDrivenParsing, (object) uri._syntax.GetType()));
      parsingError = ((long) Interlocked.Or(ref Unsafe.As<Uri.Flags, ulong>(ref uri._flags), 4611686018427387904UL) & 4611686018427387904L) == 0L ? uri.ParseMinimal() : throw new InvalidOperationException(SR.net_uri_InitializeCalledAlreadyOrTooLate);
    }

    /// <summary>Called by <see cref="T:System.Uri" /> constructors and <see cref="Overload:System.Uri.TryCreate" /> to resolve a relative URI.</summary>
    /// <param name="baseUri">A base URI.</param>
    /// <param name="relativeUri">A relative URI.</param>
    /// <param name="parsingError">Errors during the resolve process, if any.</param>
    /// <exception cref="T:System.InvalidOperationException">
    ///         <paramref name="baseUri" /> parameter is not an absolute <see cref="T:System.Uri" />
    /// 
    /// -or-
    /// 
    ///  <paramref name="baseUri" /> parameter requires user-driven parsing.</exception>
    /// <returns>The string of the resolved relative <see cref="T:System.Uri" />.</returns>
    protected virtual string? Resolve(
      Uri baseUri,
      Uri? relativeUri,
      out UriFormatException? parsingError)
    {
      if (baseUri.UserDrivenParsing)
        throw new InvalidOperationException(SR.Format(SR.net_uri_UserDrivenParsing, (object) this.GetType()));
      if (!baseUri.IsAbsoluteUri)
        throw new InvalidOperationException(SR.net_uri_NotAbsolute);
      string newUriString = (string) null;
      bool userEscaped = false;
      parsingError = (UriFormatException) null;
      Uri uri = Uri.ResolveHelper(baseUri, relativeUri, ref newUriString, ref userEscaped);
      return uri != (Uri) null ? uri.OriginalString : newUriString;
    }

    /// <summary>Determines whether <paramref name="baseUri" /> is a base URI for <paramref name="relativeUri" />.</summary>
    /// <param name="baseUri">The base URI.</param>
    /// <param name="relativeUri">The URI to test.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="baseUri" /> is a base URI for <paramref name="relativeUri" />; otherwise, <see langword="false" />.</returns>
    protected virtual bool IsBaseOf(Uri baseUri, Uri relativeUri) => baseUri.IsBaseOfHelper(relativeUri);

    /// <summary>Gets the components from a URI.</summary>
    /// <param name="uri">The URI to parse.</param>
    /// <param name="components">The <see cref="T:System.UriComponents" /> to retrieve from <paramref name="uri" />.</param>
    /// <param name="format">One of the <see cref="T:System.UriFormat" /> values that controls how special characters are escaped.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="uriFormat" /> is invalid.
    /// 
    /// -or-
    /// 
    ///  <paramref name="uriComponents" /> is not a combination of valid <see cref="T:System.UriComponents" /> values.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    ///         <paramref name="uri" /> requires user-driven parsing
    /// 
    /// -or-
    /// 
    ///  <paramref name="uri" /> is not an absolute URI. Relative URIs cannot be used with this method.</exception>
    /// <returns>A string that contains the components.</returns>
    protected virtual string GetComponents(Uri uri, UriComponents components, UriFormat format)
    {
      if ((components & UriComponents.SerializationInfoString) != (UriComponents) 0 && components != UriComponents.SerializationInfoString)
        throw new ArgumentOutOfRangeException(nameof (components), (object) components, SR.net_uri_NotJustSerialization);
      if ((format & ~UriFormat.SafeUnescaped) != (UriFormat) 0)
        throw new ArgumentOutOfRangeException(nameof (format));
      if (uri.UserDrivenParsing)
        throw new InvalidOperationException(SR.Format(SR.net_uri_UserDrivenParsing, (object) this.GetType()));
      if (!uri.IsAbsoluteUri)
        throw new InvalidOperationException(SR.net_uri_NotAbsolute);
      if (uri.DisablePathAndQueryCanonicalization && (components & UriComponents.PathAndQuery) != (UriComponents) 0)
        throw new InvalidOperationException(SR.net_uri_GetComponentsCalledWhenCanonicalizationDisabled);
      return uri.GetComponentsHelper(components, format);
    }

    /// <summary>Indicates whether a URI is well-formed.</summary>
    /// <param name="uri">The URI to check.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="uri" /> is well-formed; otherwise, <see langword="false" />.</returns>
    protected virtual bool IsWellFormedOriginalString(Uri uri) => uri.InternalIsWellFormedOriginalString();

    /// <summary>Associates a scheme and port number with a <see cref="T:System.UriParser" />.</summary>
    /// <param name="uriParser">The URI parser to register.</param>
    /// <param name="schemeName">The name of the scheme that is associated with this parser.</param>
    /// <param name="defaultPort">The default port number for the specified scheme.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="uriParser" /> parameter is null
    /// 
    /// -or-
    /// 
    ///  <paramref name="schemeName" /> parameter is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="schemeName" /> parameter is not valid
    /// 
    /// -or-
    /// 
    ///  <paramref name="defaultPort" /> parameter is not valid. The <paramref name="defaultPort" /> parameter is less than -1 or greater than 65,534.</exception>
    public static void Register(UriParser uriParser, string schemeName, int defaultPort)
    {
      if (uriParser == null)
        throw new ArgumentNullException(nameof (uriParser));
      if (schemeName == null)
        throw new ArgumentNullException(nameof (schemeName));
      if (schemeName.Length == 1)
        throw new ArgumentOutOfRangeException(nameof (schemeName));
      if (!Uri.CheckSchemeName(schemeName))
        throw new ArgumentOutOfRangeException(nameof (schemeName));
      if ((defaultPort >= (int) ushort.MaxValue || defaultPort < 0) && defaultPort != -1)
        throw new ArgumentOutOfRangeException(nameof (defaultPort));
      schemeName = schemeName.ToLowerInvariant();
      UriParser.FetchSyntax(uriParser, schemeName, defaultPort);
    }

    /// <summary>Indicates whether the parser for a scheme is registered.</summary>
    /// <param name="schemeName">The scheme name to check.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="schemeName" /> parameter is null.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="schemeName" /> parameter is not valid.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="schemeName" /> has been registered; otherwise, <see langword="false" />.</returns>
    public static bool IsKnownScheme(string schemeName)
    {
      if (schemeName == null)
        throw new ArgumentNullException(nameof (schemeName));
      UriParser uriParser = Uri.CheckSchemeName(schemeName) ? UriParser.GetSyntax(schemeName.ToLowerInvariant()) : throw new ArgumentOutOfRangeException(nameof (schemeName));
      return uriParser != null && uriParser.NotAny(UriSyntaxFlags.V1_UnknownUri);
    }

    internal UriSyntaxFlags Flags => this._flags;

    internal bool NotAny(UriSyntaxFlags flags) => this.IsFullMatch(flags, UriSyntaxFlags.None);

    internal bool InFact(UriSyntaxFlags flags) => !this.IsFullMatch(flags, UriSyntaxFlags.None);

    internal bool IsAllSet(UriSyntaxFlags flags) => this.IsFullMatch(flags, flags);

    private bool IsFullMatch(UriSyntaxFlags flags, UriSyntaxFlags expected) => (this._flags & flags) == expected;

    internal UriParser(UriSyntaxFlags flags)
    {
      this._flags = flags;
      this._scheme = string.Empty;
    }


    #nullable disable
    private static void FetchSyntax(UriParser syntax, string lwrCaseSchemeName, int defaultPort)
    {
      if (syntax.SchemeName.Length != 0)
        throw new InvalidOperationException(SR.Format(SR.net_uri_NeedFreshParser, (object) syntax.SchemeName));
      lock (UriParser.s_table)
      {
        syntax._flags &= ~UriSyntaxFlags.V1_UnknownUri;
        UriParser uriParser1 = (UriParser) UriParser.s_table[(object) lwrCaseSchemeName];
        if (uriParser1 != null)
          throw new InvalidOperationException(SR.Format(SR.net_uri_AlreadyRegistered, (object) uriParser1.SchemeName));
        UriParser uriParser2 = (UriParser) UriParser.s_tempTable[(object) syntax.SchemeName];
        if (uriParser2 != null)
        {
          lwrCaseSchemeName = uriParser2._scheme;
          UriParser.s_tempTable.Remove((object) lwrCaseSchemeName);
        }
        syntax.OnRegister(lwrCaseSchemeName, defaultPort);
        syntax._scheme = lwrCaseSchemeName;
        syntax.CheckSetIsSimpleFlag();
        syntax._port = defaultPort;
        UriParser.s_table[(object) syntax.SchemeName] = (object) syntax;
      }
    }

    internal static UriParser FindOrFetchAsUnknownV1Syntax(string lwrCaseScheme)
    {
      UriParser asUnknownV1Syntax1 = (UriParser) UriParser.s_table[(object) lwrCaseScheme];
      if (asUnknownV1Syntax1 != null)
        return asUnknownV1Syntax1;
      UriParser asUnknownV1Syntax2 = (UriParser) UriParser.s_tempTable[(object) lwrCaseScheme];
      if (asUnknownV1Syntax2 != null)
        return asUnknownV1Syntax2;
      lock (UriParser.s_table)
      {
        if (UriParser.s_tempTable.Count >= 512)
          UriParser.s_tempTable = new Hashtable(25);
        UriParser asUnknownV1Syntax3 = (UriParser) new UriParser.BuiltInUriParser(lwrCaseScheme, -1, UriSyntaxFlags.AllowAnInternetHost | UriSyntaxFlags.OptionalAuthority | UriSyntaxFlags.MayHaveUserInfo | UriSyntaxFlags.MayHavePort | UriSyntaxFlags.MayHavePath | UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment | UriSyntaxFlags.AllowEmptyHost | UriSyntaxFlags.AllowUncHost | UriSyntaxFlags.V1_UnknownUri | UriSyntaxFlags.AllowDOSPath | UriSyntaxFlags.PathIsRooted | UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath | UriSyntaxFlags.AllowIdn | UriSyntaxFlags.AllowIriParsing);
        UriParser.s_tempTable[(object) lwrCaseScheme] = (object) asUnknownV1Syntax3;
        return asUnknownV1Syntax3;
      }
    }

    internal static UriParser GetSyntax(string lwrCaseScheme) => (UriParser) (UriParser.s_table[(object) lwrCaseScheme] ?? UriParser.s_tempTable[(object) lwrCaseScheme]);

    internal bool IsSimple => this.InFact(UriSyntaxFlags.SimpleUserSyntax);

    internal void CheckSetIsSimpleFlag()
    {
      Type type = this.GetType();
      if (!(type == typeof (GenericUriParser)) && !(type == typeof (HttpStyleUriParser)) && !(type == typeof (FtpStyleUriParser)) && !(type == typeof (FileStyleUriParser)) && !(type == typeof (NewsStyleUriParser)) && !(type == typeof (GopherStyleUriParser)) && !(type == typeof (NetPipeStyleUriParser)) && !(type == typeof (NetTcpStyleUriParser)) && !(type == typeof (LdapStyleUriParser)))
        return;
      this._flags |= UriSyntaxFlags.SimpleUserSyntax;
    }

    internal UriParser InternalOnNewUri()
    {
      UriParser uriParser = this.OnNewUri();
      if (this != uriParser)
      {
        uriParser._scheme = this._scheme;
        uriParser._port = this._port;
        uriParser._flags = this._flags;
      }
      return uriParser;
    }

    internal void InternalValidate(Uri thisUri, out UriFormatException parsingError)
    {
      this.InitializeAndValidate(thisUri, out parsingError);
      long num = (long) Interlocked.Or(ref Unsafe.As<Uri.Flags, ulong>(ref thisUri._flags), 4611686018427387904UL);
    }

    internal string InternalResolve(
      Uri thisBaseUri,
      Uri uriLink,
      out UriFormatException parsingError)
    {
      return this.Resolve(thisBaseUri, uriLink, out parsingError);
    }

    internal bool InternalIsBaseOf(Uri thisBaseUri, Uri uriLink) => this.IsBaseOf(thisBaseUri, uriLink);

    internal string InternalGetComponents(
      Uri thisUri,
      UriComponents uriComponents,
      UriFormat uriFormat)
    {
      return this.GetComponents(thisUri, uriComponents, uriFormat);
    }

    internal bool InternalIsWellFormedOriginalString(Uri thisUri) => this.IsWellFormedOriginalString(thisUri);

    private sealed class BuiltInUriParser : UriParser
    {
      internal BuiltInUriParser(string lwrCaseScheme, int defaultPort, UriSyntaxFlags syntaxFlags)
        : base(syntaxFlags | UriSyntaxFlags.SimpleUserSyntax | UriSyntaxFlags.BuiltInSyntax)
      {
        this._scheme = lwrCaseScheme;
        this._port = defaultPort;
      }
    }
  }
}
