using DreamInMars.Configuration;
using DreamInMars.Dto;
using DreamInMars.Model;
using DreamInMars.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public class AccountLogic : IAccountLogic
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICreditRepository _creditRepository;
        private readonly IDbConnection _transactionalConnection;
        private readonly CreditConfiguration _creditConfiguration;
        
        public AccountLogic (
            IAccountRepository accountRepository,
            ICreditRepository creditRepository, 
            IDbConnection transactionalConnection,
            IOptions<CreditConfiguration> creditConfig )
        {
            _accountRepository = accountRepository;
            _creditRepository = creditRepository;
            _transactionalConnection = transactionalConnection;
            _creditConfiguration = creditConfig.Value;
        }

        public Task<AccountInfo> GetAccountAsync(int accountId) =>
            _accountRepository.ReadAsync(accountId);

        public async Task<int> CreateAccount(Account account)
        {
            int id = 0;
           
            _transactionalConnection.Open();
            using var transaction = _transactionalConnection.BeginTransaction();
            try
            {
                id = await _accountRepository.CreateAsync(account, _transactionalConnection, transaction);
                var credit = new Credit { 
                    AccountId = id, 
                    LastResetDate = 
                    DateTime.UtcNow.Date, 
                    Value = _creditConfiguration.DefaultCredits 
                };
                await _creditRepository.CreateAsync(credit, _transactionalConnection, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                _transactionalConnection.Close();
            }
            return id;   
        }
    }
}