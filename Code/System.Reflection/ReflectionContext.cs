// Decompiled with JetBrains decompiler
// Type: System.Reflection.ReflectionContext
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System.Reflection
{
  /// <summary>Represents a context that can provide reflection objects.</summary>
  public abstract class ReflectionContext
  {
    /// <summary>Gets the representation, in this reflection context, of an assembly that is represented by an object from another reflection context.</summary>
    /// <param name="assembly">The external representation of the assembly to represent in this context.</param>
    /// <returns>The representation of the assembly in this reflection context.</returns>
    public abstract Assembly MapAssembly(Assembly assembly);

    /// <summary>Gets the representation, in this reflection context, of a type represented by an object from another reflection context.</summary>
    /// <param name="type">The external representation of the type to represent in this context.</param>
    /// <returns>The representation of the type in this reflection context.</returns>
    public abstract TypeInfo MapType(TypeInfo type);

    /// <summary>Gets the representation of the type of the specified object in this reflection context.</summary>
    /// <param name="value">The object to represent.</param>
    /// <returns>An object that represents the type of the specified object.</returns>
    public virtual TypeInfo GetTypeForObject(object value) => value != null ? this.MapType(value.GetType().GetTypeInfo()) : throw new ArgumentNullException(nameof (value));
  }
}
