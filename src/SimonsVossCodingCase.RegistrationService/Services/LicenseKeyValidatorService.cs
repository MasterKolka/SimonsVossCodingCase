using System.Linq;

namespace SimonsVossCodingCase.RegistrationService.Services
{
    public interface ILicenseKeyValidatorService
    {
        bool IsValid(string licenseKey);
    }

    public class LicenseKeyValidatorService : ILicenseKeyValidatorService
    {
        private readonly string[] _validKeys = new[] {"1234567890123456", "1111111111111111"};
        
        public bool IsValid(string licenseKey)
        {
            return _validKeys.Contains(licenseKey);
        }
    }
}