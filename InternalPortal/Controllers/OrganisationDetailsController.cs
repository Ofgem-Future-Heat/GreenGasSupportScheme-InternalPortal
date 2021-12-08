using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Services;
using InternalPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Controllers
{
    public class OrganisationDetailsController : Controller
    {
        private readonly IGetOrganisationDetailsService _getOrganisationDetailsService;
        private readonly IUpdateOrganisationStatusService _updateOrganisationStatusService;

        public OrganisationDetailsController(IGetOrganisationDetailsService getOrganisationDetailsService, IUpdateOrganisationStatusService updateOrganisationStatusService)
        {
            _getOrganisationDetailsService = getOrganisationDetailsService;
            _updateOrganisationStatusService = updateOrganisationStatusService;
        }
        
        [HttpGet]
        [Route("/organisation-details/{organisationId}")]
        public async Task<IActionResult> OrganisationDetails(string organisationId)
        {
            var organisationDetails = await _getOrganisationDetailsService.Get(new GetOrganisationDetailsRequest()
            {
                OrganisationId = organisationId
            }, CancellationToken.None);

            var model = new OrganisationDetails()
            {
                OrganisationType = organisationDetails.OrganisationType,
                OrganisationName = organisationDetails.OrganisationName,
                OrganisationStatus = organisationDetails.OrganisationStatus,
                OrganisationRegistrationNumber = organisationDetails.OrganisationRegistrationNumber,
                OrganisationAddress = organisationDetails.OrganisationAddress,
                ResponsiblePersonName = organisationDetails.ResponsiblePersonName,
                ResponsiblePersonPhoneNumber = organisationDetails.ResponsiblePersonPhoneNumber,
                ResponsiblePersonEmail = organisationDetails.ResponsiblePersonEmail,
                LegalDocument = organisationDetails.LegalDocument,
                PhotoId = organisationDetails.PhotoId,
                LetterOfAuthority = organisationDetails.LetterOfAuthority,
                ProofOfAddress = organisationDetails.ProofOfAddress
            };
            return View("OrganisationDetails", model);
        }

        [HttpPost]
        [Route("/organisation-details/{organisationId}")]
        public async Task<IActionResult> UpdateOrganisationDetails(string organisationId, string organisationStatus)
        {
            await _updateOrganisationStatusService.UpdateStatus(new UpdateOrganisationStatus()
            {
                OrganisationId = organisationId,
                NewStatus = organisationStatus
            });

            return RedirectToAction("OrganisationDetails", new { organisationId });
        }
    }
}