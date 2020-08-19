using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Simon.Models
{
    //public partial class CommentsInfoModel
    //{ 
    //    public string id { get; set; }
    //    public string comment { get; set; }
    //    public string isMitigatingFactors { get; set; }
    //    public string mitigatingFactors { get; set; }
    //    public string dueDate { get; set; }
    //    public string lastModBy { get; set; }
    //    public string subjectTo { get; set; }
    //    public string isSubjectTo { get; set; }
    //    public string initiateProcessId { get; set; }
    //    public string status { get; set; }
    //    public double amount_06 { get; set; }
    //}

    //public partial class CommentsInfoModel
    //{
    //    public static ObservableCollection<CommentsInfoModel> FromJson(string json) => JsonConvert.DeserializeObject<ObservableCollection<CommentsInfoModel>>(json);
    //}

    public class documents
    {
        public int id { get; set; }
        public int documentDefinitionId { get; set; }
        public string documentDefinitionName { get; set; }
        public string file { get; set; }
        public string name { get; set; }
        public string alternateName { get; set; }
        public string description { get; set; }
        public string recievedDate { get; set; }
        public string asOfDate { get; set; }
        public string expirationDate { get; set; }
        public string uploadDate { get; set; }
        public bool primary { get; set; }
        public string notes { get; set; }
        public bool isIIS { get; set; }
        public string dealId { get; set; }
        public string requirementId { get; set; }
        public string mimeType { get; set; }
        public string fileGuid { get; set; }
        public int requirementAttachmentCount { get; set; }
        public string attachedRequirements { get; set; }
        public string createdBy { get; set; }
        public string createdDate { get; set; }
        public string lastModBy { get; set; }
        public string lastModByDate { get; set; }
        public string ecmExportReady { get; set; }
        public string ecmExported { get; set; }
        public string ecmCabinetCode { get; set; }
        public string ecmFileName { get; set; }
        public string ecmExportedBy { get; set; }
        public string ecmExportedDate { get; set; }
        public string ecmUploadEnabled { get; set; }
        public string active { get; set; }
        public string setAsOfDate { get; set; }
        public string entityIds { get; set; }
        public string entityType { get; set; }
        public string requirementAttachDate { get; set; }
        public string docDefinitionCode { get; set; }
    }

    public class reqQuestionDefinitions
    {
        public int reqQuestionDefinitionId { get; set; }
        public string reqDefinitionId { get; set; }
        public string reqQuestionResultId { get; set; }
        public string actualValueDate { get; set; }
        public string actualValueBit { get; set; }
        public string actualValueText { get; set; }
        public string actualValueInt { get; set; }
        public string actualValuePercent { get; set; }
        public string actualValueDecimal { get; set; }
        public string questionDefinitionType { get; set; }
        public string descriptionNotes { get; set; }
        public string comment { get; set; }
        public string questionName { get; set; }
        public string questionText { get; set; }
        public string dataType { get; set; }
        public string showHighExceptionFlag { get; set; }
        public bool showLowExceptionFlag { get; set; }
        public string expectedValueHigh { get; set; }
        public string expectedValueLow { get; set; }
        public string expectedValueLowInt { get; set; }
        public string expectedValueLowBit { get; set; }
        public string expectedValueLowOperand { get; set; }
        public string expectedValueHighOperand { get; set; }
        public string expectedValueLowDecimal { get; set; }
        public string expectedValueLowVarchar { get; set; }
        public string expectedValueLowDate { get; set; }
        public string expectedValueHighInt { get; set; }
        public string expectedValueHighBit { get; set; }
        public string expectedValueHighDecimal { get; set; }
        public string expectedValueHighPercent { get; set; }
        public string expectedValueLowPercent { get; set; }
        public string expectedValueHighVarchar { get; set; }
        public string expectedValueHighDate { get; set; }
        public string lowValueExceptionAction { get; set; }
        public string highValueExceptionAction { get; set; }
        public bool exceptionLowIsException { get; set; }
        public string exceptionHighIsException { get; set; }
        public string isQuestionException { get; set; }
        public string isException { get; set; }
        public string createdBy { get; set; }

        public string actualValue { get; set;}
        public string expectedValue { get; set; }
    }

    public class CommentsInfoModel
    {
        public string lastMessage { get; set; }
        public string lastPostDate { get; set; }
        public string lastPostBy { get; set; }
        public int? requirementId { get; set; }
        public int? reqDefinitionId { get; set; }
        public int? reqDealId { get; set; }
        public int? requirementDealId { get; set; }
        public string documentDefinitionId { get; set; }
        public string documentDefinitionName { get; set; }
        public List<documents> documents { get; set; }
        public int? documentId { get; set; }
        public string primaryDocumentName { get; set; }
        public string statusDescription { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string reqType { get; set; }
        public string typeCode { get; set; }
        public string valueLabel { get; set; }
        public string compareFlag { get; set; }
        public string showCompareFlag { get; set; }
        public string expectedValue { get; set; }
        public string actualValue { get; set; }
        public bool actualValueEnabled { get; set; }
        public bool active { get; set; }
        public string manualRequirement { get; set; }
        public string dueDate { get; set; }
        public string asOfDate { get; set; }
        public string expirationDate { get; set; }
        public string sourceCode { get; set; }
        public string source { get; set; }
        public string entityType { get; set; }
        public string initiateProcessId { get; set; }
        public int? processHistoryId { get; set; }
        public string processId { get; set; }
        public string stageId { get; set; }
        public string functionId { get; set; }
        public string processStageFunctionId { get; set; }
        public string deferStageName { get; set; }
        public string stageName { get; set; }
        public string stageStatus { get; set; }
        public string assignedTo { get; set; }
        public string assignedToFullName { get; set; }
        public string createdBy { get; set; }
        public string deferToStage { get; set; }
        public string comment { get; set; }
        public string compareDataType { get; set; }
        public string compareFunctionName { get; set; }
        public string exception { get; set; }
        public string complete { get; set; }
        public long daysDue { get; set; }
        public string group { get; set; }
        public bool requireDocument { get; set; }
        public bool missingDocument { get; set; }
        public string requireAsOfDate { get; set; }
        public string expirationPeriodDays { get; set; }
        public int? priority { get; set; }
        public string exceptionFlag { get; set; }
        public string mitigatingFactors { get; set; }
        public string stageType { get; set; }
        public string isAttention { get; set; }
        public string attentionType { get; set; }
        public string actionType { get; set; }
        public string createdDate { get; set; }
        public string lastModBy { get; set; }
        public DateTime lastModByDate { get; set; }
        public string apReqNeededforclosing { get; set; }
        public string coProcessNeededforclosing { get; set; }
        public string isSubjectTo { get; set; }
        public string isMitigatingFactors { get; set; }
        public string subjectTo { get; set; }
        public string loanRequestType { get; set; }
        public int? dealId { get; set; }
        public string isSaveComplete { get; set; }
        public string lastModbyFirstname { get; set; }
        public string lastModbyLastname { get; set; }
        public int? documentCount { get; set; }
        public string hardstop { get; set; }
        public string statusType { get; set; }
        public string frequentlyUsed { get; set; }
        public string isTickler { get; set; }
        public string threadId { get; set; }
        public string threadTitle { get; set; }
        public string primaryUserID { get; set; }
        public string primaryOfficer { get; set; }
        public List<reqQuestionDefinitions> reqQuestionDefinitions { get; set; }
    }


}

