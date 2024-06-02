namespace SimpleProjectTemplate.Api.DTOs.User;

public record OnboardingRequestDTO(
    string TargetLanguageCode,
    string? FirstName,
    string? LastName,
    string? Gender,
    DateOnly? DateOfBirth);