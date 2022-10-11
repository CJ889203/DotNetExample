// Decompiled with JetBrains decompiler
// Type: System.IO.FileStreamOptions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.IO
{
  /// <summary>Defines a variety of configuration options for <see cref="T:System.IO.FileStream" />.</summary>
  public sealed class FileStreamOptions
  {
    private FileMode _mode = FileMode.Open;
    private FileAccess _access = FileAccess.Read;
    private FileShare _share = FileShare.Read;
    private FileOptions _options;
    private long _preallocationSize;
    private int _bufferSize = 4096;

    /// <summary>One of the enumeration values that determines how to open or create the file.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">When <paramref name="value" /> contains an invalid value.</exception>
    /// <returns>One of the enumeration values of the <see cref="T:System.IO.FileMode" /> enum.</returns>
    public FileMode Mode
    {
      get => this._mode;
      set
      {
        if (value < FileMode.CreateNew || value > FileMode.Append)
          ThrowHelper.ArgumentOutOfRangeException_Enum_Value();
        this._mode = value;
      }
    }

    /// <summary>A bitwise combination of the enumeration values that determines how the file can be accessed by the <see cref="T:System.IO.FileStream" /> object. This also determines the values returned by the <see cref="P:System.IO.FileStream.CanRead" /> and <see cref="P:System.IO.FileStream.CanWrite" /> properties of the <see cref="T:System.IO.FileStream" /> object.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">When <paramref name="value" /> contains an invalid value.</exception>
    /// <returns>A bitwise combination of the <see cref="T:System.IO.FileAccess" /> enum flags.</returns>
    public FileAccess Access
    {
      get => this._access;
      set
      {
        if (value < FileAccess.Read || value > FileAccess.ReadWrite)
          ThrowHelper.ArgumentOutOfRangeException_Enum_Value();
        this._access = value;
      }
    }

    /// <summary>A bitwise combination of the enumeration values that determines how the file will be shared by processes. The default value is <see cref="F:System.IO.FileShare.Read" />.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">When <paramref name="value" /> contains an invalid value.</exception>
    /// <returns>A bitwise combination of the <see cref="T:System.IO.FileShare" /> enum flags.</returns>
    public FileShare Share
    {
      get => this._share;
      set
      {
        switch (value & ~FileShare.Inheritable)
        {
          case FileShare.None:
          case FileShare.Read:
          case FileShare.Write:
          case FileShare.ReadWrite:
          case FileShare.Delete:
          case FileShare.Read | FileShare.Delete:
          case FileShare.Write | FileShare.Delete:
          case FileShare.ReadWrite | FileShare.Delete:
            this._share = value;
            break;
          default:
            ThrowHelper.ArgumentOutOfRangeException_Enum_Value();
            goto case FileShare.None;
        }
      }
    }

    /// <summary>A bitwise combination of the enumeration values that specifies additional file options. The default value is <see cref="F:System.IO.FileOptions.None" />, which indicates synchronous IO.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">When <paramref name="value" /> contains an invalid value.</exception>
    /// <returns>A bitwise combination of the <see cref="T:System.IO.FileOptions" /> enum flags.</returns>
    public FileOptions Options
    {
      get => this._options;
      set
      {
        if (value != FileOptions.None && (value & (FileOptions) 67092479) != FileOptions.None)
          ThrowHelper.ArgumentOutOfRangeException_Enum_Value();
        this._options = value;
      }
    }

    /// <summary>The initial allocation size in bytes for the file. A positive value is effective only when a regular file is being created or overwritten (<see cref="F:System.IO.FileMode.Create" /> or <see cref="F:System.IO.FileMode.CreateNew" />). Negative values are not allowed. In other cases (including the default 0 value), it's ignored. This value is a hint and is not a strong guarantee. It is not supported on Web Assembly (WASM) and FreeBSD (the value is ignored). For Windows, Linux and macOS we will try to preallocate the disk space to fill the requested allocation size. If that turns out to be impossible, the operation is going to throw an exception. The final file length (EOF) will be determined by the number of bytes written to the file.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">When <paramref name="value" /> is negative.</exception>
    /// <returns>A non-negative number that represents the initial allocation size in bytes for the file.</returns>
    public long PreallocationSize
    {
      get => this._preallocationSize;
      set => this._preallocationSize = value >= 0L ? value : throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_NeedNonNegNum);
    }

    /// <summary>The size of the buffer used by <see cref="T:System.IO.FileStream" /> for buffering. The default buffer size is 4096.
    /// 0 or 1 means that buffering should be disabled. Negative values are not allowed.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">When <paramref name="value" /> is negative.</exception>
    /// <returns>A non-negative number that represents the buffer size used by <see cref="T:System.IO.FileStream" /> for buffering.</returns>
    public int BufferSize
    {
      get => this._bufferSize;
      set => this._bufferSize = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_NeedNonNegNum);
    }
  }
}
