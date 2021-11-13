using System.ComponentModel.DataAnnotations;
using Dapper.FastCrud;
using Winstanley.UnitOfWork.Interfaces;

namespace Winstanley.UnitOfWork.Entities
{
    public abstract record EntityBase<TKey> : IEntityBase<TKey>
    {
        [Key]
        [Required]
        [DatabaseGeneratedDefaultValue]
        public virtual TKey Id { get; set; }
    }
}
