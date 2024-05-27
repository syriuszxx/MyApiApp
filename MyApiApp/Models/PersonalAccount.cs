using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyApiApp.Models
{
    /// <summary>
    /// Table accounts for product PersonalAccount
    /// </summary>

    [Table("account")]
    public class PersonalAccount : Account
        {
            public Decimal Overdraft { get; set; }
            
            public override void UpdateBalance(Decimal Amount)
            {
                Balance = Balance + Amount;
            }

            public override void UpdateAvailableBalance(Decimal Amount)
            {
            AvailableBalance = AvailableBalance + Amount;
            }

        public override Decimal ReturnBalance()
            {
                return Balance + Overdraft;
            }
            public override void CalculateCharges()
            {
                Balance = Balance - 15;
            }
            public override void UpdateInterestRates() { }

            public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        }
    }
