using CQRS.Db;
using MediatR;

namespace CQRS.Notifications
{
    public sealed record OrderPlacedNotification(OrderId Id, string Name) : INotification;
}
