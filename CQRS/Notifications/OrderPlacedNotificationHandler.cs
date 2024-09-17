using MediatR;

namespace CQRS.Notifications
{
    public sealed class OrderPlacedNotificationHandler() : INotificationHandler<OrderPlacedNotification>
    {
        public Task Handle(OrderPlacedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Order '{notification.Name} : {notification.Id.Value}' is placed");

            return Task.CompletedTask;
        }
    }
}
