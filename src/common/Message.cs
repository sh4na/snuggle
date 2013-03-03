using System;

namespace Snuggle.Common
{
	public class Message
	{
		public enum MessageType {
			Text,
			Image
		}

		public MessageType Type { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public DateTime? SentTime { get; set; }
		public DateTime? ReceivedTime { get; set; }

		object data;
		public string Body {
			get { return Type == MessageType.Text ? (string) data : null; }
			set {
				data = value;
				Type = MessageType.Text;
			}
		}

		public Message (MessageType type, string from, string to, DateTime? sent, DateTime? received, object data)
		{
			this.Type = type;
			this.From = from;
			this.To = to;
			this.data = data;
			this.SentTime = sent;
			this.ReceivedTime = received;
		}
	}
}

