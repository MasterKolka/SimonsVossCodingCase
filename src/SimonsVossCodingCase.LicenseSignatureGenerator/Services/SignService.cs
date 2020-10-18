using System;
using System.Threading.Tasks;
using Grpc.Core;
using SimonsVossCodingCase.GrpcServices;

namespace SimonsVossCodingCase.LicenseSignatureGenerator.Services
{
    public class SignService : GrpcServices.SignService.SignServiceBase
    {
        public override Task<SignReply> Sign(SignRequest request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request?.Json))
            {
                throw new Exception("Json is empty");
            }

            var jsonEndIndex = request.Json.LastIndexOf('}');

            if (jsonEndIndex == -1)
            {
                throw new Exception("Json is invalid");
            }

            var signed = request.Json.Insert(request.Json.LastIndexOf('}'), ",\"signed\":true");

            return Task.FromResult(new SignReply {Json = signed});
        }
    }
}