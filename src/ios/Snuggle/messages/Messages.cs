using System;
using System.Drawing;
using System.Collections;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using MonoTouch.Dialog;
using MonoTouch.Dialog.Utilities;

using Snuggle.Common;

namespace Snuggle
{
	public class Messages : DialogViewController
	{
		public Messages () : base (UITableViewStyle.Plain, null)
		{
			Autorotate = true;
			EnableSearch = true;
			AutoHideSearch = true;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			var root = new RootElement ("Messages") { UnevenRows = true };
			var section = new Section ();
			root.Add (section);
			Root = root;

			if (XmppSession.RunningSession == null) {
				var session = new Common.XmppSession (Common.XmppProfile.Current);
				session.Start ();
			} else {
				Common.XmppService.OnEvent -= HandleOnEvent;
			}
			Common.XmppService.OnEvent += HandleOnEvent;

		}

		void HandleOnEvent (Snuggle.Common.ISession session, Message message)
		{
			BeginInvokeOnMainThread (delegate {
				Root[0].Add (new MessageElement (message));
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

					string time = Utils.FormatTime (new TimeSpan (DateTime.UtcNow.Ticks - message.ReceivedTime.Ticks));
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

