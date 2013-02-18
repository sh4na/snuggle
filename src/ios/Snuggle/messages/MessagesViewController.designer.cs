// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Snuggle
{
	[Register ("MessagesViewController")]
	partial class MessagesViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView tblMessages { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnSend { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtMessage { get; set; }

		[Action ("onSend:")]
		partial void onSend (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (tblMessages != null) {
				tblMessages.Dispose ();
				tblMessages = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (btnSend != null) {
				btnSend.Dispose ();
				btnSend = null;
			}

			if (txtMessage != null) {
				txtMessage.Dispose ();
				txtMessage = null;
			}
		}
	}
}
