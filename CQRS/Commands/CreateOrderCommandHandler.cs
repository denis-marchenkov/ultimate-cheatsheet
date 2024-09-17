using CQRS.Db;
using MediatR;

namespace CQRS.Commands
{
    public sealed class CreateOrderCommandHandler(AppDbContext dbContext) : IRequestHandler<CreateOrderCommand, OrderId>
    {
        public async Task<OrderId> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order { Id = OrderId.New(), Name = request.Name };

            await dbContext.AddAsync(order, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
