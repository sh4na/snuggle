using System;
using System.IO;

using Vici.CoolStorage;

namespace Snuggle.Common
{
	public static class Storage
	{

		public static void Init (bool resetData = false) {
			CheckAndCreateDatabase ("db_viciCoolStorage.db3", resetData);
		}

		static void CheckAndCreateDatabase (string dbName, bool reset)
		{	
			// determine whether or not the database exists
			bool dbExists = File.Exists (GetDBPath (dbName));

			// configure the current database, create if it doesn't exist, and then run the anonymous
			// delegate method after it's created
#if MOBILE
			if (dbExists && reset)
				File.Delete (GetDBPath (dbName));

			CSConfig.SetDB (GetDBPath (dbName), SqliteOption.CreateIfNotExists, () => {
				Profile.DBProfile.CreateDB ();
				Service.DBService.CreateDB ();
				Configuration.DBConfiguration.CreateDB ();

			});
#endif
		}

		static string GetDBPath (string dbName)
		{
			// get a reference to the documents folder
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);			
			// create the db path
			string db = Path.Combine (documents, dbName);
			return db;
		}
	}
}

