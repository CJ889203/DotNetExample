// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Match
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml

using System.Collections;


#nullable enable
namespace System.Text.RegularExpressions
{
  /// <summary>Represents the results from a single regular expression match.</summary>
  public class Match : Group
  {

    #nullable disable
    internal GroupCollection _groupcoll;
    internal Regex _regex;
    internal int _textbeg;
    internal int _textpos;
    internal int _textend;
    internal int _textstart;
    internal int[][] _matches;
    internal int[] _matchcount;
    internal bool _balancing;

    internal Match(Regex regex, int capcount, string text, int begpos, int len, int startpos)
      : base(text, new int[2], 0, "0")
    {
      this._regex = regex;
      this._matchcount = new int[capcount];
      this._matches = new int[capcount][];
      this._matches[0] = this._caps;
      this._textbeg = begpos;
      this._textend = begpos + len;
      this._textstart = startpos;
      this._balancing = false;
    }


    #nullable enable
    /// <summary>Gets the empty group. All failed matches return this empty match.</summary>
    /// <returns>An empty match.</returns>
    public static Match Empty { get; } = new Match((Regex) null, 1, string.Empty, 0, 0, 0);


    #nullable disable
    internal void Reset(Regex regex, string text, int textbeg, int textend, int textstart)
    {
      this._regex = regex;
      this.Text = text;
      this._textbeg = textbeg;
      this._textend = textend;
      this._textstart = textstart;
      foreach (int num in this._matchcount)
        num = 0;
      this._balancing = false;
      this._groupcoll?.Reset();
    }


    #nullable enable
    /// <summary>Gets a collection of groups matched by the regular expression.</summary>
    /// <returns>The character groups matched by the pattern.</returns>
    public virtual GroupCollection Groups => this._groupcoll ?? (this._groupcoll = new GroupCollection(this, (Hashtable) null));

    /// <summary>Returns a new <see cref="T:System.Text.RegularExpressions.Match" /> object with the results for the next match, starting at the position at which the last match ended (at the character after the last matched character).</summary>
    /// <exception cref="T:System.Text.RegularExpressions.RegexMatchTimeoutException">A time-out occurred.</exception>
    /// <returns>The next regular expression match.</returns>
    public Match NextMatch()
    {
      Regex regex = this._regex;
      return regex == null ? this : regex.Run(false, this.Length, this.Text, this._textbeg, this._textend - this._textbeg, this._textpos);
    }

    /// <summary>Returns the expansion of the specified replacement pattern.</summary>
    /// <param name="replacement">The replacement pattern to use.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="replacement" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.NotSupportedException">Expansion is not allowed for this pattern.</exception>
    /// <returns>The expanded version of the <paramref name="replacement" /> parameter.</returns>
    public virtual string Result(string replacement)
    {
      if (replacement == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.replacement);
      Regex regex = this._regex;
      if (regex == null)
        throw new NotSupportedException(SR.NoResultOnFailed);
      RegexReplacement regexReplacement = RegexReplacement.GetOrCreate(regex._replref, replacement, regex.caps, regex.capsize, regex.capnames, regex.roptions);
      SegmentStringBuilder segments = SegmentStringBuilder.Create();
      regexReplacement.ReplacementImpl(ref segments, this);
      return segments.ToString();
    }


    #nullable disable
    internal ReadOnlyMemory<char> GroupToStringImpl(int groupnum)
    {
      int num = this._matchcount[groupnum];
      if (num == 0)
        return new ReadOnlyMemory<char>();
      int[] match = this._matches[groupnum];
      return this.Text.AsMemory(match[(num - 1) * 2], match[num * 2 - 1]);
    }

    internal ReadOnlyMemory<char> LastGroupToStringImpl() => this.GroupToStringImpl(this._matchcount.Length - 1);


    #nullable enable
    /// <summary>Returns a <see cref="T:System.Text.RegularExpressions.Match" /> instance equivalent to the one supplied that is suitable to share between multiple threads.</summary>
    /// <param name="inner">A regular expression match equivalent to the one expected.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="inner" /> is <see langword="null" />.</exception>
    /// <returns>A regular expression match that is suitable to share between multiple threads.</returns>
    public static Match Synchronized(Match inner)
    {
      if (inner == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.inner);
      int length = inner._matchcount.Length;
      for (int groupnum = 0; groupnum < length; ++groupnum)
        Group.Synchronized(inner.Groups[groupnum]);
      return inner;
    }

    internal void AddMatch(int cap, int start, int len)
    {
      int[][] matches1 = this._matches;
      int index1 = cap;
      if (matches1[index1] == null)
        matches1[index1] = new int[2];
      int[][] matches2 = this._matches;
      int[] matchcount = this._matchcount;
      int num = matchcount[cap];
      if (num * 2 + 2 > matches2[cap].Length)
      {
        int[] numArray1 = matches2[cap];
        int[] numArray2 = new int[num * 8];
        for (int index2 = 0; index2 < num * 2; ++index2)
          numArray2[index2] = numArray1[index2];
        matches2[cap] = numArray2;
      }
      matches2[cap][num * 2] = start;
      matches2[cap][num * 2 + 1] = len;
      matchcount[cap] = num + 1;
    }

    internal void BalanceMatch(int cap)
    {
      this._balancing = true;
      int index1 = this._matchcount[cap] * 2 - 2;
      int[][] matches = this._matches;
      if (matches[cap][index1] < 0)
        index1 = -3 - matches[cap][index1];
      int index2 = index1 - 2;
      if (index2 >= 0 && matches[cap][index2] < 0)
        this.AddMatch(cap, matches[cap][index2], matches[cap][index2 + 1]);
      else
        this.AddMatch(cap, -3 - index2, -4 - index2);
    }

    internal void RemoveMatch(int cap) => --this._matchcount[cap];

    internal bool IsMatched(int cap)
    {
      int[] matchcount = this._matchcount;
      return (uint) cap < (uint) matchcount.Length && matchcount[cap] > 0 && this._matches[cap][matchcount[cap] * 2 - 1] != -2;
    }

    internal int MatchIndex(int cap)
    {
      int[][] matches = this._matches;
      int num = matches[cap][this._matchcount[cap] * 2 - 2];
      return num < 0 ? matches[cap][-3 - num] : num;
    }

    internal int MatchLength(int cap)
    {
      int[][] matches = this._matches;
      int num = matches[cap][this._matchcount[cap] * 2 - 1];
      return num < 0 ? matches[cap][-3 - num] : num;
    }

    internal void Tidy(int textpos)
    {
      this._textpos = textpos;
      this._capcount = this._matchcount[0];
      int[] match = this._matches[0];
      this.Index = match[0];
      this.Length = match[1];
      if (!this._balancing)
        return;
      this.TidyBalancing();
    }

    private void TidyBalancing()
    {
      int[] matchcount = this._matchcount;
      int[][] matches = this._matches;
      for (int index1 = 0; index1 < matchcount.Length; ++index1)
      {
        int num = matchcount[index1] * 2;
        int[] numArray = matches[index1];
        int index2 = 0;
        while (index2 < num && numArray[index2] >= 0)
          ++index2;
        int index3 = index2;
        for (; index2 < num; ++index2)
        {
          if (numArray[index2] < 0)
          {
            --index3;
          }
          else
          {
            if (index2 != index3)
              numArray[index3] = numArray[index2];
            ++index3;
          }
        }
        matchcount[index1] = index3 / 2;
      }
      this._balancing = false;
    }
  }
}
