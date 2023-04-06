namespace TaskAPI.Data
{
    public class ServiceResponse<T>
    {
        public List<string> Error { get; set; } = new List<string>();
        public string Data { get; internal set; }
        public bool Success { get; internal set; }
        public string Message { get; internal set; }
    }

}