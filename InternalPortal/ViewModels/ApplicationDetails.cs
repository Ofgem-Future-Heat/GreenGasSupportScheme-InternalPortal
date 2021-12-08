using System;
using InternalPortal.Services;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace InternalPortal.ViewModels
{
    public class ApplicationDetails
    {
        public string ApplicationId { get; set; }
        public string Reference { get; set; }
        public Enum Status { get; set; }
        public StageOneDetails StageOneDetails { get; set; }
        public StageTwoDetails StageTwoDetails { get; set; }
        public OrganisationDetails OrganisationDetails { get; set; }
    }
}