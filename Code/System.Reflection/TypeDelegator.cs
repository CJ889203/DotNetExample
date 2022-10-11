// Decompiled with JetBrains decompiler
// Type: System.Reflection.TypeDelegator
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Globalization;


#nullable enable
namespace System.Reflection
{
  /// <summary>Wraps a <see cref="T:System.Type" /> object and delegates methods to that <see langword="Type" />.</summary>
  public class TypeDelegator : TypeInfo
  {
    /// <summary>A value indicating type information.</summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    protected Type typeImpl;

    /// <summary>Returns a value that indicates whether the specified type can be assigned to this type.</summary>
    /// <param name="typeInfo">The type to check.</param>
    /// <returns>
    /// <see langword="true" /> if the specified type can be assigned to this type; otherwise, <see langword="false" />.</returns>
    public override bool IsAssignableFrom([NotNullWhen(true)] TypeInfo? typeInfo) => !((Type) typeInfo == (Type) null) && this.IsAssignableFrom(typeInfo.AsType());

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.TypeDelegator" /> class with default properties.</summary>
    protected TypeDelegator()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.TypeDelegator" /> class specifying the encapsulating instance.</summary>
    /// <param name="delegatingType">The instance of the class <see cref="T:System.Type" /> that encapsulates the call to the method of an object.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="delegatingType" /> is <see langword="null" />.</exception>
    public TypeDelegator([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type delegatingType) => this.typeImpl = (object) delegatingType != null ? delegatingType : throw new ArgumentNullException(nameof (delegatingType));

    /// <summary>Gets the GUID (globally unique identifier) of the implemented type.</summary>
    /// <returns>A GUID.</returns>
    public override Guid GUID => this.typeImpl.GUID;

    /// <summary>Gets a value that identifies this entity in metadata.</summary>
    /// <returns>A value which, in combination with the module, uniquely identifies this entity in metadata.</returns>
    public override int MetadataToken => this.typeImpl.MetadataToken;

    /// <summary>Invokes the specified member. The method that is to be invoked must be accessible and provide the most specific match with the specified argument list, under the constraints of the specified binder and invocation attributes.</summary>
    /// <param name="name">The name of the member to invoke. This may be a constructor, method, property, or field. If an empty string ("") is passed, the default member is invoked.</param>
    /// <param name="invokeAttr">The invocation attribute. This must be one of the following <see cref="T:System.Reflection.BindingFlags" /> : <see langword="InvokeMethod" />, <see langword="CreateInstance" />, <see langword="Static" />, <see langword="GetField" />, <see langword="SetField" />, <see langword="GetProperty" />, or <see langword="SetProperty" />. A suitable invocation attribute must be specified. If a static member is to be invoked, the <see langword="Static" /> flag must be set.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see langword="MemberInfo" /> objects via reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used. See <see cref="T:System.Reflection.Binder" />.</param>
    /// <param name="target">The object on which to invoke the specified member.</param>
    /// <param name="args">An array of type <see langword="Object" /> that contains the number, order, and type of the parameters of the member to be invoked. If <paramref name="args" /> contains an uninitialized <see langword="Object" />, it is treated as empty, which, with the default binder, can be widened to 0, 0.0 or a string.</param>
    /// <param name="modifiers">An array of type <see langword="ParameterModifier" /> that is the same length as <paramref name="args" />, with elements that represent the attributes associated with the arguments of the member to be invoked. A parameter has attributes associated with it in the member's signature. For ByRef, use <see langword="ParameterModifier.ByRef" />, and for none, use <see langword="ParameterModifier.None" />. The default binder does exact matching on these. Attributes such as <see langword="In" /> and <see langword="InOut" /> are not used in binding, and can be viewed using <see langword="ParameterInfo" />.</param>
    /// <param name="culture">An instance of <see langword="CultureInfo" /> used to govern the coercion of types. This is necessary, for example, to convert a string that represents 1000 to a <see cref="T:System.Double" /> value, since 1000 is represented differently by different cultures. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread's <see cref="T:System.Globalization.CultureInfo" /> is used.</param>
    /// <param name="namedParameters">A string array containing parameter names that match up, starting at element zero, with the <paramref name="args" /> array. There must be no holes in the array. If <paramref name="args" />. <see langword="Length" /> is greater than <paramref name="namedParameters" />. <see langword="Length" />, the remaining parameters are filled in order.</param>
    /// <returns>An <see langword="Object" /> representing the return value of the invoked member.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public override object? InvokeMember(
      string name,
      BindingFlags invokeAttr,
      Binder? binder,
      object? target,
      object?[]? args,
      ParameterModifier[]? modifiers,
      CultureInfo? culture,
      string[]? namedParameters)
    {
      return this.typeImpl.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
    }

