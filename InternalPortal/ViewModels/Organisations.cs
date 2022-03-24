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
        public List<Organisation> Verified { get; private set; }
        public List<Organisation> Unverified { get; private set; }

        public static async Task<Organisations> Get(IGetOrganisationsService getOrganisationsService, CancellationToken token)
        {
            var result = new Organisations { };

            var organisations = await getOrganisationsService.Get(token);

            if (organisations.Errors.Any())
            {
                return result;
            }

            var list = organisations.Organisations
                .Select(o => new Organisation()
                {
                    Id = o.OrganisationId,
                    Name = string.IsNullOrEmpty(o.OrganisationName) ? "Unnamed" : o.OrganisationName,
                    Status = string.IsNullOrEmpty(o.OrganisationStatus) ? "Not verified" : o.OrganisationStatus,
                    LastModified = o.LastModified.ToOfgemShortDate()
                })
                .ToList();

            result.Verified = list
                .Where(o => o.Status.ToLower() == "verified")
                .ToList();

            result.Unverified = list
                .Where(o => o.Status.ToLower() == "not verified")
                .ToList();

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