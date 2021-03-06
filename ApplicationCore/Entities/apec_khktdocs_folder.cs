﻿using ApplicationCore.Entities.Common;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Entities
{
    public class apec_khktdocs_folder : BaseEntity, IAggregateRoot
    {
        public string parent { get; set; }
        public string text { get; set; }
        public string created_user { get; set; }
        public string modified_user { get; set; }
    }
}
