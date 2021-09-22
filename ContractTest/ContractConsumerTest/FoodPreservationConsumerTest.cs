namespace ContractTest.ContractConsumerTest
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Inventory.Dto;
    using Inventory.Services;
    using Moq;
    using PactNet.Mocks.MockHttpService;
    using PactNet.Mocks.MockHttpService.Models;
    using Xunit;
    using Match = PactNet.Matchers.Match;

    public class FoodPreservationConsumerTest : IClassFixture<FoodPreservationConsumerBase>
    {
        private readonly FoodPreservationService _foodPreservationMock;
        private readonly IMockProviderService _mockProviderService;

        public FoodPreservationConsumerTest(FoodPreservationConsumerBase data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions();
            var mockFactory = new Mock<IHttpClientFactory>();
            var pactClient = new HttpClient
                { BaseAddress = new Uri(data.MockProviderServiceBaseUri), DefaultRequestHeaders = { { "Accept", "application/json" } } };
            mockFactory.Setup(call => call.CreateClient("foodPreservation")).Returns(pactClient);
            _foodPreservationMock = new FoodPreservationService(mockFactory.Object);
        }

        [Fact]
        [Trait("Type", "Consumer")]
        public async Task GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
        {
            _mockProviderService
                .Given("una solicitud del inventario actual")
                .UponReceiving("la cantidad de inventario en mal estado")
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
                            name = Match.Regex("Potato", "[A-Za-z]*"),
                            quantity = Match.Type(.5)
                        }, 1),
                        date = Match.Type(DateTime.Now),
                    }
                });

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
