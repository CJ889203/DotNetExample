// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Capture
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>Represents the results from a single successful subexpression capture.</summary>
  public class Capture
  {

    #nullable disable
    internal Capture(string text, int index, int length)
    {
      this.Text = text;
      this.Index = index;
      this.Length = length;
    }

    /// <summary>The position in the original string where the first character of the captured substring is found.</summary>
    /// <returns>The zero-based starting position in the original string where the captured substring is found.</returns>
    public int Index { get; private protected set; }

    /// <summary>Gets the length of the captured substring.</summary>
    /// <returns>The length of the captured substring.</returns>
    public int Length { get; private protected set; }


    #nullable enable
    internal string Text { get; set; }

    /// <summary>Gets the captured substring from the input string.</summary>
    /// <returns>The substring that is captured by the match.</returns>
    public string Value => this.Text.Substring(this.Index, this.Length);

    /// <summary>Gets the captured span from the input string.</summary>
    /// <returns>The span that is captured by the match.</returns>
    public ReadOnlySpan<char> ValueSpan => this.Text.AsSpan(this.Index, this.Length);

    /// <summary>Retrieves the captured substring from the input string by calling the <see cref="P:System.Text.RegularExpressions.Capture.Value" /> property.</summary>
    /// <returns>The substring that was captured by the match.</returns>
    public override string ToString() => this.Value;


    #nullable disable
    internal ReadOnlyMemory<char> GetLeftSubstring() => this.Text.AsMemory(0, this.Index);

    internal ReadOnlyMemory<char> GetRightSubstring() => this.Text.AsMemory(this.Index + this.Length, this.Text.Length - this.Index - this.Length);
  }
}
