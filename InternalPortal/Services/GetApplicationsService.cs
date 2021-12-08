using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ofgem.API.GGSS.Domain.Enums;

namespace InternalPortal.Services
{
    public interface IGetApplicationsService
    {
        Task<RetrieveApplicationsResponse> Get(CancellationToken cancellationToken);
    }

    public class GetApplicationsService : IGetApplicationsService
    {
        private readonly HttpClient _client;

        public GetApplicationsService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMyNmZhOTc0LTdjMDUtNGIzNy1hOGU0LTZkNWZlNmRlYjYzYiIsIm5iZiI6MTYyNzQ4MDk1MywiZXhwIjoxNjU5MDE2OTUzLCJpYXQiOjE2Mjc0ODA5NTN9.pYB3Bi65_NFCulqYdbJdhUraONb4lH__Gs9YoZbiLZM");
        }

        public async Task<RetrieveApplicationsResponse> Get(CancellationToken cancellationToken)
        {
            var serviceResponse = await _client.GetAsync("/Applications/GetAllApplications");

            if (!serviceResponse.IsSuccessStatusCode)
            {
                return new RetrieveApplicationsResponse
                {
                    Errors = new List<string>()
                    {
                        "COULD_NOT_RETRIEVE_APPLICATIONS"
                    }
                };
            }

            return JsonConvert.DeserializeObject<RetrieveApplicationsResponse>(await serviceResponse.Content.ReadAsStringAsync());
        }
    }

    public class RetrieveApplicationsResponse
    {
        [JsonProperty("errors")]
        public List<string> Errors { get; set; } = new List<string>();

        [JsonProperty("List")]
        public List<GetApplication> Applications { get; set; }
    }

    public class GetApplication
    {
        [JsonProperty("applicationId")]
        public string ApplicationId { get; set; }

        [JsonProperty("organisationName")]
        public string OrganisationName { get; set; }

        [JsonProperty("applicationStatus")]
        public string ApplicationStatus { get; set; }

        [JsonProperty("Reference")]
        public string Reference { get; set; }

        [JsonIgnore]
        public string ApplicationStatusDisplayName
        {
            get
            {
                if (Enum.TryParse<ApplicationStatus>(ApplicationStatus, true, out var result))
                {
                    var displayAttribute = result.GetType().GetMember(result.ToString()).First().GetCustomAttribute<DisplayAttribute>();

                    if (displayAttribute != null)
                    {
                        return displayAttribute.Name;
                    }
                    else
                    {
                        return result.ToString();
                    }
                }

                return "Unknown";
            }
        }

        [JsonProperty("lastModified")]
        public string LastModified { get; set; }
    }
}
