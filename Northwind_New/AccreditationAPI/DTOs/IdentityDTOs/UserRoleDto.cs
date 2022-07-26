using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.DTOs.IdentityDTOs
{
    public class UserRoleDto
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FromTime { get; set; }
        public string TOTime { get; set; }
    }
}
