namespace Inventory.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Dto;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        [HttpGet]
        public async Task<object> Get()
        {
            var rnd = new Random();
            var listInventory = new List<InventoryItem>
            {
                new InventoryItem { Name = "Potato", Quantity = rnd.Next(0, 10) },
                new InventoryItem { Name = "Salt", Quantity = rnd.Next(0, 1) },
                new InventoryItem { Name = "Beans", Quantity = rnd.Next(0, 50) }
            };

            var client = new HttpClient { BaseAddress = new Uri("https://localhost:5001/") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var requestPreserver = await client.GetAsync("FoodPreserver");
                var responsePreserver = JsonSerializer.Deserialize<ResponsePreserver>(requestPreserver.Content.ReadAsStringAsync().Result);
                listInventory.ForEach(inventoryItem =>
                {
                    inventoryItem.Quantity -= (responsePreserver.items.FirstOrDefault(i => i.name.Equals(inventoryItem.Name))?.quantity ?? 0);
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
