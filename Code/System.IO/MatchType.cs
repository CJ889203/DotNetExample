// Decompiled with JetBrains decompiler
// Type: System.IO.MatchType
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.IO
{
  /// <summary>Specifies the type of wildcard matching to use.</summary>
  public enum MatchType
  {
    /// <summary>
    ///   <para>Matches using '*' and '?' wildcards.</para>
    ///   <para>
    ///     <c>*</c> matches from zero to any amount of characters. <c>?</c> matches exactly one character. <c>*.*</c> matches any name with a period in it (with <see cref="F:System.IO.MatchType.Win32" />, this would match all items).</para>
    /// </summary>
    Simple,
    /// <summary>
    ///   <para>Match using Win32 DOS style matching semantics.</para>
    ///   <para>'*', '?', '&lt;', '&gt;', and '"' are all considered wildcards. Matches in a traditional DOS <c>/</c> Windows command prompt way. <c>*.*</c> matches all files. <c>?</c> matches collapse to periods. <c>file.??t</c> will match <c>file.t</c>, <c>file.at</c>, and <c>file.txt</c>.</para>
    /// </summary>
    Win32,
  }
}
