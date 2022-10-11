// Decompiled with JetBrains decompiler
// Type: System.Enum
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Provides the base class for enumerations.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public abstract class Enum : ValueType, IComparable, IFormattable, IConvertible
  {
    private const char EnumSeparatorChar = ',';

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void GetEnumValuesAndNames(
      QCallTypeHandle enumType,
      ObjectHandleOnStack values,
      ObjectHandleOnStack names,
      Interop.BOOL getNames);

    /// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is an enumeration value of the same type and with the same underlying value as this instance; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public override extern bool Equals([NotNullWhen(true)] object? obj);


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern object InternalBoxEnum(RuntimeType enumType, long value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern CorElementType InternalGetCorElementType();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern RuntimeType InternalGetUnderlyingType(RuntimeType enumType);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern bool InternalHasFlag(Enum flags);

    private static Enum.EnumInfo GetEnumInfo(RuntimeType enumType, bool getNames = true)
    {
      if (!(enumType.GenericCache is Enum.EnumInfo enumInfo) || getNames && enumInfo.Names == null)
      {
        ulong[] o1 = (ulong[]) null;
        string[] o2 = (string[]) null;
        RuntimeTypeHandle typeHandleInternal = enumType.GetTypeHandleInternal();
        Enum.GetEnumValuesAndNames(new QCallTypeHandle(ref typeHandleInternal), ObjectHandleOnStack.Create<ulong[]>(ref o1), ObjectHandleOnStack.Create<string[]>(ref o2), getNames ? Interop.BOOL.TRUE : Interop.BOOL.FALSE);
        enumInfo = new Enum.EnumInfo(enumType.IsDefined(typeof (FlagsAttribute), false), o1, o2);
        enumType.GenericCache = (object) enumInfo;
      }
      return enumInfo;
    }

    private string ValueToString()
    {
      ref byte local = ref this.GetRawData();
      switch (this.InternalGetCorElementType())
      {
        case CorElementType.ELEMENT_TYPE_BOOLEAN:
          return Unsafe.As<byte, bool>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_CHAR:
          return Unsafe.As<byte, char>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_I1:
          return Unsafe.As<byte, sbyte>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_U1:
          return local.ToString();
        case CorElementType.ELEMENT_TYPE_I2:
          return Unsafe.As<byte, short>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_U2:
          return Unsafe.As<byte, ushort>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_I4:
          return Unsafe.As<byte, int>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_U4:
          return Unsafe.As<byte, uint>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_I8:
          return Unsafe.As<byte, long>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_U8:
          return Unsafe.As<byte, ulong>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_R4:
          return Unsafe.As<byte, float>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_R8:
          return Unsafe.As<byte, double>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_I:
          return Unsafe.As<byte, IntPtr>(ref local).ToString();
        case CorElementType.ELEMENT_TYPE_U:
          return Unsafe.As<byte, UIntPtr>(ref local).ToString();
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
    }

    private unsafe string ValueToHexString()
    {
      ref byte local = ref this.GetRawData();
      // ISSUE: untyped stack allocation
      Span<byte> destination = new Span<byte>((void*) __untypedstackalloc(new IntPtr(8)), 8);
      int length;
      switch (this.InternalGetCorElementType())
      {
        case CorElementType.ELEMENT_TYPE_BOOLEAN:
          return local == (byte) 0 ? "00" : "01";
        case CorElementType.ELEMENT_TYPE_CHAR:
        case CorElementType.ELEMENT_TYPE_I2:
        case CorElementType.ELEMENT_TYPE_U2:
          BinaryPrimitives.WriteUInt16BigEndian(destination, Unsafe.As<byte, ushort>(ref local));
          length = 2;
          break;
        case CorElementType.ELEMENT_TYPE_I1:
        case CorElementType.ELEMENT_TYPE_U1:
          destination[0] = local;
          length = 1;
          break;
        case CorElementType.ELEMENT_TYPE_I4:
        case CorElementType.ELEMENT_TYPE_U4:
          BinaryPrimitives.WriteUInt32BigEndian(destination, Unsafe.As<byte, uint>(ref local));
          length = 4;
          break;
        case CorElementType.ELEMENT_TYPE_I8:
        case CorElementType.ELEMENT_TYPE_U8:
          BinaryPrimitives.WriteUInt64BigEndian(destination, Unsafe.As<byte, ulong>(ref local));
          length = 8;
          break;
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
      return HexConverter.ToString((ReadOnlySpan<byte>) destination.Slice(0, length));
    }

    private static string ValueToHexString(object value)
    {
      switch (Convert.GetTypeCode(value))
      {
        case TypeCode.Boolean:
          return (bool) value ? "01" : "00";
        case TypeCode.Char:
          return ((ushort) (char) value).ToString("X4", (IFormatProvider) null);
        case TypeCode.SByte:
          return ((byte) (sbyte) value).ToString("X2", (IFormatProvider) null);
        case TypeCode.Byte:
          return ((byte) value).ToString("X2", (IFormatProvider) null);
        case TypeCode.Int16:
          return ((ushort) (short) value).ToString("X4", (IFormatProvider) null);
        case TypeCode.UInt16:
          return ((ushort) value).ToString("X4", (IFormatProvider) null);
        case TypeCode.Int32:
          return ((uint) (int) value).ToString("X8", (IFormatProvider) null);
        case TypeCode.UInt32:
          return ((uint) value).ToString("X8", (IFormatProvider) null);
        case TypeCode.Int64:
          return ((ulong) (long) value).ToString("X16", (IFormatProvider) null);
        case TypeCode.UInt64:
          return ((ulong) value).ToString("X16", (IFormatProvider) null);
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
    }

    internal static string GetEnumName(RuntimeType enumType, ulong ulValue) => Enum.GetEnumName(Enum.GetEnumInfo(enumType), ulValue);

    private static string GetEnumName(Enum.EnumInfo enumInfo, ulong ulValue)
    {
      int index = Array.BinarySearch<ulong>(enumInfo.Values, ulValue);
      return index >= 0 ? enumInfo.Names[index] : (string) null;
    }

    private static string InternalFormat(RuntimeType enumType, ulong value)
    {
      Enum.EnumInfo enumInfo = Enum.GetEnumInfo(enumType);
      return !enumInfo.HasFlagsAttribute ? Enum.GetEnumName(enumInfo, value) : Enum.InternalFlagsFormat(enumInfo, value);
    }

    private static string InternalFlagsFormat(RuntimeType enumType, ulong result) => Enum.InternalFlagsFormat(Enum.GetEnumInfo(enumType), result);

    private static unsafe string InternalFlagsFormat(Enum.EnumInfo enumInfo, ulong resultValue)
    {
      string[] names = enumInfo.Names;
      ulong[] values = enumInfo.Values;
      if (resultValue == 0UL)
        return values.Length == 0 || values[0] != 0UL ? "0" : names[0];
      // ISSUE: untyped stack allocation
      Span<int> span = new Span<int>((void*) __untypedstackalloc(new IntPtr(256)), 64);
      int index1;
      for (index1 = values.Length - 1; index1 >= 0; --index1)
      {
        if ((long) values[index1] == (long) resultValue)
          return names[index1];
        if (values[index1] < resultValue)
          break;
      }
      int num1 = 0;
      int num2 = 0;
      for (; index1 >= 0; --index1)
      {
        ulong num3 = values[index1];
        if (index1 != 0 || num3 != 0UL)
        {
          if (((long) resultValue & (long) num3) == (long) num3)
          {
            resultValue -= num3;
            span[num2++] = index1;
            checked { num1 += names[index1].Length; }
          }
        }
        else
          break;
      }
      if (resultValue != 0UL)
        return (string) null;
      string str1 = string.FastAllocateString(checked (num1 + 2 * (num2 - 1)));
      Span<char> destination = new Span<char>(ref str1.GetRawStringData(), str1.Length);
      int index2;
      string str2 = names[span[index2 = num2 - 1]];
      str2.CopyTo(destination);
      destination = destination.Slice(str2.Length);
      while (--index2 >= 0)
      {
        destination[0] = ',';
        destination[1] = ' ';
        destination = destination.Slice(2);
        string str3 = names[span[index2]];
        str3.CopyTo(destination);
        destination = destination.Slice(str3.Length);
      }
      return str1;
    }

    internal static ulong ToUInt64(object value)
    {
      ulong uint64;
      switch (Convert.GetTypeCode(value))
      {
        case TypeCode.Boolean:
          uint64 = (bool) value ? 1UL : 0UL;
          break;
        case TypeCode.Char:
          uint64 = (ulong) (char) value;
          break;
        case TypeCode.SByte:
          uint64 = (ulong) (sbyte) value;
          break;
        case TypeCode.Byte:
          uint64 = (ulong) (byte) value;
          break;
        case TypeCode.Int16:
          uint64 = (ulong) (short) value;
          break;
        case TypeCode.UInt16:
          uint64 = (ulong) (ushort) value;
          break;
        case TypeCode.Int32:
          uint64 = (ulong) (int) value;
          break;
        case TypeCode.UInt32:
          uint64 = (ulong) (uint) value;
          break;
        case TypeCode.Int64:
          uint64 = (ulong) (long) value;
          break;
        case TypeCode.UInt64:
          uint64 = (ulong) value;
          break;
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
      return uint64;
    }

    private static ulong ToUInt64<TEnum>(TEnum value) where TEnum : struct, Enum
    {
      switch (Type.GetTypeCode(typeof (TEnum)))
      {
        case TypeCode.Boolean:
          return Unsafe.As<TEnum, bool>(ref value) ? 1UL : 0UL;
        case TypeCode.Char:
          return (ulong) Unsafe.As<TEnum, char>(ref value);
        case TypeCode.SByte:
          return (ulong) Unsafe.As<TEnum, sbyte>(ref value);
        case TypeCode.Byte:
          return (ulong) Unsafe.As<TEnum, byte>(ref value);
        case TypeCode.Int16:
          return (ulong) Unsafe.As<TEnum, short>(ref value);
        case TypeCode.UInt16:
          return (ulong) Unsafe.As<TEnum, ushort>(ref value);
        case TypeCode.Int32:
          return (ulong) Unsafe.As<TEnum, int>(ref value);
        case TypeCode.UInt32:
          return (ulong) Unsafe.As<TEnum, uint>(ref value);
        case TypeCode.Int64:
          return (ulong) Unsafe.As<TEnum, long>(ref value);
        case TypeCode.UInt64:
          return Unsafe.As<TEnum, ulong>(ref value);
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
    }


    #nullable enable
    /// <summary>Retrieves the name of the constant in the specified enumeration type that has the specified value.</summary>
    /// <param name="value">The value of a particular enumerated constant in terms of its underlying type.</param>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <returns>A string containing the name of the enumerated constant in <paramref name="enumType" /> whose value is <paramref name="value" />; or <see langword="null" /> if no such constant is found.</returns>
    public static string? GetName<TEnum>(TEnum value) where TEnum : struct, Enum => Enum.GetEnumName((RuntimeType) typeof (TEnum), Enum.ToUInt64<TEnum>(value));

    /// <summary>Retrieves the name of the constant in the specified enumeration that has the specified value.</summary>
    /// <param name="enumType">An enumeration type.</param>
    /// <param name="value">The value of a particular enumerated constant in terms of its underlying type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is neither of type <paramref name="enumType" /> nor does it have the same underlying type as <paramref name="enumType" />.</exception>
    /// <returns>A string containing the name of the enumerated constant in <paramref name="enumType" /> whose value is <paramref name="value" />; or <see langword="null" /> if no such constant is found.</returns>
    public static string? GetName(Type enumType, object value) => (object) enumType != null ? enumType.GetEnumName(value) : throw new ArgumentNullException(nameof (enumType));

    /// <summary>Retrieves an array of the names of the constants in a specified enumeration type.</summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <returns>A string array of the names of the constants in <typeparamref name="TEnum" />.</returns>
    public static string[] GetNames<TEnum>() where TEnum : struct, Enum => new ReadOnlySpan<string>(Enum.InternalGetNames((RuntimeType) typeof (TEnum))).ToArray();

    /// <summary>Retrieves an array of the names of the constants in a specified enumeration.</summary>
    /// <param name="enumType">An enumeration type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> parameter is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>A string array of the names of the constants in <paramref name="enumType" />.</returns>
    public static string[] GetNames(Type enumType) => (object) enumType != null ? enumType.GetEnumNames() : throw new ArgumentNullException(nameof (enumType));


    #nullable disable
    internal static string[] InternalGetNames(RuntimeType enumType) => Enum.GetEnumInfo(enumType).Names;


    #nullable enable
    /// <summary>Returns the underlying type of the specified enumeration.</summary>
    /// <param name="enumType">The enumeration whose underlying type will be retrieved.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>The underlying type of <paramref name="enumType" />.</returns>
    public static Type GetUnderlyingType(Type enumType) => !(enumType == (Type) null) ? enumType.GetEnumUnderlyingType() : throw new ArgumentNullException(nameof (enumType));

    /// <summary>Retrieves an array of the values of the constants in a specified enumeration type.</summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <returns>An array that contains the values of the constants in <typeparamref name="TEnum" />.</returns>
    public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum => (TEnum[]) Enum.GetValues(typeof (TEnum));

    /// <summary>Retrieves an array of the values of the constants in a specified enumeration.</summary>
    /// <param name="enumType">An enumeration type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The method is invoked by reflection in a reflection-only context,
    /// 
    /// -or-
    /// 
    /// <paramref name="enumType" /> is a type from an assembly loaded in a reflection-only context.</exception>
    /// <returns>An array that contains the values of the constants in <paramref name="enumType" />.</returns>
    public static Array GetValues(Type enumType) => (object) enumType != null ? enumType.GetEnumValues() : throw new ArgumentNullException(nameof (enumType));

    /// <summary>Determines whether one or more bit fields are set in the current instance.</summary>
    /// <param name="flag">An enumeration value.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="flag" /> is a different type than the current instance.</exception>
    /// <returns>
    /// <see langword="true" /> if the bit field or bit fields that are set in <paramref name="flag" /> are also set in the current instance; otherwise, <see langword="false" />.</returns>
    [Intrinsic]
    public bool HasFlag(Enum flag)
    {
      if (flag == null)
        throw new ArgumentNullException(nameof (flag));
      return this.GetType().IsEquivalentTo(flag.GetType()) ? this.InternalHasFlag(flag) : throw new ArgumentException(SR.Format(SR.Argument_EnumTypeDoesNotMatch, (object) flag.GetType(), (object) this.GetType()));
    }


    #nullable disable
    internal static ulong[] InternalGetValues(RuntimeType enumType) => Enum.GetEnumInfo(enumType, false).Values;


    #nullable enable
    /// <summary>Returns a boolean telling whether a given integral value, or its name as a string, exists in a specified enumeration.</summary>
    /// <param name="value">The value or name of a constant in <typeparamref name="TEnum" />.</param>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    /// <returns>
    /// <see langword="true" /> if a given integral value, or its name as a string, exists in a specified enumeration; <see langword="false" /> otherwise.</returns>
    public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, Enum => Array.BinarySearch<ulong>(Enum.InternalGetValues((RuntimeType) typeof (TEnum)), Enum.ToUInt64<TEnum>(value)) >= 0;

    /// <summary>Returns a Boolean telling whether a given integral value, or its name as a string, exists in a specified enumeration.</summary>
    /// <param name="enumType">An enumeration type.</param>
    /// <param name="value">The value or name of a constant in <paramref name="enumType" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="enumType" /> is not an <see langword="Enum" />.
    /// 
    /// -or-
    /// 
    /// The type of <paramref name="value" /> is an enumeration, but it is not an enumeration of type <paramref name="enumType" />.
    /// 
    /// -or-
    /// 
    /// The type of <paramref name="value" /> is not an underlying type of <paramref name="enumType" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="value" /> is not type <see cref="T:System.SByte" />, <see cref="T:System.Int16" />, <see cref="T:System.Int32" />, <see cref="T:System.Int64" />, <see cref="T:System.Byte" />, <see cref="T:System.UInt16" />, <see cref="T:System.UInt32" />, or <see cref="T:System.UInt64" />, or <see cref="T:System.String" />.</exception>
    /// <returns>
    /// <see langword="true" /> if a constant in <paramref name="enumType" /> has a value equal to <paramref name="value" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(Type enumType, object value) => (object) enumType != null ? enumType.IsEnumDefined(value) : throw new ArgumentNullException(nameof (enumType));

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.</summary>
    /// <param name="enumType">An enumeration type.</param>
    /// <param name="value">A string containing the name or value to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is either an empty string or only contains white space.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is outside the range of the underlying type of <paramref name="enumType" />.</exception>
    /// <returns>An object of type <paramref name="enumType" /> whose value is represented by <paramref name="value" />.</returns>
    public static object Parse(Type enumType, string value) => Enum.Parse(enumType, value, false);

    /// <summary>Converts the span of chars representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.</summary>
    /// <param name="enumType">An enumeration type.</param>
    /// <param name="value">A span containing the name or value to convert.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is either an empty string or only contains white space.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is outside the range of the underlying type of <paramref name="enumType" /></exception>
    /// <returns>An object of type <paramref name="enumType" /> whose value is represented by <paramref name="value" />.</returns>
    public static object Parse(Type enumType, ReadOnlySpan<char> value) => Enum.Parse(enumType, value, false);

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.</summary>
    /// <param name="enumType">An enumeration type.</param>
    /// <param name="value">A string containing the name or value to convert.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore case; <see langword="false" /> to regard case.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is either an empty string ("") or only contains white space.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is outside the range of the underlying type of <paramref name="enumType" />.</exception>
    /// <returns>An object of type <paramref name="enumType" /> whose value is represented by <paramref name="value" />.</returns>
    public static object Parse(Type enumType, string value, bool ignoreCase)
    {
      object result;
      Enum.TryParse(enumType, value, ignoreCase, true, out result);
      return result;
    }

    /// <summary>Converts the span of chars representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.</summary>
    /// <param name="enumType">An enumeration type.</param>
    /// <param name="value">A span containing the name or value to convert.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore case; <see langword="false" /> to regard case.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is either an empty string or only contains white space.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> is a name, but not one of the named constants defined for the enumeration.</exception>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> is outside the range of the underlying type of <paramref name="enumType" /></exception>
    /// <returns>An object of type <paramref name="enumType" /> whose value is represented by <paramref name="value" />.</returns>
    public static object Parse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase)
    {
      object result;
      Enum.TryParse(enumType, value, ignoreCase, true, out result);
      return result;
    }

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants specified by <typeparamref name="TEnum" /> to an equivalent enumerated object.</summary>
    /// <param name="value">A string containing the name or value to convert.</param>
    /// <typeparam name="TEnum">An enumeration type.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <typeparamref name="TEnum" /> is not an <see cref="T:System.Enum" /> type.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> does not contain enumeration information.</exception>
    /// <returns>An object of type <paramref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
    public static TEnum Parse<TEnum>(string value) where TEnum : struct => Enum.Parse<TEnum>(value, false);

    /// <summary>Converts the span of chars representation of the name or numeric value of one or more enumerated constants specified by <typeparamref name="TEnum" /> to an equivalent enumerated object.</summary>
    /// <param name="value">A span containing the name or value to convert.</param>
    /// <typeparam name="TEnum">An enumeration type.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <typeparamref name="TEnum" /> is not an <see cref="T:System.Enum" /> type</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> does not contain enumeration information</exception>
    /// <returns>
    /// <typeparamref name="TEnum" /> An object of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
    public static TEnum Parse<TEnum>(ReadOnlySpan<char> value) where TEnum : struct => Enum.Parse<TEnum>(value, false);

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants specified by <typeparamref name="TEnum" /> to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.</summary>
    /// <param name="value">A string containing the name or value to convert.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore case; <see langword="false" /> to regard case.</param>
    /// <typeparam name="TEnum">An enumeration type.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <typeparamref name="TEnum" /> is not an <see cref="T:System.Enum" /> type.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> does not contain enumeration information.</exception>
    /// <returns>An object of type <paramref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
    public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct
    {
      TEnum result;
      Enum.TryParse<TEnum>(value, ignoreCase, true, out result);
      return result;
    }

    /// <summary>Converts the span of chars representation of the name or numeric value of one or more enumerated constants specified by <typeparamref name="TEnum" /> to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.</summary>
    /// <param name="value">A span containing the name or value to convert.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore case; <see langword="false" /> to regard case.</param>
    /// <typeparam name="TEnum">An enumeration type.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <typeparamref name="TEnum" /> is not an <see cref="T:System.Enum" /> type</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="value" /> does not contain enumeration information.</exception>
    /// <returns>
    /// <typeparamref name="TEnum" /> An object of type <typeparamref name="TEnum" /> whose value is represented by <paramref name="value" />.</returns>
    public static TEnum Parse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase) where TEnum : struct
    {
      TEnum result;
      Enum.TryParse<TEnum>(value, ignoreCase, true, out result);
      return result;
    }

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.</summary>
    /// <param name="enumType">The enum type to use for parsing.</param>
    /// <param name="value">The string representation of the name or numeric value of one or more enumerated constants.</param>
    /// <param name="result">When this method returns <see langword="true" />, an object containing an enumeration constant representing the parsed value.</param>
    /// <returns>
    /// <see langword="true" /> if the conversion succeeded; <see langword="false" /> otherwise.</returns>
    public static bool TryParse(Type enumType, string? value, out object? result) => Enum.TryParse(enumType, value, false, out result);

    /// <summary>Converts the span of chars representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.</summary>
    /// <param name="enumType">The enum type to use for parsing.</param>
    /// <param name="value">The span representation of the name or numeric value of one or more enumerated constants.</param>
    /// <param name="result">When this method returns <see langword="true" />, an object containing an enumeration constant representing the parsed value.</param>
    /// <returns>
    /// <see langword="true" /> if the conversion succeeded; <see langword="false" /> otherwise.</returns>
    public static bool TryParse(Type enumType, ReadOnlySpan<char> value, out object? result) => Enum.TryParse(enumType, value, false, out result);

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.</summary>
    /// <param name="enumType">The enum type to use for parsing.</param>
    /// <param name="value">The string representation of the name or numeric value of one or more enumerated constants.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to read <paramref name="value" /> in case insensitive mode; <see langword="false" /> to read <paramref name="value" /> in case sensitive mode.</param>
    /// <param name="result">When this method returns <see langword="true" />, an object containing an enumeration constant representing the parsed value.</param>
    /// <returns>
    /// <see langword="true" /> if the conversion succeeded; <see langword="false" /> otherwise.</returns>
    public static bool TryParse(Type enumType, string? value, bool ignoreCase, out object? result) => Enum.TryParse(enumType, value, ignoreCase, false, out result);

    /// <summary>Converts the span of chars representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.</summary>
    /// <param name="enumType">The enum type to use for parsing.</param>
    /// <param name="value">The span representation of the name or numeric value of one or more enumerated constants.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to read <paramref name="enumType" /> in case insensitive mode; <see langword="false" /> to read <paramref name="enumType" /> in case sensitive mode.</param>
    /// <param name="result">When this method returns <see langword="true" />, an object containing an enumeration constant representing the parsed value.</param>
    /// <returns>
    /// <see langword="true" /> if the conversion succeeded; <see langword="false" /> otherwise.</returns>
    public static bool TryParse(
      Type enumType,
      ReadOnlySpan<char> value,
      bool ignoreCase,
      out object? result)
    {
      return Enum.TryParse(enumType, value, ignoreCase, false, out result);
    }


    #nullable disable
    private static bool TryParse(
      Type enumType,
      string value,
      bool ignoreCase,
      bool throwOnFailure,
      out object result)
    {
      if (value != null)
        return Enum.TryParse(enumType, value.AsSpan(), ignoreCase, throwOnFailure, out result);
      if (throwOnFailure)
        throw new ArgumentNullException(nameof (value));
      result = (object) null;
      return false;
    }

    private static bool TryParse(
      Type enumType,
      ReadOnlySpan<char> value,
      bool ignoreCase,
      bool throwOnFailure,
      out object result)
    {
      RuntimeType enumType1 = Enum.ValidateRuntimeType(enumType);
      value = value.TrimStart();
      if (value.Length == 0)
      {
        if (throwOnFailure)
          throw new ArgumentException(SR.Arg_MustContainEnumInfo, nameof (value));
        result = (object) null;
        return false;
      }
      switch (Type.GetTypeCode((Type) enumType1))
      {
        case TypeCode.SByte:
          int result1;
          bool int32Enum1 = Enum.TryParseInt32Enum(enumType1, value, (int) sbyte.MinValue, (int) sbyte.MaxValue, ignoreCase, throwOnFailure, TypeCode.SByte, out result1);
          result = int32Enum1 ? Enum.InternalBoxEnum(enumType1, (long) result1) : (object) null;
          return int32Enum1;
        case TypeCode.Byte:
          uint result2;
          bool uint32Enum1 = Enum.TryParseUInt32Enum(enumType1, value, (uint) byte.MaxValue, ignoreCase, throwOnFailure, TypeCode.Byte, out result2);
          result = uint32Enum1 ? Enum.InternalBoxEnum(enumType1, (long) result2) : (object) null;
          return uint32Enum1;
        case TypeCode.Int16:
          int result3;
          bool int32Enum2 = Enum.TryParseInt32Enum(enumType1, value, (int) short.MinValue, (int) short.MaxValue, ignoreCase, throwOnFailure, TypeCode.Int16, out result3);
          result = int32Enum2 ? Enum.InternalBoxEnum(enumType1, (long) result3) : (object) null;
          return int32Enum2;
        case TypeCode.UInt16:
          uint result4;
          bool uint32Enum2 = Enum.TryParseUInt32Enum(enumType1, value, (uint) ushort.MaxValue, ignoreCase, throwOnFailure, TypeCode.UInt16, out result4);
          result = uint32Enum2 ? Enum.InternalBoxEnum(enumType1, (long) result4) : (object) null;
          return uint32Enum2;
        case TypeCode.Int32:
          int result5;
          bool int32Enum3 = Enum.TryParseInt32Enum(enumType1, value, int.MinValue, int.MaxValue, ignoreCase, throwOnFailure, TypeCode.Int32, out result5);
          result = int32Enum3 ? Enum.InternalBoxEnum(enumType1, (long) result5) : (object) null;
          return int32Enum3;
        case TypeCode.UInt32:
          uint result6;
          bool uint32Enum3 = Enum.TryParseUInt32Enum(enumType1, value, uint.MaxValue, ignoreCase, throwOnFailure, TypeCode.UInt32, out result6);
          result = uint32Enum3 ? Enum.InternalBoxEnum(enumType1, (long) result6) : (object) null;
          return uint32Enum3;
        case TypeCode.Int64:
          long result7;
          bool int64Enum = Enum.TryParseInt64Enum(enumType1, value, ignoreCase, throwOnFailure, out result7);
          result = int64Enum ? Enum.InternalBoxEnum(enumType1, result7) : (object) null;
          return int64Enum;
        case TypeCode.UInt64:
          ulong result8;
          bool uint64Enum = Enum.TryParseUInt64Enum(enumType1, value, ignoreCase, throwOnFailure, out result8);
          result = uint64Enum ? Enum.InternalBoxEnum(enumType1, (long) result8) : (object) null;
          return uint64Enum;
        default:
          return Enum.TryParseRareEnum(enumType1, value, ignoreCase, throwOnFailure, out result);
      }
    }


    #nullable enable
    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. The return value indicates whether the conversion succeeded.</summary>
    /// <param name="value">The case-sensitive string representation of the enumeration name or underlying value to convert.</param>
    /// <param name="result">When this method returns, <paramref name="result" /> contains an object of type <c>TEnum</c> whose value is represented by <paramref name="value" /> if the parse operation succeeds. If the parse operation fails, <paramref name="result" /> contains the default value of the underlying type of <c>TEnum</c>. Note that this value need not be a member of the <c>TEnum</c> enumeration. This parameter is passed uninitialized.</param>
    /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value" />.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="TEnum" /> is not an enumeration type.</exception>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="value" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse<TEnum>([NotNullWhen(true)] string? value, out TEnum result) where TEnum : struct => Enum.TryParse<TEnum>(value, false, out result);

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object.</summary>
    /// <param name="value">The span representation of the name or numeric value of one or more enumerated constants.</param>
    /// <param name="result">When this method returns <see langword="true" />, an object containing an enumeration constant representing the parsed value.</param>
    /// <typeparam name="TEnum">The type of the <paramref name="result" /> object.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <typeparamref name="TEnum" /> is not an enumeration type.</exception>
    /// <returns>
    /// <see langword="true" /> if the conversion succeeded; <see langword="false" /> otherwise.</returns>
    public static bool TryParse<TEnum>(ReadOnlySpan<char> value, out TEnum result) where TEnum : struct => Enum.TryParse<TEnum>(value, false, out result);

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-sensitive. The return value indicates whether the conversion succeeded.</summary>
    /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore case; <see langword="false" /> to consider case.</param>
    /// <param name="result">When this method returns, <paramref name="result" /> contains an object of type <c>TEnum</c> whose value is represented by <paramref name="value" /> if the parse operation succeeds. If the parse operation fails, <paramref name="result" /> contains the default value of the underlying type of <c>TEnum</c>. Note that this value need not be a member of the <c>TEnum</c> enumeration. This parameter is passed uninitialized.</param>
    /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value" />.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="TEnum" /> is not an enumeration type.</exception>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="value" /> parameter was converted successfully; otherwise, <see langword="false" />.</returns>
    public static bool TryParse<TEnum>([NotNullWhen(true)] string? value, bool ignoreCase, out TEnum result) where TEnum : struct => Enum.TryParse<TEnum>(value, ignoreCase, false, out result);

    /// <summary>Converts the string representation of the name or numeric value of one or more enumerated constants to an equivalent enumerated object. A parameter specifies whether the operation is case-sensitive. The return value indicates whether the conversion succeeded.</summary>
    /// <param name="value">The span representation of the name or numeric value of one or more enumerated constants.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore case; <see langword="false" /> to consider case.</param>
    /// <param name="result">When this method returns <see langword="true" />, an object containing an enumeration constant representing the parsed value.</param>
    /// <typeparam name="TEnum">The type of the <paramref name="result" /> object.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <typeparamref name="TEnum" /> is not an enumeration type.</exception>
    /// <returns>
    /// <see langword="true" /> if the conversion succeeded; <see langword="false" /> otherwise.</returns>
    public static bool TryParse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result) where TEnum : struct => Enum.TryParse<TEnum>(value, ignoreCase, false, out result);


    #nullable disable
    private static bool TryParse<TEnum>(
      string value,
      bool ignoreCase,
      bool throwOnFailure,
      out TEnum result)
      where TEnum : struct
    {
      if (value != null)
        return Enum.TryParse<TEnum>(value.AsSpan(), ignoreCase, throwOnFailure, out result);
      if (throwOnFailure)
        throw new ArgumentNullException(nameof (value));
      result = default (TEnum);
      return false;
    }

    private static bool TryParse<TEnum>(
      ReadOnlySpan<char> value,
      bool ignoreCase,
      bool throwOnFailure,
      out TEnum result)
      where TEnum : struct
    {
      if (!typeof (TEnum).IsEnum)
        throw new ArgumentException(SR.Arg_MustBeEnum, nameof (TEnum));
      value = value.TrimStart();
      if (value.Length == 0)
      {
        if (throwOnFailure)
          throw new ArgumentException(SR.Arg_MustContainEnumInfo, nameof (value));
        result = default (TEnum);
        return false;
      }
      RuntimeType enumType = (RuntimeType) typeof (TEnum);
      switch (Type.GetTypeCode(typeof (TEnum)))
      {
        case TypeCode.SByte:
          int result1;
          bool int32Enum1 = Enum.TryParseInt32Enum(enumType, value, (int) sbyte.MinValue, (int) sbyte.MaxValue, ignoreCase, throwOnFailure, TypeCode.SByte, out result1);
          sbyte source1 = (sbyte) result1;
          result = Unsafe.As<sbyte, TEnum>(ref source1);
          return int32Enum1;
        case TypeCode.Byte:
          uint result2;
          bool uint32Enum1 = Enum.TryParseUInt32Enum(enumType, value, (uint) byte.MaxValue, ignoreCase, throwOnFailure, TypeCode.Byte, out result2);
          byte source2 = (byte) result2;
          result = Unsafe.As<byte, TEnum>(ref source2);
          return uint32Enum1;
        case TypeCode.Int16:
          int result3;
          bool int32Enum2 = Enum.TryParseInt32Enum(enumType, value, (int) short.MinValue, (int) short.MaxValue, ignoreCase, throwOnFailure, TypeCode.Int16, out result3);
          short source3 = (short) result3;
          result = Unsafe.As<short, TEnum>(ref source3);
          return int32Enum2;
        case TypeCode.UInt16:
          uint result4;
          bool uint32Enum2 = Enum.TryParseUInt32Enum(enumType, value, (uint) ushort.MaxValue, ignoreCase, throwOnFailure, TypeCode.UInt16, out result4);
          ushort source4 = (ushort) result4;
          result = Unsafe.As<ushort, TEnum>(ref source4);
          return uint32Enum2;
        case TypeCode.Int32:
          int result5;
          bool int32Enum3 = Enum.TryParseInt32Enum(enumType, value, int.MinValue, int.MaxValue, ignoreCase, throwOnFailure, TypeCode.Int32, out result5);
          result = Unsafe.As<int, TEnum>(ref result5);
          return int32Enum3;
        case TypeCode.UInt32:
          uint result6;
          bool uint32Enum3 = Enum.TryParseUInt32Enum(enumType, value, uint.MaxValue, ignoreCase, throwOnFailure, TypeCode.UInt32, out result6);
          result = Unsafe.As<uint, TEnum>(ref result6);
          return uint32Enum3;
        case TypeCode.Int64:
          long result7;
          bool int64Enum = Enum.TryParseInt64Enum(enumType, value, ignoreCase, throwOnFailure, out result7);
          result = Unsafe.As<long, TEnum>(ref result7);
          return int64Enum;
        case TypeCode.UInt64:
          ulong result8;
          bool uint64Enum = Enum.TryParseUInt64Enum(enumType, value, ignoreCase, throwOnFailure, out result8);
          result = Unsafe.As<ulong, TEnum>(ref result8);
          return uint64Enum;
        default:
          object result9;
          bool rareEnum = Enum.TryParseRareEnum(enumType, value, ignoreCase, throwOnFailure, out result9);
          result = rareEnum ? (TEnum) result9 : default (TEnum);
          return rareEnum;
      }
    }

    private static bool TryParseInt32Enum(
      RuntimeType enumType,
      ReadOnlySpan<char> value,
      int minInclusive,
      int maxInclusive,
      bool ignoreCase,
      bool throwOnFailure,
      TypeCode type,
      out int result)
    {
      Number.ParsingStatus parsingStatus = Number.ParsingStatus.OK;
      if (Enum.StartsNumber(value[0]))
      {
        parsingStatus = Number.TryParseInt32IntegerStyle(value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture.NumberFormat, out result);
        if (parsingStatus == Number.ParsingStatus.OK)
        {
          if ((uint) (result - minInclusive) <= (uint) (maxInclusive - minInclusive))
            return true;
          parsingStatus = Number.ParsingStatus.Overflow;
        }
      }
      if (parsingStatus == Number.ParsingStatus.Overflow)
      {
        if (throwOnFailure)
          Number.ThrowOverflowException(type);
      }
      else
      {
        ulong result1;
        if (Enum.TryParseByName(enumType, value, ignoreCase, throwOnFailure, out result1))
        {
          result = (int) result1;
          return true;
        }
      }
      result = 0;
      return false;
    }

    private static bool TryParseUInt32Enum(
      RuntimeType enumType,
      ReadOnlySpan<char> value,
      uint maxInclusive,
      bool ignoreCase,
      bool throwOnFailure,
      TypeCode type,
      out uint result)
    {
      Number.ParsingStatus parsingStatus = Number.ParsingStatus.OK;
      if (Enum.StartsNumber(value[0]))
      {
        parsingStatus = Number.TryParseUInt32IntegerStyle(value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture.NumberFormat, out result);
        if (parsingStatus == Number.ParsingStatus.OK)
        {
          if (result <= maxInclusive)
            return true;
          parsingStatus = Number.ParsingStatus.Overflow;
        }
      }
      if (parsingStatus == Number.ParsingStatus.Overflow)
      {
        if (throwOnFailure)
          Number.ThrowOverflowException(type);
      }
      else
      {
        ulong result1;
        if (Enum.TryParseByName(enumType, value, ignoreCase, throwOnFailure, out result1))
        {
          result = (uint) result1;
          return true;
        }
      }
      result = 0U;
      return false;
    }

    private static bool TryParseInt64Enum(
      RuntimeType enumType,
      ReadOnlySpan<char> value,
      bool ignoreCase,
      bool throwOnFailure,
      out long result)
    {
      Number.ParsingStatus parsingStatus = Number.ParsingStatus.OK;
      if (Enum.StartsNumber(value[0]))
      {
        parsingStatus = Number.TryParseInt64IntegerStyle(value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture.NumberFormat, out result);
        if (parsingStatus == Number.ParsingStatus.OK)
          return true;
      }
      if (parsingStatus == Number.ParsingStatus.Overflow)
      {
        if (throwOnFailure)
          Number.ThrowOverflowException(TypeCode.Int64);
      }
      else
      {
        ulong result1;
        if (Enum.TryParseByName(enumType, value, ignoreCase, throwOnFailure, out result1))
        {
          result = (long) result1;
          return true;
        }
      }
      result = 0L;
      return false;
    }

    private static bool TryParseUInt64Enum(
      RuntimeType enumType,
      ReadOnlySpan<char> value,
      bool ignoreCase,
      bool throwOnFailure,
      out ulong result)
    {
      Number.ParsingStatus parsingStatus = Number.ParsingStatus.OK;
      if (Enum.StartsNumber(value[0]))
      {
        parsingStatus = Number.TryParseUInt64IntegerStyle(value, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture.NumberFormat, out result);
        if (parsingStatus == Number.ParsingStatus.OK)
          return true;
      }
      if (parsingStatus == Number.ParsingStatus.Overflow)
      {
        if (throwOnFailure)
          Number.ThrowOverflowException(TypeCode.UInt64);
      }
      else if (Enum.TryParseByName(enumType, value, ignoreCase, throwOnFailure, out result))
        return true;
      result = 0UL;
      return false;
    }

    private static bool TryParseRareEnum(
      RuntimeType enumType,
      ReadOnlySpan<char> value,
      bool ignoreCase,
      bool throwOnFailure,
      [NotNullWhen(true)] out object result)
    {
      if (Enum.StartsNumber(value[0]))
      {
        Type underlyingType = Enum.GetUnderlyingType((Type) enumType);
        try
        {
          result = Enum.ToObject((Type) enumType, Convert.ChangeType((object) value.ToString(), underlyingType, (IFormatProvider) CultureInfo.InvariantCulture));
          return true;
        }
        catch (FormatException ex)
        {
        }
        catch (Exception ex) when (!throwOnFailure)
        {
          result = (object) null;
          return false;
        }
      }
      ulong result1;
      if (Enum.TryParseByName(enumType, value, ignoreCase, throwOnFailure, out result1))
      {
        try
        {
          result = Enum.ToObject((Type) enumType, result1);
          return true;
        }
        catch (Exception ex) when (!throwOnFailure)
        {
        }
      }
      result = (object) null;
      return false;
    }

    private static bool TryParseByName(
      RuntimeType enumType,
      ReadOnlySpan<char> value,
      bool ignoreCase,
      bool throwOnFailure,
      out ulong result)
    {
      ReadOnlySpan<char> readOnlySpan = value;
      Enum.EnumInfo enumInfo = Enum.GetEnumInfo(enumType);
      string[] names = enumInfo.Names;
      ulong[] values = enumInfo.Values;
      bool flag1 = true;
      ulong num = 0;
      while (value.Length > 0)
      {
        int length = value.IndexOf<char>(',');
        ReadOnlySpan<char> span;
        if (length == -1)
        {
          span = value.Trim();
          value = new ReadOnlySpan<char>();
        }
        else if (length != value.Length - 1)
        {
          span = value.Slice(0, length).Trim();
          value = value.Slice(length + 1);
        }
        else
        {
          flag1 = false;
          break;
        }
        bool flag2 = false;
        if (ignoreCase)
        {
          for (int index = 0; index < names.Length; ++index)
          {
            if (span.EqualsOrdinalIgnoreCase((ReadOnlySpan<char>) names[index]))
            {
              num |= values[index];
              flag2 = true;
              break;
            }
          }
        }
        else
        {
          for (int index = 0; index < names.Length; ++index)
          {
            if (span.EqualsOrdinal((ReadOnlySpan<char>) names[index]))
            {
              num |= values[index];
              flag2 = true;
              break;
            }
          }
        }
        if (!flag2)
        {
          flag1 = false;
          break;
        }
      }
      if (flag1)
      {
        result = num;
        return true;
      }
      if (throwOnFailure)
        throw new ArgumentException(SR.Format(SR.Arg_EnumValueNotFound, (object) readOnlySpan.ToString()));
      result = 0UL;
      return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool StartsNumber(char c) => char.IsInRange(c, '0', '9') || c == '-' || c == '+';


    #nullable enable
    /// <summary>Converts the specified object with an integer value to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> or <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> is not type <see cref="T:System.SByte" />, <see cref="T:System.Int16" />, <see cref="T:System.Int32" />, <see cref="T:System.Int64" />, <see cref="T:System.Byte" />, <see cref="T:System.UInt16" />, <see cref="T:System.UInt32" />, or <see cref="T:System.UInt64" />.</exception>
    /// <returns>An enumeration object whose value is <paramref name="value" />.</returns>
    public static object ToObject(Type enumType, object value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      switch (Convert.GetTypeCode(value))
      {
        case TypeCode.Boolean:
          return Enum.ToObject(enumType, (bool) value);
        case TypeCode.Char:
          return Enum.ToObject(enumType, (char) value);
        case TypeCode.SByte:
          return Enum.ToObject(enumType, (sbyte) value);
        case TypeCode.Byte:
          return Enum.ToObject(enumType, (byte) value);
        case TypeCode.Int16:
          return Enum.ToObject(enumType, (short) value);
        case TypeCode.UInt16:
          return Enum.ToObject(enumType, (ushort) value);
        case TypeCode.Int32:
          return Enum.ToObject(enumType, (int) value);
        case TypeCode.UInt32:
          return Enum.ToObject(enumType, (uint) value);
        case TypeCode.Int64:
          return Enum.ToObject(enumType, (long) value);
        case TypeCode.UInt64:
          return Enum.ToObject(enumType, (ulong) value);
        default:
          throw new ArgumentException(SR.Arg_MustBeEnumBaseTypeOrEnum, nameof (value));
      }
    }

    /// <summary>Converts the specified value of a specified enumerated type to its equivalent string representation according to the specified format.</summary>
    /// <param name="enumType">The enumeration type of the value to convert.</param>
    /// <param name="value">The value to convert.</param>
    /// <param name="format">The output format to use.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="enumType" />, <paramref name="value" />, or <paramref name="format" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="enumType" /> parameter is not an <see cref="T:System.Enum" /> type.
    /// 
    /// -or-
    /// 
    /// The <paramref name="value" /> is from an enumeration that differs in type from <paramref name="enumType" />.
    /// 
    /// -or-
    /// 
    /// The type of <paramref name="value" /> is not an underlying type of <paramref name="enumType" />.</exception>
    /// <exception cref="T:System.FormatException">The <paramref name="format" /> parameter contains an invalid value.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="format" /> equals "X", but the enumeration type is unknown.</exception>
    /// <returns>A string representation of <paramref name="value" />.</returns>
    public static string Format(Type enumType, object value, string format)
    {
      RuntimeType enumType1 = Enum.ValidateRuntimeType(enumType);
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (format == null)
        throw new ArgumentNullException(nameof (format));
      Type type = value.GetType();
      if (type.IsEnum)
      {
        if (!type.IsEquivalentTo(enumType))
          throw new ArgumentException(SR.Format(SR.Arg_EnumAndObjectMustBeSameType, (object) type, (object) enumType));
        return format.Length == 1 ? ((Enum) value).ToString(format) : throw new FormatException(SR.Format_InvalidEnumFormatSpecification);
      }
      Type underlyingType = Enum.GetUnderlyingType(enumType);
      if (type != underlyingType)
        throw new ArgumentException(SR.Format(SR.Arg_EnumFormatUnderlyingTypeAndObjectMustBeSameType, (object) type, (object) underlyingType));
      if (format.Length == 1)
      {
        switch (format[0])
        {
          case 'D':
          case 'd':
            return value.ToString();
          case 'F':
          case 'f':
            return Enum.InternalFlagsFormat(enumType1, Enum.ToUInt64(value)) ?? value.ToString();
          case 'G':
          case 'g':
            return Enum.InternalFormat(enumType1, Enum.ToUInt64(value)) ?? value.ToString();
          case 'X':
          case 'x':
            return Enum.ValueToHexString(value);
        }
      }
      throw new FormatException(SR.Format_InvalidEnumFormatSpecification);
    }


    #nullable disable
    internal object GetValue()
    {
      ref byte local = ref this.GetRawData();
      switch (this.InternalGetCorElementType())
      {
        case CorElementType.ELEMENT_TYPE_BOOLEAN:
          return (object) Unsafe.As<byte, bool>(ref local);
        case CorElementType.ELEMENT_TYPE_CHAR:
          return (object) Unsafe.As<byte, char>(ref local);
        case CorElementType.ELEMENT_TYPE_I1:
          return (object) Unsafe.As<byte, sbyte>(ref local);
        case CorElementType.ELEMENT_TYPE_U1:
          return (object) local;
        case CorElementType.ELEMENT_TYPE_I2:
          return (object) Unsafe.As<byte, short>(ref local);
        case CorElementType.ELEMENT_TYPE_U2:
          return (object) Unsafe.As<byte, ushort>(ref local);
        case CorElementType.ELEMENT_TYPE_I4:
          return (object) Unsafe.As<byte, int>(ref local);
        case CorElementType.ELEMENT_TYPE_U4:
          return (object) Unsafe.As<byte, uint>(ref local);
        case CorElementType.ELEMENT_TYPE_I8:
          return (object) Unsafe.As<byte, long>(ref local);
        case CorElementType.ELEMENT_TYPE_U8:
          return (object) Unsafe.As<byte, ulong>(ref local);
        case CorElementType.ELEMENT_TYPE_R4:
          return (object) Unsafe.As<byte, float>(ref local);
        case CorElementType.ELEMENT_TYPE_R8:
          return (object) Unsafe.As<byte, double>(ref local);
        case CorElementType.ELEMENT_TYPE_I:
          return (object) Unsafe.As<byte, IntPtr>(ref local);
        case CorElementType.ELEMENT_TYPE_U:
          return (object) Unsafe.As<byte, UIntPtr>(ref local);
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
    }

    private ulong ToUInt64()
    {
      ref byte local = ref this.GetRawData();
      switch (this.InternalGetCorElementType())
      {
        case CorElementType.ELEMENT_TYPE_BOOLEAN:
          return local == (byte) 0 ? 0UL : 1UL;
        case CorElementType.ELEMENT_TYPE_CHAR:
        case CorElementType.ELEMENT_TYPE_U2:
          return (ulong) Unsafe.As<byte, ushort>(ref local);
        case CorElementType.ELEMENT_TYPE_I1:
          return (ulong) Unsafe.As<byte, sbyte>(ref local);
        case CorElementType.ELEMENT_TYPE_U1:
          return (ulong) local;
        case CorElementType.ELEMENT_TYPE_I2:
          return (ulong) Unsafe.As<byte, short>(ref local);
        case CorElementType.ELEMENT_TYPE_I4:
          return (ulong) Unsafe.As<byte, int>(ref local);
        case CorElementType.ELEMENT_TYPE_U4:
        case CorElementType.ELEMENT_TYPE_R4:
          return (ulong) Unsafe.As<byte, uint>(ref local);
        case CorElementType.ELEMENT_TYPE_I8:
          return (ulong) Unsafe.As<byte, long>(ref local);
        case CorElementType.ELEMENT_TYPE_U8:
        case CorElementType.ELEMENT_TYPE_R8:
          return Unsafe.As<byte, ulong>(ref local);
        case CorElementType.ELEMENT_TYPE_I:
          return (ulong) (long) Unsafe.As<byte, IntPtr>(ref local);
        case CorElementType.ELEMENT_TYPE_U:
          return (ulong) Unsafe.As<byte, UIntPtr>(ref local);
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
    }

    /// <summary>Returns the hash code for the value of this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
      ref byte local = ref this.GetRawData();
      switch (this.InternalGetCorElementType())
      {
        case CorElementType.ELEMENT_TYPE_BOOLEAN:
          return Unsafe.As<byte, bool>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_CHAR:
          return Unsafe.As<byte, char>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_I1:
          return Unsafe.As<byte, sbyte>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_U1:
          return local.GetHashCode();
        case CorElementType.ELEMENT_TYPE_I2:
          return Unsafe.As<byte, short>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_U2:
          return Unsafe.As<byte, ushort>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_I4:
          return Unsafe.As<byte, int>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_U4:
          return Unsafe.As<byte, uint>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_I8:
          return Unsafe.As<byte, long>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_U8:
          return Unsafe.As<byte, ulong>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_R4:
          return Unsafe.As<byte, float>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_R8:
          return Unsafe.As<byte, double>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_I:
          return Unsafe.As<byte, IntPtr>(ref local).GetHashCode();
        case CorElementType.ELEMENT_TYPE_U:
          return Unsafe.As<byte, UIntPtr>(ref local).GetHashCode();
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
    }


    #nullable enable
    /// <summary>Converts the value of this instance to its equivalent string representation.</summary>
    /// <returns>The string representation of the value of this instance.</returns>
    public override string ToString() => Enum.InternalFormat((RuntimeType) this.GetType(), this.ToUInt64()) ?? this.ValueToString();

    /// <summary>Compares this instance to a specified object and returns an indication of their relative values.</summary>
    /// <param name="target">An object to compare, or <see langword="null" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="target" /> and this instance are not the same type.</exception>
    /// <exception cref="T:System.InvalidOperationException">This instance is not type <see cref="T:System.SByte" />, <see cref="T:System.Int16" />, <see cref="T:System.Int32" />, <see cref="T:System.Int64" />, <see cref="T:System.Byte" />, <see cref="T:System.UInt16" />, <see cref="T:System.UInt32" />, or <see cref="T:System.UInt64" />.</exception>
    /// <exception cref="T:System.NullReferenceException">This instance is null.</exception>
    /// <returns>A signed number that indicates the relative values of this instance and <paramref name="target" />.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> The value of this instance is less than the value of <paramref name="target" />.</description></item><item><term> Zero</term><description> The value of this instance is equal to the value of <paramref name="target" />.</description></item><item><term> Greater than zero</term><description> The value of this instance is greater than the value of <paramref name="target" />, or <paramref name="target" /> is <see langword="null" />.</description></item></list></returns>
    public int CompareTo(object? target)
    {
      if (target == this)
        return 0;
      if (target == null)
        return 1;
      if (this.GetType() != target.GetType())
        throw new ArgumentException(SR.Format(SR.Arg_EnumAndObjectMustBeSameType, (object) target.GetType(), (object) this.GetType()));
      ref byte local1 = ref this.GetRawData();
      ref byte local2 = ref target.GetRawData();
      switch (this.InternalGetCorElementType())
      {
        case CorElementType.ELEMENT_TYPE_BOOLEAN:
        case CorElementType.ELEMENT_TYPE_U1:
          return local1.CompareTo(local2);
        case CorElementType.ELEMENT_TYPE_CHAR:
        case CorElementType.ELEMENT_TYPE_U2:
          return Unsafe.As<byte, ushort>(ref local1).CompareTo(Unsafe.As<byte, ushort>(ref local2));
        case CorElementType.ELEMENT_TYPE_I1:
          return Unsafe.As<byte, sbyte>(ref local1).CompareTo(Unsafe.As<byte, sbyte>(ref local2));
        case CorElementType.ELEMENT_TYPE_I2:
          return Unsafe.As<byte, short>(ref local1).CompareTo(Unsafe.As<byte, short>(ref local2));
        case CorElementType.ELEMENT_TYPE_I4:
          return Unsafe.As<byte, int>(ref local1).CompareTo(Unsafe.As<byte, int>(ref local2));
        case CorElementType.ELEMENT_TYPE_U4:
          return Unsafe.As<byte, uint>(ref local1).CompareTo(Unsafe.As<byte, uint>(ref local2));
        case CorElementType.ELEMENT_TYPE_I8:
        case CorElementType.ELEMENT_TYPE_I:
          return Unsafe.As<byte, long>(ref local1).CompareTo(Unsafe.As<byte, long>(ref local2));
        case CorElementType.ELEMENT_TYPE_U8:
        case CorElementType.ELEMENT_TYPE_U:
          return Unsafe.As<byte, ulong>(ref local1).CompareTo(Unsafe.As<byte, ulong>(ref local2));
        case CorElementType.ELEMENT_TYPE_R4:
          return Unsafe.As<byte, float>(ref local1).CompareTo(Unsafe.As<byte, float>(ref local2));
        case CorElementType.ELEMENT_TYPE_R8:
          return Unsafe.As<byte, double>(ref local1).CompareTo(Unsafe.As<byte, double>(ref local2));
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
    }

    /// <summary>This method overload is obsolete; use <see cref="M:System.Enum.ToString(System.String)" />.</summary>
    /// <param name="format">A format specification.</param>
    /// <param name="provider">(Obsolete.)</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> does not contain a valid format specification.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="format" /> equals "X", but the enumeration type is unknown.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    [Obsolete("The provider argument is not used. Use ToString(String) instead.")]
    public string ToString(string? format, IFormatProvider? provider) => this.ToString(format);

    /// <summary>Converts the value of this instance to its equivalent string representation using the specified format.</summary>
    /// <param name="format">A format string.</param>
    /// <exception cref="T:System.FormatException">
    /// <paramref name="format" /> contains an invalid specification.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="format" /> equals "X", but the enumeration type is unknown.</exception>
    /// <returns>The string representation of the value of this instance as specified by <paramref name="format" />.</returns>
    public string ToString(string? format)
    {
      if (string.IsNullOrEmpty(format))
        return this.ToString();
      if (format.Length == 1)
      {
        switch (format[0])
        {
          case 'D':
          case 'd':
            return this.ValueToString();
          case 'F':
          case 'f':
            return Enum.InternalFlagsFormat((RuntimeType) this.GetType(), this.ToUInt64()) ?? this.ValueToString();
          case 'G':
          case 'g':
            return this.ToString();
          case 'X':
          case 'x':
            return this.ValueToHexString();
        }
      }
      throw new FormatException(SR.Format_InvalidEnumFormatSpecification);
    }

    /// <summary>This method overload is obsolete; use <see cref="M:System.Enum.ToString" />.</summary>
    /// <param name="provider">(obsolete)</param>
    /// <returns>The string representation of the value of this instance.</returns>
    [Obsolete("The provider argument is not used. Use ToString() instead.")]
    public string ToString(IFormatProvider? provider) => this.ToString();

    /// <summary>Returns the type code of the underlying type of this enumeration member.</summary>
    /// <exception cref="T:System.InvalidOperationException">The enumeration type is unknown.</exception>
    /// <returns>The type code of the underlying type of this instance.</returns>
    public TypeCode GetTypeCode()
    {
      switch (this.InternalGetCorElementType())
      {
        case CorElementType.ELEMENT_TYPE_BOOLEAN:
          return TypeCode.Boolean;
        case CorElementType.ELEMENT_TYPE_CHAR:
          return TypeCode.Char;
        case CorElementType.ELEMENT_TYPE_I1:
          return TypeCode.SByte;
        case CorElementType.ELEMENT_TYPE_U1:
          return TypeCode.Byte;
        case CorElementType.ELEMENT_TYPE_I2:
          return TypeCode.Int16;
        case CorElementType.ELEMENT_TYPE_U2:
          return TypeCode.UInt16;
        case CorElementType.ELEMENT_TYPE_I4:
          return TypeCode.Int32;
        case CorElementType.ELEMENT_TYPE_U4:
          return TypeCode.UInt32;
        case CorElementType.ELEMENT_TYPE_I8:
          return TypeCode.Int64;
        case CorElementType.ELEMENT_TYPE_U8:
          return TypeCode.UInt64;
        default:
          throw new InvalidOperationException(SR.InvalidOperation_UnknownEnumType);
      }
    }


    #nullable disable
    /// <summary>Converts the current value to a Boolean value based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This member always throws an exception.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(this.GetValue());

    /// <summary>Converts the current value to a Unicode character based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This member always throws an exception.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(this.GetValue());

    /// <summary>Converts the current value to an 8-bit signed integer based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => Convert.ToSByte(this.GetValue());

    /// <summary>Converts the current value to an 8-bit unsigned integer based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => Convert.ToByte(this.GetValue());

    /// <summary>Converts the current value to a 16-bit signed integer based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => Convert.ToInt16(this.GetValue());

    /// <summary>Converts the current value to a 16-bit unsigned integer based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    ushort IConvertible.ToUInt16(IFormatProvider provider) => Convert.ToUInt16(this.GetValue());

    /// <summary>Converts the current value to a 32-bit signed integer based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => Convert.ToInt32(this.GetValue());

    /// <summary>Converts the current value to a 32-bit unsigned integer based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => Convert.ToUInt32(this.GetValue());

    /// <summary>Converts the current value to a 64-bit signed integer based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => Convert.ToInt64(this.GetValue());

    /// <summary>Converts the current value to a 64-bit unsigned integer based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => Convert.ToUInt64(this.GetValue());

    /// <summary>Converts the current value to a single-precision floating-point number based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This member always throws an exception.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(this.GetValue());

    /// <summary>Converts the current value to a double-precision floating point number based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This member always throws an exception.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(this.GetValue());

    /// <summary>Converts the current value to a <see cref="T:System.Decimal" /> based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This member always throws an exception.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(this.GetValue());

    /// <summary>Converts the current value to a <see cref="T:System.DateTime" /> based on the underlying type.</summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <exception cref="T:System.InvalidCastException">In all cases.</exception>
    /// <returns>This member always throws an exception.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, (object) nameof (Enum), (object) "DateTime"));

    /// <summary>Converts the current value to a specified type based on the underlying type.</summary>
    /// <param name="type">The type to convert to.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>The converted value.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);


    #nullable enable
    /// <summary>Converts the specified 8-bit signed integer value to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value to convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static object ToObject(Type enumType, sbyte value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), (long) value);

    /// <summary>Converts the specified 16-bit signed integer to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value to convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
    public static object ToObject(Type enumType, short value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), (long) value);

    /// <summary>Converts the specified 32-bit signed integer to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value to convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
    public static object ToObject(Type enumType, int value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), (long) value);

    /// <summary>Converts the specified 8-bit unsigned integer to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value to convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
    public static object ToObject(Type enumType, byte value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), (long) value);

    /// <summary>Converts the specified 16-bit unsigned integer value to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value to convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static object ToObject(Type enumType, ushort value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), (long) value);

    /// <summary>Converts the specified 32-bit unsigned integer value to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value to convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static object ToObject(Type enumType, uint value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), (long) value);

    /// <summary>Converts the specified 64-bit signed integer to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value to convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
    public static object ToObject(Type enumType, long value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), value);

    /// <summary>Converts the specified 64-bit unsigned integer value to an enumeration member.</summary>
    /// <param name="enumType">The enumeration type to return.</param>
    /// <param name="value">The value to convert to an enumeration member.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="enumType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="enumType" /> is not an <see cref="T:System.Enum" />.</exception>
    /// <returns>An instance of the enumeration set to <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    public static object ToObject(Type enumType, ulong value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), (long) value);


    #nullable disable
    private static object ToObject(Type enumType, char value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), (long) value);

    private static object ToObject(Type enumType, bool value) => Enum.InternalBoxEnum(Enum.ValidateRuntimeType(enumType), value ? 1L : 0L);

    private static RuntimeType ValidateRuntimeType(Type enumType)
    {
      if (enumType == (Type) null)
        throw new ArgumentNullException(nameof (enumType));
      if (!enumType.IsEnum)
        throw new ArgumentException(SR.Arg_MustBeEnum, nameof (enumType));
      return enumType is RuntimeType runtimeType ? runtimeType : throw new ArgumentException(SR.Arg_MustBeType, nameof (enumType));
    }

    internal sealed class EnumInfo
    {
      public readonly bool HasFlagsAttribute;
      public readonly ulong[] Values;
      public readonly string[] Names;

      public EnumInfo(bool hasFlagsAttribute, ulong[] values, string[] names)
      {
        this.HasFlagsAttribute = hasFlagsAttribute;
        this.Values = values;
        this.Names = names;
      }
    }
  }
}
