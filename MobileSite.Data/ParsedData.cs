using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MobileSite.Core;

namespace MobileSite.Data
{
    public class ParsedData : IItemData
    {
        string result = string.Empty;
        List<Item> products;

        public ParsedData()
        {
            products = new List<Item>();

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
                    products.Add(new Item() { Name = name, Id = id, Price = price });
                }
            }
        }

        public Item GetById(int id)
        {
            return products.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Item> GetItemsByName(string name)
        {
            return from p in products
                   where string.IsNullOrEmpty(name) || p.Name.Contains(name)
                   orderby p.Name
                   select p;
        }

        public Item Add(Item newProduct)
        {
            products.Add(newProduct);
            newProduct.Id = products.Max(p => p.Id) + 1;
            return newProduct;
        }

        public Item Update(Item updatedProduct)
        {
            var product = products.SingleOrDefault(p => p.Id == updatedProduct.Id);
            if(product != null)
            {
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
                product.Type = updatedProduct.Type;
            }
            return product;
        }

        public int Commit()
        {
            return 0;
        }

        public Item Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
            }
            return product;

        }

        public IEnumerable<Item> GetItems()
        {
            throw new NotImplementedException();
        }
    }

    /*public class InMemoryProductData : IProductData
    {
        List<Product> products;
        public InMemoryProductData()
        {
            products = new List<Product>()
            {
               new Product { Id = 1834, Name = "Redmi Note 8 (4GB/64GB)",  Price = 11690, PriceMi92 = 12390, Type = ProductType.Phone},
               new Product { Id = 1212, Name = "Redmi Note 7 (4GB/64GB)",  Price = 11740, PriceMi92 = 11740, Type = ProductType.Phone},
               new Product { Id = 1862, Name = "Redmi Note 8 Pro (6GB/128GB)",  Price = 16390, PriceMi92 = 17990, Type = ProductType.Phone},
            };
        }
        public IEnumerable<Product> GetProductsByName(string name = null)
        {
            return from p in products
                   where string.IsNullOrEmpty(name) || p.Name.StartsWith(name)
                   orderby p.Name
                   select p;
        }

        public Product GetById(int id)
        {
            return products.SingleOrDefault(a => a.Id == id);
        }
        public Product Update(Product updatedProduct)
        {
            var product = products.SingleOrDefault(p => p.Id == updatedProduct.Id);
            if (product != null)
            {
                product.Name = updatedProduct.Name;
                product.Price = updatedProduct.Price;
                product.Type = updatedProduct.Type;
            }
            return product;
        }

        public int Commit()
        {
            return 0;
        }
    }*/
}
