namespace ESTMS.API.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string message) : base(message: message)
    {
    }
}
