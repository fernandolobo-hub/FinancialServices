using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using PublicBonds.Application.Interfaces.Repositories;
using PublicBonds.Domain.Entities;

namespace PublicBonds.Infrastructure.Data.DB
{
    public class IndexerRepository : IIndexerRepository
    {

        private readonly IConfiguration _configuration;

        public IndexerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        public async Task<IEnumerable<Indexer>> GetAllAsync()
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();

                var query = @"
                            SELECT 
                                id AS Id,
                                index_name AS IndexerName
                            FROM indexers;";

                var indexers = await connection.QueryAsync<Indexer>(query);

                return indexers;
            }
        }
    }
}
