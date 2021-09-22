namespace Inventory.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dto;

    public interface IFoodPreservationService
    {
        Task<object> FoodPreservation(List<InventoryItem> listInventory);
    }
}
