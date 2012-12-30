// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Snuggle
{
	[Register ("MessageViewCell")]
	partial class MessageViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UITableViewCell cellMain { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblMessage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView imgMessage { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (cellMain != null) {
				cellMain.Dispose ();
				cellMain = null;
			}

			if (lblMessage != null) {
				lblMessage.Dispose ();
				lblMessage = null;
			}

			if (imgMessage != null) {
				imgMessage.Dispose ();
				imgMessage = null;
			}
		}
	}
}
