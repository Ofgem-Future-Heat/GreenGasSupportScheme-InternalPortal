using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Models;
using InternalPortal.Services;
using InternalPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternalPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetOrganisationsService _getOrganisationsService;
        private readonly IGetApplicationsService _getApplicationsService;

        public HomeController(
            ILogger<HomeController> logger,
            IGetOrganisationsService getOrganisationsService,
            IGetApplicationsService getApplicationsService
            )
        {
            _logger = logger;
            _getOrganisationsService = getOrganisationsService;
            _getApplicationsService = getApplicationsService;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Dashboard Index action result called");

            var dashboard = new Dashboard(_getOrganisationsService, _getApplicationsService);

            await dashboard.Initialise(CancellationToken.None);

            return View(nameof(Index), dashboard);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/MicrosoftIdentity/Account/AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View(nameof(AccessDenied));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}