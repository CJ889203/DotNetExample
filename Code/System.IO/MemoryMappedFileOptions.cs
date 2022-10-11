// Decompiled with JetBrains decompiler
// Type: System.IO.MemoryMappedFiles.MemoryMappedFileOptions
// Assembly: System.IO.MemoryMappedFiles, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: ADB8E953-9D00-4DED-81B8-A4FE54270273
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.MemoryMappedFiles.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.MemoryMappedFiles.xml

namespace System.IO.MemoryMappedFiles
{
  /// <summary>Provides memory allocation options for memory-mapped files.</summary>
  [Flags]
  public enum MemoryMappedFileOptions
  {
    /// <summary>No memory allocation options are applied.</summary>
    None = 0,
    /// <summary>Memory allocation is delayed until a view is created with either the <see cref="M:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateViewAccessor" /> or <see cref="M:System.IO.MemoryMappedFiles.MemoryMappedFile.CreateViewStream" /> method.</summary>
    DelayAllocatePages = 67108864, // 0x04000000
  }
}
