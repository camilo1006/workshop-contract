namespace ContractTest.ContractConsumerTest
{
    using System;
    using System.IO;
    using PactNet;
    using PactNet.Mocks.MockHttpService;

    public class FoodPreservationConsumerBase : IDisposable
    {
        public FoodPreservationConsumerBase()
        {
            var pathDir =
                $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}pacts";

            PactBuilder = new PactBuilder(new PactConfig
            {
                SpecificationVersion = "2.0.0",
                PactDir = pathDir,
                LogDir = pathDir
            });

            PactBuilder.ServiceConsumer("Inventory").HasPactWith("FoodPreservation");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public IPactBuilder PactBuilder { get; }
        public IMockProviderService MockProviderService { get; }

        public int MockServerPort => 9222;

        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}/";

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
