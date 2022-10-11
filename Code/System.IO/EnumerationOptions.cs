// Decompiled with JetBrains decompiler
// Type: System.IO.EnumerationOptions
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.IO
{
  /// <summary>Provides file and directory enumeration options.</summary>
  public class EnumerationOptions
  {
    private int _maxRecursionDepth;

    internal static EnumerationOptions Compatible { get; } = new EnumerationOptions()
    {
      MatchType = MatchType.Win32,
      AttributesToSkip = (FileAttributes) 0,
      IgnoreInaccessible = false
    };

    private static EnumerationOptions CompatibleRecursive { get; } = new EnumerationOptions()
    {
      RecurseSubdirectories = true,
      MatchType = MatchType.Win32,
      AttributesToSkip = (FileAttributes) 0,
      IgnoreInaccessible = false
    };

    internal static EnumerationOptions Default { get; } = new EnumerationOptions();

    /// <summary>Initializes a new instance of the <see cref="T:System.IO.EnumerationOptions" /> class with the recommended default options.</summary>
    public EnumerationOptions()
    {
      this.IgnoreInaccessible = true;
      this.AttributesToSkip = FileAttributes.Hidden | FileAttributes.System;
      this.MaxRecursionDepth = int.MaxValue;
    }


    #nullable disable
    internal static EnumerationOptions FromSearchOption(SearchOption searchOption)
    {
      if (searchOption != SearchOption.TopDirectoryOnly && searchOption != SearchOption.AllDirectories)
        throw new ArgumentOutOfRangeException(nameof (searchOption), SR.ArgumentOutOfRange_Enum);
      return searchOption != SearchOption.AllDirectories ? EnumerationOptions.Compatible : EnumerationOptions.CompatibleRecursive;
    }

    /// <summary>Gets or sets a value that indicates whether to recurse into subdirectories while enumerating. The default is <see langword="false" />.</summary>
    /// <returns>
    /// <see langword="true" /> to recurse into subdirectories; otherwise, <see langword="false" />.</returns>
    public bool RecurseSubdirectories { get; set; }

    /// <summary>Gets or sets a value that indicates whether to skip files or directories when access is denied (for example, <see cref="T:System.UnauthorizedAccessException" /> or <see cref="T:System.Security.SecurityException" />). The default is <see langword="true" />.</summary>
    /// <returns>
    /// <see langword="true" /> to skip innacessible files or directories; otherwise, <see langword="false" />.</returns>
    public bool IgnoreInaccessible { get; set; }

    /// <summary>Gets or sets the suggested buffer size, in bytes. The default is 0 (no suggestion).</summary>
    /// <returns>The buffer size.</returns>
    public int BufferSize { get; set; }

    /// <summary>Gets or sets the attributes to skip. The default is <c>FileAttributes.Hidden | FileAttributes.System</c>.</summary>
    /// <returns>The attributes to skip.</returns>
    public FileAttributes AttributesToSkip { get; set; }

    /// <summary>Gets or sets the match type.</summary>
    /// <returns>One of the enumeration values that indicates the match type.</returns>
    public MatchType MatchType { get; set; }

    /// <summary>Gets or sets the case matching behavior.</summary>
    /// <returns>One of the enumeration values that indicates the case matching behavior.</returns>
    public MatchCasing MatchCasing { get; set; }

    /// <summary>Gets or sets a value that indicates the maximum directory depth to recurse while enumerating, when <see cref="P:System.IO.EnumerationOptions.RecurseSubdirectories" /> is set to <see langword="true" />.</summary>
    /// <returns>A number that represents the maximum directory depth to recurse while enumerating. The default value is <see cref="F:System.Int32.MaxValue" />.</returns>
    public int MaxRecursionDepth
    {
      get => this._maxRecursionDepth;
      set => this._maxRecursionDepth = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (value), SR.ArgumentOutOfRange_NeedNonNegNum);
    }

    /// <summary>Gets or sets a value that indicates whether to return the special directory entries "." and "..".</summary>
    /// <returns>
    /// <see langword="true" /> to return the special directory entries "." and ".."; otherwise, <see langword="false" />.</returns>
    public bool ReturnSpecialDirectories { get; set; }
  }
}
