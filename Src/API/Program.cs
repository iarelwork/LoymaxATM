using API.Mapping;
using Application.Services;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
        optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("mssqllocaldb")));

    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<ITransactionService, TransactionService>();

    builder.Services.AddMappings();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //better solution for real app - use migrations
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }

    //TODO: Errors Controller with logging
    app.UseExceptionHandler(c => c.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerPathFeature>()
            ?.Error;
        if (exception != null)
        {
            var response = new { error = exception.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }));

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

