// Decompiled with JetBrains decompiler
// Type: System.IO.FileSystemAclExtensions
// Assembly: System.IO.FileSystem.AccessControl, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 58F7D3AE-9598-417F-81CB-0E1C571E9485
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.AccessControl.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.AccessControl.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.AccessControl;


#nullable enable
namespace System.IO
{
  /// <summary>Provides Windows-specific static extension methods for manipulating Access Control List (ACL) security attributes for files and directories.</summary>
  public static class FileSystemAclExtensions
  {
    /// <summary>Returns the security information of a directory.</summary>
    /// <param name="directoryInfo">The existing directory from which to obtain the security information.</param>
    /// <returns>The security descriptors of all the access control sections of the directory.</returns>
    public static DirectorySecurity GetAccessControl(
      this DirectoryInfo directoryInfo)
    {
      return directoryInfo != null ? new DirectorySecurity(directoryInfo.FullName, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group) : throw new ArgumentNullException(nameof (directoryInfo));
    }

    /// <summary>Returns the security information of a directory.</summary>
    /// <param name="directoryInfo">An existing directory from which to obtain the security information.</param>
    /// <param name="includeSections">The desired access control sections to retrieve.</param>
    /// <returns>The security descriptors of the specified access control sections of the directory.</returns>
    public static DirectorySecurity GetAccessControl(
      this DirectoryInfo directoryInfo,
      AccessControlSections includeSections)
    {
      if (directoryInfo == null)
        throw new ArgumentNullException(nameof (directoryInfo));
      return new DirectorySecurity(directoryInfo.FullName, includeSections);
    }

    /// <summary>Changes the security attributes of an existing directory.</summary>
    /// <param name="directoryInfo">An existing directory.</param>
    /// <param name="directorySecurity">The security information to apply to the directory.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="directorySecurity" /> is <see langword="null" />.</exception>
    public static void SetAccessControl(
      this DirectoryInfo directoryInfo,
      DirectorySecurity directorySecurity)
    {
      if (directorySecurity == null)
        throw new ArgumentNullException(nameof (directorySecurity));
      string fullPath = Path.GetFullPath(directoryInfo.FullName);
      directorySecurity.Persist(fullPath);
    }

    /// <summary>Returns the security information of a file.</summary>
    /// <param name="fileInfo">The file from which to obtain the security information.</param>
    /// <returns>The security descriptors of all the access control sections of the file.</returns>
    public static FileSecurity GetAccessControl(this FileInfo fileInfo) => fileInfo != null ? fileInfo.GetAccessControl(AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group) : throw new ArgumentNullException(nameof (fileInfo));

    /// <summary>Returns the security information of a file.</summary>
    /// <param name="fileInfo">An existing file from which to obtain the security information.</param>
    /// <param name="includeSections">The desired access control sections to retrieve from the file.</param>
    /// <returns>The security descriptors of the specified access control sections of the file.</returns>
    public static FileSecurity GetAccessControl(
      this FileInfo fileInfo,
      AccessControlSections includeSections)
    {
      if (fileInfo == null)
        throw new ArgumentNullException(nameof (fileInfo));
      return new FileSecurity(fileInfo.FullName, includeSections);
    }

    /// <summary>Changes the security attributes of an existing file.</summary>
    /// <param name="fileInfo">An existing file.</param>
    /// <param name="fileSecurity">The security information to apply to the file.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="fileSecurity" /> is <see langword="null" />.</exception>
    public static void SetAccessControl(this FileInfo fileInfo, FileSecurity fileSecurity)
    {
      if (fileInfo == null)
        throw new ArgumentNullException(nameof (fileInfo));
      if (fileSecurity == null)
        throw new ArgumentNullException(nameof (fileSecurity));
      string fullPath = Path.GetFullPath(fileInfo.FullName);
      fileSecurity.Persist(fullPath);
    }

