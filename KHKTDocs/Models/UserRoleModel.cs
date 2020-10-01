using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHKTDocs.Models
{
    public class UserRoleModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public int isadmin { get; set; }
        public int isapprove { get; set; }
        public int isdelete { get; set; }
        public int isaccess { get; set; }
    }
}
