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
    public class GetApplicationsServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;

        public GetApplicationsServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task ShouldReturnAllApplicationsIfServiceReturnsOk()
        {
            var response = new
            {
                List = new List<TestApplication>()
                {
                    new TestApplication(){ installationName = "submitted-stage-1", reference = "app-reference", status = "1" },
                    new TestApplication(){ installationName = "submitted-stage-2", reference = "app-reference", status = "2" }
                }
            };

            SetHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(response)),
            });

            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };

            var result = await new GetApplicationsService(httpClient).Get(CancellationToken.None);

            result.Applications.Should().NotBeNullOrEmpty();
            result.Errors.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldReturnErrorsIfServiceReturnsNotOk()
        {
            SetHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };

            var result = await new GetApplicationsService(httpClient).Get(CancellationToken.None);

            result.Applications.Should().BeNullOrEmpty();
            result.Errors.Should().NotBeNullOrEmpty();
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

    internal class TestApplication
    {
        public string reference { get; set; }

        public string installationName { get; set; }

        public string status { get; set; }
    }
}