using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.API.RESTful.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        public int IDUser { get; set; }

        public string NameFirst { get; set; }

        public string NameLast { get; set; }

        public string Email {  get; set; }

        public string Password {  get; set; }

        public int Document { get; set; }

        public DateOnly Birth { get; set; }

        public int IDRol {  get; set; }

        [ForeignKey("IDRol")]
        public virtual RolModel? Roles { get; set; }
    }
}
