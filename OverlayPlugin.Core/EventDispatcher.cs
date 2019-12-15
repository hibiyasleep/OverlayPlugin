using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RainbowMage.OverlayPlugin
{
    class EventDispatcher
    {
        static Dictionary<string, Func<JObject, JToken>> handlers;
        static Dictionary<string, List<IEventReceiver>> eventFilter;
        static Dictionary<string, Func<JObject>> stateCallbacks;

        public static void Init()
        {
            handlers = new Dictionary<string, Func<JObject, JToken>>();
            eventFilter = new Dictionary<string, List<IEventReceiver>>();
            stateCallbacks = new Dictionary<string, Func<JObject>>();
        }

        private static void Log(LogLevel level, string message, params object[] args)
        {
            PluginMain.Logger.Log(level, string.Format(message, args));
        }

        public static void RegisterHandler(string name, Func<JObject, JToken> handler)
        {
            if (handlers.ContainsKey(name))
            {
                throw new Exception(string.Format(Resources.DuplicateHandlerError, name));
            }

            handlers[name] = handler;
        }

        public static void RegisterEventTypes(List<string> names)
        {
            foreach (var name in names)
            {
                eventFilter[name] = new List<IEventReceiver>();
            }
        }

        public static void RegisterEventType(string name)
        {
            RegisterEventType(name, null);
        }

        public static void RegisterEventType(string name, Func<JObject> initCallback)
        {
            eventFilter[name] = new List<IEventReceiver>();

            if (initCallback != null)
                stateCallbacks[name] = initCallback;
        }

        public static void Subscribe(string eventName, IEventReceiver receiver)
        {
            if (!eventFilter.ContainsKey(eventName))
            {
                Log(LogLevel.Error, Resources.MissingEventSubError, eventName);
                return;
            }

            if (stateCallbacks.ContainsKey(eventName))
            {
                var ev = stateCallbacks[eventName]();
                if (ev != null) receiver.HandleEvent(ev);
            }

            lock (eventFilter[eventName])
            {
                eventFilter[eventName].Add(receiver);
            }
        }

        public static void Unsubscribe(string eventName, IEventReceiver receiver)
        {
            if (eventFilter.ContainsKey(eventName))
            {
                lock (eventFilter[eventName])
                {
                    eventFilter[eventName].Remove(receiver);
                }
            }
        }

        public static void UnsubscribeAll(IEventReceiver receiver)
        {
            foreach (var item in eventFilter.Values)
            {
                lock (item)
                {
                    if (item.Contains(receiver)) item.Remove(receiver);
                }
            }
        }

        // Can be used to check that an event will be delivered before building
        // an expensive JObject that would otherwise be thrown away.
        public static bool HasSubscriber(string eventName)
        {
            if (!eventFilter.ContainsKey(eventName))
                return false;
            lock (eventFilter[eventName])
            {
                return eventFilter[eventName].Count > 0;
            }
        }

        public static void DispatchEvent(JObject e)
        {
            var eventType = e["type"].ToString();
            if (!eventFilter.ContainsKey(eventType))
            {
                throw new Exception(string.Format(Resources.MissingEventDispatchError, eventType));
            }

            lock (eventFilter[eventType])
            {
                foreach (var receiver in eventFilter[eventType])
                {
                    try
                    {
                        receiver.HandleEvent(e);
                    }
                    catch (Exception ex)
                    {
                        Log(LogLevel.Error, Resources.EventHandlerException, eventType, receiver, ex);
                    }
                }
            }
        }

        public static JToken CallHandler(JObject e)
        {
            var handlerName = e["call"].ToString();
            if (!handlers.ContainsKey(handlerName))
            {
                throw new Exception(string.Format(Resources.MissingHandlerError, handlerName));
            }

            return handlers[handlerName](e);
        }
    }
}
