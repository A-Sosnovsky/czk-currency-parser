using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parser.DAL.Context
{
    public class CurrencyValue
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        [Required]
        [Column(TypeName="Date")]
        public DateTime Date { get; set; }
        [Required]
        public decimal Value { get; set; }
        
        public Currency Currency { get; set; }
    }
}