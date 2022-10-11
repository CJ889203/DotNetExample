// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.CompressionLevel
// Assembly: System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: A9780FE1-DF26-4575-BA54-75C62CD4F39E
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.xml

namespace System.IO.Compression
{
  /// <summary>Specifies values that indicate whether a compression operation emphasizes speed or compression size.</summary>
  public enum CompressionLevel
  {
    /// <summary>The compression operation should be optimally compressed, even if the operation takes a longer time to complete.</summary>
    Optimal,
    /// <summary>The compression operation should complete as quickly as possible, even if the resulting file is not optimally compressed.</summary>
    Fastest,
    /// <summary>No compression should be performed on the file.</summary>
    NoCompression,
    /// <summary>The compression operation should create output as small as possible, even if the operation takes a longer time to complete.</summary>
    SmallestSize,
  }
}
