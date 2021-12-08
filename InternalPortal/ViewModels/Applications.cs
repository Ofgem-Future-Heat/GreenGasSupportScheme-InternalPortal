using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Extensions;
using InternalPortal.Services;

namespace InternalPortal.ViewModels
{
    public class Applications
    {
        public List<Application> StageOneSubmitted { get; internal set; }

        public List<Application> StageTwoSubmitted { get; internal set; }

        public async Task<Applications> Get(IGetApplicationsService getApplicationsService, CancellationToken token)
        {
            var result = new Applications()
            {
                StageOneSubmitted = new List<Application>(),
                StageTwoSubmitted = new List<Application>()
            };

            var applications = await getApplicationsService.Get(token);

            if (!applications.Errors.Any())
            {
                var results = applications.Applications
                    .Select(o => new Application()
                    {
                        Id = o.ApplicationId,
                        Name = o.OrganisationName,
                        Status = o.ApplicationStatusDisplayName,
                        LastModified = o.LastModified.ToOfgemShortDate(),
                        Reference = o.Reference
                    });

                result.StageOneSubmitted = results.Where(a => 
                        a.Status == "Stage One Submitted" ||
                        a.Status == "Stage One Approved" ||
                        a.Status == "Rejected")
                    .ToList();

                result.StageTwoSubmitted = results.Where(a => 
                    a.Status == "Stage Two Submitted" ||
                    a.Status == "Stage Two Approved" ||
                    a.Status == "rejected")
                    .ToList();
            }

            return result;
        }
    }

    public class Application
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string LastModified { get; set; }
        public string Reference { get; set; }
    }
}
