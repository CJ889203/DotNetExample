// Decompiled with JetBrains decompiler
// Type: System.Boolean
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>Represents a Boolean (<see langword="true" /> or <see langword="false" />) value.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Boolean : IComparable, IConvertible, IComparable<bool>, IEquatable<bool>
  {
    private readonly bool m_value;
    /// <summary>Represents the Boolean value <see langword="true" /> as a string. This field is read-only.</summary>
    public static readonly string TrueString = "True";
    /// <summary>Represents the Boolean value <see langword="false" /> as a string. This field is read-only.</summary>
    public static readonly string FalseString = "False";

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A hash code for the current <see cref="T:System.Boolean" />.</returns>
    public override int GetHashCode() => !this ? 0 : 1;

    /// <summary>Converts the value of this instance to its equivalent string representation (either "True" or "False").</summary>
    /// <returns>"True" (the value of the <see cref="F:System.Boolean.TrueString" /> property) if the value of this instance is <see langword="true" />, or "False" (the value of the <see cref="F:System.Boolean.FalseString" /> property) if the value of this instance is <see langword="false" />.</returns>
    public override string ToString() => !this ? "False" : "True";

    /// <summary>Converts the value of this instance to its equivalent string representation (either "True" or "False").</summary>
    /// <param name="provider">(Reserved) An <see cref="T:System.IFormatProvider" /> object.</param>
    /// <returns>
    /// <see cref="F:System.Boolean.TrueString" /> if the value of this instance is <see langword="true" />, or <see cref="F:System.Boolean.FalseString" /> if the value of this instance is <see langword="false" />.</returns>
    public string ToString(IFormatProvider? provider) => this.ToString();

    /// <summary>Tries to format the value of the current boolean instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
      if (this)
      {
        switch (destination.Length)
        {
          case 0:
          case 1:
          case 2:
          case 3:
            break;
          default:
            destination[0] = 'T';
            destination[1] = 'r';
            destination[2] = 'u';
            destination[3] = 'e';
            charsWritten = 4;
            return true;
        }
      }
      else
      {
        switch (destination.Length)
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
            break;
          default:
            destination[0] = 'F';
            destination[1] = 'a';
            destination[2] = 'l';
            destination[3] = 's';
            destination[4] = 'e';
            charsWritten = 5;
            return true;
        }
      }
      charsWritten = 0;
      return false;
    }

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Boolean" /> and has the same value as this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is bool flag && this == flag;

    /// <summary>Returns a value indicating whether this instance is equal to a specified <see cref="T:System.Boolean" /> object.</summary>
    /// <param name="obj">A <see cref="T:System.Boolean" /> value to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> has the same value as this instance; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public bool Equals(bool obj) => this == obj;

    /// <summary>Compares this instance to a specified object and returns an integer that indicates their relationship to one another.</summary>
    /// <param name="obj">An object to compare to this instance, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="obj" /> is not a <see cref="T:System.Boolean" />.</exception>
    /// <returns>A signed integer that indicates the relative order of this instance and <paramref name="obj" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Condition</description></listheader><item><term> Less than zero</term><description> This instance is <see langword="false" /> and <paramref name="obj" /> is <see langword="true" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="obj" /> are equal (either both are <see langword="true" /> or both are <see langword="false" />).</description></item><item><term> Greater than zero</term><description> This instance is <see langword="true" /> and <paramref name="obj" /> is <see langword="false" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="obj" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? obj)
    {
      if (obj == null)
        return 1;
      if (!(obj is bool flag))
        throw new ArgumentException(SR.Arg_MustBeBoolean);
      if (this == flag)
        return 0;
      return !this ? -1 : 1;
    }

    /// <summary>Compares this instance to a specified <see cref="T:System.Boolean" /> object and returns an integer that indicates their relationship to one another.</summary>
    /// <param name="value">A <see cref="T:System.Boolean" /> object to compare to this instance.</param>
    /// <returns>A signed integer that indicates the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Condition</description></listheader><item><term> Less than zero</term><description> This instance is <see langword="false" /> and <paramref name="value" /> is <see langword="true" />.</description></item><item><term> Zero</term><description> This instance and <paramref name="value" /> are equal (either both are <see langword="true" /> or both are <see langword="false" />).</description></item><item><term> Greater than zero</term><description> This instance is <see langword="true" /> and <paramref name="value" /> is <see langword="false" />.</description></item></list></returns>
    public int CompareTo(bool value)
    {
      if (this == value)
        return 0;
      return !this ? -1 : 1;
    }


    #nullable disable
    internal static bool IsTrueStringIgnoreCase(ReadOnlySpan<char> value)
    {
      if (value.Length != 4 || value[0] != 't' && value[0] != 'T' || value[1] != 'r' && value[1] != 'R' || value[2] != 'u' && value[2] != 'U')
        return false;
      return value[3] == 'e' || value[3] == 'E';
    }

    internal static bool IsFalseStringIgnoreCase(ReadOnlySpan<char> value)
    {
      if (value.Length != 5 || value[0] != 'f' && value[0] != 'F' || value[1] != 'a' && value[1] != 'A' || value[2] != 'l' && value[2] != 'L' || value[3] != 's' && value[3] != 'S')
        return false;
      return value[4] == 'e' || value[4] == 'E';
    }


    #nullable enable
    /// <summary>Converts the specified string representation of a logical value to its <see cref="T:System.Boolean" /> equivalent.</summary>
    /// <param name="value">A string containing the value to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not equivalent to <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is equivalent to <see cref="F:System.Boolean.TrueString" />; <see langword="false" /> if <paramref name="value" /> is equivalent to <see cref="F:System.Boolean.FalseString" />.</returns>
    public static bool Parse(string value) => value != null ? bool.Parse(value.AsSpan()) : throw new ArgumentNullException(nameof (value));

    /// <summary>Converts the specified span representation of a logical value to its <see cref="T:System.Boolean" /> equivalent.</summary>
    /// <param name="value">A span containing the characters representing the value to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is equivalent to <see cref="F:System.Boolean.TrueString" />; <see langword="false" /> if <paramref name="value" /> is equivalent to <see cref="F:System.Boolean.FalseString" />.</returns>
    public static bool Parse(ReadOnlySpan<char> value)
    {
      bool result;
      if (!bool.TryParse(value, out result))
        throw new FormatException(SR.Format(SR.Format_BadBoolean, (object) new string(value)));
      return result;
    }

    /// <summary>Tries to convert the specified string representation of a logical value to its <see cref="T:System.Boolean" /> equivalent.</summary>
    /// <param name="value">A string containing the value to convert.</param>
    /// <param name="result">When this method returns, if the conversion succeeded, contains <see langword="true" /> if <paramref name="value" /> is equal to <see cref="F:System.Boolean.TrueString" /> or <see langword="false" /> if <paramref name="value" /> is equal to <see cref="F:System.Boolean.FalseString" />. If the conversion failed, contains <see langword="false" />. The conversion fails if <paramref name="value" /> is <see langword="null" /> or is not equal to the value of either the <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" /> field.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? value, out bool result)
    {
      if (value != null)
        return bool.TryParse(value.AsSpan(), out result);
      result = false;
      return false;
    }

    /// <summary>Tries to convert the specified span representation of a logical value to its <see cref="T:System.Boolean" /> equivalent.</summary>
    /// <param name="value">A span containing the characters representing the value to convert.</param>
    /// <param name="result">When this method returns, if the conversion succeeded, contains <see langword="true" /> if <paramref name="value" /> is equal to <see cref="F:System.Boolean.TrueString" /> or <see langword="false" /> if <paramref name="value" /> is equal to <see cref="F:System.Boolean.FalseString" />. If the conversion failed, contains <see langword="false" />. The conversion fails if <paramref name="value" /> is <see langword="null" /> or is not equal to the value of either the <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" /> field.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> value, out bool result)
    {
      if (bool.IsTrueStringIgnoreCase(value))
      {
        result = true;
        return true;
      }
      if (bool.IsFalseStringIgnoreCase(value))
      {
        result = false;
        return true;
      }
      value = bool.TrimWhiteSpaceAndNull(value);
      if (bool.IsTrueStringIgnoreCase(value))
      {
        result = true;
        return true;
      }
      if (bool.IsFalseStringIgnoreCase(value))
      {
        result = false;
        return true;
      }
      result = false;
      return false;
    }


    #nullable disable
    private static ReadOnlySpan<char> TrimWhiteSpaceAndNull(ReadOnlySpan<char> value)
    {
      int num = 0;
      while (num < value.Length && (char.IsWhiteSpace(value[num]) || value[num] == char.MinValue))
        ++num;
      int index = value.Length - 1;
      while (index >= num && (char.IsWhiteSpace(value[index]) || value[index] == char.MinValue))
        --index;
      return value.Slice(num, index - num + 1);
    }

    /// <summary>Returns the type code for the <see cref="T:System.Boolean" /> value type.</summary>
    /// <returns>The enumerated constant <see cref="F:System.TypeCode.Boolean" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Boolean;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>
    /// <see langword="true" /> or <see langword="false" />.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => this;

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">You attempt to convert a <see cref="T:System.Boolean" /> value to a <see cref="T:System.Char" /> value. This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Boolean), (object) "Char"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if the value of this instance is <see langword="true" />; otherwise, 0.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>1 if this instance is <see langword="true" />; otherwise, 0.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(this);

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">You attempt to convert a <see cref="T:System.Boolean" /> value to a <see cref="T:System.DateTime" /> value. This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Boolean), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The desired type.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies culture-specific information about the format of the returned value.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidCastException">The requested type conversion is not supported.</exception>
    /// <returns>An object of the specified type, with a value that is equivalent to the value of this <see langword="Boolean" /> object.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);
  }
}
