using System;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;

namespace Simon.Controls
{
    public class CustomFlowListView : FlowListView
    {
        public CustomFlowListView()
        {
            
        }

        public static readonly BindableProperty HideScorllbarProperty =
          BindableProperty.Create("HideScorllbar", typeof(bool), typeof(CustomFlowListView), false);

        public bool HideScorllbar
        {
            get
            {
                return (bool)GetValue(HideScorllbarProperty);
            }
            set
            {
                SetValue(HideScorllbarProperty, value);
            }
        }
    }
}

