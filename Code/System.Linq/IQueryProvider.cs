// Decompiled with JetBrains decompiler
// Type: System.Linq.IQueryProvider
// Assembly: System.Linq.Expressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: BBE21D2C-A149-47D8-AE05-27D6C23D5CC3
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Linq.Expressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Linq.Expressions.xml

using System.Linq.Expressions;


#nullable enable
namespace System.Linq
{
  /// <summary>Defines methods to create and execute queries that are described by an <see cref="T:System.Linq.IQueryable" /> object.</summary>
  public interface IQueryProvider
  {
    /// <summary>Constructs an <see cref="T:System.Linq.IQueryable" /> object that can evaluate the query represented by a specified expression tree.</summary>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <returns>An <see cref="T:System.Linq.IQueryable" /> that can evaluate the query represented by the specified expression tree.</returns>
    IQueryable CreateQuery(Expression expression);

    /// <summary>Constructs an <see cref="T:System.Linq.IQueryable`1" /> object that can evaluate the query represented by a specified expression tree.</summary>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <typeparam name="TElement">The type of the elements of the <see cref="T:System.Linq.IQueryable`1" /> that is returned.</typeparam>
    /// <returns>An <see cref="T:System.Linq.IQueryable`1" /> that can evaluate the query represented by the specified expression tree.</returns>
    IQueryable<TElement> CreateQuery<TElement>(Expression expression);

    /// <summary>Executes the query represented by a specified expression tree.</summary>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <returns>The value that results from executing the specified query.</returns>
    object? Execute(Expression expression);

    /// <summary>Executes the strongly-typed query represented by a specified expression tree.</summary>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <typeparam name="TResult">The type of the value that results from executing the query.</typeparam>
    /// <returns>The value that results from executing the specified query.</returns>
    TResult Execute<TResult>(Expression expression);
  }
}
