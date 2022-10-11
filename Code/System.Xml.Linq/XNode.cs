// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XNode
// Assembly: System.Private.Xml.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 67E106B6-1B7E-4540-BB80-26A3D9D4BC13
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Xml.Linq.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Xml.XDocument.xml

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Xml.Linq
{
  /// <summary>Represents the abstract concept of a node (element, comment, document type, processing instruction, or text node) in the XML tree.</summary>
  public abstract class XNode : XObject
  {

    #nullable disable
    private static XNodeDocumentOrderComparer s_documentOrderComparer;
    private static XNodeEqualityComparer s_equalityComparer;
    internal XNode next;

    internal XNode()
    {
    }


    #nullable enable
    /// <summary>Gets the next sibling node of this node.</summary>
    /// <returns>The <see cref="T:System.Xml.Linq.XNode" /> that contains the next sibling node.</returns>
    public XNode? NextNode => this.parent != null && this != this.parent.content ? this.next : (XNode) null;

    /// <summary>Gets the previous sibling node of this node.</summary>
    /// <returns>The <see cref="T:System.Xml.Linq.XNode" /> that contains the previous sibling node.</returns>
    public XNode? PreviousNode
    {
      get
      {
        if (this.parent == null)
          return (XNode) null;
        XNode next = ((XNode) this.parent.content).next;
        XNode previousNode = (XNode) null;
        for (; next != this; next = next.next)
          previousNode = next;
        return previousNode;
      }
    }

    /// <summary>Gets a comparer that can compare the relative position of two nodes.</summary>
    /// <returns>An <see cref="T:System.Xml.Linq.XNodeDocumentOrderComparer" /> that can compare the relative position of two nodes.</returns>
    public static XNodeDocumentOrderComparer DocumentOrderComparer
    {
      get
      {
        if (XNode.s_documentOrderComparer == null)
          XNode.s_documentOrderComparer = new XNodeDocumentOrderComparer();
        return XNode.s_documentOrderComparer;
      }
    }

    /// <summary>Gets a comparer that can compare two nodes for value equality.</summary>
    /// <returns>A <see cref="T:System.Xml.Linq.XNodeEqualityComparer" /> that can compare two nodes for value equality.</returns>
    public static XNodeEqualityComparer EqualityComparer
    {
      get
      {
        if (XNode.s_equalityComparer == null)
          XNode.s_equalityComparer = new XNodeEqualityComparer();
        return XNode.s_equalityComparer;
      }
    }

    /// <summary>Adds the specified content immediately after this node.</summary>
    /// <param name="content">A content object that contains simple content or a collection of content objects to be added after this node.</param>
    /// <exception cref="T:System.InvalidOperationException">The parent is <see langword="null" />.</exception>
    public void AddAfterSelf(object? content)
    {
      if (this.parent == null)
        throw new InvalidOperationException(SR.InvalidOperation_MissingParent);
      new Inserter(this.parent, this).Add(content);
    }

    /// <summary>Adds the specified content immediately after this node.</summary>
    /// <param name="content">A parameter list of content objects.</param>
    /// <exception cref="T:System.InvalidOperationException">The parent is <see langword="null" />.</exception>
    public void AddAfterSelf(params object?[] content) => this.AddAfterSelf((object) content);

    /// <summary>Adds the specified content immediately before this node.</summary>
    /// <param name="content">A content object that contains simple content or a collection of content objects to be added before this node.</param>
    /// <exception cref="T:System.InvalidOperationException">The parent is <see langword="null" />.</exception>
    public void AddBeforeSelf(object? content)
    {
      XNode anchor = this.parent != null ? (XNode) this.parent.content : throw new InvalidOperationException(SR.InvalidOperation_MissingParent);
      while (anchor.next != this)
        anchor = anchor.next;
      if (anchor == this.parent.content)
        anchor = (XNode) null;
      new Inserter(this.parent, anchor).Add(content);
    }

    /// <summary>Adds the specified content immediately before this node.</summary>
    /// <param name="content">A parameter list of content objects.</param>
    /// <exception cref="T:System.InvalidOperationException">The parent is <see langword="null" />.</exception>
    public void AddBeforeSelf(params object?[] content) => this.AddBeforeSelf((object) content);

    /// <summary>Returns a collection of the ancestor elements of this node.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> of the ancestor elements of this node.</returns>
    public IEnumerable<XElement> Ancestors() => this.GetAncestors((XName) null, false);

    /// <summary>Returns a filtered collection of the ancestor elements of this node. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
    /// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> of the ancestor elements of this node. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.
    /// 
    /// The nodes in the returned collection are in reverse document order.
    /// 
    /// This method uses deferred execution.</returns>
    public IEnumerable<XElement> Ancestors(XName? name) => !(name != (XName) null) ? XElement.EmptySequence : this.GetAncestors(name, false);

    /// <summary>Compares two nodes to determine their relative XML document order.</summary>
    /// <param name="n1">First <see cref="T:System.Xml.Linq.XNode" /> to compare.</param>
    /// <param name="n2">Second <see cref="T:System.Xml.Linq.XNode" /> to compare.</param>
    /// <exception cref="T:System.InvalidOperationException">The two nodes do not share a common ancestor.</exception>
    /// <returns>An <see langword="int" /> containing 0 if the nodes are equal; -1 if <paramref name="n1" /> is before <paramref name="n2" />; 1 if <paramref name="n1" /> is after <paramref name="n2" />.</returns>
    public static int CompareDocumentOrder(XNode? n1, XNode? n2)
    {
      if (n1 == n2)
        return 0;
      if (n1 == null)
        return -1;
      if (n2 == null)
        return 1;
      if (n1.parent != n2.parent)
      {
        int num = 0;
        XNode xnode1 = n1;
        while (xnode1.parent != null)
        {
          xnode1 = (XNode) xnode1.parent;
          ++num;
        }
        XNode xnode2 = n2;
        while (xnode2.parent != null)
        {
          xnode2 = (XNode) xnode2.parent;
          --num;
        }
        if (xnode1 != xnode2)
          throw new InvalidOperationException(SR.InvalidOperation_MissingAncestor);
        if (num < 0)
        {
          do
          {
            n2 = (XNode) n2.parent;
            ++num;
          }
          while (num != 0);
          if (n1 == n2)
            return -1;
        }
        else if (num > 0)
        {
          do
          {
            n1 = (XNode) n1.parent;
            --num;
          }
          while (num != 0);
          if (n1 == n2)
            return 1;
        }
        for (; n1.parent != n2.parent; n2 = (XNode) n2.parent)
          n1 = (XNode) n1.parent;
      }
      else if (n1.parent == null)
        throw new InvalidOperationException(SR.InvalidOperation_MissingAncestor);
      XNode xnode = (XNode) n1.parent.content;
      do
      {
        xnode = xnode.next;
        if (xnode == n1)
          return -1;
      }
      while (xnode != n2);
      return 1;
    }

    /// <summary>Creates an <see cref="T:System.Xml.XmlReader" /> for this node.</summary>
    /// <returns>An <see cref="T:System.Xml.XmlReader" /> that can be used to read this node and its descendants.</returns>
    public XmlReader CreateReader() => (XmlReader) new XNodeReader(this, (XmlNameTable) null);

    /// <summary>Creates an <see cref="T:System.Xml.XmlReader" /> with the options specified by the <paramref name="readerOptions" /> parameter.</summary>
    /// <param name="readerOptions">A <see cref="T:System.Xml.Linq.ReaderOptions" /> object that specifies whether to omit duplicate namespaces.</param>
    /// <returns>An <see cref="T:System.Xml.XmlReader" /> object.</returns>
    public XmlReader CreateReader(ReaderOptions readerOptions) => (XmlReader) new XNodeReader(this, (XmlNameTable) null, readerOptions);

    /// <summary>Returns a collection of the sibling nodes after this node, in document order.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XNode" /> of the sibling nodes after this node, in document order.</returns>
    public IEnumerable<XNode> NodesAfterSelf()
    {
      XNode n = this;
      while (n.parent != null && n != n.parent.content)
      {
        n = n.next;
        yield return n;
      }
    }

    /// <summary>Returns a collection of the sibling nodes before this node, in document order.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XNode" /> of the sibling nodes before this node, in document order.</returns>
    public IEnumerable<XNode> NodesBeforeSelf()
    {
      XNode xnode = this;
      if (xnode.parent != null)
      {
        XNode n = (XNode) xnode.parent.content;
        do
        {
          n = n.next;
          if (n != xnode)
            yield return n;
          else
            break;
        }
        while (xnode.parent != null && xnode.parent == n.parent);
        n = (XNode) null;
      }
    }

    /// <summary>Returns a collection of the sibling elements after this node, in document order.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> of the sibling elements after this node, in document order.</returns>
    public IEnumerable<XElement> ElementsAfterSelf() => this.GetElementsAfterSelf((XName) null);

    /// <summary>Returns a filtered collection of the sibling elements after this node, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
    /// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> of the sibling elements after this node, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</returns>
    public IEnumerable<XElement> ElementsAfterSelf(XName? name) => !(name != (XName) null) ? XElement.EmptySequence : this.GetElementsAfterSelf(name);

    /// <summary>Returns a collection of the sibling elements before this node, in document order.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> of the sibling elements before this node, in document order.</returns>
    public IEnumerable<XElement> ElementsBeforeSelf() => this.GetElementsBeforeSelf((XName) null);

    /// <summary>Returns a filtered collection of the sibling elements before this node, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
    /// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> of the sibling elements before this node, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</returns>
    public IEnumerable<XElement> ElementsBeforeSelf(XName? name) => !(name != (XName) null) ? XElement.EmptySequence : this.GetElementsBeforeSelf(name);

    /// <summary>Determines if the current node appears after a specified node in terms of document order.</summary>
    /// <param name="node">The <see cref="T:System.Xml.Linq.XNode" /> to compare for document order.</param>
    /// <returns>
    /// <see langword="true" /> if this node appears after the specified node; otherwise <see langword="false" />.</returns>
    public bool IsAfter(XNode? node) => XNode.CompareDocumentOrder(this, node) > 0;

    /// <summary>Determines if the current node appears before a specified node in terms of document order.</summary>
    /// <param name="node">The <see cref="T:System.Xml.Linq.XNode" /> to compare for document order.</param>
    /// <returns>
    /// <see langword="true" /> if this node appears before the specified node; otherwise <see langword="false" />.</returns>
    public bool IsBefore(XNode? node) => XNode.CompareDocumentOrder(this, node) < 0;

    /// <summary>Creates an <see cref="T:System.Xml.Linq.XNode" /> from an <see cref="T:System.Xml.XmlReader" />.</summary>
    /// <param name="reader">An <see cref="T:System.Xml.XmlReader" /> positioned at the node to read into this <see cref="T:System.Xml.Linq.XNode" />.</param>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on a recognized node type.</exception>
    /// <exception cref="T:System.Xml.XmlException">The underlying <see cref="T:System.Xml.XmlReader" /> throws an exception.</exception>
    /// <returns>An <see cref="T:System.Xml.Linq.XNode" /> that contains the node and its descendant nodes that were read from the reader. The runtime type of the node is determined by the node type (<see cref="P:System.Xml.Linq.XObject.NodeType" />) of the first node encountered in the reader.</returns>
    public static XNode ReadFrom(XmlReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if (reader.ReadState != ReadState.Interactive)
        throw new InvalidOperationException(SR.InvalidOperation_ExpectedInteractive);
      switch (reader.NodeType)
      {
        case XmlNodeType.Element:
          return (XNode) new XElement(reader);
        case XmlNodeType.Text:
        case XmlNodeType.Whitespace:
        case XmlNodeType.SignificantWhitespace:
          return (XNode) new XText(reader);
        case XmlNodeType.CDATA:
          return (XNode) new XCData(reader);
        case XmlNodeType.ProcessingInstruction:
          return (XNode) new XProcessingInstruction(reader);
        case XmlNodeType.Comment:
          return (XNode) new XComment(reader);
        case XmlNodeType.DocumentType:
          return (XNode) new XDocumentType(reader);
        default:
          throw new InvalidOperationException(SR.Format(SR.InvalidOperation_UnexpectedNodeType, (object) reader.NodeType));
      }
    }

    /// <summary>Creates an <see cref="T:System.Xml.Linq.XNode" /> from an <see cref="T:System.Xml.XmlReader" />. The runtime type of the node is determined by the <see cref="P:System.Xml.Linq.XObject.NodeType" /> of the first node encountered in the reader.</summary>
    /// <param name="reader">A reader positioned at the node to read into this <see cref="T:System.Xml.Linq.XNode" />.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Xml.XmlReader" /> is not positioned on a recognized node type.</exception>
    /// <returns>An XNode that contains the nodes read from the reader.</returns>
    public static Task<XNode> ReadFromAsync(
      XmlReader reader,
      CancellationToken cancellationToken)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      return cancellationToken.IsCancellationRequested ? Task.FromCanceled<XNode>(cancellationToken) : XNode.ReadFromAsyncInternal(reader, cancellationToken);
    }


    #nullable disable
    private static async Task<XNode> ReadFromAsyncInternal(
      XmlReader reader,
      CancellationToken cancellationToken)
    {
      if (reader.ReadState != ReadState.Interactive)
        throw new InvalidOperationException(SR.InvalidOperation_ExpectedInteractive);
      XNode ret;
      switch (reader.NodeType)
      {
        case XmlNodeType.Element:
          return (XNode) await XElement.CreateAsync(reader, cancellationToken).ConfigureAwait(false);
        case XmlNodeType.Text:
        case XmlNodeType.Whitespace:
        case XmlNodeType.SignificantWhitespace:
          ret = (XNode) new XText(reader.Value);
          break;
        case XmlNodeType.CDATA:
          ret = (XNode) new XCData(reader.Value);
          break;
        case XmlNodeType.ProcessingInstruction:
          ret = (XNode) new XProcessingInstruction(reader.Name, reader.Value);
          break;
        case XmlNodeType.Comment:
          ret = (XNode) new XComment(reader.Value);
          break;
        case XmlNodeType.DocumentType:
          ret = (XNode) new XDocumentType(reader.Name, reader.GetAttribute("PUBLIC"), reader.GetAttribute("SYSTEM"), reader.Value);
          break;
        default:
          throw new InvalidOperationException(SR.Format(SR.InvalidOperation_UnexpectedNodeType, (object) reader.NodeType));
      }
      cancellationToken.ThrowIfCancellationRequested();
      int num = await reader.ReadAsync().ConfigureAwait(false) ? 1 : 0;
      return ret;
    }

    /// <summary>Removes this node from its parent.</summary>
    /// <exception cref="T:System.InvalidOperationException">The parent is <see langword="null" />.</exception>
    public void Remove()
    {
      if (this.parent == null)
        throw new InvalidOperationException(SR.InvalidOperation_MissingParent);
      this.parent.RemoveNode(this);
    }


    #nullable enable
    /// <summary>Replaces this node with the specified content.</summary>
    /// <param name="content">Content that replaces this node.</param>
    public void ReplaceWith(object? content)
    {
      XContainer parent = this.parent != null ? this.parent : throw new InvalidOperationException(SR.InvalidOperation_MissingParent);
      XNode anchor = (XNode) this.parent.content;
      while (anchor.next != this)
        anchor = anchor.next;
      if (anchor == this.parent.content)
        anchor = (XNode) null;
      this.parent.RemoveNode(this);
      if (anchor != null && anchor.parent != parent)
        throw new InvalidOperationException(SR.InvalidOperation_ExternalCode);
      new Inserter(parent, anchor).Add(content);
    }

    /// <summary>Replaces this node with the specified content.</summary>
    /// <param name="content">A parameter list of the new content.</param>
    public void ReplaceWith(params object?[] content) => this.ReplaceWith((object) content);

    /// <summary>Returns the indented XML for this node.</summary>
    /// <returns>A <see cref="T:System.String" /> containing the indented XML.</returns>
    public override string ToString() => this.GetXmlString(this.GetSaveOptionsFromAnnotations());

    /// <summary>Returns the XML for this node, optionally disabling formatting.</summary>
    /// <param name="options">A <see cref="T:System.Xml.Linq.SaveOptions" /> that specifies formatting behavior.</param>
    /// <returns>A <see cref="T:System.String" /> containing the XML.</returns>
    public string ToString(SaveOptions options) => this.GetXmlString(options);

    /// <summary>Compares the values of two nodes, including the values of all descendant nodes.</summary>
    /// <param name="n1">The first <see cref="T:System.Xml.Linq.XNode" /> to compare.</param>
    /// <param name="n2">The second <see cref="T:System.Xml.Linq.XNode" /> to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the nodes are equal; otherwise <see langword="false" />.</returns>
    public static bool DeepEquals(XNode? n1, XNode? n2)
    {
      if (n1 == n2)
        return true;
      return n1 != null && n2 != null && n1.DeepEquals(n2);
    }

    /// <summary>Writes this node to an <see cref="T:System.Xml.XmlWriter" />.</summary>
    /// <param name="writer">An <see cref="T:System.Xml.XmlWriter" /> into which this method will write.</param>
    public abstract void WriteTo(XmlWriter writer);

    /// <summary>Writes the current node to an <see cref="T:System.Xml.XmlWriter" />.</summary>
    /// <param name="writer">The writer to write the current node into.</param>
    /// <param name="cancellationToken">A token that can be used to request cancellation of the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous write operation.</returns>
    public abstract Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken);


    #nullable disable
    internal virtual void AppendText(StringBuilder sb)
    {
    }

    internal abstract XNode CloneNode();

    internal abstract bool DeepEquals(XNode node);

    internal IEnumerable<XElement> GetAncestors(XName name, bool self)
    {
      XNode xnode = this;
      for (XElement e = (self ? xnode : (XNode) xnode.parent) as XElement; e != null; e = e.parent as XElement)
      {
        if (name == (XName) null || e.name == name)
          yield return e;
      }
    }

    private IEnumerable<XElement> GetElementsAfterSelf(XName name)
    {
      XNode n = this;
      while (n.parent != null && n != n.parent.content)
      {
        n = n.next;
        if (n is XElement xelement && (name == (XName) null || xelement.name == name))
          yield return xelement;
      }
    }

    private IEnumerable<XElement> GetElementsBeforeSelf(XName name)
    {
      XNode xnode = this;
      if (xnode.parent != null)
      {
        XNode n = (XNode) xnode.parent.content;
        do
        {
          n = n.next;
          if (n != xnode)
          {
            if (n is XElement xelement && (name == (XName) null || xelement.name == name))
              yield return xelement;
          }
          else
            break;
        }
        while (xnode.parent != null && xnode.parent == n.parent);
        n = (XNode) null;
      }
    }

    internal abstract int GetDeepHashCode();

    internal static XmlReaderSettings GetXmlReaderSettings(LoadOptions o)
    {
      XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
      if ((o & LoadOptions.PreserveWhitespace) == LoadOptions.None)
        xmlReaderSettings.IgnoreWhitespace = true;
      xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;
      xmlReaderSettings.MaxCharactersFromEntities = 10000000L;
      return xmlReaderSettings;
    }

    internal static XmlWriterSettings GetXmlWriterSettings(SaveOptions o)
    {
      XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
      if ((o & SaveOptions.DisableFormatting) == SaveOptions.None)
        xmlWriterSettings.Indent = true;
      if ((o & SaveOptions.OmitDuplicateNamespaces) != SaveOptions.None)
        xmlWriterSettings.NamespaceHandling |= NamespaceHandling.OmitDuplicates;
      return xmlWriterSettings;
    }

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
        if (this is XText)
          settings.ConformanceLevel = ConformanceLevel.Fragment;
        using (XmlWriter writer = XmlWriter.Create((TextWriter) output, settings))
        {
          if (this is XDocument xdocument)
            xdocument.WriteContentTo(writer);
          else
            this.WriteTo(writer);
        }
        return output.ToString();
      }
    }
  }
}
