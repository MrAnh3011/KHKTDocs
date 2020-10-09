using ApplicationCore.Entities.Common;
using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities
{
    public class apec_khktdocs_document : BaseEntity, IAggregateRoot
    {
        public string document_name { get; set; }
        public string stage { get; set; }
        public string document_description { get; set; }
        public string created_user { get; set; }
        public string document_receiver { get; set; }
        public int status { get; set; }
        public DateTime? approve_date { get; set; }
        public string document_extension { get; set; }
        public int document_folder_id { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string document_agency { get; set; }
        public string approver { get; set; }
    }
}
