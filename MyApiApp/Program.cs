using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MyApiApp.Models;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MyAppApi",
        Version = "v1",
        Description = "Web API for managing customers, transactions, accounts",
        Contact = new OpenApiContact
        {
            Name = "Piotr Syrek",
            Email = "piotr.syrek@onet.pl",
            Url = new Uri("https://www.MyAppApi.pl"),
        }
    });


    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


builder.Services.AddDbContext<MyDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

//    var logger = services.GetService<ILogger<ApplicationLogger>>();   
//    services.AddSingleton(typeof(ILogger), logger);
    try
    {
        var context = services.GetRequiredService<MyDb>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
    }
}

app.Run();