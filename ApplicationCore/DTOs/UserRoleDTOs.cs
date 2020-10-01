using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class UserRoleDTOs
    {
        public int id { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public int isadmin { get; set; }
        public int isapprove { get; set; }
        public int isdelete { get; set; }
        public int issuperadmin { get; set; }
        public int isaccess { get; set; }
    }
}
