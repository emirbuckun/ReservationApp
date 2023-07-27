namespace ReservationApp.Api.Entity
{
  public class Response
  {
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
    public Response() { Message = string.Empty; }
    public Response(bool isSuccess, string message)
    {
      Message = message;
      IsSuccess = isSuccess;
    }
  }
}