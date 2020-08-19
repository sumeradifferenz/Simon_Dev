using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Simon.Helpers;
using Simon.Models;
using Simon.Views;
using Simon.Views.Popups;
using Xamarin.Forms;

namespace Simon.ViewModel
{
    public class ApprovalViewModel : BaseViewModel
    {
        string userId;
        private ObservableCollection<ApprovalMainModel> _dataItems { get; set; }
        private ObservableCollection<ApprovalMainModel> sortItems { get; set; }
        private ObservableCollection<ApprovalMainModel> tempContainerList = new ObservableCollection<ApprovalMainModel>();
        public ICommand SortingCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public NavigationHelper _helper;
        bool IsSortApplied;

        public ApprovalViewModel()
        {
            if (Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Application.Current.Properties["USERID"]);
            }
            _dataItems = new ObservableCollection<ApprovalMainModel>();
            SearchCommand = new Command(() => SearchCommandExecuteAsync());
            SortingCommand = new Command(() => SortExecute());
            SortCommand = new Command(() => SortCommandExecute());
            //ActiveTabHandler();
        }

        private ObservableCollection<DealsSortByModel> _DealsSortByItems = new ObservableCollection<DealsSortByModel>();
        public ObservableCollection<DealsSortByModel> DealsSortByItems
        {
            get { return _DealsSortByItems; }
            set
            {
                _DealsSortByItems = value;
                OnPropertyChanged(nameof(DealsSortByItems));
            }
        }

