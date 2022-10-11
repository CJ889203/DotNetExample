// Decompiled with JetBrains decompiler
// Type: System.Comparison`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System
{
  /// <summary>Represents the method that compares two objects of the same type.</summary>
  /// <param name="x">The first object to compare.</param>
  /// <param name="y">The second object to compare.</param>
  /// <typeparam name="T">The type of the objects to compare.</typeparam>
  /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.
  /// 
  /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than 0</term><description><paramref name="x" /> is less than <paramref name="y" />.</description></item><item><term> 0</term><description><paramref name="x" /> equals <paramref name="y" />.</description></item><item><term> Greater than 0</term><description><paramref name="x" /> is greater than <paramref name="y" />.</description></item></list></returns>
  public delegate int Comparison<in T>(T x, T y);
}
