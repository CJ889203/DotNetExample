// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.Extensions
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public static class Extensions
  {
    [__DynamicallyInvokable]
    public static IEnumerable<XAttribute> Attributes(
      this IEnumerable<XElement> source)
    {
      return source != null ? Extensions.GetAttributes(source, (XName) null) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XAttribute> Attributes(
      this IEnumerable<XElement> source,
      XName name)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return !(name != (XName) null) ? XAttribute.EmptySequence : Extensions.GetAttributes(source, name);
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> Ancestors<T>(this IEnumerable<T> source) where T : XNode => source != null ? Extensions.GetAncestors<T>(source, (XName) null, false) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> Ancestors<T>(
      this IEnumerable<T> source,
      XName name)
      where T : XNode
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return !(name != (XName) null) ? XElement.EmptySequence : Extensions.GetAncestors<T>(source, name, false);
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> AncestorsAndSelf(
      this IEnumerable<XElement> source)
    {
      return source != null ? Extensions.GetAncestors<XElement>(source, (XName) null, true) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> AncestorsAndSelf(
      this IEnumerable<XElement> source,
      XName name)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return !(name != (XName) null) ? XElement.EmptySequence : Extensions.GetAncestors<XElement>(source, name, true);
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XNode> Nodes<T>(this IEnumerable<T> source) where T : XContainer
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      foreach (T obj in source)
      {
        XContainer root = (XContainer) obj;
        if (root != null)
        {
          XNode n = root.LastNode;
          if (n != null)
          {
            do
            {
              n = n.next;
              yield return n;
            }
            while (n.parent == root && n != root.content);
          }
          n = (XNode) null;
        }
        root = (XContainer) null;
      }
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XNode> DescendantNodes<T>(this IEnumerable<T> source) where T : XContainer => source != null ? Extensions.GetDescendantNodes<T>(source, false) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> Descendants<T>(this IEnumerable<T> source) where T : XContainer => source != null ? Extensions.GetDescendants<T>(source, (XName) null, false) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> Descendants<T>(
      this IEnumerable<T> source,
      XName name)
      where T : XContainer
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return !(name != (XName) null) ? XElement.EmptySequence : Extensions.GetDescendants<T>(source, name, false);
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XNode> DescendantNodesAndSelf(
      this IEnumerable<XElement> source)
    {
      return source != null ? Extensions.GetDescendantNodes<XElement>(source, true) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> DescendantsAndSelf(
      this IEnumerable<XElement> source)
    {
      return source != null ? Extensions.GetDescendants<XElement>(source, (XName) null, true) : throw new ArgumentNullException(nameof (source));
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> DescendantsAndSelf(
      this IEnumerable<XElement> source,
      XName name)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return !(name != (XName) null) ? XElement.EmptySequence : Extensions.GetDescendants<XElement>(source, name, true);
    }

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> Elements<T>(this IEnumerable<T> source) where T : XContainer => source != null ? Extensions.GetElements<T>(source, (XName) null) : throw new ArgumentNullException(nameof (source));

    [__DynamicallyInvokable]
    public static IEnumerable<XElement> Elements<T>(
      this IEnumerable<T> source,
      XName name)
      where T : XContainer
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return !(name != (XName) null) ? XElement.EmptySequence : Extensions.GetElements<T>(source, name);
    }

    [__DynamicallyInvokable]
    public static IEnumerable<T> InDocumentOrder<T>(this IEnumerable<T> source) where T : XNode => (IEnumerable<T>) source.OrderBy<T, XNode>((Func<T, XNode>) (n => (XNode) n), (IComparer<XNode>) XNode.DocumentOrderComparer);

    [__DynamicallyInvokable]
    public static void Remove(this IEnumerable<XAttribute> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      foreach (XAttribute xattribute in new List<XAttribute>(source))
        xattribute?.Remove();
    }

    [__DynamicallyInvokable]
    public static void Remove<T>(this IEnumerable<T> source) where T : XNode
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      foreach (T obj in new List<T>(source))
      {
        if ((object) obj != null)
          obj.Remove();
      }
    }

    private static IEnumerable<XAttribute> GetAttributes(
      IEnumerable<XElement> source,
      XName name)
    {
      foreach (XElement e in source)
      {
        if (e != null)
        {
          XAttribute a = e.lastAttr;
          if (a != null)
          {
            do
            {
              a = a.next;
              if (name == (XName) null || a.name == name)
                yield return a;
            }
            while (a.parent == e && a != e.lastAttr);
          }
          a = (XAttribute) null;
        }
      }
    }

    private static IEnumerable<XElement> GetAncestors<T>(
      IEnumerable<T> source,
      XName name,
      bool self)
      where T : XNode
    {
      foreach (T obj in source)
      {
        XNode xnode = (XNode) obj;
        if (xnode != null)
        {
          XElement e;
          for (e = (self ? xnode : (XNode) xnode.parent) as XElement; e != null; e = e.parent as XElement)
          {
            if (name == (XName) null || e.name == name)
              yield return e;
          }
          e = (XElement) null;
        }
      }
    }

    private static IEnumerable<XNode> GetDescendantNodes<T>(
      IEnumerable<T> source,
      bool self)
      where T : XContainer
    {
      foreach (T obj in source)
      {
        XContainer root = (XContainer) obj;
        if (root != null)
        {
          if (self)
            yield return (XNode) root;
          XNode n = (XNode) root;
          while (true)
          {
            XNode firstNode;
            if (n is XContainer xcontainer && (firstNode = xcontainer.FirstNode) != null)
            {
              n = firstNode;
            }
            else
            {
              while (n != null && n != root && n == n.parent.content)
                n = (XNode) n.parent;
              if (n != null && n != root)
                n = n.next;
              else
                break;
            }
            yield return n;
          }
          n = (XNode) null;
        }
        root = (XContainer) null;
      }
    }

    private static IEnumerable<XElement> GetDescendants<T>(
      IEnumerable<T> source,
      XName name,
      bool self)
      where T : XContainer
    {
      foreach (T obj in source)
      {
        XContainer root = (XContainer) obj;
        if (root != null)
        {
          if (self)
          {
            XElement xelement = (XElement) root;
            if (name == (XName) null || xelement.name == name)
              yield return xelement;
          }
          XNode n = (XNode) root;
          XContainer xcontainer = root;
          while (true)
          {
            if (xcontainer != null && xcontainer.content is XNode)
            {
              n = ((XNode) xcontainer.content).next;
            }
            else
            {
              while (n != null && n != root && n == n.parent.content)
                n = (XNode) n.parent;
              if (n != null && n != root)
                n = n.next;
              else
                break;
            }
            XElement e = n as XElement;
            if (e != null && (name == (XName) null || e.name == name))
              yield return e;
            xcontainer = (XContainer) e;
            e = (XElement) null;
          }
          n = (XNode) null;
        }
        root = (XContainer) null;
      }
    }

    private static IEnumerable<XElement> GetElements<T>(
      IEnumerable<T> source,
      XName name)
      where T : XContainer
    {
      foreach (T obj in source)
      {
        XContainer root = (XContainer) obj;
        if (root != null)
        {
          if (root.content is XNode n)
          {
            do
            {
              n = n.next;
              if (n is XElement xelement && (name == (XName) null || xelement.name == name))
                yield return xelement;
            }
            while (n.parent == root && n != root.content);
          }
          n = (XNode) null;
        }
        root = (XContainer) null;
      }
    }
  }
}
