using System;
using System.Collections.Generic;
using jabber;
using jabber.client;

namespace Snuggle.Common
{
	internal class XmppSession : ISession
	{
		bool running;
		JabberClient client;
		Profile profile;

		public XmppSession (Profile profile)
		{
			this.profile = profile;
		}

		void Init ()
		{
			client = new JabberClient ();
			var configuration = profile[Service.Type.Xmpp];
			var j = configuration["jid"] as string;
			var server = configuration["server"] as string;
			var password = configuration["password"] as string;
			var presence = configuration["presence"];


			var jid = new JID (j);
			client.User = jid.User;
			client.Server = jid.Server;
			client.Resource = jid.Resource;
			client.NetworkHost = server;
			client.Password = password;
			client.AutoPresence = presence == null ? false : (bool)presence;
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
			if (!String.IsNullOrEmpty (msg.Body))
				Service.OnData (this, msg.Body);
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

	internal class XmppService : Service
	{
		protected override void StartSession (Profile profile)
		{
			var session = new XmppSession (profile);
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
}

