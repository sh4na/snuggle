using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Snuggle
{
	[Activity (Label = "Snuggle", MainLauncher = true)]
	public class Activity1 : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// TABS!
			this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
			AddTabbedFragment<StreamFragment> ("Stream");
			AddTabbedFragment<MessagesFragment> ("Messages");
			AddTabbedFragment<PhotosFragment> ("Photos");
		}

		private void AddTabbedFragment<T> (string tabName) where T : Fragment, new ()
		{
			var tab = this.ActionBar.NewTab ();
			tab.SetText (tabName);

			// must set event handler before adding tab
			tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e) {
				e.FragmentTransaction.Replace (Resource.Id.FragmentContainer, new T ());
			};
			
			this.ActionBar.AddTab (tab);
		}
	}
}
