using Pro.Enum;

namespace Simplify.SeedWork
{
    public class Gender : Enumeration
    {
        public static readonly Gender Active = new Gender(1, "MALE");
        public static readonly Gender InActive = new Gender(2, "FEMALE");
        public static readonly Gender Other = new Gender(3, "OTHER");

        private Gender(int id, string name) : base(id, name) { }
    }
}