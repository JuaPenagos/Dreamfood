using DreamFood.Common.Models;
using System.Web;

namespace DreamFood.Backend.Models
{
    public class RecommendationView : Recommendation
    {
        public HttpPostedFileBase ImageFileRecomm { get; set; }
    }
}