using DreamFood.Common.Models;
using System.Web;

namespace DreamFood.Backend.Models
{
    public class RestaurantView:Restaurant
    {
        public HttpPostedFileBase ImageFileMenu { get; set; }
    }
}