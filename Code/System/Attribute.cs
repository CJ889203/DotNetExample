// Decompiled with JetBrains decompiler
// Type: System.Attribute
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;


#nullable enable
namespace System
{
  /// <summary>Represents the base class for custom attributes.</summary>
  [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public abstract class Attribute
  {

    #nullable disable
    private static Attribute[] InternalGetCustomAttributes(
      PropertyInfo element,
      Type type,
      bool inherit)
    {
      Attribute[] customAttributes1 = (Attribute[]) element.GetCustomAttributes(type, inherit);
      if (!inherit)
        return customAttributes1;
      Type[] indexParameterTypes = Attribute.GetIndexParameterTypes(element);
      PropertyInfo parentDefinition = Attribute.GetParentDefinition(element, indexParameterTypes);
      if (parentDefinition == (PropertyInfo) null)
        return customAttributes1;
      Dictionary<Type, AttributeUsageAttribute> types = new Dictionary<Type, AttributeUsageAttribute>(11);
      List<Attribute> attributeList = new List<Attribute>();
      Attribute.CopyToAttributeList(attributeList, customAttributes1, types);
      do
      {
        Attribute[] customAttributes2 = Attribute.GetCustomAttributes((MemberInfo) parentDefinition, type, false);
        Attribute.AddAttributesToList(attributeList, customAttributes2, types);
        parentDefinition = Attribute.GetParentDefinition(parentDefinition, indexParameterTypes);
      }
      while (parentDefinition != (PropertyInfo) null);
      Attribute[] attributeArrayHelper = Attribute.CreateAttributeArrayHelper(type, attributeList.Count);
      attributeList.CopyTo(attributeArrayHelper, 0);
      return attributeArrayHelper;
    }

    private static bool InternalIsDefined(PropertyInfo element, Type attributeType, bool inherit)
    {
      if (element.IsDefined(attributeType, inherit))
        return true;
      if (inherit && Attribute.InternalGetAttributeUsage(attributeType).Inherited)
      {
        Type[] indexParameterTypes = Attribute.GetIndexParameterTypes(element);
        for (PropertyInfo parentDefinition = Attribute.GetParentDefinition(element, indexParameterTypes); parentDefinition != (PropertyInfo) null; parentDefinition = Attribute.GetParentDefinition(parentDefinition, indexParameterTypes))
        {
          if (parentDefinition.IsDefined(attributeType, false))
            return true;
        }
      }
      return false;
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2075:UnrecognizedReflectionPattern", Justification = "rtPropAccessor.DeclaringType is guaranteed to have the specified property because rtPropAccessor.GetParentDefinition() returned a non-null MethodInfo.")]
    private static PropertyInfo GetParentDefinition(
      PropertyInfo property,
      Type[] propertyParameters)
    {
      MethodInfo methodInfo = property.GetGetMethod(true);
      if ((object) methodInfo == null)
        methodInfo = property.GetSetMethod(true);
      RuntimeMethodInfo runtimeMethodInfo = methodInfo as RuntimeMethodInfo;
      if ((MethodInfo) runtimeMethodInfo != (MethodInfo) null)
      {
        RuntimeMethodInfo parentDefinition = runtimeMethodInfo.GetParentDefinition();
        if ((MethodInfo) parentDefinition != (MethodInfo) null)
          return parentDefinition.DeclaringType.GetProperty(property.Name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, property.PropertyType, propertyParameters, (ParameterModifier[]) null);
      }
      return (PropertyInfo) null;
    }

    private static Attribute[] InternalGetCustomAttributes(
      EventInfo element,
      Type type,
      bool inherit)
    {
      Attribute[] customAttributes1 = (Attribute[]) element.GetCustomAttributes(type, inherit);
      if (!inherit)
        return customAttributes1;
      EventInfo parentDefinition = Attribute.GetParentDefinition(element);
      if (parentDefinition == (EventInfo) null)
        return customAttributes1;
      Dictionary<Type, AttributeUsageAttribute> types = new Dictionary<Type, AttributeUsageAttribute>(11);
      List<Attribute> attributeList = new List<Attribute>();
      Attribute.CopyToAttributeList(attributeList, customAttributes1, types);
      do
      {
        Attribute[] customAttributes2 = Attribute.GetCustomAttributes((MemberInfo) parentDefinition, type, false);
        Attribute.AddAttributesToList(attributeList, customAttributes2, types);
        parentDefinition = Attribute.GetParentDefinition(parentDefinition);
      }
      while (parentDefinition != (EventInfo) null);
      Attribute[] attributeArrayHelper = Attribute.CreateAttributeArrayHelper(type, attributeList.Count);
      attributeList.CopyTo(attributeArrayHelper, 0);
      return attributeArrayHelper;
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2075:UnrecognizedReflectionPattern", Justification = "rtAdd.DeclaringType is guaranteed to have the specified event because rtAdd.GetParentDefinition() returned a non-null MethodInfo.")]
    private static EventInfo GetParentDefinition(EventInfo ev)
    {
      RuntimeMethodInfo addMethod = ev.GetAddMethod(true) as RuntimeMethodInfo;
      if ((MethodInfo) addMethod != (MethodInfo) null)
      {
        RuntimeMethodInfo parentDefinition = addMethod.GetParentDefinition();
        if ((MethodInfo) parentDefinition != (MethodInfo) null)
          return parentDefinition.DeclaringType.GetEvent(ev.Name);
      }
      return (EventInfo) null;
    }

    private static bool InternalIsDefined(EventInfo element, Type attributeType, bool inherit)
    {
      if (element.IsDefined(attributeType, inherit))
        return true;
      if (inherit && Attribute.InternalGetAttributeUsage(attributeType).Inherited)
      {
        for (EventInfo parentDefinition = Attribute.GetParentDefinition(element); parentDefinition != (EventInfo) null; parentDefinition = Attribute.GetParentDefinition(parentDefinition))
        {
          if (parentDefinition.IsDefined(attributeType, false))
            return true;
        }
      }
      return false;
    }

    private static ParameterInfo GetParentDefinition(ParameterInfo param)
    {
      RuntimeMethodInfo member = param.Member as RuntimeMethodInfo;
      if ((MethodInfo) member != (MethodInfo) null)
      {
        RuntimeMethodInfo parentDefinition = member.GetParentDefinition();
        if ((MethodInfo) parentDefinition != (MethodInfo) null)
        {
          int position = param.Position;
          return position == -1 ? parentDefinition.ReturnParameter : parentDefinition.GetParameters()[position];
        }
      }
      return (ParameterInfo) null;
    }

    private static Attribute[] InternalParamGetCustomAttributes(
      ParameterInfo param,
      Type type,
      bool inherit)
    {
      List<Type> typeList = new List<Type>();
      if ((object) type == null)
        type = typeof (Attribute);
      object[] customAttributes1 = param.GetCustomAttributes(type, false);
      for (int index = 0; index < customAttributes1.Length; ++index)
      {
        Type type1 = customAttributes1[index].GetType();
        if (!Attribute.InternalGetAttributeUsage(type1).AllowMultiple)
          typeList.Add(type1);
      }
      Attribute[] destinationArray = customAttributes1.Length != 0 ? (Attribute[]) customAttributes1 : Attribute.CreateAttributeArrayHelper(type, 0);
      if ((object) param.Member.DeclaringType == null || !inherit)
        return destinationArray;
      for (ParameterInfo parentDefinition = Attribute.GetParentDefinition(param); parentDefinition != null; parentDefinition = Attribute.GetParentDefinition(parentDefinition))
      {
        object[] customAttributes2 = parentDefinition.GetCustomAttributes(type, false);
        int elementCount = 0;
        for (int index = 0; index < customAttributes2.Length; ++index)
        {
          Type type2 = customAttributes2[index].GetType();
          AttributeUsageAttribute attributeUsage = Attribute.InternalGetAttributeUsage(type2);
          if (attributeUsage.Inherited && !typeList.Contains(type2))
          {
            if (!attributeUsage.AllowMultiple)
              typeList.Add(type2);
            ++elementCount;
          }
          else
            customAttributes2[index] = (object) null;
        }
        Attribute[] attributeArrayHelper = Attribute.CreateAttributeArrayHelper(type, elementCount);
        int index1 = 0;
        for (int index2 = 0; index2 < customAttributes2.Length; ++index2)
        {
          object obj = customAttributes2[index2];
          if (obj != null)
          {
            attributeArrayHelper[index1] = (Attribute) obj;
            ++index1;
          }
        }
        Attribute[] sourceArray = destinationArray;
        destinationArray = Attribute.CreateAttributeArrayHelper(type, sourceArray.Length + index1);
        Array.Copy((Array) sourceArray, (Array) destinationArray, sourceArray.Length);
        int length = sourceArray.Length;
        for (int index3 = 0; index3 < attributeArrayHelper.Length; ++index3)
          destinationArray[length + index3] = attributeArrayHelper[index3];
      }
      return destinationArray;
    }

    private static bool InternalParamIsDefined(ParameterInfo param, Type type, bool inherit)
    {
      if (param.IsDefined(type, false))
        return true;
      if ((object) param.Member.DeclaringType == null || !inherit)
        return false;
      for (ParameterInfo parentDefinition = Attribute.GetParentDefinition(param); parentDefinition != null; parentDefinition = Attribute.GetParentDefinition(parentDefinition))
      {
        object[] customAttributes = parentDefinition.GetCustomAttributes(type, false);
        for (int index = 0; index < customAttributes.Length; ++index)
        {
          AttributeUsageAttribute attributeUsage = Attribute.InternalGetAttributeUsage(customAttributes[index].GetType());
          if (customAttributes[index] is Attribute && attributeUsage.Inherited)
            return true;
        }
      }
      return false;
    }

    private static void CopyToAttributeList(
      List<Attribute> attributeList,
      Attribute[] attributes,
      Dictionary<Type, AttributeUsageAttribute> types)
    {
      for (int index = 0; index < attributes.Length; ++index)
      {
        attributeList.Add(attributes[index]);
        Type type = attributes[index].GetType();
        if (!types.ContainsKey(type))
          types[type] = Attribute.InternalGetAttributeUsage(type);
      }
    }

    private static Type[] GetIndexParameterTypes(PropertyInfo element)
    {
      ParameterInfo[] indexParameters = element.GetIndexParameters();
      if (indexParameters.Length == 0)
        return Type.EmptyTypes;
      Type[] indexParameterTypes = new Type[indexParameters.Length];
      for (int index = 0; index < indexParameters.Length; ++index)
        indexParameterTypes[index] = indexParameters[index].ParameterType;
      return indexParameterTypes;
    }

    private static void AddAttributesToList(
      List<Attribute> attributeList,
      Attribute[] attributes,
      Dictionary<Type, AttributeUsageAttribute> types)
    {
      for (int index = 0; index < attributes.Length; ++index)
      {
        Type type = attributes[index].GetType();
        AttributeUsageAttribute attributeUsage;
        types.TryGetValue(type, out attributeUsage);
        if (attributeUsage == null)
        {
          attributeUsage = Attribute.InternalGetAttributeUsage(type);
          types[type] = attributeUsage;
          if (attributeUsage.Inherited)
            attributeList.Add(attributes[index]);
        }
        else if (attributeUsage.Inherited && attributeUsage.AllowMultiple)
          attributeList.Add(attributes[index]);
      }
    }

    private static AttributeUsageAttribute InternalGetAttributeUsage(
      Type type)
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (AttributeUsageAttribute), false);
      if (customAttributes.Length == 1)
        return (AttributeUsageAttribute) customAttributes[0];
      if (customAttributes.Length == 0)
        return AttributeUsageAttribute.Default;
      throw new FormatException(SR.Format(SR.Format_AttributeUsage, (object) type));
    }

    private static Attribute[] CreateAttributeArrayHelper(
      Type elementType,
      int elementCount)
    {
      return (Attribute[]) Array.CreateInstance(elementType, elementCount);
    }


    #nullable enable
    /// <summary>Retrieves an array of the custom attributes applied to a member of a type. Parameters specify the member, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.MemberInfo" /> class that describes a constructor, event, field, method, or property member of a class.</param>
    /// <param name="attributeType" />
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="type" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="element" /> is not a constructor, method, property, event, type, or field.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes of type <paramref name="type" /> applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(MemberInfo element, Type attributeType) => Attribute.GetCustomAttributes(element, attributeType, true);

    /// <summary>Retrieves an array of the custom attributes applied to a member of a type. Parameters specify the member, the type of the custom attribute to search for, and whether to search ancestors of the member.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.MemberInfo" /> class that describes a constructor, event, field, method, or property member of a class.</param>
    /// <param name="attributeType" />
    /// <param name="inherit">If <see langword="true" />, specifies to also search the ancestors of <paramref name="element" /> for custom attributes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="type" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="element" /> is not a constructor, method, property, event, type, or field.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes of type <paramref name="type" /> applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(
      MemberInfo element,
      Type attributeType,
      bool inherit)
    {
      if (element == (MemberInfo) null)
        throw new ArgumentNullException(nameof (element));
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      if (!attributeType.IsSubclassOf(typeof (Attribute)) && attributeType != typeof (Attribute))
        throw new ArgumentException(SR.Argument_MustHaveAttributeBaseClass);
      Attribute[] customAttributes;
      switch (element.MemberType)
      {
        case MemberTypes.Event:
          customAttributes = Attribute.InternalGetCustomAttributes((EventInfo) element, attributeType, inherit);
          break;
        case MemberTypes.Property:
          customAttributes = Attribute.InternalGetCustomAttributes((PropertyInfo) element, attributeType, inherit);
          break;
        default:
          customAttributes = element.GetCustomAttributes(attributeType, inherit) as Attribute[];
          break;
      }
      return customAttributes;
    }

    /// <summary>Retrieves an array of the custom attributes applied to a member of a type. A parameter specifies the member.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.MemberInfo" /> class that describes a constructor, event, field, method, or property member of a class.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="element" /> is not a constructor, method, property, event, type, or field.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(MemberInfo element) => Attribute.GetCustomAttributes(element, true);

    /// <summary>Retrieves an array of the custom attributes applied to a member of a type. Parameters specify the member, the type of the custom attribute to search for, and whether to search ancestors of the member.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.MemberInfo" /> class that describes a constructor, event, field, method, or property member of a class.</param>
    /// <param name="inherit">If <see langword="true" />, specifies to also search the ancestors of <paramref name="element" /> for custom attributes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="element" /> is not a constructor, method, property, event, type, or field.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(MemberInfo element, bool inherit)
    {
      if (element == (MemberInfo) null)
        throw new ArgumentNullException(nameof (element));
      Attribute[] customAttributes;
      switch (element.MemberType)
      {
        case MemberTypes.Event:
          customAttributes = Attribute.InternalGetCustomAttributes((EventInfo) element, typeof (Attribute), inherit);
          break;
        case MemberTypes.Property:
          customAttributes = Attribute.InternalGetCustomAttributes((PropertyInfo) element, typeof (Attribute), inherit);
          break;
        default:
          customAttributes = element.GetCustomAttributes(typeof (Attribute), inherit) as Attribute[];
          break;
      }
      return customAttributes;
    }

    /// <summary>Determines whether any custom attributes are applied to a member of a type. Parameters specify the member, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.MemberInfo" /> class that describes a constructor, event, field, method, type, or property member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="element" /> is not a constructor, method, property, event, type, or field.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute of type <paramref name="attributeType" /> is applied to <paramref name="element" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(MemberInfo element, Type attributeType) => Attribute.IsDefined(element, attributeType, true);

    /// <summary>Determines whether any custom attributes are applied to a member of a type. Parameters specify the member, the type of the custom attribute to search for, and whether to search ancestors of the member.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.MemberInfo" /> class that describes a constructor, event, field, method, type, or property member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">If <see langword="true" />, specifies to also search the ancestors of <paramref name="element" /> for custom attributes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="element" /> is not a constructor, method, property, event, type, or field.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute of type <paramref name="attributeType" /> is applied to <paramref name="element" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(MemberInfo element, Type attributeType, bool inherit)
    {
      if (element == (MemberInfo) null)
        throw new ArgumentNullException(nameof (element));
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      if (!attributeType.IsSubclassOf(typeof (Attribute)) && attributeType != typeof (Attribute))
        throw new ArgumentException(SR.Argument_MustHaveAttributeBaseClass);
      bool flag;
      switch (element.MemberType)
      {
        case MemberTypes.Event:
          flag = Attribute.InternalIsDefined((EventInfo) element, attributeType, inherit);
          break;
        case MemberTypes.Property:
          flag = Attribute.InternalIsDefined((PropertyInfo) element, attributeType, inherit);
          break;
        default:
          flag = element.IsDefined(attributeType, inherit);
          break;
      }
      return flag;
    }

    /// <summary>Retrieves a custom attribute applied to a member of a type. Parameters specify the member, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.MemberInfo" /> class that describes a constructor, event, field, method, or property member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="element" /> is not a constructor, method, property, event, type, or field.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>A reference to the single custom attribute of type <paramref name="attributeType" /> that is applied to <paramref name="element" />, or <see langword="null" /> if there is no such attribute.</returns>
    public static Attribute? GetCustomAttribute(MemberInfo element, Type attributeType) => Attribute.GetCustomAttribute(element, attributeType, true);

    /// <summary>Retrieves a custom attribute applied to a member of a type. Parameters specify the member, the type of the custom attribute to search for, and whether to search ancestors of the member.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.MemberInfo" /> class that describes a constructor, event, field, method, or property member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">If <see langword="true" />, specifies to also search the ancestors of <paramref name="element" /> for custom attributes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="element" /> is not a constructor, method, property, event, type, or field.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>A reference to the single custom attribute of type <paramref name="attributeType" /> that is applied to <paramref name="element" />, or <see langword="null" /> if there is no such attribute.</returns>
    public static Attribute? GetCustomAttribute(
      MemberInfo element,
      Type attributeType,
      bool inherit)
    {
      Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
      if (customAttributes == null || customAttributes.Length == 0)
        return (Attribute) null;
      return customAttributes.Length == 1 ? customAttributes[0] : throw new AmbiguousMatchException(SR.RFLCT_AmbigCust);
    }

    /// <summary>Retrieves an array of the custom attributes applied to a method parameter. A parameter specifies the method parameter.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.ParameterInfo" /> class that describes a parameter of a member of a class.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(ParameterInfo element) => Attribute.GetCustomAttributes(element, true);

    /// <summary>Retrieves an array of the custom attributes applied to a method parameter. Parameters specify the method parameter, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.ParameterInfo" /> class that describes a parameter of a member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes of type <paramref name="attributeType" /> applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(
      ParameterInfo element,
      Type attributeType)
    {
      return Attribute.GetCustomAttributes(element, attributeType, true);
    }

