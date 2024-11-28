using PublicBonds.Application.Interfaces.Services;
using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using PublicBonds.Application.Validators;



using FluentValidation;
using PublicBonds.Domain.RequestObjects;
using PublicBonds.Infrastructure.Data.DB;
using PublicBonds.Application.Persistance;

namespace FinancialServivces.Infrastructure.Ioc
{

    public static class DependencyInjection
    { 
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddTransient(x =>
                 new MySqlConnection(x.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")));

            //services
            services.AddScoped<IInformationalService, InformationalService>();
            services.AddScoped<IDailyPricesService, DailyPricesService>();

            //repositories
            services.AddScoped<IBondTypeRepository, BondTypeRepository>();
            services.AddScoped<IBondRepository, BondRepository>();
            services.AddScoped<IDailyBondPricesRepository, DailyBondsImportRepository>();


            //validators
            services.AddScoped<IValidator<PublicBondHistoricalImportFilterRequest>, BondHistoricalImportFilterValidator>();


            return services;
        }
    }
}
