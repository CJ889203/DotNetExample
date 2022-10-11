// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XName
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Linq
{
  [KnownType(typeof (NameSerializer))]
  [__DynamicallyInvokable]
  [Serializable]
  public sealed class XName : IEquatable<XName>, ISerializable
  {
    private XNamespace ns;
    private string localName;
    private int hashCode;

    internal XName(XNamespace ns, string localName)
    {
      this.ns = ns;
      this.localName = XmlConvert.VerifyNCName(localName);
      this.hashCode = ns.GetHashCode() ^ localName.GetHashCode();
    }

    [__DynamicallyInvokable]
    public string LocalName
    {
      [__DynamicallyInvokable] get => this.localName;
    }

    [__DynamicallyInvokable]
    public XNamespace Namespace
    {
      [__DynamicallyInvokable] get => this.ns;
    }

    [__DynamicallyInvokable]
    public string NamespaceName
    {
      [__DynamicallyInvokable] get => this.ns.NamespaceName;
    }

    [__DynamicallyInvokable]
    public override string ToString() => this.ns.NamespaceName.Length == 0 ? this.localName : "{" + this.ns.NamespaceName + "}" + this.localName;

    [__DynamicallyInvokable]
    public static XName Get(string expandedName)
    {
      switch (expandedName)
      {
        case "":
          throw new ArgumentException(Res.GetString("Argument_InvalidExpandedName", (object) expandedName));
        case null:
          throw new ArgumentNullException(nameof (expandedName));
        default:
          if (expandedName[0] != '{')
            return XNamespace.None.GetName(expandedName);
          int num = expandedName.LastIndexOf('}');
          if (num <= 1 || num == expandedName.Length - 1)
            throw new ArgumentException(Res.GetString("Argument_InvalidExpandedName", (object) expandedName));
          return XNamespace.Get(expandedName, 1, num - 1).GetName(expandedName, num + 1, expandedName.Length - num - 1);
      }
    }

    [__DynamicallyInvokable]
    public static XName Get(string localName, string namespaceName) => XNamespace.Get(namespaceName).GetName(localName);

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static implicit operator XName(string expandedName) => expandedName == null ? (XName) null : XName.Get(expandedName);

    [__DynamicallyInvokable]
    public override bool Equals(object obj) => (object) this == obj;

    [__DynamicallyInvokable]
    public override int GetHashCode() => this.hashCode;

    [__DynamicallyInvokable]
    public static bool operator ==(XName left, XName right) => (object) left == (object) right;

    [__DynamicallyInvokable]
    public static bool operator !=(XName left, XName right) => (object) left != (object) right;

    [__DynamicallyInvokable]
    bool IEquatable<XName>.Equals(XName other) => (object) this == (object) other;

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      info.AddValue("name", (object) this.ToString());
      info.SetType(typeof (NameSerializer));
    }
  }
}
