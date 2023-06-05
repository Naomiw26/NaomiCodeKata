var appBuilder = WebApplication.CreateBuilder(args);
var services = appBuilder.Services;

using var app = appBuilder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapGet("/Hello", () => "Hello World!");
app.Run();

public partial class Program { }
// </snippet_all>
