using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Simplify.SeedWork
{
    public interface ISimplifyOptions
    {
        string Id { get; }
        string Slug { get; }
        string Name { get; }
        string BaseUrl { get; }
        string Version { get; }
        string DataDirectory { get; }
        Hashtable? Custom { get; }
    }

    public class SimplifyOptions : ISimplifyOptions
    {
        public const string ConfigurationSectionKey = "Core";

        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-z0-9_]*$")]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,50}$")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Base URL")]
        [Required]
        [Url(ErrorMessage = "Invalid Base URL")]
        public string BaseUrl { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        [Required]
        public string DataDirectory { get; set; } = string.Empty;

        public Hashtable? Custom { get; set; }
    }
}