using System;
using System.ComponentModel;
using System.Windows.Input;
using Simon.Helpers;
using Xamarin.Forms;

namespace Simon.Controls
{
    public class CustomEntry : Entry
    {
        /// <summary>
        /// The font property
        /// </summary>
        public static readonly BindableProperty FontProperty =
            BindableProperty.Create("Font", typeof(Font), typeof(CustomEntry), new Font());

        /// <summary>
        /// The XAlign property
        /// </summary>
        public static readonly BindableProperty XAlignProperty =
            BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(CustomEntry),
            TextAlignment.Start);

        /// <summary>
        /// The HasBorder property
        /// </summary>
        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(CustomEntry), false);

        /// <summary>
        /// The PlaceholderTextColor property
        /// </summary>
        public static readonly BindableProperty PlaceholderTextColorProperty =
            BindableProperty.Create("PlaceholderTextColor", typeof(Color), typeof(CustomEntry), Color.Default);

        /// <summary>
        /// The MaxLength property
        /// </summary>
        public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create("MaxLength", typeof(int), typeof(CustomEntry), int.MaxValue);

        /// <summary>
        /// Gets or sets the MaxLength
        /// </summary>
        public int MaxLength
        {
            get { return (int)this.GetValue(MaxLengthProperty); }
            set { this.SetValue(MaxLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Font
        /// </summary>
        public Font Font
        {
            get { return (Font)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        /// <summary>
        /// Gets or sets the X alignment of the text
        /// </summary>
        public TextAlignment XAlign
        {
            get { return (TextAlignment)GetValue(XAlignProperty); }
            set { SetValue(XAlignProperty, value); }
        }

        /// <summary>
        /// Gets or sets if the border should be shown or not
        /// </summary>
        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }

        /// <summary>
        /// Sets color for placeholder text
        /// </summary>
        public Color PlaceholderTextColor
        {
            get { return (Color)GetValue(PlaceholderTextColorProperty); }
            set { SetValue(PlaceholderTextColorProperty, value); }
        }

        /// <summary>
        /// The left swipe
        /// </summary>
        public EventHandler LeftSwipe;
        /// <summary>
        /// The right swipe
        /// </summary>
        public EventHandler RightSwipe;

        #region KeyboardFacility

        //Need to overwrite default handler because we cant Invoke otherwise

        /*
        public new event EventHandler<EventArgs> Completed;

        public static readonly BindableProperty ReturnTypeProperty =
            BindableProperty.Create<CustomEntry, KeyboardReturnType>(s => s.ReturnType, KeyboardReturnType.Done);

        public KeyboardReturnType ReturnType
        {
            get { return (KeyboardReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }
        */

        //public void InvokeCompleted()
        //{
        //    if (this.Completed != null)
        //    {
        //        this.Completed?.Invoke(this, null);
        //    }
        //    else
        //    {
        //        var control = (this) as CustomEntry;
        //        if (control != null)
        //        {
        //            control.Unfocus();
        //            OnNext();                
        //        }
        //        //AppNativeService.HideKeyboard();
        //    }
        //}

        /*
        public static readonly BindableProperty NextViewProperty =
        BindableProperty.Create("NextView", typeof(View), typeof(CustomEntry));

        public View NextView
        {
            get { return (View)GetValue(NextViewProperty); }
            set { SetValue(NextViewProperty, value); }
        }

        public void OnNext()
        {
            var control = (this) as CustomEntry;
            if (control != null)
            {
                control.Unfocus();
            }
            NextView?.Focus();
        }

        */
        #endregion

        public static readonly BindableProperty HasAutoCapitalizationProperty = BindableProperty.Create(propertyName: "HasAutoCapitalization", returnType: typeof(bool), declaringType: typeof(CustomEntry), defaultValue: true);
        public bool HasAutoCapitalization
        {
            get { return (bool)GetValue(HasAutoCapitalizationProperty); }
            set { SetValue(HasAutoCapitalizationProperty, value); }
        }

        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(propertyName: "ReturnType", returnType: typeof(KeyboardReturnType), declaringType: typeof(CustomEntry), defaultValue: KeyboardReturnType.Default);
        public KeyboardReturnType ReturnType
        {
            get { return (KeyboardReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }
        }

        public static readonly BindableProperty NextViewProperty = BindableProperty.Create("NextView", typeof(View), typeof(CustomEntry));
        public View NextView
        {
            get { return (View)GetValue(NextViewProperty); }
            set { SetValue(NextViewProperty, value); }
        }

        public void OnNext()
        {
            NextView?.Focus();
        }

        public static readonly BindableProperty ReturnKeyClickCommandProperty = BindableProperty.Create(propertyName: "ReturnKeyClickCommand", returnType: typeof(ICommand), declaringType: typeof(CustomEntry));

        public ICommand ReturnKeyClickCommand
        {
            get { return (ICommand)GetValue(ReturnKeyClickCommandProperty); }
            set { SetValue(ReturnKeyClickCommandProperty, value); }
        }
    }

}

