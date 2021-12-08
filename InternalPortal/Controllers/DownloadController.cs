using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InternalPortal.Controllers
{
    public class DownloadController : Controller
    {
        private readonly ILogger<DownloadController> _logger;
        private readonly IGetDocumentService _getDocumentService;

        public DownloadController(
            ILogger<DownloadController> logger,
            IGetDocumentService getDocumentService)
        {
            _logger = logger;
            _getDocumentService = getDocumentService;
        }

        [HttpGet]
        [Route("/application-document/download/{containerName}/{blobName}")]
        public async Task<IActionResult> DownloadDocument([FromRoute] string containerName, [FromRoute] string blobName)
        {
             _logger.LogInformation("DownloadDocument action called on Download controller");

            var documentId = $"{containerName}/{blobName}";

            var response = await _getDocumentService.Get(documentId, CancellationToken.None);

            return File(response.Contents, MediaTypeNames.Application.Octet);

        }
    }
}