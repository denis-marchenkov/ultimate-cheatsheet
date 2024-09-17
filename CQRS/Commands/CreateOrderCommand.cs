using CQRS.Db;
using MediatR;

namespace CQRS.Commands
{
    public sealed record CreateOrderCommand(string Name) : IRequest<OrderId>;
}
