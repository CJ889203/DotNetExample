// Decompiled with JetBrains decompiler
// Type: System.AppContext
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Runtime.xml

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Loader;
using System.Runtime.Versioning;
using System.Threading;


#nullable enable
namespace System
{
  /// <summary>Provides members for setting and retrieving data about an application's context.</summary>
  public static class AppContext
  {

    #nullable disable
    private static Dictionary<string, object> s_dataStore;
    private static Dictionary<string, bool> s_switches;
    private static string s_defaultBaseDirectory;


    #nullable enable
    /// <summary>Gets the file path of the base directory that the assembly resolver uses to probe for assemblies.</summary>
    /// <returns>The file path of the base directory that the assembly resolver uses to probe for assemblies.</returns>
    public static string BaseDirectory => AppContext.GetData("APP_CONTEXT_BASE_DIRECTORY") is string data ? data : AppContext.s_defaultBaseDirectory ?? (AppContext.s_defaultBaseDirectory = AppContext.GetBaseDirectoryCore());

    /// <summary>Gets the name of the framework version targeted by the current application.</summary>
    /// <returns>The name of the framework version targeted by the current application.</returns>
    public static string? TargetFrameworkName
    {
      get
      {
        Assembly entryAssembly = Assembly.GetEntryAssembly();
        if ((object) entryAssembly == null)
          return (string) null;
        return entryAssembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
      }
    }

    /// <summary>Returns the value of the named data element assigned to the current application domain.</summary>
    /// <param name="name">The name of the data element.</param>
    /// <returns>The value of <paramref name="name" />, if <paramref name="name" /> identifies a named value; otherwise, <see langword="null" />.</returns>
    public static object? GetData(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (AppContext.s_dataStore == null)
        return (object) null;
      object data;
      lock (AppContext.s_dataStore)
        AppContext.s_dataStore.TryGetValue(name, out data);
      return data;
    }

    public static void SetData(string name, object? data)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (AppContext.s_dataStore == null)
        Interlocked.CompareExchange<Dictionary<string, object>>(ref AppContext.s_dataStore, new Dictionary<string, object>(), (Dictionary<string, object>) null);
      lock (AppContext.s_dataStore)
        AppContext.s_dataStore[name] = data;
    }

    [field: DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof (UnhandledExceptionEventArgs))]
    public static event UnhandledExceptionEventHandler? UnhandledException;

    [field: DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof (FirstChanceExceptionEventArgs))]
    public static event EventHandler<FirstChanceExceptionEventArgs>? FirstChanceException;

    public static event EventHandler? ProcessExit;

    internal static void OnProcessExit()
    {
      AssemblyLoadContext.OnProcessExit();
      if (EventSource.IsSupported)
        EventListener.DisposeOnShutdown();
      EventHandler processExit = AppContext.ProcessExit;
      if (processExit == null)
        return;
      processExit((object) AppDomain.CurrentDomain, EventArgs.Empty);
    }

    /// <summary>Tries to get the value of a switch.</summary>
    /// <param name="switchName">The name of the switch.</param>
    /// <param name="isEnabled">When this method returns, contains the value of <paramref name="switchName" /> if <paramref name="switchName" /> was found, or <see langword="false" /> if <paramref name="switchName" /> was not found. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="switchName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="switchName" /> is <see cref="F:System.String.Empty" />.</exception>
    /// <returns>
    /// <see langword="true" /> if <paramref name="switchName" /> was set and the <paramref name="isEnabled" /> argument contains the value of the switch; otherwise, <see langword="false" />.</returns>
    public static bool TryGetSwitch(string switchName, out bool isEnabled)
    {
      switch (switchName)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyName, nameof (switchName));
        case null:
          throw new ArgumentNullException(nameof (switchName));
        default:
          if (AppContext.s_switches != null)
          {
            lock (AppContext.s_switches)
            {
              if (AppContext.s_switches.TryGetValue(switchName, out isEnabled))
                return true;
            }
          }
          if (AppContext.GetData(switchName) is string data && bool.TryParse(data, out isEnabled))
            return true;
          isEnabled = false;
          return false;
      }
    }

    /// <summary>Sets the value of a switch.</summary>
    /// <param name="switchName">The name of the switch.</param>
    /// <param name="isEnabled">The value of the switch.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="switchName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="switchName" /> is <see cref="F:System.String.Empty" />.</exception>
    public static void SetSwitch(string switchName, bool isEnabled)
    {
      switch (switchName)
      {
        case "":
          throw new ArgumentException(SR.Argument_EmptyName, nameof (switchName));
        case null:
          throw new ArgumentNullException(nameof (switchName));
        default:
          if (AppContext.s_switches == null)
            Interlocked.CompareExchange<Dictionary<string, bool>>(ref AppContext.s_switches, new Dictionary<string, bool>(), (Dictionary<string, bool>) null);
          lock (AppContext.s_switches)
          {
            AppContext.s_switches[switchName] = isEnabled;
            break;
          }
      }
    }


    #nullable disable
    internal static unsafe void Setup(char** pNames, char** pValues, int count)
    {
      AppContext.s_dataStore = new Dictionary<string, object>(count);
      for (int index = 0; index < count; ++index)
        AppContext.s_dataStore.Add(new string(pNames[index]), (object) new string(pValues[index]));
    }

    [UnconditionalSuppressMessage("SingleFile", "IL3000: Avoid accessing Assembly file path when publishing as a single file", Justification = "Single File apps should always set APP_CONTEXT_BASE_DIRECTORY therefore code handles Assembly.Location equals null")]
    private static string GetBaseDirectoryCore()
    {
      string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
      if (directoryName == null)
        return string.Empty;
      if (!Path.EndsInDirectorySeparator(directoryName))
        directoryName += "\\";
      return directoryName;
    }

    internal static void LogSwitchValues(RuntimeEventSource ev)
    {
      if (AppContext.s_switches != null)
      {
        lock (AppContext.s_switches)
        {
          foreach (KeyValuePair<string, bool> keyValuePair in AppContext.s_switches)
            ev.LogAppContextSwitch(keyValuePair.Key, keyValuePair.Value ? 1 : 0);
        }
      }
      if (AppContext.s_dataStore == null)
        return;
      lock (AppContext.s_dataStore)
      {
        if (AppContext.s_switches != null)
        {
          lock (AppContext.s_switches)
            LogDataStore(ev, AppContext.s_switches);
        }
        else
          LogDataStore(ev, (Dictionary<string, bool>) null);
      }

      static void LogDataStore(RuntimeEventSource ev, Dictionary<string, bool> switches)
      {
        foreach (KeyValuePair<string, object> keyValuePair in AppContext.s_dataStore)
        {
          bool result;
          // ISSUE: explicit non-virtual call
          if (keyValuePair.Value is string str && bool.TryParse(str, out result) && (switches != null ? (!__nonvirtual (switches.ContainsKey(keyValuePair.Key)) ? 1 : 0) : 1) != 0)
            ev.LogAppContextSwitch(keyValuePair.Key, result ? 1 : 0);
        }
      }
    }
  }
}
