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

        public IEnumerable<Good> Parse(string url)
        {
            var goods = new List<Good>();

            string htmlCode = string.Empty;
            var doc = new HtmlAgilityPack.HtmlDocument();

            if (url.Contains("mi-xx"))
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Only a test!");
                    htmlCode = client.DownloadString(url);
                }

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
                        string priceString = indoc.DocumentNode.SelectSingleNode("//div/div/div/meta[@itemprop='price']").Attributes["content"].Value;
                        string idString = indoc.DocumentNode.SelectSingleNode("//div/div/div/button").Attributes["data-id"].Value;
                        //priceString = priceString.Replace(" ", "");
                        int price = Int32.Parse(priceString);
                        int id = Int32.Parse(idString);
                        goods.Add(new Good() { Name = name, GoodId = id, Price = price });
                    }
                }
            }

            if (url.Contains("mi92"))
            {
                int i = 1;
                while (true)
                {
                    WebClient client = new WebClient();
                    client.Headers.Add("user-agent", "Only a test!");
                    try
                    {
                        htmlCode = client.DownloadString($"{url}/page/{i}");
                    }
                    catch
                    {
                        break;
                    }
  
                    doc.LoadHtml(htmlCode);
                    var productList = doc.DocumentNode.SelectNodes("//div[@class='products row']");

                    foreach (var product in productList)
                    {
                        var elemList = product.ChildNodes.Where(x => x.Name == "div" && x.Attributes["class"].Value == "col-sm-6 col-md-4 col-xl-3 --pb-3");
                        foreach (var elem in elemList)
                        {
                            var indoc = new HtmlAgilityPack.HtmlDocument();
                            indoc.LoadHtml(elem.InnerHtml);
                            string name = indoc.DocumentNode.SelectSingleNode("//div/div/div[@class='product-inner-row row h-100 mt-2 mt-md-0']/div[@class='woocommerce_shop_loop_item_title col col-md-12 --align-self-md-end']/div/div[@class='col-12 d-md-none']/a/h2").InnerText;
                            string priceString;
                            try
                            {
                                priceString = indoc.DocumentNode.SelectSingleNode("//div/div/div[@class='product-inner-row row h-100 mt-2 mt-md-0']/div[@class='woocommerce_shop_loop_item_title col col-md-12 --align-self-md-end']/div/div[@class='col-12 align-self-end']/div/div[@class='col col-md-12']/div/div[@class='col-md woocommerce_after_shop_loop_item pr-0']/span/span").InnerText;
                            }
                            catch
                            {
                                priceString = indoc.DocumentNode.SelectSingleNode("//div/div/div[@class='product-inner-row row h-100 mt-2 mt-md-0']/div[@class='woocommerce_shop_loop_item_title col col-md-12 --align-self-md-end']/div/div[@class='col-12 align-self-end']/div/div[@class='col col-md-12']/div/div[@class='col-md woocommerce_after_shop_loop_item pr-0']/span/ins/span").InnerText;
                            }
                            string idString = indoc.DocumentNode.SelectSingleNode("//div/div/div[@class='product-inner-row row h-100 mt-2 mt-md-0']/div[@class='woocommerce_shop_loop_item_title col col-md-12 --align-self-md-end']/div/div[@class='col-12 align-self-end']/div/div[@class='col-auto col-md-12']/div/div[@class='viewBuyButton-col col pr-0']/div/button").Attributes["data-oneclick-order--product-id"].Value;
                            priceString = priceString.Replace(" ", "");
                            priceString = priceString.Replace("&nbsp;руб.", "");
                            int price = Int32.Parse(priceString);
                            int id = Int32.Parse(idString);
                            goods.Add(new Good() { Name = name, GoodId = id, Price = price });
                        }
                    }
                    i++;
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
            return db.Goods;
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
