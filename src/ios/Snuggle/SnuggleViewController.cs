using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Snuggle
{
	public class SnuggleViewController : UIViewController
	{
		NSObject keyboardObserverWillShow;
		NSObject keyboardObserverWillHide;

		public SnuggleViewController (string nibPrefix) : base (GetDefaultNibName (nibPrefix), null) { }

		UITabBarItem tabBarItem;
		public override UITabBarItem TabBarItem {
			get {
				if (tabBarItem == null)
					tabBarItem = new UITabBarItem (this.Title, TabBarImageFromName (this.Title), 0);
				return tabBarItem;
			}
			set {
				tabBarItem = value;
			}
		}

		private static string GetDefaultNibName (string nibPrefix)
		{
			return nibPrefix + (IsPhone ? "_iPhone" : "_iPad");
		}
		
		protected static bool IsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		private static UIImage TabBarImageFromName (string name)
		{
			var file = String.Format ("assets/tabbar_{0}.png", name.ToLower ());
			return UIImage.FromFile (file);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			RegisterForKeyboardNotifications();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

			UnregisterKeyboardNotifications();
		}

		protected virtual void RegisterForKeyboardNotifications ()
		{
			keyboardObserverWillShow = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, KeyboardWillShowNotification);
			keyboardObserverWillHide = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, KeyboardWillHideNotification);
		}

		protected virtual void UnregisterKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver (keyboardObserverWillShow);
			NSNotificationCenter.DefaultCenter.RemoveObserver (keyboardObserverWillHide);
		}

		/// <summary>
		/// Gets the UIView that represents the "active" user input control (e.g. textfield, or button under a text field)
		/// </summary>
		/// <returns>
		/// A <see cref="UIView"/>
		/// </returns>
		protected virtual UIView KeyboardGetActiveView()
		{
			return this.View.FindFirstResponder();
		}

		protected virtual void KeyboardWillShowNotification (NSNotification notification)
		{
			UIView activeView = KeyboardGetActiveView ();
			if (activeView == null)
				return;
			
			UIScrollView scrollView = activeView.FindSuperviewOfType (this.View, typeof (UIScrollView)) as UIScrollView;
			if (scrollView == null)
				return;
			
			RectangleF keyboardBounds = UIKeyboard.BoundsFromNotification (notification);
			
			UIEdgeInsets contentInsets = new UIEdgeInsets (0.0f, 0.0f, keyboardBounds.Size.Height, 0.0f);
			scrollView.ContentInset = contentInsets;
			scrollView.ScrollIndicatorInsets = contentInsets;
			
			// If activeField is hidden by keyboard, scroll it so it's visible
			RectangleF viewRectAboveKeyboard = new RectangleF (this.View.Frame.Location, new SizeF (this.View.Frame.Width, this.View.Frame.Size.Height - keyboardBounds.Size.Height));
			
			RectangleF activeFieldAbsoluteFrame = activeView.Superview.ConvertRectToView (activeView.Frame, this.View);
			// activeFieldAbsoluteFrame is relative to this.View so does not include any scrollView.ContentOffset
			
			// Check if the activeField will be partially or entirely covered by the keyboard
			if (!viewRectAboveKeyboard.Contains(activeFieldAbsoluteFrame))
			{
				// Scroll to the activeField Y position + activeField.Height + current scrollView.ContentOffset.Y - the keyboard Height
				PointF scrollPoint = new PointF (0.0f, activeFieldAbsoluteFrame.Location.Y + activeFieldAbsoluteFrame.Height + scrollView.ContentOffset.Y - viewRectAboveKeyboard.Height);
				scrollView.SetContentOffset (scrollPoint, true);
			}
		}
		
		protected virtual void KeyboardWillHideNotification (NSNotification notification)
		{
			UIView activeView = KeyboardGetActiveView ();
			if (activeView == null)
				return;
			
			UIScrollView scrollView = activeView.FindSuperviewOfType (this.View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
				return;
			
			// Reset the content inset of the scrollView and animate using the current keyboard animation duration
			double animationDuration = UIKeyboard.AnimationDurationFromNotification (notification);
			UIEdgeInsets contentInsets = new UIEdgeInsets (0.0f, 0.0f, 0.0f, 0.0f);
			UIView.Animate (animationDuration, delegate{
				scrollView.ContentInset = contentInsets;
				scrollView.ScrollIndicatorInsets = contentInsets;
			});
		}
	}

	public static class ViewExtensions
	{
		/// <summary>
		/// Find the first responder in the <paramref name="view"/>'s subview hierarchy
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> that is the first responder or null if there is no first responder
		/// </returns>
		public static UIView FindFirstResponder (this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews) {
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}
		
		/// <summary>
		/// Find the first Superview of the specified type (or descendant of)
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <param name="stopAt">
		/// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
		/// </param>
		/// <param name="type">
		/// A <see cref="Type"/> to look for, this should be a UIView or descendant type
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> if it is found, otherwise null
		/// </returns>
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsAssignableFrom(view.Superview.GetType()))
				{
					return view.Superview;
				}
				
				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}
			
			return null;
		}
	}
}

