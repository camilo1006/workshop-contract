namespace FoodPreservation.Controllers
{
    using System;
    using System.Collections.Generic;
    using Dto;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class FoodPreserverController : ControllerBase
    {
        [HttpGet]
        public object Get()
        {
            var rnd = new Random();
            return new
            {
                asdf = new List<ItemToExpire>
                {
                    new ItemToExpire { Name = "Tomato", Quantity = rnd.Next(0, 5) },
                    new ItemToExpire { Name = "Beans", Quantity = rnd.Next(0, 25) }
                },
                date = DateTime.Now
            };
        }
    }
}
