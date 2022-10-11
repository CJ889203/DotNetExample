// Decompiled with JetBrains decompiler
// Type: System.IO.Pipes.PipeAccessRights
// Assembly: System.IO.Pipes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3B724542-391C-4574-8865-E11D77EDA2E9
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Pipes.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Pipes.AccessControl.xml

namespace System.IO.Pipes
{
  /// <summary>Defines the access rights to use when you create access and audit rules.</summary>
  [Flags]
  public enum PipeAccessRights
  {
    /// <summary>Specifies the right to read data from the pipe. This does not include the right to read file system attributes, extended file system attributes, or access and audit rules.</summary>
    ReadData = 1,
    /// <summary>Specifies the right to write data to a pipe. This does not include the right to write file system attributes or extended file system attributes.</summary>
    WriteData = 2,
    /// <summary>Specifies the right to read file system attributes from a pipe. This does not include the right to read data, extended file system attributes, or access and audit rules.</summary>
    ReadAttributes = 128, // 0x00000080
    /// <summary>Specifies the right to write file system attributes to a pipe. This does not include the right to write data or extended file system attributes.</summary>
    WriteAttributes = 256, // 0x00000100
    /// <summary>Specifies the right to read extended file system attributes from a pipe. This does not include the right to read data, file system attributes, or access and audit rules.</summary>
    ReadExtendedAttributes = 8,
    /// <summary>Specifies the right to write extended file system attributes to a pipe. This does not include the right to write file attributes or data.</summary>
    WriteExtendedAttributes = 16, // 0x00000010
    /// <summary>Specifies the right to create a new pipe. Setting this right also sets the <see cref="F:System.IO.Pipes.PipeAccessRights.Synchronize" /> right.</summary>
    CreateNewInstance = 4,
    /// <summary>Specifies the right to delete a pipe.</summary>
    Delete = 65536, // 0x00010000
    /// <summary>Specifies the right to read access and audit rules from the pipe. This does not include the right to read data, file system attributes, or extended file system attributes.</summary>
    ReadPermissions = 131072, // 0x00020000
    /// <summary>Specifies the right to change the security and audit rules that are associated with a pipe.</summary>
    ChangePermissions = 262144, // 0x00040000
    /// <summary>Specifies the right to change the owner of a pipe. Note that owners of a pipe have full access to that resource.</summary>
    TakeOwnership = 524288, // 0x00080000
    /// <summary>Specifies whether the application can wait for a pipe handle to synchronize with the completion of an I/O operation. This value is automatically set when allowing access to the pipe and automatically excluded when denying access to the pipe. The right to create a pipe requires this value. Note that if you do not explicitly set this value when you create a pipe, the value will be set automatically for you.</summary>
    Synchronize = 1048576, // 0x00100000
    /// <summary>Specifies the right to exert full control over a pipe, and to modify access control and audit rules. This value represents the combination of all rights in this enumeration.</summary>
    FullControl = Synchronize | TakeOwnership | ChangePermissions | ReadPermissions | Delete | CreateNewInstance | WriteExtendedAttributes | ReadExtendedAttributes | WriteAttributes | ReadAttributes | WriteData | ReadData, // 0x001F019F
    /// <summary>Specifies the right to read from the pipe. This right includes the <see cref="F:System.IO.Pipes.PipeAccessRights.ReadAttributes" />, <see cref="F:System.IO.Pipes.PipeAccessRights.ReadData" />, <see cref="F:System.IO.Pipes.PipeAccessRights.ReadExtendedAttributes" />, and <see cref="F:System.IO.Pipes.PipeAccessRights.ReadPermissions" /> rights.</summary>
    Read = ReadPermissions | ReadExtendedAttributes | ReadAttributes | ReadData, // 0x00020089
    /// <summary>Specifies the right to write to the pipe. This right includes the <see cref="F:System.IO.Pipes.PipeAccessRights.WriteAttributes" />, <see cref="F:System.IO.Pipes.PipeAccessRights.WriteData" />, and <see cref="F:System.IO.Pipes.PipeAccessRights.WriteExtendedAttributes" /> rights.</summary>
    Write = WriteExtendedAttributes | WriteAttributes | WriteData, // 0x00000112
    /// <summary>Specifies the right to read and write from the pipe. This right includes the <see cref="F:System.IO.Pipes.PipeAccessRights.ReadAttributes" />, <see cref="F:System.IO.Pipes.PipeAccessRights.ReadData" />, <see cref="F:System.IO.Pipes.PipeAccessRights.ReadExtendedAttributes" />, <see cref="F:System.IO.Pipes.PipeAccessRights.ReadPermissions" />, <see cref="F:System.IO.Pipes.PipeAccessRights.WriteAttributes" />, <see cref="F:System.IO.Pipes.PipeAccessRights.WriteData" />, and <see cref="F:System.IO.Pipes.PipeAccessRights.WriteExtendedAttributes" /> rights.</summary>
    ReadWrite = Write | Read, // 0x0002019B
    /// <summary>Specifies the right to make changes to the system access control list (SACL).</summary>
    AccessSystemSecurity = 16777216, // 0x01000000
  }
}
