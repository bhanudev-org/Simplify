using System.ComponentModel;
using System.Globalization;
using Simplify.SeedWork.Domain;

namespace Simplify.SeedWork.TypeConverters
{
    public sealed class DomainIdTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(string) && sourceType == typeof(Guid);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => destinationType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if(value is string text) return DomainId.Create(text);

            if(value is Guid guid) return DomainId.Create(guid);

            return DomainId.Empty;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) => value.ToString()!;
    }
}