    /// <summary>Gets the module that contains the implemented type.</summary>
    /// <returns>A <see cref="T:System.Reflection.Module" /> object representing the module of the implemented type.</returns>
    public override Module Module => this.typeImpl.Module;

    /// <summary>Gets the assembly of the implemented type.</summary>
    /// <returns>An <see cref="T:System.Reflection.Assembly" /> object representing the assembly of the implemented type.</returns>
    public override Assembly Assembly => this.typeImpl.Assembly;

    /// <summary>Gets a handle to the internal metadata representation of an implemented type.</summary>
    /// <returns>A <see langword="RuntimeTypeHandle" /> object.</returns>
    public override RuntimeTypeHandle TypeHandle => this.typeImpl.TypeHandle;

    /// <summary>Gets the name of the implemented type, with the path removed.</summary>
    /// <returns>A <see langword="String" /> containing the type's non-qualified name.</returns>
    public override string Name => this.typeImpl.Name;

    /// <summary>Gets the fully qualified name of the implemented type.</summary>
    /// <returns>A <see langword="String" /> containing the type's fully qualified name.</returns>
    public override string? FullName => this.typeImpl.FullName;

    /// <summary>Gets the namespace of the implemented type.</summary>
    /// <returns>A <see langword="String" /> containing the type's namespace.</returns>
    public override string? Namespace => this.typeImpl.Namespace;

    /// <summary>Gets the assembly's fully qualified name.</summary>
    /// <returns>A <see langword="String" /> containing the assembly's fully qualified name.</returns>
    public override string? AssemblyQualifiedName => this.typeImpl.AssemblyQualifiedName;

    /// <summary>Gets the base type for the current type.</summary>
    /// <returns>The base type for a type.</returns>
    public override Type? BaseType => this.typeImpl.BaseType;

    /// <summary>Gets the constructor that implemented the <see langword="TypeDelegator" />.</summary>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see langword="MemberInfo" /> objects using reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="callConvention">The calling conventions.</param>
    /// <param name="types">An array of type <see langword="Type" /> containing a list of the parameter number, order, and types. Types cannot be <see langword="null" />; use an appropriate <see langword="GetMethod" /> method or an empty array to search for a method without parameters.</param>
    /// <param name="modifiers">An array of type <see langword="ParameterModifier" /> having the same length as the <paramref name="types" /> array, whose elements represent the attributes associated with the parameters of the method to get.</param>
    /// <returns>A <see langword="ConstructorInfo" /> object for the method that matches the specified criteria, or <see langword="null" /> if a match cannot be found.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
    protected override ConstructorInfo? GetConstructorImpl(
      BindingFlags bindingAttr,
      Binder? binder,
      CallingConventions callConvention,
      Type[] types,
      ParameterModifier[]? modifiers)
    {
      return this.typeImpl.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
    }

    /// <summary>Returns an array of <see cref="T:System.Reflection.ConstructorInfo" /> objects representing constructors defined for the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <returns>An array of type <see langword="ConstructorInfo" /> containing the specified constructors defined for this class. If no constructors are defined, an empty array is returned. Depending on the value of a specified parameter, only public constructors or both public and non-public constructors will be returned.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
    public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => this.typeImpl.GetConstructors(bindingAttr);

    /// <summary>Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.</summary>
    /// <param name="name">The method name.</param>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see langword="MemberInfo" /> objects using reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="callConvention">The calling conventions.</param>
    /// <param name="types">An array of type <see langword="Type" /> containing a list of the parameter number, order, and types. Types cannot be <see langword="null" />; use an appropriate <see langword="GetMethod" /> method or an empty array to search for a method without parameters.</param>
    /// <param name="modifiers">An array of type <see langword="ParameterModifier" /> having the same length as the <paramref name="types" /> array, whose elements represent the attributes associated with the parameters of the method to get.</param>
    /// <returns>A <see langword="MethodInfoInfo" /> object for the implementation method that matches the specified criteria, or <see langword="null" /> if a match cannot be found.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
    protected override MethodInfo? GetMethodImpl(
      string name,
      BindingFlags bindingAttr,
      Binder? binder,
      CallingConventions callConvention,
      Type[]? types,
      ParameterModifier[]? modifiers)
    {
      return types == null ? this.typeImpl.GetMethod(name, bindingAttr) : this.typeImpl.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
    }

