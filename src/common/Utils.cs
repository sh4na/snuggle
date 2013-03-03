using System;

namespace Snuggle.Common
{
	public static class Utils
	{
		public static class Locale
		{
			public static string GetText (string text)
			{
				return text;
			}
		}

		const long TicksOneDay = 864000000000;
		const long TicksOneHour = 36000000000;
		const long TicksMinute = 600000000;


		static string s1 = Locale.GetText ("1 sec");
		static string sn = Locale.GetText (" secs");
		static string m1 = Locale.GetText ("1 min");
		static string mn = Locale.GetText (" mins");
		static string h1 = Locale.GetText ("1 hour");
		static string hn = Locale.GetText (" hours");
		static string d1 = Locale.GetText ("1 day");
		static string dn = Locale.GetText (" days");

		public static string FormatTime (TimeSpan ts)
		{
			int v;
			
			if (ts.Ticks < TicksMinute){
				v = ts.Seconds;
				if (v <= 1)
					return s1;
				else
					return v + sn;
			} else if (ts.Ticks < TicksOneHour){
				v = ts.Minutes;
				if (v == 1)
					return m1;
				else
					return v + mn;
			} else if (ts.Ticks < TicksOneDay){
				v = ts.Hours;
				if (v == 1)
					return h1;
				else
					return v + hn;
			} else {
				v = ts.Days;
				if (v == 1)
					return d1;
				else
					return v + dn;
			}
		}	
	}
}

