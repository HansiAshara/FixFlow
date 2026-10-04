namespace FixFlow.Api.Common.Exceptions;

public abstract class AppException : Exception
{
    public string Code { get; }

    protected AppException(string code, string message) : base(message)
    {
        Code = code;
    }
}

public class ValidationException : AppException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("Validation.Error", "One or more validation failures occurred.")
    {
        Errors = errors.ToDictionary(k => k.Key, v => v.Value);
    }
}

public class NotFoundException : AppException
{
    public NotFoundException(string entityName, object key)
        : base("Resource.NotFound", $"{entityName} with key '{key}' was not found.")
    {
    }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message = "You do not have permission to access this resource.")
        : base("Auth.Forbidden", message)
    {
    }
}

public class ConflictException : AppException
{
    public ConflictException(string message)
        : base("Resource.Conflict", message)
    {
    }
}
