// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.ZipArchive
// Assembly: System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: A9780FE1-DF26-4575-BA54-75C62CD4F39E
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.xml

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


#nullable enable
namespace System.IO.Compression
{
  /// <summary>Represents a package of compressed files in the zip archive format.</summary>
  public class ZipArchive : IDisposable
  {

    #nullable disable
    private readonly Stream _archiveStream;
    private ZipArchiveEntry _archiveStreamOwner;
    private BinaryReader _archiveReader;
    private ZipArchiveMode _mode;
    private List<ZipArchiveEntry> _entries;
    private ReadOnlyCollection<ZipArchiveEntry> _entriesCollection;
    private Dictionary<string, ZipArchiveEntry> _entriesDictionary;
    private bool _readEntries;
    private bool _leaveOpen;
    private long _centralDirectoryStart;
    private bool _isDisposed;
    private uint _numberOfThisDisk;
    private long _expectedNumberOfEntries;
    private Stream _backingStream;
    private byte[] _archiveComment;
    private Encoding _entryNameEncoding;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.ZipArchive" /> class from the specified stream.</summary>
    /// <param name="stream">The stream that contains the archive to be read.</param>
    /// <exception cref="T:System.ArgumentException">The stream is already closed or does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The contents of the stream are not in the zip archive format.</exception>
    public ZipArchive(Stream stream)
      : this(stream, ZipArchiveMode.Read, false, (Encoding) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.ZipArchive" /> class from the specified stream and with the specified mode.</summary>
    /// <param name="stream">The input or output stream.</param>
    /// <param name="mode">One of the enumeration values that indicates whether the zip archive is used to read, create, or update entries.</param>
    /// <exception cref="T:System.ArgumentException">The stream is already closed, or the capabilities of the stream do not match the mode.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> is an invalid value.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The contents of the stream could not be interpreted as a zip archive.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" /> and an entry is missing from the archive or is corrupt and cannot be read.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" /> and an entry is too large to fit into memory.</exception>
    public ZipArchive(Stream stream, ZipArchiveMode mode)
      : this(stream, mode, false, (Encoding) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.ZipArchive" /> class on the specified stream for the specified mode, and optionally leaves the stream open.</summary>
    /// <param name="stream">The input or output stream.</param>
    /// <param name="mode">One of the enumeration values that indicates whether the zip archive is used to read, create, or update entries.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.Compression.ZipArchive" /> object is disposed; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentException">The stream is already closed, or the capabilities of the stream do not match the mode.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> is an invalid value.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The contents of the stream could not be interpreted as a zip archive.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" /> and an entry is missing from the archive or is corrupt and cannot be read.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" /> and an entry is too large to fit into memory.</exception>
    public ZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen)
      : this(stream, mode, leaveOpen, (Encoding) null)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.Compression.ZipArchive" /> class on the specified stream for the specified mode, uses the specified encoding for entry names, and optionally leaves the stream open.</summary>
    /// <param name="stream">The input or output stream.</param>
    /// <param name="mode">One of the enumeration values that indicates whether the zip archive is used to read, create, or update entries.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.Compression.ZipArchive" /> object is disposed; otherwise, <see langword="false" />.</param>
    /// <param name="entryNameEncoding">The encoding to use when reading or writing entry names in this archive. Specify a value for this parameter only when an encoding is required for interoperability with zip archive tools and libraries that do not support UTF-8 encoding for entry names.</param>
    /// <exception cref="T:System.ArgumentException">The stream is already closed, or the capabilities of the stream do not match the mode.
    /// 
    /// -or-
    /// 
    /// An encoding other than UTF-8 is specified for the <paramref name="entryNameEncoding" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="mode" /> is an invalid value.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The contents of the stream could not be interpreted as a zip archive.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" /> and an entry is missing from the archive or is corrupt and cannot be read.
    /// 
    /// -or-
    /// 
    /// <paramref name="mode" /> is <see cref="F:System.IO.Compression.ZipArchiveMode.Update" /> and an entry is too large to fit into memory.</exception>
    public ZipArchive(
      Stream stream,
      ZipArchiveMode mode,
      bool leaveOpen,
      Encoding? entryNameEncoding)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      this.EntryNameEncoding = entryNameEncoding;
      Stream stream1 = (Stream) null;
      try
      {
        this._backingStream = (Stream) null;
        switch (mode)
        {
          case ZipArchiveMode.Read:
            if (!stream.CanRead)
              throw new ArgumentException(SR.ReadModeCapabilities);
            if (!stream.CanSeek)
            {
              this._backingStream = stream;
              stream1 = stream = (Stream) new MemoryStream();
              this._backingStream.CopyTo(stream);
              stream.Seek(0L, SeekOrigin.Begin);
              break;
            }
            break;
          case ZipArchiveMode.Create:
            if (!stream.CanWrite)
              throw new ArgumentException(SR.CreateModeCapabilities);
            break;
          case ZipArchiveMode.Update:
            if (!stream.CanRead || !stream.CanWrite || !stream.CanSeek)
              throw new ArgumentException(SR.UpdateModeCapabilities);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (mode));
        }
        this._mode = mode;
        this._archiveStream = mode != ZipArchiveMode.Create || stream.CanSeek ? stream : (Stream) new PositionPreservingWriteOnlyStreamWrapper(stream);
        this._archiveStreamOwner = (ZipArchiveEntry) null;
        this._archiveReader = mode != ZipArchiveMode.Create ? new BinaryReader(this._archiveStream) : (BinaryReader) null;
        this._entries = new List<ZipArchiveEntry>();
        this._entriesCollection = new ReadOnlyCollection<ZipArchiveEntry>((IList<ZipArchiveEntry>) this._entries);
        this._entriesDictionary = new Dictionary<string, ZipArchiveEntry>();
        this._readEntries = false;
        this._leaveOpen = leaveOpen;
        this._centralDirectoryStart = 0L;
        this._isDisposed = false;
        this._numberOfThisDisk = 0U;
        this._archiveComment = (byte[]) null;
        switch (mode)
        {
          case ZipArchiveMode.Read:
            this.ReadEndOfCentralDirectory();
            break;
          case ZipArchiveMode.Create:
            this._readEntries = true;
            break;
          default:
            if (this._archiveStream.Length == 0L)
            {
              this._readEntries = true;
              break;
            }
            this.ReadEndOfCentralDirectory();
            this.EnsureCentralDirectoryRead();
            using (List<ZipArchiveEntry>.Enumerator enumerator = this._entries.GetEnumerator())
            {
              while (enumerator.MoveNext())
                enumerator.Current.ThrowIfNotOpenable(false, true);
              break;
            }
        }
      }
      catch
      {
        stream1?.Dispose();
        throw;
      }
    }

