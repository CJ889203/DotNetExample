// Decompiled with JetBrains decompiler
// Type: System.Reflection.CustomAttributeNamedArgument
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents a named argument of a custom attribute in the reflection-only context.</summary>
  public readonly struct CustomAttributeNamedArgument
  {

    #nullable disable
    private readonly MemberInfo _memberInfo;
    private readonly CustomAttributeTypedArgument _value;

    /// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are equivalent.</summary>
    /// <param name="left">The structure to the left of the equality operator.</param>
    /// <param name="right">The structure to the right of the equality operator.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are equal; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(
      CustomAttributeNamedArgument left,
      CustomAttributeNamedArgument right)
    {
      return left.Equals((object) right);
    }

    /// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are different.</summary>
    /// <param name="left">The structure to the left of the inequality operator.</param>
    /// <param name="right">The structure to the right of the inequality operator.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are different; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(
      CustomAttributeNamedArgument left,
      CustomAttributeNamedArgument right)
    {
      return !left.Equals((object) right);
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> class, which represents the specified field or property of the custom attribute, and specifies the value of the field or property.</summary>
    /// <param name="memberInfo">A field or property of the custom attribute. The new <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> object represents this member and its value.</param>
    /// <param name="value">The value of the field or property of the custom attribute.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="memberInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="memberInfo" /> is not a field or property of the custom attribute.</exception>
    public CustomAttributeNamedArgument(MemberInfo memberInfo, object? value)
    {
      FieldInfo fieldInfo = (object) memberInfo != null ? memberInfo as FieldInfo : throw new ArgumentNullException(nameof (memberInfo));
      Type type;
      if ((object) fieldInfo == null)
        type = (memberInfo as PropertyInfo ?? throw new ArgumentException(SR.Argument_InvalidMemberForNamedArgument)).PropertyType;
      else
        type = fieldInfo.FieldType;
      Type argumentType = type;
      this._memberInfo = memberInfo;
      this._value = new CustomAttributeTypedArgument(argumentType, value);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> class, which represents the specified field or property of the custom attribute, and specifies a <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> object that describes the type and value of the field or property.</summary>
    /// <param name="memberInfo">A field or property of the custom attribute. The new <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> object represents this member and its value.</param>
    /// <param name="typedArgument">An object that describes the type and value of the field or property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="memberInfo" /> is <see langword="null" />.</exception>
    public CustomAttributeNamedArgument(
      MemberInfo memberInfo,
      CustomAttributeTypedArgument typedArgument)
    {
      this._memberInfo = memberInfo ?? throw new ArgumentNullException(nameof (memberInfo));
      this._value = typedArgument;
    }

    /// <summary>Returns a string that consists of the argument name, the equal sign, and a string representation of the argument value.</summary>
    /// <returns>A string that consists of the argument name, the equal sign, and a string representation of the argument value.</returns>
    public override string ToString() => (object) this._memberInfo == null ? base.ToString() : this.MemberInfo.Name + " = " + this.TypedValue.ToString(this.ArgumentType != typeof (object));

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => obj == (ValueType) this;

    internal Type ArgumentType
    {
      get
      {
        FieldInfo memberInfo = this._memberInfo as FieldInfo;
        return (object) memberInfo == null ? ((PropertyInfo) this._memberInfo).PropertyType : memberInfo.FieldType;
      }
    }

    /// <summary>Gets the attribute member that would be used to set the named argument.</summary>
    /// <returns>The attribute member that would be used to set the named argument.</returns>
    public MemberInfo MemberInfo => this._memberInfo;

    /// <summary>Gets a <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure that can be used to obtain the type and value of the current named argument.</summary>
    /// <returns>A structure that can be used to obtain the type and value of the current named argument.</returns>
    public CustomAttributeTypedArgument TypedValue => this._value;

    /// <summary>Gets the name of the attribute member that would be used to set the named argument.</summary>
    /// <returns>The name of the attribute member that would be used to set the named argument.</returns>
    public string MemberName => this.MemberInfo.Name;

    /// <summary>Gets a value that indicates whether the named argument is a field.</summary>
    /// <returns>
    /// <see langword="true" /> if the named argument is a field; otherwise, <see langword="false" />.</returns>
    public bool IsField => this.MemberInfo is FieldInfo;
  }
}
