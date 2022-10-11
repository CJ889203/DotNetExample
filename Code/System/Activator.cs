// Decompiled with JetBrains decompiler
// Type: System.Activator
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Runtime.Remoting;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Contains methods to create types of objects locally or remotely, or obtain references to existing remote objects. This class cannot be inherited.</summary>
  public static class Activator
  {
    /// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="type" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
    /// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="type" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the parameterless constructor) is invoked.</param>
    /// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="type" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.
    /// 
    /// -or-
    /// 
    /// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.
    /// 
    /// -or-
    /// 
    /// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.
    /// 
    /// -or-
    /// 
    /// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.COMException">
    /// <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="type" /> is not a valid type.</exception>
    /// <returns>A reference to the newly created object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object? CreateInstance(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type,
      BindingFlags bindingAttr,
      Binder? binder,
      object?[]? args,
      CultureInfo? culture)
    {
      return Activator.CreateInstance(type, bindingAttr, binder, args, culture, (object[]) null);
    }

    /// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the parameterless constructor) is invoked.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.
    /// 
    /// -or-
    /// 
    /// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.
    /// 
    /// -or-
    /// 
    /// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.
    /// 
    /// -or-
    /// 
    /// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MissingMemberException" />, instead.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.COMException">
    /// <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="type" /> is not a valid type.</exception>
    /// <returns>A reference to the newly created object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, params object?[]? args) => Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, args, (CultureInfo) null, (object[]) null);

    /// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the parameterless constructor) is invoked.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.
    /// 
    /// -or-
    /// 
    /// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.
    /// 
    /// -or-
    /// 
    /// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.
    /// 
    /// -or-
    /// 
    /// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.COMException">
    /// <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="type" /> is not a valid type.</exception>
    /// <returns>A reference to the newly created object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, object?[]? args, object?[]? activationAttributes) => Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, args, (CultureInfo) null, activationAttributes);

    /// <summary>Creates an instance of the specified type using that type's parameterless constructor.</summary>
    /// <param name="type">The type of object to create.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.
    /// 
    /// -or-
    /// 
    /// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.
    /// 
    /// -or-
    /// 
    /// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MissingMemberException" />, instead.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.COMException">
    /// <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="type" /> is not a valid type.</exception>
    /// <returns>A reference to the newly created object.</returns>
    [DebuggerHidden]
    [DebuggerStepThrough]
    public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type) => Activator.CreateInstance(type, false);

    /// <summary>Creates an instance of the type whose name is specified, using the named assembly file and parameterless constructor.</summary>
    /// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
    /// <param name="typeName">The name of the type to create an instance of.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
    /// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public static ObjectHandle? CreateInstanceFrom(string assemblyFile, string typeName) => Activator.CreateInstanceFrom(assemblyFile, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null, (object[]) null);

    /// <summary>Creates an instance of the type whose name is specified, using the named assembly file and parameterless constructor.</summary>
    /// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
    /// <param name="typeName">The name of the type to create an instance of.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
    /// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public static ObjectHandle? CreateInstanceFrom(
      string assemblyFile,
      string typeName,
      object?[]? activationAttributes)
    {
      return Activator.CreateInstanceFrom(assemblyFile, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null, activationAttributes);
    }

    /// <summary>Creates an instance of the type whose name is specified, using the named assembly file and the constructor that best matches the specified parameters.</summary>
    /// <param name="assemblyFile">The name of a file that contains an assembly where the type named <paramref name="typeName" /> is sought.</param>
    /// <param name="typeName">The name of the type to create an instance of.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
    /// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
    /// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the parameterless constructor) is invoked.</param>
    /// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyFile" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyFile" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required <see cref="T:System.Security.Permissions.FileIOPermission" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyFile" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
    /// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public static ObjectHandle? CreateInstanceFrom(
      string assemblyFile,
      string typeName,
      bool ignoreCase,
      BindingFlags bindingAttr,
      Binder? binder,
      object?[]? args,
      CultureInfo? culture,
      object?[]? activationAttributes)
    {
      object instance = Activator.CreateInstance(Assembly.LoadFrom(assemblyFile).GetType(typeName, true, ignoreCase), bindingAttr, binder, args, culture, activationAttributes);
      return instance == null ? (ObjectHandle) null : new ObjectHandle(instance);
    }

    /// <summary>Creates an instance of the specified type using the constructor that best matches the specified parameters.</summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="type" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
    /// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="type" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the parameterless constructor) is invoked.</param>
    /// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="type" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.
    /// 
    /// -or-
    /// 
    /// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.
    /// 
    /// -or-
    /// 
    /// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.
    /// 
    /// -or-
    /// 
    /// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.COMException">
    /// <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="type" /> is not a valid type.</exception>
    /// <returns>A reference to the newly created object.</returns>
    public static object? CreateInstance(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type,
      BindingFlags bindingAttr,
      Binder? binder,
      object?[]? args,
      CultureInfo? culture,
      object?[]? activationAttributes)
    {
      if ((object) type == null)
        throw new ArgumentNullException(nameof (type));
      if (type is TypeBuilder)
        throw new NotSupportedException(SR.NotSupported_CreateInstanceWithTypeBuilder);
      if ((bindingAttr & (BindingFlags) 255) == BindingFlags.Default)
        bindingAttr |= BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
      if (activationAttributes != null && activationAttributes.Length != 0)
        throw new PlatformNotSupportedException(SR.NotSupported_ActivAttr);
      if (type.UnderlyingSystemType is RuntimeType underlyingSystemType)
        return underlyingSystemType.CreateInstanceImpl(bindingAttr, binder, args, culture);
      throw new ArgumentException(SR.Arg_MustBeType, nameof (type));
    }

    /// <summary>Creates an instance of the type whose name is specified, using the named assembly and parameterless constructor.</summary>
    /// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
    /// <param name="typeName">The fully qualified name of the type to create an instance of.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">You cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.
    /// 
    /// -or-
    /// 
    /// The assembly name or code base is invalid.</exception>
    /// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public static ObjectHandle? CreateInstance(string assemblyName, string typeName)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Activator.CreateInstanceInternal(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null, (object[]) null, ref stackMark);
    }

    /// <summary>Creates an instance of the type whose name is specified, using the named assembly and the constructor that best matches the specified parameters.</summary>
    /// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
    /// <param name="typeName">The fully qualified name of the type to create an instance of.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to specify that the search for <paramref name="typeName" /> is not case-sensitive; <see langword="false" /> to specify that the search is case-sensitive.</param>
    /// <param name="bindingAttr">A combination of zero or more bit flags that affect the search for the <paramref name="typeName" /> constructor. If <paramref name="bindingAttr" /> is zero, a case-sensitive search for public constructors is conducted.</param>
    /// <param name="binder">An object that uses <paramref name="bindingAttr" /> and <paramref name="args" /> to seek and identify the <paramref name="typeName" /> constructor. If <paramref name="binder" /> is <see langword="null" />, the default binder is used.</param>
    /// <param name="args">An array of arguments that match in number, order, and type the parameters of the constructor to invoke. If <paramref name="args" /> is an empty array or <see langword="null" />, the constructor that takes no parameters (the parameterless constructor) is invoked.</param>
    /// <param name="culture">Culture-specific information that governs the coercion of <paramref name="args" /> to the formal types declared for the <paramref name="typeName" /> constructor. If <paramref name="culture" /> is <see langword="null" />, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor, which was invoked through reflection, threw an exception.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.
    /// 
    /// -or-
    /// 
    /// The constructor that best matches <paramref name="args" /> has <see langword="varargs" /> arguments.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.
    /// 
    /// -or-
    /// 
    /// The assembly name or code base is invalid.</exception>
    /// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public static ObjectHandle? CreateInstance(
      string assemblyName,
      string typeName,
      bool ignoreCase,
      BindingFlags bindingAttr,
      Binder? binder,
      object?[]? args,
      CultureInfo? culture,
      object?[]? activationAttributes)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Activator.CreateInstanceInternal(assemblyName, typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes, ref stackMark);
    }

    /// <summary>Creates an instance of the type whose name is specified, using the named assembly and parameterless constructor.</summary>
    /// <param name="assemblyName">The name of the assembly where the type named <paramref name="typeName" /> is sought. If <paramref name="assemblyName" /> is <see langword="null" />, the executing assembly is searched.</param>
    /// <param name="typeName">The fully qualified name of the type to create an instance of.</param>
    /// <param name="activationAttributes">An array of one or more attributes that can participate in activation. This is typically an array that contains a single <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" /> object that specifies the URL that is required to activate a remote object.
    /// 
    /// This parameter is related to client-activated objects. Client activation is a legacy technology that is retained for backward compatibility but is not recommended for new development. Distributed applications should instead use Windows Communication Foundation.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="typeName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="typename" /> was not found in <paramref name="assemblyName" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">
    /// <paramref name="assemblyName" /> was not found.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.NotSupportedException">Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.
    /// 
    /// -or-
    /// 
    /// <paramref name="activationAttributes" /> is not an empty array, and the type being created does not derive from <see cref="T:System.MarshalByRefObject" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="activationAttributes" /> is not a <see cref="T:System.Runtime.Remoting.Activation.UrlAttribute" />
    /// 
    /// array.</exception>
    /// <exception cref="T:System.BadImageFormatException">
    ///        <paramref name="assemblyName" /> is not a valid assembly.
    /// 
    /// -or-
    /// 
    /// The common language runtime (CLR) version 2.0 or later is currently loaded, and <paramref name="assemblyName" /> was compiled for a version of the CLR that is later than the currently loaded version. Note that the .NET Framework versions 2.0, 3.0, and 3.5 all use CLR version 2.0.</exception>
    /// <exception cref="T:System.IO.FileLoadException">An assembly or module was loaded twice with two different evidences.
    /// 
    /// -or-
    /// 
    /// The assembly name or code base is invalid.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">An error occurred when attempting remote activation in a target specified in <paramref name="activationAttributes" />.</exception>
    /// <returns>A handle that must be unwrapped to access the newly created instance.</returns>
    [RequiresUnreferencedCode("Type and its constructor could be removed")]
    public static ObjectHandle? CreateInstance(
      string assemblyName,
      string typeName,
      object?[]? activationAttributes)
    {
      StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
      return Activator.CreateInstanceInternal(assemblyName, typeName, false, BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, (Binder) null, (object[]) null, (CultureInfo) null, activationAttributes, ref stackMark);
    }

    /// <summary>Creates an instance of the specified type using that type's parameterless constructor.</summary>
    /// <param name="type">The type of object to create.</param>
    /// <param name="nonPublic">
    /// <see langword="true" /> if a public or nonpublic parameterless constructor can match; <see langword="false" /> if only a public parameterless constructor can match.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="type" /> is not a <see langword="RuntimeType" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns <see langword="true" />).</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///        <paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.
    /// 
    /// -or-
    /// 
    /// Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.
    /// 
    /// -or-
    /// 
    /// The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />.</exception>
    /// <exception cref="T:System.Reflection.TargetInvocationException">The constructor being called throws an exception.</exception>
    /// <exception cref="T:System.MethodAccessException">The caller does not have permission to call this constructor.</exception>
    /// <exception cref="T:System.MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.InvalidComObjectException">The COM type was not obtained through <see cref="Overload:System.Type.GetTypeFromProgID" /> or <see cref="Overload:System.Type.GetTypeFromCLSID" />.</exception>
    /// <exception cref="T:System.MissingMethodException">No matching public constructor was found.</exception>
    /// <exception cref="T:System.Runtime.InteropServices.COMException">
    /// <paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered.</exception>
    /// <exception cref="T:System.TypeLoadException">
    /// <paramref name="type" /> is not a valid type.</exception>
    /// <returns>A reference to the newly created object.</returns>
    public static object? CreateInstance([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type type, bool nonPublic) => Activator.CreateInstance(type, nonPublic, true);


    #nullable disable
    internal static object CreateInstance(Type type, bool nonPublic, bool wrapExceptions)
    {
      if ((object) type == null)
        throw new ArgumentNullException(nameof (type));
      if (!(type.UnderlyingSystemType is RuntimeType underlyingSystemType))
        throw new ArgumentException(SR.Arg_MustBeType, nameof (type));
      return underlyingSystemType.CreateInstanceDefaultCtor(!nonPublic, wrapExceptions);
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Implementation detail of Activator that linker intrinsically recognizes")]
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072:UnrecognizedReflectionPattern", Justification = "Implementation detail of Activator that linker intrinsically recognizes")]
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2096:UnrecognizedReflectionPattern", Justification = "Implementation detail of Activator that linker intrinsically recognizes")]
    private static ObjectHandle CreateInstanceInternal(
      string assemblyString,
      string typeName,
      bool ignoreCase,
      BindingFlags bindingAttr,
      Binder binder,
      object[] args,
      CultureInfo culture,
      object[] activationAttributes,
      ref StackCrawlMark stackMark)
    {
      object instance = Activator.CreateInstance((assemblyString != null ? (Assembly) RuntimeAssembly.InternalLoad(new AssemblyName(assemblyString), ref stackMark, AssemblyLoadContext.CurrentContextualReflectionContext) : (Assembly) Assembly.GetExecutingAssembly(ref stackMark)).GetType(typeName, true, ignoreCase), bindingAttr, binder, args, culture, activationAttributes);
      return instance == null ? (ObjectHandle) null : new ObjectHandle(instance);
    }


    #nullable enable
    /// <summary>Creates an instance of the type designated by the specified generic type parameter, using the parameterless constructor.</summary>
    /// <typeparam name="T">The type to create.</typeparam>
    /// <exception cref="T:System.MissingMethodException">Cannot create an instance of an abstract class, or the type that is specified for <typeparamref name="T" /> does not have a parameterless constructor.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MissingMemberException" />, instead.</exception>
    /// <returns>A reference to the newly created object.</returns>
    [Intrinsic]
    public static T CreateInstance<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>() => (T) ((RuntimeType) typeof (T)).CreateInstanceOfT();


    #nullable disable
    private static T CreateDefaultInstance<T>() where T : struct => default (T);
  }
}
