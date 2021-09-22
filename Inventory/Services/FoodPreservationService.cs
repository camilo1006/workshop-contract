namespace Inventory.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Dto;
    using Newtonsoft.Json;

    public class FoodPreservationService : IFoodPreservationService
    {
        private readonly HttpClient _client;

        public FoodPreservationService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("foodPreservation") ??
                      throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<object> FoodPreservation(List<InventoryItem> listInventory)
        {
            try
            {
                var requestPreserver = await _client.GetAsync("FoodPreserver");
                var responsePreserver =
                    JsonConvert.DeserializeObject<ResponsePreserver>(await requestPreserver.Content
                        .ReadAsStringAsync());
                listInventory.ForEach(inventoryItem =>
                {
                    inventoryItem.Quantity -=
                        responsePreserver.items.FirstOrDefault(i => i.name.Equals(inventoryItem.Name))?.quantity ?? 0;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new
            {
                items = listInventory,
                date = DateTime.Now
            };
        }
    }
}
