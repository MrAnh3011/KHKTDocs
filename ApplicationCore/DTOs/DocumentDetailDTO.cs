using System;

namespace ApplicationCore.DTOs
{
    public class DocumentDetailDTO
    {
        public int id { get; set; }
        public string document_name { get; set; }
        public string stage { get; set; }
        public string document_description { get; set; }
        public string created_user { get; set; }
        public string document_receiver { get; set; }
        public string status { get; set; }
        public string approve_date { get; set; }
        public string document_extension { get; set; }
        public string folder_name { get; set; }
        public int folder_id { get; set; }
        public string created_date { get; set; }
        public string modified_date { get; set; }
        public string document_agency { get; set; }
        public string approver { get; set; }
    }
}
