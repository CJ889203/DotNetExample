// Decompiled with JetBrains decompiler
// Type: System.Linq.EnumerableExecutor`1
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public class EnumerableExecutor<T> : EnumerableExecutor
  {
    private Expression expression;
    private Func<T> func;

    [__DynamicallyInvokable]
    public EnumerableExecutor(Expression expression) => this.expression = expression;

    internal override object ExecuteBoxed() => (object) this.Execute();

    internal T Execute()
    {
      if (this.func == null)
        this.func = Expression.Lambda<Func<T>>(new EnumerableRewriter().Visit(this.expression), (IEnumerable<ParameterExpression>) null).Compile();
      return this.func();
    }
  }
}
