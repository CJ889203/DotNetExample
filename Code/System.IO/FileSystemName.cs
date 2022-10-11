// Decompiled with JetBrains decompiler
// Type: System.IO.Enumeration.FileSystemName
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Text;


#nullable enable
namespace System.IO.Enumeration
{
  /// <summary>Provides methods for matching file system names.</summary>
  public static class FileSystemName
  {

    #nullable disable
    private static readonly char[] s_wildcardChars = new char[5]
    {
      '"',
      '<',
      '>',
      '*',
      '?'
    };
    private static readonly char[] s_simpleWildcardChars = new char[2]
    {
      '*',
      '?'
    };


    #nullable enable
    /// <summary>Translates the given Win32 expression. Change '*' and '?' to '&lt;', '&gt;' and '"' to match Win32 behavior.</summary>
    /// <param name="expression">The expression to translate.</param>
    /// <returns>A string with the translated Win32 expression.</returns>
    public static unsafe string TranslateWin32Expression(string? expression)
    {
      if (string.IsNullOrEmpty(expression) || expression == "*" || expression == "*.*")
        return "*";
      bool flag = false;
      // ISSUE: untyped stack allocation
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(new Span<char>((void*) __untypedstackalloc(new IntPtr(64)), 32));
      int length = expression.Length;
      for (int index = 0; index < length; ++index)
      {
        char c = expression[index];
        switch (c)
        {
          case '.':
            flag = true;
            if (index >= 1 && index == length - 1 && expression[index - 1] == '*')
            {
              valueStringBuilder[valueStringBuilder.Length - 1] = '<';
              break;
            }
            if (index < length - 1 && (expression[index + 1] == '?' || expression[index + 1] == '*'))
            {
              valueStringBuilder.Append('"');
              break;
            }
            valueStringBuilder.Append('.');
            break;
          case '?':
            flag = true;
            valueStringBuilder.Append('>');
            break;
          default:
            valueStringBuilder.Append(c);
            break;
        }
      }
      return !flag ? expression : valueStringBuilder.ToString();
    }

    /// <summary>Verifies if the given Win32 expression matches the given name. Supports the following wildcards: '*', '?', '&lt;', '&gt;', '"'. The backslash character '\' escapes.</summary>
    /// <param name="expression">The expression to match with, such as "*.foo".</param>
    /// <param name="name">The name to check against the expression.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore case (default), <see langword="false" /> if the match should be case-sensitive.</param>
    /// <returns>
    /// <see langword="true" /> if the given expression matches the given name; otherwise, <see langword="false" />.</returns>
    public static bool MatchesWin32Expression(
      ReadOnlySpan<char> expression,
      ReadOnlySpan<char> name,
      bool ignoreCase = true)
    {
      return FileSystemName.MatchPattern(expression, name, ignoreCase, true);
    }

    /// <summary>Verifies if the given expression matches the given name. Supports the following wildcards: '*' and '?'. The backslash character '\' escapes.</summary>
    /// <param name="expression">The expression to match with.</param>
    /// <param name="name">The name to check against the expression.</param>
    /// <param name="ignoreCase">
    /// <see langword="true" /> to ignore case (default); <see langword="false" /> if the match should be case-sensitive.</param>
    /// <returns>
    /// <see langword="true" /> if the given expression matches the given name; otherwise, <see langword="false" />.</returns>
    public static bool MatchesSimpleExpression(
      ReadOnlySpan<char> expression,
      ReadOnlySpan<char> name,
      bool ignoreCase = true)
    {
      return FileSystemName.MatchPattern(expression, name, ignoreCase, false);
    }


