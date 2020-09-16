using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Simplify.SeedWork.TypeConverters;

namespace Simplify.SeedWork.Domain
{
    [TypeConverter(typeof(DomainIdTypeConverter))]
    public readonly struct DomainId : IEquatable<DomainId>, IComparable<DomainId>
    {
        private static readonly string _emptyString = Guid.Empty.ToString();
        public static readonly DomainId Empty = default;

        private readonly string? _id;

        private DomainId(string id) => _id = id;

        public static DomainId? CreateNullable(string? value)
        {
            if(value == null) return null;

            return new DomainId(value);
        }

        public static DomainId Create(string value)
        {
            if(value == null || string.Equals(value, _emptyString, StringComparison.OrdinalIgnoreCase)) return Empty;

            return new DomainId(value);
        }

        public static DomainId Create(Guid value)
        {
            if(value == Guid.Empty) return Empty;

            return new DomainId(value.ToString());
        }

        public override bool Equals(object? obj) => obj is DomainId status && Equals(status);

        public bool Equals(DomainId other) => string.Equals(_id, other._id);

        public override int GetHashCode() => _id?.GetHashCode() ?? 0;

        public override string ToString() => _id ?? _emptyString;

#if NET5_0
public int CompareTo([AllowNull] DomainId other) => string.Compare(_id, other._id, StringComparison.Ordinal);
#elif NETSTANDARD2_0
        public int CompareTo(DomainId other) => string.Compare(_id, other._id, StringComparison.Ordinal);
#endif
        
        public static implicit operator DomainId(string value) => Create(value);

        public static implicit operator DomainId(Guid value) => Create(value);

        public static bool operator ==(DomainId lhs, DomainId rhs) => lhs.Equals(rhs);

        public static bool operator !=(DomainId lhs, DomainId rhs) => !lhs.Equals(rhs);

        public static DomainId NewGuid() => new DomainId(Guid.NewGuid().ToString());

        public static DomainId Combine(DomainId id1, DomainId id2) => new DomainId($"{id1}--{id2}");
    }
}