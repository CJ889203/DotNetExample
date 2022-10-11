// Decompiled with JetBrains decompiler
// Type: System.Reflection.PropertyInfoExtensions
// Assembly: System.Reflection.TypeExtensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 640AC10B-88E0-451A-B5D0-A4B0F7E22777
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.TypeExtensions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.TypeExtensions.xml


#nullable enable
namespace System.Reflection
{
  public static class PropertyInfoExtensions
  {
    /// <param name="property" />
    public static MethodInfo[] GetAccessors(this PropertyInfo property)
    {
      ArgumentNullException.ThrowIfNull((object) property, nameof (property));
      return property.GetAccessors();
    }

    /// <param name="property" />
    /// <param name="nonPublic" />
    public static MethodInfo[] GetAccessors(this PropertyInfo property, bool nonPublic)
    {
      ArgumentNullException.ThrowIfNull((object) property, nameof (property));
      return property.GetAccessors(nonPublic);
    }

    /// <param name="property" />
    public static MethodInfo? GetGetMethod(this PropertyInfo property)
    {
      ArgumentNullException.ThrowIfNull((object) property, nameof (property));
      return property.GetGetMethod();
    }

    /// <param name="property" />
    /// <param name="nonPublic" />
    public static MethodInfo? GetGetMethod(this PropertyInfo property, bool nonPublic)
    {
      ArgumentNullException.ThrowIfNull((object) property, nameof (property));
      return property.GetGetMethod(nonPublic);
    }

    /// <param name="property" />
    public static MethodInfo? GetSetMethod(this PropertyInfo property)
    {
      ArgumentNullException.ThrowIfNull((object) property, nameof (property));
      return property.GetSetMethod();
    }

    /// <param name="property" />
    /// <param name="nonPublic" />
    public static MethodInfo? GetSetMethod(this PropertyInfo property, bool nonPublic)
    {
      ArgumentNullException.ThrowIfNull((object) property, nameof (property));
      return property.GetSetMethod(nonPublic);
    }
  }
}
