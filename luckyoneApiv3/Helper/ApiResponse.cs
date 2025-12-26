namespace luckyoneApiv3.Helper
{
    public class ApiResponse
    {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }
    }



}
