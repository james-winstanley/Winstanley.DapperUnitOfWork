using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Winstanley.UnitOfWork.Interfaces;

namespace Winstanley.UnitOfWork.Entities
{
    public abstract record AuditEntity<TKey> : EntityBase<TKey>, IAuditEntity<TKey>
    {
        [Required]
        [StringLength(36)]
        public string CreatedBy { get; set; }

        [Column("Created")]
        public DateTime CreatedAt { get; set; }

        [StringLength(36)]
        [Column("LastModifiedBy")]
        public string UpdatedBy { get; set; }

        [Column("LastModified")]
        public DateTime? UpdatedAt { get; set; }
    }


    public abstract record AuditEntity : IAuditEntity
    {
        [Required]
        [StringLength(36)]
        public string CreatedBy { get; set; }

        [Column("Created")]
        public DateTime CreatedAt { get; set; }

        [StringLength(36)]
        [Column("LastModifiedBy")]
        public string UpdatedBy { get; set; }

        [Column("LastModified")]
        public DateTime? UpdatedAt { get; set; }
    }
}
