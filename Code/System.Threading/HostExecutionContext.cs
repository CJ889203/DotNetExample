// Decompiled with JetBrains decompiler
// Type: System.Threading.HostExecutionContext
// Assembly: System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3EED92AD-A1EE-4F59-AFCF-58DB2345788A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml


#nullable enable
namespace System.Threading
{
  /// <summary>Encapsulates and propagates the host execution context across threads.</summary>
  public class HostExecutionContext : IDisposable
  {
    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.HostExecutionContext" /> class.</summary>
    public HostExecutionContext()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Threading.HostExecutionContext" /> class using the specified state.</summary>
    /// <param name="state">An object representing the host execution context state.</param>
    public HostExecutionContext(object? state) => this.State = state;

    /// <summary>Gets or sets the state of the host execution context.</summary>
    /// <returns>An object representing the host execution context state.</returns>
    protected internal object? State { get; set; }

    /// <summary>Creates a copy of the current host execution context.</summary>
    /// <returns>A <see cref="T:System.Threading.HostExecutionContext" /> object representing the host context for the current thread.</returns>
    public virtual HostExecutionContext CreateCopy() => new HostExecutionContext(this.State);

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.HostExecutionContext" /> class.</summary>
    public void Dispose() => this.Dispose(true);

    /// <summary>When overridden in a derived class, releases the unmanaged resources used by the <see cref="T:System.Threading.WaitHandle" />, and optionally releases the managed resources.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    public virtual void Dispose(bool disposing)
    {
    }
  }
}
