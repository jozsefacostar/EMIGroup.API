using Application.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeResponseDTO>()
                .ConstructUsing(src => new EmployeeResponseDTO(
                    src.Id,
                    src.IdNum,
                    src.Name,
                    src.Salary,
                    src.CurrentPositionNavigation.Name,
                    src.LstPositionHistoryEmployee.ToList(),
                    src.LstEmployeeEmployeeProject.ToList()
            ));

            CreateMap<List<Employee>, List<EmployeeResponseDTO>>()
               .ConvertUsing((src, dest) =>
                   src.Select(emp => new EmployeeResponseDTO(
                       emp.Id,
                       emp.IdNum,
                       emp.Name,
                       emp.Salary,
                       emp.CurrentPositionNavigation.Name,
                    emp.LstPositionHistoryEmployee.ToList(),
                    emp.LstEmployeeEmployeeProject.ToList()
                   )).ToList());


        }
    }
}
