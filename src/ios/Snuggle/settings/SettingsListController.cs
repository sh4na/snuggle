
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Snuggle
{
	public partial class SettingsListController : UITableViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SettingsListController ()
			: base (UserInterfaceIdiomIsPhone ? "SettingsListController_iPhone" : "SettingsListController_iPad", null)
		{
			this.Title = "Settings";
			if (!UserInterfaceIdiomIsPhone) {
				this.ClearsSelectionOnViewWillAppear = false;
				this.ContentSizeForViewInPopover = new SizeF (320f, 600f);
			}
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
			this.TableView.Source = new DataSource (this, new string[]{"General", "Theme"});
			if (!UserInterfaceIdiomIsPhone)
				this.TableView.SelectRow (NSIndexPath.FromRowSection (0, 0), false, UITableViewScrollPosition.Middle);
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
			if (UserInterfaceIdiomIsPhone) {
				return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
			} else {
				return true;
			}
		}

	
		class DataSource : UITableViewSource
		{
			string[] tableItems;
			const string cellIdentifier = "SettingsCell";
			UIViewController controller;
			
			public DataSource (UIViewController controller, string[] items)
			{
				this.controller = controller;
				this.tableItems = items;
			}
			
			// Customize the number of sections in the table view.
			public override int NumberOfSections (UITableView tableView)
			{
				return 1;
			}
			
			public override int RowsInSection (UITableView tableview, int section)
			{
				return tableItems.Length;
			}
			
			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell (cellIdentifier);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
					if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone) {
						cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
					}
				}
				
				// Configure the cell.
				cell.TextLabel.Text = tableItems[indexPath.Row];
				return cell;
			}
			

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				SettingsDetailController detail;
				if (UserInterfaceIdiomIsPhone) {
					detail = new SettingsDetailController ();
					controller.NavigationController.PushViewController (detail, true);
				} else
					detail = ((UINavigationController)controller.SplitViewController.ViewControllers[1]).TopViewController as SettingsDetailController;

				detail.SetDetailItem (tableItems[indexPath.Row]);
			}
		}
	}
}

