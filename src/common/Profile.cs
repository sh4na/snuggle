using System;
using System.Collections.Generic;

using Vici.CoolStorage;

namespace Snuggle.Common
{
	[MapTo("Profile")]
	public class Profile : CSObject<Profile, int>
	{
		internal static void CreateDB ()
		{
			CSDatabase.ExecuteNonQuery(@"
				CREATE TABLE Profile (
					ProfileId INTEGER PRIMARY KEY AUTOINCREMENT,
					Name TEXT(100) NOT NULL,
					Nickname TEXT(100) NOT NULL
				)"
			);
		}

		public int ProfileId { get { return (int)GetField("ProfileId"); } }
		public string Name { get { return (string)GetField("Name"); } set { SetField("Name",value); } }
		public string Nickname { get { return (string)GetField("Nickname"); } set { SetField("Nickname",value); } }

		[OneToMany]
		public SettingsList Settings { get { return (SettingsList) GetField("Settings"); } }

		public SettingsList SettingsByService (ServiceType type)
		{
			return Settings.FilteredBy (type);
		}

		public SettingsList this[ServiceType type]
		{
			get { return Settings.FilteredBy (type); }
		}

		public void SetConfiguration (Service service, string key, object value)
		{
			SettingsList settings = this[service.ServiceType];
			var conf = settings.GetConfiguration (this, key);
			if (conf == null)
				conf = settings.Add (this, service, key, value);
			else {
				conf.Value = (string) value;
			}
			conf.Save ();
			settings.Save ();
		}

//		Dictionary<int, Configuration> serviceAtts = new Dictionary<int, Configuration> ();

//		public void
//				Configuration id = null;
//				serviceAtts.TryGetValue ((int)type, out id);
//				return id;
//			}
//			set {
//				if (serviceAtts.ContainsKey ((int)type))
//					serviceAtts [(int)type] = value;
//				else
//					serviceAtts.Add ((int)type, value);
//			}
//		}


		public static Profile Lookup (string username)
		{
			var profile = Profile.ReadFirst ("Nickname=@username", "@username", username);
			if (profile == null) {
				profile = Profile.New ();
				profile.Nickname = username;
			}
			return profile;
		}

		public static Profile Create (string username)
		{
			var profile = Profile.New ();
			profile.Nickname = username;
			return profile;
		}

		public new bool Save ()
		{
			return base.Save ();
		}
	}

	[Serializable]
	public partial class SettingsList : CSList<Configuration>
	{
		public SettingsList () { }
		public SettingsList FilteredBy (ServiceType type) { return (SettingsList) base.FilteredBy ("ServiceId = " + (int)type); }
		public IList<string> Keys {
			get {
				List<string> keys = new List<string> ();
				foreach (var c in this)
					keys.Add (c.Key);
				return keys;
			}
		}

		public object this[string key] {
			get {
				return this.Find (c => c.Key == key);
			}
		}

		public Configuration GetConfiguration (Profile profile, string key)
		{
			return FilteredBy ("ProfileId=@pid and Key=@key", "@pid", profile.ProfileId, "@key", key).FirstItem;
		}

		public Configuration Add (Profile profile, Service service, string key, object value)
		{
			Configuration conf = new Configuration ();
			conf.ProfileId = profile.ProfileId;
			conf.ServiceId = service.ServiceId;
			conf.Key = key;
			conf.Value = (string) value;
			conf.Type = value.GetType ().ToString ();
			this.Add (conf);
			return conf;
		}

		public bool ContainsKey (string key)
		{
			return this[key] != null;
		}
	}

}

