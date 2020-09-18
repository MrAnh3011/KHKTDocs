using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class DocumentType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Action { get; set; }
    }
}
