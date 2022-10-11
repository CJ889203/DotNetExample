// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyNameFlags
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.Reflection
{
  /// <summary>Provides information about an <see cref="T:System.Reflection.Assembly" /> reference.</summary>
  [Flags]
  public enum AssemblyNameFlags
  {
    /// <summary>Specifies that no flags are in effect.</summary>
    None = 0,
    /// <summary>Specifies that a public key is formed from the full public key rather than the public key token.</summary>
    PublicKey = 1,
    /// <summary>Specifies that just-in-time (JIT) compiler optimization is disabled for the assembly. This is the exact opposite of the meaning that is suggested by the member name.</summary>
    EnableJITcompileOptimizer = 16384, // 0x00004000
    /// <summary>Specifies that just-in-time (JIT) compiler tracking is enabled for the assembly.</summary>
    EnableJITcompileTracking = 32768, // 0x00008000
    /// <summary>Specifies that the assembly can be retargeted at runtime to an assembly from a different publisher. This value supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
    Retargetable = 256, // 0x00000100
  }
}
