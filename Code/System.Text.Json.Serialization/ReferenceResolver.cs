// Decompiled with JetBrains decompiler
// Type: System.Text.Json.Serialization.ReferenceResolver
// Assembly: System.Text.Json, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: 95FA77DB-1952-4283-B443-2F7779E6C063
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Text.Json.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Text.Json.xml


#nullable enable
namespace System.Text.Json.Serialization
{
  /// <summary>This class defines how the <see cref="T:System.Text.Json.JsonSerializer" /> deals with references on serialization and deserialization.
  /// Defines the core behavior of preserving references on serialization and deserialization.</summary>
  public abstract class ReferenceResolver
  {
    /// <summary>Adds an entry to the bag of references using the specified id and value.
    /// This method gets called when an $id metadata property from a JSON object is read.</summary>
    /// <param name="referenceId">The identifier of the JSON object or array.</param>
    /// <param name="value">The value of the CLR reference type object that results from parsing the JSON object.</param>
    public abstract void AddReference(string referenceId, object value);

    /// <summary>Gets the reference identifier of the specified value if exists; otherwise a new id is assigned.
    /// This method gets called before a CLR object is written so we can decide whether to write $id and enumerate the rest of its properties or $ref and step into the next object.</summary>
    /// <param name="value">The value of the CLR reference type object to get an id for.</param>
    /// <param name="alreadyExists">When this method returns, <see langword="true" /> if a reference to value already exists; otherwise, <see langword="false" />.</param>
    /// <returns>The reference id for the specified object.</returns>
    public abstract string GetReference(object value, out bool alreadyExists);

    /// <summary>Returns the CLR reference type object related to the specified reference id.
    /// This method gets called when $ref metadata property is read.</summary>
    /// <param name="referenceId">The reference id related to the returned object.</param>
    /// <returns>The reference type object related to the specified reference id.</returns>
    public abstract object ResolveReference(string referenceId);

    internal virtual void PopReferenceForCycleDetection() => throw new InvalidOperationException();


    #nullable disable
    internal virtual void PushReferenceForCycleDetection(object value) => throw new InvalidOperationException();

    internal virtual bool ContainsReferenceForCycleDetection(object value) => throw new InvalidOperationException();
  }
}
