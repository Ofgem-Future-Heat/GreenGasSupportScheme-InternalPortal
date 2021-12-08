using System.Threading;
using System.Threading.Tasks;
using InternalPortal.Controllers;
using InternalPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ofgem.API.GGSS.Domain.Models;
using Xunit;

namespace InternalPortal.UnitTests.Controllers
{
    public class OrganisationDetailsControllerTests
    {
        private readonly Mock<IGetOrganisationDetailsService> _getOrganisationDetailsService;
        private readonly Mock<IUpdateOrganisationStatusService> _updateOrganisationStatusService;
        private OrganisationDetailsController _controller;

        public OrganisationDetailsControllerTests()
        {
            _getOrganisationDetailsService = new Mock<IGetOrganisationDetailsService>();
            _updateOrganisationStatusService = new Mock<IUpdateOrganisationStatusService>();
        }
        
        [Fact]
        public async Task OrganisationDetailsControllerReturnsOrganisationDetailsActionView()
        {
            _getOrganisationDetailsService.Setup(a => a.Get(It.IsAny<GetOrganisationDetailsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetOrganisationDetailsResponse(){OrganisationAddress = new AddressModel()});

            _controller = new OrganisationDetailsController(_getOrganisationDetailsService.Object, _updateOrganisationStatusService.Object);
            var result = await _controller.OrganisationDetails("1234");
            
            Assert.IsType<ViewResult>(result);
        }
        
        [Fact]
        public async Task OrganisationDetailsControllerUpdatesOrganisationStatus()
        {
            _controller = new OrganisationDetailsController(_getOrganisationDetailsService.Object, _updateOrganisationStatusService.Object);
            var result = await _controller.UpdateOrganisationDetails("abc", "Verified");

            _updateOrganisationStatusService.Verify(
                s => s.UpdateStatus(It.Is<UpdateOrganisationStatus>(r =>
                    r.OrganisationId == "abc" && r.NewStatus == "Verified")), Times.Once);
        }
    }
}