// Decompiled with JetBrains decompiler
// Type: System.Threading.IOCompletionCallback
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.Overlapped.xml

namespace System.Threading
{
  /// <summary>Receives the error code, number of bytes, and overlapped value type when an I/O operation completes on the thread pool.</summary>
  /// <param name="errorCode">The error code.</param>
  /// <param name="numBytes">The number of bytes that are transferred.</param>
  /// <param name="pOVERLAP">A <see cref="T:System.Threading.NativeOverlapped" /> representing an unmanaged pointer to the native overlapped value type.</param>
  [CLSCompliant(false)]
  public unsafe delegate void IOCompletionCallback(
    uint errorCode,
    uint numBytes,
    NativeOverlapped* pOVERLAP);
}
