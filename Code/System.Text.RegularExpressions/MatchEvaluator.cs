// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.MatchEvaluator
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>Represents the method that is called each time a regular expression match is found during a <see cref="Overload:System.Text.RegularExpressions.Regex.Replace" /> method operation.</summary>
  /// <param name="match">The <see cref="T:System.Text.RegularExpressions.Match" /> object that represents a single regular expression match during a <see cref="Overload:System.Text.RegularExpressions.Regex.Replace" /> method operation.</param>
  /// <returns>A string returned by the method that is represented by the <see cref="T:System.Text.RegularExpressions.MatchEvaluator" /> delegate.</returns>
  public delegate string MatchEvaluator(Match match);
}
