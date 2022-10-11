// Decompiled with JetBrains decompiler
// Type: System.EventArgs
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;


#nullable enable
namespace System
{
  /// <summary>Represents the base class for classes that contain event data, and provides a value to use for events that do not include event data.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class EventArgs
  {
    /// <summary>Provides a value to use with events that do not have event data.</summary>
    public static readonly EventArgs Empty = new EventArgs();
  }
}
