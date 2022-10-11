// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.CompressionMode
// Assembly: System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: A9780FE1-DF26-4575-BA54-75C62CD4F39E
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.xml

namespace System.IO.Compression
{
  /// <summary>Specifies whether to compress or decompress the underlying stream.</summary>
  public enum CompressionMode
  {
    /// <summary>Decompresses the underlying stream.</summary>
    Decompress,
    /// <summary>Compresses the underlying stream.</summary>
    Compress,
  }
}
