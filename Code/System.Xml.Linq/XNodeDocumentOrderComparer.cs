// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XNodeDocumentOrderComparer
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public sealed class XNodeDocumentOrderComparer : IComparer, IComparer<XNode>
  {
    [__DynamicallyInvokable]
    public int Compare(XNode x, XNode y) => XNode.CompareDocumentOrder(x, y);

    [__DynamicallyInvokable]
    int IComparer.Compare(object x, object y)
    {
      switch (x)
      {
        case XNode x1:
        case null:
          switch (y)
          {
            case XNode y1:
            case null:
              return this.Compare(x1, y1);
            default:
              throw new ArgumentException(Res.GetString("Argument_MustBeDerivedFrom", (object) typeof (XNode)), nameof (y));
          }
        default:
          throw new ArgumentException(Res.GetString("Argument_MustBeDerivedFrom", (object) typeof (XNode)), nameof (x));
      }
    }

    [__DynamicallyInvokable]
    public XNodeDocumentOrderComparer()
    {
    }
  }
}
