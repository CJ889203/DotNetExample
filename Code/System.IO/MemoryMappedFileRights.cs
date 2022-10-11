// Decompiled with JetBrains decompiler
// Type: System.IO.MemoryMappedFiles.MemoryMappedFileRights
// Assembly: System.IO.MemoryMappedFiles, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: ADB8E953-9D00-4DED-81B8-A4FE54270273
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.MemoryMappedFiles.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.MemoryMappedFiles.xml

namespace System.IO.MemoryMappedFiles
{
  /// <summary>Specifies access rights to a memory-mapped file that is not associated with a file on disk.</summary>
  [Flags]
  public enum MemoryMappedFileRights
  {
    /// <summary>The right to read and write to a file with the restriction that write operations will not be seen by other processes.</summary>
    CopyOnWrite = 1,
    /// <summary>The right to add data to a file or remove data from a file.</summary>
    Write = 2,
    /// <summary>The right to open and copy a file as read-only.</summary>
    Read = 4,
    /// <summary>The right to run an application file.</summary>
    Execute = 8,
    /// <summary>The right to delete a file.</summary>
    Delete = 65536, // 0x00010000
    /// <summary>The right to open and copy access and audit rules from a file. This does not include the right to read data, file system attributes, or extended file system attributes.</summary>
    ReadPermissions = 131072, // 0x00020000
    /// <summary>The right to change the security and audit rules associated with a file.</summary>
    ChangePermissions = 262144, // 0x00040000
    /// <summary>The right to change the owner of a file.</summary>
    TakeOwnership = 524288, // 0x00080000
    /// <summary>The right to open and copy a file, and the right to add data to a file or remove data from a file.</summary>
    ReadWrite = Read | Write, // 0x00000006
    /// <summary>The right to open and copy a folder or file as read-only, and to run application files. This right includes the <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileRights.Read" /> right and the <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileRights.Execute" /> right.</summary>
    ReadExecute = Execute | Read, // 0x0000000C
    /// <summary>The right to open and copy a file, the right to add data to a file or remove data from a file, and the right to run an application file.</summary>
    ReadWriteExecute = ReadExecute | Write, // 0x0000000E
    /// <summary>The right to exert full control over a file, and to modify access control and audit rules. This value represents the right to do anything with a file and is the combination of all rights in this enumeration.</summary>
    FullControl = ReadWriteExecute | TakeOwnership | ChangePermissions | ReadPermissions | Delete | CopyOnWrite, // 0x000F000F
    /// <summary>The right to get or set permissions on a file.</summary>
    AccessSystemSecurity = 16777216, // 0x01000000
  }
}
