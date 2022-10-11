// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Group
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>Represents the results from a single capturing group.</summary>
  public class Group : Capture
  {

    #nullable disable
    internal static readonly Group s_emptyGroup = new Group(string.Empty, Array.Empty<int>(), 0, string.Empty);
    internal readonly int[] _caps;
    internal int _capcount;
    internal CaptureCollection _capcoll;

    internal Group(string text, int[] caps, int capcount, string name)
      : base(text, capcount == 0 ? 0 : caps[(capcount - 1) * 2], capcount == 0 ? 0 : caps[capcount * 2 - 1])
    {
      this._caps = caps;
      this._capcount = capcount;
      this.Name = name;
    }

    /// <summary>Gets a value indicating whether the match is successful.</summary>
    /// <returns>
    /// <see langword="true" /> if the match is successful; otherwise, <see langword="false" />.</returns>
    public bool Success => this._capcount != 0;


    #nullable enable
    /// <summary>Returns the name of the capturing group represented by the current instance.</summary>
    /// <returns>The name of the capturing group represented by the current instance.</returns>
    public string Name { get; }

    /// <summary>Gets a collection of all the captures matched by the capturing group, in innermost-leftmost-first order (or innermost-rightmost-first order if the regular expression is modified with the <see cref="F:System.Text.RegularExpressions.RegexOptions.RightToLeft" /> option). The collection may have zero or more items.</summary>
    /// <returns>The collection of substrings matched by the group.</returns>
    public CaptureCollection Captures => this._capcoll ?? (this._capcoll = new CaptureCollection(this));

    /// <summary>Returns a <see langword="Group" /> object equivalent to the one supplied that is safe to share between multiple threads.</summary>
    /// <param name="inner">The input <see cref="T:System.Text.RegularExpressions.Group" /> object.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inner" /> is <see langword="null" />.</exception>
    /// <returns>A regular expression <see langword="Group" /> object.</returns>
    public static Group Synchronized(Group inner)
    {
      if (inner == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.inner);
      CaptureCollection captures = inner.Captures;
      if (inner.Success)
        captures.ForceInitialized();
      return inner;
    }
  }
}
