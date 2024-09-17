using CQRS.Db;
using MediatR;

namespace CQRS.Queries
{
    public sealed record ListOrdersQuery : IRequest<List<Order>>;
}
