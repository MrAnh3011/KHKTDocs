using ApplicationCore.Entities.Common;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Document : BaseEntity,IAggregateRoot
    {
        public string DocumentId { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentCode { get; set; }
        public string DocumentNo { get; set; }
        public string DocumentUrl { get; set; }
        public string DocumentName { get; set; }
        public string DisplayName { get; set; }
        public string OrgPublish { get; set; }
        public string DocTypeName { get; set; }
        public DateTime? ActiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string BriefDescription { get; set; }
        public int Status { get; set; }
        public int IsActive { get; set; }
        public string Ext { get; set; }
        public string ExpireLink { get; set; }
        //public HttpPostedFileBase[] Files { get; set; }
    }
}
