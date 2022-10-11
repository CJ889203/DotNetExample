// Decompiled with JetBrains decompiler
// Type: System.Random
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using Internal.Runtime.CompilerServices;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#nullable enable
namespace System
{
  /// <summary>Represents a pseudo-random number generator, which is an algorithm that produces a sequence of numbers that meet certain statistical requirements for randomness.</summary>
  public class Random
  {

    #nullable disable
    private readonly Random.ImplBase _impl;

    /// <summary>Initializes a new instance of the <see cref="T:System.Random" /> class using a default seed value.</summary>
    public Random() => this._impl = this.GetType() == typeof (Random) ? (Random.ImplBase) new Random.XoshiroImpl() : (Random.ImplBase) new Random.Net5CompatDerivedImpl(this);

    /// <summary>Initializes a new instance of the <see cref="T:System.Random" /> class, using the specified seed value.</summary>
    /// <param name="Seed">A number used to calculate a starting value for the pseudo-random number sequence. If a negative number is specified, the absolute value of the number is used.</param>
    public Random(int Seed) => this._impl = this.GetType() == typeof (Random) ? (Random.ImplBase) new Random.Net5CompatSeedImpl(Seed) : (Random.ImplBase) new Random.Net5CompatDerivedImpl(this, Seed);

    private protected Random(bool isThreadSafeRandom) => this._impl = (Random.ImplBase) null;


    #nullable enable
    /// <summary>Provides a thread-safe <see cref="T:System.Random" /> instance that may be used concurrently from any thread.</summary>
    /// <returns>A <see cref="T:System.Random" /> instance.</returns>
    public static Random Shared { get; } = (Random) new Random.ThreadSafeRandom();

    /// <summary>Returns a non-negative random integer.</summary>
    /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="F:System.Int32.MaxValue" />.</returns>
    public virtual int Next() => this._impl.Next();

    /// <summary>Returns a non-negative random integer that is less than the specified maximum.</summary>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue" /> must be greater than or equal to 0.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="maxValue" /> is less than 0.</exception>
    /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue" />; that is, the range of return values ordinarily includes 0 but not <paramref name="maxValue" />. However, if <paramref name="maxValue" /> equals 0, <paramref name="maxValue" /> is returned.</returns>
    public virtual int Next(int maxValue)
    {
      if (maxValue < 0)
        Random.ThrowMaxValueMustBeNonNegative();
      return this._impl.Next(maxValue);
    }

    /// <summary>Returns a random integer that is within a specified range.</summary>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue" /> must be greater than or equal to <paramref name="minValue" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="minValue" /> is greater than <paramref name="maxValue" />.</exception>
    /// <returns>A 32-bit signed integer greater than or equal to <paramref name="minValue" /> and less than <paramref name="maxValue" />; that is, the range of return values includes <paramref name="minValue" /> but not <paramref name="maxValue" />. If <paramref name="minValue" /> equals <paramref name="maxValue" />, <paramref name="minValue" /> is returned.</returns>
    public virtual int Next(int minValue, int maxValue)
    {
      if (minValue > maxValue)
        Random.ThrowMinMaxValueSwapped();
      return this._impl.Next(minValue, maxValue);
    }

    /// <summary>Returns a non-negative random integer.</summary>
    /// <returns>A 64-bit signed integer that is greater than or equal to 0 and less than <see cref="F:System.Int64.MaxValue" />.</returns>
    public virtual long NextInt64() => this._impl.NextInt64();

    /// <summary>Returns a non-negative random integer that is less than the specified maximum.</summary>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue" /> must be greater than or equal to 0.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="maxValue" /> is less than 0.</exception>
    /// <returns>A 64-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue" />; that is, the range of return values ordinarily includes 0 but not <paramref name="maxValue" />. However, if <paramref name="maxValue" /> equals 0, <paramref name="maxValue" /> is returned.</returns>
    public virtual long NextInt64(long maxValue)
    {
      if (maxValue < 0L)
        Random.ThrowMaxValueMustBeNonNegative();
      return this._impl.NextInt64(maxValue);
    }

