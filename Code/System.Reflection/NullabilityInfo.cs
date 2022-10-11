// Decompiled with JetBrains decompiler
// Type: System.Reflection.NullabilityInfo
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents nullability information.</summary>
  public sealed class NullabilityInfo
  {

    #nullable disable
    internal NullabilityInfo(
      Type type,
      NullabilityState readState,
      NullabilityState writeState,
      NullabilityInfo elementType,
      NullabilityInfo[] typeArguments)
    {
      this.Type = type;
      this.ReadState = readState;
      this.WriteState = writeState;
      this.ElementType = elementType;
      this.GenericTypeArguments = typeArguments;
    }


    #nullable enable
    /// <summary>Gets the type of the member or generic parameter to which this instance belongs.</summary>
    public Type Type { get; }

    /// <summary>Gets the nullability read state of the member.</summary>
    public NullabilityState ReadState { get; internal set; }

    /// <summary>Gets the nullability write state of the member.</summary>
    public NullabilityState WriteState { get; internal set; }

    /// <summary>Gets the nullability information for the element type of the array.</summary>
    /// <returns>If the member type is an array, the <see cref="T:System.Reflection.NullabilityInfo" /> of the elements of the array; otherwise, <see langword="null" />.</returns>
    public NullabilityInfo? ElementType { get; }

    /// <summary>Gets the nullability information for each type parameter.</summary>
    /// <returns>If the member type is a generic type, the nullability information for each type parameter.</returns>
    public NullabilityInfo[] GenericTypeArguments { get; }
  }
}
