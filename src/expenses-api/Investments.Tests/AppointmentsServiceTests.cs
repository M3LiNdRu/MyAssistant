using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MyAssistant.Apis.Expenses.Api.Resources.Appointments;
using Xunit;

namespace Investments.Tests;

public class AppointmentsServiceTests
{
    private readonly Mock<IAppointmentsRepository> _repoMock = new();
    private readonly Mock<ILogger<IAppointmentsService>> _loggerMock = new();
    private readonly AppointmentsService _sut;

    public AppointmentsServiceTests()
    {
        _sut = new AppointmentsService(_loggerMock.Object, _repoMock.Object);
    }

    [Fact]
    public async Task AddAsync_DelegatesToRepository()
    {
        var appointment = new Appointment { Title = "Dentist", DateTime = DateTime.UtcNow.AddDays(1) };
        _repoMock.Setup(r => r.AddAsync(appointment, It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        await _sut.AddAsync(appointment, CancellationToken.None);

        _repoMock.Verify(r => r.AddAsync(appointment, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUpcomingAsync_DelegatesToRepository_WithCurrentUtcTime()
    {
        var appointments = new List<Appointment>
        {
            new() { Id = "1", Title = "A", DateTime = DateTime.UtcNow.AddDays(1) }
        };
        DateTime? capturedFrom = null;
        _repoMock.Setup(r => r.GetUpcomingAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                 .Callback<DateTime, CancellationToken>((from, _) => capturedFrom = from)
                 .ReturnsAsync(appointments);

        var before = DateTime.UtcNow;
        var result = await _sut.GetUpcomingAsync(CancellationToken.None);
        var after = DateTime.UtcNow;

        Assert.Same(appointments, result);
        Assert.NotNull(capturedFrom);
        Assert.InRange(capturedFrom.Value, before, after);
    }
}
