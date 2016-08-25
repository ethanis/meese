using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Meese
{
    public class MeeseCollectionDataSource : UICollectionViewSource
    {
        const float Padding = 5.0f;
        public static MeeseCollectionDataSource Instance { get; protected set; }
        private WeakReference<MeeseViewController> OwnerWR;
        public MeeseViewController Owner
        {
            get
            {
                MeeseViewController tartget = null;
                OwnerWR.TryGetTarget(out tartget);
                return tartget;
            }
        }

        public MeeseCollectionDataSource(MeeseViewController owner)
        {
            Instance = this;
            OwnerWR = new WeakReference<MeeseViewController>(owner);
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var mooseCell = collectionView.DequeueReusableCell(MeeseCell.Key, indexPath) as MeeseCell;

            ////Used for infinite scroll
            if (indexPath.Item >= MooseManager.Instance.Images.Count - 1)
            {
                Task.Run(async () =>
                {
                    await Owner.AppendValues();
                });
            }

            mooseCell.Configure(
                MooseManager.Instance.Images.ElementAt((int)indexPath.Item),
                MooseManager.Instance.CellSizes.ElementAt((int)indexPath.Item)
            );

            return mooseCell;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return MooseManager.Instance.Images.Count();
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            System.Diagnostics.Debug.WriteLine($"Index {indexPath.Item}; Image: {MooseManager.Instance.Images.ElementAt((int)indexPath.Item)}");
        }

        public nfloat HeightForTileAtIndexPath(NSIndexPath indexPath)
        {
            //If large tile, calculate full height of contents
            var image = MooseManager.Instance.Images.ElementAtOrDefault(indexPath.Row);
            var scaleFactor = image.Width / (Owner.View.Bounds.Width / 2);
            var imageHeight = (float)(image.Height / scaleFactor);
            var totalHeight = imageHeight + (2 * Constants.CellContentPadding);

            MooseManager.Instance.CellSizes[(int)indexPath.Item] = new CGSize((Owner.View.Bounds.Width / 2) - (2 * Constants.CellContentPadding), totalHeight);

            return totalHeight;
        }
    }
}
