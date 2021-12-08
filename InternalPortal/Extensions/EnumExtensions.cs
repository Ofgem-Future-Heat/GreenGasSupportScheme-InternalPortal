using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Ofgem.API.GGSS.Domain.Enums;

namespace InternalPortal.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayAttribute = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>();

            var displayName = displayAttribute?.GetName();
            return displayName ?? enumValue.ToString();
        }
        
        public static string GetDisplayTag(this Enum applicationStatus)
        {
            return applicationStatus switch
            {
                ApplicationStatus.StageOneSubmitted => "govuk-tag--blue",
                ApplicationStatus.Draft => "govuk-tag--yellow",
                ApplicationStatus.Rejected => "govuk-tag--red",
                ApplicationStatus.StageOneApproved => "govuk-tag--green",
                _ => "govuk-tag--blue",
            };
        }
    }
}