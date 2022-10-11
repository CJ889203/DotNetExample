// Decompiled with JetBrains decompiler
// Type: System.Reflection.ImageFileMachine
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Identifies the platform targeted by an executable.</summary>
  public enum ImageFileMachine
  {
    /// <summary>Targets a 32-bit Intel processor.</summary>
    I386 = 332, // 0x0000014C
    /// <summary>Targets an ARM processor.</summary>
    ARM = 452, // 0x000001C4
    /// <summary>Targets a 64-bit Intel processor.</summary>
    IA64 = 512, // 0x00000200
    /// <summary>Targets a 64-bit AMD processor.</summary>
    AMD64 = 34404, // 0x00008664
  }
}
