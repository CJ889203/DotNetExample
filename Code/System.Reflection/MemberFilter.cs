// Decompiled with JetBrains decompiler
// Type: System.Reflection.MemberFilter
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents a delegate that is used to filter a list of members represented in an array of <see cref="T:System.Reflection.MemberInfo" /> objects.</summary>
  /// <param name="m">The <see cref="T:System.Reflection.MemberInfo" /> object to which the filter is applied.</param>
  /// <param name="filterCriteria">An arbitrary object used to filter the list.</param>
  /// <returns>
  /// <see langword="true" /> to include the member in the filtered list; otherwise <see langword="false" />.</returns>
  public delegate bool MemberFilter(MemberInfo m, object? filterCriteria);
}
