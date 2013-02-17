using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Snuggle
{
	public class SnuggleViewController : UIViewController
	{
		public SnuggleViewController (string nibPrefix) : base (GetDefaultNibName (nibPrefix), null) { }

		UITabBarItem tabBarItem;
		public override UITabBarItem TabBarItem {
			get {
				if (tabBarItem == null)
					tabBarItem = new UITabBarItem (this.Title, TabBarImageFromName (this.Title), 0);
				return tabBarItem;
			}
			set {
				tabBarItem = value;
			}
		}

		private static string GetDefaultNibName (string nibPrefix)
		{
			return nibPrefix + (IsPhone ? "_iPhone" : "_iPad");
		}
		
		protected static bool IsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		private static UIImage TabBarImageFromName (string name)
		{
			var file = String.Format ("assets/tabbar_{0}.png", name.ToLower ());
			return UIImage.FromFile (file);
		}
	}
}

