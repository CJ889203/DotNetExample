// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XContainer
// Assembly: System.Private.Xml.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 67E106B6-1B7E-4540-BB80-26A3D9D4BC13
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Xml.Linq.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Xml.XDocument.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.Xml.Linq
{
  /// <summary>Represents a node that can contain other nodes.</summary>
  public abstract class XContainer : XNode
  {

    #nullable disable
    internal object content;

    internal XContainer()
    {
    }

    internal XContainer(XContainer other)
    {
      if (other == null)
        throw new ArgumentNullException(nameof (other));
      if (other.content is string)
      {
        this.content = other.content;
      }
      else
      {
        XNode xnode = (XNode) other.content;
        if (xnode == null)
          return;
        do
        {
          xnode = xnode.next;
          this.AppendNodeSkipNotify(xnode.CloneNode());
        }
        while (xnode != other.content);
      }
    }


    #nullable enable
    /// <summary>Gets the first child node of this node.</summary>
    /// <returns>An <see cref="T:System.Xml.Linq.XNode" /> containing the first child node of the <see cref="T:System.Xml.Linq.XContainer" />.</returns>
    public XNode? FirstNode => this.LastNode?.next;

    /// <summary>Gets the last child node of this node.</summary>
    /// <returns>An <see cref="T:System.Xml.Linq.XNode" /> containing the last child node of the <see cref="T:System.Xml.Linq.XContainer" />.</returns>
    public XNode? LastNode
    {
      get
      {
        if (this.content == null)
          return (XNode) null;
        if (this.content is XNode content1)
          return content1;
        if (this.content is string content2)
        {
          if (content2.Length == 0)
            return (XNode) null;
          XText xtext = new XText(content2);
          xtext.parent = this;
          xtext.next = (XNode) xtext;
          Interlocked.CompareExchange<object>(ref this.content, (object) xtext, (object) content2);
        }
        return (XNode) this.content;
      }
    }

    /// <summary>Adds the specified content as children of this <see cref="T:System.Xml.Linq.XContainer" />.</summary>
    /// <param name="content">A content object containing simple content or a collection of content objects to be added.</param>
    public void Add(object? content)
    {
      if (this.SkipNotify())
      {
        this.AddContentSkipNotify(content);
      }
      else
      {
        switch (content)
        {
          case null:
            break;
          case XNode n:
            this.AddNode(n);
            break;
          case string s:
            this.AddString(s);
            break;
          case XAttribute a:
            this.AddAttribute(a);
            break;
          case XStreamingElement other:
            this.AddNode((XNode) new XElement(other));
            break;
          case object[] objArray:
            for (int index = 0; index < objArray.Length; ++index)
              this.Add(objArray[index]);
            break;
          case IEnumerable enumerable:
            IEnumerator enumerator = enumerable.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
                this.Add(enumerator.Current);
              break;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          default:
            this.AddString(XContainer.GetStringValue(content));
            break;
        }
      }
    }

    /// <summary>Adds the specified content as children of this <see cref="T:System.Xml.Linq.XContainer" />.</summary>
    /// <param name="content">A parameter list of content objects.</param>
    public void Add(params object?[] content) => this.Add((object) content);

    /// <summary>Adds the specified content as the first children of this document or element.</summary>
    /// <param name="content">A content object containing simple content or a collection of content objects to be added.</param>
    public void AddFirst(object? content) => new Inserter(this, (XNode) null).Add(content);

    /// <summary>Adds the specified content as the first children of this document or element.</summary>
    /// <param name="content">A parameter list of content objects.</param>
    /// <exception cref="T:System.InvalidOperationException">The parent is <see langword="null" />.</exception>
    public void AddFirst(params object?[] content) => this.AddFirst((object) content);

    /// <summary>Creates an <see cref="T:System.Xml.XmlWriter" /> that can be used to add nodes to the <see cref="T:System.Xml.Linq.XContainer" />.</summary>
    /// <returns>An <see cref="T:System.Xml.XmlWriter" /> that is ready to have content written to it.</returns>
    public XmlWriter CreateWriter() => XmlWriter.Create((XmlWriter) new XNodeBuilder(this), new XmlWriterSettings()
    {
      ConformanceLevel = this is XDocument ? ConformanceLevel.Document : ConformanceLevel.Fragment
    });

    /// <summary>Returns a collection of the descendant nodes for this document or element, in document order.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XNode" /> containing the descendant nodes of the <see cref="T:System.Xml.Linq.XContainer" />, in document order.</returns>
    public IEnumerable<XNode> DescendantNodes() => this.GetDescendantNodes(false);

    /// <summary>Returns a collection of the descendant elements for this document or element, in document order.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> containing the descendant elements of the <see cref="T:System.Xml.Linq.XContainer" />.</returns>
    public IEnumerable<XElement> Descendants() => this.GetDescendants((XName) null, false);

    /// <summary>Returns a filtered collection of the descendant elements for this document or element, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
    /// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> containing the descendant elements of the <see cref="T:System.Xml.Linq.XContainer" /> that match the specified <see cref="T:System.Xml.Linq.XName" />.</returns>
    public IEnumerable<XElement> Descendants(XName? name) => !(name != (XName) null) ? XElement.EmptySequence : this.GetDescendants(name, false);

    /// <summary>Gets the first (in document order) child element with the specified <see cref="T:System.Xml.Linq.XName" />.</summary>
    /// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
    /// <returns>A <see cref="T:System.Xml.Linq.XElement" /> that matches the specified <see cref="T:System.Xml.Linq.XName" />, or <see langword="null" />.</returns>
    public XElement? Element(XName name)
    {
      if (this.content is XNode xnode)
      {
        do
        {
          xnode = xnode.next;
          if (xnode is XElement xelement && xelement.name == name)
            return xelement;
        }
        while (xnode != this.content);
      }
      return (XElement) null;
    }

    /// <summary>Returns a collection of the child elements of this element or document, in document order.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> containing the child elements of this <see cref="T:System.Xml.Linq.XContainer" />, in document order.</returns>
    public IEnumerable<XElement> Elements() => this.GetElements((XName) null);

    /// <summary>Returns a filtered collection of the child elements of this element or document, in document order. Only elements that have a matching <see cref="T:System.Xml.Linq.XName" /> are included in the collection.</summary>
    /// <param name="name">The <see cref="T:System.Xml.Linq.XName" /> to match.</param>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XElement" /> containing the children of the <see cref="T:System.Xml.Linq.XContainer" /> that have a matching <see cref="T:System.Xml.Linq.XName" />, in document order.</returns>
    public IEnumerable<XElement> Elements(XName? name) => !(name != (XName) null) ? XElement.EmptySequence : this.GetElements(name);

    /// <summary>Returns a collection of the child nodes of this element or document, in document order.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:System.Xml.Linq.XNode" /> containing the contents of this <see cref="T:System.Xml.Linq.XContainer" />, in document order.</returns>
    public IEnumerable<XNode> Nodes()
    {
      XContainer xcontainer = this;
      XNode n = xcontainer.LastNode;
      if (n != null)
      {
        do
        {
          n = n.next;
          yield return n;
        }
        while (n.parent == xcontainer && n != xcontainer.content);
      }
    }

    /// <summary>Removes the child nodes from this document or element.</summary>
    public void RemoveNodes()
    {
      if (this.SkipNotify())
      {
        this.RemoveNodesSkipNotify();
      }
      else
      {
        while (this.content != null)
        {
          if (this.content is string content1)
          {
            if (content1.Length > 0)
              this.ConvertTextToNode();
            else if (this is XElement)
            {
              this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
              this.content = (object) content1 == this.content ? (object) null : throw new InvalidOperationException(SR.InvalidOperation_ExternalCode);
              this.NotifyChanged((object) this, XObjectChangeEventArgs.Value);
            }
            else
              this.content = (object) null;
          }
          if (this.content is XNode content2)
          {
            XNode next = content2.next;
            this.NotifyChanging((object) next, XObjectChangeEventArgs.Remove);
            if (content2 != this.content || next != content2.next)
              throw new InvalidOperationException(SR.InvalidOperation_ExternalCode);
            if (next != content2)
              content2.next = next.next;
            else
              this.content = (object) null;
            next.parent = (XContainer) null;
            next.next = (XNode) null;
            this.NotifyChanged((object) next, XObjectChangeEventArgs.Remove);
          }
        }
      }
    }

    /// <summary>Replaces the children nodes of this document or element with the specified content.</summary>
    /// <param name="content">A content object containing simple content or a collection of content objects that replace the children nodes.</param>
    public void ReplaceNodes(object? content)
    {
      content = XContainer.GetContentSnapshot(content);
      this.RemoveNodes();
      this.Add(content);
    }

    /// <summary>Replaces the children nodes of this document or element with the specified content.</summary>
    /// <param name="content">A parameter list of content objects.</param>
    public void ReplaceNodes(params object?[] content) => this.ReplaceNodes((object) content);


    #nullable disable
    internal virtual void AddAttribute(XAttribute a)
    {
    }

    internal virtual void AddAttributeSkipNotify(XAttribute a)
    {
    }

    internal void AddContentSkipNotify(object content)
    {
      switch (content)
      {
        case null:
          break;
        case XNode n:
          this.AddNodeSkipNotify(n);
          break;
        case string s:
          this.AddStringSkipNotify(s);
          break;
        case XAttribute a:
          this.AddAttributeSkipNotify(a);
          break;
        case XStreamingElement other:
          this.AddNodeSkipNotify((XNode) new XElement(other));
          break;
        case object[] objArray:
          for (int index = 0; index < objArray.Length; ++index)
            this.AddContentSkipNotify(objArray[index]);
          break;
        case IEnumerable enumerable:
          IEnumerator enumerator = enumerable.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
              this.AddContentSkipNotify(enumerator.Current);
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
        default:
          this.AddStringSkipNotify(XContainer.GetStringValue(content));
          break;
      }
    }

    internal void AddNode(XNode n)
    {
      this.ValidateNode(n, (XNode) this);
      if (n.parent != null)
      {
        n = n.CloneNode();
      }
      else
      {
        XNode xnode = (XNode) this;
        while (xnode.parent != null)
          xnode = (XNode) xnode.parent;
        if (n == xnode)
          n = n.CloneNode();
      }
      this.ConvertTextToNode();
      this.AppendNode(n);
    }

    internal void AddNodeSkipNotify(XNode n)
    {
      this.ValidateNode(n, (XNode) this);
      if (n.parent != null)
      {
        n = n.CloneNode();
      }
      else
      {
        XNode xnode = (XNode) this;
        while (xnode.parent != null)
          xnode = (XNode) xnode.parent;
        if (n == xnode)
          n = n.CloneNode();
      }
      this.ConvertTextToNode();
      this.AppendNodeSkipNotify(n);
    }

    internal void AddString(string s)
    {
      this.ValidateString(s);
      if (this.content == null)
      {
        if (s.Length > 0)
          this.AppendNode((XNode) new XText(s));
        else if (this is XElement)
        {
          this.NotifyChanging((object) this, XObjectChangeEventArgs.Value);
          this.content = this.content == null ? (object) s : throw new InvalidOperationException(SR.InvalidOperation_ExternalCode);
          this.NotifyChanged((object) this, XObjectChangeEventArgs.Value);
        }
        else
          this.content = (object) s;
      }
      else
      {
        if (s.Length <= 0)
          return;
        this.ConvertTextToNode();
        if (this.content is XText content && !(content is XCData))
          content.Value += s;
        else
          this.AppendNode((XNode) new XText(s));
      }
    }

    internal void AddStringSkipNotify(string s)
    {
      this.ValidateString(s);
      if (this.content == null)
      {
        this.content = (object) s;
      }
      else
      {
        if (s.Length <= 0)
          return;
        if (this.content is string content2)
          this.content = (object) (content2 + s);
        else if (this.content is XText content1 && !(content1 is XCData))
          content1.text += s;
        else
          this.AppendNodeSkipNotify((XNode) new XText(s));
      }
    }

    internal void AppendNode(XNode n)
    {
      bool flag = this.NotifyChanging((object) n, XObjectChangeEventArgs.Add);
      if (n.parent != null)
        throw new InvalidOperationException(SR.InvalidOperation_ExternalCode);
      this.AppendNodeSkipNotify(n);
      if (!flag)
        return;
      this.NotifyChanged((object) n, XObjectChangeEventArgs.Add);
    }

    internal void AppendNodeSkipNotify(XNode n)
    {
      n.parent = this;
      if (this.content == null || this.content is string)
      {
        n.next = n;
      }
      else
      {
        XNode content = (XNode) this.content;
        n.next = content.next;
        content.next = n;
      }
      this.content = (object) n;
    }

    internal override void AppendText(StringBuilder sb)
    {
      if (this.content is string content)
      {
        sb.Append(content);
      }
      else
      {
        XNode xnode = (XNode) this.content;
        if (xnode == null)
          return;
        do
        {
          xnode = xnode.next;
          xnode.AppendText(sb);
        }
        while (xnode != this.content);
      }
    }

    private string GetTextOnly()
    {
      if (this.content == null)
        return (string) null;
      if (!(this.content is string content))
      {
        XNode xnode = (XNode) this.content;
        do
        {
          xnode = xnode.next;
          if (xnode.NodeType != XmlNodeType.Text)
            return (string) null;
          content += ((XText) xnode).Value;
        }
        while (xnode != this.content);
      }
      return content;
    }

    private string CollectText(ref XNode n)
    {
      string str = "";
      while (n != null && n.NodeType == XmlNodeType.Text)
      {
        str += ((XText) n).Value;
        n = n != this.content ? n.next : (XNode) null;
      }
      return str;
    }

    internal bool ContentsEqual(XContainer e)
    {
      if (this.content == e.content)
        return true;
      string textOnly = this.GetTextOnly();
      if (textOnly != null)
        return textOnly == e.GetTextOnly();
      XNode content1 = this.content as XNode;
      XNode content2 = e.content as XNode;
      if (content1 != null && content2 != null)
      {
        XNode n1 = content1.next;
        for (XNode n2 = content2.next; !(this.CollectText(ref n1) != e.CollectText(ref n2)); n2 = n2 != e.content ? n2.next : (XNode) null)
        {
          if (n1 == null && n2 == null)
            return true;
          if (n1 != null && n2 != null && n1.DeepEquals(n2))
            n1 = n1 != this.content ? n1.next : (XNode) null;
          else
            break;
        }
      }
      return false;
    }

    internal int ContentsHashCode()
    {
      string textOnly = this.GetTextOnly();
      if (textOnly != null)
        return textOnly.GetHashCode();
      int num = 0;
      if (this.content is XNode n)
      {
        do
        {
          n = n.next;
          string str = this.CollectText(ref n);
          if (str.Length > 0)
            num ^= str.GetHashCode();
          if (n != null)
            num ^= n.GetDeepHashCode();
          else
            break;
        }
        while (n != this.content);
      }
      return num;
    }

    internal void ConvertTextToNode()
    {
      string content = this.content as string;
      if (string.IsNullOrEmpty(content))
        return;
      XText xtext = new XText(content);
      xtext.parent = this;
      xtext.next = (XNode) xtext;
      this.content = (object) xtext;
    }

    internal IEnumerable<XNode> GetDescendantNodes(bool self)
    {
      XContainer xcontainer1 = this;
      if (self)
        yield return (XNode) xcontainer1;
      XNode n = (XNode) xcontainer1;
      while (true)
      {
        XNode firstNode;
        if (n is XContainer xcontainer2 && (firstNode = xcontainer2.FirstNode) != null)
        {
          n = firstNode;
        }
        else
        {
          while (n != null && n != xcontainer1 && n == n.parent.content)
            n = (XNode) n.parent;
          if (n != null && n != xcontainer1)
            n = n.next;
          else
            break;
        }
        yield return n;
      }
    }

    internal IEnumerable<XElement> GetDescendants(XName name, bool self)
    {
      XContainer xcontainer1 = this;
      if (self)
      {
        XElement xelement = (XElement) xcontainer1;
        if (name == (XName) null || xelement.name == name)
          yield return xelement;
      }
      XNode n = (XNode) xcontainer1;
      XContainer xcontainer2 = xcontainer1;
      while (true)
      {
        if (xcontainer2 != null && xcontainer2.content is XNode)
        {
          n = ((XNode) xcontainer2.content).next;
        }
        else
        {
          while (n != xcontainer1 && n == n.parent.content)
            n = (XNode) n.parent;
          if (n != xcontainer1)
            n = n.next;
          else
            break;
        }
        XElement e = n as XElement;
        if (e != null && (name == (XName) null || e.name == name))
          yield return e;
        xcontainer2 = (XContainer) e;
        e = (XElement) null;
      }
    }

    private IEnumerable<XElement> GetElements(XName name)
    {
      XContainer xcontainer = this;
      if (xcontainer.content is XNode n)
      {
        do
        {
          n = n.next;
          if (n is XElement xelement && (name == (XName) null || xelement.name == name))
            yield return xelement;
        }
        while (n.parent == xcontainer && n != xcontainer.content);
      }
    }

    internal static string GetStringValue(object value)
    {
      string str1;
      switch (value)
      {
        case string str2:
          str1 = str2;
          break;
        case int num1:
          str1 = XmlConvert.ToString(num1);
          break;
        case double num2:
          str1 = XmlConvert.ToString(num2);
          break;
        case long num3:
          str1 = XmlConvert.ToString(num3);
          break;
        case float num4:
          str1 = XmlConvert.ToString(num4);
          break;
        case Decimal num5:
          str1 = XmlConvert.ToString(num5);
          break;
        case short num6:
          str1 = XmlConvert.ToString(num6);
          break;
        case sbyte num7:
          str1 = XmlConvert.ToString(num7);
          break;
        case bool flag:
          str1 = XmlConvert.ToString(flag);
          break;
        case DateTime dateTime:
          str1 = XmlConvert.ToString(dateTime, XmlDateTimeSerializationMode.RoundtripKind);
          break;
        case DateTimeOffset dateTimeOffset:
          str1 = XmlConvert.ToString(dateTimeOffset);
          break;
        case TimeSpan timeSpan:
          str1 = XmlConvert.ToString(timeSpan);
          break;
        case XObject _:
          throw new ArgumentException(SR.Argument_XObjectValue);
        default:
          str1 = value.ToString();
          break;
      }
      return str1 ?? throw new ArgumentException(SR.Argument_ConvertToString);
    }

    internal void ReadContentFrom(XmlReader r)
    {
      if (r.ReadState != ReadState.Interactive)
        throw new InvalidOperationException(SR.InvalidOperation_ExpectedInteractive);
      XContainer.ContentReader contentReader = new XContainer.ContentReader(this);
      do
        ;
      while (contentReader.ReadContentFrom(this, r) && r.Read());
    }

    internal void ReadContentFrom(XmlReader r, LoadOptions o)
    {
      if ((o & (LoadOptions.SetBaseUri | LoadOptions.SetLineInfo)) == LoadOptions.None)
      {
        this.ReadContentFrom(r);
      }
      else
      {
        XContainer.ContentReader contentReader = r.ReadState == ReadState.Interactive ? new XContainer.ContentReader(this, r, o) : throw new InvalidOperationException(SR.InvalidOperation_ExpectedInteractive);
        do
          ;
        while (contentReader.ReadContentFrom(this, r, o) && r.Read());
      }
    }

    internal async Task ReadContentFromAsync(XmlReader r, CancellationToken cancellationToken)
    {
      XContainer rootContainer = this;
      if (r.ReadState != ReadState.Interactive)
        throw new InvalidOperationException(SR.InvalidOperation_ExpectedInteractive);
      XContainer.ContentReader cr = new XContainer.ContentReader(rootContainer);
      bool flag;
      do
      {
        cancellationToken.ThrowIfCancellationRequested();
        flag = await cr.ReadContentFromAsync(rootContainer, r).ConfigureAwait(false);
        if (flag)
          flag = await r.ReadAsync().ConfigureAwait(false);
      }
      while (flag);
      cr = (XContainer.ContentReader) null;
    }

    internal async Task ReadContentFromAsync(
      XmlReader r,
      LoadOptions o,
      CancellationToken cancellationToken)
    {
      XContainer rootContainer = this;
      XContainer.ContentReader cr;
      if ((o & (LoadOptions.SetBaseUri | LoadOptions.SetLineInfo)) == LoadOptions.None)
      {
        await rootContainer.ReadContentFromAsync(r, cancellationToken).ConfigureAwait(false);
        cr = (XContainer.ContentReader) null;
      }
      else
      {
        if (r.ReadState != ReadState.Interactive)
          throw new InvalidOperationException(SR.InvalidOperation_ExpectedInteractive);
        cr = new XContainer.ContentReader(rootContainer, r, o);
        bool flag;
        do
        {
          cancellationToken.ThrowIfCancellationRequested();
          flag = await cr.ReadContentFromAsync(rootContainer, r, o).ConfigureAwait(false);
          if (flag)
            flag = await r.ReadAsync().ConfigureAwait(false);
        }
        while (flag);
        cr = (XContainer.ContentReader) null;
      }
    }

    internal void RemoveNode(XNode n)
    {
      bool flag = this.NotifyChanging((object) n, XObjectChangeEventArgs.Remove);
      if (n.parent != this)
        throw new InvalidOperationException(SR.InvalidOperation_ExternalCode);
      XNode xnode = (XNode) this.content;
      while (xnode.next != n)
        xnode = xnode.next;
      if (xnode == n)
      {
        this.content = (object) null;
      }
      else
      {
        if (this.content == n)
          this.content = (object) xnode;
        xnode.next = n.next;
      }
      n.parent = (XContainer) null;
      n.next = (XNode) null;
      if (!flag)
        return;
      this.NotifyChanged((object) n, XObjectChangeEventArgs.Remove);
    }

    private void RemoveNodesSkipNotify()
    {
      if (this.content is XNode xnode)
      {
        do
        {
          XNode next = xnode.next;
          xnode.parent = (XContainer) null;
          xnode.next = (XNode) null;
          xnode = next;
        }
        while (xnode != this.content);
      }
      this.content = (object) null;
    }

    internal virtual void ValidateNode(XNode node, XNode previous)
    {
    }

    internal virtual void ValidateString(string s)
    {
    }

    internal void WriteContentTo(XmlWriter writer)
    {
      if (this.content == null)
        return;
      if (this.content is string content)
      {
        if (this is XDocument)
          writer.WriteWhitespace(content);
        else
          writer.WriteString(content);
      }
      else
      {
        XNode xnode = (XNode) this.content;
        do
        {
          xnode = xnode.next;
          xnode.WriteTo(writer);
        }
        while (xnode != this.content);
      }
    }

    internal async Task WriteContentToAsync(
      XmlWriter writer,
      CancellationToken cancellationToken)
    {
      XContainer xcontainer = this;
      if (xcontainer.content == null)
        return;
      if (xcontainer.content is string content)
      {
        cancellationToken.ThrowIfCancellationRequested();
        await (!(xcontainer is XDocument) ? writer.WriteStringAsync(content) : writer.WriteWhitespaceAsync(content)).ConfigureAwait(false);
      }
      else
      {
        XNode n = (XNode) xcontainer.content;
        do
        {
          n = n.next;
          await n.WriteToAsync(writer, cancellationToken).ConfigureAwait(false);
        }
        while (n != xcontainer.content);
        n = (XNode) null;
      }
    }

    private static void AddContentToList(List<object> list, object content)
    {
      IEnumerable enumerable = content is string ? (IEnumerable) null : content as IEnumerable;
      if (enumerable == null)
      {
        list.Add(content);
      }
      else
      {
        foreach (object content1 in enumerable)
        {
          if (content1 != null)
            XContainer.AddContentToList(list, content1);
        }
      }
    }

    [return: NotNullIfNotNull("content")]
    internal static object GetContentSnapshot(object content)
    {
      switch (content)
      {
        case IEnumerable _:
          List<object> list = new List<object>();
          XContainer.AddContentToList(list, content);
          return (object) list;
        default:
          return content;
      }
    }

    private sealed class ContentReader
    {
      private readonly NamespaceCache _eCache;
      private readonly NamespaceCache _aCache;
      private readonly IXmlLineInfo _lineInfo;
      private XContainer _currentContainer;
      private string _baseUri;

      public ContentReader(XContainer rootContainer) => this._currentContainer = rootContainer;

      public ContentReader(XContainer rootContainer, XmlReader r, LoadOptions o)
      {
        this._currentContainer = rootContainer;
        this._baseUri = (o & LoadOptions.SetBaseUri) != LoadOptions.None ? r.BaseURI : (string) null;
        this._lineInfo = (o & LoadOptions.SetLineInfo) != LoadOptions.None ? r as IXmlLineInfo : (IXmlLineInfo) null;
      }

      public bool ReadContentFrom(XContainer rootContainer, XmlReader r)
      {
        switch (r.NodeType)
        {
          case XmlNodeType.Element:
            XElement n = new XElement(this._eCache.Get(r.NamespaceURI).GetName(r.LocalName));
            if (r.MoveToFirstAttribute())
            {
              do
              {
                n.AppendAttributeSkipNotify(new XAttribute(this._aCache.Get(r.Prefix.Length == 0 ? string.Empty : r.NamespaceURI).GetName(r.LocalName), (object) r.Value));
              }
              while (r.MoveToNextAttribute());
              r.MoveToElement();
            }
            this._currentContainer.AddNodeSkipNotify((XNode) n);
            if (!r.IsEmptyElement)
            {
              this._currentContainer = (XContainer) n;
              goto case XmlNodeType.EndEntity;
            }
            else
              goto case XmlNodeType.EndEntity;
          case XmlNodeType.Text:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            this._currentContainer.AddStringSkipNotify(r.Value);
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.CDATA:
            this._currentContainer.AddNodeSkipNotify((XNode) new XCData(r.Value));
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EntityReference:
            if (!r.CanResolveEntity)
              throw new InvalidOperationException(SR.InvalidOperation_UnresolvedEntityReference);
            r.ResolveEntity();
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.ProcessingInstruction:
            this._currentContainer.AddNodeSkipNotify((XNode) new XProcessingInstruction(r.Name, r.Value));
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.Comment:
            this._currentContainer.AddNodeSkipNotify((XNode) new XComment(r.Value));
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.DocumentType:
            this._currentContainer.AddNodeSkipNotify((XNode) new XDocumentType(r.LocalName, r.GetAttribute("PUBLIC"), r.GetAttribute("SYSTEM"), r.Value));
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EndElement:
            if (this._currentContainer.content == null)
              this._currentContainer.content = (object) string.Empty;
            if (this._currentContainer == rootContainer)
              return false;
            this._currentContainer = this._currentContainer.parent;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EndEntity:
            return true;
          default:
            throw new InvalidOperationException(SR.Format(SR.InvalidOperation_UnexpectedNodeType, (object) r.NodeType));
        }
      }

      public async ValueTask<bool> ReadContentFromAsync(
        XContainer rootContainer,
        XmlReader r)
      {
        XElement e;
        XContainer xcontainer;
        string str;
        switch (r.NodeType)
        {
          case XmlNodeType.Element:
            e = new XElement(this._eCache.Get(r.NamespaceURI).GetName(r.LocalName));
            if (r.MoveToFirstAttribute())
            {
              do
              {
                XElement xelement = e;
                XName name = this._aCache.Get(r.Prefix.Length == 0 ? string.Empty : r.NamespaceURI).GetName(r.LocalName);
                xelement.AppendAttributeSkipNotify(new XAttribute(name, (object) await r.GetValueAsync().ConfigureAwait(false)));
                xelement = (XElement) null;
                name = (XName) null;
              }
              while (r.MoveToNextAttribute());
              r.MoveToElement();
            }
            this._currentContainer.AddNodeSkipNotify((XNode) e);
            if (!r.IsEmptyElement)
            {
              this._currentContainer = (XContainer) e;
              goto case XmlNodeType.EndEntity;
            }
            else
              goto case XmlNodeType.EndEntity;
          case XmlNodeType.Text:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            xcontainer = this._currentContainer;
            xcontainer.AddStringSkipNotify(await r.GetValueAsync().ConfigureAwait(false));
            xcontainer = (XContainer) null;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.CDATA:
            xcontainer = this._currentContainer;
            xcontainer.AddNodeSkipNotify((XNode) new XCData(await r.GetValueAsync().ConfigureAwait(false)));
            xcontainer = (XContainer) null;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EntityReference:
            if (!r.CanResolveEntity)
              throw new InvalidOperationException(SR.InvalidOperation_UnresolvedEntityReference);
            r.ResolveEntity();
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.ProcessingInstruction:
            xcontainer = this._currentContainer;
            str = r.Name;
            xcontainer.AddNodeSkipNotify((XNode) new XProcessingInstruction(str, await r.GetValueAsync().ConfigureAwait(false)));
            xcontainer = (XContainer) null;
            str = (string) null;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.Comment:
            xcontainer = this._currentContainer;
            xcontainer.AddNodeSkipNotify((XNode) new XComment(await r.GetValueAsync().ConfigureAwait(false)));
            xcontainer = (XContainer) null;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.DocumentType:
            xcontainer = this._currentContainer;
            str = r.LocalName;
            string publicId = r.GetAttribute("PUBLIC");
            string systemId = r.GetAttribute("SYSTEM");
            xcontainer.AddNodeSkipNotify((XNode) new XDocumentType(str, publicId, systemId, await r.GetValueAsync().ConfigureAwait(false)));
            xcontainer = (XContainer) null;
            str = (string) null;
            publicId = (string) null;
            systemId = (string) null;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EndElement:
            if (this._currentContainer.content == null)
              this._currentContainer.content = (object) string.Empty;
            if (this._currentContainer == rootContainer)
              return false;
            this._currentContainer = this._currentContainer.parent;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EndEntity:
            e = (XElement) null;
            return true;
          default:
            throw new InvalidOperationException(SR.Format(SR.InvalidOperation_UnexpectedNodeType, (object) r.NodeType));
        }
      }

      public bool ReadContentFrom(XContainer rootContainer, XmlReader r, LoadOptions o)
      {
        XNode n1 = (XNode) null;
        string baseUri = r.BaseURI;
        switch (r.NodeType)
        {
          case XmlNodeType.Element:
            XElement n2 = new XElement(this._eCache.Get(r.NamespaceURI).GetName(r.LocalName));
            if (this._baseUri != null && this._baseUri != baseUri)
              n2.SetBaseUri(baseUri);
            if (this._lineInfo != null && this._lineInfo.HasLineInfo())
              n2.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
            if (r.MoveToFirstAttribute())
            {
              do
              {
                XAttribute a = new XAttribute(this._aCache.Get(r.Prefix.Length == 0 ? string.Empty : r.NamespaceURI).GetName(r.LocalName), (object) r.Value);
                if (this._lineInfo != null && this._lineInfo.HasLineInfo())
                  a.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
                n2.AppendAttributeSkipNotify(a);
              }
              while (r.MoveToNextAttribute());
              r.MoveToElement();
            }
            this._currentContainer.AddNodeSkipNotify((XNode) n2);
            if (!r.IsEmptyElement)
            {
              this._currentContainer = (XContainer) n2;
              if (this._baseUri != null)
              {
                this._baseUri = baseUri;
                goto case XmlNodeType.EndEntity;
              }
              else
                goto case XmlNodeType.EndEntity;
            }
            else
              goto case XmlNodeType.EndEntity;
          case XmlNodeType.Text:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            if (this._baseUri != null && this._baseUri != baseUri || this._lineInfo != null && this._lineInfo.HasLineInfo())
            {
              n1 = (XNode) new XText(r.Value);
              goto case XmlNodeType.EndEntity;
            }
            else
            {
              this._currentContainer.AddStringSkipNotify(r.Value);
              goto case XmlNodeType.EndEntity;
            }
          case XmlNodeType.CDATA:
            n1 = (XNode) new XCData(r.Value);
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EntityReference:
            if (!r.CanResolveEntity)
              throw new InvalidOperationException(SR.InvalidOperation_UnresolvedEntityReference);
            r.ResolveEntity();
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.ProcessingInstruction:
            n1 = (XNode) new XProcessingInstruction(r.Name, r.Value);
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.Comment:
            n1 = (XNode) new XComment(r.Value);
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.DocumentType:
            n1 = (XNode) new XDocumentType(r.LocalName, r.GetAttribute("PUBLIC"), r.GetAttribute("SYSTEM"), r.Value);
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EndElement:
            if (this._currentContainer.content == null)
              this._currentContainer.content = (object) string.Empty;
            if (this._currentContainer is XElement currentContainer && this._lineInfo != null && this._lineInfo.HasLineInfo())
              currentContainer.SetEndElementLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
            if (this._currentContainer == rootContainer)
              return false;
            if (this._baseUri != null && this._currentContainer.HasBaseUri)
              this._baseUri = this._currentContainer.parent.BaseUri;
            this._currentContainer = this._currentContainer.parent;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EndEntity:
            if (n1 != null)
            {
              if (this._baseUri != null && this._baseUri != baseUri)
                n1.SetBaseUri(baseUri);
              if (this._lineInfo != null && this._lineInfo.HasLineInfo())
                n1.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
              this._currentContainer.AddNodeSkipNotify(n1);
            }
            return true;
          default:
            throw new InvalidOperationException(SR.Format(SR.InvalidOperation_UnexpectedNodeType, (object) r.NodeType));
        }
      }

      public async ValueTask<bool> ReadContentFromAsync(
        XContainer rootContainer,
        XmlReader r,
        LoadOptions o)
      {
        XNode newNode = (XNode) null;
        string baseUri = r.BaseURI;
        string str;
        switch (r.NodeType)
        {
          case XmlNodeType.Element:
            XElement e = new XElement(this._eCache.Get(r.NamespaceURI).GetName(r.LocalName));
            if (this._baseUri != null && this._baseUri != baseUri)
              e.SetBaseUri(baseUri);
            if (this._lineInfo != null && this._lineInfo.HasLineInfo())
              e.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
            if (r.MoveToFirstAttribute())
            {
              do
              {
                XName name = this._aCache.Get(r.Prefix.Length == 0 ? string.Empty : r.NamespaceURI).GetName(r.LocalName);
                XAttribute a = new XAttribute(name, (object) await r.GetValueAsync().ConfigureAwait(false));
                name = (XName) null;
                if (this._lineInfo != null && this._lineInfo.HasLineInfo())
                  a.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
                e.AppendAttributeSkipNotify(a);
              }
              while (r.MoveToNextAttribute());
              r.MoveToElement();
            }
            this._currentContainer.AddNodeSkipNotify((XNode) e);
            if (!r.IsEmptyElement)
            {
              this._currentContainer = (XContainer) e;
              if (this._baseUri != null)
              {
                this._baseUri = baseUri;
                goto case XmlNodeType.EndEntity;
              }
              else
                goto case XmlNodeType.EndEntity;
            }
            else
              goto case XmlNodeType.EndEntity;
          case XmlNodeType.Text:
          case XmlNodeType.Whitespace:
          case XmlNodeType.SignificantWhitespace:
            if (this._baseUri != null && this._baseUri != baseUri || this._lineInfo != null && this._lineInfo.HasLineInfo())
            {
              newNode = (XNode) new XText(await r.GetValueAsync().ConfigureAwait(false));
              goto case XmlNodeType.EndEntity;
            }
            else
            {
              XContainer xcontainer = this._currentContainer;
              xcontainer.AddStringSkipNotify(await r.GetValueAsync().ConfigureAwait(false));
              xcontainer = (XContainer) null;
              goto case XmlNodeType.EndEntity;
            }
          case XmlNodeType.CDATA:
            newNode = (XNode) new XCData(await r.GetValueAsync().ConfigureAwait(false));
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EntityReference:
            if (!r.CanResolveEntity)
              throw new InvalidOperationException(SR.InvalidOperation_UnresolvedEntityReference);
            r.ResolveEntity();
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.ProcessingInstruction:
            str = r.Name;
            newNode = (XNode) new XProcessingInstruction(str, await r.GetValueAsync().ConfigureAwait(false));
            str = (string) null;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.Comment:
            newNode = (XNode) new XComment(await r.GetValueAsync().ConfigureAwait(false));
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.DocumentType:
            str = r.LocalName;
            string publicId = r.GetAttribute("PUBLIC");
            string systemId = r.GetAttribute("SYSTEM");
            newNode = (XNode) new XDocumentType(str, publicId, systemId, await r.GetValueAsync().ConfigureAwait(false));
            str = (string) null;
            publicId = (string) null;
            systemId = (string) null;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EndElement:
            if (this._currentContainer.content == null)
              this._currentContainer.content = (object) string.Empty;
            if (this._currentContainer is XElement currentContainer && this._lineInfo != null && this._lineInfo.HasLineInfo())
              currentContainer.SetEndElementLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
            if (this._currentContainer == rootContainer)
              return false;
            if (this._baseUri != null && this._currentContainer.HasBaseUri)
              this._baseUri = this._currentContainer.parent.BaseUri;
            this._currentContainer = this._currentContainer.parent;
            goto case XmlNodeType.EndEntity;
          case XmlNodeType.EndEntity:
            if (newNode != null)
            {
              if (this._baseUri != null && this._baseUri != baseUri)
                newNode.SetBaseUri(baseUri);
              if (this._lineInfo != null && this._lineInfo.HasLineInfo())
                newNode.SetLineInfo(this._lineInfo.LineNumber, this._lineInfo.LinePosition);
              this._currentContainer.AddNodeSkipNotify(newNode);
              newNode = (XNode) null;
            }
            return true;
          default:
            throw new InvalidOperationException(SR.Format(SR.InvalidOperation_UnexpectedNodeType, (object) r.NodeType));
        }
      }
    }
  }
}
