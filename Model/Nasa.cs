using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DreamInMars.Model
{
    public class MarsPhotos
    {
       [JsonPropertyName("photos")]
       public List<Photo> Photos { get; set; }
    }

    public class Rover
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("landing_date")]
        public DateTime LandingDate { get; set; }

        [JsonPropertyName("launch_date")]
        public DateTime LaunchDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } 
    }

    public class Camera
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rover_id")]
        public int RoverId { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
    }

    public class Photo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("sol")]
        public int Sol { get; set; }

        [JsonPropertyName("camera")]
        public Camera Camera { get; set; }

        [JsonPropertyName("img_src")]
        public string ImageSource { get; set; }

        [JsonPropertyName("earth_date")]
        public DateTime EarthDate { get; set; }

        [JsonPropertyName("rover")]
        public Rover Rover { get; set; }
    }

    public class Image
    {
        public string Path { get; set; }
    }
}
