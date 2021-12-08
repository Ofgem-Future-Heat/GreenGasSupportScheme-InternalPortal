using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InternalPortal.Services;
using Moq;
using Moq.Protected;
using Xunit;

namespace InternalPortal.UnitTests.Services
{
    public class UpdateOrganisationStatusServiceTests
    {
        private Mock<HttpMessageHandler> _handlerMock;
        
        [Fact]
        public async Task ReturnsErrorIfOrgNotFound()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };
            var service = new UpdateOrganisationStatusService(httpClient);
            
            SetHandler(new HttpResponseMessage(HttpStatusCode.NotFound));
            
            var response = await service.UpdateStatus(new UpdateOrganisationStatus()
            {
                OrganisationId = "NonExistentOrg",
                NewStatus = "Verified"
            });

            response.Errors.Should().Contain("ORGANISATION_NOT_FOUND");
        }
        
        [Fact]
        public async Task ReturnsNoErrorIfOrgUpdated()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };
            var service = new UpdateOrganisationStatusService(httpClient);
            
            SetHandler(new HttpResponseMessage(HttpStatusCode.OK));
            
            var response = await service.UpdateStatus(new UpdateOrganisationStatus()
            {
                OrganisationId = "NonExistentOrg",
                NewStatus = "Verified"
            });

            response.Errors.Should().BeEmpty();
        }
        
        
        private void SetHandler(HttpResponseMessage response)
        {
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);
        }
    }
}