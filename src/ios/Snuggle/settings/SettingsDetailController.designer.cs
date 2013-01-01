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
		MonoTouch.UIKit.UITextField generalName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField generalUsername { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField generalServer { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField generalPassword { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField generalResource { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Toolbar != null) {
				Toolbar.Dispose ();
				Toolbar = null;
			}

			if (generalName != null) {
				generalName.Dispose ();
				generalName = null;
			}

			if (generalUsername != null) {
				generalUsername.Dispose ();
				generalUsername = null;
			}

			if (generalServer != null) {
				generalServer.Dispose ();
				generalServer = null;
			}

			if (generalPassword != null) {
				generalPassword.Dispose ();
				generalPassword = null;
			}

			if (generalResource != null) {
				generalResource.Dispose ();
				generalResource = null;
			}
		}
	}
}
