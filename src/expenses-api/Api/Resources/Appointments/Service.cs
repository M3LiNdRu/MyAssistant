using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyAssistant.Apis.Expenses.Api.Resources.Appointments
{
    public interface IAppointmentsService
    {
        Task AddAsync(Appointment appointment, CancellationToken cancellationToken);
        Task<IEnumerable<Appointment>> GetUpcomingAsync(CancellationToken cancellationToken);
    }

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IAppointmentsRepository _repository;
        private readonly ILogger<IAppointmentsService> _logger;

        public AppointmentsService(ILogger<IAppointmentsService> logger, IAppointmentsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public Task AddAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            return _repository.AddAsync(appointment, cancellationToken);
        }

        public Task<IEnumerable<Appointment>> GetUpcomingAsync(CancellationToken cancellationToken)
        {
            return _repository.GetUpcomingAsync(DateTime.UtcNow, cancellationToken);
        }
    }
}
