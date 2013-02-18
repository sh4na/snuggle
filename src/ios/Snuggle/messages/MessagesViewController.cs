
using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;



namespace Snuggle
{
	public partial class MessagesViewController : SnuggleViewController
	{
		List<MessageTableViewItemGroup> tableItems;
		List<MessageTableViewItem> items;
		Common.XmppSession session;

		public override string Title {
			get { return "Messages"; }
			set { }
		}
		
		public MessagesViewController () : base ("MessagesViewController") { }
		
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

			scrollView.Frame = UIScreen.MainScreen.Bounds;
			scrollView.ContentSize = new SizeF (UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
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
			if (IsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if (tableItems == null)
			{
				tableItems = new List<MessageTableViewItemGroup> ();
				items = new List<MessageTableViewItem> ();
//				items.Add (new MessageTableViewItem () { Message = "12334 567890 123345 678901 233456 7890" });
//				items.Add (new MessageTableViewItem () { Message = "message 2" });
//				items.Add (new MessageTableViewItem () { Message = "message 3" });

				tableItems.Add (new MessageTableViewItemGroup () { Items = items } );
				tblMessages.Source = new MessageTableSource (tableItems);
			}
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			session = new Common.XmppSession (Common.XmppProfile.Current);
			session.Start ();
			Common.XmppService.OnEvent += HandleOnEvent;
		}

		void HandleOnEvent (Snuggle.Common.ISession session, string from, object msg)
		{
			items.Add (new MessageTableViewItem () { From = from, Message = msg as string } );

			InvokeOnMainThread(delegate{
				tblMessages.ReloadData ();
				tblMessages.ScrollToRow (NSIndexPath.FromRowSection(items.Count - 1, 0), UITableViewScrollPosition.Bottom, true);
			});
		}

		protected override UIView KeyboardGetActiveView ()
		{
			return btnSend;
		}

		partial void onSend (MonoTouch.Foundation.NSObject sender)
		{
			session.Send (items[items.Count - 1].From, txtMessage.Text);
		}
	}
}

