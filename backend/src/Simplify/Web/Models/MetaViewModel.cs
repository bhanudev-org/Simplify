namespace Simplify.Web.Models
{
    public class MetaViewModel
    {
        public MetaViewModel() => Version = typeof(MetaViewModel).Assembly.GetName().Version?.ToString() ?? string.Empty;

        public string Version { get; }
    }
}