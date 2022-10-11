// Decompiled with JetBrains decompiler
// Type: System.Reflection.MethodSemanticsAttributes
// Assembly: System.Reflection.Metadata, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: FDD13CB9-4DB5-4759-8B88-2D188C369E68
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.Metadata.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.Metadata.xml

namespace System.Reflection
{
  [Flags]
  public enum MethodSemanticsAttributes
  {
    /// <summary>
    ///   <para>Used to modify the value of the property.</para>
    ///   <para>CLS-compliant setters are named with the <see langword="set_" /> prefix.</para>
    /// </summary>
    Setter = 1,
    /// <summary>
    ///   <para>Reads the value of the property.</para>
    ///   <para>CLS-compliant getters are named with get_ prefix.</para>
    /// </summary>
    Getter = 2,
    /// <summary>Other method for a property (not a getter or setter) or an event (not an adder, remover, or raiser).</summary>
    Other = 4,
    /// <summary>
    ///   <para>Used to add a handler for an event. Corresponds to the <see langword="AddOn" /> flag in the Ecma 335 CLI specification.</para>
    ///   <para>CLS-compliant adders are named the with <see langword="add_" /> prefix.</para>
    /// </summary>
    Adder = 8,
    /// <summary>
    ///   <para>Used to remove a handler for an event. Corresponds to the <see langword="RemoveOn" /> flag in the Ecma 335 CLI specification.</para>
    ///   <para>CLS-compliant removers are named with the <see langword="remove_" /> prefix.</para>
    /// </summary>
    Remover = 16, // 0x00000010
    /// <summary>
    ///   <para>Used to indicate that an event has occurred. Corresponds to the <see langword="Fire" /> flag in the Ecma 335 CLI specification.</para>
    ///   <para> CLS-compliant raisers are named with the <see langword="raise_" /> prefix.</para>
    /// </summary>
    Raiser = 32, // 0x00000020
  }
}
