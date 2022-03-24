namespace InternalPortal.Extensions
{
    public static class StringExtensions
    {
        public static string GetDisplayTag(this string applicationStatus)
        {
            return applicationStatus switch
            {
                "Stage One Submitted" => "govuk-tag--blue",
                "Stage Two Submitted" => "govuk-tag--blue",
                "Stage Three Submitted" => "govuk-tag--blue",
                "Stage One Approved" => "govuk-tag--green",
                "Stage Two Approved" => "govuk-tag--green",
                "Stage Three Approved" => "govuk-tag--green",
                "Draft" => "govuk-tag--yellow",
                "With Applicant" => "govuk-tag--yellow",
                "Stage One With Applicant" => "govuk-tag--yellow",
                "Stage Two With Applicant" => "govuk-tag--yellow",
                "Rejected" => "govuk-tag--red",
                "Stage One Rejected" => "govuk-tag--red",
                "Stage Two Rejected" => "govuk-tag--red",
                _ => "govuk-tag--blue",
            };
        }
    }
}