    /// <summary>Returns the security information of a file.</summary>
    /// <param name="fileStream">An existing file from which to obtain the security information.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="fileStream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file stream is closed.</exception>
    /// <returns>The security descriptors of all the access control sections of the file.</returns>
    public static FileSecurity GetAccessControl(this FileStream fileStream)
    {
      SafeFileHandle handle = fileStream != null ? fileStream.SafeFileHandle : throw new ArgumentNullException(nameof (fileStream));
      return !handle.IsClosed ? new FileSecurity(handle, AccessControlSections.Access | AccessControlSections.Owner | AccessControlSections.Group) : throw new ObjectDisposedException((string) null, SR.ObjectDisposed_FileClosed);
    }

    /// <summary>Changes the security attributes of an existing file.</summary>
    /// <param name="fileStream">An existing file.</param>
    /// <param name="fileSecurity">The security information to apply to the file.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="fileStream" /> or <paramref name="fileSecurity" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file stream is closed.</exception>
    public static void SetAccessControl(this FileStream fileStream, FileSecurity fileSecurity)
    {
      if (fileStream == null)
        throw new ArgumentNullException(nameof (fileStream));
      if (fileSecurity == null)
        throw new ArgumentNullException(nameof (fileSecurity));
      SafeFileHandle safeFileHandle = fileStream.SafeFileHandle;
      if (safeFileHandle.IsClosed)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_FileClosed);
      fileSecurity.Persist(safeFileHandle, fileStream.Name);
    }

    /// <summary>Creates a new directory, ensuring it is created with the specified directory security. If the directory already exists, nothing is done.</summary>
    /// <param name="directoryInfo">A directory that does not exist yet that will be created by the method.</param>
    /// <param name="directorySecurity">The access control and audit security for the directory.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="directoryInfo" /> or <paramref name="directorySecurity" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Could not find a part of the path.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the path is denied.</exception>
    public static void Create(this DirectoryInfo directoryInfo, DirectorySecurity directorySecurity)
    {
      if (directoryInfo == null)
        throw new ArgumentNullException(nameof (directoryInfo));
      if (directorySecurity == null)
        throw new ArgumentNullException(nameof (directorySecurity));
      FileSystem.CreateDirectory(directoryInfo.FullName, directorySecurity.GetSecurityDescriptorBinaryForm());
    }

