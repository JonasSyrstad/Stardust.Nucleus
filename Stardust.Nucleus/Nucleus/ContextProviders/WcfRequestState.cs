//
// wcfrequeststate.cs
// This file is part of Stardust
//
// Author: Jonas Syrstad (jsyrstad2+StardustCore@gmail.com), http://no.linkedin.com/in/jonassyrstad/) 
// Copyright (c) 2014 Jonas Syrstad. All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Concurrent;
using System.Threading;
using Stardust.Core;
using Stardust.Core.Wcf;
using Stardust.Interstellar.Trace;
using Stardust.Particles;

namespace Stardust.Nucleus.ContextProviders
{
    public static class ContextScopeExtensions
    {
        private static readonly ConcurrentDictionary<string, StardustContextProvider> StateStorage = new ConcurrentDictionary<string, StardustContextProvider>();

        internal static IStardustContext CreateScope()
        {
            var id = Guid.NewGuid();
            var ctx = ThreadSynchronizationContext.BeginContext(id);

            lock (StateStorage)
            {
                if (!StateStorage.TryAdd(id.ToString(), new StardustContextProvider())) Logging.DebugMessage("Unable to initialize context");
                if (DoLogging) Logging.DebugMessage("creating scope storage for {0}", id);
            }
            ContainerFactory.Current.Bind(typeof(Guid?), id, Scope.Context);
            ContainerFactory.Current.Bind(typeof(InvokationMarker), new InvokationMarker(DateTime.UtcNow), Scope.Context);
            ContainerFactory.Current.Bind(typeof(TraceHandler), new TraceHandler(), Scope.Context);
            ctx.Disposing += CurrentContextOnOperationCompleted;
            return ctx;
        }

        internal class InvokationMarker
        {
            private readonly DateTime timestamp;

            public InvokationMarker(DateTime timestamp)
            {
                this.timestamp = timestamp;
            }
        }

        private static bool DoLogging
        {
            get { return ConfigurationManagerHelper.GetValueOnKey("stardust.Debug") == "true"; }
        }

        public static object GetItemFromContext(this IStardustContext currentContext, string key)
        {
            var container = GetContainer(currentContext);
            lock (container)
            {
                object instance;
                if (container.TryGetValue(key, out instance)) return instance;
                if (DoLogging && key != "Stardust.Nucleus.ContextProviders.IExtendedScopeProvider") Logging.DebugMessage("Item with key {0} was not found in context {1}", key, currentContext.ContextId);
                return null;
            }
        }

        public static void SetItemInContext(this IStardustContext currentContext, string key, object item)
        {
            var container = GetContainer(currentContext);
            lock (container)
            {
                if (DoLogging) Logging.DebugMessage("inserting {0}", key);
                if (container.ContainsKey(key))
                {
                    object oldValue;
                    container.TryRemove(key, out oldValue);
                }
                if (!container.TryAdd(key, item)) Logging.DebugMessage("inserting item {0} failed", key);
            }
        }


        public static void RemoveItemFromContext(this IStardustContext currentContext, string key)
        {
            var container = GetContainer(currentContext);
            lock (container)
            {
                object instance;
                if (!container.TryRemove(key, out instance)) Logging.DebugMessage("failed to remove item '{0}'", key);
            }
        }

        public static void ClearContext(this IStardustContext currentContext)
        {
            var container = GetContainer(currentContext);
            lock (container)
            {
                container.Clear();
            }
        }

        internal static void DisposeContext(this IStardustContext currentContext)
        {
            StardustContextProvider item;
            StateStorage.TryRemove(currentContext.ContextId.ToString(), out item);
            foreach (var disposable in item.DisposeList)
            {
                disposable.TryDispose();
            }
            item.DisposeList.Clear();
            item.Dispose();
        }

        private static ConcurrentDictionary<string, object> GetContainer(IStardustContext currentContext)
        {
            var container = GetStardustContextProvider(currentContext);
            return container.Container;
        }

        private static StardustContextProvider GetStardustContextProvider(IStardustContext currentContext)
        {
            try
            {
                if (currentContext == null)
                {
                    Logging.DebugMessage("WTF??");
                    currentContext = ThreadSynchronizationContext.BeginContext(Guid.NewGuid());
                }
                return ((ThreadSynchronizationContext)currentContext).StateContainer;
            }
            catch (Exception ex)
            {
                Logging.Exception(ex);
                throw;
            }
        }

        private static void WaitOperationRelease(IStardustContext context)
        {
            var threadLocker = context.GetItemFromContext(typeof(ManualResetEvent).FullName) as ManualResetEvent;
            if (threadLocker.IsInstance())
                threadLocker.WaitOne();
        }

        internal static void RegisterForDispose(this IStardustContext currentContext, IDisposable instance)
        {
            var container = GetStardustContextProvider(currentContext);
            container.DisposeList.Add(instance);
        }

        private static void CurrentContextOnOperationCompleted(object sender, EventArgs eventArgs)
        {
            var context = (IStardustContext)sender;
            WaitOperationRelease(context);
            DisposeContext(context);
            context.Disposing -= CurrentContextOnOperationCompleted;
        }
    }
    internal class WcfRequestState : IRequestState
    {
        public object Get(Type key)
        {
            return ThreadSynchronizationContext.CurrentContext.GetItemFromContext(key.FullName);
        }

        public void Remove(Type key)
        {
            ThreadSynchronizationContext.CurrentContext.RemoveItemFromContext(key.FullName);
        }

        public void RemoveAll()
        {
            ThreadSynchronizationContext.CurrentContext.ClearContext();
        }

        public void Store(Type key, object something)
        {
            ThreadSynchronizationContext.CurrentContext.SetItemInContext(key.FullName, something);
            RegisterForDispose(something);
        }

        private static void RegisterForDispose(object something)
        {
            var item = something as IDisposable;
            if (item != null)
                ThreadSynchronizationContext.CurrentContext.RegisterForDispose(item);
        }
    }
}