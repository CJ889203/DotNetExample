// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.RegexParseException
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml

using System.Runtime.Serialization;


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>An exception as a result of a parse error in a regular expression, with detailed information in the <see cref="P:System.Text.RegularExpressions.RegexParseException.Error" /> and <see cref="P:System.Text.RegularExpressions.RegexParseException.Offset" /> properties.</summary>
  [Serializable]
  public sealed class RegexParseException : ArgumentException
  {
    /// <summary>Gets the error that happened during parsing.</summary>
    /// <returns>The error that occured during parsing.</returns>
    public RegexParseError Error { get; }

    /// <summary>Gets the zero-based character offset in the regular expression pattern where the parse error occurs.</summary>
    /// <returns>The offset at which the parse error occurs.</returns>
    public int Offset { get; }


    #nullable disable
    internal RegexParseException(RegexParseError error, int offset, string message)
      : base(message)
    {
      this.Error = error;
      this.Offset = offset;
    }

    private RegexParseException(SerializationInfo info, StreamingContext context) => throw new NotImplementedException();


    #nullable enable
    /// <summary>Sets the <paramref name="info" /> object with the parameter name and additional exception information.</summary>
    /// <param name="info">The object that holds the serialized object data.</param>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.SetType(typeof (ArgumentException));
    }
  }
}
