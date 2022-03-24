using System.Collections.Generic;
using System.Text;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace InternalPortal.ViewModels
{
    public class OrganisationDetails
    {
        public string OrganisationType { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationRegistrationNumber { get; set; }
        public AddressModel OrganisationAddress { get; set; }
        public string ResponsiblePersonName { get; set; }
        public string ResponsiblePersonSurname { get; set; }
        public string ResponsiblePersonPhoneNumber { get; set; }
        public string ResponsiblePersonEmail { get; set; }
        public DocumentValue PhotoId { get; set; }
        public DocumentValue ProofOfAddress { get; set; }
        public DocumentValue LetterOfAuthority { get; set; }
        public DocumentValue LegalDocument { get; set; }
        public string OrganisationStatus { get; set; }
        public List<UserValue> OrganisationUsers { get; set; }

        public string GetFormattedFullName()
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append($"{ResponsiblePersonName} {ResponsiblePersonSurname}");
            
            return stringBuilder.ToString();
        }

        public string GetFormattedAddress()
        {
            if(OrganisationAddress == null)
            {
                return "<p>Address not found</p>";
            }

            var lineOne = !string.IsNullOrWhiteSpace(OrganisationAddress.LineOne)
                ? OrganisationAddress.LineOne
                : "Line one not provided";

            var postcode = !string.IsNullOrWhiteSpace(OrganisationAddress.Postcode)
                ? OrganisationAddress.Postcode
                : "Postcode not provided";

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"<p>{lineOne}</p>");

            if (!string.IsNullOrWhiteSpace(OrganisationAddress.LineTwo))
            {
                stringBuilder.Append($"<p>{OrganisationAddress.LineTwo}</p>");
            }

            if (!string.IsNullOrWhiteSpace(OrganisationAddress.Town))
            {
                stringBuilder.Append($"<p>{OrganisationAddress.Town}</p>");
            }

            if (!string.IsNullOrWhiteSpace(OrganisationAddress.County))
            {
                stringBuilder.Append($"<p>{OrganisationAddress.County}</p>");
            }
 
            stringBuilder.Append($"<p>{postcode}</p>");

            return stringBuilder.ToString();
        }
    }
}