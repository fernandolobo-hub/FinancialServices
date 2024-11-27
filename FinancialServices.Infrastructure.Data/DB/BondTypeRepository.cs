using Dapper;
using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace PublicBonds.Infrastructure.Data.DB
{
    public class BondTypeRepository : IBondTypeRepository
    {

        private readonly IConfiguration _configuration;
        public BondTypeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<BondType>> GetAllAsync()
        {
            var sql = @"SELECT
                           a.id AS Id,
                           a.bond_name AS Name, 
                           a.has_coupon AS HasCoupon, 
                           a.annual_coupon_percentage AS AnnualCouponPercentage, 
                           b.index_name AS Indexer, 
                           a.vna_date_base AS VnaDateBase, 
                           a.rate_type AS RateType,
                           a.category as Category,
                           a.first_traded_at as FirstTradedAt
                    FROM bond_types a
                    LEFT JOIN indexers b
                    ON a.indexer_id = b.id";

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<BondType>(sql);
                return result;
            }
        }
    }
}
