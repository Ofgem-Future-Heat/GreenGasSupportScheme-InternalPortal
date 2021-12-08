using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;

namespace InternalPortal.Services
{
    public interface IUpdateApplicationStatusService
    {
        Task<UpdateApplicationResponse> Update(UpdateApplicationRequest request, CancellationToken token);

    }
    
    public class UpdateApplicationStatusService : IUpdateApplicationStatusService
    {
        private readonly HttpClient _client;

        public UpdateApplicationStatusService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjMyNmZhOTc0LTdjMDUtNGIzNy1hOGU0LTZkNWZlNmRlYjYzYiIsIm5iZiI6MTYyNzQ4MDk1MywiZXhwIjoxNjU5MDE2OTUzLCJpYXQiOjE2Mjc0ODA5NTN9.pYB3Bi65_NFCulqYdbJdhUraONb4lH__Gs9YoZbiLZM"); 
        }
        
        public async Task<UpdateApplicationResponse> Update(UpdateApplicationRequest request, CancellationToken token)
        {
            var response = new UpdateApplicationResponse();

            var data = new
            {
                Application = request.Application,

                UserId = request.UserId
            };
            
            var serviceResponse = await _client.PutAsJsonAsync($"/Application/{request.ApplicationId}", data, token);

            if (!serviceResponse.IsSuccessStatusCode)
            {
                response.Errors.Add("COULD_NOT_UPDATE_APPLICATION");
            }
            
            return response;
        }
    }
    public class UpdateApplicationRequest
    {
        public string ApplicationId { get; set; }
        public ApplicationValue Application { get; set; }
        public string UserId { get; set; }
    }
}