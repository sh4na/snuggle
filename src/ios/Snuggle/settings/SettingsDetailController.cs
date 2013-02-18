
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

namespace Snuggle
{
	public partial class SettingsDetailController : SnuggleViewController
	{
		UIPopoverController masterPopoverController;
		string detailItem = null;
		UIBarButtonItem saveButton;

		public SettingsDetailController () : base ("SettingsDetailController") { }
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			saveButton = new UIBarButtonItem ("Save", UIBarButtonItemStyle.Plain, DoSave);
			if (detailItem == null)
				detailItem = "General";

			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}

		void DoSave (object sender, EventArgs ev)
		{
			switch (detailItem) {
				case "General":
					var profile = Common.Profile.Lookup (generalUsername.Text);
					profile.Name = generalName.Text;
					profile.Save ();
					var service = Common.Service.GetService (Common.ServiceType.Xmpp);
					profile.SetConfiguration (service, "server", generalServer.Text);
					profile.SetConfiguration (service, "password", generalPassword.Text);
					profile.SetConfiguration (service, "resource", generalResource.Text);
//					Common.Profile p = new Snuggle.Common.Profile ();
//					p.Name = generalName.Text;
//					p.Nickname = generalUsername.Text;
//					Common.Configuration c = new Snuggle.Common.Configuration ();
//					c["server"] = generalServer;
//					c["password"] = generalPassword;
//					c["resource"] = generalResource;

					break;
				default:
					break;
			}
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;

			saveButton.Dispose ();
			saveButton = null;

			if (View.Subviews.Length > 0)
				View.Subviews[0].RemoveFromSuperview ();
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (IsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}

		[Export("splitViewController:willHideViewController:withBarButtonItem:forPopoverController:")]
		public void WillHideViewController (UISplitViewController splitController, UIViewController viewController, UIBarButtonItem barButtonItem, UIPopoverController popoverController)
		{
			barButtonItem.Title = "Settings";
			NavigationItem.SetLeftBarButtonItem (barButtonItem, true);
			masterPopoverController = popoverController;
		}
		
		[Export("splitViewController:willShowViewController:invalidatingBarButtonItem:")]
		public void WillShowViewController (UISplitViewController svc, UIViewController vc, UIBarButtonItem button)
		{
			// Called when the view is shown again in the split view, invalidating the button and popover controller.
			NavigationItem.SetLeftBarButtonItem (null, true);
			masterPopoverController = null;
		}

		public void SetDetailItem (string selected)
		{
			Console.WriteLine (selected);
			if (detailItem != selected) {
				detailItem = selected;
				ConfigureView ();
			}

			if (this.masterPopoverController != null)
				this.masterPopoverController.Dismiss (true);
		}

		void ConfigureView ()
		{
			switch (detailItem) {
				case "General":
					NSArray views = NSBundle.MainBundle.LoadNib ("GeneralSettingsView", this, null);
					var view = Runtime.GetNSObject( views.ValueAt (0) ) as UIView;
					this.View.AddSubview (view);
					this.View.BringSubviewToFront (view);
					NavigationItem.SetRightBarButtonItem (saveButton, true);

					var service = Common.Service.GetService (Common.ServiceType.Xmpp);

					var profile = Common.Profile.Lookup ("shana");
					generalName.Text = profile.Name;
					generalUsername.Text = profile.Nickname;
					var settings = profile.SettingsByService (Snuggle.Common.ServiceType.Xmpp);
					generalServer.Text = settings.GetConfiguration (profile, "server").Value;
					generalPassword.Text = settings.GetConfiguration (profile, "password").Value;
					break;
				default:
					break;
			}
		}
	}
}

