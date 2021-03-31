using DreamInMars.Dto;
using System.IO;
using System.Threading.Tasks;

namespace DreamInMars.Client
{
    public interface IDeepAiClient
    {
        Task<Stream> ProcessDeepAiImageAsync(Stream stream);

        Task<Stream> ProcessDeepAiImageAsync(Image image);
    }
}