using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ApplicationCore.Enums
{
    public enum DocumentStatus
    {
        [Description("Ban hành")]
        Release,
        [Description("Chờ duyệt")]
        Pending,
        [Description("Đã duyệt")]
        Approved
    }
}
