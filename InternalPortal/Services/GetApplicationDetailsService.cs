using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace InternalPortal.Services
{
    public interface IGetApplicationDetailsService
    {
        Task<RetrieveApplicationResponse> Get(GetApplicationDetailsRequest getApplicationDetailsRequest,
            CancellationToken cancellationToken);
    }
    
    public class GetApplicationDetailsService : IGetApplicationDetailsService
    {
        private readonly HttpClient _client;

        public GetApplicationDetailsService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMyNmZhOTc0LTdjMDUtNGIzNy1hOGU0LTZkNWZlNmRlYjYzYiIsIm5iZiI6MTYyNzQ4MDk1MywiZXhwIjoxNjU5MDE2OTUzLCJpYXQiOjE2Mjc0ODA5NTN9.pYB3Bi65_NFCulqYdbJdhUraONb4lH__Gs9YoZbiLZM"); 
        }
        
        public async Task<RetrieveApplicationResponse> Get(GetApplicationDetailsRequest getApplicationDetailsRequest, CancellationToken cancellationToken)
        {
            var response = new RetrieveApplicationResponse();

            var serviceResponse = await _client.GetAsync($"/Application/{getApplicationDetailsRequest.ApplicationId}");

            if (!serviceResponse.IsSuccessStatusCode)
            {
                response.Errors.Add("APPLICATION_DETAILS_NOT_FOUND");

                return response;
            }

            var content = await serviceResponse.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<RetrieveApplicationResponse>(content);
        }
    }

    public class GetApplicationDetailsRequest
    {
        public string ApplicationId { get; set; }
    }
}