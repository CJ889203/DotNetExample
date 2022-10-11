// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.ZipArchiveMode
// Assembly: System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: A9780FE1-DF26-4575-BA54-75C62CD4F39E
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.xml

namespace System.IO.Compression
{
  /// <summary>Specifies values for interacting with zip archive entries.</summary>
  public enum ZipArchiveMode
  {
    /// <summary>Only reading archive entries is permitted.</summary>
    Read,
    /// <summary>Only creating new archive entries is permitted.</summary>
    Create,
    /// <summary>Both read and write operations are permitted for archive entries.</summary>
    Update,
  }
}
