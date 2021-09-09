namespace Inventory.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Controllers;
    using Dto;
    using Newtonsoft.Json;

    public class FoodPreservationService : IFoodPreservationService
    {
        private readonly HttpClient _client;

        public FoodPreservationService(Uri baseUri = null)
        {
            _client = new HttpClient { BaseAddress = baseUri };
        }

        public async Task<object> FoodPreservation(List<InventoryItem> listInventory)
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
