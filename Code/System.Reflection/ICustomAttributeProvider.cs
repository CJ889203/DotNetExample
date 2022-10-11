// Decompiled with JetBrains decompiler
// Type: System.Reflection.ICustomAttributeProvider
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides custom attributes for reflection objects that support them.</summary>
  public interface ICustomAttributeProvider
  {
    /// <summary>Returns an array of all of the custom attributes defined on this member, excluding named attributes, or an empty array if there are no custom attributes.</summary>
    /// <param name="inherit">When <see langword="true" />, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <exception cref="T:System.TypeLoadException">The custom attribute type cannot be loaded.</exception>
    /// <exception cref="T:System.Reflection.AmbiguousMatchException">There is more than one attribute of type <paramref name="attributeType" /> defined on this member.</exception>
    /// <returns>An array of Objects representing custom attributes, or an empty array.</returns>
    object[] GetCustomAttributes(bool inherit);

    /// <summary>Returns an array of custom attributes defined on this member, identified by type, or an empty array if there are no custom attributes of that type.</summary>
    /// <param name="attributeType">The type of the custom attributes.</param>
    /// <param name="inherit">When <see langword="true" />, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <exception cref="T:System.TypeLoadException">The custom attribute type cannot be loaded.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="attributeType" /> is <see langword="null" />.</exception>
    /// <returns>An array of Objects representing custom attributes, or an empty array.</returns>
    object[] GetCustomAttributes(Type attributeType, bool inherit);

    /// <summary>Indicates whether one or more instance of <paramref name="attributeType" /> is defined on this member.</summary>
    /// <param name="attributeType">The type of the custom attributes.</param>
    /// <param name="inherit">When <see langword="true" />, look up the hierarchy chain for the inherited custom attribute.</param>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="attributeType" /> is defined on this member; <see langword="false" /> otherwise.</returns>
    bool IsDefined(Type attributeType, bool inherit);
  }
}
