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
		public string Currency { get; set; }
		public float ExchangeRate { get; set; }
		public DateTime Date { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public ExpenseStatus Status { get; set; }

		public Expense (Profile profile, float amount, float owed, string currency, DateTime date, string description, string category, ExpenseStatus status, float exchangeRate = 1f)
		{
			Profile = profile;
			Amount = amount;
			Owed = owed;
			Currency = currency;
			Date = date;
			Description = description;
			Category = category;
			Status = status;
			ExchangeRate = exchangeRate;
		}
	}
}

