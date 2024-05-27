using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyApiApp.Models
{
    /// <summary>
    /// User created for customer
    /// </summary>

    [Table("user")]
    public class User
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserId { get; set; }

        public String Login { get; set; }

        public String Email { get; set; }

        public string Password { get; set; }

        public String PhoneHash { get; set; }

        public Boolean ActiveAccess { get; set; }
    }
}
