using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamFood.Common.Models
{
    public class Recommendation
    {
        [Key]
        public int IdRecommendation { get; set; }

        public int IdRestaurant { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        public string RecommendationUser { get; set; }

        [Required]
        public DateTime DateRecomm { get; set; }

        [Display(Name = "Image")]
        public string ImagePathRecomm { get; set; }

        [Required]
        public double Score { get; set; }

        [JsonIgnore]
        public virtual Restaurant Restaurant { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        public string ImageFullPathRecomm
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePathRecomm))
                {
                    return "picture";
                }

                return $"https://apidreamfood.azurewebsites.net/{this.ImagePathRecomm.Substring(1)}";
            }
        }

        public override string ToString()
        {
            return this.RecommendationUser;
        }

        public Recommendation()
        {
            this.DateRecomm = DateTime.Now;
            this.IdRestaurant = 37;
            this.UserId = "1";
        }

    }
}
