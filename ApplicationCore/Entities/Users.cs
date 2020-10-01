using ApplicationCore.Entities.Common;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class Users : BaseEntity, IAggregateRoot
    {
        public int user_id { get; set; }
        public string username { get; set; }
        public string display_name { get; set; }
        public string email { get; set; }
        public string staff_id { get; set; }
        public string department_name { get; set; }
    }
}
