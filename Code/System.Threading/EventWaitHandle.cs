// Decompiled with JetBrains decompiler
// Type: System.Threading.EventWaitHandle
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;


#nullable enable
namespace System.Threading
{
  /// <summary>Represents a thread synchronization event.</summary>
  public class EventWaitHandle : WaitHandle
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.EventWaitHandle" /> class, specifying whether the wait handle is initially signaled, and whether it resets automatically or manually.</summary>
    /// <param name="initialState">
    /// <see langword="true" /> to set the initial state to signaled; <see langword="false" /> to set it to nonsignaled.</param>
    /// <param name="mode">One of the <see cref="T:System.Threading.EventResetMode" /> values that determines whether the event resets automatically or manually.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="mode" /> enum value was out of legal range.</exception>
    public EventWaitHandle(bool initialState, EventResetMode mode)
      : this(initialState, mode, (string) null, out bool _)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.EventWaitHandle" /> class, specifying whether the wait handle is initially signaled if created as a result of this call, whether it resets automatically or manually, and the name of a system synchronization event.</summary>
    /// <param name="initialState">
    /// <see langword="true" /> to set the initial state to signaled if the named event is created as a result of this call; <see langword="false" /> to set it to nonsignaled.</param>
    /// <param name="mode">One of the <see cref="T:System.Threading.EventResetMode" /> values that determines whether the event resets automatically or manually.</param>
    /// <param name="name">The name, if the synchronization object is to be shared with other processes; otherwise, <see langword="null" /> or an empty string. The name is case-sensitive.</param>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Windows only: <paramref name="name" /> specified an unknown namespace. See Object Names for more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The named event exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.EventWaitHandleRights.FullControl" />.</exception>
    /// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">A synchronization object with the provided <paramref name="name" /> cannot be created. A synchronization object of a different type might have the same name.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///           The <paramref name="mode" /> enum value was out of legal range.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    public EventWaitHandle(bool initialState, EventResetMode mode, string? name)
      : this(initialState, mode, name, out bool _)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.EventWaitHandle" /> class, specifying whether the wait handle is initially signaled if created as a result of this call, whether it resets automatically or manually, the name of a system synchronization event, and a Boolean variable whose value after the call indicates whether the named system event was created.</summary>
    /// <param name="initialState">
    /// <see langword="true" /> to set the initial state to signaled if the named event is created as a result of this call; <see langword="false" /> to set it to nonsignaled.</param>
    /// <param name="mode">One of the <see cref="T:System.Threading.EventResetMode" /> values that determines whether the event resets automatically or manually.</param>
    /// <param name="name">The name, if the synchronization object is to be shared with other processes; otherwise, <see langword="null" /> or an empty string. The name is case-sensitive.</param>
    /// <param name="createdNew">When this method returns, contains <see langword="true" /> if a local event was created (that is, if <paramref name="name" /> is <see langword="null" /> or an empty string) or if the specified named system event was created; <see langword="false" /> if the specified named system event already existed. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Windows only: <paramref name="name" /> specified an unknown namespace. See Object Names for more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The named event exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.EventWaitHandleRights.FullControl" />.</exception>
    /// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">A synchronization object with the provided <paramref name="name" /> cannot be created. A synchronization object of a different type might have the same name.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///           The <paramref name="mode" /> enum value was out of legal range.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    public EventWaitHandle(
      bool initialState,
      EventResetMode mode,
      string? name,
      out bool createdNew)
    {
      if (mode != EventResetMode.AutoReset && mode != EventResetMode.ManualReset)
        throw new ArgumentException(SR.Argument_InvalidFlag, nameof (mode));
      this.CreateEventCore(initialState, mode, name, out createdNew);
    }

    /// <summary>Opens the specified named synchronization event, if it already exists.</summary>
    /// <param name="name">The name of the synchronization object to be opened and shared with other processes. The name is case-sensitive.</param>
    /// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">A synchronization object with the provided <paramref name="name" /> cannot be opened. It may not exist, or a synchronization object of a different type might have the same name. In some cases, this exception may be thrown for invalid names.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="name" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Windows only: <paramref name="name" /> specified an unknown namespace. See Object Names for more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The named event exists, but the user does not have the security access required to use it.</exception>
    /// <returns>An  object that represents the named system event.</returns>
    [SupportedOSPlatform("windows")]
    public static EventWaitHandle OpenExisting(string name)
    {
      EventWaitHandle result;
      switch (EventWaitHandle.OpenExistingWorker(name, out result))
      {
        case OpenExistingResult.NameNotFound:
          throw new WaitHandleCannotBeOpenedException();
        case OpenExistingResult.PathNotFound:
          throw new DirectoryNotFoundException(SR.Format(SR.IO_PathNotFound_Path, (object) name));
        case OpenExistingResult.NameInvalid:
          throw new WaitHandleCannotBeOpenedException(SR.Format(SR.Threading_WaitHandleCannotBeOpenedException_InvalidHandle, (object) name));
        default:
          return result;
      }
    }

    /// <summary>Opens the specified named synchronization event, if it already exists, and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="name">The name of the synchronization object to be opened and shared with other processes. The name is case-sensitive.</param>
    /// <param name="result">When this method returns, contains a <see cref="T:System.Threading.EventWaitHandle" /> object that represents the named synchronization event if the call succeeded, or <see langword="null" /> if the call failed. This parameter is treated as uninitialized.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="name" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive. For some invalid names, the method may return <see langword="false" /> instead.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The named event exists, but the user does not have the desired security access.</exception>
    /// <returns>
    /// <see langword="true" /> if the named synchronization event was opened successfully; otherwise, <see langword="false" />. In some cases, <see langword="false" /> may be returned for invalid names.</returns>
    [SupportedOSPlatform("windows")]
    public static bool TryOpenExisting(string name, [NotNullWhen(true)] out EventWaitHandle? result) => EventWaitHandle.OpenExistingWorker(name, out result) == OpenExistingResult.Success;


    #nullable disable
    private EventWaitHandle(SafeWaitHandle handle) => this.SafeWaitHandle = handle;

    private void CreateEventCore(
      bool initialState,
      EventResetMode mode,
      string name,
      out bool createdNew)
    {
      uint flags = initialState ? 2U : 0U;
      if (mode == EventResetMode.ManualReset)
        flags |= 1U;
      SafeWaitHandle eventEx = Interop.Kernel32.CreateEventEx(IntPtr.Zero, name, flags, 34603010U);
      int lastPinvokeError = Marshal.GetLastPInvokeError();
      if (eventEx.IsInvalid)
      {
        eventEx.SetHandleAsInvalid();
        if (!string.IsNullOrEmpty(name) && lastPinvokeError == 6)
          throw new WaitHandleCannotBeOpenedException(SR.Format(SR.Threading_WaitHandleCannotBeOpenedException_InvalidHandle, (object) name));
        throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError, name);
      }
      createdNew = lastPinvokeError != 183;
      this.SafeWaitHandle = eventEx;
    }

    private static OpenExistingResult OpenExistingWorker(
      string name,
      out EventWaitHandle result)
    {
      switch (name)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyName, nameof (name));
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          result = (EventWaitHandle) null;
          SafeWaitHandle handle = Interop.Kernel32.OpenEvent(34603010U, false, name);
          if (handle.IsInvalid)
          {
            int lastPinvokeError = Marshal.GetLastPInvokeError();
            switch (lastPinvokeError)
            {
              case 2:
              case 123:
                return OpenExistingResult.NameNotFound;
              case 3:
                return OpenExistingResult.PathNotFound;
              case 6:
                return OpenExistingResult.NameInvalid;
              default:
                throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError, name);
            }
          }
          else
          {
            result = new EventWaitHandle(handle);
            return OpenExistingResult.Success;
          }
      }
    }

    /// <summary>Sets the state of the event to nonsignaled, causing threads to block.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="M:System.Threading.WaitHandle.Close" /> method was previously called on this <see cref="T:System.Threading.EventWaitHandle" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the operation succeeds; otherwise, <see langword="false" />.</returns>
    public bool Reset()
    {
      bool flag = Interop.Kernel32.ResetEvent(this.SafeWaitHandle);
      return flag ? flag : throw Win32Marshal.GetExceptionForLastWin32Error();
    }

    /// <summary>Sets the state of the event to signaled, allowing one or more waiting threads to proceed.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="M:System.Threading.WaitHandle.Close" /> method was previously called on this <see cref="T:System.Threading.EventWaitHandle" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the operation succeeds; otherwise, <see langword="false" />.</returns>
    public bool Set()
    {
      bool flag = Interop.Kernel32.SetEvent(this.SafeWaitHandle);
      return flag ? flag : throw Win32Marshal.GetExceptionForLastWin32Error();
    }

    internal static bool Set(SafeWaitHandle waitHandle) => Interop.Kernel32.SetEvent(waitHandle);
  }
}
