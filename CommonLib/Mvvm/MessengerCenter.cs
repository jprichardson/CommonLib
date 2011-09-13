using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib.Collections;
using CommonLib.Extensions;

namespace CommonLib.Mvvm
{
	public class MessengerCenter
	{
		private DictionaryList<string, KeyValuePair<object, Action<Message>>> _subscribers = new DictionaryList<string, KeyValuePair<object, Action<Message>>>();

		public int Notify(string message, object param) {
			int notificationCount = 0;
			if (_subscribers.ContainsKey(message)) {
				foreach (var subcriber in _subscribers[message])
					subcriber.Value(new Message() { MessageString = message, Object = param });
				notificationCount = _subscribers[message].Count;
			}

			return notificationCount;
		}

		public void Subscribe(object who, string message, Action<Message> action) {
			_subscribers.Add(message, new KeyValuePair<object, Action<Message>>(who, action));
		}

		public int Unsubscribe(object who, string message) {
			var removed = 0;
			if (_subscribers.ContainsKey(message)) {
				var subscribers = _subscribers[message];
				removed = subscribers.RemoveAll(kvp => kvp.Key == who);
			}
			return removed;
		}

		public class Message
		{
			public string MessageString = "";
			public object Object;
		}
	}
}
