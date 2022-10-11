// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XStreamingElement
// Assembly: System.Private.Xml.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 67E106B6-1B7E-4540-BB80-26A3D9D4BC13
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Xml.Linq.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Xml.XDocument.xml

using System.Collections.Generic;
using System.Globalization;
using System.IO;


#nullable enable
namespace System.Xml.Linq
{
  /// <summary>Represents elements in an XML tree that supports deferred streaming output.</summary>
  public class XStreamingElement
  {

    #nullable disable
    internal XName name;
    internal object content;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XElement" /> class from the specified <see cref="T:System.Xml.Linq.XName" />.</summary>
    /// <param name="name">An <see cref="T:System.Xml.Linq.XName" /> that contains the name of the element.</param>
    public XStreamingElement(XName name) => this.name = !(name == (XName) null) ? name : throw new ArgumentNullException(nameof (name));

    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XStreamingElement" /> class with the specified name and content.</summary>
    /// <param name="name">An <see cref="T:System.Xml.Linq.XName" /> that contains the element name.</param>
    /// <param name="content">The contents of the element.</param>
    public XStreamingElement(XName name, object? content)
      : this(name)
    {
      object obj;
      if (!(content is List<object>))
      {
        obj = content;
      }
      else
      {
        obj = (object) new object[1];
        obj[0] = content;
      }
      this.content = obj;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XStreamingElement" /> class with the specified name and content.</summary>
    /// <param name="name">An <see cref="T:System.Xml.Linq.XName" /> that contains the element name.</param>
    /// <param name="content">The contents of the element.</param>
    public XStreamingElement(XName name, params object?[] content)
      : this(name)
    {
      this.content = (object) content;
    }

    /// <summary>Gets or sets the name of this streaming element.</summary>
    /// <returns>An <see cref="T:System.Xml.Linq.XName" /> that contains the name of this streaming element.</returns>
    public XName Name
    {
      get => this.name;
      set => this.name = !(value == (XName) null) ? value : throw new ArgumentNullException(nameof (value));
    }

    /// <summary>Adds the specified content as children to this <see cref="T:System.Xml.Linq.XStreamingElement" />.</summary>
    /// <param name="content">Content to be added to the streaming element.</param>
    public void Add(object? content)
    {
      if (content == null)
        return;
      if (!(this.content is List<object> objectList))
      {
        objectList = new List<object>();
        if (this.content != null)
          objectList.Add(this.content);
        this.content = (object) objectList;
      }
      objectList.Add(content);
    }

    /// <summary>Adds the specified content as children to this <see cref="T:System.Xml.Linq.XStreamingElement" />.</summary>
    /// <param name="content">Content to be added to the streaming element.</param>
    public void Add(params object?[] content) => this.Add((object) content);

    /// <summary>Outputs this <see cref="T:System.Xml.Linq.XStreamingElement" /> to the specified <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="stream">The stream to output this <see cref="T:System.Xml.Linq.XDocument" /> to.</param>
    public void Save(Stream stream) => this.Save(stream, SaveOptions.None);

    /// <summary>Outputs this <see cref="T:System.Xml.Linq.XStreamingElement" /> to the specified <see cref="T:System.IO.Stream" />, optionally specifying formatting behavior.</summary>
    /// <param name="stream">The stream to output this <see cref="T:System.Xml.Linq.XDocument" /> to.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> object that specifies formatting behavior.</param>
    public void Save(Stream stream, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
        this.Save(writer);
    }

    /// <summary>Serialize this streaming element to a <see cref="T:System.IO.TextWriter" />.</summary>
    /// <param name="textWriter">A <see cref="T:System.IO.TextWriter" /> that the <see cref="T:System.Xml.Linq.XStreamingElement" /> will be written to.</param>
    public void Save(TextWriter textWriter) => this.Save(textWriter, SaveOptions.None);

    /// <summary>Serialize this streaming element to a <see cref="T:System.IO.TextWriter" />, optionally disabling formatting.</summary>
    /// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> to output the XML to.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
    public void Save(TextWriter textWriter, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      using (XmlWriter writer = XmlWriter.Create(textWriter, xmlWriterSettings))
        this.Save(writer);
    }

    /// <summary>Serialize this streaming element to an <see cref="T:System.Xml.XmlWriter" />.</summary>
    /// <param name="writer">A <see cref="T:System.Xml.XmlWriter" /> that the <see cref="T:System.Xml.Linq.XElement" /> will be written to.</param>
    public void Save(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.WriteStartDocument();
      this.WriteTo(writer);
      writer.WriteEndDocument();
    }

    /// <summary>Serialize this streaming element to a file.</summary>
    /// <param name="fileName">A <see cref="T:System.String" /> that contains the name of the file.</param>
    public void Save(string fileName) => this.Save(fileName, SaveOptions.None);

    /// <summary>Serialize this streaming element to a file, optionally disabling formatting.</summary>
    /// <param name="fileName">A <see cref="T:System.String" /> that contains the name of the file.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> object that specifies formatting behavior.</param>
    public void Save(string fileName, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      using (XmlWriter writer = XmlWriter.Create(fileName, xmlWriterSettings))
        this.Save(writer);
    }

    /// <summary>Returns the formatted (indented) XML for this streaming element.</summary>
    /// <returns>A <see cref="T:System.String" /> containing the indented XML.</returns>
    public override string ToString() => this.GetXmlString(SaveOptions.None);

    /// <summary>Returns the XML for this streaming element, optionally disabling formatting.</summary>
    /// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
    /// <returns>A <see cref="T:System.String" /> containing the XML.</returns>
    public string ToString(SaveOptions options) => this.GetXmlString(options);

    /// <summary>Writes this streaming element to an <see cref="T:System.Xml.XmlWriter" />.</summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlWriter" /> into which this method will write.</param>
    public void WriteTo(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      new StreamingElementWriter(writer).WriteStreamingElement(this);
    }


    #nullable disable
    private string GetXmlString(SaveOptions o)
    {
      using (StringWriter output = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        if ((o & SaveOptions.DisableFormatting) == SaveOptions.None)
          settings.Indent = true;
        if ((o & SaveOptions.OmitDuplicateNamespaces) != SaveOptions.None)
          settings.NamespaceHandling |= NamespaceHandling.OmitDuplicates;
        using (XmlWriter writer = XmlWriter.Create((TextWriter) output, settings))
          this.WriteTo(writer);
        return output.ToString();
      }
    }
  }
}
