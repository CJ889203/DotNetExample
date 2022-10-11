// Decompiled with JetBrains decompiler
// Type: System.Linq.IOrderedQueryable`1
// Assembly: System.Linq.Expressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: BBE21D2C-A149-47D8-AE05-27D6C23D5CC3
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Linq.Expressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Linq.Expressions.xml

using System.Collections;
using System.Collections.Generic;


#nullable enable
namespace System.Linq
{
  /// <summary>Represents the result of a sorting operation.</summary>
  /// <typeparam name="T">The type of the content of the data source.</typeparam>
  public interface IOrderedQueryable<out T> : 
    IQueryable<T>,
    IEnumerable<T>,
    IEnumerable,
    IQueryable,
    IOrderedQueryable
  {
  }
}
