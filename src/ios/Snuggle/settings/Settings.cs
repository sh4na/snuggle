using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	using Common;
	public class Settings : DialogViewController
	{
		public override string Title {
			get { return "Settings"; }
			set { }
		}

		EntryElement name, username, password, server, resource, buddy;

		public Settings () : base (UITableViewStyle.Grouped, null)
		{
			var profile = Common.Profile.Current;
			var xmpp = Common.XmppProfile.Current;

			Root = new RootElement ("Settings") {
				new Section ("Profile") {
					(name = new EntryElement ("Name", "Real Name", profile.Name))
				},
				new Section ("Xmpp") {
					(username = new EntryElement ("Username", "user@server", xmpp.Username)),
					(password = new EntryElement ("Password", "", xmpp.Password, true)),
					(server = new EntryElement ("Server", "", xmpp.NetworkHost) { AutocapitalizationType = UITextAutocapitalizationType.None, AutocorrectionType = UITextAutocorrectionType.No, KeyboardType = UIKeyboardType.Url }),
					(resource = new EntryElement ("Resource", "Home", xmpp.Resource)),
					(buddy = new EntryElement ("Snuggle Buddy", "Snuggle buddy's username", xmpp.Buddy)),
				}
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			NavigationItem.RightBarButtonItem = new UIBarButtonItem ("Save", UIBarButtonItemStyle.Plain, delegate {
				Save ();
			});
		}

		void Save ()
		{
			var xmpp = Common.XmppProfile.Current;
			xmpp.Name = name.Value;
			xmpp.Username = username.Value;
			xmpp.Password = password.Value;
			xmpp.NetworkHost = server.Value;
			xmpp.Resource = resource.Value;
			xmpp.Buddy = buddy.Value;
			xmpp.Save ();
		}
	}
}

