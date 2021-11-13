namespace Winstanley.UnitOfWork.Interfaces
{
    public interface IEntityBase<TKey>
    {
        TKey Id { get; set; }
    }
}
