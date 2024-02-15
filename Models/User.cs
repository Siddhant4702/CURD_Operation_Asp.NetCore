
using System.ComponentModel.DataAnnotations;


namespace CURD_using_SDSclient.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Enter First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Enter Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Email is Reqiured")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
