using DreamInMars.Dto;
using DreamInMars.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamInMars.Client
{
    public interface INasaMarsRoverClient
    {
        Task<IEnumerable<PhotoPath>> GetRoverImagesAsync(RoverEnums rover, int page,  DateTime? earthDate = null);
    }
}