// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XNodeEqualityComparer
// Assembly: System.Private.Xml.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 67E106B6-1B7E-4540-BB80-26A3D9D4BC13
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.Xml.Linq.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Xml.XDocument.xml

using System.Collections;
using System.Collections.Generic;


#nullable enable
namespace System.Xml.Linq
{
  /// <summary>Compares nodes to determine whether they are equal. This class cannot be inherited.</summary>
  public sealed class XNodeEqualityComparer : IEqualityComparer, IEqualityComparer<XNode>
  {
    /// <summary>Compares the values of two nodes.</summary>
    /// <param name="x">The first <see cref="T:System.Xml.Linq.XNode" /> to compare.</param>
    /// <param name="y">The second <see cref="T:System.Xml.Linq.XNode" /> to compare.</param>
    /// <returns>A <see cref="T:System.Boolean" /> indicating if the nodes are equal.</returns>
    public bool Equals(XNode? x, XNode? y) => XNode.DeepEquals(x, y);

    /// <summary>Returns a hash code based on an <see cref="T:System.Xml.Linq.XNode" />.</summary>
    /// <param name="obj">The <see cref="T:System.Xml.Linq.XNode" /> to hash.</param>
    /// <returns>A <see cref="T:System.Int32" /> that contains a value-based hash code for the node.</returns>
    public int GetHashCode(XNode obj) => obj == null ? 0 : obj.GetDeepHashCode();


    #nullable disable
    /// <summary>Compares the values of two nodes.</summary>
    /// <param name="x">The first <see cref="T:System.Xml.Linq.XNode" /> to compare.</param>
    /// <param name="y">The second <see cref="T:System.Xml.Linq.XNode" /> to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the nodes are equal; otherwise <see langword="false" />.</returns>
    bool IEqualityComparer.Equals(object x, object y)
    {
      switch (x)
      {
        case XNode x1:
        case null:
          switch (y)
          {
            case XNode y1:
            case null:
              return this.Equals(x1, y1);
            default:
              throw new ArgumentException(SR.Format(SR.Argument_MustBeDerivedFrom, (object) typeof (XNode)), nameof (y));
          }
        default:
          throw new ArgumentException(SR.Format(SR.Argument_MustBeDerivedFrom, (object) typeof (XNode)), nameof (x));
      }
    }

    /// <summary>Returns a hash code based on the value of a node.</summary>
    /// <param name="obj">The node to hash.</param>
    /// <returns>A <see cref="T:System.Int32" /> that contains a value-based hash code for the node.</returns>
    int IEqualityComparer.GetHashCode(object obj)
    {
      switch (obj)
      {
        case XNode xnode:
        case null:
          return this.GetHashCode(xnode);
        default:
          throw new ArgumentException(SR.Format(SR.Argument_MustBeDerivedFrom, (object) typeof (XNode)), nameof (obj));
      }
    }
  }
}
