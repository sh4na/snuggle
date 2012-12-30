using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Snuggle
{
	public class MessageTableViewItem
	{
		public string Message { get; set; }

		public string ImageName { get; set; }
	}

	public class MessageTableViewItemGroup
	{
		public string Name { get; set; }

		public string Footer { get; set; }

		public List<MessageTableViewItem> Items
		{
			get { return this._items; }
			set { this._items = value; }
		}
		protected List<MessageTableViewItem> _items = new List<MessageTableViewItem>();
	}

	//========================================================================
	/// <summary>
	/// Combined DataSource and Delegate for our UITableView with custom cells
	/// </summary>
	public class MessageTableSource : UITableViewSource
	{
		//---- declare vars
		protected List<MessageTableViewItemGroup> _tableItems;
		protected string _customCellIdentifier = "MessageViewCell";
		protected Dictionary<int, MessageViewCell> _cellControllers =
		new Dictionary<int, MessageViewCell>();

		public MessageTableSource (List<MessageTableViewItemGroup> items)
		{
			this._tableItems = items;
		}

		/// <summary>
		/// Called by the TableView to determine how many sections(groups) there are.
		/// </summary>
		public override int NumberOfSections (UITableView tableView)
		{
			return this._tableItems.Count;
		}

		/// <summary>
		/// Called by the TableView to determine how many cells to create for that particular section.
		/// </summary>
		public override int RowsInSection (UITableView tableview, int section)
		{
			return this._tableItems[section].Items.Count;
		}

		/// <summary>
		/// Called by the TableView to retrieve the header text for the particular section(group)
		/// </summary>
		public override string TitleForHeader (UITableView tableView, int section)
		{
			return this._tableItems[section].Name;
		}

		/// <summary>
		/// Called by the TableView to retrieve the footer text for the particular section(group)
		/// </summary>
		public override string TitleForFooter (UITableView tableView, int section)
		{
			return this._tableItems[section].Footer;
		}

		/// <summary>
		/// Called by the TableView to retreive the height of the row for the particular section and row
		/// </summary>
		public override float GetHeightForRow (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			MessageTableViewItem item = this._tableItems[indexPath.Section].Items[indexPath.Row];
			return (float) (44 * (Math.Ceiling((double)item.Message.Length / 22)));
		}

		/// <summary>
		/// Called by the TableView to get the actual UITableViewCell to render for the particular section and row
		/// </summary>
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			//---- declare vars
			UITableViewCell cell = tableView.DequeueReusableCell (this._customCellIdentifier);
			MessageViewCell customCellController = null;

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
			{
				var id = Environment.TickCount;
				while (_cellControllers.ContainsKey (id))
					id = Environment.TickCount;
				customCellController = new MessageViewCell ();
				// retreive the cell from our custom cell controller
				cell = customCellController.Cell;
				// give the cell a unique ID, so we can match it up to the controller
				cell.Tag = id;
				// store our controller with the unique ID we gave our cell
				this._cellControllers.Add (cell.Tag, customCellController);
			}
			else
			{
				// retreive our controller via it's unique ID
				customCellController = this._cellControllers[cell.Tag];
			}

			//---- create a shortcut to our item
			MessageTableViewItem item = this._tableItems[indexPath.Section].Items[indexPath.Row];

			//---- set our cell properties
			customCellController.Message = item.Message;

//			if (!string.IsNullOrEmpty (item.ImageName))
//			{
//				customCellController.Image = UIImage.FromFile ("Images/" + item.ImageName);
//			}

			//---- return the custom cell
			return cell;
		}

	}
}
