// Decompiled with JetBrains decompiler
// Type: System.IO.HandleInheritability
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

namespace System.IO
{
  /// <summary>Specifies whether the underlying handle is inheritable by child processes.</summary>
  public enum HandleInheritability
  {
    /// <summary>Specifies that the handle is not inheritable by child processes.</summary>
    None,
    /// <summary>Specifies that the handle is inheritable by child processes.</summary>
    Inheritable,
  }
}
