using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemBlog2._0.Models
{
    public class AdminPanelEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserRegId { get; set; }
        public string Heading { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        public string ImagePath { get; set; }
        public string EditorData { get; set; }
    }
}