    /// <summary>Returns an array of <see cref="T:System.Reflection.MethodInfo" /> objects representing specified methods of the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <returns>An array of <see langword="MethodInfo" /> objects representing the methods defined on this <see langword="TypeDelegator" />.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
    public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => this.typeImpl.GetMethods(bindingAttr);

    /// <summary>Returns a <see cref="T:System.Reflection.FieldInfo" /> object representing the field with the specified name.</summary>
    /// <param name="name">The name of the field to find.</param>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <returns>A <see langword="FieldInfo" /> object representing the field declared or inherited by this <see langword="TypeDelegator" /> with the specified name. Returns <see langword="null" /> if no such field is found.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
    public override FieldInfo? GetField(string name, BindingFlags bindingAttr) => this.typeImpl.GetField(name, bindingAttr);

    /// <summary>Returns an array of <see cref="T:System.Reflection.FieldInfo" /> objects representing the data fields defined for the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <returns>An array of type <see langword="FieldInfo" /> containing the fields declared or inherited by the current <see langword="TypeDelegator" />. An empty array is returned if there are no matched fields.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
    public override FieldInfo[] GetFields(BindingFlags bindingAttr) => this.typeImpl.GetFields(bindingAttr);

    /// <summary>Returns the specified interface implemented by the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
    /// <param name="name">The fully qualified name of the interface implemented by the current class.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> if the case is to be ignored; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <returns>A <see langword="Type" /> object representing the interface implemented (directly or indirectly) by the current class with the fully qualified name matching the specified name. If no interface that matches name is found, null is returned.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
    public override Type? GetInterface(string name, bool ignoreCase) => this.typeImpl.GetInterface(name, ignoreCase);

    /// <summary>Returns all the interfaces implemented on the current class and its base classes.</summary>
    /// <returns>An array of type <see langword="Type" /> containing all the interfaces implemented on the current class and its base classes. If none are defined, an empty array is returned.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
    public override Type[] GetInterfaces() => this.typeImpl.GetInterfaces();

    /// <summary>Returns the specified event.</summary>
    /// <param name="name">The name of the event to get.</param>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <returns>An <see cref="T:System.Reflection.EventInfo" /> object representing the event declared or inherited by this type with the specified name. This method returns <see langword="null" /> if no such event is found.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
    public override EventInfo? GetEvent(string name, BindingFlags bindingAttr) => this.typeImpl.GetEvent(name, bindingAttr);

    /// <summary>Returns an array of <see cref="T:System.Reflection.EventInfo" /> objects representing all the public events declared or inherited by the current <see langword="TypeDelegator" />.</summary>
    /// <returns>An array that contains all the events declared or inherited by the current type. If there are no events, an empty array is returned.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)]
    public override EventInfo[] GetEvents() => this.typeImpl.GetEvents();

    /// <summary>When overridden in a derived class, searches for the specified property whose parameters match the specified argument types and modifiers, using the specified binding constraints.</summary>
    /// <param name="name">The property to get.</param>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see langword="MemberInfo" /> objects via reflection. If <paramref name="binder" /> is <see langword="null" />, the default binder is used. See <see cref="T:System.Reflection.Binder" />.</param>
    /// <param name="returnType">The return type of the property.</param>
    /// <param name="types">A list of parameter types. The list represents the number, order, and types of the parameters. Types cannot be null; use an appropriate <see langword="GetMethod" /> method or an empty array to search for a method without parameters.</param>
    /// <param name="modifiers">An array of the same length as types with elements that represent the attributes associated with the parameters of the method to get.</param>
    /// <returns>A <see cref="T:System.Reflection.PropertyInfo" /> object for the property that matches the specified criteria, or null if a match cannot be found.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
    protected override PropertyInfo? GetPropertyImpl(
      string name,
      BindingFlags bindingAttr,
      Binder? binder,
      Type? returnType,
      Type[]? types,
      ParameterModifier[]? modifiers)
    {
      return returnType == (Type) null && types == null ? this.typeImpl.GetProperty(name, bindingAttr) : this.typeImpl.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
    }

    /// <summary>Returns an array of <see cref="T:System.Reflection.PropertyInfo" /> objects representing properties of the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <returns>An array of <see langword="PropertyInfo" /> objects representing properties defined on this <see langword="TypeDelegator" />.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) => this.typeImpl.GetProperties(bindingAttr);

