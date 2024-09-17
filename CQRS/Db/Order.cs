namespace CQRS.Db
{
    // strongly typed ID because they are nice
    public readonly record struct OrderId(Guid Value)
    {
        public static OrderId New() => new (Guid.NewGuid());
        public static OrderId Empty => new(Guid.Empty);
    }

    public class Order
    {
        public OrderId Id { get; set; } = OrderId.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
