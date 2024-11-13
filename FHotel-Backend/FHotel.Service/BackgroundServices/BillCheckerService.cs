using FHotel.Service.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace FHotel.Service.BackgroundServices
{
    //public class BillCheckerService : BackgroundService
    //{
    //    private readonly IServiceProvider _services;

    //    public BillCheckerService(IServiceProvider services)
    //    {
    //        _services = services;
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            // Check enrollments and update wallet here
    //            using (var scope = _services.CreateScope())
    //            {
    //                var billService = scope.ServiceProvider.GetRequiredService<IBillService>();
    //                await billService.CheckAndProcessBillsAsync();
    //            }

    //            // Wait for 60 seconds before next check
    //            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
    //        }
    //    }
    //}
}
