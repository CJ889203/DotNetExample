// Decompiled with JetBrains decompiler
// Type: System.Reflection.AssemblyFlags
// Assembly: System.Reflection.Metadata, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: FDD13CB9-4DB5-4759-8B88-2D188C369E68
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.Metadata.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.Metadata.xml

namespace System.Reflection
{
  [Flags]
  public enum AssemblyFlags
  {
    /// <summary>The assembly reference holds the full (unhashed) public key. Not applicable on assembly definition.</summary>
    PublicKey = 1,
    /// <summary>The implementation of the referenced assembly used at runtime is not expected to match the version seen at compile time.</summary>
    Retargetable = 256, // 0x00000100
    /// <summary>The assembly contains Windows Runtime code.</summary>
    WindowsRuntime = 512, // 0x00000200
    /// <summary>Content type masked bits that correspond to values of <see cref="T:System.Reflection.AssemblyContentType" />.</summary>
    ContentTypeMask = 3584, // 0x00000E00
    /// <summary>Just-In-Time (JIT) compiler optimization is disabled for the assembly.</summary>
    DisableJitCompileOptimizer = 16384, // 0x00004000
    /// <summary>Just-In-Time (JIT) compiler tracking is enabled for the assembly.</summary>
    EnableJitCompileTracking = 32768, // 0x00008000
  }
}
