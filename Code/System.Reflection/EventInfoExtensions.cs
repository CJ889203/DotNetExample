// Decompiled with JetBrains decompiler
// Type: System.Reflection.EventInfoExtensions
// Assembly: System.Reflection.TypeExtensions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 640AC10B-88E0-451A-B5D0-A4B0F7E22777
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Reflection.TypeExtensions.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Reflection.TypeExtensions.xml


#nullable enable
namespace System.Reflection
{
  public static class EventInfoExtensions
  {
    /// <param name="eventInfo" />
    public static MethodInfo? GetAddMethod(this EventInfo eventInfo)
    {
      ArgumentNullException.ThrowIfNull((object) eventInfo, nameof (eventInfo));
      return eventInfo.GetAddMethod();
    }

    /// <param name="eventInfo" />
    /// <param name="nonPublic" />
    public static MethodInfo? GetAddMethod(this EventInfo eventInfo, bool nonPublic)
    {
      ArgumentNullException.ThrowIfNull((object) eventInfo, nameof (eventInfo));
      return eventInfo.GetAddMethod(nonPublic);
    }

    /// <param name="eventInfo" />
    public static MethodInfo? GetRaiseMethod(this EventInfo eventInfo)
    {
      ArgumentNullException.ThrowIfNull((object) eventInfo, nameof (eventInfo));
      return eventInfo.GetRaiseMethod();
    }

    /// <param name="eventInfo" />
    /// <param name="nonPublic" />
    public static MethodInfo? GetRaiseMethod(this EventInfo eventInfo, bool nonPublic)
    {
      ArgumentNullException.ThrowIfNull((object) eventInfo, nameof (eventInfo));
      return eventInfo.GetRaiseMethod(nonPublic);
    }

    /// <param name="eventInfo" />
    public static MethodInfo? GetRemoveMethod(this EventInfo eventInfo)
    {
      ArgumentNullException.ThrowIfNull((object) eventInfo, nameof (eventInfo));
      return eventInfo.GetRemoveMethod();
    }

    /// <param name="eventInfo" />
    /// <param name="nonPublic" />
    public static MethodInfo? GetRemoveMethod(this EventInfo eventInfo, bool nonPublic)
    {
      ArgumentNullException.ThrowIfNull((object) eventInfo, nameof (eventInfo));
      return eventInfo.GetRemoveMethod(nonPublic);
    }
  }
}
