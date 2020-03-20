using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MobileSite.Core;
using MobileSite.Data;

namespace ZooSite.Pages.Products
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IProductData ProductData;

        public string Message { get; set; }
        public IEnumerable<Product> Products { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public ListModel(IConfiguration config, IProductData ProductData)
        {
            this.config = config;
            this.ProductData = ProductData;
        }

        public void OnGet()
        {
            

            Message = config["Message"];
            Products = ProductData.GetProductsByName(SearchTerm);
        }
    }
}