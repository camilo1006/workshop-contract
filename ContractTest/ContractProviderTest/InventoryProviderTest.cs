namespace ContractTest.ContractProviderTest
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FoodPreservation;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using PactNet;
    using PactNet.Infrastructure.Outputters;
    using ProviderBase;
    using Xunit;
    using Xunit.Abstractions;

    public class InventoryProviderTest
    {
        private readonly ITestOutputHelper _output;

        public InventoryProviderTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ProviderWithInventory()
        {
            //Arrange
            const string serviceUri = "http://localhost:9222";
            var config = new PactVerifierConfig
            {
                Outputters =
                    new
                        List<IOutput> //NOTE: We default to using a ConsoleOutput, however xUnit 2 does not capture the console output, so a custom outputter is required.
                        {
                            new XUnitOutput(_output)
                        },
                CustomHeaders = new Dictionary<string, string>
                {
                    { "Authorization", "Basic VGVzdA==" }
                }, //This allows the user to set request headers that will be sent with every request the verifier sends to the provider
                Verbose = true, //Output verbose verification logs to the test output
                ProviderVersion = ContractInfo._providerVersion,
                PublishVerificationResults = true
            };

            using var webHost = WebHost.CreateDefaultBuilder().UseUrls(serviceUri).UseStartup<TestStartup>()
                .ConfigureServices(
                    services => { services.AddMvc().AddApplicationPart(typeof(Startup).Assembly); }).Build();
            await webHost.StartAsync();
            IPactVerifier pactVerifier = new PactVerifier(config);

            pactVerifier
                .ServiceProvider("FoodPreservation", serviceUri)
                .HonoursPactWith("Inventory")
                .PactBroker(ContractInfo._pactUrl, new PactUriOptions(ContractInfo._token))
                .Verify();

            await webHost.StopAsync();
        }
    }
}
