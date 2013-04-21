using System;
using System.Collections.Generic;

namespace Snuggle.Common
{
	public class Payment
	{
		public Profile From { get; set; }
		public Profile To { get; set; }
		public float Amount { get; set; }
		public string Currency { get; set; }
		public DateTime Date { get; set; }

		public readonly List<Expense> Expenses = new List<Expense> ();

		public Payment (Profile from, Profile to, float amount, string currencyCode, DateTime date)
		{
			From = from;
			To = to;
			Amount = amount;
			Currency = currencyCode;
			Date = date;
		}
	}
}

