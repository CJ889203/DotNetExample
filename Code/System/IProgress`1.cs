// Decompiled with JetBrains decompiler
// Type: System.IProgress`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System
{
  /// <summary>Defines a provider for progress updates.</summary>
  /// <typeparam name="T">The type of progress update value.</typeparam>
  public interface IProgress<in T>
  {
    /// <summary>Reports a progress update.</summary>
    /// <param name="value">The value of the updated progress.</param>
    void Report(T value);
  }
}
