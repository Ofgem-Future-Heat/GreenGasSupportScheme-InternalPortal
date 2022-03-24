using System.Collections.Generic;
using System.Linq;
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
    public class OrganisationsTests
    {
        private readonly Mock<IGetOrganisationsService> _service;
        
        public OrganisationsTests()
        {
            var response = new RetrieveOrganisationsResponse()
            {
                Organisations = new List<GetOrganisations>
                {
                    new GetOrganisations() {OrganisationName = "Unverified-1", OrganisationStatus = "Not verified"},
                    new GetOrganisations() {OrganisationName = "Unverified-2", OrganisationStatus = "Not verified"},
                    new GetOrganisations() {OrganisationName = "Verified-1", OrganisationStatus = "Verified"},
                    new GetOrganisations() {OrganisationName = null, OrganisationStatus = "Verified"}
                }
            };

            _service = new Mock<IGetOrganisationsService>();

            _service
                .Setup(s => s.Get(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response));
        }
        
        [Fact]
        public async Task ShouldReturnListOfUnverifiedOrganisations()
        {
            var organisations = new Organisations();
            
            var model = await Organisations.Get(_service.Object, CancellationToken.None);

            model.Unverified.Should().HaveCount(2);
            model.Unverified.Should().Contain(o => o.Status == "Not verified");
        }
        
        [Fact]
        public async Task ShouldReturnListOfVerifiedOrganisations()
        {
            var organisations = new Organisations();
            
            var model = await Organisations.Get(_service.Object, CancellationToken.None);

            model.Verified.Should().HaveCount(2);
            model.Verified.Should().Contain(o => o.Status == "Verified");
        }
        
        [Fact]
        public async Task ShouldReturnOrganisationsWithUnnamedWhenNull()
        {
            var organisations = new Organisations();
            
            var model = await Organisations.Get(_service.Object, CancellationToken.None);

            model.Verified.Should().HaveCount(2);
            model.Verified.Should().Contain(o => o.Status == "Verified");
            model.Verified.Last().Name.Should().Be("Unnamed");
        }
    }
}