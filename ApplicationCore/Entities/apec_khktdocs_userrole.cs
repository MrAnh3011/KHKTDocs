using ApplicationCore.Entities.Common;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class apec_khktdocs_userrole : BaseEntity, IAggregateRoot
    {
        public string username { get; set; }
        public int isadmin { get; set; }
        public int isapprove { get; set; }
        public int isdelete { get; set; }
        public int issuperadmin { get; set; }
        public int isaccess { get; set; }
    }
}
