using Domain.Entities;

namespace Application.Responses;
public record EmployeeResponseDTO(int IdDto, string IdNumDto, string NameDto, decimal SalaryDto, string PositionDto, List<PositionHistory> positionHistories, List<EmployeeProject> employeeProjects);

