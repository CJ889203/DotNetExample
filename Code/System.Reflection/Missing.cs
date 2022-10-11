// Decompiled with JetBrains decompiler
// Type: System.Reflection.Missing
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents a missing <see cref="T:System.Object" />. This class cannot be inherited.</summary>
  public sealed class Missing : ISerializable
  {
    /// <summary>Represents the sole instance of the <see cref="T:System.Reflection.Missing" /> class.</summary>
    public static readonly Missing Value = new Missing();

    private Missing()
    {
    }


    #nullable disable
    /// <summary>Sets a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the logical context information needed to recreate the sole instance of the <see cref="T:System.Reflection.Missing" /> object.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to be populated with serialization information.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> object representing the destination context of the serialization.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();
  }
}
