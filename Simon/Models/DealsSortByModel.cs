using System;
using Simon.Helpers;
using Simon.ViewModel;
using Xamarin.Forms;

namespace Simon.Models
{
    public class DealsSortByModel : BaseViewModel
    {
        public string name { get; set; }

        private string _Radiobtnimg = Constants.RadiobtnUnselectImg;
        public string Radiobtnimg
        {
            get
            {
                return _Radiobtnimg;
            }
            set
            {
                SetProperty(ref _Radiobtnimg, value);
            }
        }

        public string _SortByAscDescbtnimg;
        public string SortByAscDescbtnimg
        {
            get
            {
                return _SortByAscDescbtnimg;
            }
            set
            {
                SetProperty(ref _SortByAscDescbtnimg, value);
            }
        }

        private Style _NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
        public Style NamelblStyle
        {
            get
            {
                return _NamelblStyle;
            }
            set
            {
                SetProperty(ref _NamelblStyle, value);
            }
        }
    }
}

