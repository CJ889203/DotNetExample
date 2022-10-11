// Decompiled with JetBrains decompiler
// Type: System.Threading.LockRecursionPolicy
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

namespace System.Threading
{
  /// <summary>Specifies whether a lock can be entered multiple times by the same thread.</summary>
  public enum LockRecursionPolicy
  {
    /// <summary>If a thread tries to enter a lock recursively, an exception is thrown. Some classes may allow certain recursions when this setting is in effect.</summary>
    NoRecursion,
    /// <summary>A thread can enter a lock recursively. Some classes may restrict this capability.</summary>
    SupportsRecursion,
  }
}
