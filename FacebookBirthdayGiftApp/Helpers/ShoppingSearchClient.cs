using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using FacebookBirthdayGiftApp.Models;
using Newtonsoft.Json.Linq;
using Semantics3;

namespace FacebookBirthdayGiftApp.Helpers
{
    public static class ShoppingSearchClient
    {
        private const string SearchApiTemplate = "https://www.googleapis.com/shopping/search/v1/public/products?key={0}&country=US&q={1}&alt=json";
        private static readonly HttpClient client = new HttpClient();

        public static string AppKey = ConfigurationManager.AppSettings["Search:AppKey"];

        public static Task<SearchResult> GetProductsAsync(string query)
        {
            if (String.IsNullOrEmpty(AppKey))
            {
                throw new InvalidOperationException("Search:AppKey cannot be empty. Make sure you set it in the configuration file.");
            }

            query = query.Replace(" ", "+");
            string searchQuery = String.Format(SearchApiTemplate, AppKey, query);
            var response = client.GetAsync(searchQuery).Result.EnsureSuccessStatusCode();
            return response.Content.ReadAsAsync<SearchResult>();
        }
    }

    public static class Semantics3ShoppingSearchClient
    {
        public static void GetProductsAsync(string query)
        {
            Products products = new Products(ConfigurationManager.AppSettings["Semantics3:APIKey"], ConfigurationManager.AppSettings["Semantics3:APISecret"]);
            products.products_field("search", query);
            //products.products_field("cat_id", 4992);
            //products.products_field("brand", "Toshiba");
            //products.products_field("weight", "gte", 1000000);
            //products.products_field("weight", "lte", 1500000);
            //products.products_field("sitedetails", "name", "newegg.com");
            //products.products_field("sitedetails", "latestoffers", "currency", "USD");
            //products.products_field("sitedetails", "latestoffers", "price", "gte", 100);
            JObject apiResponse = products.get_products();
        }
    }
}
