// Decompiled with JetBrains decompiler
// Type: System.Reflection.IReflectableType
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents a type that you can reflect over.</summary>
  public interface IReflectableType
  {
    /// <summary>Retrieves an object that represents this type.</summary>
    /// <returns>An object that represents this type.</returns>
    TypeInfo GetTypeInfo();
  }
}
