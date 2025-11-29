
namespace Product_Shop.EF
{
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }
        [Required]
        [Range(0, 1000000)]
        public int Quantity { get; set; }
        [Required(ErrorMessage ="Please select a category")]

        public int CategoryId { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