    /// <summary>Returns a random integer that is within a specified range.</summary>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue" /> must be greater than or equal to <paramref name="minValue" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="minValue" /> is greater than <paramref name="maxValue" />.</exception>
    /// <returns>A 64-bit signed integer greater than or equal to <paramref name="minValue" /> and less than <paramref name="maxValue" />; that is, the range of return values includes <paramref name="minValue" /> but not <paramref name="maxValue" />. If minValue equals <paramref name="maxValue" />, <paramref name="minValue" /> is returned.</returns>
    public virtual long NextInt64(long minValue, long maxValue)
    {
      if (minValue > maxValue)
        Random.ThrowMinMaxValueSwapped();
      return this._impl.NextInt64(minValue, maxValue);
    }

    /// <summary>Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.</summary>
    /// <returns>A single-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
    public virtual float NextSingle() => this._impl.NextSingle();

    /// <summary>Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.</summary>
    /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
    public virtual double NextDouble() => this._impl.NextDouble();

    /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
    /// <param name="buffer">The array to be filled with random numbers.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="buffer" /> is <see langword="null" />.</exception>
    public virtual void NextBytes(byte[] buffer)
    {
      if (buffer == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.buffer);
      this._impl.NextBytes(buffer);
    }

    /// <summary>Fills the elements of a specified span of bytes with random numbers.</summary>
    /// <param name="buffer">The array to be filled with random numbers.</param>
    public virtual void NextBytes(Span<byte> buffer) => this._impl.NextBytes(buffer);

    /// <summary>Returns a random floating-point number between 0.0 and 1.0.</summary>
    /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
    protected virtual double Sample() => this._impl.Sample();

    private static void ThrowMaxValueMustBeNonNegative() => throw new ArgumentOutOfRangeException("maxValue", SR.Format(SR.ArgumentOutOfRange_NeedNonNegNum, (object) "maxValue"));

    private static void ThrowMinMaxValueSwapped() => throw new ArgumentOutOfRangeException("minValue", SR.Format(SR.Argument_MinMaxValue, (object) "minValue", (object) "maxValue"));


    #nullable disable
    private sealed class ThreadSafeRandom : Random
    {
      [ThreadStatic]
      private static Random.XoshiroImpl t_random;

      public ThreadSafeRandom()
        : base(true)
      {
      }

      private static Random.XoshiroImpl LocalRandom => Random.ThreadSafeRandom.t_random ?? Random.ThreadSafeRandom.Create();

      [MethodImpl(MethodImplOptions.NoInlining)]
      private static Random.XoshiroImpl Create() => Random.ThreadSafeRandom.t_random = new Random.XoshiroImpl();

      public override int Next() => Random.ThreadSafeRandom.LocalRandom.Next();

      public override int Next(int maxValue)
      {
        if (maxValue < 0)
          Random.ThrowMaxValueMustBeNonNegative();
        return Random.ThreadSafeRandom.LocalRandom.Next(maxValue);
      }

      public override int Next(int minValue, int maxValue)
      {
        if (minValue > maxValue)
          Random.ThrowMinMaxValueSwapped();
        return Random.ThreadSafeRandom.LocalRandom.Next(minValue, maxValue);
      }

      public override long NextInt64() => Random.ThreadSafeRandom.LocalRandom.NextInt64();

      public override long NextInt64(long maxValue)
      {
        if (maxValue < 0L)
          Random.ThrowMaxValueMustBeNonNegative();
        return Random.ThreadSafeRandom.LocalRandom.NextInt64(maxValue);
      }

      public override long NextInt64(long minValue, long maxValue)
      {
        if (minValue > maxValue)
          Random.ThrowMinMaxValueSwapped();
        return Random.ThreadSafeRandom.LocalRandom.NextInt64(minValue, maxValue);
      }

      public override float NextSingle() => Random.ThreadSafeRandom.LocalRandom.NextSingle();

      public override double NextDouble() => Random.ThreadSafeRandom.LocalRandom.NextDouble();

      public override void NextBytes(byte[] buffer)
      {
        if (buffer == null)
          ThrowHelper.ThrowArgumentNullException(ExceptionArgument.buffer);
        Random.ThreadSafeRandom.LocalRandom.NextBytes(buffer);
      }

