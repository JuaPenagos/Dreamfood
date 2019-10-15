using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamFood.Common.Models
{
    public class Restaurant
    {
        [Key]
        public int IdRestaurant { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Required]
        public string Phone { get; set; }

        [Display(Name = "Image")]
        public string ImagePathMenu { get; set; }

        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [JsonIgnore]
        public virtual ICollection<Recommendation> Recommendation { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        public string ImageFullPathMenu
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePathMenu))
                {
                    return "picture";
                }

                return $"https://apidreamfood.azurewebsites.net/{this.ImagePathMenu.Substring(1)}";
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
