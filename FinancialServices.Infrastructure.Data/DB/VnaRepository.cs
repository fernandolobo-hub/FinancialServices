using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Domain.Entities;
using PublicBonds.Domain.Enums;

namespace PublicBonds.Infrastructure.Data.DB
{
    public class VnaRepository : IVnaRepositoy
    {

        private readonly IConfiguration _configuration;
        public VnaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        public async Task<List<Vna>> GetVnasAsync(DateTime startDate, DateTime endDate, IndexerEnum indexerEnum)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT
                                id AS Id,
                                indexer_id AS Indexer,
                                nominal_value AS NominalValue,
                                reference_date AS ReferenceDate
                            FROM vna
                            WHERE indexer_id = @indexerId
                              AND reference_date BETWEEN @startDate AND @endDate";

                var parameters = new { indexerId = (int)indexerEnum, startDate, endDate };

                var vnas = await connection.QueryAsync<Vna>(query, parameters);

                return vnas.ToList();
            }
        }

        public async Task<Vna> GetMostRecentVnaAsync(DateTime referenceDate, IndexerEnum indexer)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                // Consulta o VNA cujo reference_date seja <= referenceDate,
                // ordenando decrescentemente e pegando apenas o mais recente (TOP 1 ou LIMIT 1).
                var query = @"
                            SELECT 
                                id AS Id,
                                indexer_id AS Indexer,
                                nominal_value AS NominalValue,
                                reference_date AS ReferenceDate
                            FROM vna
                            WHERE indexer_id = @IndexerId
                                AND reference_date <= @ReferenceDate
                            ORDER BY reference_date DESC
                            LIMIT 1";

                var parameters = new
                {
                    IndexerId = (int)indexer,
                    ReferenceDate = referenceDate
                };

                var vna = await connection.QuerySingleOrDefaultAsync<Vna>(query, parameters);

                return vna;
            }
        }

    }
}
