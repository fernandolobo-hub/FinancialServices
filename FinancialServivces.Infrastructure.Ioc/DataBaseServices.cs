using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace FinancialServivces.Infrastructure.Ioc
{
    public static class DataBaseServices
    {
        public static IServiceCollection AddDatabases(this IServiceCollection services)
        {

            // Registra a conexão MySQL como um serviço

            return services;
    }
    }
}
