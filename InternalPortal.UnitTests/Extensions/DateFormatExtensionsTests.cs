using System;
using FluentAssertions;
using InternalPortal.Extensions;
using Xunit;

namespace InternalPortal.UnitTests.Extensions
{
    public class DateFormatExtensionsTests
    {
        [Fact]
        public void ShouldReturnFormattedWhenDateIsInRightFormat()
        {
            var expected = "14 Dec 2019 00:00 AM";

            var actual = "14 Dec 2019".ToOfgemShortDate();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnFormattedWhenDateIsEmptyString()
        {
            var expected = DateTime.Now.ToString("dd MMM yyyy HH:mm tt");

            var actual = "".ToOfgemShortDate();

            actual.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnFormattedWhenDateIsDateTimeString()
        {
            var expected = "18 Oct 2021 08:27 AM";

            var actual = "2021-10-18T08:27:59".ToOfgemShortDate();

            actual.Should().Be(expected);
        }
    }
}
