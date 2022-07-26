using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.DTOs.IdentityDTOs
{
    public class UserActivationStatusDto
    {
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
