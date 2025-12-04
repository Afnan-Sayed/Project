namespace ERP_API.DataAccess.Entities.User
{
    public class AppUserPermission
    {
        public int Id { get; set; }
        public required string PermissionName { get; set; }

        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();

    }


}