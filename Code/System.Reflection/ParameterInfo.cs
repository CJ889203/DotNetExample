// Decompiled with JetBrains decompiler
// Type: System.Reflection.ParameterInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>Discovers the attributes of a parameter and provides access to parameter metadata.</summary>
  public class ParameterInfo : ICustomAttributeProvider, IObjectReference
  {
    /// <summary>The attributes of the parameter.</summary>
    protected ParameterAttributes AttrsImpl;
    /// <summary>The <see langword="Type" /> of the parameter.</summary>
    protected Type? ClassImpl;
    /// <summary>The default value of the parameter.</summary>
    protected object? DefaultValueImpl;
    /// <summary>The member in which the field is implemented.</summary>
    protected MemberInfo MemberImpl;
    /// <summary>The name of the parameter.</summary>
    protected string? NameImpl;
    /// <summary>The zero-based position of the parameter in the parameter list.</summary>
    protected int PositionImpl;

    /// <summary>Initializes a new instance of the <see langword="ParameterInfo" /> class.</summary>
    protected ParameterInfo()
    {
    }

    /// <summary>Gets the attributes for this parameter.</summary>
    /// <returns>A <see langword="ParameterAttributes" /> object representing the attributes for this parameter.</returns>
    public virtual ParameterAttributes Attributes => this.AttrsImpl;

    /// <summary>Gets a value indicating the member in which the parameter is implemented.</summary>
    /// <returns>The member which implanted the parameter represented by this <see cref="T:System.Reflection.ParameterInfo" />.</returns>
    public virtual MemberInfo Member => this.MemberImpl;

    /// <summary>Gets the name of the parameter.</summary>
    /// <returns>The simple name of this parameter.</returns>
    public virtual string? Name => this.NameImpl;

    /// <summary>Gets the <see langword="Type" /> of this parameter.</summary>
    /// <returns>The <see langword="Type" /> object that represents the <see langword="Type" /> of this parameter.</returns>
    public virtual Type ParameterType => this.ClassImpl;

    /// <summary>Gets the zero-based position of the parameter in the formal parameter list.</summary>
    /// <returns>An integer representing the position this parameter occupies in the parameter list.</returns>
    public virtual int Position => this.PositionImpl;

    /// <summary>Gets a value indicating whether this is an input parameter.</summary>
    /// <returns>
    /// <see langword="true" /> if the parameter is an input parameter; otherwise, <see langword="false" />.</returns>
    public bool IsIn => (this.Attributes & ParameterAttributes.In) != 0;

    /// <summary>Gets a value indicating whether this parameter is a locale identifier (lcid).</summary>
    /// <returns>
    /// <see langword="true" /> if the parameter is a locale identifier; otherwise, <see langword="false" />.</returns>
    public bool IsLcid => (this.Attributes & ParameterAttributes.Lcid) != 0;

    /// <summary>Gets a value indicating whether this parameter is optional.</summary>
    /// <returns>
    /// <see langword="true" /> if the parameter is optional; otherwise, <see langword="false" />.</returns>
    public bool IsOptional => (this.Attributes & ParameterAttributes.Optional) != 0;

    /// <summary>Gets a value indicating whether this is an output parameter.</summary>
    /// <returns>
    /// <see langword="true" /> if the parameter is an output parameter; otherwise, <see langword="false" />.</returns>
    public bool IsOut => (this.Attributes & ParameterAttributes.Out) != 0;

    /// <summary>Gets a value indicating whether this is a <see langword="Retval" /> parameter.</summary>
    /// <returns>
    /// <see langword="true" /> if the parameter is a <see langword="Retval" />; otherwise, <see langword="false" />.</returns>
    public bool IsRetval => (this.Attributes & ParameterAttributes.Retval) != 0;

    /// <summary>Gets a value indicating the default value if the parameter has a default value.</summary>
    /// <returns>The default value of the parameter, or <see cref="F:System.DBNull.Value" /> if the parameter has no default value.</returns>
    public virtual object? DefaultValue => throw NotImplemented.ByDesign;

    /// <summary>Gets a value indicating the default value if the parameter has a default value.</summary>
    /// <returns>The default value of the parameter, or <see cref="F:System.DBNull.Value" /> if the parameter has no default value.</returns>
    public virtual object? RawDefaultValue => throw NotImplemented.ByDesign;

    /// <summary>Gets a value that indicates whether this parameter has a default value.</summary>
    /// <returns>
    /// <see langword="true" /> if this parameter has a default value; otherwise, <see langword="false" />.</returns>
    public virtual bool HasDefaultValue => throw NotImplemented.ByDesign;

    /// <summary>Determines whether the custom attribute of the specified type or its derived types is applied to this parameter.</summary>
    /// <param name="attributeType">The <see langword="Type" /> object to search for.</param>
    /// <param name="inherit">This argument is ignored for objects of this type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="attributeType" /> is not a <see cref="T:System.Type" /> object supplied by the common language runtime.</exception>
    /// <returns>
    /// <see langword="true" /> if one or more instances of <paramref name="attributeType" /> or its derived types are applied to this parameter; otherwise, <see langword="false" />.</returns>
    public virtual bool IsDefined(Type attributeType, bool inherit)
    {
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      return false;
    }

    /// <summary>Gets a collection that contains this parameter's custom attributes.</summary>
    /// <returns>A collection that contains this parameter's custom attributes.</returns>
    public virtual IEnumerable<CustomAttributeData> CustomAttributes => (IEnumerable<CustomAttributeData>) this.GetCustomAttributesData();

    /// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects for the current parameter, which can be used in the reflection-only context.</summary>
    /// <returns>A generic list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the current parameter.</returns>
    public virtual IList<CustomAttributeData> GetCustomAttributesData() => throw NotImplemented.ByDesign;

    /// <summary>Gets all the custom attributes defined on this parameter.</summary>
    /// <param name="inherit">This argument is ignored for objects of this type.</param>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <returns>An array that contains all the custom attributes applied to this parameter.</returns>
    public virtual object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();

    /// <summary>Gets the custom attributes of the specified type or its derived types that are applied to this parameter.</summary>
    /// <param name="attributeType">The custom attributes identified by type.</param>
    /// <param name="inherit">This argument is ignored for objects of this type.</param>
    /// <exception cref="T:System.ArgumentException">The type must be a type provided by the underlying runtime system.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <returns>An array that contains the custom attributes of the specified type or its derived types.</returns>
    public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      return Array.Empty<object>();
    }

    /// <summary>Gets the optional custom modifiers of the parameter.</summary>
    /// <returns>An array of <see cref="T:System.Type" /> objects that identify the optional custom modifiers of the current parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
    public virtual Type[] GetOptionalCustomModifiers() => Type.EmptyTypes;

    /// <summary>Gets the required custom modifiers of the parameter.</summary>
    /// <returns>An array of <see cref="T:System.Type" /> objects that identify the required custom modifiers of the current parameter, such as <see cref="T:System.Runtime.CompilerServices.IsConst" /> or <see cref="T:System.Runtime.CompilerServices.IsImplicitlyDereferenced" />.</returns>
    public virtual Type[] GetRequiredCustomModifiers() => Type.EmptyTypes;

    /// <summary>Gets a value that identifies this parameter in metadata.</summary>
    /// <returns>A value which, in combination with the module, uniquely identifies this parameter in metadata.</returns>
    public virtual int MetadataToken => 134217728;

    /// <summary>Returns the real object that should be deserialized instead of the object that the serialized stream specifies.</summary>
    /// <param name="context">The serialized stream from which the current object is deserialized.</param>
    /// <exception cref="T:System.Runtime.Serialization.SerializationException">The parameter's position in the parameter list of its associated member is not valid for that member's type.</exception>
    /// <returns>The actual object that is put into the graph.</returns>
    public object GetRealObject(StreamingContext context)
    {
      if (this.MemberImpl == (MemberInfo) null)
        throw new SerializationException(SR.Serialization_InsufficientState);
      switch (this.MemberImpl.MemberType)
      {
        case MemberTypes.Constructor:
        case MemberTypes.Method:
          if (this.PositionImpl == -1)
            return this.MemberImpl.MemberType == MemberTypes.Method ? (object) ((MethodInfo) this.MemberImpl).ReturnParameter : throw new SerializationException(SR.Serialization_BadParameterInfo);
          ParameterInfo[] parametersNoCopy = ((MethodBase) this.MemberImpl).GetParametersNoCopy();
          if (parametersNoCopy != null && this.PositionImpl < parametersNoCopy.Length)
            return (object) parametersNoCopy[this.PositionImpl];
          throw new SerializationException(SR.Serialization_BadParameterInfo);
        case MemberTypes.Property:
          ParameterInfo[] indexParameters = ((PropertyInfo) this.MemberImpl).GetIndexParameters();
          if (indexParameters != null && this.PositionImpl > -1 && this.PositionImpl < indexParameters.Length)
            return (object) indexParameters[this.PositionImpl];
          throw new SerializationException(SR.Serialization_BadParameterInfo);
        default:
          throw new SerializationException(SR.Serialization_NoParameterInfo);
      }
    }

    /// <summary>Gets the parameter type and name represented as a string.</summary>
    /// <returns>A string containing the type and the name of the parameter.</returns>
    public override string ToString()
    {
      string str = this.ParameterType.FormatTypeName();
      string name = this.Name;
      return name != null ? str + " " + name : str;
    }
  }
}
