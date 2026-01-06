using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWS
{
	[Serializable]
	public class Messages
	{
		public List<MessageOptions> list = new List<MessageOptions>();

		public void Initialize(int size)
		{
			for (int i = list.Count; i <= size; i++)
			{
				list.Add(AddEmptyToOption(new MessageOptions()));
			}
		}

		public MessageOptions AddEmptyToOption(MessageOptions opt)
		{
			opt.message.Add(string.Empty);
			opt.type.Add(MessageOptions.ValueType.None);
			opt.obj.Add(null);
			opt.text.Add(null);
			opt.num.Add(0f);
			opt.vect2.Add(Vector2.zero);
			opt.vect3.Add(Vector3.zero);
			return opt;
		}

		public void FillOptionWithValues(MessageOptions opt)
		{
			int count = opt.message.Count;
			if (opt.type.Count < count)
			{
				opt.type.Add(MessageOptions.ValueType.None);
			}
			if (opt.obj.Count < count)
			{
				opt.obj.Add(null);
			}
			if (opt.text.Count < count)
			{
				opt.text.Add(null);
			}
			if (opt.num.Count < count)
			{
				opt.num.Add(0f);
			}
			if (opt.vect2.Count < count)
			{
				opt.vect2.Add(Vector2.zero);
			}
			if (opt.vect3.Count < count)
			{
				opt.vect3.Add(Vector3.zero);
			}
		}

		public MessageOptions GetMessageOption(int waypoint)
		{
			Initialize(waypoint);
			return list[waypoint];
		}

		public void Execute(MonoBehaviour mono, int index)
		{
			if (list == null || list.Count - 1 < index || list[index].message == null)
			{
				return;
			}
			for (int i = 0; i < list[index].message.Count; i++)
			{
				if (!(list[index].message[i] == string.Empty))
				{
					MessageOptions messageOptions = list[index];
					switch (messageOptions.type[i])
					{
					case MessageOptions.ValueType.None:
						mono.SendMessage(messageOptions.message[i], SendMessageOptions.DontRequireReceiver);
						break;
					case MessageOptions.ValueType.Object:
						mono.SendMessage(messageOptions.message[i], messageOptions.obj[i], SendMessageOptions.DontRequireReceiver);
						break;
					case MessageOptions.ValueType.Text:
						mono.SendMessage(messageOptions.message[i], messageOptions.text[i], SendMessageOptions.DontRequireReceiver);
						break;
					case MessageOptions.ValueType.Numeric:
						mono.SendMessage(messageOptions.message[i], messageOptions.num[i], SendMessageOptions.DontRequireReceiver);
						break;
					case MessageOptions.ValueType.Vector2:
						mono.SendMessage(messageOptions.message[i], messageOptions.vect2[i], SendMessageOptions.DontRequireReceiver);
						break;
					case MessageOptions.ValueType.Vector3:
						mono.SendMessage(messageOptions.message[i], messageOptions.vect3[i], SendMessageOptions.DontRequireReceiver);
						break;
					}
				}
			}
		}
	}
}
