using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Services;

namespace InternalPortal.ViewModels
{
    public class Dashboard
    {
        private readonly IGetOrganisationsService _getOrganisationsService;
        private readonly IGetApplicationsService _getApplicationsService;

        public Organisations Organisations { get; private set; }

        public Applications Applications { get; private set; }

        public Dashboard(
            IGetOrganisationsService getOrganisationsService,
            IGetApplicationsService getApplicationsService)
        {
            _getOrganisationsService = getOrganisationsService;
            _getApplicationsService = getApplicationsService;
        }

        public async Task Initialise(CancellationToken token)
        {
            Organisations = await Organisations.Get(_getOrganisationsService, token);

            Applications = await Applications.Get(_getApplicationsService, token);
        }
    }
}
