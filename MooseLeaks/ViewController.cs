using System;
using System.Threading;
using CoreGraphics;
using UIKit;

namespace MooseLeaks
{
    public class ViewController : UIViewController
    {
        public int CurrentCounter;
        public event EventHandler<EventArgs> CounterChanged;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;

            NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("Moose", UIBarButtonItemStyle.Plain, ShowMoose), true);
            NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Count", UIBarButtonItemStyle.Plain, ShowCounterView), true);
        }

        void ShowMoose(object sender, EventArgs e)
        {
            Interlocked.Increment(ref CurrentCounter);
            if (CounterChanged != null)
                this.CounterChanged(this, new EventArgs());

            var imageView = new UIImageView(UIImage.FromBundle("moose"))
            {
                Frame = new CGRect(
                    10,
                    View.Bounds.GetMinY() + 64,
                    View.Bounds.Width - 20,
                    View.Bounds.GetMaxY() - 64 - View.Bounds.GetMidY()
                ),
                ContentMode = UIViewContentMode.ScaleAspectFit,
                ClipsToBounds = true
            };
            View.AddSubview(imageView);

            var removeTimer = new Timer(o => BeginInvokeOnMainThread(() =>
                {
                    imageView.RemoveFromSuperview();
                    imageView.Image?.Dispose();
                    imageView.Image = null;
                    imageView?.Dispose();
                    imageView = null;
                }),
                null, 1000, Timeout.Infinite);
        }

        void ShowCounterView(object sender, EventArgs e)
        {
            View.AddSubview(new MooseCountView(
                this,
                new CGRect(
                    0,
                    View.Bounds.GetMidY(),
                    View.Bounds.Width,
                    View.Bounds.Height / 2)
            ));
        }
    }

    public class MooseCountView : UIView
    {
        ViewController Owner;
        UILabel countLabel;
        UIButton removeButton;

        public MooseCountView(ViewController owner, CGRect frame)
            : base(frame)
        {
            Owner = owner;

            countLabel = new UILabel
            {
                Text = $"{Owner.CurrentCounter} time(s)",
                TextAlignment = UITextAlignment.Center,
            };
            AddSubview(countLabel);

            removeButton = new UIButton(UIButtonType.RoundedRect);
            removeButton.SetTitle("Close", UIControlState.Normal);
            AddSubview(removeButton);

            Owner.CounterChanged += Owner_CounterChanged;
            removeButton.TouchUpInside += RemoveButton_TouchUpInside;
        }

        public override void LayoutSubviews()
        {
            var bounds = Bounds;

            removeButton.Frame = new CGRect(
                bounds.GetMidX(),
                bounds.GetMinY() + 10,
                (bounds.Width / 2) - 10,
                44
            );

            countLabel.Frame = new CGRect(
                bounds.GetMinX() + 10,
                removeButton.Frame.GetMinY() + 10,
                bounds.Width - 20,
                bounds.Height - 20 - removeButton.Frame.GetMaxY()
            );
        }

        void Owner_CounterChanged(object sender, EventArgs e)
        {
            countLabel.Text = $"{Owner.CurrentCounter} time(s)";
        }

        void RemoveButton_TouchUpInside(object sender, EventArgs e)
        {
            RemoveFromSuperview();
            removeButton.TouchUpInside -= Owner_CounterChanged;

            this.Dispose();
            GC.Collect();
        }
    }
}