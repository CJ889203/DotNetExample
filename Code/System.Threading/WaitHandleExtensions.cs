// Decompiled with JetBrains decompiler
// Type: System.Threading.WaitHandleExtensions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Microsoft.Win32.SafeHandles;


#nullable enable
namespace System.Threading
{
  /// <summary>Provides convenience methods to for working with a safe handle for a wait handle.</summary>
  public static class WaitHandleExtensions
  {
    /// <summary>Gets the safe handle for a native operating system wait handle.</summary>
    /// <param name="waitHandle">A native operating system handle.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="waitHandle" /> is <see langword="null" />.</exception>
    /// <returns>The safe wait handle that wraps the native operating system wait handle.</returns>
    public static SafeWaitHandle GetSafeWaitHandle(this WaitHandle waitHandle) => waitHandle != null ? waitHandle.SafeWaitHandle : throw new ArgumentNullException(nameof (waitHandle));

    /// <summary>Sets a safe handle for a native operating system wait handle.</summary>
    /// <param name="waitHandle">A wait handle that encapsulates an operating system-specific object that waits for exclusive access to a shared resource.</param>
    /// <param name="value">The safe handle to wrap the operating system handle.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="waitHandle" /> is <see langword="null" />.</exception>
    public static void SetSafeWaitHandle(this WaitHandle waitHandle, SafeWaitHandle? value)
    {
      if (waitHandle == null)
        throw new ArgumentNullException(nameof (waitHandle));
      waitHandle.SafeWaitHandle = value;
    }
  }
}
