using DreamInMars.Model;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DreamInMars.Repository
{
    public interface IGalleryImageRepository
    {
        Task<GalleryImage> CreateAsync(GalleryImage image, IDbConnection connection = null, IDbTransaction transaction = null);
        Task DeleteAsync(long imageId);
        IEnumerable<GalleryImage> Read(int accountId, int page);
    }
}