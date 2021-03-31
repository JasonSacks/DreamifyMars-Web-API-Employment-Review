using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamInMars.Model
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        
        [MaxLength(40)]
        public string FirstName { get; set; }
        
        [MaxLength(25)]
        public string LastName { get; set; }
        
        [MaxLength(17)]
        public string PhoneNumber { get; set; }
        
        [MaxLength(40)]
        public string Address1 { get; set; }
        
        [MaxLength(40)]
        public string Address2 { get; set; }
        
        [MaxLength(35)]
        public string City { get; set; }
        
        [MaxLength(20)]
        public string State { get; set; }
        
        [MaxLength(15)]
        public string PostalCode { get; set; }
       
        [MaxLength(40)]
        public string Country { get; set; }
      
        [MaxLength(300)]
        public string Avatar { get; set; }
        public Credit Credit { get; set; }

        public DreamUser Identity { get; set; }

        public ICollection<GalleryImage> GalleryImages { get; set; }

    }
}
