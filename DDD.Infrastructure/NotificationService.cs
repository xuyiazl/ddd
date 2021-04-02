using DDD.Domain.Common.Interfaces;
using DDD.Domain.Notifications.Models;
using System.Threading.Tasks;

namespace DDD.Infrastructure
{
    public class NotificationService : INotificationService
    {
        public Task SendAsync(MessageDto message)
        {
            return Task.CompletedTask;
        }
    }
}
