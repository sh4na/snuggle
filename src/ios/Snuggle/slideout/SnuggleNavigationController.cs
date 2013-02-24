using System;

using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	public class SnuggleNavigationController : SlideoutNavigationController
	{
		public static UIViewController[] Views { get; private set; }

		public SnuggleNavigationController () : base ()
		{
			Views = new UIViewController[]
			{ 
				new StreamViewController (),
				new MessagesViewController (),
				new PhotosViewController (),
				new SettingsDetailController ()
			};

			TopView = Views[0];
			MenuView = new SnuggleDialogViewController ();
		}

		private class SnuggleDialogViewController : DialogViewController
		{
			public SnuggleDialogViewController () : base (UITableViewStyle.Plain, new RootElement ("")) { }

			public override void ViewDidLoad ()
			{
				base.ViewDidLoad ();
				
				var rootSection = new Section ();
				foreach (var viewController in Views)
					rootSection.Add (StringElementFromViewController (viewController));
				
				Root.Add (rootSection);
			}
			
			private StyledStringElement StringElementFromViewController (UIViewController viewController)
			{
				return new StyledStringElement (viewController.Title, () => { NavigationController.PushViewController(viewController, true); });
			}
		}
	}
}

