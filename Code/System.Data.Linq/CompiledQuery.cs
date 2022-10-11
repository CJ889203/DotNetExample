// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.CompiledQuery
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Data.Linq.Mapping;
using System.Data.Linq.Provider;
using System.Linq.Expressions;

namespace System.Data.Linq
{
  public sealed class CompiledQuery
  {
    private LambdaExpression query;
    private ICompiledQuery compiled;
    private MappingSource mappingSource;

    private CompiledQuery(LambdaExpression query) => this.query = query;

    public LambdaExpression Expression => this.query;

    public static Func<TArg0, TResult> Compile<TArg0, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TResult>);
    }

    public static Func<TArg0, TArg1, TResult> Compile<TArg0, TArg1, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TResult> Compile<TArg0, TArg1, TArg2, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>);
    }

    public static Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult> Compile<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(
      System.Linq.Expressions.Expression<Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>> query)
      where TArg0 : DataContext
    {
      if (query == null)
        Error.ArgumentNull(nameof (query));
      return CompiledQuery.UseExpressionCompile((LambdaExpression) query) ? query.Compile() : new Func<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(new CompiledQuery((LambdaExpression) query).Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>);
    }

    private static bool UseExpressionCompile(LambdaExpression query) => typeof (ITable).IsAssignableFrom(query.Body.Type);

    private TResult Invoke<TArg0, TResult>(TArg0 arg0) where TArg0 : DataContext => (TResult) this.ExecuteQuery((DataContext) arg0, new object[1]
    {
      (object) arg0
    });

    private TResult Invoke<TArg0, TArg1, TResult>(TArg0 arg0, TArg1 arg1) where TArg0 : DataContext => (TResult) this.ExecuteQuery((DataContext) arg0, new object[2]
    {
      (object) arg0,
      (object) arg1
    });

    private TResult Invoke<TArg0, TArg1, TArg2, TResult>(TArg0 arg0, TArg1 arg1, TArg2 arg2) where TArg0 : DataContext => (TResult) this.ExecuteQuery((DataContext) arg0, new object[3]
    {
      (object) arg0,
      (object) arg1,
      (object) arg2
    });

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[4]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[5]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[6]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[7]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[8]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7,
      TArg8 arg8)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[9]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7,
      TArg8 arg8,
      TArg9 arg9)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[10]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7,
      TArg8 arg8,
      TArg9 arg9,
      TArg10 arg10)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[11]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9,
        (object) arg10
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7,
      TArg8 arg8,
      TArg9 arg9,
      TArg10 arg10,
      TArg11 arg11)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[12]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9,
        (object) arg10,
        (object) arg11
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7,
      TArg8 arg8,
      TArg9 arg9,
      TArg10 arg10,
      TArg11 arg11,
      TArg12 arg12)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[13]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9,
        (object) arg10,
        (object) arg11,
        (object) arg12
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7,
      TArg8 arg8,
      TArg9 arg9,
      TArg10 arg10,
      TArg11 arg11,
      TArg12 arg12,
      TArg13 arg13)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[14]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9,
        (object) arg10,
        (object) arg11,
        (object) arg12,
        (object) arg13
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7,
      TArg8 arg8,
      TArg9 arg9,
      TArg10 arg10,
      TArg11 arg11,
      TArg12 arg12,
      TArg13 arg13,
      TArg14 arg14)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[15]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9,
        (object) arg10,
        (object) arg11,
        (object) arg12,
        (object) arg13,
        (object) arg14
      });
    }

    private TResult Invoke<TArg0, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9, TArg10, TArg11, TArg12, TArg13, TArg14, TArg15, TResult>(
      TArg0 arg0,
      TArg1 arg1,
      TArg2 arg2,
      TArg3 arg3,
      TArg4 arg4,
      TArg5 arg5,
      TArg6 arg6,
      TArg7 arg7,
      TArg8 arg8,
      TArg9 arg9,
      TArg10 arg10,
      TArg11 arg11,
      TArg12 arg12,
      TArg13 arg13,
      TArg14 arg14,
      TArg15 arg15)
      where TArg0 : DataContext
    {
      return (TResult) this.ExecuteQuery((DataContext) arg0, new object[16]
      {
        (object) arg0,
        (object) arg1,
        (object) arg2,
        (object) arg3,
        (object) arg4,
        (object) arg5,
        (object) arg6,
        (object) arg7,
        (object) arg8,
        (object) arg9,
        (object) arg10,
        (object) arg11,
        (object) arg12,
        (object) arg13,
        (object) arg14,
        (object) arg15
      });
    }

    private object ExecuteQuery(DataContext context, object[] args)
    {
      if (context == null)
        throw Error.ArgumentNull(nameof (context));
      if (this.compiled == null)
      {
        lock (this)
        {
          if (this.compiled == null)
          {
            this.compiled = context.Provider.Compile((System.Linq.Expressions.Expression) this.query);
            this.mappingSource = context.Mapping.MappingSource;
          }
        }
      }
      else if (context.Mapping.MappingSource != this.mappingSource)
        throw Error.QueryWasCompiledForDifferentMappingSource();
      return this.compiled.Execute(context.Provider, args).ReturnValue;
    }
  }
}
