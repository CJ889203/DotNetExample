// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.DBConvert
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

namespace System.Data.Linq
{
  public static class DBConvert
  {
    private static Type[] StringArg = new Type[1]
    {
      typeof (string)
    };

    public static T ChangeType<T>(object value) => (T) DBConvert.ChangeType(value, typeof (T));

    public static object ChangeType(object value, Type type)
    {
      if (value == null)
        return (object) null;
      Type nonNullableType = TypeSystem.GetNonNullableType(type);
      Type type1 = value.GetType();
      if (nonNullableType.IsAssignableFrom(type1))
        return value;
      if (nonNullableType == typeof (System.Data.Linq.Binary))
      {
        if (type1 == typeof (byte[]))
          return (object) new System.Data.Linq.Binary((byte[]) value);
        if (type1 == typeof (Guid))
          return (object) new System.Data.Linq.Binary(((Guid) value).ToByteArray());
        byte[] array;
        using (MemoryStream serializationStream = new MemoryStream())
        {
          new BinaryFormatter().Serialize((Stream) serializationStream, value);
          array = serializationStream.ToArray();
        }
        return (object) new System.Data.Linq.Binary(array);
      }
      if (nonNullableType == typeof (byte[]))
      {
        if (type1 == typeof (System.Data.Linq.Binary))
          return (object) ((System.Data.Linq.Binary) value).ToArray();
        if (type1 == typeof (Guid))
          return (object) ((Guid) value).ToByteArray();
        using (MemoryStream serializationStream = new MemoryStream())
        {
          new BinaryFormatter().Serialize((Stream) serializationStream, value);
          return (object) serializationStream.ToArray();
        }
      }
      else if (type1 == typeof (byte[]))
      {
        if (nonNullableType == typeof (Guid))
          return (object) new Guid((byte[]) value);
        using (MemoryStream serializationStream = new MemoryStream((byte[]) value))
          return DBConvert.ChangeType(new BinaryFormatter().Deserialize((Stream) serializationStream), nonNullableType);
      }
      else if (type1 == typeof (System.Data.Linq.Binary))
      {
        if (nonNullableType == typeof (Guid))
          return (object) new Guid(((System.Data.Linq.Binary) value).ToArray());
        using (MemoryStream serializationStream = new MemoryStream(((System.Data.Linq.Binary) value).ToArray(), false))
          return DBConvert.ChangeType(new BinaryFormatter().Deserialize((Stream) serializationStream), nonNullableType);
      }
      else
      {
        if (nonNullableType.IsEnum)
        {
          if (!(type1 == typeof (string)))
            return Enum.ToObject(nonNullableType, Convert.ChangeType(value, Enum.GetUnderlyingType(nonNullableType), (IFormatProvider) CultureInfo.InvariantCulture));
          string str = ((string) value).Trim();
          return Enum.Parse(nonNullableType, str);
        }
        if (type1.IsEnum)
          return nonNullableType == typeof (string) ? (object) Enum.GetName(type1, value) : Convert.ChangeType(Convert.ChangeType(value, Enum.GetUnderlyingType(type1), (IFormatProvider) CultureInfo.InvariantCulture), nonNullableType, (IFormatProvider) CultureInfo.InvariantCulture);
        if (nonNullableType == typeof (TimeSpan))
        {
          if (type1 == typeof (string))
            return (object) TimeSpan.Parse(value.ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
          if (type1 == typeof (DateTime))
            return (object) DateTime.Parse(value.ToString(), (IFormatProvider) CultureInfo.InvariantCulture).TimeOfDay;
          return type1 == typeof (DateTimeOffset) ? (object) DateTimeOffset.Parse(value.ToString(), (IFormatProvider) CultureInfo.InvariantCulture).TimeOfDay : (object) new TimeSpan((long) Convert.ChangeType(value, typeof (long), (IFormatProvider) CultureInfo.InvariantCulture));
        }
        if (type1 == typeof (TimeSpan))
        {
          if (nonNullableType == typeof (string))
            return (object) ((TimeSpan) value).ToString("", (IFormatProvider) CultureInfo.InvariantCulture);
          if (nonNullableType == typeof (DateTime))
            return (object) new DateTime().Add((TimeSpan) value);
          return nonNullableType == typeof (DateTimeOffset) ? (object) new DateTimeOffset().Add((TimeSpan) value) : Convert.ChangeType((object) ((TimeSpan) value).Ticks, nonNullableType, (IFormatProvider) CultureInfo.InvariantCulture);
        }
        if (nonNullableType == typeof (DateTime) && type1 == typeof (DateTimeOffset))
          return (object) ((DateTimeOffset) value).DateTime;
        if (nonNullableType == typeof (DateTimeOffset) && type1 == typeof (DateTime))
          return (object) new DateTimeOffset((DateTime) value);
        if (nonNullableType == typeof (string) && !typeof (IConvertible).IsAssignableFrom(type1))
          return type1 == typeof (char[]) ? (object) new string((char[]) value) : (object) value.ToString();
        if (type1 == typeof (string))
        {
          if (nonNullableType == typeof (Guid))
            return (object) new Guid((string) value);
          if (nonNullableType == typeof (char[]))
            return (object) ((string) value).ToCharArray();
          if (nonNullableType == typeof (XDocument) && (string) value == string.Empty)
            return (object) new XDocument();
          if (!typeof (IConvertible).IsAssignableFrom(nonNullableType))
          {
            MethodInfo method;
            if ((method = nonNullableType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, (Binder) null, DBConvert.StringArg, (ParameterModifier[]) null)) != (MethodInfo) null)
            {
              try
              {
                return SecurityUtils.MethodInfoInvoke(method, (object) null, new object[1]
                {
                  value
                });
              }
              catch (TargetInvocationException ex)
              {
                throw ex.GetBaseException();
              }
            }
          }
          return Convert.ChangeType(value, nonNullableType, (IFormatProvider) CultureInfo.InvariantCulture);
        }
        if (nonNullableType.IsGenericType && nonNullableType.GetGenericTypeDefinition() == typeof (IQueryable<>))
        {
          if (typeof (IEnumerable<>).MakeGenericType(nonNullableType.GetGenericArguments()[0]).IsAssignableFrom(type1))
            return (object) ((IEnumerable) value).AsQueryable();
        }
        try
        {
          return Convert.ChangeType(value, nonNullableType, (IFormatProvider) CultureInfo.InvariantCulture);
        }
        catch (InvalidCastException ex)
        {
          throw Error.CouldNotConvert((object) type1, (object) nonNullableType);
        }
      }
    }
  }
}
