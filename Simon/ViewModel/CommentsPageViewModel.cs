using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simon.Helpers;
using Simon.Models;
using Simon.Views;
using Xamarin.Forms;

namespace Simon.ViewModel
{
    public class CommentsPageViewModel : BaseViewModel
    {
        string userId, statusTxt, needsExceptionActions, primaryDocumentException, initiateProcessIdTxt, threadId, threadTitle, Operand;
        IEnumerable<ApprovalPendingModel> resultUserList;
        JObject jObject = null;
        public bool isQuestionAvailable = false;
        
        public NavigationHelper _helper;

        public CommentsPageViewModel()
        {
            HeaderTitle = Constants.ApprovalHeaderlblText;
            HeaderLeftImage = "back_arrow.png";

            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Xamarin.Forms.Application.Current.Properties["USERID"]);
            }
        }

        private ObservableCollection<reqQuestionDefinitions> _QuestionData = new ObservableCollection<reqQuestionDefinitions>();
        public ObservableCollection<reqQuestionDefinitions> QuestionData
        {
            get { return _QuestionData; }
            set
            {
                _QuestionData = value;
                OnPropertyChanged(nameof(QuestionData));
            }
        }

        private ObservableCollection<ApprovalPendingModel> _PendingList = new ObservableCollection<ApprovalPendingModel>();
        public ObservableCollection<ApprovalPendingModel> PendingList
        {
            get { return _PendingList; }
            set
            {
                _PendingList = value;
                OnPropertyChanged(nameof(PendingList));
            }
        }

        private string _assignedToFullNameTxt;
        public string assignedToFullNameTxt
        {
            get { return _assignedToFullNameTxt; }
            set { SetProperty(ref _assignedToFullNameTxt, value); }
        }

        private string _processIdTxt;
        public string processIdTxt
        {
            get { return _processIdTxt; }
            set { SetProperty(ref _processIdTxt, value); }
        }

        private string _stageIdTxt;
        public string stageIdTxt
        {
            get { return _stageIdTxt; }
            set { SetProperty(ref _stageIdTxt, value); }
        }

        private int _questionsListHeight = 90;

        public int QuestionsListHeight
        {
            get { return _questionsListHeight; }
            set { SetProperty(ref _questionsListHeight, value); }
        }

        private string _labelParty;
        public string LabelParty
        {
            get { return _labelParty; }
            set { SetProperty(ref _labelParty, value); }
        }

        private string _labelProduct;
        public string LabelProduct
        {
            get { return _labelProduct; }
            set { SetProperty(ref _labelProduct, value); }
        }

        private string _amount_06;
        public string amount_06
        {
            get { return _amount_06; }
            set { SetProperty(ref _amount_06, value); }
        }

        private string _statusDec;
        public string statusDec
        {
            get { return _statusDec; }
            set { SetProperty(ref _statusDec, value); }
        }

        private string _officerName;
        public string officerName
        {
            get { return _officerName; }
            set { SetProperty(ref _officerName, value); }
        }

        private string _FactorName;
        public string FactorName
        {
            get { return _FactorName; }
            set { SetProperty(ref _FactorName, value); }
        }

        private string _MitigatingFactors;
        public string MitigatingFactors
        {
            get { return _MitigatingFactors; }
            set { SetProperty(ref _MitigatingFactors, value); }
        }

        private string _subjectTo;
        public string subjectTo
        {
            get { return _subjectTo; }
            set { SetProperty(ref _subjectTo, value); }
        }

        private string _comments;
        public string comments
        {
            get { return _comments; }
            set { SetProperty(ref _comments, value); }
        }

        private string _senderName;
        public string senderName
        {
            get { return _senderName; }
            set { SetProperty(ref _senderName, value); }
        }

        private DateTime _sendingDate;
        public DateTime sendingDate
        {
            get { return _sendingDate; }
            set { SetProperty(ref _sendingDate, value); }
        }

        private string _SubjectDec;
        public string SubjectDec
        {
            get { return _SubjectDec; }
            set { SetProperty(ref _SubjectDec, value); }
        }

        private bool _isPlusVisible { get; set; } = false;
        public bool isPlusVisible
        {
            get { return _isPlusVisible; }
            set
            {
                _isPlusVisible = value;
                OnPropertyChanged(nameof(isPlusVisible));
            }
        }

        private bool _isStackExpanded { get; set; } = true;
        public bool IsStackExpanded
        {
            get { return _isStackExpanded; }
            set
            {
                _isStackExpanded = value;
                OnPropertyChanged(nameof(IsStackExpanded));
            }
        }

        private bool _isExpanderActive { get; set; } = true;
        public bool IsExpanderActive
        {
            get { return _isExpanderActive; }
            set
            {
                _isExpanderActive = value;
                OnPropertyChanged(nameof(IsExpanderActive));
            }
        }

        private bool _isQuestionListVisible { get; set; } = true;
        public bool isQuestionListVisible
        {
            get { return _isQuestionListVisible; }
            set
            {
                _isQuestionListVisible = value;
                OnPropertyChanged(nameof(isQuestionListVisible));
            }
        }

        private bool _isSubjectToVisible { get; set; } = false;
        public bool isSubjectToVisible
        {
            get { return _isSubjectToVisible; }
            set
            {
                _isSubjectToVisible = value;
                OnPropertyChanged(nameof(isSubjectToVisible));
            }
        }

        private bool _isMitigatingVisible { get; set; } = false;
        public bool isMitigatingVisible
        {
            get { return _isMitigatingVisible; }
            set
            {
                _isMitigatingVisible = value;
                OnPropertyChanged(nameof(isMitigatingVisible));
            }
        }

        private bool _commnetsVisible { get; set; } = false;
        public bool commnetsVisible
        {
            get { return _commnetsVisible; }
            set
            {
                _commnetsVisible = value;
                OnPropertyChanged(nameof(commnetsVisible));
            }
        }

        private bool _sendMessageVisible { get; set; } = false;
        public bool sendMessageVisible
        {
            get { return _sendMessageVisible; }
            set
            {
                _sendMessageVisible = value;
                OnPropertyChanged(nameof(sendMessageVisible));
            }
        }

        private bool _PendingListVisible { get; set; } = false;
        public bool PendingListVisible
        {
            get { return _PendingListVisible; }
            set
            {
                _PendingListVisible = value;
                OnPropertyChanged(nameof(PendingListVisible));
            }
        }

        private bool _ProcessUserListVisible { get; set; } = false;
        public bool ProcessUserListVisible
        {
            get { return _ProcessUserListVisible; }
            set
            {
                _ProcessUserListVisible = value;
                OnPropertyChanged(nameof(ProcessUserListVisible));
            }
        }

        private bool _Action { get; set; } = false;
        public bool Action
        {
            get { return _Action; }
            set
            {
                _Action = value;
                OnPropertyChanged(nameof(Action));
            }
        }

        private bool _ActionDropDown { get; set; } = false;
        public bool ActionDropDown
        {
            get { return _ActionDropDown; }
            set
            {
                _ActionDropDown = value;
                OnPropertyChanged(nameof(ActionDropDown));
            }
        }

        private bool _isSaveButtonVisible { get; set; } = false;
        public bool isSaveButtonVisible
        {
            get { return _isSaveButtonVisible; }
            set
            {
                _isSaveButtonVisible = value;
                OnPropertyChanged(nameof(isSaveButtonVisible));
            }
        }

        //public string plusImg = "plus.png";

        //public Image _plusImage { get; set; } = plusImg.so;
        //public Image plusImage
        //{
        //    get { return _plusImage; }
        //    set
        //    {
        //        _plusImage = value;
        //        OnPropertyChanged("plusImage");
        //    }
        //}

        public async Task FetchCommnetData(int requirementId)
        {
            using (HttpClient hc = new HttpClient())
            {
                try
                {
                    IsBusy = true;
                    var httpClient = new HttpClient();
                    var response = await hc.GetStringAsync(Config.COMMENT_INFO_URL + requirementId);
                    var obj = JsonConvert.DeserializeObject<CommentsInfoModel>(response);

                    if (obj != null)
                    {
                        officerName = obj.primaryOfficer;
                        statusDec = obj.statusDescription;
                        initiateProcessIdTxt = obj.initiateProcessId;
                        statusTxt = obj.status;
                        assignedToFullNameTxt = obj.assignedToFullName;
                        processIdTxt = obj.processId;
                        stageIdTxt = obj.stageId;
                        threadId = obj.threadId;
                        threadTitle = obj.threadTitle;

                        if (obj.reqQuestionDefinitions.Count != 0)
                        {
                            isQuestionAvailable = true;
                            isPlusVisible = true;
                            IsStackExpanded = true;
                            IsExpanderActive = true;

                            QuestionsListHeight = 90;

                            foreach (var item in obj.reqQuestionDefinitions)
                            {
                                QuestionsListHeight = QuestionsListHeight * obj.reqQuestionDefinitions.Count;

                                if (item.actualValueBit != null)
                                {
                                    switch (item.actualValueBit.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.actualValueBit = "Yes";
                                            break;
                                        case "FALSE":
                                            item.actualValueBit = "No";
                                            break;
                                    }
                                }

                                if (item.expectedValueLowBit != null)
                                {
                                    switch (item.expectedValueLowBit.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.expectedValueLowBit = "Yes";
                                            break;
                                        case "FALSE":
                                            item.expectedValueLowBit = "No";
                                            break;
                                    }
                                }

                                if (item.actualValueDate != null)
                                {
                                    switch (item.actualValueDate.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.actualValueDate = "Yes";
                                            break;
                                        case "FALSE":
                                            item.actualValueDate = "No";
                                            break;
                                    }
                                }

                                if (item.expectedValueLowDate != null)
                                {
                                    switch (item.expectedValueLowDate.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.expectedValueLowDate = "Yes";
                                            break;
                                        case "FALSE":
                                            item.expectedValueLowDate = "No";
                                            break;
                                    }
                                }

                                if (item.actualValueDecimal != null)
                                {
                                    switch (item.actualValueDecimal.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.actualValueDecimal = "Yes";
                                            break;
                                        case "FALSE":
                                            item.actualValueDecimal = "No";
                                            break;
                                    }
                                }

                                if (item.expectedValueLowDecimal != null)
                                {
                                    switch (item.expectedValueLowDecimal.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.expectedValueLowDecimal = "Yes";
                                            break;
                                        case "FALSE":
                                            item.expectedValueLowDecimal = "No";
                                            break;
                                    }
                                }

                                if (item.actualValueInt != null)
                                {
                                    switch (item.actualValueInt.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.actualValueInt = "Yes";
                                            break;
                                        case "FALSE":
                                            item.actualValueInt = "No";
                                            break;
                                    }
                                }

                                if (item.expectedValueLowInt != null)
                                {
                                    switch (item.expectedValueLowInt.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.expectedValueLowInt = "Yes";
                                            break;
                                        case "FALSE":
                                            item.expectedValueLowInt = "No";
                                            break;
                                    }
                                }

                                if (item.actualValuePercent != null)
                                {
                                    switch (item.actualValuePercent.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.actualValuePercent = "Yes";
                                            break;
                                        case "FALSE":
                                            item.actualValuePercent = "No";
                                            break;
                                    }
                                }

                                if (item.expectedValueLowPercent != null)
                                {
                                    switch (item.expectedValueLowPercent.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.expectedValueLowPercent = "Yes";
                                            break;
                                        case "FALSE":
                                            item.expectedValueLowPercent = "No";
                                            break;
                                    }
                                }

                                if (item.actualValueText != null)
                                {
                                    switch (item.actualValueText.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.actualValueText = "Yes";
                                            break;
                                        case "FALSE":
                                            item.actualValueText = "No";
                                            break;
                                    }
                                }

                                if (item.expectedValueLowVarchar != null)
                                {
                                    switch (item.expectedValueLowVarchar.Trim().ToUpper())
                                    {
                                        case "TRUE":
                                            item.expectedValueLowVarchar = "Yes";
                                            break;
                                        case "FALSE":
                                            item.expectedValueLowVarchar = "No";
                                            break;
                                    }
                                }

                                if(item.expectedValueLowOperand == "=")
                                {
                                    Operand = "";
                                }
                                else
                                {
                                    Operand = item.expectedValueLowOperand;
                                }

                                if (item.dataType == "bool")
                                {
                                    item.actualValue = "Actual : " + item.actualValueBit;
                                    item.expectedValue = "Expected : " + Operand + item.expectedValueLowBit.ToString();
                                }
                                else if (item.dataType == "int")
                                {
                                    item.actualValue = "Actual: " + item.actualValueInt;
                                    item.expectedValue = "Expected: " + Operand + item.expectedValueLowInt;
                                }
                                else if (item.dataType == "text")
                                {
                                    item.actualValue = "Actual: " + item.actualValueText;
                                    item.expectedValue = "Expected: " + Operand + item.expectedValueLowVarchar;
                                }
                                else if (item.dataType == "percent")
                                {
                                    item.actualValue = "Actual: " + item.actualValuePercent;
                                    item.expectedValue = "Expected: " + Operand + item.expectedValueLowPercent;
                                }
                                else if (item.dataType == "decimal")
                                {
                                    item.actualValue = "Actual: " + item.actualValueDecimal;
                                    item.expectedValue = "Expected: " + Operand + item.expectedValueLowDecimal;
                                }
                                else
                                {
                                    item.actualValue = "Actual: " + item.actualValueDate;
                                    item.expectedValue = "Expected: " + Operand + item.expectedValueLowDate;
                                }
                                QuestionData.Add(item);
                            }
                        }
                        else
                        {
                            isPlusVisible = false;
                            IsStackExpanded = false;
                            IsExpanderActive = false;
                            isQuestionAvailable = false;
                        }

                        if (obj.isSubjectTo == null || obj.isSubjectTo == "false")
                        {
                            isSubjectToVisible = false;
                        }
                        else
                        {
                            isSubjectToVisible = true;
                            subjectTo = obj.subjectTo;
                        }

                        if (obj.isMitigatingFactors == null || obj.isMitigatingFactors == "false")
                        {
                            isMitigatingVisible = false;
                        }
                        else
                        {
                            isMitigatingVisible = true;
                            MitigatingFactors = obj.mitigatingFactors;
                        }

                        if (obj.comment == null)
                        {
                            commnetsVisible = false;
                        }
                        else
                        {
                            commnetsVisible = true;
                            comments = obj.comment;
                        }

                        if (obj.lastMessage == null && obj.lastPostBy == null && obj.lastPostDate == null)
                        {
                            sendMessageVisible = false;
                        }
                        else
                        {
                            sendMessageVisible = true;
                            senderName = obj.lastPostBy;
                            SubjectDec = obj.lastMessage;
                            sendingDate = DateTime.Parse(obj.lastPostDate, new CultureInfo("en-US", true));
                        }

                        needsExceptionActions = "false";
                        primaryDocumentException = "false";
                        if (obj.exception == "true" || (obj.requireDocument == true && obj.missingDocument == true))
                        {
                            needsExceptionActions = "true";
                            if (obj.requireDocument == true && obj.missingDocument == true)
                                primaryDocumentException = "true";
                        }
                        else if (obj.showCompareFlag == "1" && obj.actualValue == null)
                            needsExceptionActions = null;
                        if (obj.exception != null && obj.exceptionFlag == "false" && primaryDocumentException == "false")
                            needsExceptionActions = "false";
                        GetPendingApprovalJSON();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return;
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        public ICommand MessageFrameClick { get { return new Command(MessageFrameClicked); } }
        private async void MessageFrameClicked()
        {
            try
            {
                Application.Current.Properties["THREADID"] = threadId;
                Application.Current.Properties["PARTYNAME"] = Settings.PartyName;
                Application.Current.Properties["TOPIC"] = threadTitle;
                //await Application.Current.SavePropertiesAsync();
                await Application.Current.MainPage.Navigation.PushAsync(new MessageThreadViewPage(), false);

                //_helper.NavigateToItemDetail(new MessageThreadViewPage());
            }
            catch (Exception ex)
            {
                ShowExceptionAlert(ex);
            }
        }

        //public ICommand ExpandCollapseTapped { get { return new Command(ExpandCollapse_click); } }
        //private void ExpandCollapse_click()
        //{
        //    if(isQuestionAvailable == true)
        //    {
        //        if (plusImage.Source.Equals("plus.png"))
        //        {
        //            plusImage.Source = "minus.png";
        //            isQuestionListVisible = true;
        //        }
        //        else
        //        {
        //            plusImage.Source = "plus.png";
        //            isQuestionListVisible = false;
        //        }
        //    }
        //    else
        //    {
        //        isQuestionListVisible = false;
        //    }
        //}

        public async void GetPendingApprovalJSON()
        {
            //Check network status   
            if (NetworkCheck.IsInternet())
            {
                var httpClient = new System.Net.Http.HttpClient();
                string url = Config.APPROVAL_PENDING_URL + initiateProcessIdTxt + '/' + statusTxt + '/' + userId + '/' + needsExceptionActions;
                var response = await httpClient.GetAsync(url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    // Device.BeginInvokeOnMainThread(async () =>
                    // {
                    // await DisplayAlert("No Record!!", string.Format("Response contained status code: {0}", response.StatusCode), "OK");
                    // });
                }
                else
                {
                    var assignJson = await response.Content.ReadAsStringAsync();
                    if (assignJson != "[]")
                    {
                        PendingList = ApprovalPendingModel.FromJson(assignJson);
                        PendingListVisible = true;
                        isSaveButtonVisible = true;
                        Action = true;
                        ActionDropDown = true;
                    }
                    else
                    {
                        Action = false;
                        ActionDropDown = false;
                        PendingListVisible = false;
                        isSaveButtonVisible = false;
                    }
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Simon", "No network is available.", "OK");
            }
        }

        public ICommand ClosePopup_Command { get { return new Command(ClosePopup_click); } }
        private async void ClosePopup_click()
        {
            await ClosePopup();
        }
    }
}

