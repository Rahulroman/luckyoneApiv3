namespace luckyoneApiv3.Helper
{
    public class ApiResponse
    {
            public bool success { get; set; }
            public string? message { get; set; }
            public DateTime timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? data { get; set; }
    }


    public class PaginatedResponse<T>
    { 
        public List<T>? data { get; set; }
        public int? total { get; set; }
        public int? page { get; set; }

        public int? limit { get; set; }

        public int? totalPages { get; set; }

    }



}
