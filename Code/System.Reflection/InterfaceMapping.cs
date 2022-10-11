// Decompiled with JetBrains decompiler
// Type: System.Reflection.InterfaceMapping
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Retrieves the mapping of an interface into the actual methods on a class that implements that interface.</summary>
  public struct InterfaceMapping
  {
    /// <summary>Represents the type that was used to create the interface mapping.</summary>
    public Type TargetType;
    /// <summary>Shows the type that represents the interface.</summary>
    public Type InterfaceType;
    /// <summary>Shows the methods that implement the interface.</summary>
    public MethodInfo[] TargetMethods;
    /// <summary>Shows the methods that are defined on the interface.</summary>
    public MethodInfo[] InterfaceMethods;
  }
}
