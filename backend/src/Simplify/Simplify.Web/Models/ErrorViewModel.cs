#nullable disable

namespace Simplify.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class MetaViewModel
    {
        public MetaViewModel() => Version = typeof(MetaViewModel).Assembly.GetName().Version?.ToString() ?? string.Empty;

        public string Version { get; }
    }
}