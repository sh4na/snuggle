using System;
using System.Drawing;
using System.Collections;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;

namespace Snuggle
{
	using Common;
	using Locale = Common.Utils.Locale;

	public class Messages : UIViewController
	{
		public override string Title {
			get { return Locale.GetText ("Chat"); }
			set { }
		}

		MessagesList messagesList;
		//DialogViewController messageDialog;
		//EntryElement messageEntry;

		UITextField messageEntry;
		float currentEntryHeight;

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			// TODO: move this to a base SnuggleViewController class
			var tap = new UITapGestureRecognizer ();
			tap.AddTarget (() =>{
				View.EndEditing (true);
			});
			View.AddGestureRecognizer (tap);
			tap.CancelsTouchesInView = false;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			messageEntry = new UITextField ();
			messageEntry.BackgroundColor = UIColor.White;
			messageEntry.AutocapitalizationType = UITextAutocapitalizationType.None;
			messageEntry.ReturnKeyType = UIReturnKeyType.Send;
			messageEntry.BorderStyle = UITextBorderStyle.RoundedRect;
			messageEntry.ShouldReturn += delegate {
				if (XmppSession.Current != null && XmppSession.Current.Running) {
					if (XmppSession.Current.Send (new Message (Message.MessageType.Text, Common.XmppProfile.Current.Username, Common.XmppProfile.Current.Buddy, DateTime.Now, null, messageEntry.Text))) {
						//do it in red?
					}
					messagesList.TriggerRefresh ();
					messageEntry.Text = "";
				}
				return true;
			};

			//messageEntry = new EntryElement ("Type:", "", "");


			//messageEntry.Changed += (sender, e) => currentEntryHeight = messageEntry.GetCell (messageDialog.TableView).Bounds.Height;
			//messageEntry.EntryEnded += (sender, data) => {
			//	if (XmppSession.RunningSession != null && XmppSession.RunningSession.Running)
			//		XmppSession.RunningSession.Send (XmppProfile.Current.Buddy, messageEntry.Value);
			//};

			//var root = new RootElement ("Entry");
			//var messageSection = new Section ();
			//messageSection.Add (messageEntry);
			//root.Add (messageSection);

			messagesList = new MessagesList ();
			//messageDialog = new DialogViewController (UITableViewStyle.Plain, root);

			View = new UIScrollView ();

			AddChildViewController (messagesList);
			//AddChildViewController (messageDialog);
			View.AddSubview (messagesList.View);
			//View.AddSubview (messageDialog.View);
			View.AddSubview (messageEntry);

			RegisterForKeyboardNotifications();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			
			UnregisterKeyboardNotifications();
		}

		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();

