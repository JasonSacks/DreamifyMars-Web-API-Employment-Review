using DreamInMars.Model;
using System.IO;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public interface IDeepDreamImageLogic
    {
         Task<string> TransformAndSaveImageAsync(Dto.Image image,  int value);
    }
}