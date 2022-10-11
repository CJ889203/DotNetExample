// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.RegexRunnerFactory
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>Creates a <see cref="T:System.Text.RegularExpressions.RegexRunner" /> class for a compiled regular expression.</summary>
  public abstract class RegexRunnerFactory
  {
    /// <summary>When overridden in a derived class, creates a <see cref="T:System.Text.RegularExpressions.RegexRunner" /> object for a specific compiled regular expression.</summary>
    /// <returns>A <see cref="T:System.Text.RegularExpressions.RegexRunner" /> object designed to execute a specific compiled regular expression.</returns>
    protected internal abstract RegexRunner CreateInstance();
  }
}
