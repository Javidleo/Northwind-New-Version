using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.DTOs.IdentityDTOs
{
    public class SendSmsTokenDto
    {
        [Required(ErrorMessage = "phoneNumber is required.")]
        public string phoneNumber { get; set; }
    }
}