        public ObservableCollection<ApprovalMainModel> ApprovalItems
        {
            get { return _dataItems; }
            set
            {
                _dataItems = value;
                OnPropertyChanged();
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

        private bool _IsApprovalListVisible { get; set; } = false;
        public bool IsApprovalListVisible
        {
            get { return _IsApprovalListVisible; }
            set
            {
                _IsApprovalListVisible = value;
                OnPropertyChanged(nameof(IsApprovalListVisible));
            }
        }

        private bool _IsOpenSeperatorVisible;
        public bool IsOpenSeperatorVisible
        {
            get { return _IsOpenSeperatorVisible; }
            set
            {
                _IsOpenSeperatorVisible = value;
                OnPropertyChanged(nameof(IsOpenSeperatorVisible));
            }
        }

        public string _sortingIcon { get; set; } = "sort.png";
        public string SortingIcon
        {
            get { return _sortingIcon; }
            set
            {
                _sortingIcon = value;
                OnPropertyChanged("SortingIcon");
            }
        }

        private bool _IsRecentSeperatorVisible;
        public bool IsRecentSeperatorVisible
        {
            get { return _IsRecentSeperatorVisible; }
            set
            {
                _IsRecentSeperatorVisible = value;
                OnPropertyChanged(nameof(IsRecentSeperatorVisible));
            }
        }

        private DateTime _ApprovalDate { get; set; }
        public DateTime ApprovalDate
        {
            get { return _ApprovalDate; }
            set
            {
                _ApprovalDate = value;
                OnPropertyChanged(nameof(ApprovalDate));
            }
        }

        private Style _recenttabStyle { get; set; } = (Style)App.Current.Resources["LatoRegularDarkGrayLableStyle"];
        public Style recenttabStyle
        {
            get { return _recenttabStyle; }
            set
            {
                _recenttabStyle = value;
                OnPropertyChanged(nameof(recenttabStyle));
            }
        }

        private Style _opentabStyle { get; set; } = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"];
        public Style opentabStyle
        {
            get { return _opentabStyle; }
            set
            {
                _opentabStyle = value;
                OnPropertyChanged(nameof(opentabStyle));
            }
        }

        private string _SortBylblText;
        public string SortBylblText
        {
            get
            {
                return _SortBylblText;
            }
            set
            {
                SetProperty(ref _SortBylblText, value);
            }
        }

        public string DataNotAvailable { get { return "None"; } }

        public async Task GetApprovalData()
        {
            IsBusy = true;
            App.buttonClick = 0;
            ActiveTabHandler();
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var jsonString = await hc.GetStringAsync(Config.APPROVAL_URL + userId);
                    if (jsonString != null)
                    {
                        var obj = JsonConvert.DeserializeObject<List<ApprovalMainModel>>(jsonString);

                        if (obj.Count > 0)
                        {
                            IsDataNotAvailable = false;
                            IsApprovalListVisible = true;

                            foreach (var user in obj)
                            {
                                ApprovalItems.Add(user);
                                tempContainerList.Add(user);
                            }
                        }
                        else
                        {
                            IsDataNotAvailable = true;
                            IsApprovalListVisible = false;
                        }
                        GetApprovalSortByData();
                    }
                    else
                    {
                        IsDataNotAvailable = true;
                        IsApprovalListVisible = false;
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private void SearchCommandExecuteAsync()
        {
            string value = SearchText;
            SearchDeal(value);
        }

        /// <summary>
        ///Approval list item tap command.
        /// </summary>
        public ICommand ApprovalListItemTapCommand { get { return new Command<ApprovalMainModel>(ApprovalListItemTapCommandExecute); } }
        private async void ApprovalListItemTapCommandExecute(ApprovalMainModel item)
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CommentsPage(item));
            }
            catch (Exception ex)
            {
                ShowExceptionAlert(ex);
            }
        }

        public async void SortExecute()
        {
            try
            {
                await ClosePopup();
                DealSortByPopup DealSortByPopupview = new DealSortByPopup();
                DealSortByPopupview.BindingContext = this;
                SortBylblText = Constants.SortBylblText;
                await ShowPopup(DealSortByPopupview);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void GetApprovalSortByData()
        {
            DealsSortByItems.Clear();
            DealsSortByItems.Add(new DealsSortByModel { name = Constants.BorrowerlblText, Radiobtnimg = (App.ApprovalSelectedTitle == Constants.BorrowerlblText) ? Constants.RadiobtnSelectImg : Constants.RadiobtnUnselectImg });
            DealsSortByItems.Add(new DealsSortByModel { name = Constants.ApprovalDatelblText, Radiobtnimg = (App.ApprovalSelectedTitle == Constants.ApprovalDatelblText) ? Constants.RadiobtnSelectImg : Constants.RadiobtnUnselectImg });
            DealsSortByItems.Add(new DealsSortByModel { name = Constants.ClearlblText, Radiobtnimg = (App.ApprovalSelectedTitle == Constants.ClearlblText) ? Constants.RadiobtnSelectImg : Constants.RadiobtnUnselectImg });
        }

        public ICommand DealsSortByclicked { get { return new Command<DealsSortByModel>(DealsSortBy_click); } }
        private async void DealsSortBy_click(DealsSortByModel data)
        {
            try
            {
                var IsAnyUserNameSelected = DealsSortByItems.Where(x => x.Radiobtnimg == Constants.RadiobtnSelectImg).ToList();
                if (IsAnyUserNameSelected.Count > 0 && IsAnyUserNameSelected != null)
                {
                    foreach (var item in IsAnyUserNameSelected)
                    {
                        item.Radiobtnimg = Constants.RadiobtnUnselectImg;
                        item.NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
                        item.SortByAscDescbtnimg = "";
                    }
                }

                if (data != null)
                {
                    if (data.Radiobtnimg == Constants.RadiobtnUnselectImg)
                    {
                        data.Radiobtnimg = Constants.RadiobtnSelectImg;
                        if (data.name == Constants.BorrowerlblText)
                        {
                            data.SortByAscDescbtnimg = Constants.NameAcendingImg;
                        }
                        else if (data.name == Constants.ApprovalDatelblText)
                        {
                            data.SortByAscDescbtnimg = Constants.NumberAcendingImg;
                        }
                        data.NamelblStyle = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"];
                    }
                    else
                    {
                        data.Radiobtnimg = Constants.RadiobtnUnselectImg;
                        if (data.name == Constants.BorrowerlblText)
                        {
                            data.SortByAscDescbtnimg = "";
                        }
                        else if (data.name == Constants.ApprovalDatelblText)
                        {
                            data.SortByAscDescbtnimg = "";
                        }
                        data.NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
                    }
                    App.ApprovalSelectedTitle = data.name;
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand SortByAscDesClicked { get { return new Command<DealsSortByModel>(SortByAscDes_Click); } }
        private async void SortByAscDes_Click(DealsSortByModel data)
        {
            try
            {
                if (data.name == Constants.BorrowerlblText)
                {
                    if(data.SortByAscDescbtnimg == Constants.NameDescendingImg)
                    {
                        data.SortByAscDescbtnimg = Constants.NameAcendingImg;
                    }
                    else
                    {
                        data.SortByAscDescbtnimg = Constants.NameDescendingImg;
                    }
                }
                else if (data.name == Constants.ApprovalDatelblText)
                {
                    if (data.SortByAscDescbtnimg == Constants.NumberDescendingImg)
                    {
                        data.SortByAscDescbtnimg = Constants.NumberAcendingImg;
                    }
                    else
                    {
                        data.SortByAscDescbtnimg = Constants.NumberDescendingImg;
                    }
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public async void SortCommandExecute()
        {
            try
            {
                if (DealsSortByItems != null && DealsSortByItems.Count > 0)
                {
                    foreach (var item in DealsSortByItems)
                    {
                        if (item.name == Constants.BorrowerlblText)
                        {
                            if (item.Radiobtnimg == Constants.RadiobtnSelectImg)
                            {
                                if (item.SortByAscDescbtnimg == Constants.NameAcendingImg)
                                {
                                    SortBorrowerUpCommandExecute();
                                }
                                else
                                {
                                    SortBorrowerDownCommandExecute();
                                }
                            }
                        }
                        else if (item.name == Constants.ApprovalDatelblText)
                        {
                            if (item.Radiobtnimg == Constants.RadiobtnSelectImg)
                            {
                                if (item.SortByAscDescbtnimg == Constants.NumberDescendingImg)
                                {
                                    SortDateUpCommandExecute();
                                }
                                else
                                {
                                    SortDateDownCommandExecute();
                                }
                            }
                        }
                        else if (item.name == Constants.ClearlblText)
                        {
                            if (item.Radiobtnimg == Constants.RadiobtnSelectImg)
                            {
                                clearSortingAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public async void SortBorrowerUpCommandExecute()
        {
            await ClosePopup();
            var tempRecords = tempContainerList.OrderBy(c => c.partyname_10).ToList();
            ApprovalItems.Clear();
            foreach (var item in tempRecords)
            {
                ApprovalItems.Add(item);
            }
            var approvalBorrowerUp = ApprovalItems.FirstOrDefault();
            MessagingCenter.Send<object, ApprovalMainModel>(this, "ApprovalSortBorrowerUp", approvalBorrowerUp);
        }

        public async void SortBorrowerDownCommandExecute()
        {
            await ClosePopup();
            var tempRecords = tempContainerList.OrderByDescending(c => c.partyname_10).ToList();
            ApprovalItems.Clear();
            foreach (var item in tempRecords)
            {
                ApprovalItems.Add(item);
            }
            var approvalBorrowerDown = ApprovalItems.FirstOrDefault();
            MessagingCenter.Send<object, ApprovalMainModel>(this, "ApprovalSortBorrowerDown", approvalBorrowerDown);
        }

        public async void SortDateUpCommandExecute()
        {
            await ClosePopup();
            var tempRecords = tempContainerList.OrderBy(c => c.ApprovalDate).ToList();
            ApprovalItems.Clear();
            foreach (var item in tempRecords)
            {
                ApprovalItems.Add(item);
            }
            var approvalDateUp = ApprovalItems.FirstOrDefault();
            MessagingCenter.Send<object, ApprovalMainModel>(this, "ApprovalSortDateUp", approvalDateUp);
        }

        public async void SortDateDownCommandExecute()
        {
            await ClosePopup();
            var tempRecords = tempContainerList.OrderByDescending(c => c.ApprovalDate).ToList();
            ApprovalItems.Clear();
            foreach (var item in tempRecords)
            {
                ApprovalItems.Add(item);
            }
            var approvalDateDown = ApprovalItems.FirstOrDefault();
            MessagingCenter.Send<object, ApprovalMainModel>(this, "ApprovalSortDateDown", approvalDateDown);
        }

        private async void clearSortingAsync()
        {
            await ClosePopup();
            var tempRecords = tempContainerList;
            ApprovalItems.Clear();
            foreach (var item in tempRecords)
            {
                ApprovalItems.Add(item);
            }
            var ClearApprovalSort = ApprovalItems.FirstOrDefault();
            MessagingCenter.Send<object, ApprovalMainModel>(this, "ClearApprovalSort", ClearApprovalSort);
        }

        public string _searchText = string.Empty;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                // _searchText = value;
                SetProperty(ref _searchText, value);
                SearchDeal(value);
            }
        }

        public void SearchDeal(string searchText)
        {
            try
            {
                if (tempContainerList.Count > 0)
                {
                    var tempDealList = new ObservableCollection<ApprovalMainModel>();
                    if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
                    {
                        var Keyword = searchText.ToLower();
                        if (Keyword.Length >= 1)
                        {
                            var data = tempContainerList.Where(x => x.reqname_10.ToLower().Contains(Keyword)
                                                        || x.partyname_10.ToLower().Contains(Keyword)
                                                        || x.dealid_05.Contains(Keyword)
                                                        || x.productdesc_10.ToLower().Contains(Keyword)).ToList();

                            foreach (var item in data)
                            {
                                tempDealList.Add(item);
                            }
                        }
                        else
                        {
                            foreach (var item in ApprovalItems)
                            {
                                tempDealList.Add(item);
                            }
                        }

                        ApprovalItems = new ObservableCollection<ApprovalMainModel>(tempDealList.OrderBy(o => o.dealid_05).ToList());
                    }
                    else
                    {
                        ApprovalItems = new ObservableCollection<ApprovalMainModel>(tempContainerList);
                    }
                    showHideListView();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        void showHideListView()
        {
            if (ApprovalItems.Count > 0)
            {
                IsDataNotAvailable = false;
                IsApprovalListVisible = true;
            }
            else
            {
                IsDataNotAvailable = true;
                IsApprovalListVisible = false;
            }
        }

        public ICommand OpenTab_ClickCommand { get { return new Command(OpenTab_Click); } }
        private async void OpenTab_Click()
        {
            try
            {
                Settings.ApprovePageSelectedTabIndex = 0;
                ActiveTabHandler();
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public ICommand RecentTab_ClickCommand { get { return new Command(RecentTab_Click); } }
        private async void RecentTab_Click()
        {
            try
            {
                Settings.ApprovePageSelectedTabIndex = 1;
                ActiveTabHandler();
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        public void ActiveTabHandler()
        {
            if (Settings.ApprovePageSelectedTabIndex == 0)
            {
                // Open Tab
                if (!IsBusy)
                {
                    IsDataNotAvailable = ApprovalItems.Count > 0 ? false : true;
                    IsApprovalListVisible = ApprovalItems.Count > 0 ? true : false;
                }

                IsOpenSeperatorVisible = true;
                IsRecentSeperatorVisible = false;
                opentabStyle = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"];
                recenttabStyle = (Style)App.Current.Resources["LatoRegularDarkGrayLableStyle"];
            }
            else
            {
                // Recent Tab
                IsApprovalListVisible = false;
                IsDataNotAvailable = true;
                IsOpenSeperatorVisible = false;
                IsRecentSeperatorVisible = true;
                opentabStyle = (Style)App.Current.Resources["LatoRegularDarkGrayLableStyle"];
                recenttabStyle = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"];
            }
        }

        public ICommand ClosePopup_Command { get { return new Command(ClosePopup_click); } }
        private async void ClosePopup_click()
        {
            await ClosePopup();
        }
    }
}

