using System;
using AirlineSendAgent.App;
using AirlineSendAgent.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                services.AddDbContext<SendAgentDbContext>(opt => opt.UseSqlServer
                    (context.Configuration.GetConnectionString("AirlineConnection")));

            }).Build();
            host.Services.GetService<IAppHost>().Run();
        }
    }
}
