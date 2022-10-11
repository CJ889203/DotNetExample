// Decompiled with JetBrains decompiler
// Type: System.Reflection.CustomAttributeData
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Text;


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides access to custom attribute data for assemblies, modules, types, members and parameters that are loaded into the reflection-only context.</summary>
  public class CustomAttributeData
  {
    /// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target member.</summary>
    /// <param name="target">The member whose attribute data is to be retrieved.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="target" /> is <see langword="null" />.</exception>
    /// <returns>A list of objects that represent data about the attributes that have been applied to the target member.</returns>
    public static IList<CustomAttributeData> GetCustomAttributes(
      MemberInfo target)
    {
      return (object) target != null ? target.GetCustomAttributesData() : throw new ArgumentNullException(nameof (target));
    }

    /// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target module.</summary>
    /// <param name="target">The module whose custom attribute data is to be retrieved.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="target" /> is <see langword="null" />.</exception>
    /// <returns>A list of objects that represent data about the attributes that have been applied to the target module.</returns>
    public static IList<CustomAttributeData> GetCustomAttributes(
      Module target)
    {
      return (object) target != null ? target.GetCustomAttributesData() : throw new ArgumentNullException(nameof (target));
    }

    /// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target assembly.</summary>
    /// <param name="target">The assembly whose custom attribute data is to be retrieved.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="target" /> is <see langword="null" />.</exception>
    /// <returns>A list of objects that represent data about the attributes that have been applied to the target assembly.</returns>
    public static IList<CustomAttributeData> GetCustomAttributes(
      Assembly target)
    {
      return (object) target != null ? target.GetCustomAttributesData() : throw new ArgumentNullException(nameof (target));
    }

    /// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target parameter.</summary>
    /// <param name="target">The parameter whose attribute data is to be retrieved.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="target" /> is <see langword="null" />.</exception>
    /// <returns>A list of objects that represent data about the attributes that have been applied to the target parameter.</returns>
    public static IList<CustomAttributeData> GetCustomAttributes(
      ParameterInfo target)
    {
      return target != null ? target.GetCustomAttributesData() : throw new ArgumentNullException(nameof (target));
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Reflection.CustomAttributeData" /> class.</summary>
    protected CustomAttributeData()
    {
    }

    /// <summary>Returns a string representation of the custom attribute.</summary>
    /// <returns>A string value that represents the custom attribute.</returns>
    public override unsafe string ToString()
    {
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(512)), 256));
      valueStringBuilder.Append('[');
      valueStringBuilder.Append(this.Constructor.DeclaringType.FullName);
      valueStringBuilder.Append('(');
      bool flag = true;
      IList<CustomAttributeTypedArgument> constructorArguments = this.ConstructorArguments;
      int count1 = constructorArguments.Count;
      for (int index = 0; index < count1; ++index)
      {
        if (!flag)
          valueStringBuilder.Append(", ");
        valueStringBuilder.Append(constructorArguments[index].ToString());
        flag = false;
      }
      IList<CustomAttributeNamedArgument> namedArguments = this.NamedArguments;
      int count2 = namedArguments.Count;
      for (int index = 0; index < count2; ++index)
      {
        if (!flag)
          valueStringBuilder.Append(", ");
        valueStringBuilder.Append(namedArguments[index].ToString());
        flag = false;
      }
      valueStringBuilder.Append(")]");
      return valueStringBuilder.ToString();
    }

    /// <summary>Serves as a hash function for a particular type.</summary>
    /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is equal to the current instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => obj == this;

    /// <summary>Gets the type of the attribute.</summary>
    /// <returns>The type of the attribute.</returns>
    public virtual Type AttributeType => this.Constructor.DeclaringType;

    /// <summary>Gets a <see cref="T:System.Reflection.ConstructorInfo" /> object that represents the constructor that would have initialized the custom attribute.</summary>
    /// <returns>An object that represents the constructor that would have initialized the custom attribute represented by the current instance of the <see cref="T:System.Reflection.CustomAttributeData" /> class.</returns>
    public virtual ConstructorInfo Constructor => (ConstructorInfo) null;

    /// <summary>Gets the list of positional arguments specified for the attribute instance represented by the <see cref="T:System.Reflection.CustomAttributeData" /> object.</summary>
    /// <returns>A collection of structures that represent the positional arguments specified for the custom attribute instance.</returns>
    public virtual IList<CustomAttributeTypedArgument> ConstructorArguments => (IList<CustomAttributeTypedArgument>) null;

    /// <summary>Gets the list of named arguments specified for the attribute instance represented by the <see cref="T:System.Reflection.CustomAttributeData" /> object.</summary>
    /// <returns>A collection of structures that represent the named arguments specified for the custom attribute instance.</returns>
    public virtual IList<CustomAttributeNamedArgument> NamedArguments => (IList<CustomAttributeNamedArgument>) null;
  }
}
