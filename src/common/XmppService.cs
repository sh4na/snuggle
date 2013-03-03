using System;
using System.Collections.Generic;
using jabber;
using jabber.client;

namespace Snuggle.Common
{
	public class XmppSession : ISession
	{
		bool running;
		JabberClient client;
		XmppProfile profile;

		public XmppSession (XmppProfile profile)
		{
			this.profile = profile;
		}

		public static ISession RunningSession {
			get {
				foreach (var session in XmppService.Current.Sessions) {
					if (session is XmppSession)
						return session;
				}
				return null;
			}
		} 

		void Init ()
		{
			client = new JabberClient ();
			var j = profile.Username;

			var jid = new JID (j);
			client.User = jid.User;
			client.Server = jid.Server;
			client.Resource = jid.Resource;
			client.NetworkHost = profile.NetworkHost;
			client.Password = profile.Password;

		}

		void Hookup ()
		{
			client.OnInvalidCertificate += HandleOnInvalidCertificate;
			client.OnConnect += HandleOnConnect;
			client.OnDisconnect += HandleOnDisconnect;
			client.OnMessage += HandleOnMessage;
		}

		void Release ()
		{
			client.OnInvalidCertificate -= HandleOnInvalidCertificate;
			client.OnConnect -= HandleOnConnect;
			client.OnDisconnect -= HandleOnDisconnect;
			client.OnMessage -= HandleOnMessage;
		}

		bool HandleOnInvalidCertificate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		void HandleOnMessage (object sender, jabber.protocol.client.Message msg)
		{
			if (msg.Type == jabber.protocol.client.MessageType.chat || msg.Type == jabber.protocol.client.MessageType.normal) {
				if (!String.IsNullOrEmpty (msg.Body))
					Service.OnData (this, new Message (Message.MessageType.Text, msg.From, msg.Stamp, DateTime.Now, msg.Body));
			}
		}

		void HandleOnDisconnect (object sender)
		{
			Service.OnDisconnected (this);
			Release ();
		}

		void HandleOnConnect (object sender, jabber.connection.StanzaStream stream)
		{
			Service.OnConnected (this);
		}

		public void Send (string to, string msg)
		{
			client.Message (to, msg);
		}

		public void Start ()
		{
			Stop ();

			if (client == null)
				Init ();

			Hookup ();
			running = true;

			client.Connect ();

		}

		public void Stop ()
		{
			if (!running)
				return;
			running = false;
			if (client == null)
				return;
			client.Close ();
		}

		public bool Running {
			get { return running; }
		}
	}

	public class XmppService : Service
	{
		public static XmppService Current { get; private set; }
		static XmppService ()
		{
			Current = new XmppService ();
		}

		public XmppService () : base (ServiceType.Xmpp) {}

		protected override void StartSession (Profile profile)
		{
			var session = new XmppSession (profile as XmppProfile);
			Sessions.Add (session);
			session.Start ();
		}

		protected override void Shutdown ()
		{
			foreach (var session in Sessions) {
				session.Stop ();
			}
			Sessions.Clear ();
		}
	}

	public class XmppProfile : Profile
	{
		SettingsList settings { get { return db.Settings.Filter (XmppService.Current.Id, this.db.ProfileId); } }
		public new static XmppProfile Current { get; private set; }

		static XmppProfile ()
		{
			var profile = DBProfile.ReadFirst ("Active=@active", "@active", true);
			Current = new XmppProfile (profile);
			if (profile == null) {
				Current.db.Active = true;
				Current.Name = "new user";
				Current.Save ();
			}
		}

		internal XmppProfile (DBProfile profile) : base (profile)
		{
		}
		
		public string Username { get { return settings.GetString ("username"); } set { settings.Set (XmppService.Current, this, "username", value); } }
		public string Resource { get { return settings.GetString ("resource"); } set { settings.Set (XmppService.Current, this, "resource", value); } }
		public string NetworkHost { get { return settings.GetString ("networkhost"); } set { settings.Set (XmppService.Current, this, "networkhost", value); } }
		public string Password { get { return settings.GetString ("password"); } set { settings.Set (XmppService.Current, this, "password", value); } }
		public string Buddy { get { return settings.GetString ("buddy"); } set { settings.Set (XmppService.Current, this, "buddy", value); } }
	}

}

