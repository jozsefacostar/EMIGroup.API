namespace Application.Responses;
public record AuthResponseDTO(bool success, string message, string userName, string? token, DateTime? dateExpire);

