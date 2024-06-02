namespace SimpleProjectTemplate.SharedLibrary.Exceptions;

public static class ErrorCodes
{
    // Internal Errors
    public static readonly ErrorCode INTERNAL_ERROR = new(nameof(INTERNAL_ERROR), "");
    public static readonly ErrorCode APPLICATION_ERROR = new(nameof(APPLICATION_ERROR), "SimpleProjectTemplate.Application logic error!");
    public static readonly ErrorCode DB_UPDATE_ERROR = new(nameof(DB_UPDATE_ERROR), "Could not finish database operation successfully");
    
    // Generic Errors
    public static readonly ErrorCode VALIDATION_ERROR = new(nameof(VALIDATION_ERROR), "Validation Error");
    public static readonly ErrorCode NOT_FOUND = new(nameof(NOT_FOUND), "NOT_FOUND");
    public static readonly ErrorCode DUPLICATE_ENTITY = new(nameof(DUPLICATE_ENTITY), "DUPLICATE_ENTITY");

    // User Errors
    public static readonly ErrorCode USER_NOT_VERIFIED = new(nameof(USER_NOT_VERIFIED), "User needs to be verified for this request");
    public static readonly ErrorCode USER_PASSWORD_RESET_REQUEST_EXPIRED = new(nameof(USER_PASSWORD_RESET_REQUEST_EXPIRED), "Password reset request expired");
    public static readonly ErrorCode USER_EMAIL_TAKEN = new (nameof(USER_EMAIL_TAKEN), "This email address is already taken");
    public static readonly ErrorCode USER_NOT_VALID_GENDER = new(nameof(USER_NOT_VALID_GENDER), "Not a valid gender");
}