using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InternalPortal.Controllers;
using InternalPortal.Services;
using InternalPortal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ofgem.API.GGSS.Domain.Enums;
using Ofgem.API.GGSS.Domain.ModelValues;
using Ofgem.API.GGSS.Domain.ModelValues.StageOne;
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

        [Fact]
        public async Task ShouldSetApplicationStatusesToCompleteWhenApplicationWithApplicant()
        {
            var application = new RetrieveApplicationResponse
            {
                Application = new ApplicationValue()
                {
                    Status = ApplicationStatus.StageOneApproved
                }
            };
            
            _getApplicationDetailsService
                .Setup(a => a.Get(It.IsAny<GetApplicationDetailsRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(application));
            
            UpdateApplicationRequest applicationRequest = null;

            _updateApplicationStatusService
                .Setup(c => c.Update(It.IsAny<UpdateApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback<UpdateApplicationRequest, CancellationToken>((request, token) => applicationRequest = request);

            var controller = new ApplicationDetailsController(_getApplicationDetailsService.Object,
                _getOrganisationDetailsService.Object, _updateApplicationStatusService.Object);

            var result = await controller.Index("1234567890", "StageOneWithApplicant");

            Assert.IsType<RedirectToActionResult>(result);
            
            applicationRequest.Application.Status.Should().Be(ApplicationStatus.StageOneWithApplicant);
            applicationRequest.Application.StageOne.TellUsAboutYourSite.Status.Should().Be("Completed");
            applicationRequest.Application.StageOne.ProvidePlanningPermission.Status.Should().Be("Completed");
            applicationRequest.Application.StageOne.ProductionDetails.Status.Should().Be("Completed");
        }
        
        [Fact]
        public async Task ShouldNotSetApplicationStatusesToCompleteWhenApplicationIsApproved()
        {
            var application = new RetrieveApplicationResponse
            {
                Application = new ApplicationValue()
                {
                    Status = ApplicationStatus.StageOneApproved
                }
            };
            
            _getApplicationDetailsService
                .Setup(a => a.Get(It.IsAny<GetApplicationDetailsRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(application));
            
            UpdateApplicationRequest applicationRequest = null;

            _updateApplicationStatusService
                .Setup(c => c.Update(It.IsAny<UpdateApplicationRequest>(), It.IsAny<CancellationToken>()))
                .Callback<UpdateApplicationRequest, CancellationToken>((request, token) => applicationRequest = request);

            var controller = new ApplicationDetailsController(_getApplicationDetailsService.Object,
                _getOrganisationDetailsService.Object, _updateApplicationStatusService.Object);

            var result = await controller.Index("1234567890", "StageOneApproved");

            Assert.IsType<RedirectToActionResult>(result);
            
            applicationRequest.Application.Status.Should().Be(ApplicationStatus.StageOneApproved);
            applicationRequest.Application.StageOne.TellUsAboutYourSite.Status.Should().NotBe("Completed");
            applicationRequest.Application.StageOne.ProvidePlanningPermission.Status.Should().NotBe("Completed");
            applicationRequest.Application.StageOne.ProductionDetails.Status.Should().NotBe("Completed");
        }
        
        [Fact]
        public async Task ShouldSetHasPostcodeToYesForExistingApplications()
        {
            _getApplicationDetailsService.Setup(a =>
                    a.Get(It.IsAny<GetApplicationDetailsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RetrieveApplicationResponse()
                    {
                        Application = new ApplicationValue()
                        {
                            StageOne = new StageOneValue()
                            {
                                TellUsAboutYourSite = new TellUsAboutYourSiteValue()
                                {
                                    HasPostcode = null
                                }
                            }
                        }
                    }
                );

            _getOrganisationDetailsService.Setup(a =>
                    a.Get(It.IsAny<GetOrganisationDetailsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetOrganisationDetailsResponse());
            
            var controller = new ApplicationDetailsController(_getApplicationDetailsService.Object, _getOrganisationDetailsService.Object, _updateApplicationStatusService.Object);
            var result = await controller.Index("1234567890");
            var viewResult = result as ViewResult;
            var model = viewResult.Model as ApplicationDetails;

            Assert.IsType<ViewResult>(result);
            model.StageOneDetails.HasPostcode.Should().Be("Yes");
        }
    }
}