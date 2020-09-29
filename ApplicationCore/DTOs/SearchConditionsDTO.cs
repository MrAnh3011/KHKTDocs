using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class SearchConditionsDTO
    {
        public string docfolder { get; set; }
        public string stage { get; set; }
        public string doctype { get; set; }
        public string docagency { get; set; }
        public string status { get; set; }
    }
}
