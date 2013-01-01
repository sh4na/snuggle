using System;
using System.Collections.Generic;

namespace Snuggle.Common
{
	public class Profile
	{
		internal static string TableDesc = @"
			CREATE TABLE Profile (
				ProfileId INTEGER PRIMARY KEY AUTOINCREMENT, 
				Name TEXT(100) NOT NULL,
				NickName TEXT(100) NOT NULL,
				ConfigurationId INTEGER REFERENCES Configuration(ConfigurationId)
			)
		";

		public string Name { get; set; }
		public string NickName { get; set; }

		Dictionary<int, Configuration> serviceAtts = new Dictionary<int, Configuration> ();
		public Configuration this[Service.Type type] {
			get {
				Configuration id = null;
				serviceAtts.TryGetValue ((int)type, out id);
				return id;
			}
			set {
				if (serviceAtts.ContainsKey ((int)type))
					serviceAtts [(int)type] = value;
				else
					serviceAtts.Add ((int)type, value);
			}
		}
	}
}

