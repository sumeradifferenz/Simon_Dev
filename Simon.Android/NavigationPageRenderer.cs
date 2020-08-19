using System;
using Android.App;
using Simon.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer (typeof(NavigationPage), typeof(Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer))]
namespace Simon.Droid
{
   
    //public class NavigationPageRenderer : NavigationRenderer(Context context)
        //{
        //    protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        //    {
        //        base.OnElementChanged(e);

        //        var actionBar = ((Activity)Context).ActionBar;
        //        actionBar.SetIcon(Resource.Drawable.mr_media_pause_dark);
        //    }
        //}
    }