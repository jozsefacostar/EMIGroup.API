using Application.Responses;
using Domain.Primitives;
using MediatR;
using Shared;

namespace Application.Modules.Auth.Commands;
public record AuthCommand(string userName, string password) : IRequest<RequestResult<AuthResponseDTO>>;

public sealed class AuthCommandHandle : IRequestHandler<AuthCommand, RequestResult<AuthResponseDTO>>
{
    private readonly IAuthRepository _IAuthRepository;


    public AuthCommandHandle(IAuthRepository IAuthRepository)
    {
        _IAuthRepository = IAuthRepository ?? throw new ArgumentNullException(nameof(IAuthRepository));
    }

    public async Task<RequestResult<AuthResponseDTO>> Handle(AuthCommand command, CancellationToken cancellationToken)
    {
        var getLogin = await _IAuthRepository.Login(command.userName, command.password);
        if (getLogin.success)
            return RequestResult<AuthResponseDTO>.SuccessResult(getLogin);
        else
            return RequestResult<AuthResponseDTO>.ErrorResult(getLogin.message);
    }
}

