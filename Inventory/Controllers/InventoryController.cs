namespace Inventory.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dto;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IFoodPreservationService _foodPreservationService;

        public InventoryController(IFoodPreservationService foodPreservationService)
        {
            _foodPreservationService = foodPreservationService;
        }

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

            return await _foodPreservationService.FoodPreservation(listInventory);
        }
    }

    public interface IFoodPreservationService
    {
        Task<object> FoodPreservation(List<InventoryItem> listInventory);
    }
}
