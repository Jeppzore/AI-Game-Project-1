namespace Server.Models;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public string? Error { get; set; }
}
