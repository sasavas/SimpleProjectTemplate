namespace SimpleProjectTemplate.Api.DTOs.User;

public record LoginResponseDTO(Guid UserId, string JWT, DateTime Expiry);
