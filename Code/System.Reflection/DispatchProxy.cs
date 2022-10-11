// Decompiled with JetBrains decompiler
// Type: System.Reflection.DispatchProxy
// Assembly: System.Reflection.DispatchProxy, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: AB39AD1E-790B-413E-8D9B-E956F9981F58
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.DispatchProxy.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.DispatchProxy.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides a mechanism for instantiating proxy objects and handling their method dispatch.</summary>
  public abstract class DispatchProxy
  {
    /// <summary>Whenever any method on the generated proxy type is called, this method is invoked to dispatch control.</summary>
    /// <param name="targetMethod">The method the caller invoked.</param>
    /// <param name="args">The arguments the caller passed to the method.</param>
    /// <returns>The object to return to the caller, or <see langword="null" /> for void methods.</returns>
    protected abstract object? Invoke(MethodInfo? targetMethod, object?[]? args);

    /// <summary>Creates an object instance that derives from class <typeparamref name="TProxy" /> and implements interface <typeparamref name="T" />.</summary>
    /// <typeparam name="T">The interface the proxy should implement.</typeparam>
    /// <typeparam name="TProxy">The base class to use for the proxy class.</typeparam>
    /// <exception cref="T:System.ArgumentException">
    /// <typeparamref name="T" /> is a class, or <typeparamref name="TProxy" /> is sealed or does not have a parameterless constructor.</exception>
    /// <returns>An object instance that implements <typeparamref name="T" />.</returns>
    public static T Create<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TProxy>() where TProxy : DispatchProxy => (T) DispatchProxyGenerator.CreateProxyInstance(typeof (TProxy), typeof (T));
  }
}
