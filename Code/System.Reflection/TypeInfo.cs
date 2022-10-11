// Decompiled with JetBrains decompiler
// Type: System.Reflection.TypeInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents type declarations for class types, interface types, array types, value types, enumeration types, type parameters, generic type definitions, and open or closed constructed generic types.</summary>
  public abstract class TypeInfo : Type, IReflectableType
  {

    #nullable disable
    /// <summary>Returns a representation of the current type as a <see cref="T:System.Reflection.TypeInfo" /> object.</summary>
    /// <returns>A reference to the current type.</returns>
    TypeInfo IReflectableType.GetTypeInfo() => this;


    #nullable enable
    /// <summary>Returns the current type as a <see cref="T:System.Type" /> object.</summary>
    /// <returns>The current type.</returns>
    public virtual Type AsType() => (Type) this;

    /// <summary>Gets an array of the generic type parameters of the current instance.</summary>
    /// <returns>An array that contains the current instance's generic type parameters, or an array of <see cref="P:System.Array.Length" /> zero if the current instance has no generic type parameters.</returns>
    public virtual Type[] GenericTypeParameters => !this.IsGenericTypeDefinition ? Type.EmptyTypes : this.GetGenericArguments();

    /// <summary>Returns an object that represents the specified event declared by the current type.</summary>
    /// <param name="name">The name of the event.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified event, if found; otherwise, <see langword="null" />.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)]
    public virtual EventInfo? GetDeclaredEvent(string name) => this.GetEvent(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    /// <summary>Returns an object that represents the specified field declared by the current type.</summary>
    /// <param name="name">The name of the field.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified field, if found; otherwise, <see langword="null" />.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
    public virtual FieldInfo? GetDeclaredField(string name) => this.GetField(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    /// <summary>Returns an object that represents the specified method declared by the current type.</summary>
    /// <param name="name">The name of the method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified method, if found; otherwise, <see langword="null" />.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
    public virtual MethodInfo? GetDeclaredMethod(string name) => this.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    /// <summary>Returns an object that represents the specified nested type declared by the current type.</summary>
    /// <param name="name">The name of the nested type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified nested type, if found; otherwise, <see langword="null" />.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)]
    public virtual TypeInfo? GetDeclaredNestedType(string name)
    {
      Type nestedType = this.GetNestedType(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      return (object) nestedType == null ? (TypeInfo) null : nestedType.GetTypeInfo();
    }

    /// <summary>Returns an object that represents the specified property declared by the current type.</summary>
    /// <param name="name">The name of the property.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified property, if found; otherwise, <see langword="null" />.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)]
    public virtual PropertyInfo? GetDeclaredProperty(string name) => this.GetProperty(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

    /// <summary>Returns a collection that contains all methods declared on the current type that match the specified name.</summary>
    /// <param name="name">The method name to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>A collection that contains methods that match <paramref name="name" />.</returns>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)]
    public virtual IEnumerable<MethodInfo> GetDeclaredMethods(string name)
    {
      MethodInfo[] methodInfoArray = GetDeclaredOnlyMethods((Type) this);
      for (int index = 0; index < methodInfoArray.Length; ++index)
      {
        MethodInfo methodInfo = methodInfoArray[index];
        if (methodInfo.Name == name)
          yield return methodInfo;
      }
      methodInfoArray = (MethodInfo[]) null;


      #nullable disable
      [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2070:UnrecognizedReflectionPattern", Justification = "The yield return state machine doesn't propagate annotations")]
      static MethodInfo[] GetDeclaredOnlyMethods(Type type) => type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }


    #nullable enable
    /// <summary>Gets a collection of the constructors declared by the current type.</summary>
    /// <returns>A collection of the constructors declared by the current type.</returns>
    public virtual IEnumerable<ConstructorInfo> DeclaredConstructors
    {
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] get => (IEnumerable<ConstructorInfo>) this.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>Gets a collection of the events defined by the current type.</summary>
    /// <returns>A collection of the events defined by the current type.</returns>
    public virtual IEnumerable<EventInfo> DeclaredEvents
    {
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)] get => (IEnumerable<EventInfo>) this.GetEvents(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>Gets a collection of the fields defined by the current type.</summary>
    /// <returns>A collection of the fields defined by the current type.</returns>
    public virtual IEnumerable<FieldInfo> DeclaredFields
    {
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] get => (IEnumerable<FieldInfo>) this.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>Gets a collection of the members defined by the current type.</summary>
    /// <returns>A collection of the members defined by the current type.</returns>
    public virtual IEnumerable<MemberInfo> DeclaredMembers
    {
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] get => (IEnumerable<MemberInfo>) this.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>Gets a collection of the methods defined by the current type.</summary>
    /// <returns>A collection of the methods defined by the current type.</returns>
    public virtual IEnumerable<MethodInfo> DeclaredMethods
    {
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] get => (IEnumerable<MethodInfo>) this.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>Gets a collection of the nested types defined by the current type.</summary>
    /// <returns>A collection of nested types defined by the current type.</returns>
    public virtual IEnumerable<TypeInfo> DeclaredNestedTypes
    {
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)] get
      {
        Type[] typeArray = GetDeclaredOnlyNestedTypes((Type) this);
        for (int index = 0; index < typeArray.Length; ++index)
          yield return typeArray[index].GetTypeInfo();
        typeArray = (Type[]) null;


        #nullable disable
        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2070:UnrecognizedReflectionPattern", Justification = "The yield return state machine doesn't propagate annotations")]
        static Type[] GetDeclaredOnlyNestedTypes(Type type) => type.GetNestedTypes(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      }
    }


    #nullable enable
    /// <summary>Gets a collection of the properties defined by the current type.</summary>
    /// <returns>A collection of the properties defined by the current type.</returns>
    public virtual IEnumerable<PropertyInfo> DeclaredProperties
    {
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] get => (IEnumerable<PropertyInfo>) this.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>Gets a collection of the interfaces implemented by the current type.</summary>
    /// <returns>A collection of the interfaces implemented by the current type.</returns>
    public virtual IEnumerable<Type> ImplementedInterfaces
    {
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] get => (IEnumerable<Type>) this.GetInterfaces();
    }

    /// <summary>Returns a value that indicates whether the specified type can be assigned to the current type.</summary>
    /// <param name="typeInfo">The type to check.</param>
    /// <returns>
    /// <see langword="true" /> if the specified type can be assigned to this type; otherwise, <see langword="false" />.</returns>
    public virtual bool IsAssignableFrom([NotNullWhen(true)] TypeInfo? typeInfo)
    {
      if ((Type) typeInfo == (Type) null)
        return false;
      if ((Type) this == (Type) typeInfo || typeInfo.IsSubclassOf((Type) this))
        return true;
      if (this.IsInterface)
        return typeInfo.ImplementInterface((Type) this);
      if (!this.IsGenericParameter)
        return false;
      foreach (Type parameterConstraint in this.GetGenericParameterConstraints())
      {
        if (!parameterConstraint.IsAssignableFrom((Type) typeInfo))
          return false;
      }
      return true;
    }


    #nullable disable
    internal static string GetRankString(int rank)
    {
      if (rank <= 0)
        throw new IndexOutOfRangeException();
      return rank != 1 ? "[" + new string(',', rank - 1) + "]" : "[*]";
    }
  }
}
