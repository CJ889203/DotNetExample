// Decompiled with JetBrains decompiler
// Type: System.Threading.ExecutionContext
// Assembly: System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: 227ED753-84C2-4C09-BD6A-4AE4FF4C1534
// Assembly location: C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.9\System.Private.CoreLib.dll
// XML documentation location: C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\6.0.9\ref\net6.0\System.Threading.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;


#nullable enable
namespace System.Threading
{
  /// <summary>Manages the execution context for the current thread. This class cannot be inherited.</summary>
  public sealed class ExecutionContext : IDisposable, ISerializable
  {

    #nullable disable
    internal static readonly ExecutionContext Default = new ExecutionContext();
    private static volatile ExecutionContext s_defaultFlowSuppressed;
    private readonly IAsyncLocalValueMap m_localValues;
    private readonly IAsyncLocal[] m_localChangeNotifications;
    private readonly bool m_isFlowSuppressed;
    private readonly bool m_isDefault;

    private ExecutionContext() => this.m_isDefault = true;

    private ExecutionContext(
      IAsyncLocalValueMap localValues,
      IAsyncLocal[] localChangeNotifications,
      bool isFlowSuppressed)
    {
      this.m_localValues = localValues;
      this.m_localChangeNotifications = localChangeNotifications;
      this.m_isFlowSuppressed = isFlowSuppressed;
    }


    #nullable enable
    /// <summary>Sets the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the logical context information needed to recreate an instance of the current execution context.</summary>
    /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to be populated with serialization information.</param>
    /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure representing the destination context of the serialization.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    public void GetObjectData(SerializationInfo info, StreamingContext context) => throw new PlatformNotSupportedException();

    /// <summary>Captures the execution context from the current thread.</summary>
    /// <returns>An <see cref="T:System.Threading.ExecutionContext" /> object representing the execution context for the current thread.</returns>
    public static ExecutionContext? Capture()
    {
      ExecutionContext executionContext = Thread.CurrentThread._executionContext;
      if (executionContext == null)
        executionContext = ExecutionContext.Default;
      else if (executionContext.m_isFlowSuppressed)
        executionContext = (ExecutionContext) null;
      return executionContext;
    }


    #nullable disable
    internal static ExecutionContext CaptureForRestore() => Thread.CurrentThread._executionContext;

    private ExecutionContext ShallowClone(bool isFlowSuppressed)
    {
      if (this.m_localValues != null && !AsyncLocalValueMap.IsEmpty(this.m_localValues))
        return new ExecutionContext(this.m_localValues, this.m_localChangeNotifications, isFlowSuppressed);
      return !isFlowSuppressed ? (ExecutionContext) null : ExecutionContext.s_defaultFlowSuppressed ?? (ExecutionContext.s_defaultFlowSuppressed = new ExecutionContext(AsyncLocalValueMap.Empty, new IAsyncLocal[0], true));
    }

    /// <summary>Suppresses the flow of the execution context across asynchronous threads.</summary>
    /// <exception cref="T:System.InvalidOperationException">The context flow is already suppressed.</exception>
    /// <returns>An <see cref="T:System.Threading.AsyncFlowControl" /> structure for restoring the flow.</returns>
    public static AsyncFlowControl SuppressFlow()
    {
      Thread currentThread = Thread.CurrentThread;
      ExecutionContext executionContext1 = currentThread._executionContext ?? ExecutionContext.Default;
      ExecutionContext executionContext2 = !executionContext1.m_isFlowSuppressed ? executionContext1.ShallowClone(true) : throw new InvalidOperationException(SR.InvalidOperation_CannotSupressFlowMultipleTimes);
      AsyncFlowControl asyncFlowControl = new AsyncFlowControl();
      currentThread._executionContext = executionContext2;
      asyncFlowControl.Initialize(currentThread);
      return asyncFlowControl;
    }

    /// <summary>Restores the flow of the execution context across asynchronous threads.</summary>
    /// <exception cref="T:System.InvalidOperationException">The context flow cannot be restored because it is not being suppressed.</exception>
    public static void RestoreFlow()
    {
      Thread currentThread = Thread.CurrentThread;
      ExecutionContext executionContext = currentThread._executionContext;
      currentThread._executionContext = executionContext != null && executionContext.m_isFlowSuppressed ? executionContext.ShallowClone(false) : throw new InvalidOperationException(SR.InvalidOperation_CannotRestoreUnsupressedFlow);
    }

    /// <summary>Indicates whether the flow of the execution context is currently suppressed.</summary>
    /// <returns>
    /// <see langword="true" /> if the flow is suppressed; otherwise, <see langword="false" />.</returns>
    public static bool IsFlowSuppressed()
    {
      ExecutionContext executionContext = Thread.CurrentThread._executionContext;
      return executionContext != null && executionContext.m_isFlowSuppressed;
    }

    internal bool HasChangeNotifications => this.m_localChangeNotifications != null;

    internal bool IsDefault => this.m_isDefault;