      public override void NextBytes(Span<byte> buffer) => Random.ThreadSafeRandom.LocalRandom.NextBytes(buffer);

      protected override double Sample() => throw new NotSupportedException();
    }

    internal abstract class ImplBase
    {
      public abstract double Sample();

      public abstract int Next();

      public abstract int Next(int maxValue);

      public abstract int Next(int minValue, int maxValue);

      public abstract long NextInt64();

      public abstract long NextInt64(long maxValue);

      public abstract long NextInt64(long minValue, long maxValue);

      public abstract float NextSingle();

      public abstract double NextDouble();

      public abstract void NextBytes(byte[] buffer);

      public abstract void NextBytes(Span<byte> buffer);
    }

    private sealed class Net5CompatSeedImpl : Random.ImplBase
    {
      private Random.CompatPrng _prng;

      public Net5CompatSeedImpl(int seed) => this._prng = new Random.CompatPrng(seed);

      public override double Sample() => this._prng.Sample();

      public override int Next() => this._prng.InternalSample();

      public override int Next(int maxValue) => (int) (this._prng.Sample() * (double) maxValue);

      public override int Next(int minValue, int maxValue)
      {
        long num = (long) maxValue - (long) minValue;
        return num > (long) int.MaxValue ? (int) ((long) (this._prng.GetSampleForLargeRange() * (double) num) + (long) minValue) : (int) (this._prng.Sample() * (double) num) + minValue;
      }

      public override long NextInt64()
      {
        ulong num;
        do
        {
          num = this.NextUInt64() >> 1;
        }
        while (num == (ulong) long.MaxValue);
        return (long) num;
      }

      public override long NextInt64(long maxValue) => this.NextInt64(0L, maxValue);

      public override long NextInt64(long minValue, long maxValue)
      {
        ulong num1 = (ulong) (maxValue - minValue);
        if (num1 <= 1UL)
          return minValue;
        int num2 = BitOperations.Log2Ceiling(num1);
        ulong num3;
        do
        {
          num3 = this.NextUInt64() >> 64 - num2;
        }
        while (num3 >= num1);
        return (long) num3 + minValue;
      }

      private ulong NextUInt64() => (ulong) ((long) (uint) this.Next(4194304) | (long) (uint) this.Next(4194304) << 22 | (long) (uint) this.Next(1048576) << 44);

      public override double NextDouble() => this._prng.Sample();

      public override float NextSingle() => (float) this._prng.Sample();

      public override void NextBytes(byte[] buffer) => this._prng.NextBytes((Span<byte>) buffer);

      public override void NextBytes(Span<byte> buffer) => this._prng.NextBytes(buffer);
    }

    private sealed class Net5CompatDerivedImpl : Random.ImplBase
    {
      private readonly Random _parent;
      private Random.CompatPrng _prng;

      public Net5CompatDerivedImpl(Random parent)
        : this(parent, Random.Shared.Next())
      {
      }

      public Net5CompatDerivedImpl(Random parent, int seed)
      {
        this._parent = parent;
        this._prng = new Random.CompatPrng(seed);
      }

      public override double Sample() => this._prng.Sample();

      public override int Next() => this._prng.InternalSample();

      public override int Next(int maxValue) => (int) (this._parent.Sample() * (double) maxValue);

      public override int Next(int minValue, int maxValue)
      {
        long num = (long) maxValue - (long) minValue;
        return num > (long) int.MaxValue ? (int) ((long) (this._prng.GetSampleForLargeRange() * (double) num) + (long) minValue) : (int) (this._parent.Sample() * (double) num) + minValue;
      }

      public override long NextInt64()
      {
        ulong num;
        do
        {
          num = this.NextUInt64() >> 1;
        }
        while (num == (ulong) long.MaxValue);
        return (long) num;
      }

      public override long NextInt64(long maxValue) => this.NextInt64(0L, maxValue);

