using System;
using CoreGraphics;
using UIKit;

namespace Meese
{
    public class HomeViewController : UIViewController
    {
        UIButton mooseButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;

            mooseButton = new UIButton();
            mooseButton.SetTitle("Show Meese", UIControlState.Normal);
            mooseButton.SetTitleColor(UIColor.Brown, UIControlState.Normal);
            mooseButton.Frame = new CGRect(
                20,
                (View.Bounds.Height / 2) - 64,
                View.Bounds.Width - 40,
                44
            );
            View.AddSubview(mooseButton);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            mooseButton.TouchUpInside += MooseButton_TouchUpInside;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            mooseButton.TouchUpInside -= MooseButton_TouchUpInside;
        }

        void MooseButton_TouchUpInside(object sender, EventArgs e)
        {
            NavigationController.PushViewController(new MeeseViewController(), true);
        }
    }
}

