using System;
using System.ComponentModel;
using Simon.Controls;
using Simon.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CarouselLayout), typeof(CarouselLayoutRenderer))]
namespace Simon.iOS.Renderers
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
    {
        UIScrollView _native;

        public CarouselLayoutRenderer()
        {
            PagingEnabled = true;
            ShowsHorizontalScrollIndicator = false;
        }

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			try
			{
				base.OnElementChanged(e);

				if (e.OldElement != null) return;

				_native = (UIScrollView)NativeView;
				_native.Scrolled += NativeScrolled;
				e.NewElement.PropertyChanged += ElementPropertyChanged;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void NativeScrolled(object sender, EventArgs e)
		{
			try
			{
				var center = _native.ContentOffset.X + (_native.Bounds.Width / 2);
				((CarouselLayout)Element).SelectedIndex = ((int)center) / ((int)_native.Bounds.Width);
				if (((CarouselLayout)Element).OnSwipe != null)
					((CarouselLayout)Element).OnSwipe.Invoke(null, ((CarouselLayout)Element).SelectedIndex);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			try
			{
				if (!Dragging)
				{
					ScrollToSelection(false);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void ScrollToSelection(bool animate)
		{
			try
			{
				if (Element == null) return;

				_native.SetContentOffset(new CoreGraphics.CGPoint
					(_native.Bounds.Width *
						Math.Max(0, ((CarouselLayout)Element).SelectedIndex),
						_native.ContentOffset.Y),
					animate);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}

