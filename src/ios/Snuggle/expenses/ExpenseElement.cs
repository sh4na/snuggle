using System;
using System.Globalization;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	using Expense = Common.Expense;
	public class ExpenseElement : RootElement
	{
		private const string key = "expenseelement";

		private string name { get { return expense.Description; } }
		private string amountString { get { return expense.Amount.ToString ("C2", CultureInfo.CreateSpecificCulture(expense.CurrencyCode)); } }

		private Expense expense;

		public ExpenseElement (Expense expense) : base (expense.Description)
		{
			this.expense = expense;

			this.Add (new Section () {
				new StringElement ("Paid By", this.expense.Profile.Name),
				new StringElement ("Amount", this.expense.Amount.ToString ("C2", CultureInfo.CreateSpecificCulture (this.expense.CurrencyCode))),
				new StringElement ("Currency", this.expense.CurrencyCode),
				new StringElement ("Date", this.expense.Time.ToLongDateString ()),
				new StringElement ("Category", this.expense.Category),
				new StringElement ("Status", this.expense.Status.ToString ())
			});
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