    /// <summary>Returns the events specified in <paramref name="bindingAttr" /> that are declared or inherited by the current <see langword="TypeDelegator" />.</summary>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <returns>An array of type <see langword="EventInfo" /> containing the events specified in <paramref name="bindingAttr" />. If there are no events, an empty array is returned.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
    public override EventInfo[] GetEvents(BindingFlags bindingAttr) => this.typeImpl.GetEvents(bindingAttr);

    /// <summary>Returns the nested types specified in <paramref name="bindingAttr" /> that are declared or inherited by the type wrapped by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <returns>An array of type <see langword="Type" /> containing the nested types.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
    public override Type[] GetNestedTypes(BindingFlags bindingAttr) => this.typeImpl.GetNestedTypes(bindingAttr);

    /// <summary>Returns a nested type specified by <paramref name="name" /> and in <paramref name="bindingAttr" /> that are declared or inherited by the type represented by the current <see cref="T:System.Reflection.TypeDelegator" />.</summary>
    /// <param name="name">The nested type's name.</param>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <returns>A <see langword="Type" /> object representing the nested type.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
    public override Type? GetNestedType(string name, BindingFlags bindingAttr) => this.typeImpl.GetNestedType(name, bindingAttr);

    /// <summary>Returns members (properties, methods, constructors, fields, events, and nested types) specified by the given <paramref name="name" />, <paramref name="type" />, and <paramref name="bindingAttr" />.</summary>
    /// <param name="name">The name of the member to get.</param>
    /// <param name="type">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <param name="bindingAttr">The type of members to get.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <returns>An array of type <see langword="MemberInfo" /> containing all the members of the current class and its base class meeting the specified criteria.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
    public override MemberInfo[] GetMember(
      string name,
      MemberTypes type,
      BindingFlags bindingAttr)
    {
      return this.typeImpl.GetMember(name, type, bindingAttr);
    }

    /// <summary>Returns members specified by <paramref name="bindingAttr" />.</summary>
    /// <param name="bindingAttr">A bitmask that affects the way in which the search is conducted. The value is a combination of zero or more bit flags from <see cref="T:System.Reflection.BindingFlags" />.</param>
    /// <returns>An array of type <see langword="MemberInfo" /> containing all the members of the current class and its base classes that meet the <paramref name="bindingAttr" /> filter.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
    public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => this.typeImpl.GetMembers(bindingAttr);

    public override MemberInfo GetMemberWithSameMetadataDefinitionAs(MemberInfo member) => this.typeImpl.GetMemberWithSameMetadataDefinitionAs(member);

    /// <summary>Gets the attributes assigned to the <see langword="TypeDelegator" />.</summary>
    /// <returns>A <see langword="TypeAttributes" /> object representing the implementation attribute flags.</returns>
    protected override TypeAttributes GetAttributeFlagsImpl() => this.typeImpl.Attributes;

    public override bool IsTypeDefinition => this.typeImpl.IsTypeDefinition;

    public override bool IsSZArray => this.typeImpl.IsSZArray;

    public override bool IsVariableBoundArray => this.typeImpl.IsVariableBoundArray;

    /// <summary>Returns a value that indicates whether the <see cref="T:System.Type" /> is an array.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Type" /> is an array; otherwise, <see langword="false" />.</returns>
    protected override bool IsArrayImpl() => this.typeImpl.IsArray;

    /// <summary>Returns a value that indicates whether the <see cref="T:System.Type" /> is one of the primitive types.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Type" /> is one of the primitive types; otherwise, <see langword="false" />.</returns>
    protected override bool IsPrimitiveImpl() => this.typeImpl.IsPrimitive;

    /// <summary>Returns a value that indicates whether the <see cref="T:System.Type" /> is passed by reference.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Type" /> is passed by reference; otherwise, <see langword="false" />.</returns>
    protected override bool IsByRefImpl() => this.typeImpl.IsByRef;

    public override bool IsGenericTypeParameter => this.typeImpl.IsGenericTypeParameter;

    public override bool IsGenericMethodParameter => this.typeImpl.IsGenericMethodParameter;

    /// <summary>Returns a value that indicates whether the <see cref="T:System.Type" /> is a pointer.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Type" /> is a pointer; otherwise, <see langword="false" />.</returns>
    protected override bool IsPointerImpl() => this.typeImpl.IsPointer;

    /// <summary>Returns a value that indicates whether the type is a value type; that is, not a class or an interface.</summary>
    /// <returns>
    /// <see langword="true" /> if the type is a value type; otherwise, <see langword="false" />.</returns>
    protected override bool IsValueTypeImpl() => this.typeImpl.IsValueType;

