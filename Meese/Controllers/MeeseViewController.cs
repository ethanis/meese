using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akavache;
using CoreGraphics;
using Foundation;
using UIKit;
using System.Reactive.Linq;
using System;

namespace Meese
{
    public class MeeseViewController : UIViewController
    {
        UICollectionView collectionView;
        private int previousItemCount = 0;
        readonly Random random = new Random();

        public bool IsDoneUpdating { get; private set; }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;
            var layout = new MeeseLayout();
            collectionView = new UICollectionView(
                View.Bounds,
                layout
            );
            collectionView.BackgroundColor = UIColor.White;
            collectionView.Source = new MeeseCollectionDataSource(this);
            collectionView.RegisterClassForCell(typeof(MeeseCell), MeeseCell.Key);
            View.AddSubview(collectionView);

            await AppendValues();
        }

        public async Task AppendValues()
        {
            IsDoneUpdating = false;

            var newItems = new int[20];

            MooseManager.Instance.Images.AddRange((await Task.WhenAll(newItems.Select(async x =>
            {
                MooseManager.Instance.CellSizes.Add(new CGSize());

                var imagePath = MooseManager.Instance.MeeseUrls.ElementAt(random.Next(0, MooseManager.Instance.MeeseUrls.Count));

                return await BlobCache.LocalMachine.LoadImageFromUrl(imagePath, false);
            }
            ))).ToList());

            //Only reloading rows for new data
            List<NSIndexPath> rows = new List<NSIndexPath>();
            for (int i = previousItemCount; i < MooseManager.Instance.Images.Count; i++)
            {
                var indexPath = NSIndexPath.FromItemSection(i, 0);
                rows.Add(indexPath);
            }

            if (previousItemCount > 0)
            {
                BeginInvokeOnMainThread(() =>
                {
                    var feedLayout = collectionView.CollectionViewLayout as MeeseLayout;
                    //Grabs layoutattributes for new cells
                    feedLayout.InvalidateLayout();
                    View.SetNeedsLayout();

                    UIView.AnimationsEnabled = false;
                    collectionView.PerformBatchUpdates(() =>
                    {
                        collectionView.InsertItems(rows.ToArray());
                    }, null);
                    UIView.AnimationsEnabled = true;
                });
            }
            else
            {
                BeginInvokeOnMainThread(() =>
                {
                    //Prevents default collection view animation
                    UIView.AnimationsEnabled = false;
                    collectionView.ReloadData();
                    UIView.AnimationsEnabled = true;
                });
            }

            previousItemCount = MooseManager.Instance.Images.Count;

            IsDoneUpdating = true;
        }
    }

}