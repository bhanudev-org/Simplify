using Simplify.SeedWork.Domain;

namespace Simplify.SeedWork
{
    public sealed class BloodGroup : Enumeration
    {
        public static readonly BloodGroup APositive = new BloodGroup(1, "A +VE");
        public static readonly BloodGroup ANegative = new BloodGroup(2, "A -VE");

        public static readonly BloodGroup BPositive = new BloodGroup(3, "B +VE");
        public static readonly BloodGroup BNegative = new BloodGroup(4, "B -VE");

        public static readonly BloodGroup OPositive = new BloodGroup(5, "O +VE");
        public static readonly BloodGroup ONegative = new BloodGroup(6, "O -VE");

        public static readonly BloodGroup ABPositive = new BloodGroup(7, "AB +VE");
        public static readonly BloodGroup ABNegative = new BloodGroup(8, "AB -VE");

        private BloodGroup(int id, string name) : base(id, name) { }
    }
}