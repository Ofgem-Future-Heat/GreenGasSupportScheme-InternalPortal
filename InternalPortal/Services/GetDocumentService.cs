using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace InternalPortal.Services
{
    public interface IGetDocumentService
    {
        Task<GetDocumentResponse> Get(string documentId, CancellationToken cancellationToken = default);
    }

    public class GetDocumentService : IGetDocumentService
    {
        private readonly HttpClient _client;

        public GetDocumentService(HttpClient client)
        {
            _client = client;
        }

        public async Task<GetDocumentResponse> Get(string documentId, CancellationToken cancellationToken = default)
        {
            CheckParameter(documentId);

            var response = new GetDocumentResponse();

            var serviceResponse = await _client.GetAsync($"/get/{documentId}", cancellationToken);

            if (!serviceResponse.IsSuccessStatusCode)
            {
                response.AddError("DOCUMENT_NOT_FOUND");

                return response;
            }

            response.Contents = await serviceResponse.Content.ReadAsByteArrayAsync();

            return response;
        }

        private void CheckParameter(string documentId)
        {
            if (string.IsNullOrWhiteSpace(documentId))
            {
                throw new System.ArgumentNullException("documentId");
            }
        }
    }

    public class GetDocumentResponse
    {
        public byte[] Contents { get; set; }

        public List<string> Errors { get; internal set; } = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}