using Application.Responses;
using AutoMapper;
using Domain.Interfaces.IEmployee;
using MediatR;
using Shared;

namespace Application.Modules.Employee.Queries
{
    public record GetEmployeeByIdQuery(string IdNum) : IRequest<RequestResult<EmployeeResponseDTO>>;
    internal sealed class GetAllEmployeesByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, RequestResult<EmployeeResponseDTO>>
    {
        private readonly IReadEmployeeRepository _IReadEmployeeRepository;
        private readonly IMapper _mapper;

        public GetAllEmployeesByIdQueryHandler(IReadEmployeeRepository IReadEmployeeRepository, IMapper mapper)
        {
            _IReadEmployeeRepository = IReadEmployeeRepository ?? throw new ArgumentNullException(nameof(IReadEmployeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<RequestResult<EmployeeResponseDTO>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var resultEmployee = await _IReadEmployeeRepository.GetById(query.IdNum);
            if (resultEmployee == null) return RequestResult<EmployeeResponseDTO>.SuccessResultNoRecords();
            var EmployeeResponse = _mapper.Map<EmployeeResponseDTO>(resultEmployee);
            return RequestResult<EmployeeResponseDTO>.SuccessResult(EmployeeResponse);
        }
    }
}

