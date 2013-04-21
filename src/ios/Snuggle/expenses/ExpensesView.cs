using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	using Locale = Common.Utils.Locale;
	using Expense = Common.Expense;
	using Payment = Common.Payment;
	public class ExpensesView : DialogViewController
	{
		public ExpensesView () : base (UITableViewStyle.Grouped, null)
		{
			var payments = GetDummyPayments ();
			var expenses = GetDummyExpenses ();

			Root = new RootElement (Locale.GetText ("Expenses"));

			foreach (var payment in payments) {
				Root.Add (new Section () { new PaymentElement (payment) } );
			}

			var outstandingExpensesSection = new Section (Locale.GetText ("Outstanding Expenses"));
			foreach (var expense in expenses)
				outstandingExpensesSection.Add (new ExpenseElement (expense));
			
			Root.Add (outstandingExpensesSection);
		}

		private IEnumerable<Payment> GetDummyPayments ()
		{
			List<Payment> payments = new List<Payment> ();
			for (int n = 0; n < 3; n++) {
				payments.Add (new Payment (Common.Profile.Current, null, 200f, "da-DK", DateTime.Now));
			}

			return payments;
		}

		private IEnumerable<Expense> GetDummyExpenses ()
		{
			List<Expense> expenses = new List<Expense> ();
			for (int n = 0; n < 10; n++) {
				expenses.Add (new Expense(Common.Profile.Current, 1000f, "da-DK", DateTime.Now, "Expense " + n, "Food", Common.ExpenseStatus.Pending));
			}

			return expenses;
		}
	}
}

