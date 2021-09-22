namespace ContractTest.ContractProviderTest.ProviderBase
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    public class ProviderStateMiddleware
    {
        private const string ConsumerName = "FoodPreservation";
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "una solicitud del inventario actual",
                    AddTesterIfItDoesntExist
                }
            };
        }

        private void AddTesterIfItDoesntExist()
        {
            Console.WriteLine("Before Each");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                if (context.Request.Method == HttpMethod.Post.ToString() && context.Request.Body != null)
                {
                    string jsonRequestBody;
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        jsonRequestBody = await reader.ReadToEndAsync();
                    }

                    var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                    //A null or empty provider state key must be handled
                    if (providerState != null && !providerState.State.IsNullOrEmpty() &&
                        providerState.Consumer == ConsumerName)
                    {
                        _providerStates[providerState.State].Invoke();
                    }

                    await context.Response.WriteAsync(string.Empty);
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
