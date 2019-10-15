using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamFood.Common.Models
{
    public class UserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EMail { get; set; }

        public string Phone { get; set; }

        public string TypeRestaurant { get; set; }

        public string DistanceRestaurant { get; set; }

        public string WordKey { get; set; }

        public string Password { get; set; }

        public string ImagePath { get; set; }

        public byte[] ImageArray { get; set; }
    }
}
