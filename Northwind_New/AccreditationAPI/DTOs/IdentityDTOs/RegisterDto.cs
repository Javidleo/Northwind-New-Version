using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI.DTOs.IdentityDTOs
{
    public class RegisterDto
    {
        //[Required(ErrorMessage = "User Name is required")]
        //public string Username { get; set; }

        //[EmailAddress]
        //[Required(ErrorMessage = "Email is required")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        //[StringLength(11,MinimumLength =11, ErrorMessage = "the lenght of PhoneNumber is not correct")]
        [RegularExpression("^[0][9][0-9]{9}$", ErrorMessage = "PhoneNumber is not valid")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "کلمات عبور وارد شده با هم تطابق ندارند")]
        public string ConfirmPassword { get; set; }
    }
}
