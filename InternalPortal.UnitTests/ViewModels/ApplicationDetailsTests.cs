using FluentAssertions;
using InternalPortal.ViewModels;
using Ofgem.API.GGSS.Domain.Models;
using Xunit;

namespace InternalPortal.UnitTests.ViewModels
{
    public class ApplicationDetailsTests
    {
        [Fact]
        public void ShouldReturnFormattedSiteAddress()
        {
            var model = new StageOneDetails()
            {
                SiteAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    LineTwo = "Line Two",
                    Town = "Town",
                    County = "County",
                    Postcode = "AB1 1BA"
                }
            };

            var formattedSiteAddress = model.GetSiteFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Line Two</p><p>Town</p><p>County</p><p>AB1 1BA</p>");
        }

        [Fact]
        public void ShouldReturnSiteFormattedAddressWithoutLineOne()
        {
            var model = new StageOneDetails()
            {
                SiteAddress = new AddressModel()
                {
                    LineTwo = "Line Two",
                    Town = "Town",
                    County = "County",
                    Postcode = "AB1 1BA"
                }
            };

            var formattedSiteAddress = model.GetSiteFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Address line one not provided</p><p>Line Two</p><p>Town</p><p>County</p><p>AB1 1BA</p>");
        }

        [Fact]
        public void ShouldReturnSiteFormattedAddressWithoutLineTwo()
        {
            var model = new StageOneDetails()
            {
                SiteAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    Town = "Town",
                    County = "County",
                    Postcode = "AB1 1BA"
                }
            };

            var formattedSiteAddress = model.GetSiteFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Town</p><p>County</p><p>AB1 1BA</p>");
        }

        [Fact]
        public void ShouldReturnSiteFormattedAddressWithoutTown()
        {
            var model = new StageOneDetails()
            {
                SiteAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    LineTwo = "Line Two",
                    County = "County",
                    Postcode = "AB1 1BA"
                }
            };
            
            var formattedSiteAddress = model.GetSiteFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Line Two</p><p>Town/city not provided</p><p>County</p><p>AB1 1BA</p>");
        }
        
        [Fact]
        public void ShouldReturnSiteFormattedAddressWithoutCounty()
        {
            var model = new StageOneDetails()
            {
                SiteAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    LineTwo = "Line Two",
                    Town = "Town",
                    Postcode = "AB1 1BA"
                }
            };
            
            var formattedSiteAddress = model.GetSiteFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Line Two</p><p>Town</p><p>AB1 1BA</p>");
        }
        
        [Fact]
        public void ShouldReturnSiteFormattedAddressWithoutPostcode()
        {
            var model = new StageOneDetails()
            {
                SiteAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    LineTwo = "Line Two",
                    Town = "Town",
                    County = "County"
                }
            };
            
            var formattedSiteAddress = model.GetSiteFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Line Two</p><p>Town</p><p>County</p><p>Postcode not provided</p>");
        }

        [Fact]
        public void ShouldHandleSiteAddressBeingNull()
        {
            var model = new StageOneDetails();

            var formattedSiteAddress = model.GetSiteFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Address not found</p>");
        }
        
        [Fact]
        public void ShouldReturnFormattedInjectionPointAddress()
        {
            var model = new StageOneDetails()
            {
                InjectionPointAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    LineTwo = "Line Two",
                    Town = "Town",
                    County = "County",
                    Postcode = "AB1 1BA"
                }
            };

            var formattedSiteAddress = model.GetInjectionPointFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Line Two</p><p>Town</p><p>County</p><p>AB1 1BA</p>");
        }
        
        [Fact]
        public void ShouldReturnInjectionPointSiteFormattedAddressWithoutLineOne()
        {
            var model = new StageOneDetails()
            {
                InjectionPointAddress = new AddressModel()
                {
                    LineTwo = "Line Two",
                    Town = "Town",
                    County = "County",
                    Postcode = "AB1 1BA"
                }
            };

            var formattedSiteAddress = model.GetInjectionPointFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Address line one not provided</p><p>Line Two</p><p>Town</p><p>County</p><p>AB1 1BA</p>");
        }
        
        [Fact]
        public void ShouldReturnInjectionPointSiteFormattedAddressWithoutLineTwo()
        {
            var model = new StageOneDetails()
            {
                InjectionPointAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    Town = "Town",
                    County = "County",
                    Postcode = "AB1 1BA"
                }
            };

            var formattedSiteAddress = model.GetInjectionPointFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Town</p><p>County</p><p>AB1 1BA</p>");
        }
        
        [Fact]
        public void ShouldReturnInjectionPointSiteFormattedAddressWithoutTown()
        {
            var model = new StageOneDetails()
            {
                InjectionPointAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    LineTwo = "Line Two",
                    County = "County",
                    Postcode = "AB1 1BA"
                }
            };
            
            var formattedSiteAddress = model.GetInjectionPointFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Line Two</p><p>Town/city not provided</p><p>County</p><p>AB1 1BA</p>");
        }
        
        [Fact]
        public void ShouldReturnInjectionPointSiteFormattedAddressWithoutCounty()
        {
            var model = new StageOneDetails()
            {
                InjectionPointAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    LineTwo = "Line Two",
                    Town = "Town",
                    Postcode = "AB1 1BA"
                }
            };
            
            var formattedSiteAddress = model.GetInjectionPointFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Line Two</p><p>Town</p><p>AB1 1BA</p>");
        }
        
        [Fact]
        public void ShouldReturnInjectionPointSiteFormattedAddressWithoutPostcode()
        {
            var model = new StageOneDetails()
            {
                InjectionPointAddress = new AddressModel()
                {
                    LineOne = "Line One",
                    LineTwo = "Line Two",
                    Town = "Town",
                    County = "County"
                }
            };

            var formattedSiteAddress = model.GetInjectionPointFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Line One</p><p>Line Two</p><p>Town</p><p>County</p><p>Postcode not provided</p>");
        }

        [Fact]
        public void ShouldHandleInjectionPointSiteAddressBeingNull()
        {
            var model = new StageOneDetails();

            var formattedSiteAddress = model.GetInjectionPointFormattedAddress();

            formattedSiteAddress.Should().Be("<p>Address not found</p>");
        }
    }
}