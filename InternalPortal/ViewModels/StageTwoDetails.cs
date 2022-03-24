using System.Collections.Generic;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace InternalPortal.ViewModels
{
    public class StageTwoDetails
    {
        public DocumentValue Isae3000 { get; set; }
        public List<DocumentValue> AdditionalSupportingEvidenceDocuments { get; set; }
        public string FirstSubmissionDateTime { get; set; }
    }
}