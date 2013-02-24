using System;
using System.Collections.Generic;

using Vici.CoolStorage;

namespace Snuggle.Common
{
	public abstract class Profile : DBObject
	{
		internal DBProfile db {
			get { return base.obj as DBProfile; }
			set { base.obj = value; }
		}

		public static Profile Current { get; private set; }

		internal int Id { get { return db.ProfileId; } set { db.ProfileId = value; } }
		public string Name { get { return db.Name; } set { db.Name = value; } }

		public Profile () : this((DBProfile)null) {}

		internal Profile (DBProfile db)
		{
			if (db == null)
				db = DBProfile.New ();
			this.db = db;
		}

		[MapTo("Profile")]
		internal class DBProfile : CSObject<DBProfile, int>
		{
			internal static void CreateDB ()
			{
				CSDatabase.ExecuteNonQuery(@"
					CREATE TABLE Profile (
						ProfileId INTEGER PRIMARY KEY AUTOINCREMENT,
						Name TEXT(100) NOT NULL,
						Active INTEGER DEFAULT 0
					)"
				);
			}
			
			public int ProfileId { get { return (int)GetField("ProfileId"); } set { SetField ("ProfileId", value); } }
			public string Name { get { return (string)GetField("Name"); } set { SetField("Name",value); } }
			public bool Active { get { return bool.Parse ((string)GetField("Active")); } set { SetField("Active", value ? 1 : 0); } }

			[OneToMany]
			public SettingsList Settings { get { return (SettingsList) GetField("Settings"); } }
		}	

		[Serializable]
		internal partial class SettingsList : CSList<Configuration.DBConfiguration>
		{
			public SettingsList () { }
			public SettingsList Filter (ServiceType type) { return (SettingsList) base.FilteredBy ("ServiceId=@sid", "@sid", (int)type); }
			public SettingsList Filter (string key) { return (SettingsList) base.FilteredBy ("Key=@key", "@key", key); }
			public SettingsList Filter (Profile profile) { return (SettingsList) base.FilteredBy ("ProfileId=@pid", "@pid", profile.Id); }
			public SettingsList Filter (int serviceId, int profileId) { return (SettingsList) base.FilteredBy ("ServiceId=@sid and ProfileId=@pid", "@sid", serviceId, "@pid", profileId); }
	
			public Configuration Add (Service service, Profile profile, string key, object value)
			{
				Configuration conf = new Configuration (service, profile, key, value);
				this.Add (conf.db);
				this.Save ();
				return conf;
			}
			
			public bool ContainsKey (string key)
			{
				return Filter (key).HasObjects;
			}
			public string GetString (string key)
			{
				var ret = Filter (key);
				if (ret.HasObjects)
					return ret.FirstItem.Value as string;
				return null;
			}
			
			public void Set (Service service, Profile profile, string key, object value)
			{
				var ret = Filter (key);
				if (ret.HasObjects) {
					Configuration conf;
					conf = new Configuration (ret.FirstItem);
					conf.Value = value;
				} else {
					Add (service, profile, key, value);
				}
			}
		}
	}

}

