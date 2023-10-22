namespace CMOC.Services;

public class ServiceResponse<T>
{
    public ServiceResult Result { get; set; }
    public T? Payload { get; set; }
    public string Message { get; set; } = string.Empty;
}