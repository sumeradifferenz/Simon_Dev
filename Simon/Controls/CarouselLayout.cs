using System;

using Xamarin.Forms;

namespace Simon.Controls
{
    public class CarouselLayout : ScrollView
    {
        public int SelectedIndex
        {
            get;
            set;
        }
        public EventHandler<int> OnSwipe { get; set; }
    }
}

