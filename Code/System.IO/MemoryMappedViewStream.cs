// Decompiled with JetBrains decompiler
// Type: System.IO.MemoryMappedFiles.MemoryMappedViewStream
// Assembly: System.IO.MemoryMappedFiles, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: ADB8E953-9D00-4DED-81B8-A4FE54270273
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.MemoryMappedFiles.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.MemoryMappedFiles.xml

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;


#nullable enable
namespace System.IO.MemoryMappedFiles
{
  /// <summary>Represents a view of a memory-mapped file as a sequentially accessed stream.</summary>
  public sealed class MemoryMappedViewStream : UnmanagedMemoryStream
  {

    #nullable disable
    private readonly MemoryMappedView _view;

    internal MemoryMappedViewStream(MemoryMappedView view)
    {
      this._view = view;
      this.Initialize((SafeBuffer) this._view.ViewHandle, this._view.PointerOffset, this._view.Size, MemoryMappedFile.GetFileAccess(this._view.Access));
    }


    #nullable enable
    /// <summary>Gets a handle to the view of a memory-mapped file.</summary>
    /// <returns>A wrapper for the operating system's handle to the view of the file.</returns>
    public SafeMemoryMappedViewHandle SafeMemoryMappedViewHandle => this._view.ViewHandle;

    /// <summary>Gets the number of bytes by which the starting position of this view is offset from the beginning of the memory-mapped file.</summary>
    /// <exception cref="T:System.InvalidOperationException">The object from which this instance was created is <see langword="null" />.</exception>
    /// <returns>The number of bytes between the starting position of this view and the beginning of the memory-mapped file.</returns>
    public long PointerOffset => this._view.PointerOffset;

    /// <summary>Sets the length of the current stream.</summary>
    /// <param name="value">The desired length of the current stream in bytes.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
    public override void SetLength(long value)
    {
      if (value < 0L)
        throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_NeedNonNegNum);
      throw new NotSupportedException(SR.NotSupported_MMViewStreamsFixedLength);
    }

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (!disposing || this._view.IsClosed)
          return;
        this.Flush();
      }
      finally
      {
        try
        {
          this._view.Dispose();
        }
        finally
        {
          base.Dispose(disposing);
        }
      }
    }

    /// <summary>Clears all buffers for this stream and causes any buffered data to be written to the underlying file.</summary>
    public override void Flush()
    {
      if (!this.CanSeek)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_StreamIsClosed);
      this._view.Flush((UIntPtr) (ulong) this.Capacity);
    }
  }
}
