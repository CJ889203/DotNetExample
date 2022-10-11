// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XText
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

using System.Text;

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public class XText : XNode
  {
    internal string text;

    [__DynamicallyInvokable]
    public XText(string value) => this.text = value != null ? value : throw new ArgumentNullException(nameof (value));

    [__DynamicallyInvokable]
    public XText(XText other) => this.text = other != null ? other.text : throw new ArgumentNullException(nameof (other));

    internal XText(XmlReader r)
    {
      this.text = r.Value;
      r.Read();
    }

    [__DynamicallyInvokable]
    public override XmlNodeType NodeType
    {
      [__DynamicallyInvokable] get => XmlNodeType.Text;
    }

    [__DynamicallyInvokable]
    public string Value
    {
      [__DynamicallyInvokable] get => this.text;
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
        this.text = value;
        if (!flag)
          return;
        this.NotifyChanged((object) this, XObjectChangeEventArgs.Value);
      }
    }

    [__DynamicallyInvokable]
    public override void WriteTo(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (this.parent is XDocument)
        writer.WriteWhitespace(this.text);
      else
        writer.WriteString(this.text);
    }

    internal override void AppendText(StringBuilder sb) => sb.Append(this.text);

    internal override XNode CloneNode() => (XNode) new XText(this);

    internal override bool DeepEquals(XNode node) => node != null && this.NodeType == node.NodeType && this.text == ((XText) node).text;

    internal override int GetDeepHashCode() => this.text.GetHashCode();
  }
}
