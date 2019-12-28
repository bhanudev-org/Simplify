namespace Simplify.Core.Domain
{
    public interface IEntityWithVersion
    {
        long Version { get; set; }
    }
}