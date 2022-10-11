// Decompiled with JetBrains decompiler
// Type: System.Math
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Versioning;

namespace System
{
  /// <summary>Provides constants and static methods for trigonometric, logarithmic, and other common mathematical functions.</summary>
  public static class Math
  {
    /// <summary>Represents the natural logarithmic base, specified by the constant, <see langword="e" />.</summary>
    public const double E = 2.718281828459045;
    /// <summary>Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π.</summary>
    public const double PI = 3.141592653589793;
    /// <summary>Represents the number of radians in one turn, specified by the constant, τ.</summary>
    public const double Tau = 6.283185307179586;
    private static readonly double[] roundPower10Double = new double[16]
    {
      1.0,
      10.0,
      100.0,
      1000.0,
      10000.0,
      100000.0,
      1000000.0,
      10000000.0,
      100000000.0,
      1000000000.0,
      10000000000.0,
      100000000000.0,
      1000000000000.0,
      10000000000000.0,
      100000000000000.0,
      1000000000000000.0
    };

    /// <summary>Returns the absolute value of a double-precision floating-point number.</summary>
    /// <param name="value">A number that is greater than or equal to <see cref="F:System.Double.MinValue" />, but less than or equal to <see cref="F:System.Double.MaxValue" />.</param>
    /// <returns>A double-precision floating-point number, x, such that 0 ≤ x ≤ <see cref="F:System.Double.MaxValue" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Abs(double value);

    /// <summary>Returns the absolute value of a single-precision floating-point number.</summary>
    /// <param name="value">A number that is greater than or equal to <see cref="F:System.Single.MinValue" />, but less than or equal to <see cref="F:System.Single.MaxValue" />.</param>
    /// <returns>A single-precision floating-point number, x, such that 0 ≤ x ≤ <see cref="F:System.Single.MaxValue" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern float Abs(float value);

    /// <summary>Returns the angle whose cosine is the specified number.</summary>
    /// <param name="d">A number representing a cosine, where <paramref name="d" /> must be greater than or equal to -1, but less than or equal to 1.</param>
    /// <returns>An angle, θ, measured in radians, such that 0 ≤ θ ≤ π.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="d" /> &lt; -1 or <paramref name="d" /> &gt; 1 or <paramref name="d" /> equals <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Acos(double d);

    /// <summary>Returns the angle whose hyperbolic cosine is the specified number.</summary>
    /// <param name="d">A number representing a hyperbolic cosine, where <paramref name="d" /> must be greater than or equal to 1, but less than or equal to <see cref="F:System.Double.PositiveInfinity" />.</param>
    /// <returns>An angle, θ, measured in radians, such that 0 ≤ θ ≤ ∞.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="d" /> &lt; 1 or <paramref name="d" /> equals <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Acosh(double d);

    /// <summary>Returns the angle whose sine is the specified number.</summary>
    /// <param name="d">A number representing a sine, where <paramref name="d" /> must be greater than or equal to -1, but less than or equal to 1.</param>
    /// <returns>An angle, θ, measured in radians, such that -π/2 ≤ θ ≤ π/2.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="d" /> &lt; -1 or <paramref name="d" /> &gt; 1 or <paramref name="d" /> equals <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Asin(double d);

    /// <summary>Returns the angle whose hyperbolic sine is the specified number.</summary>
    /// <param name="d">A number representing a hyperbolic sine, where <paramref name="d" /> must be greater than or equal to <see cref="F:System.Double.NegativeInfinity" />, but less than or equal to <see cref="F:System.Double.PositiveInfinity" />.</param>
    /// <returns>An angle, θ, measured in radians.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="d" /> equals <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Asinh(double d);

    /// <summary>Returns the angle whose tangent is the specified number.</summary>
    /// <param name="d">A number representing a tangent.</param>
    /// <returns>An angle, θ, measured in radians, such that -π/2 ≤ θ ≤ π/2.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="d" /> equals <see cref="F:System.Double.NaN" />, -π/2 rounded to double precision (-1.5707963267949) if <paramref name="d" /> equals <see cref="F:System.Double.NegativeInfinity" />, or π/2 rounded to double precision (1.5707963267949) if <paramref name="d" /> equals <see cref="F:System.Double.PositiveInfinity" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Atan(double d);

    /// <summary>Returns the angle whose hyperbolic tangent is the specified number.</summary>
    /// <param name="d">A number representing a hyperbolic tangent, where <paramref name="d" /> must be greater than or equal to -1, but less than or equal to 1.</param>
    /// <returns>An angle, θ, measured in radians, such that -∞ &lt; θ &lt; -1, or 1 &lt; θ &lt; ∞.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="d" /> &lt; -1 or <paramref name="d" /> &gt; 1 or <paramref name="d" /> equals <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Atanh(double d);

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
    ///  If <paramref name="x" /> or <paramref name="y" /> is <see cref="F:System.Double.NaN" />, or if <paramref name="x" /> and <paramref name="y" /> are either <see cref="F:System.Double.PositiveInfinity" /> or <see cref="F:System.Double.NegativeInfinity" />, the method returns <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Atan2(double y, double x);

    /// <summary>Returns the cube root of a specified number.</summary>
    /// <param name="d">The number whose cube root is to be found.</param>
    /// <returns>The cube root of <paramref name="d" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="x" /> equals <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Cbrt(double d);

    /// <summary>Returns the smallest integral value that is greater than or equal to the specified double-precision floating-point number.</summary>
    /// <param name="a">A double-precision floating-point number.</param>
    /// <returns>The smallest integral value that is greater than or equal to <paramref name="a" />. If <paramref name="a" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, that value is returned. Note that this method returns a <see cref="T:System.Double" /> instead of an integral type.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Ceiling(double a);

    /// <summary>Returns the cosine of the specified angle.</summary>
    /// <param name="d">An angle, measured in radians.</param>
    /// <returns>The cosine of <paramref name="d" />. If <paramref name="d" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Cos(double d);

    /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
    /// <param name="value">An angle, measured in radians.</param>
    /// <returns>The hyperbolic cosine of <paramref name="value" />. If <paramref name="value" /> is equal to <see cref="F:System.Double.NegativeInfinity" /> or <see cref="F:System.Double.PositiveInfinity" />, <see cref="F:System.Double.PositiveInfinity" /> is returned. If <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" /> is returned.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Cosh(double value);

    /// <summary>Returns <see langword="e" /> raised to the specified power.</summary>
    /// <param name="d">A number specifying a power.</param>
    /// <returns>The number <see langword="e" /> raised to the power <paramref name="d" />. If <paramref name="d" /> equals <see cref="F:System.Double.NaN" /> or <see cref="F:System.Double.PositiveInfinity" />, that value is returned. If <paramref name="d" /> equals <see cref="F:System.Double.NegativeInfinity" />, 0 is returned.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Exp(double d);

    /// <summary>Returns the largest integral value less than or equal to the specified double-precision floating-point number.</summary>
    /// <param name="d">A double-precision floating-point number.</param>
    /// <returns>The largest integral value less than or equal to <paramref name="d" />. If <paramref name="d" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, that value is returned.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Floor(double d);

