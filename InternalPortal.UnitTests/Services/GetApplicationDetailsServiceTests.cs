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
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Xunit;

namespace InternalPortal.UnitTests.Services
{
    public class GetApplicationDetailsServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;

        public GetApplicationDetailsServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
        }
        
        [Fact]
        public async Task GetApplicationDetailsOfShouldNotBeNull()
        {
            var expectedResponse = new RetrieveApplicationResponse()
            {
                Application = new ApplicationValue()
                {
                    StageOne = new StageOneValue()
                    {
                        TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                        {
                            PlantName = "ABC",
                            PlantLocation = "England"
                        }
                    }
                }
            };
            
            SetHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
            });
            
            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };

            var request = new GetApplicationDetailsRequest()
            {
                ApplicationId = "12345"
            };

            var result = await new GetApplicationDetailsService(httpClient).Get(request, CancellationToken.None);
            
            result.Application.Should().NotBeNull();
            result.Errors.Should().BeNullOrEmpty();
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