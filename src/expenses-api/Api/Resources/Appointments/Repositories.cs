using Library.MongoDb;
using Library.MongoDb.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyAssistant.Apis.Expenses.Api.Resources.Appointments
{
    public interface IAppointmentsRepository
    {
        Task AddAsync(Appointment appointment, CancellationToken cancellationToken);
        Task<IEnumerable<Appointment>> GetUpcomingAsync(DateTime from, CancellationToken cancellationToken);
    }

    public class MongoDbAppointmentsRepository : DataStore<Appointment>, IAppointmentsRepository
    {
        public MongoDbAppointmentsRepository(IOptions<DbConfigurationSettings> options) : base(options, collection: "Appointments")
        {
        }

        public Task AddAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            return base.InsertAsync(appointment, cancellationToken);
        }

        public async Task<IEnumerable<Appointment>> GetUpcomingAsync(DateTime from, CancellationToken cancellationToken)
        {
            var appointments = await base.FindAllAsync(a => a.DateTime >= from, cancellationToken);
            return appointments.OrderBy(a => a.DateTime);
        }
    }
}
