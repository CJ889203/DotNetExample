// Decompiled with JetBrains decompiler
// Type: System.Text.Json.JsonException
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml

using System.Runtime.Serialization;


#nullable enable
namespace System.Text.Json
{
  /// <summary>Defines a custom exception object that is thrown when invalid JSON text is encountered, when the defined maximum depth is passed, or the JSON text is not compatible with the type of a property on an object.</summary>
  [Serializable]
  public class JsonException : Exception
  {

    #nullable disable
    internal string _message;


    #nullable enable
    /// <summary>Creates a new exception object to relay error information to the user that includes a specified inner exception.</summary>
    /// <param name="message">The context-specific error message.</param>
    /// <param name="path">The path where the invalid JSON was encountered.</param>
    /// <param name="lineNumber">The line number (starting at 0) at which the invalid JSON was encountered when deserializing.</param>
    /// <param name="bytePositionInLine">The byte count (starting at 0) within the current line where the invalid JSON was encountered.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public JsonException(
      string? message,
      string? path,
      long? lineNumber,
      long? bytePositionInLine,
      Exception? innerException)
      : base(message, innerException)
    {
      this._message = message;
      this.LineNumber = lineNumber;
      this.BytePositionInLine = bytePositionInLine;
      this.Path = path;
    }

    /// <summary>Creates a new exception object to relay error information to the user.</summary>
    /// <param name="message">The context-specific error message.</param>
    /// <param name="path">The path where the invalid JSON was encountered.</param>
    /// <param name="lineNumber">The line number (starting at 0) at which the invalid JSON was encountered when deserializing.</param>
    /// <param name="bytePositionInLine">The byte count within the current line (starting at 0) where the invalid JSON was encountered.</param>
    public JsonException(string? message, string? path, long? lineNumber, long? bytePositionInLine)
      : base(message)
    {
      this._message = message;
      this.LineNumber = lineNumber;
      this.BytePositionInLine = bytePositionInLine;
      this.Path = path;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.JsonException" /> class, with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The context-specific error message.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public JsonException(string? message, Exception? innerException)
      : base(message, innerException)
    {
      this._message = message;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.JsonException" /> class with a specified error message.</summary>
    /// <param name="message">The context-specific error message.</param>
    public JsonException(string? message)
      : base(message)
    {
      this._message = message;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.Json.JsonException" /> class.</summary>
    public JsonException()
    {
    }

    /// <summary>Creates a new exception object with serialized data.</summary>
    /// <param name="info">The serialized object data about the exception being thrown.</param>
    /// <param name="context">An object that contains contextual information about the source or destination.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    protected JsonException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.LineNumber = (long?) info.GetValue(nameof (LineNumber), typeof (long?));
      this.BytePositionInLine = (long?) info.GetValue(nameof (BytePositionInLine), typeof (long?));
      this.Path = info.GetString(nameof (Path));
      this.SetMessage(info.GetString("ActualMessage"));
    }

    internal bool AppendPathInformation { get; set; }

    /// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.</summary>
    /// <param name="info">The serialized object data about the exception being thrown.</param>
    /// <param name="context">An object that contains contextual information about the source or destination.</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("LineNumber", (object) this.LineNumber, typeof (long?));
      info.AddValue("BytePositionInLine", (object) this.BytePositionInLine, typeof (long?));
      info.AddValue("Path", (object) this.Path, typeof (string));
      info.AddValue("ActualMessage", (object) this.Message, typeof (string));
    }

    /// <summary>Gets the zero-based number of lines read before the exception.</summary>
    /// <returns>The zero-based number of lines read before the exception.</returns>
    public long? LineNumber { get; internal set; }

    /// <summary>Gets the zero-based number of bytes read within the current line before the exception.</summary>
    /// <returns>The zero-based number of bytes read within the current line before the exception.</returns>
    public long? BytePositionInLine { get; internal set; }

    /// <summary>Gets The path within the JSON where the exception was encountered.</summary>
    /// <returns>The path within the JSON where the exception was encountered.</returns>
    public string? Path { get; internal set; }

    /// <summary>Gets a message that describes the current exception.</summary>
    /// <returns>The error message that describes the current exception.</returns>
    public override string Message => this._message ?? base.Message;


    #nullable disable
    internal void SetMessage(string message) => this._message = message;
  }
}
