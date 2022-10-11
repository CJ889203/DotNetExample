// Decompiled with JetBrains decompiler
// Type: System.IO.StringWriter
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace System.IO
{
  /// <summary>Implements a <see cref="T:System.IO.TextWriter" /> for writing information to a string. The information is stored in an underlying <see cref="T:System.Text.StringBuilder" />.</summary>
  public class StringWriter : TextWriter
  {

    #nullable disable
    private static volatile UnicodeEncoding s_encoding;
    private readonly StringBuilder _sb;
    private bool _isOpen;

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StringWriter" /> class.</summary>
    public StringWriter()
      : this(new StringBuilder(), (IFormatProvider) CultureInfo.CurrentCulture)
    {
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StringWriter" /> class with the specified format control.</summary>
    /// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting.</param>
    public StringWriter(IFormatProvider? formatProvider)
      : this(new StringBuilder(), formatProvider)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StringWriter" /> class that writes to the specified <see cref="T:System.Text.StringBuilder" />.</summary>
    /// <param name="sb">The <see cref="T:System.Text.StringBuilder" /> object to write to.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sb" /> is <see langword="null" />.</exception>
    public StringWriter(StringBuilder sb)
      : this(sb, (IFormatProvider) CultureInfo.CurrentCulture)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.StringWriter" /> class that writes to the specified <see cref="T:System.Text.StringBuilder" /> and has the specified format provider.</summary>
    /// <param name="sb">The <see cref="T:System.Text.StringBuilder" /> object to write to.</param>
    /// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="sb" /> is <see langword="null" />.</exception>
    public StringWriter(StringBuilder sb, IFormatProvider? formatProvider)
      : base(formatProvider)
    {
      this._sb = sb != null ? sb : throw new ArgumentNullException(nameof (sb), SR.ArgumentNull_Buffer);
      this._isOpen = true;
    }

    /// <summary>Closes the current <see cref="T:System.IO.StringWriter" /> and the underlying stream.</summary>
    public override void Close() => this.Dispose(true);

    /// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.StringWriter" /> and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      this._isOpen = false;
      base.Dispose(disposing);
    }

    /// <summary>Gets the <see cref="T:System.Text.Encoding" /> in which the output is written.</summary>
    /// <returns>The <see langword="Encoding" /> in which the output is written.</returns>
    public override Encoding Encoding
    {
      get
      {
        if (StringWriter.s_encoding == null)
          StringWriter.s_encoding = new UnicodeEncoding(false, false);
        return (Encoding) StringWriter.s_encoding;
      }
    }

    /// <summary>Returns the underlying <see cref="T:System.Text.StringBuilder" />.</summary>
    /// <returns>The underlying <see langword="StringBuilder" />.</returns>
    public virtual StringBuilder GetStringBuilder() => this._sb;

    /// <summary>Writes a character to the string.</summary>
    /// <param name="value">The character to write.</param>
    /// <exception cref="T:System.ObjectDisposedException">The writer is closed.</exception>
    public override void Write(char value)
    {
      if (!this._isOpen)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
      this._sb.Append(value);
    }

    /// <summary>Writes a subarray of characters to the string.</summary>
    /// <param name="buffer">The character array to write data from.</param>
    /// <param name="index">The position in the buffer at which to start reading data.</param>
    /// <param name="count">The maximum number of characters to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ArgumentException">(<paramref name="index" /> + <paramref name="count" />)&gt; <paramref name="buffer" />. <see langword="Length" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The writer is closed.</exception>
    public override void Write(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer), SR.ArgumentNull_Buffer);
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count), SR.ArgumentOutOfRange_NeedNonNegNum);
      if (buffer.Length - index < count)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      if (!this._isOpen)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
      this._sb.Append(buffer, index, count);
    }

    /// <summary>Writes the string representation of a span of chars to the current string.</summary>
    /// <param name="buffer">A span of chars to write to the string.</param>
    public override void Write(ReadOnlySpan<char> buffer)
    {
      if (this.GetType() != typeof (StringWriter))
      {
        base.Write(buffer);
      }
      else
      {
        if (!this._isOpen)
          throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
        this._sb.Append(buffer);
      }
    }

    /// <summary>Writes a string to the current string.</summary>
    /// <param name="value">The string to write.</param>
    /// <exception cref="T:System.ObjectDisposedException">The writer is closed.</exception>
    public override void Write(string? value)
    {
      if (!this._isOpen)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
      if (value == null)
        return;
      this._sb.Append(value);
    }

    /// <summary>Writes the string representation of a string builder to the current string.</summary>
    /// <param name="value">The string builder to write to the string.</param>
    public override void Write(StringBuilder? value)
    {
      if (this.GetType() != typeof (StringWriter))
      {
        base.Write(value);
      }
      else
      {
        if (!this._isOpen)
          throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
        this._sb.Append(value);
      }
    }

    /// <summary>Writes the text representation a span of characters to the string, followed by a line terminator.</summary>
    /// <param name="buffer">The span of characters to write to the string.</param>
    public override void WriteLine(ReadOnlySpan<char> buffer)
    {
      if (this.GetType() != typeof (StringWriter))
      {
        base.WriteLine(buffer);
      }
      else
      {
        if (!this._isOpen)
          throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
        this._sb.Append(buffer);
        this.WriteLine();
      }
    }

    /// <summary>Writes the text representation of a string builder to the string, followed by a line terminator.</summary>
    /// <param name="value">The string, as a string builder, to write to the string.</param>
    public override void WriteLine(StringBuilder? value)
    {
      if (this.GetType() != typeof (StringWriter))
      {
        base.WriteLine(value);
      }
      else
      {
        if (!this._isOpen)
          throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
        this._sb.Append(value);
        this.WriteLine();
      }
    }

    /// <summary>Writes a character to the string asynchronously.</summary>
    /// <param name="value">The character to write to the string.</param>
    /// <exception cref="T:System.ObjectDisposedException">The string writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The string writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(char value)
    {
      this.Write(value);
      return Task.CompletedTask;
    }

    /// <summary>Writes a string to the current string asynchronously.</summary>
    /// <param name="value">The string to write. If <paramref name="value" /> is <see langword="null" />, nothing is written to the text stream.</param>
    /// <exception cref="T:System.ObjectDisposedException">The string writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The string writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(string? value)
    {
      this.Write(value);
      return Task.CompletedTask;
    }

    /// <summary>Writes a subarray of characters to the string asynchronously.</summary>
    /// <param name="buffer">The character array to write data from.</param>
    /// <param name="index">The position in the buffer at which to start reading data.</param>
    /// <param name="count">The maximum number of characters to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="index" /> plus <paramref name="count" /> is greater than the buffer length.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The string writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The string writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(char[] buffer, int index, int count)
    {
      this.Write(buffer, index, count);
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously writes a memory region of characters to the string.</summary>
    /// <param name="buffer">The character memory region to write to the string.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(
      ReadOnlyMemory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      this.Write(buffer.Span);
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously writes the text representation of a string builder to the string.</summary>
    /// <param name="value">The string builder to write to the string.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this.GetType() != typeof (StringWriter))
        return base.WriteAsync(value, cancellationToken);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (!this._isOpen)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
      this._sb.Append(value);
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously writes a character to the string, followed by a line terminator.</summary>
    /// <param name="value">The character to write to the string.</param>
    /// <exception cref="T:System.ObjectDisposedException">The string writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The string writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(char value)
    {
      this.WriteLine(value);
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously writes a string to the current string, followed by a line terminator.</summary>
    /// <param name="value">The string to write. If the value is <see langword="null" />, only a line terminator is written.</param>
    /// <exception cref="T:System.ObjectDisposedException">The string writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The string writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(string? value)
    {
      this.WriteLine(value);
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously writes the string representation of the string builder to the current string, followed by a line terminator.</summary>
    /// <param name="value">The string builder to write to the string.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(
      StringBuilder? value,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (this.GetType() != typeof (StringWriter))
        return base.WriteLineAsync(value, cancellationToken);
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      if (!this._isOpen)
        throw new ObjectDisposedException((string) null, SR.ObjectDisposed_WriterClosed);
      this._sb.Append(value);
      this.WriteLine();
      return Task.CompletedTask;
    }

    /// <summary>asynchronously writes a subarray of characters to the string, followed by a line terminator.</summary>
    /// <param name="buffer">The character array to write data from.</param>
    /// <param name="index">The position in the buffer at which to start reading data.</param>
    /// <param name="count">The maximum number of characters to write.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The <paramref name="index" /> plus <paramref name="count" /> is greater than the buffer length.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The string writer is disposed.</exception>
    /// <exception cref="T:System.InvalidOperationException">The string writer is currently in use by a previous write operation.</exception>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(char[] buffer, int index, int count)
    {
      this.WriteLine(buffer, index, count);
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously writes the string representation of the memory region of characters to the current string, followed by a line terminator.</summary>
    /// <param name="buffer">A memory region of characters to write to the string.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="P:System.Threading.CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public override Task WriteLineAsync(
      ReadOnlyMemory<char> buffer,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return Task.FromCanceled(cancellationToken);
      this.WriteLine(buffer.Span);
      return Task.CompletedTask;
    }

    /// <summary>Asynchronously clears all buffers for the current writer and causes any buffered data to be written to the underlying device.</summary>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    public override Task FlushAsync() => Task.CompletedTask;

    /// <summary>Returns a string containing the characters written to the current <see langword="StringWriter" /> so far.</summary>
    /// <returns>The string containing the characters written to the current <see langword="StringWriter" />.</returns>
    public override string ToString() => this._sb.ToString();
  }
}
