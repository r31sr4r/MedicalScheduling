using Moq;
using MedicalScheduling.Application.Services;
using MedicalScheduling.Application.Interfaces;
using MedicalScheduling.Domain.Entities;

namespace MedicalScheduling.Tests.Application.Services;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IDoctorRepository> _mockDoctorRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _mockAppointmentRepository = new Mock<IAppointmentRepository>();
        _mockDoctorRepository = new Mock<IDoctorRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _service = new AppointmentService(
            _mockDoctorRepository.Object,
            _mockAppointmentRepository.Object,
            _mockEmailService.Object
        );
    }

    [Trait("Category", "Scheduling")]
    [Fact]
    public async Task ScheduleAppointment_ShouldThrowException_WhenTimeNotAvailable()
    {
        // Arrange
        var unavailableDate = new DateTime(2024, 12, 25, 10, 0, 0);
        _mockAppointmentRepository
            .Setup(repo => repo.GetAppointmentsByDoctorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Appointment>
            {
                new Appointment { DateTime = unavailableDate }
            });

        var appointment = new Appointment
        {
            DateTime = unavailableDate,
            DoctorId = 1,
            PatientId = 1,
            Patient = new Patient { Name = "John", Email = "john@example.com" }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ScheduleAppointment(appointment));
    }


    [Trait("Category", "Cancellation")]
    [Fact]
    public async Task CancelAppointment_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var appointment = new Appointment { Id = 1, IsActive = true };
        _mockAppointmentRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(appointment);

        // Act
        await _service.CancelAppointmentAsync(1);

        // Assert
        Assert.False(appointment.IsActive);
        _mockAppointmentRepository.Verify(repo => repo.UpdateAsync(appointment), Times.Once);
    }

    [Trait("Category", "Retrieval")]
    [Fact]
    public async Task GetAppointmentsByPatient_ShouldReturnActiveFutureAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, IsActive = true, DateTime = DateTime.Now.AddDays(1) },
            new Appointment { Id = 2, PatientId = 1, IsActive = true, DateTime = DateTime.Now.AddDays(-1) },
            new Appointment { Id = 3, PatientId = 1, IsActive = false, DateTime = DateTime.Now.AddDays(1) }
        };

        _mockAppointmentRepository
            .Setup(repo => repo.GetAppointmentsByPatientIdAsync(It.IsAny<int>()))
            .ReturnsAsync(appointments);

        // Act
        var result = await _service.GetAppointmentsByPatientAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Equal(1, result.First().Id);
    }

    [Trait("Category", "Retrieval")]
    [Fact]
    public async Task GetAvailableDates_ShouldReturnOnlyAvailableDates()
    {
        // Arrange
        var today = DateTime.Today;
        var unavailableDate = today.AddHours(10);
        _mockAppointmentRepository
            .Setup(repo => repo.GetAppointmentsByDoctorIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Appointment>
            {
                new Appointment { DateTime = unavailableDate }
            });

        // Act
        var result = await _service.GetAvailableDatesAsync(1);

        // Assert
        Assert.DoesNotContain(result, date => date == unavailableDate);
    }
}
