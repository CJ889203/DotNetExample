// Decompiled with JetBrains decompiler
// Type: System.IO.FileAccess
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.IO
{
  /// <summary>Defines constants for read, write, or read/write access to a file.</summary>
  [Flags]
  public enum FileAccess
  {
    /// <summary>Read access to the file. Data can be read from the file. Combine with <see langword="Write" /> for read/write access.</summary>
    Read = 1,
    /// <summary>Write access to the file. Data can be written to the file. Combine with <see langword="Read" /> for read/write access.</summary>
    Write = 2,
    /// <summary>Read and write access to the file. Data can be written to and read from the file.</summary>
    ReadWrite = Write | Read, // 0x00000003
  }
}
