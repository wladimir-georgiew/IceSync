using Microsoft.AspNetCore.Mvc;

namespace IceSync.WebApi.Controllers;

public class BaseController : Controller
{
    protected IActionResult Success<T>(T data)
    {
        return Json(new BaseResponse<T>()
        {
            Success = true,
            Data = data,
        });
    }
    
    protected IActionResult Success()
    {
        return Json(new BaseResponse()
        {
            Success = true,
        });
    }

    protected IActionResult Failure(string errorMessage = "An error occurred while processing the request.")
    {
        return Json(new BaseResponse()
        {
            Success = false,
            Error = new Error()
            {
                Message =
                    $"Request unsuccessful. Reason: {errorMessage}",
                Code = 500,
            }
        });
    }
    
    private record BaseResponse<T>
    {
        public required bool Success { get; init; }
        public T? Data { get; init; }
        public Error? Error { get; init; }
    }

    private record BaseResponse
    {
        public bool Success { get; init; }
        public Error? Error { get; init; }
    }

    private record Error
    {
        public string Message { get; init; }
        public int Code { get; init; }
    }
}