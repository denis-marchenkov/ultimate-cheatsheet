using CQRS.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Queries
{
    public sealed class GetOrderQueryHandler(AppDbContext dbContext) : IRequestHandler<GetOrderQuery, Order?>
    {
        public async Task<Order?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var result = await dbContext.Orders.FirstOrDefaultAsync(order => order.Id.Value == request.Id.Value, cancellationToken: cancellationToken);
            return result;
        }
    }
}
