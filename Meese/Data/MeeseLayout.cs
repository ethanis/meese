using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Meese
{
    public class MeeseLayout : UICollectionViewFlowLayout
    {
        int numberOfColumns = 2;

        private List<UICollectionViewLayoutAttributes> cache = new List<UICollectionViewLayoutAttributes>();

        private nfloat contentHeight = 0.0f;
        private nfloat contentWidth
        {
            get
            {
                var insets = CollectionView.ContentInset;
                return CollectionView.Bounds.Width - (insets.Left + insets.Right);
            }
        }

        int column = 0;
        List<nfloat> xOffset = new List<nfloat>();
        List<nfloat> yOffset = new List<nfloat>();

        public override void PrepareLayout()
        {
            //Only calculate once for each cell
            if (MeeseCollectionDataSource.Instance.Owner.IsDoneUpdating && cache.Count < MooseManager.Instance.Images.Count)
            {
                //Calculates the X Offset for every column and increments the current max Y Offset for each column
                var columnWidth = contentWidth / numberOfColumns;
                if (xOffset.Count == 0)
                {
                    for (int i = 0; i < numberOfColumns; i++)
                    {
                        xOffset.Add(i * columnWidth);
                    }
                }

                column = 0;
                if (yOffset.Count == 0)
                {
                    for (int i = 0; i < numberOfColumns; i++)
                    {
                        yOffset.Add(0);
                    }
                }

                //Iterates through the list of items
                var currentCount = cache.Count;
                for (int item = currentCount; item < CollectionView.NumberOfItemsInSection(0); item++)
                {
                    var indexPath = NSIndexPath.FromItemSection(item, 0);

                    //Asks the datasource for the height of the cell contents and calculates the cell frame.
                    var cellContentHeight = MeeseCollectionDataSource.Instance.HeightForTileAtIndexPath(indexPath);
                    var height = Constants.CellContentPadding + cellContentHeight + Constants.CellContentPadding;

                    var xPos = xOffset[column];
                    var yPos = yOffset[column];

                    var frame = new CGRect(x: xPos, y: yPos, width: columnWidth, height: height);
                    var insetFrame = frame.Inset(Constants.CellContentPadding, Constants.CellContentPadding);

                    //Creates an UICollectionViewLayoutItem with the frame and add it to the cache
                    var attributes = LayoutAttributesForItem(indexPath);
                    attributes.Frame = insetFrame;
                    cache.Add(attributes);

                    //Updates the collection view content height
                    contentHeight = (nfloat)Math.Max(contentHeight, frame.GetMaxY());
                    yOffset[column] = yOffset[column] + height;

                    column = column >= (numberOfColumns - 1) ? 0 : ++column;
                }
            }
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            UICollectionViewLayoutAttributes attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
            return attributes;
        }

        public override CGSize CollectionViewContentSize
        {
            get
            {
                return new CGSize(contentWidth, contentHeight);
            }
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var layoutAttributes = new List<UICollectionViewLayoutAttributes>();

            //Loop through the cache and look for items in the rect
            foreach (var attributes in cache)
            {
                if (attributes.Frame.IntersectsWith(rect))
                {
                    layoutAttributes.Add(attributes);
                }
            }

            return layoutAttributes.ToArray();
        }


        public void ClearCache()
        {
            cache = new List<UICollectionViewLayoutAttributes>();
            contentHeight = 0.0f;
            xOffset = new List<nfloat>();
            yOffset = new List<nfloat>();
        }
    }
}
