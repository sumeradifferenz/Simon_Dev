using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;


[assembly: ExportRenderer(typeof(SearchBar), typeof(Namespace.iOS.Renderers.ExtendedSearchBarRenderer))]

namespace Namespace.iOS.Renderers
{
    public class ExtendedSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Text")
            {
                Control.ShowsCancelButton = false;
            }
        }
    }
}

