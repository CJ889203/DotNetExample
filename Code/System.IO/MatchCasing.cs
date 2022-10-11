// Decompiled with JetBrains decompiler
// Type: System.IO.MatchCasing
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.IO
{
  /// <summary>Specifies the type of character casing to match.</summary>
  public enum MatchCasing
  {
    /// <summary>Matches using the default casing for the given platform.</summary>
    PlatformDefault,
    /// <summary>Matches respecting character casing.</summary>
    CaseSensitive,
    /// <summary>Matches ignoring character casing.</summary>
    CaseInsensitive,
  }
}
