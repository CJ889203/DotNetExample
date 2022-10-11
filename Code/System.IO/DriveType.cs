// Decompiled with JetBrains decompiler
// Type: System.IO.DriveType
// Assembly: System.IO.FileSystem.DriveInfo, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: A5622349-D755-433E-9FA3-B750E99A52EA
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.FileSystem.DriveInfo.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.FileSystem.DriveInfo.xml

namespace System.IO
{
  /// <summary>Defines constants for drive types, including CDRom, Fixed, Network, NoRootDirectory, Ram, Removable, and Unknown.</summary>
  public enum DriveType
  {
    /// <summary>The type of drive is unknown.</summary>
    Unknown,
    /// <summary>The drive does not have a root directory.</summary>
    NoRootDirectory,
    /// <summary>The drive is a removable storage device, such as a USB flash drive.</summary>
    Removable,
    /// <summary>The drive is a fixed disk.</summary>
    Fixed,
    /// <summary>The drive is a network drive.</summary>
    Network,
    /// <summary>The drive is an optical disc device, such as a CD or DVD-ROM.</summary>
    CDRom,
    /// <summary>The drive is a RAM disk.</summary>
    Ram,
  }
}
