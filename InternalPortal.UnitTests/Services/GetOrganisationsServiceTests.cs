using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InternalPortal.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace InternalPortal.UnitTests.Services
{
    public class GetOrganisationsServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;

        public GetOrganisationsServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task ShouldReturnAllOrganisationsIfServiceReturnsOk()
        {
            var expectedResponse = new
            {
                Organisations = new List<GetOrganisations>()
                {
                    new GetOrganisations(),
                    new GetOrganisations()
                }
            };

            SetHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse)),
            });
            
            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };

            var result = await new GetOrganisationsService(httpClient).Get(CancellationToken.None);

            result.Organisations.Should().NotBeNullOrEmpty();
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