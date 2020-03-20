using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MobileSite.Core;
using MobileSite.Data;

namespace ZooSite.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductData productData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        public EditModel(IProductData productData, IHtmlHelper htmlHelper)
        {
            this.productData = productData;
            this.htmlHelper = htmlHelper;
        }
        public IActionResult OnGet(int? productId)
        {
            Types = htmlHelper.GetEnumSelectList<ProductType>();
            if(productId.HasValue)
            {
                Product = productData.GetById(productId.Value);
            }
            else
            {
                Product = new Product();
            }
            if (Product == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                Types = htmlHelper.GetEnumSelectList<ProductType>();
                return Page();
            }

            if(Product.Id > 0)
            {
                productData.Update(Product);
            }
            else
            {
                productData.Add(Product);
            }
            productData.Commit();
            TempData["Message"] = "Restaurant saved!";
            return RedirectToPage("./Detail", new { productId = Product.Id });
        }
    }
}