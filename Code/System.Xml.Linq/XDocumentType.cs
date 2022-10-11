// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XDocumentType
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public class XDocumentType : XNode
  {
    private string name;
    private string publicId;
    private string systemId;
    private string internalSubset;
    private IDtdInfo dtdInfo;

    [__DynamicallyInvokable]
    public XDocumentType(string name, string publicId, string systemId, string internalSubset)
    {
      this.name = XmlConvert.VerifyName(name);
      this.publicId = publicId;
      this.systemId = systemId;
      this.internalSubset = internalSubset;
    }

    [__DynamicallyInvokable]
    public XDocumentType(XDocumentType other)
    {
      this.name = other != null ? other.name : throw new ArgumentNullException(nameof (other));
      this.publicId = other.publicId;
      this.systemId = other.systemId;
      this.internalSubset = other.internalSubset;
      this.dtdInfo = other.dtdInfo;
    }

    internal XDocumentType(XmlReader r)
    {
      this.name = r.Name;
      this.publicId = r.GetAttribute("PUBLIC");
      this.systemId = r.GetAttribute("SYSTEM");
      this.internalSubset = r.Value;
      this.dtdInfo = r.DtdInfo;
      r.Read();
    }

    internal XDocumentType(
      string name,
      string publicId,
      string systemId,
      string internalSubset,
      IDtdInfo dtdInfo)
      : this(name, publicId, systemId, internalSubset)
    {
      this.dtdInfo = dtdInfo;
    }

    [__DynamicallyInvokable]
    public string InternalSubset
    {
      [__DynamicallyInvokable] get => this.internalSubset;
      [__DynamicallyInvokable] set
      {
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
        this.internalSubset = value;
        if (!flag)
          return;
        this.NotifyChanged((object) this, XObjectChangeEventArgs.Value);
      }
    }

    [__DynamicallyInvokable]
    public string Name
    {
      [__DynamicallyInvokable] get => this.name;
      [__DynamicallyInvokable] set
      {
        value = XmlConvert.VerifyName(value);
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Name);
        this.name = value;
        if (!flag)
          return;
        this.NotifyChanged((object) this, XObjectChangeEventArgs.Name);
      }
    }

    [__DynamicallyInvokable]
    public override XmlNodeType NodeType
    {
      [__DynamicallyInvokable] get => XmlNodeType.DocumentType;
    }

    [__DynamicallyInvokable]
    public string PublicId
    {
      [__DynamicallyInvokable] get => this.publicId;
      [__DynamicallyInvokable] set
      {
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
        this.publicId = value;
        if (!flag)
          return;
        this.NotifyChanged((object) this, XObjectChangeEventArgs.Value);
      }
    }

    [__DynamicallyInvokable]
    public string SystemId
    {
      [__DynamicallyInvokable] get => this.systemId;
      [__DynamicallyInvokable] set
      {
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
        this.systemId = value;
        if (!flag)
          return;
        this.NotifyChanged((object) this, XObjectChangeEventArgs.Value);
      }
    }

    internal IDtdInfo DtdInfo => this.dtdInfo;

    [__DynamicallyInvokable]
    public override void WriteTo(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WriteDocType(this.name, this.publicId, this.systemId, this.internalSubset);
    }

    internal override XNode CloneNode() => (XNode) new XDocumentType(this);

    internal override bool DeepEquals(XNode node) => node is XDocumentType xdocumentType && this.name == xdocumentType.name && this.publicId == xdocumentType.publicId && this.systemId == xdocumentType.SystemId && this.internalSubset == xdocumentType.internalSubset;

    internal override int GetDeepHashCode() => this.name.GetHashCode() ^ (this.publicId != null ? this.publicId.GetHashCode() : 0) ^ (this.systemId != null ? this.systemId.GetHashCode() : 0) ^ (this.internalSubset != null ? this.internalSubset.GetHashCode() : 0);
  }
}
