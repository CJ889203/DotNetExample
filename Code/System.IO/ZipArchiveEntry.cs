// Decompiled with JetBrains decompiler
// Type: System.IO.Compression.ZipArchiveEntry
// Assembly: System.IO.Compression, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: A9780FE1-DF26-4575-BA54-75C62CD4F39E
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.Compression.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.Compression.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO.Compression
{
  /// <summary>Represents a compressed file within a zip archive.</summary>
  public class ZipArchiveEntry
  {

    #nullable disable
    private ZipArchive _archive;
    private readonly bool _originallyInArchive;
    private readonly int _diskNumberStart;
    private readonly ZipVersionMadeByPlatform _versionMadeByPlatform;
    private ZipVersionNeededValues _versionMadeBySpecification;
    internal ZipVersionNeededValues _versionToExtract;
    private ZipArchiveEntry.BitFlagValues _generalPurposeBitFlag;
    private ZipArchiveEntry.CompressionMethodValues _storedCompressionMethod;
    private DateTimeOffset _lastModified;
    private long _compressedSize;
    private long _uncompressedSize;
    private long _offsetOfLocalHeader;
    private long? _storedOffsetOfCompressedData;
    private uint _crc32;
    private byte[][] _compressedBytes;
    private MemoryStream _storedUncompressedData;
    private bool _currentlyOpenForWrite;
    private bool _everOpenedForWrite;
    private Stream _outstandingWriteStream;
    private uint _externalFileAttr;
    private string _storedEntryName;
    private byte[] _storedEntryNameBytes;
    private List<ZipGenericExtraField> _cdUnknownExtraFields;
    private List<ZipGenericExtraField> _lhUnknownExtraFields;
    private readonly byte[] _fileComment;
    private readonly CompressionLevel? _compressionLevel;
    private static readonly bool s_allowLargeZipArchiveEntriesInUpdateMode = IntPtr.Size > 4;

    internal ZipArchiveEntry(ZipArchive archive, ZipCentralDirectoryFileHeader cd)
    {
      this._archive = archive;
      this._originallyInArchive = true;
      this._diskNumberStart = cd.DiskNumberStart;
      this._versionMadeByPlatform = (ZipVersionMadeByPlatform) cd.VersionMadeByCompatibility;
      this._versionMadeBySpecification = (ZipVersionNeededValues) cd.VersionMadeBySpecification;
      this._versionToExtract = (ZipVersionNeededValues) cd.VersionNeededToExtract;
      this._generalPurposeBitFlag = (ZipArchiveEntry.BitFlagValues) cd.GeneralPurposeBitFlag;
      this.CompressionMethod = (ZipArchiveEntry.CompressionMethodValues) cd.CompressionMethod;
      this._lastModified = new DateTimeOffset(ZipHelper.DosTimeToDateTime(cd.LastModified));
      this._compressedSize = cd.CompressedSize;
      this._uncompressedSize = cd.UncompressedSize;
      this._externalFileAttr = cd.ExternalFileAttributes;
      this._offsetOfLocalHeader = cd.RelativeOffsetOfLocalHeader;
      this._storedOffsetOfCompressedData = new long?();
      this._crc32 = cd.Crc32;
      this._compressedBytes = (byte[][]) null;
      this._storedUncompressedData = (MemoryStream) null;
      this._currentlyOpenForWrite = false;
      this._everOpenedForWrite = false;
      this._outstandingWriteStream = (Stream) null;
      this.FullName = this.DecodeEntryName(cd.Filename);
      this._lhUnknownExtraFields = (List<ZipGenericExtraField>) null;
      this._cdUnknownExtraFields = cd.ExtraFields;
      this._fileComment = cd.FileComment;
      this._compressionLevel = new CompressionLevel?();
    }

    internal ZipArchiveEntry(
      ZipArchive archive,
      string entryName,
      CompressionLevel compressionLevel)
      : this(archive, entryName)
    {
      this._compressionLevel = new CompressionLevel?(compressionLevel);
      CompressionLevel? compressionLevel1 = this._compressionLevel;
      CompressionLevel compressionLevel2 = CompressionLevel.NoCompression;
      if (!(compressionLevel1.GetValueOrDefault() == compressionLevel2 & compressionLevel1.HasValue))
        return;
      this.CompressionMethod = ZipArchiveEntry.CompressionMethodValues.Stored;
    }

    internal ZipArchiveEntry(ZipArchive archive, string entryName)
    {
      this._archive = archive;
      this._originallyInArchive = false;
      this._diskNumberStart = 0;
      this._versionMadeByPlatform = ZipVersionMadeByPlatform.Windows;
      this._versionMadeBySpecification = ZipVersionNeededValues.Default;
      this._versionToExtract = ZipVersionNeededValues.Default;
      this._generalPurposeBitFlag = (ZipArchiveEntry.BitFlagValues) 0;
      this.CompressionMethod = ZipArchiveEntry.CompressionMethodValues.Deflate;
      this._lastModified = DateTimeOffset.Now;
      this._compressedSize = 0L;
      this._uncompressedSize = 0L;
      this._externalFileAttr = 0U;
      this._offsetOfLocalHeader = 0L;
      this._storedOffsetOfCompressedData = new long?();
      this._crc32 = 0U;
      this._compressedBytes = (byte[][]) null;
      this._storedUncompressedData = (MemoryStream) null;
      this._currentlyOpenForWrite = false;
      this._everOpenedForWrite = false;
      this._outstandingWriteStream = (Stream) null;
      this.FullName = entryName;
      this._cdUnknownExtraFields = (List<ZipGenericExtraField>) null;
      this._lhUnknownExtraFields = (List<ZipGenericExtraField>) null;
      this._fileComment = (byte[]) null;
      this._compressionLevel = new CompressionLevel?();
      if (this._storedEntryNameBytes.Length > (int) ushort.MaxValue)
        throw new ArgumentException(SR.EntryNamesTooLong);
      if (this._archive.Mode != ZipArchiveMode.Create)
        return;
      this._archive.AcquireArchiveStream(this);
    }


    #nullable enable
    /// <summary>Gets the zip archive that the entry belongs to.</summary>
    /// <returns>The zip archive that the entry belongs to, or <see langword="null" /> if the entry has been deleted.</returns>
    public ZipArchive Archive => this._archive;

    /// <summary>The 32-bit Cyclic Redundant Check.</summary>
    /// <returns>An unsigned integer (4 bytes) representing the CRC-32 field.</returns>
    [CLSCompliant(false)]
    public uint Crc32 => this._crc32;

    /// <summary>Gets the compressed size of the entry in the zip archive.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the property is not available because the entry has been modified.</exception>
    /// <returns>The compressed size of the entry in the zip archive.</returns>
    public long CompressedLength
    {
      get
      {
        if (this._everOpenedForWrite)
          throw new InvalidOperationException(SR.LengthAfterWrite);
        return this._compressedSize;
      }
    }

    /// <summary>OS and application specific file attributes.</summary>
    /// <returns>The external attributes written by the application when this entry was written. It is both host OS and application dependent.</returns>
    public int ExternalAttributes
    {
      get => (int) this._externalFileAttr;
      set
      {
        this.ThrowIfInvalidArchive();
        this._externalFileAttr = (uint) value;
      }
    }

    /// <summary>Gets the relative path of the entry in the zip archive.</summary>
    /// <returns>The relative path of the entry in the zip archive.</returns>
    public string FullName
    {
      get => this._storedEntryName;
      [MemberNotNull("_storedEntryNameBytes"), MemberNotNull("_storedEntryName")] private set
      {
        bool isUTF8;
        this._storedEntryNameBytes = value != null ? this.EncodeEntryName(value, out isUTF8) : throw new ArgumentNullException(nameof (FullName));
        this._storedEntryName = value;
        if (isUTF8)
          this._generalPurposeBitFlag |= ZipArchiveEntry.BitFlagValues.UnicodeFileName;
        else
          this._generalPurposeBitFlag &= ~ZipArchiveEntry.BitFlagValues.UnicodeFileName;
        if (!(ZipArchiveEntry.ParseFileName(value, this._versionMadeByPlatform) == ""))
          return;
        this.VersionToExtractAtLeast(ZipVersionNeededValues.ExplicitDirectory);
      }
    }

    /// <summary>Gets or sets the last time the entry in the zip archive was changed.</summary>
    /// <exception cref="T:System.NotSupportedException">The attempt to set this property failed, because the zip archive for the entry is in <see cref="F:System.IO.Compression.ZipArchiveMode.Read" /> mode.</exception>
    /// <exception cref="T:System.IO.IOException">The archive mode is set to <see cref="F:System.IO.Compression.ZipArchiveMode.Create" />.
    /// 
    /// -or-
    /// 
    ///  The archive mode is set to <see cref="F:System.IO.Compression.ZipArchiveMode.Update" /> and the entry has been opened.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">An attempt was made to set this property to a value that is either earlier than 1980 January 1 0:00:00 (midnight) or later than 2107 December 31 23:59:58 (one second before midnight).</exception>
    /// <returns>The last time the entry in the zip archive was changed.</returns>
    public DateTimeOffset LastWriteTime
    {
      get => this._lastModified;
      set
      {
        this.ThrowIfInvalidArchive();
        if (this._archive.Mode == ZipArchiveMode.Read)
          throw new NotSupportedException(SR.ReadOnlyArchive);
        if (this._archive.Mode == ZipArchiveMode.Create && this._everOpenedForWrite)
          throw new IOException(SR.FrozenAfterWrite);
        if (value.DateTime.Year < 1980 || value.DateTime.Year > 2107)
          throw new ArgumentOutOfRangeException(nameof (value), SR.DateTimeOutOfRange);
        this._lastModified = value;
      }
    }

    /// <summary>Gets the uncompressed size of the entry in the zip archive.</summary>
    /// <exception cref="T:System.InvalidOperationException">The value of the property is not available because the entry has been modified.</exception>
    /// <returns>The uncompressed size of the entry in the zip archive.</returns>
    public long Length
    {
      get
      {
        if (this._everOpenedForWrite)
          throw new InvalidOperationException(SR.LengthAfterWrite);
        return this._uncompressedSize;
      }
    }

    /// <summary>Gets the file name of the entry in the zip archive.</summary>
    /// <returns>The file name of the entry in the zip archive.</returns>
    public string Name => ZipArchiveEntry.ParseFileName(this.FullName, this._versionMadeByPlatform);

    /// <summary>Deletes the entry from the zip archive.</summary>
    /// <exception cref="T:System.IO.IOException">The entry is already open for reading or writing.</exception>
    /// <exception cref="T:System.NotSupportedException">The zip archive for this entry was opened in a mode other than <see cref="F:System.IO.Compression.ZipArchiveMode.Update" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive for this entry has been disposed.</exception>
    public void Delete()
    {
      if (this._archive == null)
        return;
      if (this._currentlyOpenForWrite)
        throw new IOException(SR.DeleteOpenEntry);
      if (this._archive.Mode != ZipArchiveMode.Update)
        throw new NotSupportedException(SR.DeleteOnlyInUpdate);
      this._archive.ThrowIfDisposed();
      this._archive.RemoveEntry(this);
      this._archive = (ZipArchive) null;
      this.UnloadStreams();
    }

    /// <summary>Opens the entry from the zip archive.</summary>
    /// <exception cref="T:System.IO.IOException">The entry is already currently open for writing.
    /// 
    /// -or-
    /// 
    /// The entry has been deleted from the archive.
    /// 
    /// -or-
    /// 
    /// The archive for this entry was opened with the <see cref="F:System.IO.Compression.ZipArchiveMode.Create" /> mode, and this entry has already been written to.</exception>
    /// <exception cref="T:System.IO.InvalidDataException">The entry is either missing from the archive or is corrupt and cannot be read.
    /// 
    /// -or-
    /// 
    /// The entry has been compressed by using a compression method that is not supported.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The zip archive for this entry has been disposed.</exception>
    /// <returns>The stream that represents the contents of the entry.</returns>
    public Stream Open()
    {
      this.ThrowIfInvalidArchive();
      switch (this._archive.Mode)
      {
        case ZipArchiveMode.Read:
          return this.OpenInReadMode(true);
        case ZipArchiveMode.Create:
          return this.OpenInWriteMode();
        default:
          return this.OpenInUpdateMode();
      }
    }

    /// <summary>Retrieves the relative path of the entry in the zip archive.</summary>
    /// <returns>The relative path of the entry, which is the value stored in the <see cref="P:System.IO.Compression.ZipArchiveEntry.FullName" /> property.</returns>
    public override string ToString() => this.FullName;

    internal bool EverOpenedForWrite => this._everOpenedForWrite;

    private long OffsetOfCompressedData
    {
      get
      {
        if (!this._storedOffsetOfCompressedData.HasValue)
        {
          this._archive.ArchiveStream.Seek(this._offsetOfLocalHeader, SeekOrigin.Begin);
          if (!ZipLocalFileHeader.TrySkipBlock(this._archive.ArchiveReader))
            throw new InvalidDataException(SR.LocalFileHeaderCorrupt);
          this._storedOffsetOfCompressedData = new long?(this._archive.ArchiveStream.Position);
        }
        return this._storedOffsetOfCompressedData.Value;
      }
    }

    private MemoryStream UncompressedData
    {
      get
      {
        if (this._storedUncompressedData == null)
        {
          this._storedUncompressedData = new MemoryStream((int) this._uncompressedSize);
          if (this._originallyInArchive)
          {
            using (Stream stream = this.OpenInReadMode(false))
            {
              try
              {
                stream.CopyTo((Stream) this._storedUncompressedData);
              }
              catch (InvalidDataException ex)
              {
                this._storedUncompressedData.Dispose();
                this._storedUncompressedData = (MemoryStream) null;
                this._currentlyOpenForWrite = false;
                this._everOpenedForWrite = false;
                throw;
              }
            }
          }
          if (this.CompressionMethod != ZipArchiveEntry.CompressionMethodValues.Stored)
            this.CompressionMethod = ZipArchiveEntry.CompressionMethodValues.Deflate;
        }
        return this._storedUncompressedData;
      }
    }

    private ZipArchiveEntry.CompressionMethodValues CompressionMethod
    {
      get => this._storedCompressionMethod;
      set
      {
        switch (value)
        {
          case ZipArchiveEntry.CompressionMethodValues.Deflate:
            this.VersionToExtractAtLeast(ZipVersionNeededValues.ExplicitDirectory);
            break;
          case ZipArchiveEntry.CompressionMethodValues.Deflate64:
            this.VersionToExtractAtLeast(ZipVersionNeededValues.Deflate64);
            break;
        }
        this._storedCompressionMethod = value;
      }
    }


    #nullable disable
    private string DecodeEntryName(byte[] entryNameBytes) => ((this._generalPurposeBitFlag & ZipArchiveEntry.BitFlagValues.UnicodeFileName) != (ZipArchiveEntry.BitFlagValues) 0 ? Encoding.UTF8 : (this._archive == null ? Encoding.UTF8 : this._archive.EntryNameEncoding ?? Encoding.UTF8)).GetString(entryNameBytes);

    private byte[] EncodeEntryName(string entryName, out bool isUTF8)
    {
      Encoding encoding = this._archive == null || this._archive.EntryNameEncoding == null ? (ZipHelper.RequiresUnicode(entryName) ? Encoding.UTF8 : Encoding.ASCII) : this._archive.EntryNameEncoding;
      isUTF8 = encoding.Equals((object) Encoding.UTF8);
      return encoding.GetBytes(entryName);
    }

    internal void WriteAndFinishLocalEntry()
    {
      this.CloseStreams();
      this.WriteLocalFileHeaderAndDataIfNeeded();
      this.UnloadStreams();
    }

    internal void WriteCentralDirectoryFileHeader()
    {
      BinaryWriter binaryWriter = new BinaryWriter(this._archive.ArchiveStream);
      Zip64ExtraField zip64ExtraField = new Zip64ExtraField();
      bool flag = false;
      uint num1;
      uint num2;
      if (this.SizesTooLarge())
      {
        flag = true;
        num1 = uint.MaxValue;
        num2 = uint.MaxValue;
        zip64ExtraField.CompressedSize = new long?(this._compressedSize);
        zip64ExtraField.UncompressedSize = new long?(this._uncompressedSize);
      }
      else
      {
        num1 = (uint) this._compressedSize;
        num2 = (uint) this._uncompressedSize;
      }
      uint num3;
      if (this._offsetOfLocalHeader > (long) uint.MaxValue)
      {
        flag = true;
        num3 = uint.MaxValue;
        zip64ExtraField.LocalHeaderOffset = new long?(this._offsetOfLocalHeader);
      }
      else
        num3 = (uint) this._offsetOfLocalHeader;
      if (flag)
        this.VersionToExtractAtLeast(ZipVersionNeededValues.Zip64);
      int num4 = (flag ? (int) zip64ExtraField.TotalSize : 0) + (this._cdUnknownExtraFields != null ? ZipGenericExtraField.TotalSize(this._cdUnknownExtraFields) : 0);
      ushort num5;
      if (num4 > (int) ushort.MaxValue)
      {
        num5 = flag ? zip64ExtraField.TotalSize : (ushort) 0;
        this._cdUnknownExtraFields = (List<ZipGenericExtraField>) null;
      }
      else
        num5 = (ushort) num4;
      binaryWriter.Write(33639248U);
      binaryWriter.Write((byte) this._versionMadeBySpecification);
      binaryWriter.Write((byte) 0);
      binaryWriter.Write((ushort) this._versionToExtract);
      binaryWriter.Write((ushort) this._generalPurposeBitFlag);
      binaryWriter.Write((ushort) this.CompressionMethod);
      binaryWriter.Write(ZipHelper.DateTimeToDosTime(this._lastModified.DateTime));
      binaryWriter.Write(this._crc32);
      binaryWriter.Write(num1);
      binaryWriter.Write(num2);
      binaryWriter.Write((ushort) this._storedEntryNameBytes.Length);
      binaryWriter.Write(num5);
      binaryWriter.Write(this._fileComment != null ? (ushort) this._fileComment.Length : (ushort) 0);
      binaryWriter.Write((ushort) 0);
      binaryWriter.Write((ushort) 0);
      binaryWriter.Write(this._externalFileAttr);
      binaryWriter.Write(num3);
      binaryWriter.Write(this._storedEntryNameBytes);
      if (flag)
        zip64ExtraField.WriteBlock(this._archive.ArchiveStream);
      if (this._cdUnknownExtraFields != null)
        ZipGenericExtraField.WriteAllBlocks(this._cdUnknownExtraFields, this._archive.ArchiveStream);
      if (this._fileComment == null)
        return;
      binaryWriter.Write(this._fileComment);
    }

    internal bool LoadLocalHeaderExtraFieldAndCompressedBytesIfNeeded()
    {
      if (this._originallyInArchive)
      {
        this._archive.ArchiveStream.Seek(this._offsetOfLocalHeader, SeekOrigin.Begin);
        this._lhUnknownExtraFields = ZipLocalFileHeader.GetExtraFields(this._archive.ArchiveReader);
      }
      if (!this._everOpenedForWrite && this._originallyInArchive)
      {
        int maxLength = Array.MaxLength;
        this._compressedBytes = new byte[this._compressedSize / (long) maxLength + 1L][];
        for (int index = 0; index < this._compressedBytes.Length - 1; ++index)
          this._compressedBytes[index] = new byte[maxLength];
        this._compressedBytes[this._compressedBytes.Length - 1] = new byte[this._compressedSize % (long) maxLength];
        this._archive.ArchiveStream.Seek(this.OffsetOfCompressedData, SeekOrigin.Begin);
        for (int index = 0; index < this._compressedBytes.Length - 1; ++index)
          ZipHelper.ReadBytes(this._archive.ArchiveStream, this._compressedBytes[index], maxLength);
        ZipHelper.ReadBytes(this._archive.ArchiveStream, this._compressedBytes[this._compressedBytes.Length - 1], (int) (this._compressedSize % (long) maxLength));
      }
      return true;
    }

    internal void ThrowIfNotOpenable(bool needToUncompress, bool needToLoadIntoMemory)
    {
      string message;
      if (!this.IsOpenable(needToUncompress, needToLoadIntoMemory, out message))
        throw new InvalidDataException(message);
    }

    private CheckSumAndSizeWriteStream GetDataCompressor(
      Stream backingStream,
      bool leaveBackingStreamOpen,
      EventHandler onClose)
    {
      bool flag = true;
      Stream baseStream;
      switch (this.CompressionMethod)
      {
        case ZipArchiveEntry.CompressionMethodValues.Stored:
          baseStream = backingStream;
          flag = false;
          break;
        default:
          baseStream = (Stream) new DeflateStream(backingStream, this._compressionLevel.GetValueOrDefault(), leaveBackingStreamOpen);
          break;
      }
      bool leaveOpenOnClose = leaveBackingStreamOpen && !flag;
      return new CheckSumAndSizeWriteStream(baseStream, backingStream, leaveOpenOnClose, this, onClose, (Action<long, long, uint, Stream, ZipArchiveEntry, EventHandler>) ((initialPosition, currentPosition, checkSum, backing, thisRef, closeHandler) =>
      {
        thisRef._crc32 = checkSum;
        thisRef._uncompressedSize = currentPosition;
        thisRef._compressedSize = backing.Position - initialPosition;
        if (closeHandler == null)
          return;
        closeHandler((object) thisRef, EventArgs.Empty);
      }));
    }

    private Stream GetDataDecompressor(Stream compressedStreamToRead)
    {
      Stream dataDecompressor;
      switch (this.CompressionMethod)
      {
        case ZipArchiveEntry.CompressionMethodValues.Deflate:
          dataDecompressor = (Stream) new DeflateStream(compressedStreamToRead, CompressionMode.Decompress, this._uncompressedSize);
          break;
        case ZipArchiveEntry.CompressionMethodValues.Deflate64:
          dataDecompressor = (Stream) new DeflateManagedStream(compressedStreamToRead, ZipArchiveEntry.CompressionMethodValues.Deflate64, this._uncompressedSize);
          break;
        default:
          dataDecompressor = compressedStreamToRead;
          break;
      }
      return dataDecompressor;
    }

    private Stream OpenInReadMode(bool checkOpenable)
    {
      if (checkOpenable)
        this.ThrowIfNotOpenable(true, false);
      return this.GetDataDecompressor((Stream) new SubReadStream(this._archive.ArchiveStream, this.OffsetOfCompressedData, this._compressedSize));
    }

    private Stream OpenInWriteMode()
    {
      this._everOpenedForWrite = !this._everOpenedForWrite ? true : throw new IOException(SR.CreateModeWriteOnceAndOneEntryAtATime);
      this._outstandingWriteStream = (Stream) new ZipArchiveEntry.DirectToArchiveWriterStream(this.GetDataCompressor(this._archive.ArchiveStream, true, (EventHandler) ((o, e) =>
      {
        ZipArchiveEntry entry = (ZipArchiveEntry) o;
        entry._archive.ReleaseArchiveStream(entry);
        entry._outstandingWriteStream = (Stream) null;
      })), this);
      return (Stream) new WrappedStream(this._outstandingWriteStream, true);
    }

    private Stream OpenInUpdateMode()
    {
      if (this._currentlyOpenForWrite)
        throw new IOException(SR.UpdateModeOneStream);
      this.ThrowIfNotOpenable(true, true);
      this._everOpenedForWrite = true;
      this._currentlyOpenForWrite = true;
      this.UncompressedData.Seek(0L, SeekOrigin.Begin);
      return (Stream) new WrappedStream((Stream) this.UncompressedData, this, (Action<ZipArchiveEntry>) (thisRef => thisRef._currentlyOpenForWrite = false));
    }

    private bool IsOpenable(bool needToUncompress, bool needToLoadIntoMemory, out string message)
    {
      message = (string) null;
      if (this._originallyInArchive)
      {
        if (needToUncompress && this.CompressionMethod != ZipArchiveEntry.CompressionMethodValues.Stored && this.CompressionMethod != ZipArchiveEntry.CompressionMethodValues.Deflate && this.CompressionMethod != ZipArchiveEntry.CompressionMethodValues.Deflate64)
        {
          switch (this.CompressionMethod)
          {
            case ZipArchiveEntry.CompressionMethodValues.BZip2:
            case ZipArchiveEntry.CompressionMethodValues.LZMA:
              message = SR.Format(SR.UnsupportedCompressionMethod, (object) this.CompressionMethod.ToString());
              break;
            default:
              message = SR.UnsupportedCompression;
              break;
          }
          return false;
        }
        if ((long) this._diskNumberStart != (long) this._archive.NumberOfThisDisk)
        {
          message = SR.SplitSpanned;
          return false;
        }
        if (this._offsetOfLocalHeader > this._archive.ArchiveStream.Length)
        {
          message = SR.LocalFileHeaderCorrupt;
          return false;
        }
        this._archive.ArchiveStream.Seek(this._offsetOfLocalHeader, SeekOrigin.Begin);
        if (!ZipLocalFileHeader.TrySkipBlock(this._archive.ArchiveReader))
        {
          message = SR.LocalFileHeaderCorrupt;
          return false;
        }
        if (this.OffsetOfCompressedData + this._compressedSize > this._archive.ArchiveStream.Length)
        {
          message = SR.LocalFileHeaderCorrupt;
          return false;
        }
        if (needToLoadIntoMemory && this._compressedSize > (long) int.MaxValue && !ZipArchiveEntry.s_allowLargeZipArchiveEntriesInUpdateMode)
        {
          message = SR.EntryTooLarge;
          return false;
        }
      }
      return true;
    }

    private bool SizesTooLarge() => this._compressedSize > (long) uint.MaxValue || this._uncompressedSize > (long) uint.MaxValue;

    private bool WriteLocalFileHeader(bool isEmptyFile)
    {
      BinaryWriter binaryWriter = new BinaryWriter(this._archive.ArchiveStream);
      Zip64ExtraField zip64ExtraField = new Zip64ExtraField();
      bool flag = false;
      uint num1;
      uint num2;
      if (isEmptyFile)
      {
        this.CompressionMethod = ZipArchiveEntry.CompressionMethodValues.Stored;
        num1 = 0U;
        num2 = 0U;
      }
      else if (this._archive.Mode == ZipArchiveMode.Create && !this._archive.ArchiveStream.CanSeek && !isEmptyFile)
      {
        this._generalPurposeBitFlag |= ZipArchiveEntry.BitFlagValues.DataDescriptor;
        flag = false;
        num1 = 0U;
        num2 = 0U;
      }
      else
      {
        this._generalPurposeBitFlag &= ~ZipArchiveEntry.BitFlagValues.DataDescriptor;
        if (this.SizesTooLarge())
        {
          flag = true;
          num1 = uint.MaxValue;
          num2 = uint.MaxValue;
          zip64ExtraField.CompressedSize = new long?(this._compressedSize);
          zip64ExtraField.UncompressedSize = new long?(this._uncompressedSize);
          this.VersionToExtractAtLeast(ZipVersionNeededValues.Zip64);
        }
        else
        {
          flag = false;
          num1 = (uint) this._compressedSize;
          num2 = (uint) this._uncompressedSize;
        }
      }
      this._offsetOfLocalHeader = binaryWriter.BaseStream.Position;
      int num3 = (flag ? (int) zip64ExtraField.TotalSize : 0) + (this._lhUnknownExtraFields != null ? ZipGenericExtraField.TotalSize(this._lhUnknownExtraFields) : 0);
      ushort num4;
      if (num3 > (int) ushort.MaxValue)
      {
        num4 = flag ? zip64ExtraField.TotalSize : (ushort) 0;
        this._lhUnknownExtraFields = (List<ZipGenericExtraField>) null;
      }
      else
        num4 = (ushort) num3;
      binaryWriter.Write(67324752U);
      binaryWriter.Write((ushort) this._versionToExtract);
      binaryWriter.Write((ushort) this._generalPurposeBitFlag);
      binaryWriter.Write((ushort) this.CompressionMethod);
      binaryWriter.Write(ZipHelper.DateTimeToDosTime(this._lastModified.DateTime));
      binaryWriter.Write(this._crc32);
      binaryWriter.Write(num1);
      binaryWriter.Write(num2);
      binaryWriter.Write((ushort) this._storedEntryNameBytes.Length);
      binaryWriter.Write(num4);
      binaryWriter.Write(this._storedEntryNameBytes);
      if (flag)
        zip64ExtraField.WriteBlock(this._archive.ArchiveStream);
      if (this._lhUnknownExtraFields != null)
        ZipGenericExtraField.WriteAllBlocks(this._lhUnknownExtraFields, this._archive.ArchiveStream);
      return flag;
    }

    private void WriteLocalFileHeaderAndDataIfNeeded()
    {
      if (this._storedUncompressedData != null || this._compressedBytes != null)
      {
        if (this._storedUncompressedData != null)
        {
          this._uncompressedSize = this._storedUncompressedData.Length;
          using (Stream destination = (Stream) new ZipArchiveEntry.DirectToArchiveWriterStream(this.GetDataCompressor(this._archive.ArchiveStream, true, (EventHandler) null), this))
          {
            this._storedUncompressedData.Seek(0L, SeekOrigin.Begin);
            this._storedUncompressedData.CopyTo(destination);
            this._storedUncompressedData.Dispose();
            this._storedUncompressedData = (MemoryStream) null;
          }
        }
        else
        {
          if (this._uncompressedSize == 0L)
            this._compressedSize = 0L;
          this.WriteLocalFileHeader(this._uncompressedSize == 0L);
          if (this._uncompressedSize == 0L)
            return;
          foreach (byte[] compressedByte in this._compressedBytes)
            this._archive.ArchiveStream.Write(compressedByte, 0, compressedByte.Length);
        }
      }
      else
      {
        if (this._archive.Mode != ZipArchiveMode.Update && this._everOpenedForWrite)
          return;
        this._everOpenedForWrite = true;
        this.WriteLocalFileHeader(true);
      }
    }

    private void WriteCrcAndSizesInLocalHeader(bool zip64HeaderUsed)
    {
      long position = this._archive.ArchiveStream.Position;
      BinaryWriter binaryWriter = new BinaryWriter(this._archive.ArchiveStream);
      bool flag1 = this.SizesTooLarge();
      bool flag2 = flag1 && !zip64HeaderUsed;
      uint num1 = flag1 ? uint.MaxValue : (uint) this._compressedSize;
      uint num2 = flag1 ? uint.MaxValue : (uint) this._uncompressedSize;
      if (flag2)
      {
        this.VersionToExtractAtLeast(ZipVersionNeededValues.Zip64);
        this._generalPurposeBitFlag |= ZipArchiveEntry.BitFlagValues.DataDescriptor;
        this._archive.ArchiveStream.Seek(this._offsetOfLocalHeader + 4L, SeekOrigin.Begin);
        binaryWriter.Write((ushort) this._versionToExtract);
        binaryWriter.Write((ushort) this._generalPurposeBitFlag);
      }
      this._archive.ArchiveStream.Seek(this._offsetOfLocalHeader + 14L, SeekOrigin.Begin);
      if (!flag2)
      {
        binaryWriter.Write(this._crc32);
        binaryWriter.Write(num1);
        binaryWriter.Write(num2);
      }
      else
      {
        binaryWriter.Write(0U);
        binaryWriter.Write(0U);
        binaryWriter.Write(0U);
      }
      if (zip64HeaderUsed)
      {
        this._archive.ArchiveStream.Seek(this._offsetOfLocalHeader + 30L + (long) this._storedEntryNameBytes.Length + 4L, SeekOrigin.Begin);
        binaryWriter.Write(this._uncompressedSize);
        binaryWriter.Write(this._compressedSize);
      }
      this._archive.ArchiveStream.Seek(position, SeekOrigin.Begin);
      if (!flag2)
        return;
      binaryWriter.Write(this._crc32);
      binaryWriter.Write(this._compressedSize);
      binaryWriter.Write(this._uncompressedSize);
    }

    private void WriteDataDescriptor()
    {
      BinaryWriter binaryWriter = new BinaryWriter(this._archive.ArchiveStream);
      binaryWriter.Write(134695760U);
      binaryWriter.Write(this._crc32);
      if (this.SizesTooLarge())
      {
        binaryWriter.Write(this._compressedSize);
        binaryWriter.Write(this._uncompressedSize);
      }
      else
      {
        binaryWriter.Write((uint) this._compressedSize);
        binaryWriter.Write((uint) this._uncompressedSize);
      }
    }

    private void UnloadStreams()
    {
      if (this._storedUncompressedData != null)
        this._storedUncompressedData.Dispose();
      this._compressedBytes = (byte[][]) null;
      this._outstandingWriteStream = (Stream) null;
    }

    private void CloseStreams()
    {
      if (this._outstandingWriteStream == null)
        return;
      this._outstandingWriteStream.Dispose();
    }

    private void VersionToExtractAtLeast(ZipVersionNeededValues value)
    {
      if (this._versionToExtract < value)
        this._versionToExtract = value;
      if (this._versionMadeBySpecification >= value)
        return;
      this._versionMadeBySpecification = value;
    }

    private void ThrowIfInvalidArchive()
    {
      if (this._archive == null)
        throw new InvalidOperationException(SR.DeletedEntry);
      this._archive.ThrowIfDisposed();
    }

    private static string GetFileName_Windows(string path)
    {
      int length = path.Length;
      while (--length >= 0)
      {
        switch (path[length])
        {
          case '/':
          case ':':
          case '\\':
            return path.Substring(length + 1);
          default:
            continue;
        }
      }
      return path;
    }

    private static string GetFileName_Unix(string path)
    {
      int length = path.Length;
      while (--length >= 0)
      {
        if (path[length] == '/')
          return path.Substring(length + 1);
      }
      return path;
    }

    internal static string ParseFileName(string path, ZipVersionMadeByPlatform madeByPlatform) => madeByPlatform != ZipVersionMadeByPlatform.Unix ? ZipArchiveEntry.GetFileName_Windows(path) : ZipArchiveEntry.GetFileName_Unix(path);

    private sealed class DirectToArchiveWriterStream : Stream
    {
      private long _position;
      private readonly CheckSumAndSizeWriteStream _crcSizeStream;
      private bool _everWritten;
      private bool _isDisposed;
      private readonly ZipArchiveEntry _entry;
      private bool _usedZip64inLH;
      private bool _canWrite;

      public DirectToArchiveWriterStream(
        CheckSumAndSizeWriteStream crcSizeStream,
        ZipArchiveEntry entry)
      {
        this._position = 0L;
        this._crcSizeStream = crcSizeStream;
        this._everWritten = false;
        this._isDisposed = false;
        this._entry = entry;
        this._usedZip64inLH = false;
        this._canWrite = true;
      }

      public override long Length
      {
        get
        {
          this.ThrowIfDisposed();
          throw new NotSupportedException(SR.SeekingNotSupported);
        }
      }

      public override long Position
      {
        get
        {
          this.ThrowIfDisposed();
          return this._position;
        }
        set
        {
          this.ThrowIfDisposed();
          throw new NotSupportedException(SR.SeekingNotSupported);
        }
      }

      public override bool CanRead => false;

      public override bool CanSeek => false;

      public override bool CanWrite => this._canWrite;

      private void ThrowIfDisposed()
      {
        if (this._isDisposed)
          throw new ObjectDisposedException(this.GetType().ToString(), SR.HiddenStreamName);
      }

      public override int Read(byte[] buffer, int offset, int count)
      {
        this.ThrowIfDisposed();
        throw new NotSupportedException(SR.ReadingNotSupported);
      }

      public override long Seek(long offset, SeekOrigin origin)
      {
        this.ThrowIfDisposed();
        throw new NotSupportedException(SR.SeekingNotSupported);
      }

      public override void SetLength(long value)
      {
        this.ThrowIfDisposed();
        throw new NotSupportedException(SR.SetLengthRequiresSeekingAndWriting);
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
        Stream.ValidateBufferArguments(buffer, offset, count);
        this.ThrowIfDisposed();
        if (count == 0)
          return;
        if (!this._everWritten)
        {
          this._everWritten = true;
          this._usedZip64inLH = this._entry.WriteLocalFileHeader(false);
        }
        this._crcSizeStream.Write(buffer, offset, count);
        this._position += (long) count;
      }

      public override void Write(ReadOnlySpan<byte> source)
      {
        this.ThrowIfDisposed();
        if (source.Length == 0)
          return;
        if (!this._everWritten)
        {
          this._everWritten = true;
          this._usedZip64inLH = this._entry.WriteLocalFileHeader(false);
        }
        this._crcSizeStream.Write(source);
        this._position += (long) source.Length;
      }

      public override void WriteByte(byte value) => this.Write(MemoryMarshal.CreateReadOnlySpan<byte>(ref value, 1));

      public override Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
      {
        Stream.ValidateBufferArguments(buffer, offset, count);
        return this.WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
      }

      public override ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default (CancellationToken))
      {
        this.ThrowIfDisposed();
        return buffer.IsEmpty ? new ValueTask() : Core(buffer, cancellationToken);

        async ValueTask Core(
          ReadOnlyMemory<byte> buffer,
          CancellationToken cancellationToken)
        {
          if (!this._everWritten)
          {
            this._everWritten = true;
            this._usedZip64inLH = this._entry.WriteLocalFileHeader(false);
          }
          await this._crcSizeStream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
          this._position += (long) buffer.Length;
        }
      }

      public override void Flush()
      {
        this.ThrowIfDisposed();
        this._crcSizeStream.Flush();
      }

      public override Task FlushAsync(CancellationToken cancellationToken)
      {
        this.ThrowIfDisposed();
        return this._crcSizeStream.FlushAsync(cancellationToken);
      }

      protected override void Dispose(bool disposing)
      {
        if (disposing && !this._isDisposed)
        {
          this._crcSizeStream.Dispose();
          if (!this._everWritten)
            this._entry.WriteLocalFileHeader(true);
          else if (this._entry._archive.ArchiveStream.CanSeek)
            this._entry.WriteCrcAndSizesInLocalHeader(this._usedZip64inLH);
          else
            this._entry.WriteDataDescriptor();
          this._canWrite = false;
          this._isDisposed = true;
        }
        base.Dispose(disposing);
      }
    }

    [Flags]
    internal enum BitFlagValues : ushort
    {
      DataDescriptor = 8,
      UnicodeFileName = 2048, // 0x0800
    }

    internal enum CompressionMethodValues : ushort
    {
      Stored = 0,
      Deflate = 8,
      Deflate64 = 9,
      BZip2 = 12, // 0x000C
      LZMA = 14, // 0x000E
    }
  }
}
