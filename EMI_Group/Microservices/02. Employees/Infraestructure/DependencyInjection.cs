using Application.Data;
using Domain.Interfaces.IEmployee;
using Domain.Interfaces.IEmployeeTransactions;
using Domain.Interfaces.IPublishMessage;
using Domain.Primitives;
using Domain.ValueObjects;
using Infraestructure.Configurations;
using Infraestructure.Messaging;
using Infraestructure.Messaging.Interface;
using Infraestructure.Messaging.Proccess;
using Infraestructure.Persistence;
using Infraestructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthSettings>(configuration.GetSection("Authorization"));
            services.AddPersistence(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));
            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            //Employee
            services.AddScoped<IWriteEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IReadEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDeleteEmployeeRepository, EmployeeRepository>();

            //EmployeeTransactions
            services.AddScoped<IEmployeeTransactionRepository, EmployeeTransactionRepository>();

            // RabbitMQ: Registramos los procesadores disponibles
            services.AddTransient<IQueueProcessor, EmailQueueProcessor>();
            services.AddTransient<IQueueProcessor, ErrorLogQueueProcessor>();
            services.AddTransient<IMessageQueueService, RabbitMQService>();
            services.AddTransient<QueueProcessorFactory>();
            return services;
        }


        public static IServiceCollection SeedData(this IServiceCollection services, ApplicationDbContext context)
        {
            #region Projects
            if (!context.Projects.Any())
            {
                context.Projects.AddRange(
                   new List<Domain.Entities.Project>() {
                new Domain.Entities.Project { Name = "COLOMBIA PROYECTO 1",Code = "CP1",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true),  DateRange = new DateRange(new DateTime(2020, 01,01),new DateTime(2025, 10,10)) },
                new Domain.Entities.Project { Name = "COLOMBIA PROYECTO 2",Code = "CP2",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) , DateRange = new DateRange(new DateTime(2020, 01,01),new DateTime(2025, 10,10)) },
                new Domain.Entities.Project { Name = "COLOMBIA PROYECTO 3",Code = "CP3",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true),  DateRange = new DateRange(new DateTime(2020, 01,01),new DateTime(2025, 10,10)) },
                new Domain.Entities.Project { Name = "CHILE PROYECTO 1",Code = "CC1",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) , DateRange = new DateRange(new DateTime(2020, 01, 01), new DateTime(2025, 10,10))},
                new Domain.Entities.Project { Name = "CHILE PROYECTO 2",Code = "CC2",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) ,  DateRange = new DateRange(new DateTime(2020, 01,01),new DateTime(2025, 10,10)) }
                   });
                context.SaveChanges();
            }
            #endregion

            #region UserRoles
            if (!context.UserRoles.Any())
            {
                context.UserRoles.AddRange(
                   new List<UserRole>() { new UserRole { Name = "Admin", Code = "ADM", AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) }, new UserRole { Name = "User", Code = "USR", AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) } });
                context.SaveChanges();
            }
            #endregion

            #region Department
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                   new List<Domain.Entities.Department>() {
                new Domain.Entities.Department { Name = "PRESIDENCIA",Code = "PRE",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Department { Name = "GERENCIA",Code = "GER",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Department { Name = "TECNOLOGIA",Code = "TEC",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Department { Name = "TESORERIA",Code = "TES",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Department { Name = "SALUD",Code = "SAL",
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) }
                   });
                context.SaveChanges();
            }
            #endregion

            #region Positions
            if (!context.Positions.Any())
            {
                context.Positions.AddRange(new List<Domain.Entities.Position>() {
                new Domain.Entities.Position(){ Name = "Presidente", RegularJob = true, Department = 1, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Position(){ Name = "Gerente de Fabrica Software", Department = 2, RegularJob = true, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true)  },
                new Domain.Entities.Position(){ Name = "Gerente de Operaciones", Department = 2, RegularJob = true, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Position(){ Name = "Gerente de Gestión Humana", Department = 2, RegularJob = true, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Position(){ Name = "Ingeniero Backend", Department = 3, RegularJob = false, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Position(){ Name = "Ingeniero Fullstack",  Department = 3,  RegularJob = false, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true)},
                new Domain.Entities.Position(){ Name = "Analista de compras",  Department = 4,  RegularJob = false, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Position(){ Name = "Médico", RegularJob = false, Department = 5,  AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Position(){ Name = "Enfermero", RegularJob = false, Department = 5,  AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true)  }
            });
                context.SaveChanges();
            }
            #endregion

            #region User
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                   new List<Domain.Entities.User>() {
                        new Domain.Entities.User { Username = "Admin1234*", PasswordHash = "89Oz2umuXizTXeorm2uUVCxIwTe/FeNtzjhsbjQj0NbyZgxWsWlQPU5SDS6DG6ik", UserRole = 1, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                        new Domain.Entities.User { Username = "User1234*", PasswordHash = "XjYgnaS5Owuu7JefrkrzNQXVkrHunR6suQobVkwwzmypEBozkfx7OmP2uGT36WGy", UserRole = 2, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) } });
                context.SaveChanges();
            }
            #endregion

            #region Employees
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(new List<Domain.Entities.Employee>() {
            new Domain.Entities.Employee(){ IdNum = "1102855259", Name = "Paulo Dasilva", CurrentPosition = 1, Salary = 50000000, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
            new Domain.Entities.Employee(){ IdNum = "1102821531", Name = "Veronica Cohelo", CurrentPosition = 2, Salary = 40000000 , AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
            new Domain.Entities.Employee(){  IdNum = "1120821531", Name = "Pedro Cortez", CurrentPosition = 3, Salary = 3500000, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
            new Domain.Entities.Employee() {  IdNum = "1102855250", Name = "Jozsef Acosta", CurrentPosition = 5, Salary = 11000000,AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
            new Domain.Entities.Employee() {  IdNum = "1102855255", Name = "Luis Moreno", CurrentPosition = 7, Salary = 10000000,AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
            });
                context.SaveChanges();
            }
            #endregion

            #region PositionHistory
            if (!context.PositionHistories.Any())
            {
                context.PositionHistories.AddRange(
                   new List<Domain.Entities.PositionHistory>() {
                new Domain.Entities.PositionHistory
                { EmployeeId = 1, PositionId = 1, DateRange = new DateRange(new DateTime(2020, 01,01),null),
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },

                new Domain.Entities.PositionHistory
                {  EmployeeId = 2, PositionId = 2, DateRange = new DateRange(new DateTime(2020, 01,01),null),
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },

                new Domain.Entities.PositionHistory
                {  EmployeeId = 3, PositionId = 3, DateRange = new DateRange(new DateTime(2020, 01,01),null),
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },

                new Domain.Entities.PositionHistory
                { EmployeeId = 4, PositionId = 5, DateRange = new DateRange(new DateTime(2020, 01,01),null),
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },

                new Domain.Entities.PositionHistory
                {   EmployeeId = 5, PositionId = 7, DateRange = new DateRange(new DateTime(2020, 01,01),null),
                    AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                   });
                context.SaveChanges();
            }
            #endregion

            #region Employee Projects
            if (!context.EmployeeProjects.Any())
            {
                context.EmployeeProjects.AddRange(
                   new List<Domain.Entities.EmployeeProject>() {
                        new Domain.Entities.EmployeeProject (1,1),
                        new Domain.Entities.EmployeeProject (1,2),
                        new Domain.Entities.EmployeeProject (1,2),
                        new Domain.Entities.EmployeeProject (1,3),
                        new Domain.Entities.EmployeeProject (1,4),
                        new Domain.Entities.EmployeeProject (1,5),
                        new Domain.Entities.EmployeeProject (2,1),
                        new Domain.Entities.EmployeeProject (2,2),
                        new Domain.Entities.EmployeeProject (2,3),
                        new Domain.Entities.EmployeeProject (3,3),
                        new Domain.Entities.EmployeeProject (3,2),
                        new Domain.Entities.EmployeeProject (4,1),
                        new Domain.Entities.EmployeeProject (5,2),
                        new Domain.Entities.EmployeeProject (5,2),
                        new Domain.Entities.EmployeeProject (5,3),
                        new Domain.Entities.EmployeeProject (5,4),
                        new Domain.Entities.EmployeeProject (5,1),
                   });
                context.SaveChanges();
            }
            #endregion




            return services;
        }

    }
}
