namespace bebegim.Models
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
        public string Detail { get; set; } // Sadece geliştirme aşamasında doldurabilirsin
    }
}