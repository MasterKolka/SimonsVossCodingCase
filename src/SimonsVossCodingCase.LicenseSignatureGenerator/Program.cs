using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SimonsVossCodingCase.LicenseSignatureGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel((context, ko) =>
                    {
                        ko.ConfigureHttpsDefaults(o =>
                        {
                            var certFileName = "ca.pfx";
                            var contentRoot = context.HostingEnvironment.ContentRootPath;
                            var path = Path.Combine(contentRoot, certFileName);
                            var serverCert = new X509Certificate2(path, "xxxx");

                            o.ServerCertificate = serverCert;
                            o.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                            
                            // Just as a POC. Validation should be implemented.
                            o.AllowAnyClientCertificate();
                        });
                    });
                    webBuilder.ConfigureLogging(builder => builder.AddApplicationInsights());
                });
    }
}