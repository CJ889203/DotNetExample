// Decompiled with JetBrains decompiler
// Type: System.Reflection.CustomAttributeTypedArgument
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents an argument of a custom attribute in the reflection-only context, or an element of an array argument.</summary>
  public readonly struct CustomAttributeTypedArgument
  {

    #nullable disable
    private readonly object _value;
    private readonly Type _argumentType;

    private static Type CustomAttributeEncodingToType(CustomAttributeEncoding encodedType)
    {
      switch (encodedType)
      {
        case CustomAttributeEncoding.Boolean:
          return typeof (bool);
        case CustomAttributeEncoding.Char:
          return typeof (char);
        case CustomAttributeEncoding.SByte:
          return typeof (sbyte);
        case CustomAttributeEncoding.Byte:
          return typeof (byte);
        case CustomAttributeEncoding.Int16:
          return typeof (short);
        case CustomAttributeEncoding.UInt16:
          return typeof (ushort);
        case CustomAttributeEncoding.Int32:
          return typeof (int);
        case CustomAttributeEncoding.UInt32:
          return typeof (uint);
        case CustomAttributeEncoding.Int64:
          return typeof (long);
        case CustomAttributeEncoding.UInt64:
          return typeof (ulong);
        case CustomAttributeEncoding.Float:
          return typeof (float);
        case CustomAttributeEncoding.Double:
          return typeof (double);
        case CustomAttributeEncoding.String:
          return typeof (string);
        case CustomAttributeEncoding.Array:
          return typeof (Array);
        case CustomAttributeEncoding.Type:
          return typeof (Type);
        case CustomAttributeEncoding.Object:
          return typeof (object);
        case CustomAttributeEncoding.Enum:
          return typeof (Enum);
        default:
          throw new ArgumentException(SR.Format(SR.Arg_EnumIllegalVal, (object) (int) encodedType), nameof (encodedType));
      }
    }

    private static unsafe object EncodedValueToRawValue(
      long val,
      CustomAttributeEncoding encodedType)
    {
      switch (encodedType)
      {
        case CustomAttributeEncoding.Boolean:
          return (object) ((byte) val > (byte) 0);
        case CustomAttributeEncoding.Char:
          return (object) (char) val;
        case CustomAttributeEncoding.SByte:
          return (object) (sbyte) val;
        case CustomAttributeEncoding.Byte:
          return (object) (byte) val;
        case CustomAttributeEncoding.Int16:
          return (object) (short) val;
        case CustomAttributeEncoding.UInt16:
          return (object) (ushort) val;
        case CustomAttributeEncoding.Int32:
          return (object) (int) val;
        case CustomAttributeEncoding.UInt32:
          return (object) (uint) val;
        case CustomAttributeEncoding.Int64:
          return (object) val;
        case CustomAttributeEncoding.UInt64:
          return (object) (ulong) val;
        case CustomAttributeEncoding.Float:
          return (object) *(float*) &val;
        case CustomAttributeEncoding.Double:
          return (object) *(double*) &val;
        default:
          throw new ArgumentException(SR.Format(SR.Arg_EnumIllegalVal, (object) (int) val), nameof (val));
      }
    }

    private static RuntimeType ResolveType(RuntimeModule scope, string typeName) => RuntimeTypeHandle.GetTypeByNameUsingCARules(typeName, scope) ?? throw new InvalidOperationException(SR.Format(SR.Arg_CATypeResolutionFailed, (object) typeName));

    internal CustomAttributeTypedArgument(
      RuntimeModule scope,
      CustomAttributeEncodedArgument encodedArg)
    {
      CustomAttributeEncoding encodedType = encodedArg.CustomAttributeType.EncodedType;
      switch (encodedType)
      {
        case CustomAttributeEncoding.Undefined:
          throw new ArgumentException((string) null, nameof (encodedArg));
        case CustomAttributeEncoding.String:
          this._argumentType = typeof (string);
          this._value = (object) encodedArg.StringValue;
          break;
        case CustomAttributeEncoding.Array:
          CustomAttributeEncoding encodedArrayType = encodedArg.CustomAttributeType.EncodedArrayType;
          this._argumentType = (encodedArrayType != CustomAttributeEncoding.Enum ? CustomAttributeTypedArgument.CustomAttributeEncodingToType(encodedArrayType) : (Type) CustomAttributeTypedArgument.ResolveType(scope, encodedArg.CustomAttributeType.EnumName)).MakeArrayType();
          if (encodedArg.ArrayValue == null)
          {
            this._value = (object) null;
            break;
          }
          CustomAttributeTypedArgument[] array = new CustomAttributeTypedArgument[encodedArg.ArrayValue.Length];
          for (int index = 0; index < array.Length; ++index)
            array[index] = new CustomAttributeTypedArgument(scope, encodedArg.ArrayValue[index]);
          this._value = (object) Array.AsReadOnly<CustomAttributeTypedArgument>(array);
          break;
        case CustomAttributeEncoding.Type:
          this._argumentType = typeof (Type);
          this._value = (object) null;
          if (encodedArg.StringValue == null)
            break;
          this._value = (object) CustomAttributeTypedArgument.ResolveType(scope, encodedArg.StringValue);
          break;
        case CustomAttributeEncoding.Enum:
          RuntimeModule scope1 = scope;
          CustomAttributeType customAttributeType = encodedArg.CustomAttributeType;
          string enumName = customAttributeType.EnumName;
          this._argumentType = (Type) CustomAttributeTypedArgument.ResolveType(scope1, enumName);
          long primitiveValue = encodedArg.PrimitiveValue;
          customAttributeType = encodedArg.CustomAttributeType;
          int encodedEnumType = (int) customAttributeType.EncodedEnumType;
          this._value = CustomAttributeTypedArgument.EncodedValueToRawValue(primitiveValue, (CustomAttributeEncoding) encodedEnumType);
          break;
        default:
          this._argumentType = CustomAttributeTypedArgument.CustomAttributeEncodingToType(encodedType);
          this._value = CustomAttributeTypedArgument.EncodedValueToRawValue(encodedArg.PrimitiveValue, encodedType);
          break;
      }
    }

    /// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures are equivalent.</summary>
    /// <param name="left">The <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure to the left of the equality operator.</param>
    /// <param name="right">The <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure to the right of the equality operator.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(
      CustomAttributeTypedArgument left,
      CustomAttributeTypedArgument right)
    {
      return left.Equals((object) right);
    }

    /// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures are different.</summary>
    /// <param name="left">The <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure to the left of the inequality operator.</param>
    /// <param name="right">The <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure to the right of the inequality operator.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures are different; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(
      CustomAttributeTypedArgument left,
      CustomAttributeTypedArgument right)
    {
      return !left.Equals((object) right);
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> class with the specified type and value.</summary>
    /// <param name="argumentType">The type of the custom attribute argument.</param>
    /// <param name="value">The value of the custom attribute argument.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="argumentType" /> is <see langword="null" />.</exception>
    public CustomAttributeTypedArgument(Type argumentType, object? value)
    {
      if ((object) argumentType == null)
        throw new ArgumentNullException(nameof (argumentType));
      this._value = CustomAttributeTypedArgument.CanonicalizeValue(value);
      this._argumentType = argumentType;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> class with the specified value.</summary>
    /// <param name="value">The value of the custom attribute argument.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="value" /> is <see langword="null" />.</exception>
    public CustomAttributeTypedArgument(object value)
    {
      this._value = value != null ? CustomAttributeTypedArgument.CanonicalizeValue(value) : throw new ArgumentNullException(nameof (value));
      this._argumentType = value.GetType();
    }

    /// <summary>Returns a string consisting of the argument name, the equal sign, and a string representation of the argument value.</summary>
    /// <returns>A string consisting of the argument name, the equal sign, and a string representation of the argument value.</returns>
    public override string ToString() => this.ToString(false);


    #nullable disable
    internal unsafe string ToString(bool typed)
    {
      if ((object) this._argumentType == null)
        return base.ToString();
      if (this.ArgumentType.IsEnum)
      {
        if (!typed)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
          interpolatedStringHandler.AppendLiteral("(");
          interpolatedStringHandler.AppendFormatted(this.ArgumentType.FullName);
          interpolatedStringHandler.AppendLiteral(")");
          interpolatedStringHandler.AppendFormatted<object>(this.Value);
          return interpolatedStringHandler.ToStringAndClear();
        }
        DefaultInterpolatedStringHandler interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(0, 1);
        interpolatedStringHandler1.AppendFormatted<object>(this.Value);
        return interpolatedStringHandler1.ToStringAndClear();
      }
      if (this.Value == null)
        return !typed ? "(" + this.ArgumentType.Name + ")null" : "null";
      if (this.ArgumentType == typeof (string))
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
        interpolatedStringHandler.AppendLiteral("\"");
        interpolatedStringHandler.AppendFormatted<object>(this.Value);
        interpolatedStringHandler.AppendLiteral("\"");
        return interpolatedStringHandler.ToStringAndClear();
      }
      if (this.ArgumentType == typeof (char))
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 1);
        interpolatedStringHandler.AppendLiteral("'");
        interpolatedStringHandler.AppendFormatted<object>(this.Value);
        interpolatedStringHandler.AppendLiteral("'");
        return interpolatedStringHandler.ToStringAndClear();
      }
      if (this.ArgumentType == typeof (Type))
        return "typeof(" + ((Type) this.Value).FullName + ")";
      if (this.ArgumentType.IsArray)
      {
        IList<CustomAttributeTypedArgument> attributeTypedArgumentList = (IList<CustomAttributeTypedArgument>) this.Value;
        Type elementType = this.ArgumentType.GetElementType();
        // ISSUE: untyped stack allocation
        ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(512)), 256));
        valueStringBuilder.Append("new ");
        valueStringBuilder.Append(elementType.IsEnum ? elementType.FullName : elementType.Name);
        valueStringBuilder.Append('[');
        int count = attributeTypedArgumentList.Count;
        valueStringBuilder.Append(count.ToString());
        valueStringBuilder.Append("] { ");
        for (int index = 0; index < count; ++index)
        {
          if (index != 0)
            valueStringBuilder.Append(", ");
          valueStringBuilder.Append(attributeTypedArgumentList[index].ToString(elementType != typeof (object)));
        }
        valueStringBuilder.Append(" }");
        return valueStringBuilder.ToString();
      }
      if (!typed)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
        interpolatedStringHandler.AppendLiteral("(");
        interpolatedStringHandler.AppendFormatted(this.ArgumentType.Name);
        interpolatedStringHandler.AppendLiteral(")");
        interpolatedStringHandler.AppendFormatted<object>(this.Value);
        return interpolatedStringHandler.ToStringAndClear();
      }
      DefaultInterpolatedStringHandler interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(0, 1);
      interpolatedStringHandler2.AppendFormatted<object>(this.Value);
      return interpolatedStringHandler2.ToStringAndClear();
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    public override int GetHashCode() => base.GetHashCode();


    #nullable enable
    /// <summary>Indicates whether this instance and a specified object are equal.</summary>
    /// <param name="obj">Another object to compare to.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => obj == (ValueType) this;

    /// <summary>Gets the type of the argument or of the array argument element.</summary>
    /// <returns>A <see cref="T:System.Type" /> object representing the type of the argument or of the array element.</returns>
    public Type ArgumentType => this._argumentType;

    /// <summary>Gets the value of the argument for a simple argument or for an element of an array argument; gets a collection of values for an array argument.</summary>
    /// <returns>An object that represents the value of the argument or element, or a generic <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> objects that represent the values of an array-type argument.</returns>
    public object? Value => this._value;


    #nullable disable
    private static object CanonicalizeValue(object value) => !(value is Enum @enum) ? value : @enum.GetValue();
  }
}
