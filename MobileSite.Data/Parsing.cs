using MobileSite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MobileSite.Data
{
    public class Parsing : IParsing
    {
        private readonly MobileSiteDbContext db;

        public Parsing(MobileSiteDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Good> Parse()
        {
            var goods = new List<Good>();

            string htmlCode = string.Empty;
            using (WebClient client = new WebClient())
            {
                htmlCode = client.DownloadString("https://www.mi-xx.ru/smartfony");
            }

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlCode);
            var productList = doc.DocumentNode.SelectNodes("//div[@id='ajaxItems']");

            foreach (var product in productList)
            {
                var elemList = product.ChildNodes.Where(x => x.Name == "article" && x.Attributes["class"].Value == "col-lg-3 col-md-4 col-sm-6 col-6");
                foreach (var elem in elemList)
                {
                    var indoc = new HtmlAgilityPack.HtmlDocument();
                    indoc.LoadHtml(elem.InnerHtml);
                    string name = indoc.DocumentNode.SelectSingleNode("//div/div[@class='title']/a").InnerText;
                    string priceString = indoc.DocumentNode.SelectSingleNode("//div/div/div/span[@class='price-cur']").InnerText;
                    string idString = indoc.DocumentNode.SelectSingleNode("//div/div/div/button").Attributes["data-id"].Value;
                    priceString = priceString.Replace(" ", "");
                    int price = Int32.Parse(priceString);
                    int id = Int32.Parse(idString);
                    goods.Add(new Good() { Name = name, GoodId = id, Price = price });
                }
            }
            return goods;
        }

        public Good Add(Good newGood)
        {
            db.Goods.Add(newGood);

            return newGood;
        }

        public IEnumerable<Good> GetGoods()
        {
            return db.Goods.OrderBy(i => i.Name);
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public void DeleteAll()
        {
            foreach(var g in db.Goods)
            {
                db.Goods.Remove(g);
            }
        }
    }
}
