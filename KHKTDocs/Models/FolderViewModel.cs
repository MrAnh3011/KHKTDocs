﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHKTDocs.Models
{
    public class FolderViewModel
    {
        public int id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string action { get; set; }
    }
}
