// Decompiled with JetBrains decompiler
// Type: System.Threading.Semaphore
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
  /// <summary>Limits the number of threads that can access a resource or pool of resources concurrently.</summary>
  public sealed class Semaphore : WaitHandle
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Semaphore" /> class, specifying the initial number of entries and the maximum number of concurrent entries.</summary>
    /// <param name="initialCount">The initial number of requests for the semaphore that can be granted concurrently.</param>
    /// <param name="maximumCount">The maximum number of requests for the semaphore that can be granted concurrently.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="initialCount" /> is greater than <paramref name="maximumCount" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="maximumCount" /> is less than 1.
    /// 
    /// -or-
    /// 
    /// <paramref name="initialCount" /> is less than 0.</exception>
    public Semaphore(int initialCount, int maximumCount)
      : this(initialCount, maximumCount, (string) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Semaphore" /> class, specifying the initial number of entries and the maximum number of concurrent entries, and optionally specifying the name of a system semaphore object.</summary>
    /// <param name="initialCount">The initial number of requests for the semaphore that can be granted concurrently.</param>
    /// <param name="maximumCount">The maximum number of requests for the semaphore that can be granted concurrently.</param>
    /// <param name="name">The name, if the synchronization object is to be shared with other processes; otherwise, <see langword="null" /> or an empty string. The name is case-sensitive.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="initialCount" /> is greater than <paramref name="maximumCount" />.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="maximumCount" /> is less than 1.
    /// 
    /// -or-
    /// 
    /// <paramref name="initialCount" /> is less than 0.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Windows only: <paramref name="name" /> specified an unknown namespace. See Object Names for more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists and has access control security, and the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.FullControl" />.</exception>
    /// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">A synchronization object with the provided <paramref name="name" /> cannot be created. A synchronization object of a different type might have the same name.</exception>
    public Semaphore(int initialCount, int maximumCount, string? name)
      : this(initialCount, maximumCount, name, out bool _)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.Semaphore" /> class, specifying the initial number of entries and the maximum number of concurrent entries, optionally specifying the name of a system semaphore object, and specifying a variable that receives a value indicating whether a new system semaphore was created.</summary>
    /// <param name="initialCount">The initial number of requests for the semaphore that can be satisfied concurrently.</param>
    /// <param name="maximumCount">The maximum number of requests for the semaphore that can be satisfied concurrently.</param>
    /// <param name="name">The name, if the synchronization object is to be shared with other processes; otherwise, <see langword="null" /> or an empty string. The name is case-sensitive.</param>
    /// <param name="createdNew">When this method returns, contains <see langword="true" /> if a local semaphore was created (that is, if <paramref name="name" /> is <see langword="null" /> or an empty string) or if the specified named system semaphore was created; <see langword="false" /> if the specified named system semaphore already existed. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="initialCount" /> is greater than <paramref name="maximumCount" />.
    /// 
    /// -or-
    /// 
    /// .NET Framework only: <paramref name="name" /> is longer than MAX_PATH (260 characters).</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="maximumCount" /> is less than 1.
    /// 
    /// -or-
    /// 
    /// <paramref name="initialCount" /> is less than 0.</exception>
    /// <exception cref="T:System.IO.IOException">
    ///         <paramref name="name" /> is invalid. This can be for various reasons, including some restrictions that may be placed by the operating system, such as an unknown prefix or invalid characters. Note that the name and common prefixes "Global" and "Local" are case-sensitive.
    /// 
    /// -or-
    /// 
    /// There was some other error. The HResult property may provide more information.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Windows only: <paramref name="name" /> specified an unknown namespace. See Object Names for more information.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists and has access control security, and the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.FullControl" />.</exception>
    /// <exception cref="T:System.Threading.WaitHandleCannotBeOpenedException">A synchronization object with the provided <paramref name="name" /> cannot be created. A synchronization object of a different type might have the same name.</exception>
    public Semaphore(int initialCount, int maximumCount, string? name, out bool createdNew)
    {
      if (initialCount < 0)
        throw new ArgumentOutOfRangeException(nameof (initialCount), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (maximumCount < 1)
        throw new ArgumentOutOfRangeException(nameof (maximumCount), SR.ArgumentOutOfRange_NeedPosNum);
      if (initialCount > maximumCount)
        throw new ArgumentException(SR.Argument_SemaphoreInitialMaximum);
      this.CreateSemaphoreCore(initialCount, maximumCount, name, out createdNew);
    }

    /// <summary>Opens the specified named semaphore, if it already exists.</summary>
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
    /// <exception cref="T:System.IO.PathTooLongException">The <paramref name="name" /> is too long. Length restrictions may depend on the operating system or configuration.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists, but the user does not have the security access required to use it.</exception>
    /// <returns>An object that represents the named system semaphore.</returns>
    [SupportedOSPlatform("windows")]
    public static Semaphore OpenExisting(string name)
    {
      Semaphore result;
      switch (Semaphore.OpenExistingWorker(name, out result))
      {
        case OpenExistingResult.NameNotFound:
          throw new WaitHandleCannotBeOpenedException();
        case OpenExistingResult.PathNotFound:
          throw new IOException(SR.Format(SR.IO_PathNotFound_Path, (object) name));
        case OpenExistingResult.NameInvalid:
          throw new WaitHandleCannotBeOpenedException(SR.Format(SR.Threading_WaitHandleCannotBeOpenedException_InvalidHandle, (object) name));
        default:
          return result;
      }
    }

    /// <summary>Opens the specified named semaphore, if it already exists, and returns a value that indicates whether the operation succeeded.</summary>
    /// <param name="name">The name of the synchronization object to be shared with other processes. The name is case-sensitive.</param>
    /// <param name="result">When this method returns, contains a <see cref="T:System.Threading.Semaphore" /> object that represents the named semaphore if the call succeeded, or <see langword="null" /> if the call failed. This parameter is treated as uninitialized.</param>
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
    /// <exception cref="T:System.UnauthorizedAccessException">The named semaphore exists, but the user does not have the security access required to use it.</exception>
    /// <returns>
    /// <see langword="true" /> if the named semaphore was opened successfully; otherwise, <see langword="false" />. In some cases, <see langword="false" /> may be returned for invalid names.</returns>
    [SupportedOSPlatform("windows")]
    public static bool TryOpenExisting(string name, [NotNullWhen(true)] out Semaphore? result) => Semaphore.OpenExistingWorker(name, out result) == OpenExistingResult.Success;

    /// <summary>Exits the semaphore and returns the previous count.</summary>
    /// <exception cref="T:System.Threading.SemaphoreFullException">The semaphore count is already at the maximum value.</exception>
    /// <exception cref="T:System.IO.IOException">A Win32 error occurred with a named semaphore.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The current semaphore represents a named system semaphore, but the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.Modify" />.
    /// 
    /// -or-
    /// 
    /// The current semaphore represents a named system semaphore, but it was not opened with <see cref="F:System.Security.AccessControl.SemaphoreRights.Modify" />.</exception>
    /// <returns>The count on the semaphore before the <see cref="Overload:System.Threading.Semaphore.Release" /> method was called.</returns>
    public int Release() => this.ReleaseCore(1);

    /// <summary>Exits the semaphore a specified number of times and returns the previous count.</summary>
    /// <param name="releaseCount">The number of times to exit the semaphore.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="releaseCount" /> is less than 1.</exception>
    /// <exception cref="T:System.Threading.SemaphoreFullException">The semaphore count is already at the maximum value.</exception>
    /// <exception cref="T:System.IO.IOException">A Win32 error occurred with a named semaphore.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The current semaphore represents a named system semaphore, but the user does not have <see cref="F:System.Security.AccessControl.SemaphoreRights.Modify" /> rights.
    /// 
    /// -or-
    /// 
    /// The current semaphore represents a named system semaphore, but it was not opened with <see cref="F:System.Security.AccessControl.SemaphoreRights.Modify" /> rights.</exception>
    /// <returns>The count on the semaphore before the <see cref="Overload:System.Threading.Semaphore.Release" /> method was called.</returns>
    public int Release(int releaseCount) => releaseCount >= 1 ? this.ReleaseCore(releaseCount) : throw new ArgumentOutOfRangeException(nameof (releaseCount), SR.ArgumentOutOfRange_NeedNonNegNum);


    #nullable disable
    private Semaphore(SafeWaitHandle handle) => this.SafeWaitHandle = handle;

    private void CreateSemaphoreCore(
      int initialCount,
      int maximumCount,
      string name,
      out bool createdNew)
    {
      SafeWaitHandle semaphoreEx = Interop.Kernel32.CreateSemaphoreEx(IntPtr.Zero, initialCount, maximumCount, name, 0U, 34603010U);
      int lastPinvokeError = Marshal.GetLastPInvokeError();
      if (semaphoreEx.IsInvalid)
      {
        if (!string.IsNullOrEmpty(name) && lastPinvokeError == 6)
          throw new WaitHandleCannotBeOpenedException(SR.Format(SR.Threading_WaitHandleCannotBeOpenedException_InvalidHandle, (object) name));
        throw Win32Marshal.GetExceptionForLastWin32Error();
      }
      createdNew = lastPinvokeError != 183;
      this.SafeWaitHandle = semaphoreEx;
    }

    private static OpenExistingResult OpenExistingWorker(
      string name,
      out Semaphore result)
    {
      switch (name)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyName, nameof (name));
        case null:
          throw new ArgumentNullException(nameof (name));
        default:
          SafeWaitHandle handle = Interop.Kernel32.OpenSemaphore(34603010U, false, name);
          if (handle.IsInvalid)
          {
            result = (Semaphore) null;
            switch (Marshal.GetLastPInvokeError())
            {
              case 2:
              case 123:
                return OpenExistingResult.NameNotFound;
              case 3:
                return OpenExistingResult.PathNotFound;
              case 6:
                return OpenExistingResult.NameInvalid;
              default:
                throw Win32Marshal.GetExceptionForLastWin32Error();
            }
          }
          else
          {
            result = new Semaphore(handle);
            return OpenExistingResult.Success;
          }
      }
    }

    private int ReleaseCore(int releaseCount)
    {
      int previousCount;
      if (!Interop.Kernel32.ReleaseSemaphore(this.SafeWaitHandle, releaseCount, out previousCount))
        throw new SemaphoreFullException();
      return previousCount;
    }
  }
}
