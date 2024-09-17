using CQRS.Db;
using MediatR;

namespace CQRS.Queries
{
    public sealed record GetOrderQuery(OrderId Id) : IRequest<Order>;
}
