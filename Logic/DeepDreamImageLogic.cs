using DreamInMars.Client;
using DreamInMars.Configuration;
using DreamInMars.Exceptions;
using DreamInMars.Model;
using DreamInMars.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public class DeepDreamImageLogic : IDeepDreamImageLogic
    {
        private readonly IGalleryImageRepository _galleryRepo;
        private readonly ICreditRepository _creditRepo;
        private readonly IDeepAiClient _deepDreamClient;
        private readonly IDbConnection _connection;
        private readonly IFileStorageLogic _fileStorageLogic;
        private readonly CreditConfiguration _creditConfig;

        public DeepDreamImageLogic(IDbConnection connection,
            IGalleryImageRepository galleryRepo,
            ICreditRepository creditRepo,
            IDeepAiClient deepAiClient,
            IFileStorageLogic fileStorageLogic,
            IOptions<CreditConfiguration> creditConfiguration
            )
        {
            _connection = connection;
            _galleryRepo = galleryRepo;
            _creditRepo = creditRepo;
            _deepDreamClient = deepAiClient;
            _creditConfig = creditConfiguration.Value;
            _fileStorageLogic = fileStorageLogic;
        }

        public async Task<string> TransformAndSaveImageAsync(Dto.Image image,  int value)
        {
            using var deepDreamStream = await _deepDreamClient.ProcessDeepAiImageAsync(image);
            try
            {
                var imagePath = await _fileStorageLogic.SaveImageAsync(deepDreamStream);
                await UpdateData(image.AccountId, imagePath);
                return imagePath;
            }
            finally
            {
                deepDreamStream.Close();
            }
        }

        private async Task UpdateData
            (int accountId, string imagePath)
        {
            var credit = await _creditRepo.ReadAsync(accountId);

            _connection.Open();
            using var transaction = _connection.BeginTransaction();
            try
            {
                credit.Value = credit.Value - _creditConfig.PaymentCredit;
                if (credit.Value < 0) throw new OutOfCreditsException();
                var image = new GalleryImage()
                {
                    AccountId = accountId,
                    Path = imagePath
                };

                await _creditRepo.UpdateAsync(credit, _connection, transaction);
                await _galleryRepo.CreateAsync(image, _connection, transaction);
                transaction.Commit();
            }
            catch(Exception)
            {
               transaction.Rollback();
               throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
