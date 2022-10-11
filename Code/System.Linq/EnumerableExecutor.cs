// Decompiled with JetBrains decompiler
// Type: System.Linq.EnumerableExecutor
// Assembly: System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 07C38AC6-DA83-4488-8A1D-0F7BFDE87C66
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Core\v4.0_4.0.0.0__b77a5c561934e089\System.Core.dll

using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
  [__DynamicallyInvokable]
  public abstract class EnumerableExecutor
  {
    internal abstract object ExecuteBoxed();

    internal static EnumerableExecutor Create(Expression expression) => (EnumerableExecutor) Activator.CreateInstance(typeof (EnumerableExecutor<>).MakeGenericType(expression.Type), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new object[1]
    {
      (object) expression
    }, (CultureInfo) null);

    [__DynamicallyInvokable]
    protected EnumerableExecutor()
    {
    }
  }
}
