using Pro.Enum;

namespace Simplify.SeedWork
{
    public class Status : Enumeration
    {
        public static readonly Status Active = new Status(1, "ACTIVE");
        public static readonly Status Hold = new Status(2, "HOLD");
        public static readonly Status Disabled = new Status(3, "DISABLED");

        private Status(int id, string name) : base(id, name) { }
    }
}