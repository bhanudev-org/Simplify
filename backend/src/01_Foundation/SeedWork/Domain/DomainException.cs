using System.Runtime.Serialization;

namespace Simplify.SeedWork.Domain
{
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }

        public DomainException(string message, Exception? inner) : base(message, inner) { }

        protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}