
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Snuggle
{
	public partial class MessageViewCell : UIViewController
	{
		public UITableViewCell Cell
		{
			get { return this.cellMain; }
		}

		public string Message
		{
			get { return this.lblMessage.Text; }
			set {
				this.lblMessage.Text = value;
				this.lblMessage.Lines = (int)Math.Ceiling((double)value.Length / 22);
			}
		}

		public UIImage Image
		{
			get { return this.imgMessage.Image; }
			set { this.imgMessage.Image = value; }
		}

/*		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}


		public MessageViewCell ()
			: base (UserInterfaceIdiomIsPhone ? "MessageViewCell_iPhone" : "MessageViewCell_iPad", null)
		{
		}
*/

		public MessageViewCell ()
		{
			MonoTouch.Foundation.NSBundle.MainBundle.LoadNib ("MessageViewCell_iPad", this, null);
			Initialize ();
		}

		void Initialize ()
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
//			if (UserInterfaceIdiomIsPhone) {
//				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
//			} else {
				return true;
//			}
		}
	}
}
