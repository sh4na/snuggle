using System;
using System.Collections.Generic;
using Vici.CoolStorage;

namespace Snuggle.Common
{
	public abstract class DBObject
	{
		protected CSObject obj;

		internal DBObject (CSObject db)
		{
			this.obj = db;
		}

		internal DBObject ()
		{
		}

		public bool Save ()
		{
			return obj.Save ();
		}
	}

	public class Configuration : DBObject
	{
		static Dictionary<string, Func<string, object>> allowedTypes = new Dictionary<string, Func<string, object>> ();

		static Configuration ()
		{
			allowedTypes.Add (typeof(bool).ToString(), x => bool.Parse (x));
			allowedTypes.Add (typeof(int).ToString(), x => int.Parse (x));
			allowedTypes.Add (typeof(string).ToString(), x => x);
		}

		internal DBConfiguration db {
			get { return base.obj as DBConfiguration; }
			set { base.obj = value; }
		}

		public Profile Profile { get { return null; } set { db.ProfileId = value.Id; } }
		public Service Service { get { return null; } set { db.ServiceId = value.Id; } }

		public string Key { get { return db.Key; } set { db.Key = value; } }
		public object Value
		{
			get {
				return GetValue ();
			}
			set {
				SetValue (value);
			}
		}

		object GetValue ()
		{
			var t = db.Type;
			Func<string, object> f = null;
			if (allowedTypes.TryGetValue (t.GetType().ToString(), out f))
				return f (t);
			return t;
		}

		void SetValue (object value)
		{
			db.Value = value == null ? "" : value.ToString ();
			db.Type = value == null ? "string" : value.GetType ().ToString ();
		}
		
		internal Configuration (DBConfiguration db) : base (db)
		{
		}

		public Configuration (Service service, Profile profile, string key, object value) : base (DBConfiguration.New ())
		{
			db.ProfileId = profile.Id;
			db.ServiceId = service.Id;
			db.Key = key;
			SetValue (value);
		}

		[MapTo("Configuration")]
		internal class DBConfiguration : CSObject<DBConfiguration, int, int, string>
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
			public Profile.DBProfile Profile { get { return (Profile.DBProfile)GetField("Profile"); } }
			
			[ManyToOne]
			public Service.DBService Service { get { return (Service.DBService)GetField("Service"); } }
			
			public int ServiceId { get { return (int) GetField("ServiceId"); } set { SetField("ServiceId",value); } }
			public int ProfileId { get { return (int) GetField("ProfileId"); } set { SetField("ProfileId",value); } }
		}
	}
}