    /// <summary>Retrieves an array of the custom attributes applied to a method parameter. Parameters specify the method parameter, the type of the custom attribute to search for, and whether to search ancestors of the method parameter.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.ParameterInfo" /> class that describes a parameter of a member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">If <see langword="true" />, specifies to also search the ancestors of <paramref name="element" /> for custom attributes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes of type <paramref name="attributeType" /> applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(
      ParameterInfo element,
      Type attributeType,
      bool inherit)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      if (!attributeType.IsSubclassOf(typeof (Attribute)) && attributeType != typeof (Attribute))
        throw new ArgumentException(SR.Argument_MustHaveAttributeBaseClass);
      if (element.Member == (MemberInfo) null)
        throw new ArgumentException(SR.Argument_InvalidParameterInfo, nameof (element));
      return element.Member.MemberType == MemberTypes.Method & inherit ? Attribute.InternalParamGetCustomAttributes(element, attributeType, inherit) : element.GetCustomAttributes(attributeType, inherit) as Attribute[];
    }

    /// <summary>Retrieves an array of the custom attributes applied to a method parameter. Parameters specify the method parameter, and whether to search ancestors of the method parameter.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.ParameterInfo" /> class that describes a parameter of a member of a class.</param>
    /// <param name="inherit">If <see langword="true" />, specifies to also search the ancestors of <paramref name="element" /> for custom attributes.</param>
    /// <exception cref="T:System.ArgumentException">The <see cref="P:System.Reflection.ParameterInfo.Member" /> property of <paramref name="element" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(ParameterInfo element, bool inherit)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      if (element.Member == (MemberInfo) null)
        throw new ArgumentException(SR.Argument_InvalidParameterInfo, nameof (element));
      return element.Member.MemberType == MemberTypes.Method & inherit ? Attribute.InternalParamGetCustomAttributes(element, (Type) null, inherit) : element.GetCustomAttributes(typeof (Attribute), inherit) as Attribute[];
    }

    /// <summary>Determines whether any custom attributes are applied to a method parameter. Parameters specify the method parameter, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.ParameterInfo" /> class that describes a parameter of a member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute of type <paramref name="attributeType" /> is applied to <paramref name="element" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(ParameterInfo element, Type attributeType) => Attribute.IsDefined(element, attributeType, true);

    /// <summary>Determines whether any custom attributes are applied to a method parameter. Parameters specify the method parameter, the type of the custom attribute to search for, and whether to search ancestors of the method parameter.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.ParameterInfo" /> class that describes a parameter of a member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">If <see langword="true" />, specifies to also search the ancestors of <paramref name="element" /> for custom attributes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.ExecutionEngineException">
    /// <paramref name="element" /> is not a method, constructor, or type.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute of type <paramref name="attributeType" /> is applied to <paramref name="element" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(ParameterInfo element, Type attributeType, bool inherit)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      if (!attributeType.IsSubclassOf(typeof (Attribute)) && attributeType != typeof (Attribute))
        throw new ArgumentException(SR.Argument_MustHaveAttributeBaseClass);
      switch (element.Member.MemberType)
      {
        case MemberTypes.Constructor:
          return element.IsDefined(attributeType, false);
        case MemberTypes.Method:
          return Attribute.InternalParamIsDefined(element, attributeType, inherit);
        case MemberTypes.Property:
          return element.IsDefined(attributeType, false);
        default:
          throw new ArgumentException(SR.Argument_InvalidParamInfo);
      }
    }

    /// <summary>Retrieves a custom attribute applied to a method parameter. Parameters specify the method parameter, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.ParameterInfo" /> class that describes a parameter of a member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>A reference to the single custom attribute of type <paramref name="attributeType" /> that is applied to <paramref name="element" />, or <see langword="null" /> if there is no such attribute.</returns>
    public static Attribute? GetCustomAttribute(ParameterInfo element, Type attributeType) => Attribute.GetCustomAttribute(element, attributeType, true);

    /// <summary>Retrieves a custom attribute applied to a method parameter. Parameters specify the method parameter, the type of the custom attribute to search for, and whether to search ancestors of the method parameter.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.ParameterInfo" /> class that describes a parameter of a member of a class.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">If <see langword="true" />, specifies to also search the ancestors of <paramref name="element" /> for custom attributes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <returns>A reference to the single custom attribute of type <paramref name="attributeType" /> that is applied to <paramref name="element" />, or <see langword="null" /> if there is no such attribute.</returns>
    public static Attribute? GetCustomAttribute(
      ParameterInfo element,
      Type attributeType,
      bool inherit)
    {
      Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
      if (customAttributes == null || customAttributes.Length == 0)
        return (Attribute) null;
      if (customAttributes.Length == 0)
        return (Attribute) null;
      return customAttributes.Length == 1 ? customAttributes[0] : throw new AmbiguousMatchException(SR.RFLCT_AmbigCust);
    }

    /// <summary>Retrieves an array of the custom attributes applied to a module. Parameters specify the module, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Module" /> class that describes a portable executable file.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes of type <paramref name="attributeType" /> applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(Module element, Type attributeType) => Attribute.GetCustomAttributes(element, attributeType, true);

    /// <summary>Retrieves an array of the custom attributes applied to a module. A parameter specifies the module.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Module" /> class that describes a portable executable file.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> is <see langword="null" />.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(Module element) => Attribute.GetCustomAttributes(element, true);

    /// <summary>Retrieves an array of the custom attributes applied to a module. Parameters specify the module, and an ignored search option.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Module" /> class that describes a portable executable file.</param>
    /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(Module element, bool inherit) => !(element == (Module) null) ? (Attribute[]) element.GetCustomAttributes(typeof (Attribute), inherit) : throw new ArgumentNullException(nameof (element));

    /// <summary>Retrieves an array of the custom attributes applied to a module. Parameters specify the module, the type of the custom attribute to search for, and an ignored search option.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Module" /> class that describes a portable executable file.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes of type <paramref name="attributeType" /> applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(
      Module element,
      Type attributeType,
      bool inherit)
    {
      if (element == (Module) null)
        throw new ArgumentNullException(nameof (element));
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      if (!attributeType.IsSubclassOf(typeof (Attribute)) && attributeType != typeof (Attribute))
        throw new ArgumentException(SR.Argument_MustHaveAttributeBaseClass);
      return (Attribute[]) element.GetCustomAttributes(attributeType, inherit);
    }

    /// <summary>Determines whether any custom attributes of a specified type are applied to a module. Parameters specify the module, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Module" /> class that describes a portable executable file.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute of type <paramref name="attributeType" /> is applied to <paramref name="element" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(Module element, Type attributeType) => Attribute.IsDefined(element, attributeType, false);

    /// <summary>Determines whether any custom attributes are applied to a module. Parameters specify the module, the type of the custom attribute to search for, and an ignored search option.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Module" /> class that describes a portable executable file.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute of type <paramref name="attributeType" /> is applied to <paramref name="element" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(Module element, Type attributeType, bool inherit)
    {
      if (element == (Module) null)
        throw new ArgumentNullException(nameof (element));
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      return attributeType.IsSubclassOf(typeof (Attribute)) || !(attributeType != typeof (Attribute)) ? element.IsDefined(attributeType, false) : throw new ArgumentException(SR.Argument_MustHaveAttributeBaseClass);
    }

    /// <summary>Retrieves a custom attribute applied to a module. Parameters specify the module, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Module" /> class that describes a portable executable file.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <returns>A reference to the single custom attribute of type <paramref name="attributeType" /> that is applied to <paramref name="element" />, or <see langword="null" /> if there is no such attribute.</returns>
    public static Attribute? GetCustomAttribute(Module element, Type attributeType) => Attribute.GetCustomAttribute(element, attributeType, true);

    /// <summary>Retrieves a custom attribute applied to a module. Parameters specify the module, the type of the custom attribute to search for, and an ignored search option.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Module" /> class that describes a portable executable file.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <returns>A reference to the single custom attribute of type <paramref name="attributeType" /> that is applied to <paramref name="element" />, or <see langword="null" /> if there is no such attribute.</returns>
    public static Attribute? GetCustomAttribute(
      Module element,
      Type attributeType,
      bool inherit)
    {
      Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
      if (customAttributes == null || customAttributes.Length == 0)
        return (Attribute) null;
      return customAttributes.Length == 1 ? customAttributes[0] : throw new AmbiguousMatchException(SR.RFLCT_AmbigCust);
    }

    /// <summary>Retrieves an array of the custom attributes applied to an assembly. Parameters specify the assembly, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Assembly" /> class that describes a reusable collection of modules.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes of type <paramref name="attributeType" /> applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType) => Attribute.GetCustomAttributes(element, attributeType, true);

    /// <summary>Retrieves an array of the custom attributes applied to an assembly. Parameters specify the assembly, the type of the custom attribute to search for, and an ignored search option.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Assembly" /> class that describes a reusable collection of modules.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes of type <paramref name="attributeType" /> applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(
      Assembly element,
      Type attributeType,
      bool inherit)
    {
      if (element == (Assembly) null)
        throw new ArgumentNullException(nameof (element));
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      if (!attributeType.IsSubclassOf(typeof (Attribute)) && attributeType != typeof (Attribute))
        throw new ArgumentException(SR.Argument_MustHaveAttributeBaseClass);
      return (Attribute[]) element.GetCustomAttributes(attributeType, inherit);
    }

    /// <summary>Retrieves an array of the custom attributes applied to an assembly. A parameter specifies the assembly.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Assembly" /> class that describes a reusable collection of modules.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> is <see langword="null" />.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(Assembly element) => Attribute.GetCustomAttributes(element, true);

    /// <summary>Retrieves an array of the custom attributes applied to an assembly. Parameters specify the assembly, and an ignored search option.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Assembly" /> class that describes a reusable collection of modules.</param>
    /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <returns>An <see cref="T:System.Attribute" /> array that contains the custom attributes applied to <paramref name="element" />, or an empty array if no such custom attributes exist.</returns>
    public static Attribute[] GetCustomAttributes(Assembly element, bool inherit) => !(element == (Assembly) null) ? (Attribute[]) element.GetCustomAttributes(typeof (Attribute), inherit) : throw new ArgumentNullException(nameof (element));

    /// <summary>Determines whether any custom attributes are applied to an assembly. Parameters specify the assembly, and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Assembly" /> class that describes a reusable collection of modules.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute of type <paramref name="attributeType" /> is applied to <paramref name="element" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(Assembly element, Type attributeType) => Attribute.IsDefined(element, attributeType, true);

    /// <summary>Determines whether any custom attributes are applied to an assembly. Parameters specify the assembly, the type of the custom attribute to search for, and an ignored search option.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Assembly" /> class that describes a reusable collection of modules.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <returns>
    /// <see langword="true" /> if a custom attribute of type <paramref name="attributeType" /> is applied to <paramref name="element" />; otherwise, <see langword="false" />.</returns>
    public static bool IsDefined(Assembly element, Type attributeType, bool inherit)
    {
      if (element == (Assembly) null)
        throw new ArgumentNullException(nameof (element));
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      return attributeType.IsSubclassOf(typeof (Attribute)) || !(attributeType != typeof (Attribute)) ? element.IsDefined(attributeType, false) : throw new ArgumentException(SR.Argument_MustHaveAttributeBaseClass);
    }

    /// <summary>Retrieves a custom attribute applied to a specified assembly. Parameters specify the assembly and the type of the custom attribute to search for.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Assembly" /> class that describes a reusable collection of modules.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <returns>A reference to the single custom attribute of type <paramref name="attributeType" /> that is applied to <paramref name="element" />, or <see langword="null" /> if there is no such attribute.</returns>
    public static Attribute? GetCustomAttribute(Assembly element, Type attributeType) => Attribute.GetCustomAttribute(element, attributeType, true);

    /// <summary>Retrieves a custom attribute applied to an assembly. Parameters specify the assembly, the type of the custom attribute to search for, and an ignored search option.</summary>
    /// <param name="element">An object derived from the <see cref="T:System.Reflection.Assembly" /> class that describes a reusable collection of modules.</param>
    /// <param name="attributeType">The type, or a base type, of the custom attribute to search for.</param>
    /// <param name="inherit">This parameter is ignored, and does not affect the operation of this method.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="element" /> or <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not derived from <see cref="T:System.Attribute" />.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">More than one of the requested attributes was found.</exception>
    /// <returns>A reference to the single custom attribute of type <paramref name="attributeType" /> that is applied to <paramref name="element" />, or <see langword="null" /> if there is no such attribute.</returns>
    public static Attribute? GetCustomAttribute(
      Assembly element,
      Type attributeType,
      bool inherit)
    {
      Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
      if (customAttributes == null || customAttributes.Length == 0)
        return (Attribute) null;
      return customAttributes.Length == 1 ? customAttributes[0] : throw new AmbiguousMatchException(SR.RFLCT_AmbigCust);
    }

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> and this instance are of the same type and have identical field values; otherwise, <see langword="false" />.</returns>
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2075:UnrecognizedReflectionPattern", Justification = "Unused fields don't make a difference for equality")]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      Type type = this.GetType();
      object obj1 = (object) this;
      for (; type != typeof (Attribute); type = type.BaseType)
      {
        FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int index = 0; index < fields.Length; ++index)
        {
          if (!Attribute.AreFieldValuesEqual(fields[index].GetValue(obj1), fields[index].GetValue(obj)))
            return false;
        }
      }
      return true;
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2075:UnrecognizedReflectionPattern", Justification = "Unused fields don't make a difference for hashcode quality")]
    public override int GetHashCode()
    {
      Type type;
      for (type = this.GetType(); type != typeof (Attribute); type = type.BaseType)
      {
        FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        object obj1 = (object) null;
        for (int index = 0; index < fields.Length; ++index)
        {
          object obj2 = fields[index].GetValue((object) this);
          if (obj2 != null && !obj2.GetType().IsArray)
            obj1 = obj2;
          if (obj1 != null)
            break;
        }
        if (obj1 != null)
          return obj1.GetHashCode();
      }
      return type.GetHashCode();
    }


    #nullable disable
    private static bool AreFieldValuesEqual(object thisValue, object thatValue)
    {
      if (thisValue == null && thatValue == null)
        return true;
      if (thisValue == null || thatValue == null)
        return false;
      Type type = thisValue.GetType();
      if (type.IsArray)
      {
        if (!type.Equals(thatValue.GetType()))
          return false;
        Array array1 = (Array) thisValue;
        Array array2 = (Array) thatValue;
        if (array1.Length != array2.Length)
          return false;
        for (int index = 0; index < array1.Length; ++index)
        {
          if (!Attribute.AreFieldValuesEqual(array1.GetValue(index), array2.GetValue(index)))
            return false;
        }
      }
      else if (!thisValue.Equals(thatValue))
        return false;
      return true;
    }


    #nullable enable
    /// <summary>When implemented in a derived class, gets a unique identifier for this <see cref="T:System.Attribute" />.</summary>
    /// <returns>An <see cref="T:System.Object" /> that is a unique identifier for the attribute.</returns>
    public virtual object TypeId => (object) this.GetType();

    /// <summary>When overridden in a derived class, returns a value that indicates whether this instance equals a specified object.</summary>
    /// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance of <see cref="T:System.Attribute" />.</param>
    /// <returns>
    /// <see langword="true" /> if this instance equals <paramref name="obj" />; otherwise, <see langword="false" />.</returns>
    public virtual bool Match(object? obj) => this.Equals(obj);

    /// <summary>When overridden in a derived class, indicates whether the value of this instance is the default value for the derived class.</summary>
    /// <returns>
    /// <see langword="true" /> if this instance is the default attribute for the class; otherwise, <see langword="false" />.</returns>
    public virtual bool IsDefaultAttribute() => false;
  }
}
