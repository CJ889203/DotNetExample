// Decompiled with JetBrains decompiler
// Type: System.IO.SearchOption
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.IO
{
  /// <summary>Specifies whether to search the current directory, or the current directory and all subdirectories.</summary>
  public enum SearchOption
  {
    /// <summary>Includes only the current directory in a search operation.</summary>
    TopDirectoryOnly,
    /// <summary>Includes the current directory and all its subdirectories in a search operation. This option includes reparse points such as mounted drives and symbolic links in the search.</summary>
    AllDirectories,
  }
}
