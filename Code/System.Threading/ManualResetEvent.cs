// Decompiled with JetBrains decompiler
// Type: System.Threading.ManualResetEvent
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

namespace System.Threading
{
  /// <summary>Represents a thread synchronization event that, when signaled, must be reset manually. This class cannot be inherited.</summary>
  public sealed class ManualResetEvent : EventWaitHandle
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.ManualResetEvent" /> class with a Boolean value indicating whether to set the initial state to signaled.</summary>
    /// <param name="initialState">
    /// <see langword="true" /> to set the initial state signaled; <see langword="false" /> to set the initial state to nonsignaled.</param>
    public ManualResetEvent(bool initialState)
      : base(initialState, EventResetMode.ManualReset)
    {
    }
  }
}
