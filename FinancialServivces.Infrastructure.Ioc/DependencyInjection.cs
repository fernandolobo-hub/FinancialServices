using FinancialServices.Application.Interfaces.Services;
using FinancialServices.Application.Interfaces.Repositories;
using FinancialServices.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using FinancialServices.Application.Validators;



using FluentValidation;
using FinancialServices.Domain.RequestObjects;
using FinancialServices.Infrastructure.Data.DB;
using FinancialServices.Application.Persistance;

namespace FinancialServivces.Infrastructure.Ioc
{

    public static class DependencyInjection
    { 
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddTransient(x =>
                 new MySqlConnection(x.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")));

            //services
            services.AddScoped<IPublicBondsInfoService, PublicBondTypesService>();
            services.AddScoped<IDailyBondsImportService, DailyBondsImportService>();

            //repositories
            services.AddScoped<IBondTypeRepository, BondTypeRepository>();
            services.AddScoped<IBondRepository, BondRepository>();
            services.AddScoped<IDailyBondsImportRepository, DailyBondsImportRepository>();


            //validators
            services.AddScoped<IValidator<PublicBondHistoricalImportFilterRequest>, BondHistoricalImportFilterValidator>();


            return services;
        }
    }
}
