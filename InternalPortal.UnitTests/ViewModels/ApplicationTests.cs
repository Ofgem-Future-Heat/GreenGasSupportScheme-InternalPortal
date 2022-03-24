
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InternalPortal.Services;
using InternalPortal.ViewModels;
using Moq;
using Xunit;

namespace InternalPortal.UnitTests.ViewModels
{
    public class ApplicationTests
    {
        [Fact]
        public async Task ShouldReturnListOfApplications()
        {
            var applications = new Applications();

            var getApplications = new Mock<IGetApplicationsService>();

            var response = new RetrieveApplicationsResponse()
            {
                Applications = new List<GetApplication>()
                {
                    GetApplicationWithStatus("StageOneSubmitted"),
                    GetApplicationWithStatus("StageTwoSubmitted")
                }
            };

            getApplications
                .Setup(a => a.Get(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));

            var model = await Applications.Get(getApplications.Object, CancellationToken.None);

            model.StageOneSubmitted.Should().NotBeNullOrEmpty();
            model.StageOneSubmitted.Count.Should().Be(1);

            model.StageTwoSubmitted.Should().NotBeNullOrEmpty();
            model.StageTwoSubmitted.Count.Should().Be(1);
        }

        private GetApplication GetApplicationWithStatus(string status)
        {
            return new GetApplication() { ApplicationStatus = status };
        }
    }
}
