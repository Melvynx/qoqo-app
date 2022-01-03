using Microsoft.AspNetCore.Mvc;

namespace qoqo.Services;

public static class ErrorService
{
    public static ActionResult BadRequest(string message)
    {
        var err = new RequestMessage { Message = message };
        return new BadRequestObjectResult(err);
    }
}

public static class SuccessService
{
    public static ActionResult Ok(string message)
    {
        var msg = new RequestMessage { Message = message };
        return new OkObjectResult(msg);
    }
}

public class RequestMessage
{
    public string Message { get; set; }
}