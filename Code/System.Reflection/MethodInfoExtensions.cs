// Decompiled with JetBrains decompiler
// Type: System.Reflection.MethodInfoExtensions
// Assembly: System.Reflection.TypeExtensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 640AC10B-88E0-451A-B5D0-A4B0F7E22777
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.TypeExtensions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.TypeExtensions.xml


#nullable enable
namespace System.Reflection
{
  public static class MethodInfoExtensions
  {
    /// <param name="method" />
    public static MethodInfo GetBaseDefinition(this MethodInfo method)
    {
      ArgumentNullException.ThrowIfNull((object) method, nameof (method));
      return method.GetBaseDefinition();
    }
  }
}
