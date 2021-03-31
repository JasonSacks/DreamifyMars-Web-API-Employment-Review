using Dapper;
using DreamInMars.Dto;
using DreamInMars.Model;
using System.Data;
using System.Threading.Tasks;

namespace DreamInMars.Repository
{
    public class AccountRepository : IAccountRepository
    {

        private readonly IDbConnection _connection;
        const string SelectQuery = @"SELECT 
                        a.AccountId, 
                        a.Avatar,
                        a.FirstName, 
                        a.LastName,
                        a.PhoneNumber,
                        u.Email,
                        a.Address1, 
                        a.Address2,
                        a.City,
                        a.State,
                        a.PostalCode,
                        a.Country
                    FROM Accounts a 
                    JOIN Users u ON a.AccountId = u.AccountId 
                    WHERE u.AccountId = @AccountId;";

        public AccountRepository(IDbConnection connection) =>
            _connection = connection;

        public async Task<int> CreateAsync(Account account, IDbConnection connection = null, IDbTransaction transaction = null )
        {
            connection = connection ?? _connection;
            var connectionState = connection.State;
            if (connectionState == ConnectionState.Closed)
                connection.Open();
            try
            {
                var sql = @"INSERT INTO Accounts (
                        FirstName, 
                        LastName,
                        Avatar
                        ) Values (
                        @firstName, 
                        @lastName,
                        @avatar); 
                        SELECT CAST(SCOPE_IDENTITY() AS int) AS Id";

                var result = await connection.ExecuteScalarAsync<int>(sql, new
                {
                    firstName = account.FirstName,
                    lastName = account.LastName,
                    avatar = account.Avatar
                },
                transaction);

                return result;
            }
            finally
            {
                if(connectionState == ConnectionState.Closed)
                {
                    _connection.Close();
                }
            }
        }

        public async Task<AccountInfo> UpdateAsync(Account account)
        {
            var sql = $@"UPDATE Accounts SET 
                        FirstName = @firstName,  
                        lastName = @lastName, 
                        phoneNumber = @phoneNumber,
                        address1 = @address1,
                        address2 = @address2,
                        city = @city, 
                        state = @state,
                        postalCode = @PostalCode,
                        country = @Country 
                      WHERE AccountId = @accountId;
                      {SelectQuery}";

            return await _connection.QueryFirstOrDefaultAsync<AccountInfo>(sql, new
            {
                accountId = account.AccountId,
                firstName = account.FirstName,
                lastName = account.LastName,
                phoneNumber = account.PhoneNumber,
                address1 = account.Address1,
                address2 = account.Address2,
                city = account.City,
                state = account.State,
                postalCode = account.PostalCode,
                country = account.Country
            });
        }

        public async Task<AccountInfo> ReadAsync(int accountId) =>
            await _connection.QueryFirstOrDefaultAsync<AccountInfo>(SelectQuery, new { AccountId = accountId});
        
    }
}
