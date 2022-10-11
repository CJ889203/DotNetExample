// Decompiled with JetBrains decompiler
// Type: System.IO.DriveInfo
// Assembly: System.IO.FileSystem.DriveInfo, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: A5622349-D755-433E-9FA3-B750E99A52EA
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.DriveInfo.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.DriveInfo.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;


#nullable enable
namespace System.IO
{
  /// <summary>Provides access to information on a drive.</summary>
  public sealed class DriveInfo : ISerializable
  {

    #nullable disable
    private readonly string _name;


    #nullable enable
    /// <summary>Provides access to information on the specified drive.</summary>
    /// <param name="driveName">A valid drive path or drive letter. This can be either uppercase or lowercase, 'a' to 'z'. A null value is not valid.</param>
    /// <exception cref="T:System.ArgumentNullException">The drive letter cannot be <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The first letter of <paramref name="driveName" /> is not an uppercase or lowercase letter from 'a' to 'z'.
    /// 
    /// -or-
    /// 
    /// <paramref name="driveName" /> does not refer to a valid drive.</exception>
    public DriveInfo(string driveName) => this._name = driveName != null ? DriveInfo.NormalizeDriveName(driveName) : throw new ArgumentNullException(nameof (driveName));


    #nullable disable
    /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data needed to serialize the target object.</summary>
    /// <param name="info">The object to populate with data.</param>
    /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();


    #nullable enable
    /// <summary>Gets the name of a drive, such as C:\.</summary>
    /// <returns>The name of the drive.</returns>
    public string Name => this._name;

    /// <summary>Gets a value that indicates whether a drive is ready.</summary>
    /// <returns>
    /// <see langword="true" /> if the drive is ready; <see langword="false" /> if the drive is not ready.</returns>
    public bool IsReady => Directory.Exists(this.Name);

    /// <summary>Gets the root directory of a drive.</summary>
    /// <returns>An object that contains the root directory of the drive.</returns>
    public DirectoryInfo RootDirectory => new DirectoryInfo(this.Name);

    /// <summary>Returns a drive name as a string.</summary>
    /// <returns>The name of the drive.</returns>
    public override string ToString() => this.Name;


    #nullable disable
    private static string NormalizeDriveName(string driveName) => DriveInfoInternal.NormalizeDriveName(driveName);

    /// <summary>Gets the drive type, such as CD-ROM, removable, network, or fixed.</summary>
    /// <returns>One of the enumeration values that specifies a drive type.</returns>
    public DriveType DriveType => (DriveType) Interop.Kernel32.GetDriveType(this.Name);


    #nullable enable
    /// <summary>Gets the name of the file system, such as NTFS or FAT32.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">The drive does not exist or is not mapped.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <returns>The name of the file system on the specified drive.</returns>
    public unsafe string DriveFormat
    {
      get
      {
        char* fileSystemName = stackalloc char[261];
        using (DisableMediaInsertionPrompt.Create())
        {
          if (!Interop.Kernel32.GetVolumeInformation(this.Name, (char*) null, 0, (int*) null, (int*) null, out int _, fileSystemName, 261))
            throw Error.GetExceptionForLastWin32DriveError(this.Name);
        }
        return new string(fileSystemName);
      }
    }

    /// <summary>Indicates the amount of available free space on a drive, in bytes.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <returns>The amount of free space available on the drive, in bytes.</returns>
    public long AvailableFreeSpace
    {
      get
      {
        uint lpOldMode;
        bool flag = Interop.Kernel32.SetThreadErrorMode(1U, out lpOldMode);
        long freeBytesForUser;
        try
        {
          if (!Interop.Kernel32.GetDiskFreeSpaceEx(this.Name, out freeBytesForUser, out long _, out long _))
            throw Error.GetExceptionForLastWin32DriveError(this.Name);
        }
        finally
        {
          if (flag)
            Interop.Kernel32.SetThreadErrorMode(lpOldMode, out uint _);
        }
        return freeBytesForUser;
      }
    }

    /// <summary>Gets the total amount of free space available on a drive, in bytes.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <returns>The total free space available on a drive, in bytes.</returns>
    public long TotalFreeSpace
    {
      get
      {
        uint lpOldMode;
        bool flag = Interop.Kernel32.SetThreadErrorMode(1U, out lpOldMode);
        long freeBytes;
        try
        {
          if (!Interop.Kernel32.GetDiskFreeSpaceEx(this.Name, out long _, out long _, out freeBytes))
            throw Error.GetExceptionForLastWin32DriveError(this.Name);
        }
        finally
        {
          if (flag)
            Interop.Kernel32.SetThreadErrorMode(lpOldMode, out uint _);
        }
        return freeBytes;
      }
    }

    /// <summary>Gets the total size of storage space on a drive, in bytes.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <returns>The total size of the drive, in bytes.</returns>
    public long TotalSize
    {
      get
      {
        uint lpOldMode;
        Interop.Kernel32.SetThreadErrorMode(1U, out lpOldMode);
        long totalBytes;
        try
        {
          if (!Interop.Kernel32.GetDiskFreeSpaceEx(this.Name, out long _, out totalBytes, out long _))
            throw Error.GetExceptionForLastWin32DriveError(this.Name);
        }
        finally
        {
          Interop.Kernel32.SetThreadErrorMode(lpOldMode, out uint _);
        }
        return totalBytes;
      }
    }

    /// <summary>Retrieves the drive names of all logical drives on a computer.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An array of type <see cref="T:System.IO.DriveInfo" /> that represents the logical drives on a computer.</returns>
    public static DriveInfo[] GetDrives()
    {
      string[] logicalDrives = DriveInfoInternal.GetLogicalDrives();
      DriveInfo[] drives = new DriveInfo[logicalDrives.Length];
      for (int index = 0; index < logicalDrives.Length; ++index)
        drives[index] = new DriveInfo(logicalDrives[index]);
      return drives;
    }

    /// <summary>Gets or sets the volume label of a drive.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The volume label is being set on a network or CD-ROM drive.
    /// 
    /// -or-
    /// 
    /// Access to the drive information is denied.</exception>
    /// <returns>The volume label.</returns>
    public unsafe string VolumeLabel
    {
      get
      {
        char* volumeName = stackalloc char[261];
        using (DisableMediaInsertionPrompt.Create())
        {
          if (!Interop.Kernel32.GetVolumeInformation(this.Name, volumeName, 261, (int*) null, (int*) null, out int _, (char*) null, 0))
            throw Error.GetExceptionForLastWin32DriveError(this.Name);
        }
        return new string(volumeName);
      }
      [SupportedOSPlatform("windows")] [param: AllowNull] set
      {
        uint lpOldMode;
        bool flag = Interop.Kernel32.SetThreadErrorMode(1U, out lpOldMode);
        try
        {
          if (Interop.Kernel32.SetVolumeLabel(this.Name, value))
            return;
          int lastPinvokeError = Marshal.GetLastPInvokeError();
          if (lastPinvokeError == 5)
            throw new UnauthorizedAccessException(SR.InvalidOperation_SetVolumeLabelFailed);
          throw Error.GetExceptionForWin32DriveError(lastPinvokeError, this.Name);
        }
        finally
        {
          if (flag)
            Interop.Kernel32.SetThreadErrorMode(lpOldMode, out uint _);
        }
      }
    }
  }
}
