using MassTransit;
using Microsoft.OpenApi.Models;
using ProductService.Consumers;
using ProductService.Policies;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IProductService, ProductService.Services.ProductService>(client =>
    {
        client.BaseAddress = new Uri("https://fakestoreapi.com");
    })
    .AddPolicyHandler(HttpPolicies.GetRetryPolicy())
    .AddPolicyHandler(HttpPolicies.GetCircuitBreakerPolicy());

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq.ecommerceclone.orb.local", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("product-service-order-created", e =>
        {
            e.ConfigureConsumer<OrderCreatedConsumer>(context);
        });
    });
});

//builder.Services.AddScoped<IProductService, ProductService.Services.ProductService>(); bunu kaldır httpclient zaten di için kayıt yapıyor.

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseSwaggerUI();
app.UseSwagger();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();