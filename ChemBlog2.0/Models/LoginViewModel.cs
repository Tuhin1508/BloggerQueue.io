using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChemBlog2._0.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name ="UserName")]
        public string UserName { get; set; }
        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
