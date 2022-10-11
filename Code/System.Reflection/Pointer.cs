// Decompiled with JetBrains decompiler
// Type: System.Reflection.Pointer
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;


#nullable enable
namespace System.Reflection
{
  /// <summary>Provides a wrapper class for pointers.</summary>
  [CLSCompliant(false)]
  public sealed class Pointer : ISerializable
  {

    #nullable disable
    private readonly unsafe void* _ptr;
    private readonly Type _ptrType;

    private unsafe Pointer(void* ptr, Type ptrType)
    {
      this._ptr = ptr;
      this._ptrType = ptrType;
    }


    #nullable enable
    /// <summary>Boxes the supplied unmanaged memory pointer and the type associated with that pointer into a managed <see cref="T:System.Reflection.Pointer" /> wrapper object. The value and the type are saved so they can be accessed from the native code during an invocation.</summary>
    /// <param name="ptr">The supplied unmanaged memory pointer.</param>
    /// <param name="type">The type associated with the <paramref name="ptr" /> parameter.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="type" /> is not a pointer.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="type" /> is <see langword="null" />.</exception>
    /// <returns>A pointer object.</returns>
    public static unsafe object Box(void* ptr, Type type)
    {
      if (type == (Type) null)
        throw new ArgumentNullException(nameof (type));
      if (!type.IsPointer)
        throw new ArgumentException(SR.Arg_MustBePointer, nameof (ptr));
      return type.IsRuntimeImplemented() ? (object) new Pointer(ptr, type) : throw new ArgumentException(SR.Arg_MustBeType, nameof (ptr));
    }

    /// <summary>Returns the stored pointer.</summary>
    /// <param name="ptr">The stored pointer.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="ptr" /> is not a pointer.</exception>
    /// <returns>This method returns void.</returns>
    public static unsafe void* Unbox(object ptr) => ptr is Pointer ? ((Pointer) ptr)._ptr : throw new ArgumentException(SR.Arg_MustBePointer, nameof (ptr));

    /// <summary>Returns a value that indicates whether the current object is equal to a specified object.</summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>
    /// <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
    public override unsafe bool Equals([NotNullWhen(true)] object? obj) => obj is Pointer pointer && this._ptr == pointer._ptr;

    /// <summary>Returns the hash code for the current object.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override unsafe int GetHashCode() => ((UIntPtr) this._ptr).GetHashCode();


    #nullable disable
    /// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name, fusion log, and additional exception information.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    internal Type GetPointerType() => this._ptrType;

    internal unsafe IntPtr GetPointerValue() => (IntPtr) this._ptr;
  }
}
