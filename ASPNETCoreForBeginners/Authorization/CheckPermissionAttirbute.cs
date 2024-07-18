using ASPNETCoreForBeginners.Entities;

namespace ASPNETCoreForBeginners.Authorization
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckPermissionAttirbute : Attribute
    {
        public CheckPermissionAttirbute(Permission permission)
        {
            Permission = permission;
        }

        public Permission Permission { get; }
    }
}
