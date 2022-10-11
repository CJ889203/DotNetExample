// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XDocument
// Assembly: System.Private.Xml.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 67E106B6-1B7E-4540-BB80-26A3D9D4BC13
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Xml.Linq.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Xml.XDocument.xml

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Xml.Linq
{
  /// <summary>Represents an XML document. For the components and usage of an <see cref="T:System.Xml.Linq.XDocument" /> object, see XDocument Class Overview.</summary>
  public class XDocument : XContainer
  {

    #nullable disable
    private XDeclaration _declaration;

    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDocument" /> class.</summary>
    public XDocument()
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDocument" /> class with the specified content.</summary>
    /// <param name="content">A parameter list of content objects to add to this document.</param>
    public XDocument(params object?[] content)
      : this()
    {
      this.AddContentSkipNotify((object) content);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDocument" /> class with the specified <see cref="T:System.Xml.Linq.XDeclaration" /> and content.</summary>
    /// <param name="declaration">An <see cref="T:System.Xml.Linq.XDeclaration" /> for the document.</param>
    /// <param name="content">The content of the document.</param>
    public XDocument(XDeclaration? declaration, params object?[] content)
      : this(content)
    {
      this._declaration = declaration;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Xml.Linq.XDocument" /> class from an existing <see cref="T:System.Xml.Linq.XDocument" /> object.</summary>
    /// <param name="other">The <see cref="T:System.Xml.Linq.XDocument" /> object that will be copied.</param>
    public XDocument(XDocument other)
      : base((XContainer) other)
    {
      if (other._declaration == null)
        return;
      this._declaration = new XDeclaration(other._declaration);
    }

    /// <summary>Gets or sets the XML declaration for this document.</summary>
    /// <returns>An <see cref="T:System.Xml.Linq.XDeclaration" /> that contains the XML declaration for this document.</returns>
    public XDeclaration? Declaration
    {
      get => this._declaration;
      set => this._declaration = value;
    }

    /// <summary>Gets the Document Type Definition (DTD) for this document.</summary>
    /// <returns>A <see cref="T:System.Xml.Linq.XDocumentType" /> that contains the DTD for this document.</returns>
    public XDocumentType? DocumentType => this.GetFirstNode<XDocumentType>();

    /// <summary>Gets the node type for this node.</summary>
    /// <returns>The node type. For <see cref="T:System.Xml.Linq.XDocument" /> objects, this value is <see cref="F:System.Xml.XmlNodeType.Document" />.</returns>
    public override XmlNodeType NodeType => XmlNodeType.Document;

    /// <summary>Gets the root element of the XML Tree for this document.</summary>
    /// <returns>The root <see cref="T:System.Xml.Linq.XElement" /> of the XML tree.</returns>
    public XElement? Root => this.GetFirstNode<XElement>();

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a file.</summary>
    /// <param name="uri">A URI string that references the file to load into a new <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the contents of the specified file.</returns>
    public static XDocument Load(string uri) => XDocument.Load(uri, LoadOptions.None);

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a file, optionally preserving white space, setting the base URI, and retaining line information.</summary>
    /// <param name="uri">A URI string that references the file to load into a new <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the contents of the specified file.</returns>
    public static XDocument Load(string uri, LoadOptions options)
    {
      XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
      using (XmlReader reader = XmlReader.Create(uri, xmlReaderSettings))
        return XDocument.Load(reader, options);
    }

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> instance by using the specified stream.</summary>
    /// <param name="stream">The stream that contains the XML data.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> object that reads the data that is contained in the stream.</returns>
    public static XDocument Load(Stream stream) => XDocument.Load(stream, LoadOptions.None);

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> instance by using the specified stream, optionally preserving white space, setting the base URI, and retaining line information.</summary>
    /// <param name="stream">The stream containing the XML data.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies whether to load base URI and line information.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> object that reads the data that is contained in the stream.</returns>
    public static XDocument Load(Stream stream, LoadOptions options)
    {
      XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
      using (XmlReader reader = XmlReader.Create(stream, xmlReaderSettings))
        return XDocument.Load(reader, options);
    }

    /// <summary>Asynchronously creates a new <see cref="T:System.Xml.Linq.XDocument" /> and initializes its underlying XML tree from the specified stream, optionally preserving white space.</summary>
    /// <param name="stream">A stream containing the raw XML to read into the newly created <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <param name="options">A set of load options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A new XDocument containing the contents of the specified <see cref="T:System.IO.Stream" />.</returns>
    public static async Task<XDocument> LoadAsync(
      Stream stream,
      LoadOptions options,
      CancellationToken cancellationToken)
    {
      XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
      xmlReaderSettings.Async = true;
      XDocument xdocument;
      using (XmlReader r = XmlReader.Create(stream, xmlReaderSettings))
        xdocument = await XDocument.LoadAsync(r, options, cancellationToken).ConfigureAwait(false);
      return xdocument;
    }

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a <see cref="T:System.IO.TextReader" />.</summary>
    /// <param name="textReader">A <see cref="T:System.IO.TextReader" /> that contains the content for the <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the contents of the specified <see cref="T:System.IO.TextReader" />.</returns>
    public static XDocument Load(TextReader textReader) => XDocument.Load(textReader, LoadOptions.None);

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a <see cref="T:System.IO.TextReader" />, optionally preserving white space, setting the base URI, and retaining line information.</summary>
    /// <param name="textReader">A <see cref="T:System.IO.TextReader" /> that contains the content for the <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the XML that was read from the specified <see cref="T:System.IO.TextReader" />.</returns>
    public static XDocument Load(TextReader textReader, LoadOptions options)
    {
      XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
      using (XmlReader reader = XmlReader.Create(textReader, xmlReaderSettings))
        return XDocument.Load(reader, options);
    }

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> and initializes its underlying XML tree using the specified <see cref="T:System.IO.TextReader" /> parameter, optionally preserving white space.</summary>
    /// <param name="textReader">A reader that contains the raw XML to read into the newly created <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <param name="options">A set of load options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A new XDocument containing the contents of the specified <see cref="T:System.IO.TextReader" />.</returns>
    public static async Task<XDocument> LoadAsync(
      TextReader textReader,
      LoadOptions options,
      CancellationToken cancellationToken)
    {
      XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
      xmlReaderSettings.Async = true;
      XDocument xdocument;
      using (XmlReader r = XmlReader.Create(textReader, xmlReaderSettings))
        xdocument = await XDocument.LoadAsync(r, options, cancellationToken).ConfigureAwait(false);
      return xdocument;
    }

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from an <see cref="T:System.Xml.XmlReader" />.</summary>
    /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> that contains the content for the <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the contents of the specified <see cref="T:System.Xml.XmlReader" />.</returns>
    public static XDocument Load(XmlReader reader) => XDocument.Load(reader, LoadOptions.None);

    /// <summary>Loads an <see cref="T:System.Xml.Linq.XDocument" /> from an <see cref="T:System.Xml.XmlReader" />, optionally setting the base URI, and retaining line information.</summary>
    /// <param name="reader">A <see cref="T:System.Xml.XmlReader" /> that will be read for the content of the <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies whether to load base URI and line information.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> that contains the XML that was read from the specified <see cref="T:System.Xml.XmlReader" />.</returns>
    public static XDocument Load(XmlReader reader, LoadOptions options)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if (reader.ReadState == ReadState.Initial)
        reader.Read();
      XDocument xdocument = XDocument.InitLoad(reader, options);
      xdocument.ReadContentFrom(reader, options);
      if (!reader.EOF)
        throw new InvalidOperationException(SR.InvalidOperation_ExpectedEndOfFile);
      return xdocument.Root != null ? xdocument : throw new InvalidOperationException(SR.InvalidOperation_MissingRoot);
    }

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> containing the contents of the specified <see cref="T:System.Xml.XmlReader" />.</summary>
    /// <param name="reader">A reader containing the XML to be read into the new <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <param name="options">A set of load options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A new XDocument containing the contents of the specified <see cref="T:System.Xml.XmlReader" />.</returns>
    public static Task<XDocument> LoadAsync(
      XmlReader reader,
      LoadOptions options,
      CancellationToken cancellationToken)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      return cancellationToken.IsCancellationRequested ? Task.FromCanceled<XDocument>(cancellationToken) : XDocument.LoadAsyncInternal(reader, options, cancellationToken);
    }


    #nullable disable
    private static async Task<XDocument> LoadAsyncInternal(
      XmlReader reader,
      LoadOptions options,
      CancellationToken cancellationToken)
    {
      if (reader.ReadState == ReadState.Initial)
      {
        int num = await reader.ReadAsync().ConfigureAwait(false) ? 1 : 0;
      }
      XDocument d = XDocument.InitLoad(reader, options);
      await d.ReadContentFromAsync(reader, options, cancellationToken).ConfigureAwait(false);
      if (!reader.EOF)
        throw new InvalidOperationException(SR.InvalidOperation_ExpectedEndOfFile);
      XDocument xdocument = d.Root != null ? d : throw new InvalidOperationException(SR.InvalidOperation_MissingRoot);
      d = (XDocument) null;
      return xdocument;
    }

    private static XDocument InitLoad(XmlReader reader, LoadOptions options)
    {
      XDocument xdocument = new XDocument();
      if ((options & LoadOptions.SetBaseUri) != LoadOptions.None)
      {
        string baseUri = reader.BaseURI;
        if (!string.IsNullOrEmpty(baseUri))
          xdocument.SetBaseUri(baseUri);
      }
      if ((options & LoadOptions.SetLineInfo) != LoadOptions.None && reader is IXmlLineInfo xmlLineInfo && xmlLineInfo.HasLineInfo())
        xdocument.SetLineInfo(xmlLineInfo.LineNumber, xmlLineInfo.LinePosition);
      if (reader.NodeType == XmlNodeType.XmlDeclaration)
        xdocument.Declaration = new XDeclaration(reader);
      return xdocument;
    }


    #nullable enable
    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a string.</summary>
    /// <param name="text">A string that contains XML.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> populated from the string that contains XML.</returns>
    public static XDocument Parse(string text) => XDocument.Parse(text, LoadOptions.None);

    /// <summary>Creates a new <see cref="T:System.Xml.Linq.XDocument" /> from a string, optionally preserving white space, setting the base URI, and retaining line information.</summary>
    /// <param name="text">A string that contains XML.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.LoadOptions" /> that specifies white space behavior, and whether to load base URI and line information.</param>
    /// <returns>An <see cref="T:System.Xml.Linq.XDocument" /> populated from the string that contains XML.</returns>
    public static XDocument Parse(string text, LoadOptions options)
    {
      using (StringReader input = new StringReader(text))
      {
        XmlReaderSettings xmlReaderSettings = XNode.GetXmlReaderSettings(options);
        using (XmlReader reader = XmlReader.Create((TextReader) input, xmlReaderSettings))
          return XDocument.Load(reader, options);
      }
    }

    /// <summary>Outputs this <see cref="T:System.Xml.Linq.XDocument" /> to the specified <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="stream">The stream to output this <see cref="T:System.Xml.Linq.XDocument" /> to.</param>
    public void Save(Stream stream) => this.Save(stream, this.GetSaveOptionsFromAnnotations());

    /// <summary>Outputs this <see cref="T:System.Xml.Linq.XDocument" /> to the specified <see cref="T:System.IO.Stream" />, optionally specifying formatting behavior.</summary>
    /// <param name="stream">The stream to output this <see cref="T:System.Xml.Linq.XDocument" /> to.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
    public void Save(Stream stream, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      if (this._declaration != null)
      {
        if (!string.IsNullOrEmpty(this._declaration.Encoding))
        {
          try
          {
            xmlWriterSettings.Encoding = Encoding.GetEncoding(this._declaration.Encoding);
          }
          catch (ArgumentException ex)
          {
          }
        }
      }
      using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
        this.Save(writer);
    }

    /// <summary>Output this <see cref="T:System.Xml.Linq.XDocument" /> to a <see cref="T:System.IO.Stream" />.</summary>
    /// <param name="stream">The stream to write the XML to.</param>
    /// <param name="options">A set of load options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveAsync(
      Stream stream,
      SaveOptions options,
      CancellationToken cancellationToken)
    {
      XDocument xdocument = this;
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      xmlWriterSettings.Async = true;
      if (xdocument._declaration != null)
      {
        if (!string.IsNullOrEmpty(xdocument._declaration.Encoding))
        {
          try
          {
            xmlWriterSettings.Encoding = Encoding.GetEncoding(xdocument._declaration.Encoding);
          }
          catch (ArgumentException ex)
          {
          }
        }
      }
      XmlWriter w = XmlWriter.Create(stream, xmlWriterSettings);
      await using (w.ConfigureAwait(false))
      {
        await xdocument.WriteToAsync(w, cancellationToken).ConfigureAwait(false);
        await w.FlushAsync().ConfigureAwait(false);
      }
      w = (XmlWriter) null;
    }

    /// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to a <see cref="T:System.IO.TextWriter" />.</summary>
    /// <param name="textWriter">A <see cref="T:System.IO.TextWriter" /> that the <see cref="T:System.Xml.Linq.XDocument" /> will be written to.</param>
    public void Save(TextWriter textWriter) => this.Save(textWriter, this.GetSaveOptionsFromAnnotations());

    /// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to a <see cref="T:System.IO.TextWriter" />, optionally disabling formatting.</summary>
    /// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> to output the XML to.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
    public void Save(TextWriter textWriter, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      using (XmlWriter writer = XmlWriter.Create(textWriter, xmlWriterSettings))
        this.Save(writer);
    }

    /// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to an <see cref="T:System.Xml.XmlWriter" />.</summary>
    /// <param name="writer">A <see cref="T:System.Xml.XmlWriter" /> that the <see cref="T:System.Xml.Linq.XDocument" /> will be written to.</param>
    public void Save(XmlWriter writer) => this.WriteTo(writer);

    /// <summary>Writes this <see cref="T:System.Xml.Linq.XDocument" /> to a <see cref="T:System.IO.TextWriter" />.</summary>
    /// <param name="textWriter">The text writer to output the XML to.</param>
    /// <param name="options">A set of load options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveAsync(
      TextWriter textWriter,
      SaveOptions options,
      CancellationToken cancellationToken)
    {
      XDocument xdocument = this;
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      xmlWriterSettings.Async = true;
      XmlWriter w = XmlWriter.Create(textWriter, xmlWriterSettings);
      await using (w.ConfigureAwait(false))
      {
        await xdocument.WriteToAsync(w, cancellationToken).ConfigureAwait(false);
        await w.FlushAsync().ConfigureAwait(false);
      }
      w = (XmlWriter) null;
    }

    /// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to a file, overwriting an existing file, if it exists.</summary>
    /// <param name="fileName">A string that contains the name of the file.</param>
    public void Save(string fileName) => this.Save(fileName, this.GetSaveOptionsFromAnnotations());

    /// <summary>Writes this <see cref="T:System.Xml.Linq.XDocument" /> to an <see cref="T:System.Xml.XmlWriter" />.</summary>
    /// <param name="writer">The writer to output the XML to.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public Task SaveAsync(XmlWriter writer, CancellationToken cancellationToken) => this.WriteToAsync(writer, cancellationToken);

    /// <summary>Serialize this <see cref="T:System.Xml.Linq.XDocument" /> to a file, optionally disabling formatting.</summary>
    /// <param name="fileName">A string that contains the name of the file.</param>
    /// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
    public void Save(string fileName, SaveOptions options)
    {
      XmlWriterSettings xmlWriterSettings = XNode.GetXmlWriterSettings(options);
      if (this._declaration != null)
      {
        if (!string.IsNullOrEmpty(this._declaration.Encoding))
        {
          try
          {
            xmlWriterSettings.Encoding = Encoding.GetEncoding(this._declaration.Encoding);
          }
          catch (ArgumentException ex)
          {
          }
        }
      }
      using (XmlWriter writer = XmlWriter.Create(fileName, xmlWriterSettings))
        this.Save(writer);
    }

    /// <summary>Write this document to an <see cref="T:System.Xml.XmlWriter" />.</summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlWriter" /> into which this method will write.</param>
    public override void WriteTo(XmlWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (this._declaration != null && this._declaration.Standalone == "yes")
        writer.WriteStartDocument(true);
      else if (this._declaration != null && this._declaration.Standalone == "no")
        writer.WriteStartDocument(false);
      else
        writer.WriteStartDocument();
      this.WriteContentTo(writer);
      writer.WriteEndDocument();
    }

    /// <summary>Writes this XDocument's underlying XML tree to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
    /// <param name="writer">The writer to output the content of this <see cref="T:System.Xml.Linq.XDocument" />.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task representing the asynchronous write operation.</returns>
    public override Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      return cancellationToken.IsCancellationRequested ? Task.FromCanceled(cancellationToken) : this.WriteToAsyncInternal(writer, cancellationToken);
    }


    #nullable disable
    private async Task WriteToAsyncInternal(
      XmlWriter writer,
      CancellationToken cancellationToken)
    {
      XDocument xdocument = this;
      await (xdocument._declaration == null || !(xdocument._declaration.Standalone == "yes") ? (xdocument._declaration == null || !(xdocument._declaration.Standalone == "no") ? writer.WriteStartDocumentAsync() : writer.WriteStartDocumentAsync(false)) : writer.WriteStartDocumentAsync(true)).ConfigureAwait(false);
      await xdocument.WriteContentToAsync(writer, cancellationToken).ConfigureAwait(false);
      await writer.WriteEndDocumentAsync().ConfigureAwait(false);
    }

    internal override void AddAttribute(XAttribute a) => throw new ArgumentException(SR.Argument_AddAttribute);

    internal override void AddAttributeSkipNotify(XAttribute a) => throw new ArgumentException(SR.Argument_AddAttribute);

    internal override XNode CloneNode() => (XNode) new XDocument(this);

    internal override bool DeepEquals(XNode node) => node is XDocument e && this.ContentsEqual((XContainer) e);

    internal override int GetDeepHashCode() => this.ContentsHashCode();

    private T GetFirstNode<T>() where T : XNode
    {
      if (this.content is XNode xnode)
      {
        do
        {
          xnode = xnode.next;
          if (xnode is T firstNode)
            return firstNode;
        }
        while (xnode != this.content);
      }
      return default (T);
    }

    internal static bool IsWhitespace(string s)
    {
      foreach (char ch in s)
      {
        switch (ch)
        {
          case '\t':
          case '\n':
          case '\r':
          case ' ':
            continue;
          default:
            return false;
        }
      }
      return true;
    }

    internal override void ValidateNode(XNode node, XNode previous)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          this.ValidateDocument(previous, XmlNodeType.DocumentType, XmlNodeType.None);
          break;
        case XmlNodeType.Text:
          this.ValidateString(((XText) node).Value);
          break;
        case XmlNodeType.CDATA:
          throw new ArgumentException(SR.Format(SR.Argument_AddNode, (object) XmlNodeType.CDATA));
        case XmlNodeType.Document:
          throw new ArgumentException(SR.Format(SR.Argument_AddNode, (object) XmlNodeType.Document));
        case XmlNodeType.DocumentType:
          this.ValidateDocument(previous, XmlNodeType.None, XmlNodeType.Element);
          break;
      }
    }

    private void ValidateDocument(XNode previous, XmlNodeType allowBefore, XmlNodeType allowAfter)
    {
      if (!(this.content is XNode xnode))
        return;
      if (previous == null)
        allowBefore = allowAfter;
      do
      {
        xnode = xnode.next;
        XmlNodeType nodeType = xnode.NodeType;
        switch (nodeType)
        {
          case XmlNodeType.Element:
          case XmlNodeType.DocumentType:
            allowBefore = nodeType == allowBefore ? XmlNodeType.None : throw new InvalidOperationException(SR.InvalidOperation_DocumentStructure);
            break;
        }
        if (xnode == previous)
          allowBefore = allowAfter;
      }
      while (xnode != this.content);
    }

    internal override void ValidateString(string s)
    {
      if (!XDocument.IsWhitespace(s))
        throw new ArgumentException(SR.Argument_AddNonWhitespace);
    }
  }
}
