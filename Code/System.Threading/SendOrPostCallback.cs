// Decompiled with JetBrains decompiler
// Type: System.Threading.SendOrPostCallback
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Represents a method to be called when a message is to be dispatched to a synchronization context.</summary>
  /// <param name="state">The object passed to the delegate.</param>
  public delegate void SendOrPostCallback(object? state);
}
