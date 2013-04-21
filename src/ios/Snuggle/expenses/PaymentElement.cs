using System;
using System.Globalization;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	using Payment = Common.Payment;
	public class PaymentElement : Element
	{
		private const string key = "paymentelement";

		private Payment payment;

		public PaymentElement (Payment payment) : base (null)
		{
			this.payment = payment;
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

