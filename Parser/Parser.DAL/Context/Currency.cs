using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Parser.DAL.Context
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(3)")]
        public string Name { get; set; }
        
        public List<CurrencyValue> Values { get; set; } 
    }
}