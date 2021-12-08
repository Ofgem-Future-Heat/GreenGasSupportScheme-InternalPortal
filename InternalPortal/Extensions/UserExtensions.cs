using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InternalPortal.Extensions
{
    public static class UserProfileBuilder
    {
        public static UserProfile CreateForCurrentUser(IServiceProvider serviceProvider)
            => serviceProvider.GetService<IHttpContextAccessor>().HttpContext.User.UserProfile();
    }

    public static class UserExtensions
    {
        public static UserProfile UserProfile(this ClaimsPrincipal user)
        {
            return new UserProfile(user);
        }

        public static string GetDisplayName(this ClaimsPrincipal user)
        {
            return new UserProfile(user).DisplayName;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            return new UserProfile(user).Id.ToString();
        }
    }

    public class UserProfile
    {
        public Guid Id { get; }
        public string Name { get; }
        public string DisplayName { get => Name; }

        public UserProfile(ClaimsPrincipal user)
        {
            if (IsAuthenticated(user))
            {
                Id = Guid.Parse(user.Claims.SingleOrDefault(claim => claim.Type == "sub")?.Value ?? "00000000-0000-0000-0000-000000000000");
                Name = user.Claims.Single(claim => claim.Type == "name").Value;
            }
            else
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000");
                Name = "Unknown";
            }
        }

        private bool IsAuthenticated(ClaimsPrincipal user)
        {
            return user != null && user.Identity != null && user.Identity.IsAuthenticated;
        }
    }
}
