// Decompiled with JetBrains decompiler
// Type: System.IO.MemoryMappedFiles.MemoryMappedFileAccess
// Assembly: System.IO.MemoryMappedFiles, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: ADB8E953-9D00-4DED-81B8-A4FE54270273
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.MemoryMappedFiles.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.MemoryMappedFiles.xml

namespace System.IO.MemoryMappedFiles
{
  /// <summary>Specifies access capabilities and restrictions for a memory-mapped file or view.</summary>
  public enum MemoryMappedFileAccess
  {
    /// <summary>Read and write access to the file.</summary>
    ReadWrite,
    /// <summary>Read-only access to the file.</summary>
    Read,
    /// <summary>Write-only access to file.</summary>
    Write,
    /// <summary>Read and write access to the file, with the restriction that any write operations will not be seen by other processes.</summary>
    CopyOnWrite,
    /// <summary>Read access to the file that can store and run executable code.</summary>
    ReadExecute,
    /// <summary>Read and write access to the file that can store and run executable code.</summary>
    ReadWriteExecute,
  }
}
