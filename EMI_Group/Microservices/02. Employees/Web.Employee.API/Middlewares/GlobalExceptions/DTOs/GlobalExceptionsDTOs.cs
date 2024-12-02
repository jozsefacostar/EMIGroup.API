namespace Web.Employee.API.Middlewares.GlobalExceptions.DTOs;
public class EmailData
{
    public string? Subject { get; set; }
    public string? To { get; set; }
    public string? Body { get; set; }
}

public class Error
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Data { get; set; }
    public string? Module { get; set; }
}
