using System.IO;
using Microsoft.Extensions.Configuration;

namespace SSHServer.Common
{
    public sealed class AppConfiguration
    {
        private readonly IConfigurationRoot _configuration;

        public AppConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfigurationRoot Configuration => _configuration;
    }
}