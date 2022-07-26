using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.DTOs.IdentityDTOs
{
    public class UserVerificationDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; }
    }
}