    /// <summary>Returns a value that indicates whether the <see cref="T:System.Type" /> is a COM object.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Type" /> is a COM object; otherwise, <see langword="false" />.</returns>
    protected override bool IsCOMObjectImpl() => this.typeImpl.IsCOMObject;

    public override bool IsByRefLike => this.typeImpl.IsByRefLike;

    /// <summary>Gets a value that indicates whether this object represents a constructed generic type.</summary>
    /// <returns>
    /// <see langword="true" /> if this object represents a constructed generic type; otherwise, <see langword="false" />.</returns>
    public override bool IsConstructedGenericType => this.typeImpl.IsConstructedGenericType;

    /// <summary>Gets the <see cref="P:System.Reflection.MemberInfo.IsCollectible" /> value for this object's <see cref="F:System.Reflection.TypeDelegator.typeImpl" />, which indicates whether this object, which is a <see cref="T:System.Reflection.MemberInfo" /> implementation, is part of an assembly held in a collectible <see cref="T:System.Runtime.Loader.AssemblyLoadContext" />.</summary>
    /// <returns>
    /// <see langword="true" /> if this object, which is a <see cref="T:System.Reflection.MemberInfo" /> implementation, is part of an assembly held in a collectible assembly load context; otherwise, <see langword="false" />.</returns>
    public override bool IsCollectible => this.typeImpl.IsCollectible;

    /// <summary>Returns the <see cref="T:System.Type" /> of the object encompassed or referred to by the current array, pointer or ByRef.</summary>
    /// <returns>The <see cref="T:System.Type" /> of the object encompassed or referred to by the current array, pointer or <see langword="ByRef" />, or <see langword="null" /> if the current <see cref="T:System.Type" /> is not an array, a pointer or a <see langword="ByRef" />.</returns>
    public override Type? GetElementType() => this.typeImpl.GetElementType();

    /// <summary>Gets a value indicating whether the current <see cref="T:System.Type" /> encompasses or refers to another type; that is, whether the current <see cref="T:System.Type" /> is an array, a pointer or a ByRef.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Type" /> is an array, a pointer or a ByRef; otherwise, <see langword="false" />.</returns>
    protected override bool HasElementTypeImpl() => this.typeImpl.HasElementType;

    /// <summary>Gets the underlying <see cref="T:System.Type" /> that represents the implemented type.</summary>
    /// <returns>The underlying type.</returns>
    public override Type UnderlyingSystemType => this.typeImpl.UnderlyingSystemType;

    /// <summary>Returns all the custom attributes defined for this type, specifying whether to search the type's inheritance chain.</summary>
    /// <param name="inherit">Specifies whether to search this type's inheritance chain to find the attributes.</param>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An array of objects containing all the custom attributes defined for this type.</returns>
    public override object[] GetCustomAttributes(bool inherit) => this.typeImpl.GetCustomAttributes(inherit);

    /// <summary>Returns an array of custom attributes identified by type.</summary>
    /// <param name="attributeType">An array of custom attributes identified by type.</param>
    /// <param name="inherit">Specifies whether to search this type's inheritance chain to find the attributes.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An array of objects containing the custom attributes defined in this type that match the <paramref name="attributeType" /> parameter, specifying whether to search the type's inheritance chain, or <see langword="null" /> if no custom attributes are defined on this type.</returns>
    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => this.typeImpl.GetCustomAttributes(attributeType, inherit);

    /// <summary>Indicates whether a custom attribute identified by <paramref name="attributeType" /> is defined.</summary>
    /// <param name="attributeType">Specifies whether to search this type's inheritance chain to find the attributes.</param>
    /// <param name="inherit">An array of custom attributes identified by type.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Reflection.ReflectionTypeLoadException">The custom attribute type cannot be loaded.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute identified by <paramref name="attributeType" /> is defined; otherwise, <see langword="false" />.</returns>
    public override bool IsDefined(Type attributeType, bool inherit) => this.typeImpl.IsDefined(attributeType, inherit);

    /// <summary>Returns an interface mapping for the specified interface type.</summary>
    /// <param name="interfaceType">The <see cref="T:System.Type" /> of the interface to retrieve a mapping of.</param>
    /// <returns>An <see cref="T:System.Reflection.InterfaceMapping" /> object representing the interface mapping for <paramref name="interfaceType" />.</returns>
    public override InterfaceMapping GetInterfaceMap([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type interfaceType) => this.typeImpl.GetInterfaceMap(interfaceType);
  }
}
