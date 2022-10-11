// Decompiled with JetBrains decompiler
// Type: System.Convert
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Converts a base data type to another base data type.</summary>
  public static class Convert
  {

    #nullable disable
    internal static readonly Type[] ConvertTypes = new Type[19]
    {
      typeof (Empty),
      typeof (object),
      typeof (System.DBNull),
      typeof (bool),
      typeof (char),
      typeof (sbyte),
      typeof (byte),
      typeof (short),
      typeof (ushort),
      typeof (int),
      typeof (uint),
      typeof (long),
      typeof (ulong),
      typeof (float),
      typeof (double),
      typeof (Decimal),
      typeof (DateTime),
      typeof (object),
      typeof (string)
    };
    private static readonly Type EnumType = typeof (Enum);
    internal static readonly char[] base64Table = new char[65]
    {
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f',
      'g',
      'h',
      'i',
      'j',
      'k',
      'l',
      'm',
      'n',
      'o',
      'p',
      'q',
      'r',
      's',
      't',
      'u',
      'v',
      'w',
      'x',
      'y',
      'z',
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      '+',
      '/',
      '='
    };

    #nullable enable
    /// <summary>A constant that represents a database column that is absent of data; that is, database null.</summary>
    public static readonly object DBNull = (object) System.DBNull.Value;


    #nullable disable
    private static bool TryDecodeFromUtf16(
      ReadOnlySpan<char> utf16,
      Span<byte> bytes,
      out int consumed,
      out int written)
    {
      ref char local1 = ref MemoryMarshal.GetReference<char>(utf16);
      ref byte local2 = ref MemoryMarshal.GetReference<byte>(bytes);
      int num1 = utf16.Length & -4;
      int length = bytes.Length;
      int elementOffset1 = 0;
      int elementOffset2 = 0;
      if (utf16.Length != 0)
      {
        ref sbyte local3 = ref MemoryMarshal.GetReference<sbyte>(Convert.DecodingMap);
        int num2;
        for (num2 = length < (num1 >> 2) * 3 ? length / 3 * 4 : num1 - 4; elementOffset1 < num2; elementOffset1 += 4)
        {
          int num3 = Convert.Decode(ref Unsafe.Add<char>(ref local1, elementOffset1), ref local3);
          if (num3 >= 0)
          {
            Convert.WriteThreeLowOrderBytes(ref Unsafe.Add<byte>(ref local2, elementOffset2), num3);
            elementOffset2 += 3;
          }
          else
            goto label_17;
        }
        if (num2 == num1 - 4 && elementOffset1 != num1)
        {
          int elementOffset3 = (int) Unsafe.Add<char>(ref local1, num1 - 4);
          int elementOffset4 = (int) Unsafe.Add<char>(ref local1, num1 - 3);
          int elementOffset5 = (int) Unsafe.Add<char>(ref local1, num1 - 2);
          int elementOffset6 = (int) Unsafe.Add<char>(ref local1, num1 - 1);
          if (((long) (elementOffset3 | elementOffset4 | elementOffset5 | elementOffset6) & 4294967040L) == 0L)
          {
            int num4 = (int) Unsafe.Add<sbyte>(ref local3, elementOffset3) << 18 | (int) Unsafe.Add<sbyte>(ref local3, elementOffset4) << 12;
            if (elementOffset6 != 61)
            {
              int num5 = (int) Unsafe.Add<sbyte>(ref local3, elementOffset5);
              int num6 = (int) Unsafe.Add<sbyte>(ref local3, elementOffset6);
              int num7 = num5 << 6;
              int num8 = num4 | num6 | num7;
              if (num8 >= 0 && elementOffset2 <= length - 3)
              {
                Convert.WriteThreeLowOrderBytes(ref Unsafe.Add<byte>(ref local2, elementOffset2), num8);
                elementOffset2 += 3;
              }
              else
                goto label_17;
            }
            else if (elementOffset5 != 61)
            {
              int num9 = (int) Unsafe.Add<sbyte>(ref local3, elementOffset5) << 6;
              int num10 = num4 | num9;
              if (num10 >= 0 && elementOffset2 <= length - 2)
              {
                Unsafe.Add<byte>(ref local2, elementOffset2) = (byte) (num10 >> 16);
                Unsafe.Add<byte>(ref local2, elementOffset2 + 1) = (byte) (num10 >> 8);
                elementOffset2 += 2;
              }
              else
                goto label_17;
            }
            else if (num4 >= 0 && elementOffset2 <= length - 1)
            {
              Unsafe.Add<byte>(ref local2, elementOffset2) = (byte) (num4 >> 16);
              ++elementOffset2;
            }
            else
              goto label_17;
            elementOffset1 += 4;
            if (num1 == utf16.Length)
              goto label_16;
          }
        }
label_17:
        consumed = elementOffset1;
        written = elementOffset2;
        return false;
      }
label_16:
      consumed = elementOffset1;
      written = elementOffset2;
      return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Decode(ref char encodedChars, ref sbyte decodingMap)
    {
      int elementOffset1 = (int) encodedChars;
      int elementOffset2 = (int) Unsafe.Add<char>(ref encodedChars, 1);
      int elementOffset3 = (int) Unsafe.Add<char>(ref encodedChars, 2);
      int elementOffset4 = (int) Unsafe.Add<char>(ref encodedChars, 3);
      if (((long) (elementOffset1 | elementOffset2 | elementOffset3 | elementOffset4) & 4294967040L) != 0L)
        return -1;
      int num1 = (int) Unsafe.Add<sbyte>(ref decodingMap, elementOffset1);
      int num2 = (int) Unsafe.Add<sbyte>(ref decodingMap, elementOffset2);
      int num3 = (int) Unsafe.Add<sbyte>(ref decodingMap, elementOffset3);
      int num4 = (int) Unsafe.Add<sbyte>(ref decodingMap, elementOffset4);
      int num5 = num1 << 18;
      int num6 = num2 << 12;
      int num7 = num3 << 6;
      return num5 | num4 | num6 | num7;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteThreeLowOrderBytes(ref byte destination, int value)
    {
      destination = (byte) (value >> 16);
      Unsafe.Add<byte>(ref destination, 1) = (byte) (value >> 8);
      Unsafe.Add<byte>(ref destination, 2) = (byte) value;
    }


    #nullable enable
    private static unsafe ReadOnlySpan<sbyte> DecodingMap => new ReadOnlySpan<sbyte>((void*) &\u003CPrivateImplementationDetails\u003E.F2830F044682E33B39018B5912634835B641562914E192CA66C654F5E4492FA8, 256);

    /// <summary>Returns the <see cref="T:System.TypeCode" /> for the specified object.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <returns>The <see cref="T:System.TypeCode" /> for <paramref name="value" />, or <see cref="F:System.TypeCode.Empty" /> if <paramref name="value" /> is <see langword="null" />.</returns>
    public static TypeCode GetTypeCode(object? value)
    {
      if (value == null)
        return TypeCode.Empty;
      return value is IConvertible convertible ? convertible.GetTypeCode() : TypeCode.Object;
    }

    /// <summary>Returns an indication whether the specified object is of type <see cref="T:System.DBNull" />.</summary>
    /// <param name="value">An object.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is of type <see cref="T:System.DBNull" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDBNull([NotNullWhen(true)] object? value)
    {
      if (value == System.DBNull.Value)
        return true;
      return value is IConvertible convertible && convertible.GetTypeCode() == TypeCode.DBNull;
    }

    /// <summary>Returns an object of the specified type whose value is equivalent to the specified object.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="typeCode">The type of object to return.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see langword="null" /> and <paramref name="typeCode" /> specifies a value type.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in a format recognized by the <paramref name="typeCode" /> type.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is out of the range of the <paramref name="typeCode" /> type.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="typeCode" /> is invalid.</exception>
    /// <returns>An object whose underlying type is <paramref name="typeCode" /> and whose value is equivalent to <paramref name="value" />.
    /// 
    /// -or-
    /// 
    /// A null reference (<see langword="Nothing" /> in Visual Basic), if <paramref name="value" /> is <see langword="null" /> and <paramref name="typeCode" /> is <see cref="F:System.TypeCode.Empty" />, <see cref="F:System.TypeCode.String" />, or <see cref="F:System.TypeCode.Object" />.</returns>
    [return: NotNullIfNotNull("value")]
    public static object? ChangeType(object? value, TypeCode typeCode) => Convert.ChangeType(value, typeCode, (IFormatProvider) CultureInfo.CurrentCulture);

    /// <summary>Returns an object of the specified type whose value is equivalent to the specified object. A parameter supplies culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="typeCode">The type of object to return.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see langword="null" /> and <paramref name="typeCode" /> specifies a value type.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in a format for the <paramref name="typeCode" /> type recognized by <paramref name="provider" />.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is out of the range of the <paramref name="typeCode" /> type.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="typeCode" /> is invalid.</exception>
    /// <returns>An object whose underlying type is <paramref name="typeCode" /> and whose value is equivalent to <paramref name="value" />.
    /// 
    /// -or-
    /// 
    /// A null reference (<see langword="Nothing" /> in Visual Basic), if <paramref name="value" /> is <see langword="null" /> and <paramref name="typeCode" /> is <see cref="F:System.TypeCode.Empty" />, <see cref="F:System.TypeCode.String" />, or <see cref="F:System.TypeCode.Object" />.</returns>
    [return: NotNullIfNotNull("value")]
    public static object? ChangeType(object? value, TypeCode typeCode, IFormatProvider? provider)
    {
      if (value == null && (typeCode == TypeCode.Empty || typeCode == TypeCode.String || typeCode == TypeCode.Object))
        return (object) null;
      if (!(value is IConvertible convertible))
        throw new InvalidCastException(SR.InvalidCast_IConvertible);
      switch (typeCode)
      {
        case TypeCode.Empty:
          throw new InvalidCastException(SR.InvalidCast_Empty);
        case TypeCode.Object:
          return value;
        case TypeCode.DBNull:
          throw new InvalidCastException(SR.InvalidCast_DBNull);
        case TypeCode.Boolean:
          return (object) convertible.ToBoolean(provider);
        case TypeCode.Char:
          return (object) convertible.ToChar(provider);
        case TypeCode.SByte:
          return (object) convertible.ToSByte(provider);
        case TypeCode.Byte:
          return (object) convertible.ToByte(provider);
        case TypeCode.Int16:
          return (object) convertible.ToInt16(provider);
        case TypeCode.UInt16:
          return (object) convertible.ToUInt16(provider);
        case TypeCode.Int32:
          return (object) convertible.ToInt32(provider);
        case TypeCode.UInt32:
          return (object) convertible.ToUInt32(provider);
        case TypeCode.Int64:
          return (object) convertible.ToInt64(provider);
        case TypeCode.UInt64:
          return (object) convertible.ToUInt64(provider);
        case TypeCode.Single:
          return (object) convertible.ToSingle(provider);
        case TypeCode.Double:
          return (object) convertible.ToDouble(provider);
        case TypeCode.Decimal:
          return (object) convertible.ToDecimal(provider);
        case TypeCode.DateTime:
          return (object) convertible.ToDateTime(provider);
        case TypeCode.String:
          return (object) convertible.ToString(provider);
        default:
          throw new ArgumentException(SR.Arg_UnknownTypeCode);
      }
    }


    #nullable disable
    internal static object DefaultToType(
      IConvertible value,
      Type targetType,
      IFormatProvider provider)
    {
      if (targetType == (Type) null)
        throw new ArgumentNullException(nameof (targetType));
      if ((object) value.GetType() == (object) targetType)
        return (object) value;
      if ((object) targetType == (object) Convert.ConvertTypes[3])
        return (object) value.ToBoolean(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[4])
        return (object) value.ToChar(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[5])
        return (object) value.ToSByte(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[6])
        return (object) value.ToByte(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[7])
        return (object) value.ToInt16(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[8])
        return (object) value.ToUInt16(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[9])
        return (object) value.ToInt32(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[10])
        return (object) value.ToUInt32(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[11])
        return (object) value.ToInt64(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[12])
        return (object) value.ToUInt64(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[13])
        return (object) value.ToSingle(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[14])
        return (object) value.ToDouble(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[15])
        return (object) value.ToDecimal(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[16])
        return (object) value.ToDateTime(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[18])
        return (object) value.ToString(provider);
      if ((object) targetType == (object) Convert.ConvertTypes[1])
        return (object) value;
      if ((object) targetType == (object) Convert.EnumType)
        return (object) (Enum) value;
      if ((object) targetType == (object) Convert.ConvertTypes[2])
        throw new InvalidCastException(SR.InvalidCast_DBNull);
      if ((object) targetType == (object) Convert.ConvertTypes[0])
        throw new InvalidCastException(SR.InvalidCast_Empty);
      throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) value.GetType().FullName, (object) targetType.FullName));
    }


    #nullable enable
    /// <summary>Returns an object of the specified type and whose value is equivalent to the specified object.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="conversionType">The type of object to return.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see langword="null" /> and <paramref name="conversionType" /> is a value type.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in a format recognized by <paramref name="conversionType" />.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is out of the range of <paramref name="conversionType" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="conversionType" /> is <see langword="null" />.</exception>
    /// <returns>An object whose type is <paramref name="conversionType" /> and whose value is equivalent to <paramref name="value" />.
    /// 
    /// -or-
    /// 
    /// A null reference (<see langword="Nothing" /> in Visual Basic), if <paramref name="value" /> is <see langword="null" /> and <paramref name="conversionType" /> is not a value type.</returns>
    [return: NotNullIfNotNull("value")]
    public static object? ChangeType(object? value, Type conversionType) => Convert.ChangeType(value, conversionType, (IFormatProvider) CultureInfo.CurrentCulture);

    /// <summary>Returns an object of the specified type whose value is equivalent to the specified object. A parameter supplies culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="conversionType">The type of object to return.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is <see langword="null" /> and <paramref name="conversionType" /> is a value type.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in a format for <paramref name="conversionType" /> recognized by <paramref name="provider" />.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is out of the range of <paramref name="conversionType" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="conversionType" /> is <see langword="null" />.</exception>
    /// <returns>An object whose type is <paramref name="conversionType" /> and whose value is equivalent to <paramref name="value" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, if the <see cref="T:System.Type" /> of <paramref name="value" /> and <paramref name="conversionType" /> are equal.
    /// 
    /// -or-
    /// 
    /// A null reference (<see langword="Nothing" /> in Visual Basic), if <paramref name="value" /> is <see langword="null" /> and <paramref name="conversionType" /> is not a value type.</returns>
    [return: NotNullIfNotNull("value")]
    public static object? ChangeType(object? value, Type conversionType, IFormatProvider? provider)
    {
      if ((object) conversionType == null)
        throw new ArgumentNullException(nameof (conversionType));
      if (value == null)
      {
        if (conversionType.IsValueType)
          throw new InvalidCastException(SR.InvalidCast_CannotCastNullToValueType);
        return (object) null;
      }
      if (!(value is IConvertible convertible))
        return value.GetType() == conversionType ? value : throw new InvalidCastException(SR.InvalidCast_IConvertible);
      if ((object) conversionType == (object) Convert.ConvertTypes[3])
        return (object) convertible.ToBoolean(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[4])
        return (object) convertible.ToChar(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[5])
        return (object) convertible.ToSByte(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[6])
        return (object) convertible.ToByte(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[7])
        return (object) convertible.ToInt16(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[8])
        return (object) convertible.ToUInt16(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[9])
        return (object) convertible.ToInt32(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[10])
        return (object) convertible.ToUInt32(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[11])
        return (object) convertible.ToInt64(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[12])
        return (object) convertible.ToUInt64(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[13])
        return (object) convertible.ToSingle(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[14])
        return (object) convertible.ToDouble(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[15])
        return (object) convertible.ToDecimal(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[16])
        return (object) convertible.ToDateTime(provider);
      if ((object) conversionType == (object) Convert.ConvertTypes[18])
        return (object) convertible.ToString(provider);
      return (object) conversionType == (object) Convert.ConvertTypes[1] ? value : convertible.ToType(conversionType, provider);
    }

    [DoesNotReturn]
    private static void ThrowCharOverflowException() => throw new OverflowException(SR.Overflow_Char);

    [DoesNotReturn]
    private static void ThrowByteOverflowException() => throw new OverflowException(SR.Overflow_Byte);

    [DoesNotReturn]
    private static void ThrowSByteOverflowException() => throw new OverflowException(SR.Overflow_SByte);

    [DoesNotReturn]
    private static void ThrowInt16OverflowException() => throw new OverflowException(SR.Overflow_Int16);

    [DoesNotReturn]
    private static void ThrowUInt16OverflowException() => throw new OverflowException(SR.Overflow_UInt16);

    [DoesNotReturn]
    private static void ThrowInt32OverflowException() => throw new OverflowException(SR.Overflow_Int32);

    [DoesNotReturn]
    private static void ThrowUInt32OverflowException() => throw new OverflowException(SR.Overflow_UInt32);

    [DoesNotReturn]
    private static void ThrowInt64OverflowException() => throw new OverflowException(SR.Overflow_Int64);

    [DoesNotReturn]
    private static void ThrowUInt64OverflowException() => throw new OverflowException(SR.Overflow_UInt64);

    /// <summary>Converts the value of a specified object to an equivalent Boolean value.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is a string that does not equal <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion of <paramref name="value" /> to a <see cref="T:System.Boolean" /> is not supported.</exception>
    /// <returns>
    /// <see langword="true" /> or <see langword="false" />, which reflects the value returned by invoking the <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" /> method for the underlying type of <paramref name="value" />. If <paramref name="value" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
    public static bool ToBoolean([NotNullWhen(true)] object? value) => value != null && ((IConvertible) value).ToBoolean((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to an equivalent Boolean value, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is a string that does not equal <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion of <paramref name="value" /> to a <see cref="T:System.Boolean" /> is not supported.</exception>
    /// <returns>
    /// <see langword="true" /> or <see langword="false" />, which reflects the value returned by invoking the <see cref="M:System.IConvertible.ToBoolean(System.IFormatProvider)" /> method for the underlying type of <paramref name="value" />. If <paramref name="value" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
    public static bool ToBoolean([NotNullWhen(true)] object? value, IFormatProvider? provider) => value != null && ((IConvertible) value).ToBoolean(provider);

    /// <summary>Returns the specified Boolean value; no actual conversion is performed.</summary>
    /// <param name="value">The Boolean value to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static bool ToBoolean(bool value) => value;

    /// <summary>Converts the value of the specified 8-bit signed integer to an equivalent Boolean value.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public static bool ToBoolean(sbyte value) => value != (sbyte) 0;

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static bool ToBoolean(char value) => ((IConvertible) value).ToBoolean((IFormatProvider) null);

    /// <summary>Converts the value of the specified 8-bit unsigned integer to an equivalent Boolean value.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    public static bool ToBoolean(byte value) => value > (byte) 0;

    /// <summary>Converts the value of the specified 16-bit signed integer to an equivalent Boolean value.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    public static bool ToBoolean(short value) => value != (short) 0;

    /// <summary>Converts the value of the specified 16-bit unsigned integer to an equivalent Boolean value.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public static bool ToBoolean(ushort value) => value > (ushort) 0;

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent Boolean value.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    public static bool ToBoolean(int value) => value != 0;

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent Boolean value.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public static bool ToBoolean(uint value) => value > 0U;

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent Boolean value.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    public static bool ToBoolean(long value) => value != 0L;

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent Boolean value.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    [CLSCompliant(false)]
    public static bool ToBoolean(ulong value) => value > 0UL;

    /// <summary>Converts the specified string representation of a logical value to its Boolean equivalent.</summary>
    /// <param name="value">A string that contains the value of either <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not equal to <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> equals <see cref="F:System.Boolean.TrueString" />, or <see langword="false" /> if <paramref name="value" /> equals <see cref="F:System.Boolean.FalseString" /> or <see langword="null" />.</returns>
    public static bool ToBoolean([NotNullWhen(true)] string? value) => value != null && bool.Parse(value);

    /// <summary>Converts the specified string representation of a logical value to its Boolean equivalent, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the value of either <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information. This parameter is ignored.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not equal to <see cref="F:System.Boolean.TrueString" /> or <see cref="F:System.Boolean.FalseString" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> equals <see cref="F:System.Boolean.TrueString" />, or <see langword="false" /> if <paramref name="value" /> equals <see cref="F:System.Boolean.FalseString" /> or <see langword="null" />.</returns>
    public static bool ToBoolean([NotNullWhen(true)] string? value, IFormatProvider? provider) => value != null && bool.Parse(value);

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent Boolean value.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    public static bool ToBoolean(float value) => (double) value != 0.0;

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent Boolean value.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    public static bool ToBoolean(double value) => value != 0.0;

    /// <summary>Converts the value of the specified decimal number to an equivalent Boolean value.</summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="value" /> is not zero; otherwise, <see langword="false" />.</returns>
    public static bool ToBoolean(Decimal value) => value != 0M;

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static bool ToBoolean(DateTime value) => ((IConvertible) value).ToBoolean((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a Unicode character.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is a null string.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion of <paramref name="value" /> to a <see cref="T:System.Char" /> is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />.</exception>
    /// <returns>A Unicode character that is equivalent to value, or <see cref="F:System.Char.MinValue" /> if <paramref name="value" /> is <see langword="null" />.</returns>
    public static char ToChar(object? value) => value != null ? ((IConvertible) value).ToChar((IFormatProvider) null) : char.MinValue;

    /// <summary>Converts the value of the specified object to its equivalent Unicode character, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is a null string.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion of <paramref name="value" /> to a <see cref="T:System.Char" /> is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />.</exception>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />, or <see cref="F:System.Char.MinValue" /> if <paramref name="value" /> is <see langword="null" />.</returns>
    public static char ToChar(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToChar(provider) : char.MinValue;

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static char ToChar(bool value) => ((IConvertible) value).ToChar((IFormatProvider) null);

    /// <summary>Returns the specified Unicode character value; no actual conversion is performed.</summary>
    /// <param name="value">The Unicode character to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static char ToChar(char value) => value;

    /// <summary>Converts the value of the specified 8-bit signed integer to its equivalent Unicode character.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" />.</exception>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static char ToChar(sbyte value)
    {
      if (value < (sbyte) 0)
        Convert.ThrowCharOverflowException();
      return (char) value;
    }

    /// <summary>Converts the value of the specified 8-bit unsigned integer to its equivalent Unicode character.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
    public static char ToChar(byte value) => (char) value;

    /// <summary>Converts the value of the specified 16-bit signed integer to its equivalent Unicode character.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" />.</exception>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
    public static char ToChar(short value)
    {
      if (value < (short) 0)
        Convert.ThrowCharOverflowException();
      return (char) value;
    }

    /// <summary>Converts the value of the specified 16-bit unsigned integer to its equivalent Unicode character.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static char ToChar(ushort value) => (char) value;

    /// <summary>Converts the value of the specified 32-bit signed integer to its equivalent Unicode character.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />.</exception>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
    public static char ToChar(int value) => Convert.ToChar((uint) value);

    /// <summary>Converts the value of the specified 32-bit unsigned integer to its equivalent Unicode character.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Char.MaxValue" />.</exception>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static char ToChar(uint value)
    {
      if (value > (uint) ushort.MaxValue)
        Convert.ThrowCharOverflowException();
      return (char) value;
    }

    /// <summary>Converts the value of the specified 64-bit signed integer to its equivalent Unicode character.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Char.MinValue" /> or greater than <see cref="F:System.Char.MaxValue" />.</exception>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
    public static char ToChar(long value) => Convert.ToChar((ulong) value);

    /// <summary>Converts the value of the specified 64-bit unsigned integer to its equivalent Unicode character.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Char.MaxValue" />.</exception>
    /// <returns>A Unicode character that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static char ToChar(ulong value)
    {
      if (value > (ulong) ushort.MaxValue)
        Convert.ThrowCharOverflowException();
      return (char) value;
    }

    /// <summary>Converts the first character of a specified string to a Unicode character.</summary>
    /// <param name="value">A string of length 1.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The length of <paramref name="value" /> is not 1.</exception>
    /// <returns>A Unicode character that is equivalent to the first and only character in <paramref name="value" />.</returns>
    public static char ToChar(string value) => Convert.ToChar(value, (IFormatProvider) null);

    /// <summary>Converts the first character of a specified string to a Unicode character, using specified culture-specific formatting information.</summary>
    /// <param name="value">A string of length 1 or <see langword="null" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information. This parameter is ignored.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The length of <paramref name="value" /> is not 1.</exception>
    /// <returns>A Unicode character that is equivalent to the first and only character in <paramref name="value" />.</returns>
    public static char ToChar(string value, IFormatProvider? provider)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      return value.Length == 1 ? value[0] : throw new FormatException(SR.Format_NeedSingleChar);
    }

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static char ToChar(float value) => ((IConvertible) value).ToChar((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static char ToChar(double value) => ((IConvertible) value).ToChar((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static char ToChar(Decimal value) => ((IConvertible) value).ToChar((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static char ToChar(DateTime value) => ((IConvertible) value).ToChar((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to an 8-bit signed integer.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(object? value) => value != null ? ((IConvertible) value).ToSByte((IFormatProvider) null) : (sbyte) 0;

    /// <summary>Converts the value of the specified object to an 8-bit signed integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToSByte(provider) : (sbyte) 0;

    /// <summary>Converts the specified Boolean value to the equivalent 8-bit signed integer.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(bool value) => !value ? (sbyte) 0 : (sbyte) 1;

    /// <summary>Returns the specified 8-bit signed integer; no actual conversion is performed.</summary>
    /// <param name="value">The 8-bit signed integer to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(sbyte value) => value;

    /// <summary>Converts the value of the specified Unicode character to the equivalent 8-bit signed integer.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(char value)
    {
      if (value > '\u007F')
        Convert.ThrowSByteOverflowException();
      return (sbyte) value;
    }

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 8-bit signed integer.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(byte value)
    {
      if (value > (byte) 127)
        Convert.ThrowSByteOverflowException();
      return (sbyte) value;
    }

    /// <summary>Converts the value of the specified 16-bit signed integer to the equivalent 8-bit signed integer.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(short value)
    {
      if (value < (short) sbyte.MinValue || value > (short) sbyte.MaxValue)
        Convert.ThrowSByteOverflowException();
      return (sbyte) value;
    }

    /// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 8-bit signed integer.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(ushort value)
    {
      if (value > (ushort) sbyte.MaxValue)
        Convert.ThrowSByteOverflowException();
      return (sbyte) value;
    }

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(int value)
    {
      if (value < (int) sbyte.MinValue || value > (int) sbyte.MaxValue)
        Convert.ThrowSByteOverflowException();
      return (sbyte) value;
    }

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(uint value)
    {
      if (value > (uint) sbyte.MaxValue)
        Convert.ThrowSByteOverflowException();
      return (sbyte) value;
    }

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(long value)
    {
      if (value < (long) sbyte.MinValue || value > (long) sbyte.MaxValue)
        Convert.ThrowSByteOverflowException();
      return (sbyte) value;
    }

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(ulong value)
    {
      if (value > (ulong) sbyte.MaxValue)
        Convert.ThrowSByteOverflowException();
      return (sbyte) value;
    }

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 8-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(float value) => Convert.ToSByte((double) value);

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 8-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(double value) => Convert.ToSByte(Convert.ToInt32(value));

    /// <summary>Converts the value of the specified decimal number to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.SByte.MaxValue" /> or less than <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 8-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(Decimal value) => Decimal.ToSByte(Decimal.Round(value, 0));

    /// <summary>Converts the specified string representation of a number to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if value is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(string? value) => value == null ? (sbyte) 0 : sbyte.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent 8-bit signed integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(string value, IFormatProvider? provider) => sbyte.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(DateTime value) => ((IConvertible) value).ToSByte((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to an 8-bit unsigned integer.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in the property format for a <see cref="T:System.Byte" /> value.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />.
    /// 
    /// -or-
    /// 
    /// Conversion from <paramref name="value" /> to the <see cref="T:System.Byte" /> type is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static byte ToByte(object? value) => value != null ? ((IConvertible) value).ToByte((IFormatProvider) null) : (byte) 0;

    /// <summary>Converts the value of the specified object to an 8-bit unsigned integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in the property format for a <see cref="T:System.Byte" /> value.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />.
    /// 
    /// -or-
    /// 
    /// Conversion from <paramref name="value" /> to the <see cref="T:System.Byte" /> type is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static byte ToByte(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToByte(provider) : (byte) 0;

    /// <summary>Converts the specified Boolean value to the equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    public static byte ToByte(bool value) => !value ? (byte) 0 : (byte) 1;

    /// <summary>Returns the specified 8-bit unsigned integer; no actual conversion is performed.</summary>
    /// <param name="value">The 8-bit unsigned integer to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static byte ToByte(byte value) => value;

    /// <summary>Converts the value of the specified Unicode character to the equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    public static byte ToByte(char value)
    {
      if (value > 'ÿ')
        Convert.ThrowByteOverflowException();
      return (byte) value;
    }

    /// <summary>Converts the value of the specified 8-bit signed integer to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The 8-bit signed integer to be converted.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static byte ToByte(sbyte value)
    {
      if (value < (sbyte) 0)
        Convert.ThrowByteOverflowException();
      return (byte) value;
    }

    /// <summary>Converts the value of the specified 16-bit signed integer to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    public static byte ToByte(short value)
    {
      if ((uint) value > (uint) byte.MaxValue)
        Convert.ThrowByteOverflowException();
      return (byte) value;
    }

    /// <summary>Converts the value of the specified 16-bit unsigned integer to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static byte ToByte(ushort value)
    {
      if (value > (ushort) byte.MaxValue)
        Convert.ThrowByteOverflowException();
      return (byte) value;
    }

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    public static byte ToByte(int value) => Convert.ToByte((uint) value);

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static byte ToByte(uint value)
    {
      if (value > (uint) byte.MaxValue)
        Convert.ThrowByteOverflowException();
      return (byte) value;
    }

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    public static byte ToByte(long value) => Convert.ToByte((ulong) value);

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static byte ToByte(ulong value)
    {
      if (value > (ulong) byte.MaxValue)
        Convert.ThrowByteOverflowException();
      return (byte) value;
    }

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">A single-precision floating-point number.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" /> or less than <see cref="F:System.Byte.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 8-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static byte ToByte(float value) => Convert.ToByte((double) value);

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" /> or less than <see cref="F:System.Byte.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 8-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static byte ToByte(double value) => Convert.ToByte(Convert.ToInt32(value));

    /// <summary>Converts the value of the specified decimal number to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">The number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Byte.MaxValue" /> or less than <see cref="F:System.Byte.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 8-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static byte ToByte(Decimal value) => Decimal.ToByte(Decimal.Round(value, 0));

    /// <summary>Converts the specified string representation of a number to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static byte ToByte(string? value) => value == null ? (byte) 0 : byte.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent 8-bit unsigned integer, using specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static byte ToByte(string? value, IFormatProvider? provider) => value == null ? (byte) 0 : byte.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static byte ToByte(DateTime value) => ((IConvertible) value).ToByte((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a 16-bit signed integer.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format for an <see cref="T:System.Int16" /> type.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static short ToInt16(object? value) => value != null ? ((IConvertible) value).ToInt16((IFormatProvider) null) : (short) 0;

    /// <summary>Converts the value of the specified object to a 16-bit signed integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format for an <see cref="T:System.Int16" /> type.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static short ToInt16(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToInt16(provider) : (short) 0;

    /// <summary>Converts the specified Boolean value to the equivalent 16-bit signed integer.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    public static short ToInt16(bool value) => !value ? (short) 0 : (short) 1;

    /// <summary>Converts the value of the specified Unicode character to the equivalent 16-bit signed integer.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static short ToInt16(char value)
    {
      if (value > '翿')
        Convert.ThrowInt16OverflowException();
      return (short) value;
    }

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 16-bit signed integer.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>A 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static short ToInt16(sbyte value) => (short) value;

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 16-bit signed integer.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static short ToInt16(byte value) => (short) value;

    /// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 16-bit signed integer.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static short ToInt16(ushort value)
    {
      if (value > (ushort) short.MaxValue)
        Convert.ThrowInt16OverflowException();
      return (short) value;
    }

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />.</exception>
    /// <returns>The 16-bit signed integer equivalent of <paramref name="value" />.</returns>
    public static short ToInt16(int value)
    {
      if (value < (int) short.MinValue || value > (int) short.MaxValue)
        Convert.ThrowInt16OverflowException();
      return (short) value;
    }

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static short ToInt16(uint value)
    {
      if (value > (uint) short.MaxValue)
        Convert.ThrowInt16OverflowException();
      return (short) value;
    }

    /// <summary>Returns the specified 16-bit signed integer; no actual conversion is performed.</summary>
    /// <param name="value">The 16-bit signed integer to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static short ToInt16(short value) => value;

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static short ToInt16(long value)
    {
      if (value < (long) short.MinValue || value > (long) short.MaxValue)
        Convert.ThrowInt16OverflowException();
      return (short) value;
    }

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static short ToInt16(ulong value)
    {
      if (value > (ulong) short.MaxValue)
        Convert.ThrowInt16OverflowException();
      return (short) value;
    }

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 16-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static short ToInt16(float value) => Convert.ToInt16((double) value);

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 16-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static short ToInt16(double value) => Convert.ToInt16(Convert.ToInt32(value));

    /// <summary>Converts the value of the specified decimal number to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int16.MaxValue" /> or less than <see cref="F:System.Int16.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 16-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static short ToInt16(Decimal value) => Decimal.ToInt16(Decimal.Round(value, 0));

    /// <summary>Converts the specified string representation of a number to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static short ToInt16(string? value) => value == null ? (short) 0 : short.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent 16-bit signed integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static short ToInt16(string? value, IFormatProvider? provider) => value == null ? (short) 0 : short.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static short ToInt16(DateTime value) => ((IConvertible) value).ToInt16((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a 16-bit unsigned integer.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the  <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(object? value) => value != null ? ((IConvertible) value).ToUInt16((IFormatProvider) null) : (ushort) 0;

    /// <summary>Converts the value of the specified object to a 16-bit unsigned integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the  <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToUInt16(provider) : (ushort) 0;

    /// <summary>Converts the specified Boolean value to the equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(bool value) => !value ? (ushort) 0 : (ushort) 1;

    /// <summary>Converts the value of the specified Unicode character to the equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <returns>The 16-bit unsigned integer equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(char value) => (ushort) value;

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(sbyte value)
    {
      if (value < (sbyte) 0)
        Convert.ThrowUInt16OverflowException();
      return (ushort) value;
    }

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(byte value) => (ushort) value;

    /// <summary>Converts the value of the specified 16-bit signed integer to the equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(short value)
    {
      if (value < (short) 0)
        Convert.ThrowUInt16OverflowException();
      return (ushort) value;
    }

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(int value) => Convert.ToUInt16((uint) value);

    /// <summary>Returns the specified 16-bit unsigned integer; no actual conversion is performed.</summary>
    /// <param name="value">The 16-bit unsigned integer to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(ushort value) => value;

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(uint value)
    {
      if (value > (uint) ushort.MaxValue)
        Convert.ThrowUInt16OverflowException();
      return (ushort) value;
    }

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(long value) => Convert.ToUInt16((ulong) value);

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(ulong value)
    {
      if (value > (ulong) ushort.MaxValue)
        Convert.ThrowUInt16OverflowException();
      return (ushort) value;
    }

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 16-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(float value) => Convert.ToUInt16((double) value);

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 16-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(double value) => Convert.ToUInt16(Convert.ToInt32(value));

    /// <summary>Converts the value of the specified decimal number to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 16-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(Decimal value) => Decimal.ToUInt16(Decimal.Round(value, 0));

    /// <summary>Converts the specified string representation of a number to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(string? value) => value == null ? (ushort) 0 : ushort.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent 16-bit unsigned integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(string? value, IFormatProvider? provider) => value == null ? (ushort) 0 : ushort.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(DateTime value) => ((IConvertible) value).ToUInt16((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a 32-bit signed integer.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the  <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static int ToInt32(object? value) => value != null ? ((IConvertible) value).ToInt32((IFormatProvider) null) : 0;

    /// <summary>Converts the value of the specified object to a 32-bit signed integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static int ToInt32(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToInt32(provider) : 0;

    /// <summary>Converts the specified Boolean value to the equivalent 32-bit signed integer.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    public static int ToInt32(bool value) => !value ? 0 : 1;

    /// <summary>Converts the value of the specified Unicode character to the equivalent 32-bit signed integer.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static int ToInt32(char value) => (int) value;

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 32-bit signed integer.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>A 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static int ToInt32(sbyte value) => (int) value;

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 32-bit signed integer.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static int ToInt32(byte value) => (int) value;

    /// <summary>Converts the value of the specified 16-bit signed integer to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static int ToInt32(short value) => (int) value;

    /// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 32-bit signed integer.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static int ToInt32(ushort value) => (int) value;

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static int ToInt32(uint value)
    {
      if ((int) value < 0)
        Convert.ThrowInt32OverflowException();
      return (int) value;
    }

    /// <summary>Returns the specified 32-bit signed integer; no actual conversion is performed.</summary>
    /// <param name="value">The 32-bit signed integer to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static int ToInt32(int value) => value;

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" /> or less than <see cref="F:System.Int32.MinValue" />.</exception>
    /// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static int ToInt32(long value)
    {
      if (value < (long) int.MinValue || value > (long) int.MaxValue)
        Convert.ThrowInt32OverflowException();
      return (int) value;
    }

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static int ToInt32(ulong value)
    {
      if (value > (ulong) int.MaxValue)
        Convert.ThrowInt32OverflowException();
      return (int) value;
    }

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" /> or less than <see cref="F:System.Int32.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 32-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static int ToInt32(float value) => Convert.ToInt32((double) value);

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" /> or less than <see cref="F:System.Int32.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 32-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static int ToInt32(double value)
    {
      if (value >= 0.0)
      {
        if (value < 2147483647.5)
        {
          int int32 = (int) value;
          double num = value - (double) int32;
          if (num > 0.5 || num == 0.5 && (int32 & 1) != 0)
            ++int32;
          return int32;
        }
      }
      else if (value >= -2147483648.5)
      {
        int int32 = (int) value;
        double num = value - (double) int32;
        if (num < -0.5 || num == -0.5 && (int32 & 1) != 0)
          --int32;
        return int32;
      }
      throw new OverflowException(SR.Overflow_Int32);
    }

    /// <summary>Converts the value of the specified decimal number to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int32.MaxValue" /> or less than <see cref="F:System.Int32.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 32-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static int ToInt32(Decimal value) => Decimal.ToInt32(Decimal.Round(value, 0));

    /// <summary>Converts the specified string representation of a number to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static int ToInt32(string? value) => value == null ? 0 : int.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent 32-bit signed integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static int ToInt32(string? value, IFormatProvider? provider) => value == null ? 0 : int.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static int ToInt32(DateTime value) => ((IConvertible) value).ToInt32((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a 32-bit unsigned integer.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(object? value) => value != null ? ((IConvertible) value).ToUInt32((IFormatProvider) null) : 0U;

    /// <summary>Converts the value of the specified object to a 32-bit unsigned integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToUInt32(provider) : 0U;

    /// <summary>Converts the specified Boolean value to the equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(bool value) => !value ? 0U : 1U;

    /// <summary>Converts the value of the specified Unicode character to the equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(char value) => (uint) value;

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(sbyte value)
    {
      if (value < (sbyte) 0)
        Convert.ThrowUInt32OverflowException();
      return (uint) value;
    }

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(byte value) => (uint) value;

    /// <summary>Converts the value of the specified 16-bit signed integer to the equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(short value)
    {
      if (value < (short) 0)
        Convert.ThrowUInt32OverflowException();
      return (uint) value;
    }

    /// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(ushort value) => (uint) value;

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(int value)
    {
      if (value < 0)
        Convert.ThrowUInt32OverflowException();
      return (uint) value;
    }

    /// <summary>Returns the specified 32-bit unsigned integer; no actual conversion is performed.</summary>
    /// <param name="value">The 32-bit unsigned integer to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(uint value) => value;

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(long value) => Convert.ToUInt32((ulong) value);

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(ulong value)
    {
      if (value > (ulong) uint.MaxValue)
        Convert.ThrowUInt32OverflowException();
      return (uint) value;
    }

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 32-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(float value) => Convert.ToUInt32((double) value);

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 32-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(double value)
    {
      uint uint32 = value >= -0.5 && value < 4294967295.5 ? (uint) value : throw new OverflowException(SR.Overflow_UInt32);
      double num = value - (double) uint32;
      if (num > 0.5 || num == 0.5 && ((int) uint32 & 1) != 0)
        ++uint32;
      return uint32;
    }

    /// <summary>Converts the value of the specified decimal number to an equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 32-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(Decimal value) => Decimal.ToUInt32(Decimal.Round(value, 0));

    /// <summary>Converts the specified string representation of a number to an equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(string? value) => value == null ? 0U : uint.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent 32-bit unsigned integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(string? value, IFormatProvider? provider) => value == null ? 0U : uint.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(DateTime value) => ((IConvertible) value).ToUInt32((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a 64-bit signed integer.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static long ToInt64(object? value) => value != null ? ((IConvertible) value).ToInt64((IFormatProvider) null) : 0L;

    /// <summary>Converts the value of the specified object to a 64-bit signed integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static long ToInt64(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToInt64(provider) : 0L;

    /// <summary>Converts the specified Boolean value to the equivalent 64-bit signed integer.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    public static long ToInt64(bool value) => value ? 1L : 0L;

    /// <summary>Converts the value of the specified Unicode character to the equivalent 64-bit signed integer.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static long ToInt64(char value) => (long) value;

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 64-bit signed integer.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static long ToInt64(sbyte value) => (long) value;

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 64-bit signed integer.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static long ToInt64(byte value) => (long) value;

    /// <summary>Converts the value of the specified 16-bit signed integer to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static long ToInt64(short value) => (long) value;

    /// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 64-bit signed integer.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static long ToInt64(ushort value) => (long) value;

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    public static long ToInt64(int value) => (long) value;

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static long ToInt64(uint value) => (long) value;

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static long ToInt64(ulong value)
    {
      if ((long) value < 0L)
        Convert.ThrowInt64OverflowException();
      return (long) value;
    }

    /// <summary>Returns the specified 64-bit signed integer; no actual conversion is performed.</summary>
    /// <param name="value">A 64-bit signed integer.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static long ToInt64(long value) => value;

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int64.MaxValue" /> or less than <see cref="F:System.Int64.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 64-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static long ToInt64(float value) => Convert.ToInt64((double) value);

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int64.MaxValue" /> or less than <see cref="F:System.Int64.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 64-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static long ToInt64(double value) => checked ((long) Math.Round(value));

    /// <summary>Converts the value of the specified decimal number to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Int64.MaxValue" /> or less than <see cref="F:System.Int64.MinValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 64-bit signed integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    public static long ToInt64(Decimal value) => Decimal.ToInt64(Decimal.Round(value, 0));

    /// <summary>Converts the specified string representation of a number to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">A string that contains a number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static long ToInt64(string? value) => value == null ? 0L : long.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent 64-bit signed integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static long ToInt64(string? value, IFormatProvider? provider) => value == null ? 0L : long.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static long ToInt64(DateTime value) => ((IConvertible) value).ToInt64((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a 64-bit unsigned integer.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(object? value) => value != null ? ((IConvertible) value).ToUInt64((IFormatProvider) null) : 0UL;

    /// <summary>Converts the value of the specified object to a 64-bit unsigned integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToUInt64(provider) : 0UL;

    /// <summary>Converts the specified Boolean value to the equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(bool value) => !value ? 0UL : 1UL;

    /// <summary>Converts the value of the specified Unicode character to the equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(char value) => (ulong) value;

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(sbyte value)
    {
      if (value < (sbyte) 0)
        Convert.ThrowUInt64OverflowException();
      return (ulong) value;
    }

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>A 64-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(byte value) => (ulong) value;

    /// <summary>Converts the value of the specified 16-bit signed integer to the equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(short value)
    {
      if (value < (short) 0)
        Convert.ThrowUInt64OverflowException();
      return (ulong) value;
    }

    /// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(ushort value) => (ulong) value;

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(int value)
    {
      if (value < 0)
        Convert.ThrowUInt64OverflowException();
      return (ulong) value;
    }

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(uint value) => (ulong) value;

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero.</exception>
    /// <returns>A 64-bit unsigned integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(long value)
    {
      if (value < 0L)
        Convert.ThrowUInt64OverflowException();
      return (ulong) value;
    }

    /// <summary>Returns the specified 64-bit unsigned integer; no actual conversion is performed.</summary>
    /// <param name="value">The 64-bit unsigned integer to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(ulong value) => value;

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 64-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(float value) => Convert.ToUInt64((double) value);

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 64-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(double value) => checked ((ulong) Math.Round(value));

    /// <summary>Converts the value of the specified decimal number to an equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is less than zero or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>
    /// <paramref name="value" />, rounded to the nearest 64-bit unsigned integer. If <paramref name="value" /> is halfway between two whole numbers, the even number is returned; that is, 4.5 is converted to 4, and 5.5 is converted to 6.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(Decimal value) => Decimal.ToUInt64(Decimal.Round(value, 0));

    /// <summary>Converts the specified string representation of a number to an equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(string? value) => value == null ? 0UL : ulong.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent 64-bit unsigned integer, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> does not consist of an optional sign followed by a sequence of digits (0 through 9).</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(string? value, IFormatProvider? provider) => value == null ? 0UL : ulong.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(DateTime value) => ((IConvertible) value).ToUInt64((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a single-precision floating-point number.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static float ToSingle(object? value) => value != null ? ((IConvertible) value).ToSingle((IFormatProvider) null) : 0.0f;

    /// <summary>Converts the value of the specified object to an single-precision floating-point number, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> does not implement <see cref="T:System.IConvertible" />.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static float ToSingle(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToSingle(provider) : 0.0f;

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent single-precision floating-point number.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>An 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static float ToSingle(sbyte value) => (float) value;

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent single-precision floating-point number.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static float ToSingle(byte value) => (float) value;

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static float ToSingle(char value) => ((IConvertible) value).ToSingle((IFormatProvider) null);

    /// <summary>Converts the value of the specified 16-bit signed integer to an equivalent single-precision floating-point number.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static float ToSingle(short value) => (float) value;

    /// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent single-precision floating-point number.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static float ToSingle(ushort value) => (float) value;

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent single-precision floating-point number.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static float ToSingle(int value) => (float) value;

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent single-precision floating-point number.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static float ToSingle(uint value) => (float) value;

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent single-precision floating-point number.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static float ToSingle(long value) => (float) value;

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent single-precision floating-point number.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static float ToSingle(ulong value) => (float) value;

    /// <summary>Returns the specified single-precision floating-point number; no actual conversion is performed.</summary>
    /// <param name="value">The single-precision floating-point number to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static float ToSingle(float value) => value;

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent single-precision floating-point number.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.
    /// 
    /// <paramref name="value" /> is rounded using rounding to nearest. For example, when rounded to two decimals, the value 2.345 becomes 2.34 and the value 2.355 becomes 2.36.</returns>
    public static float ToSingle(double value) => (float) value;

    /// <summary>Converts the value of the specified decimal number to an equivalent single-precision floating-point number.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <returns>A single-precision floating-point number that is equivalent to <paramref name="value" />.
    /// 
    /// <paramref name="value" /> is rounded using rounding to nearest. For example, when rounded to two decimals, the value 2.345 becomes 2.34 and the value 2.355 becomes 2.36.</returns>
    public static float ToSingle(Decimal value) => (float) value;

    /// <summary>Converts the specified string representation of a number to an equivalent single-precision floating-point number.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <returns>A single-precision floating-point number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static float ToSingle(string? value) => value == null ? 0.0f : float.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent single-precision floating-point number, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Single.MinValue" /> or greater than <see cref="F:System.Single.MaxValue" />.</exception>
    /// <returns>A single-precision floating-point number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static float ToSingle(string? value, IFormatProvider? provider) => value == null ? 0.0f : float.Parse(value, provider);

    /// <summary>Converts the specified Boolean value to the equivalent single-precision floating-point number.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    public static float ToSingle(bool value) => value ? 1f : 0.0f;

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static float ToSingle(DateTime value) => ((IConvertible) value).ToSingle((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to a double-precision floating-point number.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format for a <see cref="T:System.Double" /> type.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static double ToDouble(object? value) => value != null ? ((IConvertible) value).ToDouble((IFormatProvider) null) : 0.0;

    /// <summary>Converts the value of the specified object to an double-precision floating-point number, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format for a <see cref="T:System.Double" /> type.</exception>
    /// <exception cref="T:System.InvalidCastException">
    /// <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />, or zero if <paramref name="value" /> is <see langword="null" />.</returns>
    public static double ToDouble(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToDouble(provider) : 0.0;

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent double-precision floating-point number.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>The 8-bit signed integer that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static double ToDouble(sbyte value) => (double) value;

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent double-precision floating-point number.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>The double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static double ToDouble(byte value) => (double) value;

    /// <summary>Converts the value of the specified 16-bit signed integer to an equivalent double-precision floating-point number.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns>A double-precision floating-point number equivalent to <paramref name="value" />.</returns>
    public static double ToDouble(short value) => (double) value;

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static double ToDouble(char value) => ((IConvertible) value).ToDouble((IFormatProvider) null);

    /// <summary>Converts the value of the specified 16-bit unsigned integer to the equivalent double-precision floating-point number.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static double ToDouble(ushort value) => (double) value;

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent double-precision floating-point number.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static double ToDouble(int value) => (double) value;

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent double-precision floating-point number.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static double ToDouble(uint value) => (double) value;

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent double-precision floating-point number.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static double ToDouble(long value) => (double) value;

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent double-precision floating-point number.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static double ToDouble(ulong value) => (double) value;

    /// <summary>Converts the value of the specified single-precision floating-point number to an equivalent double-precision floating-point number.</summary>
    /// <param name="value">The single-precision floating-point number.</param>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static double ToDouble(float value) => (double) value;

    /// <summary>Returns the specified double-precision floating-point number; no actual conversion is performed.</summary>
    /// <param name="value">The double-precision floating-point number to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static double ToDouble(double value) => value;

    /// <summary>Converts the value of the specified decimal number to an equivalent double-precision floating-point number.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <returns>A double-precision floating-point number that is equivalent to <paramref name="value" />.</returns>
    public static double ToDouble(Decimal value) => (double) value;

    /// <summary>Converts the specified string representation of a number to an equivalent double-precision floating-point number.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static double ToDouble(string? value) => value == null ? 0.0 : double.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent double-precision floating-point number, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Double.MinValue" /> or greater than <see cref="F:System.Double.MaxValue" />.</exception>
    /// <returns>A double-precision floating-point number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static double ToDouble(string? value, IFormatProvider? provider) => value == null ? 0.0 : double.Parse(value, provider);

    /// <summary>Converts the specified Boolean value to the equivalent double-precision floating-point number.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    public static double ToDouble(bool value) => value ? 1.0 : 0.0;

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static double ToDouble(DateTime value) => ((IConvertible) value).ToDouble((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to an equivalent decimal number.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format for a <see cref="T:System.Decimal" /> type.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static Decimal ToDecimal(object? value) => value != null ? ((IConvertible) value).ToDecimal((IFormatProvider) null) : 0M;

    /// <summary>Converts the value of the specified object to an equivalent decimal number, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not in an appropriate format for a <see cref="T:System.Decimal" /> type.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static Decimal ToDecimal(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToDecimal(provider) : 0M;

    /// <summary>Converts the value of the specified 8-bit signed integer to the equivalent decimal number.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static Decimal ToDecimal(sbyte value) => (Decimal) value;

    /// <summary>Converts the value of the specified 8-bit unsigned integer to the equivalent decimal number.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>The decimal number that is equivalent to <paramref name="value" />.</returns>
    public static Decimal ToDecimal(byte value) => (Decimal) value;

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static Decimal ToDecimal(char value) => ((IConvertible) value).ToDecimal((IFormatProvider) null);

    /// <summary>Converts the value of the specified 16-bit signed integer to an equivalent decimal number.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
    public static Decimal ToDecimal(short value) => (Decimal) value;

    /// <summary>Converts the value of the specified 16-bit unsigned integer to an equivalent decimal number.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>The decimal number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static Decimal ToDecimal(ushort value) => (Decimal) value;

    /// <summary>Converts the value of the specified 32-bit signed integer to an equivalent decimal number.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
    public static Decimal ToDecimal(int value) => (Decimal) value;

    /// <summary>Converts the value of the specified 32-bit unsigned integer to an equivalent decimal number.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static Decimal ToDecimal(uint value) => (Decimal) value;

    /// <summary>Converts the value of the specified 64-bit signed integer to an equivalent decimal number.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
    public static Decimal ToDecimal(long value) => (Decimal) value;

    /// <summary>Converts the value of the specified 64-bit unsigned integer to an equivalent decimal number.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static Decimal ToDecimal(ulong value) => (Decimal) value;

    /// <summary>Converts the value of the specified single-precision floating-point number to the equivalent decimal number.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.</exception>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
    public static Decimal ToDecimal(float value) => (Decimal) value;

    /// <summary>Converts the value of the specified double-precision floating-point number to an equivalent decimal number.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is greater than <see cref="F:System.Decimal.MaxValue" /> or less than <see cref="F:System.Decimal.MinValue" />.</exception>
    /// <returns>A decimal number that is equivalent to <paramref name="value" />.</returns>
    public static Decimal ToDecimal(double value) => (Decimal) value;

    /// <summary>Converts the specified string representation of a number to an equivalent decimal number.</summary>
    /// <param name="value">A string that contains a number to convert.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>A decimal number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static Decimal ToDecimal(string? value) => value == null ? 0M : Decimal.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent decimal number, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a number in a valid format.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Decimal.MinValue" /> or greater than <see cref="F:System.Decimal.MaxValue" />.</exception>
    /// <returns>A decimal number that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static Decimal ToDecimal(string? value, IFormatProvider? provider) => value == null ? 0M : Decimal.Parse(value, provider);

    /// <summary>Returns the specified decimal number; no actual conversion is performed.</summary>
    /// <param name="value">A decimal number.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static Decimal ToDecimal(Decimal value) => value;

    /// <summary>Converts the specified Boolean value to the equivalent decimal number.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The number 1 if <paramref name="value" /> is <see langword="true" />; otherwise, 0.</returns>
    public static Decimal ToDecimal(bool value) => (Decimal) (value ? 1 : 0);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static Decimal ToDecimal(DateTime value) => ((IConvertible) value).ToDecimal((IFormatProvider) null);

    /// <summary>Returns the specified <see cref="T:System.DateTime" /> object; no actual conversion is performed.</summary>
    /// <param name="value">A date and time value.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    public static DateTime ToDateTime(DateTime value) => value;

    /// <summary>Converts the value of the specified object to a <see cref="T:System.DateTime" /> object.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface, or <see langword="null" />.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a valid date and time value.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <returns>The date and time equivalent of the value of <paramref name="value" />, or a date and time equivalent of <see cref="F:System.DateTime.MinValue" /> if <paramref name="value" /> is <see langword="null" />.</returns>
    public static DateTime ToDateTime(object? value) => value != null ? ((IConvertible) value).ToDateTime((IFormatProvider) null) : DateTime.MinValue;

    /// <summary>Converts the value of the specified object to a <see cref="T:System.DateTime" /> object, using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that implements the <see cref="T:System.IConvertible" /> interface.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a valid date and time value.</exception>
    /// <exception cref="T:System.InvalidCastException">
    ///        <paramref name="value" /> does not implement the <see cref="T:System.IConvertible" /> interface.
    /// 
    /// -or-
    /// 
    /// The conversion is not supported.</exception>
    /// <returns>The date and time equivalent of the value of <paramref name="value" />, or the date and time equivalent of <see cref="F:System.DateTime.MinValue" /> if <paramref name="value" /> is <see langword="null" />.</returns>
    public static DateTime ToDateTime(object? value, IFormatProvider? provider) => value != null ? ((IConvertible) value).ToDateTime(provider) : DateTime.MinValue;

    /// <summary>Converts the specified string representation of a date and time to an equivalent date and time value.</summary>
    /// <param name="value">The string representation of a date and time.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a properly formatted date and time string.</exception>
    /// <returns>The date and time equivalent of the value of <paramref name="value" />, or the date and time equivalent of <see cref="F:System.DateTime.MinValue" /> if <paramref name="value" /> is <see langword="null" />.</returns>
    public static DateTime ToDateTime(string? value) => value == null ? new DateTime(0L) : DateTime.Parse(value);

    /// <summary>Converts the specified string representation of a number to an equivalent date and time, using the specified culture-specific formatting information.</summary>
    /// <param name="value">A string that contains a date and time to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> is not a properly formatted date and time string.</exception>
    /// <returns>The date and time equivalent of the value of <paramref name="value" />, or the date and time equivalent of <see cref="F:System.DateTime.MinValue" /> if <paramref name="value" /> is <see langword="null" />.</returns>
    public static DateTime ToDateTime(string? value, IFormatProvider? provider) => value == null ? new DateTime(0L) : DateTime.Parse(value, provider);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    [CLSCompliant(false)]
    public static DateTime ToDateTime(sbyte value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(byte value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(short value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    [CLSCompliant(false)]
    public static DateTime ToDateTime(ushort value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(int value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    [CLSCompliant(false)]
    public static DateTime ToDateTime(uint value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(long value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    [CLSCompliant(false)]
    public static DateTime ToDateTime(ulong value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(bool value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(char value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The single-precision floating-point value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(float value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The double-precision floating-point value to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(double value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Calling this method always throws <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="value">The number to convert.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported.</exception>
    /// <returns>This conversion is not supported. No value is returned.</returns>
    public static DateTime ToDateTime(Decimal value) => ((IConvertible) value).ToDateTime((IFormatProvider) null);

    /// <summary>Converts the value of the specified object to its equivalent string representation.</summary>
    /// <param name="value">An object that supplies the value to convert, or <see langword="null" />.</param>
    /// <returns>The string representation of <paramref name="value" />, or <see cref="F:System.String.Empty" /> if <paramref name="value" /> is <see langword="null" />.</returns>
    public static string? ToString(object? value) => Convert.ToString(value, (IFormatProvider) null);

    /// <summary>Converts the value of the specified object to its equivalent string representation using the specified culture-specific formatting information.</summary>
    /// <param name="value">An object that supplies the value to convert, or <see langword="null" />.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />, or <see cref="F:System.String.Empty" /> if <paramref name="value" /> is an object whose value is <see langword="null" />. If <paramref name="value" /> is <see langword="null" />, the method returns <see langword="null" />.</returns>
    public static string? ToString(object? value, IFormatProvider? provider)
    {
      switch (value)
      {
        case IConvertible convertible:
          return convertible.ToString(provider);
        case IFormattable formattable:
          return formattable.ToString((string) null, provider);
        case null:
          return string.Empty;
        default:
          return value.ToString();
      }
    }

    /// <summary>Converts the specified Boolean value to its equivalent string representation.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(bool value) => value.ToString();

    /// <summary>Converts the specified Boolean value to its equivalent string representation.</summary>
    /// <param name="value">The Boolean value to convert.</param>
    /// <param name="provider">An instance of an object. This parameter is ignored.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(bool value, IFormatProvider? provider) => value.ToString();

    /// <summary>Converts the value of the specified Unicode character to its equivalent string representation.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(char value) => char.ToString(value);

    /// <summary>Converts the value of the specified Unicode character to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The Unicode character to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information. This parameter is ignored.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(char value, IFormatProvider? provider) => value.ToString();

    /// <summary>Converts the value of the specified 8-bit signed integer to its equivalent string representation.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static string ToString(sbyte value) => value.ToString();

    /// <summary>Converts the value of the specified 8-bit signed integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static string ToString(sbyte value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified 8-bit unsigned integer to its equivalent string representation.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(byte value) => value.ToString();

    /// <summary>Converts the value of the specified 8-bit unsigned integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(byte value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified 16-bit signed integer to its equivalent string representation.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(short value) => value.ToString();

    /// <summary>Converts the value of the specified 16-bit signed integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(short value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified 16-bit unsigned integer to its equivalent string representation.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static string ToString(ushort value) => value.ToString();

    /// <summary>Converts the value of the specified 16-bit unsigned integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static string ToString(ushort value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified 32-bit signed integer to its equivalent string representation.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(int value) => value.ToString();

    /// <summary>Converts the value of the specified 32-bit signed integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(int value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified 32-bit unsigned integer to its equivalent string representation.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static string ToString(uint value) => value.ToString();

    /// <summary>Converts the value of the specified 32-bit unsigned integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static string ToString(uint value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified 64-bit signed integer to its equivalent string representation.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(long value) => value.ToString();

    /// <summary>Converts the value of the specified 64-bit signed integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(long value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified 64-bit unsigned integer to its equivalent string representation.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static string ToString(ulong value) => value.ToString();

    /// <summary>Converts the value of the specified 64-bit unsigned integer to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static string ToString(ulong value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified single-precision floating-point number to its equivalent string representation.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(float value) => value.ToString();

    /// <summary>Converts the value of the specified single-precision floating-point number to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The single-precision floating-point number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(float value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified double-precision floating-point number to its equivalent string representation.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(double value) => value.ToString();

    /// <summary>Converts the value of the specified double-precision floating-point number to its equivalent string representation.</summary>
    /// <param name="value">The double-precision floating-point number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(double value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified decimal number to its equivalent string representation.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(Decimal value) => value.ToString();

    /// <summary>Converts the value of the specified decimal number to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The decimal number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(Decimal value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Converts the value of the specified <see cref="T:System.DateTime" /> to its equivalent string representation.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(DateTime value) => value.ToString();

    /// <summary>Converts the value of the specified <see cref="T:System.DateTime" /> to its equivalent string representation, using the specified culture-specific formatting information.</summary>
    /// <param name="value">The date and time value to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of <paramref name="value" />.</returns>
    public static string ToString(DateTime value, IFormatProvider? provider) => value.ToString(provider);

    /// <summary>Returns the specified string instance; no actual conversion is performed.</summary>
    /// <param name="value">The string to return.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    [return: NotNullIfNotNull("value")]
    public static string? ToString(string? value) => value;

    /// <summary>Returns the specified string instance; no actual conversion is performed.</summary>
    /// <param name="value">The string to return.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information. This parameter is ignored.</param>
    /// <returns>
    /// <paramref name="value" /> is returned unchanged.</returns>
    [return: NotNullIfNotNull("value")]
    public static string? ToString(string? value, IFormatProvider? provider) => value;

    /// <summary>Converts the string representation of a number in a specified base to an equivalent 8-bit unsigned integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fromBase" /> is not 2, 8, 10, or 16.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" />, which represents a base 10 unsigned number, is prefixed with a negative sign.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Byte.MinValue" /> or greater than <see cref="F:System.Byte.MaxValue" />.</exception>
    /// <returns>An 8-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static byte ToByte(string? value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
        throw new ArgumentException(SR.Arg_InvalidBase);
      if (value == null)
        return 0;
      int num = ParseNumbers.StringToInt(value.AsSpan(), fromBase, 4608);
      if ((uint) num > (uint) byte.MaxValue)
        Convert.ThrowByteOverflowException();
      return (byte) num;
    }

    /// <summary>Converts the string representation of a number in a specified base to an equivalent 8-bit signed integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fromBase" /> is not 2, 8, 10, or 16.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />.</exception>
    /// <returns>An 8-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(string? value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
        throw new ArgumentException(SR.Arg_InvalidBase);
      if (value == null)
        return 0;
      int num = ParseNumbers.StringToInt(value.AsSpan(), fromBase, 5120);
      if (fromBase != 10 && num <= (int) byte.MaxValue || num >= (int) sbyte.MinValue && num <= (int) sbyte.MaxValue)
        return (sbyte) num;
      Convert.ThrowSByteOverflowException();
      return (sbyte) num;
    }

    /// <summary>Converts the string representation of a number in a specified base to an equivalent 16-bit signed integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fromBase" /> is not 2, 8, 10, or 16.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int16.MinValue" /> or greater than <see cref="F:System.Int16.MaxValue" />.</exception>
    /// <returns>A 16-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static short ToInt16(string? value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
        throw new ArgumentException(SR.Arg_InvalidBase);
      if (value == null)
        return 0;
      int int16 = ParseNumbers.StringToInt(value.AsSpan(), fromBase, 6144);
      if (fromBase != 10 && int16 <= (int) ushort.MaxValue || int16 >= (int) short.MinValue && int16 <= (int) short.MaxValue)
        return (short) int16;
      Convert.ThrowInt16OverflowException();
      return (short) int16;
    }

    /// <summary>Converts the string representation of a number in a specified base to an equivalent 16-bit unsigned integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fromBase" /> is not 2, 8, 10, or 16.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt16.MinValue" /> or greater than <see cref="F:System.UInt16.MaxValue" />.</exception>
    /// <returns>A 16-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(string? value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
        throw new ArgumentException(SR.Arg_InvalidBase);
      if (value == null)
        return 0;
      int uint16 = ParseNumbers.StringToInt(value.AsSpan(), fromBase, 4608);
      if ((uint) uint16 > (uint) ushort.MaxValue)
        Convert.ThrowUInt16OverflowException();
      return (ushort) uint16;
    }

    /// <summary>Converts the string representation of a number in a specified base to an equivalent 32-bit signed integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fromBase" /> is not 2, 8, 10, or 16.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int32.MinValue" /> or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>A 32-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static int ToInt32(string? value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
        throw new ArgumentException(SR.Arg_InvalidBase);
      return value == null ? 0 : ParseNumbers.StringToInt(value.AsSpan(), fromBase, 4096);
    }

    /// <summary>Converts the string representation of a number in a specified base to an equivalent 32-bit unsigned integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fromBase" /> is not 2, 8, 10, or 16.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />.</exception>
    /// <returns>A 32-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(string? value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
        throw new ArgumentException(SR.Arg_InvalidBase);
      return value == null ? 0U : (uint) ParseNumbers.StringToInt(value.AsSpan(), fromBase, 4608);
    }

    /// <summary>Converts the string representation of a number in a specified base to an equivalent 64-bit signed integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fromBase" /> is not 2, 8, 10, or 16.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" />, which represents a non-base 10 signed number, is prefixed with a negative sign.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.Int64.MinValue" /> or greater than <see cref="F:System.Int64.MaxValue" />.</exception>
    /// <returns>A 64-bit signed integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    public static long ToInt64(string? value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
        throw new ArgumentException(SR.Arg_InvalidBase);
      return value == null ? 0L : ParseNumbers.StringToLong(value.AsSpan(), fromBase, 4096);
    }

    /// <summary>Converts the string representation of a number in a specified base to an equivalent 64-bit unsigned integer.</summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="fromBase">The base of the number in <paramref name="value" />, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="fromBase" /> is not 2, 8, 10, or 16.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="value" /> contains a character that is not a valid digit in the base specified by <paramref name="fromBase" />. The exception message indicates that there are no digits to convert if the first character in <paramref name="value" /> is invalid; otherwise, the message indicates that <paramref name="value" /> contains invalid trailing characters.</exception>
    /// <exception cref="T:System.OverflowException">
    ///        <paramref name="value" />, which represents a non-base 10 unsigned number, is prefixed with a negative sign.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> represents a number that is less than <see cref="F:System.UInt64.MinValue" /> or greater than <see cref="F:System.UInt64.MaxValue" />.</exception>
    /// <returns>A 64-bit unsigned integer that is equivalent to the number in <paramref name="value" />, or 0 (zero) if <paramref name="value" /> is <see langword="null" />.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(string? value, int fromBase)
    {
      if (fromBase != 2 && fromBase != 8 && fromBase != 10 && fromBase != 16)
        throw new ArgumentException(SR.Arg_InvalidBase);
      return value == null ? 0UL : (ulong) ParseNumbers.StringToLong(value.AsSpan(), fromBase, 4608);
    }

    /// <summary>Converts the value of an 8-bit unsigned integer to its equivalent string representation in a specified base.</summary>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <param name="toBase">The base of the return value, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="toBase" /> is not 2, 8, 10, or 16.</exception>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="toBase" />.</returns>
    public static string ToString(byte value, int toBase) => toBase == 2 || toBase == 8 || toBase == 10 || toBase == 16 ? ParseNumbers.IntToString((int) value, toBase, -1, ' ', 64) : throw new ArgumentException(SR.Arg_InvalidBase);

    /// <summary>Converts the value of a 16-bit signed integer to its equivalent string representation in a specified base.</summary>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <param name="toBase">The base of the return value, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="toBase" /> is not 2, 8, 10, or 16.</exception>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="toBase" />.</returns>
    public static string ToString(short value, int toBase) => toBase == 2 || toBase == 8 || toBase == 10 || toBase == 16 ? ParseNumbers.IntToString((int) value, toBase, -1, ' ', 128) : throw new ArgumentException(SR.Arg_InvalidBase);

    /// <summary>Converts the value of a 32-bit signed integer to its equivalent string representation in a specified base.</summary>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <param name="toBase">The base of the return value, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="toBase" /> is not 2, 8, 10, or 16.</exception>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="toBase" />.</returns>
    public static string ToString(int value, int toBase) => toBase == 2 || toBase == 8 || toBase == 10 || toBase == 16 ? ParseNumbers.IntToString(value, toBase, -1, ' ', 0) : throw new ArgumentException(SR.Arg_InvalidBase);

    /// <summary>Converts the value of a 64-bit signed integer to its equivalent string representation in a specified base.</summary>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <param name="toBase">The base of the return value, which must be 2, 8, 10, or 16.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="toBase" /> is not 2, 8, 10, or 16.</exception>
    /// <returns>The string representation of <paramref name="value" /> in base <paramref name="toBase" />.</returns>
    public static string ToString(long value, int toBase) => toBase == 2 || toBase == 8 || toBase == 10 || toBase == 16 ? ParseNumbers.LongToString(value, toBase, -1, ' ', 0) : throw new ArgumentException(SR.Arg_InvalidBase);

    /// <summary>Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits.</summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> is <see langword="null" />.</exception>
    /// <returns>The string representation, in base 64, of the contents of <paramref name="inArray" />.</returns>
    public static string ToBase64String(byte[] inArray) => inArray != null ? Convert.ToBase64String(new ReadOnlySpan<byte>(inArray)) : throw new ArgumentNullException(nameof (inArray));

    /// <summary>Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits. You can specify whether to insert line breaks in the return value.</summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <param name="options">
    /// <see cref="F:System.Base64FormattingOptions.InsertLineBreaks" /> to insert a line break every 76 characters, or <see cref="F:System.Base64FormattingOptions.None" /> to not insert line breaks.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> is not a valid <see cref="T:System.Base64FormattingOptions" /> value.</exception>
    /// <returns>The string representation in base 64 of the elements in <paramref name="inArray" />.</returns>
    public static string ToBase64String(byte[] inArray, Base64FormattingOptions options) => inArray != null ? Convert.ToBase64String(new ReadOnlySpan<byte>(inArray), options) : throw new ArgumentNullException(nameof (inArray));

    /// <summary>Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits. Parameters specify the subset as an offset in the input array, and the number of elements in the array to convert.</summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <param name="offset">An offset in <paramref name="inArray" />.</param>
    /// <param name="length">The number of elements of <paramref name="inArray" /> to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="length" /> is negative.
    /// 
    /// -or-
    /// 
    /// <paramref name="offset" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />.</exception>
    /// <returns>The string representation in base 64 of <paramref name="length" /> elements of <paramref name="inArray" />, starting at position <paramref name="offset" />.</returns>
    public static string ToBase64String(byte[] inArray, int offset, int length) => Convert.ToBase64String(inArray, offset, length, Base64FormattingOptions.None);

    /// <summary>Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits. Parameters specify the subset as an offset in the input array, the number of elements in the array to convert, and whether to insert line breaks in the return value.</summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <param name="offset">An offset in <paramref name="inArray" />.</param>
    /// <param name="length">The number of elements of <paramref name="inArray" /> to convert.</param>
    /// <param name="options">
    /// <see cref="F:System.Base64FormattingOptions.InsertLineBreaks" /> to insert a line break every 76 characters, or <see cref="F:System.Base64FormattingOptions.None" /> to not insert line breaks.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="length" /> is negative.
    /// 
    /// -or-
    /// 
    /// <paramref name="offset" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> is not a valid <see cref="T:System.Base64FormattingOptions" /> value.</exception>
    /// <returns>The string representation in base 64 of <paramref name="length" /> elements of <paramref name="inArray" />, starting at position <paramref name="offset" />.</returns>
    public static string ToBase64String(
      byte[] inArray,
      int offset,
      int length,
      Base64FormattingOptions options)
    {
      if (inArray == null)
        throw new ArgumentNullException(nameof (inArray));
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length), SR.ArgumentOutOfRange_Index);
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_GenericPositive);
      if (offset > inArray.Length - length)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_OffsetLength);
      return Convert.ToBase64String(new ReadOnlySpan<byte>(inArray, offset, length), options);
    }

    /// <summary>Converts the 8-bit unsigned integers inside the specified read-only span into their equivalent string representation that is encoded with base-64 digits. You can optionally specify whether to insert line breaks in the return value.</summary>
    /// <param name="bytes">A read-only span of 8-bit unsigned integers.</param>
    /// <param name="options">One of the enumeration values that specify whether to insert line breaks in the return value. The default value is <see cref="F:System.Base64FormattingOptions.None" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> is not a valid <see cref="T:System.Base64FormattingOptions" /> value.</exception>
    /// <exception cref="T:System.OutOfMemoryException">The output length was larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <returns>The string representation in base 64 of the elements in <paramref name="inArray" />. If the length of <paramref name="bytes" /> is 0, an empty string is returned.</returns>
    public static unsafe string ToBase64String(
      ReadOnlySpan<byte> bytes,
      Base64FormattingOptions options = Base64FormattingOptions.None)
    {
      if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
        throw new ArgumentException(SR.Format(SR.Arg_EnumIllegalVal, (object) (int) options), nameof (options));
      if (bytes.Length == 0)
        return string.Empty;
      bool insertLineBreaks = options == Base64FormattingOptions.InsertLineBreaks;
      string base64String = string.FastAllocateString(Convert.ToBase64_CalculateAndValidateOutputLength(bytes.Length, insertLineBreaks));
      fixed (byte* inData = &MemoryMarshal.GetReference<byte>(bytes))
      {
        IntPtr outChars;
        if (base64String == null)
        {
          outChars = IntPtr.Zero;
        }
        else
        {
          fixed (char* chPtr = &base64String.GetPinnableReference())
            outChars = (IntPtr) chPtr;
        }
        Convert.ConvertToBase64Array((char*) outChars, inData, 0, bytes.Length, insertLineBreaks);
        // ISSUE: fixed variable is out of scope
        // ISSUE: __unpin statement
        __unpin(chPtr);
      }
      return base64String;
    }

    /// <summary>Converts a subset of an 8-bit unsigned integer array to an equivalent subset of a Unicode character array encoded with base-64 digits. Parameters specify the subsets as offsets in the input and output arrays, and the number of elements in the input array to convert.</summary>
    /// <param name="inArray">An input array of 8-bit unsigned integers.</param>
    /// <param name="offsetIn">A position within <paramref name="inArray" />.</param>
    /// <param name="length">The number of elements of <paramref name="inArray" /> to convert.</param>
    /// <param name="outArray">An output array of Unicode characters.</param>
    /// <param name="offsetOut">A position within <paramref name="outArray" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> or <paramref name="outArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offsetIn" />, <paramref name="offsetOut" />, or <paramref name="length" /> is negative.
    /// 
    /// -or-
    /// 
    /// <paramref name="offsetIn" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="offsetOut" /> plus the number of elements to return is greater than the length of <paramref name="outArray" />.</exception>
    /// <returns>A 32-bit signed integer containing the number of bytes in <paramref name="outArray" />.</returns>
    public static int ToBase64CharArray(
      byte[] inArray,
      int offsetIn,
      int length,
      char[] outArray,
      int offsetOut)
    {
      return Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut, Base64FormattingOptions.None);
    }

    /// <summary>Converts a subset of an 8-bit unsigned integer array to an equivalent subset of a Unicode character array encoded with base-64 digits. Parameters specify the subsets as offsets in the input and output arrays, the number of elements in the input array to convert, and whether line breaks are inserted in the output array.</summary>
    /// <param name="inArray">An input array of 8-bit unsigned integers.</param>
    /// <param name="offsetIn">A position within <paramref name="inArray" />.</param>
    /// <param name="length">The number of elements of <paramref name="inArray" /> to convert.</param>
    /// <param name="outArray">An output array of Unicode characters.</param>
    /// <param name="offsetOut">A position within <paramref name="outArray" />.</param>
    /// <param name="options">
    /// <see cref="F:System.Base64FormattingOptions.InsertLineBreaks" /> to insert a line break every 76 characters, or <see cref="F:System.Base64FormattingOptions.None" /> to not insert line breaks.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> or <paramref name="outArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offsetIn" />, <paramref name="offsetOut" />, or <paramref name="length" /> is negative.
    /// 
    /// -or-
    /// 
    /// <paramref name="offsetIn" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="offsetOut" /> plus the number of elements to return is greater than the length of <paramref name="outArray" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> is not a valid <see cref="T:System.Base64FormattingOptions" /> value.</exception>
    /// <returns>A 32-bit signed integer containing the number of bytes in <paramref name="outArray" />.</returns>
    public static unsafe int ToBase64CharArray(
      byte[] inArray,
      int offsetIn,
      int length,
      char[] outArray,
      int offsetOut,
      Base64FormattingOptions options)
    {
      if (inArray == null)
        throw new ArgumentNullException(nameof (inArray));
      if (outArray == null)
        throw new ArgumentNullException(nameof (outArray));
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length), SR.ArgumentOutOfRange_Index);
      if (offsetIn < 0)
        throw new ArgumentOutOfRangeException(nameof (offsetIn), SR.ArgumentOutOfRange_GenericPositive);
      if (offsetOut < 0)
        throw new ArgumentOutOfRangeException(nameof (offsetOut), SR.ArgumentOutOfRange_GenericPositive);
      if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
        throw new ArgumentException(SR.Format(SR.Arg_EnumIllegalVal, (object) (int) options), nameof (options));
      int length1 = inArray.Length;
      if (offsetIn > length1 - length)
        throw new ArgumentOutOfRangeException(nameof (offsetIn), SR.ArgumentOutOfRange_OffsetLength);
      if (length1 == 0)
        return 0;
      bool insertLineBreaks = options == Base64FormattingOptions.InsertLineBreaks;
      int length2 = outArray.Length;
      int validateOutputLength = Convert.ToBase64_CalculateAndValidateOutputLength(length, insertLineBreaks);
      if (offsetOut > length2 - validateOutputLength)
        throw new ArgumentOutOfRangeException(nameof (offsetOut), SR.ArgumentOutOfRange_OffsetOut);
      int base64Array;
      fixed (char* outChars = &outArray[offsetOut])
        fixed (byte* inData = &inArray[0])
          base64Array = Convert.ConvertToBase64Array(outChars, inData, offsetIn, length, insertLineBreaks);
      return base64Array;
    }

    /// <summary>Tries to convert the 8-bit unsigned integers inside the specified read-only span into their equivalent string representation that is encoded with base-64 digits. You can optionally specify whether to insert line breaks in the return value.</summary>
    /// <param name="bytes">A read-only span of 8-bit unsigned integers.</param>
    /// <param name="chars">When this method returns <see langword="true" />, a span containing the string representation in base 64 of the elements in <paramref name="bytes" />. If the length of <paramref name="bytes" /> is 0, or when this method returns <paramref name="false" />, nothing is written into this parameter.</param>
    /// <param name="charsWritten">When this method returns, the total number of characters written into <paramref name="chars" />.</param>
    /// <param name="options">One of the enumeration values that specify whether to insert line breaks in the return value. The default value is <see cref="F:System.Base64FormattingOptions.None" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="options" /> is not a valid <see cref="T:System.Base64FormattingOptions" /> value.</exception>
    /// <returns>
    /// <see langword="true" /> if the conversion is successful; otherwise, <see langword="false" />.</returns>
    public static unsafe bool TryToBase64Chars(
      ReadOnlySpan<byte> bytes,
      Span<char> chars,
      out int charsWritten,
      Base64FormattingOptions options = Base64FormattingOptions.None)
    {
      if (options < Base64FormattingOptions.None || options > Base64FormattingOptions.InsertLineBreaks)
        throw new ArgumentException(SR.Format(SR.Arg_EnumIllegalVal, (object) (int) options), nameof (options));
      if (bytes.Length == 0)
      {
        charsWritten = 0;
        return true;
      }
      bool insertLineBreaks = options == Base64FormattingOptions.InsertLineBreaks;
      if (Convert.ToBase64_CalculateAndValidateOutputLength(bytes.Length, insertLineBreaks) > chars.Length)
      {
        charsWritten = 0;
        return false;
      }
      fixed (char* outChars = &MemoryMarshal.GetReference<char>(chars))
        fixed (byte* inData = &MemoryMarshal.GetReference<byte>(bytes))
        {
          charsWritten = Convert.ConvertToBase64Array(outChars, inData, 0, bytes.Length, insertLineBreaks);
          return true;
        }
    }


    #nullable disable
    private static unsafe int ConvertToBase64Array(
      char* outChars,
      byte* inData,
      int offset,
      int length,
      bool insertLineBreaks)
    {
      int num1 = length % 3;
      int num2 = offset + (length - num1);
      int base64Array = 0;
      int num3 = 0;
      fixed (char* chPtr1 = &Convert.base64Table[0])
      {
        for (int index = offset; index < num2; index += 3)
        {
          if (insertLineBreaks)
          {
            if (num3 == 76)
            {
              char* chPtr2 = outChars;
              int num4 = base64Array;
              int num5 = num4 + 1;
              IntPtr num6 = (IntPtr) num4 * 2;
              *(short*) ((IntPtr) chPtr2 + num6) = (short) 13;
              char* chPtr3 = outChars;
              int num7 = num5;
              base64Array = num7 + 1;
              IntPtr num8 = (IntPtr) num7 * 2;
              *(short*) ((IntPtr) chPtr3 + num8) = (short) 10;
              num3 = 0;
            }
            num3 += 4;
          }
          outChars[base64Array] = chPtr1[((int) inData[index] & 252) >> 2];
          outChars[base64Array + 1] = chPtr1[((int) inData[index] & 3) << 4 | ((int) inData[index + 1] & 240) >> 4];
          outChars[base64Array + 2] = chPtr1[((int) inData[index + 1] & 15) << 2 | ((int) inData[index + 2] & 192) >> 6];
          outChars[base64Array + 3] = chPtr1[(int) inData[index + 2] & 63];
          base64Array += 4;
        }
        int index1 = num2;
        if (insertLineBreaks && num1 != 0 && num3 == 76)
        {
          char* chPtr4 = outChars;
          int num9 = base64Array;
          int num10 = num9 + 1;
          IntPtr num11 = (IntPtr) num9 * 2;
          *(short*) ((IntPtr) chPtr4 + num11) = (short) 13;
          char* chPtr5 = outChars;
          int num12 = num10;
          base64Array = num12 + 1;
          IntPtr num13 = (IntPtr) num12 * 2;
          *(short*) ((IntPtr) chPtr5 + num13) = (short) 10;
        }
        switch (num1)
        {
          case 1:
            outChars[base64Array] = chPtr1[((int) inData[index1] & 252) >> 2];
            outChars[base64Array + 1] = chPtr1[((int) inData[index1] & 3) << 4];
            outChars[base64Array + 2] = chPtr1[64];
            outChars[base64Array + 3] = chPtr1[64];
            base64Array += 4;
            break;
          case 2:
            outChars[base64Array] = chPtr1[((int) inData[index1] & 252) >> 2];
            outChars[base64Array + 1] = chPtr1[((int) inData[index1] & 3) << 4 | ((int) inData[index1 + 1] & 240) >> 4];
            outChars[base64Array + 2] = chPtr1[((int) inData[index1 + 1] & 15) << 2];
            outChars[base64Array + 3] = chPtr1[64];
            base64Array += 4;
            break;
        }
      }
      return base64Array;
    }

    private static int ToBase64_CalculateAndValidateOutputLength(
      int inputLength,
      bool insertLineBreaks)
    {
      long left = ((long) inputLength + 2L) / 3L * 4L;
      if (left == 0L)
        return 0;
      if (insertLineBreaks)
      {
        (long Quotient, long Remainder) tuple = Math.DivRem(left, 76L);
        long quotient = tuple.Quotient;
        if (tuple.Remainder == 0L)
          --quotient;
        left += quotient * 2L;
      }
      return left <= (long) int.MaxValue ? (int) left : throw new OutOfMemoryException();
    }


    #nullable enable
    /// <summary>Converts the specified string, which encodes binary data as base-64 digits, to an equivalent 8-bit unsigned integer array.</summary>
    /// <param name="s">The string to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The length of <paramref name="s" />, ignoring white-space characters, is not zero or a multiple of 4.
    /// 
    /// -or-
    /// 
    /// The format of <paramref name="s" /> is invalid. <paramref name="s" /> contains a non-base-64 character, more than two padding characters, or a non-white space-character among the padding characters.</exception>
    /// <returns>An array of 8-bit unsigned integers that is equivalent to <paramref name="s" />.</returns>
    public static unsafe byte[] FromBase64String(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      IntPtr inputPtr;
      if (s == null)
      {
        inputPtr = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &s.GetPinnableReference())
          inputPtr = (IntPtr) chPtr;
      }
      return Convert.FromBase64CharPtr((char*) inputPtr, s.Length);
    }

    /// <summary>Tries to convert the specified string representation that is encoded with base-64 digits into a span of 8-bit unsigned integers.</summary>
    /// <param name="s">The string representation that is encoded with base-64 digits.</param>
    /// <param name="bytes">When this method returns <see langword="true" />, the converted 8-bit unsigned integers. When this method returns <see langword="false" />, either the span remains unmodified or contains an incomplete conversion of <paramref name="s" />, up to the last valid character.</param>
    /// <param name="bytesWritten">When this method returns, the number of bytes that were written in <paramref name="bytes" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the conversion was successful; otherwise, <see langword="false" />.</returns>
    public static bool TryFromBase64String(string s, Span<byte> bytes, out int bytesWritten)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Convert.TryFromBase64Chars(s.AsSpan(), bytes, out bytesWritten);
    }

    /// <summary>Tries to convert the specified span containing a string representation that is encoded with base-64 digits into a span of 8-bit unsigned integers.</summary>
    /// <param name="chars">A span containing the string representation that is encoded with base-64 digits.</param>
    /// <param name="bytes">When this method returns <see langword="true" />, the converted 8-bit unsigned integers. When this method returns <see langword="false" />, either the span remains unmodified or contains an incomplete conversion of <paramref name="chars" />, up to the last valid character.</param>
    /// <param name="bytesWritten">When this method returns, the number of bytes that were written in <paramref name="bytes" />.</param>
    /// <returns>
    /// <see langword="true" /> if the conversion was successful; otherwise, <see langword="false" />.</returns>
    public static unsafe bool TryFromBase64Chars(
      ReadOnlySpan<char> chars,
      Span<byte> bytes,
      out int bytesWritten)
    {
      // ISSUE: untyped stack allocation
      Span<char> span = new Span<char>((void*) __untypedstackalloc(new IntPtr(8)), 4);
      bytesWritten = 0;
      while (chars.Length != 0)
      {
        int consumed1;
        int written1;
        bool flag = Convert.TryDecodeFromUtf16(chars, bytes, out consumed1, out written1);
        bytesWritten += written1;
        if (flag)
          return true;
        chars = chars.Slice(consumed1);
        bytes = bytes.Slice(written1);
        if (chars[0].IsSpace())
        {
          int num = 1;
          while (num != chars.Length && chars[num].IsSpace())
            ++num;
          chars = chars.Slice(num);
          if (written1 % 3 != 0 && chars.Length != 0)
          {
            bytesWritten = 0;
            return false;
          }
        }
        else
        {
          int consumed2;
          int charsWritten;
          Convert.CopyToTempBufferWithoutWhiteSpace(chars, span, out consumed2, out charsWritten);
          if ((charsWritten & 3) != 0)
          {
            bytesWritten = 0;
            return false;
          }
          span = span.Slice(0, charsWritten);
          int written2;
          if (!Convert.TryDecodeFromUtf16((ReadOnlySpan<char>) span, bytes, out int _, out written2))
          {
            bytesWritten = 0;
            return false;
          }
          bytesWritten += written2;
          chars = chars.Slice(consumed2);
          bytes = bytes.Slice(written2);
          if (written2 % 3 != 0)
          {
            for (int index = 0; index < chars.Length; ++index)
            {
              if (!chars[index].IsSpace())
              {
                bytesWritten = 0;
                return false;
              }
            }
            return true;
          }
        }
      }
      return true;
    }


    #nullable disable
    private static void CopyToTempBufferWithoutWhiteSpace(
      ReadOnlySpan<char> chars,
      Span<char> tempBuffer,
      out int consumed,
      out int charsWritten)
    {
      charsWritten = 0;
      for (int index = 0; index < chars.Length; ++index)
      {
        char c = chars[index];
        if (!c.IsSpace())
        {
          tempBuffer[charsWritten++] = c;
          if (charsWritten == tempBuffer.Length)
          {
            consumed = index + 1;
            return;
          }
        }
      }
      consumed = chars.Length;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsSpace(this char c) => c == ' ' || c == '\t' || c == '\r' || c == '\n';


    #nullable enable
    /// <summary>Converts a subset of a Unicode character array, which encodes binary data as base-64 digits, to an equivalent 8-bit unsigned integer array. Parameters specify the subset in the input array and the number of elements to convert.</summary>
    /// <param name="inArray">A Unicode character array.</param>
    /// <param name="offset">A position within <paramref name="inArray" />.</param>
    /// <param name="length">The number of elements in <paramref name="inArray" /> to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="length" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="offset" /> plus <paramref name="length" /> indicates a position not within <paramref name="inArray" />.</exception>
    /// <exception cref="T:System.FormatException">The length of <paramref name="inArray" />, ignoring white-space characters, is not zero or a multiple of 4.
    /// 
    /// -or-
    /// 
    /// The format of <paramref name="inArray" /> is invalid. <paramref name="inArray" /> contains a non-base-64 character, more than two padding characters, or a non-white-space character among the padding characters.</exception>
    /// <returns>An array of 8-bit unsigned integers equivalent to <paramref name="length" /> elements at position <paramref name="offset" /> in <paramref name="inArray" />.</returns>
    public static unsafe byte[] FromBase64CharArray(char[] inArray, int offset, int length)
    {
      if (inArray == null)
        throw new ArgumentNullException(nameof (inArray));
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length), SR.ArgumentOutOfRange_Index);
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_GenericPositive);
      if (offset > inArray.Length - length)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_OffsetLength);
      if (inArray.Length == 0)
        return Array.Empty<byte>();
      fixed (char* chPtr = &inArray[0])
        return Convert.FromBase64CharPtr(chPtr + offset, length);
    }


    #nullable disable
    private static unsafe byte[] FromBase64CharPtr(char* inputPtr, int inputLength)
    {
      for (; inputLength > 0; --inputLength)
      {
        switch (inputPtr[inputLength - 1])
        {
          case '\t':
          case '\n':
          case '\r':
          case ' ':
            continue;
          default:
            goto label_4;
        }
      }
label_4:
      byte[] bytes = new byte[Convert.FromBase64_ComputeResultLength(inputPtr, inputLength)];
      if (!Convert.TryFromBase64Chars(new ReadOnlySpan<char>((void*) inputPtr, inputLength), (Span<byte>) bytes, out int _))
        throw new FormatException(SR.Format_BadBase64Char);
      return bytes;
    }

    private static unsafe int FromBase64_ComputeResultLength(char* inputPtr, int inputLength)
    {
      char* chPtr = inputPtr + inputLength;
      int num1 = inputLength;
      int num2 = 0;
      while (inputPtr < chPtr)
      {
        uint num3 = (uint) *inputPtr;
        ++inputPtr;
        if (num3 <= 32U)
          --num1;
        else if (num3 == 61U)
        {
          --num1;
          ++num2;
        }
      }
      switch (num2)
      {
        case 0:
          return num1 / 4 * 3 + num2;
        case 1:
          num2 = 2;
          goto case 0;
        case 2:
          num2 = 1;
          goto case 0;
        default:
          throw new FormatException(SR.Format_BadBase64Char);
      }
    }


    #nullable enable
    /// <summary>Converts the specified string, which encodes binary data as hex characters, to an equivalent 8-bit unsigned integer array.</summary>
    /// <param name="s">The string to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="s" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.FormatException">The length of <paramref name="s" />, is not zero or a multiple of 2.</exception>
    /// <exception cref="T:System.FormatException">The format of <paramref name="s" /> is invalid. <paramref name="s" /> contains a non-hex character.</exception>
    /// <returns>An array of 8-bit unsigned integers that is equivalent to <paramref name="s" />.</returns>
    public static byte[] FromHexString(string s)
    {
      if (s == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
      return Convert.FromHexString(s.AsSpan());
    }

    /// <summary>Converts the span, which encodes binary data as hex characters, to an equivalent 8-bit unsigned integer array.</summary>
    /// <param name="chars">The span to convert.</param>
    /// <exception cref="T:System.FormatException">The length of <paramref name="chars" />, is not zero or a multiple of 2.</exception>
    /// <exception cref="T:System.FormatException">The format of <paramref name="chars" /> is invalid. <paramref name="chars" /> contains a non-hex character.</exception>
    /// <returns>An array of 8-bit unsigned integers that is equivalent to <paramref name="chars" />.</returns>
    public static byte[] FromHexString(ReadOnlySpan<char> chars)
    {
      if (chars.Length == 0)
        return Array.Empty<byte>();
      byte[] bytes = (uint) chars.Length % 2U == 0U ? GC.AllocateUninitializedArray<byte>(chars.Length >> 1) : throw new FormatException(SR.Format_BadHexLength);
      return HexConverter.TryDecodeFromUtf16(chars, (Span<byte>) bytes) ? bytes : throw new FormatException(SR.Format_BadHexChar);
    }

    /// <summary>Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with uppercase hex characters.</summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="inArray" /> is too large to be encoded.</exception>
    /// <returns>The string representation in hex of the elements in <paramref name="inArray" />.</returns>
    public static string ToHexString(byte[] inArray) => inArray != null ? Convert.ToHexString(new ReadOnlySpan<byte>(inArray)) : throw new ArgumentNullException(nameof (inArray));

    /// <summary>Converts a subset of an array of 8-bit unsigned integers to its equivalent string representation that is encoded with uppercase hex characters.
    /// Parameters specify the subset as an offset in the input array and the number of elements in the array to convert.</summary>
    /// <param name="inArray">An array of 8-bit unsigned integers.</param>
    /// <param name="offset">An offset in <paramref name="inArray" />.</param>
    /// <param name="length">The number of elements of <paramref name="inArray" /> to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inArray" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="length" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> plus <paramref name="length" /> is greater than the length of <paramref name="inArray" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="inArray" /> is too large to be encoded.</exception>
    /// <returns>The string representation in hex of <paramref name="length" /> elements of <paramref name="inArray" />, starting at position <paramref name="offset" />.</returns>
    public static string ToHexString(byte[] inArray, int offset, int length)
    {
      if (inArray == null)
        throw new ArgumentNullException(nameof (inArray));
      if (length < 0)
        throw new ArgumentOutOfRangeException(nameof (length), SR.ArgumentOutOfRange_Index);
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_GenericPositive);
      if (offset > inArray.Length - length)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_OffsetLength);
      return Convert.ToHexString(new ReadOnlySpan<byte>(inArray, offset, length));
    }

    /// <summary>Converts a span of 8-bit unsigned integers to its equivalent string representation that is encoded with uppercase hex characters.</summary>
    /// <param name="bytes">A span of 8-bit unsigned integers.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bytes" /> is too large to be encoded.</exception>
    /// <returns>The string representation in hex of the elements in <paramref name="bytes" />.</returns>
    public static string ToHexString(ReadOnlySpan<byte> bytes)
    {
      if (bytes.Length == 0)
        return string.Empty;
      return bytes.Length <= 1073741823 ? HexConverter.ToString(bytes) : throw new ArgumentOutOfRangeException(nameof (bytes), SR.ArgumentOutOfRange_InputTooLarge);
    }
  }
}
