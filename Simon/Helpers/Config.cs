using System;

using Xamarin.Forms;

namespace Simon.Helpers
{
    public class Config : ContentPage
    {

        public static string BASE_URL_OLD = "http://cts-a-svr-sql-01.centralus.cloudapp.azure.com:8087/api";
        public static string BASE_URL = "http://cts-a-svr-sql-01.centralus.cloudapp.azure.com:8090/api";

        //Login Apis
        public static string LOGIN_API = BASE_URL + "/user/login";
        public static string USER_LIST_API = BASE_URL + "/participant/";

        //Dashboard Apis
        public static string CLOSING_API = BASE_URL + "/deal/GetClosingsListForMobile/";
        public static string DECISIONDUE_API = BASE_URL + "/deal/GetDecisionsDueForMobile/";

        //Approval Apis
        public static string APPROVAL_PENDING_URL = "http://cts-a-svr-sql-01.centralus.cloudapp.azure.com:8090/api/code/GetProcessStageFunction/";
        public static string APPROVAL_URL = "http://cts-a-svr-sql-01.centralus.cloudapp.azure.com:8090/api/inbox/GetApprovalForMobile/";
        public static string COMMENT_INFO_URL = "http://cts-a-svr-sql-01.centralus.cloudapp.azure.com:8090/api/requirement/getrequirementbyid/";
        public static string POST_COMMENT_INFO_URL = "http://cts-a-svr-sql-01.centralus.cloudapp.azure.com:8090/api/requirement/PostComments";
        public static string PROCESS_STAGE_FUNCTION_USER_API = "http://cts-a-svr-sql-01.centralus.cloudapp.azure.com:8090/api/code/getprocessstagefunctionuser/";
        public static string POST_UPDATED_COMMENTS = "http://cts-a-svr-sql-01.centralus.cloudapp.azure.com:8090/api/requirement/Post/";

        //Deal Apis
        public static string DEAL_DETAILS_API = BASE_URL + "/deal/GetDealsForMobile/";

        //Filter Api
        public static string FILTER_LIST_API = BASE_URL + "/inbox/GetDataByQuery?queryid=qStageFiltersMobile";

        //Message Apis
        public static string MESSAGES_API = BASE_URL + "/messaging/GetUserMessagesForMobile/";
        public static string MESSAGE_THREAD_API = BASE_URL + "/messaging/getthreadmessages/";
        public static string ADD_MESSAGES_THREAD_API = BASE_URL + "/messaging/getthreadusers/";
        public static string SAVE_MESSAGE_API = BASE_URL + "/messaging/savemessage/";
        public static string SYNC_PARTICIPANTS = BASE_URL + "/messaging/SynchParticipants/";
        public static string MARK_THREADMESSAGE_READ = BASE_URL + "/messaging/MarkThreadMessagesAsRead";
        public static string MARK_THREADMESSAGE_BOOKMARK = BASE_URL + "/messaging/PostFollowUp/";

        //Firebase api
        public static string SAVE_REGISTRATION_API = BASE_URL + "/user/SaveUserRegistration?userId=";

    }

}