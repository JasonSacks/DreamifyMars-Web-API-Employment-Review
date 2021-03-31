using Dapper;
using DreamInMars.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DreamInMars.Repository
{
    public class GalleryImageRepository : IGalleryImageRepository
    {
        private readonly IDbConnection _connection;
        private readonly DreamInMarsDbContext _dbContext;

        public GalleryImageRepository(IDbConnection connection, DreamInMarsDbContext dbContext)
        {
            _connection = connection;
            _dbContext = dbContext;
        }
        
        public async Task<GalleryImage> CreateAsync(GalleryImage image, IDbConnection connection, IDbTransaction transaction = null)
        {
            connection = connection ?? _connection;
            var connectionState = connection.State;
            if (connectionState == ConnectionState.Closed)
                connection.Open();
            try
            {
                var sql = $"INSERT INTO GalleryImages (AccountId,Path) Values (@accountId, @path); SELECT  CAST(SCOPE_IDENTITY() AS int)";
                var id = await connection.ExecuteScalarAsync<int>(
                    sql, 
                    new { accountId = image.AccountId, path = image.Path },
                    transaction);
                return new GalleryImage { AccountId = image.AccountId, GalleryId = id, Path = image.Path };
            }
            finally
            {
                if (connectionState == ConnectionState.Closed)
                    connection.Close();
            }
        }
        
        public IEnumerable<GalleryImage> Read(int accountId, int page)
        {
            page = (page <= 0) ? 0 : (page - 1) * 25;
            return _dbContext.GalleryImages?.Select(galleryImage => galleryImage)?.OrderBy(galleryImage => galleryImage.GalleryId).Skip(page)?.Take(25)?.ToList();

        }

        public async Task DeleteAsync(long imageId)
        {
            _connection.Open();
            try
            { 
                var sql = $"DELETE GalleryImages WHERE GalleryId = @galleryId;";
                await _connection.ExecuteAsync(sql, new { galleryId = imageId });
                return;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
