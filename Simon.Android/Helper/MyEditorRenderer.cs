using Android.Content;
using Simon.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(MyEditorRenderer))]

namespace Simon.Droid
{
        public class MyEditorRenderer : EditorRenderer
        {
            public MyEditorRenderer(Context context) : base(context)
            {
            }

            protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
            {
                base.OnElementChanged(e);

                if (Control != null)
                {
                    Control.FocusChange += Control_FocusChange;
                }
            }

            private void Control_FocusChange(object sender, FocusChangeEventArgs e)
            {
                if (!e.HasFocus)
                {
                    Control.SetSelection(0);
                }
            }
        }
    }