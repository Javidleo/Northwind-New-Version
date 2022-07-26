using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.DTOs.IdentityDTOs
{
    public class ChangePhoneNumberDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression("^[0][0-9]{10}$", ErrorMessage = "PhoneNumber is not valid")]
        public string PhoneNumber { get; set; }
    }
}
