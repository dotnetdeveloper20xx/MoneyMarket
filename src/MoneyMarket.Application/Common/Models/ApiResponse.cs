namespace MoneyMarket.Application.Common.Models;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public string? ErrorCode { get; init; }

    public static ApiResponse<T> SuccessResult(T data, string? message = null)
        => new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(string message, string? errorCode = null)
        => new() { Success = false, Message = message, ErrorCode = errorCode };
}
