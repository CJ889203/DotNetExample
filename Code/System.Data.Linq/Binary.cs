// Decompiled with JetBrains decompiler
// Type: System.Data.Linq.Binary
// Assembly: System.Data.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 43A00CAE-A2D4-43EB-9464-93379DA02EC9
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\System.Data.Linq.dll

using System.Runtime.Serialization;
using System.Text;

namespace System.Data.Linq
{
  [DataContract]
  [Serializable]
  public sealed class Binary : IEquatable<Binary>
  {
    [DataMember(Name = "Bytes")]
    private byte[] bytes;
    private int? hashCode;

    public Binary(byte[] value)
    {
      if (value == null)
      {
        this.bytes = new byte[0];
      }
      else
      {
        this.bytes = new byte[value.Length];
        Array.Copy((Array) value, (Array) this.bytes, value.Length);
      }
      this.ComputeHash();
    }

    public byte[] ToArray()
    {
      byte[] destinationArray = new byte[this.bytes.Length];
      Array.Copy((Array) this.bytes, (Array) destinationArray, destinationArray.Length);
      return destinationArray;
    }

    public int Length => this.bytes.Length;

    public static implicit operator Binary(byte[] value) => new Binary(value);

    public bool Equals(Binary other) => this.EqualsTo(other);

    public static bool operator ==(Binary binary1, Binary binary2)
    {
      if ((object) binary1 == (object) binary2 || (object) binary1 == null && (object) binary2 == null)
        return true;
      return (object) binary1 != null && (object) binary2 != null && binary1.EqualsTo(binary2);
    }

    public static bool operator !=(Binary binary1, Binary binary2)
    {
      if ((object) binary1 == (object) binary2 || (object) binary1 == null && (object) binary2 == null)
        return false;
      return (object) binary1 == null || (object) binary2 == null || !binary1.EqualsTo(binary2);
    }

    public override bool Equals(object obj) => this.EqualsTo(obj as Binary);

    public override int GetHashCode()
    {
      if (!this.hashCode.HasValue)
        this.ComputeHash();
      return this.hashCode.Value;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("\"");
      stringBuilder.Append(Convert.ToBase64String(this.bytes, 0, this.bytes.Length));
      stringBuilder.Append("\"");
      return stringBuilder.ToString();
    }

    private bool EqualsTo(Binary binary)
    {
      if ((object) this == (object) binary)
        return true;
      if ((object) binary == null || this.bytes.Length != binary.bytes.Length || this.GetHashCode() != binary.GetHashCode())
        return false;
      int index = 0;
      for (int length = this.bytes.Length; index < length; ++index)
      {
        if ((int) this.bytes[index] != (int) binary.bytes[index])
          return false;
      }
      return true;
    }

    private void ComputeHash()
    {
      int num1 = 314;
      int num2 = 159;
      this.hashCode = new int?(0);
      for (int index = 0; index < this.bytes.Length; ++index)
      {
        int? hashCode = this.hashCode;
        int num3 = num1;
        int? nullable = hashCode.HasValue ? new int?(hashCode.GetValueOrDefault() * num3) : new int?();
        int num4 = (int) this.bytes[index];
        this.hashCode = nullable.HasValue ? new int?(nullable.GetValueOrDefault() + num4) : new int?();
        num1 *= num2;
      }
    }
  }
}
