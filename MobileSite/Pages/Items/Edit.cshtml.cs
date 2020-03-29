using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MobileSite.Core;
using MobileSite.Data;

namespace MobileSite.Pages.Items
{
    public class EditModel : PageModel
    {
        private readonly IItemData itemData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Item Item { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        public EditModel(IItemData itemData, IHtmlHelper htmlHelper)
        {
            this.itemData = itemData;
            this.htmlHelper = htmlHelper;
        }
        public IActionResult OnGet(int? itemId)
        {
            Types = htmlHelper.GetEnumSelectList<ItemType>();
            if(itemId.HasValue)
            {
                Item = itemData.GetById(itemId.Value);
            }
            else
            {
                Item = new Item();
            }
            if (Item == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                Types = htmlHelper.GetEnumSelectList<ItemType>();
                return Page();
            }

            if(Item.Id > 0)
            {
                itemData.Update(Item);
            }
            else
            {
                itemData.Add(Item);
            }
            itemData.Commit();
            TempData["Message"] = "Item saved!";
            return RedirectToPage("./Detail", new { itemId = Item.Id });
        }
    }
}