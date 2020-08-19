using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using CoreGraphics;
using Foundation;
using Simon.Controls;
using Simon.Helpers;
using Simon.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace Simon.iOS.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        UILabel _placeholderLabel;
        double previousHeight = -1;
        int prevLines = 0;
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            try
            {
                if (Control != null)
                {
                    if (_placeholderLabel == null)
                    {
                        CreatePlaceholder();
                    }
                }

                var view = (CustomEditor)Element;
                if (view != null && Control != null)
                {
                    SetDoneButton(view);
                    Control.TintColor = view.TextColor.ToUIColor();
                    Control.Layer.BorderWidth = 0;

                    Control.LayoutManager.AllowsNonContiguousLayout = false;
                    Control.ScrollRangeToVisible(new NSRange(Control.Text.Length - 1, 1));

                    if (view.IsExpandable)
                        Control.ScrollEnabled = false;
                    else
                        Control.ScrollEnabled = true;

                    var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f));

                    //var doneButton = new UIBarButtonItem(TextResources.SecurityCodeText9, UIBarButtonItemStyle.Plain, (o, a) => {
                    //    Control.ResignFirstResponder();
                    //    if (view.ReturnCommand != null)
                    //    {
                    //        view.ReturnCommand.Execute(this);
                    //    }
                    //});

                    var nextButton = new UIBarButtonItem("Next", UIBarButtonItemStyle.Plain, (o, a) => {
                        Control.ResignFirstResponder();
                        ((IEntryController)Element).SendCompleted();
                    });
                    
                    if (view.ReturnType == ReturnType.Next)
                    {
                        toolbar.Items = new[]
                        {
                                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                                nextButton
                        };
                    }
                    else if (view.ReturnType == ReturnType.Done)
                    {
                        //toolbar.Items = new[]
                        //{
                        //        new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                        //        doneButton
                        //    };
                    }
                    this.Control.InputAccessoryView = toolbar;
                }

                if (e.NewElement != null)
                {
                    var customControl = (CustomEditor)e.NewElement;

                    if (customControl.IsExpandable)
                        Control.ScrollEnabled = false;
                    else
                        Control.ScrollEnabled = true;

                    //Control.InputAccessoryView = new UIView(CGRect.Empty);
                    // Control.InputAccessoryView =
                    UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, 50.0f, 44.0f));
                    var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
                    {
                        this.Control.ResignFirstResponder();
                    });

                    doneButton.TintColor = UIColor.Black;

                    toolbar.Items = new UIBarButtonItem[]
                    {
                        new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace),
                        doneButton
                    };
                    this.Control.InputAccessoryView = toolbar;
                    Control.ReloadInputViews();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var customControl = (CustomEditor)Element;

            if (e.PropertyName == Editor.TextProperty.PropertyName)
            {
                if (customControl.IsExpandable)
                {
                    CGSize size = Control.Text.StringSize(Control.Font, Control.Frame.Size, UILineBreakMode.WordWrap);

                    int numLines = (int)(size.Height / Control.Font.LineHeight);

                    if (prevLines > numLines)
                    {
                        customControl.HeightRequest = -1;

                    }
                    else if (string.IsNullOrEmpty(Control.Text))
                    {
                        customControl.HeightRequest = -1;
                    }

                    prevLines = numLines;
                }

                //if (!string.IsNullOrEmpty(Settings.TypedMessage))
                //{
                //    Control.Text = Settings.TypedMessage;
                //}

                _placeholderLabel.Hidden = !string.IsNullOrEmpty(Control.Text);
            }
            else if (CustomEditor.PlaceholderProperty.PropertyName == e.PropertyName)
            {
                _placeholderLabel.Text = customControl.Placeholder;
            }
            else if (CustomEditor.PlaceholderColorProperty.PropertyName == e.PropertyName)
            {
                _placeholderLabel.TextColor = customControl.PlaceholderColor.ToUIColor();
            }
            else if (CustomEditor.IsExpandableProperty.PropertyName == e.PropertyName)
            {
                if (customControl.IsExpandable)
                {
                    Control.ScrollEnabled = false;
                }
                else
                {
                    Control.ScrollEnabled = true;
                }
            }
            else if (CustomEditor.HeightProperty.PropertyName == e.PropertyName)
            {
                CGSize size = Control.Text.StringSize(Control.Font, Control.Frame.Size, UILineBreakMode.WordWrap);

                int numOfLines = (int)(size.Height / Control.Font.LineHeight);

                if (numOfLines >= 5)
                {
                    Control.ScrollEnabled = true;
                    customControl.HeightRequest = previousHeight;
                }
                else
                {
                    Control.ScrollEnabled = false;
                    previousHeight = customControl.Height;
                }
            }
            else if (CustomEditor.IsFocusedProperty.PropertyName == e.PropertyName)
            {
                if (customControl.IsFocused)
                {
                    Control.BecomeFirstResponder();
                }
                else
                {
                    Control.ResignFirstResponder();
                    return;
                }
            }
        }

        void SetDoneButton(CustomEditor view)
        {
            var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) =>
            {
                App.isFromKeyboardDoneButton = true;
                Control.ResignFirstResponder();
            });

            var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)Control.Frame.Size.Width, 44.0f));

            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                doneButton
            };
            this.Control.InputAccessoryView = toolbar;
        }

        public void CreatePlaceholder()
        {
            var element = Element as CustomEditor;

            _placeholderLabel = new UILabel
            {
                Text = element?.Placeholder,
                TextColor = element.PlaceholderColor.ToUIColor(),
                BackgroundColor = UIColor.Clear
            };

            var edgeInsets = Control.TextContainerInset;
            var lineFragmentPadding = Control.TextContainer.LineFragmentPadding;

            Control.AddSubview(_placeholderLabel);

            var vConstraints = NSLayoutConstraint.FromVisualFormat(
                "V:|-" + edgeInsets.Top + "-[PlaceholderLabel]-" + edgeInsets.Bottom + "-|", 0, new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] { _placeholderLabel }, new NSObject[] { new NSString("PlaceholderLabel") })
            );

            var hConstraints = NSLayoutConstraint.FromVisualFormat(
                "H:|-" + lineFragmentPadding + "-[PlaceholderLabel]-" + lineFragmentPadding + "-|",
                0, new NSDictionary(),
                NSDictionary.FromObjectsAndKeys(
                    new NSObject[] { _placeholderLabel }, new NSObject[] { new NSString("PlaceholderLabel") })
            );

            _placeholderLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            //Control.AddConstraints(hConstraints);
            //Control.AddConstraints(vConstraints);
        }
    }
}

