using System;
using System.Collections.Generic;

namespace Snuggle.Common
{
	public class Payment
	{
		public Profile From { get; set; }
		public Profile To { get; set; }
		public float Amount { get; set; }
		public string CurrencyCode { get; set; }
		public DateTime Time { get; set; }

		public readonly List<Expense> Expenses = new List<Expense> ();

		public Payment (Profile from, Profile to, float amount, string currencyCode, DateTime time)
		{
			From = from;
			To = to;
			Amount = amount;
			CurrencyCode = currencyCode;
			Time = time;
		}
	}
}

