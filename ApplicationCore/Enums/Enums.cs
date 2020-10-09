using System.ComponentModel;

namespace ApplicationCore.Enums
{
    public enum DocumentStatus
    {
        [Description("Chờ duyệt")]
        Pending,
        [Description("Ban hành")]
        Release,
        [Description("Đã duyệt")]
        Approved
    }
    public enum UserRole
    {
        User,
        Admin,
        SuperAdmin,
        Approve,
        Delete
    }
}
