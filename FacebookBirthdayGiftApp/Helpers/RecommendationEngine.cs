using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacebookBirthdayGiftApp.Models;

namespace FacebookBirthdayGiftApp.Helpers
{
    public static class RecommendationEngine
    {
        private static readonly string[] CategoriesToConsider =
        {
            "Entertainment",
            "Tv show",
            "Movie",
            "Book",
            "Musician/band",
            "Product/service",
            "Clothing",
            "Food/beverages",
            "Shopping/retail",
            "Games/toys"
        };

        public static async Task<List<RecommendedItem>> RecommendProductAsync(MyAppUserFriend friend)
        {
            var recommendedItem = new List<RecommendedItem>();
            if (friend.Likes != null)
            {
                var friendLikes = friend.Likes.Data;
                var friendLikesFilteredByCategory = friendLikes.Where(like => CategoriesToConsider.Contains(like.Category));

                foreach (var item in friendLikesFilteredByCategory.Take(10))
                {
                    Semantics3ShoppingSearchClient.GetProductsAsync(item.Name);
                    var result = await ShoppingSearchClient.GetProductsAsync(item.Name);
                    if (result.items == null)
                    {
                        // skip empty results
                        continue;
                    }
                    var products = result.items.Select(i => i.product).Take(2).Where(p => p != null);
                    foreach (var product in products)
                    {
                        recommendedItem.Add(new RecommendedItem
                        {
                            Product = product,
                            Reason = "likes " + item.Name
                        });
                    }
                }
            }

            return recommendedItem;
        }
    }
}