    /// <summary>Gets the collection of entries that are currently in the zip archive.</summary>
    /// <exception cref="T:System.NotSupportedException">The zip archive does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The zip archive is corrupt, and its entries cannot be retrieved.</exception>
    /// <returns>The collection of entries that are currently in the zip archive.</returns>
    public ReadOnlyCollection<ZipArchiveEntry> Entries
    {
      get
      {
        if (this._mode == ZipArchiveMode.Create)
          throw new NotSupportedException(SR.EntriesInCreateMode);
        this.ThrowIfDisposed();
        this.EnsureCentralDirectoryRead();
        return this._entriesCollection;
      }
    }

    /// <summary>Gets a value that describes the type of action the zip archive can perform on entries.</summary>
    /// <returns>One of the enumeration values that describes the type of action (read, create, or update) the zip archive can perform on entries.</returns>
    public ZipArchiveMode Mode => this._mode;

    /// <summary>Creates an empty entry that has the specified path and entry name in the zip archive.</summary>
    /// <param name="entryName">A path, relative to the root of the archive, that specifies the name of the entry to be created.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="entryName" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="entryName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The zip archive does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
    /// <returns>An empty entry in the zip archive.</returns>
    public ZipArchiveEntry CreateEntry(string entryName) => this.DoCreateEntry(entryName, new CompressionLevel?());

