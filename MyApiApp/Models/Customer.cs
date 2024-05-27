using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MyApiApp.Models
{

    /// <summary>
    /// Table customer
    /// </summary>

    [Table("customer")]
    public class Customer
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public String FirstName { get; set; }

        [Required]
        [MaxLength(150)]
        public String Surname { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string DocumentId { get; set; }

        [Required]
        [MaxLength(11)]
        public String Pesel { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        public String CategoryType { get; set; }

        public int? AddressId { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();


    }
}
