using System;
using System.IO;

using Vici.CoolStorage;

namespace Snuggle.Common
{
	public class Storage
	{
		public static Storage LocalStorage {
			get; private set;
		}

		static Storage ()
		{
			LocalStorage = new Storage ();
		}

		Storage ()
		{
			CheckAndCreateDatabase ("db_viciCoolStorage.db3");
		}

		void CheckAndCreateDatabase (string dbName)
		{	
			// determine whether or not the database exists
			bool dbExists = File.Exists (GetDBPath (dbName));

			// configure the current database, create if it doesn't exist, and then run the anonymous
			// delegate method after it's created
#if MOBILE
			CSConfig.SetDB (GetDBPath (dbName), SqliteOption.CreateIfNotExists, () => {
				CSDatabase.ExecuteNonQuery (Profile.TableDesc);
				CSDatabase.ExecuteNonQuery (Service.TableDesc);
				CSDatabase.ExecuteNonQuery (Configuration.TableDesc);
			});
#endif
		}

		string GetDBPath (string dbName)
		{
			// get a reference to the documents folder
			var documents = Environment.GetFolderPath (Environment.SpecialFolder.Personal);			
			// create the db path
			string db = Path.Combine (documents, dbName);
			return db;
		}

		public void Store (Profile profile)
		{

		}

		public Profile Load ()
		{
			return null;
		}
	}
}

