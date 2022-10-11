// Decompiled with JetBrains decompiler
// Type: System.Reflection.ICustomTypeProvider
// Assembly: System.ObjectModel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: CE91E254-92DA-467C-857F-17CEB6B52889
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.ObjectModel.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.ObjectModel.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents an object that provides a custom type.</summary>
  public interface ICustomTypeProvider
  {
    /// <summary>Gets the custom type provided by this object.</summary>
    /// <returns>The custom type.</returns>
    Type GetCustomType();
  }
}
