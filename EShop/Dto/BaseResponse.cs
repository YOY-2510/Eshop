namespace EShop.Dto
{
    public class BaseResponse<T>
    {
        public bool Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }
    
    public static BaseResponse<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new BaseResponse<T>
            {
                Status = true,
                Message = message,
                Data = data
            };
        }
        public static BaseResponse<T> FailResponse(string message = "Operation failed", T? data = default)
        {
            return new BaseResponse<T>
            {
                Status = false,
                Message = message,
                Data = data
            };
        }
    }
}
