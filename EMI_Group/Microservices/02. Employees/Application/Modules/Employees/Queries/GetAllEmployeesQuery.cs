using Application.Responses;
using AutoMapper;
using Domain.Interfaces.IEmployee;
using MediatR;
using Shared;

namespace Application.Modules.Employee.Queries
{
    public record GetAllEmployeesQuery(int? pageNumber = null, int? pageSize = null) : IRequest<RequestResult<List<EmployeeResponseDTO>>>;
    internal sealed class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, RequestResult<List<EmployeeResponseDTO>>>
    {
        private readonly IReadEmployeeRepository _IReadEmployeeRepository;
        private readonly IMapper _mapper;

        public GetAllEmployeesQueryHandler(IReadEmployeeRepository IReadEmployeeRepository, IMapper mapper)
        {
            _IReadEmployeeRepository = IReadEmployeeRepository ?? throw new ArgumentNullException(nameof(IReadEmployeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<RequestResult<List<EmployeeResponseDTO>>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            var resultEmployees = await _IReadEmployeeRepository.GetAll(query.pageNumber, query.pageSize);

            if (resultEmployees?.Count == 0) return RequestResult<List<EmployeeResponseDTO>>.SuccessResultNoRecords(this.GetType().Name);
            var EmployeeResponse = _mapper.Map<List<EmployeeResponseDTO>>(resultEmployees);
            return RequestResult<List<EmployeeResponseDTO>>.SuccessResult(EmployeeResponse);
        }
    }
}