    #nullable enable
    /// <summary>Runs a method in a specified execution context on the current thread.</summary>
    /// <param name="executionContext">The <see cref="T:System.Threading.ExecutionContext" /> to set.</param>
    /// <param name="callback">A <see cref="T:System.Threading.ContextCallback" /> delegate that represents the method to be run in the provided execution context.</param>
    /// <param name="state">The object to pass to the callback method.</param>
    /// <exception cref="T:System.InvalidOperationException">
    ///        <paramref name="executionContext" /> is <see langword="null" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="executionContext" /> was not acquired through a capture operation.
    /// 
    /// -or-
    /// 
    /// <paramref name="executionContext" /> has already been used as the argument to a <see cref="M:System.Threading.ExecutionContext.Run(System.Threading.ExecutionContext,System.Threading.ContextCallback,System.Object)" /> call.</exception>
    public static void Run(
      ExecutionContext executionContext,
      ContextCallback callback,
      object? state)
    {
      if (executionContext == null)
        ExecutionContext.ThrowNullContext();
      ExecutionContext.RunInternal(executionContext, callback, state);
    }


    #nullable disable
    internal static void RunInternal(
      ExecutionContext executionContext,
      ContextCallback callback,
      object state)
    {
      Thread currentThread = Thread.CurrentThread;
      ExecutionContext executionContext1 = currentThread._executionContext;
      if (executionContext1 != null && executionContext1.m_isDefault)
        executionContext1 = (ExecutionContext) null;
      ExecutionContext executionContext2 = executionContext1;
      SynchronizationContext synchronizationContext = currentThread._synchronizationContext;
      if (executionContext != null && executionContext.m_isDefault)
        executionContext = (ExecutionContext) null;
      if (executionContext2 != executionContext)
        ExecutionContext.RestoreChangedContextToThread(currentThread, executionContext, executionContext2);
      ExceptionDispatchInfo exceptionDispatchInfo = (ExceptionDispatchInfo) null;
      try
      {
        callback(state);
      }
      catch (Exception ex)
      {
        exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
      }
      if (currentThread._synchronizationContext != synchronizationContext)
        currentThread._synchronizationContext = synchronizationContext;
      ExecutionContext executionContext3 = currentThread._executionContext;
      if (executionContext3 != executionContext2)
        ExecutionContext.RestoreChangedContextToThread(currentThread, executionContext2, executionContext3);
      exceptionDispatchInfo?.Throw();
    }


    #nullable enable
    /// <summary>Restores a captured execution context on to the current thread.</summary>
    /// <param name="executionContext">The ExecutionContext to set.</param>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="executionContext" /> is <see langword="null" />.</exception>
    public static void Restore(ExecutionContext executionContext)
    {
      if (executionContext == null)
        ExecutionContext.ThrowNullContext();
      ExecutionContext.RestoreInternal(executionContext);
    }


    #nullable disable
    internal static void RestoreInternal(ExecutionContext executionContext)
    {
      Thread currentThread = Thread.CurrentThread;
      ExecutionContext currentContext = currentThread._executionContext;
      if (currentContext != null && currentContext.m_isDefault)
        currentContext = (ExecutionContext) null;
      if (executionContext != null && executionContext.m_isDefault)
        executionContext = (ExecutionContext) null;
      if (currentContext == executionContext)
        return;
      ExecutionContext.RestoreChangedContextToThread(currentThread, executionContext, currentContext);
    }

    internal static void RunFromThreadPoolDispatchLoop(
      Thread threadPoolThread,
      ExecutionContext executionContext,
      ContextCallback callback,
      object state)
    {
      if (executionContext != null && !executionContext.m_isDefault)
        ExecutionContext.RestoreChangedContextToThread(threadPoolThread, executionContext, (ExecutionContext) null);
      ExceptionDispatchInfo exceptionDispatchInfo = (ExceptionDispatchInfo) null;
      try
      {
        callback(state);
      }
      catch (Exception ex)
      {
        exceptionDispatchInfo = ExceptionDispatchInfo.Capture(ex);
      }
      Thread currentThread = threadPoolThread;
      ExecutionContext executionContext1 = currentThread._executionContext;
      currentThread._synchronizationContext = (SynchronizationContext) null;
      if (executionContext1 != null)
        ExecutionContext.RestoreChangedContextToThread(currentThread, (ExecutionContext) null, executionContext1);
      exceptionDispatchInfo?.Throw();
    }

    internal static void RunForThreadPoolUnsafe<TState>(
      ExecutionContext executionContext,
      Action<TState> callback,
      in TState state)
    {
      Thread.CurrentThread._executionContext = executionContext;
      if (executionContext.HasChangeNotifications)
        ExecutionContext.OnValuesChanged((ExecutionContext) null, executionContext);
      callback(state);
    }

