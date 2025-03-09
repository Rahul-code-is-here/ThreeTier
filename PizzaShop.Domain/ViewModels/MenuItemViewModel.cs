using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.DataModels
{
    public class MenuItemViewModel
    {

        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string ItemName { get; set; }

        [Required]
        public string ItemType { get; set; }


        public decimal Rate { get; set; }


        public int Quantity { get; set; }

        [Required]
        public string Unit { get; set; }

        public int UnitId { get; set; }

        public bool IsAvailable { get; set; }

        public bool IsTaxable { get; set; }

        [Range(0, 100)]
        public decimal? TaxPercentage { get; set; }

        [StringLength(10)]
        public string ShortCode { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public IFormFile Image { get; set; }
    }

    // public class MenuItemViewModel
    // {
    //     public int Id { get; set; }
    //     public string ItemName { get; set; }
    //     public string ItemType { get; set; }
    //     public decimal Rate { get; set; }
    //     public int Quantity { get; set; }
    //     public bool IsAvailable { get; set; }
    //     public int CategoryId { get; set; }
    //     public string Unit { get; set; }
    //     public bool IsTaxable { get; set; }
    //     public decimal TaxPercentage { get; set; }
    //     public string ShortCode { get; set; }
    //     public string Description { get; set; }
    // }
}