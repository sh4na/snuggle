using System;

namespace Snuggle.Common
{
	public enum ExpenseStatus
	{
		Pending,
		Paid
	}

	public class Expense
	{
		public Profile Profile { get; set; }
		public float Amount { get; set; }
		public float Owed { get; set; }
		public string CurrencyCode { get; set; }
		public DateTime Time { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public ExpenseStatus Status { get; set; }

		public Expense (Profile profile, float amount, float owed, string currencyCode, DateTime time, string description, string category, ExpenseStatus status)
		{
			Profile = profile;
			Amount = amount;
			Owed = owed;
			CurrencyCode = currencyCode;
			Time = time;
			Description = description;
			Category = category;
			Status = status;
		}
	}
}

