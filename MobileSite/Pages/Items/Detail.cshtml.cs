using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileSite.Core;
using MobileSite.Data;

namespace MobileSite.Pages.Items
{
    public class DetailModel : PageModel
    {
        private readonly IItemData itemData;

        [TempData]
        public string Message { get; set; }

        public Item Item { get; set; }

        public DetailModel(IItemData itemData)
        {
            this.itemData = itemData;
        }

        public IActionResult OnGet(int itemId)
        {
            Item = itemData.GetById(itemId);
            if(Item == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
    }
}