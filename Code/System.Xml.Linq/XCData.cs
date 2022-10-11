// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XCData
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public class XCData : XText
  {
    [__DynamicallyInvokable]
    public XCData(string value)
      : base(value)
    {
    }

    [__DynamicallyInvokable]
    public XCData(XCData other)
      : base((XText) other)
    {
    }

    internal XCData(XmlReader r)
      : base(r)
    {
    }

    [__DynamicallyInvokable]
    public override XmlNodeType NodeType
    {
      [__DynamicallyInvokable] get => XmlNodeType.CDATA;
    }

    [__DynamicallyInvokable]
    public override void WriteTo(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WriteCData(this.text);
    }

    internal override XNode CloneNode() => (XNode) new XCData(this);
  }
}
