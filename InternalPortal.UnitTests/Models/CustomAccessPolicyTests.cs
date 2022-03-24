using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using FluentAssertions;
using InternalPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace InternalPortal.UnitTests.Models
{
    public class CustomAccessPolicyTests
    {
        private readonly SomeRequirement[] _requirements;
        private readonly Mock<ClaimsPrincipal> _mockUser;

        public CustomAccessPolicyTests()
        {
            _requirements = new[] { new SomeRequirement() };

            _mockUser = new Mock<ClaimsPrincipal>();

            _mockUser.Setup(u => u.Identity).Returns(new ClaimsIdentity());

            _mockUser.Setup(u => u.Identity.IsAuthenticated).Returns(true);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void ShouldReturnTheOppositeOfTheValuePassed(bool noAuthentication, bool expected)
        {
            var result = CustomAccessPolicy.IsRestricted(noAuthentication);

            result.Should().Be(expected);
        }

        [Fact]
        public void ShouldReturnFalseWhenUserIsNotInList()
        {
            _mockUser.SetupGet(u => u.Identity.Name).Returns("James.Anderson@ofgem.gov.uk");

            var context = new AuthorizationHandlerContext(_requirements, _mockUser.Object, null);

            var result = new CustomAccessPolicy().AuthorizeAccess(context);

            result.Should().Be(false);
        }

        [Theory]
        [InlineData("Charlotte.Baker@ofgem.gov.uk")]
        [InlineData("Brighe.McColl@ofgem.gov.uk")]
        [InlineData("Peter.McKechnie@ofgem.gov.uk")]
        [InlineData("Michael.McGuire@ofgem.gov.uk")]
        [InlineData("James.Johnston@ofgem.gov.uk")]
        [InlineData("Brian.Morris@ofgem.gov.uk")]
        [InlineData("Claris.Ankunda@ofgem.gov.uk")]
        [InlineData("Andrew.Connell@ofgem.gov.uk")]
        [InlineData("Paul.McDonald@ofgem.gov.uk")]
        [InlineData("Andre.Cardozo@ofgem.gov.uk")]
        [InlineData("paul.russell@ofgem.gov.uk")]
        [InlineData("calum.ruddock@ofgem.gov.uk")]
        [InlineData("jamie.ramsay@ofgem.gov.uk")]
        [InlineData("fiona.mackinnon@ofgem.gov.uk")]
        [InlineData("kerry.fatherley@ofgem.gov.uk")]
        [InlineData("Alistair.Crighton@ofgem.gov.uk")]
        public void ShouldReturnTrueWhenUserIsInList(string name)
        {
            _mockUser.SetupGet(u => u.Identity.Name).Returns(name);

            var context = new AuthorizationHandlerContext(_requirements, _mockUser.Object, null);

            var result = new CustomAccessPolicy().AuthorizeAccess(context);

            result.Should().Be(true);
        }
    }

    internal class SomeRequirement : IAuthorizationRequirement { }
}

