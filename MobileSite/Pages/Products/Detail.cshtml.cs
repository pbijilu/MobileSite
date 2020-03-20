using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileSite.Core;
using MobileSite.Data;

namespace ZooSite.Pages.Products
{
    public class DetailModel : PageModel
    {
        private readonly IProductData productData;

        [TempData]
        public string Message { get; set; }

        public Product Product { get; set; }

        public DetailModel(IProductData productData)
        {
            this.productData = productData;
        }

        public IActionResult OnGet(int productId)
        {
            Product = productData.GetById(productId);
            if(Product == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}