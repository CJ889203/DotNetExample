// Decompiled with JetBrains decompiler
// Type: System.IO.FileNotFoundException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.IO
{
  /// <summary>The exception that is thrown when an attempt to access a file that does not exist on disk fails.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class FileNotFoundException : IOException
  {

    #nullable disable
    private FileNotFoundException(string fileName, int hResult)
      : base((string) null)
    {
      this.HResult = hResult;
      this.FileName = fileName;
      this.SetMessageField();
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with its message string set to a system-supplied message.</summary>
    public FileNotFoundException()
      : base(SR.IO_FileNotFound)
    {
      this.HResult = -2147024894;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with a specified error message.</summary>
    /// <param name="message">A description of the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    public FileNotFoundException(string? message)
      : base(message)
    {
      this.HResult = -2147024894;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">A description of the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public FileNotFoundException(string? message, Exception? innerException)
      : base(message, innerException)
    {
      this.HResult = -2147024894;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with a specified error message, and the file name that cannot be found.</summary>
    /// <param name="message">A description of the error, or <see langword="null" /> to use a system-supplied message with the given <paramref name="fileName" />. The content of <paramref name="message" /> should be understood by humans. The caller of this constructor must ensure that this string has been localized for the current system culture.</param>
    /// <param name="fileName">The full name of the file with the invalid image.</param>
    public FileNotFoundException(string? message, string? fileName)
      : base(message)
    {
      this.HResult = -2147024894;
      this.FileName = fileName;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with a specified error message, the file name that cannot be found, and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="fileName">The full name of the file with the invalid image.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public FileNotFoundException(string? message, string? fileName, Exception? innerException)
      : base(message, innerException)
    {
      this.HResult = -2147024894;
      this.FileName = fileName;
    }

    /// <summary>Gets the error message that explains the reason for the exception.</summary>
    /// <returns>The error message.</returns>
    public override string Message
    {
      get
      {
        this.SetMessageField();
        return this._message;
      }
    }

    private void SetMessageField()
    {
      if (this._message != null)
        return;
      if (this.FileName == null && this.HResult == -2146233088)
      {
        this._message = SR.IO_FileNotFound;
      }
      else
      {
        if (this.FileName == null)
          return;
        this._message = FileLoadException.FormatFileLoadExceptionMessage(this.FileName, this.HResult);
      }
    }

    /// <summary>Gets the name of the file that cannot be found.</summary>
    /// <returns>The name of the file, or <see langword="null" /> if no file name was passed to the constructor for this instance.</returns>
    public string? FileName { get; }

    /// <summary>Gets the log file that describes why loading of an assembly failed.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>The errors reported by the assembly cache.</returns>
    public string? FusionLog { get; }

    /// <summary>Returns the fully qualified name of this exception and possibly the error message, the name of the inner exception, and the stack trace.</summary>
    /// <returns>The fully qualified name of this exception and possibly the error message, the name of the inner exception, and the stack trace.</returns>
    public override string ToString()
    {
      string str = this.GetType().ToString() + ": " + this.Message;
      if (!string.IsNullOrEmpty(this.FileName))
        str = str + "\r\n" + SR.Format(SR.IO_FileName_Name, (object) this.FileName);
      if (this.InnerException != null)
        str = str + "\r\n ---> " + this.InnerException.ToString();
      if (this.StackTrace != null)
        str = str + "\r\n" + this.StackTrace;
      if (this.FusionLog != null)
      {
        if (str == null)
          str = " ";
        str = str + "\r\n\r\n" + this.FusionLog;
      }
      return str;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with the specified serialization and context information.</summary>
    /// <param name="info">An object that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">An object that contains contextual information about the source or destination.</param>
    protected FileNotFoundException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.FileName = info.GetString("FileNotFound_FileName");
      this.FusionLog = info.GetString("FileNotFound_FusionLog");
    }

    /// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name and additional exception information.</summary>
    /// <param name="info">The object that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The object that contains contextual information about the source or destination.</param>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("FileNotFound_FileName", (object) this.FileName, typeof (string));
      info.AddValue("FileNotFound_FusionLog", (object) this.FusionLog, typeof (string));
    }
  }
}