    /// <summary>Creates an empty entry that has the specified entry name and compression level in the zip archive.</summary>
    /// <param name="entryName">A path, relative to the root of the archive, that specifies the name of the entry to be created.</param>
    /// <param name="compressionLevel">One of the enumeration values that indicates whether to emphasize speed or compression effectiveness when creating the entry.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="entryName" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="entryName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The zip archive does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
    /// <returns>An empty entry in the zip archive.</returns>
    public ZipArchiveEntry CreateEntry(
      string entryName,
      CompressionLevel compressionLevel)
    {
      return this.DoCreateEntry(entryName, new CompressionLevel?(compressionLevel));
    }

    /// <summary>Called by the <see cref="M:System.IO.Compression.ZipArchive.Dispose" /> and <see cref="M:System.Object.Finalize" /> methods to release the unmanaged resources used by the current instance of the <see cref="T:System.IO.Compression.ZipArchive" /> class, and optionally finishes writing the archive and releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to finish writing the archive and release unmanaged and managed resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      if (this._isDisposed)
        return;
      try
      {
        ZipArchiveMode mode = this._mode;
        if (mode == ZipArchiveMode.Read)
          return;
        int num = (int) (mode - 1);
        this.WriteFile();
      }
      finally
      {
        this.CloseStreams();
        this._isDisposed = true;
      }
    }

    /// <summary>Releases the resources used by the current instance of the <see cref="T:System.IO.Compression.ZipArchive" /> class.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Retrieves a wrapper for the specified entry in the zip archive.</summary>
    /// <param name="entryName">A path, relative to the root of the archive, that identifies the entry to retrieve.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="entryName" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="entryName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">The zip archive does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive has been disposed.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The zip archive is corrupt, and its entries cannot be retrieved.</exception>
    /// <returns>A wrapper for the specified entry in the archive; <see langword="null" /> if the entry does not exist in the archive.</returns>
    public ZipArchiveEntry? GetEntry(string entryName)
    {
      if (entryName == null)
        throw new ArgumentNullException(nameof (entryName));
      if (this._mode == ZipArchiveMode.Create)
        throw new NotSupportedException(SR.EntriesInCreateMode);
      this.EnsureCentralDirectoryRead();
      ZipArchiveEntry entry;
      this._entriesDictionary.TryGetValue(entryName, out entry);
      return entry;
    }

    internal BinaryReader? ArchiveReader => this._archiveReader;

    internal Stream ArchiveStream => this._archiveStream;

    internal uint NumberOfThisDisk => this._numberOfThisDisk;

    internal Encoding? EntryNameEncoding
    {
      get => this._entryNameEncoding;
      private set
      {
        if (value != null && (value.Equals((object) Encoding.BigEndianUnicode) || value.Equals((object) Encoding.Unicode)))
          throw new ArgumentException(SR.EntryNameEncodingNotSupported, nameof (EntryNameEncoding));
        this._entryNameEncoding = value;
      }
    }


    #nullable disable
    private ZipArchiveEntry DoCreateEntry(
      string entryName,
      CompressionLevel? compressionLevel)
    {
      if (entryName == null)
        throw new ArgumentNullException(nameof (entryName));
      if (string.IsNullOrEmpty(entryName))
        throw new ArgumentException(SR.CannotBeEmpty, nameof (entryName));
      if (this._mode == ZipArchiveMode.Read)
        throw new NotSupportedException(SR.CreateInReadMode);
      this.ThrowIfDisposed();
      ZipArchiveEntry entry = compressionLevel.HasValue ? new ZipArchiveEntry(this, entryName, compressionLevel.Value) : new ZipArchiveEntry(this, entryName);
      this.AddEntry(entry);
      return entry;
    }

    internal void AcquireArchiveStream(ZipArchiveEntry entry)
    {
      if (this._archiveStreamOwner != null)
      {
        if (this._archiveStreamOwner.EverOpenedForWrite)
          throw new IOException(SR.CreateModeCreateEntryWhileOpen);
        this._archiveStreamOwner.WriteAndFinishLocalEntry();
      }
      this._archiveStreamOwner = entry;
    }

    private void AddEntry(ZipArchiveEntry entry)
    {
      this._entries.Add(entry);
      string fullName = entry.FullName;
      if (this._entriesDictionary.ContainsKey(fullName))
        return;
      this._entriesDictionary.Add(fullName, entry);
    }

    internal void ReleaseArchiveStream(ZipArchiveEntry entry) => this._archiveStreamOwner = (ZipArchiveEntry) null;

    internal void RemoveEntry(ZipArchiveEntry entry)
    {
      this._entries.Remove(entry);
      this._entriesDictionary.Remove(entry.FullName);
    }

    internal void ThrowIfDisposed()
    {
      if (this._isDisposed)
        throw new ObjectDisposedException(this.GetType().ToString());
    }

    private void CloseStreams()
    {
      if (!this._leaveOpen)
      {
        this._archiveStream.Dispose();
        this._backingStream?.Dispose();
        this._archiveReader?.Dispose();
      }
      else
      {
        if (this._backingStream == null)
          return;
        this._archiveStream.Dispose();
      }
    }

    private void EnsureCentralDirectoryRead()
    {
      if (this._readEntries)
        return;
      this.ReadCentralDirectory();
      this._readEntries = true;
    }

    private void ReadCentralDirectory()
    {
      try
      {
        this._archiveStream.Seek(this._centralDirectoryStart, SeekOrigin.Begin);
        long num = 0;
        bool saveExtraFieldsAndComments = this.Mode == ZipArchiveMode.Update;
        ZipCentralDirectoryFileHeader header;
        while (ZipCentralDirectoryFileHeader.TryReadBlock(this._archiveReader, saveExtraFieldsAndComments, out header))
        {
          this.AddEntry(new ZipArchiveEntry(this, header));
          ++num;
        }
        if (num != this._expectedNumberOfEntries)
          throw new InvalidDataException(SR.NumEntriesWrong);
      }
      catch (EndOfStreamException ex)
      {
        throw new InvalidDataException(SR.Format(SR.CentralDirectoryInvalid, (object) ex));
      }
    }

    private void ReadEndOfCentralDirectory()
    {
      try
      {
        this._archiveStream.Seek(-18L, SeekOrigin.End);
        long eocdStart = ZipHelper.SeekBackwardsToSignature(this._archiveStream, 101010256U, 65539) ? this._archiveStream.Position : throw new InvalidDataException(SR.EOCDNotFound);
        ZipEndOfCentralDirectoryBlock eocdBlock;
        ZipEndOfCentralDirectoryBlock.TryReadBlock(this._archiveReader, out eocdBlock);
        if ((int) eocdBlock.NumberOfThisDisk != (int) eocdBlock.NumberOfTheDiskWithTheStartOfTheCentralDirectory)
          throw new InvalidDataException(SR.SplitSpanned);
        this._numberOfThisDisk = (uint) eocdBlock.NumberOfThisDisk;
        this._centralDirectoryStart = (long) eocdBlock.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber;
        if ((int) eocdBlock.NumberOfEntriesInTheCentralDirectory != (int) eocdBlock.NumberOfEntriesInTheCentralDirectoryOnThisDisk)
          throw new InvalidDataException(SR.SplitSpanned);
        this._expectedNumberOfEntries = (long) eocdBlock.NumberOfEntriesInTheCentralDirectory;
        if (this._mode == ZipArchiveMode.Update)
          this._archiveComment = eocdBlock.ArchiveComment;
        this.TryReadZip64EndOfCentralDirectory(eocdBlock, eocdStart);
        if (this._centralDirectoryStart > this._archiveStream.Length)
          throw new InvalidDataException(SR.FieldTooBigOffsetToCD);
      }
      catch (EndOfStreamException ex)
      {
        throw new InvalidDataException(SR.CDCorrupt, (Exception) ex);
      }
      catch (IOException ex)
      {
        throw new InvalidDataException(SR.CDCorrupt, (Exception) ex);
      }
    }

    private void TryReadZip64EndOfCentralDirectory(
      ZipEndOfCentralDirectoryBlock eocd,
      long eocdStart)
    {
      if (eocd.NumberOfThisDisk != ushort.MaxValue && eocd.OffsetOfStartOfCentralDirectoryWithRespectToTheStartingDiskNumber != uint.MaxValue && eocd.NumberOfEntriesInTheCentralDirectory != ushort.MaxValue)
        return;
      this._archiveStream.Seek(eocdStart - 16L, SeekOrigin.Begin);
      if (!ZipHelper.SeekBackwardsToSignature(this._archiveStream, 117853008U, 4))
        return;
      Zip64EndOfCentralDirectoryLocator zip64EOCDLocator;
      Zip64EndOfCentralDirectoryLocator.TryReadBlock(this._archiveReader, out zip64EOCDLocator);
      if (zip64EOCDLocator.OffsetOfZip64EOCD > (ulong) long.MaxValue)
        throw new InvalidDataException(SR.FieldTooBigOffsetToZip64EOCD);
      this._archiveStream.Seek((long) zip64EOCDLocator.OffsetOfZip64EOCD, SeekOrigin.Begin);
      Zip64EndOfCentralDirectoryRecord zip64EOCDRecord;
      if (!Zip64EndOfCentralDirectoryRecord.TryReadBlock(this._archiveReader, out zip64EOCDRecord))
        throw new InvalidDataException(SR.Zip64EOCDNotWhereExpected);
      this._numberOfThisDisk = zip64EOCDRecord.NumberOfThisDisk;
      if (zip64EOCDRecord.NumberOfEntriesTotal > (ulong) long.MaxValue)
        throw new InvalidDataException(SR.FieldTooBigNumEntries);
      if (zip64EOCDRecord.OffsetOfCentralDirectory > (ulong) long.MaxValue)
        throw new InvalidDataException(SR.FieldTooBigOffsetToCD);
      if ((long) zip64EOCDRecord.NumberOfEntriesTotal != (long) zip64EOCDRecord.NumberOfEntriesOnThisDisk)
        throw new InvalidDataException(SR.SplitSpanned);
      this._expectedNumberOfEntries = (long) zip64EOCDRecord.NumberOfEntriesTotal;
      this._centralDirectoryStart = (long) zip64EOCDRecord.OffsetOfCentralDirectory;
    }

    private void WriteFile()
    {
      if (this._mode == ZipArchiveMode.Update)
      {
        List<ZipArchiveEntry> zipArchiveEntryList = new List<ZipArchiveEntry>();
        foreach (ZipArchiveEntry entry in this._entries)
        {
          if (!entry.LoadLocalHeaderExtraFieldAndCompressedBytesIfNeeded())
            zipArchiveEntryList.Add(entry);
        }
        foreach (ZipArchiveEntry zipArchiveEntry in zipArchiveEntryList)
          zipArchiveEntry.Delete();
        this._archiveStream.Seek(0L, SeekOrigin.Begin);
        this._archiveStream.SetLength(0L);
      }
      foreach (ZipArchiveEntry entry in this._entries)
        entry.WriteAndFinishLocalEntry();
      long position = this._archiveStream.Position;
      foreach (ZipArchiveEntry entry in this._entries)
        entry.WriteCentralDirectoryFileHeader();
      long sizeOfCentralDirectory = this._archiveStream.Position - position;
      this.WriteArchiveEpilogue(position, sizeOfCentralDirectory);
    }

    private void WriteArchiveEpilogue(long startOfCentralDirectory, long sizeOfCentralDirectory)
    {
      if (startOfCentralDirectory >= (long) uint.MaxValue || sizeOfCentralDirectory >= (long) uint.MaxValue || this._entries.Count >= (int) ushort.MaxValue)
      {
        long position = this._archiveStream.Position;
        Zip64EndOfCentralDirectoryRecord.WriteBlock(this._archiveStream, (long) this._entries.Count, startOfCentralDirectory, sizeOfCentralDirectory);
        Zip64EndOfCentralDirectoryLocator.WriteBlock(this._archiveStream, position);
      }
      ZipEndOfCentralDirectoryBlock.WriteBlock(this._archiveStream, (long) this._entries.Count, startOfCentralDirectory, sizeOfCentralDirectory, this._archiveComment);
    }
  }
}
