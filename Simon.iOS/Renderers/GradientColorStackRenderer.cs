using System;
using CoreAnimation;
using CoreGraphics;
using Simon.Controls;
using Simon.iOS.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace Simon.iOS.Renderer
{
    public class GradientColorStackRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null) return;
            if (e.NewElement is GradientColorStack page)
            {
                var gradientLayer = new CAGradientLayer
                {
                    Frame = View.Bounds,
                    StartPoint = new CGPoint(1, 0),
                    EndPoint = new CGPoint(1, 0.5),
                    Colors = new CGColor[] { page.StartColor.ToCGColor(), page.EndColor.ToCGColor() }
                };
                View.Layer.InsertSublayer(gradientLayer, 0);
            }
        }
    }
}
