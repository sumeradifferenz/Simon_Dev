using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Settings;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Simon.Helpers;
using Simon.Models;
using Simon.ServiceHandler;
using Simon.Views;
using Simon.Views.Popups;
using Xamarin.Forms;

namespace Simon.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            FooterItems = SessionService.BaseFooterItems;
        }

        private string _ScreenTitle { get; set; }
        public string ScreenTitle
        {
            get
            {
                return _ScreenTitle;
            }
            set
            {
                _ScreenTitle = value;
                App.ScreenTitle = value;
                OnPropertyChanged(nameof(ScreenTitle));
            }
        }

        private string _headerTitle = "";
        public string HeaderTitle
        {
            get { return _headerTitle; }
            set { SetProperty(ref _headerTitle, value); }
        }

        private string _headerLeftImage = "";
        public string HeaderLeftImage
        {
            get { return _headerLeftImage; }
            set { SetProperty(ref _headerLeftImage, value); }
        }

        // here's your shared IsBusy property
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                // again, this is very important
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        // this little bit is how we trigger the PropertyChanged notifier.
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backfield, T value,
            [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backfield, value))
            {
                return false;
            }
            backfield = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        /// <summary>
        /// ShowLoader
        /// </summary>
        /// <param name="animate">Pass true/false for animate the popup view</param>
        /// <returns></returns>
        public async Task ShowLoader(bool animate = false)
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new LoadingPopup(), animate);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// Hides loading popup page
        /// </summary>
        public ICommand ClosePopupCommand { get { return new Command(ClosePopupFromCommand); } }
        public async void ClosePopupFromCommand()
        {
            await ClosePopup(false);
        }

        /// <summary>
        /// ClosePopup
        /// </summary>
        /// <param name="animate">Pass true/false for animate the popup view</param>
        /// <returns></returns>

        public static async Task ShowPopup(PopupPage popup, bool animate = false, bool closePreviousPopup = true)
        {
            try
            {
                if (closePreviousPopup)
                    await ClosePopup();

                await PopupNavigation.Instance.PushAsync(popup, animate);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand BackCommand { get { return new Command(Back); } }
        public void Back()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    int NavigationStackCount = App.Current.MainPage.Navigation.NavigationStack.Count;
                    if (NavigationStackCount > 1)
                    {
                        var PreviousScreen = App.Current.MainPage.Navigation.NavigationStack[NavigationStackCount - 2];
                        if (PreviousScreen != null)
                        {
                            //App.ScreenTitle = ((BaseViewModel)PreviousScreen.BindingContext).;
                            //App.CurrentMenuTitle = string.Empty;
                        }
                    }
                    await App.Current.MainPage.Navigation.PopAsync(false);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task ClosePopup(bool animate = false)
        {
            try
            {
                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    await PopupNavigation.Instance.PopAsync(animate);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void ShowExceptionAlert(Exception ex)
        {
            App.Current.MainPage.DisplayAlert("Exception", ex.Message, "OK");
        }

        public async Task ShowAlert(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public async Task ShowAlertWithAction(string title, string message, string action1, string action2)
        {
            var action = await App.Current.MainPage.DisplayAlert(title, message, action1, action2);
            if (action)
            {
                Back();
            }
        }

        public ICommand FooterNavigationCommand => new Command<FooterModel>(FooterNavigationClick);
        private void FooterNavigationClick(FooterModel footer)
        {
            try
            {
                FooterNavigation(footer);
            }
            catch (Exception exception)
            {
                Console.Write(exception);
            }
        }

        public void FooterNavigation(FooterModel selectedItem)
        {
            try
            {
                //if (App.SelectedFooterItem == selectedItem) {
                //    return;
                //}

                CrossSettings.Current.Remove(Settings.ApprovePageSelectedTabKey);

                SessionService.SelectedFooterItem = selectedItem;
                App.selectedPageId = selectedItem.Id;
                SessionService.BaseFooterItems.All((arg) =>
                {
                    if (arg.Id == selectedItem.Id)
                    {
                        arg.IsSelected = true;
                    }
                    else
                    {
                        arg.IsSelected = false;
                    }
                    return true;
                });
                NavigateToTab(selectedItem.Id);
            }
            catch (Exception exception)
            {
                Debug.Write(exception.Message);
            }
        }

        public void NavigateToTab(int id)
        {
            try
            {
                switch (id)
                {
                    case 0:
                        Application.Current.MainPage = new NavigationPage(new LandingPage()) { BarTextColor = Color.Black };
                        break;
                    case 1:
                        Application.Current.MainPage = new NavigationPage(new DealsPage()) { BarTextColor = Color.Black };
                        break;
                    case 2:
                        Application.Current.MainPage = new NavigationPage(new MessagesPage()) { BarTextColor = Color.Black };
                        break;
                    case 3:
                        Application.Current.MainPage = new NavigationPage(new AssentMainPage()) { BarTextColor = Color.Black };
                        break;
                }
                //Application.Current.MainPage = new NavigationPage(new CarouselMainPage(id)) { BarTextColor = Color.Black };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private ObservableCollection<FooterModel> _footerItems = new ObservableCollection<FooterModel>();
        public ObservableCollection<FooterModel> FooterItems
        {
            get { return _footerItems; }
            set
            {
                SetProperty(ref _footerItems, value);
            }
        }
    }
}