    internal static void RestoreChangedContextToThread(
      Thread currentThread,
      ExecutionContext contextToRestore,
      ExecutionContext currentContext)
    {
      currentThread._executionContext = contextToRestore;
      if ((currentContext == null || !currentContext.HasChangeNotifications) && (contextToRestore == null || !contextToRestore.HasChangeNotifications))
        return;
      ExecutionContext.OnValuesChanged(currentContext, contextToRestore);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ResetThreadPoolThread(Thread currentThread)
    {
      ExecutionContext executionContext = currentThread._executionContext;
      currentThread._synchronizationContext = (SynchronizationContext) null;
      currentThread._executionContext = (ExecutionContext) null;
      if (executionContext == null || !executionContext.HasChangeNotifications)
        return;
      ExecutionContext.OnValuesChanged(executionContext, (ExecutionContext) null);
      currentThread._synchronizationContext = (SynchronizationContext) null;
      currentThread._executionContext = (ExecutionContext) null;
    }

    internal static void OnValuesChanged(
      ExecutionContext previousExecutionCtx,
      ExecutionContext nextExecutionCtx)
    {
      IAsyncLocal[] changeNotifications1 = previousExecutionCtx?.m_localChangeNotifications;
      IAsyncLocal[] changeNotifications2 = nextExecutionCtx?.m_localChangeNotifications;
      try
      {
        if (changeNotifications1 != null && changeNotifications2 != null)
        {
          foreach (IAsyncLocal key in changeNotifications1)
          {
            object previousValue;
            previousExecutionCtx.m_localValues.TryGetValue(key, out previousValue);
            object currentValue;
            nextExecutionCtx.m_localValues.TryGetValue(key, out currentValue);
            if (previousValue != currentValue)
              key.OnValueChanged(previousValue, currentValue, true);
          }
          if (changeNotifications2 == changeNotifications1)
            return;
          foreach (IAsyncLocal key in changeNotifications2)
          {
            object previousValue;
            if (!previousExecutionCtx.m_localValues.TryGetValue(key, out previousValue))
            {
              object currentValue;
              nextExecutionCtx.m_localValues.TryGetValue(key, out currentValue);
              if (previousValue != currentValue)
                key.OnValueChanged(previousValue, currentValue, true);
            }
          }
        }
        else if (changeNotifications1 != null)
        {
          foreach (IAsyncLocal key in changeNotifications1)
          {
            object previousValue;
            previousExecutionCtx.m_localValues.TryGetValue(key, out previousValue);
            if (previousValue != null)
              key.OnValueChanged(previousValue, (object) null, true);
          }
        }
        else
        {
          foreach (IAsyncLocal key in changeNotifications2)
          {
            object currentValue;
            nextExecutionCtx.m_localValues.TryGetValue(key, out currentValue);
            if (currentValue != null)
              key.OnValueChanged((object) null, currentValue, true);
          }
        }
      }
      catch (Exception ex)
      {
        Environment.FailFast(SR.ExecutionContext_ExceptionInAsyncLocalNotification, ex);
      }
    }

    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowNullContext() => throw new InvalidOperationException(SR.InvalidOperation_NullContext);

    internal static object GetLocalValue(IAsyncLocal local)
    {
      ExecutionContext executionContext = Thread.CurrentThread._executionContext;
      if (executionContext == null)
        return (object) null;
      object localValue;
      executionContext.m_localValues.TryGetValue(local, out localValue);
      return localValue;
    }

    internal static void SetLocalValue(
      IAsyncLocal local,
      object newValue,
      bool needChangeNotifications)
    {
      ExecutionContext executionContext = Thread.CurrentThread._executionContext;
      object previousValue = (object) null;
      bool flag = false;
      if (executionContext != null)
        flag = executionContext.m_localValues.TryGetValue(local, out previousValue);
      if (previousValue == newValue)
        return;
      IAsyncLocal[] array = (IAsyncLocal[]) null;
      bool isFlowSuppressed = false;
      IAsyncLocalValueMap asyncLocalValueMap;
      if (executionContext != null)
      {
        isFlowSuppressed = executionContext.m_isFlowSuppressed;
        asyncLocalValueMap = executionContext.m_localValues.Set(local, newValue, !needChangeNotifications);
        array = executionContext.m_localChangeNotifications;
      }
      else
        asyncLocalValueMap = AsyncLocalValueMap.Create(local, newValue, !needChangeNotifications);
      if (needChangeNotifications && !flag)
      {
        if (array == null)
        {
          array = new IAsyncLocal[1]{ local };
        }
        else
        {
          int length = array.Length;
          Array.Resize<IAsyncLocal>(ref array, length + 1);
          array[length] = local;
        }
      }
      Thread.CurrentThread._executionContext = isFlowSuppressed || !AsyncLocalValueMap.IsEmpty(asyncLocalValueMap) ? new ExecutionContext(asyncLocalValueMap, array, isFlowSuppressed) : (ExecutionContext) null;
      if (!needChangeNotifications)
        return;
      local.OnValueChanged(previousValue, newValue, false);
    }


    #nullable enable
    /// <summary>Creates a copy of the current execution context.</summary>
    /// <exception cref="T:System.InvalidOperationException">This context cannot be copied because it is used. Only newly captured contexts can be copied.</exception>
    /// <returns>An <see cref="T:System.Threading.ExecutionContext" /> object representing the current execution context.</returns>
    public ExecutionContext CreateCopy() => this;

    /// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.ExecutionContext" /> class.</summary>
    public void Dispose()
    {
    }
  }
}
