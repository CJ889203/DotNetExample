// Decompiled with JetBrains decompiler
// Type: System.Reflection.Module
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>Performs reflection on a module.</summary>
  public abstract class Module : ICustomAttributeProvider, ISerializable
  {
    /// <summary>A <see langword="TypeFilter" /> object that filters the list of types defined in this module based upon the name. This field is case-sensitive and read-only.</summary>
    public static readonly TypeFilter FilterTypeName = (TypeFilter) ((m, c) => Module.FilterTypeNameImpl(m, c, StringComparison.Ordinal));
    /// <summary>A <see langword="TypeFilter" /> object that filters the list of types defined in this module based upon the name. This field is case-insensitive and read-only.</summary>
    public static readonly TypeFilter FilterTypeNameIgnoreCase = (TypeFilter) ((m, c) => Module.FilterTypeNameImpl(m, c, StringComparison.OrdinalIgnoreCase));

    /// <summary>Gets the appropriate <see cref="T:System.Reflection.Assembly" /> for this instance of <see cref="T:System.Reflection.Module" />.</summary>
    /// <returns>An <see langword="Assembly" /> object.</returns>
    public virtual Assembly Assembly => throw NotImplemented.ByDesign;

    /// <summary>Gets a string representing the fully qualified name and path to this module.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions.</exception>
    /// <returns>The fully qualified module name.</returns>
    [RequiresAssemblyFiles("Returns <Unknown> for modules with no file path")]
    public virtual string FullyQualifiedName => throw NotImplemented.ByDesign;

    /// <summary>Gets a <see langword="String" /> representing the name of the module with the path removed.</summary>
    /// <returns>The module name with no path.</returns>
    [RequiresAssemblyFiles("Returns <Unknown> for modules with no file path")]
    public virtual string Name => throw NotImplemented.ByDesign;

    /// <summary>Gets the metadata stream version.</summary>
    /// <returns>A 32-bit integer representing the metadata stream version. The high-order two bytes represent the major version number, and the low-order two bytes represent the minor version number.</returns>
    public virtual int MDStreamVersion => throw NotImplemented.ByDesign;

    /// <summary>Gets a universally unique identifier (UUID) that can be used to distinguish between two versions of a module.</summary>
    /// <returns>A <see cref="T:System.Guid" /> that can be used to distinguish between two versions of a module.</returns>
    public virtual Guid ModuleVersionId => throw NotImplemented.ByDesign;

    /// <summary>Gets a string representing the name of the module.</summary>
    /// <returns>The module name.</returns>
    public virtual string ScopeName => throw NotImplemented.ByDesign;

    /// <summary>Gets a handle for the module.</summary>
    /// <returns>A <see cref="T:System.ModuleHandle" /> structure for the current module.</returns>
    public ModuleHandle ModuleHandle => this.GetModuleHandleImpl();

    protected virtual ModuleHandle GetModuleHandleImpl() => ModuleHandle.EmptyHandle;

    /// <summary>Gets a pair of values indicating the nature of the code in a module and the platform targeted by the module.</summary>
    /// <param name="peKind">When this method returns, a combination of the <see cref="T:System.Reflection.PortableExecutableKinds" /> values indicating the nature of the code in the module.</param>
    /// <param name="machine">When this method returns, one of the <see cref="T:System.Reflection.ImageFileMachine" /> values indicating the platform targeted by the module.</param>
    public virtual void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine) => throw NotImplemented.ByDesign;

    /// <summary>Gets a value indicating whether the object is a resource.</summary>
    /// <returns>
    /// <see langword="true" /> if the object is a resource; otherwise, <see langword="false" />.</returns>
    public virtual bool IsResource() => throw NotImplemented.ByDesign;

    /// <summary>Returns a value that indicates whether the specified attribute type has been applied to this module.</summary>
    /// <param name="attributeType">The type of custom attribute to test for.</param>
    /// <param name="inherit">This argument is ignored for objects of this type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not a <see cref="T:System.Type" /> object supplied by the runtime. For example, <paramref name="attributeType" /> is a <see cref="T:System.Reflection.Emit.TypeBuilder" /> object.</exception>
    /// <returns>
    /// <see langword="true" /> if one or more instances of <paramref name="attributeType" /> have been applied to this module; otherwise, <see langword="false" />.</returns>
    public virtual bool IsDefined(Type attributeType, bool inherit) => throw NotImplemented.ByDesign;

    /// <summary>Gets a collection that contains this module's custom attributes.</summary>
    /// <returns>A collection that contains this module's custom attributes.</returns>
    public virtual IEnumerable<CustomAttributeData> CustomAttributes => (IEnumerable<CustomAttributeData>) this.GetCustomAttributesData();

    /// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects for the current module, which can be used in the reflection-only context.</summary>
    /// <returns>A generic list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the current module.</returns>
    public virtual IList<CustomAttributeData> GetCustomAttributesData() => throw NotImplemented.ByDesign;

    /// <summary>Returns all custom attributes.</summary>
    /// <param name="inherit">This argument is ignored for objects of this type.</param>
    /// <returns>An array of type <see langword="Object" /> containing all custom attributes.</returns>
    public virtual object[] GetCustomAttributes(bool inherit) => throw NotImplemented.ByDesign;

    /// <summary>Gets custom attributes of the specified type.</summary>
    /// <param name="attributeType">The type of attribute to get.</param>
    /// <param name="inherit">This argument is ignored for objects of this type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not a <see cref="T:System.Type" /> object supplied by the runtime. For example, <paramref name="attributeType" /> is a <see cref="T:System.Reflection.Emit.TypeBuilder" /> object.</exception>
    /// <returns>An array of type <see langword="Object" /> containing all custom attributes of the specified type.</returns>
    public virtual object[] GetCustomAttributes(Type attributeType, bool inherit) => throw NotImplemented.ByDesign;

    /// <summary>Returns a method having the specified name.</summary>
    /// <param name="name">The method name.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>A <see langword="MethodInfo" /> object having the specified name, or <see langword="null" /> if the method does not exist.</returns>
    [RequiresUnreferencedCode("Methods might be removed")]
    public MethodInfo? GetMethod(string name) => name != null ? this.GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, (Binder) null, CallingConventions.Any, (Type[]) null, (ParameterModifier[]) null) : throw new ArgumentNullException(nameof (name));

    /// <summary>Returns a method having the specified name and parameter types.</summary>
    /// <param name="name">The method name.</param>
    /// <param name="types">The parameter types to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />, <paramref name="types" /> is <see langword="null" />, or <paramref name="types" /> (i) is <see langword="null" />.</exception>
    /// <returns>A <see langword="MethodInfo" /> object in accordance with the specified criteria, or <see langword="null" /> if the method does not exist.</returns>
    [RequiresUnreferencedCode("Methods might be removed")]
    public MethodInfo? GetMethod(string name, Type[] types) => this.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, (Binder) null, CallingConventions.Any, types, (ParameterModifier[]) null);

    /// <summary>Returns a method having the specified name, binding information, calling convention, and parameter types and modifiers.</summary>
    /// <param name="name">The method name.</param>
    /// <param name="bindingAttr">One of the <see langword="BindingFlags" /> bit flags used to control the search.</param>
    /// <param name="binder">An object that implements <see langword="Binder" />, containing properties related to this method.</param>
    /// <param name="callConvention">The calling convention for the method.</param>
    /// <param name="types">The parameter types to search for.</param>
    /// <param name="modifiers">An array of parameter modifiers used to make binding work with parameter signatures in which the types have been modified.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />, <paramref name="types" /> is <see langword="null" />, or <paramref name="types" /> (i) is <see langword="null" />.</exception>
    /// <returns>A <see langword="MethodInfo" /> object in accordance with the specified criteria, or <see langword="null" /> if the method does not exist.</returns>
    [RequiresUnreferencedCode("Methods might be removed")]
    public MethodInfo? GetMethod(
      string name,
      BindingFlags bindingAttr,
      Binder? binder,
      CallingConventions callConvention,
      Type[] types,
      ParameterModifier[]? modifiers)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (types == null)
        throw new ArgumentNullException(nameof (types));
      for (int index = 0; index < types.Length; ++index)
      {
        if (types[index] == (Type) null)
          throw new ArgumentNullException(nameof (types));
      }
      return this.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
    }

    /// <summary>Returns the method implementation in accordance with the specified criteria.</summary>
    /// <param name="name">The method name.</param>
    /// <param name="bindingAttr">One of the <see langword="BindingFlags" /> bit flags used to control the search.</param>
    /// <param name="binder">An object that implements <see langword="Binder" />, containing properties related to this method.</param>
    /// <param name="callConvention">The calling convention for the method.</param>
    /// <param name="types">The parameter types to search for.</param>
    /// <param name="modifiers">An array of parameter modifiers used to make binding work with parameter signatures in which the types have been modified.</param>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">
    /// <paramref name="types" /> is <see langword="null" />.</exception>
    /// <returns>A <see langword="MethodInfo" /> object containing implementation information as specified, or <see langword="null" /> if the method does not exist.</returns>
    [RequiresUnreferencedCode("Methods might be removed")]
    protected virtual MethodInfo? GetMethodImpl(
      string name,
      BindingFlags bindingAttr,
      Binder? binder,
      CallingConventions callConvention,
      Type[]? types,
      ParameterModifier[]? modifiers)
    {
      throw NotImplemented.ByDesign;
    }

    /// <summary>Returns the global methods defined on the module.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.MethodInfo" /> objects representing all the global methods defined on the module; if there are no global methods, an empty array is returned.</returns>
    [RequiresUnreferencedCode("Methods might be removed")]
    public MethodInfo[] GetMethods() => this.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

    /// <summary>Returns the global methods defined on the module that match the specified binding flags.</summary>
    /// <param name="bindingFlags">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values that limit the search.</param>
    /// <returns>An array of type <see cref="T:System.Reflection.MethodInfo" /> representing the global methods defined on the module that match the specified binding flags; if no global methods match the binding flags, an empty array is returned.</returns>
    [RequiresUnreferencedCode("Methods might be removed")]
    public virtual MethodInfo[] GetMethods(BindingFlags bindingFlags) => throw NotImplemented.ByDesign;

    /// <summary>Returns a field having the specified name.</summary>
    /// <param name="name">The field name.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <returns>A <see langword="FieldInfo" /> object having the specified name, or <see langword="null" /> if the field does not exist.</returns>
    [RequiresUnreferencedCode("Fields might be removed")]
    public FieldInfo? GetField(string name) => this.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

    /// <summary>Returns a field having the specified name and binding attributes.</summary>
    /// <param name="name">The field name.</param>
    /// <param name="bindingAttr">One of the <see langword="BindingFlags" /> bit flags used to control the search.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
    /// <returns>A <see langword="FieldInfo" /> object having the specified name and binding attributes, or <see langword="null" /> if the field does not exist.</returns>
    [RequiresUnreferencedCode("Fields might be removed")]
    public virtual FieldInfo? GetField(string name, BindingFlags bindingAttr) => throw NotImplemented.ByDesign;

    /// <summary>Returns the global fields defined on the module.</summary>
    /// <returns>An array of <see cref="T:System.Reflection.FieldInfo" /> objects representing the global fields defined on the module; if there are no global fields, an empty array is returned.</returns>
    [RequiresUnreferencedCode("Fields might be removed")]
    public FieldInfo[] GetFields() => this.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

    /// <summary>Returns the global fields defined on the module that match the specified binding flags.</summary>
    /// <param name="bindingFlags">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values that limit the search.</param>
    /// <returns>An array of type <see cref="T:System.Reflection.FieldInfo" /> representing the global fields defined on the module that match the specified binding flags; if no global fields match the binding flags, an empty array is returned.</returns>
    [RequiresUnreferencedCode("Fields might be removed")]
    public virtual FieldInfo[] GetFields(BindingFlags bindingFlags) => throw NotImplemented.ByDesign;

    /// <summary>Returns all the types defined within this module.</summary>
    /// <exception cref="T:System.Reflection.ReflectionTypeLoadException">One or more classes in a module could not be loaded.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>An array of type <see langword="Type" /> containing types defined within the module that is reflected by this instance.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type[] GetTypes() => throw NotImplemented.ByDesign;

    /// <summary>Returns the specified type, performing a case-sensitive search.</summary>
    /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="className" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The class initializers are invoked and an exception is thrown.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="className" /> is a zero-length string.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="className" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///        <paramref name="className" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context, and <paramref name="className" /> requires a dependent assembly that was not preloaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="className" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="className" /> requires a dependent assembly which was compiled for a version of the runtime later than the currently loaded version.</exception>
    /// <returns>A <see langword="Type" /> object representing the given type, if the type is in this module; otherwise, <see langword="null" />.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type? GetType(string className) => this.GetType(className, false, false);

    /// <summary>Returns the specified type, searching the module with the specified case sensitivity.</summary>
    /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> for case-insensitive search; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="className" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The class initializers are invoked and an exception is thrown.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="className" /> is a zero-length string.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="className" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///        <paramref name="className" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context, and <paramref name="className" /> requires a dependent assembly that was not preloaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="className" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="className" /> requires a dependent assembly which was compiled for a version of the runtime later than the currently loaded version.</exception>
    /// <returns>A <see langword="Type" /> object representing the given type, if the type is in this module; otherwise, <see langword="null" />.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type? GetType(string className, bool ignoreCase) => this.GetType(className, false, ignoreCase);

    /// <summary>Returns the specified type, specifying whether to make a case-sensitive search of the module and whether to throw an exception if the type cannot be found.</summary>
    /// <param name="className">The name of the type to locate. The name must be fully qualified with the namespace.</param>
    /// <param name="throwOnError">
    /// <see langword="true" /> to throw an exception if the type cannot be found; <see langword="false" /> to return <see langword="null" />.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> for case-insensitive search; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="className" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The class initializers are invoked and an exception is thrown.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="className" /> is a zero-length string.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="throwOnError" /> is <see langword="true" />, and the type cannot be found.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="className" /> requires a dependent assembly that could not be found.</exception>
    /// <exception cref="T:System.IO.FileLoadException">
    ///        <paramref name="className" /> requires a dependent assembly that was found but could not be loaded.
    /// 
    /// -or-
    /// 
    /// The current assembly was loaded into the reflection-only context, and <paramref name="className" /> requires a dependent assembly that was not preloaded.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="className" /> requires a dependent assembly, but the file is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// <paramref name="className" /> requires a dependent assembly which was compiled for a version of the runtime later than the currently loaded version.</exception>
    /// <returns>A <see cref="T:System.Type" /> object representing the specified type, if the type is declared in this module; otherwise, <see langword="null" />.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type? GetType(string className, bool throwOnError, bool ignoreCase) => throw NotImplemented.ByDesign;

    /// <summary>Returns an array of classes accepted by the given filter and filter criteria.</summary>
    /// <param name="filter">The delegate used to filter the classes.</param>
    /// <param name="filterCriteria">An Object used to filter the classes.</param>
    /// <exception cref="T:System.Reflection.ReflectionTypeLoadException">One or more classes in a module could not be loaded.</exception>
    /// <returns>An array of type <see langword="Type" /> containing classes that were accepted by the filter.</returns>
    [RequiresUnreferencedCode("Types might be removed")]
    public virtual Type[] FindTypes(TypeFilter? filter, object? filterCriteria)
    {
      Type[] types1 = this.GetTypes();
      int length = 0;
      for (int index = 0; index < types1.Length; ++index)
      {
        if (filter != null && !filter(types1[index], filterCriteria))
          types1[index] = (Type) null;
        else
          ++length;
      }
      if (length == types1.Length)
        return types1;
      Type[] types2 = new Type[length];
      int num = 0;
      for (int index = 0; index < types1.Length; ++index)
      {
        if (types1[index] != (Type) null)
          types2[num++] = types1[index];
      }
      return types2;
    }

    /// <summary>Gets a token that identifies the module in metadata.</summary>
    /// <returns>An integer token that identifies the current module in metadata.</returns>
    public virtual int MetadataToken => throw NotImplemented.ByDesign;

    /// <summary>Returns the field identified by the specified metadata token.</summary>
    /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="metadataToken" /> is not a token for a field in the scope of the current module.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> identifies a field whose parent <see langword="TypeSpec" /> has a signature containing element type <see langword="var" /> (a type parameter of a generic type) or <see langword="mvar" /> (a type parameter of a generic method).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.Reflection.FieldInfo" /> object representing the field that is identified by the specified metadata token.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public FieldInfo? ResolveField(int metadataToken) => this.ResolveField(metadataToken, (Type[]) null, (Type[]) null);

    /// <summary>Returns the field identified by the specified metadata token, in the context defined by the specified generic type parameters.</summary>
    /// <param name="metadataToken">A metadata token that identifies a field in the module.</param>
    /// <param name="genericTypeArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null" /> if that type is not generic.</param>
    /// <param name="genericMethodArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the method where the token is in scope, or <see langword="null" /> if that method is not generic.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="metadataToken" /> is not a token for a field in the scope of the current module.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> identifies a field whose parent <see langword="TypeSpec" /> has a signature containing element type <see langword="var" /> (a type parameter of a generic type) or <see langword="mvar" /> (a type parameter of a generic method), and the necessary generic type arguments were not supplied for either or both of <paramref name="genericTypeArguments" /> and <paramref name="genericMethodArguments" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.Reflection.FieldInfo" /> object representing the field that is identified by the specified metadata token.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public virtual FieldInfo? ResolveField(
      int metadataToken,
      Type[]? genericTypeArguments,
      Type[]? genericMethodArguments)
    {
      throw NotImplemented.ByDesign;
    }

    /// <summary>Returns the type or member identified by the specified metadata token.</summary>
    /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="metadataToken" /> is not a token for a type or member in the scope of the current module.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> is a <see langword="MethodSpec" /> or <see langword="TypeSpec" /> whose signature contains element type <see langword="var" /> (a type parameter of a generic type) or <see langword="mvar" /> (a type parameter of a generic method).
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> identifies a property or event.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.Reflection.MemberInfo" /> object representing the type or member that is identified by the specified metadata token.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public MemberInfo? ResolveMember(int metadataToken) => this.ResolveMember(metadataToken, (Type[]) null, (Type[]) null);

    /// <summary>Returns the type or member identified by the specified metadata token, in the context defined by the specified generic type parameters.</summary>
    /// <param name="metadataToken">A metadata token that identifies a type or member in the module.</param>
    /// <param name="genericTypeArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null" /> if that type is not generic.</param>
    /// <param name="genericMethodArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the method where the token is in scope, or <see langword="null" /> if that method is not generic.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="metadataToken" /> is not a token for a type or member in the scope of the current module.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> is a <see langword="MethodSpec" /> or <see langword="TypeSpec" /> whose signature contains element type <see langword="var" /> (a type parameter of a generic type) or <see langword="mvar" /> (a type parameter of a generic method), and the necessary generic type arguments were not supplied for either or both of <paramref name="genericTypeArguments" /> and <paramref name="genericMethodArguments" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> identifies a property or event.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.Reflection.MemberInfo" /> object representing the type or member that is identified by the specified metadata token.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public virtual MemberInfo? ResolveMember(
      int metadataToken,
      Type[]? genericTypeArguments,
      Type[]? genericMethodArguments)
    {
      throw NotImplemented.ByDesign;
    }

    /// <summary>Returns the method or constructor identified by the specified metadata token.</summary>
    /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="metadataToken" /> is not a token for a method or constructor in the scope of the current module.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> is a <see langword="MethodSpec" /> whose signature contains element type <see langword="var" /> (a type parameter of a generic type) or <see langword="mvar" /> (a type parameter of a generic method).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodBase" /> object representing the method or constructor that is identified by the specified metadata token.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public MethodBase? ResolveMethod(int metadataToken) => this.ResolveMethod(metadataToken, (Type[]) null, (Type[]) null);

    /// <summary>Returns the method or constructor identified by the specified metadata token, in the context defined by the specified generic type parameters.</summary>
    /// <param name="metadataToken">A metadata token that identifies a method or constructor in the module.</param>
    /// <param name="genericTypeArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null" /> if that type is not generic.</param>
    /// <param name="genericMethodArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the method where the token is in scope, or <see langword="null" /> if that method is not generic.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="metadataToken" /> is not a token for a method or constructor in the scope of the current module.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> is a <see langword="MethodSpec" /> whose signature contains element type <see langword="var" /> (a type parameter of a generic type) or <see langword="mvar" /> (a type parameter of a generic method), and the necessary generic type arguments were not supplied for either or both of <paramref name="genericTypeArguments" /> and <paramref name="genericMethodArguments" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.Reflection.MethodBase" /> object representing the method that is identified by the specified metadata token.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public virtual MethodBase? ResolveMethod(
      int metadataToken,
      Type[]? genericTypeArguments,
      Type[]? genericMethodArguments)
    {
      throw NotImplemented.ByDesign;
    }

    /// <summary>Returns the signature blob identified by a metadata token.</summary>
    /// <param name="metadataToken">A metadata token that identifies a signature in the module.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="metadataToken" /> is not a valid <see langword="MemberRef" />, <see langword="MethodDef" />, <see langword="TypeSpec" />, signature, or <see langword="FieldDef" /> token in the scope of the current module.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>An array of bytes representing the signature blob.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public virtual byte[] ResolveSignature(int metadataToken) => throw NotImplemented.ByDesign;

    /// <summary>Returns the string identified by the specified metadata token.</summary>
    /// <param name="metadataToken">A metadata token that identifies a string in the string heap of the module.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="metadataToken" /> is not a token for a string in the scope of the current module.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.String" /> containing a string value from the metadata string heap.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public virtual string ResolveString(int metadataToken) => throw NotImplemented.ByDesign;

    /// <summary>Returns the type identified by the specified metadata token.</summary>
    /// <param name="metadataToken">A metadata token that identifies a type in the module.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="metadataToken" /> is not a token for a type in the scope of the current module.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> is a <see langword="TypeSpec" /> whose signature contains element type <see langword="var" /> (a type parameter of a generic type) or <see langword="mvar" /> (a type parameter of a generic method).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.Type" /> object representing the type that is identified by the specified metadata token.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public Type ResolveType(int metadataToken) => this.ResolveType(metadataToken, (Type[]) null, (Type[]) null);

    /// <summary>Returns the type identified by the specified metadata token, in the context defined by the specified generic type parameters.</summary>
    /// <param name="metadataToken">A metadata token that identifies a type in the module.</param>
    /// <param name="genericTypeArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the type where the token is in scope, or <see langword="null" /> if that type is not generic.</param>
    /// <param name="genericMethodArguments">An array of <see cref="T:System.Type" /> objects representing the generic type arguments of the method where the token is in scope, or <see langword="null" /> if that method is not generic.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="metadataToken" /> is not a token for a type in the scope of the current module.
    /// 
    /// -or-
    /// 
    /// <paramref name="metadataToken" /> is a <see langword="TypeSpec" /> whose signature contains element type <see langword="var" /> (a type parameter of a generic type) or <see langword="mvar" /> (a type parameter of a generic method), and the necessary generic type arguments were not supplied for either or both of <paramref name="genericTypeArguments" /> and <paramref name="genericMethodArguments" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="metadataToken" /> is not a valid token in the scope of the current module.</exception>
    /// <returns>A <see cref="T:System.Type" /> object representing the type that is identified by the specified metadata token.</returns>
    [RequiresUnreferencedCode("Trimming changes metadata tokens")]
    public virtual Type ResolveType(
      int metadataToken,
      Type[]? genericTypeArguments,
      Type[]? genericMethodArguments)
    {
      throw NotImplemented.ByDesign;
    }

    /// <summary>Provides an <see cref="T:System.Runtime.Serialization.ISerializable" /> implementation for serialized objects.</summary>
    /// <param name="info">The information and data needed to serialize or deserialize an object.</param>
    /// <param name="context">The context for the serialization.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    public virtual void GetObjectData(SerializationInfo info, StreamingContext context) => throw NotImplemented.ByDesign;

    /// <summary>Determines whether this module and the specified object are equal.</summary>
    /// <param name="o">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="o" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? o) => base.Equals(o);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Indicates whether two <see cref="T:System.Reflection.Module" /> objects are equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Module? left, Module? right)
    {
      if ((object) right == null)
        return (object) left == null;
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>Indicates whether two <see cref="T:System.Reflection.Module" /> objects are not equal.</summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Module? left, Module? right) => !(left == right);

    /// <summary>Returns the name of the module.</summary>
    /// <returns>A <see langword="String" /> representing the name of this module.</returns>
    public override string ToString() => this.ScopeName;


    #nullable disable
    private static bool FilterTypeNameImpl(
      Type cls,
      object filterCriteria,
      StringComparison comparison)
    {
      if (!(filterCriteria is string text))
        throw new InvalidFilterCriteriaException(SR.InvalidFilterCriteriaException_CritString);
      if (text.Length > 0)
      {
        string str = text;
        if (str[str.Length - 1] == '*')
        {
          ReadOnlySpan<char> readOnlySpan = text.AsSpan(0, text.Length - 1);
          return cls.Name.AsSpan().StartsWith(readOnlySpan, comparison);
        }
      }
      return cls.Name.Equals(text, comparison);
    }
  }
}
