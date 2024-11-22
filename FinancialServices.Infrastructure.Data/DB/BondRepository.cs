using Dapper;
using FinancialServices.Application.Interfaces.Repositories;
using FinancialServices.Application.Persistance;
using FinancialServices.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Linq.Expressions;

namespace FinancialServices.Infrastructure.Data.DB
{
    public class BondRepository : IBondRepository
    {

        private readonly IConfiguration _configuration;
        public BondRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }


        public async Task<IEnumerable<Bond>> GetAllAsync()
        {
            var sql = @"
                        SELECT 
                            b.id AS Id,                   -- Id de Bond
                            b.maturity_date AS MaturityDate, 
                            bt.id AS Id,                  -- Id de BondType
                            bt.bond_name AS Name,         -- Nome do BondType
                            bt.has_coupon AS HasCoupon, 
                            bt.annual_coupon_percentage AS AnnualCouponPercentage,
                            bt.indexer_id AS Indexer,
                            bt.vna_date_base AS VnaDateBase,
                            bt.rate_type AS RateType,
                            bt.category AS Category
                        FROM bonds b
                        JOIN bond_types bt ON b.bond_type_id = bt.id;";

            try
            {
                using (var connection = GetConnection())
                {
                    await connection.OpenAsync();

                    // Mapeamento automático usando splitOn
                    var result = await connection.QueryAsync<Bond, BondType, Bond>(
                        sql,
                        map: (bond, bondType) =>
                        {
                            // Configura o BondType corretamente
                            bond.Type = bondType;
                            return bond;
                        },
                        splitOn: "Id" // Indica onde dividir entre Bond e BondType com base em "Id"
                    );

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter os Bonds da base", ex);
                throw;
            }
        }



        public async Task<int> SaveAsync(Bond bond)
        {
            var query = @"
                        INSERT INTO bonds (title, bond_type_id, maturity_date)
                        VALUES (@Title, @BondTypeId, @MaturityDate);
                        SELECT LAST_INSERT_ID();";

            var bondTypeId = bond.Type.Id;

            var parameters = new
            {
                Title = bond.Name,
                BondTypeId = bondTypeId,
                MaturityDate = bond.MaturityDate
            };

            try
            {
                using (var connection = GetConnection())
                {
                    await connection.OpenAsync();

                    var bondId = await connection.QuerySingleAsync<int>(query, parameters);

                    return bondId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao inserir bond na base", ex);
                throw;
            }
        }
    }
}
