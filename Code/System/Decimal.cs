// Decompiled with JetBrains decompiler
// Type: System.Decimal
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  /// <summary>Represents a decimal floating-point number.</summary>
  [NonVersionable]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public readonly struct Decimal : 
    ISpanFormattable,
    IFormattable,
    IComparable,
    IConvertible,
    IComparable<Decimal>,
    IEquatable<Decimal>,
    ISerializable,
    IDeserializationCallback,
    IMinMaxValue<Decimal>,
    ISignedNumber<Decimal>,
    INumber<Decimal>,
    IAdditionOperators<Decimal, Decimal, Decimal>,
    IAdditiveIdentity<Decimal, Decimal>,
    IComparisonOperators<Decimal, Decimal>,
    IEqualityOperators<Decimal, Decimal>,
    IDecrementOperators<Decimal>,
    IDivisionOperators<Decimal, Decimal, Decimal>,
    IIncrementOperators<Decimal>,
    IModulusOperators<Decimal, Decimal, Decimal>,
    IMultiplicativeIdentity<Decimal, Decimal>,
    IMultiplyOperators<Decimal, Decimal, Decimal>,
    ISpanParseable<Decimal>,
    IParseable<Decimal>,
    ISubtractionOperators<Decimal, Decimal, Decimal>,
    IUnaryNegationOperators<Decimal, Decimal>,
    IUnaryPlusOperators<Decimal, Decimal>
  {
    /// <summary>Represents the number zero (0).</summary>
    public const Decimal Zero = 0M;
    /// <summary>Represents the number one (1).</summary>
    public const Decimal One = 1M;
    /// <summary>Represents the number negative one (-1).</summary>
    public const Decimal MinusOne = -1M;
    /// <summary>Represents the largest possible value of <see cref="T:System.Decimal" />. This field is constant and read-only.</summary>
    public const Decimal MaxValue = Decimal.MaxValue;
    /// <summary>Represents the smallest possible value of <see cref="T:System.Decimal" />. This field is constant and read-only.</summary>
    public const Decimal MinValue = Decimal.MinValue;
    private readonly int _flags;
    private readonly uint _hi32;
    private readonly ulong _lo64;

    internal Decimal(Currency value) => this = Decimal.FromOACurrency(value.m_value);

    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified 32-bit signed integer.</summary>
    /// <param name="value">The value to represent as a <see cref="T:System.Decimal" />.</param>
    public Decimal(int value)
    {
      if (value >= 0)
      {
        this._flags = 0;
      }
      else
      {
        this._flags = int.MinValue;
        value = -value;
      }
      this._lo64 = (ulong) (uint) value;
      this._hi32 = 0U;
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified 32-bit unsigned integer.</summary>
    /// <param name="value">The value to represent as a <see cref="T:System.Decimal" />.</param>
    [CLSCompliant(false)]
    public Decimal(uint value)
    {
      this._flags = 0;
      this._lo64 = (ulong) value;
      this._hi32 = 0U;
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified 64-bit signed integer.</summary>
    /// <param name="value">The value to represent as a <see cref="T:System.Decimal" />.</param>
    public Decimal(long value)
    {
      if (value >= 0L)
      {
        this._flags = 0;
      }
      else
      {
        this._flags = int.MinValue;
        value = -value;
      }
      this._lo64 = (ulong) value;
      this._hi32 = 0U;
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified 64-bit unsigned integer.</summary>
    /// <param name="value">The value to represent as a <see cref="T:System.Decimal" />.</param>
    [CLSCompliant(false)]
    public Decimal(ulong value)
    {
      this._flags = 0;
      this._lo64 = value;
      this._hi32 = 0U;
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified single-precision floating-point number.</summary>
    /// <param name="value">The value to represent as a <see cref="T:System.Decimal" />.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NegativeInfinity" />.</exception>
    public Decimal(float value) => Decimal.DecCalc.VarDecFromR4(value, out Decimal.AsMutable(ref this));

    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to the value of the specified double-precision floating-point number.</summary>
    /// <param name="value">The value to represent as a <see cref="T:System.Decimal" />.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NegativeInfinity" />.</exception>
    public Decimal(double value) => Decimal.DecCalc.VarDecFromR8(value, out Decimal.AsMutable(ref this));


    #nullable disable
    private Decimal(SerializationInfo info, StreamingContext context)
    {
      this._flags = info != null ? info.GetInt32("flags") : throw new ArgumentNullException(nameof (info));
      this._hi32 = (uint) info.GetInt32("hi");
      this._lo64 = (ulong) (uint) info.GetInt32("lo") + ((ulong) info.GetInt32("mid") << 32);
    }

    /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
    /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      info.AddValue("flags", this._flags);
      info.AddValue("hi", (int) this.High);
      info.AddValue("lo", (int) this.Low);
      info.AddValue("mid", (int) this.Mid);
    }

    /// <summary>Converts the specified 64-bit signed integer, which contains an OLE Automation Currency value, to the equivalent <see cref="T:System.Decimal" /> value.</summary>
    /// <param name="cy">An OLE Automation Currency value.</param>
    /// <returns>A <see cref="T:System.Decimal" /> that contains the equivalent of <paramref name="cy" />.</returns>
    public static Decimal FromOACurrency(long cy)
    {
      bool isNegative = false;
      ulong lo;
      if (cy < 0L)
      {
        isNegative = true;
        lo = (ulong) -cy;
      }
      else
        lo = (ulong) cy;
      int scale = 4;
      if (lo != 0UL)
      {
        for (; scale != 0 && lo % 10UL == 0UL; lo /= 10UL)
          --scale;
      }
      return new Decimal((int) lo, (int) (lo >> 32), 0, isNegative, (byte) scale);
    }

    /// <summary>Converts the specified <see cref="T:System.Decimal" /> value to the equivalent OLE Automation Currency value, which is contained in a 64-bit signed integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <returns>A 64-bit signed integer that contains the OLE Automation equivalent of <paramref name="value" />.</returns>
    public static long ToOACurrency(Decimal value) => Decimal.DecCalc.VarCyFromDec(ref Decimal.AsMutable(ref value));

    private static bool IsValid(int flags) => (flags & 2130771967) == 0 && (uint) (flags & 16711680) <= 1835008U;


    #nullable enable
    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to a decimal value represented in binary and contained in a specified array.</summary>
    /// <param name="bits">An array of 32-bit signed integers containing a representation of a decimal value.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="bits" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The length of the <paramref name="bits" /> is not 4.
    /// 
    /// -or-
    /// 
    /// The representation of the decimal value in <paramref name="bits" /> is not valid.</exception>
    public Decimal(int[] bits)
      : this((ReadOnlySpan<int>) (bits ?? throw new ArgumentNullException(nameof (bits))))
    {
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> to a decimal value represented in binary and contained in the specified span.</summary>
    /// <param name="bits">A span of four <see cref="T:System.Int32" /> values that contains a binary representation of a decimal value.</param>
    /// <exception cref="T:System.ArgumentException">The length of <paramref name="bits" /> is not 4, or the representation of the decimal value in <paramref name="bits" /> is not valid.</exception>
    public Decimal(ReadOnlySpan<int> bits)
    {
      if (bits.Length == 4)
      {
        int flags = bits[3];
        if (Decimal.IsValid(flags))
        {
          this._lo64 = (ulong) (uint) bits[0] + ((ulong) (uint) bits[1] << 32);
          this._hi32 = (uint) bits[2];
          this._flags = flags;
          return;
        }
      }
      throw new ArgumentException(SR.Arg_DecBitCtor);
    }

    /// <summary>Initializes a new instance of <see cref="T:System.Decimal" /> from parameters specifying the instance's constituent parts.</summary>
    /// <param name="lo">The low 32 bits of a 96-bit integer.</param>
    /// <param name="mid">The middle 32 bits of a 96-bit integer.</param>
    /// <param name="hi">The high 32 bits of a 96-bit integer.</param>
    /// <param name="isNegative">
    /// <see langword="true" /> to indicate a negative number; <see langword="false" /> to indicate a positive number.</param>
    /// <param name="scale">A power of 10 ranging from 0 to 28.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="scale" /> is greater than 28.</exception>
    public Decimal(int lo, int mid, int hi, bool isNegative, byte scale)
    {
      if (scale > (byte) 28)
        throw new ArgumentOutOfRangeException(nameof (scale), SR.ArgumentOutOfRange_DecimalScale);
      this._lo64 = (ulong) (uint) lo + ((ulong) (uint) mid << 32);
      this._hi32 = (uint) hi;
      this._flags = (int) scale << 16;
      if (!isNegative)
        return;
      this._flags |= int.MinValue;
    }


    #nullable disable
    /// <summary>Runs when the deserialization of an object has been completed.</summary>
    /// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>
    /// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Decimal" /> object contains invalid or corrupted data.</exception>
    void IDeserializationCallback.OnDeserialization(object sender)
    {
      if (!Decimal.IsValid(this._flags))
        throw new SerializationException(SR.Overflow_Decimal);
    }

    private Decimal(int lo, int mid, int hi, int flags)
    {
      if (!Decimal.IsValid(flags))
        throw new ArgumentException(SR.Arg_DecBitCtor);
      this._lo64 = (ulong) (uint) lo + ((ulong) (uint) mid << 32);
      this._hi32 = (uint) hi;
      this._flags = flags;
    }

    private Decimal(in Decimal d, int flags)
    {
      this = d;
      this._flags = flags;
    }

    internal static Decimal Abs(in Decimal d) => new Decimal(in d, d._flags & int.MaxValue);

    /// <summary>Adds two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The first value to add.</param>
    /// <param name="d2">The second value to add.</param>
    /// <exception cref="T:System.OverflowException">The sum of <paramref name="d1" /> and <paramref name="d2" /> is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The sum of <paramref name="d1" /> and <paramref name="d2" />.</returns>
    public static Decimal Add(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.DecAddSub(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2), false);
      return d1;
    }

    /// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
    /// <param name="d">A decimal number.</param>
    /// <returns>The smallest integral value that is greater than or equal to the <paramref name="d" /> parameter. Note that this method returns a <see cref="T:System.Decimal" /> instead of an integral type.</returns>
    public static Decimal Ceiling(Decimal d)
    {
      int flags = d._flags;
      if ((flags & 16711680) != 0)
        Decimal.DecCalc.InternalRound(ref Decimal.AsMutable(ref d), (uint) (byte) (flags >> 16), MidpointRounding.ToPositiveInfinity);
      return d;
    }

    /// <summary>Compares two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The first value to compare.</param>
    /// <param name="d2">The second value to compare.</param>
    /// <returns>A signed number indicating the relative values of <paramref name="d1" /> and <paramref name="d2" />.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description><paramref name="d1" /> is less than <paramref name="d2" />.</description></item><item><term> Zero</term><description><paramref name="d1" /> and <paramref name="d2" /> are equal.</description></item><item><term> Greater than zero</term><description><paramref name="d1" /> is greater than <paramref name="d2" />.</description></item></list></returns>
    public static int Compare(Decimal d1, Decimal d2) => Decimal.DecCalc.VarDecCmp(in d1, in d2);


    #nullable enable
    /// <summary>Compares this instance to a specified object and returns a comparison of their relative values.</summary>
    /// <param name="value">The object to compare with this instance, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is not a <see cref="T:System.Decimal" />.</exception>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />, or <paramref name="value" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? value)
    {
      if (value == null)
        return 1;
      return value is Decimal d2 ? Decimal.DecCalc.VarDecCmp(in this, in d2) : throw new ArgumentException(SR.Arg_MustBeDecimal);
    }

    /// <summary>Compares this instance to a specified <see cref="T:System.Decimal" /> object and returns a comparison of their relative values.</summary>
    /// <param name="value">The object to compare with this instance.</param>
    /// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance is less than <paramref name="value" />.</description></item><item><term> Zero</term><description> This instance is equal to <paramref name="value" />.</description></item><item><term> Greater than zero</term><description> This instance is greater than <paramref name="value" />.</description></item></list></returns>
    public int CompareTo(Decimal value) => Decimal.DecCalc.VarDecCmp(in this, in value);

    /// <summary>Divides two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The dividend.</param>
    /// <param name="d2">The divisor.</param>
    /// <exception cref="T:System.DivideByZeroException">
    /// <paramref name="d2" /> is zero.</exception>
    /// <exception cref="T:System.OverflowException">The return value (that is, the quotient) is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The result of dividing <paramref name="d1" /> by <paramref name="d2" />.</returns>
    public static Decimal Divide(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.VarDecDiv(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2));
      return d1;
    }

    /// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Object" /> represent the same type and value.</summary>
    /// <param name="value">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is a <see cref="T:System.Decimal" /> and equal to this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? value) => value is Decimal d2 && Decimal.DecCalc.VarDecCmp(in this, in d2) == 0;

    /// <summary>Returns a value indicating whether this instance and a specified <see cref="T:System.Decimal" /> object represent the same value.</summary>
    /// <param name="value">An object to compare to this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(Decimal value) => Decimal.DecCalc.VarDecCmp(in this, in value) == 0;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => Decimal.DecCalc.GetHashCode(in this);

    /// <summary>Returns a value indicating whether two specified instances of <see cref="T:System.Decimal" /> represent the same value.</summary>
    /// <param name="d1">The first value to compare.</param>
    /// <param name="d2">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> and <paramref name="d2" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool Equals(Decimal d1, Decimal d2) => Decimal.DecCalc.VarDecCmp(in d1, in d2) == 0;

    /// <summary>Rounds a specified <see cref="T:System.Decimal" /> number to the closest integer toward negative infinity.</summary>
    /// <param name="d">The value to round.</param>
    /// <returns>If <paramref name="d" /> has a fractional part, the next whole <see cref="T:System.Decimal" /> number toward negative infinity that is less than <paramref name="d" />.
    /// 
    /// -or-
    /// 
    /// If <paramref name="d" /> doesn't have a fractional part, <paramref name="d" /> is returned unchanged. Note that the method returns an integral value of type <see cref="T:System.Decimal" />.</returns>
    public static Decimal Floor(Decimal d)
    {
      int flags = d._flags;
      if ((flags & 16711680) != 0)
        Decimal.DecCalc.InternalRound(ref Decimal.AsMutable(ref d), (uint) (byte) (flags >> 16), MidpointRounding.ToNegativeInfinity);
      return d;
    }

    /// <summary>Converts the numeric value of this instance to its equivalent string representation.</summary>
    /// <returns>A string that represents the value of this instance.</returns>
    public override string ToString() => Number.FormatDecimal(this, (ReadOnlySpan<char>) (char[]) null, NumberFormatInfo.CurrentInfo);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation, using the specified format.</summary>
    /// <param name="format">A standard or custom numeric format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format) => Number.FormatDecimal(this, (ReadOnlySpan<char>) format, NumberFormatInfo.CurrentInfo);

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified culture-specific format information.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="provider" />.</returns>
    public string ToString(IFormatProvider? provider) => Number.FormatDecimal(this, (ReadOnlySpan<char>) (char[]) null, NumberFormatInfo.GetInstance(provider));

    /// <summary>Converts the numeric value of this instance to its equivalent string representation using the specified format and culture-specific format information.</summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> is invalid.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
    public string ToString(string? format, IFormatProvider? provider) => Number.FormatDecimal(this, (ReadOnlySpan<char>) format, NumberFormatInfo.GetInstance(provider));

    /// <summary>Tries to format the value of the current decimal instance into the provided span of characters.</summary>
    /// <param name="destination">When this method returns, this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, the number of characters that were written in <paramref name="destination" />.</param>
    /// <param name="format">A span containing the charactes that represent a standard or custom format string that defines the acceptable format for <paramref name="destination" />.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format = default (ReadOnlySpan<char>),
      IFormatProvider? provider = null)
    {
      return Number.TryFormatDecimal(this, format, NumberFormatInfo.GetInstance(provider), destination, out charsWritten);
    }

    /// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent.</summary>
    /// <param name="s">The string representation of the number to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The equivalent to the number contained in <paramref name="s" />.</returns>
    public static Decimal Parse(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseDecimal((ReadOnlySpan<char>) s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number in a specified style to its <see cref="T:System.Decimal" /> equivalent.</summary>
    /// <param name="s">The string representation of the number to convert.</param>
    /// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" /></exception>
    /// <returns>The <see cref="T:System.Decimal" /> number equivalent to the number contained in <paramref name="s" /> as specified by <paramref name="style" />.</returns>
    public static Decimal Parse(string s, NumberStyles style)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseDecimal((ReadOnlySpan<char>) s, style, NumberFormatInfo.CurrentInfo);
    }

    /// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified culture-specific format information.</summary>
    /// <param name="s">The string representation of the number to convert.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific parsing information about <paramref name="s" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not of the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The <see cref="T:System.Decimal" /> number equivalent to the number contained in <paramref name="s" /> as specified by <paramref name="provider" />.</returns>
    public static Decimal Parse(string s, IFormatProvider? provider)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseDecimal((ReadOnlySpan<char>) s, NumberStyles.Number, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified style and culture-specific format.</summary>
    /// <param name="s">The string representation of the number to convert.</param>
    /// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific information about the format of <paramref name="s" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="s" /> is not in the correct format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="s" /> represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>The <see cref="T:System.Decimal" /> number equivalent to the number contained in <paramref name="s" /> as specified by <paramref name="style" /> and <paramref name="provider" />.</returns>
    public static Decimal Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Number.ParseDecimal((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the span representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified style and culture-specific format.</summary>
    /// <param name="s">The span containing the characters representing the number to convert.</param>
    /// <param name="style">A bitwise combination of <see cref="T:System.Globalization.NumberStyles" /> values that indicates the style elements that can be present in <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific information about the format of <paramref name="s" />.</param>
    /// <returns>The <see cref="T:System.Decimal" /> number equivalent to the number contained in <paramref name="s" /> as specified by <paramref name="style" /> and <paramref name="provider" />.</returns>
    public static Decimal Parse(
      ReadOnlySpan<char> s,
      NumberStyles style = NumberStyles.Number,
      IFormatProvider? provider = null)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      return Number.ParseDecimal(s, style, NumberFormatInfo.GetInstance(provider));
    }

    /// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">The string representation of the number to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.Decimal" /> number that is equivalent to the numeric value contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not a number in a valid format, or represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. This parameter is passed uininitialized; any value originally supplied in <paramref name="result" /> is overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, out Decimal result)
    {
      if (s != null)
        return Number.TryParseDecimal((ReadOnlySpan<char>) s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;
      result = new Decimal();
      return false;
    }

    /// <summary>Converts the span representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified style and culture-specific format. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.Decimal" /> number that is equivalent to the numeric value contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not a number in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. This parameter is passed uininitialized; any value originally supplied in <paramref name="result" /> is overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, out Decimal result) => Number.TryParseDecimal(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result) == Number.ParsingStatus.OK;

    /// <summary>Converts the string representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified style and culture-specific format. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">The string representation of the number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
    /// <param name="provider">An object that supplies culture-specific parsing information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.Decimal" /> number that is equivalent to the numeric value contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not a number in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. This parameter is passed uininitialized; any value originally supplied in <paramref name="result" /> is overwritten.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="style" /> is not a <see cref="T:System.Globalization.NumberStyles" /> value.
    /// 
    /// -or-
    /// 
    /// <paramref name="style" /> is the <see cref="F:System.Globalization.NumberStyles.AllowHexSpecifier" /> value.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      [NotNullWhen(true)] string? s,
      NumberStyles style,
      IFormatProvider? provider,
      out Decimal result)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      if (s != null)
        return Number.TryParseDecimal((ReadOnlySpan<char>) s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
      result = new Decimal();
      return false;
    }

    /// <summary>Converts the span representation of a number to its <see cref="T:System.Decimal" /> equivalent using the specified style and culture-specific format. A return value indicates whether the conversion succeeded or failed.</summary>
    /// <param name="s">A span containing the characters representing the number to convert.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.NumberStyles.Number" />.</param>
    /// <param name="provider">An object that supplies culture-specific parsing information about <paramref name="s" />.</param>
    /// <param name="result">When this method returns, contains the <see cref="T:System.Decimal" /> number that is equivalent to the numeric value contained in <paramref name="s" />, if the conversion succeeded, or zero if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is <see langword="null" /> or <see cref="F:System.String.Empty" />, is not a number in a format compliant with <paramref name="style" />, or represents a number less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />. This parameter is passed uininitialized; any value originally supplied in <paramref name="result" /> is overwritten.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="s" /> was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out Decimal result)
    {
      NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
      return Number.TryParseDecimal(s, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
    }

    /// <summary>Converts the value of a specified instance of <see cref="T:System.Decimal" /> to its equivalent binary representation.</summary>
    /// <param name="d">The value to convert.</param>
    /// <returns>A 32-bit signed integer array with four elements that contain the binary representation of <paramref name="d" />.</returns>
    public static int[] GetBits(Decimal d) => new int[4]
    {
      (int) d.Low,
      (int) d.Mid,
      (int) d.High,
      d._flags
    };

    /// <summary>Converts the value of a specified instance of <see cref="T:System.Decimal" /> to its equivalent binary representation.</summary>
    /// <param name="d">The value to convert.</param>
    /// <param name="destination">The span into which to store the four-integer binary representation.</param>
    /// <exception cref="T:System.ArgumentException">The destination span was not long enough to store the binary representation.</exception>
    /// <returns>
    /// <see langword="4" />, which is the number of integers in the binary representation.</returns>
    public static int GetBits(Decimal d, Span<int> destination)
    {
      switch (destination.Length)
      {
        case 0:
        case 1:
        case 2:
        case 3:
          ThrowHelper.ThrowArgumentException_DestinationTooShort();
          break;
      }
      destination[0] = (int) d.Low;
      destination[1] = (int) d.Mid;
      destination[2] = (int) d.High;
      destination[3] = d._flags;
      return 4;
    }

    /// <summary>Tries to convert the value of a specified instance of <see cref="T:System.Decimal" /> to its equivalent binary representation.</summary>
    /// <param name="d">The value to convert.</param>
    /// <param name="destination">The span into which to store the binary representation.</param>
    /// <param name="valuesWritten">The number of integers written to the destination.</param>
    /// <returns>
    /// <see langword="true" /> if the decimal's binary representation was written to the destination; <see langword="false" /> if the destination wasn't long enough.</returns>
    public static bool TryGetBits(Decimal d, Span<int> destination, out int valuesWritten)
    {
      switch (destination.Length)
      {
        case 0:
        case 1:
        case 2:
        case 3:
          valuesWritten = 0;
          return false;
        default:
          destination[0] = (int) d.Low;
          destination[1] = (int) d.Mid;
          destination[2] = (int) d.High;
          destination[3] = d._flags;
          valuesWritten = 4;
          return true;
      }
    }


    #nullable disable
    internal static void GetBytes(in Decimal d, Span<byte> buffer)
    {
      BinaryPrimitives.WriteInt32LittleEndian(buffer, (int) d.Low);
      BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(4), (int) d.Mid);
      BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(8), (int) d.High);
      BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(12), d._flags);
    }

    internal static Decimal ToDecimal(ReadOnlySpan<byte> span) => new Decimal(BinaryPrimitives.ReadInt32LittleEndian(span), BinaryPrimitives.ReadInt32LittleEndian(span.Slice(4)), BinaryPrimitives.ReadInt32LittleEndian(span.Slice(8)), BinaryPrimitives.ReadInt32LittleEndian(span.Slice(12)));

    internal static ref readonly Decimal Max(in Decimal d1, in Decimal d2) => ref (Decimal.DecCalc.VarDecCmp(in d1, in d2) < 0 ? ref d2 : ref d1);

    internal static ref readonly Decimal Min(in Decimal d1, in Decimal d2) => ref (Decimal.DecCalc.VarDecCmp(in d1, in d2) >= 0 ? ref d2 : ref d1);

    /// <summary>Computes the remainder after dividing two <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The dividend.</param>
    /// <param name="d2">The divisor.</param>
    /// <exception cref="T:System.DivideByZeroException">
    /// <paramref name="d2" /> is zero.</exception>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The remainder after dividing <paramref name="d1" /> by <paramref name="d2" />.</returns>
    public static Decimal Remainder(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.VarDecMod(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2));
      return d1;
    }

    /// <summary>Multiplies two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The multiplicand.</param>
    /// <param name="d2">The multiplier.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The result of multiplying <paramref name="d1" /> and <paramref name="d2" />.</returns>
    public static Decimal Multiply(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.VarDecMul(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2));
      return d1;
    }

    /// <summary>Returns the result of multiplying the specified <see cref="T:System.Decimal" /> value by negative one.</summary>
    /// <param name="d">The value to negate.</param>
    /// <returns>A decimal number with the value of <paramref name="d" />, but the opposite sign.
    /// 
    /// -or-
    /// 
    /// Zero, if <paramref name="d" /> is zero.</returns>
    public static Decimal Negate(Decimal d) => new Decimal(in d, d._flags ^ int.MinValue);

    /// <summary>Rounds a decimal value to the nearest integer.</summary>
    /// <param name="d">A decimal number to round.</param>
    /// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" /> value.</exception>
    /// <returns>The integer that is nearest to the <paramref name="d" /> parameter. If <paramref name="d" /> is halfway between two integers, one of which is even and the other odd, the even number is returned.</returns>
    public static Decimal Round(Decimal d) => Decimal.Round(ref d, 0, MidpointRounding.ToEven);

    /// <summary>Rounds a <see cref="T:System.Decimal" /> value to a specified number of decimal places.</summary>
    /// <param name="d">A decimal number to round.</param>
    /// <param name="decimals">A value from 0 to 28 that specifies the number of decimal places to round to.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="decimals" /> is not a value from 0 to 28.</exception>
    /// <returns>The decimal number equivalent to <paramref name="d" /> rounded to <paramref name="decimals" /> decimal places.</returns>
    public static Decimal Round(Decimal d, int decimals) => Decimal.Round(ref d, decimals, MidpointRounding.ToEven);

    /// <summary>Rounds a decimal value to an integer using the specified rounding strategy.</summary>
    /// <param name="d">A decimal number to round.</param>
    /// <param name="mode">One of the enumeration values that specifies which rounding strategy to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mode" /> is not a <see cref="T:System.MidpointRounding" /> value.</exception>
    /// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" /> object.</exception>
    /// <returns>The integer that <paramref name="d" /> is rounded to using the <paramref name="mode" /> rounding strategy.</returns>
    public static Decimal Round(Decimal d, MidpointRounding mode) => Decimal.Round(ref d, 0, mode);

    /// <summary>Rounds a decimal value to the specified precision using the specified rounding strategy.</summary>
    /// <param name="d">A decimal number to round.</param>
    /// <param name="decimals">The number of significant decimal places (precision) in the return value.</param>
    /// <param name="mode">One of the enumeration values that specifies which rounding strategy to use.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="decimals" /> is less than 0 or greater than 28.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mode" /> is not a <see cref="T:System.MidpointRounding" /> value.</exception>
    /// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" /> object.</exception>
    /// <returns>The number that <paramref name="d" /> is rounded to using the <paramref name="mode" /> rounding strategy and with a precision of <paramref name="decimals" />. If the precision of <paramref name="d" /> is less than <paramref name="decimals" />, <paramref name="d" /> is returned unchanged.</returns>
    public static Decimal Round(Decimal d, int decimals, MidpointRounding mode) => Decimal.Round(ref d, decimals, mode);

    private static Decimal Round(ref Decimal d, int decimals, MidpointRounding mode)
    {
      if ((uint) decimals > 28U)
        throw new ArgumentOutOfRangeException(nameof (decimals), SR.ArgumentOutOfRange_DecimalRound);
      switch (mode)
      {
        case MidpointRounding.ToEven:
        case MidpointRounding.AwayFromZero:
        case MidpointRounding.ToZero:
        case MidpointRounding.ToNegativeInfinity:
        case MidpointRounding.ToPositiveInfinity:
          int scale = d.Scale - decimals;
          if (scale > 0)
            Decimal.DecCalc.InternalRound(ref Decimal.AsMutable(ref d), (uint) scale, mode);
          return d;
        default:
          throw new ArgumentException(SR.Format(SR.Argument_InvalidEnumValue, (object) mode, (object) "MidpointRounding"), nameof (mode));
      }
    }

    internal static int Sign(in Decimal d) => ((long) d.Low64 | (long) d.High) != 0L ? d._flags >> 31 | 1 : 0;

    /// <summary>Subtracts one specified <see cref="T:System.Decimal" /> value from another.</summary>
    /// <param name="d1">The minuend.</param>
    /// <param name="d2">The subtrahend.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The result of subtracting <paramref name="d2" /> from <paramref name="d1" />.</returns>
    public static Decimal Subtract(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.DecAddSub(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2), true);
      return d1;
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer equivalent to <paramref name="value" />.</returns>
    public static byte ToByte(Decimal value)
    {
      uint uint32;
      try
      {
        uint32 = Decimal.ToUInt32(value);
      }
      catch (OverflowException ex)
      {
        Number.ThrowOverflowException(TypeCode.Byte);
        throw;
      }
      if ((int) uint32 != (int) (byte) uint32)
        Number.ThrowOverflowException(TypeCode.Byte);
      return (byte) uint32;
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 8-bit signed integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(Decimal value)
    {
      int int32;
      try
      {
        int32 = Decimal.ToInt32(value);
      }
      catch (OverflowException ex)
      {
        Number.ThrowOverflowException(TypeCode.SByte);
        throw;
      }
      if (int32 != (int) (sbyte) int32)
        Number.ThrowOverflowException(TypeCode.SByte);
      return (sbyte) int32;
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 16-bit signed integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer equivalent to <paramref name="value" />.</returns>
    public static short ToInt16(Decimal value)
    {
      int int32;
      try
      {
        int32 = Decimal.ToInt32(value);
      }
      catch (OverflowException ex)
      {
        Number.ThrowOverflowException(TypeCode.Int16);
        throw;
      }
      if (int32 != (int) (short) int32)
        Number.ThrowOverflowException(TypeCode.Int16);
      return (short) int32;
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent double-precision floating-point number.</summary>
    /// <param name="d">The decimal number to convert.</param>
    /// <returns>A double-precision floating-point number equivalent to <paramref name="d" />.</returns>
    public static double ToDouble(Decimal d) => Decimal.DecCalc.VarR8FromDec(in d);

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 32-bit signed integer.</summary>
    /// <param name="d">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="d" /> is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer equivalent to the value of <paramref name="d" />.</returns>
    public static int ToInt32(Decimal d)
    {
      Decimal.Truncate(ref d);
      if (((int) d.High | (int) d.Mid) == 0)
      {
        int low = (int) d.Low;
        if (!d.IsNegative)
        {
          if (low >= 0)
            return low;
        }
        else
        {
          int int32 = -low;
          if (int32 <= 0)
            return int32;
        }
      }
      throw new OverflowException(SR.Overflow_Int32);
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 64-bit signed integer.</summary>
    /// <param name="d">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="d" /> is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer equivalent to the value of <paramref name="d" />.</returns>
    public static long ToInt64(Decimal d)
    {
      Decimal.Truncate(ref d);
      long int64_1 = d.High == 0U ? (long) d.Low64 : throw new OverflowException(SR.Overflow_Int64);
      if (!d.IsNegative)
      {
        if (int64_1 >= 0L)
          return int64_1;
      }
      else
      {
        long int64_2 = -int64_1;
        if (int64_2 <= 0L)
          return int64_2;
      }
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.UInt16.MaxValue" /> or less than <see cref="F:System.UInt16.MinValue" />.</exception>
    /// <returns>A 16-bit unsigned integer equivalent to the value of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(Decimal value)
    {
      uint uint32;
      try
      {
        uint32 = Decimal.ToUInt32(value);
      }
      catch (OverflowException ex)
      {
        Number.ThrowOverflowException(TypeCode.UInt16);
        throw;
      }
      if ((int) uint32 != (int) (ushort) uint32)
        Number.ThrowOverflowException(TypeCode.UInt16);
      return (ushort) uint32;
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 32-bit unsigned integer.</summary>
    /// <param name="d">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="d" /> is negative or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer equivalent to the value of <paramref name="d" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(Decimal d)
    {
      Decimal.Truncate(ref d);
      if (((int) d.High | (int) d.Mid) == 0)
      {
        uint low = d.Low;
        if (!d.IsNegative || low == 0U)
          return low;
      }
      throw new OverflowException(SR.Overflow_UInt32);
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent 64-bit unsigned integer.</summary>
    /// <param name="d">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="d" /> is negative or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit unsigned integer equivalent to the value of <paramref name="d" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(Decimal d)
    {
      Decimal.Truncate(ref d);
      ulong uint64 = d.High == 0U ? d.Low64 : throw new OverflowException(SR.Overflow_UInt64);
      if (!d.IsNegative || uint64 == 0UL)
        return uint64;
    }

    /// <summary>Converts the value of the specified <see cref="T:System.Decimal" /> to the equivalent single-precision floating-point number.</summary>
    /// <param name="d">The decimal number to convert.</param>
    /// <returns>A single-precision floating-point number equivalent to the value of <paramref name="d" />.</returns>
    public static float ToSingle(Decimal d) => Decimal.DecCalc.VarR4FromDec(in d);

    /// <summary>Returns the integral digits of the specified <see cref="T:System.Decimal" />; any fractional digits are discarded.</summary>
    /// <param name="d">The decimal number to truncate.</param>
    /// <returns>The result of <paramref name="d" /> rounded toward zero, to the nearest whole number.</returns>
    public static Decimal Truncate(Decimal d)
    {
      Decimal.Truncate(ref d);
      return d;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Truncate(ref Decimal d)
    {
      int flags = d._flags;
      if ((flags & 16711680) == 0)
        return;
      Decimal.DecCalc.InternalRound(ref Decimal.AsMutable(ref d), (uint) (byte) (flags >> 16), MidpointRounding.ToZero);
    }

    /// <summary>Defines an implicit conversion of an 8-bit unsigned integer to a <see cref="T:System.Decimal" />.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>The converted 8-bit unsigned integer.</returns>
    public static implicit operator Decimal(byte value) => new Decimal((uint) value);

    /// <summary>Defines an implicit conversion of an 8-bit signed integer to a <see cref="T:System.Decimal" />.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>The converted 8-bit signed integer.</returns>
    [CLSCompliant(false)]
    public static implicit operator Decimal(sbyte value) => new Decimal((int) value);

    /// <summary>Defines an implicit conversion of a 16-bit signed integer to a <see cref="T:System.Decimal" />.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns>The converted 16-bit signed integer.</returns>
    public static implicit operator Decimal(short value) => new Decimal((int) value);

    /// <summary>Defines an implicit conversion of a 16-bit unsigned integer to a <see cref="T:System.Decimal" />.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>The converted 16-bit unsigned integer.</returns>
    [CLSCompliant(false)]
    public static implicit operator Decimal(ushort value) => new Decimal((uint) value);

    /// <summary>Defines an implicit conversion of a Unicode character to a <see cref="T:System.Decimal" />.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <returns>The converted Unicode character.</returns>
    public static implicit operator Decimal(char value) => new Decimal((uint) value);

    /// <summary>Defines an implicit conversion of a 32-bit signed integer to a <see cref="T:System.Decimal" />.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns>The converted 32-bit signed integer.</returns>
    public static implicit operator Decimal(int value) => new Decimal(value);

    /// <summary>Defines an implicit conversion of a 32-bit unsigned integer to a <see cref="T:System.Decimal" />.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns>The converted 32-bit unsigned integer.</returns>
    [CLSCompliant(false)]
    public static implicit operator Decimal(uint value) => new Decimal(value);

    /// <summary>Defines an implicit conversion of a 64-bit signed integer to a <see cref="T:System.Decimal" />.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <returns>The converted 64-bit signed integer.</returns>
    public static implicit operator Decimal(long value) => new Decimal(value);

    /// <summary>Defines an implicit conversion of a 64-bit unsigned integer to a <see cref="T:System.Decimal" />.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <returns>The converted 64-bit unsigned integer.</returns>
    [CLSCompliant(false)]
    public static implicit operator Decimal(ulong value) => new Decimal(value);

    /// <summary>Defines an explicit conversion of a single-precision floating-point number to a <see cref="T:System.Decimal" />.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NegativeInfinity" />.</exception>
    /// <returns>The converted single-precision floating point number.</returns>
    public static explicit operator Decimal(float value) => new Decimal(value);

    /// <summary>Defines an explicit conversion of a double-precision floating-point number to a <see cref="T:System.Decimal" />.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NegativeInfinity" />.</exception>
    /// <returns>The converted double-precision floating point number.</returns>
    public static explicit operator Decimal(double value) => new Decimal(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to an 8-bit unsigned integer.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that represents the converted <see cref="T:System.Decimal" />.</returns>
    public static explicit operator byte(Decimal value) => Decimal.ToByte(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to an 8-bit signed integer.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that represents the converted <see cref="T:System.Decimal" />.</returns>
    [CLSCompliant(false)]
    public static explicit operator sbyte(Decimal value) => Decimal.ToSByte(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a Unicode character.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />.</exception>
    /// <returns>A Unicode character that represents the converted <see cref="T:System.Decimal" />.</returns>
    public static explicit operator char(Decimal value)
    {
      try
      {
        return (char) Decimal.ToUInt16(value);
      }
      catch (OverflowException ex)
      {
        throw new OverflowException(SR.Overflow_Char, (Exception) ex);
      }
    }

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a 16-bit signed integer.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that represents the converted <see cref="T:System.Decimal" />.</returns>
    public static explicit operator short(Decimal value) => Decimal.ToInt16(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a 16-bit unsigned integer.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that represents the converted <see cref="T:System.Decimal" />.</returns>
    [CLSCompliant(false)]
    public static explicit operator ushort(Decimal value) => Decimal.ToUInt16(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a 32-bit signed integer.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer that represents the converted <see cref="T:System.Decimal" />.</returns>
    public static explicit operator int(Decimal value) => Decimal.ToInt32(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a 32-bit unsigned integer.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer that represents the converted <see cref="T:System.Decimal" />.</returns>
    [CLSCompliant(false)]
    public static explicit operator uint(Decimal value) => Decimal.ToUInt32(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a 64-bit signed integer.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer that represents the converted <see cref="T:System.Decimal" />.</returns>
    public static explicit operator long(Decimal value) => Decimal.ToInt64(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a 64-bit unsigned integer.
    /// 
    /// This API is not CLS-compliant.</summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is negative or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit unsigned integer that represents the converted <see cref="T:System.Decimal" />.</returns>
    [CLSCompliant(false)]
    public static explicit operator ulong(Decimal value) => Decimal.ToUInt64(value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a single-precision floating-point number.</summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A single-precision floating-point number that represents the converted <see cref="T:System.Decimal" />.</returns>
    public static explicit operator float(Decimal value) => Decimal.DecCalc.VarR4FromDec(in value);

    /// <summary>Defines an explicit conversion of a <see cref="T:System.Decimal" /> to a double-precision floating-point number.</summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A double-precision floating-point number that represents the converted <see cref="T:System.Decimal" />.</returns>
    public static explicit operator double(Decimal value) => Decimal.DecCalc.VarR8FromDec(in value);

    /// <summary>Returns the value of the <see cref="T:System.Decimal" /> operand (the sign of the operand is unchanged).</summary>
    /// <param name="d">The operand to return.</param>
    /// <returns>The value of the operand, <paramref name="d" />.</returns>
    public static Decimal operator +(Decimal d) => d;

    /// <summary>Negates the value of the specified <see cref="T:System.Decimal" /> operand.</summary>
    /// <param name="d">The value to negate.</param>
    /// <returns>The result of <paramref name="d" /> multiplied by negative one (-1).</returns>
    public static Decimal operator -(Decimal d) => new Decimal(in d, d._flags ^ int.MinValue);

    /// <summary>Increments the <see cref="T:System.Decimal" /> operand by 1.</summary>
    /// <param name="d">The value to increment.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The value of <paramref name="d" /> incremented by 1.</returns>
    public static Decimal operator ++(Decimal d) => Decimal.Add(d, Decimal.One);

    /// <summary>Decrements the <see cref="T:System.Decimal" /> operand by one.</summary>
    /// <param name="d">The value to decrement.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The value of <paramref name="d" /> decremented by 1.</returns>
    public static Decimal operator --(Decimal d) => Decimal.Subtract(d, Decimal.One);

    /// <summary>Adds two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The first value to add.</param>
    /// <param name="d2">The second value to add.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The result of adding <paramref name="d1" /> and <paramref name="d2" />.</returns>
    public static Decimal operator +(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.DecAddSub(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2), false);
      return d1;
    }

    /// <summary>Subtracts two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The minuend.</param>
    /// <param name="d2">The subtrahend.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The result of subtracting <paramref name="d2" /> from <paramref name="d1" />.</returns>
    public static Decimal operator -(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.DecAddSub(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2), true);
      return d1;
    }

    /// <summary>Multiplies two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The first value to multiply.</param>
    /// <param name="d2">The second value to multiply.</param>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The result of multiplying <paramref name="d1" /> by <paramref name="d2" />.</returns>
    public static Decimal operator *(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.VarDecMul(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2));
      return d1;
    }

    /// <summary>Divides two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The dividend.</param>
    /// <param name="d2">The divisor.</param>
    /// <exception cref="T:System.DivideByZeroException">
    /// <paramref name="d2" /> is zero.</exception>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The result of dividing <paramref name="d1" /> by <paramref name="d2" />.</returns>
    public static Decimal operator /(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.VarDecDiv(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2));
      return d1;
    }

    /// <summary>Returns the remainder resulting from dividing two specified <see cref="T:System.Decimal" /> values.</summary>
    /// <param name="d1">The dividend.</param>
    /// <param name="d2">The divisor.</param>
    /// <exception cref="T:System.DivideByZeroException">
    /// <paramref name="d2" /> is <see langword="zero" />.</exception>
    /// <exception cref="T:System.OverflowException">The return value is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>The remainder resulting from dividing <paramref name="d1" /> by <paramref name="d2" />.</returns>
    public static Decimal operator %(Decimal d1, Decimal d2)
    {
      Decimal.DecCalc.VarDecMod(ref Decimal.AsMutable(ref d1), ref Decimal.AsMutable(ref d2));
      return d1;
    }

    /// <summary>Returns a value that indicates whether two <see cref="T:System.Decimal" /> values are equal.</summary>
    /// <param name="d1">The first value to compare.</param>
    /// <param name="d2">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> and <paramref name="d2" /> are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Decimal d1, Decimal d2) => Decimal.DecCalc.VarDecCmp(in d1, in d2) == 0;

    /// <summary>Returns a value that indicates whether two <see cref="T:System.Decimal" /> objects have different values.</summary>
    /// <param name="d1">The first value to compare.</param>
    /// <param name="d2">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> and <paramref name="d2" /> are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Decimal d1, Decimal d2) => Decimal.DecCalc.VarDecCmp(in d1, in d2) != 0;

    /// <summary>Returns a value indicating whether a specified <see cref="T:System.Decimal" /> is less than another specified <see cref="T:System.Decimal" />.</summary>
    /// <param name="d1">The first value to compare.</param>
    /// <param name="d2">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> is less than <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <(Decimal d1, Decimal d2) => Decimal.DecCalc.VarDecCmp(in d1, in d2) < 0;

    /// <summary>Returns a value indicating whether a specified <see cref="T:System.Decimal" /> is less than or equal to another specified <see cref="T:System.Decimal" />.</summary>
    /// <param name="d1">The first value to compare.</param>
    /// <param name="d2">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> is less than or equal to <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator <=(Decimal d1, Decimal d2) => Decimal.DecCalc.VarDecCmp(in d1, in d2) <= 0;

    /// <summary>Returns a value indicating whether a specified <see cref="T:System.Decimal" /> is greater than another specified <see cref="T:System.Decimal" />.</summary>
    /// <param name="d1">The first value to compare.</param>
    /// <param name="d2">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> is greater than <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >(Decimal d1, Decimal d2) => Decimal.DecCalc.VarDecCmp(in d1, in d2) > 0;

    /// <summary>Returns a value indicating whether a specified <see cref="T:System.Decimal" /> is greater than or equal to another specified <see cref="T:System.Decimal" />.</summary>
    /// <param name="d1">The first value to compare.</param>
    /// <param name="d2">The second value to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="d1" /> is greater than or equal to <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
    public static bool operator >=(Decimal d1, Decimal d2) => Decimal.DecCalc.VarDecCmp(in d1, in d2) >= 0;

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.Decimal" />.</summary>
    /// <returns>The enumerated constant <see cref="F:System.TypeCode.Decimal" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.Decimal;

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>
    /// <see langword="true" /> if the value of the current instance is not zero; otherwise, <see langword="false" />.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(this);

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>None. This conversion is not supported.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Decimal), (object) "Char"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.SByte" />.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToByte(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Byte" />.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Int16" />.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToUInt16(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.UInt16" />.</returns>
    ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">The parameter is ignored.</param>
    /// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Int32" />.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt32(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.UInt32" />.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Int64" />.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToInt64(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.OverflowException">The resulting integer value is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.UInt64" />.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToSingle(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Single" />.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => Decimal.DecCalc.VarR4FromDec(in this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDouble(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, converted to a <see cref="T:System.Double" />.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => Decimal.DecCalc.VarR8FromDec(in this);

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToDecimal(System.IFormatProvider)" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <returns>The value of the current instance, unchanged.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => this;

    /// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">This parameter is ignored.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>None. This conversion is not supported.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Decimal), (object) "DateTime"));

    /// <summary>For a description of this member, see <see cref="M:System.IConvertible.ToType(System.Type,System.IFormatProvider)" />.</summary>
    /// <param name="type">The type to which to convert the value of this <see cref="T:System.Decimal" /> instance.</param>
    /// <param name="provider">An <see cref="T:System.IFormatProvider" /> implementation that supplies culture-specific information about the format of the returned value.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidCastException">The requested type conversion is not supported.</exception>
    /// <returns>The value of the current instance, converted to a <paramref name="type" />.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IAdditionOperators<Decimal, Decimal, Decimal>.op_Addition(
      Decimal left,
      Decimal right)
    {
      return left + right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IAdditiveIdentity<Decimal, Decimal>.AdditiveIdentity => new Decimal(0, 0, 0, false, (byte) 1);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<Decimal, Decimal>.op_LessThan(
      Decimal left,
      Decimal right)
    {
      return left < right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<Decimal, Decimal>.op_LessThanOrEqual(
      Decimal left,
      Decimal right)
    {
      return left <= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<Decimal, Decimal>.op_GreaterThan(
      Decimal left,
      Decimal right)
    {
      return left > right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IComparisonOperators<Decimal, Decimal>.op_GreaterThanOrEqual(
      Decimal left,
      Decimal right)
    {
      return left >= right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IDecrementOperators<Decimal>.op_Decrement(Decimal value) => --value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IDivisionOperators<Decimal, Decimal, Decimal>.op_Division(
      Decimal left,
      Decimal right)
    {
      return left / right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<Decimal, Decimal>.op_Equality(
      Decimal left,
      Decimal right)
    {
      return left == right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IEqualityOperators<Decimal, Decimal>.op_Inequality(
      Decimal left,
      Decimal right)
    {
      return left != right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IIncrementOperators<Decimal>.op_Increment(Decimal value) => ++value;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IMinMaxValue<Decimal>.MinValue => new Decimal(-1, -1, -1, true, (byte) 0);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IMinMaxValue<Decimal>.MaxValue => new Decimal(-1, -1, -1, false, (byte) 0);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IModulusOperators<Decimal, Decimal, Decimal>.op_Modulus(
      Decimal left,
      Decimal right)
    {
      return left % right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IMultiplicativeIdentity<Decimal, Decimal>.MultiplicativeIdentity => new Decimal(10, 0, 0, false, (byte) 1);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IMultiplyOperators<Decimal, Decimal, Decimal>.op_Multiply(
      Decimal left,
      Decimal right)
    {
      return left * right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.One => new Decimal(10, 0, 0, false, (byte) 1);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.Zero => new Decimal(0, 0, 0, false, (byte) 1);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.Abs(Decimal value) => Math.Abs(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Decimal INumber<Decimal>.Create<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (Decimal) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (Decimal) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (Decimal) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (Decimal) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (Decimal) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (Decimal) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (Decimal) (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (Decimal) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (Decimal) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (Decimal) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (Decimal) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (Decimal) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (Decimal) (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return Decimal.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Decimal INumber<Decimal>.CreateSaturating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (Decimal) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (Decimal) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (Decimal) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (Decimal) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (Decimal) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (Decimal) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (Decimal) (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (Decimal) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (Decimal) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (Decimal) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (Decimal) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (Decimal) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (Decimal) (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return Decimal.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Decimal INumber<Decimal>.CreateTruncating<TOther>(TOther value)
    {
      if (typeof (TOther) == typeof (byte))
        return (Decimal) (byte) (object) value;
      if (typeof (TOther) == typeof (char))
        return (Decimal) (char) (object) value;
      if (typeof (TOther) == typeof (Decimal))
        return (Decimal) (object) value;
      if (typeof (TOther) == typeof (double))
        return (Decimal) (double) (object) value;
      if (typeof (TOther) == typeof (short))
        return (Decimal) (short) (object) value;
      if (typeof (TOther) == typeof (int))
        return (Decimal) (int) (object) value;
      if (typeof (TOther) == typeof (long))
        return (Decimal) (long) (object) value;
      if (typeof (TOther) == typeof (IntPtr))
        return (Decimal) (long) (IntPtr) (object) value;
      if (typeof (TOther) == typeof (sbyte))
        return (Decimal) (sbyte) (object) value;
      if (typeof (TOther) == typeof (float))
        return (Decimal) (float) (object) value;
      if (typeof (TOther) == typeof (ushort))
        return (Decimal) (ushort) (object) value;
      if (typeof (TOther) == typeof (uint))
        return (Decimal) (uint) (object) value;
      if (typeof (TOther) == typeof (ulong))
        return (Decimal) (ulong) (object) value;
      if (typeof (TOther) == typeof (UIntPtr))
        return (Decimal) (ulong) (UIntPtr) (object) value;
      ThrowHelper.ThrowNotSupportedException();
      return Decimal.Zero;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.Clamp(Decimal value, Decimal min, Decimal max) => Math.Clamp(value, min, max);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    (Decimal Quotient, Decimal Remainder) INumber<Decimal>.DivRem(
      Decimal left,
      Decimal right)
    {
      return (left / right, left % right);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.Max(Decimal x, Decimal y) => Math.Max(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.Min(Decimal x, Decimal y) => Math.Min(x, y);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.Parse(
      string s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return Decimal.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.Parse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider)
    {
      return Decimal.Parse(s, style, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal INumber<Decimal>.Sign(Decimal value) => (Decimal) Math.Sign(value);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool INumber<Decimal>.TryCreate<TOther>(TOther value, out Decimal result)
    {
      if (typeof (TOther) == typeof (byte))
      {
        result = (Decimal) (byte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (char))
      {
        result = (Decimal) (char) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (Decimal))
      {
        result = (Decimal) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (double))
      {
        result = (Decimal) (double) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (short))
      {
        result = (Decimal) (short) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (int))
      {
        result = (Decimal) (int) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (long))
      {
        result = (Decimal) (long) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (IntPtr))
      {
        result = (Decimal) (long) (IntPtr) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (sbyte))
      {
        result = (Decimal) (sbyte) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (float))
      {
        result = (Decimal) (float) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ushort))
      {
        result = (Decimal) (ushort) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (uint))
      {
        result = (Decimal) (uint) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (ulong))
      {
        result = (Decimal) (ulong) (object) value;
        return true;
      }
      if (typeof (TOther) == typeof (UIntPtr))
      {
        result = (Decimal) (ulong) (UIntPtr) (object) value;
        return true;
      }
      ThrowHelper.ThrowNotSupportedException();
      result = new Decimal();
      return false;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<Decimal>.TryParse(
      [NotNullWhen(true)] string s,
      NumberStyles style,
      IFormatProvider provider,
      out Decimal result)
    {
      return Decimal.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool INumber<Decimal>.TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider provider,
      out Decimal result)
    {
      return Decimal.TryParse(s, style, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IParseable<Decimal>.Parse(string s, IFormatProvider provider) => Decimal.Parse(s, provider);

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool IParseable<Decimal>.TryParse(
      [NotNullWhen(true)] string s,
      IFormatProvider provider,
      out Decimal result)
    {
      return Decimal.TryParse(s, NumberStyles.Number, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal ISignedNumber<Decimal>.NegativeOne => Decimal.MinusOne;

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal ISpanParseable<Decimal>.Parse(
      ReadOnlySpan<char> s,
      IFormatProvider provider)
    {
      return Decimal.Parse(s, NumberStyles.Number, provider);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    bool ISpanParseable<Decimal>.TryParse(
      ReadOnlySpan<char> s,
      IFormatProvider provider,
      out Decimal result)
    {
      return Decimal.TryParse(s, NumberStyles.Number, provider, out result);
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal ISubtractionOperators<Decimal, Decimal, Decimal>.op_Subtraction(
      Decimal left,
      Decimal right)
    {
      return left - right;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IUnaryNegationOperators<Decimal, Decimal>.op_UnaryNegation(
      Decimal value)
    {
      return -value;
    }

    [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
    Decimal IUnaryPlusOperators<Decimal, Decimal>.op_UnaryPlus(
      Decimal value)
    {
      return value;
    }

    internal uint High => this._hi32;

    internal uint Low => (uint) this._lo64;

    internal uint Mid => (uint) (this._lo64 >> 32);

    internal bool IsNegative => this._flags < 0;

    internal int Scale => (int) (byte) (this._flags >> 16);

    private ulong Low64 => this._lo64;

    private static ref Decimal.DecCalc AsMutable(ref Decimal d) => ref Unsafe.As<Decimal, Decimal.DecCalc>(ref d);

    internal static uint DecDivMod1E9(ref Decimal value) => Decimal.DecCalc.DecDivMod1E9(ref Decimal.AsMutable(ref value));

    [StructLayout(LayoutKind.Explicit)]
    private struct DecCalc
    {
      [FieldOffset(0)]
      private uint uflags;
      [FieldOffset(4)]
      private uint uhi;
      [FieldOffset(8)]
      private uint ulo;
      [FieldOffset(12)]
      private uint umid;
      [FieldOffset(8)]
      private ulong ulomid;
      private static readonly uint[] s_powers10 = new uint[10]
      {
        1U,
        10U,
        100U,
        1000U,
        10000U,
        100000U,
        1000000U,
        10000000U,
        100000000U,
        1000000000U
      };
      private static readonly ulong[] s_ulongPowers10 = new ulong[19]
      {
        10UL,
        100UL,
        1000UL,
        10000UL,
        100000UL,
        1000000UL,
        10000000UL,
        100000000UL,
        1000000000UL,
        10000000000UL,
        100000000000UL,
        1000000000000UL,
        10000000000000UL,
        100000000000000UL,
        1000000000000000UL,
        10000000000000000UL,
        100000000000000000UL,
        1000000000000000000UL,
        10000000000000000000UL
      };
      private static readonly double[] s_doublePowers10 = new double[81]
      {
        1.0,
        10.0,
        100.0,
        1000.0,
        10000.0,
        100000.0,
        1000000.0,
        10000000.0,
        100000000.0,
        1000000000.0,
        10000000000.0,
        100000000000.0,
        1000000000000.0,
        10000000000000.0,
        100000000000000.0,
        1000000000000000.0,
        10000000000000000.0,
        1E+17,
        1E+18,
        1E+19,
        1E+20,
        1E+21,
        1E+22,
        1E+23,
        1E+24,
        1E+25,
        1E+26,
        1E+27,
        1E+28,
        1E+29,
        1E+30,
        1E+31,
        1E+32,
        1E+33,
        1E+34,
        1E+35,
        1E+36,
        1E+37,
        1E+38,
        1E+39,
        1E+40,
        1E+41,
        1E+42,
        1E+43,
        1E+44,
        1E+45,
        1E+46,
        1E+47,
        1E+48,
        1E+49,
        1E+50,
        1E+51,
        1E+52,
        1E+53,
        1E+54,
        1E+55,
        1E+56,
        1E+57,
        1E+58,
        1E+59,
        1E+60,
        1E+61,
        1E+62,
        1E+63,
        1E+64,
        1E+65,
        1E+66,
        1E+67,
        1E+68,
        1E+69,
        1E+70,
        1E+71,
        1E+72,
        1E+73,
        1E+74,
        1E+75,
        1E+76,
        1E+77,
        1E+78,
        1E+79,
        1E+80
      };
      private static readonly Decimal.DecCalc.PowerOvfl[] PowerOvflValues = new Decimal.DecCalc.PowerOvfl[8]
      {
        new Decimal.DecCalc.PowerOvfl(429496729U, 2576980377U, 2576980377U),
        new Decimal.DecCalc.PowerOvfl(42949672U, 4123168604U, 687194767U),
        new Decimal.DecCalc.PowerOvfl(4294967U, 1271310319U, 2645699854U),
        new Decimal.DecCalc.PowerOvfl(429496U, 3133608139U, 694066715U),
        new Decimal.DecCalc.PowerOvfl(42949U, 2890341191U, 2216890319U),
        new Decimal.DecCalc.PowerOvfl(4294U, 4154504685U, 2369172679U),
        new Decimal.DecCalc.PowerOvfl(429U, 2133437386U, 4102387834U),
        new Decimal.DecCalc.PowerOvfl(42U, 4078814305U, 410238783U)
      };

      private uint High
      {
        get => this.uhi;
        set => this.uhi = value;
      }

      private uint Low
      {
        get => this.ulo;
        set => this.ulo = value;
      }

      private uint Mid
      {
        get => this.umid;
        set => this.umid = value;
      }

      private bool IsNegative => (int) this.uflags < 0;

      private int Scale => (int) (byte) (this.uflags >> 16);

      private ulong Low64
      {
        get => this.ulomid;
        set => this.ulomid = value;
      }

      private static unsafe uint GetExponent(float f) => (uint) (byte) (*(uint*) &f >> 23);

      private static unsafe uint GetExponent(double d) => (uint) ((ulong) *(long*) &d >> 52) & 2047U;

      private static ulong UInt32x32To64(uint a, uint b) => (ulong) a * (ulong) b;

      private static void UInt64x64To128(ulong a, ulong b, ref Decimal.DecCalc result)
      {
        ulong num1 = Decimal.DecCalc.UInt32x32To64((uint) a, (uint) b);
        ulong num2 = Decimal.DecCalc.UInt32x32To64((uint) a, (uint) (b >> 32));
        ulong num3 = Decimal.DecCalc.UInt32x32To64((uint) (a >> 32), (uint) (b >> 32)) + (num2 >> 32);
        ulong num4;
        ulong num5 = num1 + (num4 = num2 << 32);
        if (num5 < num4)
          ++num3;
        ulong num6 = Decimal.DecCalc.UInt32x32To64((uint) (a >> 32), (uint) b);
        ulong num7 = num3 + (num6 >> 32);
        ulong num8;
        ulong num9 = num5 + (num8 = num6 << 32);
        if (num9 < num8)
          ++num7;
        if (num7 > (ulong) uint.MaxValue)
          Number.ThrowOverflowException(TypeCode.Decimal);
        result.Low64 = num9;
        result.High = (uint) num7;
      }

      private static uint Div96By32(ref Decimal.DecCalc.Buf12 bufNum, uint den)
      {
        if (bufNum.U2 != 0U)
        {
          ulong high64 = bufNum.High64;
          ulong num1 = high64 / (ulong) den;
          bufNum.High64 = num1;
          ulong num2 = (ulong) ((long) high64 - (long) ((uint) num1 * den) << 32) | (ulong) bufNum.U0;
          if (num2 == 0UL)
            return 0;
          uint num3 = (uint) (num2 / (ulong) den);
          bufNum.U0 = num3;
          return (uint) num2 - num3 * den;
        }
        ulong low64 = bufNum.Low64;
        if (low64 == 0UL)
          return 0;
        ulong num = low64 / (ulong) den;
        bufNum.Low64 = num;
        return (uint) (low64 - num * (ulong) den);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool Div96ByConst(ref ulong high64, ref uint low, uint pow)
      {
        ulong num1 = high64 / (ulong) pow;
        uint num2 = (uint) (((ulong) ((long) high64 - (long) num1 * (long) pow << 32) + (ulong) low) / (ulong) pow);
        if ((int) low != (int) num2 * (int) pow)
          return false;
        high64 = num1;
        low = num2;
        return true;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static void Unscale(ref uint low, ref ulong high64, ref int scale)
      {
        while ((byte) low == (byte) 0 && scale >= 8 && Decimal.DecCalc.Div96ByConst(ref high64, ref low, 100000000U))
          scale -= 8;
        if (((int) low & 15) == 0 && scale >= 4 && Decimal.DecCalc.Div96ByConst(ref high64, ref low, 10000U))
          scale -= 4;
        if (((int) low & 3) == 0 && scale >= 2 && Decimal.DecCalc.Div96ByConst(ref high64, ref low, 100U))
          scale -= 2;
        if (((int) low & 1) != 0 || scale < 1 || !Decimal.DecCalc.Div96ByConst(ref high64, ref low, 10U))
          return;
        --scale;
      }

      private static uint Div96By64(ref Decimal.DecCalc.Buf12 bufNum, ulong den)
      {
        uint u2 = bufNum.U2;
        if (u2 == 0U)
        {
          ulong low64 = bufNum.Low64;
          if (low64 < den)
            return 0;
          uint num1 = (uint) (low64 / den);
          ulong num2 = low64 - (ulong) num1 * den;
          bufNum.Low64 = num2;
          return num1;
        }
        uint num3 = (uint) (den >> 32);
        if (u2 >= num3)
        {
          ulong num4 = bufNum.Low64 - (den << 32);
          uint num5 = 0;
          do
          {
            --num5;
            num4 += den;
          }
          while (num4 >= den);
          bufNum.Low64 = num4;
          return num5;
        }
        ulong high64 = bufNum.High64;
        if (high64 < (ulong) num3)
          return 0;
        uint a = (uint) (high64 / (ulong) num3);
        ulong num6 = (ulong) bufNum.U0 | (ulong) ((long) high64 - (long) (a * num3) << 32);
        ulong num7 = Decimal.DecCalc.UInt32x32To64(a, (uint) den);
        ulong num8 = num6 - num7;
        if (num8 > ~num7)
        {
          do
          {
            --a;
            num8 += den;
          }
          while (num8 >= den);
        }
        bufNum.Low64 = num8;
        return a;
      }

      private static uint Div128By96(
        ref Decimal.DecCalc.Buf16 bufNum,
        ref Decimal.DecCalc.Buf12 bufDen)
      {
        ulong high64 = bufNum.High64;
        uint u2 = bufDen.U2;
        if (high64 < (ulong) u2)
          return 0;
        uint a = (uint) (high64 / (ulong) u2);
        uint num1 = (uint) high64 - a * u2;
        ulong num2 = Decimal.DecCalc.UInt32x32To64(a, bufDen.U0);
        ulong num3 = Decimal.DecCalc.UInt32x32To64(a, bufDen.U1) + (num2 >> 32);
        ulong num4 = (ulong) (uint) num2 | num3 << 32;
        ulong num5 = num3 >> 32;
        ulong num6 = bufNum.Low64 - num4;
        uint num7 = num1 - (uint) num5;
        if (num6 > ~num4)
        {
          --num7;
          if (num7 < ~(uint) num5)
            goto label_7;
        }
        else if (num7 <= ~(uint) num5)
          goto label_7;
        ulong low64 = bufDen.Low64;
        do
        {
          --a;
          num6 += low64;
          num7 += u2;
        }
        while ((num6 >= low64 || num7++ >= u2) && num7 >= u2);
label_7:
        bufNum.Low64 = num6;
        bufNum.U2 = num7;
        return a;
      }

      private static uint IncreaseScale(ref Decimal.DecCalc.Buf12 bufNum, uint power)
      {
        ulong num1 = Decimal.DecCalc.UInt32x32To64(bufNum.U0, power);
        bufNum.U0 = (uint) num1;
        ulong num2 = (num1 >> 32) + Decimal.DecCalc.UInt32x32To64(bufNum.U1, power);
        bufNum.U1 = (uint) num2;
        ulong num3 = (num2 >> 32) + Decimal.DecCalc.UInt32x32To64(bufNum.U2, power);
        bufNum.U2 = (uint) num3;
        return (uint) (num3 >> 32);
      }

      private static void IncreaseScale64(ref Decimal.DecCalc.Buf12 bufNum, uint power)
      {
        ulong num1 = Decimal.DecCalc.UInt32x32To64(bufNum.U0, power);
        bufNum.U0 = (uint) num1;
        ulong num2 = (num1 >> 32) + Decimal.DecCalc.UInt32x32To64(bufNum.U1, power);
        bufNum.High64 = num2;
      }

      private static unsafe int ScaleResult(Decimal.DecCalc.Buf24* bufRes, uint hiRes, int scale)
      {
        uint* result = (uint*) bufRes;
        int num1 = 0;
        if (hiRes > 2U)
        {
          num1 = (((int) hiRes * 32 - 64 - 1 - BitOperations.LeadingZeroCount(*(uint*) ((IntPtr) result + (IntPtr) ((long) hiRes * 4L)))) * 77 >> 8) + 1;
          if (num1 > scale)
            goto label_30;
        }
        if (num1 < scale - 28)
          num1 = scale - 28;
        if (num1 != 0)
        {
          scale -= num1;
          uint num2 = 0;
          uint remainder = 0;
          while (true)
          {
            uint num3;
            do
            {
              num2 |= remainder;
              uint quotient;
              switch (num1 - 1)
              {
                case 0:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 10U);
                  break;
                case 1:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 100U);
                  break;
                case 2:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 1000U);
                  break;
                case 3:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 10000U);
                  break;
                case 4:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 100000U);
                  break;
                case 5:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 1000000U);
                  break;
                case 6:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 10000000U);
                  break;
                case 7:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 100000000U);
                  break;
                default:
                  num3 = Decimal.DecCalc.DivByConst(result, hiRes, out quotient, out remainder, 1000000000U);
                  break;
              }
              *(int*) ((IntPtr) result + (IntPtr) ((long) hiRes * 4L)) = (int) quotient;
              if (quotient == 0U && hiRes != 0U)
                --hiRes;
              num1 -= 9;
            }
            while (num1 > 0);
            if (hiRes > 2U)
            {
              if (scale != 0)
              {
                num1 = 1;
                --scale;
              }
              else
                goto label_30;
            }
            else
            {
              uint num4 = num3 >> 1;
              if (num4 <= remainder && (num4 < remainder || ((int) *result & 1 | (int) num2) != 0))
              {
                uint* numPtr = result;
                uint num5 = *numPtr + 1U;
                int num6 = (int) num5;
                *numPtr = (uint) num6;
                if (num5 == 0U)
                {
                  uint num7 = 0;
                  uint num8;
                  do
                  {
                    IntPtr num9 = (IntPtr) result + (IntPtr) ((long) ++num7 * 4L);
                    num8 = *(uint*) num9 + 1U;
                    *(int*) num9 = (int) num8;
                  }
                  while (num8 == 0U);
                  if (num7 > 2U)
                  {
                    if (scale != 0)
                    {
                      hiRes = num7;
                      num2 = 0U;
                      remainder = 0U;
                      num1 = 1;
                      --scale;
                    }
                    else
                      goto label_30;
                  }
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
            }
          }
        }
        return scale;
label_30:
        Number.ThrowOverflowException(TypeCode.Decimal);
        return 0;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static unsafe uint DivByConst(
        uint* result,
        uint hiRes,
        out uint quotient,
        out uint remainder,
        uint power)
      {
        uint num1 = *(uint*) ((IntPtr) result + (IntPtr) ((long) hiRes * 4L));
        remainder = num1 - (quotient = num1 / power) * power;
        for (uint index = hiRes - 1U; (int) index >= 0; --index)
        {
          ulong num2 = (ulong) *(uint*) ((IntPtr) result + (IntPtr) ((long) index * 4L)) + ((ulong) remainder << 32);
          remainder = (uint) num2 - (uint) (*(int*) ((IntPtr) result + (IntPtr) ((long) index * 4L)) = (int) (uint) (num2 / (ulong) power)) * power;
        }
        return power;
      }

      private static int OverflowUnscale(ref Decimal.DecCalc.Buf12 bufQuo, int scale, bool sticky)
      {
        if (--scale < 0)
          Number.ThrowOverflowException(TypeCode.Decimal);
        bufQuo.U2 = 429496729U;
        ulong num1 = 25769803776UL + (ulong) bufQuo.U1;
        uint num2 = (uint) (num1 / 10UL);
        bufQuo.U1 = num2;
        ulong num3 = (ulong) ((long) num1 - (long) (num2 * 10U) << 32) + (ulong) bufQuo.U0;
        uint num4 = (uint) (num3 / 10UL);
        bufQuo.U0 = num4;
        switch ((uint) (num3 - (ulong) (num4 * 10U)))
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
            return scale;
          case 5:
            if (sticky || ((int) bufQuo.U0 & 1) != 0)
              goto default;
            else
              goto case 0;
          default:
            Decimal.DecCalc.Add32To96(ref bufQuo, 1U);
            goto case 0;
        }
      }

      private static int SearchScale(ref Decimal.DecCalc.Buf12 bufQuo, int scale)
      {
        uint u2 = bufQuo.U2;
        ulong low64 = bufQuo.Low64;
        int num = 0;
        if (u2 <= 429496729U)
        {
          Decimal.DecCalc.PowerOvfl[] powerOvflValues = Decimal.DecCalc.PowerOvflValues;
          if (scale > 19)
          {
            num = 28 - scale;
            if (u2 < powerOvflValues[num - 1].Hi)
              goto label_18;
          }
          else if (u2 < 4U || u2 == 4U && low64 <= 5441186219426131129UL)
            return 9;
          if (u2 > 42949U)
          {
            if (u2 > 4294967U)
            {
              num = 2;
              if (u2 > 42949672U)
                --num;
            }
            else
            {
              num = 4;
              if (u2 > 429496U)
                --num;
            }
          }
          else if (u2 > 429U)
          {
            num = 6;
            if (u2 > 4294U)
              --num;
          }
          else
          {
            num = 8;
            if (u2 > 42U)
              --num;
          }
          if ((int) u2 == (int) powerOvflValues[num - 1].Hi && low64 > powerOvflValues[num - 1].MidLo)
            --num;
        }
label_18:
        if (num + scale < 0)
          Number.ThrowOverflowException(TypeCode.Decimal);
        return num;
      }

      private static bool Add32To96(ref Decimal.DecCalc.Buf12 bufNum, uint value) => (bufNum.Low64 += (ulong) value) >= (ulong) value || ++bufNum.U2 != 0U;

      internal static unsafe void DecAddSub(
        ref Decimal.DecCalc d1,
        ref Decimal.DecCalc d2,
        bool sign)
      {
        ulong a1 = d1.Low64;
        uint a2 = d1.High;
        uint num1 = d1.uflags;
        uint uflags = d2.uflags;
        uint num2 = uflags ^ num1;
        sign ^= (num2 & 2147483648U) > 0U;
        ulong num3;
        uint num4;
        if (((int) num2 & 16711680) != 0)
        {
          uint num5 = num1;
          num1 = (uint) ((int) uflags & 16711680 | (int) num1 & int.MinValue);
          int index = (int) num1 - (int) num5 >> 16;
          if (index < 0)
          {
            index = -index;
            num1 = num5;
            if (sign)
              num1 ^= 2147483648U;
            a1 = d2.Low64;
            a2 = d2.High;
            d2 = d1;
          }
          if (a2 == 0U)
          {
            if (a1 <= (ulong) uint.MaxValue)
            {
              if ((uint) a1 == 0U)
              {
                uint num6 = num1 & 2147483648U;
                if (sign)
                  num6 ^= 2147483648U;
                d1 = d2;
                d1.uflags = d2.uflags & 16711680U | num6;
                return;
              }
              while (index > 9)
              {
                index -= 9;
                a1 = Decimal.DecCalc.UInt32x32To64((uint) a1, 1000000000U);
                if (a1 > (ulong) uint.MaxValue)
                  goto label_14;
              }
              a1 = Decimal.DecCalc.UInt32x32To64((uint) a1, Decimal.DecCalc.s_powers10[index]);
              goto label_52;
            }
label_14:
            do
            {
              uint b = 1000000000;
              if (index < 9)
                b = Decimal.DecCalc.s_powers10[index];
              ulong num7 = Decimal.DecCalc.UInt32x32To64((uint) a1, b);
              ulong num8 = Decimal.DecCalc.UInt32x32To64((uint) (a1 >> 32), b) + (num7 >> 32);
              a1 = (ulong) (uint) num7 + (num8 << 32);
              a2 = (uint) (num8 >> 32);
              if ((index -= 9) <= 0)
                goto label_52;
            }
            while (a2 == 0U);
          }
          ulong num9;
          do
          {
            uint b = 1000000000;
            if (index < 9)
              b = Decimal.DecCalc.s_powers10[index];
            ulong num10 = Decimal.DecCalc.UInt32x32To64((uint) a1, b);
            ulong num11 = Decimal.DecCalc.UInt32x32To64((uint) (a1 >> 32), b) + (num10 >> 32);
            a1 = (ulong) (uint) num10 + (num11 << 32);
            num9 = (num11 >> 32) + Decimal.DecCalc.UInt32x32To64(a2, b);
            index -= 9;
            if (num9 <= (ulong) uint.MaxValue)
              a2 = (uint) num9;
            else
              goto label_22;
          }
          while (index > 0);
          goto label_52;
label_22:
          Decimal.DecCalc.Buf24 buf24;
          Unsafe.SkipInit<Decimal.DecCalc.Buf24>(out buf24);
          buf24.Low64 = a1;
          buf24.Mid64 = num9;
          uint hiRes = 3;
          for (; index > 0; index -= 9)
          {
            uint b = 1000000000;
            if (index < 9)
              b = Decimal.DecCalc.s_powers10[index];
            ulong num12 = 0;
            uint* numPtr = (uint*) &buf24;
            uint num13 = 0;
            do
            {
              ulong num14 = num12 + Decimal.DecCalc.UInt32x32To64(*(uint*) ((IntPtr) numPtr + (IntPtr) ((long) num13 * 4L)), b);
              *(int*) ((IntPtr) numPtr + (IntPtr) ((long) num13 * 4L)) = (int) (uint) num14;
              ++num13;
              num12 = num14 >> 32;
            }
            while (num13 <= hiRes);
            if ((uint) num12 != 0U)
              *(int*) ((IntPtr) numPtr + (IntPtr) ((long) ++hiRes * 4L)) = (int) (uint) num12;
          }
          ulong low64_1 = buf24.Low64;
          ulong low64_2 = d2.Low64;
          uint u2 = buf24.U2;
          uint high = d2.High;
          if (sign)
          {
            num3 = low64_1 - low64_2;
            num4 = u2 - high;
            if (num3 > low64_1)
            {
              --num4;
              if (num4 < u2)
                goto label_45;
            }
            else if (num4 <= u2)
              goto label_45;
            uint* numPtr = (uint*) &buf24;
            uint num15 = 3;
            uint num16;
            do
            {
              IntPtr num17 = (IntPtr) numPtr + (IntPtr) ((long) num15++ * 4L);
              num16 = *(uint*) num17;
              *(int*) num17 = (int) num16 - 1;
            }
            while (num16 == 0U);
            if (*(uint*) ((IntPtr) numPtr + (IntPtr) ((long) hiRes * 4L)) == 0U && --hiRes <= 2U)
              goto label_59;
          }
          else
          {
            num3 = low64_2 + low64_1;
            num4 = high + u2;
            if (num3 < low64_1)
            {
              ++num4;
              if (num4 > u2)
                goto label_45;
            }
            else if (num4 >= u2)
              goto label_45;
            uint* numPtr = (uint*) &buf24;
            uint num18 = 3;
            do
            {
              IntPtr num19 = (IntPtr) numPtr + (IntPtr) ((long) num18++ * 4L);
              uint num20 = *(uint*) num19 + 1U;
              *(int*) num19 = (int) num20;
              if (num20 != 0U)
                goto label_45;
            }
            while (hiRes >= num18);
            *(int*) ((IntPtr) numPtr + (IntPtr) ((long) num18 * 4L)) = 1;
            hiRes = num18;
          }
label_45:
          buf24.Low64 = num3;
          buf24.U2 = num4;
          int num21 = Decimal.DecCalc.ScaleResult(&buf24, hiRes, (int) (byte) (num1 >> 16));
          num1 = (uint) ((int) num1 & -16711681 | num21 << 16);
          num3 = buf24.Low64;
          num4 = buf24.U2;
          goto label_59;
        }
label_52:
        ulong num22 = a1;
        uint num23 = a2;
        if (sign)
        {
          num3 = num22 - d2.Low64;
          num4 = num23 - d2.High;
          if (num3 <= num22)
            goto label_55;
          else
            goto label_54;
label_46:
          num1 ^= 2147483648U;
          num4 = ~num4;
          num3 = (ulong) -(long) num3;
          if (num3 == 0UL)
          {
            ++num4;
            goto label_59;
          }
          else
            goto label_59;
label_54:
          --num4;
          if (num4 < num23)
            goto label_59;
          else
            goto label_46;
label_55:
          if (num4 > num23)
            goto label_46;
        }
        else
        {
          num3 = num22 + d2.Low64;
          num4 = num23 + d2.High;
          if (num3 >= num22)
            goto label_58;
          else
            goto label_57;
label_48:
          if (((int) num1 & 16711680) == 0)
            Number.ThrowOverflowException(TypeCode.Decimal);
          num1 -= 65536U;
          ulong num24 = (ulong) num4 + 4294967296UL;
          num4 = (uint) (num24 / 10UL);
          ulong num25 = (ulong) ((long) num24 - (long) (num4 * 10U) << 32) + (num3 >> 32);
          uint num26 = (uint) (num25 / 10UL);
          ulong num27 = (ulong) ((long) num25 - (long) (num26 * 10U) << 32) + (ulong) (uint) num3;
          ulong num28 = (ulong) num26 << 32;
          uint num29 = (uint) (num27 / 10UL);
          num3 = num28 + (ulong) num29;
          uint num30 = (uint) num27 - num29 * 10U;
          if (num30 >= 5U && (num30 > 5U || ((long) num3 & 1L) != 0L) && ++num3 == 0UL)
          {
            ++num4;
            goto label_59;
          }
          else
            goto label_59;
label_57:
          ++num4;
          if (num4 > num23)
            goto label_59;
          else
            goto label_48;
label_58:
          if (num4 < num23)
            goto label_48;
        }
label_59:
        d1.uflags = num1;
        d1.High = num4;
        d1.Low64 = num3;
      }

      internal static long VarCyFromDec(ref Decimal.DecCalc pdecIn)
      {
        int scale = pdecIn.Scale - 4;
        long num1;
        if (scale < 0)
        {
          if (pdecIn.High == 0U)
          {
            uint a = Decimal.DecCalc.s_powers10[-scale];
            ulong num2 = Decimal.DecCalc.UInt32x32To64(a, pdecIn.Mid);
            if (num2 <= (ulong) uint.MaxValue)
            {
              ulong num3;
              ulong num4 = Decimal.DecCalc.UInt32x32To64(a, pdecIn.Low) + (num3 = num2 << 32);
              if (num4 >= num3)
                num1 = (long) num4;
              else
                goto label_13;
            }
            else
              goto label_13;
          }
          else
            goto label_13;
        }
        else
        {
          if (scale != 0)
            Decimal.DecCalc.InternalRound(ref pdecIn, (uint) scale, MidpointRounding.ToEven);
          if (pdecIn.High == 0U)
            num1 = (long) pdecIn.Low64;
          else
            goto label_13;
        }
        if (num1 >= 0L || num1 == long.MinValue && pdecIn.IsNegative)
        {
          if (pdecIn.IsNegative)
            num1 = -num1;
          return num1;
        }
label_13:
        throw new OverflowException(SR.Overflow_Currency);
      }

      internal static int VarDecCmp(in Decimal d1, in Decimal d2)
      {
        if (((long) d2.Low64 | (long) d2.High) == 0L)
          return ((long) d1.Low64 | (long) d1.High) == 0L ? 0 : d1._flags >> 31 | 1;
        if (((long) d1.Low64 | (long) d1.High) == 0L)
          return -(d2._flags >> 31 | 1);
        int num = (d1._flags >> 31) - (d2._flags >> 31);
        return num != 0 ? num : Decimal.DecCalc.VarDecCmpSub(in d1, in d2);
      }

      private static int VarDecCmpSub(in Decimal d1, in Decimal d2)
      {
        int flags = d2._flags;
        int num1 = flags >> 31 | 1;
        int num2 = flags - d1._flags;
        ulong a1 = d1.Low64;
        uint a2 = d1.High;
        ulong num3 = d2.Low64;
        uint num4 = d2.High;
        if (num2 != 0)
        {
          int index = num2 >> 16;
          if (index < 0)
          {
            index = -index;
            num1 = -num1;
            ulong num5 = a1;
            a1 = num3;
            num3 = num5;
            uint num6 = a2;
            a2 = num4;
            num4 = num6;
          }
          do
          {
            uint b = index >= 9 ? 1000000000U : Decimal.DecCalc.s_powers10[index];
            ulong num7 = Decimal.DecCalc.UInt32x32To64((uint) a1, b);
            ulong num8 = Decimal.DecCalc.UInt32x32To64((uint) (a1 >> 32), b) + (num7 >> 32);
            a1 = (ulong) (uint) num7 + (num8 << 32);
            ulong num9 = (num8 >> 32) + Decimal.DecCalc.UInt32x32To64(a2, b);
            if (num9 > (ulong) uint.MaxValue)
              return num1;
            a2 = (uint) num9;
          }
          while ((index -= 9) > 0);
        }
        uint num10 = a2 - num4;
        if (num10 != 0U)
        {
          if (num10 > a2)
            num1 = -num1;
          return num1;
        }
        ulong num11 = a1 - num3;
        if (num11 == 0UL)
          num1 = 0;
        else if (num11 > a1)
          num1 = -num1;
        return num1;
      }

      internal static unsafe void VarDecMul(ref Decimal.DecCalc d1, ref Decimal.DecCalc d2)
      {
        int scale = (int) (byte) (d1.uflags + d2.uflags >> 16);
        Decimal.DecCalc.Buf24 buf24;
        Unsafe.SkipInit<Decimal.DecCalc.Buf24>(out buf24);
        uint hiRes;
        if (((int) d1.High | (int) d1.Mid) == 0)
        {
          if (((int) d2.High | (int) d2.Mid) == 0)
          {
            ulong num1 = Decimal.DecCalc.UInt32x32To64(d1.Low, d2.Low);
            if (scale > 28)
            {
              if (scale <= 47)
              {
                int index = scale - 29;
                ulong num2 = Decimal.DecCalc.s_ulongPowers10[index];
                ulong num3 = num1 / num2;
                ulong num4 = num1 - num3 * num2;
                num1 = num3;
                ulong num5 = num2 >> 1;
                if (num4 >= num5 && (num4 > num5 || ((uint) num1 & 1U) > 0U))
                  ++num1;
                scale = 28;
              }
              else
                goto label_35;
            }
            d1.Low64 = num1;
            d1.uflags = (uint) (((int) d2.uflags ^ (int) d1.uflags) & int.MinValue | scale << 16);
            return;
          }
          ulong num6 = Decimal.DecCalc.UInt32x32To64(d1.Low, d2.Low);
          buf24.U0 = (uint) num6;
          ulong num7 = Decimal.DecCalc.UInt32x32To64(d1.Low, d2.Mid) + (num6 >> 32);
          buf24.U1 = (uint) num7;
          ulong num8 = num7 >> 32;
          if (d2.High != 0U)
          {
            num8 += Decimal.DecCalc.UInt32x32To64(d1.Low, d2.High);
            if (num8 > (ulong) uint.MaxValue)
            {
              buf24.Mid64 = num8;
              hiRes = 3U;
              goto label_32;
            }
          }
          buf24.U2 = (uint) num8;
          hiRes = 2U;
        }
        else if (((int) d2.High | (int) d2.Mid) == 0)
        {
          ulong num9 = Decimal.DecCalc.UInt32x32To64(d2.Low, d1.Low);
          buf24.U0 = (uint) num9;
          ulong num10 = Decimal.DecCalc.UInt32x32To64(d2.Low, d1.Mid) + (num9 >> 32);
          buf24.U1 = (uint) num10;
          ulong num11 = num10 >> 32;
          if (d1.High != 0U)
          {
            num11 += Decimal.DecCalc.UInt32x32To64(d2.Low, d1.High);
            if (num11 > (ulong) uint.MaxValue)
            {
              buf24.Mid64 = num11;
              hiRes = 3U;
              goto label_32;
            }
          }
          buf24.U2 = (uint) num11;
          hiRes = 2U;
        }
        else
        {
          ulong num12 = Decimal.DecCalc.UInt32x32To64(d1.Low, d2.Low);
          buf24.U0 = (uint) num12;
          ulong num13 = Decimal.DecCalc.UInt32x32To64(d1.Low, d2.Mid) + (num12 >> 32);
          ulong num14 = Decimal.DecCalc.UInt32x32To64(d1.Mid, d2.Low) + num13;
          buf24.U1 = (uint) num14;
          ulong num15 = num14 >= num13 ? num14 >> 32 : num14 >> 32 | 4294967296UL;
          ulong num16 = Decimal.DecCalc.UInt32x32To64(d1.Mid, d2.Mid) + num15;
          if ((d1.High | d2.High) > 0U)
          {
            ulong num17 = Decimal.DecCalc.UInt32x32To64(d1.Low, d2.High);
            ulong num18 = num16 + num17;
            uint num19 = 0;
            if (num18 < num17)
              num19 = 1U;
            ulong num20 = Decimal.DecCalc.UInt32x32To64(d1.High, d2.Low);
            ulong num21 = num18 + num20;
            buf24.U2 = (uint) num21;
            if (num21 < num20)
              ++num19;
            ulong num22 = (ulong) num19 << 32 | num21 >> 32;
            ulong num23 = Decimal.DecCalc.UInt32x32To64(d1.Mid, d2.High) + num22;
            uint num24 = 0;
            if (num23 < num22)
              num24 = 1U;
            ulong num25 = Decimal.DecCalc.UInt32x32To64(d1.High, d2.Mid);
            ulong num26 = num23 + num25;
            buf24.U3 = (uint) num26;
            if (num26 < num25)
              ++num24;
            ulong num27 = (ulong) num24 << 32 | num26 >> 32;
            buf24.High64 = Decimal.DecCalc.UInt32x32To64(d1.High, d2.High) + num27;
            hiRes = 5U;
          }
          else
          {
            buf24.Mid64 = num16;
            hiRes = 3U;
          }
        }
        for (uint* numPtr = (uint*) &buf24; numPtr[(int) hiRes] == 0U; --hiRes)
        {
          if (hiRes == 0U)
            goto label_35;
        }
label_32:
        if (hiRes > 2U || scale > 28)
          scale = Decimal.DecCalc.ScaleResult(&buf24, hiRes, scale);
        d1.Low64 = buf24.Low64;
        d1.High = buf24.U2;
        d1.uflags = (uint) (((int) d2.uflags ^ (int) d1.uflags) & int.MinValue | scale << 16);
        return;
label_35:
        d1 = new Decimal.DecCalc();
      }

      internal static void VarDecFromR4(float input, out Decimal.DecCalc result)
      {
        result = new Decimal.DecCalc();
        int num1 = (int) Decimal.DecCalc.GetExponent(input) - 126;
        if (num1 < -94)
          return;
        if (num1 > 96)
          Number.ThrowOverflowException(TypeCode.Decimal);
        uint num2 = 0;
        if ((double) input < 0.0)
        {
          input = -input;
          num2 = 2147483648U;
        }
        double a1 = (double) input;
        int index1 = 6 - (num1 * 19728 >> 16);
        if (index1 >= 0)
        {
          if (index1 > 28)
            index1 = 28;
          a1 *= Decimal.DecCalc.s_doublePowers10[index1];
        }
        else if (index1 != -1 || a1 >= 10000000.0)
          a1 /= Decimal.DecCalc.s_doublePowers10[-index1];
        else
          index1 = 0;
        if (a1 < 1000000.0 && index1 < 28)
        {
          a1 *= 10.0;
          ++index1;
        }
        uint a2;
        if (Sse41.IsSupported)
        {
          a2 = (uint) (int) Math.Round(a1);
        }
        else
        {
          a2 = (uint) (int) a1;
          double num3 = a1 - (double) (int) a2;
          if (num3 > 0.5 || num3 == 0.5 && ((int) a2 & 1) != 0)
            ++a2;
        }
        if (a2 == 0U)
          return;
        if (index1 < 0)
        {
          int index2 = -index1;
          if (index2 < 10)
            result.Low64 = Decimal.DecCalc.UInt32x32To64(a2, Decimal.DecCalc.s_powers10[index2]);
          else if (index2 > 18)
          {
            Decimal.DecCalc.UInt64x64To128(Decimal.DecCalc.UInt32x32To64(a2, Decimal.DecCalc.s_powers10[index2 - 18]), 1000000000000000000UL, ref result);
          }
          else
          {
            ulong b = Decimal.DecCalc.UInt32x32To64(a2, Decimal.DecCalc.s_powers10[index2 - 9]);
            ulong num4 = Decimal.DecCalc.UInt32x32To64(1000000000U, (uint) (b >> 32));
            ulong num5 = Decimal.DecCalc.UInt32x32To64(1000000000U, (uint) b);
            result.Low = (uint) num5;
            ulong num6 = num4 + (num5 >> 32);
            result.Mid = (uint) num6;
            ulong num7 = num6 >> 32;
            result.High = (uint) num7;
          }
        }
        else
        {
          int num8 = index1;
          if (num8 > 6)
            num8 = 6;
          if (((int) a2 & 15) == 0 && num8 >= 4)
          {
            uint num9 = a2 / 10000U;
            if ((int) a2 == (int) num9 * 10000)
            {
              a2 = num9;
              index1 -= 4;
              num8 -= 4;
            }
          }
          if (((int) a2 & 3) == 0 && num8 >= 2)
          {
            uint num10 = a2 / 100U;
            if ((int) a2 == (int) num10 * 100)
            {
              a2 = num10;
              index1 -= 2;
              num8 -= 2;
            }
          }
          if (((int) a2 & 1) == 0 && num8 >= 1)
          {
            uint num11 = a2 / 10U;
            if ((int) a2 == (int) num11 * 10)
            {
              a2 = num11;
              --index1;
            }
          }
          num2 |= (uint) (index1 << 16);
          result.Low = a2;
        }
        result.uflags = num2;
      }

      internal static void VarDecFromR8(double input, out Decimal.DecCalc result)
      {
        result = new Decimal.DecCalc();
        int num1 = (int) Decimal.DecCalc.GetExponent(input) - 1022;
        if (num1 < -94)
          return;
        if (num1 > 96)
          Number.ThrowOverflowException(TypeCode.Decimal);
        uint num2 = 0;
        if (input < 0.0)
        {
          input = -input;
          num2 = 2147483648U;
        }
        double a1 = input;
        int index1 = 14 - (num1 * 19728 >> 16);
        if (index1 >= 0)
        {
          if (index1 > 28)
            index1 = 28;
          a1 *= Decimal.DecCalc.s_doublePowers10[index1];
        }
        else if (index1 != -1 || a1 >= 1000000000000000.0)
          a1 /= Decimal.DecCalc.s_doublePowers10[-index1];
        else
          index1 = 0;
        if (a1 < 100000000000000.0 && index1 < 28)
        {
          a1 *= 10.0;
          ++index1;
        }
        ulong a2;
        if (Sse41.IsSupported)
        {
          a2 = (ulong) (long) Math.Round(a1);
        }
        else
        {
          a2 = (ulong) (long) a1;
          double num3 = a1 - (double) (long) a2;
          if (num3 > 0.5 || num3 == 0.5 && ((long) a2 & 1L) != 0L)
            ++a2;
        }
        if (a2 == 0UL)
          return;
        if (index1 < 0)
        {
          int index2 = -index1;
          if (index2 < 10)
          {
            uint b = Decimal.DecCalc.s_powers10[index2];
            ulong num4 = Decimal.DecCalc.UInt32x32To64((uint) a2, b);
            ulong num5 = Decimal.DecCalc.UInt32x32To64((uint) (a2 >> 32), b);
            result.Low = (uint) num4;
            ulong num6 = num5 + (num4 >> 32);
            result.Mid = (uint) num6;
            ulong num7 = num6 >> 32;
            result.High = (uint) num7;
          }
          else
            Decimal.DecCalc.UInt64x64To128(a2, Decimal.DecCalc.s_ulongPowers10[index2 - 1], ref result);
        }
        else
        {
          int num8 = index1;
          if (num8 > 14)
            num8 = 14;
          if ((byte) a2 == (byte) 0 && num8 >= 8)
          {
            ulong num9 = a2 / 100000000UL;
            if ((int) (uint) a2 == (int) (uint) (num9 * 100000000UL))
            {
              a2 = num9;
              index1 -= 8;
              num8 -= 8;
            }
          }
          if (((int) (uint) a2 & 15) == 0 && num8 >= 4)
          {
            ulong num10 = a2 / 10000UL;
            if ((int) (uint) a2 == (int) (uint) (num10 * 10000UL))
            {
              a2 = num10;
              index1 -= 4;
              num8 -= 4;
            }
          }
          if (((int) (uint) a2 & 3) == 0 && num8 >= 2)
          {
            ulong num11 = a2 / 100UL;
            if ((int) (uint) a2 == (int) (uint) (num11 * 100UL))
            {
              a2 = num11;
              index1 -= 2;
              num8 -= 2;
            }
          }
          if (((int) (uint) a2 & 1) == 0 && num8 >= 1)
          {
            ulong num12 = a2 / 10UL;
            if ((int) (uint) a2 == (int) (uint) (num12 * 10UL))
            {
              a2 = num12;
              --index1;
            }
          }
          num2 |= (uint) (index1 << 16);
          result.Low64 = a2;
        }
        result.uflags = num2;
      }

      internal static float VarR4FromDec(in Decimal value) => (float) Decimal.DecCalc.VarR8FromDec(in value);

      internal static double VarR8FromDec(in Decimal value)
      {
        double num = ((double) value.Low64 + (double) value.High * 1.8446744073709552E+19) / Decimal.DecCalc.s_doublePowers10[value.Scale];
        if (value.IsNegative)
          num = -num;
        return num;
      }

      internal static int GetHashCode(in Decimal d)
      {
        if (((long) d.Low64 | (long) d.High) == 0L)
          return 0;
        uint flags = (uint) d._flags;
        if (((int) flags & 16711680) == 0 || ((int) d.Low & 1) != 0)
          return (int) flags ^ (int) d.High ^ (int) d.Mid ^ (int) d.Low;
        int scale = (int) (byte) (flags >> 16);
        uint low = d.Low;
        ulong high64 = (ulong) d.High << 32 | (ulong) d.Mid;
        Decimal.DecCalc.Unscale(ref low, ref high64, ref scale);
        return ((int) flags & -16711681 | scale << 16) ^ (int) (uint) (high64 >> 32) ^ (int) (uint) high64 ^ (int) low;
      }

      internal static unsafe void VarDecDiv(ref Decimal.DecCalc d1, ref Decimal.DecCalc d2)
      {
        Decimal.DecCalc.Buf12 buf12;
        Unsafe.SkipInit<Decimal.DecCalc.Buf12>(out buf12);
        int scale = (int) (sbyte) (d1.uflags - d2.uflags >> 16);
        bool flag = false;
        if (((int) d2.High | (int) d2.Mid) == 0)
        {
          uint low = d2.Low;
          if (low == 0U)
            throw new DivideByZeroException();
          buf12.Low64 = d1.Low64;
          buf12.U2 = d1.High;
          uint a = Decimal.DecCalc.Div96By32(ref buf12, low);
          uint num1;
          do
          {
            int index;
            if (a == 0U)
            {
              if (scale < 0)
                index = Math.Min(9, -scale);
              else
                goto label_34;
            }
            else
            {
              flag = true;
              if (scale == 28 || (index = Decimal.DecCalc.SearchScale(ref buf12, scale)) == 0)
              {
                uint num2 = a << 1;
                if (num2 < a || num2 >= low && (num2 > low || ((int) buf12.U0 & 1) != 0))
                  goto label_38;
                else
                  goto label_34;
              }
            }
            uint num3 = Decimal.DecCalc.s_powers10[index];
            scale += index;
            if (Decimal.DecCalc.IncreaseScale(ref buf12, num3) == 0U)
            {
              ulong num4 = Decimal.DecCalc.UInt32x32To64(a, num3);
              num1 = (uint) (num4 / (ulong) low);
              a = (uint) num4 - num1 * low;
            }
            else
              goto label_40;
          }
          while (Decimal.DecCalc.Add32To96(ref buf12, num1));
          scale = Decimal.DecCalc.OverflowUnscale(ref buf12, scale, a > 0U);
        }
        else
        {
          uint num5 = d2.High;
          if (num5 == 0U)
            num5 = d2.Mid;
          int num6 = BitOperations.LeadingZeroCount(num5);
          Decimal.DecCalc.Buf16 bufNum;
          Unsafe.SkipInit<Decimal.DecCalc.Buf16>(out bufNum);
          bufNum.Low64 = d1.Low64 << num6;
          bufNum.High64 = (ulong) d1.Mid + ((ulong) d1.High << 32) >> 32 - num6;
          ulong den = d2.Low64 << num6;
          if (d2.High == 0U)
          {
            buf12.U2 = 0U;
            buf12.U1 = Decimal.DecCalc.Div96By64(ref *(Decimal.DecCalc.Buf12*) &bufNum.U1, den);
            buf12.U0 = Decimal.DecCalc.Div96By64(ref *(Decimal.DecCalc.Buf12*) &bufNum, den);
            uint num7;
            do
            {
              int index;
              if (bufNum.Low64 == 0UL)
              {
                if (scale < 0)
                  index = Math.Min(9, -scale);
                else
                  goto label_34;
              }
              else
              {
                flag = true;
                if (scale == 28 || (index = Decimal.DecCalc.SearchScale(ref buf12, scale)) == 0)
                {
                  ulong low64 = bufNum.Low64;
                  ulong num8;
                  if ((long) low64 < 0L || (num8 = low64 << 1) > den || (long) num8 == (long) den && ((int) buf12.U0 & 1) != 0)
                    goto label_38;
                  else
                    goto label_34;
                }
              }
              uint power = Decimal.DecCalc.s_powers10[index];
              scale += index;
              if (Decimal.DecCalc.IncreaseScale(ref buf12, power) == 0U)
              {
                Decimal.DecCalc.IncreaseScale64(ref *(Decimal.DecCalc.Buf12*) &bufNum, power);
                num7 = Decimal.DecCalc.Div96By64(ref *(Decimal.DecCalc.Buf12*) &bufNum, den);
              }
              else
                goto label_40;
            }
            while (Decimal.DecCalc.Add32To96(ref buf12, num7));
            scale = Decimal.DecCalc.OverflowUnscale(ref buf12, scale, bufNum.Low64 > 0UL);
          }
          else
          {
            Decimal.DecCalc.Buf12 bufDen;
            Unsafe.SkipInit<Decimal.DecCalc.Buf12>(out bufDen);
            bufDen.Low64 = den;
            bufDen.U2 = (uint) ((ulong) d2.Mid + ((ulong) d2.High << 32) >> 32 - num6);
            buf12.Low64 = (ulong) Decimal.DecCalc.Div128By96(ref bufNum, ref bufDen);
            buf12.U2 = 0U;
            uint num9;
            do
            {
              int index;
              if (((long) bufNum.Low64 | (long) bufNum.U2) == 0L)
              {
                if (scale < 0)
                  index = Math.Min(9, -scale);
                else
                  goto label_34;
              }
              else
              {
                flag = true;
                if (scale == 28 || (index = Decimal.DecCalc.SearchScale(ref buf12, scale)) == 0)
                {
                  if ((int) bufNum.U2 >= 0)
                  {
                    uint num10 = bufNum.U1 >> 31;
                    bufNum.Low64 <<= 1;
                    bufNum.U2 = (bufNum.U2 << 1) + num10;
                    if (bufNum.U2 > bufDen.U2 || (int) bufNum.U2 == (int) bufDen.U2 && (bufNum.Low64 > bufDen.Low64 || (long) bufNum.Low64 == (long) bufDen.Low64 && ((int) buf12.U0 & 1) != 0))
                      goto label_38;
                    else
                      goto label_34;
                  }
                  else
                    goto label_38;
                }
              }
              uint power = Decimal.DecCalc.s_powers10[index];
              scale += index;
              if (Decimal.DecCalc.IncreaseScale(ref buf12, power) == 0U)
              {
                bufNum.U3 = Decimal.DecCalc.IncreaseScale(ref *(Decimal.DecCalc.Buf12*) &bufNum, power);
                num9 = Decimal.DecCalc.Div128By96(ref bufNum, ref bufDen);
              }
              else
                goto label_40;
            }
            while (Decimal.DecCalc.Add32To96(ref buf12, num9));
            scale = Decimal.DecCalc.OverflowUnscale(ref buf12, scale, (bufNum.Low64 | bufNum.High64) > 0UL);
          }
        }
label_34:
        if (flag)
        {
          uint u0 = buf12.U0;
          ulong high64 = buf12.High64;
          Decimal.DecCalc.Unscale(ref u0, ref high64, ref scale);
          d1.Low = u0;
          d1.Mid = (uint) high64;
          d1.High = (uint) (high64 >> 32);
        }
        else
        {
          d1.Low64 = buf12.Low64;
          d1.High = buf12.U2;
        }
        d1.uflags = (uint) (((int) d1.uflags ^ (int) d2.uflags) & int.MinValue | scale << 16);
        return;
label_38:
        if (++buf12.Low64 == 0UL && ++buf12.U2 == 0U)
        {
          scale = Decimal.DecCalc.OverflowUnscale(ref buf12, scale, true);
          goto label_34;
        }
        else
          goto label_34;
label_40:
        Number.ThrowOverflowException(TypeCode.Decimal);
      }

      internal static void VarDecMod(ref Decimal.DecCalc d1, ref Decimal.DecCalc d2)
      {
        if (((int) d2.ulo | (int) d2.umid | (int) d2.uhi) == 0)
          throw new DivideByZeroException();
        if (((int) d1.ulo | (int) d1.umid | (int) d1.uhi) == 0)
          return;
        d2.uflags = (uint) ((int) d2.uflags & int.MaxValue | (int) d1.uflags & int.MinValue);
        int num1 = Decimal.DecCalc.VarDecCmpSub(in Unsafe.As<Decimal.DecCalc, Decimal>(ref d1), in Unsafe.As<Decimal.DecCalc, Decimal>(ref d2));
        if (num1 == 0)
        {
          d1.ulo = 0U;
          d1.umid = 0U;
          d1.uhi = 0U;
          if (d2.uflags <= d1.uflags)
            return;
          d1.uflags = d2.uflags;
        }
        else
        {
          if ((num1 ^ (int) d1.uflags & int.MinValue) < 0)
            return;
          int scale = (int) (sbyte) (d1.uflags - d2.uflags >> 16);
          if (scale > 0)
          {
            do
            {
              uint b = scale >= 9 ? 1000000000U : Decimal.DecCalc.s_powers10[scale];
              ulong num2 = Decimal.DecCalc.UInt32x32To64(d2.Low, b);
              d2.Low = (uint) num2;
              ulong num3 = (num2 >> 32) + ((ulong) d2.Mid + ((ulong) d2.High << 32)) * (ulong) b;
              d2.Mid = (uint) num3;
              d2.High = (uint) (num3 >> 32);
            }
            while ((scale -= 9) > 0);
            scale = 0;
          }
          do
          {
            if (scale < 0)
            {
              d1.uflags = d2.uflags;
              Decimal.DecCalc.Buf12 bufQuo;
              Unsafe.SkipInit<Decimal.DecCalc.Buf12>(out bufQuo);
              bufQuo.Low64 = d1.Low64;
              bufQuo.U2 = d1.High;
              uint b;
              do
              {
                int index = Decimal.DecCalc.SearchScale(ref bufQuo, 28 + scale);
                if (index != 0)
                {
                  b = index >= 9 ? 1000000000U : Decimal.DecCalc.s_powers10[index];
                  scale += index;
                  ulong num4 = Decimal.DecCalc.UInt32x32To64(bufQuo.U0, b);
                  bufQuo.U0 = (uint) num4;
                  ulong num5 = num4 >> 32;
                  bufQuo.High64 = num5 + bufQuo.High64 * (ulong) b;
                }
                else
                  break;
              }
              while (b == 1000000000U && scale < 0);
              d1.Low64 = bufQuo.Low64;
              d1.High = bufQuo.U2;
            }
            if (d1.High == 0U)
            {
              d1.Low64 %= d2.Low64;
              return;
            }
            if (((int) d2.High | (int) d2.Mid) == 0)
            {
              uint low = d2.Low;
              ulong num6 = ((ulong) d1.High << 32 | (ulong) d1.Mid) % (ulong) low << 32 | (ulong) d1.Low;
              d1.Low64 = num6 % (ulong) low;
              d1.High = 0U;
            }
            else
              goto label_22;
          }
          while (scale < 0);
          goto label_9;
label_22:
          Decimal.DecCalc.VarDecModFull(ref d1, ref d2, scale);
          return;
label_9:;
        }
      }

      private static unsafe void VarDecModFull(
        ref Decimal.DecCalc d1,
        ref Decimal.DecCalc d2,
        int scale)
      {
        uint num1 = d2.High;
        if (num1 == 0U)
          num1 = d2.Mid;
        int num2 = BitOperations.LeadingZeroCount(num1);
        Decimal.DecCalc.Buf28 buf28;
        Unsafe.SkipInit<Decimal.DecCalc.Buf28>(out buf28);
        buf28.Buf24.Low64 = d1.Low64 << num2;
        buf28.Buf24.Mid64 = (ulong) d1.Mid + ((ulong) d1.High << 32) >> 32 - num2;
        uint num3 = 3;
        for (; scale < 0; scale += 9)
        {
          uint b = scale <= -9 ? 1000000000U : Decimal.DecCalc.s_powers10[-scale];
          uint* numPtr = (uint*) &buf28;
          ulong num4 = Decimal.DecCalc.UInt32x32To64(buf28.Buf24.U0, b);
          buf28.Buf24.U0 = (uint) num4;
          for (int index = 1; (long) index <= (long) num3; ++index)
          {
            num4 = (num4 >> 32) + Decimal.DecCalc.UInt32x32To64(numPtr[index], b);
            numPtr[index] = (uint) num4;
          }
          if (num4 > (ulong) int.MaxValue)
            *(int*) ((IntPtr) numPtr + (IntPtr) ((long) ++num3 * 4L)) = (int) (uint) (num4 >> 32);
        }
        if (d2.High == 0U)
        {
          ulong den = d2.Low64 << num2;
          switch (num3)
          {
            case 4:
              int num5 = (int) Decimal.DecCalc.Div96By64(ref *(Decimal.DecCalc.Buf12*) &buf28.Buf24.U2, den);
              break;
            case 5:
              int num6 = (int) Decimal.DecCalc.Div96By64(ref *(Decimal.DecCalc.Buf12*) &buf28.Buf24.U3, den);
              goto case 4;
            case 6:
              int num7 = (int) Decimal.DecCalc.Div96By64(ref *(Decimal.DecCalc.Buf12*) &buf28.Buf24.U4, den);
              goto case 5;
          }
          int num8 = (int) Decimal.DecCalc.Div96By64(ref *(Decimal.DecCalc.Buf12*) &buf28.Buf24.U1, den);
          int num9 = (int) Decimal.DecCalc.Div96By64(ref *(Decimal.DecCalc.Buf12*) &buf28, den);
          d1.Low64 = buf28.Buf24.Low64 >> num2;
          d1.High = 0U;
        }
        else
        {
          Decimal.DecCalc.Buf12 bufDen;
          Unsafe.SkipInit<Decimal.DecCalc.Buf12>(out bufDen);
          bufDen.Low64 = d2.Low64 << num2;
          bufDen.U2 = (uint) ((ulong) d2.Mid + ((ulong) d2.High << 32) >> 32 - num2);
          switch (num3)
          {
            case 4:
              int num10 = (int) Decimal.DecCalc.Div128By96(ref *(Decimal.DecCalc.Buf16*) &buf28.Buf24.U1, ref bufDen);
              break;
            case 5:
              int num11 = (int) Decimal.DecCalc.Div128By96(ref *(Decimal.DecCalc.Buf16*) &buf28.Buf24.U2, ref bufDen);
              goto case 4;
            case 6:
              int num12 = (int) Decimal.DecCalc.Div128By96(ref *(Decimal.DecCalc.Buf16*) &buf28.Buf24.U3, ref bufDen);
              goto case 5;
          }
          int num13 = (int) Decimal.DecCalc.Div128By96(ref *(Decimal.DecCalc.Buf16*) &buf28, ref bufDen);
          d1.Low64 = (buf28.Buf24.Low64 >> num2) + (ulong) ((long) buf28.Buf24.U2 << 32 - num2 << 32);
          d1.High = buf28.Buf24.U2 >> num2;
        }
      }

      internal static void InternalRound(ref Decimal.DecCalc d, uint scale, MidpointRounding mode)
      {
        d.uflags -= scale << 16;
        uint num1 = 0;
        uint num2;
        uint num3;
        while (scale >= 9U)
        {
          scale -= 9U;
          uint uhi = d.uhi;
          if (uhi == 0U)
          {
            ulong low64 = d.Low64;
            ulong num4 = low64 / 1000000000UL;
            d.Low64 = num4;
            num2 = (uint) (low64 - num4 * 1000000000UL);
          }
          else
          {
            uint num5;
            d.uhi = num5 = uhi / 1000000000U;
            num2 = uhi - num5 * 1000000000U;
            uint umid = d.umid;
            if (((int) umid | (int) num2) != 0)
            {
              uint num6;
              d.umid = num6 = (uint) (((ulong) num2 << 32 | (ulong) umid) / 1000000000UL);
              num2 = umid - num6 * 1000000000U;
            }
            uint ulo = d.ulo;
            if (((int) ulo | (int) num2) != 0)
            {
              uint num7;
              d.ulo = num7 = (uint) (((ulong) num2 << 32 | (ulong) ulo) / 1000000000UL);
              num2 = ulo - num7 * 1000000000U;
            }
          }
          num3 = 1000000000U;
          if (scale != 0U)
            num1 |= num2;
          else
            goto label_19;
        }
        num3 = Decimal.DecCalc.s_powers10[(int) scale];
        uint uhi1 = d.uhi;
        if (uhi1 == 0U)
        {
          ulong low64 = d.Low64;
          if (low64 == 0UL)
          {
            if (mode <= MidpointRounding.ToZero)
              return;
            num2 = 0U;
          }
          else
          {
            ulong num8 = low64 / (ulong) num3;
            d.Low64 = num8;
            num2 = (uint) (low64 - num8 * (ulong) num3);
          }
        }
        else
        {
          uint num9;
          d.uhi = num9 = uhi1 / num3;
          num2 = uhi1 - num9 * num3;
          uint umid = d.umid;
          if (((int) umid | (int) num2) != 0)
          {
            uint num10;
            d.umid = num10 = (uint) (((ulong) num2 << 32 | (ulong) umid) / (ulong) num3);
            num2 = umid - num10 * num3;
          }
          uint ulo = d.ulo;
          if (((int) ulo | (int) num2) != 0)
          {
            uint num11;
            d.ulo = num11 = (uint) (((ulong) num2 << 32 | (ulong) ulo) / (ulong) num3);
            num2 = ulo - num11 * num3;
          }
        }
label_19:
        switch (mode)
        {
          case MidpointRounding.ToEven:
            uint num12 = num2 << 1;
            if (((int) num1 | (int) d.ulo & 1) != 0)
              ++num12;
            if (num3 >= num12)
              return;
            break;
          case MidpointRounding.AwayFromZero:
            uint num13 = num2 << 1;
            if (num3 > num13)
              return;
            break;
          case MidpointRounding.ToZero:
            return;
          case MidpointRounding.ToNegativeInfinity:
            if (((int) num2 | (int) num1) == 0 || !d.IsNegative)
              return;
            break;
          default:
            if (((int) num2 | (int) num1) == 0 || d.IsNegative)
              return;
            break;
        }
        if (++d.Low64 != 0UL)
          return;
        ++d.uhi;
      }

      internal static uint DecDivMod1E9(ref Decimal.DecCalc value)
      {
        ulong num1 = ((ulong) value.uhi << 32) + (ulong) value.umid;
        ulong num2 = num1 / 1000000000UL;
        value.uhi = (uint) (num2 >> 32);
        value.umid = (uint) num2;
        ulong num3 = (ulong) ((long) num1 - (long) ((uint) num2 * 1000000000U) << 32) + (ulong) value.ulo;
        uint num4 = (uint) (num3 / 1000000000UL);
        value.ulo = num4;
        return (uint) num3 - num4 * 1000000000U;
      }

      private struct PowerOvfl
      {
        public readonly uint Hi;
        public readonly ulong MidLo;

        public PowerOvfl(uint hi, uint mid, uint lo)
        {
          this.Hi = hi;
          this.MidLo = ((ulong) mid << 32) + (ulong) lo;
        }
      }

      [StructLayout(LayoutKind.Explicit)]
      private struct Buf12
      {
        [FieldOffset(0)]
        public uint U0;
        [FieldOffset(4)]
        public uint U1;
        [FieldOffset(8)]
        public uint U2;
        [FieldOffset(0)]
        private ulong ulo64LE;
        [FieldOffset(4)]
        private ulong uhigh64LE;

        public ulong Low64
        {
          get => this.ulo64LE;
          set => this.ulo64LE = value;
        }

        public ulong High64
        {
          get => this.uhigh64LE;
          set => this.uhigh64LE = value;
        }
      }

      [StructLayout(LayoutKind.Explicit)]
      private struct Buf16
      {
        [FieldOffset(0)]
        public uint U0;
        [FieldOffset(4)]
        public uint U1;
        [FieldOffset(8)]
        public uint U2;
        [FieldOffset(12)]
        public uint U3;
        [FieldOffset(0)]
        private ulong ulo64LE;
        [FieldOffset(8)]
        private ulong uhigh64LE;

        public ulong Low64
        {
          get => this.ulo64LE;
          set => this.ulo64LE = value;
        }

        public ulong High64
        {
          get => this.uhigh64LE;
          set => this.uhigh64LE = value;
        }
      }

      [StructLayout(LayoutKind.Explicit)]
      private struct Buf24
      {
        [FieldOffset(0)]
        public uint U0;
        [FieldOffset(4)]
        public uint U1;
        [FieldOffset(8)]
        public uint U2;
        [FieldOffset(12)]
        public uint U3;
        [FieldOffset(16)]
        public uint U4;
        [FieldOffset(20)]
        public uint U5;
        [FieldOffset(0)]
        private ulong ulo64LE;
        [FieldOffset(8)]
        private ulong umid64LE;
        [FieldOffset(16)]
        private ulong uhigh64LE;

        public ulong Low64
        {
          get => this.ulo64LE;
          set => this.ulo64LE = value;
        }

        public ulong Mid64
        {
          set => this.umid64LE = value;
        }

        public ulong High64
        {
          set => this.uhigh64LE = value;
        }
      }

      private struct Buf28
      {
        public Decimal.DecCalc.Buf24 Buf24;
        public uint U6;
      }
    }
  }
}
