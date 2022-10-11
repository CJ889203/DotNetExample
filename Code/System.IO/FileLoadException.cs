// Decompiled with JetBrains decompiler
// Type: System.IO.FileLoadException
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.IO
{
  /// <summary>The exception that is thrown when a managed assembly is found but cannot be loaded.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class FileLoadException : IOException
  {

    #nullable disable
    private FileLoadException(string fileName, int hResult)
      : base((string) null)
    {
      this.HResult = hResult;
      this.FileName = fileName;
      this._message = FileLoadException.FormatFileLoadExceptionMessage(this.FileName, this.HResult);
    }

    internal static string FormatFileLoadExceptionMessage(string fileName, int hResult)
    {
      string s1 = (string) null;
      FileLoadException.GetFileLoadExceptionMessage(hResult, new StringHandleOnStack(ref s1));
      string s2 = (string) null;
      if (hResult == -2147024703)
        s2 = SR.Arg_BadImageFormatException;
      else
        FileLoadException.GetMessageForHR(hResult, new StringHandleOnStack(ref s2));
      return string.Format(s1, (object) fileName, (object) s2);
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void GetFileLoadExceptionMessage(
      int hResult,
      StringHandleOnStack retString);

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    private static extern void GetMessageForHR(int hresult, StringHandleOnStack retString);

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileLoadException" /> class, setting the <see cref="P:System.Exception.Message" /> property of the new instance to a system-supplied message that describes the error, such as "Could not load the specified file." This message takes into account the current system culture.</summary>
    public FileLoadException()
      : base(SR.IO_FileLoad)
    {
      this.HResult = -2146232799;
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileLoadException" /> class with the specified error message.</summary>
    /// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    public FileLoadException(string? message)
      : base(message)
    {
      this.HResult = -2146232799;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileLoadException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public FileLoadException(string? message, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2146232799;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileLoadException" /> class with a specified error message and the name of the file that could not be loaded.</summary>
    /// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    /// <param name="fileName">A <see cref="T:System.String" /> containing the name of the file that was not loaded.</param>
    public FileLoadException(string? message, string? fileName)
      : base(message)
    {
      this.HResult = -2146232799;
      this.FileName = fileName;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileLoadException" /> class with a specified error message, the name of the file that could not be loaded, and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
    /// <param name="fileName">A <see cref="T:System.String" /> containing the name of the file that was not loaded.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
    public FileLoadException(string? message, string? fileName, Exception? inner)
      : base(message, inner)
    {
      this.HResult = -2146232799;
      this.FileName = fileName;
    }

    /// <summary>Gets the error message and the name of the file that caused this exception.</summary>
    /// <returns>A string containing the error message and the name of the file that caused this exception.</returns>
    public override string Message => this._message ?? (this._message = FileLoadException.FormatFileLoadExceptionMessage(this.FileName, this.HResult));

    /// <summary>Gets the name of the file that causes this exception.</summary>
    /// <returns>A <see cref="T:System.String" /> containing the name of the file with the invalid image, or a null reference if no file name was passed to the constructor for the current instance.</returns>
    public string? FileName { get; }

    /// <summary>Gets the log file that describes why an assembly load failed.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <returns>A string containing errors reported by the assembly cache.</returns>
    public string? FusionLog { get; }

    /// <summary>Returns the fully qualified name of the current exception, and possibly the error message, the name of the inner exception, and the stack trace.</summary>
    /// <returns>A string containing the fully qualified name of this exception, and possibly the error message, the name of the inner exception, and the stack trace, depending on which <see cref="T:System.IO.FileLoadException" /> constructor is used.</returns>
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

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.FileLoadException" /> class with serialized data.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    protected FileLoadException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.FileName = info.GetString("FileLoad_FileName");
      this.FusionLog = info.GetString("FileLoad_FusionLog");
    }

    /// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the file name and additional exception information.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("FileLoad_FileName", (object) this.FileName, typeof (string));
      info.AddValue("FileLoad_FusionLog", (object) this.FusionLog, typeof (string));
    }
  }
}
