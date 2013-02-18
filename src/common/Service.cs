using System;
using System.Collections.Generic;
using Vici.CoolStorage;

namespace Snuggle.Common
{
	public interface ISession
	{
		void Start ();
		void Stop ();
		bool Running { get; }
	}


	[MapTo("Configuration")]
	public class Configuration : CSObject<Configuration, int, int, string>
	{
		internal static void CreateDB ()
		{
			CSDatabase.ExecuteNonQuery( @"
				CREATE TABLE Configuration (
					ProfileId INTEGER NOT NULL REFERENCES Profile(ProfileId),
					ServiceId INTEGER NOT NULL REFERENCES Service(ServiceId),
					Key TEXT(100) NOT NULL,
					Type TEXT(20) NOT NULL,
					Value TEXT(100) NOT NULL,
					PRIMARY KEY (ProfileId, ServiceId, Key)
				)"
			);
		}

		public string Key { get { return (string)GetField("Key"); } set { SetField("Key",value); } }
		public string Type { get { return (string)GetField("Type"); } set { SetField("Type",value); } }
		public string Value { get { return (string)GetField("Value"); } set { SetField("Value",value); } }

		[ManyToOne]
		public Profile Profile { get { return (Profile)GetField("Profile"); } set { SetField ("Profile", value); } }

		[ManyToOne]
		public Service Service { get { return (Service)GetField("Service"); }  set { SetField ("Service", value); } }

		public int ServiceId { get { return (int) GetField("ServiceId"); } set { SetField("ServiceId",value); } }
		public int ProfileId { get { return (int) GetField("ProfileId"); } set { SetField("ProfileId",value); } }

		public new bool Save ()
		{
			return base.Save ();
		}


//		[OneToMany]
//		public ConfigurationCollection Settings { get { return (ConfigurationCollection) GetField("Settings"); } }
//
//		Dictionary<string, object> attributes = new Dictionary<string, object> ();
//		public object this[string attribute] {
//			get {
//				object o = null;
//				attributes.TryGetValue (attribute, out o);
//				return o;
//			}
//			set {
//				if (attributes.ContainsKey (attribute))
//					attributes[attribute] = value;
//				else {
//					attributes.Add (attribute, value);
//				}
//			}
//		}

//		public void Load (Service.Type serviceType)
//		{
//			var entries = ConfigEntry.List ("ServiceId = " + (int)serviceType);
//			foreach (var entry in entries) {
//				if (entry.Type == typeof(bool).ToString ())
//					attributes[entry.Key] = bool.Parse (entry.Value);
//				else if (entry.Type == typeof(int).ToString ())
//					attributes[entry.Key] = int.Parse (entry.Value);
//				else
//					attributes[entry.Key] = entry.Value;
//			}
//		}
//
//		public void Save (Service.Type serviceType)
//		{
//			var entries = ConfigEntry.List ("ServiceId = " + (int)serviceType);
//			foreach (var entry in entries)
//				ConfigEntry.Delete (entry.ConfigurationId);
//
//			foreach (KeyValuePair<string, object> att in attributes) {
//				var entry = ConfigEntry.New ();
//				entry.Key = att.Key;
//				entry.Type = att.Value.GetType ().ToString ();
//				entry.Value = (string)att.Value;
//				entry.ServiceId = (int)serviceType;
//				entry.Save ();
//			}
//		}
	}

	public enum ServiceType
	{
		None,
		Xmpp
	}

	[MapTo("Service")]
	public class Service : CSObject<Service, int>
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

		[NotMapped]
		public ServiceType ServiceType { get { return (ServiceType) Type; } }

		public static event Action<ISession> OnConnect;
		public static event Action<ISession> OnDisconnect;
		public static event Action<ISession, object> OnEvent;

		//public static Dictionary<int, Service> Services = new Dictionary<int, Service> ();

		List<ISession> sessions = new List<ISession> ();
		internal List<ISession> Sessions {
			get { return sessions; }
		}

		public static Service GetService (ServiceType type)
		{
			var service = Service.ReadFirst ("Type=@type", "@type", (int)type);
			if (service == null) {
				service = Service.New ();
				service.Type = (int) type;
				service.Name = type.ToString ();
				service.Save ();
			}
			return service;
		}


		public static void Connect (Profile profile, ServiceType type)
		{
			Service service = GetService (type);
			//if (Services.TryGetValue ((int) type, out service))
			service.StartSession (profile);
		}

		public static void Disconnect (ServiceType type)
		{
			Service service = GetService (type);
			//if (Services.TryGetValue ((int) type, out service))
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

		public new bool Save ()
		{
			return base.Save ();
		}

	}
}

