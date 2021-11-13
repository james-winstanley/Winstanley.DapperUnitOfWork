using System;

namespace Winstanley.UnitOfWork.Interfaces
{
    public interface IAuditEntity
    {
        DateTime CreatedAt { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        string UpdatedBy { get; set; }
    }


    public interface IAuditEntity<TKey> : IAuditEntity, IEntityBase<TKey>
    {
    }
}
