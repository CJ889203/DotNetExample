// Decompiled with JetBrains decompiler
// Type: System.Tuple
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml


#nullable enable
namespace System
{
  /// <summary>Provides static methods for creating tuple objects.</summary>
  public static class Tuple
  {
    /// <summary>Creates a new 1-tuple, or singleton.</summary>
    /// <param name="item1">The value of the only component of the tuple.</param>
    /// <typeparam name="T1">The type of the only component of the tuple.</typeparam>
    /// <returns>A tuple whose value is (<paramref name="item1" />).</returns>
    public static Tuple<T1> Create<T1>(T1 item1) => new Tuple<T1>(item1);

    /// <summary>Creates a new 2-tuple, or pair.</summary>
    /// <param name="item1">The value of the first component of the tuple.</param>
    /// <param name="item2">The value of the second component of the tuple.</param>
    /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
    /// <returns>A 2-tuple whose value is (<paramref name="item1" />, <paramref name="item2" />).</returns>
    public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2) => new Tuple<T1, T2>(item1, item2);

    /// <summary>Creates a new 3-tuple, or triple.</summary>
    /// <param name="item1">The value of the first component of the tuple.</param>
    /// <param name="item2">The value of the second component of the tuple.</param>
    /// <param name="item3">The value of the third component of the tuple.</param>
    /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
    /// <returns>A 3-tuple whose value is (<paramref name="item1" />, <paramref name="item2" />, <paramref name="item3" />).</returns>
    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3) => new Tuple<T1, T2, T3>(item1, item2, item3);

    /// <summary>Creates a new 4-tuple, or quadruple.</summary>
    /// <param name="item1">The value of the first component of the tuple.</param>
    /// <param name="item2">The value of the second component of the tuple.</param>
    /// <param name="item3">The value of the third component of the tuple.</param>
    /// <param name="item4">The value of the fourth component of the tuple.</param>
    /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
    /// <returns>A 4-tuple whose value is (<paramref name="item1" />, <paramref name="item2" />, <paramref name="item3" />, <paramref name="item4" />).</returns>
    public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4)
    {
      return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
    }

    /// <summary>Creates a new 5-tuple, or quintuple.</summary>
    /// <param name="item1">The value of the first component of the tuple.</param>
    /// <param name="item2">The value of the second component of the tuple.</param>
    /// <param name="item3">The value of the third component of the tuple.</param>
    /// <param name="item4">The value of the fourth component of the tuple.</param>
    /// <param name="item5">The value of the fifth component of the tuple.</param>
    /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
    /// <returns>A 5-tuple whose value is (<paramref name="item1" />, <paramref name="item2" />, <paramref name="item3" />, <paramref name="item4" />, <paramref name="item5" />).</returns>
    public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5)
    {
      return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
    }

    /// <summary>Creates a new 6-tuple, or sextuple.</summary>
    /// <param name="item1">The value of the first component of the tuple.</param>
    /// <param name="item2">The value of the second component of the tuple.</param>
    /// <param name="item3">The value of the third component of the tuple.</param>
    /// <param name="item4">The value of the fourth component of the tuple.</param>
    /// <param name="item5">The value of the fifth component of the tuple.</param>
    /// <param name="item6">The value of the sixth component of the tuple.</param>
    /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
    /// <returns>A 6-tuple whose value is (<paramref name="item1" />, <paramref name="item2" />, <paramref name="item3" />, <paramref name="item4" />, <paramref name="item5" />, <paramref name="item6" />).</returns>
    public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5,
      T6 item6)
    {
      return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
    }

    /// <summary>Creates a new 7-tuple, or septuple.</summary>
    /// <param name="item1">The value of the first component of the tuple.</param>
    /// <param name="item2">The value of the second component of the tuple.</param>
    /// <param name="item3">The value of the third component of the tuple.</param>
    /// <param name="item4">The value of the fourth component of the tuple.</param>
    /// <param name="item5">The value of the fifth component of the tuple.</param>
    /// <param name="item6">The value of the sixth component of the tuple.</param>
    /// <param name="item7">The value of the seventh component of the tuple.</param>
    /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh component of the tuple.</typeparam>
    /// <returns>A 7-tuple whose value is (<paramref name="item1" />, <paramref name="item2" />, <paramref name="item3" />, <paramref name="item4" />, <paramref name="item5" />, <paramref name="item6" />, <paramref name="item7" />).</returns>
    public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5,
      T6 item6,
      T7 item7)
    {
      return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
    }

    /// <summary>Creates a new 8-tuple, or octuple.</summary>
    /// <param name="item1">The value of the first component of the tuple.</param>
    /// <param name="item2">The value of the second component of the tuple.</param>
    /// <param name="item3">The value of the third component of the tuple.</param>
    /// <param name="item4">The value of the fourth component of the tuple.</param>
    /// <param name="item5">The value of the fifth component of the tuple.</param>
    /// <param name="item6">The value of the sixth component of the tuple.</param>
    /// <param name="item7">The value of the seventh component of the tuple.</param>
    /// <param name="item8">The value of the eighth component of the tuple.</param>
    /// <typeparam name="T1">The type of the first component of the tuple.</typeparam>
    /// <typeparam name="T2">The type of the second component of the tuple.</typeparam>
    /// <typeparam name="T3">The type of the third component of the tuple.</typeparam>
    /// <typeparam name="T4">The type of the fourth component of the tuple.</typeparam>
    /// <typeparam name="T5">The type of the fifth component of the tuple.</typeparam>
    /// <typeparam name="T6">The type of the sixth component of the tuple.</typeparam>
    /// <typeparam name="T7">The type of the seventh component of the tuple.</typeparam>
    /// <typeparam name="T8">The type of the eighth component of the tuple.</typeparam>
    /// <returns>An 8-tuple (octuple) whose value is (<paramref name="item1" />, <paramref name="item2" />, <paramref name="item3" />, <paramref name="item4" />, <paramref name="item5" />, <paramref name="item6" />, <paramref name="item7" />, <paramref name="item8" />).</returns>
    public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(
      T1 item1,
      T2 item2,
      T3 item3,
      T4 item4,
      T5 item5,
      T6 item6,
      T7 item7,
      T8 item8)
    {
      return new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(item1, item2, item3, item4, item5, item6, item7, new Tuple<T8>(item8));
    }

    internal static int CombineHashCodes(int h1, int h2) => (h1 << 5) + h1 ^ h2;

    internal static int CombineHashCodes(int h1, int h2, int h3) => Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2), h3);

    internal static int CombineHashCodes(int h1, int h2, int h3, int h4) => Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2), Tuple.CombineHashCodes(h3, h4));

    internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5) => Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2, h3, h4), h5);

    internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6) => Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2, h3, h4), Tuple.CombineHashCodes(h5, h6));

    internal static int CombineHashCodes(
      int h1,
      int h2,
      int h3,
      int h4,
      int h5,
      int h6,
      int h7)
    {
      return Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2, h3, h4), Tuple.CombineHashCodes(h5, h6, h7));
    }

    internal static int CombineHashCodes(
      int h1,
      int h2,
      int h3,
      int h4,
      int h5,
      int h6,
      int h7,
      int h8)
    {
      return Tuple.CombineHashCodes(Tuple.CombineHashCodes(h1, h2, h3, h4), Tuple.CombineHashCodes(h5, h6, h7, h8));
    }
  }
}
