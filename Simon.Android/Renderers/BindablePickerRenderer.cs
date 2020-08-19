using System;
using Android.Graphics.Drawables;
using Simon.Controls;
using Simon.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using PickerRenderer = Xamarin.Forms.Platform.Android.PickerRenderer;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer))]
namespace Simon.Droid.Renderers
{
    [Obsolete]
    public class BindablePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetStroke(0, Android.Graphics.Color.Transparent);
                    Control.SetBackground(gd);
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}

