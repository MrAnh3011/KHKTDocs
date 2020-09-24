using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class UserDTOs
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public string staff_id { get; set; }
        public string department_name { get; set; }
        public string full_name { get; set; }
    }
}
