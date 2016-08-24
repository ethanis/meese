using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Splat;
using UIKit;


namespace Meese
{
    public class MeeseCell : UICollectionViewCell
    {
        internal const string Key = "MeeseCell";
        readonly UIImageView itemImage;

        [Export("initWithFrame:")]
        public MeeseCell(CGRect frame) : base(frame)
        {
            ContentView.Layer.BorderWidth = 2f;
            ContentView.Layer.BorderColor = UIColor.Gray.CGColor;
            ContentView.Layer.CornerRadius = 5f;
            ContentView.ClipsToBounds = true;
            ContentView.BackgroundColor = UIColor.White;

            itemImage = new UIImageView();
            ContentView.AddSubview(itemImage);
        }

        public void Configure(IBitmap image, CGSize size)
        {
            itemImage.Image = image.ToNative();
            itemImage.Frame = new CGRect(new CGPoint(0, 0),
                                         size);
        }
    }
}