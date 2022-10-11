// Decompiled with JetBrains decompiler
// Type: System.Reflection.TypeExtensions
// Assembly: System.Reflection.TypeExtensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 640AC10B-88E0-451A-B5D0-A4B0F7E22777
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.TypeExtensions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.TypeExtensions.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Reflection
{
  public static class TypeExtensions
  {
    /// <param name="type" />
    /// <param name="types" />
    public static ConstructorInfo? GetConstructor([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] this Type type, Type[] types)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetConstructor(types);
    }

    /// <param name="type" />
    public static ConstructorInfo[] GetConstructors([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetConstructors();
    }

    /// <param name="type" />
    /// <param name="bindingAttr" />
    public static ConstructorInfo[] GetConstructors(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] this Type type,
      BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetConstructors(bindingAttr);
    }

    /// <param name="type" />
    public static MemberInfo[] GetDefaultMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)] this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetDefaultMembers();
    }

    /// <param name="type" />
    /// <param name="name" />
    public static EventInfo? GetEvent([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] this Type type, string name)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetEvent(name);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="bindingAttr" />
    public static EventInfo? GetEvent(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)] this Type type,
      string name,
      BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetEvent(name, bindingAttr);
    }

    /// <param name="type" />
    public static EventInfo[] GetEvents([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents)] this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetEvents();
    }

    /// <param name="type" />
    /// <param name="bindingAttr" />
    public static EventInfo[] GetEvents([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicEvents | DynamicallyAccessedMemberTypes.NonPublicEvents)] this Type type, BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetEvents(bindingAttr);
    }

    /// <param name="type" />
    /// <param name="name" />
    public static FieldInfo? GetField([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] this Type type, string name)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetField(name);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="bindingAttr" />
    public static FieldInfo? GetField(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] this Type type,
      string name,
      BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetField(name, bindingAttr);
    }

    /// <param name="type" />
    public static FieldInfo[] GetFields([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetFields();
    }

    /// <param name="type" />
    /// <param name="bindingAttr" />
    public static FieldInfo[] GetFields([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)] this Type type, BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetFields(bindingAttr);
    }

    /// <param name="type" />
    public static Type[] GetGenericArguments(this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetGenericArguments();
    }

    /// <param name="type" />
    public static Type[] GetInterfaces([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetInterfaces();
    }

    /// <param name="type" />
    /// <param name="name" />
    public static MemberInfo[] GetMember([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)] this Type type, string name)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMember(name);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="bindingAttr" />
    public static MemberInfo[] GetMember(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] this Type type,
      string name,
      BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMember(name, bindingAttr);
    }

    /// <param name="type" />
    public static MemberInfo[] GetMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicEvents)] this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMembers();
    }

    /// <param name="type" />
    /// <param name="bindingAttr" />
    public static MemberInfo[] GetMembers([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] this Type type, BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMembers(bindingAttr);
    }

    /// <param name="type" />
    /// <param name="name" />
    public static MethodInfo? GetMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type type, string name)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMethod(name);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="bindingAttr" />
    public static MethodInfo? GetMethod(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] this Type type,
      string name,
      BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMethod(name, bindingAttr);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="types" />
    public static MethodInfo? GetMethod([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type type, string name, Type[] types)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMethod(name, types);
    }

    /// <param name="type" />
    public static MethodInfo[] GetMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMethods();
    }

    /// <param name="type" />
    /// <param name="bindingAttr" />
    public static MethodInfo[] GetMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] this Type type, BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetMethods(bindingAttr);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="bindingAttr" />
    public static Type? GetNestedType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)] this Type type, string name, BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetNestedType(name, bindingAttr);
    }

    /// <param name="type" />
    /// <param name="bindingAttr" />
    public static Type[] GetNestedTypes([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicNestedTypes | DynamicallyAccessedMemberTypes.NonPublicNestedTypes)] this Type type, BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetNestedTypes(bindingAttr);
    }

    /// <param name="type" />
    public static PropertyInfo[] GetProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetProperties();
    }

    /// <param name="type" />
    /// <param name="bindingAttr" />
    public static PropertyInfo[] GetProperties([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] this Type type, BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetProperties(bindingAttr);
    }

    /// <param name="type" />
    /// <param name="name" />
    public static PropertyInfo? GetProperty([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type, string name)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetProperty(name);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="bindingAttr" />
    public static PropertyInfo? GetProperty(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties)] this Type type,
      string name,
      BindingFlags bindingAttr)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetProperty(name, bindingAttr);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="returnType" />
    public static PropertyInfo? GetProperty(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type,
      string name,
      Type? returnType)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetProperty(name, returnType);
    }

    /// <param name="type" />
    /// <param name="name" />
    /// <param name="returnType" />
    /// <param name="types" />
    public static PropertyInfo? GetProperty(
      [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] this Type type,
      string name,
      Type? returnType,
      Type[] types)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.GetProperty(name, returnType, types);
    }

    /// <param name="type" />
    /// <param name="c" />
    public static bool IsAssignableFrom(this Type type, [NotNullWhen(true)] Type? c)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.IsAssignableFrom(c);
    }

    /// <param name="type" />
    /// <param name="o" />
    public static bool IsInstanceOfType(this Type type, [NotNullWhen(true)] object? o)
    {
      ArgumentNullException.ThrowIfNull((object) type, nameof (type));
      return type.IsInstanceOfType(o);
    }
  }
}