    #nullable disable
    private static unsafe bool MatchPattern(
      ReadOnlySpan<char> expression,
      ReadOnlySpan<char> name,
      bool ignoreCase,
      bool useExtendedWildcards)
    {
      if (expression.Length == 0 || name.Length == 0)
        return false;
      if (expression[0] == '*')
      {
        if (expression.Length == 1)
          return true;
        ReadOnlySpan<char> span = expression.Slice(1);
        if (span.IndexOfAny<char>((ReadOnlySpan<char>) (useExtendedWildcards ? FileSystemName.s_wildcardChars : FileSystemName.s_simpleWildcardChars)) == -1)
          return name.Length >= span.Length && name.EndsWith(span, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
      }
      int num1 = 0;
      int num2 = 1;
      char c1 = char.MinValue;
      Span<int> span1 = new Span<int>();
      // ISSUE: untyped stack allocation
      Span<int> span2 = new Span<int>((void*) __untypedstackalloc(new IntPtr(64)), 16);
      // ISSUE: untyped stack allocation
      Span<int> span3 = new Span<int>((void*) __untypedstackalloc(new IntPtr(64)), 16);
      span3[0] = 0;
      int num3 = expression.Length * 2;
      bool flag1 = false;
      while (!flag1)
      {
        if (num1 < name.Length)
          c1 = name[num1++];
        else if (span3[num2 - 1] != num3)
          flag1 = true;
        else
          break;
        int index1 = 0;
        int num4 = 0;
        int index2 = 0;
        while (index1 < num2)
        {
          int index3 = (span3[index1++] + 1) / 2;
          while (index3 < expression.Length)
          {
            int num5 = index3 * 2;
            char c2 = expression[index3];
            if (num4 >= span2.Length - 2)
            {
              int length = span2.Length * 2;
              Span<int> destination1 = (Span<int>) new int[length];
              span2.CopyTo(destination1);
              span2 = destination1;
              Span<int> destination2 = (Span<int>) new int[length];
              span3.CopyTo(destination2);
              span3 = destination2;
            }
            if (c2 != '*')
            {
              if (useExtendedWildcards && c2 == '<')
              {
                bool flag2 = false;
                if (!flag1 && c1 == '.')
                {
                  for (int index4 = num1; index4 < name.Length; ++index4)
                  {
                    if (name[index4] == '.')
                    {
                      flag2 = true;
                      break;
                    }
                  }
                }
                if (((flag1 ? 1 : (c1 != '.' ? 1 : 0)) | (flag2 ? 1 : 0)) == 0)
                  goto label_44;
              }
              else
              {
                int num6 = num5 + 2;
                if (useExtendedWildcards && c2 == '>')
                {
                  if (!flag1 && c1 != '.')
                  {
                    span2[num4++] = num6;
                    break;
                  }
                  goto label_45;
                }
                else if (useExtendedWildcards && c2 == '"')
                {
                  if (!flag1)
                  {
                    if (c1 == '.')
                    {
                      span2[num4++] = num6;
                      break;
                    }
                    break;
                  }
                  goto label_45;
                }
                else
                {
                  if (c2 == '\\')
                  {
                    int index5;
                    if ((index5 = index3 + 1) == expression.Length)
                    {
                      span2[num4++] = num3;
                      break;
                    }
                    num6 = index5 * 2 + 2;
                    c2 = expression[index5];
                  }
                  if (!flag1)
                  {
                    if (c2 == '?')
                    {
                      span2[num4++] = num6;
                      break;
                    }
                    if ((ignoreCase ? ((int) char.ToUpperInvariant(c2) == (int) char.ToUpperInvariant(c1) ? 1 : 0) : ((int) c2 == (int) c1 ? 1 : 0)) != 0)
                    {
                      span2[num4++] = num6;
                      break;
                    }
                    break;
                  }
                  break;
                }
              }
            }
            span2[num4++] = num5;
label_44:
            span2[num4++] = num5 + 1;
label_45:
            if (++index3 == expression.Length)
              span2[num4++] = num3;
          }
          if (index1 < num2 && index2 < num4)
          {
            for (; index2 < num4; ++index2)
            {
              int length = span3.Length;
              while (index1 < length && span3[index1] < span2[index2])
                ++index1;
            }
          }
        }
        if (num4 == 0)
          return false;
        Span<int> span4 = span3;
        span3 = span2;
        span2 = span4;
        num2 = num4;
      }
      return span3[num2 - 1] == num3;
    }
  }
}
