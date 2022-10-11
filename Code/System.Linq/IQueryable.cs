// Decompiled with JetBrains decompiler
// Type: System.Linq.IQueryable
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections;
using System.Linq.Expressions;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public interface IQueryable : IEnumerable
  {
    [__DynamicallyInvokable]
    Expression Expression { [__DynamicallyInvokable] get; }

    [__DynamicallyInvokable]
    Type ElementType { [__DynamicallyInvokable] get; }

    [__DynamicallyInvokable]
    IQueryProvider Provider { [__DynamicallyInvokable] get; }
  }
}
