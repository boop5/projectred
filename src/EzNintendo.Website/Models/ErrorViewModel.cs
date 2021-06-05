namespace EzNintendo.Website.Models
{
    public sealed class ErrorViewModel
    {
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string RequestId { get; set; }
    }
}