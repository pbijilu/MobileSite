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
    public class DeleteModel : PageModel
    {
        private readonly IItemData itemData;

        public Item Item { get; set; }

        public DeleteModel(IItemData itemData)
        {
            this.itemData = itemData;
        }

        public IActionResult OnGet(int itemId)
        {
            Item = itemData.GetById(itemId);
            if (Item == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost(int itemId)
        {
            var item = itemData.Delete(itemId);
            itemData.Commit();

            if (item == null)
            {
                return RedirectToPage("./NotFound");
            }

            TempData["Message"] = $"{item.Name} deleted";
            return RedirectToPage("./List");
        }
    }
}