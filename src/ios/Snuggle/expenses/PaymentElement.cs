using System;
using System.Globalization;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	using Payment = Common.Payment;
	public class PaymentElement : RootElement
	{
		private const string key = "paymentelement";

		private Payment payment;

		public PaymentElement (Payment payment) : base ("Payment")
		{
			this.payment = payment;

			this.Add (new Section () { 
				new StringElement ("From", this.payment.From.Name),
				new StringElement ("To", this.payment.To.Name),
				new StringElement ("Date", this.payment.Time.ToLongDateString ()),
				new StringElement ("Amount", this.payment.Amount.ToString("C2", CultureInfo.CreateSpecificCulture (this.payment.CurrencyCode))),
				new StringElement ("Currency", this.payment.CurrencyCode)
			});

			var expensesSection = new Section ("Expenses");
			foreach (var expense in this.payment.Expenses)
				expensesSection.Add (new ExpenseElement (expense));

			this.Add (expensesSection);
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
			cell.TextLabel.Text = this.payment.Time.ToShortDateString ();
			cell.TextLabel.TextAlignment = UITextAlignment.Left;
			cell.DetailTextLabel.Text = this.payment.Amount.ToString ("C2", CultureInfo.CreateSpecificCulture (this.payment.CurrencyCode));
			return cell;
		}
	}
}

