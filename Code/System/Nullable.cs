// Decompiled with JetBrains decompiler
// Type: System.Nullable
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;


#nullable enable
namespace System
{
  /// <summary>Supports a value type that can be assigned <see langword="null" />. This class cannot be inherited.</summary>
  public static class Nullable
  {
    /// <summary>Compares the relative values of two <see cref="T:System.Nullable`1" /> objects.</summary>
    /// <param name="n1">A <see cref="T:System.Nullable`1" /> object.</param>
    /// <param name="n2">A <see cref="T:System.Nullable`1" /> object.</param>
    /// <typeparam name="T">The underlying value type of the <paramref name="n1" /> and <paramref name="n2" /> parameters.</typeparam>
    /// <returns>An integer that indicates the relative values of the <paramref name="n1" /> and <paramref name="n2" /> parameters.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term> Less than zero</term><description> The <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n1" /> is <see langword="false" />, and the <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n2" /> is <see langword="true" />, or the <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is less than the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.</description></item><item><term> Zero</term><description> The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="false" />, or the <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is equal to the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.</description></item><item><term> Greater than zero</term><description> The <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n1" /> is <see langword="true" />, and the <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n2" /> is <see langword="false" />, or the <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is greater than the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.</description></item></list></returns>
    public static int Compare<T>(T? n1, T? n2) where T : struct => n1.HasValue ? (n2.HasValue ? Comparer<T>.Default.Compare(n1.value, n2.value) : 1) : (n2.HasValue ? -1 : 0);

    /// <summary>Indicates whether two specified <see cref="T:System.Nullable`1" /> objects are equal.</summary>
    /// <param name="n1">A <see cref="T:System.Nullable`1" /> object.</param>
    /// <param name="n2">A <see cref="T:System.Nullable`1" /> object.</param>
    /// <typeparam name="T">The underlying value type of the <paramref name="n1" /> and <paramref name="n2" /> parameters.</typeparam>
    /// <returns>
    ///        <see langword="true" /> if the <paramref name="n1" /> parameter is equal to the <paramref name="n2" /> parameter; otherwise, <see langword="false" />.
    /// 
    /// The return value depends on the <see cref="P:System.Nullable`1.HasValue" /> and <see cref="P:System.Nullable`1.Value" /> properties of the two parameters that are compared.
    /// 
    /// <list type="table"><listheader><term> Return Value</term><description> Description</description></listheader><item><term><see langword="true" /></term><description> The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="false" />, or the <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the <see cref="P:System.Nullable`1.Value" /> properties of the parameters are equal.</description></item><item><term><see langword="false" /></term><description> The <see cref="P:System.Nullable`1.HasValue" /> property is <see langword="true" /> for one parameter and <see langword="false" /> for the other parameter, or the <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the <see cref="P:System.Nullable`1.Value" /> properties of the parameters are unequal.</description></item></list></returns>
    public static bool Equals<T>(T? n1, T? n2) where T : struct => n1.HasValue ? n2.HasValue && EqualityComparer<T>.Default.Equals(n1.value, n2.value) : !n2.HasValue;

    /// <summary>Returns the underlying type argument of the specified nullable type.</summary>
    /// <param name="nullableType">A <see cref="T:System.Type" /> object that describes a closed generic nullable type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="nullableType" /> is <see langword="null" />.</exception>
    /// <returns>The type argument of the <paramref name="nullableType" /> parameter, if the <paramref name="nullableType" /> parameter is a closed generic nullable type; otherwise, <see langword="null" />.</returns>
    public static Type? GetUnderlyingType(Type nullableType)
    {
      if ((object) nullableType == null)
        throw new ArgumentNullException(nameof (nullableType));
      return nullableType.IsGenericType && !nullableType.IsGenericTypeDefinition && (object) nullableType.GetGenericTypeDefinition() == (object) typeof (Nullable<>) ? nullableType.GetGenericArguments()[0] : (Type) null;
    }
  }
}
