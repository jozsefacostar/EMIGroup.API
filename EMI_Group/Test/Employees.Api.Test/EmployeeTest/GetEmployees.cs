using Domain.Primitives;
using Moq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Application;
using Application.Common.Behaviors;
using Application.Data;
using Microsoft.EntityFrameworkCore;
using Infraestructure.Persistence;
using Domain.Interfaces.IEmployee;
using Infraestructure.Persistence.Repositories;
using Application.Modules.Employees.Validators;
using Application.Modules.Employees.Commands;
using Employees.Api.Test.EmployeeTest;

namespace Order.Api.Tests.CustomerTest
{
    public class GestEmployess
    {
        private readonly Mock<IUnitOfWork> _iUnitOfWork;
        private readonly Mock<IWriteEmployeeRepository> _IWriteEmployeeRepository;
        private readonly IMediator _mediator;

        public GestEmployess()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IWriteEmployeeRepository, EmployeeRepository>();
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
            });
            services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            var serviceProvider = services.BuildServiceProvider();
            _mediator = serviceProvider.GetRequiredService<IMediator>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            TestDataSeeder.SeedData(context);
        }

        [Fact]
        public async Task ShouldReturnFailureIfTheNameIsEmpty()
        {
            // Arrange
            var command = new CreateEmployeeCommand("123456", "", 1, 568000000, DateTime.Now.AddYears(-1), null);

            // Act
            var result = await _mediator.Send(command);

            // Assert 
            Assert.False(result.Success);
            Assert.Equal("Validaciones: Name: 'Nombre' no debería estar vacío.", result.Message);
        }

        [Fact]
        public async Task ShouldReturnFailureIfTheIdentificaciónIsNull()
        {
            // Arrange
            var command = new CreateEmployeeCommand("", "Sandra Martinez", 1, 568000000, DateTime.Now.AddYears(-1), null);

            // Act
            var result = await _mediator.Send(command);

            // Assert 
            Assert.False(result.Success);
            Assert.Equal("Validaciones: IdNum: 'Identificación' no debería estar vacío.", result.Message);

        }
        [Fact]
        public async Task ShouldReturnFailureIfThePositionIsZero()
        {
            // Arrange
            var command = new CreateEmployeeCommand("110254851", "Sandra Martinez", 0, 568000000, DateTime.Now.AddYears(-1), null);

            // Act
            var result = await _mediator.Send(command);

            // Assert 
            Assert.False(result.Success);
            Assert.Equal("Validaciones: CurrentPosition: 'Posición actual' no debería estar vacío.,CurrentPosition: 'Posición actual' debe ser mayor que '0'.", result.Message);

        }
        [Fact]
        public async Task ShouldReturnFailureIfTheSalaryIsZero()
        {
            // Arrange
            var command = new CreateEmployeeCommand("110254851", "Sandra Martinez", 1, 0, DateTime.Now.AddYears(-1), null);

            // Act
            var result = await _mediator.Send(command);

            // Assert 
            Assert.False(result.Success);
            Assert.Equal("Validaciones: Salary: 'Salario' no debería estar vacío.,Salary: 'Salario' debe ser mayor que '0'.", result.Message);

        }
    }
}
