﻿// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.RegexCompilationInfo
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>Provides information about a regular expression that is used to compile a regular expression to a stand-alone assembly.</summary>
  public class RegexCompilationInfo
  {

    #nullable disable
    private string _pattern;
    private string _name;
    private string _nspace;
    private TimeSpan _matchTimeout;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.RegularExpressions.RegexCompilationInfo" /> class that contains information about a regular expression to be included in an assembly.</summary>
    /// <param name="pattern">The regular expression to compile.</param>
    /// <param name="options">The regular expression options to use when compiling the regular expression.</param>
    /// <param name="name">The name of the type that represents the compiled regular expression.</param>
    /// <param name="fullnamespace">The namespace to which the new type belongs.</param>
    /// <param name="ispublic">
    /// <see langword="true" /> to make the compiled regular expression publicly visible; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="name" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="pattern" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="fullnamespace" /> is <see langword="null" />.</exception>
    public RegexCompilationInfo(
      string pattern,
      RegexOptions options,
      string name,
      string fullnamespace,
      bool ispublic)
      : this(pattern, options, name, fullnamespace, ispublic, Regex.s_defaultMatchTimeout)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.RegularExpressions.RegexCompilationInfo" /> class that contains information about a regular expression with a specified time-out value to be included in an assembly.</summary>
    /// <param name="pattern">The regular expression to compile.</param>
    /// <param name="options">The regular expression options to use when compiling the regular expression.</param>
    /// <param name="name">The name of the type that represents the compiled regular expression.</param>
    /// <param name="fullnamespace">The namespace to which the new type belongs.</param>
    /// <param name="ispublic">
    /// <see langword="true" /> to make the compiled regular expression publicly visible; otherwise, <see langword="false" />.</param>
    /// <param name="matchTimeout">The default time-out interval for the regular expression.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="name" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///        <paramref name="pattern" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="fullnamespace" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="matchTimeout" /> is negative, zero, or greater than approximately 24 days.</exception>
    public RegexCompilationInfo(
      string pattern,
      RegexOptions options,
      string name,
      string fullnamespace,
      bool ispublic,
      TimeSpan matchTimeout)
    {
      this.Pattern = pattern;
      this.Name = name;
      this.Namespace = fullnamespace;
      this.Options = options;
      this.IsPublic = ispublic;
      this.MatchTimeout = matchTimeout;
    }

    /// <summary>Gets or sets a value that indicates whether the compiled regular expression has public visibility.</summary>
    /// <returns>
    /// <see langword="true" /> if the regular expression has public visibility; otherwise, <see langword="false" />.</returns>
    public bool IsPublic { get; set; }

    /// <summary>Gets or sets the regular expression's default time-out interval.</summary>
    /// <returns>The default maximum time interval that can elapse in a pattern-matching operation before a <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException" /> is thrown, or <see cref="F:System.Text.RegularExpressions.Regex.InfiniteMatchTimeout" /> if time-outs are disabled.</returns>
    public TimeSpan MatchTimeout
    {
      get => this._matchTimeout;
      set
      {
        Regex.ValidateMatchTimeout(value);
        this._matchTimeout = value;
      }
    }

    /// <summary>Gets or sets the name of the type that represents the compiled regular expression.</summary>
    /// <exception cref="T:System.ArgumentNullException">The value for this property is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The value for this property is an empty string.</exception>
    /// <returns>The name of the new type.</returns>
    public string Name
    {
      get => this._name;
      [MemberNotNull("_name")] set
      {
        switch (value)
        {
          case "":
            throw new ArgumentException(SR.Format(SR.InvalidEmptyArgument, (object) nameof (Name)), nameof (Name));
          case null:
            throw new ArgumentNullException(nameof (Name));
          default:
            this._name = value;
            break;
        }
      }
    }

    /// <summary>Gets or sets the namespace to which the new type belongs.</summary>
    /// <exception cref="T:System.ArgumentNullException">The value for this property is <see langword="null" />.</exception>
    /// <returns>The namespace of the new type.</returns>
    public string Namespace
    {
      get => this._nspace;
      [MemberNotNull("_nspace")] set => this._nspace = value ?? throw new ArgumentNullException(nameof (Namespace));
    }

    /// <summary>Gets or sets the options to use when compiling the regular expression.</summary>
    /// <returns>A bitwise combination of the enumeration values.</returns>
    public RegexOptions Options { get; set; }

    /// <summary>Gets or sets the regular expression to compile.</summary>
    /// <exception cref="T:System.ArgumentNullException">The value for this property is <see langword="null" />.</exception>
    /// <returns>The regular expression to compile.</returns>
    public string Pattern
    {
      get => this._pattern;
      [MemberNotNull("_pattern")] set => this._pattern = value ?? throw new ArgumentNullException(nameof (Pattern));
    }
  }
}
