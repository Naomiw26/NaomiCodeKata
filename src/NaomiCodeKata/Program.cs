using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RPGCombatKata.Api.Characters;
using RPGCombatKata.Api.Service.Controllers;
using RPGCombatKata.Api.Service.Exceptions;
using RPGCombatKata.Api.Service.Services;
using RPGCombatKata.Domain;
using RPGCombatKata.Infrastructure;
using System.Reflection;


var appBuilder = WebApplication.CreateBuilder(args);
var services = appBuilder.Services;
services.AddDbContext<CharactersDb>(opt => opt.UseInMemoryDatabase("Characters"));
services.AddScoped<CharacterRepository>();
services.AddScoped<ICharacterReader>(x => x.GetRequiredService<CharacterRepository>());
services.AddScoped<ICharacterWriter>(x => x.GetRequiredService<CharacterRepository>());
services.AddAutoMapper(typeof(CharacterProfile));
services.AddScoped<CharactersService>();
services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter))).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

CharactersController controller = new CharactersController(app);
app.UseHttpsRedirection();
app.UseRouting();
app.Run();

public partial class Program
{

}

