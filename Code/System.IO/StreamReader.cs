// Decompiled with JetBrains decompiler
// Type: System.IO.StreamReader
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Implements a <see cref="T:System.IO.TextReader" /> that reads characters from a byte stream in a particular encoding.</summary>
  public class StreamReader : TextReader
  {
    /// <summary>A <see cref="T:System.IO.StreamReader" /> object around an empty stream.</summary>
    public static readonly StreamReader Null = (StreamReader) new StreamReader.NullStreamReader();

    #nullable disable
    private readonly Stream _stream;
    private Encoding _encoding;
    private Decoder _decoder;
    private readonly byte[] _byteBuffer;
    private char[] _charBuffer;
    private int _charPos;
    private int _charLen;
    private int _byteLen;
    private int _bytePos;
    private int _maxCharsPerBuffer;
    private bool _disposed;
    private bool _detectEncoding;
    private bool _checkPreamble;
    private bool _isBlocked;
    private readonly bool _closable;
    private Task _asyncReadTask = Task.CompletedTask;

    private void CheckAsyncTaskInProgress()
    {
      if (this._asyncReadTask.IsCompleted)
        return;
      StreamReader.ThrowAsyncIOInProgress();
    }

    [DoesNotReturn]
    private static void ThrowAsyncIOInProgress() => throw new InvalidOperationException(SR.InvalidOperation_AsyncIOInProgress);

    private StreamReader()
    {
      this._stream = Stream.Null;
      this._closable = true;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream.</summary>
    /// <param name="stream">The stream to be read.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    public StreamReader(Stream stream)
      : this(stream, true)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream, with the specified byte order mark detection option.</summary>
    /// <param name="stream">The stream to be read.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> is <see langword="null" />.</exception>
    public StreamReader(Stream stream, bool detectEncodingFromByteOrderMarks)
      : this(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks, 1024, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream, with the specified character encoding.</summary>
    /// <param name="stream">The stream to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    public StreamReader(Stream stream, Encoding encoding)
      : this(stream, encoding, bufferSize: 1024)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream, with the specified character encoding and byte order mark detection option.</summary>
    /// <param name="stream">The stream to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="stream" /> does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    public StreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
      : this(stream, encoding, detectEncodingFromByteOrderMarks, 1024, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream, with the specified character encoding, byte order mark detection option, and buffer size.</summary>
    /// <param name="stream">The stream to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <param name="bufferSize">The minimum buffer size.</param>
    /// <exception cref="T:System.ArgumentException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="bufferSize" /> is less than or equal to zero.</exception>
    public StreamReader(
      Stream stream,
      Encoding encoding,
      bool detectEncodingFromByteOrderMarks,
      int bufferSize)
      : this(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified stream based on the specified character encoding, byte order mark detection option, and buffer size, and optionally leaves the stream open.</summary>
    /// <param name="stream">The stream to read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">
    /// <see langword="true" /> to look for byte order marks at the beginning of the file; otherwise, <see langword="false" />.</param>
    /// <param name="bufferSize">The minimum buffer size.</param>
    /// <param name="leaveOpen">
    /// <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.StreamReader" /> object is disposed; otherwise, <see langword="false" />.</param>
    public StreamReader(
      Stream stream,
      Encoding? encoding = null,
      bool detectEncodingFromByteOrderMarks = true,
      int bufferSize = -1,
      bool leaveOpen = false)
    {
      if (stream == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.stream);
      if (encoding == null)
        encoding = Encoding.UTF8;
      if (!stream.CanRead)
        throw new ArgumentException(SR.Argument_StreamNotReadable);
      if (bufferSize == -1)
        bufferSize = 1024;
      else if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), SR.ArgumentOutOfRange_NeedPosNum);
      this._stream = stream;
      this._encoding = encoding;
      this._decoder = encoding.GetDecoder();
      if (bufferSize < 128)
        bufferSize = 128;
      this._byteBuffer = new byte[bufferSize];
      this._maxCharsPerBuffer = encoding.GetMaxCharCount(bufferSize);
      this._charBuffer = new char[this._maxCharsPerBuffer];
      this._detectEncoding = detectEncodingFromByteOrderMarks;
      this._checkPreamble = encoding.Preamble.Length > 0;
      this._closable = !leaveOpen;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    public StreamReader(string path)
      : this(path, true)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified byte order mark detection option.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    public StreamReader(string path, bool detectEncodingFromByteOrderMarks)
      : this(path, Encoding.UTF8, detectEncodingFromByteOrderMarks, 1024)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified character encoding.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    public StreamReader(string path, Encoding encoding)
      : this(path, encoding, true, 1024)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified character encoding and byte order mark detection option.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    public StreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
      : this(path, encoding, detectEncodingFromByteOrderMarks, 1024)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file name, with the specified character encoding, byte order mark detection option, and buffer size.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <param name="bufferSize">The minimum buffer size, in number of 16-bit characters.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="buffersize" /> is less than or equal to zero.</exception>
    public StreamReader(
      string path,
      Encoding encoding,
      bool detectEncodingFromByteOrderMarks,
      int bufferSize)
      : this(StreamReader.ValidateArgsAndOpenPath(path, encoding, bufferSize), encoding, detectEncodingFromByteOrderMarks, bufferSize, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file path, using the default encoding, enabling detection of byte order marks at the beginning of the file, and configured with the specified <see cref="T:System.IO.FileStreamOptions" /> object.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="options">An object that specifies the configuration options for the underlying <see cref="T:System.IO.FileStream" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="stream" /> is not readable.
    /// 
    /// -or-
    /// 
    ///           <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="options" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    public StreamReader(string path, FileStreamOptions options)
      : this(path, Encoding.UTF8, true, options)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamReader" /> class for the specified file path, with the specified character encoding, byte order mark detection option, and configured with the specified <see cref="T:System.IO.FileStreamOptions" /> object.</summary>
    /// <param name="path">The complete file path to be read.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
    /// <param name="options">An object that specifies the configuration options for the underlying <see cref="T:System.IO.FileStream" />.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="stream" /> is not readable.
    /// 
    ///           -or-
    /// 
    /// <paramref name="path" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="path" /> or <paramref name="encoding" /> or <paramref name="options" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.NotSupportedException">
    /// <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    public StreamReader(
      string path,
      Encoding encoding,
      bool detectEncodingFromByteOrderMarks,
      FileStreamOptions options)
      : this(StreamReader.ValidateArgsAndOpenPath(path, encoding, options), encoding, detectEncodingFromByteOrderMarks, 1024)
    {
    }


    #nullable disable
    private static Stream ValidateArgsAndOpenPath(
      string path,
      Encoding encoding,
      FileStreamOptions options)
    {
      StreamReader.ValidateArgs(path, encoding);
      if (options == null)
        throw new ArgumentNullException(nameof (options));
      return (options.Access & FileAccess.Read) != (FileAccess) 0 ? (Stream) new FileStream(path, options) : throw new ArgumentException(SR.Argument_StreamNotReadable, nameof (options));
    }

    private static Stream ValidateArgsAndOpenPath(
      string path,
      Encoding encoding,
      int bufferSize)
    {
      StreamReader.ValidateArgs(path, encoding);
      if (bufferSize <= 0)
        throw new ArgumentOutOfRangeException(nameof (bufferSize), SR.ArgumentOutOfRange_NeedPosNum);
      return (Stream) new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096);
    }

    private static void ValidateArgs(string path, Encoding encoding)
    {
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (path.Length == 0)
        throw new ArgumentException(SR.Argument_EmptyPath);
    }

    /// <summary>Closes the <see cref="T:System.IO.StreamReader" /> object and the underlying stream, and releases any system resources associated with the reader.</summary>
    public override void Close() => this.Dispose(true);

    /// <summary>Closes the underlying stream, releases the unmanaged resources used by the <see cref="T:System.IO.StreamReader" />, and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      this._disposed = true;
      if (!this._closable)
        return;
      try
      {
        if (!disposing)
          return;
        this._stream.Close();
      }
      finally
      {
        this._charPos = 0;
        this._charLen = 0;
        base.Dispose(disposing);
      }
    }


    #nullable enable
    /// <summary>Gets the current character encoding that the current <see cref="T:System.IO.StreamReader" /> object is using.</summary>
    /// <returns>The current character encoding used by the current reader. The value can be different after the first call to any <see cref="Overload:System.IO.StreamReader.Read" /> method of <see cref="T:System.IO.StreamReader" />, since encoding autodetection is not done until the first call to a <see cref="Overload:System.IO.StreamReader.Read" /> method.</returns>
    public virtual Encoding CurrentEncoding => this._encoding;

    /// <summary>Returns the underlying stream.</summary>
    /// <returns>The underlying stream.</returns>
    public virtual Stream BaseStream => this._stream;

    /// <summary>Clears the internal buffer.</summary>
    public void DiscardBufferedData()
    {
      this.CheckAsyncTaskInProgress();
      this._byteLen = 0;
      this._charLen = 0;
      this._charPos = 0;
      if (this._encoding != null)
        this._decoder = this._encoding.GetDecoder();
      this._isBlocked = false;
    }

    /// <summary>Gets a value that indicates whether the current stream position is at the end of the stream.</summary>
    /// <exception cref="T:System.ObjectDisposedException">The underlying stream has been disposed.</exception>
    /// <returns>
    /// <see langword="true" /> if the current stream position is at the end of the stream; otherwise <see langword="false" />.</returns>
    public bool EndOfStream
    {
      get
      {
        this.ThrowIfDisposed();
        this.CheckAsyncTaskInProgress();
        return this._charPos >= this._charLen && this.ReadBuffer() == 0;
      }
    }

    /// <summary>Returns the next available character but does not consume it.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <returns>An integer representing the next character to be read, or -1 if there are no characters to be read or if the stream does not support seeking.</returns>
    public override int Peek()
    {
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      return this._charPos == this._charLen && (this._isBlocked || this.ReadBuffer() == 0) ? -1 : (int) this._charBuffer[this._charPos];
    }

    /// <summary>Reads the next character from the input stream and advances the character position by one character.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <returns>The next character from the input stream represented as an <see cref="T:System.Int32" /> object, or -1 if no more characters are available.</returns>
    public override int Read()
    {
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      if (this._charPos == this._charLen && this.ReadBuffer() == 0)
        return -1;
      int num = (int) this._charBuffer[this._charPos];
      ++this._charPos;
      return num;
    }

    /// <summary>Reads a specified maximum of characters from the current stream into a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<c>index + count - 1</c>) replaced by the characters read from the current source.</param>
    /// <param name="index">The index of <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read.</param>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the stream is closed.</exception>
    /// <returns>The number of characters that have been read, or 0 if at the end of the stream and no data was read. The number will be less than or equal to the <paramref name="count" /> parameter, depending on whether the data is available within the stream.</returns>
    public override int Read(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0 || count < 0)
        throw new ArgumentOutOfRangeException(index < 0 ? nameof (index) : nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      return this.ReadSpan(new Span<char>(buffer, index, count));
    }

    /// <summary>Reads the characters from the current stream into a span.</summary>
    /// <param name="buffer">When this method returns, contains the specified span of characters replaced by the characters read from the current source.</param>
    /// <exception cref="T:System.IO.IOException">The number of characters read from the stream is larger than the <paramref name="buffer" /> length.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <returns>The number of characters that have been read, or 0 if at the end of the stream and no data was read. The number will be less than or equal to the <paramref name="buffer" /> length, depending on whether the data is available within the stream.</returns>
    public override int Read(Span<char> buffer) => !(this.GetType() == typeof (StreamReader)) ? base.Read(buffer) : this.ReadSpan(buffer);


    #nullable disable
    private int ReadSpan(Span<char> buffer)
    {
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      int start = 0;
      bool readToUserBuffer = false;
      int length1 = buffer.Length;
      while (length1 > 0)
      {
        int length2 = this._charLen - this._charPos;
        if (length2 == 0)
          length2 = this.ReadBuffer(buffer.Slice(start), out readToUserBuffer);
        if (length2 != 0)
        {
          if (length2 > length1)
            length2 = length1;
          if (!readToUserBuffer)
          {
            new Span<char>(this._charBuffer, this._charPos, length2).CopyTo(buffer.Slice(start));
            this._charPos += length2;
          }
          start += length2;
          length1 -= length2;
          if (this._isBlocked)
            break;
        }
        else
          break;
      }
      return start;
    }


    #nullable enable
    /// <summary>Reads all characters from the current position to the end of the stream.</summary>
    /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <returns>The rest of the stream as a string, from the current position to the end. If the current position is at the end of the stream, returns an empty string ("").</returns>
    public override string ReadToEnd()
    {
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      StringBuilder stringBuilder = new StringBuilder(this._charLen - this._charPos);
      do
      {
        stringBuilder.Append(this._charBuffer, this._charPos, this._charLen - this._charPos);
        this._charPos = this._charLen;
        this.ReadBuffer();
      }
      while (this._charLen > 0);
      return stringBuilder.ToString();
    }

    /// <summary>Reads a specified maximum number of characters from the current stream and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<c>index + count - 1</c>) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.StreamReader" /> is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The number of characters that have been read. The number will be less than or equal to <paramref name="count" />, depending on whether all input characters have been read.</returns>
    public override int ReadBlock(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0 || count < 0)
        throw new ArgumentOutOfRangeException(index < 0 ? nameof (index) : nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      return base.ReadBlock(buffer, index, count);
    }

    /// <summary>Reads the characters from the current stream and writes the data to a buffer.</summary>
    /// <param name="buffer">When this method returns, contains the specified span of characters replaced by the characters read from the current source.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.StreamReader" /> is closed.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <returns>The number of characters that have been read. The number will be less than or equal to the <paramref name="buffer" /> length, depending on whether all input characters have been read.</returns>
    public override int ReadBlock(Span<char> buffer)
    {
      if (this.GetType() != typeof (StreamReader))
        return base.ReadBlock(buffer);
      int start = 0;
      int num;
      do
      {
        num = this.ReadSpan(buffer.Slice(start));
        start += num;
      }
      while (num > 0 && start < buffer.Length);
      return start;
    }

    private void CompressBuffer(int n)
    {
      Buffer.BlockCopy((Array) this._byteBuffer, n, (Array) this._byteBuffer, 0, this._byteLen - n);
      this._byteLen -= n;
    }

    private void DetectEncoding()
    {
      if (this._byteLen < 2)
        return;
      this._detectEncoding = false;
      bool flag = false;
      if (this._byteBuffer[0] == (byte) 254 && this._byteBuffer[1] == byte.MaxValue)
      {
        this._encoding = Encoding.BigEndianUnicode;
        this.CompressBuffer(2);
        flag = true;
      }
      else if (this._byteBuffer[0] == byte.MaxValue && this._byteBuffer[1] == (byte) 254)
      {
        if (this._byteLen < 4 || this._byteBuffer[2] != (byte) 0 || this._byteBuffer[3] != (byte) 0)
        {
          this._encoding = Encoding.Unicode;
          this.CompressBuffer(2);
          flag = true;
        }
        else
        {
          this._encoding = Encoding.UTF32;
          this.CompressBuffer(4);
          flag = true;
        }
      }
      else if (this._byteLen >= 3 && this._byteBuffer[0] == (byte) 239 && this._byteBuffer[1] == (byte) 187 && this._byteBuffer[2] == (byte) 191)
      {
        this._encoding = Encoding.UTF8;
        this.CompressBuffer(3);
        flag = true;
      }
      else if (this._byteLen >= 4 && this._byteBuffer[0] == (byte) 0 && this._byteBuffer[1] == (byte) 0 && this._byteBuffer[2] == (byte) 254 && this._byteBuffer[3] == byte.MaxValue)
      {
        this._encoding = (Encoding) new UTF32Encoding(true, true);
        this.CompressBuffer(4);
        flag = true;
      }
      else if (this._byteLen == 2)
        this._detectEncoding = true;
      if (!flag)
        return;
      this._decoder = this._encoding.GetDecoder();
      int maxCharCount = this._encoding.GetMaxCharCount(this._byteBuffer.Length);
      if (maxCharCount > this._maxCharsPerBuffer)
        this._charBuffer = new char[maxCharCount];
      this._maxCharsPerBuffer = maxCharCount;
    }

    private bool IsPreamble()
    {
      if (!this._checkPreamble)
        return this._checkPreamble;
      ReadOnlySpan<byte> preamble = this._encoding.Preamble;
      int num1 = this._byteLen >= preamble.Length ? preamble.Length - this._bytePos : this._byteLen - this._bytePos;
      int num2 = 0;
      while (num2 < num1)
      {
        if ((int) this._byteBuffer[this._bytePos] != (int) preamble[this._bytePos])
        {
          this._bytePos = 0;
          this._checkPreamble = false;
          break;
        }
        ++num2;
        ++this._bytePos;
      }
      if (this._checkPreamble && this._bytePos == preamble.Length)
      {
        this.CompressBuffer(preamble.Length);
        this._bytePos = 0;
        this._checkPreamble = false;
        this._detectEncoding = false;
      }
      return this._checkPreamble;
    }

    internal virtual int ReadBuffer()
    {
      this._charLen = 0;
      this._charPos = 0;
      if (!this._checkPreamble)
        this._byteLen = 0;
      do
      {
        if (this._checkPreamble)
        {
          int num = this._stream.Read(this._byteBuffer, this._bytePos, this._byteBuffer.Length - this._bytePos);
          if (num == 0)
          {
            if (this._byteLen > 0)
            {
              this._charLen += this._decoder.GetChars(this._byteBuffer, 0, this._byteLen, this._charBuffer, this._charLen);
              this._bytePos = this._byteLen = 0;
            }
            return this._charLen;
          }
          this._byteLen += num;
        }
        else
        {
          this._byteLen = this._stream.Read(this._byteBuffer, 0, this._byteBuffer.Length);
          if (this._byteLen == 0)
            return this._charLen;
        }
        this._isBlocked = this._byteLen < this._byteBuffer.Length;
        if (!this.IsPreamble())
        {
          if (this._detectEncoding && this._byteLen >= 2)
            this.DetectEncoding();
          this._charLen += this._decoder.GetChars(this._byteBuffer, 0, this._byteLen, this._charBuffer, this._charLen);
        }
      }
      while (this._charLen == 0);
      return this._charLen;
    }


    #nullable disable
    private int ReadBuffer(Span<char> userBuffer, out bool readToUserBuffer)
    {
      this._charLen = 0;
      this._charPos = 0;
      if (!this._checkPreamble)
        this._byteLen = 0;
      int num1 = 0;
      readToUserBuffer = userBuffer.Length >= this._maxCharsPerBuffer;
      do
      {
        if (this._checkPreamble)
        {
          int num2 = this._stream.Read(this._byteBuffer, this._bytePos, this._byteBuffer.Length - this._bytePos);
          if (num2 == 0)
          {
            if (this._byteLen > 0)
            {
              if (readToUserBuffer)
              {
                num1 = this._decoder.GetChars(new ReadOnlySpan<byte>(this._byteBuffer, 0, this._byteLen), userBuffer.Slice(num1), false);
                this._charLen = 0;
              }
              else
              {
                num1 = this._decoder.GetChars(this._byteBuffer, 0, this._byteLen, this._charBuffer, num1);
                this._charLen += num1;
              }
            }
            return num1;
          }
          this._byteLen += num2;
        }
        else
        {
          this._byteLen = this._stream.Read(this._byteBuffer, 0, this._byteBuffer.Length);
          if (this._byteLen == 0)
            break;
        }
        this._isBlocked = this._byteLen < this._byteBuffer.Length;
        if (!this.IsPreamble())
        {
          if (this._detectEncoding && this._byteLen >= 2)
          {
            this.DetectEncoding();
            readToUserBuffer = userBuffer.Length >= this._maxCharsPerBuffer;
          }
          this._charPos = 0;
          if (readToUserBuffer)
          {
            num1 += this._decoder.GetChars(new ReadOnlySpan<byte>(this._byteBuffer, 0, this._byteLen), userBuffer.Slice(num1), false);
            this._charLen = 0;
          }
          else
          {
            num1 = this._decoder.GetChars(this._byteBuffer, 0, this._byteLen, this._charBuffer, num1);
            this._charLen += num1;
          }
        }
      }
      while (num1 == 0);
      this._isBlocked &= num1 < userBuffer.Length;
      return num1;
    }


    #nullable enable
    /// <summary>Reads a line of characters from the current stream and returns the data as a string.</summary>
    /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <returns>The next line from the input stream, or <see langword="null" /> if the end of the input stream is reached.</returns>
    public override string? ReadLine()
    {
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      if (this._charPos == this._charLen && this.ReadBuffer() == 0)
        return (string) null;
      StringBuilder stringBuilder = (StringBuilder) null;
      do
      {
        int charPos = this._charPos;
        do
        {
          char ch = this._charBuffer[charPos];
          switch (ch)
          {
            case '\n':
            case '\r':
              string str;
              if (stringBuilder != null)
              {
                stringBuilder.Append(this._charBuffer, this._charPos, charPos - this._charPos);
                str = stringBuilder.ToString();
              }
              else
                str = new string(this._charBuffer, this._charPos, charPos - this._charPos);
              this._charPos = charPos + 1;
              if (ch == '\r' && (this._charPos < this._charLen || this.ReadBuffer() > 0) && this._charBuffer[this._charPos] == '\n')
                ++this._charPos;
              return str;
            default:
              ++charPos;
              continue;
          }
        }
        while (charPos < this._charLen);
        int charCount = this._charLen - this._charPos;
        if (stringBuilder == null)
          stringBuilder = new StringBuilder(charCount + 80);
        stringBuilder.Append(this._charBuffer, this._charPos, charCount);
      }
      while (this.ReadBuffer() > 0);
      return stringBuilder.ToString();
    }

    /// <summary>Reads a line of characters asynchronously from the current stream and returns the data as a string.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters in the next line is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the next line from the stream, or is <see langword="null" /> if all the characters have been read.</returns>
    public override Task<string?> ReadLineAsync()
    {
      if (this.GetType() != typeof (StreamReader))
        return base.ReadLineAsync();
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task<string> task = this.ReadLineAsyncInternal();
      this._asyncReadTask = (Task) task;
      return task;
    }


    #nullable disable
    private async Task<string> ReadLineAsyncInternal()
    {
      bool flag1 = this._charPos == this._charLen;
      if (flag1)
        flag1 = await this.ReadBufferAsync(CancellationToken.None).ConfigureAwait(false) == 0;
      if (flag1)
        return (string) null;
      StringBuilder sb = (StringBuilder) null;
      ConfiguredValueTaskAwaitable<int> valueTaskAwaitable;
      do
      {
        char[] charBuffer = this._charBuffer;
        int charLen = this._charLen;
        int charPos1 = this._charPos;
        int index = charPos1;
        do
        {
          char ch = charBuffer[index];
          switch (ch)
          {
            case '\n':
            case '\r':
              string s;
              if (sb != null)
              {
                sb.Append(charBuffer, charPos1, index - charPos1);
                s = sb.ToString();
              }
              else
                s = new string(charBuffer, charPos1, index - charPos1);
              int num1;
              this._charPos = num1 = index + 1;
              bool flag2 = ch == '\r';
              if (flag2)
              {
                bool flag3 = num1 < charLen;
                if (!flag3)
                {
                  valueTaskAwaitable = this.ReadBufferAsync(CancellationToken.None).ConfigureAwait(false);
                  flag3 = await valueTaskAwaitable > 0;
                }
                flag2 = flag3;
              }
              if (flag2)
              {
                int charPos2 = this._charPos;
                if (this._charBuffer[charPos2] == '\n')
                {
                  int num2;
                  this._charPos = num2 = charPos2 + 1;
                }
              }
              return s;
            default:
              ++index;
              continue;
          }
        }
        while (index < charLen);
        int charCount = charLen - charPos1;
        if (sb == null)
          sb = new StringBuilder(charCount + 80);
        sb.Append(charBuffer, charPos1, charCount);
        valueTaskAwaitable = this.ReadBufferAsync(CancellationToken.None).ConfigureAwait(false);
      }
      while (await valueTaskAwaitable > 0);
      return sb.ToString();
    }


    #nullable enable
    /// <summary>Reads all characters from the current position to the end of the stream asynchronously and returns them as one string.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">The number of characters is larger than <see cref="F:System.Int32.MaxValue" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains a string with the characters from the current position to the end of the stream.</returns>
    public override Task<string> ReadToEndAsync()
    {
      if (this.GetType() != typeof (StreamReader))
        return base.ReadToEndAsync();
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task<string> endAsyncInternal = this.ReadToEndAsyncInternal();
      this._asyncReadTask = (Task) endAsyncInternal;
      return endAsyncInternal;
    }


    #nullable disable
    private async Task<string> ReadToEndAsyncInternal()
    {
      StringBuilder sb = new StringBuilder(this._charLen - this._charPos);
      do
      {
        int charPos = this._charPos;
        sb.Append(this._charBuffer, charPos, this._charLen - charPos);
        this._charPos = this._charLen;
        int num = await this.ReadBufferAsync(CancellationToken.None).ConfigureAwait(false);
      }
      while (this._charLen > 0);
      string endAsyncInternal = sb.ToString();
      sb = (StringBuilder) null;
      return endAsyncInternal;
    }


    #nullable enable
    /// <summary>Reads a specified maximum number of characters from the current stream asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read. If the end of the stream is reached before the specified number of characters is written into the buffer, the current method returns.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of characters read into the buffer. The result value can be less than the number of characters requested if the number of characters currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override Task<int> ReadAsync(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0 || count < 0)
        throw new ArgumentOutOfRangeException(index < 0 ? nameof (index) : nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      if (this.GetType() != typeof (StreamReader))
        return base.ReadAsync(buffer, index, count);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task<int> task = this.ReadAsyncInternal(new Memory<char>(buffer, index, count), CancellationToken.None).AsTask();
      this._asyncReadTask = (Task) task;
      return task;
    }

    /// <summary>Asynchronously reads the characters from the current stream into a memory block.</summary>
    /// <param name="buffer">When this method returns, contains the specified memory block of characters replaced by the characters read from the current source.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A value task that represents the asynchronous read operation. The value of the type parameter of the value task contains the number of characters that have been read, or 0 if at the end of the stream and no data was read. The number will be less than or equal to the <paramref name="buffer" /> length, depending on whether the data is available within the stream.</returns>
    public override ValueTask<int> ReadAsync(
      Memory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this.GetType() != typeof (StreamReader))
        return base.ReadAsync(buffer, cancellationToken);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      return cancellationToken.IsCancellationRequested ? ValueTask.FromCanceled<int>(cancellationToken) : this.ReadAsyncInternal(buffer, cancellationToken);
    }


    #nullable disable
    internal override async ValueTask<int> ReadAsyncInternal(
      Memory<char> buffer,
      CancellationToken cancellationToken)
    {
      bool flag = this._charPos == this._charLen;
      ValueTask<int> valueTask;
      if (flag)
      {
        valueTask = this.ReadBufferAsync(cancellationToken);
        flag = await valueTask.ConfigureAwait(false) == 0;
      }
      if (flag)
        return 0;
      int charsRead = 0;
      bool readToUserBuffer = false;
      byte[] tmpByteBuffer = this._byteBuffer;
      Stream tmpStream = this._stream;
      int count = buffer.Length;
      while (count > 0)
      {
        int n = this._charLen - this._charPos;
        Span<char> span;
        if (n == 0)
        {
          this._charLen = 0;
          this._charPos = 0;
          if (!this._checkPreamble)
            this._byteLen = 0;
          readToUserBuffer = count >= this._maxCharsPerBuffer;
          do
          {
            ConfiguredValueTaskAwaitable<int> valueTaskAwaitable;
            if (this._checkPreamble)
            {
              int bytePos = this._bytePos;
              valueTask = tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer, bytePos, tmpByteBuffer.Length - bytePos), cancellationToken);
              valueTaskAwaitable = valueTask.ConfigureAwait(false);
              int num = await valueTaskAwaitable;
              if (num == 0)
              {
                if (this._byteLen > 0)
                {
                  if (readToUserBuffer)
                  {
                    Decoder decoder = this._decoder;
                    ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(tmpByteBuffer, 0, this._byteLen);
                    span = buffer.Span;
                    Span<char> chars = span.Slice(charsRead);
                    n = decoder.GetChars(bytes, chars, false);
                    this._charLen = 0;
                  }
                  else
                  {
                    n = this._decoder.GetChars(tmpByteBuffer, 0, this._byteLen, this._charBuffer, 0);
                    this._charLen += n;
                  }
                }
                this._isBlocked = true;
                break;
              }
              this._byteLen += num;
            }
            else
            {
              valueTask = tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer), cancellationToken);
              valueTaskAwaitable = valueTask.ConfigureAwait(false);
              this._byteLen = await valueTaskAwaitable;
              if (this._byteLen == 0)
              {
                this._isBlocked = true;
                break;
              }
            }
            this._isBlocked = this._byteLen < tmpByteBuffer.Length;
            if (!this.IsPreamble())
            {
              if (this._detectEncoding && this._byteLen >= 2)
              {
                this.DetectEncoding();
                readToUserBuffer = count >= this._maxCharsPerBuffer;
              }
              this._charPos = 0;
              if (readToUserBuffer)
              {
                int num = n;
                Decoder decoder = this._decoder;
                ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(tmpByteBuffer, 0, this._byteLen);
                span = buffer.Span;
                Span<char> chars1 = span.Slice(charsRead);
                int chars2 = decoder.GetChars(bytes, chars1, false);
                n = num + chars2;
                this._charLen = 0;
              }
              else
              {
                n = this._decoder.GetChars(tmpByteBuffer, 0, this._byteLen, this._charBuffer, 0);
                this._charLen += n;
              }
            }
          }
          while (n == 0);
          if (n == 0)
            break;
        }
        if (n > count)
          n = count;
        if (!readToUserBuffer)
        {
          span = new Span<char>(this._charBuffer, this._charPos, n);
          span.CopyTo(buffer.Span.Slice(charsRead));
          this._charPos += n;
        }
        charsRead += n;
        count -= n;
        if (this._isBlocked)
          break;
      }
      return charsRead;
    }


    #nullable enable
    /// <summary>Reads a specified maximum number of characters from the current stream asynchronously and writes the data to a buffer, beginning at the specified index.</summary>
    /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
    /// <param name="index">The position in <paramref name="buffer" /> at which to begin writing.</param>
    /// <param name="count">The maximum number of characters to read. If the end of the stream is reached before the specified number of characters is written into the buffer, the method returns.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">The sum of <paramref name="index" /> and <paramref name="count" /> is larger than the buffer length.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    /// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of characters read into the buffer. The result value can be less than the number of characters requested if the number of characters currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0 || count < 0)
        throw new ArgumentOutOfRangeException(index < 0 ? nameof (index) : nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      if (this.GetType() != typeof (StreamReader))
        return base.ReadBlockAsync(buffer, index, count);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      Task<int> task = base.ReadBlockAsync(buffer, index, count);
      this._asyncReadTask = (Task) task;
      return task;
    }

    /// <summary>Asynchronously reads the characters from the current stream and writes the data to a buffer.</summary>
    /// <param name="buffer">When this method returns, contains the specified memory block of characters replaced by the characters read from the current source.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A value task that represents the asynchronous read operation. The value of the type parameter of the value task contains the total number of characters read into the buffer. The result value can be less than the number of characters requested if the number of characters currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
    public override ValueTask<int> ReadBlockAsync(
      Memory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this.GetType() != typeof (StreamReader))
        return base.ReadBlockAsync(buffer, cancellationToken);
      this.ThrowIfDisposed();
      this.CheckAsyncTaskInProgress();
      if (cancellationToken.IsCancellationRequested)
        return ValueTask.FromCanceled<int>(cancellationToken);
      ValueTask<int> valueTask = this.ReadBlockAsyncInternal(buffer, cancellationToken);
      if (valueTask.IsCompletedSuccessfully)
        return valueTask;
      Task<int> task = valueTask.AsTask();
      this._asyncReadTask = (Task) task;
      return new ValueTask<int>(task);
    }


    #nullable disable
    private async ValueTask<int> ReadBufferAsync(CancellationToken cancellationToken)
    {
      this._charLen = 0;
      this._charPos = 0;
      byte[] tmpByteBuffer = this._byteBuffer;
      Stream tmpStream = this._stream;
      if (!this._checkPreamble)
        this._byteLen = 0;
      do
      {
        ConfiguredValueTaskAwaitable<int> valueTaskAwaitable;
        if (this._checkPreamble)
        {
          int bytePos = this._bytePos;
          valueTaskAwaitable = tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer, bytePos, tmpByteBuffer.Length - bytePos), cancellationToken).ConfigureAwait(false);
          int num = await valueTaskAwaitable;
          if (num == 0)
          {
            if (this._byteLen > 0)
            {
              this._charLen += this._decoder.GetChars(tmpByteBuffer, 0, this._byteLen, this._charBuffer, this._charLen);
              this._bytePos = 0;
              this._byteLen = 0;
            }
            return this._charLen;
          }
          this._byteLen += num;
        }
        else
        {
          valueTaskAwaitable = tmpStream.ReadAsync(new Memory<byte>(tmpByteBuffer), cancellationToken).ConfigureAwait(false);
          this._byteLen = await valueTaskAwaitable;
          if (this._byteLen == 0)
            return this._charLen;
        }
        this._isBlocked = this._byteLen < tmpByteBuffer.Length;
        if (!this.IsPreamble())
        {
          if (this._detectEncoding && this._byteLen >= 2)
            this.DetectEncoding();
          this._charLen += this._decoder.GetChars(tmpByteBuffer, 0, this._byteLen, this._charBuffer, this._charLen);
        }
      }
      while (this._charLen == 0);
      return this._charLen;
    }

    private void ThrowIfDisposed()
    {
      if (!this._disposed)
        return;
      ThrowObjectDisposedException();

      void ThrowObjectDisposedException() => throw new ObjectDisposedException(this.GetType().Name, SR.ObjectDisposed_ReaderClosed);
    }

    private sealed class NullStreamReader : StreamReader
    {
      public override Encoding CurrentEncoding => Encoding.Unicode;

      protected override void Dispose(bool disposing)
      {
      }

      public override int Peek() => -1;

      public override int Read() => -1;

      public override int Read(char[] buffer, int index, int count) => 0;

      public override string ReadLine() => (string) null;

      public override string ReadToEnd() => string.Empty;

      internal override int ReadBuffer() => 0;
    }
  }
}
