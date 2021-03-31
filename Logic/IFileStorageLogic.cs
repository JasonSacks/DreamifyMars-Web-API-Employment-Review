using System.IO;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public interface IFileStorageLogic
    {
        Task<string> SaveImageAsync(Stream stream);
    }
}