using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Persistance;
using FinancialServivces.Infrastructure.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddInfrastructure();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var bondTypeRepository = services.GetRequiredService<IBondTypeRepository>();
    var bondRepository = services.GetRequiredService<IBondRepository>();
    BondTypeCaching.Initialize(bondTypeRepository);
    BondCaching.Initialize(bondRepository);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
