using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyAssistant.Apis.Expenses.Api.Resources.Appointments;
using Xunit;

namespace Investments.Tests;

public class AppointmentsControllerTests
{
    private readonly Mock<IAppointmentsService> _serviceMock = new();
    private readonly AppointmentsController _sut;

    public AppointmentsControllerTests()
    {
        _sut = new AppointmentsController(_serviceMock.Object, new Mock<ILogger<AppointmentsController>>().Object);
    }

    [Fact]
    public async Task AddAppointment_ReturnsOk_AndUsesProvidedTimestamp()
    {
        Appointment? added = null;
        _serviceMock.Setup(s => s.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                    .Callback<Appointment, CancellationToken>((a, _) => added = a)
                    .Returns(Task.CompletedTask);

        var providedTimestamp = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        var request = new Request
        {
            Title = "Dentist",
            Description = "Checkup",
            DateTime = new DateTime(2026, 2, 1, 9, 0, 0, DateTimeKind.Utc),
            Timestamp = providedTimestamp
        };

        var result = await _sut.AddAppointment(request, CancellationToken.None);

        Assert.IsType<OkResult>(result);
        Assert.NotNull(added);
        Assert.Equal("Dentist", added!.Title);
        Assert.Equal("Checkup", added.Description);
        Assert.Equal(request.DateTime, added.DateTime);
        Assert.Equal(providedTimestamp, added.Timestamp);
    }

    [Fact]
    public async Task AddAppointment_DefaultsTimestamp_WhenNotProvided()
    {
        Appointment? added = null;
        _serviceMock.Setup(s => s.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
                    .Callback<Appointment, CancellationToken>((a, _) => added = a)
                    .Returns(Task.CompletedTask);

        var request = new Request { Title = "Dentist", DateTime = DateTime.UtcNow.AddDays(1) };

        var before = DateTime.UtcNow;
        await _sut.AddAppointment(request, CancellationToken.None);
        var after = DateTime.UtcNow;

        Assert.NotNull(added);
        Assert.InRange(added!.Timestamp, before, after);
    }

    [Fact]
    public async Task GetUpcomingAppointments_ReturnsOk_WithList()
    {
        var appointments = new List<Appointment>
        {
            new() { Id = "1", Title = "A", DateTime = DateTime.UtcNow.AddDays(1) },
            new() { Id = "2", Title = "B", DateTime = DateTime.UtcNow.AddDays(2) }
        };
        _serviceMock.Setup(s => s.GetUpcomingAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(appointments);

        var result = await _sut.GetUpcomingAppointments(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsAssignableFrom<IEnumerable<Appointment>>(ok.Value);
        Assert.Equal(2, response.Count());
    }
}
