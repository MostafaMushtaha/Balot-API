using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stack.DTOs.Enums.Modules.Groups;
using Stack.Entities.DomainEntities.Groups;

namespace Stack.Entities.DomainEntities.Groups
{
    public class GroupDTO : DomainBaseEntity
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public virtual ICollection<Group_MemberDTO> Members { get; set; }

        public void CreateAsActive(string Name, string UserID)
        {
            this.Status = (int)GroupStatus.Active;
            this.Name = Name;
            this.CreatedBy = UserID;
            this.CreationDate = DateTime.UtcNow;
        }

        public void AddOwner()
        {
            Group_MemberDTO owner = new Group_MemberDTO
            {
                GroupID = this.ID,
                UserID = this.CreatedBy,
                IsOwner = true,
                Title = "Owner",
            };

            if (this.Members == null)
            {
                this.Members = new List<Group_MemberDTO>();
            }
            this.Members.Add(owner);
        }
    }
}
