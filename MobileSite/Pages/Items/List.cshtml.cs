using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MobileSite.Core;
using MobileSite.Data;

namespace MobileSite.Pages.Items
{
    public class ListModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly IItemData itemData;

        public string Message { get; set; }
        public IEnumerable<Item> Items { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public ListModel(IConfiguration config, IItemData itemData)
        {
            this.config = config;
            this.itemData = itemData;
        }

        public void OnGet()
        {
            Message = config["Message"];
            Items = itemData.GetItemsByName(SearchTerm);
        }
    }
}