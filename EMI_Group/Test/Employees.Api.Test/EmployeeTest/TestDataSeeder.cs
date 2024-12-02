using Domain.ValueObjects;
using Infraestructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees.Api.Test.EmployeeTest
{
    public static class TestDataSeeder
    {
        public static void SeedData(ApplicationDbContext context)
        {
            #region Projects
            if (!context.Projects.Any())
            {
                context.Projects.AddRange(
                    new List<Domain.Entities.Project>()
                    {
                    new Domain.Entities.Project { Name = "COLOMBIA PROYECTO 1", Code = "CP1", AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true), DateRange = new DateRange(new DateTime(2020, 01, 01), new DateTime(2025, 10, 10)) },
                    new Domain.Entities.Project { Name = "COLOMBIA PROYECTO 2", Code = "CP2", AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true), DateRange = new DateRange(new DateTime(2020, 01, 01), new DateTime(2025, 10, 10)) }
                    });
                context.SaveChanges();
            }
            #endregion

            #region users
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new List<Domain.Entities.User>()
                    {
                    new Domain.Entities.User { Username = "Admin1234*", PasswordHash = "password_hash", UserRole = 1, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                    new Domain.Entities.User { Username = "User1234*", PasswordHash = "password_hash", UserRole = 2, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) }
                    });
                context.SaveChanges();
            }
            #endregion

            #region Employees
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(new List<Domain.Entities.Employee>()
            {
                new Domain.Entities.Employee(){ IdNum = "1102855259", Name = "Paulo Dasilva", CurrentPosition = 1, Salary = 50000000, AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) },
                new Domain.Entities.Employee(){ IdNum = "1102821531", Name = "Veronica Cohelo", CurrentPosition = 2, Salary = 40000000 , AuditInfo = new AuditInfo(DateTime.Now, null, null, null, true) }
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
        }
    }
}
