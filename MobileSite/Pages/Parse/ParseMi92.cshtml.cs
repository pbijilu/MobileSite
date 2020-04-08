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
    public class ParseMi92Model : PageModel
    {
        private readonly IItemData itemData;
        private readonly IParsing parsing;

        public ParseMi92Model(IItemData itemData, IParsing parsing)
        {
            this.itemData = itemData;
            this.parsing = parsing;
            ExistingGoodsCount = 0;
            NewGoods = new List<Good>();
        }

        public IEnumerable<Good> Goods { get; set; }
        public List<Good> NewGoods { get; set; }
        public int ExistingGoodsCount { get; set; }


        public void OnGet()
        {
            parsing.DeleteAll();

            Goods = parsing.Parse("https://mi92.ru/catalog/smartfonyi/");

            foreach (var good in Goods)
            {
                if (!itemData.GetItems().Where(i => i.IdMi92 == good.GoodId).Any())
                    NewGoods.Add(good);
                else
                    {
                        parsing.Add(good);
                        ExistingGoodsCount++;
                    }
            }
            parsing.Commit();
        }

        public IActionResult OnPost()
        {
            foreach (var good in parsing.GetGoods())
            {
                Item item = itemData.GetItems().Where(i => i.IdMi92 == good.GoodId).Single();
                item.PriceMi92 = good.Price;
                itemData.Update(item);
            }
            itemData.Commit();

            parsing.DeleteAll();
            parsing.Commit();

            return RedirectToPage("../Items/List");
        }
    }
}