// Decompiled with JetBrains decompiler
// Type: System.Version
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;


#nullable enable
namespace System
{
  /// <summary>Represents the version number of an assembly, operating system, or the common language runtime. This class cannot be inherited.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public sealed class Version : 
    ICloneable,
    IComparable,
    IComparable<Version?>,
    IEquatable<Version?>,
    ISpanFormattable,
    IFormattable
  {
    private readonly int _Major;
    private readonly int _Minor;
    private readonly int _Build;
    private readonly int _Revision;

    /// <summary>Initializes a new instance of the <see cref="T:System.Version" /> class with the specified major, minor, build, and revision numbers.</summary>
    /// <param name="major">The major version number.</param>
    /// <param name="minor">The minor version number.</param>
    /// <param name="build">The build number.</param>
    /// <param name="revision">The revision number.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="major" />, <paramref name="minor" />, <paramref name="build" />, or <paramref name="revision" /> is less than zero.</exception>
    public Version(int major, int minor, int build, int revision)
    {
      if (major < 0)
        throw new ArgumentOutOfRangeException(nameof (major), SR.ArgumentOutOfRange_Version);
      if (minor < 0)
        throw new ArgumentOutOfRangeException(nameof (minor), SR.ArgumentOutOfRange_Version);
      if (build < 0)
        throw new ArgumentOutOfRangeException(nameof (build), SR.ArgumentOutOfRange_Version);
      if (revision < 0)
        throw new ArgumentOutOfRangeException(nameof (revision), SR.ArgumentOutOfRange_Version);
      this._Major = major;
      this._Minor = minor;
      this._Build = build;
      this._Revision = revision;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Version" /> class using the specified major, minor, and build values.</summary>
    /// <param name="major">The major version number.</param>
    /// <param name="minor">The minor version number.</param>
    /// <param name="build">The build number.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="major" />, <paramref name="minor" />, or <paramref name="build" /> is less than zero.</exception>
    public Version(int major, int minor, int build)
    {
      if (major < 0)
        throw new ArgumentOutOfRangeException(nameof (major), SR.ArgumentOutOfRange_Version);
      if (minor < 0)
        throw new ArgumentOutOfRangeException(nameof (minor), SR.ArgumentOutOfRange_Version);
      if (build < 0)
        throw new ArgumentOutOfRangeException(nameof (build), SR.ArgumentOutOfRange_Version);
      this._Major = major;
      this._Minor = minor;
      this._Build = build;
      this._Revision = -1;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Version" /> class using the specified major and minor values.</summary>
    /// <param name="major">The major version number.</param>
    /// <param name="minor">The minor version number.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="major" /> or <paramref name="minor" /> is less than zero.</exception>
    public Version(int major, int minor)
    {
      if (major < 0)
        throw new ArgumentOutOfRangeException(nameof (major), SR.ArgumentOutOfRange_Version);
      if (minor < 0)
        throw new ArgumentOutOfRangeException(nameof (minor), SR.ArgumentOutOfRange_Version);
      this._Major = major;
      this._Minor = minor;
      this._Build = -1;
      this._Revision = -1;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Version" /> class using the specified string.</summary>
    /// <param name="version">A string containing the major, minor, build, and revision numbers, where each number is delimited with a period character ('.').</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="version" /> has fewer than two components or more than four components.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="version" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">A major, minor, build, or revision component is less than zero.</exception>
    /// <exception cref="T:System.FormatException">At least one component of <paramref name="version" /> does not parse to an integer.</exception>
    /// <exception cref="T:System.OverflowException">At least one component of <paramref name="version" /> represents a number greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    public Version(string version)
    {
      Version version1 = Version.Parse(version);
      this._Major = version1.Major;
      this._Minor = version1.Minor;
      this._Build = version1.Build;
      this._Revision = version1.Revision;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Version" /> class.</summary>
    public Version()
    {
      this._Build = -1;
      this._Revision = -1;
    }


    #nullable disable
    private Version(Version version)
    {
      this._Major = version._Major;
      this._Minor = version._Minor;
      this._Build = version._Build;
      this._Revision = version._Revision;
    }


    #nullable enable
    /// <summary>Returns a new <see cref="T:System.Version" /> object whose value is the same as the current <see cref="T:System.Version" /> object.</summary>
    /// <returns>A new <see cref="T:System.Object" /> whose values are a copy of the current <see cref="T:System.Version" /> object.</returns>
    public object Clone() => (object) new Version(this);

    /// <summary>Gets the value of the major component of the version number for the current <see cref="T:System.Version" /> object.</summary>
    /// <returns>The major version number.</returns>
    public int Major => this._Major;

    /// <summary>Gets the value of the minor component of the version number for the current <see cref="T:System.Version" /> object.</summary>
    /// <returns>The minor version number.</returns>
    public int Minor => this._Minor;

    /// <summary>Gets the value of the build component of the version number for the current <see cref="T:System.Version" /> object.</summary>
    /// <returns>The build number, or -1 if the build number is undefined.</returns>
    public int Build => this._Build;

    /// <summary>Gets the value of the revision component of the version number for the current <see cref="T:System.Version" /> object.</summary>
    /// <returns>The revision number, or -1 if the revision number is undefined.</returns>
    public int Revision => this._Revision;

    /// <summary>Gets the high 16 bits of the revision number.</summary>
    /// <returns>A 16-bit signed integer.</returns>
    public short MajorRevision => (short) (this._Revision >> 16);

    /// <summary>Gets the low 16 bits of the revision number.</summary>
    /// <returns>A 16-bit signed integer.</returns>
    public short MinorRevision => (short) (this._Revision & (int) ushort.MaxValue);

    /// <summary>Compares the current <see cref="T:System.Version" /> object to a specified object and returns an indication of their relative values.</summary>
    /// <param name="version">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="version" /> is not of type <see cref="T:System.Version" />.</exception>
    /// <returns>A signed integer that indicates the relative values of the two objects, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> The current <see cref="T:System.Version" /> object is a version before <paramref name="version" />.</description></item><item><term> Zero</term><description> The current <see cref="T:System.Version" /> object is the same version as <paramref name="version" />.</description></item><item><term> Greater than zero</term><description> The current <see cref="T:System.Version" /> object is a version subsequent to <paramref name="version" />, or <paramref name="version" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? version)
    {
      if (version == null)
        return 1;
      return this.CompareTo(version as Version ?? throw new ArgumentException(SR.Arg_MustBeVersion));
    }

    /// <summary>Compares the current <see cref="T:System.Version" /> object to a specified <see cref="T:System.Version" /> object and returns an indication of their relative values.</summary>
    /// <param name="value">A <see cref="T:System.Version" /> object to compare to the current <see cref="T:System.Version" /> object, or <see langword="null" />.</param>
    /// <returns>A signed integer that indicates the relative values of the two objects, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> The current <see cref="T:System.Version" /> object is a version before <paramref name="value" />.</description></item><item><term> Zero</term><description> The current <see cref="T:System.Version" /> object is the same version as <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> The current <see cref="T:System.Version" /> object is a version subsequent to <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(Version? value)
    {
      if ((object) value == (object) this)
        return 0;
      if ((object) value == null)
        return 1;
      if (this._Major == value._Major)
      {
        if (this._Minor == value._Minor)
        {
          if (this._Build == value._Build)
          {
            if (this._Revision == value._Revision)
              return 0;
            return this._Revision <= value._Revision ? -1 : 1;
          }
          return this._Build <= value._Build ? -1 : 1;
        }
        return this._Minor <= value._Minor ? -1 : 1;
      }
      return this._Major <= value._Major ? -1 : 1;
    }

    /// <summary>Returns a value indicating whether the current <see cref="T:System.Version" /> object is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with the current <see cref="T:System.Version" /> object, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if the current <see cref="T:System.Version" /> object and <paramref name="obj" /> are both <see cref="T:System.Version" /> objects, and every component of the current <see cref="T:System.Version" /> object matches the corresponding component of <paramref name="obj" />; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => this.Equals(obj as Version);

    /// <summary>Returns a value indicating whether the current <see cref="T:System.Version" /> object and a specified <see cref="T:System.Version" /> object represent the same value.</summary>
    /// <param name="obj">A <see cref="T:System.Version" /> object to compare to the current <see cref="T:System.Version" /> object, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if every component of the current <see cref="T:System.Version" /> object matches the corresponding component of the <paramref name="obj" /> parameter; otherwise, <see langword="false" />.</returns>
    public bool Equals([NotNullWhen(true)] Version? obj)
    {
      if ((object) obj == (object) this)
        return true;
      return (object) obj != null && this._Major == obj._Major && this._Minor == obj._Minor && this._Build == obj._Build && this._Revision == obj._Revision;
    }

    /// <summary>Returns a hash code for the current <see cref="T:System.Version" /> object.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => 0 | (this._Major & 15) << 28 | (this._Minor & (int) byte.MaxValue) << 20 | (this._Build & (int) byte.MaxValue) << 12 | this._Revision & 4095;

    /// <summary>Converts the value of the current <see cref="T:System.Version" /> object to its equivalent <see cref="T:System.String" /> representation.</summary>
    /// <returns>The <see cref="T:System.String" /> representation of the values of the major, minor, build, and revision components of the current <see cref="T:System.Version" /> object, as depicted in the following format. Each component is separated by a period character ('.'). Square brackets ('[' and ']') indicate a component that will not appear in the return value if the component is not defined:
    /// 
    /// major.minor[.build[.revision]]
    /// 
    /// For example, if you create a <see cref="T:System.Version" /> object using the constructor <c>Version(1,1)</c>, the returned string is "1.1". If you create a <see cref="T:System.Version" /> object using the constructor <c>Version(1,3,4,2)</c>, the returned string is "1.3.4.2".</returns>
    public override string ToString() => this.ToString(this.DefaultFormatFieldCount);

    /// <summary>Converts the value of the current <see cref="T:System.Version" /> object to its equivalent <see cref="T:System.String" /> representation. A specified count indicates the number of components to return.</summary>
    /// <param name="fieldCount">The number of components to return. The <paramref name="fieldCount" /> ranges from 0 to 4.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fieldCount" /> is less than 0, or more than 4.
    /// 
    /// -or-
    /// 
    /// <paramref name="fieldCount" /> is more than the number of components defined in the current <see cref="T:System.Version" /> object.</exception>
    /// <returns>The <see cref="T:System.String" /> representation of the values of the major, minor, build, and revision components of the current <see cref="T:System.Version" /> object, each separated by a period character ('.'). The <paramref name="fieldCount" /> parameter determines how many components are returned.
    /// 
    /// <list type="table"><listheader><term> fieldCount</term><description> Return Value</description></listheader><item><term> 0</term><description> An empty string ("").</description></item><item><term> 1</term><description> major</description></item><item><term> 2</term><description> major.minor</description></item><item><term> 3</term><description> major.minor.build</description></item><item><term> 4</term><description> major.minor.build.revision</description></item></list>
    /// 
    /// For example, if you create <see cref="T:System.Version" /> object using the constructor <c>Version(1,3,5)</c>, <c>ToString(2)</c> returns "1.3" and <c>ToString(4)</c> throws an exception.</returns>
    public unsafe string ToString(int fieldCount)
    {
      // ISSUE: untyped stack allocation
      Span<char> destination = new Span<char>((void*) __untypedstackalloc(new IntPtr(94)), 47);
      int charsWritten;
      this.TryFormat(destination, fieldCount, out charsWritten);
      return destination.Slice(0, charsWritten).ToString();
    }


    #nullable disable
    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <param name="format">The format to use.
    /// -or-
    /// A <see langword="null" /> reference (<see langword="Nothing" /> in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
    /// <param name="formatProvider">The provider to use to format the value.
    /// -or-
    /// A <see langword="null" /> reference (<see langword="Nothing" /> in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
    /// <returns>The value of the current instance in the specified format.</returns>
    string IFormattable.ToString(string format, IFormatProvider formatProvider) => this.ToString();


    #nullable enable
    /// <summary>Tries to format this version instance into a span of characters.</summary>
    /// <param name="destination">When this method returns, the formatted version in the span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(Span<char> destination, out int charsWritten) => this.TryFormat(destination, this.DefaultFormatFieldCount, out charsWritten);

    /// <summary>Tries to format this version instance into a span of characters.</summary>
    /// <param name="destination">When this method returns, the formatted version in the span of characters.</param>
    /// <param name="fieldCount">The number of components to return. This value ranges from 0 to 4.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(Span<char> destination, int fieldCount, out int charsWritten)
    {
      uint num1 = (uint) fieldCount;
      if (num1 <= 4U)
      {
        if (num1 >= 3U)
        {
          if (this._Build != -1)
          {
            if (num1 == 4U && this._Revision == -1)
              ThrowArgumentException("3");
          }
          else
            ThrowArgumentException("2");
        }
      }
      else
        ThrowArgumentException("4");
      int num2 = 0;
      for (int index = 0; index < fieldCount; ++index)
      {
        if (index != 0)
        {
          if (destination.IsEmpty)
          {
            charsWritten = 0;
            return false;
          }
          destination[0] = '.';
          destination = destination.Slice(1);
          ++num2;
        }
        int num3;
        switch (index)
        {
          case 0:
            num3 = this._Major;
            break;
          case 1:
            num3 = this._Minor;
            break;
          case 2:
            num3 = this._Build;
            break;
          default:
            num3 = this._Revision;
            break;
        }
        int charsWritten1;
        if (!((uint) num3).TryFormat(destination, out charsWritten1))
        {
          charsWritten = 0;
          return false;
        }
        num2 += charsWritten1;
        destination = destination.Slice(charsWritten1);
      }
      charsWritten = num2;
      return true;


      #nullable disable
      static void ThrowArgumentException(string failureUpperBound) => throw new ArgumentException(SR.Format(SR.ArgumentOutOfRange_Bounds_Lower_Upper, (object) "0", (object) failureUpperBound), "fieldCount");
    }

    /// <summary>Tries to format the value of the current instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    bool ISpanFormattable.TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format,
      IFormatProvider provider)
    {
      return this.TryFormat(destination, this.DefaultFormatFieldCount, out charsWritten);
    }

    private int DefaultFormatFieldCount
    {
      get
      {
        if (this._Build == -1)
          return 2;
        return this._Revision != -1 ? 4 : 3;
      }
    }


    #nullable enable
    /// <summary>Converts the string representation of a version number to an equivalent <see cref="T:System.Version" /> object.</summary>
    /// <param name="input">A string that contains a version number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="input" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="input" /> has fewer than two or more than four version components.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">At least one component in <paramref name="input" /> is less than zero.</exception>
    /// <exception cref="T:System.FormatException">At least one component in <paramref name="input" /> is not an integer.</exception>
    /// <exception cref="T:System.OverflowException">At least one component in <paramref name="input" /> represents a number that is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>An object that is equivalent to the version number specified in the <paramref name="input" /> parameter.</returns>
    public static Version Parse(string input) => input != null ? Version.ParseVersion(input.AsSpan(), true) : throw new ArgumentNullException(nameof (input));

    /// <summary>Converts the specified read-only span of characters that represents a version number to an equivalent <see cref="T:System.Version" /> object.</summary>
    /// <param name="input">A read-only span of characters that contains a version number to convert.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="input" /> has fewer than two or more than four version components.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">At least one component in <paramref name="input" /> is less than zero.</exception>
    /// <exception cref="T:System.FormatException">At least one component in <paramref name="input" /> is not an integer.</exception>
    /// <exception cref="T:System.OverflowException">At least one component in <paramref name="input" /> represents a number that is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>An object that is equivalent to the version number specified in the <paramref name="input" /> parameter.</returns>
    public static Version Parse(ReadOnlySpan<char> input) => Version.ParseVersion(input, true);

    /// <summary>Tries to convert the string representation of a version number to an equivalent <see cref="T:System.Version" /> object, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="input">A string that contains a version number to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.Version" /> equivalent of the number that is contained in <paramref name="input" />, if the conversion succeeded. If <paramref name="input" /> is <see langword="null" />, <see cref="F:System.String.Empty" />, or if the conversion fails, <paramref name="result" /> is <see langword="null" /> when the method returns.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="input" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out Version? result)
    {
      if (input != null)
        return (result = Version.ParseVersion(input.AsSpan(), false)) != (Version) null;
      result = (Version) null;
      return false;
    }

    /// <summary>Tries to convert the specified read-only span of characters representing a version number to an equivalent <see cref="T:System.Version" /> object, and returns a value that indicates whether the conversion succeeded.</summary>
    /// <param name="input">A read-only span of characters that contains a version number to convert.</param>
    /// <param name="result">When this method returns, the <see cref="T:System.Version" /> equivalent of the number that is contained in <paramref name="input" />, if the conversion succeeded. If <paramref name="input" /> is <see langword="null" />, <see cref="F:System.String.Empty" />, or if the conversion fails, <paramref name="result" /> is <see langword="null" /> when the method returns.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="input" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> input, [NotNullWhen(true)] out Version? result) => (result = Version.ParseVersion(input, false)) != (Version) null;


    #nullable disable
    private static Version ParseVersion(ReadOnlySpan<char> input, bool throwOnFailure)
    {
      int length = input.IndexOf<char>('.');
      if (length < 0)
      {
        if (throwOnFailure)
          throw new ArgumentException(SR.Arg_VersionString, nameof (input));
        return (Version) null;
      }
      int num1 = -1;
      int num2 = input.Slice(length + 1).IndexOf<char>('.');
      if (num2 != -1)
      {
        num2 += length + 1;
        num1 = input.Slice(num2 + 1).IndexOf<char>('.');
        if (num1 != -1)
        {
          num1 += num2 + 1;
          if (input.Slice(num1 + 1).Contains<char>('.'))
          {
            if (throwOnFailure)
              throw new ArgumentException(SR.Arg_VersionString, nameof (input));
            return (Version) null;
          }
        }
      }
      int parsedComponent1;
      if (!Version.TryParseComponent(input.Slice(0, length), nameof (input), throwOnFailure, out parsedComponent1))
        return (Version) null;
      if (num2 != -1)
      {
        int parsedComponent2;
        if (!Version.TryParseComponent(input.Slice(length + 1, num2 - length - 1), nameof (input), throwOnFailure, out parsedComponent2))
          return (Version) null;
        int parsedComponent3;
        int parsedComponent4;
        int parsedComponent5;
        return num1 != -1 ? (!Version.TryParseComponent(input.Slice(num2 + 1, num1 - num2 - 1), "build", throwOnFailure, out parsedComponent3) || !Version.TryParseComponent(input.Slice(num1 + 1), "revision", throwOnFailure, out parsedComponent4) ? (Version) null : new Version(parsedComponent1, parsedComponent2, parsedComponent3, parsedComponent4)) : (!Version.TryParseComponent(input.Slice(num2 + 1), "build", throwOnFailure, out parsedComponent5) ? (Version) null : new Version(parsedComponent1, parsedComponent2, parsedComponent5));
      }
      int parsedComponent6;
      return !Version.TryParseComponent(input.Slice(length + 1), nameof (input), throwOnFailure, out parsedComponent6) ? (Version) null : new Version(parsedComponent1, parsedComponent6);
    }

    private static bool TryParseComponent(
      ReadOnlySpan<char> component,
      string componentName,
      bool throwOnFailure,
      out int parsedComponent)
    {
      if (throwOnFailure)
      {
        if ((parsedComponent = int.Parse(component, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture)) < 0)
          throw new ArgumentOutOfRangeException(componentName, SR.ArgumentOutOfRange_Version);
        return true;
      }
      return int.TryParse(component, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out parsedComponent) && parsedComponent >= 0;
    }


    #nullable enable
    /// <summary>Determines whether two specified <see cref="T:System.Version" /> objects are equal.</summary>
    /// <param name="v1">The first <see cref="T:System.Version" /> object.</param>
    /// <param name="v2">The second <see cref="T:System.Version" /> object.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="v1" /> equals <paramref name="v2" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Version? v1, Version? v2) => (object) v2 == null ? (object) v1 == null : (object) v2 == (object) v1 || v2.Equals(v1);

    /// <summary>Determines whether two specified <see cref="T:System.Version" /> objects are not equal.</summary>
    /// <param name="v1">The first <see cref="T:System.Version" /> object.</param>
    /// <param name="v2">The second <see cref="T:System.Version" /> object.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="v1" /> does not equal <paramref name="v2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Version? v1, Version? v2) => !(v1 == v2);

    /// <summary>Determines whether the first specified <see cref="T:System.Version" /> object is less than the second specified <see cref="T:System.Version" /> object.</summary>
    /// <param name="v1">The first <see cref="T:System.Version" /> object.</param>
    /// <param name="v2">The second <see cref="T:System.Version" /> object.</param>
    /// <exception cref="T:System.ArgumentNullException">.NET Framework only: <paramref name="v1" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="v1" /> is less than <paramref name="v2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <(Version? v1, Version? v2) => (object) v1 == null ? v2 != null : v1.CompareTo(v2) < 0;

    /// <summary>Determines whether the first specified <see cref="T:System.Version" /> object is less than or equal to the second <see cref="T:System.Version" /> object.</summary>
    /// <param name="v1">The first <see cref="T:System.Version" /> object.</param>
    /// <param name="v2">The second <see cref="T:System.Version" /> object.</param>
    /// <exception cref="T:System.ArgumentNullException">.NET Framework only: <paramref name="v1" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="v1" /> is less than or equal to <paramref name="v2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <=(Version? v1, Version? v2) => (object) v1 == null || v1.CompareTo(v2) <= 0;

    /// <summary>Determines whether the first specified <see cref="T:System.Version" /> object is greater than the second specified <see cref="T:System.Version" /> object.</summary>
    /// <param name="v1">The first <see cref="T:System.Version" /> object.</param>
    /// <param name="v2">The second <see cref="T:System.Version" /> object.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="v1" /> is greater than <paramref name="v2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >(Version? v1, Version? v2) => v2 < v1;

    /// <summary>Determines whether the first specified <see cref="T:System.Version" /> object is greater than or equal to the second specified <see cref="T:System.Version" /> object.</summary>
    /// <param name="v1">The first <see cref="T:System.Version" /> object.</param>
    /// <param name="v2">The second <see cref="T:System.Version" /> object.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="v1" /> is greater than or equal to <paramref name="v2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >=(Version? v1, Version? v2) => v2 <= v1;
  }
}
