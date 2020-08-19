using System;
using System.Diagnostics;
using Simon.Controls;
using Simon.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer))]
namespace Simon.iOS.Renderers
{
    public class BindablePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            try
            {
                base.OnElementChanged(e);
                if(e!=null)
                {
                    var view = e.NewElement as Picker;
                    this.Control.BorderStyle = UITextBorderStyle.None;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
        }
    }
}

