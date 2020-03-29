using System.ComponentModel.DataAnnotations;

namespace MobileSite.Core
{
    public class Item
    {
        public int Id { get; set; }
    
        [Required, StringLength(80)]
        public string Name { get; set; }

        public int IdMixx { get; set; }

        [Required]
        public int Price { get; set; }

        public int IdMi92 { get; set; }

        public int PriceMi92 { get; set; }

        public ItemType Type { get; set; }
    }
}
