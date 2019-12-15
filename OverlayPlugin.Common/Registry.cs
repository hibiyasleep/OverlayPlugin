using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowMage.OverlayPlugin
{
    public class Registry
    {
        public static TinyIoCContainer Container { get; private set; }
        private static List<Type> _overlays;
        private static List<IEventSource> _eventSources;
        private static List<Type> _esQueue;
        private static bool _esReady = false;

        public static IEnumerable<Type> Overlays
        {
            get
            {
                return _overlays;
            }
        }

        public static IEnumerable<IEventSource> EventSources
        {
            get
            {
                return _eventSources;
            }
        }

        public static event EventHandler<EventSourceRegisteredEventArgs> EventSourceRegistered;

        public static void Init()
        {
            Container = new TinyIoCContainer();
            _overlays = new List<Type>();
            _eventSources = new List<IEventSource>();
            _esQueue = new List<Type>();
            _esReady = false;
        }

        public static void Clear()
        {
            Container.Dispose();
            Container = null;

            _overlays = null;
            _eventSources = null;
        }

        public static void RegisterOverlay<T>()
            where T: class, IOverlay
        {
            _overlays.Add(typeof(T));
            Container.Register<T>();
        }

        public static void UnregisterOverlay<T>()
            where T: class, IOverlay
        {
            _overlays.Remove(typeof(T));
        }

        public static void RegisterEventSource(IEventSource source)
        {
            Container.BuildUp(source);
            _eventSources.Add(source);

            // If an event source is registered at runtime, we have to load the config
            // and start it immeditately.
            if (_esReady)
            {
                source.LoadConfig(Container.Resolve<IPluginConfig>());
                source.Start();

                EventSourceRegistered?.Invoke(null, new EventSourceRegisteredEventArgs(source));
            }
        }

        public static void RegisterEventSource<T>()
            where T: class, IEventSource
        {
            var esType = typeof(T);
            Container.Register(esType);

            if (_esReady)
            {
                RegisterEventSource((T)Container.Resolve(esType));
            } else
            {
                _esQueue.Add(esType);
            }
        }

        public static void StartEventSources()
        {
            foreach (var es in _esQueue)
            {
                RegisterEventSource((IEventSource)Container.Resolve(es));
            }
            _esQueue.Clear();
            _esReady = true;

            var config = Container.Resolve<IPluginConfig>();
            foreach (var source in _eventSources)
            {
                source.LoadConfig(config);
                source.Start();
            }
        }

        #region Shortcuts
        public static T Resolve<T>()
            where T : class
        {
            return Container.Resolve<T>();
        }

        public static TinyIoCContainer.RegisterOptions Register<T>()
            where T : class
        {
            return Container.Register<T>();
        }

        public static TinyIoCContainer.RegisterOptions Register<TInterface, Tclass>()
            where TInterface : class
            where Tclass : class, TInterface
        {
            return Container.Register<TInterface, Tclass>();
        }

        public static TinyIoCContainer.RegisterOptions Register<T>(T obj)
            where T : class
        {
            return Container.Register<T>(obj);
        }
        #endregion
    }

    public class EventSourceRegisteredEventArgs : EventArgs
    {
        public IEventSource EventSource { get; private set; }

        public EventSourceRegisteredEventArgs(IEventSource eventSource)
        {
            this.EventSource = eventSource;
        }
    }
}
