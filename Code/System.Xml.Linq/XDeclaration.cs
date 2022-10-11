// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XDeclaration
// Assembly: System.Private.Xml.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 67E106B6-1B7E-4540-BB80-26A3D9D4BC13
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Xml.Linq.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Xml.XDocument.xml

using System.Text;


#nullable enable
namespace System.Xml.Linq
{
  /// <summary>Represents an XML declaration.</summary>
  public class XDeclaration
  {

    #nullable disable
    private string _version;
    private string _encoding;
    private string _standalone;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDeclaration" /> class with the specified version, encoding, and standalone status.</summary>
    /// <param name="version">The version of the XML, usually "1.0".</param>
    /// <param name="encoding">The encoding for the XML document.</param>
    /// <param name="standalone">A string containing "yes" or "no" that specifies whether the XML is standalone or requires external entities to be resolved.</param>
    public XDeclaration(string? version, string? encoding, string? standalone)
    {
      this._version = version;
      this._encoding = encoding;
      this._standalone = standalone;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDeclaration" /> class from another <see cref="T:System.Xml.Linq.XDeclaration" /> object.</summary>
    /// <param name="other">The <see cref="T:System.Xml.Linq.XDeclaration" /> used to initialize this <see cref="T:System.Xml.Linq.XDeclaration" /> object.</param>
    public XDeclaration(XDeclaration other)
    {
      this._version = other != null ? other._version : throw new ArgumentNullException(nameof (other));
      this._encoding = other._encoding;
      this._standalone = other._standalone;
    }


    #nullable disable
    internal XDeclaration(XmlReader r)
    {
      this._version = r.GetAttribute("version");
      this._encoding = r.GetAttribute("encoding");
      this._standalone = r.GetAttribute("standalone");
      r.Read();
    }


    #nullable enable
    /// <summary>Gets or sets the encoding for this document.</summary>
    /// <returns>A <see cref="T:System.String" /> containing the code page name for this document.</returns>
    public string? Encoding
    {
      get => this._encoding;
      set => this._encoding = value;
    }

    /// <summary>Gets or sets the standalone property for this document.</summary>
    /// <returns>A <see cref="T:System.String" /> containing the standalone property for this document.</returns>
    public string? Standalone
    {
      get => this._standalone;
      set => this._standalone = value;
    }

    /// <summary>Gets or sets the version property for this document.</summary>
    /// <returns>A <see cref="T:System.String" /> containing the version property for this document.</returns>
    public string? Version
    {
      get => this._version;
      set => this._version = value;
    }

    /// <summary>Provides the declaration as a formatted string.</summary>
    /// <returns>A <see cref="T:System.String" /> that contains the formatted XML string.</returns>
    public override string ToString()
    {
      StringBuilder sb = StringBuilderCache.Acquire();
      sb.Append("<?xml");
      if (this._version != null)
      {
        sb.Append(" version=\"");
        sb.Append(this._version);
        sb.Append('"');
      }
      if (this._encoding != null)
      {
        sb.Append(" encoding=\"");
        sb.Append(this._encoding);
        sb.Append('"');
      }
      if (this._standalone != null)
      {
        sb.Append(" standalone=\"");
        sb.Append(this._standalone);
        sb.Append('"');
      }
      sb.Append("?>");
      return StringBuilderCache.GetStringAndRelease(sb);
    }
  }
}
