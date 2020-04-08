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
    public class ParseMiXXModel : PageModel
    {
        private readonly IItemData itemData;
        private readonly IParsing parsing;

        public IEnumerable<Good> Goods { get; set; }

        public ParseMiXXModel(IItemData itemData, IParsing parsing)
        {
            this.itemData = itemData;
            this.parsing = parsing;
        }
        public void OnGet()
        {
            parsing.DeleteAll();

            Goods = parsing.Parse("https://www.mi-xx.ru/smartfony?page=0");

            foreach (var good in Goods)
            { 
                parsing.Add(good);
            }
            parsing.Commit();
        }

        public IActionResult OnPost()
        {
           foreach (var good in parsing.GetGoods())
            {
                if (itemData.GetItems().Where(i => i.IdMixx == good.GoodId).Any())
                {
                    Item item = itemData.GetItems().Where(i => i.IdMixx == good.GoodId).Single();
                    item.Price = good.Price;
                    itemData.Update(item);
                }
                else    
                    itemData.Add(new Item() {IdMixx = good.GoodId, Name = good.Name, Price = good.Price });
            }

            itemData.Commit();

            parsing.DeleteAll();
            parsing.Commit();

            return RedirectToPage("../Items/List");
        }
    }
}