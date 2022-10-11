// Decompiled with JetBrains decompiler
// Type: System.Reflection.RuntimeReflectionExtensions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides methods that retrieve information about types at run time.</summary>
  public static class RuntimeReflectionExtensions
  {
    /// <summary>Retrieves a collection that represents all the fields defined on a specified type.</summary>
    /// <param name="type">The type that contains the fields.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <returns>A collection of fields for the specified type.</returns>
    public static IEnumerable<FieldInfo> GetRuntimeFields([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] this Type type) => !(type == (Type) null) ? (IEnumerable<FieldInfo>) type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : throw new ArgumentNullException(nameof (type));

    /// <summary>Retrieves a collection that represents all methods defined on a specified type.</summary>
    /// <param name="type">The type that contains the methods.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <returns>A collection of methods for the specified type.</returns>
    public static IEnumerable<MethodInfo> GetRuntimeMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] this Type type) => !(type == (Type) null) ? (IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : throw new ArgumentNullException(nameof (type));

    /// <summary>Retrieves a collection that represents all the properties defined on a specified type.</summary>
    /// <param name="type">The type that contains the properties.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <returns>A collection of properties for the specified type.</returns>
    public static IEnumerable<PropertyInfo> GetRuntimeProperties(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] this Type type)
    {
      return !(type == (Type) null) ? (IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : throw new ArgumentNullException(nameof (type));
    }

    /// <summary>Retrieves a collection that represents all the events defined on a specified type.</summary>
    /// <param name="type">The type that contains the events.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <returns>A collection of events for the specified type.</returns>
    public static IEnumerable<EventInfo> GetRuntimeEvents([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)] this Type type) => !(type == (Type) null) ? (IEnumerable<EventInfo>) type.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : throw new ArgumentNullException(nameof (type));

    /// <summary>Retrieves an object that represents a specified field.</summary>
    /// <param name="type">The type that contains the field.</param>
    /// <param name="name">The name of the field.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified field, or <see langword="null" /> if the field is not found.</returns>
    public static FieldInfo? GetRuntimeField([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] this Type type, string name) => !(type == (Type) null) ? type.GetField(name) : throw new ArgumentNullException(nameof (type));

    /// <summary>Retrieves an object that represents a specified method.</summary>
    /// <param name="type">The type that contains the method.</param>
    /// <param name="name">The name of the method.</param>
    /// <param name="parameters">An array that contains the method's parameters.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one method is found with the specified name.</exception>
    /// <returns>An object that represents the specified method, or <see langword="null" /> if the method is not found.</returns>
    public static MethodInfo? GetRuntimeMethod(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type type,
      string name,
      Type[] parameters)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      return type.GetMethod(name, parameters);
    }

    /// <summary>Retrieves an object that represents a specified property.</summary>
    /// <param name="type">The type that contains the property.</param>
    /// <param name="name">The name of the property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="type" /> is not a <see langword="RuntimeType" />.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one property with the requested name was found.</exception>
    /// <returns>An object that represents the specified property, or <see langword="null" /> if the property is not found.</returns>
    public static PropertyInfo? GetRuntimeProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type, string name) => !(type == (Type) null) ? type.GetProperty(name) : throw new ArgumentNullException(nameof (type));

    /// <summary>Retrieves an object that represents the specified event.</summary>
    /// <param name="type">The type that contains the event.</param>
    /// <param name="name">The name of the event.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="type" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified event, or <see langword="null" /> if the event is not found.</returns>
    public static EventInfo? GetRuntimeEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] this Type type, string name) => !(type == (Type) null) ? type.GetEvent(name) : throw new ArgumentNullException(nameof (type));

    /// <summary>Retrieves an object that represents the specified method on the direct or indirect base class where the method was first declared.</summary>
    /// <param name="method">The method to retrieve information about.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="method" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified method's initial declaration on a base class.</returns>
    public static MethodInfo? GetRuntimeBaseDefinition(this MethodInfo method) => !(method == (MethodInfo) null) ? method.GetBaseDefinition() : throw new ArgumentNullException(nameof (method));

    /// <summary>Returns an interface mapping for the specified type and the specified interface.</summary>
    /// <param name="typeInfo">The type to retrieve a mapping for.</param>
    /// <param name="interfaceType">The interface to retrieve a mapping for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///         <paramref name="typeInfo" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="interfaceType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="interfaceType" /> is not implemented by <paramref name="typeInfo" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="interfaceType" /> does not refer to an interface.
    /// 
    /// -or-
    /// 
    /// <paramref name="typeInfo" /> or <paramref name="interfaceType" /> is an open generic type.
    /// 
    /// -or-
    /// 
    /// <paramref name="interfaceType" /> is a generic interface, and <paramref name="typeInfo" /> is an array type.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="typeInfo" /> represents a generic type parameter.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///         <paramref name="typeInfo" /> is a <see cref="T:System.Reflection.Emit.TypeBuilder" /> instance whose <see cref="M:System.Reflection.Emit.TypeBuilder.CreateType" /> method has not yet been called.
    /// 
    /// -or-
    /// 
    /// The invoked method is not supported in the base class. Derived classes must provide an implementation.</exception>
    /// <returns>An object that represents the interface mapping for the specified interface and type.</returns>
    public static InterfaceMapping GetRuntimeInterfaceMap(
      this TypeInfo typeInfo,
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type interfaceType)
    {
      return !((Type) typeInfo == (Type) null) ? typeInfo.GetInterfaceMap(interfaceType) : throw new ArgumentNullException(nameof (typeInfo));
    }

    /// <summary>Gets an object that represents the method represented by the specified delegate.</summary>
    /// <param name="del">The delegate to examine.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="del" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.MemberAccessException">The caller does not have access to the method represented by the delegate (for example, if the method is private).</exception>
    /// <returns>An object that represents the method.</returns>
    public static MethodInfo GetMethodInfo(this Delegate del) => (object) del != null ? del.Method : throw new ArgumentNullException(nameof (del));
  }
}
