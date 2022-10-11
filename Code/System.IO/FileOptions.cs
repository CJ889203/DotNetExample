// Decompiled with JetBrains decompiler
// Type: System.IO.FileOptions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.IO
{
  /// <summary>Represents advanced options for creating a <see cref="T:System.IO.FileStream" /> object.</summary>
  [Flags]
  public enum FileOptions
  {
    /// <summary>Indicates that no additional options should be used when creating a <see cref="T:System.IO.FileStream" /> object.</summary>
    None = 0,
    /// <summary>Indicates that the system should write through any intermediate cache and go directly to disk.</summary>
    WriteThrough = -2147483648, // 0x80000000
    /// <summary>Indicates that a file can be used for asynchronous reading and writing.</summary>
    Asynchronous = 1073741824, // 0x40000000
    /// <summary>Indicates that the file is accessed randomly. The system can use this as a hint to optimize file caching.</summary>
    RandomAccess = 268435456, // 0x10000000
    /// <summary>Indicates that a file is automatically deleted when it is no longer in use.</summary>
    DeleteOnClose = 67108864, // 0x04000000
    /// <summary>Indicates that the file is to be accessed sequentially from beginning to end. The system can use this as a hint to optimize file caching. If an application moves the file pointer for random access, optimum caching may not occur; however, correct operation is still guaranteed. Specifying this flag can increase performance in some cases.</summary>
    SequentialScan = 134217728, // 0x08000000
    /// <summary>Indicates that a file is encrypted and can be decrypted only by using the same user account used for encryption.</summary>
    Encrypted = 16384, // 0x00004000
  }
}
