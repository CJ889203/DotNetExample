// Decompiled with JetBrains decompiler
// Type: System.Lazy`2
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Provides a lazy indirect reference to an object and its associated metadata for use by the Managed Extensibility Framework.</summary>
  /// <typeparam name="T">The type of the object referenced.</typeparam>
  /// <typeparam name="TMetadata">The type of the metadata.</typeparam>
  public class Lazy<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T, TMetadata> : 
    Lazy<T>
  {

    #nullable disable
    private readonly TMetadata _metadata;


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`2" /> class with the specified metadata that uses the specified function to get the referenced object.</summary>
    /// <param name="valueFactory">A function that returns the referenced object.</param>
    /// <param name="metadata">The metadata associated with the referenced object.</param>
    public Lazy(Func<T> valueFactory, TMetadata metadata)
      : base(valueFactory)
    {
      this._metadata = metadata;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`2" /> class with the specified metadata.</summary>
    /// <param name="metadata">The metadata associated with the referenced object.</param>
    public Lazy(TMetadata metadata) => this._metadata = metadata;

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`2" /> class with the specified metadata and thread safety value.</summary>
    /// <param name="metadata">The metadata associated with the referenced object.</param>
    /// <param name="isThreadSafe">Indicates whether the <see cref="T:System.Lazy`2" /> object that is created will be thread-safe.</param>
    public Lazy(TMetadata metadata, bool isThreadSafe)
      : base(isThreadSafe)
    {
      this._metadata = metadata;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`2" /> class with the specified metadata and thread safety value that uses the specified function to get the referenced object.</summary>
    /// <param name="valueFactory">A function that returns the referenced object.</param>
    /// <param name="metadata">The metadata associated with the referenced object.</param>
    /// <param name="isThreadSafe">Indicates whether the <see cref="T:System.Lazy`2" /> object that is created will be thread-safe.</param>
    public Lazy(Func<T> valueFactory, TMetadata metadata, bool isThreadSafe)
      : base(valueFactory, isThreadSafe)
    {
      this._metadata = metadata;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`2" /> class with the specified metadata and thread synchronization mode.</summary>
    /// <param name="metadata">The metadata associated with the referenced object.</param>
    /// <param name="mode">The thread synchronization mode.</param>
    public Lazy(TMetadata metadata, LazyThreadSafetyMode mode)
      : base(mode)
    {
      this._metadata = metadata;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Lazy`2" /> class with the specified metadata and thread synchronization mode that uses the specified function to get the referenced object.</summary>
    /// <param name="valueFactory">A function that returns the referenced object.</param>
    /// <param name="metadata">The metadata associated with the referenced object.</param>
    /// <param name="mode">The thread synchronization mode.</param>
    public Lazy(Func<T> valueFactory, TMetadata metadata, LazyThreadSafetyMode mode)
      : base(valueFactory, mode)
    {
      this._metadata = metadata;
    }

    /// <summary>Gets the metadata associated with the referenced object.</summary>
    /// <returns>The metadata associated with the referenced object.</returns>
    public TMetadata Metadata => this._metadata;
  }
}