    /// <summary>Creates a new file stream, ensuring it is created with the specified properties and security settings.</summary>
    /// <param name="fileInfo">A file that does not exist yet that will be created by the method.</param>
    /// <param name="mode">One of the enumeration values that specifies how the operating system should open a file.</param>
    /// <param name="rights">One of the enumeration values that defines the access rights to use when creating access and audit rules.</param>
    /// <param name="share">One of the enumeration values for controlling the kind of access other file stream objects can have to the same file.</param>
    /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
    /// <param name="options">One of the enumeration values that describes how to create or overwrite the file.</param>
    /// <param name="fileSecurity">An object that determines the access control and audit security for the file.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="rights" /> and <paramref name="mode" /> combination is invalid.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="fileInfo" /> or <paramref name="fileSecurity" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="mode" /> or <paramref name="share" /> are out of their legal enum range.
    /// 
    /// -or-
    /// 
    /// <paramref name="bufferSize" /> is not a positive number.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Could not find a part of the path.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the path is denied.</exception>
    /// <returns>A file stream for the newly created file.</returns>
    public static FileStream Create(
      this FileInfo fileInfo,
      FileMode mode,
      FileSystemRights rights,
      FileShare share,
      int bufferSize,
      FileOptions options,
      FileSecurity fileSecurity)
    {
      if (fileInfo == null)
        throw new ArgumentNullException(nameof (fileInfo));
      if (fileSecurity == null)
        throw new ArgumentNullException(nameof (fileSecurity));
      FileShare fileShare = share & ~FileShare.Inheritable;
      if (mode < FileMode.CreateNew || mode > FileMode.Append)
        throw new ArgumentOutOfRangeException(nameof (mode), SR.ArgumentOutOfRange_Enum);
      if (fileShare < FileShare.None || fileShare > (FileShare.ReadWrite | FileShare.Delete))
        throw new ArgumentOutOfRangeException(nameof (share), SR.ArgumentOutOfRange_Enum);
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), SR.ArgumentOutOfRange_NeedPosNum);
      if ((rights & FileSystemRights.Write) == (FileSystemRights) 0 && (mode == FileMode.Truncate || mode == FileMode.CreateNew || mode == FileMode.Create || mode == FileMode.Append))
        throw new ArgumentException(SR.Format(SR.Argument_InvalidFileModeAndFileSystemRightsCombo, (object) mode, (object) rights));
      SafeFileHandle fileHandle = FileSystemAclExtensions.CreateFileHandle(fileInfo.FullName, mode, rights, share, options, fileSecurity);
      try
      {
        return new FileStream(fileHandle, FileSystemAclExtensions.GetFileStreamFileAccess(rights), bufferSize, (options & FileOptions.Asynchronous) != 0);
      }
      catch
      {
        fileHandle.Dispose();
        throw;
      }
    }

    /// <summary>Creates a directory and returns it, ensuring it is created with the specified directory security. If the directory already exists, the existing directory is returned.</summary>
    /// <param name="directorySecurity">An object that determines the access control and audit security for the directory.</param>
    /// <param name="path">The path of the directory to create.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="directorySecurity" /> or <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is empty.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Could not find a part of the path.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the path is denied.</exception>
    /// <returns>A directory information object representing either a created directory with the provided security properties, or the existing directory.</returns>
    public static DirectoryInfo CreateDirectory(
      this DirectorySecurity directorySecurity,
      string path)
    {
      if (directorySecurity == null)
        throw new ArgumentNullException(nameof (directorySecurity));
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Arg_PathEmpty);
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          DirectoryInfo directoryInfo = new DirectoryInfo(path);
          directoryInfo.Create(directorySecurity);
          return directoryInfo;
      }
    }

    private static FileAccess GetFileStreamFileAccess(FileSystemRights rights)
    {
      FileAccess streamFileAccess = (FileAccess) 0;
      if ((rights & FileSystemRights.ReadData) != (FileSystemRights) 0 || (rights & (FileSystemRights) -2147483648) != (FileSystemRights) 0)
        streamFileAccess = FileAccess.Read;
      if ((rights & FileSystemRights.WriteData) != (FileSystemRights) 0 || (rights & (FileSystemRights) 1073741824) != (FileSystemRights) 0)
        streamFileAccess = streamFileAccess == FileAccess.Read ? FileAccess.ReadWrite : FileAccess.Write;
      return streamFileAccess;
    }


    #nullable disable
    private static unsafe SafeFileHandle CreateFileHandle(
      string fullPath,
      FileMode mode,
      FileSystemRights rights,
      FileShare share,
      FileOptions options,
      FileSecurity security)
    {
      if (mode == FileMode.Append)
        mode = FileMode.OpenOrCreate;
      int dwFlagsAndAttributes = (int) (options | (FileOptions) 1048576 | FileOptions.None);
      SafeFileHandle file;
      fixed (byte* numPtr = security.GetSecurityDescriptorBinaryForm())
      {
        Interop.Kernel32.SECURITY_ATTRIBUTES securityAttributes = new Interop.Kernel32.SECURITY_ATTRIBUTES()
        {
          nLength = (uint) sizeof (Interop.Kernel32.SECURITY_ATTRIBUTES),
          bInheritHandle = (share & FileShare.Inheritable) != FileShare.None ? Interop.BOOL.TRUE : Interop.BOOL.FALSE,
          lpSecurityDescriptor = (IntPtr) (void*) numPtr
        };
        using (DisableMediaInsertionPrompt.Create())
        {
          file = Interop.Kernel32.CreateFile(fullPath, (int) rights, share, &securityAttributes, mode, dwFlagsAndAttributes, IntPtr.Zero);
          FileSystemAclExtensions.ValidateFileHandle(file, fullPath);
        }
      }
      return file;
    }

    private static void ValidateFileHandle(SafeFileHandle handle, string fullPath)
    {
      if (handle.IsInvalid)
      {
        int errorCode = Marshal.GetLastWin32Error();
        if (errorCode == 3 && fullPath.Length == Path.GetPathRoot(fullPath).Length)
          errorCode = 5;
        throw Win32Marshal.GetExceptionForWin32Error(errorCode, fullPath);
      }
    }
  }
}
