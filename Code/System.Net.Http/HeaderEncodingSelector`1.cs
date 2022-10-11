// Decompiled with JetBrains decompiler
// Type: System.Net.Http.HeaderEncodingSelector`1
// Assembly: System.Net.Http, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: D3111F3C-D07D-4FFA-B4EC-BDA891CAE931
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Net.Http.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Net.Http.xml

using System.Text;


#nullable enable
namespace System.Net.Http
{
  /// <summary>Represents a method that specifies the encoding to use when interpreting header values.</summary>
  /// <param name="headerName">The name of the header to specify for the encoding.</param>
  /// <param name="context">The type we are encoding/decoding the headers for.</param>
  /// <typeparam name="TContext">The type of the headers that are being encoded/decoded.</typeparam>
  /// <returns>The encoding to use, or <see langword="null" /> to use the default behavior.</returns>
  public delegate Encoding? HeaderEncodingSelector<TContext>(
    string headerName,
    TContext context);
}
