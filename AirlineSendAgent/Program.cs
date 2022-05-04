using System;
using AirlineSendAgent.App;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AirlineSendAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
            {
                services.AddSingleton<IAppHost, AppHost>();

            }).Build();
            host.Services.GetService<IAppHost>().Run();
        }
    }
}
