using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.DTOs.IdentityDTOs
{
    public class EditRoleDto
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
