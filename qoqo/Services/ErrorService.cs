using Microsoft.AspNetCore.Mvc;

namespace qoqo.Services;

public static class ErrorService
{
    public static ActionResult BadRequest(string message)
    {
        var err = new BadRequestError { Message = message };
        return new BadRequestObjectResult(err);
    }
}

public class BadRequestError
{
    public string Message { get; set; }
}