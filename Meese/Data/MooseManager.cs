using System;
using System.Collections.Generic;
using CoreGraphics;
using Splat;

namespace Meese
{
    public class MooseManager
    {
        private static MooseManager instance;
        public static MooseManager Instance => instance ?? (instance = new MooseManager());
        private MooseManager()
        {
            MeeseUrls.Add(@"http://www.cbc.ca/natureofthings/content/images/episodes/moose_infographic.jpg");
            MeeseUrls.Add(@"http://yourshot.nationalgeographic.com/u/ss/fQYSUbVfts-T7pS2VP2wnKyN8wxywmXtY0-Fwsgxo7Tk4K9J9hQVvmN9FFdi0VnJn-0g92FXmVsw9nVCNVCr/");
            MeeseUrls.Add(@"https://upload.wikimedia.org/wikipedia/commons/0/09/Moose_animal_pair_bull_and_cow_moose.jpg");
            MeeseUrls.Add(@"https://pbs.twimg.com/profile_images/387131788/Moose_400x400.jpg");
            MeeseUrls.Add(@"http://www.saveminnesotamoose.org/img/hp-main-image.jpg");
            MeeseUrls.Add(@"http://dehayf5mhw1h7.cloudfront.net/wp-content/uploads/sites/486/2016/04/22021122/Moose-blurb-jpg.jpg");
            MeeseUrls.Add(@"http://www.bullmooseindustries.com/sites/default/files/moose-home-slide.png");
            MeeseUrls.Add(@"https://richwolf.files.wordpress.com/2011/08/moose-3-8-5x111.jpg");
            MeeseUrls.Add(@"http://a-z-animals.com/media/animals/images/470x370/moose1.jpg");
        }

        public List<CGSize> CellSizes { get; set; } = new List<CGSize>();
        public List<IBitmap> Images { get; set; } = new List<IBitmap>();
        public List<string> MeeseUrls { get; private set; } = new List<string>();
    }
}
