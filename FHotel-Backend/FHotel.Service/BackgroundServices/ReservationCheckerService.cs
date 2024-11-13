using FHotel.Service.Services.Interfaces;
using FHotel.Services.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.BackgroundServices
{
    public class ReservationCheckerService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public ReservationCheckerService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Check enrollments and update wallet here
                using (var scope = _services.CreateScope())
                {
                    var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
                    await reservationService.CheckAndProcessReservationsAsync();
                }

                // Wait for 60 seconds before next check
                await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
            }
        }
    }
}
