using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    public class DealViewModel : BaseViewModel
    {
        string userId, selectedName;
        private ObservableCollection<DealsMainModel> _dealList = new ObservableCollection<DealsMainModel>();
        private ObservableCollection<DealsMainModel> AllDealListItems = new ObservableCollection<DealsMainModel>();
        private ObservableCollection<DealsMainModel> FilteredList = new ObservableCollection<DealsMainModel>();
        public List<Stage> StagesList = new List<Stage>();

        public bool isFromSorting = false;
        private bool _isLoadingInfinite = false;
        private int _totalRecords = 0;
        private bool _isLoadingInfiniteEnabled = false;
        private int _CurrentPage = 1;
        private int _LastPage = 0;
        private bool _isTeamLoading = false;
        bool isSortApplied = false;

        public ICommand FilterCommand { get; set; }
        public ICommand ApplayFilterCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        public ICommand SortingCommand { get; set; }
        public ICommand SortAmountUpCommand { get; set; }
        public ICommand SortAmountDownCommand { get; set; }
        public ICommand SortBorrowerUpCommand { get; set; }
        public ICommand SortBorrowerDownCommand { get; set; }
        public ICommand SortDueDateUpCommand { get; set; }
        public ICommand SortDueDateDownCommand { get; set; }
        public ICommand SortClosingUpCommand { get; set; }
        public ICommand SortClosingDownCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand LoadMoreCommand { get; private set; }

        public List<DealsMainModel> statusItemSource { get; set; }
        public List<DealsMainModel> productItemSource { get; set; }

        //Deal SortBy Popup
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

        private DealsSortByModel _selectedSortByData;
        public DealsSortByModel SelectedSortByData
        {
            get
            {
                return _selectedSortByData;
            }
            set
            {
                SetProperty(ref _selectedSortByData, value);
            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetProperty(ref _Name, value);
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

        //Deals Filterby Popup
        private ObservableCollection<DealsSortByModel> _DealsFilterByItems = new ObservableCollection<DealsSortByModel>();

        public ObservableCollection<DealsSortByModel> DealsFilterByItems
        {
            get { return _DealsFilterByItems; }
            set
            {
                _DealsFilterByItems = value;
                OnPropertyChanged(nameof(DealsFilterByItems));
            }
        }

        public bool IsLoadingInfinite
        {
            get { return _isLoadingInfinite; }
            set { SetProperty(ref _isLoadingInfinite, value); }
        }
        public int TotalRecords
        {
            get { return _totalRecords; }
            set { SetProperty(ref _totalRecords, value); }
        }
        public bool IsLoadingInfiniteEnabled
        {
            get { return _isLoadingInfiniteEnabled; }
            set { SetProperty(ref _isLoadingInfiniteEnabled, value); }
        }


        public DealViewModel()
        {
            if (Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Application.Current.Properties["USERID"]);
            }

            FilterCommand = new Command(() => FilterCommandExcecute());
            ApplayFilterCommand = new Command(() => ApplayFilterCommandExcecute());
            ClearFilterCommand = new Command(() => ClearFilterCommandExcecute());

            SearchCommand = new Command(() => SearchCommandExecuteAsync());

            SortingCommand = new Command(async () =>await SortingCommandExecuteAsync());
            SortAmountUpCommand = new Command(() => SortAmtUpCommandExecute());
            SortAmountDownCommand = new Command(() => SortAmtDownCommandExecute());
            SortBorrowerUpCommand = new Command(() => SortBorrowerUpCommandExecute());
            SortBorrowerDownCommand = new Command(() => SortBorrowerDownCommandExecute());
            SortDueDateUpCommand = new Command(() => SortDueDateUpCommandExecute());
            SortDueDateDownCommand = new Command(() => SortDueDateDownCommandExecute());
            SortClosingUpCommand = new Command(() => SortClosingUpCommandExecute());
            SortClosingDownCommand = new Command(() => SortClosingDownCommandExecute());
            SortCommand = new Command(() => SortCommandExecute());
            LoadMoreCommand = new Command(async () => { await LoadMore_click(); });
            //SearchCommand = new Command(() => SearchCommandExecute());
        }

        public void GetDealsSortByData()
        {
            DealsSortByItems.Clear();
            DealsSortByItems.Add(new DealsSortByModel { name = Constants.BorrowerlblText, Radiobtnimg = (selectedName == Constants.BorrowerlblText) ? Constants.RadiobtnSelectImg : Constants.RadiobtnUnselectImg });
            DealsSortByItems.Add(new DealsSortByModel { name = Constants.AmountlblText, Radiobtnimg = (selectedName == Constants.AmountlblText) ? Constants.RadiobtnSelectImg : Constants.RadiobtnUnselectImg });
            DealsSortByItems.Add(new DealsSortByModel { name = Constants.DueDatelblText, Radiobtnimg = (selectedName == Constants.DueDatelblText) ? Constants.RadiobtnSelectImg : Constants.RadiobtnUnselectImg });
            DealsSortByItems.Add(new DealsSortByModel { name = Constants.ClosingDatelblText, Radiobtnimg = (selectedName == Constants.ClosingDatelblText) ? Constants.RadiobtnSelectImg : Constants.RadiobtnUnselectImg });
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
                        else if (data.name == Constants.AmountlblText || data.name == Constants.DueDatelblText || data.name == Constants.ClosingDatelblText)
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
                        else if (data.name == Constants.AmountlblText || data.name == Constants.DueDatelblText || data.name == Constants.ClosingDatelblText)
                        {
                            data.SortByAscDescbtnimg = "";
                        }
                        data.NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
                    }

                    SelectedSortByData = data;
                    if (!string.IsNullOrEmpty(SelectedSortByData.name))
                    {
                        Name = SelectedSortByData.name;
                        selectedName = Name;
                    }
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
                    data.SortByAscDescbtnimg = Constants.NameDescendingImg;
                }
                else if (data.name == Constants.AmountlblText || data.name == Constants.DueDatelblText || data.name == Constants.ClosingDatelblText)
                {
                    data.SortByAscDescbtnimg = Constants.NumberDescendingImg;
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
                IsLoadingInfiniteEnabled = true;
                if (DealsSortByItems != null && DealsSortByItems.Count > 0)
                {
                    foreach (var item in DealsSortByItems)
                    {
                        isSortApplied = true;
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
                        else if (item.name == Constants.AmountlblText)
                        {
                            if (item.Radiobtnimg == Constants.RadiobtnSelectImg)
                            {
                                if (item.SortByAscDescbtnimg == Constants.NumberDescendingImg)
                                {
                                    SortAmtUpCommandExecute();
                                }
                                else
                                {
                                    SortAmtDownCommandExecute();
                                }
                            }
                        }
                        else if (item.name == Constants.DueDatelblText)
                        {
                            if (item.Radiobtnimg == Constants.RadiobtnSelectImg)
                            {
                                if (item.SortByAscDescbtnimg == Constants.NumberDescendingImg)
                                {
                                    SortDueDateUpCommandExecute();
                                }
                                else
                                {
                                    SortDueDateDownCommandExecute();
                                }
                            }
                        }
                        else if (item.name == Constants.ClosingDatelblText)
                        {
                            if (item.Radiobtnimg == Constants.RadiobtnSelectImg)
                            {
                                if (item.SortByAscDescbtnimg == Constants.NumberDescendingImg)
                                {
                                    SortClosingUpCommandExecute();
                                }
                                else
                                {
                                    SortClosingDownCommandExecute();
                                }
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

        public ICommand DealsFilterByclicked { get { return new Command<DealsSortByModel>(DealsFilterBy_click); } }
        private async void DealsFilterBy_click(DealsSortByModel data)
        {
            try
            {
                //var IsAnyUserNameSelected = DealsFilterByItems.Where(x => x.Radiobtnimg == Constants.CheckboxImg).ToList();
                //if (IsAnyUserNameSelected.Count > 0 && IsAnyUserNameSelected != null)
                //{
                //    foreach (var item in IsAnyUserNameSelected)
                //    {
                //        item.Radiobtnimg = Constants.CheckboxUnselectImg;
                //        item.NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
                //    }
                //}

                if (data != null)
                {
                    if (data.Radiobtnimg == Constants.CheckboxUnselectImg)
                    {
                        data.Radiobtnimg = Constants.CheckboxImg;
                        data.NamelblStyle = (Style)App.Current.Resources["LatoBoldDarkBlueLableStyle"];
                    }
                    else
                    {
                        data.Radiobtnimg = Constants.CheckboxUnselectImg;
                        data.NamelblStyle = (Style)App.Current.Resources["LatoRegularGrayLableStyle"];
                    }

                    SelectedSortByData = data;
                    if (!string.IsNullOrEmpty(SelectedSortByData.name))
                    {
                        Name = SelectedSortByData.name;
                    }
                }
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Debug.WriteLine(ex.Message);
            }
        }

        private void SearchCommandExecuteAsync()
        {
            string value = SearchText;
            SearchDeal(value);
        }

        private async Task SortingCommandExecuteAsync()
        {
            await ClosePopup();
            DealSortByPopup DealSortByPopupview = new DealSortByPopup();
            DealSortByPopupview.BindingContext = this;
            SortBylblText = Constants.SortBylblText;
            this.GetDealsSortByData();
            await ShowPopup(DealSortByPopupview);
            //if (IsSortingPopupVisible)
            //{
            //    IsSortingPopupVisible = false;
            //}
            //else
            //{
            //    IsSortingPopupVisible = true;
            //    IsFilterPopupVisible = false;
            //}
            //DeletePopup DealPopupview = new DeletePopup();
            //DealPopupview.BindingContext = this;
            //await ShowPopup(DealPopupview);
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

        public bool IsFilterOn { get; set; } = false;

        private bool _IsListDataAvailable { get; set; } = false;
        public bool IsListDataAvailable
        {
            get { return _IsListDataAvailable; }
            set
            {
                _IsListDataAvailable = value;
                OnPropertyChanged(nameof(IsListDataAvailable));
            }
        }

        private bool _isFilterPopupVisible;
        public bool IsFilterPopupVisible
        {
            get { return _isFilterPopupVisible; }
            set
            {
                _isFilterPopupVisible = value;
                OnPropertyChanged(nameof(IsFilterPopupVisible));
            }
        }

        private bool _IsLoadingInfiniteVisible { get; set; } = false;
        public bool IsLoadingInfiniteVisible
        {
            get { return _IsLoadingInfiniteVisible; }
            set
            {
                _IsLoadingInfiniteVisible = value;
                OnPropertyChanged(nameof(IsLoadingInfiniteVisible));
            }
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
                if (AllDealListItems.Count > 0)
                {
                    var tempDealList = new ObservableCollection<DealsMainModel>();
                    if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText))
                    {
                        var Keyword = searchText.ToLower();
                        if (Keyword.Length >= 1)
                        {
                            var saerchedList = AllDealListItems.Where(x => x.partyName.ToLower().Contains(Keyword)
                                            || x.product.ToLower().Contains(Keyword)
                                            || x.dealId.Contains(Keyword)
                                            || x.status.ToLower().Contains(Keyword)).ToList();
                            foreach (var item in saerchedList)
                            {
                                tempDealList.Add(item);
                            }
                            //DealList.Clear();
                            //DealList = new ObservableCollection<DealsMainModel>(saerchedList);
                            //showHideListView();
                        }
                        else
                        {
                            foreach (var item in DealList)
                            {
                                tempDealList.Add(item);
                            }
                            //DealList.Clear();
                            //DealList = new ObservableCollection<DealsMainModel>(AllItems);
                            //showHideListView();
                        }

                        DealList = new ObservableCollection<DealsMainModel>(tempDealList.OrderBy(o => o.dealId).ToList());
                    }
                    else
                    {
                        DealList = new ObservableCollection<DealsMainModel>(AllDealListItems);
                    }
                    showHideListView();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private bool _isSortingPopupVisible;
        public bool IsSortingPopupVisible
        {
            get { return _isSortingPopupVisible; }
            set
            {
                _isSortingPopupVisible = value;
                OnPropertyChanged(nameof(IsSortingPopupVisible));
            }
        }

        void showHideListView()
        {
            if (DealList.Count > 0)
            {
                IsListDataAvailable = true;
                IsDataNotAvailable = false;
            }
            else
            {
                IsListDataAvailable = false;
                IsDataNotAvailable = true;
            }
        }

        private async void FilterCommandExcecute()
        {
            try
            {
                await ClosePopup();
                DealsFilterPopup DealFilterByPopupview = new DealsFilterPopup();
                DealFilterByPopupview.BindingContext = this;
                // this.GetFilterByData();
                await ShowPopup(DealFilterByPopupview);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                ShowExceptionAlert(ex);
            }
        }

        //public void GetFilterByData()
        //{
        //    DealsFilterByItems.Clear();
        //    //DealList = new ObservableCollection<DealsMainModel>(AllDealListItems);
        //    var statusList = this.StagesList.Select(w => w.stagelabel_10);
        //    StatusPickerList = new ObservableCollection<string>(statusList.Distinct().ToList());
        //    if (StatusPickerList != null && StatusPickerList.Count > 0)
        //    {
        //        foreach (var item in StatusPickerList)
        //        {
        //            DealsFilterByItems.Add(new DealsSortByModel { name = item, Radiobtnimg = Constants.CheckboxUnselectImg });
        //        }
        //    }
        //}

        private async void ApplayFilterCommandExcecute()
        {
            try
            {
                FilteredList.Clear();
                IsLoadingInfiniteEnabled = true;
                var IsAnyItemSelected = DealsFilterByItems.Where(x => x.Radiobtnimg == Constants.CheckboxImg).ToList();
                if (IsAnyItemSelected.Count > 0 && IsAnyItemSelected != null)
                {
                    ObservableCollection<DealsMainModel> filterData = new ObservableCollection<DealsMainModel>();
                    foreach (var item in IsAnyItemSelected)
                    {
                        var data = this.AllDealListItems.Where(w => (w.status.Equals(item.name))).ToList();
                        filterData = new ObservableCollection<DealsMainModel>(data);
                        foreach (var item1 in filterData)
                        {
                            FilteredList.Add(item1);
                        }
                        filterData.Clear();
                    }

                    if (FilteredList.Count > 0)
                    {
                        await ClosePopup();
                        IsListDataAvailable = true;
                        IsDataNotAvailable = false;
                        IsFilterOn = true;
                        DealList.Clear();
                        DealList = new ObservableCollection<DealsMainModel>(FilteredList.ToList());
                        var firstMessage = DealList.FirstOrDefault();
                        MessagingCenter.Send<object, DealsMainModel>(this, "DealsFilterApplied", firstMessage);
                    }
                    else
                    {
                        await ClosePopup();
                        IsListDataAvailable = false;
                        IsDataNotAvailable = true;
                        IsFilterOn = true;
                        DealList = new ObservableCollection<DealsMainModel>(FilteredList.ToList());
                    }
                }
                else
                {
                    //If no filter option is selcted clear the filter %
                    await ClosePopup();
                    if (AllDealListItems.Count > 0)
                    {
                        IsListDataAvailable = false;
                        IsDataNotAvailable = false;
                        IsFilterOn = false;
                        DealList = new ObservableCollection<DealsMainModel>(AllDealListItems);
                    }
                    //await ShowAlert("Alert", "Please select filter option");
                }

                //string status = StatusPicked;
                //if (!string.IsNullOrEmpty(status))
                //{
                //    var data = this.DealList.Where(w => (w.status.Equals(StatusPicked))).ToList();
                //    FilteredList = new ObservableCollection<DealsMainModel>(data);
                //    if (FilteredList.Count > 0)
                //    {
                //        IsListDataAvailable = true;
                //        IsDataNotAvailable = false;
                //        DealList.Clear();
                //        DealList = new ObservableCollection<DealsMainModel>(FilteredList.ToList());
                //    }
                //    else
                //    {
                //        IsListDataAvailable = false;
                //        IsDataNotAvailable = true;
                //        DealList = new ObservableCollection<DealsMainModel>(AllItems);
                //    }
                //}
                //else
                //{
                //    await ShowAlert("Alert", "Please select filter option");
                //}
            }
            catch (Exception ex)
            {
                await ClosePopup();
                Console.Write(ex.Message);
                ShowExceptionAlert(ex);
            }
        }

        private void ClearFilterCommandExcecute()
        {
            try
            {
                IsFilterPopupVisible = false;
                StatusPicked = string.Empty;
                showHideListView();
                //if (IsFilterApplied)
                //{
                //    IsFilterApplied = false;
                //    DealList.Clear();
                //    DealList = new ObservableCollection<DealsMainModel>(AllItems);
                //}
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                ShowExceptionAlert(ex);
            }
        }

        private string _statusPicked;
        public string StatusPicked
        {
            get { return _statusPicked; }
            set
            {
                if (_statusPicked != value)
                {
                    _statusPicked = value;
                    DealList = new ObservableCollection<DealsMainModel>(AllDealListItems);
                    OnPropertyChanged(nameof(StatusPicked));
                }
            }
        }

        private ObservableCollection<string> _statusPikcerList = new ObservableCollection<string>();
        public ObservableCollection<string> StatusPickerList
        {
            get { return _statusPikcerList; }
            set
            {
                _statusPikcerList = value;
                OnPropertyChanged(nameof(StatusPickerList));
            }
        }

        public ObservableCollection<DealsMainModel> DealList
        {
            get { return _dealList; }
            set
            {
                _dealList = value;
                OnPropertyChanged(nameof(DealList));
            }
        }

        private ObservableCollection<DealsMainModel> _selectedStatus;
        public ObservableCollection<DealsMainModel> SelectedStatus
        {
            get
            {
                return _selectedStatus;
            }
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                //SetProperty(ref _selectedStatus, value);
            }
        }

        private string _searchedText;
        public string SearchedText
        {
            get { return _searchedText; }
            set
            {
                _searchedText = value;
                OnPropertyChanged(nameof(SearchedText));
            }
        }

        private bool _isStopVisible = false;
        public bool IsStopVisible
        {
            get
            {
                return _isStopVisible;
            }
            set
            {
                _isStopVisible = value;
                OnPropertyChanged(nameof(IsStopVisible));
            }
        }

        public async Task FetchDealData()
        {
            _isTeamLoading = true;
            IsLoadingInfiniteEnabled = true;
            try
            {
                DealList = new ObservableCollection<DealsMainModel>();
                IsBusy = true;
                _CurrentPage = 1;
                await GetFilterData();
                await LoadData(_CurrentPage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception:>" + ex);
            }
            finally
            {
                IsBusy = false;
                _isTeamLoading = false;
            }
        }

        async Task LoadData(int page)
        {
            var tempOpenData = new ObservableCollection<DealsMainModel>(DealList);
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var jsonString = await hc.GetStringAsync(Config.DEAL_DETAILS_API + userId + "/" + page);
                    if (jsonString != "")
                    {
                        var obj = JsonConvert.DeserializeObject<List<DealsMainModel>>(jsonString);
                        if (obj.Count > 0)
                        {
                            IsDataNotAvailable = false;
                            IsListDataAvailable = true;

                            foreach (var item in obj)
                            {
                                TotalRecords = item.totalRecords;
                                _LastPage = Convert.ToInt32(item.totalPages);
                                tempOpenData.Add(item);
                            }

                            ObservableCollection<DealsMainModel> OrerbyIdDesc = new ObservableCollection<DealsMainModel>(tempOpenData.OrderBy(x => x.dealId));
                            DealList = new ObservableCollection<DealsMainModel>(OrerbyIdDesc);
                            AllDealListItems = new ObservableCollection<DealsMainModel>(OrerbyIdDesc);
                        }
                        else
                        {
                            IsDataNotAvailable = true;
                            IsListDataAvailable = false;
                        }
                    }
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        SearchDeal(SearchText);
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public async Task GetFilterData()
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, Config.FILTER_LIST_API);
                    var client = new HttpClient();
                    var content1 = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Config.FILTER_LIST_API, content1);
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(content))
                    {
                        DealsFilterByItems.Clear();
                        StagesList = JsonConvert.DeserializeObject<List<Stage>>(content);
                        var statusList = this.StagesList.Select(w => w.stagelabel_10);
                        StatusPickerList = new ObservableCollection<string>(statusList.Distinct().ToList());
                        if (StatusPickerList != null && StatusPickerList.Count > 0)
                        {
                            foreach (var item in StatusPickerList)
                            {
                                DealsFilterByItems.Add(new DealsSortByModel { name = item, Radiobtnimg = Constants.CheckboxUnselectImg });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        public async Task LoadMore_click()
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    if (IsFilterOn == true)
                    {
                        //IsFilterOn = false;
                        IsLoadingInfinite = false;
                        IsLoadingInfiniteEnabled = false;
                        return;
                    }

                    if (isSortApplied == true)
                    {
                        //isSortApplied = false;
                        IsLoadingInfinite = false;
                        IsLoadingInfiniteEnabled = false;
                        return;
                    }

                    IsBusy = false;
                    if (_LastPage == _CurrentPage)
                    {
                        IsLoadingInfinite = false;
                        IsLoadingInfiniteEnabled = false;
                        return;
                    }
                    if (_isTeamLoading) { IsLoadingInfinite = false; return; }
                    _CurrentPage++;

                    await LoadData(_CurrentPage);
                }
                catch (Exception ex)
                {
                    IsBusy = false;
                    ShowExceptionAlert(ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }
            IsLoadingInfinite = false;
        }

        public async void SortAmtUpCommandExecute()
        {
            await ClosePopup();
            var tempRecords = _dealList.OrderBy(c => c.amount).ToList();//ascending order
            DealList.Clear();
            foreach (var item in tempRecords)
            {
                DealList.Add(item);
            }
            _isStopVisible = false;
            var dealAmountUp = DealList.FirstOrDefault();
            MessagingCenter.Send<object, DealsMainModel>(this, "DealsSortAmountUp", dealAmountUp);
        }

        public async void SortAmtDownCommandExecute()
        {
            await ClosePopup();
            var tempRecords = _dealList.OrderByDescending(c => c.amount).ToList();//Descending order
            DealList.Clear();
            foreach (var item in tempRecords)
            {
                DealList.Add(item);
            }
            IsStopVisible = false;
            var dealAmountDown = DealList.FirstOrDefault();
            MessagingCenter.Send<object, DealsMainModel>(this, "DealsSortAmountDown", dealAmountDown);
        }

        public async void SortBorrowerUpCommandExecute()
        {
            await ClosePopup();
            var tempRecords = _dealList.OrderBy(c => c.partyName).ToList();//ascending order
            DealList.Clear();
            foreach (var item in tempRecords)
            {
                DealList.Add(item);
            }
            _isStopVisible = false;
            var dealBorrowerUp = DealList.FirstOrDefault();
            MessagingCenter.Send<object, DealsMainModel>(this, "DealsSortBorrowerUp", dealBorrowerUp);
        }

        public async void SortBorrowerDownCommandExecute()
        {
            await ClosePopup();
            var tempRecords = _dealList.OrderByDescending(c => c.partyName).ToList();//Descending order
            DealList.Clear();
            foreach (var item in tempRecords)
            {
                DealList.Add(item);
            }
            IsStopVisible = false;
            var dealBorrowerDown = DealList.FirstOrDefault();
            MessagingCenter.Send<object, DealsMainModel>(this, "DealsSortBorrowerDown", dealBorrowerDown);
        }

        public async void SortDueDateUpCommandExecute()
        {
            await ClosePopup();
            var tempRecords = _dealList.OrderBy(c => c.decisionDueDate).ToList();//ascending order
            DealList.Clear();
            foreach (var item in tempRecords)
            {
                DealList.Add(item);
            }
            _isStopVisible = false;
            var dealDueUp = DealList.FirstOrDefault();
            MessagingCenter.Send<object, DealsMainModel>(this, "DealsSortDueUp", dealDueUp);
        }

        public async void SortDueDateDownCommandExecute()
        {
            await ClosePopup();
            var tempRecords = _dealList.OrderByDescending(c => c.decisionDueDate).ToList();//Descending order
            DealList.Clear();
            foreach (var item in tempRecords)
            {
                DealList.Add(item);
            }
            IsStopVisible = false;
            var dealDueDown = DealList.FirstOrDefault();
            MessagingCenter.Send<object, DealsMainModel>(this, "DealsSortDueDown", dealDueDown);
        }

        public async void SortClosingUpCommandExecute()
        {
            await ClosePopup();
            var tempRecords = _dealList.OrderBy(c => c.estimatedClosingDate).ToList();//ascending order
            DealList.Clear();
            foreach (var item in tempRecords)
            {
                DealList.Add(item);
            }
            _isStopVisible = false;
            var dealClosingUp = DealList.FirstOrDefault();
            MessagingCenter.Send<object, DealsMainModel>(this, "DealsSortClosingUp", dealClosingUp);
        }

        public async void SortClosingDownCommandExecute()
        {
            await ClosePopup();
            var tempRecords = _dealList.OrderByDescending(c => c.estimatedClosingDate).ToList();//Descending order
            DealList.Clear();
            foreach (var item in tempRecords)
            {
                DealList.Add(item);
            }
            IsStopVisible = false;
            var dealClosingDown = DealList.FirstOrDefault();
            MessagingCenter.Send<object, DealsMainModel>(this, "DealsSortClosingDown", dealClosingDown);
        }

        public ICommand ClosePopup_Command { get { return new Command(ClosePopup_click); } }
        private async void ClosePopup_click()
        {
            await ClosePopup();
        }
    }
}