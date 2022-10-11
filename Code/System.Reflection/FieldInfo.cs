// Decompiled with JetBrains decompiler
// Type: System.Reflection.FieldInfo
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
  /// <summary>Discovers the attributes of a field and provides access to field metadata.</summary>
  public abstract class FieldInfo : MemberInfo
  {
    /// <summary>Gets a <see cref="T:System.Reflection.FieldInfo" /> for the field represented by the specified handle.</summary>
    /// <param name="handle">A <see cref="T:System.RuntimeFieldHandle" /> structure that contains the handle to the internal metadata representation of a field.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="handle" /> is invalid.</exception>
    /// <returns>A <see cref="T:System.Reflection.FieldInfo" /> object representing the field specified by <paramref name="handle" />.</returns>
    public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle)
    {
      FieldInfo fieldFromHandle = !handle.IsNullHandle() ? RuntimeType.GetFieldInfo(handle.GetRuntimeFieldInfo()) : throw new ArgumentException(SR.Argument_InvalidHandle, nameof (handle));
      Type declaringType = fieldFromHandle.DeclaringType;
      if (declaringType != (Type) null && declaringType.IsGenericType)
        throw new ArgumentException(SR.Format(SR.Argument_FieldDeclaringTypeGeneric, (object) fieldFromHandle.Name, (object) declaringType.GetGenericTypeDefinition()));
      return fieldFromHandle;
    }

    /// <summary>Gets a <see cref="T:System.Reflection.FieldInfo" /> for the field represented by the specified handle, for the specified generic type.</summary>
    /// <param name="handle">A <see cref="T:System.RuntimeFieldHandle" /> structure that contains the handle to the internal metadata representation of a field.</param>
    /// <param name="declaringType">A <see cref="T:System.RuntimeTypeHandle" /> structure that contains the handle to the generic type that defines the field.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="handle" /> is invalid.
    /// 
    /// -or-
    /// 
    /// <paramref name="declaringType" /> is not compatible with <paramref name="handle" />. For example, <paramref name="declaringType" /> is the runtime type handle of the generic type definition, and <paramref name="handle" /> comes from a constructed type.</exception>
    /// <returns>A <see cref="T:System.Reflection.FieldInfo" /> object representing the field specified by <paramref name="handle" />, in the generic type specified by <paramref name="declaringType" />.</returns>
    public static FieldInfo GetFieldFromHandle(
      RuntimeFieldHandle handle,
      RuntimeTypeHandle declaringType)
    {
      return !handle.IsNullHandle() ? RuntimeType.GetFieldInfo(declaringType.GetRuntimeType(), handle.GetRuntimeFieldInfo()) : throw new ArgumentException(SR.Argument_InvalidHandle);
    }

    /// <summary>Gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a field.</summary>
    /// <returns>A <see cref="T:System.Reflection.MemberTypes" /> value indicating that this member is a field.</returns>
    public override MemberTypes MemberType => MemberTypes.Field;

    /// <summary>Gets the attributes associated with this field.</summary>
    /// <returns>The <see langword="FieldAttributes" /> for this field.</returns>
    public abstract FieldAttributes Attributes { get; }

    /// <summary>Gets the type of this field object.</summary>
    /// <returns>The type of this field object.</returns>
    public abstract Type FieldType { get; }

    /// <summary>Gets a value indicating whether the field can only be set in the body of the constructor.</summary>
    /// <returns>
    /// <see langword="true" /> if the field has the <see langword="InitOnly" /> attribute set; otherwise, <see langword="false" />.</returns>
    public bool IsInitOnly => (this.Attributes & FieldAttributes.InitOnly) != 0;

    /// <summary>Gets a value indicating whether the value is written at compile time and cannot be changed.</summary>
    /// <returns>
    /// <see langword="true" /> if the field has the <see langword="Literal" /> attribute set; otherwise, <see langword="false" />.</returns>
    public bool IsLiteral => (this.Attributes & FieldAttributes.Literal) != 0;

    /// <summary>Gets a value indicating whether this field has the <see langword="NotSerialized" /> attribute.</summary>
    /// <returns>
    /// <see langword="true" /> if the field has the <see langword="NotSerialized" /> attribute set; otherwise, <see langword="false" />.</returns>
    public bool IsNotSerialized => (this.Attributes & FieldAttributes.NotSerialized) != 0;

    /// <summary>Gets a value indicating whether the corresponding <see langword="PinvokeImpl" /> attribute is set in <see cref="T:System.Reflection.FieldAttributes" />.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see langword="PinvokeImpl" /> attribute is set in <see cref="T:System.Reflection.FieldAttributes" />; otherwise, <see langword="false" />.</returns>
    public bool IsPinvokeImpl => (this.Attributes & FieldAttributes.PinvokeImpl) != 0;

    /// <summary>Gets a value indicating whether the corresponding <see langword="SpecialName" /> attribute is set in the <see cref="T:System.Reflection.FieldAttributes" /> enumerator.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see langword="SpecialName" /> attribute is set in <see cref="T:System.Reflection.FieldAttributes" />; otherwise, <see langword="false" />.</returns>
    public bool IsSpecialName => (this.Attributes & FieldAttributes.SpecialName) != 0;

    /// <summary>Gets a value indicating whether the field is static.</summary>
    /// <returns>
    /// <see langword="true" /> if this field is static; otherwise, <see langword="false" />.</returns>
    public bool IsStatic => (this.Attributes & FieldAttributes.Static) != 0;

    /// <summary>Gets a value indicating whether the potential visibility of this field is described by <see cref="F:System.Reflection.FieldAttributes.Assembly" />; that is, the field is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.</summary>
    /// <returns>
    /// <see langword="true" /> if the visibility of this field is exactly described by <see cref="F:System.Reflection.FieldAttributes.Assembly" />; otherwise, <see langword="false" />.</returns>
    public bool IsAssembly => (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;

    /// <summary>Gets a value indicating whether the visibility of this field is described by <see cref="F:System.Reflection.FieldAttributes.Family" />; that is, the field is visible only within its class and derived classes.</summary>
    /// <returns>
    /// <see langword="true" /> if access to this field is exactly described by <see cref="F:System.Reflection.FieldAttributes.Family" />; otherwise, <see langword="false" />.</returns>
    public bool IsFamily => (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;

    /// <summary>Gets a value indicating whether the visibility of this field is described by <see cref="F:System.Reflection.FieldAttributes.FamANDAssem" />; that is, the field can be accessed from derived classes, but only if they are in the same assembly.</summary>
    /// <returns>
    /// <see langword="true" /> if access to this field is exactly described by <see cref="F:System.Reflection.FieldAttributes.FamANDAssem" />; otherwise, <see langword="false" />.</returns>
    public bool IsFamilyAndAssembly => (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;

    /// <summary>Gets a value indicating whether the potential visibility of this field is described by <see cref="F:System.Reflection.FieldAttributes.FamORAssem" />; that is, the field can be accessed by derived classes wherever they are, and by classes in the same assembly.</summary>
    /// <returns>
    /// <see langword="true" /> if access to this field is exactly described by <see cref="F:System.Reflection.FieldAttributes.FamORAssem" />; otherwise, <see langword="false" />.</returns>
    public bool IsFamilyOrAssembly => (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;

    /// <summary>Gets a value indicating whether the field is private.</summary>
    /// <returns>
    /// <see langword="true" /> if the field is private; otherwise; <see langword="false" />.</returns>
    public bool IsPrivate => (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;

    /// <summary>Gets a value indicating whether the field is public.</summary>
    /// <returns>
    /// <see langword="true" /> if this field is public; otherwise, <see langword="false" />.</returns>
    public bool IsPublic => (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;

    /// <summary>Gets a value that indicates whether the current field is security-critical or security-safe-critical at the current trust level.</summary>
    /// <returns>
    /// <see langword="true" /> if the current field is security-critical or security-safe-critical at the current trust level; <see langword="false" /> if it is transparent.</returns>
    public virtual bool IsSecurityCritical => true;

    /// <summary>Gets a value that indicates whether the current field is security-safe-critical at the current trust level.</summary>
    /// <returns>
    /// <see langword="true" /> if the current field is security-safe-critical at the current trust level; <see langword="false" /> if it is security-critical or transparent.</returns>
    public virtual bool IsSecuritySafeCritical => false;

    /// <summary>Gets a value that indicates whether the current field is transparent at the current trust level.</summary>
    /// <returns>
    /// <see langword="true" /> if the field is security-transparent at the current trust level; otherwise, <see langword="false" />.</returns>
    public virtual bool IsSecurityTransparent => false;

    /// <summary>Gets a <see langword="RuntimeFieldHandle" />, which is a handle to the internal metadata representation of a field.</summary>
    /// <returns>A handle to the internal metadata representation of a field.</returns>
    public abstract RuntimeFieldHandle FieldHandle { get; }

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Indicates whether two <see cref="T:System.Reflection.FieldInfo" /> objects are equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(FieldInfo? left, FieldInfo? right)
    {
      if ((object) right == null)
        return (object) left == null;
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>Indicates whether two <see cref="T:System.Reflection.FieldInfo" /> objects are not equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(FieldInfo? left, FieldInfo? right) => !(left == right);

    /// <summary>When overridden in a derived class, returns the value of a field supported by a given object.</summary>
    /// <param name="obj">The object whose field value will be returned.</param>
    /// <exception cref="T:System.Reflection.TargetException">The field is non-static and <paramref name="obj" /> is <see langword="null" />.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.</exception>
    /// <exception cref="T:System.NotSupportedException">A field is marked literal, but the field does not have one of the accepted literal types.</exception>
    /// <exception cref="T:System.FieldAccessException">The caller does not have permission to access this field.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.ArgumentException">The method is neither declared nor inherited by the class of <paramref name="obj" />.</exception>
    /// <returns>An object containing the value of the field reflected by this instance.</returns>
    public abstract object? GetValue(object? obj);

    /// <summary>Sets the value of the field supported by the given object.</summary>
    /// <param name="obj">The object whose field value will be set.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <exception cref="T:System.FieldAccessException">The caller does not have permission to access this field.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The <paramref name="obj" /> parameter is <see langword="null" /> and the field is an instance field.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.</exception>
    /// <exception cref="T:System.ArgumentException">The field does not exist on the object.
    /// 
    /// -or-
    /// 
    /// The <paramref name="value" /> parameter cannot be converted and stored in the field.</exception>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public void SetValue(object? obj, object? value) => this.SetValue(obj, value, BindingFlags.Default, Type.DefaultBinder, (CultureInfo) null);

    /// <summary>When overridden in a derived class, sets the value of the field supported by the given object.</summary>
    /// <param name="obj">The object whose field value will be set.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <param name="invokeAttr">A field of <see langword="Binder" /> that specifies the type of binding that is desired (for example, <see langword="Binder.CreateInstance" /> or <see langword="Binder.ExactBinding" />).</param>
    /// <param name="binder">A set of properties that enables the binding, coercion of argument types, and invocation of members through reflection. If <paramref name="binder" /> is <see langword="null" />, then <see langword="Binder.DefaultBinding" /> is used.</param>
    /// <param name="culture">The software preferences of a particular culture.</param>
    /// <exception cref="T:System.FieldAccessException">The caller does not have permission to access this field.</exception>
    /// <exception cref="T:System.Reflection.TargetException">The <paramref name="obj" /> parameter is <see langword="null" /> and the field is an instance field.</exception>
    /// <exception cref="T:System.ArgumentException">The field does not exist on the object.
    /// 
    /// -or-
    /// 
    /// The <paramref name="value" /> parameter cannot be converted and stored in the field.</exception>
    public abstract void SetValue(
      object? obj,
      object? value,
      BindingFlags invokeAttr,
      Binder? binder,
      CultureInfo? culture);

    /// <summary>Sets the value of the field supported by the given object.</summary>
    /// <param name="obj">A <see cref="T:System.TypedReference" /> structure that encapsulates a managed pointer to a location and a runtime representation of the type that can be stored at that location.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <exception cref="T:System.NotSupportedException">The caller requires the Common Language Specification (CLS) alternative, but called this method instead.</exception>
    [CLSCompliant(false)]
    public virtual void SetValueDirect(TypedReference obj, object value) => throw new NotSupportedException(SR.NotSupported_AbstractNonCLS);

    /// <summary>Returns the value of a field supported by a given object.</summary>
    /// <param name="obj">A <see cref="T:System.TypedReference" /> structure that encapsulates a managed pointer to a location and a runtime representation of the type that might be stored at that location.</param>
    /// <exception cref="T:System.NotSupportedException">The caller requires the Common Language Specification (CLS) alternative, but called this method instead.</exception>
    /// <returns>An <see langword="Object" /> containing a field value.</returns>
    [CLSCompliant(false)]
    public virtual object? GetValueDirect(TypedReference obj) => throw new NotSupportedException(SR.NotSupported_AbstractNonCLS);

    /// <summary>Returns a literal value associated with the field by a compiler.</summary>
    /// <exception cref="T:System.InvalidOperationException">The Constant table in unmanaged metadata does not contain a constant value for the current field.</exception>
    /// <exception cref="T:System.FormatException">The type of the value is not one of the types permitted by the Common Language Specification (CLS). See the ECMA Partition II specification Metadata Logical Format: Other Structures, Element Types used in Signatures.</exception>
    /// <exception cref="T:System.NotSupportedException">The constant value for the field is not set.</exception>
    /// <returns>An <see cref="T:System.Object" /> that contains the literal value associated with the field. If the literal value is a class type with an element value of zero, the return value is <see langword="null" />.</returns>
    public virtual object? GetRawConstantValue() => throw new NotSupportedException(SR.NotSupported_AbstractNonCLS);

    /// <summary>Gets an array of types that identify the optional custom modifiers of the field.</summary>
    /// <returns>An array of <see cref="T:System.Type" /> objects that identify the optional custom modifiers of the current field, such as <see cref="T:System.Runtime.CompilerServices.IsConst" />.</returns>
    public virtual Type[] GetOptionalCustomModifiers() => throw NotImplemented.ByDesign;

    /// <summary>Gets an array of types that identify the required custom modifiers of the property.</summary>
    /// <returns>An array of <see cref="T:System.Type" /> objects that identify the required custom modifiers of the current property, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
    public virtual Type[] GetRequiredCustomModifiers() => throw NotImplemented.ByDesign;
  }
}
