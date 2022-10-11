// Decompiled with JetBrains decompiler
// Type: System.Xml.Linq.XObjectChangeEventArgs
// Assembly: System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 0BA49A34-043C-4DB0-B03B-2DE95259DDD8
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Xml.Linq.dll

namespace System.Xml.Linq
{
  [__DynamicallyInvokable]
  public class XObjectChangeEventArgs : EventArgs
  {
    private XObjectChange objectChange;
    [__DynamicallyInvokable]
    public static readonly XObjectChangeEventArgs Add = new XObjectChangeEventArgs(XObjectChange.Add);
    [__DynamicallyInvokable]
    public static readonly XObjectChangeEventArgs Remove = new XObjectChangeEventArgs(XObjectChange.Remove);
    [__DynamicallyInvokable]
    public static readonly XObjectChangeEventArgs Name = new XObjectChangeEventArgs(XObjectChange.Name);
    [__DynamicallyInvokable]
    public static readonly XObjectChangeEventArgs Value = new XObjectChangeEventArgs(XObjectChange.Value);

    [__DynamicallyInvokable]
    public XObjectChangeEventArgs(XObjectChange objectChange) => this.objectChange = objectChange;

    [__DynamicallyInvokable]
    public XObjectChange ObjectChange
    {
      [__DynamicallyInvokable] get => this.objectChange;
    }
  }
}
