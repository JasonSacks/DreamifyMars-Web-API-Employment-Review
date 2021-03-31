using DreamInMars.Dto;
using DreamInMars.Model;
using DreamInMars.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamInMars.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryImageRepository _galleryRepo;
        
        public GalleryController(IGalleryImageRepository repo) =>
                _galleryRepo = repo;

        [HttpGet]
        [Route("Images")]
        public  IActionResult GetGallery(int accountId, int page)
        {
            if (page < 0) page = 1;
            var result = _galleryRepo.Read(accountId, page);
            return result != null ? Ok(new Response(result)) : Ok(new Response(new List<GalleryImage>()));
        }
        
        [HttpDelete]
        [Route("Delete/{galleryId}")]
        public async Task<IActionResult> DeleteImagePath(int galleryId)
        {
            await _galleryRepo.DeleteAsync(galleryId);
            return Ok(new Response(new { }));
        }

    }
}
