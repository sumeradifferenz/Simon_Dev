using System;
using System.Linq;
using Simon.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CarouselPage), typeof(CustomCarouselPageRenderer))]
namespace Simon.iOS.Renderers
{
    public class CustomCarouselPageRenderer : CarouselPageRenderer
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //Prevent vertical carousel scroll on iOS 11, 
            //we only want horizontal scroll in the carousel
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                View.Subviews.OfType<UIScrollView>().Single().ContentInsetAdjustmentBehavior =
                UIScrollViewContentInsetAdjustmentBehavior.Never;
            }
        }
    }
}

