// Decompiled with JetBrains decompiler
// Type: System.MathF
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace System
{
  /// <summary>Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions.</summary>
  public static class MathF
  {
    /// <summary>Represents the natural logarithmic base, specified by the constant, <see langword="e" />.</summary>
    public const float E = 2.7182817f;
    /// <summary>Represents the ratio of the circumference of a circle to its diameter, specified by the constant, p.</summary>
    public const float PI = 3.1415927f;
    /// <summary>Represents the number of radians in one turn, specified by the constant, τ.</summary>
    public const float Tau = 6.2831855f;
    private static readonly float[] roundPower10Single = new float[7]
    {
      1f,
      10f,
      100f,
      1000f,
      10000f,
      100000f,
      1000000f
    };

    /// <summary>Returns the angle whose cosine is the specified number.</summary>
    /// <param name="x">A number representing a cosine, where <paramref name="x" /> must be greater than or equal to -1, but less than or equal to 1.</param>
    /// <returns>An angle, θ, measured in radians, such that 0 ≤ θ ≤ π.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> &lt; -1 or <paramref name="x" /> &gt; 1 or <paramref name="x" /> equals <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Acos(float x);

    /// <summary>Returns the angle whose hyperbolic cosine is the specified number.</summary>
    /// <param name="x">A number representing a hyperbolic cosine, where <paramref name="x" /> must be greater than or equal to 1, but less than or equal to <see cref="F:System.Single.PositiveInfinity" />.</param>
    /// <returns>An angle, θ, measured in radians, such that 0 ≤ θ ≤ ∞.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> &lt; 1 or <paramref name="x" /> equals <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Acosh(float x);

    /// <summary>Returns the angle whose sine is the specified number.</summary>
    /// <param name="x">A number representing a sine, where <paramref name="x" /> must be greater than or equal to -1, but less than or equal to 1.</param>
    /// <returns>An angle, θ, measured in radians, such that -π/2 ≤ θ ≤ π/2.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> &lt; -1 or <paramref name="x" /> &gt; 1 or <paramref name="x" /> equals <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Asin(float x);

    /// <summary>Returns the angle whose hyperbolic sine is the specified number.</summary>
    /// <param name="x">A number representing a hyperbolic sine, where <paramref name="x" /> must be greater than or equal to <see cref="F:System.Single.NegativeInfinity" />, but less than or equal to <see cref="F:System.Single.PositiveInfinity" />.</param>
    /// <returns>An angle, θ, measured in radians.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> equals <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Asinh(float x);

    /// <summary>Returns the angle whose tangent is the specified number.</summary>
    /// <param name="x">A number representing a tangent.</param>
    /// <returns>An angle, θ, measured in radians, such that -π/2 ≤ θ ≤ π/2.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> equals <see cref="F:System.Single.NaN" />, -π/2 rounded to double precision (-1.5707963267949) if <paramref name="x" /> equals <see cref="F:System.Single.NegativeInfinity" />, or π/2 rounded to double precision (1.5707963267949) if <paramref name="x" /> equals <see cref="F:System.Single.PositiveInfinity" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Atan(float x);

    /// <summary>Returns the angle whose hyperbolic tangent is the specified number.</summary>
    /// <param name="x">A number representing a hyperbolic tangent, where <paramref name="x" /> must be greater than or equal to -1, but less than or equal to 1.</param>
    /// <returns>An angle, θ, measured in radians, such that -∞ &lt; θ &lt;-1, or 1 &lt; θ &lt; ∞.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> &lt; -1 or <paramref name="x" /> &gt; 1 or <paramref name="x" /> equals <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Atanh(float x);

    /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
    /// <param name="y">The y coordinate of a point.</param>
    /// <param name="x">The x coordinate of a point.</param>
    /// <returns>An angle, θ, measured in radians, such that -π ≤ θ ≤ π, and tan(θ) = <paramref name="y" /> / <paramref name="x" />, where (<paramref name="x" />, <paramref name="y" />) is a point in the Cartesian plane. Observe the following:
    /// 
    /// -   For (<paramref name="x" />, <paramref name="y" />) in quadrant 1, 0 &lt; θ &lt; π/2.
    /// 
    /// -   For (<paramref name="x" />, <paramref name="y" />) in quadrant 2, π/2 &lt; θ ≤ π.
    /// 
    /// -   For (<paramref name="x" />, <paramref name="y" />) in quadrant 3, -π &lt; θ &lt; -π/2.
    /// 
    /// -   For (<paramref name="x" />, <paramref name="y" />) in quadrant 4, -π/2 &lt; θ &lt; 0.
    /// 
    ///  For points on the boundaries of the quadrants, the return value is the following:
    /// 
    /// -   If y is 0 and x is not negative, θ = 0.
    /// 
    /// -   If y is 0 and x is negative, θ = π.
    /// 
    /// -   If y is positive and x is 0, θ = π/2.
    /// 
    /// -   If y is negative and x is 0, θ = -π/2.
    /// 
    /// -   If y is 0 and x is 0, θ = 0.
    /// 
    ///  If <paramref name="x" /> or <paramref name="y" /> is <see cref="F:System.Single.NaN" />, or if <paramref name="x" /> and <paramref name="y" /> are either <see cref="F:System.Single.PositiveInfinity" /> or <see cref="F:System.Single.NegativeInfinity" />, the method returns <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Atan2(float y, float x);

    /// <summary>Returns the cube root of a specified number.</summary>
    /// <param name="x">The number whose cube root is to be found.</param>
    /// <returns>The cube root of <paramref name="x" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> is equals <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Cbrt(float x);

    /// <summary>Returns the smallest integral value that is greater than or equal to the specified single-precision floating-point number.</summary>
    /// <param name="x">A single-precision floating-point number.</param>
    /// <returns>The smallest integral value that is greater than or equal to <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NegativeInfinity" />, or <see cref="F:System.Single.PositiveInfinity" />, that value is returned. Note that this method returns a <see cref="T:System.Single" /> instead of an integral type.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Ceiling(float x);

    /// <summary>Returns the cosine of the specified angle.</summary>
    /// <param name="x">An angle, measured in radians.</param>
    /// <returns>The cosine of <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NegativeInfinity" />, or <see cref="F:System.Single.PositiveInfinity" />, this method returns <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Cos(float x);

    /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
    /// <param name="x">An angle, measured in radians.</param>
    /// <returns>The hyperbolic cosine of <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NegativeInfinity" /> or <see cref="F:System.Single.PositiveInfinity" />, <see cref="F:System.Single.PositiveInfinity" /> is returned. If <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Cosh(float x);

    /// <summary>Returns <see langword="e" /> raised to the specified power.</summary>
    /// <param name="x">A number specifying a power.</param>
    /// <returns>The number <see langword="e" /> raised to the power <paramref name="x" />. If <paramref name="x" /> equals <see cref="F:System.Single.NaN" /> or <see cref="F:System.Single.PositiveInfinity" />, that value is returned. If <paramref name="x" /> equals <see cref="F:System.Single.NegativeInfinity" />, 0 is returned.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Exp(float x);

    /// <summary>Returns the largest integral value less than or equal to the specified single-precision floating-point number.</summary>
    /// <param name="x">A single-precision floating-point number.</param>
    /// <returns>The largest integral value less than or equal to <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NegativeInfinity" />, or <see cref="F:System.Single.PositiveInfinity" />, that value is returned.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Floor(float x);

    /// <summary>Returns (x * y) + z, rounded as one ternary operation.</summary>
    /// <param name="x">The number to be multiplied with <paramref name="y" />.</param>
    /// <param name="y">The number to be multiplied with <paramref name="x" />.</param>
    /// <param name="z">The number to be added to the result of <paramref name="x" /> multiplied by <paramref name="y" />.</param>
    /// <returns>(x * y) + z, rounded as one ternary operation.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float FusedMultiplyAdd(float x, float y, float z);

    /// <summary>Returns the base 2 integer logarithm of a specified number.</summary>
    /// <param name="x">The number whose logarithm is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /> parameter</term><description> Return value</description></listheader><item><term> Default</term><description> The base 2 integer log of <paramref name="x" />; that is, (int)log2(<paramref name="x" />).</description></item><item><term> Zero</term><description><see cref="F:System.Int32.MinValue" /></description></item><item><term> Equal to <see cref="F:System.Single.NaN" /> or <see cref="F:System.Single.PositiveInfinity" /> or <see cref="F:System.Single.NegativeInfinity" /></term><description><see cref="F:System.Int32.MaxValue" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int ILogB(float x);

    /// <summary>Returns the natural (base <see langword="e" />) logarithm of a specified number.</summary>
    /// <param name="x">The number whose logarithm is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /> parameter</term><description> Return value</description></listheader><item><term> Positive</term><description> The natural logarithm of <paramref name="x" />; that is, ln <paramref name="x" />, or log e <paramref name="x" /></description></item><item><term> Zero</term><description><see cref="F:System.Single.NegativeInfinity" /></description></item><item><term> Negative</term><description><see cref="F:System.Single.NaN" /></description></item><item><term> Equal to <see cref="F:System.Single.NaN" /></term><description><see cref="F:System.Single.NaN" /></description></item><item><term> Equal to <see cref="F:System.Single.PositiveInfinity" /></term><description><see cref="F:System.Single.PositiveInfinity" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Log(float x);

    /// <summary>Returns the base 2 logarithm of a specified number.</summary>
    /// <param name="x">A number whose logarithm is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /> parameter</term><description> Return value</description></listheader><item><term> Positive</term><description> The base 2 log of <paramref name="x" />; that is, log 2<paramref name="x" />.</description></item><item><term> Zero</term><description><see cref="F:System.Single.NegativeInfinity" /></description></item><item><term> Negative</term><description><see cref="F:System.Single.NaN" /></description></item><item><term> Equal to <see cref="F:System.Single.NaN" /></term><description><see cref="F:System.Single.NaN" /></description></item><item><term> Equal to <see cref="F:System.Single.PositiveInfinity" /></term><description><see cref="F:System.Single.PositiveInfinity" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Log2(float x);

    /// <summary>Returns the base 10 logarithm of a specified number.</summary>
    /// <param name="x">A number whose logarithm is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /> parameter</term><description> Return value</description></listheader><item><term> Positive</term><description> The base 10 log of <paramref name="x" />; that is, log 10<paramref name="x" />.</description></item><item><term> Zero</term><description><see cref="F:System.Single.NegativeInfinity" /></description></item><item><term> Negative</term><description><see cref="F:System.Single.NaN" /></description></item><item><term> Equal to <see cref="F:System.Single.NaN" /></term><description><see cref="F:System.Single.NaN" /></description></item><item><term> Equal to <see cref="F:System.Single.PositiveInfinity" /></term><description><see cref="F:System.Single.PositiveInfinity" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Log10(float x);

    /// <summary>Returns a specified number raised to the specified power.</summary>
    /// <param name="x">A single-precision floating-point number to be raised to a power.</param>
    /// <param name="y">A single-precision floating-point number that specifies a power.</param>
    /// <returns>The number <paramref name="x" /> raised to the power <paramref name="y" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Pow(float x, float y);

    /// <summary>Returns the sine of the specified angle.</summary>
    /// <param name="x">An angle, measured in radians.</param>
    /// <returns>The sine of <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NegativeInfinity" />, or <see cref="F:System.Single.PositiveInfinity" />, this method returns <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Sin(float x);

    /// <summary>Returns the sine and cosine of the specified angle.</summary>
    /// <param name="x">An angle, measured in radians.</param>
    /// <returns>The sine and cosine of <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NegativeInfinity" />, or <see cref="F:System.Single.PositiveInfinity" />, this method returns <see cref="F:System.Single.NaN" />.</returns>
    public static unsafe (float Sin, float Cos) SinCos(float x)
    {
      float num1;
      float num2;
      MathF.SinCos(x, &num1, &num2);
      return (num1, num2);
    }

    /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
    /// <param name="x">An angle, measured in radians.</param>
    /// <returns>The hyperbolic sine of <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NegativeInfinity" />, <see cref="F:System.Single.PositiveInfinity" />, or <see cref="F:System.Single.NaN" />, this method returns a <see cref="T:System.Single" /> equal to <paramref name="x" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Sinh(float x);

    /// <summary>Returns the square root of a specified number.</summary>
    /// <param name="x">The number whose square root is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /> parameter</term><description> Return value</description></listheader><item><term> Zero or positive</term><description> The positive square root of <paramref name="x" />.</description></item><item><term> Negative</term><description><see cref="F:System.Single.NaN" /></description></item><item><term> Equals <see cref="F:System.Single.NaN" /></term><description><see cref="F:System.Single.NaN" /></description></item><item><term> Equals <see cref="F:System.Single.PositiveInfinity" /></term><description><see cref="F:System.Single.PositiveInfinity" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Sqrt(float x);

    /// <summary>Returns the tangent of the specified angle.</summary>
    /// <param name="x">An angle, measured in radians.</param>
    /// <returns>The tangent of <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NegativeInfinity" />, or <see cref="F:System.Single.PositiveInfinity" />, this method returns <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Tan(float x);

    /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
    /// <param name="x">An angle, measured in radians.</param>
    /// <returns>The hyperbolic tangent of <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Single.NegativeInfinity" />, this method returns -1. If value is equal to <see cref="F:System.Single.PositiveInfinity" />, this method returns 1. If <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />, this method returns <see cref="F:System.Single.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Tanh(float x);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern unsafe float ModF(float x, float* intptr);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern unsafe void SinCos(float x, float* sin, float* cos);

    /// <summary>Returns the absolute value of a single-precision floating-point number.</summary>
    /// <param name="x">A number that is greater than or equal to <see cref="F:System.Single.MinValue" />, but less than or equal to <see cref="F:System.Single.MaxValue" />.</param>
    /// <returns>A single-precision floating-point number, x, such that 0 ≤ x ≤ <see cref="F:System.Single.MaxValue" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Abs(float x) => Math.Abs(x);

    /// <summary>Returns the next smallest value that compares less than <paramref name="x" />.</summary>
    /// <param name="x">The value to decrement.</param>
    /// <returns>The next smallest value that compares less than <paramref name="x" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NegativeInfinity" /> if <paramref name="x" /> is equals <see cref="F:System.Single.NegativeInfinity" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> equals <see cref="F:System.Single.NaN" />.</returns>
    public static float BitDecrement(float x)
    {
      int int32Bits = BitConverter.SingleToInt32Bits(x);
      return (int32Bits & 2139095040) >= 2139095040 ? (int32Bits != 2139095040 ? x : float.MaxValue) : (int32Bits == 0 ? -1E-45f : BitConverter.Int32BitsToSingle(int32Bits + (int32Bits < 0 ? 1 : -1)));
    }

    /// <summary>Returns the next largest value that is greater than <paramref name="x" />.</summary>
    /// <param name="x">The value to increment.</param>
    /// <returns>The next largest value that is greater than <paramref name="x" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.PositiveInfinity" /> if <paramref name="x" /> equals <see cref="F:System.Single.PositiveInfinity" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Single.NaN" /> if <paramref name="x" /> is equals <see cref="F:System.Single.NaN" />.</returns>
    public static float BitIncrement(float x)
    {
      int int32Bits = BitConverter.SingleToInt32Bits(x);
      return (int32Bits & 2139095040) >= 2139095040 ? (int32Bits != -8388608 ? x : float.MinValue) : (int32Bits == int.MinValue ? float.Epsilon : BitConverter.Int32BitsToSingle(int32Bits + (int32Bits < 0 ? -1 : 1)));
    }

    /// <summary>Returns a value with the magnitude of <paramref name="x" /> and the sign of <paramref name="y" />.</summary>
    /// <param name="x">A number whose magnitude is used in the result.</param>
    /// <param name="y">A number whose sign is the used in the result.</param>
    /// <returns>A value with the magnitude of <paramref name="x" /> and the sign of <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CopySign(float x, float y)
    {
      return Sse.IsSupported || AdvSimd.IsSupported ? VectorMath.ConditionalSelectBitwise(Vector128.CreateScalarUnsafe(-0.0f), Vector128.CreateScalarUnsafe(y), Vector128.CreateScalarUnsafe(x)).ToScalar<float>() : SoftwareFallback(x, y);

      static float SoftwareFallback(float x, float y) => BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits(x) & int.MaxValue | BitConverter.SingleToInt32Bits(y) & int.MinValue);
    }

    /// <summary>Returns the remainder resulting from the division of a specified number by another specified number.</summary>
    /// <param name="x">A dividend.</param>
    /// <param name="y">A divisor.</param>
    /// <returns>A number equal to <paramref name="x" /> - (<paramref name="y" /> Q), where Q is the quotient of <paramref name="x" /> / <paramref name="y" /> rounded to the nearest integer (if <paramref name="x" /> / <paramref name="y" /> falls halfway between two integers, the even integer is returned).
    /// 
    /// If <paramref name="x" /> - (<paramref name="y" /> Q) is zero, the value +0 is returned if <paramref name="x" /> is positive, or -0 if <paramref name="x" /> is negative.
    /// 
    /// If <paramref name="y" /> = 0, <see cref="F:System.Single.NaN" /> is returned.</returns>
    public static float IEEERemainder(float x, float y)
    {
      if (float.IsNaN(x))
        return x;
      if (float.IsNaN(y))
        return y;
      float num = x % y;
      if (float.IsNaN(num))
        return float.NaN;
      if ((double) num == 0.0 && float.IsNegative(x))
        return -0.0f;
      float x1 = num - MathF.Abs(y) * (float) MathF.Sign(x);
      if ((double) MathF.Abs(x1) == (double) MathF.Abs(num))
      {
        float x2 = x / y;
        return (double) MathF.Abs(MathF.Round(x2)) > (double) MathF.Abs(x2) ? x1 : num;
      }
      return (double) MathF.Abs(x1) < (double) MathF.Abs(num) ? x1 : num;
    }

    /// <summary>Returns the logarithm of a specified number in a specified base.</summary>
    /// <param name="x">The number whose logarithm is to be found.</param>
    /// <param name="y">The base.</param>
    /// <returns>One of the values in the following table. (+Infinity denotes <see cref="F:System.Single.PositiveInfinity" />, -Infinity denotes <see cref="F:System.Single.NegativeInfinity" />, and NaN denotes <see cref="F:System.Single.NaN" />.)
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /></term><description><paramref name="newBase" /></description><description> Return value</description></listheader><item><term><paramref name="x" />&gt; 0</term><description> (0 &lt;<paramref name="newBase" />&lt; 1) -or-(<paramref name="newBase" />&gt; 1)</description><description> lognewBase(a)</description></item><item><term><paramref name="x" />&lt; 0</term><description> (any value)</description><description> NaN</description></item><item><term> (any value)</term><description><paramref name="newBase" />&lt; 0</description><description> NaN</description></item><item><term><paramref name="x" /> != 1</term><description><paramref name="newBase" /> = 0</description><description> NaN</description></item><item><term><paramref name="x" /> != 1</term><description><paramref name="newBase" /> = +Infinity</description><description> NaN</description></item><item><term><paramref name="x" /> = NaN</term><description> (any value)</description><description> NaN</description></item><item><term> (any value)</term><description><paramref name="newBase" /> = NaN</description><description> NaN</description></item><item><term> (any value)</term><description><paramref name="newBase" /> = 1</description><description> NaN</description></item><item><term><paramref name="x" /> = 0</term><description> 0 &lt;<paramref name="newBase" />&lt; 1</description><description> +Infinity</description></item><item><term><paramref name="x" /> = 0</term><description><paramref name="newBase" />&gt; 1</description><description> -Infinity</description></item><item><term><paramref name="x" /> =  +Infinity</term><description> 0 &lt;<paramref name="newBase" />&lt; 1</description><description> -Infinity</description></item><item><term><paramref name="x" /> =  +Infinity</term><description><paramref name="newBase" />&gt; 1</description><description> +Infinity</description></item><item><term><paramref name="x" /> = 1</term><description><paramref name="newBase" /> = 0</description><description> 0</description></item><item><term><paramref name="x" /> = 1</term><description><paramref name="newBase" /> = +Infinity</description><description> 0</description></item></list></returns>
    public static float Log(float x, float y)
    {
      if (float.IsNaN(x))
        return x;
      if (float.IsNaN(y))
        return y;
      return (double) y == 1.0 || (double) x != 1.0 && ((double) y == 0.0 || float.IsPositiveInfinity(y)) ? float.NaN : MathF.Log(x) / MathF.Log(y);
    }

    /// <summary>Returns the larger of two single-precision floating-point numbers.</summary>
    /// <param name="x">The first of two single-precision floating-point numbers to compare.</param>
    /// <param name="y">The second of two single-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="x" /> or <paramref name="y" />, whichever is larger. If <paramref name="x" />, or <paramref name="y" />, or both <paramref name="x" /> and <paramref name="y" /> are equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Max(float x, float y) => Math.Max(x, y);

    /// <summary>Returns the larger magnitude of two single-precision floating-point numbers.</summary>
    /// <param name="x">The first of two single-precision floating-point numbers to compare.</param>
    /// <param name="y">The second of two single-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="x" /> or <paramref name="y" />, whichever has the larger magnitude. If <paramref name="x" />, or <paramref name="y" />, or both <paramref name="x" /> and <paramref name="y" /> are equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
    public static float MaxMagnitude(float x, float y)
    {
      float f = MathF.Abs(x);
      float num = MathF.Abs(y);
      return (double) f > (double) num || float.IsNaN(f) || (double) f == (double) num && !float.IsNegative(x) ? x : y;
    }

    /// <summary>Returns the smaller of two single-precision floating-point numbers.</summary>
    /// <param name="x">The first of two single-precision floating-point numbers to compare.</param>
    /// <param name="y">The second of two single-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="x" /> or <paramref name="y" />, whichever is smaller. If <paramref name="x" />, <paramref name="y" />, or both <paramref name="x" /> and <paramref name="y" /> are equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Min(float x, float y) => Math.Min(x, y);

    /// <summary>Returns the smaller magnitude of two single-precision floating-point numbers.</summary>
    /// <param name="x">The first of two single-precision floating-point numbers to compare.</param>
    /// <param name="y">The second of two single-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="x" /> or <paramref name="y" />, whichever has the smaller magnitude. If <paramref name="x" />, or <paramref name="y" />, or both <paramref name="x" /> and <paramref name="y" /> are equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
    public static float MinMagnitude(float x, float y)
    {
      float f = MathF.Abs(x);
      float num = MathF.Abs(y);
      return (double) f < (double) num || float.IsNaN(f) || (double) f == (double) num && float.IsNegative(x) ? x : y;
    }

    /// <summary>Returns an estimate of the reciprocal of a specified number.</summary>
    /// <param name="x">The number whose reciprocal is to be estimated.</param>
    /// <returns>An estimate of the reciprocal of <paramref name="x" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReciprocalEstimate(float x)
    {
      if (Sse.IsSupported)
        return Sse.ReciprocalScalar(Vector128.CreateScalarUnsafe(x)).ToScalar<float>();
      if (!AdvSimd.Arm64.IsSupported)
        ;
      return 1f / x;
    }

    /// <summary>Returns an estimate of the reciprocal square root of a specified number.</summary>
    /// <param name="x">The number whose reciprocal square root is to be estimated.</param>
    /// <returns>An estimate of the reciprocal square root <paramref name="x" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReciprocalSqrtEstimate(float x)
    {
      if (Sse.IsSupported)
        return Sse.ReciprocalSqrtScalar(Vector128.CreateScalarUnsafe(x)).ToScalar<float>();
      if (!AdvSimd.Arm64.IsSupported)
        ;
      return 1f / MathF.Sqrt(x);
    }

    /// <summary>Rounds a single-precision floating-point value to the nearest integral value, and rounds midpoint values to the nearest even number.</summary>
    /// <param name="x">A single-precision floating-point number to be rounded.</param>
    /// <returns>The integer nearest <paramref name="x" />. If the fractional component of <paramref name="x" /> is halfway between two integers, one of which is even and the other odd, then the even number is returned. Note that this method returns a <see cref="T:System.Single" /> instead of an integral type.</returns>
    [Intrinsic]
    public static float Round(float x)
    {
      uint uint32Bits = BitConverter.SingleToUInt32Bits(x);
      int exponentFromBits = float.ExtractExponentFromBits(uint32Bits);
      if (exponentFromBits <= 126)
        return (int) uint32Bits << 1 == 0 ? x : MathF.CopySign(exponentFromBits != 126 || float.ExtractSignificandFromBits(uint32Bits) == 0U ? 0.0f : 1f, x);
      if (exponentFromBits >= 150)
        return x;
      uint num1 = (uint) (1 << 150 - exponentFromBits);
      uint num2 = num1 - 1U;
      uint num3 = uint32Bits + (num1 >> 1);
      return BitConverter.UInt32BitsToSingle(((int) num3 & (int) num2) != 0 ? num3 & ~num2 : num3 & ~num1);
    }

    /// <summary>Rounds a single-precision floating-point value to a specified number of fractional digits, and rounds midpoint values to the nearest even number.</summary>
    /// <param name="x">A single-precision floating-point number to be rounded.</param>
    /// <param name="digits">The number of fractional digits in the return value.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="digits" /> is less than 0 or greater than 6.</exception>
    /// <returns>The number nearest to <paramref name="x" /> that contains a number of fractional digits equal to <paramref name="digits" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(float x, int digits) => MathF.Round(x, digits, MidpointRounding.ToEven);

    /// <summary>Rounds a single-precision floating-point value to an integer using the specified rounding convention.</summary>
    /// <param name="x">A single-precision floating-point number to be rounded.</param>
    /// <param name="mode">One of the enumeration values that specifies which rounding strategy to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
    /// <returns>The integer that <paramref name="x" /> is rounded to using the <paramref name="mode" /> rounding convention. This method returns a <see cref="T:System.Single" /> instead of an integral type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(float x, MidpointRounding mode) => MathF.Round(x, 0, mode);

    /// <summary>Rounds a single-precision floating-point value to a specified number of fractional digits using the specified rounding convention.</summary>
    /// <param name="x">A single-precision floating-point number to be rounded.</param>
    /// <param name="digits">The number of fractional digits in the return value.</param>
    /// <param name="mode">One of the enumeration values that specifies which rounding strategy to use.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="digits" /> is less than 0 or greater than 6.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
    /// <returns>The number that <paramref name="x" /> is rounded to that has <paramref name="digits" /> fractional digits. If <paramref name="x" /> has fewer fractional digits than <paramref name="digits" />, <paramref name="x" /> is returned unchanged.</returns>
    public static unsafe float Round(float x, int digits, MidpointRounding mode)
    {
      if (digits < 0 || digits > 6)
        throw new ArgumentOutOfRangeException(nameof (digits), SR.ArgumentOutOfRange_RoundingDigits_MathF);
      if (mode < MidpointRounding.ToEven || mode > MidpointRounding.ToPositiveInfinity)
        throw new ArgumentException(SR.Format(SR.Argument_InvalidEnumValue, (object) mode, (object) "MidpointRounding"), nameof (mode));
      if ((double) MathF.Abs(x) < 100000000.0)
      {
        float num = MathF.roundPower10Single[digits];
        x *= num;
        switch (mode)
        {
          case MidpointRounding.ToEven:
            x = MathF.Round(x);
            break;
          case MidpointRounding.AwayFromZero:
            float x1 = MathF.ModF(x, &x);
            if ((double) MathF.Abs(x1) >= 0.5)
            {
              x += (float) MathF.Sign(x1);
              break;
            }
            break;
          case MidpointRounding.ToZero:
            x = MathF.Truncate(x);
            break;
          case MidpointRounding.ToNegativeInfinity:
            x = MathF.Floor(x);
            break;
          case MidpointRounding.ToPositiveInfinity:
            x = MathF.Ceiling(x);
            break;
          default:
            throw new ArgumentException(SR.Format(SR.Argument_InvalidEnumValue, (object) mode, (object) "MidpointRounding"), nameof (mode));
        }
        x /= num;
      }
      return x;
    }

    /// <summary>Returns an integer that indicates the sign of a single-precision floating-point number.</summary>
    /// <param name="x">A signed number.</param>
    /// <exception cref="T:System.ArithmeticException">
    /// <paramref name="x" /> is equal to <see cref="F:System.Single.NaN" />.</exception>
    /// <returns>A number that indicates the sign of <paramref name="x" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> -1</term><description><paramref name="x" /> is less than zero.</description></item><item><term> 0</term><description><paramref name="x" /> is equal to zero.</description></item><item><term> 1</term><description><paramref name="x" /> is greater than zero.</description></item></list></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(float x) => Math.Sign(x);

    /// <summary>Calculates the integral part of a specified single-precision floating-point number.</summary>
    /// <param name="x">A number to truncate.</param>
    /// <returns>The integral part of <paramref name="x" />; that is, the number that remains after any fractional digits have been discarded, or one of the values listed in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /></term><description> Return value</description></listheader><item><term><see cref="F:System.Single.NaN" /></term><description><see cref="F:System.Single.NaN" /></description></item><item><term><see cref="F:System.Single.NegativeInfinity" /></term><description><see cref="F:System.Single.NegativeInfinity" /></description></item><item><term><see cref="F:System.Single.PositiveInfinity" /></term><description><see cref="F:System.Single.PositiveInfinity" /></description></item></list></returns>
    public static unsafe float Truncate(float x)
    {
      double num = (double) MathF.ModF(x, &x);
      return x;
    }

    /// <summary>Returns x * 2^n computed efficiently.</summary>
    /// <param name="x">A single-precision floating-point number that specifies the base value.</param>
    /// <param name="n">A single-precision floating-point number that specifies the power.</param>
    /// <returns>x * 2^n computed efficiently.</returns>
    public static float ScaleB(float x, int n)
    {
      float num = x;
      if (n > (int) sbyte.MaxValue)
      {
        num *= 1.7014118E+38f;
        n -= (int) sbyte.MaxValue;
        if (n > (int) sbyte.MaxValue)
        {
          num *= 1.7014118E+38f;
          n -= (int) sbyte.MaxValue;
          if (n > (int) sbyte.MaxValue)
            n = (int) sbyte.MaxValue;
        }
      }
      else if (n < -126)
      {
        num *= 1.9721523E-31f;
        n += 102;
        if (n < -126)
        {
          num *= 1.9721523E-31f;
          n += 102;
          if (n < -126)
            n = -126;
        }
      }
      float single = BitConverter.Int32BitsToSingle((int) sbyte.MaxValue + n << 23);
      return num * single;
    }
  }
}
