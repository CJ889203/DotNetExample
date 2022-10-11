// Decompiled with JetBrains decompiler
// Type: System.INumber`1
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Versioning;


#nullable enable
namespace System
{
  [RequiresPreviewFeatures("Generic Math is in preview.", Url = "https://aka.ms/dotnet-warnings/generic-math-preview")]
  public interface INumber<TSelf> : 
    IAdditionOperators<TSelf, TSelf, TSelf>,
    IAdditiveIdentity<TSelf, TSelf>,
    IComparisonOperators<TSelf, TSelf>,
    IComparable,
    IComparable<TSelf>,
    IEqualityOperators<TSelf, TSelf>,
    IEquatable<TSelf>,
    IDecrementOperators<TSelf>,
    IDivisionOperators<TSelf, TSelf, TSelf>,
    IIncrementOperators<TSelf>,
    IModulusOperators<TSelf, TSelf, TSelf>,
    IMultiplicativeIdentity<TSelf, TSelf>,
    IMultiplyOperators<TSelf, TSelf, TSelf>,
    ISpanFormattable,
    IFormattable,
    ISpanParseable<TSelf>,
    IParseable<TSelf>,
    ISubtractionOperators<TSelf, TSelf, TSelf>,
    IUnaryNegationOperators<TSelf, TSelf>,
    IUnaryPlusOperators<TSelf, TSelf>
    where TSelf : INumber<TSelf>
  {
    static TSelf One { get; }

    static TSelf Zero { get; }

    static TSelf Abs(TSelf value);

    static TSelf Clamp(TSelf value, TSelf min, TSelf max);

    static TSelf Create<TOther>(TOther value) where TOther : INumber<TOther>;

    static TSelf CreateSaturating<TOther>(TOther value) where TOther : INumber<TOther>;

    static TSelf CreateTruncating<TOther>(TOther value) where TOther : INumber<TOther>;

    static (TSelf Quotient, TSelf Remainder) DivRem(TSelf left, TSelf right);

    static TSelf Max(TSelf x, TSelf y);

    static TSelf Min(TSelf x, TSelf y);

    static TSelf Parse(string s, NumberStyles style, IFormatProvider? provider);

    static TSelf Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider);

    static TSelf Sign(TSelf value);

    static bool TryCreate<TOther>(TOther value, out TSelf result) where TOther : INumber<TOther>;

    static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out TSelf result);

    static bool TryParse(
      ReadOnlySpan<char> s,
      NumberStyles style,
      IFormatProvider? provider,
      out TSelf result);
  }
}
