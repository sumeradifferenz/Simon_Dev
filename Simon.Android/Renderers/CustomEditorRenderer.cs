using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Simon.Controls;
using Simon.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EditorWithAutoSize), typeof(CustomEditorRenderer))]
namespace Simon.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        bool initial = true;
        bool isFirstTime = true;

        public CustomEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            try
            {
                var view = (EditorWithAutoSize)Element;
                if (view != null && Control != null)
                {
                    Control.SetPadding(10, 10, 10, 10);
                    Control.SetBackgroundColor(view.BackgroundColor.ToAndroid());
                    Control.Gravity = GravityFlags.Start;
                    //IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                    //IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
                    Control.SetMaxLines(5);

                    var nativeEditText = (global::Android.Widget.EditText)Control;

                    //While scrolling inside Editor stop scrolling parent view.
                    nativeEditText.OverScrollMode = OverScrollMode.Always;
                    nativeEditText.ScrollBarStyle = ScrollbarStyles.InsideInset;
                    nativeEditText.SetOnTouchListener(new DroidTouchListener());

                    //For Scrolling in Editor innner area
                    Control.VerticalScrollBarEnabled = true;
                    Control.ScrollBarStyle = Android.Views.ScrollbarStyles.InsideInset;

                    Control.FocusChange += (sender, eh) =>
                    {
                        if (eh.HasFocus)
                        {
                            if (isFirstTime)
                            {
                                isFirstTime = false;
                                return;
                            }
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                isFirstTime = true;
                                var imm = (InputMethodManager)Control.Context.GetSystemService(Context.InputMethodService);
                                imm.HideSoftInputFromWindow(Control.WindowToken, HideSoftInputFlags.None);
                            });
                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                isFirstTime = true;
                                var imm = (InputMethodManager)Control.Context.GetSystemService(Context.InputMethodService);
                                imm.HideSoftInputFromWindow(Control.WindowToken, HideSoftInputFlags.None);
                            });
                        }
                    };
                    //JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);
                }

                if (e.NewElement != null)
                {
                    var customControl = (EditorWithAutoSize)Element;
                    if (!string.IsNullOrEmpty(customControl.Placeholder))
                    {
                        Control.Hint = customControl.Placeholder;
                    }
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception);
            }
        }

        public class DroidTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
        {
            public bool OnTouch(Android.Views.View v, MotionEvent e)
            {
                v.Parent?.RequestDisallowInterceptTouchEvent(true);
                if ((e.Action & MotionEventActions.Up) != 0 && (e.ActionMasked & MotionEventActions.Up) != 0)
                {
                    v.Parent?.RequestDisallowInterceptTouchEvent(false);
                }
                return false;
            }
        }
    }
}

