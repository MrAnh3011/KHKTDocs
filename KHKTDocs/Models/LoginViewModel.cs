using System.ComponentModel.DataAnnotations;

namespace KHKTDocs.Models
{
    public class LoginViewModel
    {
        public string Username { set; get; }
        public string SessionKey { set; get; }
        public string AllowDevelop { set; get; }
        public string AllowViewAllData { set; get; }
        public string DisplayName { set; get; }
    }
}
