// Decompiled with JetBrains decompiler
// Type: System.DBNull
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.Serialization;


#nullable enable
namespace System
{
  /// <summary>Represents a nonexistent value. This class cannot be inherited.</summary>
  [Serializable]
  public sealed class DBNull : ISerializable, IConvertible
  {
    /// <summary>Represents the sole instance of the <see cref="T:System.DBNull" /> class.</summary>
    public static readonly DBNull Value = new DBNull();

    private DBNull()
    {
    }


    #nullable disable
    private DBNull(SerializationInfo info, StreamingContext context) => throw new NotSupportedException(SR.NotSupported_DBNullSerial);


    #nullable enable
    /// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and returns the data needed to serialize the <see cref="T:System.DBNull" /> object.</summary>
    /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing information required to serialize the <see cref="T:System.DBNull" /> object.</param>
    /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object containing the source and destination of the serialized stream associated with the <see cref="T:System.DBNull" /> object.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    public void GetObjectData(SerializationInfo info, StreamingContext context) => UnitySerializationHolder.GetUnitySerializationInfo(info, 2);

    /// <summary>Returns an empty string (<see cref="F:System.String.Empty" />).</summary>
    /// <returns>An empty string (<see cref="F:System.String.Empty" />).</returns>
    public override string ToString() => string.Empty;

    /// <summary>Returns an empty string using the specified <see cref="T:System.IFormatProvider" />.</summary>
    /// <param name="provider">The <see cref="T:System.IFormatProvider" /> to be used to format the return value.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to obtain the format information from the current locale setting of the operating system.</param>
    /// <returns>An empty string (<see cref="F:System.String.Empty" />).</returns>
    public string ToString(IFormatProvider? provider) => string.Empty;

    /// <summary>Gets the <see cref="T:System.TypeCode" /> value for <see cref="T:System.DBNull" />.</summary>
    /// <returns>The <see cref="T:System.TypeCode" /> value for <see cref="T:System.DBNull" />, which is <see cref="F:System.TypeCode.DBNull" />.</returns>
    public TypeCode GetTypeCode() => TypeCode.DBNull;


    #nullable disable
    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    bool IConvertible.ToBoolean(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    char IConvertible.ToChar(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    sbyte IConvertible.ToSByte(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    byte IConvertible.ToByte(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    short IConvertible.ToInt16(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    ushort IConvertible.ToUInt16(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    int IConvertible.ToInt32(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    uint IConvertible.ToUInt32(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    long IConvertible.ToInt64(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    ulong IConvertible.ToUInt64(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    float IConvertible.ToSingle(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    double IConvertible.ToDouble(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    Decimal IConvertible.ToDecimal(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>This conversion is not supported. Attempting to make this conversion throws an <see cref="T:System.InvalidCastException" />.</summary>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify <see langword="null" />.)</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <returns>None. The return value for this member is not used.</returns>
    DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new InvalidCastException(SR.InvalidCast_FromDBNull);

    /// <summary>Converts the current <see cref="T:System.DBNull" /> object to the specified type.</summary>
    /// <param name="type">The type to convert the current <see cref="T:System.DBNull" /> object to.</param>
    /// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface and is used to augment the conversion. If <see langword="null" /> is specified, format information is obtained from the current culture.</param>
    /// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DBNull" /> type.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <returns>The boxed equivalent of the current <see cref="T:System.DBNull" /> object, if that conversion is supported; otherwise, an exception is thrown and no value is returned.</returns>
    object IConvertible.ToType(Type type, IFormatProvider provider) => Convert.DefaultToType((IConvertible) this, type, provider);
  }
}
