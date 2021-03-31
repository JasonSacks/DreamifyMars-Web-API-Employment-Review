using Dapper;
using DreamInMars.Model;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DreamInMars.Repository
{
    public class CreditRepository : ICreditRepository
    {
        private readonly IDbConnection _connection;
    
        public CreditRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task<int> CreateAsync(Credit credit, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            connection = connection ?? _connection;
            var connectionState = connection.State;
            if (connectionState == ConnectionState.Closed)
                connection.Open();
            try
            {
                var sql = @"INSERT INTO credits (
                                AccountId, 
                                Value, 
                                LastResetDate
                                ) VALUES (
                                @accountId,
                                @value,
                                @date
                                ); 
                                SELECT  CAST(SCOPE_IDENTITY() AS int) AS Id";

                return await connection.ExecuteScalarAsync<int>(sql, new
                {
                    value = credit.Value,
                    date = DateTime.UtcNow.Date,
                    accountId = credit.AccountId
                },
                transaction);
            }
            finally
            {
                if (connectionState == ConnectionState.Closed)
                    _connection.Close();
            }
        }

        public async Task<Credit> ReadAsync(int accountId)
        {
            _connection.Open();
            try
            {
                var sql = @"SELECT * FROM credits 
                            WHERE AccountId = @id";
                return await _connection.QueryFirstOrDefaultAsync<Credit>(sql, new { id = accountId });
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task UpdateAsync(Credit credit, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            connection = connection ?? _connection;
            var connectionState = connection.State;
            if (connectionState == ConnectionState.Closed)
                connection.Open();
            try
            {
                var sql = @"UPDATE credits SET 
                                Value = @value, 
                                LastResetDate = @date 
                            WHERE AccountId = @accountId;";
                await connection.ExecuteAsync(sql, new { 
                    value = credit.Value, 
                    date = credit.LastResetDate, 
                    accountId = credit.AccountId 
                },
                transaction);
            }
            finally
            {
                if (connectionState == ConnectionState.Closed)
                    connection.Close();
            }
        }
    }
}
