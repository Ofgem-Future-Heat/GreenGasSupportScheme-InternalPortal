using System;
using System.Collections.Generic;
using InternalPortal.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace InternalPortal.Models
{
    public class CustomAccessPolicy
    {
        private readonly List<string> ALLOWED_USERS =
            new List<String>
            {
                "Charlotte Baker",
                "Paul Russell",
                "Peter McKechnie",
                "Michael McGuire",
                "Gillian Roberts",
                "Brian Morris",
                "Brighe McColl",
                "James Johnston",
                "Grant McKenna",
                "Mark Butcher"
              };

        public static bool IsRestricted(IWebHostEnvironment environment)
        {
            return false;

            return !(environment.IsDevelopment()
                || environment.IsEnvironment("Docker")
                || environment.IsEnvironment("FDEV"));
        }

        public bool AuthorizeAccess(AuthorizationHandlerContext context)
        {
            try
            {
                return ALLOWED_USERS.Exists(u => u.ToLower() == new UserProfile(context.User).DisplayName.ToLower());
            }
            catch
            {
                return true;
            }
        }
    }
}

