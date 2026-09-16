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
    public async Task GetByMonthAsync_DelegatesToRepository_WithFirstDayOfGivenMonth()
    {
        var appointments = new List<Appointment>
        {
            new() { Id = "1", Title = "A", DateTime = new DateTime(2026, 3, 5) }
        };
        DateTime? capturedDate = null;
        _repoMock.Setup(r => r.GetByMonthAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                 .Callback<DateTime, CancellationToken>((date, _) => capturedDate = date)
                 .ReturnsAsync(appointments);

        var result = await _sut.GetByMonthAsync(2026, 3, CancellationToken.None);

        Assert.Same(appointments, result);
        Assert.Equal(new DateTime(2026, 3, 1), capturedDate);
    }
}