    /// <summary>Returns (x * y) + z, rounded as one ternary operation.</summary>
    /// <param name="x">The number to be multiplied with <paramref name="y" />.</param>
    /// <param name="y">The number to be multiplied with <paramref name="x" />.</param>
    /// <param name="z">The number to be added to the result of <paramref name="x" /> multiplied by <paramref name="y" />.</param>
    /// <returns>(x * y) + z, rounded as one ternary operation.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double FusedMultiplyAdd(double x, double y, double z);

    /// <summary>Returns the base 2 integer logarithm of a specified number.</summary>
    /// <param name="x">The number whose logarithm is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /> parameter</term><description> Return value</description></listheader><item><term> Default</term><description> The base 2 integer log of <paramref name="x" />; that is, (int)log2(<paramref name="x" />).</description></item><item><term> Zero</term><description><see cref="F:System.Int32.MinValue" /></description></item><item><term> Equal to <see cref="F:System.Double.NaN" /> or <see cref="F:System.Double.PositiveInfinity" /> or <see cref="F:System.Double.NegativeInfinity" /></term><description><see cref="F:System.Int32.MaxValue" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int ILogB(double x);

    /// <summary>Returns the natural (base <see langword="e" />) logarithm of a specified number.</summary>
    /// <param name="d">The number whose logarithm is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="d" /> parameter</term><description> Return value</description></listheader><item><term> Positive</term><description> The natural logarithm of <paramref name="d" />; that is, ln <paramref name="d" />, or log e <paramref name="d" /></description></item><item><term> Zero</term><description><see cref="F:System.Double.NegativeInfinity" /></description></item><item><term> Negative</term><description><see cref="F:System.Double.NaN" /></description></item><item><term> Equal to <see cref="F:System.Double.NaN" /></term><description><see cref="F:System.Double.NaN" /></description></item><item><term> Equal to <see cref="F:System.Double.PositiveInfinity" /></term><description><see cref="F:System.Double.PositiveInfinity" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Log(double d);

    /// <summary>Returns the base 2 logarithm of a specified number.</summary>
    /// <param name="x">A number whose logarithm is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="x" /> parameter</term><description> Return value</description></listheader><item><term> Positive</term><description> The base 2 log of <paramref name="x" />; that is, log 2<paramref name="x" />.</description></item><item><term> Zero</term><description><see cref="F:System.Double.NegativeInfinity" /></description></item><item><term> Negative</term><description><see cref="F:System.Double.NaN" /></description></item><item><term> Equal to <see cref="F:System.Double.NaN" /></term><description><see cref="F:System.Double.NaN" /></description></item><item><term> Equal to <see cref="F:System.Double.PositiveInfinity" /></term><description><see cref="F:System.Double.PositiveInfinity" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Log2(double x);

    /// <summary>Returns the base 10 logarithm of a specified number.</summary>
    /// <param name="d">A number whose logarithm is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="d" /> parameter</term><description> Return value</description></listheader><item><term> Positive</term><description> The base 10 log of <paramref name="d" />; that is, log 10<paramref name="d" />.</description></item><item><term> Zero</term><description><see cref="F:System.Double.NegativeInfinity" /></description></item><item><term> Negative</term><description><see cref="F:System.Double.NaN" /></description></item><item><term> Equal to <see cref="F:System.Double.NaN" /></term><description><see cref="F:System.Double.NaN" /></description></item><item><term> Equal to <see cref="F:System.Double.PositiveInfinity" /></term><description><see cref="F:System.Double.PositiveInfinity" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Log10(double d);

    /// <summary>Returns a specified number raised to the specified power.</summary>
    /// <param name="x">A double-precision floating-point number to be raised to a power.</param>
    /// <param name="y">A double-precision floating-point number that specifies a power.</param>
    /// <returns>The number <paramref name="x" /> raised to the power <paramref name="y" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Pow(double x, double y);

    /// <summary>Returns the sine of the specified angle.</summary>
    /// <param name="a">An angle, measured in radians.</param>
    /// <returns>The sine of <paramref name="a" />. If <paramref name="a" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Sin(double a);

    /// <summary>Returns the sine and cosine of the specified angle.</summary>
    /// <param name="x">An angle, measured in radians.</param>
    /// <returns>The sine and cosine of <paramref name="x" />. If <paramref name="x" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
    public static unsafe (double Sin, double Cos) SinCos(double x)
    {
      double num1;
      double num2;
      Math.SinCos(x, &num1, &num2);
      return (num1, num2);
    }

    /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
    /// <param name="value">An angle, measured in radians.</param>
    /// <returns>The hyperbolic sine of <paramref name="value" />. If <paramref name="value" /> is equal to <see cref="F:System.Double.NegativeInfinity" />, <see cref="F:System.Double.PositiveInfinity" />, or <see cref="F:System.Double.NaN" />, this method returns a <see cref="T:System.Double" /> equal to <paramref name="value" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Sinh(double value);

    /// <summary>Returns the square root of a specified number.</summary>
    /// <param name="d">The number whose square root is to be found.</param>
    /// <returns>One of the values in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="d" /> parameter</term><description> Return value</description></listheader><item><term> Zero or positive</term><description> The positive square root of <paramref name="d" />.</description></item><item><term> Negative</term><description><see cref="F:System.Double.NaN" /></description></item><item><term> Equals <see cref="F:System.Double.NaN" /></term><description><see cref="F:System.Double.NaN" /></description></item><item><term> Equals <see cref="F:System.Double.PositiveInfinity" /></term><description><see cref="F:System.Double.PositiveInfinity" /></description></item></list></returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Sqrt(double d);

    /// <summary>Returns the tangent of the specified angle.</summary>
    /// <param name="a">An angle, measured in radians.</param>
    /// <returns>The tangent of <paramref name="a" />. If <paramref name="a" /> is equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NegativeInfinity" />, or <see cref="F:System.Double.PositiveInfinity" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Tan(double a);

    /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
    /// <param name="value">An angle, measured in radians.</param>
    /// <returns>The hyperbolic tangent of <paramref name="value" />. If <paramref name="value" /> is equal to <see cref="F:System.Double.NegativeInfinity" />, this method returns -1. If value is equal to <see cref="F:System.Double.PositiveInfinity" />, this method returns 1. If <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />, this method returns <see cref="F:System.Double.NaN" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern double Tanh(double value);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern unsafe double ModF(double x, double* intptr);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern unsafe void SinCos(double x, double* sin, double* cos);

    /// <summary>Returns the absolute value of a 16-bit signed integer.</summary>
    /// <param name="value">A number that is greater than <see cref="F:System.Int16.MinValue" />, but less than or equal to <see cref="F:System.Int16.MaxValue" />.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> equals <see cref="F:System.Int16.MinValue" />.</exception>
    /// <returns>A 16-bit signed integer, x, such that 0 ≤ x ≤ <see cref="F:System.Int16.MaxValue" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short Abs(short value)
    {
      if (value < (short) 0)
      {
        value = -value;
        if (value < (short) 0)
          Math.ThrowAbsOverflow();
      }
      return value;
    }

