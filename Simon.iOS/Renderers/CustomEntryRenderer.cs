using System;
using System.ComponentModel;
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using Simon.Controls;
using Simon.Helpers;
using Simon.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Simon.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {/// <summary>
     /// The on element changed callback.
     /// </summary>
     /// <param name="e">The event arguments.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = e.NewElement as CustomEntry;

            if (view != null)
            {
                SetFont(view);
                SetTextAlignment(view);
                SetBorder(view);
                SetPlaceholderTextColor(view);
                SetMaxLength(view);

                ResizeHeight();
                view.Unfocused += (object sender, FocusEventArgs ev) =>
                {
                    Control.ResignFirstResponder();
                    view.Text = (!view.IsPassword) ? view.Text?.Trim() : view.Text;
                };

                view.Focused += (object sender, FocusEventArgs ev) =>
                {
                    Control.BecomeFirstResponder();
                };


                try
                {

                    UIView paddingView = new UIView(new CoreGraphics.CGRect(0, 0, 0, 20));
                    Control.LeftView = paddingView;
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.SpellCheckingType = UITextSpellCheckingType.No;             // No Spellchecking
                    Control.AutocorrectionType = UITextAutocorrectionType.No;
                    Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
                    if (Control.KeyboardType == UIKeyboardType.EmailAddress)
                    {
                        Control.AutocapitalizationType = UITextAutocapitalizationType.None;
                    }
                    SetReturnType(view);

                    //Control.ShouldReturn += (UITextField tf) =>
                    //{
                    //    view?.OnNext();
                    //    return true;
                    //};

                    if (!view.IsEnabled)
                    {
                        Control.TextColor = view.TextColor.ToUIColor();
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }


        void SetReturnType(CustomEntry entry)
        {
            if (Control != null)
            {
                var type = entry.ReturnType;

                switch (type)
                {
                    case KeyboardReturnType.Go:
                        Control.ReturnKeyType = UIReturnKeyType.Go;
                        break;
                    case KeyboardReturnType.Next:
                        Control.ReturnKeyType = UIReturnKeyType.Next;
                        break;
                    case KeyboardReturnType.Send:
                        Control.ReturnKeyType = UIReturnKeyType.Send;
                        break;
                    case KeyboardReturnType.Search:
                        Control.ReturnKeyType = UIReturnKeyType.Search;
                        break;
                    case KeyboardReturnType.Done:
                        Control.ReturnKeyType = UIReturnKeyType.Done;
                        break;
                    default:
                        Control.ReturnKeyType = UIReturnKeyType.Default;
                        break;
                }

                Control.ReturnKeyType = entry.ReturnType.GetValueFromDescription();

                if (Control.ReturnKeyType == UIReturnKeyType.Done || Control.ReturnKeyType == UIReturnKeyType.Send || Control.ReturnKeyType == UIReturnKeyType.Search)
                {
                    //Control.ReturnKeyType = UIKit.UIReturnKeyType.Done;
                    Control.ShouldReturn += (CustomEntry) =>
                    {
                        if (entry.ReturnKeyClickCommand != null)
                        {
                            entry.ReturnKeyClickCommand.Execute(null);
                        }
                        return false;
                    };
                }

                if (entry.Keyboard == Keyboard.Numeric || entry.Keyboard == Keyboard.Telephone)
                {
                    UIButton DoneButton = new UIButton();
                    DoneButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
                    DoneButton.Frame = new CGRect(0.0f, 0.0f, 50.0f, 40.0f);


                    if (Control.ReturnKeyType == UIReturnKeyType.Next)
                    {
                        DoneButton.SetTitle("Next", UIControlState.Normal);
                        //DoneButton.TouchUpInside += (s, e) => { entry?.OnNext(); };
                        DoneButton.TouchUpInside += NextClick;
                    }
                    else
                    {
                        DoneButton.SetTitle("Done", UIControlState.Normal);
                        //DoneButton.TouchUpInside += (s, e) => { Control.ResignFirstResponder(); };
                        DoneButton.TouchUpInside += DoneClick;
                    }

                    var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f));
                    toolbar.BarStyle = UIBarStyle.Default;
                    toolbar.TintColor = UIColor.Black;
                    toolbar.BackgroundColor = UIKit.UIColor.Clear;
                    toolbar.Items = new[] {
                            new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                            new UIBarButtonItem(DoneButton)
                        };
                    Control.InputAccessoryView = toolbar;
                }
            }
        }

        public void NextClick(object sender, EventArgs e)
        {
            Control.ResignFirstResponder();
            var view = (CustomEntry)Element;
            view.OnNext();
        }

        public void DoneClick(object sender, EventArgs e)
        {
            Control.ResignFirstResponder();
        }

        /// <summary>
        /// The on element property changed callback
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (CustomEntry)Element;

            if (e.PropertyName == CustomEntry.FontProperty.PropertyName)
                SetFont(view);
            if (e.PropertyName == CustomEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
            if (e.PropertyName == CustomEntry.HasBorderProperty.PropertyName)
                SetBorder(view);
            if (e.PropertyName == CustomEntry.PlaceholderTextColorProperty.PropertyName)
                SetPlaceholderTextColor(view);
            if (e.PropertyName == CustomEntry.KeyboardProperty.PropertyName)
            {
                if (true)
                {

                }
            }

            if (Control != null)
            {
                if (Control.ReturnKeyType == UIReturnKeyType.Next)
                {
                    Control.ReturnKeyType = UIKit.UIReturnKeyType.Next;

                    if (e.PropertyName == "Renderer")
                        Control.ShouldReturn += (CAEntry) =>
                        {
                            view.OnNext();
                            return false;
                        };
                }
                ResizeHeight();
            }

        }

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetTextAlignment(CustomEntry view)
        {
            switch (view.XAlign)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetFont(CustomEntry view)
        {
            //UIFont uiFont;
            //if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
            //    Control.Font = uiFont;
            //else if (view.Font == Font.Default)
            //{
            //    Control.Font = UIFont.SystemFontOfSize(17f);
            //}
            //nfloat fontsize = new nfloat(view.FontSize);
            //Control.Font = UIFont.SystemFontOfSize(fontsize);

            var fontsize = view.FontSize;

            var font = UIKit.UIFont.FromName("LatoRegular", (System.nfloat)fontsize);
            if (font != null)
            {
                Control.Font = font;
            }
        }

        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetBorder(CustomEntry view)
        {
            //Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
            if (view.HasBorder)
            {
                Control.BorderStyle = UITextBorderStyle.RoundedRect;
            }
            else
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.BorderWidth = 0;
            }
        }

        /// <summary>
        /// Sets the maxLength.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetMaxLength(CustomEntry view)
        {
            Control.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= view.MaxLength;
            };
        }

        /// <summary>
        /// Resizes the height.
        /// </summary>
        private void ResizeHeight()
        {
            if (Element.HeightRequest >= 0) return;

            var height = Math.Max(Bounds.Height,
                new UITextField { Font = Control.Font }.IntrinsicContentSize.Height);

            Control.Frame = new CGRect(0.0f, 0.0f, (nfloat)Element.Width, (nfloat)height);

            Element.HeightRequest = height;
        }

        /// <summary>
        /// Sets the color of the placeholder text.
        /// </summary>
        /// <param name="view">The view.</param>
        void SetPlaceholderTextColor(CustomEntry view)
        {
            /*
            UIColor *color = [UIColor lightTextColor];
            YOURTEXTFIELD.attributedPlaceholder = [[NSAttributedString alloc] initWithString:@"PlaceHolder Text" attributes:@{NSForegroundColorAttributeName: color}];
            */
            if (string.IsNullOrEmpty(view.Placeholder) == false && view.PlaceholderTextColor != Color.Default)
            {
                NSAttributedString placeholderString = new NSAttributedString(view.Placeholder, new UIStringAttributes() { ForegroundColor = view.PlaceholderTextColor.ToUIColor() });
                Control.AttributedPlaceholder = placeholderString;
            }
        }
    }
    public static class EnumExtensions
    {
        public static UIReturnKeyType GetValueFromDescription(this KeyboardReturnType value)
        {
            var type = typeof(UIReturnKeyType);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == value.ToString())
                        return (UIReturnKeyType)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value.ToString())
                        return (UIReturnKeyType)field.GetValue(null);
                }
            }
            throw new NotSupportedException($"Not supported on iOS: {value}");
        }


    }

}

