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
using PublicBonds.Application.Factories;
using PublicBonds.Application.Interfaces.Factories;

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
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<IChronosService, ChronosService>();
            services.AddScoped<IHolidaysService, HolidaysService>();
            services.AddScoped<IVnaService, VnaService>();


            //repositories
            services.AddScoped<IBondTypeRepository, BondTypeRepository>();
            services.AddScoped<IBondRepository, BondRepository>();
            services.AddScoped<IDailyBondPricesRepository, DailyBondPricesRepository>();
            services.AddScoped<IHolidaysRepository, HolidaysRepository>();
            services.AddScoped<IVnaRepositoy, VnaRepository>();
            services.AddScoped<IIndexerRepository, IndexerRepository>();


            //validators
            services.AddScoped<IValidator<PublicBondHistoricalImportFilterRequest>, DailyPricesImportFilterValidator>();
            services.AddScoped<IValidator<PublicBondsHistoricalDataFilterRequest>, DailyPricesInfoRequestValidator>();
            services.AddScoped<IValidator<BondFilterRequest>, BondFilterRequestValidator>();
            services.AddScoped<IValidator<BondPricingRequest>, PricingRequestValidator>();
            services.AddScoped<IValidator<VnaFilterRequest>, VnaFilterRequestValidator>();

            //factories
            services.AddScoped<IBondPricingStrategyFactory, BondPricingStrategyFactory>();

            return services;
        }
    }
}
