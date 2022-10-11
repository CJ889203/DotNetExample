// Decompiled with JetBrains decompiler
// Type: System.GCKind
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System
{
  /// <summary>Specifies the kind of a garbage collection.</summary>
  public enum GCKind
  {
    /// <summary>Any kind of collection.</summary>
    Any,
    /// <summary>A gen0 or gen1 collection.</summary>
    Ephemeral,
    /// <summary>A blocking gen2 collection.</summary>
    FullBlocking,
    /// <summary>A background collection. This is always a generation 2 collection.</summary>
    Background,
  }
}
