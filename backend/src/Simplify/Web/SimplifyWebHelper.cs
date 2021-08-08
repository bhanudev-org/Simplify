using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;

namespace Simplify.Web
{
    public static class SimplifyWebHelper
    {
        public static readonly Assembly Assembly = typeof(SimplifyWebHelper).Assembly;

        public static string CreateMD5(string input)
        {
            Guard.NotNull(input);
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if(name != null)
            {
                var field = type.GetField(name);
                if(field != null)
                {
                    var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    return attr?.Description ?? string.Empty;
                }
            }

            return string.Empty;
        }
    }
}