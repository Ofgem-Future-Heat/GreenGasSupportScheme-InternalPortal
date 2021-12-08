using System;
using System.Text;
using Ofgem.API.GGSS.Domain.Models;
using Ofgem.API.GGSS.Domain.ModelValues;

namespace InternalPortal.ViewModels
{
    public class StageOneDetails
    {
        public string Location { get; set; }
        public string PlantName { get; set; }
        public DocumentValue ConnectionAgreement { get; set; }
        public AddressModel SiteAddress { get; set; }
        public AddressModel InjectionPointAddress { get; set; }
        public string EquipmentDescription { get; set; }
        public string PlanningOutcome { get; set; }
        public DocumentValue PlanningUpload { get; set; }
        public string PlanningPermissionStatement { get; set; }
        public string BiomethaneVolume { get; set; }
        public string EligibleBiomethane { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public string GetSiteFormattedAddress()
        {
            if(SiteAddress == null)
            {
                return "<p>Address not found</p>";
            }

            var lineOne = !string.IsNullOrWhiteSpace(SiteAddress.LineOne)
                ? SiteAddress.LineOne
                : "Address line one not provided";

            var town = !string.IsNullOrWhiteSpace(SiteAddress.Town)
                ? SiteAddress.Town
                : "Town/city not provided";
            
            var postcode = !string.IsNullOrWhiteSpace(SiteAddress.Postcode)
                ? SiteAddress.Postcode
                : "Postcode not provided";

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"<p>{lineOne}</p>");

            if (!string.IsNullOrWhiteSpace(SiteAddress.LineTwo))
            {
                stringBuilder.Append($"<p>{SiteAddress.LineTwo}</p>");
            }
            
            stringBuilder.Append($"<p>{town}</p>");
            
            if (!string.IsNullOrWhiteSpace(SiteAddress.County))
            {
                stringBuilder.Append($"<p>{SiteAddress.County}</p>");
            }
 
            stringBuilder.Append($"<p>{postcode}</p>");

            return stringBuilder.ToString();
        }
        
        public string GetInjectionPointFormattedAddress()
        {
            if(InjectionPointAddress == null)
            {
                return "<p>Address not found</p>";
            }

            var lineOne = !string.IsNullOrWhiteSpace(InjectionPointAddress.LineOne)
                ? InjectionPointAddress.LineOne
                : "Address line one not provided";

            var postcode = !string.IsNullOrWhiteSpace(InjectionPointAddress.Postcode)
                ? InjectionPointAddress.Postcode
                : "Postcode not provided";

            var town = !string.IsNullOrWhiteSpace(InjectionPointAddress.Town)
                ? InjectionPointAddress.Town
                : "Town/city not provided";

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"<p>{lineOne}</p>");

            if (!string.IsNullOrWhiteSpace(InjectionPointAddress.LineTwo))
            {
                stringBuilder.Append($"<p>{InjectionPointAddress.LineTwo}</p>");
            }
            
            stringBuilder.Append($"<p>{town}</p>");
            
            if (!string.IsNullOrWhiteSpace(InjectionPointAddress.County))
            {
                stringBuilder.Append($"<p>{InjectionPointAddress.County}</p>");
            }
 
            stringBuilder.Append($"<p>{postcode}</p>");

            return stringBuilder.ToString();
        }
    }
}