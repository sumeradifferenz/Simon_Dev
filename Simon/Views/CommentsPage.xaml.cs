using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Simon.Helpers;
using Simon.Models;
using Simon.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Config = Simon.Helpers.Config;
using Picker = Xamarin.Forms.Picker;

namespace Simon.Views
{
    public partial class CommentsPage : ContentPage
    {
        CommentsInfoModel ObjCommentList = new CommentsInfoModel();
        CommentsPageViewModel vm = null;
        IEnumerable<ApprovalPendingModel> result;
        IEnumerable<ProcessStageUserModel> resultUserList;
        JObject jObject = null;
        int requirementId;
        string dealId;
        string userId, isStatusString, statusUpdatedTxt, shortMobileNameTxt, notifyDefinitionTxt, processStageFunctionIdTxt, functionIdTxt, typeUpdatedTxt;

        DateTime currentDateTime = DateTime.Now;
        JObject oJsonObject = new JObject();

        public CommentsPage(ApprovalMainModel item)
        {
            InitializeComponent();

            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("USERID"))
            {
                userId = Convert.ToString(Xamarin.Forms.Application.Current.Properties["USERID"]);
            }
            requirementId = item.requirementid_05;
            dealIdTxt.Text = item.dealid_05;
            dealId = item.dealid_05;
            partyNamelblTxt.Text = item.partyname_10;
            productNameLbl.Text = item.productdesc_10;

            reqName_10.Text = item.reqname_10;
            double numba = item.amount_06;
            string s = numba.ToString("C0");
            isStatusString = item.approvaltype;
            amountLbl.Text = s;
            Settings.PartyName = item.partyname_10;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            vm = new CommentsPageViewModel();
            this.BindingContext = vm;
            if (NetworkCheck.IsInternet())
            {
                await vm.FetchCommnetData(requirementId);
            }
            else
            {
                await DisplayAlert("Simon", "No network is available.", "OK");
            }
        }
        
