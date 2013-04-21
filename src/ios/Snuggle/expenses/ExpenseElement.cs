using System;
using System.Globalization;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	using Expense = Common.Expense;
	public class ExpenseElement : Element
	{
		private const string key = "expenseelement";

		private string name { get { return expense.Description; } }
		private string amountString { get { return expense.Amount.ToString ("C2", CultureInfo.CreateSpecificCulture(expense.CurrencyCode)); } }

		private Expense expense;

		public ExpenseElement (Expense expense) : base (null)
		{
			this.expense = expense;
		}

		public override UITableViewCell GetCell (UITableView tableView)
		{
			UITableViewCell cell = tableView.DequeueReusableCell (key);
			if (cell == null)
			{
				cell = new UITableViewCell (UITableViewCellStyle.Value1, key);
				cell.SelectionStyle = (UITableViewCellSelectionStyle.Blue);
			}

			cell.Accessory = UITableViewCellAccessory.None;
			cell.TextLabel.Text = this.name;
			cell.TextLabel.TextAlignment = UITextAlignment.Left;
			cell.DetailTextLabel.Text = this.amountString;
			return cell;
		}
	}
}

