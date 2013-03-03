using System;
using System.Collections.Generic;
using Vici.CoolStorage;

namespace Snuggle.Common
{
	public interface ISession
	{
		void Start ();
		void Stop ();
		bool Send (Message message);
		bool Running { get; }
		List<Message> Messages { get; }
	}

	public abstract class Session : ISession
	{
		List<Message> messages = new List<Message> ();
		public List<Message> Messages { get { return messages; } }

		public abstract bool Running { get; }
		public abstract void Start ();
		public abstract void Stop ();
		public abstract bool Send (Message message);
	}

	public enum ServiceType
	{
		None,
		Xmpp
	}

	public class Service : DBObject
	{
		internal DBService db {
			get { return base.obj as DBService; }
			set { base.obj = value; }
		}

		internal int Id { get { return db.ServiceId; } set { db.ServiceId = value; } }
		public string Name { get { return db.Name; } set { db.Name = value; } }
		public ServiceType Type { get { return (ServiceType) db.Type; } set { db.Type = (int) value; } }


		public Service (ServiceType type)
		{
			db = DBService.ReadFirst ("Type=@type", "@type", (int)type);
			if (db == null) {
				db = DBService.New ();
				ServiceType = type;
				db.Save ();
			}
		}

		internal Service (DBService db)
		{
			this.db = db;
		}

		[MapTo("Service")]
		internal class DBService : CSObject<DBService, int>
		{
			internal static void CreateDB ()
			{
				CSDatabase.ExecuteNonQuery(@"
					CREATE TABLE Service (
						ServiceId INTEGER PRIMARY KEY AUTOINCREMENT,
						Name TEXT(100) NOT NULL,
						Type INTEGER NOT NULL
					)"
				);
			}

			public int ServiceId { get { return (int) GetField ("ServiceId"); } set { SetField ("ServiceId", value); } }
			public int Type { get { return (int) GetField ("Type"); } set { SetField ("Type", value); } }
			public string Name { get { return (string) GetField ("Name"); } set { SetField ("Name", value); } }

			[OneToMany]
			public CSList<Profile.DBProfile> Profiles { get { return (CSList<Profile.DBProfile>) GetField("Profiles"); } }


		}

		public ServiceType ServiceType {
			get { return (ServiceType) db.Type; }
			set {
				db.Type = (int) value;
				db.Name = value.ToString ();
			}
		}

		public static event Action<ISession> OnConnect;
		public static event Action<ISession> OnDisconnect;
		public static event Action<ISession, Message> OnEvent;

		//public static Dictionary<int, Service> Services = new Dictionary<int, Service> ();

		List<ISession> sessions = new List<ISession> ();
		internal List<ISession> Sessions {
			get { return sessions; }
		}

		public static void Connect (Profile profile, ServiceType type)
		{
			Service service = null;
			if (type == Snuggle.Common.ServiceType.Xmpp) {
				service = XmppService.Current;
				if (service == null)
					throw new Exception ("Service " + type + " not found");
				var session = XmppSession.Current;
				if (session == null)
					service.StartSession (profile);
				else
					session.Start ();
			}

			throw new Exception ("Service " + type + " not found");
		}

		public static void Disconnect (ServiceType type)
		{
			Service service = null;
			if (type == Snuggle.Common.ServiceType.Xmpp)
				service = XmppService.Current;
			
			if (service == null)
				throw new Exception ("Service " + type + " not found");
			service.Shutdown ();
		}

		public virtual ISession StartSession (Profile profile = null) { return null; }
		public virtual void Shutdown () {}


		internal static void OnConnected (ISession session)
		{
			if (OnConnect != null)
				OnConnect (session);
		}

		internal static void OnDisconnected (ISession session)
		{
			if (OnDisconnect != null)
				OnDisconnect (session);
		}

		internal static void OnData (Session session, Message message)
		{
			session.Messages.Add (message);
			if (OnEvent != null)
				OnEvent (session, message);
		}

	}
}

