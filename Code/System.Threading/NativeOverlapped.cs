// Decompiled with JetBrains decompiler
// Type: System.Threading.NativeOverlapped
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Overlapped.xml

namespace System.Threading
{
  /// <summary>Provides an explicit layout that is visible from unmanaged code and that will have the same layout as the Win32 OVERLAPPED structure with additional reserved fields at the end.</summary>
  public struct NativeOverlapped
  {
    /// <summary>Specifies a system-dependent status. Reserved for operating system use.</summary>
    public IntPtr InternalLow;
    /// <summary>Specifies the length of the data transferred. Reserved for operating system use.</summary>
    public IntPtr InternalHigh;
    /// <summary>Specifies a file position at which to start the transfer.</summary>
    public int OffsetLow;
    /// <summary>Specifies the high word of the byte offset at which to start the transfer.</summary>
    public int OffsetHigh;
    /// <summary>Specifies the handle to an event set to the signaled state when the operation is complete. The calling process must set this member either to zero or to a valid event handle before calling any overlapped functions.</summary>
    public IntPtr EventHandle;
  }
}
