using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InternalPortal.Services
{
    public interface IGetOrganisationsService
    {
        Task<RetrieveOrganisationsResponse> Get(CancellationToken cancellationToken);
    }
    
    public class GetOrganisationsService : IGetOrganisationsService
    {
        private readonly HttpClient _client;

        public GetOrganisationsService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMyNmZhOTc0LTdjMDUtNGIzNy1hOGU0LTZkNWZlNmRlYjYzYiIsIm5iZiI6MTYyNzQ4MDk1MywiZXhwIjoxNjU5MDE2OTUzLCJpYXQiOjE2Mjc0ODA5NTN9.pYB3Bi65_NFCulqYdbJdhUraONb4lH__Gs9YoZbiLZM"); 
        }
        
        public async Task<RetrieveOrganisationsResponse> Get(CancellationToken cancellationToken)
        {
            var response = new RetrieveOrganisationsResponse();
            
            var serviceResponse = await _client.GetAsync($"/Organisations/getAllOrganisations");

            if (!serviceResponse.IsSuccessStatusCode)
            {
                response.Errors.Add("COULD_NOT_RETRIEVE_ORGANISATIONS");
                return response;
            }

            var content = await serviceResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RetrieveOrganisationsResponse>(content);
        }
    }
    
    public class RetrieveOrganisationsResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
        public List<GetOrganisations> Organisations { get; set; }
    }

    public class GetOrganisations
    {
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationStatus { get; set; }
        public string LastModified { get; set; }
    }
}