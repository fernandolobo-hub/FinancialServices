using Dapper;
using PublicBonds.Application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using MySqlConnector;


namespace PublicBonds.Infrastructure.Data.DB
{
    public class HolidaysRepository : IHolidaysRepository
    {

        private readonly IConfiguration _configuration;
        public HolidaysRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<HashSet<DateTime>> GetHollidaysAsync(DateTime startDate, DateTime endDate)
        {
            var query = @"
                                SELECT 
                                    date AS Date
                                FROM bank_holidays
                                WHERE date BETWEEN @startDate AND @endDate;";

            try
            {
                using (var connection = GetConnection())
                {
                    await connection.OpenAsync();
                    var result = connection.Query<DateTime>(query, new { startDate, endDate }).ToHashSet();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting hollidays", ex);
                throw;
            }
        }
    }
}
