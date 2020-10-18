using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SimonsVossCodingCase.RegistrationService.Models
{
    public class RegistrationModel
    {
        [Required]
        [StringLength(30)]
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }

        [StringLength(30)]
        [JsonPropertyName("contactPerson")]
        public string ContactPerson { get; set; }
        
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [Required]
        [StringLength(16, MinimumLength = 16)]
        [JsonPropertyName("licenseKey")]
        public string LicenseKey { get; set; }
    }
}