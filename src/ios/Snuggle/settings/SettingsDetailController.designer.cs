// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Snuggle
{
	[Register ("SettingsDetailController")]
	partial class SettingsDetailController
	{
		[Outlet]
		MonoTouch.UIKit.UIToolbar Toolbar { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtNickname { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField xmppUsername { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField xmppServer { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField xmppPassword { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField xmppResource { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField xmppHost { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView scrollView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Toolbar != null) {
				Toolbar.Dispose ();
				Toolbar = null;
			}

			if (txtName != null) {
				txtName.Dispose ();
				txtName = null;
			}

			if (txtNickname != null) {
				txtNickname.Dispose ();
				txtNickname = null;
			}

			if (xmppUsername != null) {
				xmppUsername.Dispose ();
				xmppUsername = null;
			}

			if (xmppServer != null) {
				xmppServer.Dispose ();
				xmppServer = null;
			}

			if (xmppPassword != null) {
				xmppPassword.Dispose ();
				xmppPassword = null;
			}

			if (xmppResource != null) {
				xmppResource.Dispose ();
				xmppResource = null;
			}

			if (xmppHost != null) {
				xmppHost.Dispose ();
				xmppHost = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
		}
	}
}
