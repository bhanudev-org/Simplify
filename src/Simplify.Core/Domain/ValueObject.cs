using System.Collections.Generic;
using System.Linq;

namespace Simplify.Core.Domain
{
    public abstract class ValueObject : IValueObject
    {
        protected abstract IEnumerable<object> GetAtomicValues();

        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
                return false;

            var valueObject = (ValueObject)obj;

            return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
        }

        public override int GetHashCode() =>
            GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if(a is null && b is null)
                return true;

            if(a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b) => !(a == b);

        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if(left is null ^ right is null) return false;
            return left is null || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !EqualOperator(left, right);

        public ValueObject GetCopy() => MemberwiseClone() as ValueObject;
    }
}