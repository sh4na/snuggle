
using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Snuggle
{
	public partial class MessagesViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MessagesViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MessagesViewController_iPhone" : "MessagesViewController_iPad", null)
		{
			this.Title = "Messages";
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

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			var items = new List<MessageTableViewItem> ();
			items.Add (new MessageTableViewItem () { Message = "12334 567890 123345 678901 233456 7890" });
			items.Add (new MessageTableViewItem () { Message = "message 2" });
			items.Add (new MessageTableViewItem () { Message = "message 3" });
			List<MessageTableViewItemGroup> tableItems = new List<MessageTableViewItemGroup> ();
			tableItems.Add (new MessageTableViewItemGroup () { Items = items } );

			this.tblMessages.Source = new MessageTableSource (tableItems);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}
	}
}

