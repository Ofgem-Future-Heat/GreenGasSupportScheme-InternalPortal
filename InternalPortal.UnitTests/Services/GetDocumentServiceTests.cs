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
    public class GetDocumentServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;

        public GetDocumentServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task ShouldReturnErrorCollectionWhenGetFails()
        {
            SetHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            });

            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };

            var result = await new GetDocumentService(httpClient).Get("document-id", CancellationToken.None);

            result.Errors.Should().NotBeEmpty();
            result.Contents.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task ShouldReturnEmptyErrorCollectionWhenGetSucceeds()
        {
            SetHandler(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("string-content")
            });

            var httpClient = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:1234/") };

            var result = await new GetDocumentService(httpClient).Get("document-id", CancellationToken.None);

            result.Errors.Should().BeNullOrEmpty();
            result.Contents.Should().NotBeEmpty();
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

