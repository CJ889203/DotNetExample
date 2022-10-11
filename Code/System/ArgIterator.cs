// Decompiled with JetBrains decompiler
// Type: System.ArgIterator
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Runtime.CompilerServices;


#nullable enable
namespace System
{
  /// <summary>Represents a variable-length argument list; that is, the parameters of a function that takes a variable number of arguments.</summary>
  [IsByRefLike]
  public struct ArgIterator
  {
    private IntPtr ArgCookie;
    private IntPtr sigPtr;
    private IntPtr sigPtrLen;
    private IntPtr ArgPtr;
    private int RemainingArgs;

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern ArgIterator(IntPtr arglist);

    /// <summary>Initializes a new instance of the <see cref="T:System.ArgIterator" /> structure using the specified argument list.</summary>
    /// <param name="arglist">An argument list consisting of mandatory and optional arguments.</param>
    public ArgIterator(RuntimeArgumentHandle arglist)
      : this(arglist.Value)
    {
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern unsafe ArgIterator(IntPtr arglist, void* ptr);


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.ArgIterator" /> structure using the specified argument list and a pointer to an item in the list.</summary>
    /// <param name="arglist">An argument list consisting of mandatory and optional arguments.</param>
    /// <param name="ptr">A pointer to the argument in <paramref name="arglist" /> to access first, or the first mandatory argument in <paramref name="arglist" /> if <paramref name="ptr" /> is <see langword="null" />.</param>
    [CLSCompliant(false)]
    public unsafe ArgIterator(RuntimeArgumentHandle arglist, void* ptr)
      : this(arglist.Value, ptr)
    {
    }

    /// <summary>Returns the next argument in a variable-length argument list.</summary>
    /// <exception cref="T:System.InvalidOperationException">An attempt was made to read beyond the end of the list.</exception>
    /// <returns>The next argument as a <see cref="T:System.TypedReference" /> object.</returns>
    [CLSCompliant(false)]
    public unsafe TypedReference GetNextArg()
    {
      TypedReference nextArg = new TypedReference();
      this.FCallGetNextArg((void*) &nextArg);
      return nextArg;
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern unsafe void FCallGetNextArg(void* result);

    /// <summary>Returns the next argument in a variable-length argument list that has a specified type.</summary>
    /// <param name="rth">A runtime type handle that identifies the type of the argument to retrieve.</param>
    /// <exception cref="T:System.InvalidOperationException">An attempt was made to read beyond the end of the list.</exception>
    /// <exception cref="T:System.ArgumentNullException">The pointer to the remaining arguments is zero.</exception>
    /// <returns>The next argument as a <see cref="T:System.TypedReference" /> object.</returns>
    [CLSCompliant(false)]
    public unsafe TypedReference GetNextArg(RuntimeTypeHandle rth)
    {
      if (this.sigPtr != IntPtr.Zero)
        return this.GetNextArg();
      if (this.ArgPtr == IntPtr.Zero)
        throw new ArgumentNullException();
      TypedReference nextArg = new TypedReference();
      this.InternalGetNextArg((void*) &nextArg, rth.GetRuntimeType());
      return nextArg;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern unsafe void InternalGetNextArg(void* result, RuntimeType rt);

    /// <summary>Concludes processing of the variable-length argument list represented by this instance.</summary>
    public void End()
    {
    }

    /// <summary>Returns the number of arguments remaining in the argument list.</summary>
    /// <returns>The number of remaining arguments.</returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern int GetRemainingCount();

    [MethodImpl(MethodImplOptions.InternalCall)]
    private extern unsafe void* _GetNextArgType();

    /// <summary>Returns the type of the next argument.</summary>
    /// <returns>The type of the next argument.</returns>
    public unsafe RuntimeTypeHandle GetNextArgType() => new RuntimeTypeHandle(Type.GetTypeFromHandleUnsafe((IntPtr) this._GetNextArgType()));

    /// <summary>Returns the hash code of this object.</summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => ValueType.GetHashCodeOfPtr(this.ArgCookie);


    #nullable enable
    /// <summary>This method is not supported, and always throws <see cref="T:System.NotSupportedException" />.</summary>
    /// <param name="o">An object to be compared to this instance.</param>
    /// <exception cref="T:System.NotSupportedException">This method is not supported.</exception>
    /// <returns>This comparison is not supported. No value is returned.</returns>
    public override bool Equals(object? o) => throw new NotSupportedException(SR.NotSupported_NYI);
  }
}
