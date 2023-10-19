using System;
using System.Collections.Generic;
using System.Text;

namespace Stack.Entities.DomainEntities
{
    public class DomainBaseEntity
    {
        public long ID { get; set; }
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }

    }
}
