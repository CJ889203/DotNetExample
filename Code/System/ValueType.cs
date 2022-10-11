// Decompiled with JetBrains decompiler
// Type: System.ValueType
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;


#nullable enable
namespace System
{
  /// <summary>Provides the base class for value types.</summary>
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public abstract class ValueType
  {
    /// <summary>Indicates whether this instance and a specified object are equal.</summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2075:UnrecognizedReflectionPattern", Justification = "Trimmed fields don't make a difference for equality")]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
      if (obj == null)
        return false;
      Type type = this.GetType();
      if (obj.GetType() != type)
        return false;
      object a = (object) this;
      if (ValueType.CanCompareBits((object) this))
        return ValueType.FastEqualsCheck(a, obj);
      FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      for (int index = 0; index < fields.Length; ++index)
      {
        object obj1 = fields[index].GetValue(a);
        object obj2 = fields[index].GetValue(obj);
        if (obj1 == null)
        {
          if (obj2 != null)
            return false;
        }
        else if (!obj1.Equals(obj2))
          return false;
      }
      return true;
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool CanCompareBits(object obj);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool FastEqualsCheck(object a, object b);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    [MethodImpl(MethodImplOptions.InternalCall)]
    public override extern int GetHashCode();

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern int GetHashCodeOfPtr(IntPtr ptr);


    #nullable enable
    /// <summary>Returns the fully qualified type name of this instance.</summary>
    /// <returns>The fully qualified type name.</returns>
    public override string? ToString() => this.GetType().ToString();
  }
}
