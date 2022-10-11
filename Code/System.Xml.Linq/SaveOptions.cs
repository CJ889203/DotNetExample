// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.SaveOptions
// Assembly: System.Private.Xml.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 67E106B6-1B7E-4540-BB80-26A3D9D4BC13
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Xml.Linq.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Xml.XDocument.xml

namespace System.Xml.Linq
{
  /// <summary>Specifies serialization options.</summary>
  [Flags]
  public enum SaveOptions
  {
    /// <summary>Format (indent) the XML while serializing.</summary>
    None = 0,
    /// <summary>Preserve all insignificant white space while serializing.</summary>
    DisableFormatting = 1,
    /// <summary>Remove the duplicate namespace declarations while serializing.</summary>
    OmitDuplicateNamespaces = 2,
  }
}
