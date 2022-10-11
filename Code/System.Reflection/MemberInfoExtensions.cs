// Decompiled with JetBrains decompiler
// Type: System.Reflection.MemberInfoExtensions
// Assembly: System.Reflection.TypeExtensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 640AC10B-88E0-451A-B5D0-A4B0F7E22777
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.TypeExtensions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.TypeExtensions.xml


#nullable enable
namespace System.Reflection
{
  public static class MemberInfoExtensions
  {
    /// <summary>Returns a value that indicates whether a metadata token is available for the specified member.</summary>
    /// <param name="member">The member to analyze, as reftype.</param>
    /// <returns>
    /// <see langword="true" /> if there is a metadata token available for the given member; otherwise, <see langword="false" />.</returns>
    public static bool HasMetadataToken(this MemberInfo member)
    {
      ArgumentNullException.ThrowIfNull((object) member, nameof (member));
      try
      {
        return member.GetMetadataTokenOrZeroOrThrow() != 0;
      }
      catch (InvalidOperationException ex)
      {
        return false;
      }
    }

    /// <summary>Gets a metadata token for the given member, if available.</summary>
    /// <param name="member">The member from which to retrieve the token, as reftype.</param>
    /// <exception cref="T:System.InvalidOperationException">There is no metadata token available.</exception>
    /// <returns>An integer representing the metadata token. The returned token is never nil. If unavailable, an exception is thrown.</returns>
    public static int GetMetadataToken(this MemberInfo member)
    {
      ArgumentNullException.ThrowIfNull((object) member, nameof (member));
      int tokenOrZeroOrThrow = member.GetMetadataTokenOrZeroOrThrow();
      return tokenOrZeroOrThrow != 0 ? tokenOrZeroOrThrow : throw new InvalidOperationException(SR.NoMetadataTokenAvailable);
    }


    #nullable disable
    private static int GetMetadataTokenOrZeroOrThrow(this MemberInfo member)
    {
      int metadataToken = member.MetadataToken;
      return (metadataToken & 16777215) == 0 ? 0 : metadataToken;
    }
  }
}
