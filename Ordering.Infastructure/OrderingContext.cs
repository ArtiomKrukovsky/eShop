using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregateModels.Ordering;
using System;
using Ordering.Infrastructure.EntityConfigurations;

namespace Ordering.Infrastructure
{
    public class OrderingContext: DbContext
    {
        public const string DEFAULT_SCHEMA = "ordering";
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        private readonly IMediator _mediator;

        public OrderingContext(DbContextOptions<OrderingContext> options) : base(options) { }

        public OrderingContext(DbContextOptions<OrderingContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());
        }
    }
}
