using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimonsVossCodingCase.GrpcServices;
using SimonsVossCodingCase.RegistrationService.Configuration;

namespace SimonsVossCodingCase.RegistrationService.Services
{
    public interface IRemoteSignService
    {
        Task<string> SignJson(string json, CancellationToken cancellationToken);
    }

    public class RemoteSignService : IRemoteSignService
    {
        private readonly ILogger<RemoteSignService> _logger;
        private readonly string _serverAddress;
        private readonly HttpClientHandler _httpClientHandler;

        public RemoteSignService(IWebHostEnvironment webHostEnvironment, IOptions<ApiConfiguration> apiConfiguration, ILogger<RemoteSignService> logger)
        {
            _logger = logger;
            _serverAddress = apiConfiguration.Value.LicenseServerApiUrl;
            _httpClientHandler = new HttpClientHandler();
            _httpClientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            _httpClientHandler.ClientCertificates.Add(
                new X509Certificate2(
                    Path.Combine(webHostEnvironment.ContentRootPath, "regserver.full.pfx"),
                    "xxxx"
                )
            );
        }

        public async Task<string> SignJson(string json, CancellationToken cancellationToken)
        {
            using var channel =
                GrpcChannel.ForAddress(_serverAddress, new GrpcChannelOptions()
                {
                    HttpHandler = _httpClientHandler
                });
            var client = new SignService.SignServiceClient(channel);

            try
            {
                var response = await client.SignAsync(new SignRequest() {Json = json}, null, null, cancellationToken);

                return response?.Json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed signature generation");
                throw new Exception("Failed signature generation");
            }
        }
    }
}