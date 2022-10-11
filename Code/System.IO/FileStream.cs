// Decompiled with JetBrains decompiler
// Type: System.IO.FileStream
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.IO.Strategies;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Provides a <see cref="T:System.IO.Stream" /> for a file, supporting both synchronous and asynchronous read and write operations.</summary>
  public class FileStream : Stream
  {

    #nullable disable
    private readonly FileStreamStrategy _strategy;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class for the specified file handle, with the specified read/write permission.</summary>
    /// <param name="handle">A file handle for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="access">A bitwise combination of the enumeration values that sets the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="access" /> is not a field of <see cref="T:System.IO.FileAccess" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified file handle, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file handle is set for read-only access.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This constructor has been deprecated. Use FileStream(SafeFileHandle handle, FileAccess access) instead.")]
    public FileStream(IntPtr handle, FileAccess access)
      : this(handle, access, true, 4096, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class for the specified file handle, with the specified read/write permission and <see langword="FileStream" /> instance ownership.</summary>
    /// <param name="handle">A file handle for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="access">A bitwise combination of the enumeration values that sets the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object.</param>
    /// <param name="ownsHandle">
    /// <see langword="true" /> if the file handle will be owned by this <see langword="FileStream" /> instance; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="access" /> is not a field of <see cref="T:System.IO.FileAccess" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified file handle, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file handle is set for read-only access.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This constructor has been deprecated. Use FileStream(SafeFileHandle handle, FileAccess access) and optionally make a new SafeFileHandle with ownsHandle=false if needed instead.")]
    public FileStream(IntPtr handle, FileAccess access, bool ownsHandle)
      : this(handle, access, ownsHandle, 4096, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class for the specified file handle, with the specified read/write permission, <see langword="FileStream" /> instance ownership, and buffer size.</summary>
    /// <param name="handle">A file handle for the file that this <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="access">A bitwise combination of the enumeration values that sets the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object.</param>
    /// <param name="ownsHandle">
    /// <see langword="true" /> if the file handle will be owned by this <see langword="FileStream" /> instance; otherwise, <see langword="false" />.</param>
    /// <param name="bufferSize">A positive <see cref="T:System.Int32" /> value greater than 0 indicating the buffer size. The default buffer size is 4096.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified file handle, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file handle is set for read-only access.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This constructor has been deprecated. Use FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) and optionally make a new SafeFileHandle with ownsHandle=false if needed instead.")]
    public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
      : this(handle, access, ownsHandle, bufferSize, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class for the specified file handle, with the specified read/write permission, <see langword="FileStream" /> instance ownership, buffer size, and synchronous or asynchronous state.</summary>
    /// <param name="handle">A file handle for the file that this <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="access">A bitwise combination of the enumeration values that sets the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object.</param>
    /// <param name="ownsHandle">
    /// <see langword="true" /> if the file handle will be owned by this <see langword="FileStream" /> instance; otherwise, <see langword="false" />.</param>
    /// <param name="bufferSize">A positive <see cref="T:System.Int32" /> value greater than 0 indicating the buffer size. The default buffer size is 4096.</param>
    /// <param name="isAsync">
    /// <see langword="true" /> if the handle was opened asynchronously (that is, in overlapped I/O mode); otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="access" /> is less than <see langword="FileAccess.Read" /> or greater than <see langword="FileAccess.ReadWrite" /> or <paramref name="bufferSize" /> is less than or equal to 0.</exception>
    /// <exception cref="T:System.ArgumentException">The handle is invalid.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified file handle, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file handle is set for read-only access.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("This constructor has been deprecated. Use FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) and optionally make a new SafeFileHandle with ownsHandle=false if needed instead.")]
    public FileStream(
      IntPtr handle,
      FileAccess access,
      bool ownsHandle,
      int bufferSize,
      bool isAsync)
    {
      SafeFileHandle handle1 = new SafeFileHandle(handle, ownsHandle);
      try
      {
        FileStream.ValidateHandle(handle1, access, bufferSize, isAsync);
        this._strategy = FileStreamHelpers.ChooseStrategy(this, handle1, access, bufferSize, isAsync);
      }
      catch
      {
        GC.SuppressFinalize((object) handle1);
        throw;
      }
    }

    private static void ValidateHandle(SafeFileHandle handle, FileAccess access, int bufferSize)
    {
      if (handle.IsInvalid)
        throw new ArgumentException(SR.Arg_InvalidHandle, nameof (handle));
      if (access < FileAccess.Read || access > FileAccess.ReadWrite)
        throw new ArgumentOutOfRangeException(nameof (access), SR.ArgumentOutOfRange_Enum);
      if (bufferSize < 0)
      {
        ThrowHelper.ThrowArgumentOutOfRangeException_NeedNonNegNum(nameof (bufferSize));
      }
      else
      {
        if (!handle.IsClosed)
          return;
        ThrowHelper.ThrowObjectDisposedException_FileClosed();
      }
    }

    private static void ValidateHandle(
      SafeFileHandle handle,
      FileAccess access,
      int bufferSize,
      bool isAsync)
    {
      FileStream.ValidateHandle(handle, access, bufferSize);
      if (isAsync && !handle.IsAsync)
      {
        ThrowHelper.ThrowArgumentException_HandleNotAsync(nameof (handle));
      }
      else
      {
        if (isAsync || !handle.IsAsync)
          return;
        ThrowHelper.ThrowArgumentException_HandleNotSync(nameof (handle));
      }
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class for the specified file handle, with the specified read/write permission.</summary>
    /// <param name="handle">A file handle for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="access">A bitwise combination of the enumeration values that sets the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="access" /> is not a field of <see cref="T:System.IO.FileAccess" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified file handle, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file handle is set for read-only access.</exception>
    public FileStream(SafeFileHandle handle, FileAccess access)
      : this(handle, access, 4096)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class for the specified file handle, with the specified read/write permission, and buffer size.</summary>
    /// <param name="handle">A file handle for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="access">A <see cref="T:System.IO.FileAccess" /> constant that sets the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object.</param>
    /// <param name="bufferSize">A positive <see cref="T:System.Int32" /> value greater than 0 indicating the buffer size. The default buffer size is 4096.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="handle" /> parameter is an invalid handle.
    /// 
    /// -or-
    /// 
    /// The <paramref name="handle" /> parameter is a synchronous handle and it was used asynchronously.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="bufferSize" /> parameter is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified file handle, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file handle is set for read-only access.</exception>
    public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize)
    {
      FileStream.ValidateHandle(handle, access, bufferSize);
      this._strategy = FileStreamHelpers.ChooseStrategy(this, handle, access, bufferSize, handle.IsAsync);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class for the specified file handle, with the specified read/write permission, buffer size, and synchronous or asynchronous state.</summary>
    /// <param name="handle">A file handle for the file that this <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="access">A bitwise combination of the enumeration values that sets the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object.</param>
    /// <param name="bufferSize">A positive <see cref="T:System.Int32" /> value greater than 0 indicating the buffer size. The default buffer size is 4096.</param>
    /// <param name="isAsync">
    /// <see langword="true" /> if the handle was opened asynchronously (that is, in overlapped I/O mode); otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="handle" /> parameter is an invalid handle.
    /// 
    /// -or-
    /// 
    /// The <paramref name="handle" /> parameter is a synchronous handle and it was used asynchronously.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="bufferSize" /> parameter is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as a disk error, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified file handle, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file handle is set for read-only access.</exception>
    public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
    {
      FileStream.ValidateHandle(handle, access, bufferSize, isAsync);
      this._strategy = FileStreamHelpers.ChooseStrategy(this, handle, access, bufferSize, isAsync);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified path and creation mode.</summary>
    /// <param name="path">A relative or absolute path for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="mode">One of the enumeration values that determines how to open or create the file.</param>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is an empty string (""), contains only white space, or contains one or more invalid characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in an NTFS environment.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">
    /// <paramref name="path" /> specifies a file that is read-only.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> contains an invalid value.</exception>
    public FileStream(string path, FileMode mode)
      : this(path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 4096, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified path, creation mode, and read/write permission.</summary>
    /// <param name="path">A relative or absolute path for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="mode">One of the enumeration values that determines how to open or create the file.</param>
    /// <param name="access">A bitwise combination of the enumeration values that determines how the file can be accessed by the <see langword="FileStream" /> object. This also determines the values returned by the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object. <see cref="P:System.IO.FileStream.CanSeek" /> is <see langword="true" /> if <paramref name="path" /> specifies a disk file.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is an empty string (""), contains only white space, or contains one or more invalid characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in an NTFS environment.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified <paramref name="path" />, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file or directory is set for read-only access.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> contains an invalid value.</exception>
    public FileStream(string path, FileMode mode, FileAccess access)
      : this(path, mode, access, FileShare.Read, 4096, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified path, creation mode, read/write permission, and sharing permission.</summary>
    /// <param name="path">A relative or absolute path for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="mode">One of the enumeration values that determines how to open or create the file.</param>
    /// <param name="access">A bitwise combination of the enumeration values that determines how the file can be accessed by the <see langword="FileStream" /> object. This also determines the values returned by the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object. <see cref="P:System.IO.FileStream.CanSeek" /> is <see langword="true" /> if <paramref name="path" /> specifies a disk file.</param>
    /// <param name="share">A bitwise combination of the enumeration values that determines how the file will be shared by processes.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is an empty string (""), contains only white space, or contains one or more invalid characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in an NTFS environment.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified <paramref name="path" />, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file or directory is set for read-only access.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> contains an invalid value.</exception>
    public FileStream(string path, FileMode mode, FileAccess access, FileShare share)
      : this(path, mode, access, share, 4096, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified path, creation mode, read/write and sharing permission, and buffer size.</summary>
    /// <param name="path">A relative or absolute path for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="mode">One of the enumeration values that determines how to open or create the file.</param>
    /// <param name="access">A bitwise combination of the enumeration values that determines how the file can be accessed by the <see langword="FileStream" /> object. This also determines the values returned by the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object. <see cref="P:System.IO.FileStream.CanSeek" /> is <see langword="true" /> if <paramref name="path" /> specifies a disk file.</param>
    /// <param name="share">A bitwise combination of the enumeration values that determines how the file will be shared by processes.</param>
    /// <param name="bufferSize">A positive <see cref="T:System.Int32" /> value greater than 0 indicating the buffer size. The default buffer size is 4096.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is an empty string (""), contains only white space, or contains one or more invalid characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in an NTFS environment.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="bufferSize" /> is negative or zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" />, <paramref name="access" />, or <paramref name="share" /> contain an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified <paramref name="path" />, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file or directory is set for read-only access.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    public FileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      int bufferSize)
      : this(path, mode, access, share, bufferSize, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified path, creation mode, read/write and sharing permission, buffer size, and synchronous or asynchronous state.</summary>
    /// <param name="path">A relative or absolute path for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="mode">One of the enumeration values that determines how to open or create the file.</param>
    /// <param name="access">A bitwise combination of the enumeration values that determines how the file can be accessed by the <see langword="FileStream" /> object. This also determines the values returned by the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object. <see cref="P:System.IO.FileStream.CanSeek" /> is <see langword="true" /> if <paramref name="path" /> specifies a disk file.</param>
    /// <param name="share">A bitwise combination of the enumeration values that determines how the file will be shared by processes.</param>
    /// <param name="bufferSize">A positive <see cref="T:System.Int32" /> value greater than 0 indicating the buffer size. The default buffer size is 4096.</param>
    /// <param name="useAsync">Specifies whether to use asynchronous I/O or synchronous I/O. However, note that the underlying operating system might not support asynchronous I/O, so when specifying <see langword="true" />, the handle might be opened synchronously depending on the platform. When opened asynchronously, the <see cref="M:System.IO.FileStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> and <see cref="M:System.IO.FileStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> methods perform better on large reads or writes, but they might be much slower for small reads or writes. If the application is designed to take advantage of asynchronous I/O, set the <paramref name="useAsync" /> parameter to <see langword="true" />. Using asynchronous I/O correctly can speed up applications by as much as a factor of 10, but using it without redesigning the application for asynchronous I/O can decrease performance by as much as a factor of 10.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is an empty string (""), contains only white space, or contains one or more invalid characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in an NTFS environment.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="bufferSize" /> is negative or zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" />, <paramref name="access" />, or <paramref name="share" /> contain an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified <paramref name="path" />, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file or directory is set for read-only access.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    public FileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      int bufferSize,
      bool useAsync)
      : this(path, mode, access, share, bufferSize, useAsync ? FileOptions.Asynchronous : FileOptions.None)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified path, creation mode, read/write and sharing permission, the access other FileStreams can have to the same file, the buffer size, and additional file options.</summary>
    /// <param name="path">A relative or absolute path for the file that the current <see langword="FileStream" /> object will encapsulate.</param>
    /// <param name="mode">One of the enumeration values that determines how to open or create the file.</param>
    /// <param name="access">A bitwise combination of the enumeration values that determines how the file can be accessed by the <see langword="FileStream" /> object. This also determines the values returned by the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see langword="FileStream" /> object. <see cref="P:System.IO.FileStream.CanSeek" /> is <see langword="true" /> if <paramref name="path" /> specifies a disk file.</param>
    /// <param name="share">A bitwise combination of the enumeration values that determines how the file will be shared by processes.</param>
    /// <param name="bufferSize">A positive <see cref="T:System.Int32" /> value greater than 0 indicating the buffer size. The default buffer size is 4096.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies additional file options.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">.NET Framework and .NET Core versions older than 2.1: <paramref name="path" /> is an empty string (""), contains only white space, or contains one or more invalid characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in an NTFS environment.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> refers to a non-file device, such as "con:", "com1:", "lpt1:", etc. in a non-NTFS environment.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="bufferSize" /> is negative or zero.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" />, <paramref name="access" />, or <paramref name="share" /> contain an invalid value.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="access" /> requested is not permitted by the operating system for the specified <paramref name="path" />, such as when <paramref name="access" /> is <see langword="Write" /> or <see langword="ReadWrite" /> and the file or directory is set for read-only access.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.IO.FileOptions.Encrypted" /> is specified for <paramref name="options" />, but file encryption is not supported on the current platform.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    public FileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      int bufferSize,
      FileOptions options)
      : this(path, mode, access, share, bufferSize, options, 0L)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileStream" /> class with the specified path, creation mode, read/write and sharing permission, buffer size, additional file options, preallocation size, and the access other FileStreams can have to the same file.</summary>
    /// <param name="path">A relative or absolute path for the file that the current <see cref="T:System.IO.FileStream" /> instance will encapsulate.</param>
    /// <param name="options">An object that describes optional <see cref="T:System.IO.FileStream" /> parameters to use.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="options" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="path" /> is an empty string, contains only white space, or contains one or more invalid characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="path" /> refers to a non-file device, such as <c>CON:</c>, <c>COM1:</c>, or <c>LPT1:</c>, in an NTFS environment.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> refers to a non-file device, such as <c>CON:</c>, <c>COM1:</c>, <c>LPT1:</c>, etc. in a non-NTFS environment.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found, such as when <see cref="P:System.IO.FileStreamOptions.Mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.
    /// 
    /// -or-
    /// 
    /// The stream has been closed.
    /// 
    /// -or-
    /// 
    /// The disk was full (when <see cref="P:System.IO.FileStreamOptions.PreallocationSize" /> was provided and <paramref name="path" /> was pointing to a regular file).
    /// 
    /// -or-
    /// 
    /// The file was too large (when <see cref="P:System.IO.FileStreamOptions.PreallocationSize" /> was provided and <paramref name="path" /> was pointing to a regular file).</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The <see cref="P:System.IO.FileStreamOptions.Access" /> requested is not permitted by the operating system for the specified <paramref name="path" />, such as when <see cref="P:System.IO.FileStreamOptions.Access" />  is <see cref="F:System.IO.FileAccess.Write" /> or <see cref="F:System.IO.FileAccess.ReadWrite" /> and the file or directory is set for read-only access.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.IO.FileOptions.Encrypted" /> is specified for <see cref="P:System.IO.FileStreamOptions.Options" /> , but file encryption is not supported on the current platform.</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    public FileStream(string path, FileStreamOptions options)
    {
      switch (path)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyPath, nameof (path));
        case null:
          throw new ArgumentNullException(nameof (path), SR.ArgumentNull_Path);
        default:
          if (options == null)
            throw new ArgumentNullException(nameof (options));
          if ((options.Access & FileAccess.Read) != (FileAccess) 0 && options.Mode == FileMode.Append)
            throw new ArgumentException(SR.Argument_InvalidAppendMode, nameof (options));
          if ((options.Access & FileAccess.Write) == (FileAccess) 0 && (options.Mode == FileMode.Truncate || options.Mode == FileMode.CreateNew || options.Mode == FileMode.Create || options.Mode == FileMode.Append))
            throw new ArgumentException(SR.Format(SR.Argument_InvalidFileModeAndAccessCombo, (object) options.Mode, (object) options.Access), nameof (options));
          if (options.PreallocationSize > 0L)
            FileStreamHelpers.ValidateArgumentsForPreallocation(options.Mode, options.Access);
          FileStreamHelpers.SerializationGuard(options.Access);
          this._strategy = FileStreamHelpers.ChooseStrategy(this, path, options.Mode, options.Access, options.Share, options.BufferSize, options.Options, options.PreallocationSize);
          break;
      }
    }


    #nullable disable
    private FileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      int bufferSize,
      FileOptions options,
      long preallocationSize)
    {
      FileStreamHelpers.ValidateArguments(path, mode, access, share, bufferSize, options, preallocationSize);
      this._strategy = FileStreamHelpers.ChooseStrategy(this, path, mode, access, share, bufferSize, options, preallocationSize);
    }

    /// <summary>Gets the operating system file handle for the file that the current <see langword="FileStream" /> object encapsulates.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>The operating system file handle for the file encapsulated by this <see langword="FileStream" /> object, or -1 if the <see langword="FileStream" /> has been closed.</returns>
    [Obsolete("FileStream.Handle has been deprecated. Use FileStream's SafeFileHandle property instead.")]
    public virtual IntPtr Handle => this._strategy.Handle;

    /// <summary>Prevents other processes from reading from or writing to the <see cref="T:System.IO.FileStream" />.</summary>
    /// <param name="position">The beginning of the range to lock. The value of this parameter must be equal to or greater than zero (0).</param>
    /// <param name="length">The range to be locked.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> or <paramref name="length" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.IO.IOException">The process cannot access the file because another process has locked a portion of the file.</exception>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("tvos")]
    public virtual void Lock(long position, long length)
    {
      if (position < 0L || length < 0L)
        ThrowHelper.ThrowArgumentOutOfRangeException_NeedNonNegNum(position < 0L ? nameof (position) : nameof (length));
      else if (this._strategy.IsClosed)
        ThrowHelper.ThrowObjectDisposedException_FileClosed();
      this._strategy.Lock(position, length);
    }

    /// <summary>Allows access by other processes to all or part of a file that was previously locked.</summary>
    /// <param name="position">The beginning of the range to unlock.</param>
    /// <param name="length">The range to be unlocked.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> or <paramref name="length" /> is negative.</exception>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("macos")]
    [UnsupportedOSPlatform("tvos")]
    public virtual void Unlock(long position, long length)
    {
      if (position < 0L || length < 0L)
        ThrowHelper.ThrowArgumentOutOfRangeException_NeedNonNegNum(position < 0L ? nameof (position) : nameof (length));
      else if (this._strategy.IsClosed)
        ThrowHelper.ThrowObjectDisposedException_FileClosed();
      this._strategy.Unlock(position, length);
    }


    #nullable enable
    /// <summary>Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (this._strategy.IsClosed)
        ThrowHelper.ThrowObjectDisposedException_FileClosed();
      return this._strategy.FlushAsync(cancellationToken);
    }

    /// <summary>Reads a block of bytes from the stream and writes the data in a given buffer.</summary>
    /// <param name="buffer" />
    /// <param name="offset">The byte offset in <paramref name="array" /> at which the read bytes will be placed.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="offset" /> and <paramref name="count" /> describe an invalid range in <paramref name="array" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The total number of bytes read into the buffer. This might be less than the number of bytes requested if that number of bytes are not currently available, or zero if the end of the stream is reached.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
      this.ValidateReadWriteArgs(buffer, offset, count);
      return this._strategy.Read(buffer, offset, count);
    }

    /// <summary>Reads a sequence of bytes from the current file stream and advances the position within the file stream by the number of bytes read.</summary>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this region are replaced by the bytes read from the current file stream.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    public override int Read(Span<byte> buffer) => this._strategy.Read(buffer);

    /// <summary>Asynchronously reads a sequence of bytes from the current file stream and writes them to a byte array beginning at a specified offset, advances the position within the file stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation and wraps the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled<int>(cancellationToken);
      if (!this._strategy.CanRead)
      {
        if (this._strategy.IsClosed)
          ThrowHelper.ThrowObjectDisposedException_FileClosed();
        ThrowHelper.ThrowNotSupportedException_UnreadableStream();
      }
      return this._strategy.ReadAsync(buffer, offset, count, cancellationToken);
    }

    /// <summary>Asynchronously reads a sequence of bytes from the current file stream and writes them to a memory region, advances the position within the file stream by the number of bytes read, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation and wraps the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled<int>(cancellationToken);
      if (!this._strategy.CanRead)
      {
        if (this._strategy.IsClosed)
          ThrowHelper.ThrowObjectDisposedException_FileClosed();
        ThrowHelper.ThrowNotSupportedException_UnreadableStream();
      }
      return this._strategy.ReadAsync(buffer, cancellationToken);
    }

    /// <summary>Writes a block of bytes to the file stream.</summary>
    /// <param name="buffer" />
    /// <param name="offset">The zero-based byte offset in <paramref name="array" /> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="offset" /> and <paramref name="count" /> describe an invalid range in <paramref name="array" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.
    /// 
    /// -or-
    /// 
    ///  Another thread may have caused an unexpected change in the position of the operating system's file handle.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The current stream instance does not support writing.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
      this.ValidateReadWriteArgs(buffer, offset, count);
      this._strategy.Write(buffer, offset, count);
    }

    /// <summary>Writes a sequence of bytes from a read-only span to the current file stream and advances the current position within this file stream by the number of bytes written.</summary>
    /// <param name="buffer">A region of memory. This method copies the contents of this region to the current file stream.</param>
    public override void Write(ReadOnlySpan<byte> buffer) => this._strategy.Write(buffer);

    /// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (!this._strategy.CanWrite)
      {
        if (this._strategy.IsClosed)
          ThrowHelper.ThrowObjectDisposedException_FileClosed();
        ThrowHelper.ThrowNotSupportedException_UnwritableStream();
      }
      return this._strategy.WriteAsync(buffer, offset, count, cancellationToken);
    }

    /// <summary>Asynchronously writes a sequence of bytes from a memory region to the current file stream, advances the current position within this file stream by the number of bytes written, and monitors cancellation requests.</summary>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled(cancellationToken);
      if (!this._strategy.CanWrite)
      {
        if (this._strategy.IsClosed)
          ThrowHelper.ThrowObjectDisposedException_FileClosed();
        ThrowHelper.ThrowNotSupportedException_UnwritableStream();
      }
      return this._strategy.WriteAsync(buffer, cancellationToken);
    }

    /// <summary>Clears buffers for this stream and causes any buffered data to be written to the file.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    public override void Flush() => this.Flush(false);

    /// <summary>Clears buffers for this stream and causes any buffered data to be written to the file, and also clears all intermediate file buffers.</summary>
    /// <param name="flushToDisk">
    /// <see langword="true" /> to flush all intermediate file buffers; otherwise, <see langword="false" />.</param>
    public virtual void Flush(bool flushToDisk)
    {
      if (this._strategy.IsClosed)
        ThrowHelper.ThrowObjectDisposedException_FileClosed();
      this._strategy.Flush(flushToDisk);
    }

    /// <summary>Gets a value that indicates whether the current stream supports reading.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports reading; <see langword="false" /> if the stream is closed or was opened with write-only access.</returns>
    public override bool CanRead => this._strategy.CanRead;

    /// <summary>Gets a value that indicates whether the current stream supports writing.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports writing; <see langword="false" /> if the stream is closed or was opened with read-only access.</returns>
    public override bool CanWrite => this._strategy.CanWrite;


    #nullable disable
    private void ValidateReadWriteArgs(byte[] buffer, int offset, int count)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (!this._strategy.IsClosed)
        return;
      ThrowHelper.ThrowObjectDisposedException_FileClosed();
    }

    /// <summary>Sets the length of this stream to the given value.</summary>
    /// <param name="value">The new length of the stream.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error has occurred.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Attempted to set the <paramref name="value" /> parameter to less than 0.</exception>
    public override void SetLength(long value)
    {
      if (value < 0L)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      else if (this._strategy.IsClosed)
        ThrowHelper.ThrowObjectDisposedException_FileClosed();
      else if (!this.CanSeek)
        ThrowHelper.ThrowNotSupportedException_UnseekableStream();
      else if (!this.CanWrite)
        ThrowHelper.ThrowNotSupportedException_UnwritableStream();
      this._strategy.SetLength(value);
    }


    #nullable enable
    /// <summary>Gets a <see cref="T:Microsoft.Win32.SafeHandles.SafeFileHandle" /> object that represents the operating system file handle for the file that the current <see cref="T:System.IO.FileStream" /> object encapsulates.</summary>
    /// <returns>An object that represents the operating system file handle for the file that the current <see cref="T:System.IO.FileStream" /> object encapsulates.</returns>
    public virtual SafeFileHandle SafeFileHandle => this._strategy.SafeFileHandle;

    /// <summary>Gets the absolute path of the file opened in the <see langword="FileStream" />.</summary>
    /// <returns>A string that is the absolute path of the file.</returns>
    public virtual string Name => this._strategy.Name;

    /// <summary>Gets a value that indicates whether the <see langword="FileStream" /> was opened asynchronously or synchronously.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see langword="FileStream" /> was opened asynchronously; otherwise, <see langword="false" />.</returns>
    public virtual bool IsAsync => this._strategy.IsAsync;

    /// <summary>Gets the length in bytes of the stream.</summary>
    /// <exception cref="T:System.NotSupportedException">
    /// <see cref="P:System.IO.FileStream.CanSeek" /> for this stream is <see langword="false" />.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error, such as the file being closed, occurred.</exception>
    /// <returns>A long value representing the length of the stream in bytes.</returns>
    public override long Length
    {
      get
      {
        if (this._strategy.IsClosed)
          ThrowHelper.ThrowObjectDisposedException_FileClosed();
        else if (!this.CanSeek)
          ThrowHelper.ThrowNotSupportedException_UnseekableStream();
        return this._strategy.Length;
      }
    }

    /// <summary>Gets or sets the current position of this stream.</summary>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">Attempted to set the position to a negative value.</exception>
    /// <exception cref="T:System.IO.EndOfStreamException">Attempted seeking past the end of a stream that does not support this.</exception>
    /// <returns>The current position of this stream.</returns>
    public override long Position
    {
      get
      {
        if (this._strategy.IsClosed)
          ThrowHelper.ThrowObjectDisposedException_FileClosed();
        else if (!this.CanSeek)
          ThrowHelper.ThrowNotSupportedException_UnseekableStream();
        return this._strategy.Position;
      }
      set
      {
        if (value < 0L)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        this._strategy.Seek(value, SeekOrigin.Begin);
      }
    }

    /// <summary>Reads a byte from the file and advances the read position one byte.</summary>
    /// <exception cref="T:System.NotSupportedException">The current stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The current stream is closed.</exception>
    /// <returns>The byte, cast to an <see cref="T:System.Int32" />, or -1 if the end of the stream has been reached.</returns>
    public override int ReadByte() => this._strategy.ReadByte();

    /// <summary>Writes a byte to the current position in the file stream.</summary>
    /// <param name="value">A byte to write to the stream.</param>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    public override void WriteByte(byte value) => this._strategy.WriteByte(value);

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.FileStream" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing) => this._strategy.DisposeInternal(disposing);

    internal void DisposeInternal(bool disposing) => this.Dispose(disposing);

    /// <summary>Asynchronously releases the unmanaged resources used by the <see cref="T:System.IO.FileStream" />.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override ValueTask DisposeAsync() => this._strategy.DisposeAsync();

    public override void CopyTo(Stream destination, int bufferSize)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      this._strategy.CopyTo(destination, bufferSize);
    }

    /// <summary>Asynchronously reads the bytes from the current file stream and writes them to another stream, using a specified buffer size and cancellation token.</summary>
    /// <param name="destination">The stream to which the contents of the current file stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero. The default size is 81920.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    public override Task CopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      Stream.ValidateCopyToArguments(destination, bufferSize);
      return this._strategy.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    /// <summary>Begins an asynchronous read operation. Consider using <see cref="M:System.IO.FileStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.</summary>
    /// <param name="buffer">The buffer to read data into.</param>
    /// <param name="offset">The byte offset in <paramref name="array" /> at which to begin reading.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="callback">The method to be called when the asynchronous read operation is completed.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
    /// <exception cref="T:System.ArgumentException">The array length minus <paramref name="offset" /> is less than <paramref name="numBytes" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="numBytes" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An asynchronous read was attempted past the end of the file.</exception>
    /// <returns>An object that references the asynchronous read.</returns>
    public override IAsyncResult BeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? callback,
      object? state)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (this._strategy.IsClosed)
        ThrowHelper.ThrowObjectDisposedException_FileClosed();
      else if (!this.CanRead)
        ThrowHelper.ThrowNotSupportedException_UnreadableStream();
      return this._strategy.BeginRead(buffer, offset, count, callback, state);
    }

    /// <summary>Waits for the pending asynchronous read operation to complete. (Consider using <see cref="M:System.IO.FileStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
    /// <param name="asyncResult">The reference to the pending asynchronous request to wait for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">This <see cref="T:System.IAsyncResult" /> object was not created by calling <see cref="M:System.IO.FileStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> on this class.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <see cref="M:System.IO.FileStream.EndRead(System.IAsyncResult)" /> is called multiple times.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception>
    /// <returns>The number of bytes read from the stream, between 0 and the number of bytes you requested. Streams only return 0 at the end of the stream, otherwise, they should block until at least 1 byte is available.</returns>
    public override int EndRead(IAsyncResult asyncResult) => asyncResult != null ? this._strategy.EndRead(asyncResult) : throw new ArgumentNullException(nameof (asyncResult));

    /// <summary>Begins an asynchronous write operation. Consider using <see cref="M:System.IO.FileStream.WriteAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.</summary>
    /// <param name="buffer">The buffer containing data to write to the current stream.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="array" /> at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="callback">The method to be called when the asynchronous write operation is completed.</param>
    /// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="array" /> length minus <paramref name="offset" /> is less than <paramref name="numBytes" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="offset" /> or <paramref name="numBytes" /> is negative.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>An object that references the asynchronous write.</returns>
    public override IAsyncResult BeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback? callback,
      object? state)
    {
      Stream.ValidateBufferArguments(buffer, offset, count);
      if (this._strategy.IsClosed)
        ThrowHelper.ThrowObjectDisposedException_FileClosed();
      else if (!this.CanWrite)
        ThrowHelper.ThrowNotSupportedException_UnwritableStream();
      return this._strategy.BeginWrite(buffer, offset, count, callback, state);
    }

    /// <summary>Ends an asynchronous write operation and blocks until the I/O operation is complete. (Consider using <see cref="M:System.IO.FileStream.WriteAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
    /// <param name="asyncResult">The pending asynchronous I/O request.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">This <see cref="T:System.IAsyncResult" /> object was not created by calling <see cref="M:System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> on this class.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <see cref="M:System.IO.FileStream.EndWrite(System.IAsyncResult)" /> is called multiple times.</exception>
    /// <exception cref="T:System.IO.IOException">The stream is closed or an internal error has occurred.</exception>
    public override void EndWrite(IAsyncResult asyncResult)
    {
      if (asyncResult == null)
        throw new ArgumentNullException(nameof (asyncResult));
      this._strategy.EndWrite(asyncResult);
    }

    /// <summary>Gets a value that indicates whether the current stream supports seeking.</summary>
    /// <returns>
    /// <see langword="true" /> if the stream supports seeking; <see langword="false" /> if the stream is closed or if the <see langword="FileStream" /> was constructed from an operating-system handle such as a pipe or output to the console.</returns>
    public override bool CanSeek => this._strategy.CanSeek;

    /// <summary>Sets the current position of this stream to the given value.</summary>
    /// <param name="offset">The point relative to <paramref name="origin" /> from which to begin seeking.</param>
    /// <param name="origin">Specifies the beginning, the end, or the current position as a reference point for <paramref name="offset" />, using a value of type <see cref="T:System.IO.SeekOrigin" />.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the <see langword="FileStream" /> is constructed from a pipe or console output.</exception>
    /// <exception cref="T:System.ArgumentException">Seeking is attempted before the beginning of the stream.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The new position in the stream.</returns>
    public override long Seek(long offset, SeekOrigin origin) => this._strategy.Seek(offset, origin);


    #nullable disable
    internal Task BaseFlushAsync(CancellationToken cancellationToken) => base.FlushAsync(cancellationToken);

    internal int BaseRead(Span<byte> buffer) => base.Read(buffer);

    internal Task<int> BaseReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return base.ReadAsync(buffer, offset, count, cancellationToken);
    }

    internal ValueTask<int> BaseReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return base.ReadAsync(buffer, cancellationToken);
    }

    internal void BaseWrite(ReadOnlySpan<byte> buffer) => base.Write(buffer);

    internal Task BaseWriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return base.WriteAsync(buffer, offset, count, cancellationToken);
    }

    internal ValueTask BaseWriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return base.WriteAsync(buffer, cancellationToken);
    }

    internal ValueTask BaseDisposeAsync() => base.DisposeAsync();

    internal Task BaseCopyToAsync(
      Stream destination,
      int bufferSize,
      CancellationToken cancellationToken)
    {
      return base.CopyToAsync(destination, bufferSize, cancellationToken);
    }

    internal IAsyncResult BaseBeginRead(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return base.BeginRead(buffer, offset, count, callback, state);
    }

    internal int BaseEndRead(IAsyncResult asyncResult) => base.EndRead(asyncResult);

    internal IAsyncResult BaseBeginWrite(
      byte[] buffer,
      int offset,
      int count,
      AsyncCallback callback,
      object state)
    {
      return base.BeginWrite(buffer, offset, count, callback, state);
    }

    internal void BaseEndWrite(IAsyncResult asyncResult) => base.EndWrite(asyncResult);
  }
}
