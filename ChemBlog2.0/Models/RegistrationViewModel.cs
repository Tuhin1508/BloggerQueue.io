using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChemBlog2._0.Models
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name="User Name")]
        [StringLength(100,ErrorMessage ="Please Enter Your User Name Here",MinimumLength =6)]
        public string Name { get; set; }
        [Required]
        [Display(Name="Email")]
        [EmailAddress]
        public string EmialAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
