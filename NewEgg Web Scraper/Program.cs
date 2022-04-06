using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NewEgg_Web_Scraper.Models;

namespace NewEgg_WebScraping
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {
            Console.WriteLine("Enter Number of pages to scrape:");
            int pages = Convert.ToInt32(Console.ReadLine());
            List<Product> Products = new List<Product>();

            if (pages == 0)
            {
                GetHtmlAsync();
            }

            Products.Add(new Product()
            {
                ProductTitle = "Product Name",
                ProductPrice = "Price",
                ProductLink = "Product (URL)"
            });


            for (int i = 0; i < pages; i++)
            {
                int iterate = i + 1;
                var url = "https://www.newegg.com/Gaming-Desktops/SubCategory/ID-3742/Page-"+ iterate;
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                var inner = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var newnode = htmlDocument.DocumentNode.SelectNodes("//div[@class=\"item-cell\"]").ToList();
                Console.WriteLine("");
                Console.WriteLine("Scraping Page #" + iterate + "...");

                foreach (var item in newnode)
                {
                    // displaying final outputs
                    string productTitle = item.SelectSingleNode(".//a[@class=\"item-title\"]").InnerText;
                    string productPrice = item.SelectSingleNode(".//li[@class=\"price-current\"]").InnerText;
                    string productLink = item.SelectSingleNode(".//a[@class=\"item-title\"]").Attributes["href"].Value;
                   

                    Products.Add(new Product()
                    {
                        ProductTitle = productTitle.ToString().Replace(",", ""),
                        ProductPrice = productPrice.ToString().Replace(",", ""),
                        ProductLink = productLink.ToString().Replace(",", "")
                    });
                }
            }

            var exportRaw = DisplayData(Products);
            ExportCSV(exportRaw);

            Console.WriteLine();
        }

        public static void ExportCSV(String[] output)
        {
            String textOutputPath = @"NEWEGG_Products.csv";
            File.WriteAllLines(textOutputPath, output); //export
            Console.WriteLine(" ");
            Console.WriteLine("Finished.. NewEGG Products Export location:");
            Console.WriteLine(System.IO.Directory.GetCurrentDirectory() + @"\" + textOutputPath);
            Console.ReadLine();
        }

        public static string[] DisplayData(List<Product> output)
        {
            var result = output.Select(k =>
               k.ProductTitle + "," +
               k.ProductPrice + "," +
               k.ProductLink
               ).ToArray();

            return result;
        }
    }
}
