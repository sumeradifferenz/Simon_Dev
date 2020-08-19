using System;

using Xamarin.Forms;

namespace Simon.Helpers
{
    public class Constants
    {
        public const string AuthStateKey = "authState";
        public const string AuthServiceDiscoveryKey = "authServiceDiscovery";

        public const string ClientId = "0oaj9pg2yFiLOl8mu356";
        public const string RedirectUri = "com.okta.dev-377260:/callback";
        public const string OrgUrl = "https://dev-377260.okta.com";

        public const string AuthorizationServerId = "default";
        public const string Prompt = "None";
        public static readonly string DiscoveryEndpoint =
            $"{OrgUrl}/oauth2/{AuthorizationServerId}/.well-known/openid-configuration";


        public static readonly string[] Scopes = new string[] {
            "openid", "profile", "email", "offline_access" };


        public const string MessageScreenTitle = "Messages";
        public const string HomeScreenTitle = "Home";
        public const string DealsScreenTitle = "Deals";
        public const string ApprovalScreenTitle = "Approve";
        public const string ApprovalRequestScreenTitle = "Approval Requests";

        //Anjali
        //Lable title 
        public const string LoginText = "Login";
        public const string LogindetailText = "Enter your credentials to connect.";
        public const string SelectUsernameText = "Select Username";
        public const string SelectUsernamePlaceholdertxt = "Select";
        public const string PasswordText = "Password";
        public const string OpenTabText = "Open";
        public const string RecentTabText = "Recent";
        public const string WelcomeText = "Welcome";
        public const string UpcomingText = "Upcoming";
        public const string ClosingsTabText = "Closings";
        public const string DecisionDueTabText = "Decision Due";
        public const string SortBylblText = "Sort By";
        public const string FilterBylblText = "Filter By";
        public const string DeletealertlblText = "Are you sure you want to delete partcipant from list ?";
        public const string DeletePartcipantlblText = "Delete Participant";
        public const string BorrowerlblText = "Borrower";
        public const string LastPostDatelblText = "Last Post Date";
        public const string AmountlblText = "Amount";
        public const string DueDatelblText = "Due Date";
        public const string ClosingDatelblText = "Closing Date";
        public const string ParticipantsTabText = "Participants";
        public const string ReadlblText = "Read Messages";
        public const string UnReadlblText = "UnRead Messages";
        public const string RealAllText = "All";
        public const string ApprovalHeaderlblText = "Approval";
        public const string OfficerlblText = "Officer";
        public const string SubmittedBylblText = "Submitted By";
        public const string MitigatingFactorlblText = "Mitigating Factor";
        public const string ApprovalConditionsblText = "Conditions for Approval";
        public const string CommentslblText = "Comments";
        public const string ActionlblText = "Action";
        public const string SentBylblText = "Sent By : ";
        public const string ActuallblText = "Actual : ";
        public const string ExpectedlblText = "Expected : ";
        public const string ParticipantNotAvailablelblText = "Participants not available.";
        public const string MessagesNotAvailablelblText = "Messages are not Available.";
        public const string MessageslblText = "Messages";
        public const string DataNotAvailablelblText = "Data not Available.";
        public const string NoDataFoundText = "No Data Found.";
        public const string ApprovalsNeededlblText = "Approvals Needed";
        public const string DeallblText = "Deal"; 
        public const string ApprovalDatelblText = "Approval Request Date";
		public const string ClearlblText = "Clear";
        public const string LastPostDateText = "lastPostDate";
        public const string PartyNamelblText = "partyName";


        //Message for Empty List
        public const string NoneText = "None";

        //Button title
        public const string DoneBtnText = "Done";
        public const string ApplyBtnText = "Apply";
        public const string YesBtnText = "Yes";

        //img title
        public const string NameAcendingImg = "name_ascending.png";
        public const string NameDescendingImg = "name_decending.png";
        public const string NumberAcendingImg = "number_ascending.png";
        public const string NumberDescendingImg = "number_decending.png";
        public const string RadiobtnUnselectImg = "radio_unselect.png";
        public const string RadiobtnSelectImg = "radio_select.png";
        public const string CheckboxImg = "check_box.png";
        public const string CheckboxUnselectImg = "check_uncheck.png";
    }
}

