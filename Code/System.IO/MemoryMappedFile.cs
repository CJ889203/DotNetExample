// Decompiled with JetBrains decompiler
// Type: System.IO.MemoryMappedFiles.MemoryMappedFile
// Assembly: System.IO.MemoryMappedFiles, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: ADB8E953-9D00-4DED-81B8-A4FE54270273
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.MemoryMappedFiles.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.MemoryMappedFiles.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;


#nullable enable
namespace System.IO.MemoryMappedFiles
{
  /// <summary>Represents a memory-mapped file.</summary>
  public class MemoryMappedFile : IDisposable
  {

    #nullable disable
    private readonly SafeMemoryMappedFileHandle _handle;
    private readonly bool _leaveOpen;
    private readonly FileStream _fileStream;

    private MemoryMappedFile(SafeMemoryMappedFileHandle handle)
    {
      this._handle = handle;
      this._leaveOpen = true;
    }

    private MemoryMappedFile(
      SafeMemoryMappedFileHandle handle,
      FileStream fileStream,
      bool leaveOpen)
    {
      this._handle = handle;
      this._fileStream = fileStream;
      this._leaveOpen = leaveOpen;
    }


    #nullable enable
    /// <summary>Opens an existing memory-mapped file that has the specified name in system memory.</summary>
    /// <param name="mapName">The name of the memory-mapped file.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="mapName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mapName" /> is an empty string.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified for <paramref name="mapName" /> does not exist.</exception>
    /// <returns>A memory-mapped file that has the specified name.</returns>
    [SupportedOSPlatform("windows")]
    public static MemoryMappedFile OpenExisting(string mapName) => MemoryMappedFile.OpenExisting(mapName, MemoryMappedFileRights.ReadWrite, HandleInheritability.None);

    /// <summary>Opens an existing memory-mapped file that has the specified name and access rights in system memory.</summary>
    /// <param name="mapName">The name of the memory-mapped file to open.</param>
    /// <param name="desiredAccessRights">One of the enumeration values that specifies the access rights to apply to the memory-mapped file.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="mapName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mapName" /> is an empty string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="desiredAccessRights" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileRights" /> enumeration value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified for <paramref name="mapName" /> does not exist.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    [SupportedOSPlatform("windows")]
    public static MemoryMappedFile OpenExisting(
      string mapName,
      MemoryMappedFileRights desiredAccessRights)
    {
      return MemoryMappedFile.OpenExisting(mapName, desiredAccessRights, HandleInheritability.None);
    }

    /// <summary>Opens an existing memory-mapped file that has the specified name, access rights, and inheritability in system memory.</summary>
    /// <param name="mapName">The name of the memory-mapped file to open.</param>
    /// <param name="desiredAccessRights">One of the enumeration values that specifies the access rights to apply to the memory-mapped file.</param>
    /// <param name="inheritability">One of the enumeration values that specifies whether a handle to the memory-mapped file can be inherited by a child process. The default is <see cref="F:System.IO.HandleInheritability.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="mapName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mapName" /> is an empty string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="desiredAccessRights" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileRights" /> enumeration value.
    /// 
    /// -or-
    /// 
    /// <paramref name="inheritability" /> is not a valid <see cref="T:System.IO.HandleInheritability" /> enumeration value.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The requested access is invalid for the memory-mapped file.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file specified for <paramref name="mapName" /> does not exist.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    [SupportedOSPlatform("windows")]
    public static MemoryMappedFile OpenExisting(
      string mapName,
      MemoryMappedFileRights desiredAccessRights,
      HandleInheritability inheritability)
    {
      switch (mapName)
      {
        case "":
          throw new ArgumentException(SR.Argument_MapNameEmptyString);
        case null:
          throw new ArgumentNullException(nameof (mapName), SR.ArgumentNull_MapName);
        default:
          if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
            throw new ArgumentOutOfRangeException(nameof (inheritability));
          if ((desiredAccessRights & ~(MemoryMappedFileRights.FullControl | MemoryMappedFileRights.AccessSystemSecurity)) != (MemoryMappedFileRights) 0)
            throw new ArgumentOutOfRangeException(nameof (desiredAccessRights));
          return new MemoryMappedFile(MemoryMappedFile.OpenCore(mapName, inheritability, desiredAccessRights, false));
      }
    }

