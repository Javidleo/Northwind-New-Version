using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.DTOs.IdentityDTOs
{
    public class AddNationalCodeDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "NationalCode is required")]
        //[StringLength(10,MinimumLength =10,ErrorMessage = "the lenght of NationalCode is not correct.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "NationalCode is not valid")]
        public string NationalCode { get; set; }
    }
}
