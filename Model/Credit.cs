using System;
using System.ComponentModel.DataAnnotations;

namespace DreamInMars.Model
{
    public class Credit
    {
        [Key]
        public int CreditId { get; set; }
        public int AccountId { get; set; }
        public int Value { get; set; }
        public DateTime LastResetDate { get; set; }
    }
}
