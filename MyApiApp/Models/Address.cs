using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyApiApp.Models
{
    /// <summary>
    /// Table accounts
    /// </summary>
    [Table("address")]
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AddressId { get; set; }

        [Required]
        [MaxLength(150)]
        public String Street { get; set; }

        [Required]
        public String BlgNumber { get; set; }

        [Required]
        [MaxLength(10)]
        public String ZipCode { get; set; }

        [Required]
        [MaxLength(50)]
        public String City { get; set; }

        [Required]
        [MaxLength(250)]
        public string AddressLine { get; set; }


        [Required]
        public AddressType AddressTypeId { get ; set; }        
    }
    /// <summary>
    /// address type definition    
    /// </summary>
    public enum AddressType
    {
        /// <summary>
        /// address declared
        /// </summary>
        Registered = 0,
        /// <summary>
        /// act of registering one's place of residence with the local authorities
        /// </summary>
        Residence = 1,
        /// <summary>
        /// mail corespondence address
        /// </summary>
        Correspondence = 2,
        /// <summary>
        /// used temporary, needs to be updated before activating account
        /// </summary>
        Temporary = 3,
        /// <summary>
        /// acceble in case of empoyer is provaiding facility for living in work place
        /// </summary>
        Work = 4
    }

}

