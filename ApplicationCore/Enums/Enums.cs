using System.ComponentModel;

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
