using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;
using InternalPortal.Controllers;
using InternalPortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InternalPortal.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        HomeController _controller;
        
        private readonly Mock<ILogger<HomeController>> _logger;
        private readonly Mock<IGetOrganisationsService> _getOrganisationsService;
        private readonly Mock<IGetApplicationsService> _getApplicationsService;

        public HomeControllerTests()
        {
            _logger = new Mock<ILogger<HomeController>>();
            _getOrganisationsService = new Mock<IGetOrganisationsService>();
            _getApplicationsService = new Mock<IGetApplicationsService>();

            _getApplicationsService.Setup(a => a.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RetrieveApplicationsResponse()
                {
                    Applications = new List<GetApplication>()
                });
        }
        
        [Fact]
        public async Task HomeControllerReturnsIndexActionView()
        {
            _getOrganisationsService.Setup(a => a.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RetrieveOrganisationsResponse()
            {
                Organisations = new List<GetOrganisations>()
            });
            
            _controller = new HomeController(
                _logger.Object,
                _getOrganisationsService.Object,
                _getApplicationsService.Object);

            var result = await _controller.Index();
            
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task HomeControllerReturnsIndexActionViewWhenGetOrganisationFails()
        {
            _getOrganisationsService.Setup(a => a.Get(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RetrieveOrganisationsResponse()
                {
                    Errors = new List<string>()
                    {
                        "error-from-service"
                    }
                });

            _controller = new HomeController(
                _logger.Object,
                _getOrganisationsService.Object,
                _getApplicationsService.Object);

            var result = await _controller.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}