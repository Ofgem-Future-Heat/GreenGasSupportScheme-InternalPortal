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
using Ofgem.API.GGSS.Domain.ModelValues.StageTwo;
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

            var stageOneApplication = application.Application.StageOne;

            var stageTwoApplication = application.Application.StageTwo;

            var hasPostcode = String.IsNullOrEmpty(stageOneApplication.TellUsAboutYourSite.HasPostcode)
                ? "Yes"
                : stageOneApplication.TellUsAboutYourSite.HasPostcode;

            var stageTwoHasFirstSubmissionDateTime = !String.IsNullOrEmpty(stageTwoApplication.FirstSubmissionDateTime);

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
                    Location = stageOneApplication.TellUsAboutYourSite.PlantLocation,
                    PlantName = stageOneApplication.TellUsAboutYourSite.PlantName,
                    HasPostcode = hasPostcode,
                    LatitudeLongitudeAnaerobic = stageOneApplication.TellUsAboutYourSite.LatitudeLongitudeAnaerobic,
                    LatitudeLongitudeInjection = stageOneApplication.TellUsAboutYourSite.LatitudeLongitudeInjection,
                    ConnectionAgreement = stageOneApplication.TellUsAboutYourSite.CapacityCheckDocument,
                    SiteAddress = stageOneApplication.TellUsAboutYourSite.PlantAddress,
                    InjectionPointAddress = stageOneApplication.TellUsAboutYourSite.InjectionPointAddress,
                    EquipmentDescription = stageOneApplication.TellUsAboutYourSite.EquipmentDescription,
                    PlanningOutcome = stageOneApplication.ProvidePlanningPermission.PlanningPermissionOutcome,
                    PlanningUpload = stageOneApplication.ProvidePlanningPermission.PlanningPermissionDocument,
                    PlanningPermissionStatement = stageOneApplication.ProvidePlanningPermission.PlanningPermissionStatement,
                    BiomethaneVolume = stageOneApplication.ProductionDetails.MaximumInitialCapacity,
                    EligibleBiomethane = stageOneApplication.ProductionDetails.EligibleBiomethane,
                    ExpectedStartDate = stageOneApplication.ProductionDetails.InjectionStartDate,
                    FirstSubmissionDateTime = stageOneApplication.FirstSubmissionDateTime.ToOfgemShortDate()
                },
                StageTwoDetails = new StageTwoDetails()
                {
                    Isae3000 = stageTwoApplication.Isae3000.Document,
                    AdditionalSupportingEvidenceDocuments = stageTwoApplication.AdditionalSupportingEvidence.AdditionalSupportingEvidenceDocuments,
                    FirstSubmissionDateTime = stageTwoHasFirstSubmissionDateTime ? stageTwoApplication.FirstSubmissionDateTime.ToOfgemShortDate() : ""

                },
                OrganisationDetails = new OrganisationDetails()
                {
                    OrganisationName = organisation.OrganisationName,
                    OrganisationType = organisation.OrganisationType,
                    OrganisationStatus = organisation.OrganisationStatus,
                    OrganisationRegistrationNumber = organisation.OrganisationRegistrationNumber,
                    OrganisationAddress = organisation.OrganisationAddress,
                    OrganisationUsers = organisation.OrganisationUsers,
                    ResponsiblePersonName = organisation.ResponsiblePersonName,
                    ResponsiblePersonSurname = organisation.ResponsiblePersonSurname,
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
            
            application.Application.Status = Enum.Parse<ApplicationStatus>(status);

            if (application.Application.Status == ApplicationStatus.StageOneWithApplicant ||
                application.Application.Status == ApplicationStatus.StageTwoWithApplicant ||
                application.Application.Status == ApplicationStatus.StageThreeWithApplicant)
            {
                SetApplicationStatusesToComplete(application);
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

        private static void SetApplicationStatusesToComplete(RetrieveApplicationResponse application)
        {
            if (application.Application.StageTwo.Isae3000.Status == "Submitted")
            {
                application.Application.StageTwo.Isae3000.Status = "Completed";
                application.Application.StageTwo.AdditionalSupportingEvidence.Status = "Completed";
            }
            else
            {
                application.Application.StageOne.TellUsAboutYourSite.Status = "Completed";
                application.Application.StageOne.ProvidePlanningPermission.Status = "Completed";
                application.Application.StageOne.ProductionDetails.Status = "Completed";
            }
        }
    }
}
