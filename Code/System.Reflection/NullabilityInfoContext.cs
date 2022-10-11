// Decompiled with JetBrains decompiler
// Type: System.Reflection.NullabilityInfoContext
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Collections.ObjectModel;


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides APIs for populating nullability information and context from reflection members: <see cref="T:System.Reflection.ParameterInfo" />, <see cref="T:System.Reflection.FieldInfo" />, <see cref="T:System.Reflection.PropertyInfo" />, and <see cref="T:System.Reflection.EventInfo" />.</summary>
  public sealed class NullabilityInfoContext
  {

    #nullable disable
    private readonly Dictionary<Module, NullabilityInfoContext.NotAnnotatedStatus> _publicOnlyModules = new Dictionary<Module, NullabilityInfoContext.NotAnnotatedStatus>();
    private readonly Dictionary<MemberInfo, NullabilityState> _context = new Dictionary<MemberInfo, NullabilityState>();

    internal static bool IsSupported { get; }

    private NullabilityState GetNullableContext(MemberInfo memberInfo)
    {
      for (; memberInfo != (MemberInfo) null; memberInfo = (MemberInfo) memberInfo.DeclaringType)
      {
        NullabilityState nullableContext;
        if (this._context.TryGetValue(memberInfo, out nullableContext))
          return nullableContext;
        foreach (CustomAttributeData customAttributeData in (IEnumerable<CustomAttributeData>) memberInfo.GetCustomAttributesData())
        {
          if (customAttributeData.AttributeType.Name == "NullableContextAttribute" && customAttributeData.AttributeType.Namespace == "System.Runtime.CompilerServices" && customAttributeData.ConstructorArguments.Count == 1)
          {
            nullableContext = NullabilityInfoContext.TranslateByte(customAttributeData.ConstructorArguments[0].Value);
            this._context.Add(memberInfo, nullableContext);
            return nullableContext;
          }
        }
      }
      return NullabilityState.Unknown;
    }


    #nullable enable
    /// <summary>Populates a <see cref="T:System.Reflection.NullabilityInfo" /> for the given <see cref="T:System.Reflection.ParameterInfo" />. If the <c>nullablePublicOnly</c> feature is set for an assembly, like it does in the .NET SDK, the private and/or internal member's nullability attributes are omitted, and the API will return the <see cref="F:System.Reflection.NullabilityState.Unknown" /> state.</summary>
    /// <param name="parameterInfo">The parameter for which to populate the nullability information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="parameterInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Reflection.NullabilityInfo" /> instance.</returns>
    public NullabilityInfo Create(ParameterInfo parameterInfo)
    {
      if (parameterInfo == null)
        throw new ArgumentNullException(nameof (parameterInfo));
      NullabilityInfoContext.EnsureIsSupported();
      MethodInfo member = parameterInfo.Member as MethodInfo;
      if ((object) member != null && this.IsPrivateOrInternalMethodAndAnnotationDisabled(member))
        return new NullabilityInfo(parameterInfo.ParameterType, NullabilityState.Unknown, NullabilityState.Unknown, (NullabilityInfo) null, Array.Empty<NullabilityInfo>());
      IList<CustomAttributeData> customAttributesData = parameterInfo.GetCustomAttributesData();
      NullabilityInfo nullabilityInfo = this.GetNullabilityInfo(parameterInfo.Member, parameterInfo.ParameterType, customAttributesData);
      if (nullabilityInfo.ReadState != NullabilityState.Unknown)
        this.CheckParameterMetadataType(parameterInfo, nullabilityInfo);
      this.CheckNullabilityAttributes(nullabilityInfo, customAttributesData);
      return nullabilityInfo;
    }


    #nullable disable
    private void CheckParameterMetadataType(ParameterInfo parameter, NullabilityInfo nullability)
    {
      MethodInfo member = parameter.Member as MethodInfo;
      if ((object) member == null)
        return;
      MethodInfo metadataDefinition = NullabilityInfoContext.GetMethodMetadataDefinition(member);
      ParameterInfo parameterInfo = (ParameterInfo) null;
      if (string.IsNullOrEmpty(parameter.Name))
      {
        parameterInfo = metadataDefinition.ReturnParameter;
      }
      else
      {
        ParameterInfo[] parameters = metadataDefinition.GetParameters();
        for (int index = 0; index < parameters.Length; ++index)
        {
          if (parameter.Position == index && parameter.Name == parameters[index].Name)
          {
            parameterInfo = parameters[index];
            break;
          }
        }
      }
      if (parameterInfo == null)
        return;
      this.CheckGenericParameters(nullability, (MemberInfo) metadataDefinition, parameterInfo.ParameterType);
    }

    private static MethodInfo GetMethodMetadataDefinition(MethodInfo method)
    {
      if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
        method = method.GetGenericMethodDefinition();
      return (MethodInfo) NullabilityInfoContext.GetMemberMetadataDefinition((MemberInfo) method);
    }

    private void CheckNullabilityAttributes(
      NullabilityInfo nullability,
      IList<CustomAttributeData> attributes)
    {
      foreach (CustomAttributeData attribute in (IEnumerable<CustomAttributeData>) attributes)
      {
        if (attribute.AttributeType.Namespace == "System.Diagnostics.CodeAnalysis")
        {
          if (attribute.AttributeType.Name == "NotNullAttribute" && nullability.ReadState == NullabilityState.Nullable)
          {
            nullability.ReadState = NullabilityState.NotNull;
            break;
          }
          if ((attribute.AttributeType.Name == "MaybeNullAttribute" || attribute.AttributeType.Name == "MaybeNullWhenAttribute") && nullability.ReadState == NullabilityState.NotNull && !nullability.Type.IsValueType)
          {
            nullability.ReadState = NullabilityState.Nullable;
            break;
          }
          if (attribute.AttributeType.Name == "DisallowNullAttribute" && nullability.WriteState == NullabilityState.Nullable)
          {
            nullability.WriteState = NullabilityState.NotNull;
            break;
          }
          if (attribute.AttributeType.Name == "AllowNullAttribute" && nullability.WriteState == NullabilityState.NotNull && !nullability.Type.IsValueType)
          {
            nullability.WriteState = NullabilityState.Nullable;
            break;
          }
        }
      }
    }


    #nullable enable
    /// <summary>Populates a <see cref="T:System.Reflection.NullabilityInfo" /> for the given <see cref="T:System.Reflection.PropertyInfo" />. If the <c>nullablePublicOnly</c> feature is set for an assembly, like it does in the .NET SDK, the private and/or internal member's nullability attributes are omitted, and the API will return the <see cref="F:System.Reflection.NullabilityState.Unknown" /> state.</summary>
    /// <param name="propertyInfo">The property for which to populate the nullability information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="propertyInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Reflection.NullabilityInfo" /> instance.</returns>
    public NullabilityInfo Create(PropertyInfo propertyInfo)
    {
      if ((object) propertyInfo == null)
        throw new ArgumentNullException(nameof (propertyInfo));
      NullabilityInfoContext.EnsureIsSupported();
      NullabilityInfo nullabilityInfo = this.GetNullabilityInfo((MemberInfo) propertyInfo, propertyInfo.PropertyType, propertyInfo.GetCustomAttributesData());
      MethodInfo getMethod = propertyInfo.GetGetMethod(true);
      MethodInfo setMethod = propertyInfo.GetSetMethod(true);
      if (getMethod != (MethodInfo) null)
      {
        if (this.IsPrivateOrInternalMethodAndAnnotationDisabled(getMethod))
          nullabilityInfo.ReadState = NullabilityState.Unknown;
        this.CheckNullabilityAttributes(nullabilityInfo, getMethod.ReturnParameter.GetCustomAttributesData());
      }
      else
        nullabilityInfo.ReadState = NullabilityState.Unknown;
      if (setMethod != (MethodInfo) null)
      {
        if (this.IsPrivateOrInternalMethodAndAnnotationDisabled(setMethod))
          nullabilityInfo.WriteState = NullabilityState.Unknown;
        this.CheckNullabilityAttributes(nullabilityInfo, setMethod.GetParameters()[0].GetCustomAttributesData());
      }
      else
        nullabilityInfo.WriteState = NullabilityState.Unknown;
      return nullabilityInfo;
    }


    #nullable disable
    private bool IsPrivateOrInternalMethodAndAnnotationDisabled(MethodInfo method) => (method.IsPrivate || method.IsFamilyAndAssembly || method.IsAssembly) && this.IsPublicOnly(method.IsPrivate, method.IsFamilyAndAssembly, method.IsAssembly, method.Module);


    #nullable enable
    /// <summary>Populates a <see cref="T:System.Reflection.NullabilityInfo" /> for the given <see cref="T:System.Reflection.EventInfo" />. If the <c>nullablePublicOnly</c> feature is set for an assembly, like it does in the .NET SDK, the private and/or internal member's nullability attributes are omitted, and the API will return the <see cref="F:System.Reflection.NullabilityState.Unknown" /> state.</summary>
    /// <param name="eventInfo">The event for which to populate nullability information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="eventInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Reflection.NullabilityInfo" /> instance.</returns>
    public NullabilityInfo Create(EventInfo eventInfo)
    {
      if ((object) eventInfo == null)
        throw new ArgumentNullException(nameof (eventInfo));
      NullabilityInfoContext.EnsureIsSupported();
      return this.GetNullabilityInfo((MemberInfo) eventInfo, eventInfo.EventHandlerType, eventInfo.GetCustomAttributesData());
    }

    /// <summary>Populates a <see cref="T:System.Reflection.NullabilityInfo" /> for the given <see cref="T:System.Reflection.FieldInfo" />. If the <c>nullablePublicOnly</c> feature is set for an assembly, like it does in the .NET SDK, the private and/or internal member's nullability attributes are omitted, and the API will return the <see cref="F:System.Reflection.NullabilityState.Unknown" /> state.</summary>
    /// <param name="fieldInfo">The field for which to populate the nullability information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="fieldInfo" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Reflection.NullabilityInfo" /> instance.</returns>
    public NullabilityInfo Create(FieldInfo fieldInfo)
    {
      if ((object) fieldInfo == null)
        throw new ArgumentNullException(nameof (fieldInfo));
      NullabilityInfoContext.EnsureIsSupported();
      if (this.IsPrivateOrInternalFieldAndAnnotationDisabled(fieldInfo))
        return new NullabilityInfo(fieldInfo.FieldType, NullabilityState.Unknown, NullabilityState.Unknown, (NullabilityInfo) null, Array.Empty<NullabilityInfo>());
      IList<CustomAttributeData> customAttributesData = fieldInfo.GetCustomAttributesData();
      NullabilityInfo nullabilityInfo = this.GetNullabilityInfo((MemberInfo) fieldInfo, fieldInfo.FieldType, customAttributesData);
      this.CheckNullabilityAttributes(nullabilityInfo, customAttributesData);
      return nullabilityInfo;
    }

    private static void EnsureIsSupported()
    {
      if (!NullabilityInfoContext.IsSupported)
        throw new InvalidOperationException(SR.NullabilityInfoContext_NotSupported);
    }


    #nullable disable
    private bool IsPrivateOrInternalFieldAndAnnotationDisabled(FieldInfo fieldInfo) => (fieldInfo.IsPrivate || fieldInfo.IsFamilyAndAssembly || fieldInfo.IsAssembly) && this.IsPublicOnly(fieldInfo.IsPrivate, fieldInfo.IsFamilyAndAssembly, fieldInfo.IsAssembly, fieldInfo.Module);

    private bool IsPublicOnly(
      bool isPrivate,
      bool isFamilyAndAssembly,
      bool isAssembly,
      Module module)
    {
      NullabilityInfoContext.NotAnnotatedStatus notAnnotatedStatus;
      if (!this._publicOnlyModules.TryGetValue(module, out notAnnotatedStatus))
      {
        notAnnotatedStatus = this.PopulateAnnotationInfo(module.GetCustomAttributesData());
        this._publicOnlyModules.Add(module, notAnnotatedStatus);
      }
      return notAnnotatedStatus != NullabilityInfoContext.NotAnnotatedStatus.None && (isPrivate | isFamilyAndAssembly && notAnnotatedStatus.HasFlag((Enum) NullabilityInfoContext.NotAnnotatedStatus.Private) || isAssembly && notAnnotatedStatus.HasFlag((Enum) NullabilityInfoContext.NotAnnotatedStatus.Internal));
    }

    private NullabilityInfoContext.NotAnnotatedStatus PopulateAnnotationInfo(
      IList<CustomAttributeData> customAttributes)
    {
      foreach (CustomAttributeData customAttribute in (IEnumerable<CustomAttributeData>) customAttributes)
      {
        if (customAttribute.AttributeType.Name == "NullablePublicOnlyAttribute" && customAttribute.AttributeType.Namespace == "System.Runtime.CompilerServices" && customAttribute.ConstructorArguments.Count == 1)
          return ((!(customAttribute.ConstructorArguments[0].Value is bool flag) ? 0 : 1) & (flag ? 1 : 0)) != 0 ? NullabilityInfoContext.NotAnnotatedStatus.Private | NullabilityInfoContext.NotAnnotatedStatus.Internal : NullabilityInfoContext.NotAnnotatedStatus.Private;
      }
      return NullabilityInfoContext.NotAnnotatedStatus.None;
    }

    private NullabilityInfo GetNullabilityInfo(
      MemberInfo memberInfo,
      Type type,
      IList<CustomAttributeData> customAttributes)
    {
      return this.GetNullabilityInfo(memberInfo, type, customAttributes, 0);
    }

    private NullabilityInfo GetNullabilityInfo(
      MemberInfo memberInfo,
      Type type,
      IList<CustomAttributeData> customAttributes,
      int index)
    {
      NullabilityState state = NullabilityState.Unknown;
      NullabilityInfo elementType = (NullabilityInfo) null;
      NullabilityInfo[] typeArguments = Array.Empty<NullabilityInfo>();
      Type type1 = type;
      if (type.IsValueType)
      {
        type1 = Nullable.GetUnderlyingType(type);
        if (type1 != (Type) null)
        {
          state = NullabilityState.Nullable;
        }
        else
        {
          type1 = type;
          state = NullabilityState.NotNull;
        }
      }
      else
      {
        if (!NullabilityInfoContext.ParseNullableState(customAttributes, index, ref state))
          state = this.GetNullableContext(memberInfo);
        if (type.IsArray)
          elementType = this.GetNullabilityInfo(memberInfo, type.GetElementType(), customAttributes, index + 1);
      }
      if (type1.IsGenericType)
      {
        Type[] genericArguments = type1.GetGenericArguments();
        typeArguments = new NullabilityInfo[genericArguments.Length];
        int index1 = 0;
        int num = 0;
        for (; index1 < genericArguments.Length; ++index1)
        {
          Type underlyingType = Nullable.GetUnderlyingType(genericArguments[index1]);
          if ((object) underlyingType == null)
            underlyingType = genericArguments[index1];
          Type type2 = underlyingType;
          if (!type2.IsValueType || type2.IsGenericType)
            ++num;
          typeArguments[index1] = this.GetNullabilityInfo(memberInfo, genericArguments[index1], customAttributes, index + num);
        }
      }
      NullabilityInfo nullability = new NullabilityInfo(type, state, state, elementType, typeArguments);
      if (!type.IsValueType && state != NullabilityState.Unknown)
        this.TryLoadGenericMetaTypeNullability(memberInfo, nullability);
      return nullability;
    }

    private static bool ParseNullableState(
      IList<CustomAttributeData> customAttributes,
      int index,
      ref NullabilityState state)
    {
      foreach (CustomAttributeData customAttribute in (IEnumerable<CustomAttributeData>) customAttributes)
      {
        if (customAttribute.AttributeType.Name == "NullableAttribute" && customAttribute.AttributeType.Namespace == "System.Runtime.CompilerServices" && customAttribute.ConstructorArguments.Count == 1)
        {
          switch (customAttribute.ConstructorArguments[0].Value)
          {
            case byte b1:
              state = NullabilityInfoContext.TranslateByte(b1);
              return true;
            case ReadOnlyCollection<CustomAttributeTypedArgument> readOnlyCollection:
              if (index < readOnlyCollection.Count)
              {
                if (readOnlyCollection[index].Value is byte b)
                {
                  state = NullabilityInfoContext.TranslateByte(b);
                  return true;
                }
                goto label_12;
              }
              else
                goto label_12;
            default:
              goto label_12;
          }
        }
      }
label_12:
      return false;
    }

    private void TryLoadGenericMetaTypeNullability(
      MemberInfo memberInfo,
      NullabilityInfo nullability)
    {
      MemberInfo metadataDefinition = NullabilityInfoContext.GetMemberMetadataDefinition(memberInfo);
      Type metaType = (Type) null;
      FieldInfo fieldInfo = metadataDefinition as FieldInfo;
      if ((object) fieldInfo != null)
      {
        metaType = fieldInfo.FieldType;
      }
      else
      {
        PropertyInfo property = metadataDefinition as PropertyInfo;
        if ((object) property != null)
          metaType = NullabilityInfoContext.GetPropertyMetaType(property);
      }
      if (!(metaType != (Type) null))
        return;
      this.CheckGenericParameters(nullability, metadataDefinition, metaType);
    }

    private static MemberInfo GetMemberMetadataDefinition(MemberInfo member)
    {
      Type declaringType = member.DeclaringType;
      return declaringType != (Type) null && declaringType.IsGenericType && !declaringType.IsGenericTypeDefinition ? declaringType.GetGenericTypeDefinition().GetMemberWithSameMetadataDefinitionAs(member) : member;
    }

    private static Type GetPropertyMetaType(PropertyInfo property)
    {
      MethodInfo getMethod = property.GetGetMethod(true);
      return (object) getMethod != null ? getMethod.ReturnType : property.GetSetMethod(true).GetParameters()[0].ParameterType;
    }

    private void CheckGenericParameters(
      NullabilityInfo nullability,
      MemberInfo metaMember,
      Type metaType)
    {
      if (metaType.IsGenericParameter)
      {
        NullabilityState state = nullability.ReadState;
        if (state == NullabilityState.NotNull && !NullabilityInfoContext.ParseNullableState(metaType.GetCustomAttributesData(), 0, ref state))
          state = this.GetNullableContext((MemberInfo) metaType);
        nullability.ReadState = state;
        nullability.WriteState = state;
      }
      else
      {
        if (!metaType.ContainsGenericParameters)
          return;
        if (nullability.GenericTypeArguments.Length != 0)
        {
          Type[] genericArguments = metaType.GetGenericArguments();
          for (int index = 0; index < genericArguments.Length; ++index)
          {
            if (genericArguments[index].IsGenericParameter)
            {
              NullabilityInfo nullabilityInfo = this.GetNullabilityInfo(metaMember, genericArguments[index], genericArguments[index].GetCustomAttributesData(), index + 1);
              nullability.GenericTypeArguments[index].ReadState = nullabilityInfo.ReadState;
              nullability.GenericTypeArguments[index].WriteState = nullabilityInfo.WriteState;
            }
            else
              this.UpdateGenericArrayElements(nullability.GenericTypeArguments[index].ElementType, metaMember, genericArguments[index]);
          }
        }
        else
          this.UpdateGenericArrayElements(nullability.ElementType, metaMember, metaType);
      }
    }

    private void UpdateGenericArrayElements(
      NullabilityInfo elementState,
      MemberInfo metaMember,
      Type metaType)
    {
      if (!metaType.IsArray || elementState == null || !metaType.GetElementType().IsGenericParameter)
        return;
      Type elementType = metaType.GetElementType();
      NullabilityInfo nullabilityInfo = this.GetNullabilityInfo(metaMember, elementType, elementType.GetCustomAttributesData(), 0);
      elementState.ReadState = nullabilityInfo.ReadState;
      elementState.WriteState = nullabilityInfo.WriteState;
    }

    private static NullabilityState TranslateByte(object value) => value is byte b ? NullabilityInfoContext.TranslateByte(b) : NullabilityState.Unknown;

    private static NullabilityState TranslateByte(byte b)
    {
      NullabilityState nullabilityState;
      switch (b)
      {
        case 1:
          nullabilityState = NullabilityState.NotNull;
          break;
        case 2:
          nullabilityState = NullabilityState.Nullable;
          break;
        default:
          nullabilityState = NullabilityState.Unknown;
          break;
      }
      return nullabilityState;
    }

    static NullabilityInfoContext()
    {
      bool isEnabled;
      NullabilityInfoContext.IsSupported = !AppContext.TryGetSwitch("System.Reflection.NullabilityInfoContext.IsSupported", out isEnabled) || isEnabled;
    }

    [Flags]
    private enum NotAnnotatedStatus
    {
      None = 0,
      Private = 1,
      Internal = 2,
    }
  }
}