      public override long NextInt64(long minValue, long maxValue)
      {
        ulong num1 = (ulong) (maxValue - minValue);
        if (num1 <= 1UL)
          return minValue;
        int num2 = BitOperations.Log2Ceiling(num1);
        ulong num3;
        do
        {
          num3 = this.NextUInt64() >> 64 - num2;
        }
        while (num3 >= num1);
        return (long) num3 + minValue;
      }

      private ulong NextUInt64() => (ulong) ((long) (uint) this._parent.Next(4194304) | (long) (uint) this._parent.Next(4194304) << 22 | (long) (uint) this._parent.Next(1048576) << 44);

      public override double NextDouble() => this._parent.Sample();

      public override float NextSingle() => (float) this._parent.Sample();

      public override void NextBytes(byte[] buffer) => this._prng.NextBytes((Span<byte>) buffer);

      public override void NextBytes(Span<byte> buffer)
      {
        for (int index = 0; index < buffer.Length; ++index)
          buffer[index] = (byte) this._parent.Next();
      }
    }

    private struct CompatPrng
    {
      private int[] _seedArray;
      private int _inext;
      private int _inextp;

      public CompatPrng(int seed)
      {
        int[] numArray = new int[56];
        int num1 = 161803398 - (seed == int.MinValue ? int.MaxValue : Math.Abs(seed));
        numArray[55] = num1;
        int num2 = 1;
        int index1 = 0;
        for (int index2 = 1; index2 < 55; ++index2)
        {
          if ((index1 += 21) >= 55)
            index1 -= 55;
          numArray[index1] = num2;
          num2 = num1 - num2;
          if (num2 < 0)
            num2 += int.MaxValue;
          num1 = numArray[index1];
        }
        for (int index3 = 1; index3 < 5; ++index3)
        {
          for (int index4 = 1; index4 < 56; ++index4)
          {
            int num3 = index4 + 30;
            if (num3 >= 55)
              num3 -= 55;
            numArray[index4] -= numArray[1 + num3];
            if (numArray[index4] < 0)
              numArray[index4] += int.MaxValue;
          }
        }
        this._seedArray = numArray;
        this._inext = 0;
        this._inextp = 21;
      }

      internal double Sample() => (double) this.InternalSample() * 4.656612875245797E-10;

      internal void NextBytes(Span<byte> buffer)
      {
        for (int index = 0; index < buffer.Length; ++index)
          buffer[index] = (byte) this.InternalSample();
      }

      internal int InternalSample()
      {
        int index1;
        if ((index1 = this._inext + 1) >= 56)
          index1 = 1;
        int index2;
        if ((index2 = this._inextp + 1) >= 56)
          index2 = 1;
        int[] seedArray = this._seedArray;
        int num = seedArray[index1] - seedArray[index2];
        if (num == int.MaxValue)
          --num;
        if (num < 0)
          num += int.MaxValue;
        seedArray[index1] = num;
        this._inext = index1;
        this._inextp = index2;
        return num;
      }

      internal double GetSampleForLargeRange()
      {
        int num = this.InternalSample();
        if (this.InternalSample() % 2 == 0)
          num = -num;
        return ((double) num + 2147483646.0) / 4294967293.0;
      }
    }

    internal sealed class XoshiroImpl : Random.ImplBase
    {
      private ulong _s0;
      private ulong _s1;
      private ulong _s2;
      private ulong _s3;

