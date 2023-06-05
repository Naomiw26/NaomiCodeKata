using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTestsSample.Tests;

// <snippet1>
public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
        });

        builder.UseEnvironment("Development");
    }
}
// </snippet1>
