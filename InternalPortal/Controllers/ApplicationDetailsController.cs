using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Extensions;
using InternalPortal.Services;
using InternalPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace InternalPortal.Controllers
{
    public class ApplicationDetailsController : Controller
    {
        private readonly IGetApplicationDetailsService _getApplicationDetailsService;
        private readonly IGetOrganisationDetailsService _getOrganisationDetailsService;
        private readonly IUpdateApplicationStatusService _updateApplicationStatusService;

        public ApplicationDetailsController(
            IGetApplicationDetailsService getApplicationDetailsService,
            IGetOrganisationDetailsService getOrganisationDetailsService,
            IUpdateApplicationStatusService updateApplicationStatusService)
        {
            _getApplicationDetailsService = getApplicationDetailsService;
            _getOrganisationDetailsService = getOrganisationDetailsService;
            _updateApplicationStatusService = updateApplicationStatusService;
        }
        
        [HttpGet]
        [Route("/application-details/{applicationId}")]
        public async Task<IActionResult> Index([FromRoute] string applicationId)
        {
            var application = await _getApplicationDetailsService.Get(new GetApplicationDetailsRequest()
            {
                ApplicationId = applicationId
            }, CancellationToken.None);

            var organisation = await _getOrganisationDetailsService.Get(new GetOrganisationDetailsRequest()
            {
                OrganisationId = application.OrganisationId.ToString()
            }, CancellationToken.None);
            
            var applicationDetails = new ApplicationDetails()
            {
                ApplicationId = applicationId,
                Reference = application.Application.Reference,
                Status = application.Application.Status,
                StageOneDetails = new StageOneDetails()
                {
                    Location = application.Application.StageOne.TellUsAboutYourSite.PlantLocation,
                    PlantName = application.Application.StageOne.TellUsAboutYourSite.PlantName,
                    ConnectionAgreement = application.Application.StageOne.TellUsAboutYourSite.CapacityCheckDocument,
                    SiteAddress = application.Application.StageOne.TellUsAboutYourSite.PlantAddress,
                    InjectionPointAddress = application.Application.StageOne.TellUsAboutYourSite.InjectionPointAddress,
                    EquipmentDescription = application.Application.StageOne.TellUsAboutYourSite.EquipmentDescription,
                    PlanningOutcome = application.Application.StageOne.ProvidePlanningPermission.PlanningPermissionOutcome,
                    PlanningUpload = application.Application.StageOne.ProvidePlanningPermission.PlanningPermissionDocument,
                    PlanningPermissionStatement = application.Application.StageOne.ProvidePlanningPermission.PlanningPermissionStatement,
                    BiomethaneVolume = application.Application.StageOne.ProductionDetails.MaximumInitialCapacity,
                    EligibleBiomethane = application.Application.StageOne.ProductionDetails.EligibleBiomethane,
                    ExpectedStartDate = application.Application.StageOne.ProductionDetails.InjectionStartDate,
                },
                StageTwoDetails = new StageTwoDetails()
                {
                    Isae3000 = application.Application.StageTwo.Isae3000.Document,
                    AdditionalSupportingEvidenceDocuments = application.Application.StageTwo.AdditionalSupportingEvidence.AdditionalSupportingEvidenceDocuments
                },
                OrganisationDetails = new OrganisationDetails()
                {
                    OrganisationName = organisation.OrganisationName,
                    OrganisationType = organisation.OrganisationType,
                    OrganisationStatus = organisation.OrganisationStatus,
                    OrganisationRegistrationNumber = organisation.OrganisationRegistrationNumber,
                    OrganisationAddress = organisation.OrganisationAddress,
                    ResponsiblePersonName = organisation.ResponsiblePersonName,
                    ResponsiblePersonPhoneNumber = organisation.ResponsiblePersonPhoneNumber,
                    ResponsiblePersonEmail = organisation.ResponsiblePersonEmail,
                    LegalDocument = organisation.LegalDocument,
                    PhotoId = organisation.PhotoId,
                    LetterOfAuthority = organisation.LetterOfAuthority,
                    ProofOfAddress = organisation.ProofOfAddress
                }
            };

            return View(nameof(Index), applicationDetails);
        }

        [HttpPost]
        [Route("/application-details/{applicationId}")]
        public async Task<IActionResult> Index(string applicationId, string status)
        {
            var application = await _getApplicationDetailsService.Get(new GetApplicationDetailsRequest()
            {
                ApplicationId = applicationId
            }, CancellationToken.None);
            
            Enum.TryParse(status, out ApplicationStatus newStatus);
            application.Application.Status = newStatus;

            if (status == "Draft")
            {
                SetApplicationStatusToComplete(application);
            }

            var request = new UpdateApplicationRequest
            {
                ApplicationId = applicationId,
                Application = application.Application,
                UserId = User.GetUserId()
            };
            
            await _updateApplicationStatusService.Update(request, CancellationToken.None);
            return RedirectToAction("Index");
        }

        private static void SetApplicationStatusToComplete(RetrieveApplicationResponse application)
        {
            application.Application.StageOne.TellUsAboutYourSite.Status = "Completed";
            application.Application.StageOne.ProvidePlanningPermission.Status = "Completed";
            application.Application.StageOne.ProductionDetails.Status = "Completed";
        }
    }
}
