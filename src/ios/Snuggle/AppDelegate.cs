using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Snuggle
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
	
		// class-level declarations
		UIWindow window;
		UITabBarController tabBarController;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			var storage = Common.Storage.LocalStorage;

			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			// tab navigation
			tabBarController = new UITabBarController ();
			tabBarController.ViewControllers = new UIViewController[]
			{ 
				new StreamViewController (),
				new MessagesViewController (),
				new PhotosViewController (),
				GetSettingsController ()
			};
			window.RootViewController = tabBarController;

			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}

		public UIViewController GetSettingsController ()
		{
			var master = new SettingsListController ();
			var masterNavigationController = new UINavigationController (master);
			UIViewController rootController = masterNavigationController;

			if (!UserInterfaceIdiomIsPhone) {
				var splitViewController = new UISplitViewController ();
				var detail = new SettingsDetailController ();
				var detailNavigationController = new UINavigationController (detail);
				splitViewController.WeakDelegate = detail;
				splitViewController.ViewControllers = new UIViewController[] {
					masterNavigationController,
					detailNavigationController
				};
				rootController = splitViewController;
			}
			rootController.Title = "Settings";
			var tabBarIcon = new UITabBarItem ("Settings", UIImage.FromFile ("assets/tabbar_settings.png"), 0);
			rootController.TabBarItem = tabBarIcon;
			return rootController;
		}
	}
}

