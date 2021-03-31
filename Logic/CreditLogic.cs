using DreamInMars.Configuration;
using DreamInMars.Exceptions;
using DreamInMars.Model;
using DreamInMars.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public class CreditLogic : ICreditLogic
    {
        private readonly ICreditRepository _creditRepo;
        private readonly CreditConfiguration _configuration;

        public CreditLogic(ICreditRepository creditRepo, IOptions<CreditConfiguration> config)
        {
            _creditRepo = creditRepo;
            _configuration = config.Value;
        }

        public async Task<Credit> GetCreditsAsync(int accountId)
        {
            var credit = await _creditRepo.ReadAsync(accountId);
            var date = DateTime.UtcNow.Date;
            var resetDate = credit.LastResetDate.Date;
            var timeSpan = (date - resetDate);
            if (timeSpan.Days >= 1 )
            {
                credit.LastResetDate = date;
                credit.Value = _configuration.DefaultCredits;
                await _creditRepo.UpdateAsync(credit);
            }
            return credit;
        }
    }
}
