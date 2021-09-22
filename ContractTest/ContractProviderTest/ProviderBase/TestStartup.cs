namespace ContractTest.ContractProviderTest.ProviderBase
{
    using FoodPreservation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    public class TestStartup : Startup
    {
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ProviderStateMiddleware>();
            base.Configure(app, env);
        }
    }
}
