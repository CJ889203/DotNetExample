// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.RegexMatchTimeoutException
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>The exception that is thrown when the execution time of a regular expression pattern-matching method exceeds its time-out interval.</summary>
  [TypeForwardedFrom("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class RegexMatchTimeoutException : TimeoutException, ISerializable
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException" /> class with information about the regular expression pattern, the input text, and the time-out interval.</summary>
    /// <param name="regexInput">The input text processed by the regular expression engine when the time-out occurred.</param>
    /// <param name="regexPattern">The pattern used by the regular expression engine when the time-out occurred.</param>
    /// <param name="matchTimeout">The time-out interval.</param>
    public RegexMatchTimeoutException(
      string regexInput,
      string regexPattern,
      TimeSpan matchTimeout)
      : base(SR.RegexMatchTimeoutException_Occurred)
    {
      this.Input = regexInput;
      this.Pattern = regexPattern;
      this.MatchTimeout = matchTimeout;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException" /> class with a system-supplied message.</summary>
    public RegexMatchTimeoutException()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException" /> class with the specified message string.</summary>
    /// <param name="message">A string that describes the exception.</param>
    public RegexMatchTimeoutException(string message)
      : base(message)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="message">A string that describes the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public RegexMatchTimeoutException(string message, Exception inner)
      : base(message, inner)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException" /> class with serialized data.</summary>
    /// <param name="info">The object that contains the serialized data.</param>
    /// <param name="context">The stream that contains the serialized data.</param>
    protected RegexMatchTimeoutException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.Input = info.GetString("regexInput");
      this.Pattern = info.GetString("regexPattern");
      this.MatchTimeout = new TimeSpan(info.GetInt64("timeoutTicks"));
    }


    #nullable disable
    /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data needed to serialize a <see cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException" /> object.</summary>
    /// <param name="info">The serialization information object to populate with data.</param>
    /// <param name="context">The destination for this serialization.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      this.GetObjectData(info, context);
      info.AddValue("regexInput", (object) this.Input);
      info.AddValue("regexPattern", (object) this.Pattern);
      info.AddValue("timeoutTicks", this.MatchTimeout.Ticks);
    }


    #nullable enable
    /// <summary>Gets the input text that the regular expression engine was processing when the time-out occurred.</summary>
    /// <returns>The regular expression input text.</returns>
    public string Input { get; } = string.Empty;

    /// <summary>Gets the regular expression pattern that was used in the matching operation when the time-out occurred.</summary>
    /// <returns>The regular expression pattern.</returns>
    public string Pattern { get; } = string.Empty;

    /// <summary>Gets the time-out interval for a regular expression match.</summary>
    /// <returns>The time-out interval.</returns>
    public TimeSpan MatchTimeout { get; } = TimeSpan.FromTicks(-1L);
  }
}
