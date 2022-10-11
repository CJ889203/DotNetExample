// Decompiled with JetBrains decompiler
// Type: System.Reflection.IntrospectionExtensions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Contains methods for converting <see cref="T:System.Type" /> objects.</summary>
  public static class IntrospectionExtensions
  {
    /// <summary>Returns the <see cref="T:System.Reflection.TypeInfo" /> representation of the specified type.</summary>
    /// <param name="type">The type to convert.</param>
    /// <returns>The converted object.</returns>
    public static TypeInfo GetTypeInfo(this Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      return type is IReflectableType reflectableType ? reflectableType.GetTypeInfo() : (TypeInfo) new TypeDelegator(type);
    }
  }
}
