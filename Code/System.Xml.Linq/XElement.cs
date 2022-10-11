// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XElement
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

using MS.Internal.Xml.Linq.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Xml.Linq
{
  [XmlSchemaProvider(null, IsAny = true)]
  [TypeDescriptionProvider(typeof (XTypeDescriptionProvider<XElement>))]
  [__DynamicallyInvokable]
  public class XElement : XContainer, IXmlSerializable
  {
    private static IEnumerable<XElement> emptySequence;
    internal XName name;
    internal XAttribute lastAttr;

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> EmptySequence
    {
      [__DynamicallyInvokable] get
      {
        if (XElement.emptySequence == null)
          XElement.emptySequence = (IEnumerable<XElement>) new XElement[0];
        return XElement.emptySequence;
      }
    }

    [__DynamicallyInvokable]
    public XElement(XName name) => this.name = !(name == (XName) null) ? name : throw new ArgumentNullException(nameof (name));

    [__DynamicallyInvokable]
    public XElement(XName name, object content)
      : this(name)
    {
      this.AddContentSkipNotify(content);
    }

    [__DynamicallyInvokable]
    public XElement(XName name, params object[] content)
      : this(name, (object) content)
    {
    }

    [__DynamicallyInvokable]
    public XElement(XElement other)
      : base((XContainer) other)
    {
      this.name = other.name;
      XAttribute other1 = other.lastAttr;
      if (other1 == null)
        return;
      do
      {
        other1 = other1.next;
        this.AppendAttributeSkipNotify(new XAttribute(other1));
      }
      while (other1 != other.lastAttr);
    }

    [__DynamicallyInvokable]
    public XElement(XStreamingElement other)
    {
      this.name = other != null ? other.name : throw new ArgumentNullException(nameof (other));
      this.AddContentSkipNotify(other.content);
    }

    internal XElement()
      : this((XName) "default")
    {
    }

    internal XElement(XmlReader r)
      : this(r, LoadOptions.None)
    {
    }

    internal XElement(XmlReader r, LoadOptions o) => this.ReadElementFrom(r, o);

    [__DynamicallyInvokable]
    public XAttribute FirstAttribute
    {
      [__DynamicallyInvokable] get => this.lastAttr == null ? (XAttribute) null : this.lastAttr.next;
    }

    [__DynamicallyInvokable]
    public bool HasAttributes
    {
      [__DynamicallyInvokable] get => this.lastAttr != null;
    }

    [__DynamicallyInvokable]
    public bool HasElements
    {
      [__DynamicallyInvokable] get
      {
        if (this.content is XNode xnode)
        {
          while (!(xnode is XElement))
          {
            xnode = xnode.next;
            if (xnode == this.content)
              goto label_4;
          }
          return true;
        }
label_4:
        return false;
      }
    }

    [__DynamicallyInvokable]
    public bool IsEmpty
    {
      [__DynamicallyInvokable] get => this.content == null;
    }

    [__DynamicallyInvokable]
    public XAttribute LastAttribute
    {
      [__DynamicallyInvokable] get => this.lastAttr;
    }

    [__DynamicallyInvokable]
    public XName Name
    {
      [__DynamicallyInvokable] get => this.name;
      [__DynamicallyInvokable] set
      {
        if (value == (XName) null)
          throw new ArgumentNullException(nameof (value));
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
      [__DynamicallyInvokable] get => XmlNodeType.Element;
    }

    [__DynamicallyInvokable]
    public string Value
    {
      [__DynamicallyInvokable] get
      {
        if (this.content == null)
          return string.Empty;
        if (this.content is string content)
          return content;
        StringBuilder sb = new StringBuilder();
        this.AppendText(sb);
        return sb.ToString();
      }
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        this.RemoveNodes();
        this.Add((object) value);
      }
    }

    [__DynamicallyInvokable]
    public IEnumerable<XElement> AncestorsAndSelf() => this.GetAncestors((XName) null, true);

    [__DynamicallyInvokable]
    public IEnumerable<XElement> AncestorsAndSelf(XName name) => !(name != (XName) null) ? XElement.EmptySequence : this.GetAncestors(name, true);

    [__DynamicallyInvokable]
    public XAttribute Attribute(XName name)
    {
      XAttribute xattribute = this.lastAttr;
      if (xattribute != null)
      {
        do
        {
          xattribute = xattribute.next;
          if (xattribute.name == name)
            return xattribute;
        }
        while (xattribute != this.lastAttr);
      }
      return (XAttribute) null;
    }

    [__DynamicallyInvokable]
    public IEnumerable<XAttribute> Attributes() => this.GetAttributes((XName) null);

    [__DynamicallyInvokable]
    public IEnumerable<XAttribute> Attributes(XName name) => !(name != (XName) null) ? XAttribute.EmptySequence : this.GetAttributes(name);

    [__DynamicallyInvokable]
    public IEnumerable<XNode> DescendantNodesAndSelf() => this.GetDescendantNodes(true);

    [__DynamicallyInvokable]
    public IEnumerable<XElement> DescendantsAndSelf() => this.GetDescendants((XName) null, true);

    [__DynamicallyInvokable]
    public IEnumerable<XElement> DescendantsAndSelf(XName name) => !(name != (XName) null) ? XElement.EmptySequence : this.GetDescendants(name, true);

    [__DynamicallyInvokable]
    public XNamespace GetDefaultNamespace()
    {
      string namespaceOfPrefixInScope = this.GetNamespaceOfPrefixInScope("xmlns", (XElement) null);
      return namespaceOfPrefixInScope == null ? XNamespace.None : XNamespace.Get(namespaceOfPrefixInScope);
    }

    [__DynamicallyInvokable]
    public XNamespace GetNamespaceOfPrefix(string prefix)
    {
      switch (prefix)
      {
        case "":
          throw new ArgumentException(Res.GetString("Argument_InvalidPrefix", (object) prefix));
        case "xmlns":
          return XNamespace.Xmlns;
        case null:
          throw new ArgumentNullException(nameof (prefix));
        default:
          string namespaceOfPrefixInScope = this.GetNamespaceOfPrefixInScope(prefix, (XElement) null);
          if (namespaceOfPrefixInScope != null)
            return XNamespace.Get(namespaceOfPrefixInScope);
          return prefix == "xml" ? XNamespace.Xml : (XNamespace) null;
      }
    }

    [__DynamicallyInvokable]
    public string GetPrefixOfNamespace(XNamespace ns)
    {
      string str = !(ns == (XNamespace) null) ? ns.NamespaceName : throw new ArgumentNullException(nameof (ns));
      bool flag1 = false;
      outOfScope = this;
      do
      {
        XAttribute xattribute = outOfScope.lastAttr;
        if (xattribute != null)
        {
          bool flag2 = false;
          do
          {
            xattribute = xattribute.next;
            if (xattribute.IsNamespaceDeclaration)
            {
              if (xattribute.Value == str && xattribute.Name.NamespaceName.Length != 0 && (!flag1 || this.GetNamespaceOfPrefixInScope(xattribute.Name.LocalName, outOfScope) == null))
                return xattribute.Name.LocalName;
              flag2 = true;
            }
          }
          while (xattribute != outOfScope.lastAttr);
          flag1 |= flag2;
        }
      }
      while (outOfScope.parent is XElement outOfScope);
      if ((object) str == (object) "http://www.w3.org/XML/1998/namespace")
      {
        if (!flag1 || this.GetNamespaceOfPrefixInScope("xml", (XElement) null) == null)
          return "xml";
      }
      else if ((object) str == (object) "http://www.w3.org/2000/xmlns/")
        return "xmlns";
      return (string) null;
    }

    [__DynamicallyInvokable]
    public static XElement Load(string uri) => XElement.Load(uri, LoadOptions.None);

    [__DynamicallyInvokable]
    public static XElement Load(string uri, LoadOptions options)
    {
      XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
      using (XmlReader reader = XmlReader.Create(uri, xmlReaderSettings))
        return XElement.Load(reader, options);
    }

    [__DynamicallyInvokable]
    public static XElement Load(Stream stream) => XElement.Load(stream, LoadOptions.None);

    [__DynamicallyInvokable]
    public static XElement Load(Stream stream, LoadOptions options)
    {
      XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
      using (XmlReader reader = XmlReader.Create(stream, xmlReaderSettings))
        return XElement.Load(reader, options);
    }

    [__DynamicallyInvokable]
    public static XElement Load(TextReader textReader) => XElement.Load(textReader, LoadOptions.None);

    [__DynamicallyInvokable]
    public static XElement Load(TextReader textReader, LoadOptions options)
    {
      XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
      using (XmlReader reader = XmlReader.Create(textReader, xmlReaderSettings))
        return XElement.Load(reader, options);
    }

    [__DynamicallyInvokable]
    public static XElement Load(XmlReader reader) => XElement.Load(reader, LoadOptions.None);

    [__DynamicallyInvokable]
    public static XElement Load(XmlReader reader, LoadOptions options)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      XElement xelement = reader.MoveToContent() == XmlNodeType.Element ? new XElement(reader, options) : throw new InvalidOperationException(Res.GetString("InvalidOperation_ExpectedNodeType", (object) XmlNodeType.Element, (object) reader.NodeType));
      int content = (int) reader.MoveToContent();
      if (!reader.EOF)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_ExpectedEndOfFile"));
      return xelement;
    }

    [__DynamicallyInvokable]
    public static XElement Parse(string text) => XElement.Parse(text, LoadOptions.None);

    [__DynamicallyInvokable]
    public static XElement Parse(string text, LoadOptions options)
    {
      using (StringReader input = new StringReader(text))
      {
        XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
        using (XmlReader reader = XmlReader.Create((TextReader) input, xmlReaderSettings))
          return XElement.Load(reader, options);
      }
    }

    [__DynamicallyInvokable]
    public void RemoveAll()
    {
      this.RemoveAttributes();
      this.RemoveNodes();
    }

    [__DynamicallyInvokable]
    public void RemoveAttributes()
    {
      if (this.SkipNotify())
      {
        this.RemoveAttributesSkipNotify();
      }
      else
      {
        while (this.lastAttr != null)
        {
          XAttribute next = this.lastAttr.next;
          this.NotifyChanging((object) next, XObjectChangeEventArgs.Remove);
          if (this.lastAttr == null || next != this.lastAttr.next)
            throw new InvalidOperationException(Res.GetString("InvalidOperation_ExternalCode"));
          if (next != this.lastAttr)
            this.lastAttr.next = next.next;
          else
            this.lastAttr = (XAttribute) null;
          next.parent = (XContainer) null;
          next.next = (XAttribute) null;
          this.NotifyChanged((object) next, XObjectChangeEventArgs.Remove);
        }
      }
    }

    [__DynamicallyInvokable]
    public void ReplaceAll(object content)
    {
      content = XContainer.GetContentSnapshot(content);
      this.RemoveAll();
      this.Add(content);
    }

    [__DynamicallyInvokable]
    public void ReplaceAll(params object[] content) => this.ReplaceAll((object) content);

    [__DynamicallyInvokable]
    public void ReplaceAttributes(object content)
    {
      content = XContainer.GetContentSnapshot(content);
      this.RemoveAttributes();
      this.Add(content);
    }

    [__DynamicallyInvokable]
    public void ReplaceAttributes(params object[] content) => this.ReplaceAttributes((object) content);

    public void Save(string fileName) => this.Save(fileName, this.GetSaveOptionsFromAnnotations());

    public void Save(string fileName, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      using (XmlWriter writer = XmlWriter.Create(fileName, xmlWriterSettings))
        this.Save(writer);
    }

    [__DynamicallyInvokable]
    public void Save(Stream stream) => this.Save(stream, this.GetSaveOptionsFromAnnotations());

    [__DynamicallyInvokable]
    public void Save(Stream stream, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
        this.Save(writer);
    }

    [__DynamicallyInvokable]
    public void Save(TextWriter textWriter) => this.Save(textWriter, this.GetSaveOptionsFromAnnotations());

    [__DynamicallyInvokable]
    public void Save(TextWriter textWriter, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      using (XmlWriter writer = XmlWriter.Create(textWriter, xmlWriterSettings))
        this.Save(writer);
    }

    [__DynamicallyInvokable]
    public void Save(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WriteStartDocument();
      this.WriteTo(writer);
      writer.WriteEndDocument();
    }

    [__DynamicallyInvokable]
    public void SetAttributeValue(XName name, object value)
    {
      XAttribute a = this.Attribute(name);
      if (value == null)
      {
        if (a == null)
          return;
        this.RemoveAttribute(a);
      }
      else if (a != null)
        a.Value = XContainer.GetStringValue(value);
      else
        this.AppendAttribute(new XAttribute(name, value));
    }

    [__DynamicallyInvokable]
    public void SetElementValue(XName name, object value)
    {
      XElement n = this.Element(name);
      if (value == null)
      {
        if (n == null)
          return;
        this.RemoveNode((XNode) n);
      }
      else if (n != null)
        n.Value = XContainer.GetStringValue(value);
      else
        this.AddNode((XNode) new XElement(name, (object) XContainer.GetStringValue(value)));
    }

    [__DynamicallyInvokable]
    public void SetValue(object value) => this.Value = value != null ? XContainer.GetStringValue(value) : throw new ArgumentNullException(nameof (value));

    [__DynamicallyInvokable]
    public override void WriteTo(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      new ElementWriter(writer).WriteElement(this);
    }

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator string(XElement element) => element?.Value;

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator bool(XElement element)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      return XmlConvert.ToBoolean(element.Value.ToLower(CultureInfo.InvariantCulture));
    }

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator bool?(XElement element) => element == null ? new bool?() : new bool?(XmlConvert.ToBoolean(element.Value.ToLower(CultureInfo.InvariantCulture)));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator int(XElement element) => element != null ? XmlConvert.ToInt32(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator int?(XElement element) => element == null ? new int?() : new int?(XmlConvert.ToInt32(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator uint(XElement element) => element != null ? XmlConvert.ToUInt32(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator uint?(XElement element) => element == null ? new uint?() : new uint?(XmlConvert.ToUInt32(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator long(XElement element) => element != null ? XmlConvert.ToInt64(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator long?(XElement element) => element == null ? new long?() : new long?(XmlConvert.ToInt64(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator ulong(XElement element) => element != null ? XmlConvert.ToUInt64(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator ulong?(XElement element) => element == null ? new ulong?() : new ulong?(XmlConvert.ToUInt64(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator float(XElement element) => element != null ? XmlConvert.ToSingle(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator float?(XElement element) => element == null ? new float?() : new float?(XmlConvert.ToSingle(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator double(XElement element) => element != null ? XmlConvert.ToDouble(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator double?(XElement element) => element == null ? new double?() : new double?(XmlConvert.ToDouble(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator Decimal(XElement element) => element != null ? XmlConvert.ToDecimal(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator Decimal?(XElement element) => element == null ? new Decimal?() : new Decimal?(XmlConvert.ToDecimal(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator DateTime(XElement element)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      return DateTime.Parse(element.Value, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator DateTime?(XElement element) => element == null ? new DateTime?() : new DateTime?(DateTime.Parse(element.Value, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator DateTimeOffset(XElement element) => element != null ? XmlConvert.ToDateTimeOffset(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator DateTimeOffset?(XElement element) => element == null ? new DateTimeOffset?() : new DateTimeOffset?(XmlConvert.ToDateTimeOffset(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator TimeSpan(XElement element) => element != null ? XmlConvert.ToTimeSpan(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator TimeSpan?(XElement element) => element == null ? new TimeSpan?() : new TimeSpan?(XmlConvert.ToTimeSpan(element.Value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator Guid(XElement element) => element != null ? XmlConvert.ToGuid(element.Value) : throw new ArgumentNullException(nameof (element));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator Guid?(XElement element) => element == null ? new Guid?() : new Guid?(XmlConvert.ToGuid(element.Value));

    [__DynamicallyInvokable]
    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    [__DynamicallyInvokable]
    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if (this.parent != null || this.annotations != null || this.content != null || this.lastAttr != null)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_DeserializeInstance"));
      if (reader.MoveToContent() != XmlNodeType.Element)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_ExpectedNodeType", (object) XmlNodeType.Element, (object) reader.NodeType));
      this.ReadElementFrom(reader, LoadOptions.None);
    }

    [__DynamicallyInvokable]
    void IXmlSerializable.WriteXml(XmlWriter writer) => this.WriteTo(writer);

    internal override void AddAttribute(XAttribute a)
    {
      if (this.Attribute(a.Name) != null)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_DuplicateAttribute"));
      if (a.parent != null)
        a = new XAttribute(a);
      this.AppendAttribute(a);
    }

    internal override void AddAttributeSkipNotify(XAttribute a)
    {
      if (this.Attribute(a.Name) != null)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_DuplicateAttribute"));
      if (a.parent != null)
        a = new XAttribute(a);
      this.AppendAttributeSkipNotify(a);
    }

    internal void AppendAttribute(XAttribute a)
    {
      bool flag = this.NotifyChanging((object) a, XObjectChangeEventArgs.Add);
      if (a.parent != null)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_ExternalCode"));
      this.AppendAttributeSkipNotify(a);
      if (!flag)
        return;
      this.NotifyChanged((object) a, XObjectChangeEventArgs.Add);
    }

    internal void AppendAttributeSkipNotify(XAttribute a)
    {
      a.parent = (XContainer) this;
      if (this.lastAttr == null)
      {
        a.next = a;
      }
      else
      {
        a.next = this.lastAttr.next;
        this.lastAttr.next = a;
      }
      this.lastAttr = a;
    }

    private bool AttributesEqual(XElement e)
    {
      XAttribute xattribute1 = this.lastAttr;
      XAttribute xattribute2 = e.lastAttr;
      if (xattribute1 != null && xattribute2 != null)
      {
        do
        {
          xattribute1 = xattribute1.next;
          xattribute2 = xattribute2.next;
          if (xattribute1.name != xattribute2.name || xattribute1.value != xattribute2.value)
            return false;
        }
        while (xattribute1 != this.lastAttr);
        return xattribute2 == e.lastAttr;
      }
      return xattribute1 == null && xattribute2 == null;
    }

    internal override XNode CloneNode() => (XNode) new XElement(this);

    internal override bool DeepEquals(XNode node) => node is XElement e && this.name == e.name && this.ContentsEqual((XContainer) e) && this.AttributesEqual(e);

    private IEnumerable<XAttribute> GetAttributes(XName name)
    {
      XElement xelement = this;
      XAttribute a = xelement.lastAttr;
      if (a != null)
      {
        do
        {
          a = a.next;
          if (name == (XName) null || a.name == name)
            yield return a;
        }
        while (a.parent == xelement && a != xelement.lastAttr);
      }
    }

    private string GetNamespaceOfPrefixInScope(string prefix, XElement outOfScope)
    {
      for (XElement xelement = this; xelement != outOfScope; xelement = xelement.parent as XElement)
      {
        XAttribute xattribute = xelement.lastAttr;
        if (xattribute != null)
        {
          do
          {
            xattribute = xattribute.next;
            if (xattribute.IsNamespaceDeclaration && xattribute.Name.LocalName == prefix)
              return xattribute.Value;
          }
          while (xattribute != xelement.lastAttr);
        }
      }
      return (string) null;
    }

    internal override int GetDeepHashCode()
    {
      int deepHashCode = this.name.GetHashCode() ^ this.ContentsHashCode();
      XAttribute xattribute = this.lastAttr;
      if (xattribute != null)
      {
        do
        {
          xattribute = xattribute.next;
          deepHashCode ^= xattribute.GetDeepHashCode();
        }
        while (xattribute != this.lastAttr);
      }
      return deepHashCode;
    }

    private void ReadElementFrom(XmlReader r, LoadOptions o)
    {
      if (r.ReadState != ReadState.Interactive)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_ExpectedInteractive"));
      this.name = XNamespace.Get(r.NamespaceURI).GetName(r.LocalName);
      if ((o & LoadOptions.SetBaseUri) != LoadOptions.None)
      {
        string baseUri = r.BaseURI;
        if (baseUri != null && baseUri.Length != 0)
          this.SetBaseUri(baseUri);
      }
      xmlLineInfo = (IXmlLineInfo) null;
      if ((o & LoadOptions.SetLineInfo) != LoadOptions.None && r is IXmlLineInfo xmlLineInfo && xmlLineInfo.HasLineInfo())
        this.SetLineInfo(xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
      if (r.MoveToFirstAttribute())
      {
        do
        {
          XAttribute a = new XAttribute(XNamespace.Get(r.Prefix.Length == 0 ? string.Empty : r.NamespaceURI).GetName(r.LocalName), (object) r.Value);
          if (xmlLineInfo != null && xmlLineInfo.HasLineInfo())
            a.SetLineInfo(xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
          this.AppendAttributeSkipNotify(a);
        }
        while (r.MoveToNextAttribute());
        r.MoveToElement();
      }
      if (!r.IsEmptyElement)
      {
        r.Read();
        this.ReadContentFrom(r, o);
      }
      r.Read();
    }

    internal void RemoveAttribute(XAttribute a)
    {
      bool flag = this.NotifyChanging((object) a, XObjectChangeEventArgs.Remove);
      if (a.parent != this)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_ExternalCode"));
      XAttribute xattribute = this.lastAttr;
      XAttribute next;
      while ((next = xattribute.next) != a)
        xattribute = next;
      if (xattribute == a)
      {
        this.lastAttr = (XAttribute) null;
      }
      else
      {
        if (this.lastAttr == a)
          this.lastAttr = xattribute;
        xattribute.next = a.next;
      }
      a.parent = (XContainer) null;
      a.next = (XAttribute) null;
      if (!flag)
        return;
      this.NotifyChanged((object) a, XObjectChangeEventArgs.Remove);
    }

    private void RemoveAttributesSkipNotify()
    {
      if (this.lastAttr == null)
        return;
      XAttribute xattribute = this.lastAttr;
      do
      {
        XAttribute next = xattribute.next;
        xattribute.parent = (XContainer) null;
        xattribute.next = (XAttribute) null;
        xattribute = next;
      }
      while (xattribute != this.lastAttr);
      this.lastAttr = (XAttribute) null;
    }

    internal void SetEndElementLineInfo(int lineNumber, int linePosition) => this.AddAnnotation((object) new LineInfoEndElementAnnotation(lineNumber, linePosition));

    internal override void ValidateNode(XNode node, XNode previous)
    {
      switch (node)
      {
        case XDocument _:
          throw new ArgumentException(Res.GetString("Argument_AddNode", (object) XmlNodeType.Document));
        case XDocumentType _:
          throw new ArgumentException(Res.GetString("Argument_AddNode", (object) XmlNodeType.DocumentType));
      }
    }
  }
}
