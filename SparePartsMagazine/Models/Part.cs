using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SparePartsMagazine.Models
{
    public class Part
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        [StringLength(60, MinimumLength = 1)]
        [Required(ErrorMessage = "Pole Wymagane")]
        public string Name { get; set; }

        [Display(Name = "Cena")]
        [Required(ErrorMessage = "Pole Wymagane")]
        [Range(0.0, Double.MaxValue,ErrorMessage = "{0} nie jest poprawną wartością")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Data wprowadzenia")]
        [DataType(DataType.Date)]
        public DateTimeOffset InputDate { get; set; }

        [Display(Name = "Data modyfikacji")]
        [DataType(DataType.Date)]
        public DateTimeOffset ModificationDate { get; set; }
    }
}
