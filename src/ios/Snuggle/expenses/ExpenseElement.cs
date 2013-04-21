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
		private string amountString { get { return expense.Currency + " " + expense.Amount.ToString ("N2", CultureInfo.CurrentUICulture); } }

		private Expense expense;

		public ExpenseElement (Expense expense) : base (expense.Description)
		{
			this.expense = expense;

			this.Add (new Section () {
				new StringElement ("Paid By", this.expense.Profile.Name),
				new StringElement ("Amount", this.expense.Currency + " " + this.expense.Amount.ToString ("N2", CultureInfo.CurrentUICulture)),
				new StringElement ("Owed", this.expense.Currency + " " + this.expense.Owed.ToString ("N2", CultureInfo.CurrentUICulture)),
				new StringElement ("Date", this.expense.Date.ToLongDateString ()),
				new StringElement ("Category", this.expense.Category),
				new StringElement ("Status", this.expense.Status.ToString ())
			});

			if (this.expense.Status == Common.ExpenseStatus.Paid)
				this[0].Add (new StringElement ("Exchange Rate", this.expense.ExchangeRate.ToString ("N4", CultureInfo.CurrentUICulture)));
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

