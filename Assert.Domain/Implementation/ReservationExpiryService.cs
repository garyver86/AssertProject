using Assert.Domain.Models;
using Assert.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class ReservationExpiryService : BackgroundService
    {
        private readonly ILogger<ReservationExpiryService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly AutomaticExecutingConfiguration _settings;


        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1); // Garantiza ejecución single-thread

        public ReservationExpiryService(
            ILogger<ReservationExpiryService> logger,
            IServiceProvider serviceProvider,
            IOptions<AutomaticExecutingConfiguration> settings)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _settings = settings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de expiración de reservas iniciado. Intervalo: {Interval} minutos",
                                  _settings.Res_CheckIntervalMinutes);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Usar semáforo para evitar ejecuciones concurrentes
                    if (await _semaphore.WaitAsync(TimeSpan.FromSeconds(5), stoppingToken))
                    {
                        try
                        {
                            using var scope = _serviceProvider.CreateScope();
                            var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();

                            await CheckAndExpireReservations(bookService);
                            await CheckAndFinishReservation(bookService);
                        }
                        finally
                        {
                            _semaphore.Release();
                        }
                    }
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "Error crítico en el servicio de expiración");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }

                await Task.Delay(TimeSpan.FromMinutes(_settings.Res_CheckIntervalMinutes), stoppingToken);
            }
        }

        private async Task CheckAndExpireReservations(IBookService _bookService)
        {
            var now = DateTime.UtcNow;
            var expirationThreshold = now.AddHours(-_settings.Res_ExpirationHours);
            await _bookService.CheckAndExpireReservation(expirationThreshold);
        }

        private async Task CheckAndFinishReservation(IBookService _bookService)
        {
            var now = DateTime.UtcNow;
            var expirationThreshold = now.AddHours(-_settings.Res_ExpirationHours);
            await _bookService.CheckAndFinishReservation(expirationThreshold);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deteniendo servicio de expiración de reservas");
            await base.StopAsync(cancellationToken);
        }
    }
}
