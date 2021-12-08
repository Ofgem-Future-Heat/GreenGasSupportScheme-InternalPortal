using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InternalPortal.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Ofgem.API.GGSS.Domain.Models;
using Xunit;

namespace InternalPortal.UnitTests.Services
{
    public class GetOrganisationDetailsServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;

        public GetOrganisationDetailsServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
        }
        
        [Fact]
        public async Task ShouldReturnOrganisationDetailsIfServiceReturnsOk()
        {
            var expectedResponse = new GetOrganisationDetailsResponse()
            {
                OrganisationName = "OrgName",
                OrganisationAddress = new AddressModel(),
                OrganisationType = "Private",
                OrganisationRegistrationNumber = "1234"
            };
            
            SetHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse)),
            });
            
            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };

            var result =
                await new GetOrganisationDetailsService(httpClient).Get(new GetOrganisationDetailsRequest(),
                    CancellationToken.None);

            result.OrganisationName.Should().Be("OrgName");
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