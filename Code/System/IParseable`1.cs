// Decompiled with JetBrains decompiler
// Type: System.IParseable`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
  public interface IParseable<TSelf> where TSelf : IParseable<TSelf>
  {
    static TSelf Parse(string s, IFormatProvider? provider);

    static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out TSelf result);
  }
}
