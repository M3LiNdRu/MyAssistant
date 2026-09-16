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
/// <see cref="IAppointmentsRepository.GetByMonthAsync"/> implementations must honor,
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

    public Task<IEnumerable<Appointment>> GetByMonthAsync(DateTime date, CancellationToken cancellationToken)
    {
        var result = _buffer
            .Where(a => a.DateTime >= date && a.DateTime < date.AddMonths(1))
            .OrderBy(a => a.DateTime)
            .AsEnumerable();
        return Task.FromResult(result);
    }
}

public class InMemoryAppointmentsRepositoryTests
{
    private readonly FakeAppointmentsRepository _sut = new();

    [Fact]
    public async Task GetByMonthAsync_ExcludesAppointmentsOutsideMonth()
    {
        var month = new DateTime(2026, 3, 1);
        await _sut.AddAsync(new Appointment { Title = "PreviousMonth", DateTime = new DateTime(2026, 2, 28) }, CancellationToken.None);
        await _sut.AddAsync(new Appointment { Title = "InMonth", DateTime = new DateTime(2026, 3, 15) }, CancellationToken.None);
        await _sut.AddAsync(new Appointment { Title = "NextMonth", DateTime = new DateTime(2026, 4, 1) }, CancellationToken.None);

        var result = (await _sut.GetByMonthAsync(month, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("InMonth", result[0].Title);
    }

    [Fact]
    public async Task GetByMonthAsync_IncludesFirstInstantOfMonth_ExcludesFirstInstantOfNextMonth()
    {
        var month = new DateTime(2026, 3, 1);
        await _sut.AddAsync(new Appointment { Title = "FirstInstant", DateTime = new DateTime(2026, 3, 1, 0, 0, 0) }, CancellationToken.None);
        await _sut.AddAsync(new Appointment { Title = "NextMonthFirstInstant", DateTime = new DateTime(2026, 4, 1, 0, 0, 0) }, CancellationToken.None);

        var result = (await _sut.GetByMonthAsync(month, CancellationToken.None)).ToList();

        Assert.Single(result);
        Assert.Equal("FirstInstant", result[0].Title);
    }

    [Fact]
    public async Task GetByMonthAsync_ReturnsEmpty_WhenNoAppointmentsInMonth()
    {
        var month = new DateTime(2026, 3, 1);
        await _sut.AddAsync(new Appointment { Title = "OtherMonth", DateTime = new DateTime(2026, 5, 1) }, CancellationToken.None);

        var result = (await _sut.GetByMonthAsync(month, CancellationToken.None)).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByMonthAsync_ReturnsMultipleSameDayAppointments_OrderedByDateTimeAscending()
    {
        var month = new DateTime(2026, 3, 1);
        await _sut.AddAsync(new Appointment { Title = "Third", DateTime = new DateTime(2026, 3, 10, 15, 0, 0) }, CancellationToken.None);
        await _sut.AddAsync(new Appointment { Title = "First", DateTime = new DateTime(2026, 3, 10, 9, 0, 0) }, CancellationToken.None);
        await _sut.AddAsync(new Appointment { Title = "Second", DateTime = new DateTime(2026, 3, 10, 12, 0, 0) }, CancellationToken.None);

        var result = (await _sut.GetByMonthAsync(month, CancellationToken.None)).ToList();

        Assert.Equal(new[] { "First", "Second", "Third" }, result.Select(a => a.Title));
    }
}
