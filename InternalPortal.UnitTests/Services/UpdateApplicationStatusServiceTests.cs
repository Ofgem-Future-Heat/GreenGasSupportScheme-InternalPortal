using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InternalPortal.Services;
using Moq;
using Moq.Protected;
using Ofgem.API.GGSS.Domain.ModelValues;
using Xunit;

namespace InternalPortal.UnitTests.Services
{
    public class UpdateApplicationStatusServiceTests
    {
        private Mock<HttpMessageHandler> _handlerMock;
        
        [Fact]
        public async Task ReturnsErrorIfApplicationNotFound()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };
            var service = new UpdateApplicationStatusService(httpClient);
            
            SetHandler(new HttpResponseMessage(HttpStatusCode.NotFound));
            
            var response = await service.Update(new UpdateApplicationRequest()
            {
                Application = new ApplicationValue(),
                UserId = "12345"
            },CancellationToken.None);
        
            response.Errors.Should().Contain("COULD_NOT_UPDATE_APPLICATION");
        }
        
        [Fact]
        public async Task ReturnsNoErrorIfApplicationUpdated()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };
            var service = new UpdateApplicationStatusService(httpClient);
            
            SetHandler(new HttpResponseMessage(HttpStatusCode.OK));
            
            var response = await service.Update(new UpdateApplicationRequest()
            {
                Application = new ApplicationValue(),
                UserId = "12345"
            },CancellationToken.None);
        
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