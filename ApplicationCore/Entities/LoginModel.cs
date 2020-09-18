using ApplicationCore.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Entities
{
    public class LoginModel: BaseEntity
    {
        public string Username { set; get; }

        public string SessionKey { set; get; }

        public string AllowDevelop { set; get; }

        public string AllowViewAllData { set; get; }

        public string DisplayName { set; get; }
    }
}
