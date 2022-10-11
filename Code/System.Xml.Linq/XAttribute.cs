// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XAttribute
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

using MS.Internal.Xml.Linq.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace System.Xml.Linq
{
  [TypeDescriptionProvider(typeof (XTypeDescriptionProvider<XAttribute>))]
  [__DynamicallyInvokable]
  public class XAttribute : XObject
  {
    private static IEnumerable<XAttribute> emptySequence;
    internal XAttribute next;
    internal XName name;
    internal string value;

    [__DynamicallyInvokable]
    public static IEnumerable<XAttribute> EmptySequence
    {
      [__DynamicallyInvokable] get
      {
        if (XAttribute.emptySequence == null)
          XAttribute.emptySequence = (IEnumerable<XAttribute>) new XAttribute[0];
        return XAttribute.emptySequence;
      }
    }

    [__DynamicallyInvokable]
    public XAttribute(XName name, object value)
    {
      if (name == (XName) null)
        throw new ArgumentNullException(nameof (name));
      string str = value != null ? XContainer.GetStringValue(value) : throw new ArgumentNullException(nameof (value));
      XAttribute.ValidateAttribute(name, str);
      this.name = name;
      this.value = str;
    }

    [__DynamicallyInvokable]
    public XAttribute(XAttribute other)
    {
      this.name = other != null ? other.name : throw new ArgumentNullException(nameof (other));
      this.value = other.value;
    }

    [__DynamicallyInvokable]
    public bool IsNamespaceDeclaration
    {
      [__DynamicallyInvokable] get
      {
        string namespaceName = this.name.NamespaceName;
        return namespaceName.Length == 0 ? this.name.LocalName == "xmlns" : (object) namespaceName == (object) "http://www.w3.org/2000/xmlns/";
      }
    }

    [__DynamicallyInvokable]
    public XName Name
    {
      [__DynamicallyInvokable] get => this.name;
    }

    [__DynamicallyInvokable]
    public XAttribute NextAttribute
    {
      [__DynamicallyInvokable] get => this.parent == null || ((XElement) this.parent).lastAttr == this ? (XAttribute) null : this.next;
    }

    [__DynamicallyInvokable]
    public override XmlNodeType NodeType
    {
      [__DynamicallyInvokable] get => XmlNodeType.Attribute;
    }

    [__DynamicallyInvokable]
    public XAttribute PreviousAttribute
    {
      [__DynamicallyInvokable] get
      {
        if (this.parent == null)
          return (XAttribute) null;
        XAttribute xattribute = ((XElement) this.parent).lastAttr;
        while (xattribute.next != this)
          xattribute = xattribute.next;
        return xattribute == ((XElement) this.parent).lastAttr ? (XAttribute) null : xattribute;
      }
    }

    [__DynamicallyInvokable]
    public string Value
    {
      [__DynamicallyInvokable] get => this.value;
      [__DynamicallyInvokable] set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        XAttribute.ValidateAttribute(this.name, value);
        bool flag = this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
        this.value = value;
        if (!flag)
          return;
        this.NotifyChanged((object) this, XObjectChangeEventArgs.Value);
      }
    }

    [__DynamicallyInvokable]
    public void Remove()
    {
      if (this.parent == null)
        throw new InvalidOperationException(Res.GetString("InvalidOperation_MissingParent"));
      ((XElement) this.parent).RemoveAttribute(this);
    }

    [__DynamicallyInvokable]
    public void SetValue(object value) => this.Value = value != null ? XContainer.GetStringValue(value) : throw new ArgumentNullException(nameof (value));

    [__DynamicallyInvokable]
    public override string ToString()
    {
      using (StringWriter output = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        using (XmlWriter xmlWriter = XmlWriter.Create((TextWriter) output, new XmlWriterSettings()
        {
          ConformanceLevel = ConformanceLevel.Fragment
        }))
          xmlWriter.WriteAttributeString(this.GetPrefixOfNamespace(this.name.Namespace), this.name.LocalName, this.name.NamespaceName, this.value);
        return output.ToString().Trim();
      }
    }

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator string(XAttribute attribute) => attribute?.value;

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator bool(XAttribute attribute)
    {
      if (attribute == null)
        throw new ArgumentNullException(nameof (attribute));
      return XmlConvert.ToBoolean(attribute.value.ToLower(CultureInfo.InvariantCulture));
    }

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator bool?(XAttribute attribute) => attribute == null ? new bool?() : new bool?(XmlConvert.ToBoolean(attribute.value.ToLower(CultureInfo.InvariantCulture)));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator int(XAttribute attribute) => attribute != null ? XmlConvert.ToInt32(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator int?(XAttribute attribute) => attribute == null ? new int?() : new int?(XmlConvert.ToInt32(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator uint(XAttribute attribute) => attribute != null ? XmlConvert.ToUInt32(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator uint?(XAttribute attribute) => attribute == null ? new uint?() : new uint?(XmlConvert.ToUInt32(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator long(XAttribute attribute) => attribute != null ? XmlConvert.ToInt64(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator long?(XAttribute attribute) => attribute == null ? new long?() : new long?(XmlConvert.ToInt64(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator ulong(XAttribute attribute) => attribute != null ? XmlConvert.ToUInt64(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator ulong?(XAttribute attribute) => attribute == null ? new ulong?() : new ulong?(XmlConvert.ToUInt64(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator float(XAttribute attribute) => attribute != null ? XmlConvert.ToSingle(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator float?(XAttribute attribute) => attribute == null ? new float?() : new float?(XmlConvert.ToSingle(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator double(XAttribute attribute) => attribute != null ? XmlConvert.ToDouble(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator double?(XAttribute attribute) => attribute == null ? new double?() : new double?(XmlConvert.ToDouble(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator Decimal(XAttribute attribute) => attribute != null ? XmlConvert.ToDecimal(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator Decimal?(XAttribute attribute) => attribute == null ? new Decimal?() : new Decimal?(XmlConvert.ToDecimal(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator DateTime(XAttribute attribute)
    {
      if (attribute == null)
        throw new ArgumentNullException(nameof (attribute));
      return DateTime.Parse(attribute.value, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator DateTime?(XAttribute attribute) => attribute == null ? new DateTime?() : new DateTime?(DateTime.Parse(attribute.value, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator DateTimeOffset(XAttribute attribute) => attribute != null ? XmlConvert.ToDateTimeOffset(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator DateTimeOffset?(XAttribute attribute) => attribute == null ? new DateTimeOffset?() : new DateTimeOffset?(XmlConvert.ToDateTimeOffset(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator TimeSpan(XAttribute attribute) => attribute != null ? XmlConvert.ToTimeSpan(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator TimeSpan?(XAttribute attribute) => attribute == null ? new TimeSpan?() : new TimeSpan?(XmlConvert.ToTimeSpan(attribute.value));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator Guid(XAttribute attribute) => attribute != null ? XmlConvert.ToGuid(attribute.value) : throw new ArgumentNullException(nameof (attribute));

    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public static explicit operator Guid?(XAttribute attribute) => attribute == null ? new Guid?() : new Guid?(XmlConvert.ToGuid(attribute.value));

    internal int GetDeepHashCode() => this.name.GetHashCode() ^ this.value.GetHashCode();

    internal string GetPrefixOfNamespace(XNamespace ns)
    {
      string namespaceName = ns.NamespaceName;
      if (namespaceName.Length == 0)
        return string.Empty;
      if (this.parent != null)
        return ((XElement) this.parent).GetPrefixOfNamespace(ns);
      if ((object) namespaceName == (object) "http://www.w3.org/XML/1998/namespace")
        return "xml";
      return (object) namespaceName == (object) "http://www.w3.org/2000/xmlns/" ? "xmlns" : (string) null;
    }

    private static void ValidateAttribute(XName name, string value)
    {
      string namespaceName = name.NamespaceName;
      if ((object) namespaceName == (object) "http://www.w3.org/2000/xmlns/")
      {
        if (value.Length == 0)
          throw new ArgumentException(Res.GetString("Argument_NamespaceDeclarationPrefixed", (object) name.LocalName));
        if (value == "http://www.w3.org/XML/1998/namespace")
        {
          if (name.LocalName != "xml")
            throw new ArgumentException(Res.GetString("Argument_NamespaceDeclarationXml"));
        }
        else
        {
          if (value == "http://www.w3.org/2000/xmlns/")
            throw new ArgumentException(Res.GetString("Argument_NamespaceDeclarationXmlns"));
          string localName = name.LocalName;
          if (localName == "xml")
            throw new ArgumentException(Res.GetString("Argument_NamespaceDeclarationXml"));
          if (localName == "xmlns")
            throw new ArgumentException(Res.GetString("Argument_NamespaceDeclarationXmlns"));
        }
      }
      else
      {
        if (namespaceName.Length != 0 || !(name.LocalName == "xmlns"))
          return;
        if (value == "http://www.w3.org/XML/1998/namespace")
          throw new ArgumentException(Res.GetString("Argument_NamespaceDeclarationXml"));
        if (value == "http://www.w3.org/2000/xmlns/")
          throw new ArgumentException(Res.GetString("Argument_NamespaceDeclarationXmlns"));
      }
    }
  }
}
