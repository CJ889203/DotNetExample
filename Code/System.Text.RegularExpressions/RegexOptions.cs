// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.RegexOptions
// Assembly: System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 9E234CFB-607D-4CAE-9C21-1A71C799D034
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.RegularExpressions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.RegularExpressions.xml

namespace System.Text.RegularExpressions
{
  /// <summary>Provides enumerated values to use to set regular expression options.</summary>
  [Flags]
  public enum RegexOptions
  {
    /// <summary>Specifies that no options are set. For more information about the default behavior of the regular expression engine, see the "Default Options" section in the Regular Expression Options article.</summary>
    None = 0,
    /// <summary>Specifies case-insensitive matching. For more information, see the "Case-Insensitive Matching " section in the Regular Expression Options article.</summary>
    IgnoreCase = 1,
    /// <summary>Multiline mode. Changes the meaning of ^ and $ so they match at the beginning and end, respectively, of any line, and not just the beginning and end of the entire string. For more information, see the "Multiline Mode" section in the Regular Expression Options article.</summary>
    Multiline = 2,
    /// <summary>Specifies that the only valid captures are explicitly named or numbered groups of the form (?&lt;name&gt;...). This allows unnamed parentheses to act as noncapturing groups without the syntactic clumsiness of the expression (?:...). For more information, see the "Explicit Captures Only" section in the Regular Expression Options article.</summary>
    ExplicitCapture = 4,
    /// <summary>Specifies that the regular expression is compiled to MSIL code, instead of being interpreted. Compiled regular expressions maximize run-time performance at the expense of initialization time. This value should not be assigned to the <see cref="P:System.Text.RegularExpressions.RegexCompilationInfo.Options" /> property when calling the <see cref="M:System.Text.RegularExpressions.Regex.CompileToAssembly(System.Text.RegularExpressions.RegexCompilationInfo[],System.Reflection.AssemblyName)" /> method. For more information, see the "Compiled Regular Expressions" section in the Regular Expression Options article.</summary>
    Compiled = 8,
    /// <summary>Specifies single-line mode. Changes the meaning of the dot (.) so it matches every character (instead of every character except \n). For more information, see the "Single-line Mode" section in the Regular Expression Options article.</summary>
    Singleline = 16, // 0x00000010
    /// <summary>Eliminates unescaped white space from the pattern and enables comments marked with #. However, this value does not affect or eliminate white space in character classes, numeric quantifiers, or tokens that mark the beginning of individual regular expression language elements. For more information, see the "Ignore White Space" section of the Regular Expression Options article.</summary>
    IgnorePatternWhitespace = 32, // 0x00000020
    /// <summary>Specifies that the search will be from right to left instead of from left to right. For more information, see the "Right-to-Left Mode" section in the Regular Expression Options article.</summary>
    RightToLeft = 64, // 0x00000040
    /// <summary>Enables ECMAScript-compliant behavior for the expression. This value can be used only in conjunction with the <see cref="F:System.Text.RegularExpressions.RegexOptions.IgnoreCase" />, <see cref="F:System.Text.RegularExpressions.RegexOptions.Multiline" />, and <see cref="F:System.Text.RegularExpressions.RegexOptions.Compiled" /> values. The use of this value with any other values results in an exception.
    /// 
    /// For more information on the <see cref="F:System.Text.RegularExpressions.RegexOptions.ECMAScript" /> option, see the "ECMAScript Matching Behavior" section in the Regular Expression Options article.</summary>
    ECMAScript = 256, // 0x00000100
    /// <summary>Specifies that cultural differences in language is ignored. For more information, see the "Comparison Using the Invariant Culture" section in the Regular Expression Options article.</summary>
    CultureInvariant = 512, // 0x00000200
  }
}