    /// <summary>Returns the absolute value of a 32-bit signed integer.</summary>
    /// <param name="value">A number that is greater than <see cref="F:System.Int32.MinValue" />, but less than or equal to <see cref="F:System.Int32.MaxValue" />.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> equals <see cref="F:System.Int32.MinValue" />.</exception>
    /// <returns>A 32-bit signed integer, x, such that 0 ≤ x ≤ <see cref="F:System.Int32.MaxValue" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(int value)
    {
      if (value < 0)
      {
        value = -value;
        if (value < 0)
          Math.ThrowAbsOverflow();
      }
      return value;
    }

    /// <summary>Returns the absolute value of a 64-bit signed integer.</summary>
    /// <param name="value">A number that is greater than <see cref="F:System.Int64.MinValue" />, but less than or equal to <see cref="F:System.Int64.MaxValue" />.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> equals <see cref="F:System.Int64.MinValue" />.</exception>
    /// <returns>A 64-bit signed integer, x, such that 0 ≤ x ≤ <see cref="F:System.Int64.MaxValue" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Abs(long value)
    {
      if (value < 0L)
      {
        value = -value;
        if (value < 0L)
          Math.ThrowAbsOverflow();
      }
      return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    public static IntPtr Abs([NativeInteger] IntPtr value)
    {
      if (value < IntPtr.Zero)
      {
        value = -value;
        if (value < IntPtr.Zero)
          Math.ThrowAbsOverflow();
      }
      return value;
    }

    /// <summary>Returns the absolute value of an 8-bit signed integer.</summary>
    /// <param name="value">A number that is greater than <see cref="F:System.SByte.MinValue" />, but less than or equal to <see cref="F:System.SByte.MaxValue" />.</param>
    /// <exception cref="T:System.OverflowException">
    /// <paramref name="value" /> equals <see cref="F:System.SByte.MinValue" />.</exception>
    /// <returns>An 8-bit signed integer, x, such that 0 ≤ x ≤ <see cref="F:System.SByte.MaxValue" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte Abs(sbyte value)
    {
      if (value < (sbyte) 0)
      {
        value = -value;
        if (value < (sbyte) 0)
          Math.ThrowAbsOverflow();
      }
      return value;
    }

    /// <summary>Returns the absolute value of a <see cref="T:System.Decimal" /> number.</summary>
    /// <param name="value">A number that is greater than or equal to <see cref="F:System.Decimal.MinValue" />, but less than or equal to <see cref="F:System.Decimal.MaxValue" />.</param>
    /// <returns>A decimal number, x, such that 0 ≤ x ≤ <see cref="F:System.Decimal.MaxValue" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Abs(Decimal value) => Decimal.Abs(in value);

    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowAbsOverflow() => throw new OverflowException(SR.Overflow_NegateTwosCompNum);

    /// <summary>Produces the full product of two 32-bit numbers.</summary>
    /// <param name="a">The first number to multiply.</param>
    /// <param name="b">The second number to multiply.</param>
    /// <returns>The number containing the product of the specified numbers.</returns>
    public static long BigMul(int a, int b) => (long) a * (long) b;

    /// <summary>Produces the full product of two unsigned 64-bit numbers.</summary>
    /// <param name="a">The first number to multiply.</param>
    /// <param name="b">The second number to multiply.</param>
    /// <param name="low">The low 64-bit of the product of the specified numbers.</param>
    /// <returns>The high 64-bit of the product of the specied numbers.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe ulong BigMul(ulong a, ulong b, out ulong low)
    {
      if (Bmi2.X64.IsSupported)
      {
        ulong num1;
        ulong num2 = Bmi2.X64.MultiplyNoFlags(a, b, &num1);
        low = num1;
        return num2;
      }
      if (!ArmBase.Arm64.IsSupported)
        ;
      return SoftwareFallback(a, b, out low);

      static ulong SoftwareFallback(ulong a, ulong b, out ulong low)
      {
        uint num1 = (uint) a;
        uint num2 = (uint) (a >> 32);
        uint num3 = (uint) b;
        uint num4 = (uint) (b >> 32);
        ulong num5 = (ulong) num1 * (ulong) num3;
        ulong num6 = (ulong) num2 * (ulong) num3 + (num5 >> 32);
        ulong num7 = (ulong) num1 * (ulong) num4 + (ulong) (uint) num6;
        low = num7 << 32 | (ulong) (uint) num5;
        return (ulong) num2 * (ulong) num4 + (num6 >> 32) + (num7 >> 32);
      }
    }

    /// <summary>Produces the full product of two 64-bit numbers.</summary>
    /// <param name="a">The first number to multiply.</param>
    /// <param name="b">The second number to multiply.</param>
    /// <param name="low">The low 64-bit of the product of the specified numbers.</param>
    /// <returns>The high 64-bit of the product of the specied numbers.</returns>
    public static long BigMul(long a, long b, out long low)
    {
      if (!ArmBase.Arm64.IsSupported)
        ;
      ulong low1;
      ulong num = Math.BigMul((ulong) a, (ulong) b, out low1);
      low = (long) low1;
      return (long) num - (a >> 63 & b) - (b >> 63 & a);
    }

    /// <summary>Returns the next smallest value that compares less than <paramref name="x" />.</summary>
    /// <param name="x">The value to decrement.</param>
    /// <returns>The next smallest value that compares less than <paramref name="x" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NegativeInfinity" /> if <paramref name="x" /> equals <see cref="F:System.Double.NegativeInfinity" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="x" /> equals <see cref="F:System.Double.NaN" />.</returns>
    public static double BitDecrement(double x)
    {
      long int64Bits = BitConverter.DoubleToInt64Bits(x);
      return (int64Bits >> 32 & 2146435072L) >= 2146435072L ? (int64Bits != 9218868437227405312L ? x : double.MaxValue) : (int64Bits == 0L ? -5E-324 : BitConverter.Int64BitsToDouble(int64Bits + (int64Bits < 0L ? 1L : -1L)));
    }

    /// <summary>Returns the next largest value that compares greater than <paramref name="x" />.</summary>
    /// <param name="x">The value to increment.</param>
    /// <returns>The next largest value that compares greater than <paramref name="x" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.PositiveInfinity" /> if <paramref name="x" /> equals <see cref="F:System.Double.PositiveInfinity" />.
    /// 
    /// -or-
    /// 
    /// <see cref="F:System.Double.NaN" /> if <paramref name="x" /> equals <see cref="F:System.Double.NaN" />.</returns>
    public static double BitIncrement(double x)
    {
      long int64Bits = BitConverter.DoubleToInt64Bits(x);
      return (int64Bits >> 32 & 2146435072L) >= 2146435072L ? (int64Bits != -4503599627370496L ? x : double.MinValue) : (int64Bits == long.MinValue ? double.Epsilon : BitConverter.Int64BitsToDouble(int64Bits + (int64Bits < 0L ? -1L : 1L)));
    }

    /// <summary>Returns a value with the magnitude of <paramref name="x" /> and the sign of <paramref name="y" />.</summary>
    /// <param name="x">A number whose magnitude is used in the result.</param>
    /// <param name="y">A number whose sign is the used in the result.</param>
    /// <returns>A value with the magnitude of <paramref name="x" /> and the sign of <paramref name="y" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double CopySign(double x, double y)
    {
      return Sse2.IsSupported || AdvSimd.IsSupported ? VectorMath.ConditionalSelectBitwise(Vector128.CreateScalarUnsafe(-0.0), Vector128.CreateScalarUnsafe(y), Vector128.CreateScalarUnsafe(x)).ToScalar<double>() : SoftwareFallback(x, y);

      static double SoftwareFallback(double x, double y) => BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(x) & long.MaxValue | BitConverter.DoubleToInt64Bits(y) & long.MinValue);
    }

