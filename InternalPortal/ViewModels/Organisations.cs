using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Extensions;
using InternalPortal.Services;

namespace InternalPortal.ViewModels
{
    public class Organisations
    {
        public List<Organisation> List { get; internal set; }

        public async Task<Organisations> Get(IGetOrganisationsService getOrganisationsService, CancellationToken token)
        {
            var result = new Organisations() { List = new List<Organisation>() };

            var organisations = await getOrganisationsService.Get(token);

            if (!organisations.Errors.Any())
            {
                result.List = organisations.Organisations
                    .Select(o => new Organisation()
                    {
                        Id = o.OrganisationId,
                        Name = o.OrganisationName,
                        Status = o.OrganisationStatus,
                        LastModified = o.LastModified.ToOfgemShortDate()
                    })
                    .ToList();
            }

            return result;
        }
    }

    public class Organisation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string LastModified { get; set; }
    }
}