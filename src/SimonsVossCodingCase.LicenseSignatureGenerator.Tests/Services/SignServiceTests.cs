using System;
using System.Threading.Tasks;
using FluentAssertions;
using SimonsVossCodingCase.GrpcServices;
using Xunit;
using SignService = SimonsVossCodingCase.LicenseSignatureGenerator.Services.SignService;

namespace SimonsVossCodingCase.LicenseSignatureGenerator.Tests.Services
{
    public class SignServiceTests
    {
        [Theory]
        [InlineData("","Json is empty")]
        [InlineData(null,"Value cannot be null. (Parameter 'value')")]
        [InlineData("{\"test\":true","Json is invalid")]
        [InlineData("{\"test\":true}","{\"test\":true,\"signed\":true}")]
        public async Task SignJsonTest(string json, string result)
        {
            var service = new SignService();

            try
            {
                var signResult = await service.Sign(new SignRequest() {Json = json}, null);
                signResult.Json.Should().BeEquivalentTo(result);
            }
            catch (Exception ex)
            {
                ex.Message.Should().BeEquivalentTo(result);
            }
        }
    }
}