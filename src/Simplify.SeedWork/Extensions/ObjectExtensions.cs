namespace Simplify.SeedWork.Extensions
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object obj) where T : class => (T)obj;
    }
}