    /// <summary>Calculates the quotient of two 32-bit signed integers and also returns the remainder in an output parameter.</summary>
    /// <param name="a">The dividend.</param>
    /// <param name="b">The divisor.</param>
    /// <param name="result">The remainder.</param>
    /// <exception cref="T:System.DivideByZeroException">
    /// <paramref name="b" /> is zero.</exception>
    /// <returns>The quotient of the specified numbers.</returns>
    public static int DivRem(int a, int b, out int result)
    {
      int num = a / b;
      result = a - num * b;
      return num;
    }

    /// <summary>Calculates the quotient of two 64-bit signed integers and also returns the remainder in an output parameter.</summary>
    /// <param name="a">The dividend.</param>
    /// <param name="b">The divisor.</param>
    /// <param name="result">The remainder.</param>
    /// <exception cref="T:System.DivideByZeroException">
    /// <paramref name="b" /> is zero.</exception>
    /// <returns>The quotient of the specified numbers.</returns>
    public static long DivRem(long a, long b, out long result)
    {
      long num = a / b;
      result = a - num * b;
      return num;
    }

    /// <summary>Produces the quotient and the remainder of two signed 8-bit numbers.</summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The quotient and the remainder of the specified numbers.</returns>
    [NonVersionable]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (sbyte Quotient, sbyte Remainder) DivRem(sbyte left, sbyte right)
    {
      sbyte num = (sbyte) ((int) left / (int) right);
      return (num, (sbyte) ((int) left - (int) num * (int) right));
    }

    /// <summary>Produces the quotient and the remainder of two unsigned 8-bit numbers.</summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The quotient and the remainder of the specified numbers.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (byte Quotient, byte Remainder) DivRem(byte left, byte right)
    {
      byte num = (byte) ((uint) left / (uint) right);
      return (num, (byte) ((uint) left - (uint) num * (uint) right));
    }

    /// <summary>Produces the quotient and the remainder of two signed 16-bit numbers.</summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The quotient and the remainder of the specified numbers.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (short Quotient, short Remainder) DivRem(short left, short right)
    {
      short num = (short) ((int) left / (int) right);
      return (num, (short) ((int) left - (int) num * (int) right));
    }

    /// <summary>Produces the quotient and the remainder of two unsigned 16-bit numbers.</summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The quotient and the remainder of the specified numbers.</returns>
    [NonVersionable]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (ushort Quotient, ushort Remainder) DivRem(ushort left, ushort right)
    {
      ushort num = (ushort) ((uint) left / (uint) right);
      return (num, (ushort) ((uint) left - (uint) num * (uint) right));
    }

    /// <summary>Produces the quotient and the remainder of two signed 32-bit numbers.</summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The quotient and the remainder of the specified numbers.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int Quotient, int Remainder) DivRem(int left, int right)
    {
      int num = left / right;
      return (num, left - num * right);
    }

    /// <summary>Produces the quotient and the remainder of two unsigned 32-bit numbers.</summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The quotient and the remainder of the specified numbers.</returns>
    [NonVersionable]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (uint Quotient, uint Remainder) DivRem(uint left, uint right)
    {
      uint num = left / right;
      return (num, left - num * right);
    }

    /// <summary>Produces the quotient and the remainder of two signed 64-bit numbers.</summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The quotient and the remainder of the specified numbers.</returns>
    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (long Quotient, long Remainder) DivRem(long left, long right)
    {
      long num = left / right;
      return (num, left - num * right);
    }

    /// <summary>Produces the quotient and the remainder of two unsigned 64-bit numbers.</summary>
    /// <param name="left">The dividend.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>The quotient and the remainder of the specified numbers.</returns>
    [NonVersionable]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (ulong Quotient, ulong Remainder) DivRem(ulong left, ulong right)
    {
      ulong num = left / right;
      return (num, left - num * right);
    }

    [NonVersionable]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger(new bool[] {true, true})]
    public static (IntPtr Quotient, IntPtr Remainder) DivRem([NativeInteger] IntPtr left, [NativeInteger] IntPtr right)
    {
      IntPtr num = left / right;
      return (num, left - num * right);
    }