        public async Task postUpdatedMessage()
        {
            try
            {
                await vm.ShowLoader();
                if (App.buttonClick == 0)
                {
                    App.buttonClick++;
                    
                    var client = new HttpClient();
                    HttpClient hc = new HttpClient();
                    var response1 = await hc.GetStringAsync(Config.COMMENT_INFO_URL + requirementId);
                    var obj = JsonConvert.DeserializeObject<CommentsInfoModel>(response1);

                    //if (userListPicker.SelectedIndex != -1)
                    //{
                    var values = new Dictionary<object, object>
                    {
                        { "lastMessage", obj.lastMessage},
                        { "lastPostDate", obj.lastPostDate },
                        { "lastPostBy", obj.lastPostBy },
                        { "requirementId",obj.requirementId },
                        { "reqDefinitionId",obj.reqDefinitionId },
                        { "reqDealId",obj.reqDealId },
                        { "requirementDealId", obj.requirementDealId },
                        { "documentDefinitionId",obj.documentDefinitionId},
                        { "documentDefinitionName",obj.documentDefinitionName},
                        { "documentId",obj.documentId},
                        { "primaryDocumentName",obj.primaryDocumentName},
                        { "statusDescription",obj.statusDescription},
                        { "status", statusUpdatedTxt },
                        { "description",obj.description},
                        { "name",obj.name},
                        { "type",obj.type },
                        { "reqType",obj.reqType},
                        { "typeCode",obj.typeCode },
                        { "valueLabel",obj.valueLabel},
                        { "compareFlag",obj.compareFlag},
                        { "showCompareFlag",obj.showCompareFlag },
                        { "expectedValue",obj.expectedValue },
                        { "actualValue",obj.actualValue },
                        { "actualValueEnabled",obj.actualValueEnabled },
                        { "active",obj.active },
                        { "manualRequirement",obj.manualRequirement },
                        { "dueDate", obj.dueDate },
                        { "asOfDate", obj.asOfDate },
                        { "expirationDate", obj.expirationDate },
                        { "sourceCode",obj.sourceCode },
                        { "source",obj.source },
                        { "entityType",obj.entityType },
                        { "initiateProcessId", obj.initiateProcessId },
                        { "processHistoryId",obj.processHistoryId },
                        { "processId", obj.processId },
                        { "stageId",obj.stageId },
                        { "functionId",functionIdTxt },
                        { "processStageFunctionId",processStageFunctionIdTxt },
                        { "deferStageName",obj.deferStageName },
                        { "stageName",obj.stageName},
                        { "stageStatus", obj.stageStatus },
                        { "assignedTo",obj.assignedTo },
                        { "assignedToFullName",obj.assignedToFullName },
                        { "createdBy",obj.createdBy },
                        { "deferToStage",obj.deferToStage },
                        { "comment", obj.comment },
                        { "compareDataType",obj.compareDataType},
                        { "compareFunctionName",obj.compareFunctionName},
                        { "exception",obj.exception },
                        { "complete",obj.complete },
                        { "daysDue",obj.daysDue },
                        { "group",obj.group},
                        { "requireDocument", obj.requireDocument },
                        { "missingDocument", obj.missingDocument },
                        { "requireAsOfDate", obj.requireAsOfDate },
                        { "expirationPeriodDays",obj.expirationPeriodDays },
                        { "priority", obj.priority },
                        { "exceptionFlag",obj.exceptionFlag },
                        { "mitigatingFactors", obj.mitigatingFactors },
                        { "stageType", obj.stageType},
                        { "isAttention", obj.isAttention },
                        { "attentionType", obj.attentionType },
                        { "actionType", typeUpdatedTxt },
                        { "createdDate", obj.createdDate },
                        { "lastModBy",userId },
                        { "lastModByDate",currentDateTime },
                        { "apReqNeededforclosing", obj.apReqNeededforclosing },
                        { "coProcessNeededforclosing",obj.coProcessNeededforclosing },
                        { "isSubjectTo", obj.isSubjectTo },
                        { "isMitigatingFactors",obj.isMitigatingFactors },
                        { "subjectTo", obj.subjectTo },
                        { "loanRequestType", obj.loanRequestType },
                        { "dealId", obj.dealId },
                        { "isSaveComplete",obj.isSaveComplete },
                        { "lastModbyFirstname",obj.lastModbyFirstname},
                        { "lastModbyLastname",obj.lastModbyLastname },
                        { "documentCount",obj.documentCount },
                        { "hardstop",obj.hardstop },
                        { "statusType",obj.statusType },
                        { "frequentlyUsed",obj.frequentlyUsed },
                        { "isTickler",obj.isTickler },
                        { "threadId",obj.threadId },
                    };

                    string url = Config.POST_UPDATED_COMMENTS + notifyDefinitionTxt;
                    var content1 = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content1);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("Data Not Sent!!", string.Format("Response contained status code: {0}", response.StatusCode), "OK");
                        });
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine(content);
                        await ShowMessage("Simon", content, "OK", async () =>
                        {
                            await Navigation.PopToRootAsync();
                        });
                        //await DisplayAlert("Simon", content, "OK", "Cancel");
                        // the call is awaited
                        //// the execution is stopped here, the next line won't be executed until the user chooses Yes or No
                        //if (yesSelected)  // No compile error, as the result will be bool, since we awaited the Task<bool>
                        //{
                        //   await Navigation.PopToRootAsync();
                        //}
                        //else
                        //{ return; }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                await BaseViewModel.ClosePopup();
            }
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            await DisplayAlert( title, message, buttonText);
            afterHideCallback?.Invoke();
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            if (picker.SelectedItem == null)
            {
                return;
            }
            ApprovalPendingModel selectedPickerItem = picker.SelectedItem as ApprovalPendingModel;
            functionIdTxt = selectedPickerItem.functionId;
            shortMobileNameTxt = selectedPickerItem.shortMobileName;
            typeUpdatedTxt = selectedPickerItem.type;
            notifyDefinitionTxt = selectedPickerItem.notifyDefinition;
            processStageFunctionIdTxt = selectedPickerItem.processStageFunctionId;
            statusUpdatedTxt = selectedPickerItem.id;
            if (typeUpdatedTxt.Equals("U") || typeUpdatedTxt.Equals("u"))
            {
                vm.ProcessUserListVisible = true;
                processStageUserPicker.Title = vm.assignedToFullNameTxt;
                GetProcessStageUserList();
            }
            else
            {
                vm.ProcessUserListVisible = false;
            }
        }
        protected override bool OnBackButtonPressed()
        {
            this.vm.Back();
            return true;
        }
        public async void GetProcessStageUserList()
        {
            if (NetworkCheck.IsInternet())
            {
                var httpClient = new System.Net.Http.HttpClient();
                string url = Config.PROCESS_STAGE_FUNCTION_USER_API + vm.processIdTxt + '/' + vm.stageIdTxt + '/' + functionIdTxt + '/' + userId;
                var response = await httpClient.GetAsync(url);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("No Record!!", string.Format("Response contained status code: {0}", response.StatusCode), "OK");
                    });
                }
                else
                {
                    var assignJson = await response.Content.ReadAsStringAsync();
                    if (assignJson != "")
                    {
                        resultUserList = JsonConvert.DeserializeObject<IEnumerable<ProcessStageUserModel>>(assignJson);
                        foreach (var Data in resultUserList)
                        {
                            var lastPostDateStr = Data.userId;
                        }
                        processStageUserPicker.ItemsSource = (System.Collections.IList)resultUserList;
                    }
                }
            }
            else
            {
                await DisplayAlert("JSONParsing", "No network is available.", "OK");
            }
        }

        public async void onApproveBtnClicked(object sender, EventArgs e)
        {
            await postUpdatedMessage();
        }

        private async void btnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(false);
        }
        public async void onCancelBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(false);
        }
        public void onMitigatingEditBtnClicked(object sender, EventArgs e)
        {
            //entryMitigatingFactor.IsReadOnly = false;
        }
        //public void onCommentEditClicked(object sender, EventArgs e)
        //{
        //    if (commentTxt != "")
        //    {
        //        //entryCommentTxt.IsReadOnly = true;
        //        //EditorCommentTxt.IsVisible = true;
        //        //frameECommenttxt.IsVisible = true;
        //    }
        //    else
        //    {
        //        //entryCommentTxt.IsReadOnly = false;
        //        //EditorCommentTxt.HeightRequest = 0;
        //        //EditorCommentTxt.IsVisible = false;
        //        //frameECommenttxt.HeightRequest = 0;
        //        //frameECommenttxt.IsVisible = false;
        //    }

        //}
        public void onApprovalEditBtnClicked(object sender, EventArgs e)
        {
            //entryApproval.IsReadOnly = false;
        }

        void DropDownButton_Clicked(System.Object sender, System.EventArgs e)
        {
            userListPicker.Focus();
        }
    }
}


//{
//	"status": "sApproved2ndLO",
//    "functionId":"fsDealApproval",
//    "processStageFunctionId" : "1561",
//    "actionType" : null,
//    "lastModBy": "tjohns71",
//    "lastModByDate" : "6/12/2020 4:03:09 PM"
//}