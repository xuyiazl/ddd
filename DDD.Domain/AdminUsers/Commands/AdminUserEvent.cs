using DDD.Domain.Entities;
using MediatR;

namespace DDD.Domain.AdminUsers.Commands
{
    public class CreateAdminUserEvent : INotification
    {
        public long Id { get; set; }
        public AdminUser User { get; set; }
        public CreateAdminUserEvent(long id, AdminUser user)
        {
            Id = id;
            User = user;
        }
    }

    public class UpdateAdminUserEvent : INotification
    {
        public long Id { get; set; }
        public AdminUser User { get; set; }
        public UpdateAdminUserEvent(long id, AdminUser user)
        {
            Id = id;
            User = user;
        }
    }

    public class DeleteAdminUserEvent : INotification
    {
        public long Id { get; set; }
        public DeleteAdminUserEvent(long id)
        {
            Id = id;
        }
    }
}
