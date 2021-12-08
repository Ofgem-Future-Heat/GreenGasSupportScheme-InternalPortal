using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Controllers;
using InternalPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.Responses.Applications;
using Xunit;

namespace InternalPortal.UnitTests.Controllers
{
    public class ApplicationDetailsControllerTests
    {
        private readonly Mock<IGetApplicationDetailsService> _getApplicationDetailsService;
        private readonly Mock<IGetOrganisationDetailsService> _getOrganisationDetailsService;
        private readonly Mock<IUpdateApplicationStatusService> _updateApplicationStatusService;

        public ApplicationDetailsControllerTests()
        {
            _getApplicationDetailsService = new Mock<IGetApplicationDetailsService>();
            _getOrganisationDetailsService = new Mock<IGetOrganisationDetailsService>();
            _updateApplicationStatusService = new Mock<IUpdateApplicationStatusService>();
        }
        
        [Fact]
        public async Task ApplicationDetailsControllerReturnsApplicationDetailsView()
        {
            _getApplicationDetailsService.Setup(a =>
                    a.Get(It.IsAny<GetApplicationDetailsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RetrieveApplicationResponse()
                    {
                        Application = new ApplicationValue()
                    }
                );

            _getOrganisationDetailsService.Setup(a =>
                    a.Get(It.IsAny<GetOrganisationDetailsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetOrganisationDetailsResponse());
            
            var controller = new ApplicationDetailsController(_getApplicationDetailsService.Object, _getOrganisationDetailsService.Object, _updateApplicationStatusService.Object);
            var result = await controller.Index("1234567890");
            
            Assert.IsType<ViewResult>(result);
        }
    }
}