﻿using System;
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
        [Column(TypeName="Money")]
        public decimal Value { get; set; }
        [Required]
        public int Amount { get; set; }

        [Column(TypeName = "Money")]
        public decimal UnitValue { get; set; }
        
        public Currency Currency { get; set; }
    }
}