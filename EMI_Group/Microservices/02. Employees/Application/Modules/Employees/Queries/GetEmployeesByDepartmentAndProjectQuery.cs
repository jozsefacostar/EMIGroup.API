using Application.Responses;
using AutoMapper;
using Domain.Interfaces.IEmployee;
using MediatR;
using Shared;

namespace Application.Modules.Employee.Queries
{
    public record GetEmployeesByDepartmentAndProjectQuery(string codeDeparment) : IRequest<RequestResult<EmployeeResponseDTO>>;
    internal sealed class GetEmployeesByDepartmentAndProjectQueryHandler : IRequestHandler<GetEmployeesByDepartmentAndProjectQuery, RequestResult<EmployeeResponseDTO>>
    {
        private readonly IReadEmployeeRepository _IReadEmployeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeesByDepartmentAndProjectQueryHandler(IReadEmployeeRepository IReadEmployeeRepository, IMapper mapper)
        {
            _IReadEmployeeRepository = IReadEmployeeRepository ?? throw new ArgumentNullException(nameof(IReadEmployeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<RequestResult<EmployeeResponseDTO>> Handle(GetEmployeesByDepartmentAndProjectQuery query, CancellationToken cancellationToken)
        {
            var resultEmployee = await _IReadEmployeeRepository.GetEmployeesByDepartmentAndProjectQuery(query.codeDeparment);
            if (resultEmployee == null) return RequestResult<EmployeeResponseDTO>.SuccessResultNoRecords();
            var EmployeeResponse = _mapper.Map<List<EmployeeResponseDTO>>(resultEmployee);
            return RequestResult<EmployeeResponseDTO>.SuccessResult(EmployeeResponse);
        }
    }
}

