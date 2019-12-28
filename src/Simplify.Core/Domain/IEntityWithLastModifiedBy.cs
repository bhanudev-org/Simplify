namespace Simplify.Core.Domain
{
    public interface IEntityWithLastModifiedBy
    {
        string LastModifiedBy { get; set; }
    }
}