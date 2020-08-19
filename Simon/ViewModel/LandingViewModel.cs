using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.ServiceHandler;
using Xamarin.Forms;

namespace Simon.ViewModel
{
    public class LandingViewModel : BaseViewModel
    {
        string userId;
        private ObservableCollection<LandingModel> _closingList = new ObservableCollection<LandingModel>();
        private ObservableCollection<LandingModel> _decisionDueList = new ObservableCollection<LandingModel>();

        public LandingViewModel()
        {
            if (Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Application.Current.Properties["USERID"]);
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand ClosingListRefereshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await FetchClosingData();

                    IsRefreshing = false;
                });
            }
        }

        public ICommand DecisionDueListRefereshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await FetchDecisionDueData();

                    IsRefreshing = false;
                });
            }
        }

        private bool _IsDataNotAvailable { get; set; } = false;
        public bool IsDataNotAvailable
        {
            get { return _IsDataNotAvailable; }
            set
            {
                _IsDataNotAvailable = value;
                OnPropertyChanged(nameof(IsDataNotAvailable));
            }
        }

        private bool _IsClosingsListVisible { get; set; } = true;
        public bool IsClosingsListVisible
        {
            get { return _IsClosingsListVisible; }
            set
            {
                _IsClosingsListVisible = value;
                OnPropertyChanged(nameof(IsClosingsListVisible));
            }
        }

        private bool _IsDecisionDueListVisible { get; set; } = false;
        public bool IsDecisionDueListVisible
        {
            get { return _IsDecisionDueListVisible; }
            set
            {
                _IsDecisionDueListVisible = value;
                OnPropertyChanged(nameof(IsDecisionDueListVisible));
            }
        }

        private bool _IsClosingSeperatorVisible { get; set; } = true;
        public bool IsClosingSeperatorVisible
        {
            get { return _IsClosingSeperatorVisible; }
            set
            {
                _IsClosingSeperatorVisible = value;
                OnPropertyChanged(nameof(IsClosingSeperatorVisible));
            }
        }

        private bool _IsDecisionDueSeperatorVisible { get; set; } = false;
        public bool IsDecisionDueSeperatorVisible
        {
            get { return _IsDecisionDueSeperatorVisible; }
            set
            {
                _IsDecisionDueSeperatorVisible = value;
                OnPropertyChanged(nameof(IsDecisionDueSeperatorVisible));
            }
        }

        private Style _DecisionDuetabStyle { get; set; } = (Style)App.Current.Resources["LatoRegularDarkGrayLableStyle"];
        public Style DecisionDuetabStyle
        {
            get { return _DecisionDuetabStyle; }
            set
            {
                _DecisionDuetabStyle = value;
                OnPropertyChanged(nameof(DecisionDuetabStyle));
            }
        }

        private Style _ClosingstabStyle { get; set; } = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"];
        public Style ClosingstabStyle
        {
            get { return _ClosingstabStyle; }
            set
            {
                _ClosingstabStyle = value;
                OnPropertyChanged(nameof(ClosingstabStyle));
            }
        }

        public ICommand ClosingsTab_ClickCommand { get { return new Command(ClosingsTab_Click); } }
        private async void ClosingsTab_Click()
        {
            try
            {
                if (ClosingList.Count < 0 || ClosingList.Count == 0)
                {
                    await FetchClosingData();
                }
 
                ClosingstabStyle = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"];
                DecisionDuetabStyle = (Style)App.Current.Resources["LatoRegularDarkGrayLableStyle"]; ;
                IsClosingSeperatorVisible = true;
                IsDecisionDueSeperatorVisible = false;
                if (ClosingList.Count > 0)
                {
                    IsDataNotAvailable = false;
                    IsClosingsListVisible = true;
                    IsDecisionDueListVisible = false;
                }
                else
                {
                    IsDataNotAvailable = true;
                    IsClosingsListVisible = true;
                    IsDecisionDueListVisible = false;
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand DecisionDueTab_ClickCommand { get { return new Command(DecisionDueTab_Click); } }
        private async void DecisionDueTab_Click()
        {
            try
            {
                if (DecisionDueList.Count < 0 || DecisionDueList.Count==0)
                {
                    await FetchDecisionDueData();
                }

                ClosingstabStyle = (Style)App.Current.Resources["LatoRegularDarkGrayLableStyle"];
                DecisionDuetabStyle = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"]; ;
                IsClosingSeperatorVisible = false;
                IsDecisionDueSeperatorVisible = true;
                if (DecisionDueList.Count > 0)
                {
                    IsDataNotAvailable = false;
                    IsDecisionDueListVisible = true;
                    IsClosingsListVisible = false;
                }
                else
                {
                    IsDataNotAvailable = true;
                    IsDecisionDueListVisible = true;
                    IsClosingsListVisible = false;
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public ObservableCollection<LandingModel> ClosingList
        {
            get { return _closingList; }
            set
            {
                _closingList = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<LandingModel> DecisionDueList
        {
            get { return _decisionDueList; }
            set
            {
                _decisionDueList = value;
                OnPropertyChanged(nameof(DecisionDueList));
            }
        }

        public DateTime estimatedClosingDate { get; set; }

        public string MyDateString
        {
            get { return this.estimatedClosingDate.ToString("MM/dd"); }

        }

        public DateTime decisionDueDate { get; set; }

        public string DecisionDueDateString
        {
            get { return this.decisionDueDate.ToString("MM/dd"); }

        }

        public async Task FetchClosingData()
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    //IsBusy = true;
                    var jsonString = await hc.GetStringAsync(Config.CLOSING_API + userId);
                    ClosingList = LandingModel.FromJson(jsonString);
                    //IsBusy = false;
                    if (ClosingList.Count > 0)
                    {
                        IsDataNotAvailable = false;
                        IsClosingsListVisible = true;
                        IsDecisionDueListVisible = false;
                    }
                    else
                    {
                        IsDataNotAvailable = true;
                        IsClosingsListVisible = true;
                        IsDecisionDueListVisible = false;
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task FetchDecisionDueData()
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    //IsBusy = true;
                    var jsonString = await hc.GetStringAsync(Config.DECISIONDUE_API + userId);
                    DecisionDueList = LandingModel.FromJson(jsonString);
                    //IsBusy = false;
                    if (DecisionDueList.Count > 0)
                    {
                        IsDataNotAvailable = false;
                        IsDecisionDueListVisible = true;
                        IsClosingsListVisible = false;
                    }
                    else
                    {
                        IsDataNotAvailable = true;
                        IsDecisionDueListVisible = true;
                        IsClosingsListVisible = false;
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

    }
}

