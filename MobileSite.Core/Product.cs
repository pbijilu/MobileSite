using System.ComponentModel.DataAnnotations;

namespace MobileSite.Core
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }
        public int PriceMi92 { get; set; }
        public ProductType Type { get; set; }
    }
}
