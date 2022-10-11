// Decompiled with JetBrains decompiler
// Type: System.Reflection.MemberInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Reflection
{
  /// <summary>Obtains information about the attributes of a member and provides access to member metadata.</summary>
  public abstract class MemberInfo : ICustomAttributeProvider
  {

    #nullable disable
    internal virtual bool CacheEquals(object o) => throw new NotImplementedException();

    internal bool HasSameMetadataDefinitionAsCore<TOther>(MemberInfo other) where TOther : MemberInfo
    {
      if ((object) other == null)
        throw new ArgumentNullException(nameof (other));
      return other is TOther && this.MetadataToken == other.MetadataToken && this.Module.Equals((object) other.Module);
    }

    /// <summary>When overridden in a derived class, gets a <see cref="T:System.Reflection.MemberTypes" /> value indicating the type of the member - method, constructor, event, and so on.</summary>
    /// <returns>A <see cref="T:System.Reflection.MemberTypes" /> value indicating the type of member.</returns>
    public abstract MemberTypes MemberType { get; }


    #nullable enable
    /// <summary>Gets the name of the current member.</summary>
    /// <returns>A <see cref="T:System.String" /> containing the name of this member.</returns>
    public abstract string Name { get; }

    /// <summary>Gets the class that declares this member.</summary>
    /// <returns>The <see langword="Type" /> object for the class that declares this member.</returns>
    public abstract Type? DeclaringType { get; }

    /// <summary>Gets the class object that was used to obtain this instance of <see langword="MemberInfo" />.</summary>
    /// <returns>The <see langword="Type" /> object through which this <see langword="MemberInfo" /> object was obtained.</returns>
    public abstract Type? ReflectedType { get; }

    /// <summary>Gets the module in which the type that declares the member represented by the current <see cref="T:System.Reflection.MemberInfo" /> is defined.</summary>
    /// <exception cref="T:System.NotImplementedException">This method is not implemented.</exception>
    /// <returns>The <see cref="T:System.Reflection.Module" /> in which the type that declares the member represented by the current <see cref="T:System.Reflection.MemberInfo" /> is defined.</returns>
    public virtual Module Module => (this as Type ?? throw NotImplemented.ByDesign).Module;

    /// <param name="other" />
    public virtual bool HasSameMetadataDefinitionAs(MemberInfo other) => throw NotImplemented.ByDesign;

    /// <summary>When overridden in a derived class, indicates whether one or more attributes of the specified type or of its derived types is applied to this member.</summary>
    /// <param name="attributeType">The type of custom attribute to search for. The search includes derived types.</param>
    /// <param name="inherit">
    /// <see langword="true" /> to search this member's inheritance chain to find the attributes; otherwise, <see langword="false" />. This parameter is ignored for properties and events.</param>
    /// <returns>
    /// <see langword="true" /> if one or more instances of <paramref name="attributeType" /> or any of its derived types is applied to this member; otherwise, <see langword="false" />.</returns>
    public abstract bool IsDefined(Type attributeType, bool inherit);

    /// <summary>When overridden in a derived class, returns an array of all custom attributes applied to this member.</summary>
    /// <param name="inherit">
    /// <see langword="true" /> to search this member's inheritance chain to find the attributes; otherwise, <see langword="false" />. This parameter is ignored for properties and events.</param>
    /// <exception cref="T:System.InvalidOperationException">This member belongs to a type that is loaded into the reflection-only context. See How to: Load Assemblies into the Reflection-Only Context.</exception>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type could not be loaded.</exception>
    /// <returns>An array that contains all the custom attributes applied to this member, or an array with zero elements if no attributes are defined.</returns>
    public abstract object[] GetCustomAttributes(bool inherit);

    /// <summary>When overridden in a derived class, returns an array of custom attributes applied to this member and identified by <see cref="T:System.Type" />.</summary>
    /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
    /// <param name="inherit">
    /// <see langword="true" /> to search this member's inheritance chain to find the attributes; otherwise, <see langword="false" />. This parameter is ignored for properties and events.</param>
    /// <exception cref="T:System.TypeLoadException">A custom attribute type cannot be loaded.</exception>
    /// <exception cref="T:System.ArgumentNullException">If <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">This member belongs to a type that is loaded into the reflection-only context. See How to: Load Assemblies into the Reflection-Only Context.</exception>
    /// <returns>An array of custom attributes applied to this member, or an array with zero elements if no attributes assignable to <paramref name="attributeType" /> have been applied.</returns>
    public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

    /// <summary>Gets a collection that contains this member's custom attributes.</summary>
    /// <returns>A collection that contains this member's custom attributes.</returns>
    public virtual IEnumerable<CustomAttributeData> CustomAttributes => (IEnumerable<CustomAttributeData>) this.GetCustomAttributesData();

    /// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target member.</summary>
    /// <returns>A generic list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target member.</returns>
    public virtual IList<CustomAttributeData> GetCustomAttributesData() => throw NotImplemented.ByDesign;

    /// <summary>Gets a value that indicates whether this <see cref="T:System.Reflection.MemberInfo" /> object is part of an assembly held in a collectible <see cref="T:System.Runtime.Loader.AssemblyLoadContext" />.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Reflection.MemberInfo" /> is part of an assembly held in a collectible assembly load context; otherwise, <see langword="false" />.</returns>
    public virtual bool IsCollectible => true;

    /// <summary>Gets a value that identifies a metadata element.</summary>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Reflection.MemberInfo" /> represents an array method, such as <see langword="Address" />, on an array type whose element type is a dynamic type that has not been completed. To get a metadata token in this case, pass the <see cref="T:System.Reflection.MemberInfo" /> object to the <see cref="M:System.Reflection.Emit.ModuleBuilder.GetMethodToken(System.Reflection.MethodInfo)" /> method; or use the <see cref="M:System.Reflection.Emit.ModuleBuilder.GetArrayMethodToken(System.Type,System.String,System.Reflection.CallingConventions,System.Type,System.Type[])" /> method to get the token directly, instead of using the <see cref="M:System.Reflection.Emit.ModuleBuilder.GetArrayMethod(System.Type,System.String,System.Reflection.CallingConventions,System.Type,System.Type[])" /> method to get a <see cref="T:System.Reflection.MethodInfo" /> first.</exception>
    /// <returns>A value which, in combination with <see cref="P:System.Reflection.MemberInfo.Module" />, uniquely identifies a metadata element.</returns>
    public virtual int MetadataToken => throw new InvalidOperationException();

    /// <summary>Returns a value that indicates whether this instance is equal to a specified object.</summary>
    /// <param name="obj">An object to compare with this instance, or <see langword="null" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> equals the type and value of this instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>Indicates whether two <see cref="T:System.Reflection.MemberInfo" /> objects are equal.</summary>
    /// <param name="left">The <see cref="T:System.Reflection.MemberInfo" /> to compare to <paramref name="right" />.</param>
    /// <param name="right">The <see cref="T:System.Reflection.MemberInfo" /> to compare to <paramref name="left" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise <see langword="false" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(MemberInfo? left, MemberInfo? right)
    {
      if ((object) right == null)
        return (object) left == null;
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>Indicates whether two <see cref="T:System.Reflection.MemberInfo" /> objects are not equal.</summary>
    /// <param name="left">The <see cref="T:System.Reflection.MemberInfo" /> to compare to <paramref name="right" />.</param>
    /// <param name="right">The <see cref="T:System.Reflection.MemberInfo" /> to compare to <paramref name="left" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise <see langword="false" />.</returns>
    public static bool operator !=(MemberInfo? left, MemberInfo? right) => !(left == right);
  }
}
