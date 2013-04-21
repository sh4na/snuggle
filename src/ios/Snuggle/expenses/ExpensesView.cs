using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace Snuggle
{
	using Locale = Common.Utils.Locale;
	using Common;
	public class ExpensesView : DialogViewController
	{
		public override string Title {
			get { return Locale.GetText ("Expenses"); }
			set { }
		}

		public ExpensesView () : base (UITableViewStyle.Grouped, null)
		{
			var payments = GetDummyPayments ();
			var expenses = GetDummyExpenses ();

			Root = new RootElement (Title);

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
			for (int n = 0; n < 2; n++) {
				var payment = new Payment (Profile.Current, Profile.Current, 200f, "DKK", DateTime.Now);
				payment.Expenses.AddRange (GetDummyExpenses (ExpenseStatus.Paid));
				payments.Add (payment);
			}

			return payments;
		}

		private IEnumerable<Expense> GetDummyExpenses (ExpenseStatus status = ExpenseStatus.Pending)
		{
			List<Expense> expenses = new List<Expense> ();
			for (int n = 0; n < 10; n++) {
				expenses.Add (new Expense(Common.Profile.Current, 1000f, 500f, "DKK", DateTime.Now, "Expense " + n, "Food", status));
			}

			return expenses;
		}
	}
}

