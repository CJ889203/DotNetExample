// Decompiled with JetBrains decompiler
// Type: System.Threading.Volatile
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using Internal.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;


#nullable enable
namespace System.Threading
{
  /// <summary>Contains methods for performing volatile memory operations.</summary>
  public static class Volatile
  {
    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [Intrinsic]
    [NonVersionable]
    public static bool Read(ref bool location) => Unsafe.As<bool, Volatile.VolatileBoolean>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref bool location, bool value) => Unsafe.As<bool, Volatile.VolatileBoolean>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [Intrinsic]
    [NonVersionable]
    public static byte Read(ref byte location) => Unsafe.As<byte, Volatile.VolatileByte>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref byte location, byte value) => Unsafe.As<byte, Volatile.VolatileByte>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [Intrinsic]
    [NonVersionable]
    public static unsafe double Read(ref double location) => *(double*) &Volatile.Read(ref Unsafe.As<double, long>(ref location));

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [Intrinsic]
    [NonVersionable]
    public static unsafe void Write(ref double location, double value) => Volatile.Write(ref Unsafe.As<double, long>(ref location), *(long*) &value);

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [Intrinsic]
    [NonVersionable]
    public static short Read(ref short location) => Unsafe.As<short, Volatile.VolatileInt16>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref short location, short value) => Unsafe.As<short, Volatile.VolatileInt16>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [Intrinsic]
    [NonVersionable]
    public static int Read(ref int location) => Unsafe.As<int, Volatile.VolatileInt32>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref int location, int value) => Unsafe.As<int, Volatile.VolatileInt32>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [Intrinsic]
    [NonVersionable]
    public static long Read(ref long location) => (long) Unsafe.As<long, Volatile.VolatileIntPtr>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref long location, long value) => Unsafe.As<long, Volatile.VolatileIntPtr>(ref location).Value = (IntPtr) value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [Intrinsic]
    [NonVersionable]
    public static IntPtr Read(ref IntPtr location) => Unsafe.As<IntPtr, Volatile.VolatileIntPtr>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref IntPtr location, IntPtr value) => Unsafe.As<IntPtr, Volatile.VolatileIntPtr>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static sbyte Read(ref sbyte location) => Unsafe.As<sbyte, Volatile.VolatileSByte>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref sbyte location, sbyte value) => Unsafe.As<sbyte, Volatile.VolatileSByte>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [Intrinsic]
    [NonVersionable]
    public static float Read(ref float location) => Unsafe.As<float, Volatile.VolatileSingle>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref float location, float value) => Unsafe.As<float, Volatile.VolatileSingle>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static ushort Read(ref ushort location) => Unsafe.As<ushort, Volatile.VolatileUInt16>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref ushort location, ushort value) => Unsafe.As<ushort, Volatile.VolatileUInt16>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static uint Read(ref uint location) => Unsafe.As<uint, Volatile.VolatileUInt32>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref uint location, uint value) => Unsafe.As<uint, Volatile.VolatileUInt32>(ref location).Value = value;

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static ulong Read(ref ulong location) => (ulong) Volatile.Read(ref Unsafe.As<ulong, long>(ref location));

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref ulong location, ulong value) => Volatile.Write(ref Unsafe.As<ulong, long>(ref location), (long) value);

    /// <summary>Reads the value of the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <returns>The value that was read.</returns>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static UIntPtr Read(ref UIntPtr location) => Unsafe.As<UIntPtr, Volatile.VolatileUIntPtr>(ref location).Value;

    /// <summary>Writes the specified value to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the value is written.</param>
    /// <param name="value">The value to write.</param>
    [CLSCompliant(false)]
    [Intrinsic]
    [NonVersionable]
    public static void Write(ref UIntPtr location, UIntPtr value) => Unsafe.As<UIntPtr, Volatile.VolatileUIntPtr>(ref location).Value = value;

    /// <summary>Reads the object reference from the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears after this method in the code, the processor cannot move it before this method.</summary>
    /// <param name="location">The field to read.</param>
    /// <typeparam name="T">The type of field to read. This must be a reference type, not a value type.</typeparam>
    /// <returns>The reference to <paramref name="T" /> that was read. This reference is the latest written by any processor in the computer, regardless of the number of processors or the state of processor cache.</returns>
    [Intrinsic]
    [NonVersionable]
    [return: NotNullIfNotNull("location")]
    public static T Read<T>([NotNullIfNotNull("location")] ref T location) where T : class? => Unsafe.As<T>(Unsafe.As<T, Volatile.VolatileObject>(ref location).Value);

    /// <summary>Writes the specified object reference to the specified field. On systems that require it, inserts a memory barrier that prevents the processor from reordering memory operations as follows: If a read or write appears before this method in the code, the processor cannot move it after this method.</summary>
    /// <param name="location">The field where the object reference is written.</param>
    /// <param name="value">The object reference to write.</param>
    /// <typeparam name="T">The type of field to write. This must be a reference type, not a value type.</typeparam>
    [Intrinsic]
    [NonVersionable]
    public static void Write<T>([NotNullIfNotNull("value")] ref T location, T value) where T : class? => Unsafe.As<T, Volatile.VolatileObject>(ref location).Value = (object) value;


    #nullable disable
    private struct VolatileBoolean
    {
      public volatile bool Value;
    }

    private struct VolatileByte
    {
      public volatile byte Value;
    }

    private struct VolatileInt16
    {
      public volatile short Value;
    }

    private struct VolatileInt32
    {
      public volatile int Value;
    }

    private struct VolatileIntPtr
    {
      public volatile IntPtr Value;
    }

    private struct VolatileSByte
    {
      public volatile sbyte Value;
    }

    private struct VolatileSingle
    {
      public volatile float Value;
    }

    private struct VolatileUInt16
    {
      public volatile ushort Value;
    }

    private struct VolatileUInt32
    {
      public volatile uint Value;
    }

    private struct VolatileUIntPtr
    {
      public volatile UIntPtr Value;
    }

    private struct VolatileObject
    {
      public volatile object Value;
    }
  }
}
