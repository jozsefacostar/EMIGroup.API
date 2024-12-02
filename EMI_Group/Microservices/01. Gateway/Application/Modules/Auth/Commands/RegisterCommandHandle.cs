using Application.Responses;
using MediatR;
using Shared;

namespace Application.Modules.Auth.Commands;
public record RegisterCommand(string userName, string password) : IRequest<RequestResult<AuthResponseDTO>>;

public sealed class RegisterCommandHandle : IRequestHandler<RegisterCommand, RequestResult<AuthResponseDTO>>
{
    private readonly IAuthRepository _IAuthRepository;


    public RegisterCommandHandle(IAuthRepository IAuthRepository)
    {
        _IAuthRepository = IAuthRepository ?? throw new ArgumentNullException(nameof(IAuthRepository));
    }

    public async Task<RequestResult<AuthResponseDTO>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var createUser = await _IAuthRepository.CreateUser(command.userName, command.password);
        if (createUser.success)
            return RequestResult<AuthResponseDTO>.SuccessRecord();
        else
            return RequestResult<AuthResponseDTO>.ErrorResult(createUser.message);
    }
}

