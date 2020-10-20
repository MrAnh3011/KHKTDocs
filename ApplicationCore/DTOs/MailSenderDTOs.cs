using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class MailSenderDTOs
    {
        public string approver { get; set; }
        public string requester { get; set; }
        public string docname { get; set; }
        public DateTime? docdate { get; set; }
        public string note { get; set; }
        public string link { get; set; }
        public string approverMail { get; set; }
        public string stage { get; set; }
        public string folder { get; set; }
        public string status { get; set; }
        public string sendermail { get; set; }
    }
}
