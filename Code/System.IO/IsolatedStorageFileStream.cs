// Decompiled with JetBrains decompiler
// Type: System.IO.IsolatedStorage.IsolatedStorageFileStream
// Assembly: System.IO.IsolatedStorage, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 87FE0B2F-0A44-4572-BEFC-C86F7165516A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.IsolatedStorage.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.IsolatedStorage.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO.IsolatedStorage
{
  /// <summary>Exposes a file within isolated storage.</summary>
  public class IsolatedStorageFileStream : FileStream
  {

    #nullable disable
    private readonly FileStream _fs;
    private readonly IsolatedStorageFile _isf;
    private readonly string _givenPath;
    private readonly string _fullPath;


    #nullable enable
    /// <summary>Initializes a new instance of an <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object giving access to the file designated by <paramref name="path" /> in the specified <paramref name="mode" />.</summary>
    /// <param name="path">The relative path of the file within isolated storage.</param>
    /// <param name="mode">One of the <see cref="T:System.IO.FileMode" /> values.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> is badly formed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory in <paramref name="path" /> does not exist.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" /></exception>
    public IsolatedStorageFileStream(string path, FileMode mode)
      : this(path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, (IsolatedStorageFile) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> class giving access to the file designated by <paramref name="path" />, in the specified <paramref name="mode" />, and in the context of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> specified by <paramref name="isf" />.</summary>
    /// <param name="path">The relative path of the file within isolated storage.</param>
    /// <param name="mode">One of the <see cref="T:System.IO.FileMode" /> values.</param>
    /// <param name="isf">The <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> in which to open the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" />.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> is badly formed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">
    /// <paramref name="isf" /> does not have a quota.</exception>
    public IsolatedStorageFileStream(string path, FileMode mode, IsolatedStorageFile? isf)
      : this(path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, isf)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> class giving access to the file designated by <paramref name="path" />, in the specified <paramref name="mode" />, with the kind of <paramref name="access" /> requested.</summary>
    /// <param name="path">The relative path of the file within isolated storage.</param>
    /// <param name="mode">One of the <see cref="T:System.IO.FileMode" /> values.</param>
    /// <param name="access">A bitwise combination of the <see cref="T:System.IO.FileAccess" /> values.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> is badly formed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    public IsolatedStorageFileStream(string path, FileMode mode, FileAccess access)
      : this(path, mode, access, access == FileAccess.Read ? FileShare.Read : FileShare.None, 1024, (IsolatedStorageFile) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> class giving access to the file designated by <paramref name="path" /> in the specified <paramref name="mode" />, with the specified file <paramref name="access" />, and in the context of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> specified by <paramref name="isf" />.</summary>
    /// <param name="path">The relative path of the file within isolated storage.</param>
    /// <param name="mode">One of the <see cref="T:System.IO.FileMode" /> values.</param>
    /// <param name="access">A bitwise combination of the <see cref="T:System.IO.FileAccess" /> values.</param>
    /// <param name="isf">The <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> in which to open the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" />.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> is badly formed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The isolated store is closed.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">
    /// <paramref name="isf" /> does not have a quota.</exception>
    public IsolatedStorageFileStream(
      string path,
      FileMode mode,
      FileAccess access,
      IsolatedStorageFile? isf)
      : this(path, mode, access, access == FileAccess.Read ? FileShare.Read : FileShare.None, 1024, isf)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> class giving access to the file designated by <paramref name="path" />, in the specified <paramref name="mode" />, with the specified file <paramref name="access" />, using the file sharing mode specified by <paramref name="share" />.</summary>
    /// <param name="path">The relative path of the file within isolated storage.</param>
    /// <param name="mode">One of the <see cref="T:System.IO.FileMode" /> values.</param>
    /// <param name="access">A bitwise combination of the <see cref="T:System.IO.FileAccess" /> values.</param>
    /// <param name="share">A bitwise combination of the <see cref="T:System.IO.FileShare" /> values.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> is badly formed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    public IsolatedStorageFileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share)
      : this(path, mode, access, share, 1024, (IsolatedStorageFile) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> class giving access to the file designated by <paramref name="path" />, in the specified <paramref name="mode" />, with the specified file <paramref name="access" />, using the file sharing mode specified by <paramref name="share" />, and in the context of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> specified by <paramref name="isf" />.</summary>
    /// <param name="path">The relative path of the file within isolated storage.</param>
    /// <param name="mode">One of the <see cref="T:System.IO.FileMode" /> values.</param>
    /// <param name="access">A bitwise combination of the <see cref="T:System.IO.FileAccess" /> values.</param>
    /// <param name="share">A bitwise combination of the <see cref="T:System.IO.FileShare" /> values.</param>
    /// <param name="isf">The <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> in which to open the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" />.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> is badly formed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">
    /// <paramref name="isf" /> does not have a quota.</exception>
    public IsolatedStorageFileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      IsolatedStorageFile? isf)
      : this(path, mode, access, share, 1024, isf)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> class giving access to the file designated by <paramref name="path" />, in the specified <paramref name="mode" />, with the specified file <paramref name="access" />, using the file sharing mode specified by <paramref name="share" />, with the <paramref name="buffersize" /> specified.</summary>
    /// <param name="path">The relative path of the file within isolated storage.</param>
    /// <param name="mode">One of the <see cref="T:System.IO.FileMode" /> values.</param>
    /// <param name="access">A bitwise combination of the <see cref="T:System.IO.FileAccess" /> values.</param>
    /// <param name="share">A bitwise combination of the <see cref="T:System.IO.FileShare" /> values.</param>
    /// <param name="bufferSize">The <see cref="T:System.IO.FileStream" /> buffer size.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> is badly formed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    public IsolatedStorageFileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      int bufferSize)
      : this(path, mode, access, share, bufferSize, (IsolatedStorageFile) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> class giving access to the file designated by <paramref name="path" />, in the specified <paramref name="mode" />, with the specified file <paramref name="access" />, using the file sharing mode specified by <paramref name="share" />, with the <paramref name="buffersize" /> specified, and in the context of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> specified by <paramref name="isf" />.</summary>
    /// <param name="path">The relative path of the file within isolated storage.</param>
    /// <param name="mode">One of the <see cref="T:System.IO.FileMode" /> values.</param>
    /// <param name="access">A bitwise combination of the <see cref="T:System.IO.FileAccess" /> values.</param>
    /// <param name="share">A bitwise combination of the <see cref="T:System.IO.FileShare" /> values.</param>
    /// <param name="bufferSize">The <see cref="T:System.IO.FileStream" /> buffer size.</param>
    /// <param name="isf">The <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFile" /> in which to open the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" />.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="path" /> is badly formed.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">No file was found and the <paramref name="mode" /> is set to <see cref="F:System.IO.FileMode.Open" />.</exception>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">
    /// <paramref name="isf" /> does not have a quota.</exception>
    public IsolatedStorageFileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      int bufferSize,
      IsolatedStorageFile? isf)
      : this(path, mode, access, share, bufferSize, IsolatedStorageFileStream.InitializeFileStream(path, mode, access, share, bufferSize, isf))
    {
    }


    #nullable disable
    private IsolatedStorageFileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      int bufferSize,
      IsolatedStorageFileStream.InitialiationData initializationData)
      : base(new SafeFileHandle(initializationData.NestedStream.SafeFileHandle.DangerousGetHandle(), false), access, bufferSize)
    {
      this._isf = initializationData.StorageFile;
      this._givenPath = path;
      this._fullPath = initializationData.FullPath;
      this._fs = initializationData.NestedStream;
    }

    private static IsolatedStorageFileStream.InitialiationData InitializeFileStream(
      string path,
      FileMode mode,
      FileAccess access,
      FileShare share,
      int bufferSize,
      IsolatedStorageFile isf)
    {
      switch (path)
      {
        case "":
        case "\\":
          throw new ArgumentException(SR.IsolatedStorage_Path);
        case null:
          throw new ArgumentNullException(nameof (path));
        default:
          bool flag = false;
          if (isf == null)
          {
            isf = IsolatedStorageFile.GetUserStoreForDomain();
            flag = true;
          }
          if (isf.Disposed)
            throw new ObjectDisposedException((string) null, SR.IsolatedStorage_StoreNotOpen);
          switch (mode)
          {
            case FileMode.CreateNew:
            case FileMode.Create:
            case FileMode.Open:
            case FileMode.OpenOrCreate:
            case FileMode.Truncate:
            case FileMode.Append:
              IsolatedStorageFileStream.InitialiationData initialiationData = new IsolatedStorageFileStream.InitialiationData()
              {
                FullPath = isf.GetFullPath(path),
                StorageFile = isf
              };
              try
              {
                initialiationData.NestedStream = new FileStream(initialiationData.FullPath, mode, access, share, bufferSize, FileOptions.None);
              }
              catch (Exception ex)
              {
                try
                {
                  if (flag)
                    initialiationData.StorageFile?.Dispose();
                }
                catch
                {
                }
                throw IsolatedStorageFile.GetIsolatedStorageException(SR.IsolatedStorage_Operation_ISFS, ex);
              }
              return initialiationData;
            default:
              throw new ArgumentException(SR.IsolatedStorage_FileOpenMode);
          }
      }
    }

    /// <summary>Gets a Boolean value indicating whether the file can be read.</summary>
    /// <returns>
    /// <see langword="true" /> if an <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object can be read; otherwise, <see langword="false" />.</returns>
    public override bool CanRead => this._fs.CanRead;

    /// <summary>Gets a Boolean value indicating whether you can write to the file.</summary>
    /// <returns>
    /// <see langword="true" /> if an <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object can be written; otherwise, <see langword="false" />.</returns>
    public override bool CanWrite => this._fs.CanWrite;

    /// <summary>Gets a Boolean value indicating whether seek operations are supported.</summary>
    /// <returns>
    /// <see langword="true" /> if an <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object supports seek operations; otherwise, <see langword="false" />.</returns>
    public override bool CanSeek => this._fs.CanSeek;

    /// <summary>Gets the length of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</summary>
    /// <returns>The length of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object in bytes.</returns>
    public override long Length => this._fs.Length;

    /// <summary>Gets or sets the current position of the current <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The position cannot be set to a negative number.</exception>
    /// <returns>The current position of this <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</returns>
    public override long Position
    {
      get => this._fs.Position;
      set => this._fs.Position = value;
    }

    /// <summary>Gets a Boolean value indicating whether the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object was opened asynchronously or synchronously.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object supports asynchronous access; otherwise, <see langword="false" />.</returns>
    public override bool IsAsync => this._fs.IsAsync;

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing || this._fs == null)
          return;
        this._fs.Dispose();
      }
      finally
      {
        base.Dispose(disposing);
      }
    }

    /// <summary>Asynchronously releases the unmanaged resources used by the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" />.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public override ValueTask DisposeAsync()
    {
      if (this.GetType() != typeof (IsolatedStorageFileStream))
        return base.DisposeAsync();
      return this._fs == null ? new ValueTask() : this._fs.DisposeAsync();
    }

    /// <summary>Clears buffers for this stream and causes any buffered data to be written to the file.</summary>
    public override void Flush() => this._fs.Flush();

    /// <summary>Clears buffers for this stream and causes any buffered data to be written to the file, and also clears all intermediate file buffers.</summary>
    /// <param name="flushToDisk">
    /// <see langword="true" /> to flush all intermediate file buffers; otherwise, <see langword="false" />.</param>
    public override void Flush(bool flushToDisk) => this._fs.Flush(flushToDisk);


    #nullable enable
    /// <summary>Asynchronously clears buffers for this stream and causes any buffered data to be written to the file.</summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync(CancellationToken cancellationToken) => this._fs.FlushAsync(cancellationToken);

    /// <summary>Sets the length of this <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object to the specified <paramref name="value" />.</summary>
    /// <param name="value">The new length of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="value" /> is a negative number.</exception>
    public override void SetLength(long value) => this._fs.SetLength(value);

    /// <summary>Copies bytes from the current buffered <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object to a byte array.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The offset in the buffer at which to begin writing.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <returns>The total number of bytes read into the <paramref name="buffer" />. This can be less than the number of bytes requested if that many bytes are not currently available, or zero if the end of the stream is reached.</returns>
    public override int Read(byte[] buffer, int offset, int count) => this._fs.Read(buffer, offset, count);

    /// <summary>Copies bytes from the current buffered <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object to a byte span.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <returns>The total number of bytes read into the <paramref name="buffer" />. This can be less than the number of bytes requested if that many bytes are not currently available, or zero if the end of the stream is reached.</returns>
    public override int Read(Span<byte> buffer) => this._fs.Read(buffer);

    /// <summary>Asynchronously copies bytes from the current buffered <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object to a byte array.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The offset in the buffer at which to begin writing.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation. It wraps the total number of bytes read into the <paramref name="buffer" />. This can be less than the number of bytes requested if that many bytes are not currently available, or zero if the end of the stream is reached.</returns>
    public override Task<int> ReadAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return this._fs.ReadAsync(buffer, offset, count, cancellationToken);
    }

    /// <summary>Asynchronously copies bytes from the current buffered <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object to a byte memory range.</summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation. It wraps the total number of bytes read into the <paramref name="buffer" />. This can be less than the number of bytes requested if that many bytes are not currently available, or zero if the end of the stream is reached.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<byte> buffer,
      CancellationToken cancellationToken)
    {
      return this._fs.ReadAsync(buffer, cancellationToken);
    }

    /// <summary>Reads a single byte from the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object in isolated storage.</summary>
    /// <returns>The 8-bit unsigned integer value read from the isolated storage file.</returns>
    public override int ReadByte() => this._fs.ReadByte();

    /// <summary>Sets the current position of this <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object to the specified value.</summary>
    /// <param name="offset">The new position of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</param>
    /// <param name="origin">One of the <see cref="T:System.IO.SeekOrigin" /> values.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="origin" /> must be one of the <see cref="T:System.IO.SeekOrigin" /> values.</exception>
    /// <returns>The new position in the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</returns>
    public override long Seek(long offset, SeekOrigin origin) => this._fs.Seek(offset, origin);

    /// <summary>Writes a block of bytes to the isolated storage file stream object using data read from a buffer consisting of a byte array.</summary>
    /// <param name="buffer">The byte array from which to copy bytes to the current isolated storage file stream.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> from which to begin.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The write attempt exceeds the quota for the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</exception>
    public override void Write(byte[] buffer, int offset, int count) => this._fs.Write(buffer, offset, count);

    /// <summary>Writes a block of bytes to the isolated storage file stream object using data read from a buffer consisting of a read-only byte span.</summary>
    /// <param name="buffer">The read-only byte span from which to copy bytes to the current isolated storage file stream.</param>
    public override void Write(ReadOnlySpan<byte> buffer) => this._fs.Write(buffer);

    /// <summary>Asynchronously writes a block of bytes to the isolated storage file stream object using data read from a buffer consisting of a byte array.</summary>
    /// <param name="buffer">The byte array from which to copy bytes to the current isolated storage file stream.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer" /> from which to begin.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(
      byte[] buffer,
      int offset,
      int count,
      CancellationToken cancellationToken)
    {
      return this._fs.WriteAsync(buffer, offset, count, cancellationToken);
    }

    /// <summary>Asynchronously writes a block of bytes to the isolated storage file stream object using data read from a buffer consisting of a read-only byte memory range.</summary>
    /// <param name="buffer">The read-only byte memory from which to copy bytes to the current isolated storage file stream.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override ValueTask WriteAsync(
      ReadOnlyMemory<byte> buffer,
      CancellationToken cancellationToken)
    {
      return this._fs.WriteAsync(buffer, cancellationToken);
    }

    /// <summary>Writes a single byte to the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</summary>
    /// <param name="value">The byte value to write to the isolated storage file.</param>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The write attempt exceeds the quota for the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object.</exception>
    public override void WriteByte(byte value) => this._fs.WriteByte(value);

    /// <summary>Begins an asynchronous read.</summary>
    /// <param name="array" />
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin reading.</param>
    /// <param name="numBytes">The maximum number of bytes to read.</param>
    /// <param name="userCallback">The method to call when the asynchronous read operation is completed. This parameter is optional.</param>
    /// <param name="stateObject">The status of the asynchronous read.</param>
    /// <exception cref="T:System.IO.IOException">An asynchronous read was attempted past the end of the file.</exception>
    /// <returns>An <see cref="T:System.IAsyncResult" /> object that represents the asynchronous read, which is possibly still pending. This <see cref="T:System.IAsyncResult" /> must be passed to this stream's <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFileStream.EndRead(System.IAsyncResult)" /> method to determine how many bytes were read. This can be done either by the same code that called <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFileStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> or in a callback passed to <see cref="M:System.IO.IsolatedStorage.IsolatedStorageFileStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</returns>
    public override IAsyncResult BeginRead(
      byte[] array,
      int offset,
      int numBytes,
      AsyncCallback? userCallback,
      object? stateObject)
    {
      return this._fs.BeginRead(array, offset, numBytes, userCallback, stateObject);
    }

    /// <summary>Begins an asynchronous write.</summary>
    /// <param name="array" />
    /// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="numBytes">The maximum number of bytes to write.</param>
    /// <param name="userCallback">The method to call when the asynchronous write operation is completed. This parameter is optional.</param>
    /// <param name="stateObject">The status of the asynchronous write.</param>
    /// <exception cref="T:System.IO.IOException">An asynchronous write was attempted past the end of the file.</exception>
    /// <returns>An <see cref="T:System.IAsyncResult" /> that represents the asynchronous write, which is possibly still pending. This <see cref="T:System.IAsyncResult" /> must be passed to this stream's <see cref="M:System.IO.Stream.EndWrite(System.IAsyncResult)" /> method to ensure that the write is complete, then frees resources appropriately. This can be done either by the same code that called <see cref="M:System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> or in a callback passed to <see cref="M:System.IO.Stream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" />.</returns>
    public override IAsyncResult BeginWrite(
      byte[] array,
      int offset,
      int numBytes,
      AsyncCallback? userCallback,
      object? stateObject)
    {
      return this._fs.BeginWrite(array, offset, numBytes, userCallback, stateObject);
    }

    /// <summary>Ends a pending asynchronous read request.</summary>
    /// <param name="asyncResult">The pending asynchronous request.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <returns>The number of bytes read from the stream, between zero and the number of requested bytes. Streams will only return zero at the end of the stream. Otherwise, they will block until at least one byte is available.</returns>
    public override int EndRead(IAsyncResult asyncResult) => this._fs.EndRead(asyncResult);

    /// <summary>Ends an asynchronous write.</summary>
    /// <param name="asyncResult">The pending asynchronous I/O request to end.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> parameter is <see langword="null" />.</exception>
    public override void EndWrite(IAsyncResult asyncResult) => this._fs.EndWrite(asyncResult);

    /// <summary>Gets the file handle for the file that the current <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object encapsulates. Accessing this property is not permitted on an <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object, and throws an <see cref="T:System.IO.IsolatedStorage.IsolatedStorageException" />.</summary>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The <see cref="P:System.IO.IsolatedStorage.IsolatedStorageFileStream.Handle" /> property always generates this exception.</exception>
    /// <returns>The file handle for the file that the current <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object encapsulates.</returns>
    [Obsolete("IsolatedStorageFileStream.Handle has been deprecated. Use IsolatedStorageFileStream's SafeFileHandle property instead.")]
    public override IntPtr Handle => this._fs.Handle;

    /// <summary>Allows other processes to access all or part of a file that was previously locked.</summary>
    /// <param name="position">The starting position of the range to unlock. The value of this parameter must be equal to or greater than 0 (zero).</param>
    /// <param name="length">The number of bytes to unlock.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> or <paramref name="length" /> is negative.</exception>
    [UnsupportedOSPlatform("macos")]
    public override void Unlock(long position, long length) => this._fs.Unlock(position, length);

    /// <summary>Prevents other processes from reading from or writing to the stream.</summary>
    /// <param name="position">The starting position of the range to lock. The value of this parameter must be equal to or greater than 0 (zero).</param>
    /// <param name="length">The number of bytes to lock.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="position" /> or <paramref name="length" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The file is closed.</exception>
    /// <exception cref="T:System.IO.IOException">The process cannot access the file because another process has locked a portion of the file.</exception>
    [UnsupportedOSPlatform("macos")]
    public override void Lock(long position, long length) => this._fs.Lock(position, length);

    /// <summary>Gets a <see cref="T:Microsoft.Win32.SafeHandles.SafeFileHandle" /> object that represents the operating system file handle for the file that the current <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object encapsulates.</summary>
    /// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The <see cref="P:System.IO.IsolatedStorage.IsolatedStorageFileStream.SafeFileHandle" /> property always generates this exception.</exception>
    /// <returns>A <see cref="T:Microsoft.Win32.SafeHandles.SafeFileHandle" /> object that represents the operating system file handle for the file that the current <see cref="T:System.IO.IsolatedStorage.IsolatedStorageFileStream" /> object encapsulates.</returns>
    public override SafeFileHandle SafeFileHandle => throw new IsolatedStorageException(SR.IsolatedStorage_Operation_ISFS);


    #nullable disable
    private struct InitialiationData
    {
      public FileStream NestedStream;
      public IsolatedStorageFile StorageFile;
      public string FullPath;
    }
  }
}
