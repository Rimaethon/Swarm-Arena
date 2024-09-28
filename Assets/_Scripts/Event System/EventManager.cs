using System;
using System.Collections.Generic;
using System.Linq;

namespace Managers
{
	public static class EventManager
	{
		public static void RegisterHandler<TEvent>(Action<TEvent> handler) where TEvent : EventBase
		{
			EventHandlers<TEvent>.Register(handler);
		}

		public static void UnregisterHandler<TEventType>(Action<TEventType> handler) where TEventType : EventBase
		{
			EventHandlers<TEventType>.Unregister(handler);

		}

		public static void Send<TEventType>(TEventType eventData) where TEventType : EventBase, new()
		{
			EventHandlers<TEventType>.Handle(eventData);
		}


	}

	public static class EventHandlers<TEvent> where TEvent : EventBase
	{
		private static readonly List<Action<TEvent>> handlers = new List<Action<TEvent>>();

		public static void Register(Action<TEvent> handler)
		{
			if (!handlers.Contains(handler))
			{
				handlers.Add(handler);
			}
		}

		public static void Unregister(Action<TEvent> handler)
		{
			if (handlers.Contains(handler))
			{
				handlers.Remove(handler);
			}
		}

		public static void Handle(TEvent eventData)
		{
			foreach (Action<TEvent> handler in handlers.ToList())
			{
				handler.Invoke(eventData);
			}
		}
	}
}
