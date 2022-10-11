// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XComment
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public class XComment : XNode
  {
    internal string value;

    [__DynamicallyInvokable]
    public XComment(string value) => this.value = value != null ? value : throw new ArgumentNullException(nameof (value));

    [__DynamicallyInvokable]
    public XComment(XComment other) => this.value = other != null ? other.value : throw new ArgumentNullException(nameof (other));

    internal XComment(XmlReader r)
    {
      this.value = r.Value;
      r.Read();
    }

    [__DynamicallyInvokable]
    public override XmlNodeType NodeType
    {
      [__DynamicallyInvokable] get => XmlNodeType.Comment;
    }

    [__DynamicallyInvokable]
    public string Value
    {
      [__DynamicallyInvokable] get => this.value;
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
        this.value = value;
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
      writer.WriteComment(this.value);
    }

    internal override XNode CloneNode() => (XNode) new XComment(this);

    internal override bool DeepEquals(XNode node) => node is XComment xcomment && this.value == xcomment.value;

    internal override int GetDeepHashCode() => this.value.GetHashCode();
  }
}
