namespace Backend.Shared.Response {
    public class FailResponse<T> {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
        public T Errors { get; set; }

        public FailResponse(T errors, string message = null) {
            Errors = errors;
            Message = message;
        }
    }
}