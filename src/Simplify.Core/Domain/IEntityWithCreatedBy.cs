namespace Simplify.Core.Domain
{
    public interface IEntityWithCreatedBy
    {
        string CreatedBy { get; set; }
    }
}