    /// <summary>Creates a memory-mapped file from a file on disk.</summary>
    /// <param name="path">The path to file to map.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is an empty string, contains only white space, or has one or more invalid characters, as defined by the <see cref="M:System.IO.Path.GetInvalidFileNameChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to an invalid device.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the maximum length defined by the operating system.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions for the file.</exception>
    /// <returns>A memory-mapped file.</returns>
    public static MemoryMappedFile CreateFromFile(string path) => MemoryMappedFile.CreateFromFile(path, FileMode.Open, (string) null, 0L, MemoryMappedFileAccess.ReadWrite);

    /// <summary>Creates a memory-mapped file that has the specified access mode from a file on disk.</summary>
    /// <param name="path">The path to the file to map.</param>
    /// <param name="mode">Access mode; must be <see cref="F:System.IO.FileMode.Open" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is an empty string, contains only white space, or has one or more invalid characters, as defined by the <see cref="M:System.IO.Path.GetInvalidFileNameChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to an invalid device.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Append" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Create" />, <see cref="F:System.IO.FileMode.CreateNew" />, or <see cref="F:System.IO.FileMode.Truncate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.OpenOrCreate" /> and the file on disk does not exist.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the maximum length defined by the operating system.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions for the file.</exception>
    /// <returns>A memory-mapped file that has the specified access mode.</returns>
    public static MemoryMappedFile CreateFromFile(string path, FileMode mode) => MemoryMappedFile.CreateFromFile(path, mode, (string) null, 0L, MemoryMappedFileAccess.ReadWrite);

    /// <summary>Creates a memory-mapped file that has the specified access mode and name from a file on disk.</summary>
    /// <param name="path">The path to the file to map.</param>
    /// <param name="mode">Access mode; must be <see cref="F:System.IO.FileMode.Open" />.</param>
    /// <param name="mapName">A name to assign to the memory-mapped file, or <see langword="null" /> for a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> that you do not intend to share across processes.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is an empty string, contains only white space, or has one or more invalid characters, as defined by the <see cref="M:System.IO.Path.GetInvalidFileNameChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to an invalid device.
    /// 
    /// -or-
    /// 
    /// <paramref name="mapName" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Append" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Create" />, <see cref="F:System.IO.FileMode.CreateNew" />, or <see cref="F:System.IO.FileMode.Truncate" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.OpenOrCreate" /> and the file on disk does not exist.
    /// 
    /// -or-
    /// 
    /// An I/O error occurred.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the maximum length defined by the operating system.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions for the file.</exception>
    /// <returns>A memory-mapped file that has the specified name and access mode.</returns>
    public static MemoryMappedFile CreateFromFile(
      string path,
      FileMode mode,
      string? mapName)
    {
      return MemoryMappedFile.CreateFromFile(path, mode, mapName, 0L, MemoryMappedFileAccess.ReadWrite);
    }

    /// <summary>Creates a memory-mapped file that has the specified access mode, name, and capacity from a file on disk.</summary>
    /// <param name="path">The path to the file to map.</param>
    /// <param name="mode">Access mode; can be any of the <see cref="T:System.IO.FileMode" /> enumeration values except <see cref="F:System.IO.FileMode.Append" />.</param>
    /// <param name="mapName">A name to assign to the memory-mapped file, or <see langword="null" /> for a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> that you do not intend to share across processes.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file. Specify 0 to set the capacity to the size of the file on disk.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="path" /> is an empty string, contains only white space, or has one or more invalid characters, as defined by the <see cref="M:System.IO.Path.GetInvalidFileNameChars" /> method.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to an invalid device.
    /// 
    /// -or-
    /// 
    /// <paramref name="mapName" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Append" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="capacity" /> is greater than the size of the logical address space.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is less than the file size (but not zero).
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is zero, and the size of the file on disk is also zero.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the maximum length defined by the operating system.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions for the file.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    public static MemoryMappedFile CreateFromFile(
      string path,
      FileMode mode,
      string? mapName,
      long capacity)
    {
      return MemoryMappedFile.CreateFromFile(path, mode, mapName, capacity, MemoryMappedFileAccess.ReadWrite);
    }