    [NonVersionable]
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger(new bool[] {true, true})]
    public static (UIntPtr Quotient, UIntPtr Remainder) DivRem(
      [NativeInteger] UIntPtr left,
      [NativeInteger] UIntPtr right)
    {
      UIntPtr num = left / right;
      return (num, left - num * right);
    }

    /// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
    /// <param name="d">A decimal number.</param>
    /// <returns>The smallest integral value that is greater than or equal to <paramref name="d" />. Note that this method returns a <see cref="T:System.Decimal" /> instead of an integral type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Ceiling(Decimal d) => Decimal.Ceiling(d);

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Clamp(byte value, byte min, byte max)
    {
      if ((int) min > (int) max)
        Math.ThrowMinMaxException<byte>(min, max);
      if ((int) value < (int) min)
        return min;
      return (int) value > (int) max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt;<paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Clamp(Decimal value, Decimal min, Decimal max)
    {
      if (min > max)
        Math.ThrowMinMaxException<Decimal>(min, max);
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.
    /// 
    /// -or-
    /// 
    ///  <see cref="F:System.Double.NaN" /> if <paramref name="value" /> equals <see cref="F:System.Double.NaN" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Clamp(double value, double min, double max)
    {
      if (min > max)
        Math.ThrowMinMaxException<double>(min, max);
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short Clamp(short value, short min, short max)
    {
      if ((int) min > (int) max)
        Math.ThrowMinMaxException<short>(min, max);
      if ((int) value < (int) min)
        return min;
      return (int) value > (int) max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
    {
      if (min > max)
        Math.ThrowMinMaxException<int>(min, max);
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Clamp(long value, long min, long max)
    {
      if (min > max)
        Math.ThrowMinMaxException<long>(min, max);
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    public static IntPtr Clamp([NativeInteger] IntPtr value, [NativeInteger] IntPtr min, [NativeInteger] IntPtr max)
    {
      if (min > max)
        Math.ThrowMinMaxException<IntPtr>(min, max);
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte Clamp(sbyte value, sbyte min, sbyte max)
    {
      if ((int) min > (int) max)
        Math.ThrowMinMaxException<sbyte>(min, max);
      if ((int) value < (int) min)
        return min;
      return (int) value > (int) max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.
    /// 
    /// -or-
    /// 
    ///  <see cref="F:System.Single.NaN" /> if <paramref name="value" /> equals <see cref="F:System.Single.NaN" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max)
    {
      if ((double) min > (double) max)
        Math.ThrowMinMaxException<float>(min, max);
      if ((double) value < (double) min)
        return min;
      return (double) value > (double) max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Clamp(ushort value, ushort min, ushort max)
    {
      if ((int) min > (int) max)
        Math.ThrowMinMaxException<ushort>(min, max);
      if ((int) value < (int) min)
        return min;
      return (int) value > (int) max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Clamp(uint value, uint min, uint max)
    {
      if (min > max)
        Math.ThrowMinMaxException<uint>(min, max);
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    /// <summary>Returns <paramref name="value" /> clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.</summary>
    /// <param name="value">The value to be clamped.</param>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        <paramref name="value" /> if <paramref name="min" /> ≤ <paramref name="value" /> ≤ <paramref name="max" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="min" /> if <paramref name="value" /> &lt; <paramref name="min" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="max" /> if <paramref name="max" /> &lt; <paramref name="value" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Clamp(ulong value, ulong min, ulong max)
    {
      if (min > max)
        Math.ThrowMinMaxException<ulong>(min, max);
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NativeInteger]
    public static UIntPtr Clamp([NativeInteger] UIntPtr value, [NativeInteger] UIntPtr min, [NativeInteger] UIntPtr max)
    {
      if (min > max)
        Math.ThrowMinMaxException<UIntPtr>(min, max);
      if (value < min)
        return min;
      return value > max ? max : value;
    }

    /// <summary>Returns the largest integral value less than or equal to the specified decimal number.</summary>
    /// <param name="d">A decimal number.</param>
    /// <returns>The largest integral value less than or equal to <paramref name="d" />.  Note that the method returns an integral value of type <see cref="T:System.Decimal" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Floor(Decimal d) => Decimal.Floor(d);

    /// <summary>Returns the remainder resulting from the division of a specified number by another specified number.</summary>
    /// <param name="x">A dividend.</param>
    /// <param name="y">A divisor.</param>
    /// <returns>A number equal to <paramref name="x" /> - (<paramref name="y" /> Q), where Q is the quotient of <paramref name="x" /> / <paramref name="y" /> rounded to the nearest integer (if <paramref name="x" /> / <paramref name="y" /> falls halfway between two integers, the even integer is returned).
    /// 
    /// If <paramref name="x" /> - (<paramref name="y" /> Q) is zero, the value +0 is returned if <paramref name="x" /> is positive, or -0 if <paramref name="x" /> is negative.
    /// 
    /// If <paramref name="y" /> = 0, <see cref="F:System.Double.NaN" /> is returned.</returns>
    public static double IEEERemainder(double x, double y)
    {
      if (double.IsNaN(x))
        return x;
      if (double.IsNaN(y))
        return y;
      double d = x % y;
      if (double.IsNaN(d))
        return double.NaN;
      if (d == 0.0 && double.IsNegative(x))
        return -0.0;
      double num = d - Math.Abs(y) * (double) Math.Sign(x);
      if (Math.Abs(num) == Math.Abs(d))
      {
        double a = x / y;
        return Math.Abs(Math.Round(a)) > Math.Abs(a) ? num : d;
      }
      return Math.Abs(num) < Math.Abs(d) ? num : d;
    }

    /// <summary>Returns the logarithm of a specified number in a specified base.</summary>
    /// <param name="a">The number whose logarithm is to be found.</param>
    /// <param name="newBase">The base of the logarithm.</param>
    /// <returns>One of the values in the following table. (+Infinity denotes <see cref="F:System.Double.PositiveInfinity" />, -Infinity denotes <see cref="F:System.Double.NegativeInfinity" />, and NaN denotes <see cref="F:System.Double.NaN" />.)
    /// 
    /// <list type="table"><listheader><term><paramref name="a" /></term><description><paramref name="newBase" /></description><description> Return value</description></listheader><item><term><paramref name="a" />&gt; 0</term><description> (0 &lt;<paramref name="newBase" />&lt; 1) -or-(<paramref name="newBase" />&gt; 1)</description><description> lognewBase(a)</description></item><item><term><paramref name="a" />&lt; 0</term><description> (any value)</description><description> NaN</description></item><item><term> (any value)</term><description><paramref name="newBase" />&lt; 0</description><description> NaN</description></item><item><term><paramref name="a" /> != 1</term><description><paramref name="newBase" /> = 0</description><description> NaN</description></item><item><term><paramref name="a" /> != 1</term><description><paramref name="newBase" /> = +Infinity</description><description> NaN</description></item><item><term><paramref name="a" /> = NaN</term><description> (any value)</description><description> NaN</description></item><item><term> (any value)</term><description><paramref name="newBase" /> = NaN</description><description> NaN</description></item><item><term> (any value)</term><description><paramref name="newBase" /> = 1</description><description> NaN</description></item><item><term><paramref name="a" /> = 0</term><description> 0 &lt;<paramref name="newBase" />&lt; 1</description><description> +Infinity</description></item><item><term><paramref name="a" /> = 0</term><description><paramref name="newBase" />&gt; 1</description><description> -Infinity</description></item><item><term><paramref name="a" /> =  +Infinity</term><description> 0 &lt;<paramref name="newBase" />&lt; 1</description><description> -Infinity</description></item><item><term><paramref name="a" /> =  +Infinity</term><description><paramref name="newBase" />&gt; 1</description><description> +Infinity</description></item><item><term><paramref name="a" /> = 1</term><description><paramref name="newBase" /> = 0</description><description> 0</description></item><item><term><paramref name="a" /> = 1</term><description><paramref name="newBase" /> = +Infinity</description><description> 0</description></item></list></returns>
    public static double Log(double a, double newBase)
    {
      if (double.IsNaN(a))
        return a;
      if (double.IsNaN(newBase))
        return newBase;
      return newBase == 1.0 || a != 1.0 && (newBase == 0.0 || double.IsPositiveInfinity(newBase)) ? double.NaN : Math.Log(a) / Math.Log(newBase);
    }

    /// <summary>Returns the larger of two 8-bit unsigned integers.</summary>
    /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [NonVersionable]
    public static byte Max(byte val1, byte val2) => (int) val1 < (int) val2 ? val2 : val1;

    /// <summary>Returns the larger of two decimal numbers.</summary>
    /// <param name="val1">The first of two decimal numbers to compare.</param>
    /// <param name="val2">The second of two decimal numbers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Max(Decimal val1, Decimal val2) => Decimal.Max(in val1, in val2);

    /// <summary>Returns the larger of two double-precision floating-point numbers.</summary>
    /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
    /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger. If <paramref name="val1" />, <paramref name="val2" />, or both <paramref name="val1" /> and <paramref name="val2" /> are equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" /> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Max(double val1, double val2) => val1 != val2 ? (!double.IsNaN(val1) && val2 >= val1 ? val2 : val1) : (!double.IsNegative(val2) ? val2 : val1);

    /// <summary>Returns the larger of two 16-bit signed integers.</summary>
    /// <param name="val1">The first of two 16-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 16-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [NonVersionable]
    public static short Max(short val1, short val2) => (int) val1 < (int) val2 ? val2 : val1;

    /// <summary>Returns the larger of two 32-bit signed integers.</summary>
    /// <param name="val1">The first of two 32-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 32-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [NonVersionable]
    public static int Max(int val1, int val2) => val1 < val2 ? val2 : val1;

    /// <summary>Returns the larger of two 64-bit signed integers.</summary>
    /// <param name="val1">The first of two 64-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 64-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [NonVersionable]
    public static long Max(long val1, long val2) => val1 < val2 ? val2 : val1;

    [NonVersionable]
    [return: NativeInteger]
    public static IntPtr Max([NativeInteger] IntPtr val1, [NativeInteger] IntPtr val2) => val1 < val2 ? val2 : val1;

    /// <summary>Returns the larger of two 8-bit signed integers.</summary>
    /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static sbyte Max(sbyte val1, sbyte val2) => (int) val1 < (int) val2 ? val2 : val1;

    /// <summary>Returns the larger of two single-precision floating-point numbers.</summary>
    /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
    /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger. If <paramref name="val1" />, or <paramref name="val2" />, or both <paramref name="val1" /> and <paramref name="val2" /> are equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Max(float val1, float val2) => (double) val1 != (double) val2 ? (!float.IsNaN(val1) && (double) val2 >= (double) val1 ? val2 : val1) : (!float.IsNegative(val2) ? val2 : val1);

    /// <summary>Returns the larger of two 16-bit unsigned integers.</summary>
    /// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static ushort Max(ushort val1, ushort val2) => (int) val1 < (int) val2 ? val2 : val1;

    /// <summary>Returns the larger of two 32-bit unsigned integers.</summary>
    /// <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static uint Max(uint val1, uint val2) => val1 < val2 ? val2 : val1;

    /// <summary>Returns the larger of two 64-bit unsigned integers.</summary>
    /// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is larger.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static ulong Max(ulong val1, ulong val2) => val1 < val2 ? val2 : val1;

    [CLSCompliant(false)]
    [NonVersionable]
    [return: NativeInteger]
    public static UIntPtr Max([NativeInteger] UIntPtr val1, [NativeInteger] UIntPtr val2) => val1 < val2 ? val2 : val1;

    /// <summary>Returns the larger magnitude of two double-precision floating-point numbers.</summary>
    /// <param name="x">The first of two double-precision floating-point numbers to compare.</param>
    /// <param name="y">The second of two double-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="x" /> or <paramref name="y" />, whichever has the larger magnitude. If <paramref name="x" />, or <paramref name="y" />, or both <paramref name="x" /> and <paramref name="y" /> are equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" /> is returned.</returns>
    public static double MaxMagnitude(double x, double y)
    {
      double d = Math.Abs(x);
      double num = Math.Abs(y);
      return d > num || double.IsNaN(d) || d == num && !double.IsNegative(x) ? x : y;
    }

    /// <summary>Returns the smaller of two 8-bit unsigned integers.</summary>
    /// <param name="val1">The first of two 8-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 8-bit unsigned integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [NonVersionable]
    public static byte Min(byte val1, byte val2) => (int) val1 > (int) val2 ? val2 : val1;

    /// <summary>Returns the smaller of two decimal numbers.</summary>
    /// <param name="val1">The first of two decimal numbers to compare.</param>
    /// <param name="val2">The second of two decimal numbers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Min(Decimal val1, Decimal val2) => Decimal.Min(in val1, in val2);

    /// <summary>Returns the smaller of two double-precision floating-point numbers.</summary>
    /// <param name="val1">The first of two double-precision floating-point numbers to compare.</param>
    /// <param name="val2">The second of two double-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller. If <paramref name="val1" />, <paramref name="val2" />, or both <paramref name="val1" /> and <paramref name="val2" /> are equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" /> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Min(double val1, double val2) => val1 != val2 ? (!double.IsNaN(val1) && val1 >= val2 ? val2 : val1) : (!double.IsNegative(val1) ? val2 : val1);

    /// <summary>Returns the smaller of two 16-bit signed integers.</summary>
    /// <param name="val1">The first of two 16-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 16-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [NonVersionable]
    public static short Min(short val1, short val2) => (int) val1 > (int) val2 ? val2 : val1;

    /// <summary>Returns the smaller of two 32-bit signed integers.</summary>
    /// <param name="val1">The first of two 32-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 32-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [NonVersionable]
    public static int Min(int val1, int val2) => val1 > val2 ? val2 : val1;

    /// <summary>Returns the smaller of two 64-bit signed integers.</summary>
    /// <param name="val1">The first of two 64-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 64-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [NonVersionable]
    public static long Min(long val1, long val2) => val1 > val2 ? val2 : val1;

    [NonVersionable]
    [return: NativeInteger]
    public static IntPtr Min([NativeInteger] IntPtr val1, [NativeInteger] IntPtr val2) => val1 > val2 ? val2 : val1;

    /// <summary>Returns the smaller of two 8-bit signed integers.</summary>
    /// <param name="val1">The first of two 8-bit signed integers to compare.</param>
    /// <param name="val2">The second of two 8-bit signed integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static sbyte Min(sbyte val1, sbyte val2) => (int) val1 > (int) val2 ? val2 : val1;

    /// <summary>Returns the smaller of two single-precision floating-point numbers.</summary>
    /// <param name="val1">The first of two single-precision floating-point numbers to compare.</param>
    /// <param name="val2">The second of two single-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller. If <paramref name="val1" />, <paramref name="val2" />, or both <paramref name="val1" /> and <paramref name="val2" /> are equal to <see cref="F:System.Single.NaN" />, <see cref="F:System.Single.NaN" /> is returned.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Min(float val1, float val2) => (double) val1 != (double) val2 ? (!float.IsNaN(val1) && (double) val1 >= (double) val2 ? val2 : val1) : (!float.IsNegative(val1) ? val2 : val1);

    /// <summary>Returns the smaller of two 16-bit unsigned integers.</summary>
    /// <param name="val1">The first of two 16-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 16-bit unsigned integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static ushort Min(ushort val1, ushort val2) => (int) val1 > (int) val2 ? val2 : val1;

    /// <summary>Returns the smaller of two 32-bit unsigned integers.</summary>
    /// <param name="val1">The first of two 32-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 32-bit unsigned integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static uint Min(uint val1, uint val2) => val1 > val2 ? val2 : val1;

    /// <summary>Returns the smaller of two 64-bit unsigned integers.</summary>
    /// <param name="val1">The first of two 64-bit unsigned integers to compare.</param>
    /// <param name="val2">The second of two 64-bit unsigned integers to compare.</param>
    /// <returns>Parameter <paramref name="val1" /> or <paramref name="val2" />, whichever is smaller.</returns>
    [CLSCompliant(false)]
    [NonVersionable]
    public static ulong Min(ulong val1, ulong val2) => val1 > val2 ? val2 : val1;

    [CLSCompliant(false)]
    [NonVersionable]
    [return: NativeInteger]
    public static UIntPtr Min([NativeInteger] UIntPtr val1, [NativeInteger] UIntPtr val2) => val1 > val2 ? val2 : val1;

    /// <summary>Returns the smaller magnitude of two double-precision floating-point numbers.</summary>
    /// <param name="x">The first of two double-precision floating-point numbers to compare.</param>
    /// <param name="y">The second of two double-precision floating-point numbers to compare.</param>
    /// <returns>Parameter <paramref name="x" /> or <paramref name="y" />, whichever has the smaller magnitude. If <paramref name="x" />, or <paramref name="y" />, or both <paramref name="x" /> and <paramref name="y" /> are equal to <see cref="F:System.Double.NaN" />, <see cref="F:System.Double.NaN" /> is returned.</returns>
    public static double MinMagnitude(double x, double y)
    {
      double d = Math.Abs(x);
      double num = Math.Abs(y);
      return d < num || double.IsNaN(d) || d == num && double.IsNegative(x) ? x : y;
    }

    /// <summary>Returns an estimate of the reciprocal of a specified number.</summary>
    /// <param name="d">The number whose reciprocal is to be estimated.</param>
    /// <returns>An estimate of the reciprocal of <paramref name="d" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReciprocalEstimate(double d)
    {
      if (!AdvSimd.Arm64.IsSupported)
        ;
      return 1.0 / d;
    }

    /// <summary>Returns an estimate of the reciprocal square root of a specified number.</summary>
    /// <param name="d">The number whose reciprocal square root is to be estimated.</param>
    /// <returns>An estimate of the reciprocal square root <paramref name="d" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReciprocalSqrtEstimate(double d)
    {
      if (!AdvSimd.Arm64.IsSupported)
        ;
      return 1.0 / Math.Sqrt(d);
    }

    /// <summary>Rounds a decimal value to the nearest integral value, and rounds midpoint values to the nearest even number.</summary>
    /// <param name="d">A decimal number to be rounded.</param>
    /// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" />.</exception>
    /// <returns>The integer nearest the <paramref name="d" /> parameter. If the fractional component of <paramref name="d" /> is halfway between two integers, one of which is even and the other odd, the even number is returned. Note that this method returns a <see cref="T:System.Decimal" /> instead of an integral type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Round(Decimal d) => Decimal.Round(d, 0);

    /// <summary>Rounds a decimal value to a specified number of fractional digits, and rounds midpoint values to the nearest even number.</summary>
    /// <param name="d">A decimal number to be rounded.</param>
    /// <param name="decimals">The number of decimal places in the return value.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="decimals" /> is less than 0 or greater than 28.</exception>
    /// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" />.</exception>
    /// <returns>The number nearest to <paramref name="d" /> that contains a number of fractional digits equal to <paramref name="decimals" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Round(Decimal d, int decimals) => Decimal.Round(d, decimals);

    /// <summary>Rounds a decimal value an integer using the specified rounding convention.</summary>
    /// <param name="d">A decimal number to be rounded.</param>
    /// <param name="mode">One of the enumeration values that specifies which rounding strategy to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
    /// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" />.</exception>
    /// <returns>The integer that <paramref name="d" /> is rounded to. This method returns a <see cref="T:System.Decimal" /> instead of an integral type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Round(Decimal d, MidpointRounding mode) => Decimal.Round(d, 0, mode);

    /// <summary>Rounds a decimal value to a specified number of fractional digits using the specified rounding convention.</summary>
    /// <param name="d">A decimal number to be rounded.</param>
    /// <param name="decimals">The number of decimal places in the return value.</param>
    /// <param name="mode">One of the enumeration values that specifies which rounding strategy to use.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="decimals" /> is less than 0 or greater than 28.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
    /// <exception cref="T:System.OverflowException">The result is outside the range of a <see cref="T:System.Decimal" />.</exception>
    /// <returns>The number with <paramref name="decimals" /> fractional digits that <paramref name="d" /> is rounded to. If <paramref name="d" /> has fewer fractional digits than <paramref name="decimals" />, <paramref name="d" /> is returned unchanged.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Round(Decimal d, int decimals, MidpointRounding mode) => Decimal.Round(d, decimals, mode);

    /// <summary>Rounds a double-precision floating-point value to the nearest integral value, and rounds midpoint values to the nearest even number.</summary>
    /// <param name="a">A double-precision floating-point number to be rounded.</param>
    /// <returns>The integer nearest <paramref name="a" />. If the fractional component of <paramref name="a" /> is halfway between two integers, one of which is even and the other odd, then the even number is returned. Note that this method returns a <see cref="T:System.Double" /> instead of an integral type.</returns>
    [Intrinsic]
    public static double Round(double a)
    {
      ulong uint64Bits = BitConverter.DoubleToUInt64Bits(a);
      int exponentFromBits = double.ExtractExponentFromBits(uint64Bits);
      if (exponentFromBits <= 1022)
        return (long) uint64Bits << 1 == 0L ? a : Math.CopySign(exponentFromBits != 1022 || double.ExtractSignificandFromBits(uint64Bits) == 0UL ? 0.0 : 1.0, a);
      if (exponentFromBits >= 1075)
        return a;
      ulong num1 = 1UL << 1075 - exponentFromBits;
      ulong num2 = num1 - 1UL;
      ulong num3 = uint64Bits + (num1 >> 1);
      return BitConverter.UInt64BitsToDouble(((long) num3 & (long) num2) != 0L ? num3 & ~num2 : num3 & ~num1);
    }

    /// <summary>Rounds a double-precision floating-point value to a specified number of fractional digits, and rounds midpoint values to the nearest even number.</summary>
    /// <param name="value">A double-precision floating-point number to be rounded.</param>
    /// <param name="digits">The number of fractional digits in the return value.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="digits" /> is less than 0 or greater than 15.</exception>
    /// <returns>The number nearest to <paramref name="value" /> that contains a number of fractional digits equal to <paramref name="digits" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Round(double value, int digits) => Math.Round(value, digits, MidpointRounding.ToEven);

    /// <summary>Rounds a double-precision floating-point value to an integer using the specified rounding convention.</summary>
    /// <param name="value">A double-precision floating-point number to be rounded.</param>
    /// <param name="mode">One of the enumeration values that specifies which rounding strategy to use.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
    /// <returns>The integer that <paramref name="value" /> is rounded to. This method returns a <see cref="T:System.Double" /> instead of an integral type.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Round(double value, MidpointRounding mode) => Math.Round(value, 0, mode);

    /// <summary>Rounds a double-precision floating-point value to a specified number of fractional digits using the specified rounding convention.</summary>
    /// <param name="value">A double-precision floating-point number to be rounded.</param>
    /// <param name="digits">The number of fractional digits in the return value.</param>
    /// <param name="mode">One of the enumeration values that specifies which rounding strategy to use.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="digits" /> is less than 0 or greater than 15.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="mode" /> is not a valid value of <see cref="T:System.MidpointRounding" />.</exception>
    /// <returns>The number that has <paramref name="digits" /> fractional digits that <paramref name="value" /> is rounded to. If <paramref name="value" /> has fewer fractional digits than <paramref name="digits" />, <paramref name="value" /> is returned unchanged.</returns>
    public static unsafe double Round(double value, int digits, MidpointRounding mode)
    {
      if (digits < 0 || digits > 15)
        throw new ArgumentOutOfRangeException(nameof (digits), SR.ArgumentOutOfRange_RoundingDigits);
      if (mode < MidpointRounding.ToEven || mode > MidpointRounding.ToPositiveInfinity)
        throw new ArgumentException(SR.Format(SR.Argument_InvalidEnumValue, (object) mode, (object) "MidpointRounding"), nameof (mode));
      if (Math.Abs(value) < 10000000000000000.0)
      {
        double num1 = Math.roundPower10Double[digits];
        value *= num1;
        switch (mode)
        {
          case MidpointRounding.ToEven:
            value = Math.Round(value);
            break;
          case MidpointRounding.AwayFromZero:
            double num2 = Math.ModF(value, &value);
            if (Math.Abs(num2) >= 0.5)
            {
              value += (double) Math.Sign(num2);
              break;
            }
            break;
          case MidpointRounding.ToZero:
            value = Math.Truncate(value);
            break;
          case MidpointRounding.ToNegativeInfinity:
            value = Math.Floor(value);
            break;
          case MidpointRounding.ToPositiveInfinity:
            value = Math.Ceiling(value);
            break;
          default:
            throw new ArgumentException(SR.Format(SR.Argument_InvalidEnumValue, (object) mode, (object) "MidpointRounding"), nameof (mode));
        }
        value /= num1;
      }
      return value;
    }

    /// <summary>Returns an integer that indicates the sign of a decimal number.</summary>
    /// <param name="value">A signed decimal number.</param>
    /// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> -1</term><description><paramref name="value" /> is less than zero.</description></item><item><term> 0</term><description><paramref name="value" /> is equal to zero.</description></item><item><term> 1</term><description><paramref name="value" /> is greater than zero.</description></item></list></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(Decimal value) => Decimal.Sign(in value);

    /// <summary>Returns an integer that indicates the sign of a double-precision floating-point number.</summary>
    /// <param name="value">A signed number.</param>
    /// <exception cref="T:System.ArithmeticException">
    /// <paramref name="value" /> is equal to <see cref="F:System.Double.NaN" />.</exception>
    /// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> -1</term><description><paramref name="value" /> is less than zero.</description></item><item><term> 0</term><description><paramref name="value" /> is equal to zero.</description></item><item><term> 1</term><description><paramref name="value" /> is greater than zero.</description></item></list></returns>
    public static int Sign(double value)
    {
      if (value < 0.0)
        return -1;
      if (value > 0.0)
        return 1;
      if (value == 0.0)
        return 0;
      throw new ArithmeticException(SR.Arithmetic_NaN);
    }

    /// <summary>Returns an integer that indicates the sign of a 16-bit signed integer.</summary>
    /// <param name="value">A signed number.</param>
    /// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> -1</term><description><paramref name="value" /> is less than zero.</description></item><item><term> 0</term><description><paramref name="value" /> is equal to zero.</description></item><item><term> 1</term><description><paramref name="value" /> is greater than zero.</description></item></list></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(short value) => Math.Sign((int) value);

    /// <summary>Returns an integer that indicates the sign of a 32-bit signed integer.</summary>
    /// <param name="value">A signed number.</param>
    /// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> -1</term><description><paramref name="value" /> is less than zero.</description></item><item><term> 0</term><description><paramref name="value" /> is equal to zero.</description></item><item><term> 1</term><description><paramref name="value" /> is greater than zero.</description></item></list></returns>
    public static int Sign(int value) => value >> 31 | (int) ((uint) -value >> 31);

    /// <summary>Returns an integer that indicates the sign of a 64-bit signed integer.</summary>
    /// <param name="value">A signed number.</param>
    /// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> -1</term><description><paramref name="value" /> is less than zero.</description></item><item><term> 0</term><description><paramref name="value" /> is equal to zero.</description></item><item><term> 1</term><description><paramref name="value" /> is greater than zero.</description></item></list></returns>
    public static int Sign(long value) => (int) (value >> 63 | (long) ((ulong) -value >> 63));

    public static int Sign([NativeInteger] IntPtr value) => (int) ((long) (value >> 63) | (long) ((ulong) (long) -value >> 63));

    /// <summary>Returns an integer that indicates the sign of an 8-bit signed integer.</summary>
    /// <param name="value">A signed number.</param>
    /// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> -1</term><description><paramref name="value" /> is less than zero.</description></item><item><term> 0</term><description><paramref name="value" /> is equal to zero.</description></item><item><term> 1</term><description><paramref name="value" /> is greater than zero.</description></item></list></returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Sign(sbyte value) => Math.Sign((int) value);

    /// <summary>Returns an integer that indicates the sign of a single-precision floating-point number.</summary>
    /// <param name="value">A signed number.</param>
    /// <exception cref="T:System.ArithmeticException">
    /// <paramref name="value" /> is equal to <see cref="F:System.Single.NaN" />.</exception>
    /// <returns>A number that indicates the sign of <paramref name="value" />, as shown in the following table.
    /// 
    /// <list type="table"><listheader><term> Return value</term><description> Meaning</description></listheader><item><term> -1</term><description><paramref name="value" /> is less than zero.</description></item><item><term> 0</term><description><paramref name="value" /> is equal to zero.</description></item><item><term> 1</term><description><paramref name="value" /> is greater than zero.</description></item></list></returns>
    public static int Sign(float value)
    {
      if ((double) value < 0.0)
        return -1;
      if ((double) value > 0.0)
        return 1;
      if ((double) value == 0.0)
        return 0;
      throw new ArithmeticException(SR.Arithmetic_NaN);
    }

    /// <summary>Calculates the integral part of a specified decimal number.</summary>
    /// <param name="d">A number to truncate.</param>
    /// <returns>The integral part of <paramref name="d" />; that is, the number that remains after any fractional digits have been discarded.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Decimal Truncate(Decimal d) => Decimal.Truncate(d);

    /// <summary>Calculates the integral part of a specified double-precision floating-point number.</summary>
    /// <param name="d">A number to truncate.</param>
    /// <returns>The integral part of <paramref name="d" />; that is, the number that remains after any fractional digits have been discarded, or one of the values listed in the following table.
    /// 
    /// <list type="table"><listheader><term><paramref name="d" /></term><description> Return value</description></listheader><item><term><see cref="F:System.Double.NaN" /></term><description><see cref="F:System.Double.NaN" /></description></item><item><term><see cref="F:System.Double.NegativeInfinity" /></term><description><see cref="F:System.Double.NegativeInfinity" /></description></item><item><term><see cref="F:System.Double.PositiveInfinity" /></term><description><see cref="F:System.Double.PositiveInfinity" /></description></item></list></returns>
    public static unsafe double Truncate(double d)
    {
      Math.ModF(d, &d);
      return d;
    }

    [DoesNotReturn]
    private static void ThrowMinMaxException<T>(T min, T max) => throw new ArgumentException(SR.Format(SR.Argument_MinMaxValue, (object) min, (object) max));

    /// <summary>Returns x * 2^n computed efficiently.</summary>
    /// <param name="x">A double-precision floating-point number that specifies the base value.</param>
    /// <param name="n">A 32-bit integer that specifies the power.</param>
    /// <returns>x * 2^n computed efficiently.</returns>
    public static double ScaleB(double x, int n)
    {
      double num1 = x;
      if (n > 1023)
      {
        num1 *= 8.98846567431158E+307;
        n -= 1023;
        if (n > 1023)
        {
          num1 *= 8.98846567431158E+307;
          n -= 1023;
          if (n > 1023)
            n = 1023;
        }
      }
      else if (n < -1022)
      {
        num1 *= 2.004168360008973E-292;
        n += 969;
        if (n < -1022)
        {
          num1 *= 2.004168360008973E-292;
          n += 969;
          if (n < -1022)
            n = -1022;
        }
      }
      double num2 = BitConverter.Int64BitsToDouble((long) (1023 + n) << 52);
      return num1 * num2;
    }
  }
}
