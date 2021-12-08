using FluentAssertions;
using InternalPortal.ViewModels;
using Ofgem.API.GGSS.Domain.Models;
using Xunit;

namespace InternalPortal.UnitTests.ViewModels
{
    public class OrganisationDetailsTests
    {
        [Fact]
        public void ShouldReturnFormattedAddress()
        {
            var model = new OrganisationDetails()
            {
                OrganisationAddress = new AddressModel()
                {
                    LineOne = "Address line one",
                    LineTwo = "Address line two",
                    Town = "Town",
                    County = "County",
                    Postcode = "XX1 1XX"
                }
            };

            var formattedAddress = model.GetFormattedAddress();

            formattedAddress.Should().Be("<p>Address line one</p><p>Address line two</p><p>Town</p><p>County</p><p>XX1 1XX</p>");
        }

        [Fact]
        public void ShouldReturnFormattedAddressWithoutLineOne()
        {
            var model = new OrganisationDetails()
            {
                OrganisationAddress = new AddressModel()
                {
                    LineTwo = "Address line two",
                    Town = "Town",
                    County = "County",
                    Postcode = "XX1 1XX"
                }
            };

            var formattedAddress = model.GetFormattedAddress();

            formattedAddress.Should().Be("<p>Line one not provided</p><p>Address line two</p><p>Town</p><p>County</p><p>XX1 1XX</p>");
        }

        [Fact]
        public void ShouldReturnFormattedAddressWithoutLineTwo()
        {
            var model = new OrganisationDetails()
            {
                OrganisationAddress = new AddressModel()
                {
                    LineOne = "Address line one",
                    Town = "Town",
                    County = "County",
                    Postcode = "XX1 1XX"
                }
            };

            var formattedAddress = model.GetFormattedAddress();

            formattedAddress.Should().Be("<p>Address line one</p><p>Town</p><p>County</p><p>XX1 1XX</p>");
        }

        [Fact]
        public void ShouldReturnFormattedAddressWithoutTown()
        {
            var model = new OrganisationDetails()
            {
                OrganisationAddress = new AddressModel()
                {
                    LineOne = "Address line one",
                    LineTwo = "Address line two",
                    County = "County",
                    Postcode = "XX1 1XX"
                }
            };

            var formattedAddress = model.GetFormattedAddress();

            formattedAddress.Should().Be("<p>Address line one</p><p>Address line two</p><p>County</p><p>XX1 1XX</p>");
        }

        [Fact]
        public void ShouldReturnFormattedAddressWithoutCounty()
        {
            var model = new OrganisationDetails()
            {
                OrganisationAddress = new AddressModel()
                {
                    LineOne = "Address line one",
                    LineTwo = "Address line two",
                    Town = "Town",
                    Postcode = "XX1 1XX"
                }
            };

            var formattedAddress = model.GetFormattedAddress();

            formattedAddress.Should().Be("<p>Address line one</p><p>Address line two</p><p>Town</p><p>XX1 1XX</p>");
        }

        [Fact]
        public void ShouldReturnFormattedAddressWithoutPostcode()
        {
            var model = new OrganisationDetails()
            {
                OrganisationAddress = new AddressModel()
                {
                    LineOne = "Address line one",
                    LineTwo = "Address line two",
                    Town = "Town",
                    County = "County"
                }
            };

            var formattedAddress = model.GetFormattedAddress();

            formattedAddress.Should().Be("<p>Address line one</p><p>Address line two</p><p>Town</p><p>County</p><p>Postcode not provided</p>");
        }

        [Fact]
        public void ShouldHandleOrganisationAddressBeingNull()
        {
            var model = new OrganisationDetails();

            var formattedAddress = model.GetFormattedAddress();

            formattedAddress.Should().Be("<p>Address not found</p>");
        }
    }
}
