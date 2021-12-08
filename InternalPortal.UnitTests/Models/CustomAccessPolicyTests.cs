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

        [Fact]
        public void ShouldReturnFalseWhenEnvironmentIsDevelopment()
        {
            var environment = new Mock<IWebHostEnvironment>();

            environment
                .Setup(e => e.EnvironmentName)
                .Returns("Development");

            var result = CustomAccessPolicy.IsRestricted(environment.Object);

            result.Should().Be(false);
        }

        [Fact]
        public void ShouldReturnFalseWhenEnvironmentIsDocker()
        {
            var environment = new Mock<IWebHostEnvironment>();

            environment
                .Setup(e => e.EnvironmentName)
                .Returns("Docker");

            var result = CustomAccessPolicy.IsRestricted(environment.Object);

            result.Should().Be(false);
        }

        [Fact]
        public void ShouldReturnFalseWhenEnvironmentIsFDEV()
        {
            var environment = new Mock<IWebHostEnvironment>();

            environment
                .Setup(e => e.EnvironmentName)
                .Returns("FDEV");

            var result = CustomAccessPolicy.IsRestricted(environment.Object);

            result.Should().Be(false);
        }

        [Fact(Skip = "To be reinstated after sprint review")]
        public void ShouldReturnTrueWhenEnvironmentIsASIT()
        {
            var environment = new Mock<IWebHostEnvironment>();

            environment
                .Setup(e => e.EnvironmentName)
                .Returns("ASIT");

            var result = CustomAccessPolicy.IsRestricted(environment.Object);

            result.Should().Be(true);
        }


        [Fact(Skip = "To be reinstated after sprint review")]
        public void ShouldReturnTrueWhenEnvironmentIsNotRecognised()
        {
            var environment = new Mock<IWebHostEnvironment>();

            environment
                .Setup(e => e.EnvironmentName)
                .Returns("not-recognised");

            var result = CustomAccessPolicy.IsRestricted(environment.Object);

            result.Should().Be(true);
        }

        [Fact]
        public void ShouldReturnFalseWhenUserIsNotInList()
        {
            _mockUser.SetupGet(u => u.Claims)
                .Returns(new List<Claim> {
                                new Claim(ClaimTypes.NameIdentifier, "326fa974-7c05-4b37-a8e4-6d5fe6deb63b"),
                                new Claim("name", "James Anderson"),
                                new Claim("identityprovider", "326fa974-7c05-4b37-a8e4-6d5fe6deb63b")
                }.AsEnumerable());

            var context = new AuthorizationHandlerContext(_requirements, _mockUser.Object, null);

            var result = new CustomAccessPolicy().AuthorizeAccess(context);

            result.Should().Be(false);
        }

        [Theory]
        [InlineData("Charlotte", "Baker")]
        [InlineData("Paul", "Russell")]
        [InlineData("Peter", "McKechnie")]
        [InlineData("Michael", "McGuire")]
        [InlineData("Gillian", "Roberts")]
        [InlineData("Brian", "Morris")]
        [InlineData("Brighe", "McColl")]
        [InlineData("James", "Johnston")]
        public void ShouldReturnTrueWhenUserIsInList(string firstName, string lastName)
        {
            _mockUser.SetupGet(u => u.Claims)
                .Returns(new List<Claim> {
                                new Claim(ClaimTypes.NameIdentifier, "326fa974-7c05-4b37-a8e4-6d5fe6deb63b"),
                                new Claim("name", $"{firstName} {lastName}")
                }.AsEnumerable());

            var context = new AuthorizationHandlerContext(_requirements, _mockUser.Object, null);

            var result = new CustomAccessPolicy().AuthorizeAccess(context);

            result.Should().Be(true);
        }
    }

    internal class SomeRequirement : IAuthorizationRequirement { }
}