      public unsafe XoshiroImpl()
      {
        ulong* buffer = stackalloc ulong[4];
        do
        {
          Interop.GetRandomBytes((byte*) buffer, 32);
          this._s0 = buffer[0];
          this._s1 = buffer[1];
          this._s2 = buffer[2];
          this._s3 = buffer[3];
        }
        while (((long) this._s0 | (long) this._s1 | (long) this._s2 | (long) this._s3) == 0L);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      internal uint NextUInt32() => (uint) (this.NextUInt64() >> 32);

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      internal ulong NextUInt64()
      {
        ulong s0 = this._s0;
        ulong s1 = this._s1;
        ulong s2 = this._s2;
        ulong s3 = this._s3;
        ulong num1 = BitOperations.RotateLeft(s1 * 5UL, 7) * 9UL;
        ulong num2 = s1 << 17;
        ulong num3 = s2 ^ s0;
        ulong num4 = s3 ^ s1;
        ulong num5 = s1 ^ num3;
        ulong num6 = s0 ^ num4;
        ulong num7 = num3 ^ num2;
        ulong num8 = BitOperations.RotateLeft(num4, 45);
        this._s0 = num6;
        this._s1 = num5;
        this._s2 = num7;
        this._s3 = num8;
        return num1;
      }

      public override int Next()
      {
        ulong num;
        do
        {
          num = this.NextUInt64() >> 33;
        }
        while (num == (ulong) int.MaxValue);
        return (int) num;
      }

      public override int Next(int maxValue)
      {
        if (maxValue <= 1)
          return 0;
        int num1 = BitOperations.Log2Ceiling((uint) maxValue);
        ulong num2;
        do
        {
          num2 = this.NextUInt64() >> 64 - num1;
        }
        while (num2 >= (ulong) (uint) maxValue);
        return (int) num2;
      }

      public override int Next(int minValue, int maxValue)
      {
        ulong num1 = (ulong) maxValue - (ulong) minValue;
        if (num1 <= 1UL)
          return minValue;
        int num2 = BitOperations.Log2Ceiling(num1);
        ulong num3;
        do
        {
          num3 = this.NextUInt64() >> 64 - num2;
        }
        while (num3 >= num1);
        return (int) num3 + minValue;
      }

      public override long NextInt64()
      {
        ulong num;
        do
        {
          num = this.NextUInt64() >> 1;
        }
        while (num == (ulong) long.MaxValue);
        return (long) num;
      }

      public override long NextInt64(long maxValue)
      {
        if (maxValue <= 1L)
          return 0;
        int num1 = BitOperations.Log2Ceiling((ulong) maxValue);
        ulong num2;
        do
        {
          num2 = this.NextUInt64() >> 64 - num1;
        }
        while (num2 >= (ulong) maxValue);
        return (long) num2;
      }

      public override long NextInt64(long minValue, long maxValue)
      {
        ulong num1 = (ulong) (maxValue - minValue);
        if (num1 <= 1UL)
          return minValue;
        int num2 = BitOperations.Log2Ceiling(num1);
        ulong num3;
        do
        {
          num3 = this.NextUInt64() >> 64 - num2;
        }
        while (num3 >= num1);
        return (long) num3 + minValue;
      }

      public override void NextBytes(byte[] buffer) => this.NextBytes((Span<byte>) buffer);

      public override unsafe void NextBytes(Span<byte> buffer)
      {
        ulong s0 = this._s0;
        ulong s1 = this._s1;
        ulong num1 = this._s2;
        ulong num2 = this._s3;
        for (; buffer.Length >= 8; buffer = buffer.Slice(8))
        {
          Unsafe.WriteUnaligned<ulong>(ref MemoryMarshal.GetReference<byte>(buffer), BitOperations.RotateLeft(s1 * 5UL, 7) * 9UL);
          ulong num3 = s1 << 17;
          ulong num4 = num1 ^ s0;
          ulong num5 = num2 ^ s1;
          s1 ^= num4;
          s0 ^= num5;
          num1 = num4 ^ num3;
          num2 = BitOperations.RotateLeft(num5, 45);
        }
        if (!buffer.IsEmpty)
        {
          byte* numPtr = (byte*) &(BitOperations.RotateLeft(s1 * 5UL, 7) * 9UL);
          for (int index = 0; index < buffer.Length; ++index)
            buffer[index] = numPtr[index];
          ulong num6 = s1 << 17;
          ulong num7 = num1 ^ s0;
          ulong num8 = num2 ^ s1;
          s1 ^= num7;
          s0 ^= num8;
          num1 = num7 ^ num6;
          num2 = BitOperations.RotateLeft(num8, 45);
        }
        this._s0 = s0;
        this._s1 = s1;
        this._s2 = num1;
        this._s3 = num2;
      }

      public override double NextDouble() => (double) (this.NextUInt64() >> 11) * 1.1102230246251565E-16;

      public override float NextSingle() => (float) (this.NextUInt64() >> 40) * 5.9604645E-08f;

      public override double Sample() => throw new NotSupportedException();
    }
  }
}
