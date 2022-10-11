// Decompiled with JetBrains decompiler
// Type: System.Threading.LockCookie
// Assembly: System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 3EED92AD-A1EE-4F59-AFCF-58DB2345788A
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Threading.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics.CodeAnalysis;


#nullable enable
namespace System.Threading
{
  /// <summary>Defines the lock that implements single-writer/multiple-reader semantics. This is a value type.</summary>
  public struct LockCookie
  {
    internal LockCookieFlags _flags;
    internal ushort _readerLevel;
    internal ushort _writerLevel;
    internal int _threadID;

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => (int) (this._flags + (int) this._readerLevel + (int) this._writerLevel + this._threadID);

    /// <summary>Indicates whether a specified object is a <see cref="T:System.Threading.LockCookie" /> and is equal to the current instance.</summary>
    /// <param name="obj">The object to compare to the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if the value of <paramref name="obj" /> is equal to the value of the current instance; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is LockCookie lockCookie && this.Equals(lockCookie);

    /// <summary>Indicates whether the current instance is equal to the specified <see cref="T:System.Threading.LockCookie" />.</summary>
    /// <param name="obj">The <see cref="T:System.Threading.LockCookie" /> to compare to the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> is equal to the value of the current instance; otherwise, <see langword="false" />.</returns>
    public bool Equals(LockCookie obj) => this._flags == obj._flags && (int) this._readerLevel == (int) obj._readerLevel && (int) this._writerLevel == (int) obj._writerLevel && this._threadID == obj._threadID;

    /// <summary>Indicates whether two <see cref="T:System.Threading.LockCookie" /> structures are equal.</summary>
    /// <param name="a">The <see cref="T:System.Threading.LockCookie" /> to compare to <paramref name="b" />.</param>
    /// <param name="b">The <see cref="T:System.Threading.LockCookie" /> to compare to <paramref name="a" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(LockCookie a, LockCookie b) => a.Equals(b);

    /// <summary>Indicates whether two <see cref="T:System.Threading.LockCookie" /> structures are not equal.</summary>
    /// <param name="a">The <see cref="T:System.Threading.LockCookie" /> to compare to <paramref name="b" />.</param>
    /// <param name="b">The <see cref="T:System.Threading.LockCookie" /> to compare to <paramref name="a" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(LockCookie a, LockCookie b) => !(a == b);
  }
}
