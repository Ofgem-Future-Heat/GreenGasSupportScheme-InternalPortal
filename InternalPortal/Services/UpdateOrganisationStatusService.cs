using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InternalPortal.Services
{
    public interface IUpdateOrganisationStatusService
    {
        Task<UpdateOrganisationStatusResponse> UpdateStatus(UpdateOrganisationStatus request);
    }

    public class UpdateOrganisationStatusService : IUpdateOrganisationStatusService
    {
        private readonly HttpClient _client;

        public UpdateOrganisationStatusService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMyNmZhOTc0LTdjMDUtNGIzNy1hOGU0LTZkNWZlNmRlYjYzYiIsIm5iZiI6MTYyNzQ4MDk1MywiZXhwIjoxNjU5MDE2OTUzLCJpYXQiOjE2Mjc0ODA5NTN9.pYB3Bi65_NFCulqYdbJdhUraONb4lH__Gs9YoZbiLZM"); 
        }
        
        public async Task<UpdateOrganisationStatusResponse> UpdateStatus(UpdateOrganisationStatus request)
        {
            var response = new UpdateOrganisationStatusResponse();
            
            var httpRequest = new HttpRequestMessage()
            {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri($"{_client.BaseAddress}Organisation/{request.OrganisationId}/details"),
                Content = new StringContent(JsonConvert.SerializeObject(new { OrganisationStatus = request.NewStatus }), Encoding.Default, "application/json"),
            };

            var result = await _client.SendAsync(httpRequest);

            if (!result.IsSuccessStatusCode)
            {
                response.Errors.Add("ORGANISATION_NOT_FOUND");
            }
            
            return response;
        }
    }

    public class UpdateOrganisationStatus
    {
        public string OrganisationId { get; set; }
        public string NewStatus { get; set; }
    }

    public class UpdateOrganisationStatusResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}