using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyApiApp.Models
{
    /// <summary>
    /// Table accounts
    /// </summary>
    [Table("account")]
    public abstract class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(10)]
        public String AccountNumber { get; set; }

        [Required]
        [MaxLength(28)]
        public string Iban { get; set; }

        public string BranchId { get; set; }

        public Decimal Balance { get; set; }
        public Decimal AvailableBalance { get; set; }

        [Required]
        public string Ccy { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }


        public abstract void UpdateBalance(Decimal Amount);
        public abstract void UpdateAvailableBalance(Decimal Amount);
        public abstract Decimal ReturnBalance();
        public abstract void CalculateCharges();
        public abstract void UpdateInterestRates();

    }

}

