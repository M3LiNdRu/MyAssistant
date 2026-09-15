using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyAssistant.Apis.Expenses.Api.Resources.Appointments;
using Xunit;

namespace Investments.Tests;

/// <summary>
/// In-memory fake used purely to unit test the ordering/filtering contract that
/// <see cref="IAppointmentsRepository.GetUpcomingAsync"/> implementations must honor,
/// since the production implementation talks to MongoDB.
/// </summary>
public class FakeAppointmentsRepository : IAppointmentsRepository
{
    private readonly List<Appointment> _buffer = new();

    public Task AddAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        appointment.Id ??= Guid.NewGuid().ToString();
        _buffer.Add(appointment);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Appointment>> GetUpcomingAsync(DateTime from, CancellationToken cancellationToken)
    {
        var result = _buffer
            .Where(a => a.DateTime >= from)
            .OrderBy(a => a.DateTime)
            .AsEnumerable();
        return Task.FromResult(result);
    }
}

public class InMemoryAppointmentsRepositoryTests
{
    private readonly FakeAppointmentsRepository _sut = new();

    [Fact]
    public async Task GetUpcomingAsync_ExcludesPastAppointments()
    {
        var now = DateTime.UtcNow;
        await _sut.AddAsync(new Appointment { Title = "Past", DateTime = now.AddDays(-1) }, CancellationToken.None);
        await _sut.AddAsync(new Appointment { Title = "Future", DateTime = now.AddDays(1) }, CancellationToken.None);

        var result = (await _sut.GetUpcomingAsync(now, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("Future", result[0].Title);
    }

    [Fact]
    public async Task GetUpcomingAsync_ReturnsAppointments_OrderedByDateTimeAscending()
    {
        var now = DateTime.UtcNow;
        await _sut.AddAsync(new Appointment { Title = "Third", DateTime = now.AddDays(3) }, CancellationToken.None);
        await _sut.AddAsync(new Appointment { Title = "First", DateTime = now.AddDays(1) }, CancellationToken.None);
        await _sut.AddAsync(new Appointment { Title = "Second", DateTime = now.AddDays(2) }, CancellationToken.None);

        var result = (await _sut.GetUpcomingAsync(now, CancellationToken.None)).ToList();

        Assert.Equal(new[] { "First", "Second", "Third" }, result.Select(a => a.Title));
    }
}
