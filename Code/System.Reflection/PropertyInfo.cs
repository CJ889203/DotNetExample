// Decompiled with JetBrains decompiler
// Type: System.Reflection.PropertyInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Reflection
{
  /// <summary>Discovers the attributes of a property and provides access to property metadata.</summary>
  public abstract class PropertyInfo : MemberInfo
  {
    /// <summary>Gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a property.</summary>
    /// <returns>A <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a property.</returns>
    public override MemberTypes MemberType => MemberTypes.Property;

    /// <summary>Gets the type of this property.</summary>
    /// <returns>The type of this property.</returns>
    public abstract Type PropertyType { get; }

    /// <summary>When overridden in a derived class, returns an array of all the index parameters for the property.</summary>
    /// <returns>An array of type <see langword="ParameterInfo" /> containing the parameters for the indexes. If the property is not indexed, the array has 0 (zero) elements.</returns>
    public abstract ParameterInfo[] GetIndexParameters();

    /// <summary>Gets the attributes for this property.</summary>
    /// <returns>The attributes of this property.</returns>
    public abstract PropertyAttributes Attributes { get; }

    /// <summary>Gets a value indicating whether the property is the special name.</summary>
    /// <returns>
    /// <see langword="true" /> if this property is the special name; otherwise, <see langword="false" />.</returns>
    public bool IsSpecialName => (this.Attributes & PropertyAttributes.SpecialName) != 0;

    /// <summary>Gets a value indicating whether the property can be read.</summary>
    /// <returns>
    /// <see langword="true" /> if this property can be read; otherwise, <see langword="false" />.</returns>
    public abstract bool CanRead { get; }

    /// <summary>Gets a value indicating whether the property can be written to.</summary>
    /// <returns>
    /// <see langword="true" /> if this property can be written to; otherwise, <see langword="false" />.</returns>
    public abstract bool CanWrite { get; }

    /// <summary>Returns an array whose elements reflect the public <see langword="get" /> and <see langword="set" /> accessors of the property reflected by the current instance.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.MethodInfo" /> objects that reflect the public <see langword="get" /> and <see langword="set" /> accessors of the property reflected by the current instance, if found; otherwise, this method returns an array with zero (0) elements.</returns>
    public MethodInfo[] GetAccessors() => this.GetAccessors(false);

    /// <summary>Returns an array whose elements reflect the public and, if specified, non-public <see langword="get" /> and <see langword="set" /> accessors of the property reflected by the current instance.</summary>
    /// <param name="nonPublic">Indicates whether non-public methods should be returned in the returned array. <see langword="true" /> if non-public methods are to be included; otherwise, <see langword="false" />.</param>
    /// <returns>An array whose elements reflect the <see langword="get" /> and <see langword="set" /> accessors of the property reflected by the current instance. If <paramref name="nonPublic" /> is <see langword="true" />, this array contains public and non-public <see langword="get" /> and <see langword="set" /> accessors. If <paramref name="nonPublic" /> is <see langword="false" />, this array contains only public <see langword="get" /> and <see langword="set" /> accessors. If no accessors with the specified visibility are found, this method returns an array with zero (0) elements.</returns>
    public abstract MethodInfo[] GetAccessors(bool nonPublic);

    /// <summary>Gets the <see langword="get" /> accessor for this property.</summary>
    /// <returns>The <see langword="get" /> accessor for this property.</returns>
    public virtual MethodInfo? GetMethod => this.GetGetMethod(true);

    /// <summary>Returns the public <see langword="get" /> accessor for this property.</summary>
    /// <returns>A <see langword="MethodInfo" /> object representing the public <see langword="get" /> accessor for this property, or <see langword="null" /> if the <see langword="get" /> accessor is non-public or does not exist.</returns>
    public MethodInfo? GetGetMethod() => this.GetGetMethod(false);

    /// <summary>When overridden in a derived class, returns the public or non-public <see langword="get" /> accessor for this property.</summary>
    /// <param name="nonPublic">Indicates whether a non-public <see langword="get" /> accessor should be returned. <see langword="true" /> if a non-public accessor is to be returned; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.Security.SecurityException">The requested method is non-public and the caller does not have <see cref="T:System.Security.Permissions.ReflectionPermission" /> to reflect on this non-public method.</exception>
    /// <returns>A <see langword="MethodInfo" /> object representing the <see langword="get" /> accessor for this property, if <paramref name="nonPublic" /> is <see langword="true" />. Returns <see langword="null" /> if <paramref name="nonPublic" /> is <see langword="false" /> and the <see langword="get" /> accessor is non-public, or if <paramref name="nonPublic" /> is <see langword="true" /> but no <see langword="get" /> accessors exist.</returns>
    public abstract MethodInfo? GetGetMethod(bool nonPublic);

    /// <summary>Gets the <see langword="set" /> accessor for this property.</summary>
    /// <returns>The <see langword="set" /> accessor for this property, or <see langword="null" /> if the property is read-only.</returns>
    public virtual MethodInfo? SetMethod => this.GetSetMethod(true);

    /// <summary>Returns the public <see langword="set" /> accessor for this property.</summary>
    /// <returns>The <see langword="MethodInfo" /> object representing the <see langword="Set" /> method for this property if the <see langword="set" /> accessor is public, or <see langword="null" /> if the <see langword="set" /> accessor is not public.</returns>
    public MethodInfo? GetSetMethod() => this.GetSetMethod(false);

    /// <summary>When overridden in a derived class, returns the <see langword="set" /> accessor for this property.</summary>
    /// <param name="nonPublic">Indicates whether the accessor should be returned if it is non-public. <see langword="true" /> if a non-public accessor is to be returned; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.Security.SecurityException">The requested method is non-public and the caller does not have <see cref="T:System.Security.Permissions.ReflectionPermission" /> to reflect on this non-public method.</exception>
    /// <returns>This property's <see langword="Set" /> method, or <see langword="null" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Condition</description></listheader><item><term> The <see langword="Set" /> method for this property.</term><description> The <see langword="set" /> accessor is public, OR <paramref name="nonPublic" /> is <see langword="true" /> and the <see langword="set" /> accessor is non-public.</description></item><item><term><see langword="null" /></term><description><paramref name="nonPublic" /> is <see langword="true" />, but the property is read-only, OR <paramref name="nonPublic" /> is <see langword="false" /> and the <see langword="set" /> accessor is non-public, OR there is no <see langword="set" /> accessor.</description></item></list></returns>
    public abstract MethodInfo? GetSetMethod(bool nonPublic);

    /// <summary>Returns an array of types representing the optional custom modifiers of the property.</summary>
    /// <returns>An array of <see cref="T:System.Type" /> objects that identify the optional custom modifiers of the current property, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
    public virtual Type[] GetOptionalCustomModifiers() => Type.EmptyTypes;

    /// <summary>Returns an array of types representing the required custom modifiers of the property.</summary>
    /// <returns>An array of <see cref="T:System.Type" /> objects that identify the required custom modifiers of the current property, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
    public virtual Type[] GetRequiredCustomModifiers() => Type.EmptyTypes;

    /// <summary>Returns the property value of a specified object.</summary>
    /// <param name="obj">The object whose property value will be returned.</param>
    /// <returns>The property value of the specified object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public object? GetValue(object? obj) => this.GetValue(obj, (object[]) null);

    /// <summary>Returns the property value of a specified object with optional index values for indexed properties.</summary>
    /// <param name="obj">The object whose property value will be returned.</param>
    /// <param name="index">Optional index values for indexed properties. The indexes of indexed properties are zero-based. This value should be <see langword="null" /> for non-indexed properties.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="index" /> array does not contain the type of arguments needed.
    /// 
    /// -or-
    /// 
    /// The property's <see langword="get" /> accessor is not found.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The object does not match the target type, or a property is an instance property but <paramref name="obj" /> is <see langword="null" />.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.</exception>
    /// <exception cref="T:System.Reflection.TargetParameterCountException">The number of parameters in <paramref name="index" /> does not match the number of parameters the indexed property takes.</exception>
    /// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while retrieving the property value. For example, an index value specified for an indexed property is out of range. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
    /// <returns>The property value of the specified object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public virtual object? GetValue(object? obj, object?[]? index) => this.GetValue(obj, BindingFlags.Default, (Binder) null, index, (CultureInfo) null);

    /// <summary>When overridden in a derived class, returns the property value of a specified object that has the specified binding, index, and culture-specific information.</summary>
    /// <param name="obj">The object whose property value will be returned.</param>
    /// <param name="invokeAttr">A bitwise combination of the following enumeration members that specify the invocation attribute: <see langword="InvokeMethod" />, <see langword="CreateInstance" />, <see langword="Static" />, <see langword="GetField" />, <see langword="SetField" />, <see langword="GetProperty" />, and <see langword="SetProperty" />. You must specify a suitable invocation attribute. For example, to invoke a static member, set the <see langword="Static" /> flag.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects through reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="index">Optional index values for indexed properties. This value should be <see langword="null" /> for non-indexed properties.</param>
    /// <param name="culture">The culture for which the resource is to be localized. If the resource is not localized for this culture, the <see cref="P:System.Globalization.CultureInfo.Parent" /> property will be called successively in search of a match. If this value is <see langword="null" />, the culture-specific information is obtained from the <see cref="P:System.Globalization.CultureInfo.CurrentUICulture" /> property.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="index" /> array does not contain the type of arguments needed.
    /// 
    /// -or-
    /// 
    /// The property's <see langword="get" /> accessor is not found.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The object does not match the target type, or a property is an instance property but <paramref name="obj" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Reflection.TargetParameterCountException">The number of parameters in <paramref name="index" /> does not match the number of parameters the indexed property takes.</exception>
    /// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while retrieving the property value. For example, an index value specified for an indexed property is out of range. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
    /// <returns>The property value of the specified object.</returns>
    public abstract object? GetValue(
      object? obj,
      BindingFlags invokeAttr,
      Binder? binder,
      object?[]? index,
      CultureInfo? culture);

    /// <summary>Returns a literal value associated with the property by a compiler.</summary>
    /// <exception cref="T:System.InvalidOperationException">The Constant table in unmanaged metadata does not contain a constant value for the current property.</exception>
    /// <exception cref="T:System.FormatException">The type of the value is not one of the types permitted by the Common Language Specification (CLS). See the ECMA Partition II specification, Metadata.</exception>
    /// <returns>An <see cref="T:System.Object" /> that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is <see langword="null" />.</returns>
    public virtual object? GetConstantValue() => throw NotImplemented.ByDesign;

    /// <summary>Returns a literal value associated with the property by a compiler.</summary>
    /// <exception cref="T:System.InvalidOperationException">The Constant table in unmanaged metadata does not contain a constant value for the current property.</exception>
    /// <exception cref="T:System.FormatException">The type of the value is not one of the types permitted by the Common Language Specification (CLS). See the ECMA Partition II specification, Metadata Logical Format: Other Structures, Element Types used in Signatures.</exception>
    /// <returns>An <see cref="T:System.Object" /> that contains the literal value associated with the property. If the literal value is a class type with an element value of zero, the return value is <see langword="null" />.</returns>
    public virtual object? GetRawConstantValue() => throw NotImplemented.ByDesign;

    /// <summary>Sets the property value of a specified object.</summary>
    /// <param name="obj">The object whose property value will be set.</param>
    /// <param name="value">The new property value.</param>
    /// <exception cref="T:System.ArgumentException">The property's <see langword="set" /> accessor is not found.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> cannot be converted to the type of <see cref="P:System.Reflection.PropertyInfo.PropertyType" />.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The type of <paramref name="obj" /> does not match the target type, or a property is an instance property but <paramref name="obj" /> is <see langword="null" />.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.</exception>
    /// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while setting the property value. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public void SetValue(object? obj, object? value) => this.SetValue(obj, value, (object[]) null);

    /// <summary>Sets the property value of a specified object with optional index values for index properties.</summary>
    /// <param name="obj">The object whose property value will be set.</param>
    /// <param name="value">The new property value.</param>
    /// <param name="index">Optional index values for indexed properties. This value should be <see langword="null" /> for non-indexed properties.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="index" /> array does not contain the type of arguments needed.
    /// 
    /// -or-
    /// 
    /// The property's <see langword="set" /> accessor is not found.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> cannot be converted to the type of <see cref="P:System.Reflection.PropertyInfo.PropertyType" />.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The object does not match the target type, or a property is an instance property but <paramref name="obj" /> is <see langword="null" />.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.</exception>
    /// <exception cref="T:System.Reflection.TargetParameterCountException">The number of parameters in <paramref name="index" /> does not match the number of parameters the indexed property takes.</exception>
    /// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while setting the property value. For example, an index value specified for an indexed property is out of range. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public virtual void SetValue(object? obj, object? value, object?[]? index) => this.SetValue(obj, value, BindingFlags.Default, (Binder) null, index, (CultureInfo) null);

    /// <summary>When overridden in a derived class, sets the property value for a specified object that has the specified binding, index, and culture-specific information.</summary>
    /// <param name="obj">The object whose property value will be set.</param>
    /// <param name="value">The new property value.</param>
    /// <param name="invokeAttr">A bitwise combination of the following enumeration members that specify the invocation attribute: <see langword="InvokeMethod" />, <see langword="CreateInstance" />, <see langword="Static" />, <see langword="GetField" />, <see langword="SetField" />, <see langword="GetProperty" />, or <see langword="SetProperty" />. You must specify a suitable invocation attribute. For example, to invoke a static member, set the <see langword="Static" /> flag.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects through reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="index">Optional index values for indexed properties. This value should be <see langword="null" /> for non-indexed properties.</param>
    /// <param name="culture">The culture for which the resource is to be localized. If the resource is not localized for this culture, the <see cref="P:System.Globalization.CultureInfo.Parent" /> property will be called successively in search of a match. If this value is <see langword="null" />, the culture-specific information is obtained from the <see cref="P:System.Globalization.CultureInfo.CurrentUICulture" /> property.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="index" /> array does not contain the type of arguments needed.
    /// 
    /// -or-
    /// 
    /// The property's <see langword="set" /> accessor is not found.
    /// 
    /// -or-
    /// 
    /// <paramref name="value" /> cannot be converted to the type of <see cref="P:System.Reflection.PropertyInfo.PropertyType" />.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The object does not match the target type, or a property is an instance property but <paramref name="obj" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Reflection.TargetParameterCountException">The number of parameters in <paramref name="index" /> does not match the number of parameters the indexed property takes.</exception>
    /// <exception cref="T:System.MethodAccessException">There was an illegal attempt to access a private or protected method inside a class.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred while setting the property value. For example, an index value specified for an indexed property is out of range. The <see cref="P:System.Exception.InnerException" /> property indicates the reason for the error.</exception>
    public abstract void SetValue(
      object? obj,
      object? value,
      BindingFlags invokeAttr,
      Binder? binder,
      object?[]? index,
      CultureInfo? culture);

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Indicates whether two <see cref="T:System.Reflection.PropertyInfo" /> objects are equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(PropertyInfo? left, PropertyInfo? right)
    {
      if ((object) right == null)
        return (object) left == null;
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>Indicates whether two <see cref="T:System.Reflection.PropertyInfo" /> objects are not equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(PropertyInfo? left, PropertyInfo? right) => !(left == right);
  }
}
