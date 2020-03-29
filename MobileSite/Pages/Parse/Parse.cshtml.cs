using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MobileSite.Core;
using MobileSite.Data;

namespace MobileSite.Pages.Parse
{
    public class ParseModel : PageModel
    {
        private readonly IItemData itemData;
        private readonly IParsing parsing;

        public IEnumerable<Good> Goods { get; set; }

        public ParseModel(IItemData itemData, IParsing parsing)
        {
            this.itemData = itemData;
            this.parsing = parsing;
        }
        public void OnGet()
        {
            Goods = parsing.Parse();

            foreach (var g in Goods)
            { 
                parsing.Add(g);
            }
            parsing.Commit();
        }

        public IActionResult OnPost()
        {
            Goods = parsing.GetGoods();

            foreach (var g in Goods)
            {
                if (itemData.GetItems().Where(i => i.IdMixx == g.GoodId).Any())
                {
                    Item item = itemData.GetItems().Where(i => i.IdMixx == g.GoodId).Single();
                    item.Price = g.Price;
                    itemData.Update(item);
                }
                else    
                    itemData.Add(new Item() {IdMixx = g.GoodId, Name = g.Name, Price = g.Price });
            }

            itemData.Commit();

            parsing.DeleteAll();
            parsing.Commit();

            return RedirectToPage("../Items/List");
        }
    }
}