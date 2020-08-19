using System;
using System.ComponentModel;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Simon.Controls;
using Simon.Droid.Renderers;
using Simon.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Simon.Droid.Renderers
{
    [Obsolete]
    public class CustomEntryRenderer : EntryRenderer
    {
        private const int MinDistance = 10;

        private float downX, downY, upX, upY;

        private Drawable originalBackground;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = (CustomEntry)Element;
            if (view != null && Control != null)
            {
                //if (Control != null && e.NewElement != null && e.NewElement.IsPassword)
                //{
                //    Control.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
                //    Control.TransformationMethod = new PasswordTransformationMethod();
                //}



                // Editor Action is called when the return button is pressed
                //Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                //{
                //    if (view?.ReturnType != KeyboardReturnType.Next)
                //        view?.Unfocus();
                //    // Call all the methods attached to base_entry event handler Completed
                //    view?.OnNext();
                //};

                SetFont(view);
                SetTextAlignment(view);
                //SetBorder(view);
                SetPlaceholderTextColor(view);
                SetMaxLength(view);

                //Control.SetPadding(30, 30, Control.PaddingRight, 0);
                Control.SetPadding(0, 0, 10, 0);

                //if (e.NewElement == null)
                //{
                //    this.Touch -= HandleTouch;
                //}

                //if (e.OldElement == null)
                //{
                //    this.Touch += HandleTouch;
                //}

                if (!view.IsEnabled)
                {
                    Control.SetTextColor(view.TextColor.ToAndroid());
                }
                Control.SetBackgroundColor(global::Android.Graphics.Color.Argb(0, 0, 0, 0));


                view.Unfocused += (object sender, FocusEventArgs ev) =>
                {
                    view.Text = (!view.IsPassword) ? view.Text?.Trim() : view.Text;
                };

                //Control.SetRawInputType(InputTypes.TextFlagNoSuggestions | InputTypes.TextFlagCapSentences);


                /////////
                /*
               if (view.ReturnType == KeyboardReturnType.Next)
               {
                   Control.ImeOptions = ImeAction.Next;
                   Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                   {
                       view.OnNext();
                   };
               }
               else if (view.ReturnType == KeyboardReturnType.Done)
               {
                 Control.ImeOptions = ImeAction.Done;
                 Control.SetImeActionLabel("Done", ImeAction.Done);
                 view.Unfocus();
               }
               */
                ////////
                SetReturnType(view);

                //IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                //IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
                //JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.CustomCursor);

            }
        }

      
        void SetReturnType(CustomEntry entry)
        {
            if (entry != null && Control != null)
            {
                var type = entry.ReturnType;

                switch (type)
                {
                    case KeyboardReturnType.Go:
                        Control.ImeOptions = ImeAction.Go;
                        Control.SetImeActionLabel("Go", ImeAction.Go);
                        break;
                    case KeyboardReturnType.Next:
                        Control.ImeOptions = ImeAction.Next;
                        Control.SetImeActionLabel("Next", ImeAction.Next);
                        break;
                    case KeyboardReturnType.Send:
                        Control.ImeOptions = ImeAction.Send;
                        Control.SetImeActionLabel("Send", ImeAction.Send);
                        break;
                    case KeyboardReturnType.Search:
                        Control.ImeOptions = ImeAction.Search;
                        Control.SetImeActionLabel("Search", ImeAction.Search);
                        break;
                    default:
                        Control.ImeOptions = ImeAction.Done;
                        Control.SetImeActionLabel("Done", ImeAction.Done);
                        break;
                }

                if (entry.ReturnType == KeyboardReturnType.Next)
                {
                    //Control.ImeOptions = ImeAction.Next;
                    Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                    {
                        entry.OnNext();
                    };
                }

                if (entry.ReturnType == KeyboardReturnType.Done)
                {
                    //Control.ImeOptions = ImeAction.Done;
                    //Control.SetImeActionLabel("Done", ImeAction.Done);

                    Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                    {
                        entry.Unfocus();
                        if (entry.ReturnKeyClickCommand != null)
                        {
                            entry.ReturnKeyClickCommand.Execute(null);
                        }
                    };
                }

                if (entry.ReturnType == KeyboardReturnType.Search)
                {
                    //Control.ImeOptions = ImeAction.Send;
                    //Control.SetImeActionLabel("Search", ImeAction.Search);

                    Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                    {
                        entry.Unfocus();
                        if (entry.ReturnKeyClickCommand != null)
                        {
                            entry.ReturnKeyClickCommand.Execute(null);
                        }
                    };
                }
            }
        }

        /// <summary>
        /// Handles the touch.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Android.Views.View.TouchEventArgs"/> instance containing the event data.</param>
        void HandleTouch(object sender, TouchEventArgs e)
        {
            var element = (CustomEntry)this.Element;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    this.downX = e.Event.GetX();
                    this.downY = e.Event.GetY();
                    return;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                case MotionEventActions.Move:
                    this.upX = e.Event.GetX();
                    this.upY = e.Event.GetY();

                    float deltaX = this.downX - this.upX;
                    float deltaY = this.downY - this.upY;

                    // swipe horizontal?
                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                    {
                        if (Math.Abs(deltaX) > MinDistance)
                        {
                            //if (deltaX < 0)
                            //{
                            //  element.OnRightSwipe(this, EventArgs.Empty);
                            //  return;
                            //}

                            //if (deltaX > 0)
                            //{
                            //  element.OnLeftSwipe(this, EventArgs.Empty);
                            //  return;
                            //}
                        }
                        else
                        {
                            Log.Info("ExtendedEntry", "Horizontal Swipe was only " + Math.Abs(deltaX) + " long, need at least " + MinDistance);
                            return; // We don't consume the event
                        }
                    }
                    // swipe vertical?
                    //                    else 
                    //                    {
                    //                        if(Math.abs(deltaY) > MIN_DISTANCE){
                    //                            // top or down
                    //                            if(deltaY < 0) { this.onDownSwipe(); return true; }
                    //                            if(deltaY > 0) { this.onUpSwipe(); return true; }
                    //                        }
                    //                        else {
                    //                            Log.i(logTag, "Vertical Swipe was only " + Math.abs(deltaX) + " long, need at least " + MIN_DISTANCE);
                    //                            return false; // We don't consume the event
                    //                        }
                    //                    }

                    return;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var view = (CustomEntry)Element;

            if (e.PropertyName == CustomEntry.ReturnTypeProperty.PropertyName)
            {
                SetReturnType(view);
            }
            if (e.PropertyName == CustomEntry.FontProperty.PropertyName)
            {
                SetFont(view);
            }
            else if (e.PropertyName == CustomEntry.XAlignProperty.PropertyName)
            {
                SetTextAlignment(view);
            }
            else if (e.PropertyName == CustomEntry.HasBorderProperty.PropertyName)
            {
                SetBorder(view);

            }
            else if (e.PropertyName == CustomEntry.PlaceholderTextColorProperty.PropertyName)
            {
                SetPlaceholderTextColor(view);
            }
            else if (e.PropertyName == CustomEntry.HorizontalTextAlignmentProperty.PropertyName)
            {
                SetTextAlignment(view);
            }
            else if (e.PropertyName == CustomEntry.IsPasswordProperty.PropertyName)
            {
                if (view.IsPassword)
                {
                    Control.TransformationMethod = PasswordTransformationMethod.Instance;
                }
                else
                {
                    Control.TransformationMethod = null;
                }
            }
            else
            {
                base.OnElementPropertyChanged(sender, e);
                if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                {
                    this.Control.SetBackgroundColor(view.BackgroundColor.ToAndroid());
                }
            }
        }

        ///// <summary>
        ///// Sets the border.
        ///// </summary>
        ///// <param name="view">The view.</param>
        private void SetBorder(CustomEntry view)
        {
            if (view.HasBorder == false)
            {
                var shape = new ShapeDrawable(new RectShape());
                shape.Paint.Alpha = 0;
                shape.Paint.SetStyle(Paint.Style.Stroke);
                Control.SetBackgroundDrawable(shape);
            }
            else
            {
                Control.SetBackground(originalBackground);
            }
        }

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetTextAlignment(CustomEntry view)
        {
            //switch (view.XAlign)
            //{
            //    case Xamarin.Forms.TextAlignment.Center:
            //        Control.Gravity = GravityFlags.CenterHorizontal;
            //        break;
            //    case Xamarin.Forms.TextAlignment.End:
            //        Control.Gravity = GravityFlags.End;
            //        break;
            //    case Xamarin.Forms.TextAlignment.Start:
            //        Control.Gravity = GravityFlags.Start;
            //        break;
            //}

            switch (view.HorizontalTextAlignment)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = GravityFlags.Center;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = GravityFlags.End | GravityFlags.CenterVertical;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = GravityFlags.Start | GravityFlags.CenterVertical;
                    break;
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetFont(CustomEntry view)
        {
            if (view.Font != Font.Default)
            {
                Control.TextSize = view.Font.ToScaledPixel();
                //Control.Typeface = view.Font.ToExtendedTypeface(Context);
            }
        }

        /// <summary>
        /// Sets the color of the placeholder text.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetPlaceholderTextColor(CustomEntry view)
        {
            if (view.PlaceholderTextColor != Xamarin.Forms.Color.Default)
            {
                Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());
            }
        }

        /// <summary>
        /// Sets the MaxLength characteres.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetMaxLength(CustomEntry view)
        {
            Control.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(view.MaxLength) });
        }


    }
    public static class EnumExtensions
    {
        public static ImeAction GetValueFromDescription(this KeyboardReturnType value)
        {
            try
            {
                var type = typeof(ImeAction);
                if (!type.IsEnum) throw new InvalidOperationException();
                foreach (var field in type.GetFields())
                {
                    var attribute = Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute != null)
                    {
                        if (attribute.Description == value.ToString())
                            return (ImeAction)field.GetValue(null);
                    }
                    else
                    {
                        if (field.Name == value.ToString())
                            return (ImeAction)field.GetValue(null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            throw new NotSupportedException($"Not supported on Android: {value}");
        }
    }
}

