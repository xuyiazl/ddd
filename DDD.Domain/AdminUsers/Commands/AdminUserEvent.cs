using DDD.Domain.Core.Events;
using DDD.Domain.Entities;
using MediatR;

namespace DDD.Domain.AdminUsers.Commands
{
    public class CreateAdminUserEvent : Event
    {
        public long Id { get; set; }
        public AdminUser User { get; set; }
        public CreateAdminUserEvent(long id, AdminUser user)
        {
            Id = id;
            User = user;
            AggregateId = id;
        }
    }

    public class UpdateAdminUserEvent : Event
    {
        public long Id { get; set; }
        public AdminUser User { get; set; }
        public UpdateAdminUserEvent(long id, AdminUser user)
        {
            Id = id;
            User = user;
            AggregateId = id;
        }
    }

    public class DeleteAdminUserEvent : Event
    {
        public long Id { get; set; }
        public DeleteAdminUserEvent(long id)
        {
            Id = id;
            AggregateId = id;
        }
    }
}