    /// <summary>Creates a memory-mapped file that has the specified access mode, name, capacity, and access type from a file on disk.</summary>
    /// <param name="path">The path to the file to map.</param>
    /// <param name="mode">Access mode; can be any of the <see cref="T:System.IO.FileMode" /> enumeration values except <see cref="F:System.IO.FileMode.Append" />.</param>
    /// <param name="mapName">A name to assign to the memory-mapped file, or <see langword="null" /> for a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> that you do not intend to share across processes.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file. Specify 0 to set the capacity to the size of the file on disk.</param>
    /// <param name="access">One of the enumeration values that specifies the type of access allowed to the memory-mapped file.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mapName" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not an allowed value.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> specifies an empty file.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is specified as <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read" /> and capacity is greater than the size of the file indicated by <paramref name="path" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.FileMode.Append" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="capacity" /> is greater than the size of the logical address space.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is less than the file size (but not zero).
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is zero, and the size of the file on disk is also zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a defined <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> value.
    /// 
    /// -or-
    /// 
    /// The size of the file indicated by <paramref name="path" /> is greater than <paramref name="capacity" />.</exception>
    /// <exception cref="T:System.IO.IOException">-or-
    /// 
    /// An I/O error occurred.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">
    /// <paramref name="path" /> exceeds the maximum length defined by the operating system.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions for the file.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    public static MemoryMappedFile CreateFromFile(
      string path,
      FileMode mode,
      string? mapName,
      long capacity,
      MemoryMappedFileAccess access)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      switch (mapName)
      {
        case "":
          throw new ArgumentException(SR.Argument_MapNameEmptyString);
        default:
          if (capacity < 0L)
            throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_PositiveOrDefaultCapacityRequired);
          if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
            throw new ArgumentOutOfRangeException(nameof (access));
          if (mode == FileMode.Append)
            throw new ArgumentException(SR.Argument_NewMMFAppendModeNotAllowed, nameof (mode));
          if (mode == FileMode.Truncate)
            throw new ArgumentException(SR.Argument_NewMMFTruncateModeNotAllowed, nameof (mode));
          if (access == MemoryMappedFileAccess.Write)
            throw new ArgumentException(SR.Argument_NewMMFWriteAccessNotAllowed, nameof (access));
          bool existed = File.Exists(path);
          FileStream fileStream = new FileStream(path, mode, MemoryMappedFile.GetFileAccess(access), FileShare.Read, 4096, FileOptions.None);
          if (capacity == 0L && fileStream.Length == 0L)
          {
            MemoryMappedFile.CleanupFile(fileStream, existed, path);
            throw new ArgumentException(SR.Argument_EmptyFile);
          }
          if (capacity == 0L)
            capacity = fileStream.Length;
          SafeMemoryMappedFileHandle core;
          try
          {
            core = MemoryMappedFile.CreateCore(fileStream, mapName, HandleInheritability.None, access, MemoryMappedFileOptions.None, capacity);
          }
          catch
          {
            MemoryMappedFile.CleanupFile(fileStream, existed, path);
            throw;
          }
          return new MemoryMappedFile(core, fileStream, false);
      }
    }

    /// <summary>Creates a memory-mapped file from an existing file with the specified access mode, name, inheritability, and capacity.</summary>
    /// <param name="fileStream">The file stream of the existing file.</param>
    /// <param name="mapName">A name to assign to the memory-mapped file, or <see langword="null" /> for a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> that you do not intend to share across processes.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file. Specify 0 to set the capacity to the size of <c>filestream</c>.</param>
    /// <param name="access">One of the enumeration values that specifies the type of access allowed to the memory-mapped file.
    /// 
    /// This parameter can't be set to <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" />.</param>
    /// <param name="inheritability">One of the enumeration values that specifies whether a handle to the memory-mapped file can be inherited by a child process. The default is <see cref="F:System.IO.HandleInheritability.None" />.</param>
    /// <param name="leaveOpen">A value that indicates whether to close the source file stream when the <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> is disposed.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mapName" /> is <see langword="null" /> or an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> and the length of the file are zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is set to <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" /> or <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" /> enumeration value, which is not allowed.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is set to <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Read" /> and <paramref name="capacity" /> is larger than the length of <see langword="filestream" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="fileStream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="capacity" /> is less than zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is less than the file size.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.
    /// 
    /// -or-
    /// 
    /// <paramref name="inheritability" /> is not a valid <see cref="T:System.IO.HandleInheritability" /> enumeration value.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    public static MemoryMappedFile CreateFromFile(
      FileStream fileStream,
      string? mapName,
      long capacity,
      MemoryMappedFileAccess access,
      HandleInheritability inheritability,
      bool leaveOpen)
    {
      if (fileStream == null)
        throw new ArgumentNullException(nameof (fileStream), SR.ArgumentNull_FileStream);
      switch (mapName)
      {
        case "":
          throw new ArgumentException(SR.Argument_MapNameEmptyString);
        default:
          if (capacity < 0L)
            throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_PositiveOrDefaultCapacityRequired);
          if (capacity == 0L && fileStream.Length == 0L)
            throw new ArgumentException(SR.Argument_EmptyFile);
          if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
            throw new ArgumentOutOfRangeException(nameof (access));
          if (access == MemoryMappedFileAccess.Write)
            throw new ArgumentException(SR.Argument_NewMMFWriteAccessNotAllowed, nameof (access));
          if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
            throw new ArgumentOutOfRangeException(nameof (inheritability));
          fileStream.Flush();
          if (capacity == 0L)
            capacity = fileStream.Length;
          return new MemoryMappedFile(MemoryMappedFile.CreateCore(fileStream, mapName, inheritability, access, MemoryMappedFileOptions.None, capacity), fileStream, leaveOpen);
      }
    }

    /// <summary>Creates a memory-mapped file that has the specified capacity in system memory.</summary>
    /// <param name="mapName">A name to assign to the memory-mapped file, or <see langword="null" /> for a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> that you do not intend to share across processes.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mapName" /> is an empty string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than or equal to zero.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: Calls to the <c>CreateNew</c> method with a named memory mapped file (that is, a non-null <paramref name="mapName" />) are supported on Windows operating systems only.</exception>
    /// <returns>A memory-mapped file that has the specified name and capacity.</returns>
    public static MemoryMappedFile CreateNew(string? mapName, long capacity) => MemoryMappedFile.CreateNew(mapName, capacity, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, HandleInheritability.None);

    /// <summary>Creates a memory-mapped file that has the specified capacity and access type in system memory.</summary>
    /// <param name="mapName">A name to assign to the memory-mapped file, or <see langword="null" /> for a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> that you do not intend to share across processes.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file.</param>
    /// <param name="access">One of the enumeration values that specifies the type of access allowed to the memory-mapped file. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mapName" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is set to write-only with the <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" /> enumeration value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="capacity" /> is less than or equal to zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: Calls to the <c>CreateNew</c> method with a named memory mapped file (that is, a non-null <paramref name="mapName" />) are supported on Windows operating systems only.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    public static MemoryMappedFile CreateNew(
      string? mapName,
      long capacity,
      MemoryMappedFileAccess access)
    {
      return MemoryMappedFile.CreateNew(mapName, capacity, access, MemoryMappedFileOptions.None, HandleInheritability.None);
    }

    /// <summary>Creates a memory-mapped file that has the specified name, capacity, access type, memory allocation options and inheritability.</summary>
    /// <param name="mapName">A name to assign to the memory-mapped file, or <see langword="null" /> for a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> that you do not intend to share across processes.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file.</param>
    /// <param name="access">One of the enumeration values that specifies the type of access allowed to the memory-mapped file. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
    /// <param name="options">A bitwise combination of enumeration values that specifies memory allocation options for the memory-mapped file.</param>
    /// <param name="inheritability">A value that specifies whether a handle to the memory-mapped file can be inherited by a child process. The default is <see cref="F:System.IO.HandleInheritability.None" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mapName" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is set to write-only with the <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" /> enumeration value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="capacity" /> is less than or equal to zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.
    /// 
    /// -or-
    /// 
    /// <paramref name="inheritability" /> is not a valid <see cref="T:System.IO.HandleInheritability" /> value.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">.NET Core and .NET 5+ only: Calls to the <c>CreateNew</c> method with a named memory mapped file (that is, a non-null <paramref name="mapName" />) are supported on Windows operating systems only.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    public static MemoryMappedFile CreateNew(
      string? mapName,
      long capacity,
      MemoryMappedFileAccess access,
      MemoryMappedFileOptions options,
      HandleInheritability inheritability)
    {
      switch (mapName)
      {
        case "":
          throw new ArgumentException(SR.Argument_MapNameEmptyString);
        default:
          if (capacity <= 0L)
            throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_NeedPositiveNumber);
          if (IntPtr.Size == 4 && capacity > (long) uint.MaxValue)
            throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed);
          if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
            throw new ArgumentOutOfRangeException(nameof (access));
          if (access == MemoryMappedFileAccess.Write)
            throw new ArgumentException(SR.Argument_NewMMFWriteAccessNotAllowed, nameof (access));
          if ((options & ~MemoryMappedFileOptions.DelayAllocatePages) != MemoryMappedFileOptions.None)
            throw new ArgumentOutOfRangeException(nameof (options));
          if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
            throw new ArgumentOutOfRangeException(nameof (inheritability));
          return new MemoryMappedFile(MemoryMappedFile.CreateCore((FileStream) null, mapName, inheritability, access, options, capacity));
      }
    }

    /// <summary>Creates or opens a memory-mapped file that has the specified name and capacity in system memory.</summary>
    /// <param name="mapName">The name of the memory-mapped file.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mapName" /> is an empty string.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="capacity" /> is greater than the size of the logical address space.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is less than or equal to zero.</exception>
    /// <returns>A memory-mapped file that has the specified name and size.</returns>
    [SupportedOSPlatform("windows")]
    public static MemoryMappedFile CreateOrOpen(string mapName, long capacity) => MemoryMappedFile.CreateOrOpen(mapName, capacity, MemoryMappedFileAccess.ReadWrite, MemoryMappedFileOptions.None, HandleInheritability.None);

    /// <summary>Creates or opens a memory-mapped file that has the specified name, capacity and access type in system memory.</summary>
    /// <param name="mapName">The name of the memory-mapped file.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file.</param>
    /// <param name="access">One of the enumeration values that specifies the type of access allowed to the memory-mapped file. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="mapName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mapName" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is set to write-only with the <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" /> enumeration value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="capacity" /> is greater than the size of the logical address space.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is less than or equal to zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The operating system denied the specified access to the file; for example, access is set to <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" /> or <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />, but the file or directory is read-only.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    [SupportedOSPlatform("windows")]
    public static MemoryMappedFile CreateOrOpen(
      string mapName,
      long capacity,
      MemoryMappedFileAccess access)
    {
      return MemoryMappedFile.CreateOrOpen(mapName, capacity, access, MemoryMappedFileOptions.None, HandleInheritability.None);
    }

    /// <summary>Creates a new empty memory mapped file or opens an existing memory mapped file if one exists with the same name. If opening an existing file, the capacity, options, and memory arguments will be ignored.</summary>
    /// <param name="mapName">The name of the memory-mapped file.</param>
    /// <param name="capacity">The maximum size, in bytes, to allocate to the memory-mapped file.</param>
    /// <param name="access">One of the enumeration values that specifies the type of access allowed to the memory-mapped file. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
    /// <param name="options">A bitwise combination of values that indicate the memory allocation options to apply to the file.</param>
    /// <param name="inheritability">A value that specifies whether a handle to the memory-mapped file can be inherited by a child process. The default is <see cref="F:System.IO.HandleInheritability.None" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="mapName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="mapName" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is set to write-only with the <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" /> enumeration value.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="capacity" /> is greater than the size of the logical address space.
    /// 
    /// -or-
    /// 
    /// <paramref name="capacity" /> is less than or equal to zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.
    /// 
    /// -or-
    /// 
    /// <paramref name="inheritability" /> is not a valid <see cref="T:System.IO.HandleInheritability" /> enumeration value.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The operating system denied the specified access to the file; for example, access is set to <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.Write" /> or <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />, but the file or directory is read-only.</exception>
    /// <returns>A memory-mapped file that has the specified characteristics.</returns>
    [SupportedOSPlatform("windows")]
    public static MemoryMappedFile CreateOrOpen(
      string mapName,
      long capacity,
      MemoryMappedFileAccess access,
      MemoryMappedFileOptions options,
      HandleInheritability inheritability)
    {
      switch (mapName)
      {
        case "":
          throw new ArgumentException(SR.Argument_MapNameEmptyString);
        case null:
          throw new ArgumentNullException(nameof (mapName), SR.ArgumentNull_MapName);
        default:
          if (capacity <= 0L)
            throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_NeedPositiveNumber);
          if (IntPtr.Size == 4 && capacity > (long) uint.MaxValue)
            throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed);
          if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
            throw new ArgumentOutOfRangeException(nameof (access));
          if ((options & ~MemoryMappedFileOptions.DelayAllocatePages) != MemoryMappedFileOptions.None)
            throw new ArgumentOutOfRangeException(nameof (options));
          if (inheritability < HandleInheritability.None || inheritability > HandleInheritability.Inheritable)
            throw new ArgumentOutOfRangeException(nameof (inheritability));
          return new MemoryMappedFile(access != MemoryMappedFileAccess.Write ? MemoryMappedFile.CreateOrOpenCore(mapName, inheritability, access, options, capacity) : MemoryMappedFile.OpenCore(mapName, inheritability, access, true));
      }
    }

    /// <summary>Creates a stream that maps to a view of the memory-mapped file.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the memory-mapped file is unauthorized.</exception>
    /// <returns>A stream of memory.</returns>
    public MemoryMappedViewStream CreateViewStream() => this.CreateViewStream(0L, 0L, MemoryMappedFileAccess.ReadWrite);

    /// <summary>Creates a stream that maps to a view of the memory-mapped file, and that has the specified offset and size.</summary>
    /// <param name="offset">The byte at which to start the view.</param>
    /// <param name="size">The size of the view. Specify 0 (zero) to create a view that starts at <paramref name="offset" /> and ends approximately at the end of the memory-mapped file.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="size" /> is a negative value.
    /// 
    /// -or-
    /// 
    /// <paramref name="size" /> is greater than the logical address space.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the memory-mapped file is unauthorized.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="size" /> is greater than the total virtual memory.</exception>
    /// <returns>A stream of memory that has the specified offset and size.</returns>
    public MemoryMappedViewStream CreateViewStream(long offset, long size) => this.CreateViewStream(offset, size, MemoryMappedFileAccess.ReadWrite);

    /// <summary>Creates a stream that maps to a view of the memory-mapped file, and that has the specified offset, size, and access type.</summary>
    /// <param name="offset">The byte at which to start the view.</param>
    /// <param name="size">The size of the view. Specify 0 (zero) to create a view that starts at <paramref name="offset" /> and ends approximately at the end of the memory-mapped file.</param>
    /// <param name="access">One of the enumeration values that specifies the type of access allowed to the memory-mapped file. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="size" /> is a negative value.
    /// 
    /// -or-
    /// 
    /// <paramref name="size" /> is greater than the logical address space.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is not a valid <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFileAccess" /> enumeration value.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="access" /> is invalid for the memory-mapped file.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///        <paramref name="size" /> is greater than the total virtual memory.
    /// 
    /// -or-
    /// 
    /// <paramref name="access" /> is invalid for the memory-mapped file.</exception>
    /// <returns>A stream of memory that has the specified characteristics.</returns>
    public MemoryMappedViewStream CreateViewStream(
      long offset,
      long size,
      MemoryMappedFileAccess access)
    {
      if (offset < 0L)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (size < 0L)
        throw new ArgumentOutOfRangeException(nameof (size), SR.ArgumentOutOfRange_PositiveOrDefaultSizeRequired);
      if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
        throw new ArgumentOutOfRangeException(nameof (access));
      if (IntPtr.Size == 4 && size > (long) uint.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (size), SR.ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed);
      return new MemoryMappedViewStream(MemoryMappedView.CreateView(this._handle, access, offset, size));
    }

    /// <summary>Creates a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedViewAccessor" /> that maps to a view of the memory-mapped file.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the memory-mapped file is unauthorized.</exception>
    /// <returns>A randomly accessible block of memory.</returns>
    public MemoryMappedViewAccessor CreateViewAccessor() => this.CreateViewAccessor(0L, 0L, MemoryMappedFileAccess.ReadWrite);

    /// <summary>Creates a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedViewAccessor" /> that maps to a view of the memory-mapped file, and that has the specified offset and size.</summary>
    /// <param name="offset">The byte at which to start the view.</param>
    /// <param name="size">The size of the view. Specify 0 (zero) to create a view that starts at <paramref name="offset" /> and ends approximately at the end of the memory-mapped file.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="size" /> is a negative value.
    /// 
    /// -or-
    /// 
    /// <paramref name="size" /> is greater than the logical address space.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the memory-mapped file is unauthorized.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A randomly accessible block of memory.</returns>
    public MemoryMappedViewAccessor CreateViewAccessor(
      long offset,
      long size)
    {
      return this.CreateViewAccessor(offset, size, MemoryMappedFileAccess.ReadWrite);
    }

    /// <summary>Creates a <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedViewAccessor" /> that maps to a view of the memory-mapped file, and that has the specified offset, size, and access restrictions.</summary>
    /// <param name="offset">The byte at which to start the view.</param>
    /// <param name="size">The size of the view. Specify 0 (zero) to create a view that starts at <paramref name="offset" /> and ends approximately at the end of the memory-mapped file.</param>
    /// <param name="access">One of the enumeration values that specifies the type of access allowed to the memory-mapped file. The default is <see cref="F:System.IO.MemoryMappedFiles.MemoryMappedFileAccess.ReadWrite" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="offset" /> or <paramref name="size" /> is a negative value.
    /// 
    /// -or-
    /// 
    /// <paramref name="size" /> is greater than the logical address space.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="access" /> is invalid for the memory-mapped file.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>A randomly accessible block of memory.</returns>
    public MemoryMappedViewAccessor CreateViewAccessor(
      long offset,
      long size,
      MemoryMappedFileAccess access)
    {
      if (offset < 0L)
        throw new ArgumentOutOfRangeException(nameof (offset), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (size < 0L)
        throw new ArgumentOutOfRangeException(nameof (size), SR.ArgumentOutOfRange_PositiveOrDefaultSizeRequired);
      if (access < MemoryMappedFileAccess.ReadWrite || access > MemoryMappedFileAccess.ReadWriteExecute)
        throw new ArgumentOutOfRangeException(nameof (access));
      if (IntPtr.Size == 4 && size > (long) uint.MaxValue)
        throw new ArgumentOutOfRangeException(nameof (size), SR.ArgumentOutOfRange_CapacityLargerThanLogicalAddressSpaceNotAllowed);
      return new MemoryMappedViewAccessor(MemoryMappedView.CreateView(this._handle, access, offset, size));
    }

    /// <summary>Releases all resources used by the <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" />.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.MemoryMappedFiles.MemoryMappedFile" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      try
      {
        if (this._handle.IsClosed)
          return;
        this._handle.Dispose();
      }
      finally
      {
        if (this._fileStream != null && !this._leaveOpen)
          this._fileStream.Dispose();
      }
    }

    /// <summary>Gets the file handle of a memory-mapped file.</summary>
    /// <returns>The handle to the memory-mapped file.</returns>
    public SafeMemoryMappedFileHandle SafeMemoryMappedFileHandle => this._handle;

    internal static FileAccess GetFileAccess(MemoryMappedFileAccess access)
    {
      switch (access)
      {
        case MemoryMappedFileAccess.ReadWrite:
        case MemoryMappedFileAccess.CopyOnWrite:
        case MemoryMappedFileAccess.ReadWriteExecute:
          return FileAccess.ReadWrite;
        case MemoryMappedFileAccess.Read:
        case MemoryMappedFileAccess.ReadExecute:
          return FileAccess.Read;
        default:
          return FileAccess.Write;
      }
    }


    #nullable disable
    private static void CleanupFile(FileStream fileStream, bool existed, string path)
    {
      fileStream.Dispose();
      if (existed)
        return;
      File.Delete(path);
    }

    private static void VerifyMemoryMappedFileAccess(
      MemoryMappedFileAccess access,
      long capacity,
      FileStream fileStream)
    {
      if (access == MemoryMappedFileAccess.Read && capacity > fileStream.Length)
        throw new ArgumentException(SR.Argument_ReadAccessWithLargeCapacity);
      if (fileStream.Length > capacity)
        throw new ArgumentOutOfRangeException(nameof (capacity), SR.ArgumentOutOfRange_CapacityGEFileSizeRequired);
    }

    private static SafeMemoryMappedFileHandle CreateCore(
      FileStream fileStream,
      string mapName,
      HandleInheritability inheritability,
      MemoryMappedFileAccess access,
      MemoryMappedFileOptions options,
      long capacity)
    {
      SafeFileHandle safeFileHandle = fileStream?.SafeFileHandle;
      Interop.Kernel32.SECURITY_ATTRIBUTES secAttrs = MemoryMappedFile.GetSecAttrs(inheritability);
      if (fileStream != null)
        MemoryMappedFile.VerifyMemoryMappedFileAccess(access, capacity, fileStream);
      SafeMemoryMappedFileHandle core = safeFileHandle != null ? Interop.CreateFileMapping(safeFileHandle, ref secAttrs, (int) ((MemoryMappedFileOptions) MemoryMappedFile.GetPageAccess(access) | options), capacity, mapName) : Interop.CreateFileMapping(new IntPtr(-1), ref secAttrs, (int) ((MemoryMappedFileOptions) MemoryMappedFile.GetPageAccess(access) | options), capacity, mapName);
      int lastPinvokeError = Marshal.GetLastPInvokeError();
      if (!core.IsInvalid)
      {
        if (lastPinvokeError == 183)
        {
          core.Dispose();
          throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError);
        }
        return core;
      }
      core.Dispose();
      throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError);
    }

    private static SafeMemoryMappedFileHandle OpenCore(
      string mapName,
      HandleInheritability inheritability,
      MemoryMappedFileAccess access,
      bool createOrOpen)
    {
      return MemoryMappedFile.OpenCore(mapName, inheritability, MemoryMappedFile.GetFileMapAccess(access), createOrOpen);
    }

    private static SafeMemoryMappedFileHandle OpenCore(
      string mapName,
      HandleInheritability inheritability,
      MemoryMappedFileRights rights,
      bool createOrOpen)
    {
      return MemoryMappedFile.OpenCore(mapName, inheritability, MemoryMappedFile.GetFileMapAccess(rights), createOrOpen);
    }

    private static SafeMemoryMappedFileHandle CreateOrOpenCore(
      string mapName,
      HandleInheritability inheritability,
      MemoryMappedFileAccess access,
      MemoryMappedFileOptions options,
      long capacity)
    {
      SafeMemoryMappedFileHandle mappedFileHandle = (SafeMemoryMappedFileHandle) null;
      Interop.Kernel32.SECURITY_ATTRIBUTES secAttrs = MemoryMappedFile.GetSecAttrs(inheritability);
      int num = 14;
      int millisecondsTimeout = 0;
      while (num > 0)
      {
        mappedFileHandle = Interop.CreateFileMapping(new IntPtr(-1), ref secAttrs, (int) ((MemoryMappedFileOptions) MemoryMappedFile.GetPageAccess(access) | options), capacity, mapName);
        if (mappedFileHandle.IsInvalid)
        {
          mappedFileHandle.Dispose();
          int lastPinvokeError1 = Marshal.GetLastPInvokeError();
          if (lastPinvokeError1 != 5)
            throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError1);
          mappedFileHandle = Interop.OpenFileMapping(MemoryMappedFile.GetFileMapAccess(access), (inheritability & HandleInheritability.Inheritable) != 0, mapName);
          if (mappedFileHandle.IsInvalid)
          {
            mappedFileHandle.Dispose();
            int lastPinvokeError2 = Marshal.GetLastPInvokeError();
            if (lastPinvokeError2 != 2)
              throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError2);
            --num;
            if (millisecondsTimeout == 0)
            {
              millisecondsTimeout = 10;
            }
            else
            {
              Thread.Sleep(millisecondsTimeout);
              millisecondsTimeout *= 2;
            }
          }
          else
            break;
        }
        else
          break;
      }
      return mappedFileHandle != null && !mappedFileHandle.IsInvalid ? mappedFileHandle : throw new InvalidOperationException(SR.InvalidOperation_CantCreateFileMapping);
    }

    private static int GetFileMapAccess(MemoryMappedFileRights rights) => (int) rights;

    internal static int GetFileMapAccess(MemoryMappedFileAccess access)
    {
      switch (access)
      {
        case MemoryMappedFileAccess.ReadWrite:
          return 6;
        case MemoryMappedFileAccess.Read:
          return 4;
        case MemoryMappedFileAccess.Write:
          return 2;
        case MemoryMappedFileAccess.CopyOnWrite:
          return 1;
        case MemoryMappedFileAccess.ReadExecute:
          return 36;
        default:
          return 38;
      }
    }

    internal static int GetPageAccess(MemoryMappedFileAccess access)
    {
      switch (access)
      {
        case MemoryMappedFileAccess.ReadWrite:
          return 4;
        case MemoryMappedFileAccess.Read:
          return 2;
        case MemoryMappedFileAccess.CopyOnWrite:
          return 8;
        case MemoryMappedFileAccess.ReadExecute:
          return 32;
        default:
          return 64;
      }
    }

    private static SafeMemoryMappedFileHandle OpenCore(
      string mapName,
      HandleInheritability inheritability,
      int desiredAccessRights,
      bool createOrOpen)
    {
      SafeMemoryMappedFileHandle mappedFileHandle = Interop.OpenFileMapping(desiredAccessRights, (inheritability & HandleInheritability.Inheritable) != 0, mapName);
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (!mappedFileHandle.IsInvalid)
        return mappedFileHandle;
      mappedFileHandle.Dispose();
      if (createOrOpen && lastWin32Error == 2)
        throw new ArgumentException(SR.Argument_NewMMFWriteAccessNotAllowed, "access");
      throw Win32Marshal.GetExceptionForWin32Error(lastWin32Error);
    }

    private static Interop.Kernel32.SECURITY_ATTRIBUTES GetSecAttrs(
      HandleInheritability inheritability)
    {
      Interop.Kernel32.SECURITY_ATTRIBUTES secAttrs = new Interop.Kernel32.SECURITY_ATTRIBUTES();
      if ((inheritability & HandleInheritability.Inheritable) != HandleInheritability.None)
      {
        secAttrs = new Interop.Kernel32.SECURITY_ATTRIBUTES();
        secAttrs.nLength = (uint) sizeof (Interop.Kernel32.SECURITY_ATTRIBUTES);
        secAttrs.bInheritHandle = Interop.BOOL.TRUE;
      }
      return secAttrs;
    }
  }
}
