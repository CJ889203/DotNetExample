// Decompiled with JetBrains decompiler
// Type: System.Threading.Mutex
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using Microsoft.Win32.SafeHandles;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;


#nullable enable
namespace System.Threading
{
  /// <summary>A synchronization primitive that can also be used for interprocess synchronization.</summary>
  public sealed class Mutex : WaitHandle
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Mutex" /> class with a Boolean value that indicates whether the calling thread should have initial ownership of the mutex, a string that is the name of the mutex, and a Boolean value that, when the method returns, indicates whether the calling thread was granted initial ownership of the mutex.</summary>
    /// <param name="initiallyOwned">
    /// <see langword="true" /> to give the calling thread initial ownership of the named system mutex if the named system mutex is created as a result of this call; otherwise, <see langword="false" />.</param>
    /// <param name="name">The name, if the synchronization object is to be shared with other processes; otherwise, <see langword="null" /> or an empty string. The name is case-sensitive.</param>
    /// <param name="createdNew">When this method returns, contains a Boolean that is <see langword="true" /> if a local mutex was created (that is, if <paramref name="name" /> is <see langword="null" /> or an empty string) or if the specified named system mutex was created; <see langword="false" /> if the specified named system mutex already existed. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The named mutex exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.MutexRights.FullControl" />.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Windows only: <paramref name="name" /> specified an unknown namespace. See Object Names for more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">A synchronization object with the provided <paramref name="name" /> cannot be created. A synchronization object of a different type might have the same name.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    public Mutex(bool initiallyOwned, string? name, out bool createdNew) => this.CreateMutexCore(initiallyOwned, name, out createdNew);

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Mutex" /> class with a Boolean value that indicates whether the calling thread should have initial ownership of the mutex, and a string that is the name of the mutex.</summary>
    /// <param name="initiallyOwned">
    /// <see langword="true" /> to give the calling thread initial ownership of the named system mutex if the named system mutex is created as a result of this call; otherwise, <see langword="false" />.</param>
    /// <param name="name">The name, if the synchronization object is to be shared with other processes; otherwise, <see langword="null" /> or an empty string. The name is case-sensitive.</param>
    /// <exception cref="T:System.UnauthorizedAccessException">The named mutex exists and has access control security, but the user does not have <see cref="F:System.Security.AccessControl.MutexRights.FullControl" />.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Windows only: <paramref name="name" /> specified an unknown namespace. See Object Names for more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">A synchronization object with the provided <paramref name="name" /> cannot be created. A synchronization object of a different type might have the same name.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    public Mutex(bool initiallyOwned, string? name) => this.CreateMutexCore(initiallyOwned, name, out bool _);

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Mutex" /> class with a Boolean value that indicates whether the calling thread should have initial ownership of the mutex.</summary>
    /// <param name="initiallyOwned">
    /// <see langword="true" /> to give the calling thread initial ownership of the mutex; otherwise, <see langword="false" />.</param>
    public Mutex(bool initiallyOwned) => this.CreateMutexCore(initiallyOwned, (string) null, out bool _);

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Mutex" /> class with default properties.</summary>
    public Mutex() => this.CreateMutexCore(false, (string) null, out bool _);


    #nullable disable
    private Mutex(SafeWaitHandle handle) => this.SafeWaitHandle = handle;


    #nullable enable
    /// <summary>Opens the specified named mutex, if it already exists.</summary>
    /// <param name="name">The name of the synchronization object to be shared with other processes. The name is case-sensitive.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="name" /> is an empty string.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">A synchronization object with the provided <paramref name="name" /> cannot be created. A synchronization object of a different type might have the same name. In some cases, this exception may be thrown for invalid names.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Windows only: <paramref name="name" /> specified an unknown namespace. See Object Names for more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The named mutex exists, but the user does not have the security access required to use it.</exception>
    /// <returns>An object that represents the named system mutex.</returns>
    public static Mutex OpenExisting(string name)
    {
      Mutex result;
      switch (Mutex.OpenExistingWorker(name, out result))
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

    /// <summary>Opens the specified named mutex, if it already exists, and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="name">The name of the synchronization object to be shared with other processes. The name is case-sensitive.</param>
    /// <param name="result">When this method returns, contains a <see cref="T:System.Threading.Mutex" /> object that represents the named mutex if the call succeeded, or <see langword="null" /> if the call failed. This parameter is treated as uninitialized.</param>
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
    /// <exception cref="T:System.UnauthorizedAccessException">The named mutex exists, but the user does not have the security access required to use it.</exception>
    /// <returns>
    /// <see langword="true" /> if the named mutex was opened successfully; otherwise, <see langword="false" />. In some cases, <see langword="false" /> may be returned for invalid names.</returns>
    public static bool TryOpenExisting(string name, [NotNullWhen(true)] out Mutex? result) => Mutex.OpenExistingWorker(name, out result) == OpenExistingResult.Success;


    #nullable disable
    private void CreateMutexCore(bool initiallyOwned, string name, out bool createdNew)
    {
      uint flags = initiallyOwned ? 1U : 0U;
      SafeWaitHandle mutexEx = Interop.Kernel32.CreateMutexEx(IntPtr.Zero, name, flags, 34603009U);
      int lastPinvokeError = Marshal.GetLastPInvokeError();
      if (mutexEx.IsInvalid)
      {
        mutexEx.SetHandleAsInvalid();
        if (lastPinvokeError == 6)
          throw new WaitHandleCannotBeOpenedException(SR.Format(SR.Threading_WaitHandleCannotBeOpenedException_InvalidHandle, (object) name));
        throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError, name);
      }
      createdNew = lastPinvokeError != 183;
      this.SafeWaitHandle = mutexEx;
    }

    private static OpenExistingResult OpenExistingWorker(
      string name,
      out Mutex result)
    {
      switch (name)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyName, nameof (name));
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          result = (Mutex) null;
          SafeWaitHandle handle = Interop.Kernel32.OpenMutex(34603009U, false, name);
          if (handle.IsInvalid)
          {
            int lastPinvokeError = Marshal.GetLastPInvokeError();
            if (2 == lastPinvokeError || 123 == lastPinvokeError)
              return OpenExistingResult.NameNotFound;
            if (3 == lastPinvokeError)
              return OpenExistingResult.PathNotFound;
            if (6 == lastPinvokeError)
              return OpenExistingResult.NameInvalid;
            throw Win32Marshal.GetExceptionForWin32Error(lastPinvokeError, name);
          }
          result = new Mutex(handle);
          return OpenExistingResult.Success;
      }
    }

    /// <summary>Releases the <see cref="T:System.Threading.Mutex" /> once.</summary>
    /// <exception cref="T:System.ApplicationException">The calling thread does not own the mutex.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current instance has already been disposed.</exception>
    public void ReleaseMutex()
    {
      if (!Interop.Kernel32.ReleaseMutex(this.SafeWaitHandle))
        throw new ApplicationException(SR.Arg_SynchronizationLockException);
    }
  }
}
