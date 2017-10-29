using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SSHServer.Common;

namespace SSHServer
{
    class Program
    {
        private static bool _isRunning;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                e.Cancel = true;
                _isRunning = false;
            };

            var appConfiguration = new AppConfiguration();

            var services = new ServiceCollection();
            services.AddLogging((builder) => 
            {
                builder
                    .AddConfiguration(appConfiguration.Configuration.GetSection("Logging"))
                    .AddConsole()
                    .AddDebug();
            });
            services.AddSingleton<AppConfiguration>(appConfiguration);
            services.AddTransient<Server>();

            var container = services.BuildServiceProvider();

            var server = container.GetService<Server>();
            server.Start();
            _isRunning = true;

            while (_isRunning)
            {
                server.Poll();
                Thread.Sleep(25);
            }

            server.Stop();
        }
    }
}
