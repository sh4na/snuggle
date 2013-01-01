using System;
using System.Collections.Generic;

namespace Snuggle.Common
{
	public interface ISession
	{
		void Start ();
		void Stop ();
		bool Running { get; }
	}

	public class Configuration
	{
		internal static string TableDesc = @"
			CREATE TABLE Configuration (
				ConfigurationId INTEGER PRIMARY KEY AUTOINCREMENT, 
				Key TEXT(100) NOT NULL,
				Type TEXT(20) NOT NULL,
				Value TEXT(100) NOT NULL,
				ServiceId INTEGER REFERENCES Service(ServiceId)
			)
		";


		Dictionary<string, object> attributes = new Dictionary<string, object> ();
		public object this[string attribute] {
			get {
				object o = null;
				attributes.TryGetValue (attribute, out o);
				return o;
			}
			set {
				if (attributes.ContainsKey (attribute))
					attributes[attribute] = value;
				else
					attributes.Add (attribute, value);
			}
		}
	}

	public class Service
	{
		internal static string TableDesc = @"
			CREATE TABLE Service (
				ServiceId INTEGER PRIMARY KEY AUTOINCREMENT, 
				Name TEXT(100) NOT NULL,
				Type INT NOT NULL
			)
		";

		public enum Type
		{
			Xmpp
		}

		public static event Action<ISession> OnConnect;
		public static event Action<ISession> OnDisconnect;
		public static event Action<ISession, object> OnEvent;

		public static Dictionary<int, Service> Services = new Dictionary<int, Service> ();

		List<ISession> sessions = new List<ISession> ();
		internal List<ISession> Sessions {
			get { return sessions; }
		}

		public static void Connect (Profile profile, Type type)
		{
			Service service = null;
			if (Services.TryGetValue ((int) type, out service))
				service.StartSession (profile);
		}

		public static void Disconnect (Type type)
		{
			Service service = null;
			if (Services.TryGetValue ((int) type, out service))
				service.Shutdown ();
		}

		protected virtual void StartSession (Profile profile) {}
		protected virtual void Shutdown () {}


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

		internal static void OnData (ISession session, object data)
		{
			if (OnEvent != null)
				OnEvent (session, data);
		}
	}
}

