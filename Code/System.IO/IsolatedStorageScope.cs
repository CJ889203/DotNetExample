// Decompiled with JetBrains decompiler
// Type: System.IO.IsolatedStorage.IsolatedStorageScope
// Assembly: System.IO.IsolatedStorage, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 87FE0B2F-0A44-4572-BEFC-C86F7165516A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.IsolatedStorage.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.IsolatedStorage.xml

namespace System.IO.IsolatedStorage
{
  /// <summary>Enumerates the levels of isolated storage scope that are supported by <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" />.</summary>
  [Flags]
  public enum IsolatedStorageScope
  {
    /// <summary>No isolated storage usage.</summary>
    None = 0,
    /// <summary>Isolated storage scoped by user identity.</summary>
    User = 1,
    /// <summary>Isolated storage scoped to the application domain identity.</summary>
    Domain = 2,
    /// <summary>Isolated storage scoped to the identity of the assembly.</summary>
    Assembly = 4,
    /// <summary>The isolated store can be placed in a location on the file system that might roam (if roaming user data is enabled on the underlying operating system).</summary>
    Roaming = 8,
    /// <summary>Isolated storage scoped to the machine.</summary>
    Machine = 16, // 0x00000010
    /// <summary>Isolated storage scoped to the application.</summary>
    Application = 32, // 0x00000020
  }
}
