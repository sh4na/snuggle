using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	using Common;
	using Locale = Common.Utils.Locale;
	public class Settings : DialogViewController
	{
		public override string Title {
			get { return Locale.GetText ("Settings"); }
			set { }
		}

		EntryElement name, username, password, server, resource, buddy;

		public Settings () : base (UITableViewStyle.Grouped, null)
		{
			var profile = Common.Profile.Current;
			var xmpp = Common.XmppProfile.Current;

			Root = new RootElement ("Settings") {
				new Section (Locale.GetText ("Profile")) {
					(name = new EntryElement (Locale.GetText ("Name"), Locale.GetText ("Real Name"), profile.Name))
				},
				new Section (Locale.GetText ("Xmpp")) {
					(username = new EntryElement (Locale.GetText ("Username"), Locale.GetText ("user@server"), xmpp.Username)),
					(password = new EntryElement (Locale.GetText ("Password"), "", xmpp.Password, true)),
					(server = new EntryElement (Locale.GetText ("Server"), "", xmpp.NetworkHost) { AutocapitalizationType = UITextAutocapitalizationType.None, AutocorrectionType = UITextAutocorrectionType.No, KeyboardType = UIKeyboardType.Url }),
					(resource = new EntryElement (Locale.GetText ("Resource"), "Home", xmpp.Resource)),
					(buddy = new EntryElement (Locale.GetText ("Snuggle Buddy"), Locale.GetText ("Snuggle buddy's username"), xmpp.Buddy)),
				}
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			// TODO: move this to a base SnuggleViewController class
			// http://stackoverflow.com/questions/10824092/monotouch-dialog-dismissing-keyboard-by-touching-anywhere-in-dialogviewcontroll
			var tap = new UITapGestureRecognizer ();
			tap.AddTarget (() =>{
				View.EndEditing (true);
			});
			View.AddGestureRecognizer (tap);
			tap.CancelsTouchesInView = false;

			NavigationItem.RightBarButtonItem = new UIBarButtonItem (Locale.GetText ("Save"), UIBarButtonItemStyle.Plain, delegate {
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

