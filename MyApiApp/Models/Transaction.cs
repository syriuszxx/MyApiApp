using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyApiApp.Models
{
    /// <summary>
    /// Transactions on accountId
    /// </summary>

    [Table("transaction")]
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Decimal Amount { get; set; }

        public int? AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public String Type { get; set; }

        [Required]
        public String Status { get; set; }
    }
}

