namespace Backend.Shared.Response {
    public class SuccessResponse<T> {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }

        public SuccessResponse(T data, string message = null) {
            Data = data;
            Message = message;
        }
    }
}
