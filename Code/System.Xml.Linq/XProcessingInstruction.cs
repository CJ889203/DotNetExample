// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XProcessingInstruction
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public class XProcessingInstruction : XNode
  {
    internal string target;
    internal string data;

    [__DynamicallyInvokable]
    public XProcessingInstruction(string target, string data)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      XProcessingInstruction.ValidateName(target);
      this.target = target;
      this.data = data;
    }

    [__DynamicallyInvokable]
    public XProcessingInstruction(XProcessingInstruction other)
    {
      this.target = other != null ? other.target : throw new ArgumentNullException(nameof (other));
      this.data = other.data;
    }

    internal XProcessingInstruction(XmlReader r)
    {
      this.target = r.Name;
      this.data = r.Value;
      r.Read();
    }

    [__DynamicallyInvokable]
    public string Data
    {
      [__DynamicallyInvokable] get => this.data;
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
        this.data = value;
        if (!flag)
          return;
        this.NotifyChanged((object) this, XObjectChangeEventArgs.Value);
      }
    }

    [__DynamicallyInvokable]
    public override XmlNodeType NodeType
    {
      [__DynamicallyInvokable] get => XmlNodeType.ProcessingInstruction;
    }

    [__DynamicallyInvokable]
    public string Target
    {
      [__DynamicallyInvokable] get => this.target;
      [__DynamicallyInvokable] set
      {
        XProcessingInstruction.ValidateName(value);
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Name);
        this.target = value;
        if (!flag)
          return;
        this.NotifyChanged((object) this, XObjectChangeEventArgs.Name);
      }
    }

    [__DynamicallyInvokable]
    public override void WriteTo(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WriteProcessingInstruction(this.target, this.data);
    }

    internal override XNode CloneNode() => (XNode) new XProcessingInstruction(this);

    internal override bool DeepEquals(XNode node) => node is XProcessingInstruction xprocessingInstruction && this.target == xprocessingInstruction.target && this.data == xprocessingInstruction.data;

    internal override int GetDeepHashCode() => this.target.GetHashCode() ^ this.data.GetHashCode();

    private static void ValidateName(string name)
    {
      XmlConvert.VerifyNCName(name);
      if (string.Compare(name, "xml", StringComparison.OrdinalIgnoreCase) == 0)
        throw new ArgumentException(Res.GetString("Argument_InvalidPIName", (object) name));
    }
  }
}
