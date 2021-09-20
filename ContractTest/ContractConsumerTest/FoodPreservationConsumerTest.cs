namespace ContractTest.ContractConsumerTest
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Inventory.Dto;
    using Inventory.Services;
    using PactNet.Mocks.MockHttpService;
    using PactNet.Mocks.MockHttpService.Models;
    using Xunit;

    public class FoodPreservationConsumerTest : IClassFixture<FoodPreservationConsumerBase>
    {
        private readonly FoodPreservationService _foodPreservationMock;
        private readonly IMockProviderService _mockProviderService;
        private readonly Uri _mockProviderServiceBaseUri;

        public FoodPreservationConsumerTest(FoodPreservationConsumerBase data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = new Uri(data.MockProviderServiceBaseUri);
            _foodPreservationMock = new FoodPreservationService(_mockProviderServiceBaseUri);
        }

        [Fact]
        public async Task GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
        {
            //Arrange
            _mockProviderService
                .Given("There is a something with id 'tester'")
                .UponReceiving("A GET request to retrieve the something")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/FoodPreserver",
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new {
                        items = Match.MinType(new
                        {
                            name = Match.Type("Potato"),
                            quantity = Match.Type(.5)
                        }, 1),
                        date = Match.Type(DateTime.Now)
                    }
                });

            //Act
            var rnd = new Random();
            var listInventory = new List<InventoryItem>
            {
                new InventoryItem { Name = "Potato", Quantity = rnd.Next(0, 10) },
                new InventoryItem { Name = "Salt", Quantity = rnd.Next(0, 1) },
                new InventoryItem { Name = "Beans", Quantity = rnd.Next(0, 50) }
            };
            await _foodPreservationMock.FoodPreservation(listInventory);
            _mockProviderService
                .VerifyInteractions();
        }
    }
}
