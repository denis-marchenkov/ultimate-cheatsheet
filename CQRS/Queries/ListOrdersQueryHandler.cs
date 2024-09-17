using CQRS.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Queries
{
    public sealed class ListOrdersQueryHandler(AppDbContext dbContext) : IRequestHandler<ListOrdersQuery, List<Order>>
    {
        public async Task<List<Order>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Orders.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
