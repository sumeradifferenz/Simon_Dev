using System;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Models
{
    public class FooterModel : BaseViewModel
    {
        #region private fields
        private int _id;
        private string _name;
        private ImageSource _image;
        private ImageSource _unselectedImage;
        private ImageSource _selectedImage;
        private Color _itemTextColor;
        private bool _isSelected = false;
        public Style _TablblStyle;
        public int _MsgCount;
        public bool _isMsgBadgeVisible = false;
        #endregion

        #region public properties

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public ImageSource Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public ImageSource UnSelectedImage
        {
            get { return _unselectedImage; }
            set { SetProperty(ref _unselectedImage, value); }
        }

        public ImageSource SelectedImage
        {
            get { return _selectedImage; }
            set { SetProperty(ref _selectedImage, value); }
        }

        public Color ItemTextColor
        {
            get { return _itemTextColor; }
            set { SetProperty(ref _itemTextColor, value); }
        }

        public Style TablblStyle
        {
            get
            {
                return _TablblStyle;
            }
            set
            {
                SetProperty(ref _TablblStyle, value);
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
                if (value)
                {
                    Image = SelectedImage;
                    ItemTextColor = (Color)App.Current.Resources["WhiteColor"];
                    TablblStyle = (Style)App.Current.Resources["LatoHeavyWhiteLableStyle"];
                }
                else
                {
                    Image = UnSelectedImage;
                    ItemTextColor = (Color)App.Current.Resources["TabUnSelectedLblColor"];
                    TablblStyle = (Style)App.Current.Resources["LatoRegularTabGrayLableStyle"];
                }
            }
        }

        public int MsgCount
        {
            get { return _MsgCount; }
            set { SetProperty(ref _MsgCount, value); }
        }

        public bool isMsgBadgeVisible
        {
            get { return _isMsgBadgeVisible; }
            set { SetProperty(ref _isMsgBadgeVisible, value); }
        }

        #endregion
    }
}
