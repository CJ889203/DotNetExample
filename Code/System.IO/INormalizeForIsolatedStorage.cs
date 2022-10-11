// Decompiled with JetBrains decompiler
// Type: System.IO.IsolatedStorage.INormalizeForIsolatedStorage
// Assembly: System.IO.IsolatedStorage, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 87FE0B2F-0A44-4572-BEFC-C86F7165516A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.IO.IsolatedStorage.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.IO.IsolatedStorage.xml


#nullable enable
namespace System.IO.IsolatedStorage
{
  /// <summary>Enables comparisons between an isolated store and an application domain and assembly's evidence.</summary>
  public interface INormalizeForIsolatedStorage
  {
    /// <summary>When overridden in a derived class, returns a normalized copy of the object on which it is called.</summary>
    /// <returns>A normalized object that represents the instance on which this method was called. This instance can be a string, stream, or any serializable object.</returns>
    object Normalize();
  }
}
