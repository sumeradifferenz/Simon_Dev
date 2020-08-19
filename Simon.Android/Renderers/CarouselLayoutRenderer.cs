using System;
using System.ComponentModel;
using System.Threading;
using System.Timers;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Simon.Controls;
using Simon.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Math = Java.Lang.Math;

[assembly: ExportRenderer(typeof(CarouselLayout), typeof(CarouselLayoutRenderer))]
namespace Simon.Droid.Renderers
{
    public class CarouselLayoutRenderer : ScrollViewRenderer
    {
        int _prevScrollX;
        int _deltaX;
        bool _motionDown;
        System.Timers.Timer _deltaXResetTimer;
        System.Timers.Timer _scrollStopTimer;
        HorizontalScrollView _scrollView;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            try
            {
                base.OnElementChanged(e);
                if (e.NewElement == null) return;

                _deltaXResetTimer = new System.Timers.Timer(100) { AutoReset = false };
                _deltaXResetTimer.Elapsed += (object sender, ElapsedEventArgs args) => _deltaX = 0;

                _scrollStopTimer = new System.Timers.Timer(200) { AutoReset = false };
                _scrollStopTimer.Elapsed += (object sender, ElapsedEventArgs args2) => UpdateSelectedIndex();

                e.NewElement.PropertyChanged += ElementPropertyChanged;
            }
            catch (System.Exception ex)
            {
                //Console.WriteLine(ex.Message + ex.Source);
            }
        }

        void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "Renderer")
                {
                    //_scrollView = (HorizontalScrollView)typeof(ScrollViewRenderer)
                    //	.GetField ("hScrollView", BindingFlags.NonPublic | BindingFlags.Instance)
                    //	.GetValue (this);

                    //_scrollView.HorizontalScrollBarEnabled = false;
                    //_scrollView.Touch += HScrollViewTouch;
                    var horizontal = this.ViewGroup.GetChildAt(0) as ViewGroup;
                    if (horizontal is HorizontalScrollView)
                    {
                        _scrollView = horizontal as HorizontalScrollView;
                        _scrollView.HorizontalScrollBarEnabled = false;
                        _scrollView.Touch += HScrollViewTouch;
                    }
                }
                /*if (e.PropertyName == "ScrollX" && !_motionDown) {
                    ScrollToIndex (((CarouselLayout)this.Element).SelectedIndex);
                }*/
            }
            catch (System.Exception ex)
            {
            }
        }

        void HScrollViewTouch(object sender, TouchEventArgs e)
        {
            try
            {
                e.Handled = false;

                switch (e.Event.Action)
                {
                    case MotionEventActions.Move:
                        _deltaXResetTimer.Stop();
                        _deltaX = _scrollView.ScrollX - _prevScrollX;
                        _prevScrollX = _scrollView.ScrollX;

                        UpdateSelectedIndex();

                        _deltaXResetTimer.Start();
                        break;
                    case MotionEventActions.Down:
                        _motionDown = true;
                        _scrollStopTimer.Stop();
                        break;
                    case MotionEventActions.Up:
                        _motionDown = false;
                        SnapScroll();
                        _scrollStopTimer.Start();
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        void UpdateSelectedIndex()
        {
            try
            {
                var center = _scrollView.ScrollX + (_scrollView.Width / 2);
                var carouselLayout = (CarouselLayout)this.Element;
                carouselLayout.SelectedIndex = (center / _scrollView.Width);
            }
            catch (System.Exception ex)
            {
            }
        }

        void SnapScroll()
        {
            try
            {
                var roughIndex = (float)_scrollView.ScrollX / _scrollView.Width;

                var targetIndex =
                    _deltaX < 0 ? System.Math.Floor(roughIndex)
                    : _deltaX > 0 ? Math.Ceil(roughIndex)
                    : Math.Round(roughIndex);

                ScrollToIndex((int)targetIndex);
            }
            catch (System.Exception ex)
            {
            }
        }

        void ScrollToIndex(int targetIndex)
        {
            try
            {
                ((CarouselLayout)this.Element).SelectedIndex = targetIndex;
                var targetX = targetIndex * _scrollView.Width;
                _scrollView.Post(new Runnable(() =>
                {
                    _scrollView.SmoothScrollTo(targetX, 0);
                }));
                if (((CarouselLayout)Element).OnSwipe != null)
                    ((CarouselLayout)Element).OnSwipe.Invoke(null, ((CarouselLayout)this.Element).SelectedIndex);
            }
            catch (System.Exception ex)
            {
            }
        }

        bool _initialized = false;

        public CarouselLayoutRenderer(Context context) : base(context)
        {
        }

        public override void Draw(Canvas canvas)
        {
            try
            {
                base.Draw(canvas);
                if (_initialized) return;
                _initialized = true;
                var carouselLayout = (CarouselLayout)this.Element;
                _scrollView.ScrollTo(carouselLayout.SelectedIndex * Width, 0);

            }
            catch (System.Exception ex)
            {
            }
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            try
            {
                if (_initialized && (w != oldw))
                {
                    _initialized = false;
                }
                base.OnSizeChanged(w, h, oldw, oldh);
            }
            catch (System.Exception ex)
            {
            }
        }
    }
}

