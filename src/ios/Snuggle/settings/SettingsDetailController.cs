
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

namespace Snuggle
{
	public partial class SettingsDetailController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		UIPopoverController masterPopoverController;
		string detailItem;

		public SettingsDetailController ()
			: base (UserInterfaceIdiomIsPhone ? "SettingsDetailController_iPhone" : "SettingsDetailController_iPad", null)
		{
		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			ConfigureView ();
		}
		
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			// Clear any references to subviews of the main view in order to
			// allow the Garbage Collector to collect them sooner.
			//
			// e.g. myOutlet.Dispose (); myOutlet = null;
			
			ReleaseDesignerOutlets ();
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			if (UserInterfaceIdiomIsPhone) {
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
					NSArray views = NSBundle.MainBundle.LoadNib("GeneralSettingsView", this, null);
					var view = Runtime.GetNSObject( views.ValueAt(0) ) as UIView;
					this.View.AddSubview (view);
					this.View.BringSubviewToFront (view);
					break;
				default:
					break;
			}
		}
	}
}

