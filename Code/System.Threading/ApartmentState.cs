// Decompiled with JetBrains decompiler
// Type: System.Threading.ApartmentState
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Thread.xml

namespace System.Threading
{
  /// <summary>Specifies the apartment state of a <see cref="T:System.Threading.Thread" />.</summary>
  public enum ApartmentState
  {
    /// <summary>The <see cref="T:System.Threading.Thread" /> will create and enter a single-threaded apartment.</summary>
    STA,
    /// <summary>The <see cref="T:System.Threading.Thread" /> will create and enter a multithreaded apartment.</summary>
    MTA,
    /// <summary>The <see cref="P:System.Threading.Thread.ApartmentState" /> property has not been set.</summary>
    Unknown,
  }
}
