// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XNamespace
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

using System.Threading;

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public sealed class XNamespace
  {
    internal const string xmlPrefixNamespace = "http://www.w3.org/XML/1998/namespace";
    internal const string xmlnsPrefixNamespace = "http://www.w3.org/2000/xmlns/";
    private static XHashtable<WeakReference> namespaces;
    private static WeakReference refNone;
    private static WeakReference refXml;
    private static WeakReference refXmlns;
    private string namespaceName;
    private int hashCode;
    private XHashtable<XName> names;
    private const int NamesCapacity = 8;
    private const int NamespacesCapacity = 32;

    internal XNamespace(string namespaceName)
    {
      this.namespaceName = namespaceName;
      this.hashCode = namespaceName.GetHashCode();
      this.names = new XHashtable<XName>(new XHashtable<XName>.ExtractKeyDelegate(XNamespace.ExtractLocalName), 8);
    }

    [__DynamicallyInvokable]
    public string NamespaceName
    {
      [__DynamicallyInvokable] get => this.namespaceName;
    }

    [__DynamicallyInvokable]
    public XName GetName(string localName) => localName != null ? this.GetName(localName, 0, localName.Length) : throw new ArgumentNullException(nameof (localName));

    [__DynamicallyInvokable]
    public override string ToString() => this.namespaceName;

    [__DynamicallyInvokable]
    public static XNamespace None
    {
      [__DynamicallyInvokable] get => XNamespace.EnsureNamespace(ref XNamespace.refNone, string.Empty);
    }

    [__DynamicallyInvokable]
    public static XNamespace Xml
    {
      [__DynamicallyInvokable] get => XNamespace.EnsureNamespace(ref XNamespace.refXml, "http://www.w3.org/XML/1998/namespace");
    }

    [__DynamicallyInvokable]
    public static XNamespace Xmlns
    {
      [__DynamicallyInvokable] get => XNamespace.EnsureNamespace(ref XNamespace.refXmlns, "http://www.w3.org/2000/xmlns/");
    }

    [__DynamicallyInvokable]
    public static XNamespace Get(string namespaceName) => namespaceName != null ? XNamespace.Get(namespaceName, 0, namespaceName.Length) : throw new ArgumentNullException(nameof (namespaceName));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static implicit operator XNamespace(string namespaceName) => namespaceName == null ? (XNamespace) null : XNamespace.Get(namespaceName);

    [__DynamicallyInvokable]
    public static XName operator +(XNamespace ns, string localName) => !(ns == (XNamespace) null) ? ns.GetName(localName) : throw new ArgumentNullException(nameof (ns));

    [__DynamicallyInvokable]
    public override bool Equals(object obj) => (object) this == obj;

    [__DynamicallyInvokable]
    public override int GetHashCode() => this.hashCode;

    [__DynamicallyInvokable]
    public static bool operator ==(XNamespace left, XNamespace right) => (object) left == (object) right;

    [__DynamicallyInvokable]
    public static bool operator !=(XNamespace left, XNamespace right) => (object) left != (object) right;

    internal XName GetName(string localName, int index, int count)
    {
      XName xname;
      return this.names.TryGetValue(localName, index, count, out xname) ? xname : this.names.Add(new XName(this, localName.Substring(index, count)));
    }

    internal static XNamespace Get(string namespaceName, int index, int count)
    {
      if (count == 0)
        return XNamespace.None;
      if (XNamespace.namespaces == null)
        Interlocked.CompareExchange<XHashtable<WeakReference>>(ref XNamespace.namespaces, new XHashtable<WeakReference>(new XHashtable<WeakReference>.ExtractKeyDelegate(XNamespace.ExtractNamespace), 32), (XHashtable<WeakReference>) null);
      XNamespace xnamespace;
      do
      {
        WeakReference weakReference;
        if (!XNamespace.namespaces.TryGetValue(namespaceName, index, count, out weakReference))
        {
          if (count == "http://www.w3.org/XML/1998/namespace".Length && string.CompareOrdinal(namespaceName, index, "http://www.w3.org/XML/1998/namespace", 0, count) == 0)
            return XNamespace.Xml;
          if (count == "http://www.w3.org/2000/xmlns/".Length && string.CompareOrdinal(namespaceName, index, "http://www.w3.org/2000/xmlns/", 0, count) == 0)
            return XNamespace.Xmlns;
          weakReference = XNamespace.namespaces.Add(new WeakReference((object) new XNamespace(namespaceName.Substring(index, count))));
        }
        xnamespace = weakReference != null ? (XNamespace) weakReference.Target : (XNamespace) null;
      }
      while (xnamespace == (XNamespace) null);
      return xnamespace;
    }

    private static string ExtractLocalName(XName n) => n.LocalName;

    private static string ExtractNamespace(WeakReference r)
    {
      XNamespace target;
      return r == null || (target = (XNamespace) r.Target) == (XNamespace) null ? (string) null : target.NamespaceName;
    }

    private static XNamespace EnsureNamespace(
      ref WeakReference refNmsp,
      string namespaceName)
    {
      XNamespace target;
      while (true)
      {
        WeakReference comparand = refNmsp;
        if (comparand != null)
        {
          target = (XNamespace) comparand.Target;
          if (target != (XNamespace) null)
            break;
        }
        Interlocked.CompareExchange<WeakReference>(ref refNmsp, new WeakReference((object) new XNamespace(namespaceName)), comparand);
      }
      return target;
    }
  }
}