			//float height = messageEntry.GetCell (messageDialog.TableView).Bounds.Height;
			float height = 44;
			messagesList.View.Frame = new RectangleF (0, 0, this.View.Bounds.Width, this.View.Bounds.Height - height);
			//messageDialog.View.Frame = new RectangleF (0, this.View.Bounds.Height - height, this.View.Bounds.Width, height);
			messageEntry.Frame = new RectangleF (0, this.View.Bounds.Height - height, this.View.Bounds.Width, height);
		}

		NSObject keyboardObserverWillShow;
		NSObject keyboardObserverWillHide;

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

		protected virtual UIView KeyboardGetActiveView()
		{
			//if (messagesList.Root[0].Elements.Count > 0)
			//	return messagesList.Root[0].Elements[messagesList.Root[0].Elements.Count - 1].GetCell (messagesList.TableView);
			//return messageDialog.View;
			return messageEntry;
			return null;
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
			messagesList.TableView.ContentInset = new UIEdgeInsets (keyboardBounds.Height, 0, 0, 0);
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
			RectangleF keyboardBounds = UIKeyboard.BoundsFromNotification (notification);
			messagesList.TableView.ContentInset = new UIEdgeInsets (0, 0, 0, 0);
		}

	}

	public class MessagesList : DialogViewController
	{
		public MessagesList () : base (UITableViewStyle.Plain, new RootElement ("") { UnevenRows = true })
		{
			Autorotate = true;
			EnableSearch = false;
			AutoHideSearch = true;
			Root.Add (new Section ());
			this.RefreshRequested += (sender, e) => {
				for (int i = Root[0].Elements.Count; i < XmppSession.Current.Messages.Count; i++)
					Root[0].Add (new MessageElement (XmppSession.Current.Messages[i]));
				var inset = TableView.ContentInset;
				ReloadComplete ();
				TableView.ContentInset = inset;
				ScrollLastEntryToView ();
			};
			Service.OnEvent += HandleOnEvent;
		}

		void ScrollLastEntryToView ()
		{
			TableView.ScrollToRow (Root[0].Elements[Root[0].Elements.Count - 1].IndexPath, UITableViewScrollPosition.Middle, true);
		}

		public override void ViewDidLoad ()
		{
			XmppSession.StartSession ();
			base.ViewDidLoad ();
		}

		void HandleOnEvent (Snuggle.Common.ISession session, Message message)
		{
			BeginInvokeOnMainThread (delegate {
				TriggerRefresh ();
			});
		}


		class MessageElement : Element, IElementSizing
		{
			static NSString key = new NSString ("messageelement");
			public Message Message;

			public MessageElement (Message message) : base (null)
			{
				Message = message;
			}

			public override UITableViewCell GetCell (UITableView tv)
			{
				var cell = tv.DequeueReusableCell (key) as MessageCell;
				if (cell == null)
					cell = new MessageCell (UITableViewCellStyle.Default, key, Message);
				else
					cell.Update (Message);
				
				return cell;
			}

			public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
			{
				//var profile = new DetailTweetViewController (Tweet);
				//dvc.ActivateController (profile);
			}
			
			public override bool Matches (string text)
			{
				return Message.Body.IndexOf (text, StringComparison.CurrentCultureIgnoreCase) != -1;
			}
			
			#region IElementSizing implementation
			public float GetHeight (UITableView tableView, NSIndexPath indexPath)
			{
				return MessageCell.GetCellHeight (tableView.Bounds, Message);
			}
			#endregion
		}

		class MessageCell : UITableViewCell
		{
			const int userSize = 14;
			const int textSize = 15;
			const int timeSize = 12;
			
			const int PicSize = 48;
			const int PicXPad = 10;
			const int PicYPad = 5;
			
			const int PicAreaWidth = 2 * PicXPad + PicSize;
			
			const int TextHeightPadding = 4;
			const int TextWidthPadding = 4;
			const int TextYOffset = userSize + 8;
			const int MinHeight = PicSize + 2 * PicYPad;
			const int TimeWidth = 46;
			
			static UIFont userFont = UIFont.BoldSystemFontOfSize (userSize);
			static UIFont textFont = UIFont.SystemFontOfSize (textSize);
			static UIFont timeFont = UIFont.SystemFontOfSize (timeSize);
			static UIColor timeColor = UIColor.FromRGB (147, 170, 204);


			MessageView messageView;

			static CGGradient bottomGradient, topGradient;
			static MessageCell ()
			{
				using (var rgb = CGColorSpace.CreateDeviceRGB()){
					float [] colorsBottom = {
						1, 1, 1, .5f,
						0.93f, 0.93f, 0.93f, .5f
					};
					bottomGradient = new CGGradient (rgb, colorsBottom, null);
					float [] colorsTop = {
						0.93f, 0.93f, 0.93f, .5f,
						1, 1, 1, 0.5f
					};
					topGradient = new CGGradient (rgb, colorsTop, null);
				}
			}

			public MessageCell (UITableViewCellStyle style, NSString ident, Message message) : base (style, ident)
			{
				SelectionStyle = UITableViewCellSelectionStyle.Blue;
				
				messageView = new MessageView (message);
				Update (message);
				ContentView.Add (messageView);
			}

			public void Update (Message message)
			{
				messageView.Update (message);
				SetNeedsDisplay ();
			}

			public static float GetCellHeight (RectangleF bounds, Message message)
			{
				bounds.Height = 999;
				
				// Keep the same as LayoutSubviews
				bounds.X = 0;
				bounds.Width -= PicAreaWidth+TextWidthPadding;
				
				using (var nss = new NSString (message.Body)){
					var dim = nss.StringSize (textFont, bounds.Size, UILineBreakMode.WordWrap);
					return Math.Max (dim.Height + TextYOffset + 2*TextWidthPadding, MinHeight);
				}
			}

			public override void LayoutSubviews ()
			{
				base.LayoutSubviews ();
				
				messageView.Frame = ContentView.Bounds;
				messageView.SetNeedsDisplay ();
			}

			class MessageView : UIView, IImageUpdated
			{
				Message message;


				public MessageView (Message message) : base ()
				{
					Update (message);
					Opaque = true;
					BackgroundColor = UIColor.White;
				}

				public void Update (Message message)
				{
					this.message = message;

				}

				public override void Draw (System.Drawing.RectangleF rect)
				{
					var context = UIGraphics.GetCurrentContext ();
					UIColor textColor;

					var bounds = Bounds;
					var midx = bounds.Width/2;
					UIColor.White.SetColor ();
					context.FillRect (bounds);
					context.DrawLinearGradient (bottomGradient, new PointF (midx, bounds.Height-17), new PointF (midx, bounds.Height), 0);
					context.DrawLinearGradient (topGradient, new PointF (midx, 1), new PointF (midx, 3), 0);
					textColor = UIColor.Black;

					float xPic, xText;
					xText = PicAreaWidth;
					xPic = PicXPad;

					textColor.SetColor ();
					DrawString (message.From, new RectangleF (xText, TextHeightPadding, bounds.Width-PicAreaWidth-TextWidthPadding-TimeWidth, userSize), userFont);
					DrawString (message.Body, new RectangleF (xText, bounds.Y + TextYOffset, bounds.Width-PicAreaWidth-TextWidthPadding, bounds.Height-TextYOffset), textFont, UILineBreakMode.WordWrap);
					timeColor.SetColor ();

					string time = Utils.FormatTime (new TimeSpan (DateTime.UtcNow.Ticks - (message.ReceivedTime.HasValue ? message.ReceivedTime.Value.Ticks : 0)));
					DrawString (time, new RectangleF (xText, TextHeightPadding, bounds.Width-PicAreaWidth-TextWidthPadding, timeSize),
					            timeFont, UILineBreakMode.Clip, UITextAlignment.Right);

				}

				#region IImageUpdated implementation

				void IImageUpdated.UpdatedImage (Uri uri)
				{

				}

				#endregion
			}
		}
	}
}

