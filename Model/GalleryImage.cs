using System.ComponentModel.DataAnnotations;

namespace DreamInMars.Model
{
    public class GalleryImage
    {
        [Key]
        public int GalleryId { get; set; }
       
        public int AccountId { get; set; }

        [MaxLength(300)]
        public string Path { get; set; }
    }
}
