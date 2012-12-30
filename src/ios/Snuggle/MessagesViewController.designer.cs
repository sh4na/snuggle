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

		void ReleaseDesignerOutlets ()
		{
			if (tblMessages != null) {
				tblMessages.Dispose ();
				tblMessages = null;
			}
		}
	}
}
