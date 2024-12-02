
using Application.Responses;
using Domain.Entities;

namespace Application
{
    public interface IAuthRepository
    {
        Task<AuthResponseDTO> Login(string username, string password);
        Task<AuthResponseDTO> CreateUser(string username, string password);
    }
}
